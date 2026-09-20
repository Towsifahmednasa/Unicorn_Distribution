---
page_id: PAGE-694
module: M17
page: ProviderDropoutRequestList
tier: A
generated: false   # upgraded by manual code review (Tier A)
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "COMPLETE", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-694 — ProviderDropoutRequestList

> **Evidence level:** Tier A - reviewed manually against `.aspx`, `.aspx.cs`, and the BLL/DAL code they reach (review date 2026-09-19). Claims are tagged CONFIRMED / INFERRED / UNKNOWN. Stored-procedure bodies are NOT in the repository unless a section says so; column-level effects of such SPs stay `UNKNOWN`. Sections 1, 4-10, 13-15 and 28 still carry the mechanically extracted content (re-checked where noted); all other sections were rewritten.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-694 |
| Module | M17 — SAP, Mobile-App & External Integration (INFERRED from folder + file name) |
| Page name | ProviderDropoutRequestList |
| Kind | List / View (INFERRED from name) |
| Web folder | eProgram_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | eProgram_UI_ProviderDropoutRequestList |
| aspx size | 10321 bytes |
| Code-behind LOC | 152 |
| Inline script size | 7578 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
"Provider Dropout List": a jQuery/DataTables screen listing **dropout requests for e-Program providers** (provider code/name, program, mobile, NID, email, outlet, dropout reason, inserted-at UTC) that arrive from an external system into `tblProviderDropoutIntrigration`; a user presses **Approve** per row. Approved rows show a disabled "Approved" button and the approver/UTC approval date. CONFIRMED: `Solution.Web/eProgram_UI/ProviderDropoutRequestList.aspx(.cs)`, `Library.DAL/SAP_IntegrationDAL/sql/*.sql`.

## 3. Business Objective
Give the ERP an approval gate over provider (e-Program participant, INFERRED: program-based outlet/health providers) dropout notifications received from an external program system. No downstream ERP action after approval is coded in the repo (UNKNOWN).

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/eProgram_UI/ProviderDropoutRequestList.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/eProgram_UI/ProviderDropoutRequestList.aspx |
| Code-behind | Solution.Web/eProgram_UI/ProviderDropoutRequestList.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | SAP_IntrigationPointDAL (Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
No server/HTML controls with an ID were extracted (viewer-only or code-generated page).

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- `document.ready`: disables DataTables error alerts, calls `loadProviderDropoutList()` -> `$.ajax POST ProviderDropoutRequestList.aspx/GetProviderDropoutIntrigrationList` (timeout 30 s, JSON `{}`); response `d = {isSuccess, message, data[]}`; renders `<tr>` per item with HTML-escaped values (`safeValue`); Approve button only when `isApprove` is false; then `bindDropoutGrid()` (DataTable).
- Approve click (`.btn-approve-dropout`): reads `data-id`, `approveProviderDropout(id,$btn)` -> `POST .../ApproveProviderDropoutIntrigration {providerIDropoutIntrigrationd:id}`; on success sets button to disabled "Approved", `ShowSuccesalert`, reload list after 150 ms; on fail re-enables button and `faildalert`.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| eProgram_UI_ProviderDropoutRequestList.Page_Load | Solution.Web/eProgram_UI/ProviderDropoutRequestList.aspx.cs:12 | object sender, EventArgs e | eProgram_UI_ProviderDropoutRequestList.Page_Load | no | no |
| eProgram_UI_ProviderDropoutRequestList.GetProviderDropoutIntrigrationList | Solution.Web/eProgram_UI/ProviderDropoutRequestList.aspx.cs:18 |  | eProgram_UI_ProviderDropoutRequestList.GetProviderDropoutIntrigrationList → SAP_IntrigationPointDAL.GetProviderDropoutIntrigrationListDAL → SP: sp_Get_ProviderDropoutIntrigrationList | yes | no |
| eProgram_UI_ProviderDropoutRequestList.ApproveProviderDropoutIntrigration | Solution.Web/eProgram_UI/ProviderDropoutRequestList.aspx.cs:97 | long providerIDropoutIntrigrationd | eProgram_UI_ProviderDropoutRequestList.ApproveProviderDropoutIntrigration → SAP_IntrigationPointDAL.ApproveProviderDropoutIntrigrationDAL → SP: sp_Up_ProviderDropoutIntrigrationApprove | yes | yes |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| eProgram_UI_ProviderDropoutRequestList.GetProviderDropoutIntrigrationList | Solution.Web/eProgram_UI/ProviderDropoutRequestList.aspx.cs:18 |  | ~/eProgram_UI/ProviderDropoutRequestList.aspx/GetProviderDropoutIntrigrationList | via BLL |
| eProgram_UI_ProviderDropoutRequestList.ApproveProviderDropoutIntrigration | Solution.Web/eProgram_UI/ProviderDropoutRequestList.aspx.cs:97 | long providerIDropoutIntrigrationd | ~/eProgram_UI/ProviderDropoutRequestList.aspx/ApproveProviderDropoutIntrigration | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SAP_IntrigationPointDAL.GetProviderDropoutIntrigrationListDAL | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:82 | sp_Get_ProviderDropoutIntrigrationList |  |
| SAP_IntrigationPointDAL.ApproveProviderDropoutIntrigrationDAL | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:613 | sp_Up_ProviderDropoutIntrigrationApprove | @ApproveBy, @providerIDropoutIntrigrationd |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_ProviderDropoutIntrigrationList | SAP_IntrigationPointDAL.GetProviderDropoutIntrigrationListDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Up_ProviderDropoutIntrigrationApprove | SAP_IntrigationPointDAL.ApproveProviderDropoutIntrigrationDAL | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M17-015 | **Approve rule.** `sp_Up_ProviderDropoutIntrigrationApprove(@providerIDropoutIntrigrationd BIGINT, @ApproveBy NVARCHAR(50))` sets `IsApprove = 1`, `IsApproveBy = @ApproveBy` (= `Session["UserId"]` as string), `ApproveDate = SYSUTCDATETIME()` **only where the row exists and `ISNULL(IsApprove,0)=0`**. Approving an already-approved or non-existent id updates 0 rows but the SP does not signal it, so the page reports "Approved successfully." (idempotent, no error). | `Library.DAL/SAP_IntegrationDAL/sql/sp_Up_ProviderDropoutIntrigrationApprove.sql:6-21`; `ProviderDropoutRequestList.aspx.cs:97-124`; DAL `SAP_IntrigationPointDAL.cs:612-639` | SP (in repo) + code-behind | CONFIRMED |
| BR-M17-016 | **List rule.** `sp_Get_ProviderDropoutIntrigrationList` returns all rows of `tblProviderDropoutIntrigration` (no paging, no filter) ordered by `insertedAt DESC, providerIDropoutIntrigrationd DESC`, with `IsApprove` defaulted to 0. Approval columns are added by `alter_tblProviderDropoutIntrigration_AddApprovalColumns.sql` (`IsApprove BIT NOT NULL DEFAULT 0`, `IsApproveBy NVARCHAR(50) NULL`, `ApproveDate DATETIME2 NULL`, idempotent `COL_LENGTH` guards). Dates shown as `dd-MMM-yyyy hh:mm tt` **without timezone conversion** (headers say UTC). | `sp_Get_ProviderDropoutIntrigrationList.sql:6-27`; `alter_...sql`; `ProviderDropoutRequestList.aspx.cs:34-70` | SP + code-behind | CONFIRMED |

## 20. Permission Matrix
- Session keys: `UserId` (approve only, read inside the DAL: `HttpContext.Current.Session["UserId"]`; the web method has `EnableSession=true`). If no session, the DAL throws NullReferenceException -> caught -> message = exception text.
- **The list method `GetProviderDropoutIntrigrationList` is `[WebMethod]` without `EnableSession` and with no auth check**: provider PII (mobile no, NID, email) is returned to any HTTP client that can POST to the page-method URL (INFERRED reachability; `Web.config` has no `<authorization>` deny).
- No role check for approval.

## 21. Status Lifecycle
`IsApprove`: `0` (pending, default) -> `1` (approved) via `sp_Up_ProviderDropoutIntrigrationApprove`. No un-approve or reject path in the repo. Who inserts rows into `tblProviderDropoutIntrigration` (external system, `insertedAt` set by inserter) - UNKNOWN (no insert code in repo). What happens after approval (e.g. deactivate provider/notify SAP) - UNKNOWN.

## 22. Error Handling
- Server: web-method try/catch returns `{isSuccess:false, message: ex.Message}` (raw exception text to the browser).
- Client: `fail()` handlers -> `faildalert("Request failed."/"Request timeout."/"Approve request failed.")`; empty data => empty table without message; DataTable error mode `none`.

## 23. Audit
Audit columns proven: `IsApproveBy`, `ApproveDate (UTC)`, `insertedAt` (inserter-set). No audit log table.

## 24. Integration
**Inbound external integration** (rows are created outside the repo) - see `specs/integrations/INT-06-provider-dropout-inbound.md`.

## 25. Reports
None (screen only).

## 26. Dependencies
BLL: —; DAL: SAP_IntrigationPointDAL; DAO: ResultInfo; Tables (inline): —

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-694-01 | Positive | Open page with 3 pending rows | 3 rows with blue Approve buttons, newest first | BR-M17-016 |
| TC-694-02 | Positive | Approve one row | SP sets IsApprove=1/IsApproveBy=UserId/ApproveDate=UTC; button becomes disabled Approved; list reloads | BR-M17-015 |
| TC-694-03 | Edge | Approve the same id twice (two tabs) | second call updates 0 rows, still reports success | BR-M17-015 |
| TC-694-04 | Negative | POST approve with non-existent id | 0 rows, reports success | BR-M17-015 |
| TC-694-05 | Negative | POST list method without cookie | returns data (no check) - verify | permission |
| TC-694-06 | Edge | Empty table | empty grid, no message | BR-M17-016 |
| TC-694-07 | Security | Reason text with `<script>` | escaped by `safeValue` | - |

## 28. Reverse Engineering References
- `Solution.Web/eProgram_UI/ProviderDropoutRequestList.aspx`, `Solution.Web/eProgram_UI/ProviderDropoutRequestList.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | `ProviderDropoutRequestList.aspx(.cs)` | SP scripts in repo | MISSING | - |
| DB objects | None existed | uses `tblProviderDropoutIntrigration` | table create script not in repo (only ALTER for 3 columns) | PARTIAL | GAP-M17-006 |

## 30. Known Gaps
- Unauthenticated PII list endpoint (INFERRED); no per-role approval control.
- Idempotent approve masks non-existent ids; no rejection path.
- Table DDL (base columns `providerIDropoutIntrigrationd, providerId, programShortName, providerCode, providerName, mobileNo, nid, email, outlet, dropoutReason, insertedAt`) inferred only from the SELECT list; creation script absent; inserter unknown.
- Raw exception messages returned to client; whole table returned (no paging) then rendered client-side.
- Misspelled names ("Intrigration") baked into DB objects.

## 31. Change Impact
`SAP_IntrigationPointDAL` shared with SAP pages. SP names/columns are also consumed by any external system that inserts into the table (contract UNKNOWN).

