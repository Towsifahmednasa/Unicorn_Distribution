"""Aggregate module deliverables into system-level docs (business, workflows, modules, gaps, ...)."""
import os, re, glob, json, collections
from gen_common import ROOT, MODULES, MOD, ASPX, COMPONENTS, table, write

def rd(p):
    return open(os.path.join(ROOT, p), encoding='utf-8', errors='replace').read()

def section(text, pat):
    m = re.search(r'(?m)^(##\s+[^\n]*(?:' + pat + r')[^\n]*)\n', text, re.I)
    if not m: return ''
    rest = text[m.end():]
    n = re.search(r'(?m)^##\s', rest)
    return (rest[:n.start()] if n else rest).strip()

def shift(md, by=2):
    return re.sub(r'(?m)^(#{1,4})\s', lambda m: '#' * (len(m.group(1)) + by) + ' ', md)

READ = {m[0]: rd(f'specs/{m[1]}/README.md') for m in MODULES}
ALLTXT = '\n'.join(READ.values())
# ---- counts
BR_IDS = set()
WF_FILES = []; RPT_FILES = []
for m in MODULES:
    base = f'specs/{m[1]}'
    for f in glob.glob(os.path.join(ROOT, base, '**', '*.md'), recursive=True):
        t = open(f, encoding='utf-8', errors='replace').read()
        BR_IDS |= set(re.findall(r'BR-M\d\d-\d{3}', t))
    WF_FILES += [(m, os.path.basename(f), f) for f in sorted(glob.glob(os.path.join(ROOT, base, 'workflows', '*.md')))]
    RPT_FILES += [(m, os.path.basename(f)) for f in sorted(glob.glob(os.path.join(ROOT, base, 'reports', 'RPT-*.md')))]
BR_BY_MOD = collections.Counter(b[3:6] for b in BR_IDS)
GAP_ROWS = []
for m in MODULES:
    g = glob.glob(os.path.join(ROOT, 'specs', m[1], 'GAPS-*.md'))
    for f in g:
        for line in open(f, encoding='utf-8', errors='replace'):
            if re.match(r'\|\s*GAP-M\d\d-\d+', line):
                GAP_ROWS.append((m[0], line.rstrip()))
GAP_BY_MOD = collections.Counter(a for a, _ in GAP_ROWS)
TIER = collections.Counter()
for p in ASPX:
    t = open(os.path.join(ROOT, p['file']), encoding='utf-8').read(400)
    TIER[re.search(r'tier:\s*(\w)', t).group(1)] += 1
STATS = json.load(open('stats.json'))

def bri(m):
    t = READ[m[0]]
    s = section(t, r'business rules')
    return s

# ------------------------------------------------------------------ business.md
def gen_business():
    L = ['# Business Documentation (reverse-engineered)', '',
         f'> Source of truth is the code. Every rule below is tagged CONFIRMED / INFERRED / UNKNOWN in its module spec. **{len(BR_IDS)} business rules** were recorded ({", ".join(f"{k}: {v}" for k, v in sorted(BR_BY_MOD.items()))}). Only {TIER["A"]} of {len(ASPX)} pages were reviewed in depth (Tier A); rules from the other pages are unrecorded unless a module README says otherwise. Stored-procedure bodies are absent for ~1,000 procedures, so many business rules that live in SQL are UNKNOWN.', '',
         '## 1. Business Overview', '',
         'The system supports a pharmaceutical (and general) **sales & distribution and field-force operation**: head-office and distribution centres (DC), warehouses and sub-depots hold stock; field officers (MIO/MIA) and the sales hierarchy (national → zone/region → area → territory → market) drive orders for customers (pharmacies/providers) and doctors; orders become proforma/invoices, are picked, loaded and delivered; payments and returns are recorded and posted to an accounts database; stock moves between warehouse, DC, sub-depot and SAP; HR features cover attendance, leave, tour plans and expense claims. (Sources: `docs/architecture.md`, `specs/00-system/system-overview.md`, module READMEs.)', '',
         '## 2. Business Terminology (INFERRED from names and code; confirm with business owners)', '',
         table(['Term', 'Meaning as used in code', 'Confidence'], [
             ['DC', 'Distribution centre / depot (tblDCStore, ComUnit = company/distribution unit)', 'INFERRED'], ['WH / Central store', 'Central warehouse (tblCentralStore)', 'INFERRED'],
             ['Sub-depot', 'Secondary depot with its own stack (tblSubDepot*, tblSubInvoice*)', 'CONFIRMED (code structure)'], ['MIO / MIA', 'Field sales/market officer and assistant (tblMIOInfo, tblMIAInfo)', 'INFERRED'],
             ['DZSM / RSM / NSM / ASM / AM', 'Sales hierarchy levels above MIO', 'INFERRED'], ['Proforma invoice', 'Pre-invoice created from orders, converted to invoice on confirmation', 'CONFIRMED (M07)'],
             ['Chalan / Challan', 'Delivery/transfer document', 'CONFIRMED'], ['Top sheet', 'Delivery/loading summary grouping invoices', 'INFERRED'], ['STO', 'Stock transport order (inter-store transfer)', 'CONFIRMED'],
             ['MIGO', 'SAP goods movement (import pages)', 'INFERRED'], ['DWSP', 'Day-wise sales plan (target module)', 'INFERRED'], ['DCR / DCP / CVR / RX', 'Doctor call report / plan / visit report / prescription', 'INFERRED'],
             ['TADA', 'Travel & daily-allowance claim', 'INFERRED'], ['ComUnit', 'Company unit / distribution unit context stored in session', 'CONFIRMED (Login.aspx.cs)']]), '',
         '## 3. Modules', '',
         table(['ID', 'Module', 'Pages', 'Business rules', 'Spec'], [[m[0], m[2], sum(1 for p in ASPX if p['module'] == m[0]), BR_BY_MOD.get(m[0][1:], 0), f'[README](../specs/{m[1]}/README.md)'] for m in MODULES]), '',
         '## 4. Business Processes', '',
         'The main chain, as evidenced by pages, tables and module workflows (details in `docs/workflows.md`):', '',
         '1. **Master data** (M03 structure, M04 products/prices/campaigns, M05 customers, M15/M02 users & employees).',
         '2. **Order** (M06): manual entry/edit, Excel upload, approval of special orders, DC assignment.',
         '3. **Proforma → invoice → picking → challan/top sheet → loading → delivery confirmation** (M07); stock leaves DC stores.',
         '4. **Collection** (M09): customer payments allocated to invoices, money receipts, deposit slips; accounts voucher posting to the accounts DB (only from CustomerPayment).',
         '5. **Returns** (M08): market/delivery returns, return invoices, posting.',
         '6. **Stock** (M10/M11/M12): warehouse stock in/out, adjustments, freeze, transfers DC↔DC/WH/sub-depot, SAP challan hand-off, valuation reports.',
         '7. **Targets** (M14) and **field force** (M15/M16): plans, visits, prescriptions, leave and claims with approvals (M18).',
         '8. **Reporting** (M13 and per-module viewers): Crystal Reports and dashboards.', '',
         '## 5. Workflows', '', f'{len(WF_FILES)} traced workflows exist under `specs/<module>/workflows/`; indexed in [workflows.md](workflows.md).', '',
         '## 6. Business Rules (compiled from module READMEs)', '',
         'Each rule row carries its Source, implementation location and Confidence in the module README index; the full text of each rule is in the page spec §19 or the README.', '']
    for m in MODULES:
        s = bri(m)
        L += [f'### {m[0]} — {m[2]} ({BR_BY_MOD.get(m[0][1:], 0)} rules)', '', s or '_No rules index found in module README._', '']
    L += ['## 7. Approval Rules', '',
          'Approval pages live in `Solution.Web/Approval_UI` (M18) plus approval steps embedded in module pages (order special approval, stock-in approval, credit-limit approval, stock-out approval, transfer approvals, leave/expense/tour-plan/attendance approvals). Key observed facts (see `specs/approval-workflow/README.md` §3–5 for cited rules):', '',
          '- The generic engine `ClsApprovalAction` + `tblAction*` tables exists, but **no M18 page uses `tblAction*`**; approvals are implemented per page with status flags and hard-coded approver/role/MenuId values (e.g. employee 496, MenuId 381 shared by Leave and Order).',
          '- Role gating on approval pages is UI-only; there is no server-side re-check (M18 gaps).',
          '- Web-created customers/doctors are inserted already approved (`ActionStatus=\'2\'`; `sp_Save_CustomerMaster`, `sp_Save_DoctorMaster` hard-code Approved).', '',
          '## 8. Roles', '',
          'Roles are data (`tbl_UserRoleInfo`, `tblRoleType`, `tblUserType`); the login returns `UserRoleID`, `RoleTypeId`, `RoleTypeName`, `UserType` (Admin has no ComUnitId). **Role names/permissions are UNKNOWN from the repository.** Role id 2 is a hard-coded bypass in code (M02 README). See `specs/00-system/role-permission.md`.', '',
          '## 9. Permissions', '',
          section(READ['M02'], r'authorization model')[:6000] or 'See M02 README.', '',
          '## 10. Status Lifecycles (compiled)', '']
    for m in MODULES:
        s = section(READ[m[0]], r'status lifecycle')
        if s: L += [f'### {m[0]} — {m[2]}', '', s[:3500], '']
    L += ['## 11. Business Policies (observed in code — not necessarily intended)', '',
          '- Codes/ids are generated as MAX+1 without locking (~150 sites) — duplicates possible under concurrency (`docs/architecture.md`).',
          '- Credit-limit enforcement is a UI button state only in order→invoice pages (M06 README); editing/rejecting an approved limit makes the customer unlimited (M05).',
          '- Only `CustomerPayment` posts vouchers to the accounts DB; other payment paths and reversals do not (M09 README).',
          '- Two stock models coexist: in-place quantity updates vs. negative-issue ledger rows with weighted-average/batch ledgers (`docs/database/stored-procedures.md`, M10 README).',
          '- Sub-depot stock carries no cost (M12 README).', '',
          '## 12. Exceptions / anomalies', '', f'See [specs/SPEC-GAPS.md](../specs/SPEC-GAPS.md) ({len(GAP_ROWS)} module gaps) and `docs/security.md` risk register.', '',
          '## 13. Integrations', '', 'See [integrations.md](integrations.md) and `specs/integrations/README.md` (14 integrations: SAP, mobile app shared DB, provider dropout, Excel imports, GeoIP, SQL Agent job).', '',
          '## 14. Important Calculations', '',
          'Calculation rules extracted from module rule indexes (lines mentioning formulas/totals/VAT/discount/rounding/average). Authoritative text is in the page specs.', '']
    calc = []
    for m in MODULES:
        for line in bri(m).splitlines():
            if line.startswith('|') and re.search(r'(?i)(calculat|formula|vat|discount|round|weighted|average|total|percent|%|balance|margin|achiev)', line) and 'BR-M' in line:
                calc.append(line)
    L += calc[:120]
    return '\n'.join(L) + '\n'

# ------------------------------------------------------------------ workflows.md
def gen_workflows():
    L = ['# Workflows', '', f'{len(WF_FILES)} traced workflows (page → handler → BLL → DAL → SP/SQL; data and status changes). Each file is under `specs/<module>/workflows/`.', '',
         '## End-to-end business chain (evidence: module READMEs)', '',
         '```', 'Master data (M03/M04/M05) -> Order (M06) -> Proforma/Invoice (M07) -> Picking / Challan / Top sheet / Loading (M07) -> Delivery confirmation', '   -> Payment & voucher (M09)  |  Return (M08)  |  Stock effects (M10/M11/M12) -> SAP hand-off (M17)', '```', '',
         '## Index', '']
    rows = []
    for m, name, f in WF_FILES:
        t = open(f, encoding='utf-8', errors='replace').read()
        h = re.search(r'(?m)^#\s+(.*)', t)
        rows.append([m[0], f"[{name[:-3]}](../specs/{m[1]}/workflows/{name})", (h.group(1) if h else name)[:110]])
    L.append(table(['Module', 'Workflow file', 'Title'], rows))
    return '\n'.join(L) + '\n'

def gen_modules():
    L = ['# Modules', '', 'Module boundaries are INFERRED from folders, page/class/SP naming and code structure (no module registry exists). Full inventory: `docs/reverse-engineering/module-inventory.md`.', '']
    for m in MODULES:
        s = section(READ[m[0]], r'purpose')
        pp = [p for p in ASPX if p['module'] == m[0]]
        L += [f'## {m[0]} — {m[2]}', '', f'- Pages: {len(pp)}; business rules: {BR_BY_MOD.get(m[0][1:], 0)}; workflows: {sum(1 for w in WF_FILES if w[0][0] == m[0])}; report docs: {sum(1 for r in RPT_FILES if r[0][0] == m[0])}; gaps: {GAP_BY_MOD.get(m[0], 0)}',
              f'- Spec: [specs/{m[1]}/README.md](../specs/{m[1]}/README.md)', '', (s.split('\n\n')[0] if s else m[3])[:1500], '']
    return '\n'.join(L) + '\n'

def gen_status():
    L = ['# Status Lifecycle (system level)', '', 'Compiled from each module README. Status columns/values are code-derived; where stored procedures set states (bodies absent) transitions are UNKNOWN.', '']
    for m in MODULES:
        s = section(READ[m[0]], r'status lifecycle|state machine')
        if s: L += [f'## {m[0]} — {m[2]}', '', s[:5000], '']
    return '\n'.join(L) + '\n'

def gen_roles():
    L = ['# Roles & Permissions (system level)', '',
         '**Roles are database data; the repository contains no role list.** Login yields `UserRoleID`, `RoleTypeId`, `RoleTypeName`, `UserType`; the DB function `dbo.MainMenu2(@UserId,@UserRoleID)` (body not in repo) renders the menu; role→menu rows are managed by `UserPermission`/`AdminMenuPanal` (`sp_Save_MenuRole`, `tblMenuDistribution`). Authentication: Forms mode configured but the app authenticates via Session only; `Web.config` has no `<authorization>` element (`docs/security.md`).', '',
         '## Authorization model as implemented (M02)', '', section(READ['M02'], r'authorization model') or '', '']
    for m in MODULES:
        s = section(READ[m[0]], r'roles')
        if s: L += [f'## {m[0]} — {m[2]}: permissions observed', '', s[:3500], '']
    return '\n'.join(L) + '\n'

def gen_gaps_spec():
    L = ['# Spec Gaps', '',
         f'Aggregated from `specs/<module>/GAPS-Mxx.md` ({len(GAP_ROWS)} module-level gaps) plus system-level gaps. Status `Open` = observed, not fixed (this was a documentation-only exercise); `Unverified` = cannot be verified from the repo.', '',
         '## System-level gaps', '',
         table(['GAP ID', 'Module', 'Page', 'Description', 'Impact', 'Source', 'Status'], [
             ['GAP-SYS-001', 'All', 'All', 'No `database/` directory: no DDL for tables, views, functions, triggers, keys, indexes. Only ~41 SP bodies exist (root `*.sql`); ~990 SP names are known by call only.', 'Column/validation/transaction behaviour of most writes UNKNOWN; DB coverage cannot be COMPLETE', 'docs/reverse-engineering/database-inventory.md', 'Open'],
             ['GAP-SYS-002', 'All', f'{TIER["B"]} pages', f'{TIER["B"]} of {len(ASPX)} pages are Tier B (mechanically extracted, not reviewed): purpose, rules, statuses, permissions UNKNOWN', 'Specs are leads not requirements for these pages', 'specs/SPEC-COVERAGE.md', 'Open'],
             ['GAP-SYS-003', 'All', '—', 'No menu data in repo (`dbo.MainMenu2`, `tblMainMenu`, `tblMenuDistribution`): exact navigation and role→page mapping UNKNOWN; module assignment is INFERRED from names', 'Module boundaries/permissions may be wrong for some pages (e.g. `CompanyWiseBranch` is a deposit-slip page filed under M03)', 'specs/00-system/navigation.md', 'Open'],
             ['GAP-SYS-004', 'All', '—', 'No automated tests; `docs/testing.md` lists the manual approach. Test cases in specs are proposed, not executed', 'Regression risk on every change', 'docs/testing.md', 'Open'],
             ['GAP-SYS-005', 'All', '—', 'Non-page components (36: master pages, 24 user controls, handlers, ASMX) have no individual spec', 'Shared code impact only via module READMEs', 'docs/reverse-engineering/page-inventory.md', 'Open'],
             ['GAP-SYS-006', 'M03', 'CompanyWiseBranch', 'Module misclassification found by M03 review: page is a deposit-slip page (belongs to M09)', 'Wrong module link', 'specs/organisation-market-structure/README.md', 'Open'],
             ['GAP-SYS-007', 'All', '—', 'Duplicate/conflicting SP scripts (spInsertReturnInvoice_new, sp_Del_B2BDelete, tbl_INV_CentralStoreStoIssue, sp_Report_DCStore_StockValuationLedger): which version is deployed is UNKNOWN', 'Spec/DB conflict cannot be resolved from repo', 'docs/database/stored-procedures.md', 'Open'],
             ['GAP-SYS-008', 'All', '—', 'Cross-cutting security posture (plaintext passwords, hard-coded `sa` credentials, no `<authorization>`, unauthenticated page methods/viewers) — documented, not fixed', 'High operational risk', 'docs/security.md', 'Open']]), '',
         '## Gap classes (task checklist mapping)', '',
         table(['Class', 'Where recorded'], [
             ['Missing pages / features', 'GAP-SYS-005; per-module GAPS (dead/unlinked pages)'], ['Missing UI/frontend/backend mapping', f'Tier-B pages ({TIER["B"]}); coverage table'], ['Missing APIs', 'docs/api.md (no REST layer; web methods unauthenticated)'],
             ['Missing database mapping', 'GAP-SYS-001; per-module "SP bodies absent" gaps'], ['Missing business rules / permissions / status transitions', 'GAP-SYS-002; per-module GAPS'], ['Missing reports', 'reports/README.md per module (missing .rpt e.g. crpSalesTrendExcelReport.rpt — M13)'],
             ['Missing integrations', 'specs/integrations/README.md (dead config INT-10, disabled HTTP INT-04)'], ['Spec/code conflicts', 'None: no prior specs existed (SPEC-COVERAGE "Prior spec: MISSING")'], ['Spec/database conflicts', 'GAP-SYS-007 (script vs script); code-vs-SP conflicts in M08/M11 GAPS'], ['Unknown behavior', 'All UNKNOWN tags; per-module "still unverified" sections']]), '',
         '## Module gaps', '', f'Counts: {", ".join(f"{k}: {v}" for k, v in sorted(GAP_BY_MOD.items()))}', '',
         '| GAP ID | Module | Page | Description | Impact | Source | Status |', '|---|---|---|---|---|---|---|']
    L += [r for _, r in GAP_ROWS]
    return '\n'.join(L) + '\n'

def gen_re_gaps():
    return f'''# Reverse-Engineering Gaps

Documentation-level gaps of this reverse-engineering baseline (application gaps are in `specs/SPEC-GAPS.md`).

| ID | Area | Gap | Effect | How to close |
|---|---|---|---|---|
| RE-01 | Database | No schema/DDL, only ~41 SP bodies of ~1,034 referenced names | Table/column/validation/transaction facts for most writes are UNKNOWN | Script the live DB (`sys.sql_modules`, tables, FKs, indexes, views, functions incl. `dbo.MainMenu2`, triggers) into a `database/` folder and re-run inventories |
| RE-02 | Pages | {TIER['B']} Tier-B pages not reviewed | Purpose, rules, statuses unknown | Upgrade in module priority order (see `.claude/SKILL.md` §8) |
| RE-03 | Menu/roles | Menu/role data live in DB | Module inference by name; permission matrix per role UNKNOWN | Export `tblMainMenu`, `tblMenuDistribution`, role tables |
| RE-04 | Extraction | Regex-based static analysis: dynamic controls, SQL assembled across methods, and helper builders can be missed/mis-attributed; table names from inline SQL only | Tier-B tables/columns are lower bounds | Verify per page when upgrading |
| RE-05 | Reports | `.rpt` internals (formulas, subreports, SP inside) not opened | Report calculation detail UNKNOWN | Open in Crystal Reports designer |
| RE-06 | Runtime | Nothing was executed (no build, no DB) | Behaviour "inferred" items untested | Run the manual test cases per module |
| RE-07 | Static assets | JS libs, CSS, `PrecompiledWeb`, `packages/`, `graphify-out/` not analysed | Out of scope | — |
| RE-08 | MVC template | `MVC_TreeGrid_Template` not in solution; only stub controller documented | — | — |
'''

def gen_integrations():
    idx = rd('specs/integrations/README.md')
    return '# Integrations\n\nCompiled from `specs/integrations/` (14 proven integrations). Per-integration detail (payloads, tables, SPs, status flags, error handling) is in each `INT-nn` file.\n\n' + idx.split('\n', 1)[1] + '\n'

def gen_integration_map():
    idx = rd('specs/integrations/README.md')
    return '# Integration Map\n\n```\n Browser (WebForms pages, page methods, ASMX) <-> Solution.Web <-> Library.BLL/DAL <-> SalesDisDB_Unicorn_DB\n                                                     |-> accounts DB (voucher posting, cross-DB)\n                                                     |-> SAP (staging DB SAP_API_Data, MakeRESTRequest SP, CSV, HTTP disabled)\n                                                     |-> mobile app (shared DB / WebView pages, sp_Webapi_*, View_Webapi_*)\n                                                     |-> Excel uploads (OLEDB), SMTP (hard-coded, dead code), ipapi.co GeoIP, SQL Agent job\n```\n\n' + idx.split('\n', 1)[1] + '\n'

def gen_api():
    return f'''# API Documentation

The system is **ASP.NET WebForms; there is no REST API layer**. Programmatic endpoints are `[WebMethod]` page methods ({STATS['webmethods']} found), one ASMX service (`SInventoryWebService`), HTTP handlers (`PictureHandler.ashx`, `SignatureHandler.ashx`, `HandlerDocCV.ashx`) and one page route alias (`user-records`). The complete list with endpoint, class, action, parameters, BLL/DAL, SPs and tables is in [reverse-engineering/api-inventory.md](reverse-engineering/api-inventory.md) (API-001…).

## Conventions (observed)
- Transport: ASP.NET AJAX PageMethods (`Page.aspx/Method`, JSON POST) and ASMX script service; `jsonSerialization maxJsonLength=50000000`.
- Authentication: Forms mode configured but not used; endpoints rely on the session cookie only, **and most do not read Session at all**; `Web.config` has no `<authorization>` (docs/security.md; M13/M14/M15/M17 GAPS).
- Errors: unhandled exceptions surface as ASP.NET AJAX 500 responses; no envelope convention; many methods swallow exceptions.
- Validation: per method, mostly absent; SQL often concatenated (see GAPS).

## Where per-page detail is
Page specs §13 (API Mapping): e.g. `PAGE-464` (AdminDashboard, 28 web methods, also `specs/sales-reports-dashboard/workflows/WF-M13-04`), `DoctorModule_UI/FieldForce`, `CommonDataLoad`, `Setup` (M15/M17/M14), integration endpoints in `specs/integrations/INT-11`, `INT-12`.
'''

def gen_database():
    return f'''# Database Documentation

**Source available:** no schema; {STATS['sp_bodies']} stored-procedure bodies from 20 SQL scripts; everything else reverse-engineered from C# call sites. Read [reverse-engineering/database-inventory.md](reverse-engineering/database-inventory.md) first for what is and is not known.

| Object class | Verified in repo | Known by reference |
|---|---|---|
| Stored procedures | {STATS['sp_bodies']} bodies | {STATS['sps']} names ({STATS['sp_conv']} `sp_*`-convention) |
| Tables | ~60 touched by scripts (partial columns) | {STATS['tables']} names in inline SQL |
| Views | 0 definitions | {STATS['views']} (`View_*`) |
| Functions | 0 definitions | 3 called (`dbo.MainMenu2`, `dbo.Remove_SpecialCharacters`, `GetmonthlycustomerSaleDate`) |
| Triggers | 0 | UNKNOWN |
| Constraints / indexes | 1 index, 1 PK, 8 defaults (tblUnitPriceRollback etc.) | UNKNOWN |

## Files
- [database/tables.md](database/tables.md) — script-derived column facts + generated table index (columns observed in code).
- [database/stored-procedures.md](database/stored-procedures.md) — Part A: full analysis of {STATS['sp_bodies']} SP bodies (duplicates/conflicts flagged); Part B: generated interface index of all {STATS['sps']} names.
- [database/views.md](database/views.md), [functions.md](database/functions.md), [triggers.md](database/triggers.md), [relationships.md](database/relationships.md) (inferred joins).

## Data-access patterns
Stored procedures via `DataAccessManager`; inline parameterized SQL via `ClsCommonInternalDAL`/`SInventorySql`; string-concatenated SQL in several DAL methods and pages (SQL-injection risk; see `docs/security.md`). PKs are frequently `MAX+1`. Databases: main `SalesDisDB_Unicorn_DB`, accounts DB (cross-DB voucher posting), SAP staging DB, runtime archive DB (`tblArcDBConnect`).

## Top data-integrity findings (from SP scripts and module reviews)
- Two coexisting stock models (in-place quantity vs ledger of negative issues); `sp_Adjust_DCStore_Stock_Only` can re-inflate stock.
- Voucher `TableID 2018` shared by `ReturnInvoiceId` and `CustReturnId`; returns can be netted against wrong invoices.
- Most writers have no transaction; dynamic SQL concatenated in 7 SPs; customer/doctor save SPs hard-code Approved.
'''

def gen_navigation():
    return f'''# Navigation

## How navigation works (verified)
- **Menu is DB-driven.** `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` executes `SELECT dbo.MainMenu2(@UserId,@UserRoleID)`; the returned HTML is put in `menu.InnerHtml`. The function body and menu rows are not in the repo → exact menu tree per role is **UNKNOWN**. The static menu markup in master pages is commented-out template code. Older `MainMasterPage.master.cs` builds a menu from `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`.
- **Master pages:** `NewMasterPage.master` (current), `MainMasterPage.master`, `MasterPage.master`. 89 pages use none of them (stand-alone, incl. most report viewers).
- **Routing:** file-based (`~/Folder/Page.aspx`); the only alias is `user-records` (`Global.asax`).
- **Landing:** after login → `Dashboard_UI/AdminDashboard.aspx` if `IsMainDashboard`, else a validated `aspxerrorpath` return URL, else `Dashboard_UI/DashboardOne.aspx` (`Login.aspx.cs`).
- **In-app links:** Entry/View/Edit pages link to each other via `Response.Redirect`; per-page inbound/outbound references are listed in each page spec §4. Menu maintenance pages: `UserPermission`, `AdminMenuPanal`.

## Folder → module map
{table(['Web folder', 'Pages', 'Module(s)'], [[f, n, ', '.join(sorted(mods))] for f, n, mods in sorted(_folders(), key=lambda x: x[0])])}

Details: `specs/authentication-navigation/README.md` (login, session, menu), `docs/reverse-engineering/page-inventory.md`.
'''

def _folders():
    d = collections.defaultdict(lambda: [0, set()])
    for p in ASPX:
        d[p['folder']][0] += 1; d[p['folder']][1].add(p['module'])
    return [(k, v[0], v[1]) for k, v in d.items()]

def gen_docs_readme():
    return f'''# Documentation Index — Unicorn Distribution ERP

Reverse-engineered on 2026-09-19 from source (no docs/specs/database folders existed before). **Read `specs/SPEC-COVERAGE.md` and `specs/SPEC-GAPS.md` to know how far to trust each page.**

| Doc | Content |
|---|---|
| [business.md](business.md) | Business overview, terminology, {len(BR_IDS)} rules, lifecycles, calculations |
| [architecture.md](architecture.md) | Solution/layers/data access/config with traced examples |
| [database.md](database.md) + [database/](database/) | DB knowledge, SP bodies, tables, views, functions, triggers, relationships |
| [api.md](api.md) | Web methods/ASMX/handlers (no REST) |
| [security.md](security.md) | Auth, secrets, injection, risk register |
| [coding-standard.md](coding-standard.md) | Conventions as practiced |
| [testing.md](testing.md) / [deployment.md](deployment.md) | Test approach, build & deploy |
| [navigation.md](navigation.md), [modules.md](modules.md), [integrations.md](integrations.md), [workflows.md](workflows.md) | System views |
| [reverse-engineering/](reverse-engineering/) | Inventories (system, module, page, API, database, dependencies), gaps, tools |

Specs: [`../specs/README.md`](../specs/README.md). Claude working rules: [`../.claude/SKILL.md`](../.claude/SKILL.md).
'''

def gen_specs_readme():
    rows = [[m[0], f'[{m[2]}]({m[1]}/README.md)', sum(1 for p in ASPX if p['module'] == m[0]), BR_BY_MOD.get(m[0][1:], 0), sum(1 for w in WF_FILES if w[0][0] == m[0]), sum(1 for r in RPT_FILES if r[0][0] == m[0]), GAP_BY_MOD.get(m[0], 0)] for m in MODULES]
    return f'''# Specs

Feature/page specifications recovered from the running code (nothing was specified before). One spec per `.aspx` page: `specs/<module>/pages/PAGE-nnn-<name>.md` with 31 fixed sections and machine-readable front matter (`tier`, `coverage`).

- **Tier A** ({TIER['A']} pages): reviewed against markup, code-behind and BLL/DAL chain; business rules `BR-Mxx-nnn`.
- **Tier B** ({TIER['B']} pages): mechanical extraction; treat as leads.
- [SPEC-COVERAGE.md](SPEC-COVERAGE.md) — per page/dimension status. [SPEC-GAPS.md](SPEC-GAPS.md) — gaps ({len(GAP_ROWS)} module gaps + system gaps).

| Module | Name | Pages | Rules | Workflows | Report docs | Gaps |
|---|---|---|---|---|---|---|
''' + '\n'.join('| ' + ' | '.join(str(x) for x in r) + ' |' for r in rows) + '''

Other: [00-system/](00-system/) (overview, navigation, roles/permissions, status lifecycle, integration map), [integrations/](integrations/README.md).

Workflow for changes: `.claude/SKILL.md`. Regenerate coverage/inventories with `docs/reverse-engineering/tools/README.md`.
'''

if __name__ == '__main__':
    write('docs/business.md', gen_business())
    write('docs/workflows.md', gen_workflows())
    write('docs/modules.md', gen_modules())
    write('docs/integrations.md', gen_integrations())
    write('docs/api.md', gen_api())
    write('docs/database.md', gen_database())
    write('docs/navigation.md', gen_navigation())
    write('docs/README.md', gen_docs_readme())
    write('docs/reverse-engineering/gaps.md', gen_re_gaps())
    write('specs/README.md', gen_specs_readme())
    write('specs/SPEC-GAPS.md', gen_gaps_spec())
    write('specs/00-system/status-lifecycle.md', gen_status())
    write('specs/00-system/role-permission.md', gen_roles())
    write('specs/00-system/integration-map.md', gen_integration_map())
    write('specs/00-system/navigation.md', gen_navigation())
    print('BR', len(BR_IDS), dict(BR_BY_MOD), 'GAP', len(GAP_ROWS), 'WF', len(WF_FILES), 'RPT', len(RPT_FILES), dict(TIER))
