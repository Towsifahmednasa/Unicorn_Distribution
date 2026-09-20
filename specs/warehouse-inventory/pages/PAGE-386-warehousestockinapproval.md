---
page_id: PAGE-386
module: M10
page: WarehouseStockInApproval
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-386 — WarehouseStockInApproval

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, BLL, DAL and DAO source. Confidence tags: CONFIRMED (read directly in code), INFERRED, UNKNOWN. Stored-procedure bodies are NOT in the repository unless stated; SP effects are UNKNOWN. Sections not rewritten below were mechanically extracted and re-checked where noted.
## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-386 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | WarehouseStockInApproval |
| Kind | Approval (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_WarehouseStockInApproval |
| aspx size | 18267 bytes |
| Code-behind LOC | 307 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**Purpose (CONFIRMED):** Checker screen listing all warehouse stock-in documents in status `Posted`; an approver ticks rows, chooses **Approve** or **Reject** and submits. Also prints the stock-in report per row (`WHStockInReportViewer.aspx?reqId=`). Source: `Solution.Web/SInventory_UI/WarehouseStockInApproval.aspx.cs:16-26,124-239`.

## 3. Business Objective
Enforce maker/checker control over inbound stock documents. For material-conversion receipts the approval call also invokes the SP `sp_Acc_MaterialConversionReceive` (effect UNKNOWN - INFERRED to book the received goods). For regular receipts the code only flips the status (central-store posting code is commented out) - see BR-M10-014.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `whstockinreportviewer.aspx`

## 5. Route
`~/SInventory_UI/WarehouseStockInApproval.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/WarehouseStockInApproval.aspx |
| Code-behind | Solution.Web/SInventory_UI/WarehouseStockInApproval.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | WarehouseStockInApprovalBll (Library.BLL/SInventory_BLL/WarehouseStockInApprovalBll.cs) |
| DAL class | WarehouseStockInApprovalDal (Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs) |
| DAO/entity class | WHStockInToCenteralStore (Library.DAO/SInventory_Entities/WHStockInToCenteralStore.cs) |
| DAO/entity class | WarehouseStockInMasterDao (Library.DAO/SInventory_Entities/WarehouseStockInMasterDao.cs) |

## 7. UI Control Inventory
| Control | Label | Required | Notes |
|---|---|---|---|
| statusRadioButtonList | Take Action: `Approve` (Value 1) / `Reject` (Value 0) | Yes - `Validation()` shows `Please select an operation!!` | no default selection |
| loadGridView | Code, StockIn Date, Challan No/Date, Total Qty, Total Amt, Status, EntryBy, EntryDate, Report, select, edit | DataKeyNames `WHStockInMasterID, MaterialConversionMasterId` | source `WarehouseStockInApprovalDal.GetWarehouseStockInInformation` |
| chkSelectAll / chkSelect | select | at least one row: `Please select at least one row!!!` | header checkbox postback |
| submitButton (LinkButton -> `btnSubmit0_Click`) | Submit | - | no confirm dialog |
| cancelButton | Reset Information | - | handler body is empty |
| printButton (ImageButton) | Report | - | opens report viewer popup |
| editImageButton | (edit) | - | opens `AdvanceSalDeductionEdit.aspx?ID=` (unrelated payroll page - copy/paste artifact) |

## 8. Field Mapping
Approve: `WHStockInMasterID` (DataKeys[0]) -> `WarehouseStockInMasterDao.WHStockInMasterID`; `Status='Approved'`; `ApproveBy=Session["LoginName"]`; `ApproveDate=DateTime.Today` -> `approvalBll.ApprovalUpdateBLL` -> `WarehouseStockInApprovalDal.ApprovalUpdateDal` (`UPDATE tblWHStockInMaster SET Status,ApproveBy,ApproveDate WHERE WHStockInMasterID`). Reject: `Status = statusRadioButtonList.SelectedItem.Text` (= `Reject`). MC receipts: `ApproveWarehouseStockInMaster` -> SP `sp_Acc_MaterialConversionReceive(@ApprovalStatus,@MasterId,@ApprovalBy,@ApprovalDate)` (`WarehouseStockInApprovalDal.cs:30-41`).

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| statusRadioButtonList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| statusRadioButtonList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Handler | Effect | DB |
|---|---|---|---|
| submitButton | `btnSubmit0_Click :124` -> `Validation()` -> `SubmitApproval()` | per checked row: (approve) if `MaterialConversionMasterId>0` call SP `sp_Acc_MaterialConversionReceive`, then always `UPDATE tblWHStockInMaster`; (reject) `UPDATE ... Status='Reject'` | `tblWHStockInMaster`; SP tables UNKNOWN |
| printButton | `printButton_Click :293` | sets `Session["ReportType"]="STD"`, opens `../SInventory_RPTVIEW/WHStockInReportViewer.aspx?reqId=<id>` | none |
| cancelButton | `cancelButton_Click :27` | empty | none |

## 11. Frontend Flow
Init: `WarehouseStockInInformation()` binds the `Posted` list (`:16-26`). After every submit the grid is reloaded (`:135`). Alerts: `Stock Approved Successfully!!`, `Stock Rejected Successfully!!!`. No page JS.

## 12. Backend Flow
`SubmitApproval` (`:170-239`):
- **Approve** (radio text != `Reject`): loop rows; for each checked row build DAO (`Status="Approved"`, `ApproveBy`, `ApproveDate=Today`); `MaterialConversionCount = Convert.ToInt32(DataKeys[i][1].ToString())`; if `>0` -> `ApproveWarehouseStockInMaster` (SP `sp_Acc_MaterialConversionReceive`); then `ApprovalUpdateBLL` -> UPDATE status. Central-store insert (`PrepareData` + `InsertDataIntoCenterlWarehouse`) is **commented out** (`:197-204`).
- **Reject**: `Status = "Reject"`, same audit fields, UPDATE only.
- No transaction; the SP call and the status UPDATE are separate. No try/catch. `DataKeys[i][1]` for a regular stock-in is the NULL `MaterialConversionMasterId` (inserts in `SaveStockInMasterInformation` never set it) -> `DBNull.ToString()==""` -> `Convert.ToInt32("")` FormatException unless the DB default is 0 (UNKNOWN).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| WarehouseStockInApprovalBll.LoadWarehouseStockInInformation | Library.BLL/SInventory_BLL/WarehouseStockInApprovalBll.cs:28 |
| WarehouseStockInApprovalBll.GetMenuIdByMenuName | Library.BLL/SInventory_BLL/WarehouseStockInApprovalBll.cs:36 |
| WarehouseStockInApprovalBll.GetAssignedAppUser | Library.BLL/SInventory_BLL/WarehouseStockInApprovalBll.cs:32 |
| WarehouseStockInApprovalBll.ApproveWarehouseStockInMaster | Library.BLL/SInventory_BLL/WarehouseStockInApprovalBll.cs:46 |
| WarehouseStockInApprovalBll.ApprovalUpdateBLL | Library.BLL/SInventory_BLL/WarehouseStockInApprovalBll.cs:40 |
| WarehouseStockInApprovalBll.LoadWarehouseStockInInfo | Library.BLL/SInventory_BLL/WarehouseStockInApprovalBll.cs:59 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| WarehouseStockInApprovalDal.GetWarehouseStockInInformation | Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs:28 | inline SQL: tblManufacturer, tblWHStockInMaster |  |
| WarehouseStockInApprovalDal.GetMenuIdByMenuName | Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs:54 | inline SQL: tblMainMenu |  |
| WarehouseStockInApprovalDal.GetAssignedAppUser | Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs:49 | inline SQL: tblAppSetup |  |
| WarehouseStockInApprovalDal.ApproveWarehouseStockInMaster | Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs:36 | sp_Acc_MaterialConversionReceive | @ApprovalBy, @ApprovalDate, @ApprovalStatus, @MasterId |
| WarehouseStockInApprovalDal.ApprovalUpdateDal | Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs:59 | inline SQL: tblWHStockInMaster |  |
| WarehouseStockInApprovalDal.GetLoadWarehouseStockInInfo | Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs:67 | inline SQL: tblProduct, tblWHStockInDetail, tblWHStockInMaster |  |

## 16. Stored Procedure Mapping
| SP | Called from | Definition | Effect |
|---|---|---|---|
| `sp_Acc_MaterialConversionReceive` (`@ApprovalStatus, @MasterId, @ApprovalBy, @ApprovalDate`) | `WarehouseStockInApprovalDal.ApproveWarehouseStockInMaster :30-41` via `ClsCommonInternalDAL.UpdateAction` | body NOT in repository | UNKNOWN (name suggests accounting/inventory posting of a material-conversion receipt) |
| `ExecuteAllSqlQueryByStoreProcedure` | status UPDATE, queue SELECT | absent | generic |

## 17. Database Mapping
R `tblWHStockInMaster` LEFT JOIN `tblManufacturer` (`Status='Posted'`); W `tblWHStockInMaster.Status/ApproveBy/ApproveDate`. Unused helper queries: `tblAppSetup` (SL, UserId) and `tblMainMenu` (URL LIKE) in `GetAssignedAppUser/GetMenuIdByMenuName`; `tblCentralStore` INSERT in `SaveWHStockIntoCenterlWarehouse` (dead - never called).

## 18. Validation Matrix
| Field | Rule | Backend | Message | BR |
|---|---|---|---|---|
| Action radio | required | `Validation :143` | Please select an operation!! | BR-M10-013 |
| Row selection | >=1 checked | `Validation :150-165` | Please select at least one row!!! | BR-M10-013 |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M10-012 | The approval queue contains only stock-in masters with `Status='Posted'` (rejected items are excluded). | `Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs:22-27` | `GetWarehouseStockInInformation` | CONFIRMED |
| BR-M10-013 | An action (Approve/Reject) and at least one checked row are required. Approve writes `Status='Approved'`; Reject writes `Status='Reject'`; both stamp `ApproveBy=Session["LoginName"]`, `ApproveDate=Today`. | `Solution.Web/SInventory_UI/WarehouseStockInApproval.aspx.cs:124-239` | `Validation`, `SubmitApproval` | CONFIRMED |
| BR-M10-014 | Approving a stock-in linked to a material conversion (`MaterialConversionMasterId>0`) first calls SP `sp_Acc_MaterialConversionReceive`. Approving a regular stock-in only changes status: the code that would copy lines into `tblCentralStore` is commented out, so central stock is not created here (whatever posts stock afterwards is not in the repository). | `WarehouseStockInApproval.aspx.cs:191-206`; `WarehouseStockInApprovalDal.cs:30-41,77-113` | `SubmitApproval` | CONFIRMED (code); downstream effect UNKNOWN |
| BR-M10-015 | Approver entitlement is NOT enforced: the `DataTrue()` check against `tblMainMenu`/`tblAppSetup` is commented out; any user able to open the page can approve/reject and may approve their own entries (no maker != checker test). | `WarehouseStockInApproval.aspx.cs:20-24,53-79` | - | CONFIRMED |

## 20. Permission Matrix
Only `Session["LoginName"]` is read (audit). `tblAppSetup` (approver assignment per menu) is loaded by `DataTrue()` but the call is commented (`:20`). Effective control = DB-driven menu visibility only.

## 21. Status Lifecycle
`Posted` -> `Approved` (`:187`) or `Reject` (`:227`); no reversal here. `Reject` documents can be edited/resubmitted from the View page (which lists `Posted`/`Reject`); resubmission sets `Posted` again.

## 22. Error Handling
No try/catch; SP failure or format exception propagates as an unhandled error. Success messages are shown even if an SP/UPDATE silently returned 0 rows (return values ignored).

## 23. Audit
`ApproveBy`, `ApproveDate` (date only, `DateTime.Today`) on `tblWHStockInMaster`; SP-side audit UNKNOWN.

## 24. Integration
Cross-DB effect possible through `sp_Acc_MaterialConversionReceive` (material conversion data lives in `CSTL_ERPDB_Unicorn`); content UNKNOWN.

## 25. Reports
`SInventory_RPTVIEW/WHStockInReportViewer.aspx` -> `crpWHStockIn.rpt` (RPT-M10-03).

## 26. Dependencies
`WarehouseStockInApprovalBll/Dal`, `WarehouseStockInMasterDao`, `WHStockInToCenteralStore` (dead), `ClsCommonInternalDAL.UpdateAction`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-386-01 | Open page | grid shows only `Posted` masters | BR-M10-012 |
| TC-386-02 | Submit without action | `Please select an operation!!` | BR-M10-013 |
| TC-386-03 | Action but no row | `Please select at least one row!!!` | BR-M10-013 |
| TC-386-04 | Approve a material-conversion stock-in | SP `sp_Acc_MaterialConversionReceive` invoked, then Status `Approved` | BR-M10-014 |
| TC-386-05 | Approve a regular stock-in | Status `Approved`; no `tblCentralStore` insert from this code; verify FormatException risk on NULL `MaterialConversionMasterId` | BR-M10-014 |
| TC-386-06 | Reject | Status `Reject`; row appears in View list | BR-M10-013 |
| TC-386-07 | Same user creates and approves | allowed | BR-M10-015 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/WarehouseStockInApproval.aspx`, `Solution.Web/SInventory_UI/WarehouseStockInApproval.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual | Status | Reference |
|---|---|---|---|---|
| Prior spec | None existed | - | MISSING | - |
| Stock posting on approval | None existed | status only (regular) | UNKNOWN | `WarehouseStockInApproval.aspx.cs:197-204` |

## 30. Known Gaps
- GAP-M10-003: approving a regular stock-in does not create central stock in code; downstream mechanism unknown - stock-out (`WarehouseStockOut`) joins `tblCentralStore.MigoDetailID = WHStockInDetailID` and would show empty stock if nothing posts it.
- Possible `FormatException` for NULL `MaterialConversionMasterId`.
- No maker!=checker or approver-assignment control (BR-M10-015).
- Edit icon opens unrelated `AdvanceSalDeductionEdit.aspx` (copy/paste).
- No transactions; SP body missing.

## 31. Change Impact
Changing `ApprovalUpdateDal` affects PAGE-386 only; `ApproveWarehouseStockInMaster` shares the SP with material conversion. Queue depends on PAGE-385/370 setting `Posted`.

