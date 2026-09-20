---
page_id: PAGE-099
module: M03
page: MarketStructure_TransferApprove
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-099 — MarketStructure_TransferApprove

> **Evidence level:** Tier A. `MarketStructure_TransferApprove.aspx(.cs)` (all 650 lines) and `MarketStructureTransferDAL.GetList/UpdateMarketStructure_TransferApprove` read. SP bodies absent, therefore *what approval actually changes* is `UNKNOWN`. See `workflows/WF-M03-02-market-structure-transfer-approval.md`.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / Module | PAGE-099 / M03 |
| Name | MarketStructure_TransferApprove (breadcrumb "Transfer Approval List", `.aspx:26`) |
| Kind | Approval (list with multi-select approve) |
| Folder / master | TransferUI / NewMasterPage.master |
| Class | `TransferUI_MarketStructure_TransferApprove`; 650 lines (≈60% is duplicated select-all handlers) |

## 2. Purpose
CONFIRMED: shows pending structure-transfer requests of one kind (Market, Sub-Territory, Territory, Area or Zone) chosen by a radio list, and lets the approver tick rows and approve them.

## 3. Business Objective
INFERRED: second-step control over hierarchy changes raised on PAGE-095/096/100/101/102 so that a structure change becomes effective only after approval.

## 4. Navigation
Menu path UNKNOWN. Reset redirects to `Customer_Doctor_Transfer.aspx` (`.cs:570`, unrelated page — likely copy-paste). Success reloads itself (`.cs:412`).

## 5. Route
`~/TransferUI/MarketStructure_TransferApprove.aspx`

## 6. Source Files
`Solution.Web/TransferUI/MarketStructure_TransferApprove.aspx(.cs)`; registered-but-hidden controls `~/MasterSetup_UI/IVMarketStructureFrom_To.ascx` and `IVMarketStructureTo.ascx` (inside `<div runat="server" visible="false">`, `.aspx:99`, so never rendered but still instantiated and their `Page_Load` still runs — role-scoped `GetHigharcyInfoByEmployeeId` DB call happens on every request); `Library.DAL/Transfer_DAL/MarketStructureTransferDAL.cs` (`GetList` 18-37, `UpdateMarketStructure_TransferApprove` 153-197); `CommonDataLoad.GetValidationCheck`.

## 7. UI Control Inventory
| Control | Label | Required | Default | State | Source | Message |
|---|---|---|---|---|---|---|
| `txtProbationEndDate` | "Effective Date" (pickadate) | **not used** — never read in code-behind | empty | enabled | – | – |
| `rbType` (RadioButtonList, AutoPostBack) | Market Transfer / Sub-Territory Transfer / Territory Transfer / Area Transfer / Zone Transfer (values `Market`, `Sub-Territory`, `Territory`, `Area`, `Zone`) | one selected | "Market" | – | static | – |
| `gv_market` | column "Market" (`MarketName`) | – | – | key `MarketStructureTranferId` | `sp_Get_MSList_Approve(@Parm=Market)` | – |
| `gv_subTerri` | "Sub-Territory" (`TSubTerritoryName`) | – | – | same key | `@Parm=Sub-Territory` | – |
| `gv_Terrritory` | "Territory" (`TTerritoryName`) | – | – | same | `@Parm=Territory` | – |
| `gv_Area` | "Area" (`TAreaName`) | – | – | same | `@Parm=Area` | – |
| `gv_Zone` | "Zone" (`TRegionName`) | – | – | same | `@Parm=Zone` | – |
| each grid: `chkSelect` per row + header select-all (`gv_*chkSelectAll`, AutoPostBack) | SL#, checkbox | ≥1 | unchecked | – | – | – |
| `btnSearch` | Search | – | – | in hidden block, effectively unreachable | – | – |
| `btnSave` | Submit | – | – | `sweetAlertConfirm_Submit` | – | – |
| `btnReset` | Reset | – | – | – | – | – |
Only the *destination* level name is shown (no source parent, no destination parent, no requester, no date) — the approver cannot see from/to context in this page.

## 8. Field Mapping
| UI | Code | Param | Column |
|---|---|---|---|
| `rbType` selected value | `Type = rbType.Items[i].Value` (`.cs:290,314,338,361,384`) | `@Type` (approve) / `@Parm` (list) | UNKNOWN |
| ticked row key `MarketStructureTranferId` | `Convert.ToInt32(gv.DataKeys[i][0])` (variable named `EmpId`) → CSV | `@MasterId` (one SP call per id) | UNKNOWN |
| Session UserId | `.cs:409` | `@ApprovedBy` | UNKNOWN |

## 9. Dropdown Mapping
No dropdowns (radio list is static; hidden From/To controls unused).

## 10. Button Action Mapping
| Button | Handler | Chain | SP | Change |
|---|---|---|---|---|
| (page load / `rbType` change) | `Page_Load` → `btnSearch_Click(null,null)` (`.cs:41-44`), `rbType_SelectedIndexChanged` (`.cs:59`) | `MarketStructureTransferDAL.GetList(type)` | `sp_Get_MSList_Approve(@Parm)` | read |
| Submit | `btnSave_Click` `.cs:279` | `Validation()` → `UpdateMarketStructure_TransferApprove(type, csv, user)` | `sp_Update_MarketStructure_Approve` × N | write (UNKNOWN) |
| header check-all ×5 | `gv_*chkSelectAll_CheckedChanged` `.cs:573-649` | in-memory | – | – |
| Reset | `btnReset_Click` `.cs:568` | redirect `Customer_Doctor_Transfer.aspx` | – | – |
Dead code: `chkSelect_CheckedChanged` + `CheckCustInOrder` (calls `GetValidationCheck(id,"CustomerTransfer")` and un-ticks with "Order Pending!") are not wired to any control in the aspx (`chkSelect` has no event), and `chkSelectAll_CheckedChanged` (`.cs:573`) references header id `chkSelectAll` that does not exist.

## 11. Frontend Flow
UpdatePanel; `rbType` AutoPostBack reloads the relevant grid; other four grids are re-bound to `null` first (`.cs:428-443`) so only one grid shows. Header checkbox postbacks tick/untick all rows. `pageLoad()` initialises pickadate/select2. No AJAX/JS logic.

## 12. Backend Flow
1. `Page_Load` (first load) → `btnSearch_Click` → clears the five grids → for the selected radio item calls `GetList(value)` and binds the matching grid. Whole block wrapped in `try { } catch (Exception ex) { }` (`.cs:448-507`) — DB errors silently give an empty grid.
2. `btnSave_Click` → `Validation()` (see §18) → for the selected radio: iterate that grid's rows, collect `MarketStructureTranferId` of ticked rows into CSV → `Res = UpdateMarketStructure_TransferApprove(Type, CSV, UserId)`.
3. DAL (`MarketStructureTransferDAL.cs:153-197`): if CSV non-empty, split by ","; for each non-empty id call `accessManager.UpdateData("sp_Update_MarketStructure_Approve", {@MasterId,@ApprovedBy,@Type})`. `isSuccess` = last call's result. One implicit transaction for the whole batch; exception → rollback + rethrow (page has no try/catch around it).
4. Success → `successalert('Operation successful!','Success','MarketStructure_TransferApprove.aspx')` else `faildalert('Operation Faild!')`.

## 13. API Mapping
None.

## 14. Service Mapping
No BLL (DAL called directly; static `MarketStructureTransferDAL` instance `.cs:20`).

## 15. Repository Mapping
| DAL method | Source | SP | Params |
|---|---|---|---|
| `GetList(string Parm)` | MarketStructureTransferDAL.cs:18 | `sp_Get_MSList_Approve` | `@Parm` |
| `UpdateMarketStructure_TransferApprove` | MarketStructureTransferDAL.cs:153 | `sp_Update_MarketStructure_Approve` | `@MasterId,@ApprovedBy,@Type` |
| `CommonDataLoad.GetValidationCheck` (dead path) | CommonDataLoad.cs:1581 | `sp_check_Vali_MarketStructure` | `@MasterId,@PageName` |

## 16. Stored Procedure Mapping
`sp_Get_MSList_Approve` (read; returns `MarketStructureTranferId` + `MarketName`/`TSubTerritoryName`/`TTerritoryName`/`TAreaName`/`TRegionName` — from the grid bindings), `sp_Update_MarketStructure_Approve` (write; the step that most likely applies or finalises the re-parenting — UNKNOWN), `sp_check_Vali_MarketStructure` (read, dead path). Bodies NOT in repository.

## 17. Database Mapping
No inline SQL. Tables UNKNOWN (transfer-request table keyed by `MarketStructureTranferId`).

## 18. Validation Matrix
| Rule | Server condition | Message | BR |
|---|---|---|---|
| Grid must have rows | `gv.Rows.Count == 0` for the selected type (`.cs:81,118,155,192,230`) | "Table can not be Empty!" (`alert`) | BR-M03-020 |
| ≥1 row ticked | loop `chkSelect.Checked` (`.cs:91-110` etc.) | "Please Select at least one row !!!" | BR-M03-019 |
| Effective date | **not validated / not used** | – | BR-M03-022 |
| Approver ≠ requester | not checked | – | BR-M03-023 |

## 19. Business Rules
| ID | Rule | Source | Impl | Conf |
|---|---|---|---|---|
| BR-M03-019 | At least one request row must be ticked to approve. | MarketStructure_TransferApprove.aspx.cs:91-110,126-147,163-184,200-221,238-259 | `Validation()` | CONFIRMED |
| BR-M03-020 | An empty list blocks approval. | .cs:81,118,155,192,230 | `Validation()` | CONFIRMED |
| BR-M03-021 | Approval is per request row: one `sp_Update_MarketStructure_Approve` call per ticked `MarketStructureTranferId` with `@Type` = radio value, `@ApprovedBy` = session UserId; all calls in one transaction; reported result = last call. | MarketStructureTransferDAL.cs:161-179; DataAccessManager.cs | DAL | CONFIRMED |
| BR-M03-022 | The "Effective Date" input is decorative — its value never reaches the DAL/SP. | .aspx:59; whole .cs | – | CONFIRMED |
| BR-M03-023 | No segregation of duties: the same user can raise (PAGE-095/096/100/101/102) and approve; no role/level check in code. | .cs:409; no Session role test | – | CONFIRMED (absence) |
| BR-M03-024 | Approval list is per transfer type: radio values `Market`,`Sub-Territory`,`Territory`,`Area`,`Zone` are passed verbatim as `@Parm`/`@Type`; only one type can be approved per submit. | .aspx:72-76; .cs:450-504 | page | CONFIRMED |
| BR-M03-025 | List loads on first page load and on radio change; no filtering by date/requester/status in C#. | .cs:41-44,59-65 | page | CONFIRMED |
| BR-M03-026 | There is **no reject/cancel** action; a request leaves the list only via approve (or by whatever the SP does). | whole page | – | CONFIRMED (absence) |

## 20. Permission Matrix
Session read: `UserId` (unguarded). Hidden controls read `RoleTypeName/EmpInfoId/RoleTypeId` (try/catch) and call `GetHigharcyInfoByEmployeeId` — for AM/DZSM/NSM/MIO roles `dtMarket.Rows[0]` is accessed unguarded (`IVMarketStructureFrom_To.ascx.cs:113-138`), so a user with that role but no hierarchy row would throw on **every load of this page** (INFERRED). No role gate for approvers; menu-only access; no `<authorization>` in Web.config.

## 21. Status Lifecycle
Not stored/displayed on the page. INFERRED lifecycle of a transfer request: `Requested (pending)` → `Approved`. No reject/cancel/expiry. The list SP presumably returns only pending rows (rows disappear after approval on reload — INFERRED). See WF-M03-02.

## 22. Error Handling
List load: empty catch. Save: validation alerts; DAL exceptions unhandled; `UpdateMarketStructure_TransferApprove` sets `isSuccess=false`+`ErrorMessage` then rethrows.

## 23. Audit
`@ApprovedBy` = session UserId; timestamp presumably inside SP (UNKNOWN). The unused "Effective Date" suggests an intended effective-dating that is not implemented.

## 24. Integration
None visible.

## 25. Reports
None.

## 26. Dependencies
Depends on rows produced by PAGE-095/096/100/101/102; hidden IVMarketStructure controls; `CommonDataLoad`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Open page | Market list loaded (default radio) | BR-M03-025 |
| TC-02 | Switch radio to Zone | only zone grid shown | BR-M03-024 |
| TC-03 | Submit with empty grid | alert "Table can not be Empty!" | BR-M03-020 |
| TC-04 | Submit with no ticks | alert "Please Select at least one row !!!" | BR-M03-019 |
| TC-05 | Tick 3, submit | 3 approve calls, one txn, success alert, list reload | BR-M03-021 |
| TC-06 | Enter effective date, submit | date ignored | BR-M03-022 |
| TC-07 | Same user requests then approves | allowed | BR-M03-023 |
| TC-08 | DB error on list | empty grid, no message | §22 |
| TC-09 | Click Reset | goes to Customer_Doctor_Transfer.aspx | §4 |

## 28. Reverse Engineering References
`Solution.Web/TransferUI/MarketStructure_TransferApprove.aspx(.cs)`, `MarketStructureTransferDAL.cs:18-37,153-197`, `MasterSetup_UI/IVMarketStructureFrom_To.ascx(.cs)`.

## 29. Spec vs Actual
| Area | Spec | Actual | DB | Status | Ref |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Effective date | None existed | field present, unused | – | DEFECT-SUSPECT | BR-M03-022 |
| Approval effect | None existed | delegated to SP | absent | UNKNOWN | §16 |

## 30. Known Gaps
No reject; no segregation of duties; effective date unused; approver sees only destination name (no context); Reset goes to unrelated page; hidden controls run role queries and can throw; ~350 lines duplicated select-all code; dead `CheckCustInOrder`; `try/catch` swallowing; SP bodies absent (GAP-M03-001).

## 31. Change Impact
Coupled to the five transfer pages via `MarketStructureTranferId`/`@Type` values; `sp_Update_MarketStructure_Approve`/`sp_Get_MSList_Approve` referenced only here (grep). `GetValidationCheck` is shared with TerritoryWiseDepotSetup and Customer_Doctor_Transfer.
