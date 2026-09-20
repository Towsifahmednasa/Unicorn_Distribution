# Business Documentation (reverse-engineered)

> Source of truth is the code. Every rule below is tagged CONFIRMED / INFERRED / UNKNOWN in its module spec. **1053 business rules** were recorded (M01: 31, M02: 49, M03: 103, M04: 70, M05: 57, M06: 75, M07: 61, M08: 54, M09: 58, M10: 77, M11: 40, M12: 50, M13: 68, M14: 28, M15: 80, M16: 64, M17: 27, M18: 61). Only 165 of 706 pages were reviewed in depth (Tier A); rules from the other pages are unrecorded unless a module README says otherwise. Stored-procedure bodies are absent for ~1,000 procedures, so many business rules that live in SQL are UNKNOWN.

## 1. Business Overview

The system supports a pharmaceutical (and general) **sales & distribution and field-force operation**: head-office and distribution centres (DC), warehouses and sub-depots hold stock; field officers (MIO/MIA) and the sales hierarchy (national → zone/region → area → territory → market) drive orders for customers (pharmacies/providers) and doctors; orders become proforma/invoices, are picked, loaded and delivered; payments and returns are recorded and posted to an accounts database; stock moves between warehouse, DC, sub-depot and SAP; HR features cover attendance, leave, tour plans and expense claims. (Sources: `docs/architecture.md`, `specs/00-system/system-overview.md`, module READMEs.)

## 2. Business Terminology (INFERRED from names and code; confirm with business owners)

| Term | Meaning as used in code | Confidence |
|---|---|---|
| DC | Distribution centre / depot (tblDCStore, ComUnit = company/distribution unit) | INFERRED |
| WH / Central store | Central warehouse (tblCentralStore) | INFERRED |
| Sub-depot | Secondary depot with its own stack (tblSubDepot*, tblSubInvoice*) | CONFIRMED (code structure) |
| MIO / MIA | Field sales/market officer and assistant (tblMIOInfo, tblMIAInfo) | INFERRED |
| DZSM / RSM / NSM / ASM / AM | Sales hierarchy levels above MIO | INFERRED |
| Proforma invoice | Pre-invoice created from orders, converted to invoice on confirmation | CONFIRMED (M07) |
| Chalan / Challan | Delivery/transfer document | CONFIRMED |
| Top sheet | Delivery/loading summary grouping invoices | INFERRED |
| STO | Stock transport order (inter-store transfer) | CONFIRMED |
| MIGO | SAP goods movement (import pages) | INFERRED |
| DWSP | Day-wise sales plan (target module) | INFERRED |
| DCR / DCP / CVR / RX | Doctor call report / plan / visit report / prescription | INFERRED |
| TADA | Travel & daily-allowance claim | INFERRED |
| ComUnit | Company unit / distribution unit context stored in session | CONFIRMED (Login.aspx.cs) |

## 3. Modules

| ID | Module | Pages | Business rules | Spec |
|---|---|---|---|---|
| M01 | Authentication, Session & Navigation | 11 | 0 | [README](../specs/authentication-navigation/README.md) |
| M02 | User, Role & Permission Administration | 16 | 0 | [README](../specs/user-role-permission/README.md) |
| M03 | Organisation & Market Structure Master | 75 | 0 | [README](../specs/organisation-market-structure/README.md) |
| M04 | Product, Pricing & Promotion Master | 64 | 0 | [README](../specs/product-pricing-promotion/README.md) |
| M05 | Customer & Provider Master | 40 | 0 | [README](../specs/customer-master/README.md) |
| M06 | Order Management | 29 | 0 | [README](../specs/order-management/README.md) |
| M07 | Proforma, Invoicing & Delivery | 43 | 0 | [README](../specs/invoicing-delivery/README.md) |
| M08 | Sales Return & Invoice Return | 20 | 0 | [README](../specs/sales-return/README.md) |
| M09 | Collections, Payments & Money Receipts | 43 | 0 | [README](../specs/collection-payment/README.md) |
| M10 | Warehouse, DC Stock & Inventory | 56 | 0 | [README](../specs/warehouse-inventory/README.md) |
| M11 | Stock Transfer & Chalan Receive | 28 | 0 | [README](../specs/stock-transfer/README.md) |
| M12 | Sub-Depot Operations | 37 | 0 | [README](../specs/sub-depot/README.md) |
| M13 | Sales Reports, Analytics & Dashboards | 75 | 0 | [README](../specs/sales-reports-dashboard/README.md) |
| M14 | Target & DWSP Management | 24 | 0 | [README](../specs/target-management/README.md) |
| M15 | HR, Field-Force & Employee Administration | 74 | 0 | [README](../specs/hr-field-force/README.md) |
| M16 | Doctor / Provider Master, Visits & Prescriptions | 40 | 0 | [README](../specs/doctor-visit-prescription/README.md) |
| M17 | SAP, Mobile-App & External Integration | 19 | 0 | [README](../specs/sap-external-integration/README.md) |
| M18 | Approval Workflow | 12 | 0 | [README](../specs/approval-workflow/README.md) |

## 4. Business Processes

The main chain, as evidenced by pages, tables and module workflows (details in `docs/workflows.md`):

1. **Master data** (M03 structure, M04 products/prices/campaigns, M05 customers, M15/M02 users & employees).
2. **Order** (M06): manual entry/edit, Excel upload, approval of special orders, DC assignment.
3. **Proforma → invoice → picking → challan/top sheet → loading → delivery confirmation** (M07); stock leaves DC stores.
4. **Collection** (M09): customer payments allocated to invoices, money receipts, deposit slips; accounts voucher posting to the accounts DB (only from CustomerPayment).
5. **Returns** (M08): market/delivery returns, return invoices, posting.
6. **Stock** (M10/M11/M12): warehouse stock in/out, adjustments, freeze, transfers DC↔DC/WH/sub-depot, SAP challan hand-off, valuation reports.
7. **Targets** (M14) and **field force** (M15/M16): plans, visits, prescriptions, leave and claims with approvals (M18).
8. **Reporting** (M13 and per-module viewers): Crystal Reports and dashboards.

## 5. Workflows

104 traced workflows exist under `specs/<module>/workflows/`; indexed in [workflows.md](workflows.md).

## 6. Business Rules (compiled from module READMEs)

Each rule row carries its Source, implementation location and Confidence in the module README index; the full text of each rule is in the page spec §19 or the README.

### M01 — Authentication, Session & Navigation (0 rules)

| ID | One-liner | Documented in |
|---|---|---|
| BR-M01-001 | User name then password must be non-blank (trimmed) | PAGE-001 |
| BR-M01-002 | Login = `LoginName` + plaintext `Password` equality + `UserStatus='active'` | PAGE-001 |
| BR-M01-003 | `TOP 1` without ORDER BY; `ComUnitId` non-deterministic for multi-unit users | PAGE-001 |
| BR-M01-004 | Session keys created at login | PAGE-001 |
| BR-M01-005 | `ComUnitId` set only for non-Admin `UserType` | PAGE-001 |
| BR-M01-006 | `IsPasswordChange` null→0 (must change); missing column→"True" | PAGE-001 |
| BR-M01-007 | Landing: IsMainDashboard → AdminDashboard; deep-link; else DashboardOne | PAGE-001 |
| BR-M01-008 | Deep-link ignored only if it starts with `http` | PAGE-001 |
| BR-M01-009 | No lockout/attempt counter; `tblLoginLog` write disabled | PAGE-001 |
| BR-M01-010 | Login inserts `tblUserSessionTracking` row, errors swallowed | PAGE-001 |
| BR-M01-011 | Active tracking rows with same SessionId closed at login | PAGE-001 |
| BR-M01-012 | Client IP resolution from headers; public IPs geolocated via ipapi.co | PAGE-001 |
| BR-M01-013 | `NewMasterPage`: on first load (not postback) missing `Session["UserId"]` → blank keys, redirect `../Login.aspx`; any exception in header/menu code also redirects | WF-M01-02 (`NewMasterPage.master.cs:19-62`) — CONFIRMED |
| BR-M01-014 | Menu HTML = `dbo.MainMenu2(UserId, UserRoleID)` injected into `menu.InnerHtml` | WF-M01-02 (`NewMasterPage.master.cs:69-75`, `UserDAL.cs:33`) — CONFIRMED (function body UNKNOWN) |
| BR-M01-015 | Forced-change modal shown when `IsPasswordChange` is false/"false"/"0" on all pages except ChangePassword; UI-only | PAGE-009, WF-M01-03 |
| BR-M01-016 | Logout blanks 4 session keys and redirects; no Abandon, no tracking close | WF-M01-05 (`NewMasterPage.master.cs:88-95`) — CONFIRMED |
| BR-M01-017 | Header shows `EmpName (EmpMasterCode)` and `DesigName` | WF-M01-02 (`NewMasterPage.master.cs:28-39`) — CONFIRMED |
| BR-M01-018 | New password 12–20 chars with lower, upper, digit, special | PAGE-009 |
| BR-M01-019 | New password must differ from current (plaintext) | PAGE-009 |
| BR-M01-020 | Confirm must equal new | PAGE-009 |
| BR-M01-021 | Update sets Password (raw), IsPasswordChange=1, UpdateBy/Date; no old-password check | PAGE-009 |
| BR-M01-022 | UserProfile accepts any non-empty matching password; no policy | PAGE-010 |
| BR-M01-023 | Legacy menu row = name + `../folder/page` or `#` + parent | PAGE-002 |
| BR-M01-024 | Menu/grant `SL = MAX+1` | PAGE-002, PAGE-008 |
| BR-M01-025 | At most 4 menu levels in the admin UI | PAGE-002 |
| BR-M01-026 | Legacy grant row = (UserId, MenuSL, Status=true); granted rows locked | PAGE-008 |
| BR-M01-027 | Granting a child auto-grants parent and grandparent (not for "save all") | PAGE-008 |
| BR-M01-028 | Revoking removes descendants to 3 levels | PAGE-008 |
| BR-M01-029 | `MainMasterPage`: `UserId==1` sees whole tblMainMenu | PAGE-008, WF-M01-02 (`MainMasterPage.master.cs:49`) |
| BR-M01-030 | Location trail needs employee+date; blanks dropped; sorted by time; start/end labelled | PAGE-011 |
| BR-M01-031 | Excel export is client-side HTML `.xls` | PAGE-011 |

### M02 — User, Role & Permission Administration (0 rules)

| ID | One-liner | Page |
|---|---|---|
| BR-M02-001…013 | UserSetup: user type required; type→person link (1/3 employee, 6 DA); mandatory fields; DA id in `EmpInfoId`; DA default login=password=code and auto-edit; exclusive dashboard flag; Active/Inactive; DC block only for roles 29/14/25/18/20/"DA ASSISTANCE"; DC CSV → `sp_Save_UserCompanyUnit`; duplicate check via SPs; create vs update by hidden id, `@EntryBy`=UserId; IMEI optional; plaintext password shown | PAGE-015 |
| BR-M02-014…016 | UserSetupforEMP: IMEI mandatory, DC only role 14, no dashboard flags, `?EmpID=` copies EmpInfoId into user id | PAGE-016 |
| BR-M02-017…023 | OrderPermission: zone/area filter; candidate list; ≥1 row; validation non-blocking; EntryBy=LoginName; last-row result; no consumer in repo | PAGE-017 |
| BR-M02-024…027 | ApprovalStepMap: unique order/role per chain; default step 1; master+detail save without txn; role *types* | PAGE-026 |
| BR-M02-028…035 | Role permission: Permission flag stored only for ticked rows; auto-tick; replace-set save; capability flags; App shows Permission only; role required; endpoints unauthenticated; Add/Edit flags barely consumed | PAGE-027 |
| BR-M02-036…041 | DCUserPermission: user+DC required; replace set; warehouse id 15 → CWH flag; exactly 11 DCs → National flag; unfiltered lists; two writers | PAGE-020 |
| BR-M02-042…045 | UserRoleEntry: mandatory fields; edit blocked by check SP; no create duplicate check; entryBy | PAGE-013 |
| BR-M02-046…049 | ApprovalSetupEntry: approvers per menu; selectable filters; consumer check mostly disabled; concatenated SQL | PAGE-019 |

### M03 — Organisation & Market Structure Master (0 rules)

_No rules index found in module README._

### M04 — Product, Pricing & Promotion Master (0 rules)

| BR | One-liner | Page |
|---|---|---|
| 001 | Campaign header mandatory fields (name, customer type, campaign type, category, product line, dates) | 111 |
| 002 | Campaign type 1/2/3 decides mandatory trigger inputs (product+qty / product+min+max / min+max) | 111 |
| 003 | >= 1 offer row | 111 |
| 004 | >= 1 market or customer row (unsatisfiable on live page for new records) | 111 |
| 005 | Offer type to column mapping (percent / amount / qty) per campaign type | 111 |
| 006 | Offer-row entry validation | 111 |
| 007 | Detail DTO mapping (Quantity, BonusQuantity, DiscountPercentage, QuantityDteail) | 111 |
| 008 | One campaign master per selected customer type | 111 |
| 009 | Payment type COD/NCOD/Both -> IsPTforCOD/IsPTforOther | 111 |
| 010 | Trade Policy vs FCFS exclusive on client only | 111 |
| 011 | Ratio-wise increment flag per detail with bonus product | 111 |
| 012 | Edit = delete old master + insert new id | 111 |
| 013 | Session map id 4 passed to sp_Update_BonusCampaignpkCampaignSetupId | 111 |
| 014 | Save atomic per call; per-type loop not atomic overall | 111 |
| 015 | No date-order/overlap/duplicate validation | 111 |
| 016 | E-mail notification (disabled on live page); hard-coded SMTP secret | 111 |
| 017 | Edit-load customer-type restore (CustomerTypeIdNew list) | 111 |
| 018 | Campaign list filter (today's validity unless status chosen) | 116 |
| 019 | List de-duplicates by name+type (keeps last row) | 116 |
| 020 | New Entry hidden for EmpInfoId 496 | 116 |
| 021 | Sync Now -> sp_CampaignUpdateFromPage | 116 |
| 022 | _final: in-place update, children re-inserted | 112 |
| 023 | _final: multiple-product vs manual-ration exclusive | 112 |
| 024 | _final: manual ration bands seeded/persisted, unvalidated | 112 |
| 025 | _final: multiple product rows | 112 |
| 026 | _final: category/FCFS/ratio/payment not persisted | 112 |
| 027 | _final: real SMTP send | 112 |
| 028 | _new: multi-product UI is a non-persistent stub | 114 |
| 029 | PT/OLd identical code; payment flags null; PT radio unbound | 115, 117 |
| 030 | _Mult: multi-select product not implemented, offer product list empty | 113 |
| 031 | Quoted price header required fields | 118 |
| 032 | Quoted price customer rule (create list / edit single) | 118 |
| 033 | >=1 product ticked with final amount | 118 |
| 034 | Create fans out one master per customer; cumulative detail-list bug | 118 |
| 035 | Quoted price arithmetic (see section 7) | 118 |
| 036 | UnitPrice=final amount, Vat=discount % storage | 118 |
| 037 | Edit re-saves details without visible delete | 118 |
| 040 | Product create mandatory fields | 147 |
| 041 | Product code uniqueness (create only) | 147 |
| 042 | Update omits PackSizeId/CaseId, no validation | 147 |
| 043 | Image upload client-only type check | 147 |
| 044 | Product group 1 selects all DCs; other value clears | 147 |
| 045 | UOM list needs UOMSAPCode (never set by app) | 147 |
| 046 | DC mapping replaced wholesale; separate transaction | 147 |
| 047 | ProductId = MAX+1 | 147 |
| 048 | No inactivation guard | 147 |
| 050 | Price fields required, no VAT formula or range checks | 157 |
| 051 | One price row per product on create | 157 |
| 052 | Price replace flow via edit + Inactive | 157 |
| 053 | Old price archived to tblUnitPriceRollback and deleted; two transactions | 157 |
| 054 | Archive never displayed | 157 |
| 055 | TPVat = UnitPrice + VATAmountPerUnit; downstream lookups assume one row | 157 |
| 056 | Edit mode via Session["UnitPrice"], cleared on load | 157 |
| 058 | MIO can be in one promo group (SP check on tick only) | 125 |
| 059 | Promo MIO tag save requirements; removal depends on SP | 125 |
| 060 | Active MIOs only; promo products ProductGroupId IN (2,3) | 125 |
| 061 | Possible duplicate tag master; dead WebMethod path | 125 |
| 062 | Promo group code = 'PG-' + (1000 + MAX(id) + 1); name unrestricted (PromoGroupDAL.cs:17-40, PAGE-123 Tier B) | 123 |
| 063 | Group-wise promo qty mandatory inputs | 120 |
| 064 | Stock guard checks only first ticked row | 120 |
| 065 | Stock = total central stock, no duplicate check | 120 |
| 066 | Common quantity fill | 120 |
| 067 | Month name vs date representation | 120 |
| 068 | Edit not implemented; Update re-saves | 120 |
| 069 | Dead legacy AJAX path | 120 |
| 070 | Special discount lookup tblProductDiscount by customer+product+date (invoice pages) | WF-06 |
| 071 | Trade-policy tier table chosen by date (13-Dec-2024) and OldTradePolicy flag | WF-06 |
| 072 | Invoice line discount/net formulas | WF-06 |
| 073 | Hard-coded campaign names in order DAL | WF-06 |
| 074 | ProductDiscountEntry / ProductBonusEntry saves disabled | 143, 145 |

### M05 — Customer & Provider Master (0 rules)

| BR | One-liner | Where |
|---|---|---|
| 001 | Only name, type, mobile, address mandatory on CustomerEntry | 168 |
| 002 | Mobile exactly 11 chars | 168 |
| 003 | Same handler creates/updates by MID; user = UserId | 168 |
| 004 | Code `C`+max approved suffix+1, SP-generated | 168 |
| 005 | Web-created customer auto-Approved (`ActionStatus='2'`) | 168 |
| 006 | Geography/station types derived from market, override form | 168 |
| 007 | Tagged sales persons ids/codes; first = SalesPersonId | 168 |
| 008 | Save atomic; update rebuilds product lines | 168 |
| 009 | Update never changes code/status/DC | 168 |
| 010 | No duplicate check; "Already Exist!" generic | 168 |
| 011 | Non-admin restriction dead code | 168 |
| 012 | Provider/platform dropdown filtered by customer flags | 168 |
| 013 | Inactive shown with "(Inactive)" | 168 |
| 014 | Legacy entry: all fields mandatory | 186 |
| 015 | Unique customer code; PK = MAX+1 in app | 186 |
| 016 | Defaults (shipping "N/A", Active); inactive date rule | 186 |
| 017 | Fixed flag, ComUnitName parsing | 186 |
| 018 | Legacy insert omits status/audit/ids | 186 |
| 019 | Credit limit needs company, customer, amount>0, days>0 | 190 |
| 020 | One active limit per company+customer | 190 |
| 021 | Create/edit ⇒ `Posted` | 190 |
| 022 | Approval queue = Posted | 189 |
| 023 | Batch approve/reject, no role/SoD | 189 |
| 024 | Enforce only active+Approved; rows summed | 189 |
| 025 | Permission = no limit OR (order+due ≤ limit AND age ≤ days) | 189 |
| 026 | Due / oldest-unpaid definitions | 189 |
| 027 | Block is UI-level only | 189 |
| 028 | RemainingCreditDays on invoice print | 189 |
| 029 | Credit delete = IsActive 0 | 189 |
| 030 | Excel parsing rules (.xls/.xlsx, first sheet) | 192 |
| 031 | Upload requires date, manufacturer, rows, 20 non-blank columns | 192 |
| 032 | Batch header `CMEF-n`, flags false | 192 |
| 033 | Row failures swallowed | 192 |
| 034 | Verify/transfer/delete gates | 192 |
| 035 | Property import = pending batch | 203 |
| 036 | Update type radio → ConvertType; 5 columns | 203 |
| 037 | Batch atomic | 203 |
| 038 | No code validation; blank batch possible | 203 |
| 039 | Import approval: headers only, approve only | 203 |
| 040 | Doctor import columns/type mapping | 205 |
| 041 | Doctor approval chain | 205 |
| 042 | Provider-type search needs a type; some filters ignored | 167 |
| 043 | Provider-type change immediate | 167 |
| 044 | Only customer-flagged provider types | 167 |
| 045 | Default 30-day window, no mandatory scope | 173 |
| 046 | Add/Edit by RAdd/REdit; role 2 bypass | 173 |
| 047 | ActionStatus 0/1/2/3 labels | 173 |
| 048 | Duplicate mobile flagged in approval queue | 173 |
| 049 | Salesperson filter primary or tagged | 173 |
| 050 | Doctor code `D`+max+1, auto-approved | 205 |
| 051 | Doctor edit rebuilds child rows | 205 |
| 052 | DIC DC preset but not filtered | 167 |
| 053 | Customer type: dup name check on edit; default-check dead | WF-07 |
| 054 | Program/SMC/station type save patterns (dup ⇒ failure) | WF-07 |
| 055 | Legacy category: `CUS-nnn` code, unique name | WF-08 |
| 056 | Bulk reassign skips customers with invoices | WF-08 |
| 057 | Fixed/regular flag upload sequence | WF-08 |
| 058 | Legacy edit guards differ between 185 and 194; proc-name mismatch | WF-08 |
| 059 | Tag-change batches `CTME-n`, own tables/SPs | WF-04 |

### M06 — Order Management (0 rules)

Ids are sequential across the module; the rule text, source lines and confidence are in the page specs named.
| BR | One-liner | Where |
|---|---|---|
| BR-M06-001 | Invoiced order cannot be edited (page-level only) | PAGE-221 |
| BR-M06-002 | Sales person mandatory | PAGE-221 |
| BR-M06-003 | Customer mandatory/resolvable | PAGE-221 |
| BR-M06-004 | DC mandatory | PAGE-221 |
| BR-M06-005 | ≥1 row, qty>0; unresolved rows silently dropped | PAGE-221 |
| BR-M06-006 | Totals: Gross=Σ TP×qty, VAT=Σ vat×qty, no discount, Net=Gross+VAT | PAGE-221 |
| BR-M06-007 | Default sales person/DC derived from customer tags and user-DC map | PAGE-221 |
| BR-M06-008 | Order-number generation (page proposal vs SP `ODR-yyyymmdd<id>`) | PAGE-221 |
| BR-M06-009 | Manual orders born approved/invoice-ready (`ActionStatus='2'`…) | PAGE-221 |
| BR-M06-010 | Market hierarchy snapshotted onto order | PAGE-221 |
| BR-M06-011 | Edit deletes and re-inserts all lines | PAGE-221 |
| BR-M06-012 | TP editable, no limits | PAGE-221 |
| BR-M06-013 | Gift/campaign flags; inverted computed campaign flag on edit | PAGE-221 |
| BR-M06-014 | Approver options driven by user+page step config | PAGE-220 |
| BR-M06-015 | Queue = `IsSpecialApproval=1` orders at the user's step status | PAGE-220 |
| BR-M06-016 | Operation and ≥1 order required | PAGE-220 |
| BR-M06-017 | Approval = overwrite ActionStatus/ApprovedBy/ApprovedDate, no re-check | PAGE-220 |
| BR-M06-018 | Approver sees customer due (two formulas) | PAGE-220 |
| BR-M06-019 | Eligibility filter for invoicing list | PAGE-217 |
| BR-M06-020 | Credit permission formula (limit + day-limit) | PAGE-217 |
| BR-M06-021 | Credit block is UI-only | PAGE-217/218 |
| BR-M06-022 | Defaults / session memory of filters | PAGE-217 |
| BR-M06-023 | Session handoff of order id | PAGE-217 |
| BR-M06-024 | Bulk generation needs selection + DA; UI column hidden | PAGE-217 |
| BR-M06-025 | Same-day duplicate check unreachable | PAGE-217 |
| BR-M06-026 | Bulk errors swallowed | PAGE-217 |
| BR-M06-027 | Auto-delete of proforma on header/detail mismatch >10 | PAGE-217 |
| BR-M06-028 | One proforma per order | PAGE-218 |
| BR-M06-029 | Order lines re-verified before save | PAGE-218 |
| BR-M06-030 | Required: order no/date, pay type, DA | PAGE-218 |
| BR-M06-031 | No blank line | PAGE-218 |
| BR-M06-032 | Receivable ≥ 0 | PAGE-218 |
| BR-M06-033 | Stock cap / zero-stock lines Undelivered | PAGE-218 |
| BR-M06-034 | Line net formula; order price overrides list price | PAGE-218 |
| BR-M06-035 | Header totals + recompute from details | PAGE-218 |
| BR-M06-036 | Customer credit spread equally, consumed via tblReturnAmount | PAGE-218 |
| BR-M06-037 | Discount % source (trade policy, INSTITUTION/RCP, FCB FY-23-24) | PAGE-218 |
| BR-M06-038 | Hard-coded campaign names | PAGE-218 |
| BR-M06-039 | Gift lines zero-priced | PAGE-218 |
| BR-M06-040 | Batch-wise stock issue (negative Issue rows) | PAGE-218 |
| BR-M06-041 | Invoice number `INV-<DC><10-digit MAX(InvoiceId)+1>` | PAGE-218 |
| BR-M06-042 | Batch tagging tblInvoiceBatch | PAGE-218 |
| BR-M06-043 | Header recomputed from details | PAGE-218 |
| BR-M06-044 | Delivery charge in grand total/receivable | PAGE-218 |
| BR-M06-045 | FixedCustomer copied | PAGE-218 |
| BR-M06-046 | Compensation `sp_Deletenvoice` on failure | PAGE-218 |
| BR-M06-047 | Redirect if grid ≠ order lines | PAGE-218 |
| BR-M06-048 | Print may delete anomalous invoice | PAGE-218 |
| BR-M06-049 | Line delivery status Full/Partial/Reject | PAGE-215 |
| BR-M06-050 | Invoice-level status roll-up | PAGE-215 |
| BR-M06-051 | One confirmation per invoice; DEL-/RTN- numbering | PAGE-215 |
| BR-M06-052 | Reason required for Reject (typo skips Partial) | PAGE-215 |
| BR-M06-053 | Undelivered qty returned to stock as Receive row | PAGE-215 |
| BR-M06-054 | Delivery figures + journal posting | PAGE-215 |
| BR-M06-055 | Payment* columns mirrored at confirmation | PAGE-215 |
| BR-M06-056 | Hard-coded product offers, `ProductCode2` defect | PAGE-215 |
| BR-M06-057 | Same status logic (legacy variant) | PAGE-214 |
| BR-M06-058 | Forced auto full-cash payment | PAGE-214 |
| BR-M06-059 | Double `DEL-DEL-` prefix | PAGE-214 |
| BR-M06-060 | No guard / no posting in legacy variant | PAGE-214 |
| BR-M06-061 | Short-delivered qty returns to stock | PAGE-214 |
| BR-M06-062 | "Parial" typo | PAGE-214 |
| BR-M06-063 | Delete offers only un-invoiced orders | PAGE-222 |
| BR-M06-064 | SP refusal reported as "Proforma Already Generated" | PAGE-222 |
| BR-M06-065 | UserId passed as `@LoginName` | PAGE-222 |
| BR-M06-066 | DC-change page lists only DC-less orders; filters inert | PAGE-207 |
| BR-M06-067 | Assignment needs selection + DC + route | PAGE-207 |
| BR-M06-068 | Routes limited to selected DC | PAGE-207 |
| BR-M06-069 | Upload needs document date, manufacturer and rows; columns mapped by fixed position; only first sheet read | PAGE-225 (`OrderListUpload.aspx.cs:41-107,153-210`) — CONFIRMED, Tier-B page |
| BR-M06-070 | Uploaded master starts `GenerateOrder=false`, `EntryBy=LoginName` | PAGE-225 (`:114-122`) — CONFIRMED |
| BR-M06-071 | Master can be deleted only while `GenerateOrder=false` | PAGE-224 (`OrderList.aspx.cs:46-62`) — CONFIRMED |
| BR-M06-072 | Pending list = staged orders of a DC/date range not present in `tblOrder` for that range; DC + dates required | PAGE-231 (`PendingOrderDal.cs:15-29`; `PendingOrder.aspx.cs:61-71`) — CONFIRMED |
| BR-M06-073 | Pending re-import runs single-order SP only if code absent from `tblOrder`; message always "Order Processed" | PAGE-231 (`PendingOrderBll.cs:20-53`) — CONFIRMED |
| BR-M06-074 | OrderEdit changes only DC id/code/name of an order by id (no invoice-state check, no route change) | PAGE-223 (`OrderEdit.aspx.cs:49-71`; `OrderListDAL.cs:270-280`) — CONFIRMED, Tier-B page |
| BR-M06-075 | OrderListView lists all orders with `IsInvoice='0'` and previews them with totals (Σ qty, TP, VAT, Net) | PAGE-226 (`OrderInfoDAL.cs:592-601`; `OrderListView.aspx.cs:87-107`) — CONFIRMED, Tier-B page |

### M07 — Proforma, Invoicing & Delivery (0 rules)

| BR | One-liner | Page |
|---|---|---|
| 001-005 | Proforma bulk creation calls `sp_AutoInvoiceGeneration` per ticked order; order grid never populated; unconditional success; SC/market scoping; defaults | PAGE-272 |
| 006-010 | Manual invoice header from order code, dup-product rule, product info, line calc (TP, VAT, discount, net), header sums | PAGE-267 |
| 011 | Invoice no `INV-<ComUnitCode>` + global MAX(InvoiceId)+1 padded 10 | PAGE-267 |
| 012 | tblInvoiceBatch batch number (route prefix) | PAGE-267 |
| 013-014 | FEFO stock allocation on tblDCStore Receive rows, Issue rows negative, bonus handling, per-batch detail split | PAGE-267 |
| 015-017 | Gift zero-pricing; persisted detail formulas; header recomputed from details (+DeliveryCharge, -AdjustAmount) | PAGE-267 |
| 018-019 | No transaction; manual page fails at Isgift (no OrderDetailsId) leaving orphan header | PAGE-267 |
| 020-025 | Payment-confirmation worklist (`PaymentInvoiceNo IS NULL`), single-item status dropdown, Go-to-Partial, dead ALL Submit, total label, dead redirect | PAGE-264 |
| 026-033 | Loading summary: filters, status lists, common status, Sales Confirm 3-call chain, Cash/Rejection, Partial redirect, success flag, mid-loop rebind | PAGE-271 |
| 034-036 | Top sheet search per DA; Search auto-deletes anomalous proformas; anomaly criterion (>10, after 2022-11-16) | PAGE-277 |
| 037-039 | `sp_Delete_ProformaInvoice`: only proforma, stock reversal (delete Issue row with exact qty), bookkeeping + order reset | PAGE-277 |
| 040-041 | Top-sheet code per click; report parameters ids vs numbers | PAGE-277 |
| 042-046 | DC picking: DC scoping bug, search excludes picked, create without validation, DPK numbering, slip totals | PAGE-260 |
| 047-053 | Warehouse picking (issue) rules | PAGE-259 |
| 054-057 | Challan list/edit/export/print rules | PAGE-257 |
| 058-061 | Excel old-data upload and confirmation rules | PAGE-266 |

### M08 — Sales Return & Invoice Return (0 rules)

| BR | Rule (abridged) | Page |
|---|---|---|
| BR-M08-001 | A market return is created without any link to an original invoice/order: InvoiceId=0, OrderId=0; OrderNo forced to "No order"; SP writes IsSalesReturnWithoutOrder='No order', SubInvoiceId=0. The list page filters on IsSalesReturn... | PAGE-294 |
| BR-M08-002 | DC is mandatory; DC choice order: dropdown → Session DICCompanyUnitId → hdComUnitId. Stock and price are looked up for the chosen DC only. | PAGE-294 |
| BR-M08-003 | The DC list is the user's DCs (tblUserCompanyUnit); if the user has ≤1 mapped DC (Items.Count ≤ 1 incl. placeholder) the list falls back to all company units. | PAGE-294 |
| BR-M08-004 | Customer, sales-person (MIO) name, return date and remarks are mandatory. | PAGE-294 |
| BR-M08-005 | Every return line requires an expiry date and batch no; manufacturing date is optional; there is no relation to an existing batch. | PAGE-294 |
| BR-M08-006 | Only products with tblProduct.GroupId = 1 are offered for return; selected quantity must be a positive whole number at selection time. | PAGE-294 |
| BR-M08-007 | Line calculation: TP = unitPrice×qty; VAT = unitVAT×qty; discount % = trade-policy % (non-fixed customers; 0 if FixedCustomer or lookup fails) replacing the product-discount % computed in RecalculateRow; discount amt = TP×%/100; N... | PAGE-294 |
| BR-M08-008 | Net Return Amount = max(Return Total − Deduction Charge, 0); if the posted net is ≤0 while grand>0 it is recomputed the same way. Deduction has no upper validation. | PAGE-294 |
| BR-M08-009 | Unit price and cost price shown in the grid are user-editable; the entered cost price (cogsTextBox) becomes CostPrice on the detail, CostPerUnit in tblDCStore, and the base of the COGS reversal voucher. | PAGE-294 |
| BR-M08-010 | Return invoice number = 'RIN--'+yymmdd+(MAX(ReturnInvoiceId of tblReturnInvoice rows dated same day, default 10000)+1001) — uses the global identity value, not a per-day counter. | PAGE-294 |
| BR-M08-011 | Stock effect: for each line with returned qty>0 and a DC, a new tblDCStore 'Receive' row is inserted (location "Depot Return Store", StockCondition "Non-Transcational Sales Return", TotalQuantity=StockQty=qty, ChalanNo=return invo... | PAGE-294 |
| BR-M08-012 | Posting is automatic after save (sp_SalesReturnPosting): idempotent per return (skips if a voucher with TableID=2018 (AR) / 2023 (COGS, IsInventoryValuation=1) and CommonPK_ID=ReturnInvoiceId exists). | PAGE-294 |
| BR-M08-013 | AR-reversal voucher (Journal, category 2, status 'Approved', by 'Auto Process', posting date = return date): Dr Sales Return GL (PageId 23/FlagId 1020, fallback 4170) = ReturnNetAmount; Cr Delivery/Deduction Income (23/1032, fallb... | PAGE-294 |
| BR-M08-014 | COGS/inventory voucher: Dr Finished-Goods Inventory (23/1018, fb 4156) / Cr COGS (23/1019, fb 4162) for Σ (TotalQuantity or Quantity) × (CostPrice, else latest tblDCStore.CostPerUnit>0 of the product in any DC); IsInventoryValuati... | PAGE-294 |
| BR-M08-015 | Customer subsidiary (type 1) and reference info (type 1018) rows are created on the fly in the accounts DB if missing. Financial year = active year of company 1 (else the year containing posting date, else hard-coded id 2). | PAGE-294 |
| BR-M08-016 | Voucher code = 'JVP-'+yymmdd+(max of last 5 digits of same-day JVP codes, default 10000)+1; IsPosting=1 set on tblReturnInvoice at the end. | PAGE-294 |
| BR-M08-017 | No VAT-liability reversal is posted: VAT is inside the Sales Return debit (gross) and the receivable credit; deduction is booked as income without VAT. | PAGE-294 |
| BR-M08-018 | tblReturnAmount.Amount = Return Total (gross, before deduction). | PAGE-294 |
| BR-M08-019 | The flow is only for invoices already payment-confirmed (PaymentInvoiceNo not null) and not yet second-returned (SndReturnInvoiceNo null) — enforced by the list query, plus a server duplicate guard on SndReturnInvoiceNo. | PAGE-298 |
| BR-M08-020 | The user must press Calculate after any DQty change and before saving. | PAGE-298 |
| BR-M08-021 | DQty (accepted quantity) defaults to the previously paid quantity and cannot exceed it (values ≥ previous are reset to previous). | PAGE-298 |
| BR-M08-022 | Line status: DQty = TQty ⇒ Full; 0 < DQty < TQty ⇒ Partial; DQty = 0 (or DQty > TQty, reset to 0) ⇒ Reject. Reason dropdown enabled for Partial/Reject. | PAGE-298 |
| BR-M08-023 | A reason is mandatory only for lines whose status literal equals "Reject" or the misspelt "Parial" — Partial lines effectively skip the check. | PAGE-298 |
| BR-M08-024 | Header status: every line Full ⇒ Full; every line Reject ⇒ Reject; otherwise Partial. Stored in tblInvoice.sndReturnInvoiceStatus; SndReturnInvoiceNo='SRN-'+InvoiceNo. | PAGE-298 |
| BR-M08-025 | Stock effect: for Partial/Reject lines add (PaymentQuantity − DQty) back to the original tblDCStore batch row (StockQty = StockQty + qty); Full lines: none. Bonus quantity is forced to 0. | PAGE-298 |
| BR-M08-026 | Line snapshot stored in tblInvoiceDetailReturn with PreviousQuantity = payment qty and sndReturnQuantity/TotalQuantity = DQty (i.e. the *retained* quantity, not the returned quantity). | PAGE-298 |
| BR-M08-027 | Line amounts recomputed on DQty: TP = unit price × DQty; VAT = unit VAT × DQty; Net = TP − discount + VAT; lines with zero TP get VAT/discount/net = 0; campaign/trade-term discount % recomputed from totals (old-trade-policy vs new... | PAGE-298 |
| BR-M08-028 | This page posts no accounting entry; whether posting occurs downstream from tblInvoice.sndReturn* is UNKNOWN. | PAGE-298 |
| BR-M08-029 | Print number shown is "RTN-"+InvoiceNo while the stored return number is "SRN-"+InvoiceNo. | PAGE-298 |
| BR-M08-030 | Product duplicates within the grid are rejected by ProductCodeValidation (returns false → "No: Product Already Inserted!!!"), only for manual product edits. | PAGE-298 |
| BR-M08-031 | Only delivery invoices of non-direct orders (tblOrder.IsDirect = 0/NULL) are searchable, and an invoice that already has a tblCustomerReturnMaster row can never be returned again (whole-invoice, one-time). | PAGE-287 |
| BR-M08-032 | Available quantity = DeliveryTotalQuantity − Σ prior ReturnQty per invoice detail; return qty is an integer 0…available; total must be >0. | PAGE-287 |
| BR-M08-033 | Line return amount = Return Qty × (UnitPrice + UnitVatAmount) where UnitPrice DataKey is the alias ROUND(DeliveryNetAmount/Quantity,4) (first of two UnitPrice columns) — VAT is added again on top of a delivery net amount. | PAGE-287 |
| BR-M08-034 | Deduction ≤ Total Return Amount (blocked at save, reset on edit); Net = max(Total − Deduction, 0). | PAGE-287 |
| BR-M08-035 | Return is saved with ActionStatus='Posted' — no approval step in this flow. | PAGE-287 |
| BR-M08-036 | Stock effect: returned qty is added as a new 'Receive' ledger row (StockCondition='SalesReturn', TotalQuantity=0, StockQty=qty, cost = original cost) referencing the original batch row. | PAGE-287 |
| BR-M08-037 | Voucher 1 (Journal, 'Approved', by 'admin'/'Auto Process', date=today, TableID 2018, CommonPK_ID=CustReturnId): Dr Sales Return (23/1020, fb 4170) = TotalAmount; Cr Delivery Charge Income (23/1032, fb 5968) = deduction if >0; Cr A... | PAGE-287 |
| BR-M08-038 | Voucher 2 (TableID 2023) posts only if StockValue>0: Dr Finished Goods (23/1016, fb 4156) / Cr COGS (23/1021, fb 4162) where StockValue = Σ (tblCentralStore.UnitPrice + VATPerUnit) × ReturnQty, traced invoice detail → tblDCStore.T... | PAGE-287 |
| BR-M08-039 | IsPosting=1 on tblCustomerReturnMaster after posting; sp_DeliverySalesReturnPosting has no idempotency check — re-executing creates duplicate vouchers. | PAGE-287 |
| BR-M08-040 | Only ticked lines are returned; a line's cumulative returned qty (PRQty = Σ earlier tblReturnInvoiceDetail.TotalQuantity for that original invoice detail + this RQty) may not exceed the line's total delivered qty (TQty). | PAGE-290 |
| BR-M08-041 | Return-line VAT is recomputed as TP × product VAT % / 100 and discount % is re-derived from the *order's* trade terms (GetTradeTerm), instead of using the invoiced unit VAT/discount. | PAGE-290 |
| BR-M08-042 | Amount defect: the saved detail TotalPrice/TotalPriceVatAmount are computed from qtyTextBox (delivered quantity) × unit price/VAT, while Quantity/TotalQuantity are the returned qty (dQty) — saved amounts do not match saved quantit... | PAGE-290 |
| BR-M08-043 | Returned stock is not re-admitted to tblDCStore; a tblDCStoreFreeze row (StockCondition='ReturnStock', StockQty=dQty) records it. Nothing is posted to accounts from this page. | PAGE-290 |
| BR-M08-044 | Return Invoice No = 'INV-' + <ComUnitCode> + '-' + zero-padded (COUNT of that unit's tblReturnInvoice rows + 1) (8 digits) — same INV- prefix as sales invoices; count-based so deletions create duplicates. | PAGE-290 |
| BR-M08-045 | ProformaorInvoiceReturn (PAGE-288) refuses to delete a delivery invoice when its lines already have freeze stock consumed (tblDCStoreFreeze.StockQty <> TotalQuantity) — the freeze rows created here are the trigger. | PAGE-290 |
| BR-M08-046 | The worklist lists invoices of the chosen DC + route + territory that are payment-confirmed (PaymentInvoiceNo not null) and not yet second-returned (SndReturnInvoiceNo null), sorted by invoice no; optional partial invoice-no searc... | PAGE-296 |
| BR-M08-047 | Only status "Partial Dues" exists in the row dropdown (disabled); it exposes *Go to Partial*; an invoice flagged IsAdjustInvoice=True forces index 0 and disables the dropdown. | PAGE-296 |
| BR-M08-048 | "ALL Submit" only acts on ticked rows whose status equals "Full", which cannot occur — the bulk full-confirmation path is dead. | PAGE-296 |
| BR-M08-049 | The DC list is restricted to the user's DCs (tblUserCompanyUnit); first DC is auto-selected; routes come from the "second-times sales return" route SP for that DC. | PAGE-296 |
| BR-M08-050 | Only invoices without a delivery invoice number can be deleted through sp_Delete_ProformaInvoice; other invoices are silently ignored by the SP (page still says "Delete successfully"). | PAGE-288 |
| BR-M08-051 | Stock reversal is by deleting the invoice's negative 'Issue' ledger row (matched by ReferenceDCStoreId, Trans_Type='Issue', StockQty = −TotalQuantity, one row per detail) — the Receive row's StockQty is *not* increased; over-infla... | PAGE-288 |
| BR-M08-052 | Audit trail: each detail is copied into tblInvoiceDetail_DeleterRecord, a tblProInvoiceReturnTrack row is written, and a tblInvoiceDeleteLog row records who/when. | PAGE-288 |
| BR-M08-053 | Deleting re-opens the source order (tblOrder.IsInvoice=0, tblOrderDetail.Status=NULL) so it can be re-invoiced; payments/returns tied to the invoice in tblReturnAmount are removed. | PAGE-288 |
| BR-M08-054 | "Delivery" deletion is refused if any of the invoice's freeze-stock rows has been partially consumed (StockQty<>TotalQuantity). | PAGE-288 |

### M09 — Collections, Payments & Money Receipts (0 rules)

| ID | One-line rule | Page(s) |
|---|---|---|
| BR-M09-001 | Collectable invoice = has `PaymentInvoiceNo`, no `SndReturnInvoiceNo`, Due>0 (payment-stage amounts) | 311 |
| BR-M09-002 | Payable ("Confirm Amount") = Σ PaymentNetAmount + DeliveryCharge; returns not deducted | 311 |
| BR-M09-003 | Ticking a row defaults pay=Due, discount=0, AIT=0; ≥1 row required | 311 |
| BR-M09-004 | pay+discount+AIT+previous ≤ Confirm Amount (UI-event check only) | 311, 315 |
| BR-M09-005 | Bulk disburse allocates oldest-first, leftover discarded; no advance | 311 |
| BR-M09-006 | TP/VAT split: VAT first, then TP | 311, 314, 315, 318 |
| BR-M09-007 | Status Full iff pay+AIT+discount+previous == Confirm Amount, else Partial | 311 |
| BR-M09-008 | Each row needs one cash/bank ledger account; default by invoice payment type | 311 |
| BR-M09-009 | Collection-By default from role/designation (X. MIO / MIO / DIC) | 311 |
| BR-M09-010 | Duplicate guard (SP + same invoice/date/amount insert guard) | 311, 314 |
| BR-M09-011 | One tblCustomerPay header per ticked row; RefNo=cheque no | 311 |
| BR-M09-012 | Auto-posted approved voucher structure (Dr cash/bank, discount, AIT; Cr AR) | 311 |
| BR-M09-013 | Voucher type/code rules (CVB-/CVC-, yyMMdd+counter; NULL type if no account) | 311 |
| BR-M09-014 | GL resolution via chart of accounts and Table_Integration_AccountsConnectivity (PageId 163) | 311 |
| BR-M09-015 | Voucher posted only if payment date is inside a financial year; else silent skip | 311 |
| BR-M09-016 | COGS posting when delivery-stage due ≤ 0 | 311 |
| BR-M09-017 | Posting failure is swallowed; payment stays | 311 |
| BR-M09-018 | Receipt no = RefNo else `MR-`+8-digit CustPayId (report SP) | 311, 330 |
| BR-M09-019 | Payment date user-editable, drives FY/voucher date | 311 |
| BR-M09-020 | Depot and customer mandatory | 311 |
| BR-M09-021 | Payment-stage confirmation allowed once per invoice | 334 |
| BR-M09-022 | Calculate before save (flag never reset) | 334 |
| BR-M09-023 | Line status Full/Partial/Reject by DQty vs TQty; header roll-up | 334 |
| BR-M09-024 | Return reason required for Reject (typo lets Partial skip) | 334 |
| BR-M09-025 | Accepted qty ≤ confirmed qty (`<=` bug in handler) | 334 |
| BR-M09-026 | Not-accepted qty returns to DC stock | 334 |
| BR-M09-027 | Line amounts: TP, VAT, discount, NP formulas | 334 |
| BR-M09-028 | Stamps PaymentInvoiceNo "RTN-…" making invoice collectable | 334 |
| BR-M09-029 | Zero-TP lines zeroed in totals | 334 |
| BR-M09-030 | Discount % source order (fixed customer, old policy, campaigns, trade term) | 334 |
| BR-M09-031 | Excel layout InvoiceNo/Amount/CollectionBy(MIO|DIC) | 314 |
| BR-M09-032 | Bulk save validations incl. server over-payment check | 314 |
| BR-M09-033 | Only PaymentStatus NULL/Partial invoices match upload | 314 |
| BR-M09-034 | Excel payments: Cash, no accounts, no discount/AIT, **no voucher** | 314 |
| BR-M09-035 | Legacy route-based variant rules (no posting) | 315 |
| BR-M09-036 | Legacy CollectionBy from DA text | 315 |
| BR-M09-037 | Second-return invoices excluded from 311; collected only on 318 | 318 |
| BR-M09-038 | Snd save rules = legacy variant | 318 |
| BR-M09-039 | Receivable report type ↔ SP @Type; DZSM/AM UI steering | 336 |
| BR-M09-040 | Footer sums rounded to whole taka; receivable formula in SP | 336 |
| BR-M09-041 | Delete payment line: remark required, UI-only "latest date" limit | 313 |
| BR-M09-042 | Delete does not reverse voucher/status in C# | 313 |
| BR-M09-043 | Reverse: reset invoice status, delete first payment header+details | 335 |
| BR-M09-044 | Reverse is hard delete, no voucher reversal/log/permission | 335 |
| BR-M09-045 | Outstanding = Σ DeliveryNetAmount+DeliveryCharge − Σ(pay+disc+AIT ≤ as-on) >0 | 306 |
| BR-M09-046 | "Credit Days" = DATEDIFF(day, InvoiceDate, AsOn); no buckets | 306 |
| BR-M09-047 | Payments counted by payment date ≤ as-on | 306 |
| BR-M09-048 | Aging uses delivery-stage amounts (differs from collection list) | 306 |
| BR-M09-049 | `@OnlyDueInvoices` unused; sales-person via customer master; no depot security | 306 |
| BR-M09-050 | Deposit slip delete: only users 51419000000 / 50588, deposit dated within today−60 days; soft delete (`IsDelete`) enforced in UI and in the UPDATE's WHERE | 322 (`DepositList.aspx.cs:155-159`, `CompanywisebranchDal.cs:115-118`) — CONFIRMED |
| BR-M09-051 | Deposit slip upload: typed Total Amount must equal Σ Excel amounts; every row needs bank + mapped account for the user's depot (`sp_GET_BAnkInfoById_ByIdExcel`, `sp_Get_BankSAPMappingAccounTNo`) or the row is red and the save blocked; company = `Session["ComUnitId"]` | 323 (`DepositSlipExcelUpload.aspx.cs:386-524`) — CONFIRMED |
| BR-M09-052 | Deposit rows are inserted one by one via `sp_I_Diposit_New` (DepositCode from `sp_Get_DepositCodeByDIC`, MIO from SAP territory code); success message tests only the id returned for the **last** row | 323, 314 (`:238-334`) — CONFIRMED |
| BR-M09-053 | `Aging.aspx` bucket headers: "1 to 10", "11 to 20", "21 to 30", "31 to 40", "41 to 50", "51+" days, each with TP Amount + Net Amount, plus Total TP / Total Net; bucket assignment logic is in `sp_Get_AgingReceivableReport` (not in repo) — bucket boundaries as *labels* only | 305 (`Aging.aspx.cs:54-94`, `Aging.aspx:248-280`) — CONFIRMED (labels) / UNKNOWN (calc) |
| BR-M09-054 | MoneyReceiptPreview receipt no = voucher code of the payment (accounts TableID 2017) else RefNo else `RP`+7-digit CustPayId (differs from report SP numbering BR-M09-018) | 331 (`MoneyReceiptPreview.aspx.cs:66-105`) — CONFIRMED |
| BR-M09-055 | MarketWiseCustomerPayment (legacy): Σ row payments must equal the typed Payment Amount; status Full if pay+previous+adjust == invoice amount else Partial via `UpdateInvoicePaymentAmount` (writes tblInvoice.PaymentAmount = pay+previous); single-detail `SaveCustPayment` returns true even when its `Existence` check skipped the insert; no discount/AIT/posting | 328 (`:216-327`, `CustPaymentBLL.cs:55-90`) — CONFIRMED |
| BR-M09-056 | Posting audit thresholds: expected voucher amount for a payment = Σ(PaymentAmount+Discount+AIT) per CustPayId; statuses MISSING_VOUCHER, MISSING_VOUCHER_DETAILS, DR_CR_UNBALANCED (strict), MASTER_VALUE_MISMATCH / DETAIL_VALUE_MISMATCH (> 1.00 BDT) | audit script (`SP_Audit_Sales_Accounts_Voucher_Posting.sql:148-219`) — CONFIRMED |
| BR-M09-057 | CustomerPaymentReport lists all payment headers by header PaymentDate for **all depots**; voucher no = accounts voucher code else RefNo else `CT`+8-digit id; "Bank/Cash" is always the literal "General Cash"; Amount = header PaymentAmount (excludes discount/AIT) | 316 (`CustomerPaymentReport.aspx.cs:174-241`) — CONFIRMED |
| BR-M09-058 | Full payment rollback (page `SalesOperationRollback`, outside M09 list): single transaction — insert reversal vouchers (Dr/Cr swapped, dated now) for TableID 2017 vouchers of the payment, reset invoice `PaymentStatus/PaymentAmount` to NULL, log via `sp_InsertCustPayDetail_DeleteLog`, delete detail+header; located by `tblCustomerPay.CollectionNo` (column origin UNKNOWN) | `SalesOperationRollbackDAL.cs:524-688` — CONFIRMED (code) / UNKNOWN (column) |

### M10 — Warehouse, DC Stock & Inventory (0 rules)

Tier-A page rules (details in each page spec section 19):

| BR | One-line rule | Page |
|---|---|---|
| BR-M10-001 | Stock-in header needs manufacturer, supplier, date, challan no/date | 385 |
| BR-M10-002 | Each line needs code, batch, exp, mfg, qty, VAT (price optional) | 385 |
| BR-M10-003 | New stock-in Status=Posted; id MAX+1; code WHS-nnnnnnn | 385 |
| BR-M10-004 | Line = qty*price + qty*VAT; header totals summed | 385 |
| BR-M10-005 | Product pick defaults price=CostPrice, VAT=VATAmountPerUnit | 385 |
| BR-M10-006 | Exp = Mfg + 2 years; exp >= mfg | 385/370 |
| BR-M10-007 | Edit deletes/reinserts lines, resets Posted, blanks ManufacId | 385 |
| BR-M10-008 | Save hidden when sp_GET_btnShow returns rows | 385/389/392 |
| BR-M10-009 | Stock-in save does not touch stock tables | 385 |
| BR-M10-010 | Edit page requires price too | 387 |
| BR-M10-011 | Edit page blanks Status/ManufacId (drops out of queues) | 387 |
| BR-M10-012 | Approval queue = Status Posted | 386 |
| BR-M10-013 | Approve/Reject needs action + row; stamps ApproveBy/Date | 386 |
| BR-M10-014 | MC receipts call sp_Acc_MaterialConversionReceive; regular ones status only | 386 |
| BR-M10-015 | No approver entitlement / maker!=checker check | 386 |
| BR-M10-016 | MC list = unreceived production output lines (anti-join, non-Reject) | 370 |
| BR-M10-017 | MC list filtered by company/production unit; WH by company | 370 |
| BR-M10-018 | MC save needs warehouse, dates, challan, selected lines | 370 |
| BR-M10-019 | MC save Posted with MaterialConversion ids; cost = CostPerUnit, VAT 0 | 370 |
| BR-M10-020 | MC edit replaces lines; Reject frees lines | 370 |
| BR-M10-021 | MC totals over selected rows | 370 |
| BR-M10-022 | Stock-out driven from Approved stock-in via Session WHId | 389 |
| BR-M10-023 | Stock-out validations (reason, lines, non-zero qty) | 389 |
| BR-M10-024 | Out qty <= C.Stock only on postback | 389 |
| BR-M10-025 | Stock-out inserts master/detail, tblCentralStore.Quantity -= qty by MigoDetailID | 389 |
| BR-M10-026 | Stock-out bypasses weighted-average/batch ledgers | 389 |
| BR-M10-027 | Freeze only Available stock (qty>0 for DC) | 381 |
| BR-M10-028 | Conditions limited by tblStockConditionPermission per user | 381 |
| BR-M10-029 | Central-WH mode only if Session CentralWareHouse=True; DC list per user | 381 |
| BR-M10-030 | Freeze needs qty !=0, condition, qty <= stock (integer compare) | 381 |
| BR-M10-031 | DC freeze: header + tblDCStoreFreeze + tblDCStore.StockQty -= qty | 381 |
| BR-M10-032 | WH freeze via this page writes tblDCStoreFreeze rows nobody can release | 381 |
| BR-M10-033 | Bulk freeze reports success regardless of skipped rows | 381 |
| BR-M10-034 | Release list = tblDCStoreFreeze qty>0 by DC/manufacturer | 375 |
| BR-M10-035 | Release qty non-zero and <= freeze qty | 375 |
| BR-M10-036 | Release: freeze -= qty then tblDCStore.StockQty += qty (2 commits) | 375 |
| BR-M10-037 | Release rows staged, no duplicates | 375 |
| BR-M10-038 | Sample conversion needs SC, action, date, product, rows | 379 |
| BR-M10-039 | Source rows tblDCStore StockQty>0 (any condition) | 379 |
| BR-M10-040 | Qty <= stock only on postback | 379 |
| BR-M10-041 | Save decrements tblDCStore for both actions; no counterpart | 379 |
| BR-M10-042 | Delete reverses (adds back) | 379 |
| BR-M10-043 | Depot voucher needs DC, invoice/N-A, date, reason, product, rows | 367 |
| BR-M10-044 | Voucher stock rows by DC code + product + active price | 367 |
| BR-M10-045 | Voucher qty <= stock only on postback | 367 |
| BR-M10-046 | Voucher save Posted, no stock change; approved not deletable | 367 |
| BR-M10-047 | Approval: Posted queue; Approve via SP, Reject via UPDATE | 363 |
| BR-M10-048 | No approver entitlement check | 363 |
| BR-M10-049 | WH adjustment needs date, store, remarks, ticked qty | 392 |
| BR-M10-050 | Adjust rows Quantity>0; qty <= stock on postback | 392 |
| BR-M10-051 | Transaction no WHA-yymm + running sequence from 10001 | 392 |
| BR-M10-052 | Adjustment auto-Approved and always decreases stock | 392 |
| BR-M10-053 | Delete adjustment restores stock | 392/393 |

Rules verified in Tier-B pages and SP scripts (recorded here, sources given):

| BR | Rule | Source |
|---|---|---|
| BR-M10-054 | Saving condition permissions upserts only checked conditions (Permission=true); unchecking never revokes | `StockConditionPermission.aspx.cs:76-96`; `StockConditionPermissionBll.cs:31-56` |
| BR-M10-055 | WH condition freeze (PAGE-394): rows tblCentralStore Available and Quantity>0; needs selection, condition, qty; inserts tblWhStockConditionFreeze + tblWhStoreFreeze, tblCentralStore.Quantity -= qty; no server stock-limit check (only integer postback compare) | `WhStockConditionFreeze.aspx.cs`; `WhStockConditionFreezeDal.cs:22-66` |
| BR-M10-056 | WH freeze release (PAGE-390): list tblWhStoreFreeze StockQty>0; qty integer, >0, <= displayed stock (postback); submit: freeze StockQty -= qty, tblCentralStore.Quantity += qty by ReceiveId | `WhFreezeStockRelease.aspx.cs`; `WhFreezeStockReleaseDal.cs:17-65` |
| BR-M10-057 | Stock batch update (PAGE-380): rows tblDCStore StockQty>0 per DC; ticked rows need batch, mfg, exp; SP `sp_UD_StockBatch_new(@dcStoreId,@batch,@mfgdate,@expDate,@updateBy,@StockQty)`; unparsable StockQty silently becomes 0 | `StockBatchUpdate.aspx.cs`; `StockBatchUpdateDal.cs:17-36` |
| BR-M10-058 | Country/opening stock entry (PAGE-360) is legacy: inserts tblCurrentStock/tblDCStore with old columns (ReceiveId, Quantity); validation messages are copy-paste ("Please Input Company Name!!"); BLL returns false on the normal path | `CountryStockEntry.aspx.cs`; `DCStoreBLL.cs:17-68`; `DCStoreDAL.cs:16-30` |
| BR-M10-059 | Sample stock for warehouse (PAGE-376) changes tblCentralStore.StockInQty (not Quantity); rows StockInQty>0; delete re-adds | `SampleStockForWHDal.cs:33-40,86-101` |
| BR-M10-060 | DC transaction-date rule: rows with StockQty<0 or Trans_Type='Issue' dated ISNULL(ReferenceTransactionDate, StockRcvDate); others StockRcvDate | `SP_Update_StockValuation_TransactionDate_Logic.sql:90,147-148,158,230,296,319` |
| BR-M10-061 | ToDate = end of day (DATEADD(SECOND,86399,date)); FromDate used as supplied | `...sql:30,205,364` |
| BR-M10-062 | DC ledger opening (dated < FromDate): qty=SUM(StockQty), value=SUM(TotalCostAmount), rate=SUM(cost)/SUM(qty) if qty>0 else AVG(CostPerUnit); fallback tblUnitPrice.CostPrice (IsActive=1) | `...sql:44-46,83-92` |
| BR-M10-063 | Movement classes: Receive From WH (Receive/NULL/Available, no return id, condition not B2BReceive/TransferReceive/SalesReturn/Return, chalan not IDT%/RIN%/RINV%, qty>0); B2B Receive (condition B2BReceive/TransferReceive or chalan IDT%, qty>0); Return Rcv (ReturnFromInvoiceDetailId<>0 or condition SalesReturn/Return or chalan RIN%/RINV%); Sales (negative rows not B2BIssue/TransferIssue, ABS; or Issue/Sales rows); B2B Issue (condition B2BIssue/TransferIssue, ABS) | `...sql:95-150` |
| BR-M10-064 | Closing Qty = Opening + RcvFromWH + B2BRcv + ReturnRcv - Sales - B2BIssue; Closing Rate = latest CostPerUnit>0 (order StockRcvDate DESC, DCStoreId DESC, dated <= ToDate) else CostPrice; Closing Value = Closing Qty x Closing Rate | `...sql:56-66,152-159` |
| BR-M10-065 | Rows with zero opening and zero movements are excluded; DC/category/product filters optional | `...sql:163-176` |
| BR-M10-066 | National valuation, central store: latest tbl_INV_CentralStore_WeightedAvg row per Item_Id (max Weighted_Avg_Id) with Trans_Date <= ToDate gives Close_Qty, Avg_Rate, Close_Amt; opening from latest row with Trans_Date < FromDate | `...sql:283-307` |
| BR-M10-067 | National valuation, DC part: SUM(StockQty) and SUM(TotalCostAmount) per product as of ToDate; one dynamic pivot column per tblCompanyUnit name; National Total = central value + depot value; only products with positive opening/closing qty | `...sql:207-337` |
| BR-M10-068 | Batch drill-down by location: Central, Opening National, Total Depots, National, or one depot; SUM per batch, only positive | `...sql:366-481` |
| BR-M10-069 | Central STO issue valued at pre-issue average rate (Close_Amt/Close_Qty); new average = closing amt/qty; idempotent per (Mst_Trans_Id, Ref_No, Company, Item); batch row negative qty; no insufficient-stock test | `SP_Fix_CentralStoreStoIssue_Corrected.sql:99-199` |
| BR-M10-070 | Anomaly definitions: orphan Issue row = Issue with no tblInvoiceDetail (DCStoreId=ReferenceDCStoreId, TotalQuantity=ABS(StockQty)); corrupted Receive = StockQty > TotalQuantity | `sp_Check_DCStore_StockAnomalies.sql:20-91` |
| BR-M10-071 | sp_Adjust_DCStore_Stock_Only deletes matching Issue rows for an invoice and resets Receive StockQty to TotalQuantity (general step for every Receive row with StockQty <> TotalQuantity) in one transaction | `SP_Adjust_DCStore_Stock_Only.sql:30-113` |
| BR-M10-072 | Ledger semantics of tblDCStore: a sales issue inserts a new negative row (Trans_Type='Issue', ReferenceDCStoreId=source, ReferenceTransactionDate=now, cost carried) instead of updating the Receive row; DC receipts insert Receive rows with CostPerUnit/TotalCostAmount; B2B receipts StockCondition='B2BReceive' | `InvoiceDAL.cs:667-702`; `RequisitionDAL.cs:515-548`; `ChalanDAL.cs:126-165`; `sp_Delete_ProformaInvoice_Update.sql:108-140` |
| BR-M10-073 | M10 pages (freeze, sample conversion, stock batch update, other stock action) and B2B challan SP still change Receive-row StockQty in place, mixing two models | `StockConditionFreezeDAL.cs:90-98`; `SampleTypeConventionDal.cs:128-136`; `OtherStockActionDAL.cs:125-132`; `SP_NewStockTransferDcToDc_Related.sql:325-341` |
| BR-M10-074 | Inactivated unit price rows move to tblUnitPriceRollback (RollbackBy, RollbackDate) inside one transaction | `SP_Create_tblUnitPriceRollback.sql`; `ProductUnitPriceDAL.cs:220-245` |
| BR-M10-075 | WH monitoring: closing = opening(snapshot at exact fromDate) + StockIn(ReceiveDate range) - Issue(requisition picking); amount not computed | `WhStockMonitoringReportDal.cs:17` |
| BR-M10-076 | DC opening report reads tblDCStore_OpeningBalance at an exact DCOpeningBalanceDate | `DcOpeningStockReportDal.cs:24-45` |
| BR-M10-077 | Central lot availability = inbound batch rows (Trans_Qty>0) less outbound rows by Issue_Weighted_Avg_Id+batch; picking order ExpDate, ReceiveDate (FEFO) | `RequisitionDAL.cs:300-350` |

### M11 — Stock Transfer & Chalan Receive (0 rules)

| BR | Rule (abridged) | Page |
|---|---|---|
| BR-M11-001 | Sender and receiver DCs are identified by typed unit code resolved in tblCompanyUnit; unknown code is rejected and cleared; no rule stops from = to and no user/DC scoping applies. | PAGE-412 |
| BR-M11-002 | A challan requires number, date, manufacturer, from unit, to unit and ≥1 product line. | PAGE-412 |
| BR-M11-003 | Available stock per batch/expiry = SUM(StockQty) of tblDCStore rows (Trans_Type Receive+Issue) of the from-unit and product, shown only if > 0; unit price/VAT come from the active tblUnitPrice. | PAGE-412 |
| BR-M11-004 | Transfer quantity must not exceed displayed stock — enforced only when the qty box posts back; no server-side re-check at Save. | PAGE-412 |
| BR-M11-005 | The same source DCStoreId cannot be added twice (list de-duplication by DCStoreId). | PAGE-412 |
| BR-M11-006 | Challan number = IDT + id (MAX(ChalanId)+1), left-padded with zeros to 6 digits only when the id has 1-3 digits; ids ≥ 1000 are unpadded. | PAGE-412 |
| BR-M11-007 | Challan value = Σ qty × active unit price; VAT = Σ qty × VAT/unit; grand = value + VAT (per detail Value, Vat, ValueWVat). | PAGE-412 |
| BR-M11-008 | Stock leaves the sender at save time: each detail with qty>0 inserts a negative Trans_Type='Issue', StockCondition='B2BIssue' ledger row referencing the source row (ReferenceDCStoreId) and the challan detail; the goods are "in tra... | PAGE-412 |
| BR-M11-009 | Receiving (PAGE-423) inserts a destination Receive row (B2BReceive) for the received quantity and flips IsDeliver to 'True'; the quantity difference stays as a restricted tblDCStoreFreeze row for damage. | PAGE-412 |
| BR-M11-010 | Deleting a B2B challan from B2BTransferView is allowed only when its status text is not "Received" (status = IsDeliver='True'); the page deletes tblChalanInfo first, then calls sp_Del_B2BDelete. | PAGE-412 |
| BR-M11-011 | sp_Del_B2BDelete exists in two conflicting definitions: v1 (2026-09-01) adds Quantity back onto the source Receive row and leaves Issue rows; v2 (2026-09-02, "Fix STO Chalan Process") deletes the Issue (and B2BReceive) ledger rows... | PAGE-412 |
| BR-M11-012 | SAP-origin challans: sp_SAP_B2BChallanMasterInsert builds a challan header from SAP stock movement (plants mapped by tblCompanyUnit.SAP_Code, products by tblProduct.SAP_Code), skipping challans already in tblWHStockInMaster; missi... | PAGE-412 |
| BR-M11-013 | sp_SAP_B2BChallanDetailInsert consumes source DC stock FIFO by expiry using the decrement model (StockQty -=), not Issue rows; detail quantity = full SAP line qty per allocation step; stale-variable defect creates phantom details ... | PAGE-412 |
| BR-M11-014 | SAP hand-off states on the challan: SAP_ChallanSend (challan payload sent: sp_SAP_ChallanSendDetailByChalanId supplies SAP product code, batch, qty, UoM) and SAP_Challan_ConfirmationSend (receipt confirmation sent) are set to 1 by... | PAGE-412 |
| BR-M11-015 | An STO needs manufacturer, warehouse, DC and at least one line with code, name and requested qty; requisition number is system-generated REQ-+7 digits from MAX(ReqId)+1. | PAGE-420 |
| BR-M11-016 | Lines are created batch-specific (IsFromBatch=true, BatchNO from the picked central-store batch) so pick/issue draws from that batch first. | PAGE-420 |
| BR-M11-017 | Requested qty ≤ displayed central stock is checked only on postback; not re-checked at save or at pick (pick spills to next batch by FIFO Quantity order when short). | PAGE-420 |
| BR-M11-018 | Saving an STO does not move stock; stock moves at picking/issue (central store Quantity set to remaining; weighted-average issue at the running average rate). | PAGE-420 |
| BR-M11-019 | Editing an STO replaces all its lines; batch numbers are not carried into the re-inserted lines (BatchNO NULL). | PAGE-420 |
| BR-M11-020 | Central-store stock shown for search is Quantity>0 per receive lot for the product regardless of warehouse selected. | PAGE-420 |
| BR-M11-021 | STO issue costing: tbl_INV_CentralStoreStoIssue values the issue at the latest weighted-average closing rate of the product/company (Close_Amt/Close_Qty of last row), producing Curr_Amt = −qty × rate; new average = closing amount/... | PAGE-420 |
| BR-M11-022 | Only one open (untransferred) DC→WH challan per DC at a time: a new one is refused while any challan of that from-unit has IsTransfer IS NULL. | PAGE-408 |
| BR-M11-023 | Challan number SDC+id (MAX(SChalanId)+1; zero-padded to 6 only for 1-3 digit ids). | PAGE-408 |
| BR-M11-024 | Lines may come from normal DC stock (tblDCStore, StockQty>0) or freeze stock (tblDCStoreFreeze, DCStoreFreezeId>0); each line carries a purpose (tblPurpose, default 1). | PAGE-408 |
| BR-M11-025 | Challan value = Σ qty × unit price (active tblUnitPrice), VAT = Σ qty × VAT/unit. | PAGE-408 |
| BR-M11-026 | Saving the challan does not reduce stock; stock is decremented only when a user presses *Transfer* on PAGE-409 (IsTransfer='OK'): StockQty := StockQty − Quantity on the Receive row (or freeze row) — an in-place decrement, not an I... | PAGE-408 |
| BR-M11-027 | Transfer (PAGE-409) does not verify quantity ≤ current stock (result may be negative) and marks the challan transferred based on the last row's update result only. | PAGE-408 |
| BR-M11-028 | Warehouse and manufacturer are pre-forced to id 1 on load (defaults), still changeable. | PAGE-408 |
| BR-M11-029 | Only STO lines not yet transferred (tblStockInTransfar.IsTransfared IS NULL) of the requisition are shown; when none remain the page reports "Stock Already Received!!". | PAGE-413 |
| BR-M11-030 | Received qty defaults to issued qty; unreceived (damaged) qty is entered, bounded by issued qty, and required on every row (0 allowed). | PAGE-413 |
| BR-M11-031 | Stock effect: one tblDCStore Receive row per line, StockQty = received qty (not issued), TotalQuantity = issued qty, guarded so a StockInTransfarId can create only one row. | PAGE-413 |
| BR-M11-032 | Unreceived/damaged qty is not added to available stock: it is written to tblDCStoreFreeze as 'Restricted' (freeze stock) at the new receive row. | PAGE-413 |
| BR-M11-033 | Valuation: CostPerUnit = STO unit price (excl. VAT); TotalCostAmount = STO total incl. VAT — the DC ledger cost is the transfer price, not the warehouse weighted-average cost, and is internally inconsistent. | PAGE-413 |
| BR-M11-034 | Completion flags: tblStockInTransfar.IsTransfared='OK' per line and tblRequisition.ReceiveIssue='OK' (removes the STO from the DC's receive list). | PAGE-413 |
| BR-M11-035 | SAP hand-off: after saving, the challan/STO number is passed to MakeRESTRequest with the user id; any failure is ignored, so the ERP receipt stands even if SAP is not notified; no state flag is set by this page. | PAGE-413 |
| BR-M11-036 | Only challans with IsDeliver = false are receivable; IsDeliver becomes 'OK' once at least one line was stocked in or frozen; the challan is then closed for all lines. | PAGE-414 |
| BR-M11-037 | Stock effect (branch A): a line sourced from normal DC stock (DCStoreId>0, no freeze id) is added to central stock as a new tblCentralStore row Available/Regular with Quantity = received qty; lines sourced from freeze stock are ad... | PAGE-414 |
| BR-M11-038 | Unreceived/damaged quantity always goes to WH freeze stock (tblWhStockConditionFreeze + tblWhStoreFreeze, Restricted). | PAGE-414 |
| BR-M11-039 | Valuation: price and VAT per unit are those of the DC row's original STO line; totals are computed on the issued quantity, so TotalPrice/TotalVAT/TotalAmount overstate value when qty is short. | PAGE-414 |
| BR-M11-040 | tblCentralStore.MigoDetailID is populated with the challan detail id (SChalanDetailsId), and tblWhStockConditionFreeze.ReceiveId with the same id, not with a MIGO or central ReceiveId (the freeze store's ReceiveId is separately re... | PAGE-414 |

### M12 — Sub-Depot Operations (0 rules)

| BR ID | Rule (summary) | Confidence |
|---|---|---|
| BR-M12-001 | A sub-depot is registered under exactly one parent DC (tblSubDepot.ComUnitId) | CONFIRMED |
| BR-M12-002 | Sub-depot registration requires name, address, code, mobile no and parent DC (messages `Please Input Sales Center Name!!`, `Please Input Address!!`, `Please Input Code!!` | CONFIRMED |
| BR-M12-003 | Sub-depot code is proposed as a hard-coded 3-letter prefix by parent-DC id (1 BAR, 2 BOG, 3 CHA, 4 CUM, 6 KUS, 7 KHU, 8 MYM, 9 RAJ, 10 RAN, 11 SYL, 12 DHA) + zero-padded  | CONFIRMED |
| BR-M12-004 | Sub-depot documents use the text prefix `SDC` + 6-digit zero-padded (MAX(id)+1): DC->Sub-depot chalan (tblSubDepotChalanInfo.SChalanId), Sub-depot->DC return chalan (tblS | CONFIRMED |
| BR-M12-005 | DC->Sub-depot product search lists tblDCStore rows of the selected DC (matched by ComUnitCode) for the selected product with an ACTIVE tblUnitPrice row | CONFIRMED |
| BR-M12-006 | Add-to-list keeps only rows whose checkbox is ticked AND whose Transfer Qty is non-empty; a DCStoreId already in the chalan list is silently not added again (HasDCStoreId | CONFIRMED |
| BR-M12-007 | Transfer quantity accepts digits only (FilteredTextBoxExtender ValidChars 0-9) | CONFIRMED |
| BR-M12-008 | Chalan submit validation (DC->Sub and Sub->DC): chalan no present, chalan date present, manufacturer selected, from-unit code present, to-unit code present, at least one  | CONFIRMED |
| BR-M12-009 | Chalan valuation: per line Value = UnitPrice x Qty; Vat = Qty x VATAmountPerUnit; ValueWVat = Value + Vat; header TotalValue/TotalVat/GrandTotal are the sums | CONFIRMED |
| BR-M12-010 | DC->Sub-depot send: header saved with IsDeliver=false (INSERT tblSubDepotChalanInfo), each line saved to tblSubDepotChalanDetail, and DC stock is reduced IMMEDIATELY by r | CONFIRMED |
| BR-M12-011 | Sub-depot->DC return send mirrors BR-10 but writes tblSubDepotChalanReturnInfo/tblSubDepotChalanRetuenDetail and reduces the SUB-DEPOT store row by read-modify-write `UPD | CONFIRMED |
| BR-M12-012 | Sub-depot->DC product search returns sub-depot stock (`StockQty>0`, active unit price) for the selected product from ALL sub-depots: the sub-depot argument passed by the  | CONFIRMED |
| BR-M12-013 | Sub-depot receive list shows chalans of the selected sub-depot with IsDeliver='False' OR NULL (list) - the receive screen itself reloads only IsDeliver='False' (a NULL fl | CONFIRMED |
| BR-M12-014 | Sub-depot receive creates one NEW tblSubDepotStore row per chalan line: StockQty = received qty, TotalQuantity = quantity sent, DamageQty = damaged qty, MfgDate/ExpDate f | CONFIRMED |
| BR-M12-015 | Received qty and damaged/un-received qty textboxes drive each other on post-back (rcv = sent - damage; damage = sent - rcv) | CONFIRMED |
| BR-M12-016 | Damaged/un-received qty > 0 on receive creates a freeze row (INSERT tblSubDepotStoreFreeze; StockCondition 'Restricted'; StockQty = TotalQuantity = damage qty; SubDCStore | CONFIRMED |
| BR-M12-017 | A chalan is flagged delivered (IsDeliver='True') as a side-effect of saving the FIRST received line (ChalanUpdate is called inside DCStockIn2 for every line) | CONFIRMED |
| BR-M12-018 | DC receipt of a Sub-depot->DC return inserts a new tblDCStore row per line (plain INSERT with SChalanDetailsId, StockCondition 'Available', no Trans_Type/CostPerUnit/Tota | CONFIRMED |
| BR-M12-019 | The DC-side list of pending Sub-depot->DC returns is not filtered by DC or sub-depot: `SubdeportChalanLoadInReceive(comunitId)` ignores its argument and returns every ret | CONFIRMED |
| BR-M12-020 | DC-side damaged qty on a return receipt is stored in tblDCStoreFreeze as 'Restricted' (StockQty = damage), with ComUnitId taken from the hidden `hdComUnitId` (= ComUnitId | INFERRED |
| BR-M12-021 | Direct sub-depot stock adjustment (Stock Adjustment / StockOutSubDepot): product search reads tblSubDepotStore of the chosen sub-depot with StockQty>0 and active unit pri | CONFIRMED |
| BR-M12-022 | StockOutSubDepot Submit does not call its own Validation(): with an empty list a header row is still inserted; the header id is `MAX+1` so `id>0` is always true and `Stoc | CONFIRMED |
| BR-M12-023 | Sub-depot Stock Adjustment Voucher: required DC, proforma invoice, stock-out date, reason, product and at least one line (`Please Select Distribution center!!`, `Please S | CONFIRMED |
| BR-M12-024 | Voucher approval: only vouchers with Status='Posted' are listed; Approve sets Status='Approved', ApprovedBy=LoginName, ApprovedDate=today via SP sp_UD_SubDcStockOutApprov | INFERRED |
| BR-M12-025 | Voucher rejection is routed through the DC stock-out method: it updates `tblDeStockOutMaster` (the main-depot DC stock-out master) WHERE DcStockOutMasterId = <sub-depot v | CONFIRMED |
| BR-M12-026 | A voucher can be deleted only while Status != 'Approved' (master row deleted, then details). | CONFIRMED |
| BR-M12-027 | Access to the voucher approval list is granted only if the page's menu row (tblMainMenu.URL like the file name) has an assignment in tblAppSetup for the session UserId; o | CONFIRMED |
| BR-M12-028 | Freeze list (Freeze Stock Release): rows of tblSubDepotStoreFreeze with StockQty>0 for the chosen sub-depot and manufacturer, ordered by ExpDate, with Amount = tblUnitPri | CONFIRMED |
| BR-M12-029 | Release quantity rules: return qty must be non-empty and not 0 (`Insert Return Quantity !!`) and <= the row's StockQty (`Return Quantity must be Less then Stock Qty`) | CONFIRMED |
| BR-M12-030 | Release effect: tblSubDepotStoreFreeze.StockQty := StockQty - qty and tblSubDepotStore.StockQty := StockQty + qty on the ORIGINAL store row (SubDCStoreId), by read-modify | CONFIRMED |
| BR-M12-031 | Rows whose StockCondition is 'Restricted' can be released only when Session LoginName = '21900' (hard-coded user) | CONFIRMED |
| BR-M12-032 | Sub-depot payable invoices: tblSubInvoiceMaster with DeliveryTpGrandTotal>0, matching ComUnitId and MarketId, PaymentStatus NULL or 'Partial', DelivaryInvoiceNo NOT NULL  | CONFIRMED |
| BR-M12-033 | Payment save validation: at least one invoice ticked (`Please Select Invoice from List!!`); each ticked row has a pay amount (`Please fill out Pay Amount !!`); payment am | CONFIRMED |
| BR-M12-034 | Over-payment guard: on pay-amount change, if (entered + previous payment) > invoice amount the box is reset to 0 with `Cannot Be Greater then Invoice Quantity ` (sic) | CONFIRMED |
| BR-M12-035 | Payment status: if (pay + previous) equals the Del-invoice amount (grid cell 7) status='Full' else 'Partial'; tblSubInvoiceMaster.PaymentAmount is overwritten with (pay + | CONFIRMED |
| BR-M12-036 | Payment persistence: per ticked invoice one tblCustomerPay row (MarketId, ComUnitId, date, amount, PayType text, RefNo, RefDate, CreateBy=LoginName, CreateDate) and one t | CONFIRMED |
| BR-M12-037 | Sub-depot payment reverse (by order code): finds the sub invoice via tblOrder.OrderCode, sets tblSubInvoiceMaster.PaymentStatus/PaymentAmount to NULL and deletes ONLY the | CONFIRMED |
| BR-M12-038 | Sub-depot proforma candidates: tblOrder with IsSubDepo=1, IsInvoice=0, chosen DC, manufacturer and distribution route; the route list only offers routes flagged tblRouteI | CONFIRMED |
| BR-M12-039 | Batch id for a proforma run = literal `06-` + (max numeric suffix of tblSubInvoiceMasterBatch.BatchNo + 1); the `06` prefix and the route-list DC id (6) are hard-coded ra | CONFIRMED |
| BR-M12-040 | Proforma generation per ticked order calls SP sp_Process_SubDepoProformaInvoiceByOrderId(@OrderId,@UserId,@BatchNo1) (body not in repo) | CONFIRMED |
| BR-M12-041 | Delivery confirmation of a sub-depot proforma: status Full -> SP sp_SubdeportDeliveryConformationFull(@InvoiceNo,@UpdateBy,@UpdateDate); Reject -> sp_SubDeportDeliveryCon | CONFIRMED |
| BR-M12-042 | Deleting a sub-depot proforma or delivery invoice calls sp_Delete_ProformaInvoice_SubDeport / sp_DeleteDeliveryInvoiceSubdepo | INFERRED |
| BR-M12-043 | Sub-depot stock report is quantity-only and is built from five buckets per sub-depot/product/batch: Available (tblSubDepotStore.StockQty>0 & StockCondition='Available'),  | CONFIRMED |
| BR-M12-044 | Deleting a sub-depot sales-return from the list runs the shared return delete: ReturnInvoice, ReturnInvoiceDetail, ReturnAmount rows and every tblDCStoreFreeze row linked | INFERRED |
| BR-M12-045 | Sub-depot receive and stock-report pages branch on Session UserType: Admin sees all DCs, others only DCs assigned in tblUserCompanyUnit (DCLoad(ddl,userId)) | CONFIRMED |
| BR-M12-046 | On DC change the Stock Adjustment Voucher page overwrites the login session value `Session["ComUnitId"]` with the selected DC id, altering the identity used by later page | CONFIRMED |
| BR-M12-047 | Selecting a product on DC->Sub-depot and Stock Adjustment pages fires the DC dropdown handler (`onselectedindexchanged=salescenterDropDownList1_SelectedIndexChanged`), wh | CONFIRMED |
| BR-M12-048 | The Print button on the Sub-depot->DC return page opens the DC->Sub-depot chalan viewer (`DcToSubdepotChalanReportViewer`), which queries tblSubDepotChalanInfo/Detail by  | INFERRED |
| BR-M12-049 | On the Sub-depot->DC return page `ChalanDetail.DCStoreId` (persisted to tblSubDepotChalanRetuenDetail.DCStoreId) carries the SUB-DEPOT store id (DataKey `SubDCStoreId`) | INFERRED |
| BR-M12-050 | The Remove (delete row) handler of the Sub-depot->DC page rebuilds the chalan grid with a `DCStoreId` column while the grid is bound with DataKeyNames `SubDCStoreId,...`; | INFERRED |

### M13 — Sales Reports, Analytics & Dashboards (0 rules)

| BR id | Pages | Rule (short) | Confidence |
|---|---|---|---|
| BR-M13-001 | 528 | Rollback requires a non-empty Code/No, a non-empty reason (remarks) and a live session (Session["LoginName"] != null); otherwise an alert is shown and nothing is written | CONFIRMED |
| BR-M13-002 | 528 | Proforma rollback is refused when the invoice already carries a DelivaryInvoiceNo ("Delivery Invoice already created! Please delete the Delivery Invoice first.") | CONFIRMED |
| BR-M13-003 | 528 | Proforma rollback (all in one SqlTransaction): (1) insert tblInvoiceDeleteLog row (DelInvoiceNo='PROFORMA', DeleteBy=login); (2) if OrderId>0: tblOrder.IsInvoice=0 and tb | CONFIRMED |
| BR-M13-004 | 528 | Stock reversal for a proforma line with Quantity>0: Rule 1 delete TOP(1) tblDCStore row where ReferenceDCStoreId=line.DCStoreId AND Trans_Type='Issue' AND StockQty=-Quant | CONFIRMED |
| BR-M13-005 | 528 | Delivery-invoice rollback is refused when any tblCustPayDetail row exists for the invoice ("Payment has been settled for this Delivery Invoice | CONFIRMED |
| BR-M13-006 | 528 | Delivery rollback first validates stock: for every line with returnQty = Quantity - DeliveryQuantity > 0, SUM(tblDCStore.StockQty) for ProductCode+BatchNo+ExpDate must be | CONFIRMED |
| BR-M13-007 | 528 | Delivery rollback reverses accounting: every voucher in CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster with CommonPK_ID=InvoiceId and TableID=2014 is copied to a NEW vou | CONFIRMED |
| BR-M13-008 | 528 | After reversal the delivery invoice is "un-delivered", not deleted: tblInvoiceDeleteLog + tblInvoiceDetail_DeleterRecord rows are written, tblInvoice.DelivaryInvoiceNo/De | CONFIRMED |
| BR-M13-009 | 528 | Payment (money receipt) rollback: locate tblCustomerPay.CollectionNo=code; reverse vouchers with CommonPK_ID=CustPayId and TableID=2017 (same swap-Dr/Cr copy as delivery) | CONFIRMED |
| BR-M13-010 | 528 | Each rollback executes on its own connection inside one SqlTransaction; any exception rolls back everything and returns "Error: <message>" which the page shows verbatim i | CONFIRMED |
| BR-M13-011 | 514 | "STO Generate" (SG) rollback lists tblRequisition rows with CreatePicking IS NULL (grouped with SUM(ReqQty)) and the delete icon physically deletes the requisition header | CONFIRMED |
| BR-M13-012 | 514 | "Picking Generate" (PG) rollback lists requisitions with Submit NULL/empty and CreatePicking set | CONFIRMED |
| BR-M13-013 | 514 | "Challan Generate" (CG) rollback lists tblRequisition with Submit='OK' and ReceiveIssue NULL/empty (issued but not yet received by DC) and performs the picking rollback s | CONFIRMED |
| BR-M13-014 | 514 | The logistics rollback steps are separate auto-committed SQL calls (SInventorySql.Execute) with no transaction; the success alert depends only on the picking-reset and de | CONFIRMED |
| BR-M13-015 | 464 | Dashboard access: UserRoleID "2" is admitted without a permission lookup; any other role must return >=1 row from GetPermissionForUserRole(<page path>) (sp_GET_MainPermis | CONFIRMED |
| BR-M13-016 | 464 | KPI header scope by RoleTypeName (Session): DZSM -> hierarchy RegionIds (sp_HigharchyInfoByEmployeeId) joined by comma -> sp_Get_DashboardTopBarData_Zone(@ZoneId); NSM -> | CONFIRMED |
| BR-M13-017 | 464 | GetSalesChartData and GetOrderChartData build one column per selected month (MMM-yyyy) and a "Total" column that is the SUM of the month values DIVIDED by the number of s | CONFIRMED |
| BR-M13-018 | 464 | Several endpoints re-use their date parameters as selectors: for GetSalesChartData fromdt = "Invoice"\|"Payment" (SP choice) and todt = customer-type id; for GetOrderChart | CONFIRMED |
| BR-M13-019 | 464 | Drill-down filters are assembled in browser JS as a SQL fragment (" AND tblOrder.CustTypeId=. | INFERRED |
| BR-M13-020 | 464 | Month-loop start/end dates are built with new DateTime(year, month, DateTime.Now.Day); on days 29-31 this throws for shorter months, and the loop guard (Month<=Month && Y | CONFIRMED |
| BR-M13-021 | 531, 494, 493 | Invoice (proforma) KPIs: NumberofProformaInvoice = COUNT(DISTINCT InvoiceId), SumofNetProformaAmount = SUM(NetAmount) - SUM(TotalPriceVatAmount) (net of VAT), ProTpVat =  | CONFIRMED |
| BR-M13-022 | 531, 494, 493 | Sales (delivered) KPIs: only DeliveryInvoiceStatus IN ('Partial','Full') AND TpGrandTotal>0 AND UpdateDate BETWEEN dates; NumberofInvoiceSold = COUNT(DISTINCT InvoiceId), | CONFIRMED |
| BR-M13-023 | 531, 494, 493 | Return (delivery-rejection) KPIs: lines with DeliveryStatus IN ('Reject','Partial') and UpdateDate in range: SumofNetReturnAmount = (SUM(NetAmount)-SUM(TotalPriceVatAmoun | CONFIRMED |
| BR-M13-024 | 531, 536, 518, 533, 520 | Derived columns (exact formulas in inline SQL): NetInvoiceAmt = SumofNetProformaAmount+ProTpVat; NetReturnAmt = SumofNetReturnAmount+DelReTpVat; NetSalesAmt = SumofNetSal | CONFIRMED (inline SQL) / UNKNOWN (SP-fed pages) |
| BR-M13-025 | 531, 494, 493 | Undelivered invoices: DeliveryInvoiceStatus IS NULL and InvoiceDate in range -> NumberofUndelInvoice, SumofNetUnAmount (NetAmount-TotalPriceVatAmount), UnTpVat; orders: N | CONFIRMED |
| BR-M13-026 | 531, 494 | CustomerCoverPer = (number of tblCustMaster customers of the ComUnit having an invoice in the period * 100) / (all tblCustMaster customers of the ComUnit) using INTEGER d | CONFIRMED |
| BR-M13-027 | 531, 532, 533, 535, 517 | Date bases differ per measure and are therefore not comparable across columns: proforma = InvoiceDate, sales/returns/deliveries = UpdateDate, orders = SubmissionDate, col | CONFIRMED |
| BR-M13-028 | 532, 533, 535, 523, 520 | Report scope radio (rbReportTypeName 1 Distribution Center, 2 Zone, 3 Area, 4 Territory) is translated into the SP argument @Type: "SC", "Zone", then for Area/Territory " | CONFIRMED |
| BR-M13-029 | 532, 533, 535, 523 | Role narrowing on load (Session RoleTypeName) | CONFIRMED |
| BR-M13-030 | 532, 533, 535, 506, 517 | Page footers are computed server-side in C# by summing the returned DataTable per column (null -> 0) | CONFIRMED |
| BR-M13-031 | 532 | TotalSummaryNew.LoadInfo only runs when zone/sales-center/territory legacy dropdowns are all empty (otherwise message "Please Select Date Range!!"); with the current UI t | CONFIRMED |
| BR-M13-032 | 531, 469 | On TotalSummary the Excel button (excelButton1 -> excelButton1_Click) opens BusinessSummaryViewer.aspx?fromDate&toDate in a popup, which ignores every on-screen filter an | CONFIRMED |
| BR-M13-033 | 506 | DZSMTotalSummary derives the caller hierarchy from Session (RoleTypeName/EmpInfoId/RoleTypeId) via GetHigharcyInfoByEmployeeId: AM -> AreaId list, DZSM -> RegionId list,  | CONFIRMED |
| BR-M13-034 | 506 | The grid is fed by TotalSummaryDAL.DZSMwiseLoadSummaryparm(from,to,parm) -> sp_GET_DZSMwiseReportParam_ByProcess(@fromdate,@todate,@parm) where parm is the raw ZoneSelect | CONFIRMED |
| BR-M13-035 | 463 | SixTenProjectReportApps (mobile/web-view page, no master page) takes EmpInfoId from the QueryString, loads that employee's RoleTypeId/RoleType via Get_EmpGeneralInfoByEmp | CONFIRMED |
| BR-M13-036 | 463 | KPI tiles are sums over the SP result: lblKpiDCP = SUM(DCP), lblKpiDCR = SUM(DCR), lblKpiRX = SUM(RX), lblKpiSales = SUM(TSalesNetTP); non-numeric/NULL cells count as 0 ( | CONFIRMED |
| BR-M13-037 | 495 | COGS-based GP report is realised on the PAYMENT date: the date filter is CONVERT(date, CPY.PaymentDate) BETWEEN from AND to (if only From is given, To = today); company u | CONFIRMED |
| BR-M13-038 | 495 | GP arithmetic is not in C#: the SP returns Month, Date, ComUnitName, BUWise, Particulars, SKU, Qty, OrderSenderName, RealizationAmount, VATAmount, TaxAmount, COGSAmount,  | CONFIRMED (page) / UNKNOWN (SP) |
| BR-M13-039 | 495 | Search results are cached in ASP.NET Cache for 10 minutes under "COGSBasedGPReportData_<UserId>"; btnExport exports the cached table when present, so an export after chan | CONFIRMED |
| BR-M13-040 | 511 | GpSalesReport (Territory-wise GP sales) filters by CONVERT(date, I.UpdateDate) BETWEEN from AND to, ComUnit and market-structure levels via a concatenated @Parm to sp_Get | CONFIRMED |
| BR-M13-041 | 505 | The main DynamicSalesReport grid calls SP GetDynamicSalesReportList(@fromdate,@todate,@Type="",@Area,@Terr,@ZonId,@grpId) using ONLY the two dates and the group/zone/area | CONFIRMED |
| BR-M13-042 | 505 | Each numeric cell is a LinkButton that stores the row's hidden TerritoryId in _hfTerritoryId and opens an AJAX modal loaded with a per-metric list: invoices (GetProformaI | CONFIRMED (page) / UNKNOWN (most SPs) |
| BR-M13-043 | 505 | Defect: TotalNoOfHqDoctorParam hard-codes ProgramTypeId="4" but tests `if (ProgramTypeId == "")`, so the program-type predicate is never added; HQ, Ex-HQ and OS doctor dr | CONFIRMED |
| BR-M13-044 | 505 | Entering an invoice From-date before 01-Apr-2021 resets it to "01 April, 2021". | CONFIRMED |
| BR-M13-045 | 520 | MonitoringReport shows, per scope level (SC/Zone/Area/Territory), the invoice/return/sales/outstanding block (same columns as TotalSummary) plus period roll-forward colum | CONFIRMED (wiring) / UNKNOWN (formulas) |
| BR-M13-046 | 523 | ProductWiseSalesReport (MIS) calls sp_RPT_MIS_ProductWiseSalesReport(@fromdate,@todate,@Type,@Area,@Terr,@ZonId) and displays Sales (TP/Discount/VAT/Gross), Return, "Just | CONFIRMED (wiring) / UNKNOWN (formulas) |
| BR-M13-047 | 517 | MIOWiseBusinessSummary is a copy of TotalSummaryNew2 whose only functional change is the data call LoadMIOWiseBusinessSummaryDAL(salesCenter, from, to) -> sp_RPT_MIOWiseB | CONFIRMED |
| BR-M13-048 | 535 | TotalSummaryNew_T is TotalSummaryNew2 with LoadSummaryProductcodewiseGyash__New -> sp_BusinessSummaryMISReport_TT(@fromdate,@todate,@Type,@Area,@Terr,@ZoneId) and the rec | CONFIRMED |
| BR-M13-049 | 533 | TotalSummaryNew2 ("Business Summary" in the legacy static menu) calls LoadSummaryProductcodewiseGyash(from,to,Type,Area,Terr,ZoneId) -> sp_BusinessSummaryMISReport_All_Ne | CONFIRMED |
| BR-M13-050 | 466, 491, 481, 508, 485, 516... | Sales-detail Excel report (crpAllSalesReport.rpt): one row per DELIVERED invoice line (ID.DeliveryStatus IN ('Full','Partial')), UNION ALL of depot (tblInvoice/tblInvoice | CONFIRMED |
| BR-M13-051 | 485, 516 | MIO/territory/product/district/region-wise sales is produced by appending a caller-built predicate to the sales query: MIOProductSalesReport builds a predicate such as AN | CONFIRMED |
| BR-M13-052 | 474, 475, 478, 484, 476, 487... | Company/ComUnit/Region/Zone/District/MIA/Customer sales reports aggregate View_MIAWiseSalesReport by ProductCode+Product: TotalQty = SUM(TotalQuantity), TotalAmount = SUM | CONFIRMED |
| BR-M13-053 | 478 | DistrictSalesReportDAL builds both its header and detail queries by string concatenation of districtId and fromDate/toDate (date formatting depends on server culture) - S | CONFIRMED |
| BR-M13-054 | 489, 529 | SalesTrendReport period = 01-Jan to 31-Dec of the chosen year; data from sp_GET_MonthwiseSale(@FromDate,@ToDate); CRP uses crpSalesTrendReport.rpt, the Excel option loads | CONFIRMED |
| BR-M13-055 | 477, 502 | DayWiseBusinessReport period is hard-wired to 01-<month>-<year> . | CONFIRMED |
| BR-M13-056 | 488, 526 | SalesComparisone report: per district, SaleValue = SUM(DeliveryTpGrandTotal) of invoices with DeliveryInvoiceStatus IN ('Full','Partial') in the calendar month of @Report | CONFIRMED |
| BR-M13-057 | 486, 522 | ProductWiseNationalSalesReport: for @FromDate month and each of the 3 previous months per product: DeliveryTotal = SUM(DeliveryTotalQuantity), SaleValue = SUM(DeliveryNet | CONFIRMED |
| BR-M13-058 | 483, 512 | HistoricalReport: the list page passes toDate = fromDateTextBox value (defect: both ends are the From date) and the DAL selects columns of tblCentralStore relabelled as M | CONFIRMED |
| BR-M13-059 | 515 | MIAWiseSalesReport option "Total Between Date" opens MiaSalesReportViewer.aspx, which exists only as MiaSalesReportViewer.aspx.exclude (excluded from the build) - dead li | CONFIRMED |
| BR-M13-060 | 470, 471, 472, 473, 493 | BranchTotalSummary offers four Excel exports: BusinessSummaryViewer2 (delivered sales by UpdateDate per depot/day, rptBranchSales.rpt), Viewer3 (proforma by InvoiceDate,  | CONFIRMED |
| BR-M13-061 | 468, 492 | AuditReportOne lists DELETED orders (tblOrderDel + tblOrderDetailDel) with MIO/RSM/territory/zone, deleter (tblUser.LoginName) and DelDate via sp_Get_OrderDelTrackingList | CONFIRMED |
| BR-M13-062 | 467 | AreaWiseMonthlySalesReportViewer: national=false -> sp_AreawiseDailyOpeningClosingStockDepowise(@fromDate,@toDate,@CiD) else sp_AreawiseDailyOpeningClosingStockNational(@ | CONFIRMED |
| BR-M13-063 | 524 | ProformaandSalesReport exports the invoice life-cycle via sp_InvoceLifecycle(@fromDate,@toDate); the district/ComUnit argument is accepted by the DAL but never sent to th | CONFIRMED |
| BR-M13-064 | 509, 510, 508, 481 | FinalsalesReport / FinalsalesReport_T (identical code, class name only differs) show sp_GetFinalSales(@fromdate,@todate) plus a customer/invoice count panel from sp_Numbe | CONFIRMED |
| BR-M13-065 | 499 | CustomerWiseSale builds a month-by-month pivot in C# (TotalSummaryBLL.CustomerWiseSale): for each month between From and To it calls GetmonthlycustomerSaleDate(first, las | CONFIRMED |
| BR-M13-066 | 521 | PendingSalesConfirmationReport lists orders/invoices awaiting confirmation via sp_GET_PendingSalesConfirmationReport(@Parm) (predicate string incl | CONFIRMED (wiring) / UNKNOWN (SP) |
| BR-M13-067 | 527 | SalesConfirmationReport_New filters (ComUnit, market structure, program type, chemist type, customer, product, order sender) are concatenated into @Parm/@Parm2 for sp_Get | CONFIRMED |
| BR-M13-068 | 466, 481, 469, 470, 471, 472... | Viewer pages (SInventory_RPTVIEW) are stand-alone pages with no master page and no Session/login or role check; they trust QueryString (fromDate, toDate, ids, rptType, Na | INFERRED |

### M14 — Target & DWSP Management (0 rules)

_No rules index found in module README._

### M15 — HR, Field-Force & Employee Administration (0 rules)

_No rules index found in module README._

### M16 — Doctor / Provider Master, Visits & Prescriptions (0 rules)

Full text (source, implementation, confidence) is in the Tier-A page specs: PAGE-669 (BR-001..016), PAGE-658 (026..031), PAGE-662 (033), PAGE-663 (034), PAGE-653 (037..043), PAGE-654 (044..048), PAGE-674 (049..054), PAGE-675 (055..061), PAGE-673 (062..064). Rules 017-025, 032, 035, 036 are defined here (Tier-B pages read).

| BR | One-liner |
|---|---|
| BR-M16-001 | Doctor name mandatory (only server check) |
| BR-M16-002 | No other doctor field validated server-side |
| BR-M16-003 | Chamber needs name+type; phone 11 chars if given |
| BR-M16-004 | DoctorId>0 -> update SP, else save SP |
| BR-M16-005 | DoctorCode = 'D' + max numeric suffix (approved doctors)+1 |
| BR-M16-006 | Web-created doctors saved Approved ('2'), IsFromApp=0 |
| BR-M16-007 | Sales-person tagging CSV ids/codes; SalesPersonId = first |
| BR-M16-008 | Active/Inactive dates = page-load date; update overwrites both |
| BR-M16-009 | Degree list depends on doctor type |
| BR-M16-010 | Update deletes and re-inserts child rows |
| BR-M16-011 | Chambers used by Rx/DCR survive update purge |
| BR-M16-012 | Non-admin field locks (largely ineffective) |
| BR-M16-013 | Market/hierarchy inputs hidden; not editable |
| BR-M16-014 | Any failure reported as "Already Exist!" |
| BR-M16-015 | Bad special-day date stored NULL |
| BR-M16-016 | Program type / SMC type lists filtered by group flags |
| BR-M16-017 | sp_Get_DoctorList: IsDoctor=1 only, order by code desc, status text 0/1/2/3, tagged sales persons resolved (`sql:884-930`) |
| BR-M16-018 | DoctorView filters: default last 30 days by EntryDate; sales person matches SalesPersonId or tagged CSV; hierarchy filters inert (`DoctorView.aspx.cs:417-504`) |
| BR-M16-019 | DoctorView permission: UserRoleID != "2" needs page-permission row; RAdd/REdit control buttons (`DoctorView.aspx.cs:166-198`) |
| BR-M16-020 | DoctorView export: CSV, all pages, no Edit column (`DoctorView.aspx.cs:232-287`) |
| BR-M16-021 | Transfer needs selection + target market; customers blocked if order pending; runs as ApprovedBy=session user (`Customer_Doctor_Transfer.aspx.cs`) |
| BR-M16-022 | Lookup save: update checks duplicate via sp_check_*; insert has no C# check (`DoctorCategoryDal.cs:27-52`) |
| BR-M16-023 | Lookup delete via sp_Delete_* with @DeleteBy (`DoctorCategoryDal.cs:141-143`) |
| BR-M16-024 | Only Active lookup rows feed dropdowns, sp_Get_*_Active (`CommonDataLoad.cs:732-1117`) |
| BR-M16-025 | DoctorModule PrescriptionType save hard-codes user "1" (`DoctorModule_UI/PrescriptionType.aspx.cs:44-50`) |
| BR-M16-026 | DCRList default today; filters date/status/role/employee/platform |
| BR-M16-027 | DCRList and PrescriptionView scoped by RoleTypeName MIO/AM/DZSM/NSM |
| BR-M16-028 | Approve button only on Rejected rows, Reject only on Approved |
| BR-M16-029 | Approve/Reject appends tblDCRApprovalLog via sp_webapi_SaveDCRAppLog (Step+1, MenuId 382) |
| BR-M16-030 | No log row -> silent no-op |
| BR-M16-031 | DCRList hierarchy filters inert (hidden control) |
| BR-M16-032 | DoctorDailyVisit approval filter uses DCR.IsApproved 0/1 (`DoctorDailyVisit.aspx:191-203,491-493`) |
| BR-M16-033 | DCP list defaults to current month/year; raw SQL filter from browser |
| BR-M16-034 | DoctorVisitApprovalList non-functional (Razor URLs in WebForms) |
| BR-M16-035 | Monitoring dashboard drill-down is a stub; two link types unhandled (`DoctorVisitMonitoring.aspx.cs:120-235`) |
| BR-M16-036 | MIS/Salesman reports: robust date parse, EmpInfoId->EmpMasterCode lookup, "[]" on error (`DoctorVisitDAL.cs:154-275`) |
| BR-M16-037..043 | Prescription capture: image mandatory; Validation() dead; chambers per doctor; save flow + image file; approved lock UI-only; no product dedupe; EntryBy = Captured By |
| BR-M16-044..048 | Rx approval: list <> 'Approved'; >=1 row; bulk SP call; undefined `rbValue` defect; status vocabulary mismatch |
| BR-M16-049..054 | DoctorInfoReport: type required; view required (silent); default month; last process date; footer sums; export rules |
| BR-M16-055..061 | Monthly pivots: date-range pivot; 'Select' default; AM area / DZSM zone required; territory bug; text-vs-id; inert DCR/RX radio; unencoded HTML |
| BR-M16-062..064 | DCR page: only 3 report types reachable, hidden filters; hidden columns removed; dead DAL method calling RX SP |

### M17 — SAP, Mobile-App & External Integration (0 rules)

_No rules index found in module README._

### M18 — Approval Workflow (0 rules)

| BR | One-liner |
|---|---|
| BR-M18-001 | Next `Step` = row's current `Step` + 1 (all list pages) |
| BR-M18-002 | Approve/Reject buttons visible only when row `ToRoleTypeId` = user `RoleTypeId`, else "Waiting for Another Approver" (UI only) |
| BR-M18-003 | Gate-exempt RoleTypeIds differ per page (5,4,14 [+9] [+3]) |
| BR-M18-004 | Hard-coded employee 496 approves as "Accepted" and sees all rows (Leave, Expense, DA, Mileage) |
| BR-M18-005 | Hard-coded MenuId per page (302,303,356,371,372,376,377,379,381,382) |
| BR-M18-006 | Acting user's Group/Region/Area/Territory passed into the log (where the lookup is enabled) |
| BR-M18-007 | Scope by RoleTypeName AM→area, DZSM→region, NSM→group (DIC→DC on Order) via `@Role/@param` |
| BR-M18-008 | Filters are raw SQL text appended to dynamic SQL in `@param` |
| BR-M18-009 | Audit fields: Date/EntryDateS/ApproveDateS = now, EntryByS = EntryByApp = UserId, FromEmpId = EmpInfoId |
| BR-M18-010 | Bulk processes ticked rows whose action button is visible; per-row counts |
| BR-M18-011 | Menu-permission row required for page unless UserRoleID = 2 (Order: also unless UserType Admin) |
| BR-M18-012 | DIC role: DC dropdown preselected to DICCompanyUnitId and disabled |
| BR-M18-013 | Save "success" = no exception (`SaveData`) or scalar > 0 (`SaveDataReturnPrimaryKey`) or rows affected > 0 (customer) |
| BR-M18-014 | Status vocabulary: Verified / Accepted / Rejected |
| BR-M18-015 | Customer approve always writes "Accepted" |
| BR-M18-016 | Customers with ActionStatus 2 or 3 excluded from queue |
| BR-M18-017 | Customers sharing CellNo with another non-rejected customer flagged "Already Exists" |
| BR-M18-018 | Customer filter/search UI hidden server-side |
| BR-M18-019 | Customer UI reports success even if save failed |
| BR-M18-020 | ActionStatus map 0 Pending / 1 Verified / 2 Approved / 3 Rejected; waiting role default "Pending Approval" |
| BR-M18-021 | Doctor approve always "Verified" |
| BR-M18-022 | Doctor page: no scope, no filters, no hierarchy ids |
| BR-M18-023 | Doctor waiting message "Waiting for Another Employee" |
| BR-M18-024 | Order approve = "Accepted" iff RoleTypeId 5 else "Verified" |
| BR-M18-025 | Orders with ActionStatus 2/3 excluded |
| BR-M18-026 | AM/DZSM/NSM/DIC see orders ≤ 7 days old |
| BR-M18-027 | DIC scope = selected DC (`dcMas.DCId`) |
| BR-M18-028 | Order page redirects to Login when Session UserId missing |
| BR-M18-029 | Order and Leave both use MenuId 381 |
| BR-M18-030 | Leave approve "Accepted" iff RoleTypeId ∈ {5,9} or EmpInfoId 496 else "Verified" |
| BR-M18-031 | Leave reject writes Type "Order" |
| BR-M18-032 | Leave date filter default last 7 days..today on EntryDate |
| BR-M18-033 | Leave approve link disabled if status "Approved"; reject disabled if "Rejected" |
| BR-M18-034 | Role 3 gate-exempt on Leave only |
| BR-M18-035 | Leave/Order edit button never visible |
| BR-M18-036 | Leave log hierarchy ids always 0 |
| BR-M18-037 | Expense approve "Accepted" iff RoleTypeId 5 or EmpInfoId 496 |
| BR-M18-038 | Expense sends approval id 0 (SP must find log by TableId) |
| BR-M18-039 | Expense Type "ExpanseClaim" (approve) / "Expense" (reject) |
| BR-M18-040 | Expense edit opens ExpenseClaim with id and Rid=RoleTypeId |
| BR-M18-041 | Expense date filter default today..today |
| BR-M18-042 | Expense: AM/DZSM/NSM see claims ≤ 7 days old |
| BR-M18-043 | Expense page has no bulk approval |
| BR-M18-044 | Tour plan Type "Leave" for both actions |
| BR-M18-045 | Tour plan default filters: current year/month |
| BR-M18-046 | Tour plan page has no bulk approval |
| BR-M18-047 | DCP Type "DCP" (approve) / "Leave" (reject) |
| BR-M18-048 | DCP Reset goes to TourPlanApproval |
| BR-M18-049 | DCP default filters: current year/month |
| BR-M18-050 | Customer with pending order cannot be ticked for transfer approval |
| BR-M18-051 | Transfer Submit calls `sp_Update_Customer_Doctor_TransferApprove` per ticked row; last result decides message |
| BR-M18-052 | Transfer Submit is gated by `Validation()` on an unbound grid → never executes |
| BR-M18-053 | Transfer page toggles customer/doctor lists via transfer-approval list SPs |
| BR-M18-054 | Transfer page has no Session/permission check |
| BR-M18-055 | Transfer approver recorded as Session UserId |
| BR-M18-056 | `tblAction` action list = rows with `IsShow=1` |
| BR-M18-057 | tblAction engine uses only the first matching user/page row |
| BR-M18-058 | "Cancel" action stays enabled (unless equal to the condition) |
| BR-M18-059 | Action whose text equals the row's `ActionCondition` is disabled |
| BR-M18-060 | No assignment → all actions disabled |
| BR-M18-061 | Approval chain config = master (menu, from-role) + details (to-role, order); saved by separate SP calls, no transaction, no authorisation (`ApprovalStepMap`) |
(BR-M18-056..061 sources: `ClsApprovalAction.cs:25-63`, `ApprovalMapDAL.cs:99-156`, `ApprovalStepMap.aspx.cs:21-34`; CONFIRMED.)

## 7. Approval Rules

Approval pages live in `Solution.Web/Approval_UI` (M18) plus approval steps embedded in module pages (order special approval, stock-in approval, credit-limit approval, stock-out approval, transfer approvals, leave/expense/tour-plan/attendance approvals). Key observed facts (see `specs/approval-workflow/README.md` §3–5 for cited rules):

- The generic engine `ClsApprovalAction` + `tblAction*` tables exists, but **no M18 page uses `tblAction*`**; approvals are implemented per page with status flags and hard-coded approver/role/MenuId values (e.g. employee 496, MenuId 381 shared by Leave and Order).
- Role gating on approval pages is UI-only; there is no server-side re-check (M18 gaps).
- Web-created customers/doctors are inserted already approved (`ActionStatus='2'`; `sp_Save_CustomerMaster`, `sp_Save_DoctorMaster` hard-code Approved).

## 8. Roles

Roles are data (`tbl_UserRoleInfo`, `tblRoleType`, `tblUserType`); the login returns `UserRoleID`, `RoleTypeId`, `RoleTypeName`, `UserType` (Admin has no ComUnitId). **Role names/permissions are UNKNOWN from the repository.** Role id 2 is a hard-coded bypass in code (M02 README). See `specs/00-system/role-permission.md`.

## 9. Permissions

This section states precisely what the code enforces (CONFIRMED by reading code/config unless tagged).

**Layers that exist**
1. **Authentication gate = session, not Forms.** `Web.config` declares Forms auth but no code issues an auth cookie and there is no `<authorization>`/`<location>`; access control is `Session["UserId"]` (M01 GAP-M01-002).
2. **Master-page guard.** `NewMasterPage`: first GET only (redirect to Login if `Session["UserId"] == null`); `MainMasterPage`: every request; `MasterPage.master`: none. Postbacks to NewMasterPage pages, static `[WebMethod]` page methods, `.ashx`, and 132 no-master `.aspx` are outside this guard.
3. **Menu hiding (the main "authorization").** The navigation HTML comes from `dbo.MainMenu2(UserId, UserRoleID)` (body UNKNOWN); pages are shown or hidden by menu row only. **The menu does not stop direct URL access.**
4. **Per-page server-side URL permission — 17 pages only.** `UserPersmissionValidation()` in: 10 `Approval_UI` list pages, `Dashboard_UI/AdminDashboard`, `DoctorModule_UI/{AppMonitoringList, AttendanceListApproval, UserRecords}`, `MasterSetup_UI/{CustomerView, DoctorView, EmployeeRecords}`. Logic: `Session["UserRoleID"] != "2"` → call `sp_GET_MainPermissionByUserRoleandPageUrl(@RoleId, @PageName)` (`CommonDataLoad.cs:17-38`, path normalised); **no row or any exception → redirect to `DashboardOne.aspx`**; role id **2 is a hard-coded bypass** (32 code sites test `!= "2"`). It runs only when `!IsPostBack`. `RAdd`/`REdit` are applied only in `UserRecords`, `CustomerView`, `DoctorView`; elsewhere the lines are commented out; `RView`/`RDelete` are never consumed in C#.
5. **Approver mapping.** `tblAppSetup` check (`DataTrue()`) live in 1 page only (PAGE-019 BR-M02-048). The user-based action mapping (`tblActionUserWiseApproval`/`tblActionPageWiseStep`/`tblActionSteps`/`tblActionStepDetail`/`tblAction`, `ClsApprovalAction`, `OrderListDAL.LoadApprovalControlDAL`) enables/disables approval radio buttons on `ManualOrderApproval` by `Session["LoginName"]` and page file name — UI control state only (the class builds SQL by concatenating login name/page name). No page in the repo writes those tables.
6. **Role-type gating in code.** `Session["RoleTypeId"]/RoleTypeName` are compared to literals in approval/dashboard pages (RoleTypeId 2/3/4/5/9/14; `RoleTypeName=="DIC"` in 20 places); `Session["UserType"]=="Admin"` in 19 places; approval-list rows compare `ToRoleTypeId` (from the approval chain data) with the session role type before showing action controls (e.g. `CustomerApproveList.aspx.cs:532-548`) — UI-level.
7. **Data scoping by session.** `ComUnitId`, `DICCompanyUnitId`, `DICUnitId`, `CentralWareHouse` (from `tblUserCompanyUnit`/`tblUser`) parameterise SPs/queries in other modules; whether each page applies them is outside M02 (note `CWHPermission`, `NationalReportPermission` have no C# reader).

**What is enforced server-side vs only hidden**
| Control | Server-side enforced? |
|---|---|
| Being logged in for NewMasterPage GET | Yes (first load only) |
| Being logged in for POST/WebMethod/no-master pages | No (unless a method reads `Session`) |
| Role may open a given page | **Only for the 17 pages above** (and role 2 always); all other pages: menu-hiding only |
| Add / Edit flag | 3 pages (UI visibility of New button/Edit column); otherwise not |
| View / Delete flag | Not consumed anywhere in C# |
| Who may administer users, roles, permissions, DC scope, approvers, approval chains | **No check at all** (PAGE-015/016/013/027/026/020/019/017): any logged-in user with the URL — and, for the AJAX endpoints of PAGE-027/026, any HTTP client without a session |
| Approver identity when approving | Mostly not (check commented out in 3 of 4 pages) |
| Password strength at admin creation | No (only self-change on ChangePassword, M01) |

**Weaknesses (documented, not fixed):** plaintext passwords visible in admin screens (GAP-M02-002); `SaveMenu` role-permission and approval-chain endpoints unauthenticated (GAP-M02-001); role 2 magic bypass; hard-coded role ids (GAP-M02-006/023); permission model has three overlapping mechanisms (role-menu, per-user legacy grants, approver tables) with unknown SP semantics (GAP-M02-015).

## 10. Status Lifecycles (compiled)

### M01 — Authentication, Session & Navigation

- **User account** (as seen by login): `tblUser.UserStatus = 'active'` required (M02 edits it: 'Active'/'Inactive').
- **Password-change flag:** `IsPasswordChange` 0/NULL → (ChangePassword/UserProfile) 1. Who resets to 0: UNKNOWN.
- **Session:** none → (login) authenticated → (logout: keys blanked, session object survives) → (timeout 24000 min / recycle) gone. Tracking row: `IsActiveSession` 1 → 0 only when a later login reuses the same SessionId.

### M02 — User, Role & Permission Administration

- **User:** `UserStatus` Active ⇄ Inactive (PAGE-015); login requires `'active'`. `IsPasswordChange` 0/NULL → 1 (M01). No delete UI except legacy `UserBLL.DeleteUserInfo` (physical `DELETE FROM tblUser`, legacy pages).
- **Role:** `IsActive` true/false with date; block rule UNKNOWN.
- **Role permission / approval chain / approver list / DC set:** replace-all sets, no status.
- **Order delegation:** time window From–To.

### M03 — Organisation & Market Structure Master

- Node active flag (all hierarchy levels): `IsActive` + `AcOrInAcDate`; deactivation guarded by `sp_check_Vali_MarketStructure`.
- Structure transfer request: `Requested → Approved` (INFERRED); no reject.
- Bulk market import: `Staged → Applied` (approve via `sp_Save_MarketPropToTable`).
- Financial-year processing: `(none) → Completed` opening (`tblOpeningBalanceFinancialYearLog.Status='Completed'`) gates purge.
- Deposit: `IsDelete` soft flag (other page, 60-day window).

### M04 — Product, Pricing & Promotion Master

- Campaign: `IsActive` flag + `FromDate..Todate`; no transition code; edit replaces the row (live page) - PAGE-111 sec 21.
- Unit price: Active row -> archived (IsActive 0, InActiveDate) in `tblUnitPriceRollback` + new active row - PAGE-157 sec 21.
- Product: `IsActive` bit only; no delete.
- Quoted price / promo tag / promo qty: no status column handled in C#.

### M05 — Customer & Provider Master

- **Customer** `tblCustMaster.ActionStatus`: `0` Pending → `1` Verified → `2` Approved / `3` Rejected (labels CONFIRMED in `sp_Get_CustMasterList_Approve`); web create ⇒ `2`; transitions of 0/1/3 by approval flow (UNKNOWN SP). `IsActive` separate.
- **Credit limit**: `Posted` → `Approved` | `Reject`; edit ⇒ `Posted`; delete ⇒ `IsActive=0`.
- **Excel batch**: Uploaded → Verified (`VerifyedAll`) → Transferred (`Transfer`) | Deleted (before transfer).
- **Property import batch**: Pending → Approved (INFERRED).

### M06 — Order Management

| State carrier | Values seen | Set by |
|---|---|---|
| `tblOrder.ActionStatus` | `'2'` (web manual, invoiceable), `'Accepted'`, `'Approved'` (approval text), other approval texts (UNKNOWN) | `sp_Save/Update_ManualOrderMaster` ('2'); `OrderListDAL.ApprovalUpdateDAL` (action text); app orders: UNKNOWN |
| `tblOrder.IsSpecialApproval` | 1 = needs special approval | UNKNOWN producer |
| `tblOrder.IsInvoice` | 0 → 1 ('True') on invoice creation; 1 → 0 on proforma delete | `OrderInfoDAL.UpdateInvoiceStatus`; `sp_Delete_ProformaInvoice`; `sp_Process_ProformaInvoiceByOrderId` (UNKNOWN) |
| `tblOrder.IsPrepareforInvoice` | 1/NULL = ready | manual SPs |
| `tblOrder.ComUnitId` NULL → set | DC assignment | `sp_Update_OrderDC` (UNKNOWN), OrderEdit |
| `tblOrderDetail.Status` | NULL, `'Undelivered'` | PAGE-218 (`InvoiceDAL.UpdateOrder`), delete-proforma resets NULL |
| `tblInvoice.DelivaryInvoiceNo`/`DeliveryInvoiceStatus` | NULL → `DEL-…`; Full / Partial / Reject | PAGE-215/214 |
Flow: `[created ActionStatus='2' | app order (approval?) ] → [special approval → 'Approved'] → [DC assigned] → [credit ok] → IsInvoice=1 (proforma) → delivery Full/Partial/Reject`; reverse: delete proforma → `IsInvoice=0`.

### M07 — Proforma, Invoicing & Delivery

**Order/invoice (proforma -> delivery):**
1. Order created (M06) `tblOrder.IsInvoice = 0`.
2. Invoice creation (M06 by-order pages, or `sp_AutoInvoiceGeneration`): `tblInvoice` row with `DelivaryInvoiceNo IS NULL` (proforma), `tblInvoiceDetail` batch rows, `tblDCStore` Issue rows (stock reserved), `tblOrder.IsInvoice = 1` (INFERRED from the reset in `sp_Delete_ProformaInvoice`).
3. Proforma deletion (PAGE-277, M06, `ProformaorInvoiceReturn`): row deleted, Issue rows removed, `IsInvoice = 0`, `tblOrderDetail.Status = NULL`, logged.
4. Loading summary (PAGE-271): `LoadingSummaryStatus` = `Full Dues` | `Cash` | `Rejection` (`sp_UP_LoadingSummary`); `Partial Dues` -> partial screen. "Full Dues" also runs `sp_DeliveryConformationFull_New` (delivery invoice: `DelivaryInvoiceNo`, `DeliveryInvoiceStatus` 'Full'/'Partial', `DeliveryTpGrandTotal` - INFERRED) and `sp_Sales_DeliverySalesPosting` (voucher TableID 2014).
5. Payment confirmation (PAGE-264 -> PaymentPartial M09): `PaymentInvoiceNo` (NULL until confirmed; `RTN-`+delivery no on partial), `PaymentInvoiceStatus` ('Reject'), `PaymentStatus` ('Partial').
**DC picking:** presence in `tblDCPickingDetail` = picked (no status column).
**Requisition/STO challan:** `tblRequisition.CreatePicking` NULL/'' -> 'OK' and `Submit` = 'OK' at PAGE-259 save; `tblRequsitionChild.IsPicking/IsIssue` = 'OK'; `tblStockInTransfar.IsIssue` = 'OK'; later DC receipt (other module) moves stock into `tblDCStore` Receive rows.
Details per page in §21 of each Tier-A spec.

### M08 — Sales Return & Invoice Return

| Object | States | Set by |
|---|---|---|
| tblReturnInvoice (market return) | created (IsPosting NULL/0) -> posted (IsPosting=1) | `sp_SalesReturnPosting` (PAGE-294) |
| tblCustomerReturnMaster (delivery return) | ActionStatus 'Posted' at creation (the design also has pending/approved: `ApprovalUpdateDAL`, unused); IsPosting 0/NULL -> 1 | PAGE-287 / `sp_DeliverySalesReturnPosting` |
| tblInvoice second-time return | `sndReturnInvoiceStatus` NULL -> Full / Partial / Reject; `SndReturnInvoiceNo` set (`SRN-`+InvoiceNo) | PAGE-298 |
| tblInvoiceDetailReturn.sndReturnStatus | Full / Partial / Reject | PAGE-298 |
| tblDCStoreFreeze.StockCondition | 'ReturnStock' (legacy return) | PAGE-290 |
| Invoice deletion | invoice row deleted; log rows written; `tblOrder.IsInvoice` 1 -> 0 | `sp_Delete_ProformaInvoice` (PAGE-288) |
| Worklist gate | shown while `PaymentInvoiceNo` not null and `SndReturnInvoiceNo` null | PAGE-296 |

### M09 — Collections, Payments & Money Receipts

- **Invoice payment stage** (`tblInvoice.PaymentInvoiceStatus` / `PaymentInvoiceNo`, set by PAGE-334): none → Full | Partial | Reject (with `RTN-…` number). Line-level `tblInvoiceDetail.PaymentStatus` same domain.
- **Invoice collection status** (`tblInvoice.PaymentStatus`, `PaymentAmount`, written by UNKNOWN SP `sp_Update_InvoiceFinalPayment` from PAGE-311/314/315/318 with "Full"/"Partial"): NULL → Partial → Full; reset to NULL by PAGE-335. Collectable filters elsewhere use `PaymentStatus IS NULL OR 'Partial'`.
- **Payment rows** (`tblCustomerPay`, `tblCustPayDetail`): no status column; created final; removed by PAGE-335 (hard delete, first payment only) or PAGE-313 (SP delete with log).
- **Accounts voucher**: created `Approved` immediately (CSTL_ACCDB_Unicorn `ActionStatus`); never reversed by any code in this module.
- **Deposit slip** (`tblCompanyWiseDeposit`): active (`IsDelete=0/NULL`) → deleted (`IsDelete=1`, DeleteBy/DeleteDate) within 60 days by two named users.

### M10 — Warehouse, DC Stock & Inventory

- Stock-in (`tblWHStockInMaster.Status`): new -> Posted -> Approved | Reject; Reject -> (edit) Posted; blank via WarehouseStockInEdit (defect).
- Depot voucher (`tblDeStockOutMaster.Status`): Posted -> Approved (SP) | Reject; Approved not deletable.
- WH adjustment (`ActionStatus`): Approved at creation.
- Sample conversion (`tblSampleStockForDcMaster.Status`): Posted only.
- Stock condition: Available -> (freeze) condition in freeze table -> (release) qty returned; no status column.
- tblDCStore rows: Trans_Type Receive / Issue; StockCondition Available / B2BReceive / B2BIssue / TransferReceive / TransferIssue / SalesReturn / Return / Restricted / Blocked (values seen in code).

### M11 — Stock Transfer & Chalan Receive

| Object | States | Set by |
|---|---|---|
| tblChalanInfo.IsDeliver (DC-DC) | 'False' (created, in transit) -> 'True' (received) | PAGE-412 insert / `ChalanDAL.ChalanUpdate` (PAGE-423) |
| tblChalanInfo SAP flags | SAP_ChallanSend 0->1; SAP_Challan_ConfirmationSend 0->1 | `sp_SAP_Up_SAP_ChallanSendByChalanId` / `...ConfirmByChalanId` (external caller) |
| tblDepotToWHChalanInfo | IsTransfer NULL -> 'OK' (stock deducted, PAGE-409); IsDeliver 'False' -> 'OK' (WH received, PAGE-414) | PAGE-409, PAGE-414 |
| STO chain | tblRequisition Submit 'OK' (issued) -> ReceiveIssue 'OK' (DC received); tblStockInTransfar IsIssue 'OK' -> IsTransfared 'OK' | PAGE-219/259, PAGE-413 |
| tblDCStore ledger | Issue row (B2BIssue, negative) at DC-DC send; Receive row 'B2BReceive'/'Available' at receipt | PAGE-412 / PAGE-423 / PAGE-413 |
| Freeze stock | tblDCStoreFreeze / tblWhStoreFreeze StockCondition 'Restricted' | receive pages |

### M12 — Sub-Depot Operations

**DC -> Sub-depot chalan** (`tblSubDepotChalanInfo.IsDeliver`): `[none] --PAGE-450 submit--> IsDeliver=false (in transit; DC tblDCStore.StockQty already reduced)` `--PAGE-446 first line saved--> IsDeliver=true (sub-depot tblSubDepotStore rows Available; damage -> tblSubDepotStoreFreeze Restricted)`. No reject/cancel/partial state (BR-M12-010,013,014,016,017).
**Sub-depot -> DC return** (`tblSubDepotChalanReturnInfo.IsDeliver`): `PAGE-451 submit -> false (sub-depot StockQty already reduced)` `--PAGE-441 first line--> true (new tblDCStore row, ComUnitId hard-coded 6; damage -> tblDCStoreFreeze Restricted)` (BR-M12-011,018,019).
**Freeze row** (`tblSubDepotStoreFreeze.StockCondition`): created `Restricted` (receive damage) or `ReturnStock` (sales return, `ReturnInvoiceDAL.cs:1212`); qty released back to `Available` store row by PAGE-453 (Restricted only by login 21900); `Blocked` counted by report but never created by M12 code (BR-M12-028..031).
**Adjustment voucher** (`tblSubDepotStockOutMaster.Status`): `Posted -> Approved` (SP `sp_UD_SubDcStockOutApproval`, stock effect UNKNOWN); `Reject` wrongly updates the DC table (BR-M12-023..026). Direct adjustment (PAGE-448) has no status; stock reduced at once (BR-M12-021).
**Sub invoice**: order `IsInvoice 0` -> proforma (`DelivaryInvoiceNo` NULL) -> delivery `DeliveryInvoiceStatus Full/Partial/Reject` (SPs UNKNOWN) -> `PaymentStatus NULL/Partial/Full` (BR-M12-038..041,035).

### M13 — Sales Reports, Analytics & Dashboards

Report pages are read-only. Statuses they read: `tblInvoice.DeliveryInvoiceStatus` (NULL = undelivered, Partial, Full), `tblInvoiceDetail.DeliveryStatus` (Full, Partial, Reject), `tblInvoice.PaymentStatus/PaymentAmount`, `tblOrder.IsInvoice`, `tblRequisition.CreatePicking/Submit/ReceiveIssue`. Statuses **modified** only by the two rollback pages: order invoiced -> not invoiced; delivery Full/Partial -> NULL; payment settled -> NULL; requisition challan-issued -> picking -> STO -> deleted (BR-M13-002..014). Forward transitions are set by other modules.

### M14 — Target & DWSP Management

No status/approval flags are set by any M14 page (CONFIRMED). Data states are only "row exists for (level, year, month, entity)". Inline **UpdateData** on the four `*View` pages re-saves an existing row (id > 0 -> update path in the same SP). The DWSP monthly report has an *Approval Status* filter (values from `SeedDataDAL.GetApprovalStatusList`) that is produced elsewhere (UNKNOWN who sets it; no M14 code writes it).

### M15 — HR, Field-Force & Employee Administration

- Employee/User/Hierarchy: Active/Inactive (BR-M15-009).
- Approval log: `Verified` -> `Accepted` (final) or `Rejected` (WF-M15-05, WF-M15-06). Approval-status master `sp_GET_ActionStatusList` (`SoftwareUseId`, `WebShow`) - values UNKNOWN.
- Tour plan: `ApprovalStatus` `2` approved, `3` rejected, other pending (BR-M15-070).
- Leave application: `Pending / Approved / Rejected` labels seen in JS (`LeaveApplicationEntry.aspx`), SP-driven (UNKNOWN).

### M16 — Doctor / Provider Master, Visits & Prescriptions

1. **Doctor master** (`tblDoctorMaster.ApprovalStatus`): 0 Pending, 1 Verified, 2 Approved, 3 Rejected (decode in `sp_Get_DoctorList`). Web save inserts '2'. Verify/reject by `Approval_UI/DoctorApprovalList` (other module). Separate `IsActive` flag with Activedate/InactiveDate.
2. **Call plan (DCP) -> DCR -> approval**: monthly plan headers (per employee, `ApprovalStatus`) created outside web; DCP approval page non-functional; DCR (`tbl_DCRInfo`) created by app; web list shows status text; Approved <-> Rejected toggle writes `tblDCRApprovalLog` (Type 'DCR', Status Accepted/Rejected, Step+1). Second flag `IsApproved` (0/1) in Daily Visit list. Reports filter by approval status. See WF-M16-03.
3. **Prescription (RX)**: capture (Prescription.aspx) -> ApprovalStatus (domain UNKNOWN; '2' = approved/locked in UI) -> approval page cannot submit (JS defect) -> RX reports. See WF-M16-04.

### M17 — SAP, Mobile-App & External Integration

- SAP challan: staged (`action`) -> received by `sp_SAP_StockReceive` -> hidden (INFERRED). ERP challan: `SAP_ChallanSend` 0/1, `SAP_Challan_ConfirmationSend` 0/1.
- Provider dropout: `IsApprove` 0 -> 1.
- API customer: `AddtoMainCustomer` 0 -> 1. MIGO: `StockUpload` 0 -> 1.
- Bank deposit: intended staged -> sent (sent step disabled).
- Customer app approval ActionStatus 0 Pending / 1 Verified / 2 Approved / 3 Rejected.

### M18 — Approval Workflow

- Log statuses written by pages: `Verified` (intermediate), `Accepted` (final; page-specific rule), `Rejected` — with `Step` incremented per action (WF-M18-01/02).
- Customer header `tblCustMaster.ActionStatus`: '0'/NULL Pending → '1' Verified → '2' Approved | '3' Rejected (list SP mapping, CONFIRMED). Doctor `ApprovalStatus` '2' = approved (repo SP inserts '2'). Others: UNKNOWN.
- Transfer request: pending → approved (SP-defined); no reject in live path.
- Numeric ↔ word mapping (`Accepted`→2?) is inside unavailable SPs (GAP-M18-026).

## 11. Business Policies (observed in code — not necessarily intended)

- Codes/ids are generated as MAX+1 without locking (~150 sites) — duplicates possible under concurrency (`docs/architecture.md`).
- Credit-limit enforcement is a UI button state only in order→invoice pages (M06 README); editing/rejecting an approved limit makes the customer unlimited (M05).
- Only `CustomerPayment` posts vouchers to the accounts DB; other payment paths and reversals do not (M09 README).
- Two stock models coexist: in-place quantity updates vs. negative-issue ledger rows with weighted-average/batch ledgers (`docs/database/stored-procedures.md`, M10 README).
- Sub-depot stock carries no cost (M12 README).

## 12. Exceptions / anomalies

See [specs/SPEC-GAPS.md](../specs/SPEC-GAPS.md) (574 module gaps) and `docs/security.md` risk register.

## 13. Integrations

See [integrations.md](integrations.md) and `specs/integrations/README.md` (14 integrations: SAP, mobile app shared DB, provider dropout, Excel imports, GeoIP, SQL Agent job).

## 14. Important Calculations

Calculation rules extracted from module rule indexes (lines mentioning formulas/totals/VAT/discount/rounding/average). Authoritative text is in the page specs.

| BR-M06-006 | Totals: Gross=Σ TP×qty, VAT=Σ vat×qty, no discount, Net=Gross+VAT | PAGE-221 |
| BR-M06-018 | Approver sees customer due (two formulas) | PAGE-220 |
| BR-M06-020 | Credit permission formula (limit + day-limit) | PAGE-217 |
| BR-M06-034 | Line net formula; order price overrides list price | PAGE-218 |
| BR-M06-035 | Header totals + recompute from details | PAGE-218 |
| BR-M06-037 | Discount % source (trade policy, INSTITUTION/RCP, FCB FY-23-24) | PAGE-218 |
| BR-M06-044 | Delivery charge in grand total/receivable | PAGE-218 |
| BR-M06-075 | OrderListView lists all orders with `IsInvoice='0'` and previews them with totals (Σ qty, TP, VAT, Net) | PAGE-226 (`OrderInfoDAL.cs:592-601`; `OrderListView.aspx.cs:87-107`) — CONFIRMED, Tier-B page |
| BR-M08-007 | Line calculation: TP = unitPrice×qty; VAT = unitVAT×qty; discount % = trade-policy % (non-fixed customers; 0 if FixedCustomer or lookup fails) replacing the product-discount % computed in RecalculateRow; discount amt = TP×%/100; N... | PAGE-294 |
| BR-M08-008 | Net Return Amount = max(Return Total − Deduction Charge, 0); if the posted net is ≤0 while grand>0 it is recomputed the same way. Deduction has no upper validation. | PAGE-294 |
| BR-M08-011 | Stock effect: for each line with returned qty>0 and a DC, a new tblDCStore 'Receive' row is inserted (location "Depot Return Store", StockCondition "Non-Transcational Sales Return", TotalQuantity=StockQty=qty, ChalanNo=return invo... | PAGE-294 |
| BR-M08-014 | COGS/inventory voucher: Dr Finished-Goods Inventory (23/1018, fb 4156) / Cr COGS (23/1019, fb 4162) for Σ (TotalQuantity or Quantity) × (CostPrice, else latest tblDCStore.CostPerUnit>0 of the product in any DC); IsInventoryValuati... | PAGE-294 |
| BR-M08-017 | No VAT-liability reversal is posted: VAT is inside the Sales Return debit (gross) and the receivable credit; deduction is booked as income without VAT. | PAGE-294 |
| BR-M08-018 | tblReturnAmount.Amount = Return Total (gross, before deduction). | PAGE-294 |
| BR-M08-020 | The user must press Calculate after any DQty change and before saving. | PAGE-298 |
| BR-M08-026 | Line snapshot stored in tblInvoiceDetailReturn with PreviousQuantity = payment qty and sndReturnQuantity/TotalQuantity = DQty (i.e. the *retained* quantity, not the returned quantity). | PAGE-298 |
| BR-M08-027 | Line amounts recomputed on DQty: TP = unit price × DQty; VAT = unit VAT × DQty; Net = TP − discount + VAT; lines with zero TP get VAT/discount/net = 0; campaign/trade-term discount % recomputed from totals (old-trade-policy vs new... | PAGE-298 |
| BR-M08-032 | Available quantity = DeliveryTotalQuantity − Σ prior ReturnQty per invoice detail; return qty is an integer 0…available; total must be >0. | PAGE-287 |
| BR-M08-033 | Line return amount = Return Qty × (UnitPrice + UnitVatAmount) where UnitPrice DataKey is the alias ROUND(DeliveryNetAmount/Quantity,4) (first of two UnitPrice columns) — VAT is added again on top of a delivery net amount. | PAGE-287 |
| BR-M08-034 | Deduction ≤ Total Return Amount (blocked at save, reset on edit); Net = max(Total − Deduction, 0). | PAGE-287 |
| BR-M08-036 | Stock effect: returned qty is added as a new 'Receive' ledger row (StockCondition='SalesReturn', TotalQuantity=0, StockQty=qty, cost = original cost) referencing the original batch row. | PAGE-287 |
| BR-M08-037 | Voucher 1 (Journal, 'Approved', by 'admin'/'Auto Process', date=today, TableID 2018, CommonPK_ID=CustReturnId): Dr Sales Return (23/1020, fb 4170) = TotalAmount; Cr Delivery Charge Income (23/1032, fb 5968) = deduction if >0; Cr A... | PAGE-287 |
| BR-M08-038 | Voucher 2 (TableID 2023) posts only if StockValue>0: Dr Finished Goods (23/1016, fb 4156) / Cr COGS (23/1021, fb 4162) where StockValue = Σ (tblCentralStore.UnitPrice + VATPerUnit) × ReturnQty, traced invoice detail → tblDCStore.T... | PAGE-287 |
| BR-M08-040 | Only ticked lines are returned; a line's cumulative returned qty (PRQty = Σ earlier tblReturnInvoiceDetail.TotalQuantity for that original invoice detail + this RQty) may not exceed the line's total delivered qty (TQty). | PAGE-290 |
| BR-M08-041 | Return-line VAT is recomputed as TP × product VAT % / 100 and discount % is re-derived from the *order's* trade terms (GetTradeTerm), instead of using the invoiced unit VAT/discount. | PAGE-290 |
| BR-M08-042 | Amount defect: the saved detail TotalPrice/TotalPriceVatAmount are computed from qtyTextBox (delivered quantity) × unit price/VAT, while Quantity/TotalQuantity are the returned qty (dQty) — saved amounts do not match saved quantit... | PAGE-290 |
| BR-M08-045 | ProformaorInvoiceReturn (PAGE-288) refuses to delete a delivery invoice when its lines already have freeze stock consumed (tblDCStoreFreeze.StockQty <> TotalQuantity) — the freeze rows created here are the trigger. | PAGE-290 |
| BR-M08-051 | Stock reversal is by deleting the invoice's negative 'Issue' ledger row (matched by ReferenceDCStoreId, Trans_Type='Issue', StockQty = −TotalQuantity, one row per detail) — the Receive row's StockQty is *not* increased; over-infla... | PAGE-288 |
| BR-M08-054 | "Delivery" deletion is refused if any of the invoice's freeze-stock rows has been partially consumed (StockQty<>TotalQuantity). | PAGE-288 |
| BR-M09-003 | Ticking a row defaults pay=Due, discount=0, AIT=0; ≥1 row required | 311 |
| BR-M09-004 | pay+discount+AIT+previous ≤ Confirm Amount (UI-event check only) | 311, 315 |
| BR-M09-006 | TP/VAT split: VAT first, then TP | 311, 314, 315, 318 |
| BR-M09-007 | Status Full iff pay+AIT+discount+previous == Confirm Amount, else Partial | 311 |
| BR-M09-012 | Auto-posted approved voucher structure (Dr cash/bank, discount, AIT; Cr AR) | 311 |
| BR-M09-022 | Calculate before save (flag never reset) | 334 |
| BR-M09-027 | Line amounts: TP, VAT, discount, NP formulas | 334 |
| BR-M09-029 | Zero-TP lines zeroed in totals | 334 |
| BR-M09-030 | Discount % source order (fixed customer, old policy, campaigns, trade term) | 334 |
| BR-M09-034 | Excel payments: Cash, no accounts, no discount/AIT, **no voucher** | 314 |
| BR-M09-040 | Footer sums rounded to whole taka; receivable formula in SP | 336 |
| BR-M09-051 | Deposit slip upload: typed Total Amount must equal Σ Excel amounts; every row needs bank + mapped account for the user's depot (`sp_GET_BAnkInfoById_ByIdExcel`, `sp_Get_BankSAPMappingAccounTNo`) or the row is red and the save blocked; company = `Session["ComUnitId"]` | 323 (`DepositSlipExcelUpload.aspx.cs:386-524`) — CONFIRMED |
| BR-M09-053 | `Aging.aspx` bucket headers: "1 to 10", "11 to 20", "21 to 30", "31 to 40", "41 to 50", "51+" days, each with TP Amount + Net Amount, plus Total TP / Total Net; bucket assignment logic is in `sp_Get_AgingReceivableReport` (not in repo) — bucket boundaries as *labels* only | 305 (`Aging.aspx.cs:54-94`, `Aging.aspx:248-280`) — CONFIRMED (labels) / UNKNOWN (calc) |
| BR-M09-055 | MarketWiseCustomerPayment (legacy): Σ row payments must equal the typed Payment Amount; status Full if pay+previous+adjust == invoice amount else Partial via `UpdateInvoicePaymentAmount` (writes tblInvoice.PaymentAmount = pay+previous); single-detail `SaveCustPayment` returns true even when its `Existence` check skipped the insert; no discount/AIT/posting | 328 (`:216-327`, `CustPaymentBLL.cs:55-90`) — CONFIRMED |
| BR-M09-056 | Posting audit thresholds: expected voucher amount for a payment = Σ(PaymentAmount+Discount+AIT) per CustPayId; statuses MISSING_VOUCHER, MISSING_VOUCHER_DETAILS, DR_CR_UNBALANCED (strict), MASTER_VALUE_MISMATCH / DETAIL_VALUE_MISMATCH (> 1.00 BDT) | audit script (`SP_Audit_Sales_Accounts_Voucher_Posting.sql:148-219`) — CONFIRMED |
| BR-M09-057 | CustomerPaymentReport lists all payment headers by header PaymentDate for **all depots**; voucher no = accounts voucher code else RefNo else `CT`+8-digit id; "Bank/Cash" is always the literal "General Cash"; Amount = header PaymentAmount (excludes discount/AIT) | 316 (`CustomerPaymentReport.aspx.cs:174-241`) — CONFIRMED |
| BR-M10-002 | Each line needs code, batch, exp, mfg, qty, VAT (price optional) | 385 |
| BR-M10-004 | Line = qty*price + qty*VAT; header totals summed | 385 |
| BR-M10-005 | Product pick defaults price=CostPrice, VAT=VATAmountPerUnit | 385 |
| BR-M10-019 | MC save Posted with MaterialConversion ids; cost = CostPerUnit, VAT 0 | 370 |
| BR-M10-021 | MC totals over selected rows | 370 |
| BR-M10-026 | Stock-out bypasses weighted-average/batch ledgers | 389 |
| BR-M10-062 | DC ledger opening (dated < FromDate): qty=SUM(StockQty), value=SUM(TotalCostAmount), rate=SUM(cost)/SUM(qty) if qty>0 else AVG(CostPerUnit); fallback tblUnitPrice.CostPrice (IsActive=1) | `...sql:44-46,83-92` |
| BR-M10-063 | Movement classes: Receive From WH (Receive/NULL/Available, no return id, condition not B2BReceive/TransferReceive/SalesReturn/Return, chalan not IDT%/RIN%/RINV%, qty>0); B2B Receive (condition B2BReceive/TransferReceive or chalan IDT%, qty>0); Return Rcv (ReturnFromInvoiceDetailId<>0 or condition SalesReturn/Return or chalan RIN%/RINV%); Sales (negative rows not B2BIssue/TransferIssue, ABS; or Issue/Sales rows); B2B Issue (condition B2BIssue/TransferIssue, ABS) | `...sql:95-150` |
| BR-M10-066 | National valuation, central store: latest tbl_INV_CentralStore_WeightedAvg row per Item_Id (max Weighted_Avg_Id) with Trans_Date <= ToDate gives Close_Qty, Avg_Rate, Close_Amt; opening from latest row with Trans_Date < FromDate | `...sql:283-307` |
| BR-M10-067 | National valuation, DC part: SUM(StockQty) and SUM(TotalCostAmount) per product as of ToDate; one dynamic pivot column per tblCompanyUnit name; National Total = central value + depot value; only products with positive opening/closing qty | `...sql:207-337` |
| BR-M10-068 | Batch drill-down by location: Central, Opening National, Total Depots, National, or one depot; SUM per batch, only positive | `...sql:366-481` |
| BR-M10-069 | Central STO issue valued at pre-issue average rate (Close_Amt/Close_Qty); new average = closing amt/qty; idempotent per (Mst_Trans_Id, Ref_No, Company, Item); batch row negative qty; no insufficient-stock test | `SP_Fix_CentralStoreStoIssue_Corrected.sql:99-199` |
| BR-M10-070 | Anomaly definitions: orphan Issue row = Issue with no tblInvoiceDetail (DCStoreId=ReferenceDCStoreId, TotalQuantity=ABS(StockQty)); corrupted Receive = StockQty > TotalQuantity | `sp_Check_DCStore_StockAnomalies.sql:20-91` |
| BR-M10-071 | sp_Adjust_DCStore_Stock_Only deletes matching Issue rows for an invoice and resets Receive StockQty to TotalQuantity (general step for every Receive row with StockQty <> TotalQuantity) in one transaction | `SP_Adjust_DCStore_Stock_Only.sql:30-113` |
| BR-M10-072 | Ledger semantics of tblDCStore: a sales issue inserts a new negative row (Trans_Type='Issue', ReferenceDCStoreId=source, ReferenceTransactionDate=now, cost carried) instead of updating the Receive row; DC receipts insert Receive rows with CostPerUnit/TotalCostAmount; B2B receipts StockCondition='B2BReceive' | `InvoiceDAL.cs:667-702`; `RequisitionDAL.cs:515-548`; `ChalanDAL.cs:126-165`; `sp_Delete_ProformaInvoice_Update.sql:108-140` |
| BR-M10-074 | Inactivated unit price rows move to tblUnitPriceRollback (RollbackBy, RollbackDate) inside one transaction | `SP_Create_tblUnitPriceRollback.sql`; `ProductUnitPriceDAL.cs:220-245` |
| BR-M10-076 | DC opening report reads tblDCStore_OpeningBalance at an exact DCOpeningBalanceDate | `DcOpeningStockReportDal.cs:24-45` |
| BR-M10-077 | Central lot availability = inbound batch rows (Trans_Qty>0) less outbound rows by Issue_Weighted_Avg_Id+batch; picking order ExpDate, ReceiveDate (FEFO) | `RequisitionDAL.cs:300-350` |
| BR-M11-003 | Available stock per batch/expiry = SUM(StockQty) of tblDCStore rows (Trans_Type Receive+Issue) of the from-unit and product, shown only if > 0; unit price/VAT come from the active tblUnitPrice. | PAGE-412 |
| BR-M11-007 | Challan value = Σ qty × active unit price; VAT = Σ qty × VAT/unit; grand = value + VAT (per detail Value, Vat, ValueWVat). | PAGE-412 |
| BR-M11-018 | Saving an STO does not move stock; stock moves at picking/issue (central store Quantity set to remaining; weighted-average issue at the running average rate). | PAGE-420 |
| BR-M11-021 | STO issue costing: tbl_INV_CentralStoreStoIssue values the issue at the latest weighted-average closing rate of the product/company (Close_Amt/Close_Qty of last row), producing Curr_Amt = −qty × rate; new average = closing amount/... | PAGE-420 |
| BR-M11-025 | Challan value = Σ qty × unit price (active tblUnitPrice), VAT = Σ qty × VAT/unit. | PAGE-408 |
| BR-M11-031 | Stock effect: one tblDCStore Receive row per line, StockQty = received qty (not issued), TotalQuantity = issued qty, guarded so a StockInTransfarId can create only one row. | PAGE-413 |
| BR-M11-033 | Valuation: CostPerUnit = STO unit price (excl. VAT); TotalCostAmount = STO total incl. VAT — the DC ledger cost is the transfer price, not the warehouse weighted-average cost, and is internally inconsistent. | PAGE-413 |
| BR-M11-039 | Valuation: price and VAT per unit are those of the DC row's original STO line; totals are computed on the issued quantity, so TotalPrice/TotalVAT/TotalAmount overstate value when qty is short. | PAGE-414 |
| BR-M12-009 | Chalan valuation: per line Value = UnitPrice x Qty; Vat = Qty x VATAmountPerUnit; ValueWVat = Value + Vat; header TotalValue/TotalVat/GrandTotal are the sums | CONFIRMED |
| BR-M12-014 | Sub-depot receive creates one NEW tblSubDepotStore row per chalan line: StockQty = received qty, TotalQuantity = quantity sent, DamageQty = damaged qty, MfgDate/ExpDate f | CONFIRMED |
| BR-M12-016 | Damaged/un-received qty > 0 on receive creates a freeze row (INSERT tblSubDepotStoreFreeze; StockCondition 'Restricted'; StockQty = TotalQuantity = damage qty; SubDCStore | CONFIRMED |
| BR-M12-032 | Sub-depot payable invoices: tblSubInvoiceMaster with DeliveryTpGrandTotal>0, matching ComUnitId and MarketId, PaymentStatus NULL or 'Partial', DelivaryInvoiceNo NOT NULL  | CONFIRMED |
| BR-M13-017 | 464 | GetSalesChartData and GetOrderChartData build one column per selected month (MMM-yyyy) and a "Total" column that is the SUM of the month values DIVIDED by the number of s | CONFIRMED |
| BR-M13-021 | 531, 494, 493 | Invoice (proforma) KPIs: NumberofProformaInvoice = COUNT(DISTINCT InvoiceId), SumofNetProformaAmount = SUM(NetAmount) - SUM(TotalPriceVatAmount) (net of VAT), ProTpVat =  | CONFIRMED |
| BR-M13-022 | 531, 494, 493 | Sales (delivered) KPIs: only DeliveryInvoiceStatus IN ('Partial','Full') AND TpGrandTotal>0 AND UpdateDate BETWEEN dates; NumberofInvoiceSold = COUNT(DISTINCT InvoiceId), | CONFIRMED |
| BR-M13-023 | 531, 494, 493 | Return (delivery-rejection) KPIs: lines with DeliveryStatus IN ('Reject','Partial') and UpdateDate in range: SumofNetReturnAmount = (SUM(NetAmount)-SUM(TotalPriceVatAmoun | CONFIRMED |
| BR-M13-024 | 531, 536, 518, 533, 520 | Derived columns (exact formulas in inline SQL): NetInvoiceAmt = SumofNetProformaAmount+ProTpVat; NetReturnAmt = SumofNetReturnAmount+DelReTpVat; NetSalesAmt = SumofNetSal | CONFIRMED (inline SQL) / UNKNOWN (SP-fed pages) |
| BR-M13-025 | 531, 494, 493 | Undelivered invoices: DeliveryInvoiceStatus IS NULL and InvoiceDate in range -> NumberofUndelInvoice, SumofNetUnAmount (NetAmount-TotalPriceVatAmount), UnTpVat; orders: N | CONFIRMED |
| BR-M13-031 | 532 | TotalSummaryNew.LoadInfo only runs when zone/sales-center/territory legacy dropdowns are all empty (otherwise message "Please Select Date Range!!"); with the current UI t | CONFIRMED |
| BR-M13-032 | 531, 469 | On TotalSummary the Excel button (excelButton1 -> excelButton1_Click) opens BusinessSummaryViewer.aspx?fromDate&toDate in a popup, which ignores every on-screen filter an | CONFIRMED |
| BR-M13-033 | 506 | DZSMTotalSummary derives the caller hierarchy from Session (RoleTypeName/EmpInfoId/RoleTypeId) via GetHigharcyInfoByEmployeeId: AM -> AreaId list, DZSM -> RegionId list,  | CONFIRMED |
| BR-M13-034 | 506 | The grid is fed by TotalSummaryDAL.DZSMwiseLoadSummaryparm(from,to,parm) -> sp_GET_DZSMwiseReportParam_ByProcess(@fromdate,@todate,@parm) where parm is the raw ZoneSelect | CONFIRMED |
| BR-M13-038 | 495 | GP arithmetic is not in C#: the SP returns Month, Date, ComUnitName, BUWise, Particulars, SKU, Qty, OrderSenderName, RealizationAmount, VATAmount, TaxAmount, COGSAmount,  | CONFIRMED (page) / UNKNOWN (SP) |
| BR-M13-043 | 505 | Defect: TotalNoOfHqDoctorParam hard-codes ProgramTypeId="4" but tests `if (ProgramTypeId == "")`, so the program-type predicate is never added; HQ, Ex-HQ and OS doctor dr | CONFIRMED |
| BR-M13-045 | 520 | MonitoringReport shows, per scope level (SC/Zone/Area/Territory), the invoice/return/sales/outstanding block (same columns as TotalSummary) plus period roll-forward colum | CONFIRMED (wiring) / UNKNOWN (formulas) |
| BR-M13-046 | 523 | ProductWiseSalesReport (MIS) calls sp_RPT_MIS_ProductWiseSalesReport(@fromdate,@todate,@Type,@Area,@Terr,@ZonId) and displays Sales (TP/Discount/VAT/Gross), Return, "Just | CONFIRMED (wiring) / UNKNOWN (formulas) |
| BR-M13-047 | 517 | MIOWiseBusinessSummary is a copy of TotalSummaryNew2 whose only functional change is the data call LoadMIOWiseBusinessSummaryDAL(salesCenter, from, to) -> sp_RPT_MIOWiseB | CONFIRMED |
| BR-M13-048 | 535 | TotalSummaryNew_T is TotalSummaryNew2 with LoadSummaryProductcodewiseGyash__New -> sp_BusinessSummaryMISReport_TT(@fromdate,@todate,@Type,@Area,@Terr,@ZoneId) and the rec | CONFIRMED |
| BR-M13-049 | 533 | TotalSummaryNew2 ("Business Summary" in the legacy static menu) calls LoadSummaryProductcodewiseGyash(from,to,Type,Area,Terr,ZoneId) -> sp_BusinessSummaryMISReport_All_Ne | CONFIRMED |
| BR-M13-052 | 474, 475, 478, 484, 476, 487... | Company/ComUnit/Region/Zone/District/MIA/Customer sales reports aggregate View_MIAWiseSalesReport by ProductCode+Product: TotalQty = SUM(TotalQuantity), TotalAmount = SUM | CONFIRMED |
| BR-M13-056 | 488, 526 | SalesComparisone report: per district, SaleValue = SUM(DeliveryTpGrandTotal) of invoices with DeliveryInvoiceStatus IN ('Full','Partial') in the calendar month of @Report | CONFIRMED |
| BR-M13-057 | 486, 522 | ProductWiseNationalSalesReport: for @FromDate month and each of the 3 previous months per product: DeliveryTotal = SUM(DeliveryTotalQuantity), SaleValue = SUM(DeliveryNet | CONFIRMED |
| BR-M13-059 | 515 | MIAWiseSalesReport option "Total Between Date" opens MiaSalesReportViewer.aspx, which exists only as MiaSalesReportViewer.aspx.exclude (excluded from the build) - dead li | CONFIRMED |
| BR-M13-060 | 470, 471, 472, 473, 493 | BranchTotalSummary offers four Excel exports: BusinessSummaryViewer2 (delivered sales by UpdateDate per depot/day, rptBranchSales.rpt), Viewer3 (proforma by InvoiceDate,  | CONFIRMED |
| BR-M13-065 | 499 | CustomerWiseSale builds a month-by-month pivot in C# (TotalSummaryBLL.CustomerWiseSale): for each month between From and To it calls GetmonthlycustomerSaleDate(first, las | CONFIRMED |
