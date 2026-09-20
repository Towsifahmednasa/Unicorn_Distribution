---
page_id: PAGE-363
module: M10
page: DcStockOutApproval
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-363 — DcStockOutApproval

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, BLL, DAL and DAO source. Confidence tags: CONFIRMED (read directly in code), INFERRED, UNKNOWN. Stored-procedure bodies are NOT in the repository unless stated; SP effects are UNKNOWN. Sections not rewritten below were mechanically extracted and re-checked where noted.
## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-363 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | DcStockOutApproval |
| Kind | Approval (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_DcStockOutApproval |
| aspx size | 7503 bytes |
| Code-behind LOC | 229 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**Purpose (CONFIRMED):** Checker screen for Depot Stock Adjustment Vouchers in status `Posted`: select vouchers, choose Approve or Reject, submit. View icon opens the voucher report. Source: `Solution.Web/SInventory_UI/DcStockOutApproval.aspx.cs`.

## 3. Business Objective
Maker/checker control before a DC stock write-off/adjustment (PAGE-367) takes effect. Approve calls SP `sp_UD_DcStockOutApproval` (body absent; presumed to apply the stock effect - UNKNOWN); Reject sets a status only.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `dcstockoutreportviewer.aspx`

## 5. Route
`~/SInventory_UI/DcStockOutApproval.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/DcStockOutApproval.aspx |
| Code-behind | Solution.Web/SInventory_UI/DcStockOutApproval.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | DeStockOutBLL (Library.BLL/SInventory_BLL/DeStockOutBLL.cs) |
| DAL class | DeStockOutDal (Library.DAL/SInventory_DAL/DeStockOutDal.cs) |
| DAO/entity class | DcStockOutMasterDao (Library.DAO/SInventory_Entities/DcStockOutMasterDao.cs) |

## 7. UI Control Inventory
| Control | Label | Required | Notes |
|---|---|---|---|
| statusRadioButtonList | Approval Status `Approve` (1) / `Reject` (0) | Yes (`Please select an operation!!`) | no default |
| loadGridView | SL, DC, Invoice, Reason, Stock Out Date, Status, View, select | DataKeys `DcStockOutMasterId` | `DeStockOutDal.DcStockOutAppDal`: `tblDeStockOutMaster LEFT JOIN tblCompanyUnit, tblInvoice WHERE Status='Posted'` |
| chkSelect / chkSelectAll | select | >=1 row (`Please select at least one row!!!`) | - |
| submitButton | Submit | - | `btnSubmit0_Click :121` |
| LinkButton1 / View command | View | - | `Session["DcStockOutMasterId"]`; opens `DcStockOutReportViewer.aspx?DcStockOutMasterId=` |

## 8. Field Mapping
Approve: `DcStockOutMasterId`, `Status='Approved'`, `ApprovedBy=LoginName`, `ApprovedDate=Today` -> `DeStockOutBLL.UpdateStockOutMasterDataForApprovalBll` -> `DeStockOutDal.UpdateStockOutMasterDataForApprovalDal :182-190` -> SP `sp_UD_DcStockOutApproval(@DcStockOutMasterId,@Status,@ApprovedBy,@ApprovedDate)` via `ClsCommonInternalDAL.UpdateAction`. Reject: same fields, `Status = radio text (Reject)` -> `ApprovalUpdateBLL` -> `ApprovalUpdateDal :136-149` = `UPDATE tblDeStockOutMaster SET Status, ApprovedBy, ApprovedDate WHERE DcStockOutMasterId`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| statusRadioButtonList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| Submit | `btnSubmit0_Click` -> `Validation` -> `SubmitApproval` | per checked row: approve -> SP; reject -> UPDATE; grid reload |
| View | `loadGridView_RowCommand :87` | opens report popup |

## 11. Frontend Flow
Init: `DcStockOutViewForApproval()` (queue). After submit the queue is reloaded. Alerts `Stock Adjustment Approved Successfully!!` / `Rejected !!`. No page JS.

## 12. Backend Flow
`SubmitApproval :173-226`: radio text `!= 'Reject'` -> for checked rows call SP wrapper; else UPDATE. No transaction, no try/catch; success messages irrespective of SP outcome; `DataTrue()` entitlement check exists but its call is commented out (`:22-26`).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| DeStockOutBLL.DcStockOutAppBll | Library.BLL/SInventory_BLL/DeStockOutBLL.cs:93 |
| DeStockOutBLL.GetMenuIdByMenuName | Library.BLL/SInventory_BLL/DeStockOutBLL.cs:73 |
| DeStockOutBLL.GetAssignedAppUser | Library.BLL/SInventory_BLL/DeStockOutBLL.cs:79 |
| DeStockOutBLL.UpdateStockOutMasterDataForApprovalBll | Library.BLL/SInventory_BLL/DeStockOutBLL.cs:107 |
| DeStockOutBLL.ApprovalUpdateBLL | Library.BLL/SInventory_BLL/DeStockOutBLL.cs:86 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| DeStockOutDal.DcStockOutAppDal | Library.DAL/SInventory_DAL/DeStockOutDal.cs:152 | inline SQL: tblCompanyUnit, tblDeStockOutMaster, tblInvoice |  |
| DeStockOutDal.GetMenuIdByMenuName | Library.DAL/SInventory_DAL/DeStockOutDal.cs:117 | inline SQL: tblMainMenu | @MenuName |
| DeStockOutDal.GetAssignedAppUser | Library.DAL/SInventory_DAL/DeStockOutDal.cs:126 | inline SQL: tblAppSetup | @SL, @UserId |
| DeStockOutDal.UpdateStockOutMasterDataForApprovalDal | Library.DAL/SInventory_DAL/DeStockOutDal.cs:183 | sp_UD_DcStockOutApproval | @ApprovedBy, @ApprovedDate, @DcStockOutMasterId, @Status |
| DeStockOutDal.ApprovalUpdateDal | Library.DAL/SInventory_DAL/DeStockOutDal.cs:137 | inline SQL: tblDeStockOutMaster | @ApprovedBy, @ApprovedDate, @DcStockOutMasterId, @Status |

## 16. Stored Procedure Mapping
| SP | Called from | Definition | Effect |
|---|---|---|---|
| `sp_UD_DcStockOutApproval` | `DeStockOutDal.cs:182-190` | body NOT in repository | UNKNOWN |

## 17. Database Mapping
R `tblDeStockOutMaster`, `tblCompanyUnit`, `tblInvoice`; W `tblDeStockOutMaster.Status/ApprovedBy/ApprovedDate` (direct UPDATE for reject; SP for approve, other tables UNKNOWN). Helper reads `tblMainMenu`, `tblAppSetup` unused.

## 18. Validation Matrix
| Field | Rule | Message | BR |
|---|---|---|---|
| Action radio | required | Please select an operation!! | BR-M10-047 |
| Rows | >=1 checked | Please select at least one row!!! | BR-M10-047 |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M10-047 | Only vouchers with `Status='Posted'` are listed; Approve sets `Approved` (through SP `sp_UD_DcStockOutApproval` with approver and today's date), Reject sets `Reject` with approver/date via a direct UPDATE. An action and at least one row are mandatory. | `Library.DAL/SInventory_DAL/DeStockOutDal.cs:136-190`; `DcStockOutApproval.aspx.cs:121-226` | `SubmitApproval` | CONFIRMED (SP effect UNKNOWN) |
| BR-M10-048 | Approver entitlement is not enforced (the `tblAppSetup` check in `DataTrue()` is commented out); the creator can approve their own voucher. | `DcStockOutApproval.aspx.cs:22-26,52-78` | - | CONFIRMED |

## 20. Permission Matrix
`Session["LoginName"]` (ApprovedBy). `DataTrue()` (menu/app-setup based approver check) is dead code.

## 21. Status Lifecycle
`Posted` -> `Approved` (SP) / `Reject` (UPDATE). `Approved` vouchers cannot be deleted by the list page. No reversal path.

## 22. Error Handling
No error handling; success alerts shown regardless of outcome.

## 23. Audit
`ApprovedBy`, `ApprovedDate` (date only).

## 24. Integration
None.

## 25. Reports
`DcStockOutReportViewer.aspx` -> `crpDcStockOut.rpt` (RPT-M10-04).

## 26. Dependencies
`DeStockOutBLL/Dal`, `DcStockOutMasterDao`, `ClsCommonInternalDAL.UpdateAction`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-363-01 | Open page | only Posted vouchers | BR-M10-047 |
| TC-363-02 | Approve one | Status Approved, ApprovedBy set; verify DC stock effect in DB (UNKNOWN) | BR-M10-047 |
| TC-363-03 | Reject | Status `Reject` | BR-M10-047 |
| TC-363-04 | Creator approves own voucher | allowed | BR-M10-048 |
| TC-363-05 | No action selected | alert | BR-M10-047 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/DcStockOutApproval.aspx`, `Solution.Web/SInventory_UI/DcStockOutApproval.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | - | MISSING |
| Stock effect of approval | None existed | in SP (UNKNOWN) | UNKNOWN |

## 30. Known Gaps
- GAP-M10-011: SP body missing.
- No maker!=checker or approver-assignment control.
- Approval and stock effect not transactional with the page (SP call only).

## 31. Change Impact
Shares `DeStockOutDal` with PAGE-367/368; SP `sp_UD_DcStockOutApproval` used only here in repo.

