"""Generate inventory / database / dependency documents from facts.json (mechanical, evidence-based)."""
import sys, os, re, json, collections, glob
import xml.etree.ElementTree as ET
import gen_specs
from gen_specs import *

# ---------------------------------------------------------------- SQL script bodies (only DDL present in the repo)
SQL_FILES = [f for f in glob.glob(os.path.join(ROOT, '*.sql'))] + glob.glob(os.path.join(ROOT, 'Library.DAL', 'SAP_IntegrationDAL', 'sql', '*.sql'))
SP_BODIES = collections.defaultdict(list)   # lower name -> [file:line]
TABLE_ALTERS = collections.defaultdict(list)
for f in SQL_FILES:
    txt = open(f, encoding='utf-8-sig', errors='replace').read()
    rp = os.path.relpath(f, ROOT).replace('\\', '/')
    for m in re.finditer(r'(?im)^\s*(?:CREATE\s+OR\s+ALTER|CREATE|ALTER)\s+PROC(?:EDURE)?\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?', txt):
        SP_BODIES[m.group(1).lower()].append(f"{rp}:{txt.count(chr(10), 0, m.start()) + 1}")
    for m in re.finditer(r'(?im)^\s*ALTER\s+TABLE\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?\s+(ADD|ALTER\s+COLUMN)\s+\[?(\w+)\]?\s*([A-Za-z]+(?:\(\s*\w+(?:\s*,\s*\d+)?\s*\))?)', txt):
        TABLE_ALTERS[m.group(1).lower()].append((m.group(2).upper(), m.group(3), m.group(4), f"{rp}:{txt.count(chr(10), 0, m.start()) + 1}"))
json.dump({k: '; '.join(v) for k, v in SP_BODIES.items()}, open(os.path.join(SP_DIR, 'sp_bodies.json'), 'w'))
gen_specs.SP_BODY = {k: '; '.join(v) for k, v in SP_BODIES.items()}

# ---------------------------------------------------------------- analyses
gen_specs.SP_PAGES = collections.defaultdict(list)
ANALYSES = {}
for p in ASPX:
    a = analyze(p)
    ANALYSES[p['path']] = a
    for s in a['sps']:
        gen_specs.SP_PAGES[s].append(p)
PAGES_BY_TABLE = collections.defaultdict(set)
for p in ASPX:
    a = ANALYSES[p['path']]
    for t in a['tr'] | a['tw']:
        PAGES_BY_TABLE[t].add(p['id'])

def is_sp_like(n):
    return bool(re.match(r'(?i)^(sp|usp)_?', n))

# ---------------------------------------------------------------- SP index
SPI = collections.defaultdict(lambda: {'callers': [], 'params': set(), 'dal_tables_r': set(), 'dal_tables_w': set()})
for path, c in F['cs'].items():
    for m in c['methods']:
        for s in m['sps']:
            e = SPI[s]
            e['callers'].append((m['class'], m['name'], path, m['line'], layer_of(path)))
            e['params'] |= set(m['sqlparams'])
# add param usage when SP method delegates to a param builder in the same class (best-effort)
TBI = collections.defaultdict(lambda: {'read': [], 'write': [], 'cols': collections.defaultdict(set), 'views': False})
for path, c in F['cs'].items():
    for m in c['methods']:
        for t in m['tread']: TBI[t]['read'].append((m['class'], m['name'], path, m['line']))
        for t in m['twrite']: TBI[t]['write'].append((m['class'], m['name'], path, m['line']))
        for t, cols in m['inserts']:
            for col in cols: TBI[t]['cols'][col].add('INSERT')
        for t, cols in m['updates']:
            for col in cols: TBI[t]['cols'][col].add('UPDATE')
        for t, col, pn in m['colmap']:
            TBI[t]['cols'][col].add(f"bound {pn}")
for t in list(TBI):
    if not (TBI[t]['read'] or TBI[t]['write']): del TBI[t]
JOINS = collections.Counter(); JOIN_SRC = {}
for path, c in F['cs'].items():
    for m in c['methods']:
        for a_, b_ in m['joins']:
            k = tuple(sorted([a_, b_]))
            JOINS[k] += 1
            JOIN_SRC.setdefault(k, f"{path}:{m['line']}")

def rel_(p): return p

# ---------------------------------------------------------------- 1. page inventory
def gen_page_inventory():
    rows = []
    for p in ASPX:
        a = ANALYSES[p['path']]
        css = ', '.join(x.split('/')[-1] for x in p['link_css'][:3]) or '—'
        js = ('inline(' + str(p['js_len']) + ' chars)' if p['js_len'] else '') + (' ' + ', '.join(x.split('/')[-1] for x in p['script_src'][:3]) if p['script_src'] else '')
        api = ', '.join([m['name'] for m in a['wm']][:4]) + (f" (+{len(a['wm']) - 4})" if len(a['wm']) > 4 else '')
        rows.append([p['id'], p['module'], p['name'], route_of(p), p['path'].split('/')[-1], f"{p['dattrs'].get('Inherits', '?')}", f"{len(a['handlers'])} methods", js.strip() or '—', css, api or '—',
                     'Present; suspected legacy/duplicate (INFERRED)' if LEGACY_RE.search(p['name']) else 'Present', f"[spec](../../{p['file']})"])
    t = ['# Page Inventory', '', f"_Generated from `Solution.Web` by static analysis. {len(ASPX)} `.aspx` pages + {len(COMPONENTS)} components (master pages, user controls, handlers, services, app files). Module assignment is INFERRED from folder and file name because the menu (`tblMainMenu`) is stored in the database and not in the repository._", '',
         '## Pages', '', table(['Page ID', 'Module', 'Page Name', 'Route', 'View', 'Controller (code-behind class)', 'Action (methods)', 'JavaScript', 'CSS', 'API (WebMethods)', 'Status', 'Spec'], rows), '',
         '## Components (non-page files)', '']
    crow = []
    for p in COMPONENTS:
        cs = p['cs']
        crow.append([p['id'], p['kind'], p['path'], p['dattrs'].get('Inherits') or p['dattrs'].get('Class') or '', p['cs'] or ('CodeBehind ' + p['dattrs'].get('CodeBehind', '') if p['dattrs'].get('CodeBehind') else '—'), f"{len(p['controls'])} controls"])
    t.append(table(['ID', 'Kind', 'Path', 'Class', 'Code-behind', 'Notes'], crow))
    return '\n'.join(t) + '\n'

# ---------------------------------------------------------------- 2. module inventory
def gen_module_inventory():
    t = ['# Module Inventory', '',
         'Modules are **functional groupings discovered from evidence**: web folders (`Solution.Web/*_UI`), page names, DAL/BLL namespaces, SP names and Crystal report names. The application has no explicit module registry in code; the runtime menu is DB-driven. Module boundaries are therefore INFERRED and each page records this in its spec.', '']
    rows = []
    for m in MODULES:
        pp = [p for p in ASPX if p['module'] == m[0]]
        kinds = collections.Counter(p['kind'] for p in pp)
        sps = set(); tbl = set(); wm = 0; rep = set(); dal = set()
        for p in pp:
            a = ANALYSES[p['path']]
            sps |= a['sps']; tbl |= a['tr'] | a['tw']; wm += len(a['wm']); rep |= a['reports']; dal |= a['dal']
        rows.append([m[0], m[2], m[3], len(pp), '; '.join(f"{k}: {v}" for k, v in kinds.most_common()), wm, len(sps), len(tbl), len(rep) + kinds.get('Report viewer', 0), 'Active (pages present)', f"[README](../../specs/{m[1]}/README.md)"])
    t.append(table(['Module ID', 'Module Name', 'Purpose', 'Pages', 'Page kinds', 'WebMethods', 'SPs referenced', 'Tables (inline SQL)', 'Reports (viewers+rpt)', 'Status', 'Spec'], rows))
    t += ['', 'Roles: the application defines roles in the database (`tbl_UserRoleInfo`, `tblRoleType`, `tblUserType`; login returns `UserRoleID`, `RoleTypeId`, `RoleTypeName`, `UserType`). Role names are data, not code, and are therefore UNKNOWN from the repository. See `specs/00-system/role-permission.md`.', '']
    for m in MODULES:
        pp = [p for p in ASPX if p['module'] == m[0]]
        t += [f"## {m[0]} — {m[2]}", '', m[3], '',
              f"- Web folders: {', '.join(sorted({p['folder'] for p in pp}))}",
              f"- DAL classes reached: {', '.join(sorted({c for p in pp for c in ANALYSES[p['path']]['dal']})[:25]) or '—'}",
              f"- BLL classes reached: {', '.join(sorted({c for p in pp for c in ANALYSES[p['path']]['bll']})[:25]) or '—'}",
              f"- Tables read/written by inline SQL: {', '.join(sorted({x for p in pp for x in (ANALYSES[p['path']]['tr'] | ANALYSES[p['path']]['tw'])})[:40]) or '—'}",
              f"- Stored procedures referenced: {len({s for p in pp for s in ANALYSES[p['path']]['sps']})}",
              f"- Pages: " + ', '.join(f"[{p['name']}](../../{p['file']})" for p in pp), '']
    return '\n'.join(t) + '\n'

# ---------------------------------------------------------------- 3. API inventory
def gen_api_inventory():
    rows = []; n = 0
    for path, c in sorted(F['cs'].items()):
        if not path.startswith('Solution.Web') and not path.startswith('MVC_'):
            continue
        for m in c['methods']:
            if m['webmethod']:
                n += 1
                nodes = trace(path, m['name'], line=m['line'])
                sps = sorted({s for x in nodes for s in x['m']['sps']})
                tr = sorted({s for x in nodes for s in x['m']['tread']}); tw = sorted({s for x in nodes for s in x['m']['twrite']})
                pg = PAGE_BY_PATH.get(path[:-3])
                svc = ', '.join(sorted({f"{x['cls']}.{x['name']}" for x in nodes if x['layer'].startswith('Service')}))[:120]
                dal = ', '.join(sorted({f"{x['cls']}.{x['name']}" for x in nodes if x['layer'].startswith('Repository')}))[:120]
                if path.endswith('.aspx.cs'):
                    ep = '~/' + path.split('Solution.Web/')[1][:-3] + '/' + m['name']
                    call = 'jQuery/PageMethods from ' + (pg['id'] if pg else path)
                else:
                    ep = '~/' + (path.split('Solution.Web/')[1] if 'Solution.Web/' in path else path).replace('.cs', '') + '/' + m['name']
                    call = 'asmx web service (see callers below)'
                rows.append([f"API-{n:03d}", 'POST (ASP.NET PageMethod/ASMX default; INFERRED)', ep, f"{m['class']}", m['name'], m['params'][:90], svc or '—', dal or '—', ', '.join(sps)[:150] or '—', ', '.join(tr + tw)[:120] or '—', call, 'Forms-auth session cookie (no per-method authorization found)'])
    t = ['# API Inventory', '',
         'The system is ASP.NET WebForms; it has **no REST controller layer** except one stub MVC controller (`MVC_TreeGrid_Template/Controllers/ReportController.cs`, a template not wired into `Solution.sln`). Programmatic endpoints are: (a) `[WebMethod]` static page methods callable from client script (PageMethods/`$.ajax` to `Page.aspx/Method`), (b) ASMX web services (`SInventoryWebService.asmx`, code in `App_Code/SInventoryWebService.cs`), (c) HTTP handlers (`*.ashx`), (d) the `user-records` route. Mobile-app data endpoints are the many `[WebMethod]` methods in `DoctorModule_UI/CommonDataLoad.aspx.cs`, `Target_UI/CommonDataLoad.aspx.cs`, `Setup.aspx.cs`, `FieldForce.aspx.cs` and `Dashboard_UI/AdminDashboard.aspx.cs` (INFERRED from method names and SP names such as `sp_Webapi_*`).', '',
         f'## Web methods ({n})', '',
         table(['API ID', 'HTTP Method', 'Endpoint', 'Controller (class)', 'Action', 'Request (params)', 'Service (BLL)', 'Repository (DAL)', 'Stored procedure(s)', 'Tables (inline SQL)', 'Caller', 'Authentication / Authorization'], rows), '']
    t += ['## ASMX / handlers / app files', '']
    rr = []
    for p in COMPONENTS:
        if p['ext'] in ('.asmx', '.ashx', '.asax'):
            rr.append([p['id'], p['kind'], p['path'], json.dumps(p['dattrs'])[:200]])
    t.append(table(['ID', 'Kind', 'Path', 'Directive attributes'], rr))
    t += ['', '## Routes', '', '`Global.asax` registers exactly one `MapPageRoute`: `user-records` → `~/DoctorModule_UI/UserRecords.aspx` (source: `Solution.Web/Global.asax`). `SInventory_UI/Global.asax` and `SubDepot_UI/Global.asax` are nested app files (empty handlers; ignored by IIS when not at the application root).', '',
          '## Response fields, error contracts', '', 'Web methods return .NET objects serialised by ASP.NET JSON (`jsonSerialization maxJsonLength=50000000` in `Web.config`) or raw strings/DataTables converted to JSON; per-method response field lists are UNKNOWN at inventory level (see the page spec §13 for pages that were reviewed at Tier A). Errors surface as ASP.NET AJAX exceptions (HTTP 500 with JSON `Message`) — there is no error-envelope convention (INFERRED from absence of a custom handler).', '']
    return '\n'.join(t) + '\n'

# ---------------------------------------------------------------- 4. database inventory & docs
def spmod(s):
    mods = collections.Counter(p['module'] for p in gen_specs.SP_PAGES.get(s, []))
    return ','.join(k for k, _ in mods.most_common(3)) or '—'

def gen_sp_index():
    rows = []
    for s in sorted(SPI, key=str.lower):
        e = SPI[s]
        pg = gen_specs.SP_PAGES.get(s, [])
        callers = sorted({f"{c[0]}.{c[1]}" for c in e['callers']})
        body = SP_BODIES.get(s.lower())
        conv = 'sp-convention' if is_sp_like(s) else 'NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive)'
        rows.append([s, ', '.join(sorted(e['params']))[:140] or '—', '; '.join(callers[:3]) + (f" (+{len(callers) - 3})" if len(callers) > 3 else ''), f"{len(pg)}" + (f" ({', '.join(q['id'] for q in pg[:4])}{'…' if len(pg) > 4 else ''})" if pg else ''), spmod(s), body or 'NOT IN REPO', conv])
    return rows

def gen_database_inventory():
    n_sp = len(SPI); n_conv = sum(1 for s in SPI if is_sp_like(s)); n_tbl = len(TBI)
    views = sorted(t for t in TBI if re.match(r'(?i)^(view_|vw)', t))
    tbl_only = sorted(t for t in TBI if t not in views)
    t = ['# Database Inventory', '',
         '## What is (and is not) in the repository', '',
         '- There is **no `database/` directory** and **no schema dump**. The only SQL in the repository is: 17 ad-hoc deployment/fix scripts at the repo root and 3 scripts in `Library.DAL/SAP_IntegrationDAL/sql/` — see below.',
         f'- Database objects were therefore reverse-engineered from the **C# data-access code**: {n_sp} distinct stored-procedure names are referenced ({n_conv} follow the `sp_*` convention), {n_tbl} table/view names appear in inline SQL.',
         f'- Stored procedures whose bodies are present in the repo: {len(SP_BODIES)} (see `docs/database/stored-procedures.md`). For every other SP only the call interface (name, SqlParameter names, callers, pages) is knowable.',
         '- No `CREATE TABLE` (other than `#temp` tables), `CREATE VIEW`, `CREATE FUNCTION`, `CREATE TRIGGER`, PK/FK/constraint or index definitions exist in the repository except: `CREATE INDEX IX_tblUnitPriceRollback_ProductId` (SP_Create_tblUnitPriceRollback.sql). Counts for functions and triggers are therefore **0 verified / UNKNOWN actual**.',
         '- Databases named in code: `SalesDisDB_Unicorn_DB` (main; `DataBase.SalesDB`), cross-database references `CSTL_ACCDB_Unicorn` (accounts), `CSTL_ERPDB_Unicorn`, `SalesDisDB_SMC_NEWDB` (legacy name), and an archive DB configured at runtime via `tblArcDBConnect` (`ArchiveDbConnectRepository.cs`).', '',
         '## SQL scripts present', '']
    rows = []
    for f in sorted(SQL_FILES):
        txt = open(f, encoding='utf-8-sig', errors='replace').read()
        rp = os.path.relpath(f, ROOT).replace('\\', '/')
        objs = sorted({m.group(1) for m in re.finditer(r'(?im)^\s*(?:CREATE\s+OR\s+ALTER|CREATE|ALTER)\s+PROC(?:EDURE)?\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?', txt)})
        alt = sorted({m.group(1) for m in re.finditer(r'(?im)^\s*ALTER\s+TABLE\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?', txt)})
        rows.append([rp, len(txt), ', '.join(objs) or '—', ', '.join(alt) or '—'])
    t.append(table(['Script', 'Bytes', 'Stored procedures created/altered', 'Tables altered'], rows))
    t += ['', '## Tables referenced by inline SQL', '', table(['Table', 'Read by (DAL/page methods)', 'Written by', 'Columns observed (INSERT/UPDATE)', 'Pages (inline-SQL reach)'],
          [[x, len(TBI[x]['read']), len(TBI[x]['write']), len(TBI[x]['cols']), len(PAGES_BY_TABLE.get(x, []))] for x in tbl_only]), '',
          '## Views referenced', '', table(['View', 'Read by (methods)'], [[v, len(TBI[v]['read'])] for v in views]), '',
          '## Report datasets (typed DataSets, `Library.CrystalReports/SInventory_DS/*.xsd`)', '']
    ns = '{http://www.w3.org/2001/XMLSchema}'
    rr = []
    for f in sorted(glob.glob(os.path.join(ROOT, 'Library.CrystalReports', 'SInventory_DS', '*.xsd'))):
        try:
            tr_ = ET.parse(f).getroot()
            top = [e for e in tr_.findall(ns + 'element')][:1]
            tabs = []
            if top:
                for el in top[0].iter(ns + 'element'):
                    if el is top[0]: continue
                    kids = [k for k in el.iter(ns + 'element') if k is not el]
                    if kids:
                        tabs.append((el.get('name'), [k.get('name') for k in kids]))
            for tn, cols in tabs or [('(none)', [])]:
                rr.append([os.path.basename(f), tn, len(cols), ', '.join(cols)[:220]])
        except Exception as ex:
            rr.append([os.path.basename(f), 'parse error', 0, str(ex)[:80]])
    t.append(table(['XSD', 'DataTable', '# columns', 'Columns'], rr))
    t += ['', '## Crystal Report files', '']
    rpts = sorted(set(glob.glob(os.path.join(ROOT, '**', '*.rpt'), recursive=True)))
    rpts = [r for r in rpts if 'PrecompiledWeb' not in r and 'graphify-out' not in r and '\\bin\\' not in r and '\\obj\\' not in r]
    used = collections.defaultdict(list)
    for p in ASPX:
        for r in ANALYSES[p['path']]['reports']:
            used[os.path.basename(r).lower()].append(p['id'])
    t.append(table(['Report file', 'Location', 'Referenced by page(s)'], [[os.path.basename(r), os.path.relpath(r, ROOT).replace('\\', '/'), ', '.join(used.get(os.path.basename(r).lower(), [])) or 'none found (unused or loaded dynamically)'] for r in rpts]))
    return '\n'.join(t) + '\n', len(rpts)

def gen_tables_md():
    t = ['<!-- GENERATED-INDEX-BEGIN: tables -->', '## Generated table index (inline-SQL evidence)', '',
         'Columns below are **observed** in `INSERT INTO t (cols)` and `UPDATE t SET col=` statements in the C# code and in `SqlParameter` → column bindings. They are a **lower bound** of the real schema (no DDL exists in the repo). Data types, PK/FK, defaults and indexes are UNKNOWN unless stated in the "From SQL scripts" section above.', '']
    for tb in sorted(TBI, key=str.lower):
        if re.match(r'(?i)^(view_|vw)', tb): continue
        e = TBI[tb]
        cols = sorted(e['cols'])
        rd = sorted({f"{c[0]}.{c[1]}" for c in e['read']})
        wr = sorted({f"{c[0]}.{c[1]}" for c in e['write']})
        mods = sorted({p['module'] for p in ASPX if tb in (ANALYSES[p['path']]['tr'] | ANALYSES[p['path']]['tw'])})
        t += [f"### {tb}", '',
              f"- Modules (pages reaching it via inline SQL): {', '.join(mods) or 'none via pages (SP-only or helper use)'}",
              f"- Read by ({len(rd)}): {', '.join(rd[:6])}{' …' if len(rd) > 6 else ''}",
              f"- Written by ({len(wr)}): {', '.join(wr[:6])}{' …' if len(wr) > 6 else ''}",
              f"- Columns observed ({len(cols)}): {', '.join(cols[:60])}{' …' if len(cols) > 60 else ''}",
              f"- ALTERs in repo scripts: " + ('; '.join(f"{a[0]} {a[1]} {a[2]} ({a[3]})" for a in TABLE_ALTERS.get(tb.lower(), [])) or 'none'), '']
    t.append('<!-- GENERATED-INDEX-END: tables -->')
    return '\n'.join(t) + '\n'

def gen_sp_md():
    rows = gen_sp_index()
    t = ['<!-- GENERATED-INDEX-BEGIN: sp -->', f'## Generated stored-procedure interface index ({len(rows)} names)', '',
         'Source: every string literal used as a stored-procedure name in `Library.DAL`, `Solution.Web` code-behind and `App_Code` (first argument of `GetSqlDataReader/GetDataTable/GetDataSet/SaveData*/UpdateData/DeleteData/RunStoreProcedure/...`, `SqlCommand(...)`/`CommandText`, `EXEC name` in inline SQL, and literals matching `sp_*`). '
         '**Parameters** = `SqlParameter` names in the calling DAL/page method(s) (union). **Definition** = where a `CREATE/ALTER PROCEDURE` exists in the repo scripts; `NOT IN REPO` means the body could not be read. Tables/columns/validation of SPs without a body are UNKNOWN.', '',
         table(['Stored procedure', 'Parameters (from callers)', 'Callers (class.method)', 'Pages (count)', 'Modules', 'Definition', 'Name check'], rows), '',
         '<!-- GENERATED-INDEX-END: sp -->']
    return '\n'.join(t) + '\n'

def gen_views_md():
    views = sorted(t for t in TBI if re.match(r'(?i)^(view_|vw)', t))
    t = ['# Database Views', '', '**No `CREATE VIEW` definition exists in the repository.** Views are known only by name from inline SQL. Definitions/columns: UNKNOWN (must be scripted from the live database `SalesDisDB_Unicorn_DB`).', '',
         table(['View', 'Read by (class.method)', 'Modules of pages using it', 'Columns referenced (from SELECT lists: not extracted)'],
               [[v, '; '.join(sorted({f"{c[0]}.{c[1]}" for c in TBI[v]['read']})[:6]), ', '.join(sorted({p['module'] for p in ASPX if v in (ANALYSES[p['path']]['tr'] | ANALYSES[p['path']]['tw'])})) or '—', 'UNKNOWN'] for v in views]), '',
         '## Notes', '', '- `View_CustomerMaster` is the most-used object (216 textual references) and is the read model for customer lookups; `View_Webapi_EmployeeFieldForceInfo` (84 refs) feeds mobile/API endpoints (INFERRED from name).',
         '- `View_*CurrentStock` views (`View_CentralStoreCurrentStock`, `View_DCStoreCurrentStock`, `VIew_SubDepotCurrentStock`, `View_TotalCurrentStockofCompanyWithStockInTransfar`, `View_ProDCStock`) are the stock read models for warehouse / DC / sub-depot / national stock (INFERRED from names).', '']
    return '\n'.join(t) + '\n'

def gen_rel_md():
    t = ['# Inferred Table Relationships', '',
         '**No foreign-key constraints are available** (no DDL in repo). The relationships below are **INFERRED** from `JOIN ... ON a.col = b.col` predicates in inline SQL of the C# code: each row shows how many distinct methods use the join. Cardinality and enforcement are UNKNOWN. Master/detail pairs are inferred from naming (`tblX` / `tblXDetail`).', '',
         table(['Table.Column A', 'Table.Column B', 'Methods using join', 'Example source', 'Confidence'],
               [[a, b, n, JOIN_SRC[(a, b)], 'INFERRED' if n < 3 else 'INFERRED (≥3 uses)'] for (a, b), n in JOINS.most_common(400)]), '', '## Naming-based master/detail pairs (INFERRED)', '']
    names = set(TBI)
    pairs = []
    for x in sorted(names):
        for suf in ('Detail', 'Details', 'Child'):
            y = x + suf
            if y in names: pairs.append([x, y, 'naming'])
    t.append(table(['Master', 'Detail', 'Basis'], pairs))
    return '\n'.join(t) + '\n'

# ---------------------------------------------------------------- 5. dependency map & system inventory
def gen_dependency_map():
    t = ['# Dependency Map', '', '## Project / assembly dependencies (from `Solution.sln` and `*.csproj`)', '']
    rows = []
    for cs in ['Library.DAL/Library.DAL.csproj', 'Library.BLL/Library.BLL.csproj', 'Library.DAO/Library.DAO.csproj', 'Library.CrystalReports/Library.CrystalReports.csproj']:
        x = open(os.path.join(ROOT, cs), encoding='utf-8-sig', errors='replace').read()
        tf = re.search(r'<TargetFrameworkVersion>([^<]+)', x)
        pr = re.findall(r'<ProjectReference Include="([^"]+)"', x)
        refs = re.findall(r'<Reference Include="([^",]+)', x)
        rows.append([cs, tf.group(1) if tf else '?', ', '.join(os.path.basename(a) for a in pr) or '—', ', '.join(sorted(set(refs))[:30])])
    rows.append(['Solution.Web (website project, `Solution.sln` ProjectReferences)', 'v4.0', 'Library.DAL.dll; Library.DAO.dll; Library.BLL.dll', 'CrystalDecisions 13.0.2000.0, log4net 1.2.10, Newtonsoft.Json 13, EPPlus 6.0.3, Enterprise Library Data (`DLL/`)'])
    t += [table(['Project', 'Target framework', 'Project refs', 'Assembly refs'], rows), '',
          'Layering: **Solution.Web → Library.BLL → Library.DAL → SQL Server**, with `Library.DAO` entities shared by all and `Library.CrystalReports` holding `.rpt` + typed datasets. Pages also call DAL classes directly (see `direct_dal` gap flag in page specs).', '',
          '## BLL → DAL class dependencies', '']
    dep = collections.defaultdict(set)
    for path, c in F['cs'].items():
        if path.startswith('Library.BLL'):
            for v, tp in c['vars'].items():
                if tp in class_idx and file_of_class.get(tp, '').startswith('Library.DAL'):
                    for cls in c['classes']: dep[cls].add(tp)
    t.append(table(['BLL class', 'DAL classes used'], [[k, ', '.join(sorted(v))] for k, v in sorted(dep.items())]))
    t += ['', '## Pages per DAL class (top 40)', '']
    cnt = collections.Counter()
    for p in ASPX:
        for d in ANALYSES[p['path']]['dal']: cnt[d] += 1
    t.append(table(['DAL class', 'Pages reaching it', 'File'], [[k, v, file_of_class.get(k, '')] for k, v in cnt.most_common(40)]))
    t += ['', '## Cross-module table usage (tables reached from pages of ≥ 3 modules)', '']
    tm = collections.defaultdict(set)
    for p in ASPX:
        for x in ANALYSES[p['path']]['tr'] | ANALYSES[p['path']]['tw']: tm[x].add(p['module'])
    t.append(table(['Table', 'Modules'], [[k, ', '.join(sorted(v))] for k, v in sorted(tm.items(), key=lambda kv: -len(kv[1])) if len(v) >= 3][:60]))
    t += ['', '## Stored procedures shared by ≥ 3 pages (change-impact hot-spots)', '']
    t.append(table(['Stored procedure', 'Pages', 'Modules'], [[s, len(pg), spmod(s)] for s, pg in sorted(gen_specs.SP_PAGES.items(), key=lambda kv: -len(kv[1])) if len(pg) >= 3][:60]))
    return '\n'.join(t) + '\n'

if __name__ == '__main__':
    write('docs/reverse-engineering/page-inventory.md', gen_page_inventory())
    write('docs/reverse-engineering/module-inventory.md', gen_module_inventory())
    write('docs/reverse-engineering/api-inventory.md', gen_api_inventory())
    dbinv, nrpt = gen_database_inventory()
    write('docs/reverse-engineering/database-inventory.md', dbinv)
    write('docs/reverse-engineering/dependency-map.md', gen_dependency_map())
    # generated blocks for docs/database/* (merged into agent-written files by merge_blocks.py)
    os.makedirs(os.path.join(SP_DIR, 'blocks'), exist_ok=True)
    open(os.path.join(SP_DIR, 'blocks', 'tables.md'), 'w', encoding='utf-8').write(gen_tables_md())
    open(os.path.join(SP_DIR, 'blocks', 'sp.md'), 'w', encoding='utf-8').write(gen_sp_md())
    write('docs/database/views.md', gen_views_md())
    write('docs/database/relationships.md', gen_rel_md())
    stats = {'modules': len(MODULES), 'pages': len(ASPX), 'components': len(COMPONENTS), 'tables': len([t for t in TBI if not re.match(r'(?i)^(view_|vw)', t)]),
             'views': len([t for t in TBI if re.match(r'(?i)^(view_|vw)', t)]), 'sps': len(SPI), 'sp_conv': sum(1 for s in SPI if is_sp_like(s)), 'sp_bodies': len(SP_BODIES),
             'webmethods': sum(1 for c in F['cs'].values() if c['path'].startswith(('Solution.Web', 'MVC')) for m in c['methods'] if m['webmethod']), 'rpt': nrpt,
             'viewers': sum(1 for p in ASPX if p['kind'] == 'Report viewer'), 'joins': len(JOINS)}
    json.dump(stats, open(os.path.join(SP_DIR, 'stats.json'), 'w'), indent=1)
    print(stats)
