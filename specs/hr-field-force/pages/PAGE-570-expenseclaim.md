---
page_id: PAGE-570
module: M15
page: ExpenseClaim
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-570 — ExpenseClaim

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, reachable DAL/DAO code and page-local JS. Stored-procedure bodies are NOT in the repository (none of this module's SPs are in the root `*.sql` scripts), so SP-internal behaviour is marked `UNKNOWN`. Sections 1, 4-6, 13-15 and 28 keep the generated (static-analysis) content, which was spot-checked; sections carrying `Verified` notes were checked line by line.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-570 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | ExpenseClaim |
| Kind | Page (INFERRED from name) |
| Web folder | DoctorModule_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DoctorModule_UI_ExpenseClaim |
| aspx size | 32427 bytes |
| Code-behind LOC | 106 |
| Inline script size | 18235 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
CONFIRMED: create/edit an **expense claim**: expense date, expense type, claiming employee, amount, comments, a receipt image and the **custom field rows defined by the expense type** (field + value). List: `ExpenseClaimView.aspx` (PAGE-571); approval in `Approval_UI/ExpenseApprovalList.aspx` (cross-module); also opened from PAGE-564 with `?id=&Rid=`.

## 3. Business Objective
CONFIRMED by code: record reimbursable field expenses with supporting details and image so they can be approved and included in the monthly expense report (RPT-M15-02).

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `Approval_UI/ExpenseApprovalList.aspx`, `DoctorModule_UI/AttendanceInfoList.aspx`, `DoctorModule_UI/AttendanceInfoReport.aspx`, `DoctorModule_UI/AttendanceListApproval.aspx`, `DoctorModule_UI/ExpenseClaimView.aspx`

Outbound references: `expenseclaimview.aspx`

## 5. Route
`~/DoctorModule_UI/ExpenseClaim.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorModule_UI/ExpenseClaim.aspx |
| Code-behind | Solution.Web/DoctorModule_UI/ExpenseClaim.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | AppPrimaryDAL (Library.DAL/DoctorModule_DAL/AppPrimaryDAL.cs) |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |
| DAL class | Setup2DAL (Library.DAL/DoctorModule_DAL/Setup2DAL.cs) |
| DAO/entity class | ExpenseClaimDAOTT (Library.DAO/DoctorModule_DAO/MileageClaimDAO.cs) |
| DAO/entity class | ExpenseClaimDetailsDAO (Library.DAO/DoctorModule_DAO/ExpenseClaimDetailsDAO.cs) |
| DAO/entity class | ImagePath (Library.DAO/DoctorModule_DAO/ImagePath.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| ExpenseDate | input | Select Date | no validator found |  |  |  |  |
| ExpenseTypeIdSelect | select |  | no validator found |  |  |  |  |
| EntryBySelect | select |  | no validator found |  |  |  |  |
| ExpenseAmount | input |  | no validator found |  |  |  |  |
| Comments | textarea |  | no validator found |  |  |  |  |
| FieldNameSelect | select |  | no validator found |  |  |  |  |
| txtValue | input |  | no validator found |  |  |  |  |
| addButton | button |  | no validator found |  |  |  | onclick→PreviewExpenseDetails() |
| imageUploadForm | input |  | no validator found |  |  |  | onchange→ImageToBase64(this) |
| btnSave | button |  | no validator found |  |  |  | onclick→Save() |
| masterId | input | @ViewBag.Id | no validator found |  |  |  |  |
| imgeBase64Str | input |  | no validator found |  |  |  |  |
| masterId | input | 0 | no validator found |  |  |  |  |
| HfFieldName | input | ' + FieldNameId + ' | no validator found |  |  |  |  |
| HfRe | input | ' + txtValue + ' | no validator found |  |  |  |  |
| HfFieldName | input | ' + FieldNameId + ' | no validator found |  |  |  |  |
| HfRe | input | ' + txtRequired + ' | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ExpenseTypeIdSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| EntryBySelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| FieldNameSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
Page has no server events (`Page_Load` empty); everything is jQuery + page-methods (`ExpenseClaim.aspx:293-765`): choosing an expense type clears the detail grid and loads its fields (`GetExpenseField_ByExpenseType`); "Add to list" (`ValidationAddtoList`, `PreviewExpenseDetails`) appends field/value rows; image -> `ImageToBase64` -> hidden `imgeBase64Str`; `Validation()` then `Save()` -> `FinalSave()` posts `{ExpenseClaimID, ExpenseTypeId, EmpInfoId, Amount, ExpenseDate, Remarks, ImageBase64String, ExpenseClaimDetailsDAOs:[{ExpenseTypDetailsId, ValueText}]}` to `ExpenseClaim.aspx/Save_ExpenseClaim`; success -> `ExpenseClaimView.aspx`. Edit: `GetData(id, RId)` -> `GetExpenseClaimEditData`; **if `ApprovalStatus == "2"` the Save button is hidden unless `RId == 5`**. Amount key filter (numeric, `Number Only!`).

## 12. Backend Flow
`Save_ExpenseClaim` (`ExpenseClaim.aspx.cs`) -> `Setup2DAL.Save_ExpenseClaim(typeMaster, Session["UserId"])` (`Library.DAL/DoctorModule_DAL/Setup2DAL.cs:3842`): id>0 -> `sp_Update_ExpenseClaim(@UpdateBy)` else `sp_Save_ExpenseClaimMaster(@EntryBy)` (params `@ExpenseClaimID,@ExpenseTypeId,@ExpenseDate,@EmpInfoId,@Amount,@Remarks`); pk>0 -> image (if any) written to `<ImagePath><ImagePreName><id>.jpg` from `AppPrimaryDAL.GetImagePath("Expense")` inside `try{}catch{}`; each detail -> `sp_Save_ExpenseClaimDetails(@ExpenseTypDetailsId,@ExpenseClaimID,@ValueText)` (also on update - no visible delete). Other web-methods: `GetExpenseType`, `GetEmployeeList_Active`, `GetAreaList_Active_ByZoneId`, `GetThana_WitTagDetails(_forEditPage)` (leftovers). The `MasterId = result.Id; MasterId = 0;` lines are dead.

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| DoctorModule_UI_ExpenseClaim.GetAreaList_Active_ByZoneId | Solution.Web/DoctorModule_UI/ExpenseClaim.aspx.cs:26 | int id | ~/DoctorModule_UI/ExpenseClaim.aspx/GetAreaList_Active_ByZoneId | via BLL |
| DoctorModule_UI_ExpenseClaim.GetThana_WitTagDetails | Solution.Web/DoctorModule_UI/ExpenseClaim.aspx.cs:34 |  | ~/DoctorModule_UI/ExpenseClaim.aspx/GetThana_WitTagDetails | via BLL |
| DoctorModule_UI_ExpenseClaim.Save_ExpenseClaim | Solution.Web/DoctorModule_UI/ExpenseClaim.aspx.cs:44 | ExpenseClaimMasterDAO typeMaster | ~/DoctorModule_UI/ExpenseClaim.aspx/Save_ExpenseClaim | via BLL |
| DoctorModule_UI_ExpenseClaim.GetExpenseField_ByExpenseType | Solution.Web/DoctorModule_UI/ExpenseClaim.aspx.cs:69 | int id | ~/DoctorModule_UI/ExpenseClaim.aspx/GetExpenseField_ByExpenseType | via BLL |
| DoctorModule_UI_ExpenseClaim.GetExpenseClaimEditData | Solution.Web/DoctorModule_UI/ExpenseClaim.aspx.cs:78 | int id | ~/DoctorModule_UI/ExpenseClaim.aspx/GetExpenseClaimEditData | via BLL |
| DoctorModule_UI_ExpenseClaim.GetExpenseType | Solution.Web/DoctorModule_UI/ExpenseClaim.aspx.cs:85 |  | ~/DoctorModule_UI/ExpenseClaim.aspx/GetExpenseType | via BLL |
| DoctorModule_UI_ExpenseClaim.GetEmployeeList_Active | Solution.Web/DoctorModule_UI/ExpenseClaim.aspx.cs:93 |  | ~/DoctorModule_UI/ExpenseClaim.aspx/GetEmployeeList_Active | via BLL |
| DoctorModule_UI_ExpenseClaim.GetThana_WitTagDetails_forEditPage | Solution.Web/DoctorModule_UI/ExpenseClaim.aspx.cs:100 | int id | ~/DoctorModule_UI/ExpenseClaim.aspx/GetThana_WitTagDetails_forEditPage | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| Setup2DAL.GetAreaList_OnlyActive_ByZoneId | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:514 | sp_Get_AreaList_OnlyActive_ByZoneId | @id |
| SeedDataDAL.GetThana_All_WithTagInfo | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:406 | sp_Get_Thana_WithTagInfo |  |
| Setup2DAL.Save_ExpenseClaim | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:3843 | sp_Save_ExpenseClaimDetails, sp_Save_ExpenseClaimMaster, sp_Update_ExpenseClaim | @Amount, @EmpInfoId, @EntryBy, @ExpenseClaimID, @ExpenseDate, @ExpenseTypDetailsId, @ExpenseTypeId,  |
| AppPrimaryDAL.GetImagePath | Library.DAL/DoctorModule_DAL/AppPrimaryDAL.cs:58 | sp_Webapi_Get_ImagePath | @type |
| CommonDataLoad.GetExpenseField_ByExpenseType | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1501 | sp_Get_ExpenseDetails_ByExpenseId | @id |
| Setup2DAL.GetExpenseClaimEditData | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:2585 | sp_Get_DoctorExpenseClaim_ById | @id |
| CommonDataLoad.GetExpenseType | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:449 | sp_CS_GetExpenseType_Active |  |
| CommonDataLoad.GetEmployeeList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:467 | sp_CS_GetEmpGeneralInfo_All |  |
| Setup2DAL.GetThana_All_WithTagInfo_ForEditPage | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:2917 | sp_Get_Thana_WithTagInfo_TerritoryEdit | @id |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_GetEmpGeneralInfo_All | CommonDataLoad.GetEmployeeList_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetExpenseType_Active | CommonDataLoad.GetExpenseType | body NOT in repository | read-like name (INFERRED) |
| sp_Get_AreaList_OnlyActive_ByZoneId | Setup2DAL.GetAreaList_OnlyActive_ByZoneId | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorExpenseClaim_ById | Setup2DAL.GetExpenseClaimEditData | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ExpenseDetails_ByExpenseId | CommonDataLoad.GetExpenseField_ByExpenseType | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Thana_WithTagInfo | SeedDataDAL.GetThana_All_WithTagInfo | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Thana_WithTagInfo_TerritoryEdit | Setup2DAL.GetThana_All_WithTagInfo_ForEditPage | body NOT in repository | read-like name (INFERRED) |
| sp_Save_ExpenseClaimDetails | Setup2DAL.Save_ExpenseClaim | body NOT in repository | write-like name |
| sp_Save_ExpenseClaimMaster | Setup2DAL.Save_ExpenseClaim | body NOT in repository | write-like name |
| sp_Update_ExpenseClaim | Setup2DAL.Save_ExpenseClaim | body NOT in repository | write-like name |
| sp_Webapi_Get_ImagePath | AppPrimaryDAL.GetImagePath | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| ExpenseDate, ExpenseTypeIdSelect (not ""/0), EntryBySelect (employee), ExpenseAmount, >=1 detail row | required | client `Validation()` (`ExpenseClaim.aspx:536-598`) | `Please fill out of this field!`, `Please add to list a Row!!` | BR-M15-065 |
| ExpenseAmount | numeric characters only (keypress) | client | `Number Only!` | BR-M15-068 |
| Server | none | - | - | GAP-M15-018 |

## 19. Business Rules
| BR ID | Description | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M15-065 | Claim needs date, expense type, employee, amount and at least one detail row (client-side only). Required flag of individual detail fields (`IsRequied`) is not enforced in this page. | `ExpenseClaim.aspx:536-598` | page JS | CONFIRMED |
| BR-M15-066 | When the claim's `ApprovalStatus` is `"2"` (final approved), Save is hidden unless the caller passes `Rid=5` (approver role type); client-only lock. | `ExpenseClaim.aspx:685-700` | page JS | CONFIRMED |
| BR-M15-067 | Receipt image is saved to disk as `<prefix><ClaimId>.jpg` in the folder configured for type `Expense`; failures are swallowed and the DB save still succeeds. | `Setup2DAL.cs:3870-3895` | DAL | CONFIRMED |
| BR-M15-068 | Amount is a typed value sent as `Amount`; it is not computed from expense-type fixed amount or field values in web code; validation of fixed amount / duplicate claims: none. | `ExpenseClaim.aspx:625-660` | JS + DAL | CONFIRMED |

## 20. Permission Matrix
Session `UserId` for audit only; no role check; employee list = all active employees, so any user can file for anyone. `Rid` query parameter (approver role type) is trusted from the URL.

## 21. Status Lifecycle
Claim `ApprovalStatus` (`2` = approved/accepted; other values UNKNOWN); approval log statuses Verified/Accepted/Rejected written by `ExpenseApprovalList` (`Type "ExpanseClaim"`, MenuId 356). This page never changes status.

## 22. Error Handling
`FinalSave` `error:` handler is empty (silent failure); failed result calls `_saveError()`; image exceptions swallowed; DAL rollback + rethrow.

## 23. Audit
`@EntryBy` / `@UpdateBy` = `Session["UserId"]`.

## 24. Integration
sp_Webapi_Get_ImagePath

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: AppPrimaryDAL, CommonDataLoad, SeedDataDAL, Setup2DAL; DAO: ExpenseClaimDAOTT, ExpenseClaimDetailsDAO, ImagePath, ResultInfo; Tables (inline): —

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-570-01 | Type with 2 fields, add both rows, Save | Master + 2 details saved | BR-M15-065 |
| TC-570-02 | No detail row | `Please add to list a Row!!` | BR-M15-065 |
| TC-570-03 | Open approved claim with `Rid=2` | Save hidden | BR-M15-066 |
| TC-570-04 | Same with `Rid=5` (URL edited) | Save visible (URL-trusted) | BR-M15-066 |
| TC-570-05 | Image + unwritable folder (edge) | DB saved, no file, no error | BR-M15-067 |
| TC-570-06 | Fixed-amount type, different amount (edge) | Accepted | BR-M15-068 |

## 28. Reverse Engineering References
- `Solution.Web/DoctorModule_UI/ExpenseClaim.aspx`, `Solution.Web/DoctorModule_UI/ExpenseClaim.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | ExpenseClaim.aspx(.cs) | n/a | MISSING | - |
| Fixed amount / approval lock | None existed | client-only | UNKNOWN | ACTUAL-GAP | GAP-M15-018 |

## 30. Known Gaps
- **GAP-M15-018**; empty error handler; dead location/thana web-methods; base64 image posted as JSON (large payloads).

## 31. Change Impact
Claim tables read by `Approval_UI/ExpenseApprovalList`, `sp_RPT_MonthlyExpense` and mobile claim APIs; `Setup2DAL` shared (~40 pages).

