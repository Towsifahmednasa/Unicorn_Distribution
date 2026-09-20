---
page_id: PAGE-700
module: M18
page: DoctorCustomerTransferApproval
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-700 — DoctorCustomerTransferApproval

> **Evidence level:** Tier A — `.aspx` (281 lines), `.aspx.cs` (376 LOC), `CustomerInfoDAL.UpdateCustomer_Doctor_TransferApprove` (:170), `GetCustomerTranferList` (:427), `DoctorDAL.GetDoctorTransferList` (:438), `CommonDataLoad.GetValidationCheck` (:1581) read. SP bodies (`sp_Get_CustomerTranferApprovalList`, `sp_Get_DoctorTranferApprovalList`, `sp_Update_Customer_Doctor_TransferApprove`, `sp_check_Vali_MarketStructure`) NOT in the repository → `UNKNOWN`. **Key finding: the Submit button cannot perform any action as coded (see BR-M18-052).**

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / module | PAGE-700 / M18 |
| Page name | DoctorCustomerTransferApproval (breadcrumb "Doctor/Customer Approval") |
| Kind | Bulk approval of market transfers of customers or doctors (checkbox list + Submit) |
| Folder / master | Approval_UI / NewMasterPage.master |
| Class | Approval_UI_DoctorCustomerTransferApproval |
| Related | `MasterSetup_UI/Customer_Doctor_Transfer.aspx` (request page; calls `sp_Update_Customer_Doctor_Transfer`, `CustomerInfoDAL.cs:150`); `TransferUI/CustomerDocTransferApproval.aspx` (PAGE-202, the approval page that is wired to a working handler); `TransferUI/MarketStructure_TransferApprove.aspx` (PAGE-099) |

## 2. Purpose
An authorised user lists pending customer-transfer or doctor-transfer requests (toggle "Customer Transfer" / "Doctor Transfer"), ticks rows and submits to approve them. INFERRED from SP names and `Type="Cust"/"Doc"`.

## 3. Business Objective
Finalise the reassignment of customers/doctors to a new market/sales person/DC/route (arguments `MarketId`, `DCID`, `RouteId` exist in the DAL but the page passes empty strings). Customers with a pending order cannot be ticked. INFERRED.

## 4. Navigation
DB menu. On success the JS `successalert(..., 'Customer_Doctor_Transfer.aspx')` navigates to a page that does not exist in `Approval_UI` (the real one is `MasterSetup_UI/Customer_Doctor_Transfer.aspx`; this page's code was copied from it, GAP-M18-011). The live transfer approval page is `TransferUI/CustomerDocTransferApproval.aspx` (PAGE-202, `specs/customer-master/pages/PAGE-202-customerdoctransferapproval.md`), which uses `_DAL.CustomerDocTransferApprovalDAL` per row; this M18 page is a stale variant (INFERRED: no menu/link to it was found; menu content UNKNOWN). Outbound from the (unbound) stock-out grid: `../SInventory_RPTVIEW/DcStockOutReportViewer.aspx?DcStockOutMasterId=` (`.cs:147`) — unreachable.

## 5. Route
`~/Approval_UI/DoctorCustomerTransferApproval.aspx`.

## 6. Source Files
View/code-behind; `CustomerInfoDAL`, `DoctorDAL`, `CommonDataLoad`, BLL `Library.BLL.SInventory_BLL.DeStockOutBLL` (leftover), DAO `DcStockOutMasterDao`, `ResultInfo`.

## 7. UI Control Inventory
| Control | Label | Required | Default | Notes |
|---|---|---|---|---|
| rbType | "Customer Transfer" (0, selected) / "Doctor Transfer" (1) | — | Customer | AutoPostBack → `rbType_SelectedIndexChanged` |
| gv_Customer_List | (grid) | — | bound to `sp_Get_CustomerTranferApprovalList(@Parm="")` | key `MasterID`; hidden `hfCustomerMasterId=CustomerMasterId`; columns SL, select-all checkbox (AutoPostBack), row checkbox `chkSelect` (AutoPostBack → `CheckCustInOrder`), Customer Code, Customer Name, Customer Type |
| gv_Doctor_List | (grid) | — | bound to `sp_Get_DoctorTranferApprovalList(@Parm="")` | key `DoctorId`; columns SL, `chkDoctorSelectAll` (AutoPostBack), `chkDoctorSelect`, Doctor Code, Doctor Name, Designation, Degree, Doctor Speciality |
| submitButton | Submit | — | — | `OnClick=btnSubmit0_Click` |
| statusRadioButtonList | Approval Status | — | "Approve" (value 1, selected), "Reject" (value 0) | inside `display:none` div — user cannot see/change it |
| loadGridView | (stock-out list: Sales Center, Invoice No, Reason, StockOut Date, Status, Report button) | — | **never bound** | leftover from a DC stock-out approval page; key `DcStockOutMasterId` |
| UpdatePanel/UpdateProgress | — | — | — | spinner during postback |

## 8. Field Mapping
| UI | Value | DAL param | SP param |
|---|---|---|---|
| customer `DataKeys[i][0]` (`MasterID`) | `EmpId.ToString()` (variable misnamed) | `Id` | `@MasterId` |
| — | `""` | `MarketId` | `@MarketId` |
| Session["UserId"] | `HttpContext.Current.Session["UserId"]` | `SessionUser` | `@ApprovedBy` |
| literal | "Cust" or "Doc" | `Type` | `@Type` |
| — | `""` | `DCID` | `@DCID` |
| — | `""` | `TouteId` (sic) | `@RouteId` |
Doctor path uses `gv_Doctor_List.DataKeys[i][0]` (`DoctorId`) as `@MasterId` with `Type="Doc"`. Tables/columns UNKNOWN. Every parameter is passed as string; empty strings for `@MarketId/@DCID/@RouteId` mean the SP either ignores them or writes empty values (UNKNOWN).

## 9. Dropdown Mapping
None (radio list only).

## 10. Button Action Mapping
| Button | Handler | DAL | SP | DB |
|---|---|---|---|---|
| rbType toggle | `rbType_SelectedIndexChanged` :62 | `GetCustomerTranferList("")` or `GetDoctorTransferList("")` | `sp_Get_CustomerTranferApprovalList` / `sp_Get_DoctorTranferApprovalList` | read; exceptions swallowed (:91) |
| chkSelect | `chkSelect_CheckedChanged` :153 → `CheckCustInOrder` | `GetValidationCheck(customerMasterId,"CustomerTransfer")` | `sp_check_Vali_MarketStructure(@MasterId,@PageName)` | read |
| header select-all (customer) | `chkSelectAll_CheckedChanged` :202 | same per row | same | read |
| header select-all (doctor) | `chkDoctorSelectAll_CheckedChanged` :34 | — | — | none |
| Submit | `btnSubmit0_Click` :215 | `UpdateCustomer_Doctor_TransferApprove` per ticked row | `sp_Update_Customer_Doctor_TransferApprove` | write (UNKNOWN) — but see BR-M18-052 |

## 11. Frontend Flow
Type toggle postback rebinds one grid, clears the other. Ticking a customer checkbox triggers an async postback that unticks it and shows `faildalert('Order Pending!','Faild')` if `sp_check_Vali_MarketStructure` returns any row. Success: `successalert('Operation successful!','Success','Customer_Doctor_Transfer.aspx')`; failure: `faildalert('Operation Faild!','Faild')`.

## 12. Backend Flow
`Page_Load`: no Session check, no permission check, no try/catch; first load `rbType_SelectedIndexChanged(null,null)` (:28) which clears both grids, then binds customer (index 0 selected) or doctor list; **all exceptions swallowed** (empty catch :91) → empty grid.
`CheckCustInOrder` (:168-201): if row ticked, call validation SP; `dt.Rows.Count > 0` → untick + "Order Pending!"; exceptions swallowed.
`btnSubmit0_Click` (:215-280): `if (Validation())` → loop tick rows of the active grid and call `UpdateCustomer_Doctor_TransferApprove(Type, Id, "", UserId, "", "")` once per ticked row; **`Res` retains only the last call's result**, so a failure on an earlier row is masked if the last succeeds (and vice versa); if no row ticked `Res.isSuccess` is `false`… (Res is a new `ResultInfo`, `isSuccess` default false) → "Operation Faild!".
`Validation()` (:282-318): if `statusRadioButtonList.SelectedValue == ""` → `alert('Please select an operation!!')`; **else iterates `loadGridView.Rows` (the unbound stock-out grid, zero rows), so `count` stays 0 and the method returns `false`**. `Validation()` is therefore always false and the `if (Validation())` block in `btnSubmit0_Click` never executes: pressing Submit does nothing and shows no message (the radio list always has a selected item). CONFIRMED by code reading: `DcStockOutViewForApproval()` (:93-100) that binds `loadGridView` is private and never called; `SubmitApproval()` (:320-373) is never called either; `DataTrue()` (:103-129) unused.
The DAL (`UpdateCustomer_Doctor_TransferApprove` :170-203) uses `UpdateData` → `ExecuteNonQueryVoid` (always true unless exception), commits on `SqlConnectionClose`, rollback+rethrow on exception; per call it opens/closes its own transaction (one transaction per ticked row; no all-or-nothing).

## 13. API Mapping
None.

## 14. Service Mapping
`DeStockOutBLL` instantiated (`aStockOutBll`) only for dead code (`DcStockOutAppBll`, `GetMenuIdByMenuName`, `GetAssignedAppUser`, `UpdateStockOutMasterDataForApprovalBll`, `ApprovalUpdateBLL`); not used by the live flow.

## 15. Repository Mapping
| DAL | Source | SP |
|---|---|---|
| CustomerInfoDAL.GetCustomerTranferList | CustomerInfoDAL.cs:427 | sp_Get_CustomerTranferApprovalList(@Parm) |
| DoctorDAL.GetDoctorTransferList | DoctorDAL.cs:438 | sp_Get_DoctorTranferApprovalList(@Parm) |
| CommonDataLoad.GetValidationCheck | CommonDataLoad.cs:1581 | sp_check_Vali_MarketStructure(@MasterId,@PageName) |
| CustomerInfoDAL.UpdateCustomer_Doctor_TransferApprove | CustomerInfoDAL.cs:170 | sp_Update_Customer_Doctor_TransferApprove(@MasterId,@MarketId,@ApprovedBy,@Type,@DCID,@RouteId) |

## 16. Stored Procedure Mapping
All four bodies absent (GAP-M18-001).

## 17. Database Mapping
Tables UNKNOWN. Semantic guess (INFERRED): a transfer-request table with `MasterID`, joined to `tblCustMaster`/`tblDoctorMaster`.

## 18. Validation Matrix
| Field | Rule | Message |
|---|---|---|
| Customer tick | pending order exists (validation SP returns rows) → untick | "Order Pending!" |
| Submit | `Validation()` (stock-out grid based; always false) | none shown |
| Approval Status | must have a selection (always Approve by default, hidden) | "Please select an operation!!" (unreachable) |
| Result | `Res.isSuccess` | "Operation successful!" / "Operation Faild!" (unreachable in practice) |

## 19. Business Rules
| BR | Description | Source | Conf. |
|---|---|---|---|
| BR-M18-050 | A customer with a pending order (validation SP `sp_check_Vali_MarketStructure`, page name "CustomerTransfer") cannot be selected for transfer approval | DoctorCustomerTransferApproval.aspx.cs:168-201 | CONFIRMED (SP logic UNKNOWN) |
| BR-M18-051 | Submit calls `sp_Update_Customer_Doctor_TransferApprove` once per ticked row with Type "Cust"/"Doc", ApprovedBy = Session UserId, MarketId/DCID/RouteId empty; last call's result decides the message | :215-280; CustomerInfoDAL.cs:170-185 | CONFIRMED |
| BR-M18-052 | Submit is gated by `Validation()` which inspects the unbound `loadGridView` → always false → transfer approval never executes from this page | :282-318, :93-100 | CONFIRMED |
| BR-M18-053 | Customer/Doctor toggle rebinding; lists come from transfer-approval SPs with empty filter | :62-92 | CONFIRMED |
| BR-M18-054 | No Session/permission check on this page (menu only) | .cs whole file | CONFIRMED |
| BR-M18-055 | Approver identity recorded = `Session["UserId"]` (not EmpInfoId) | :238,:256 | CONFIRMED |
| BR-M18-013 | Save success = no exception (`UpdateData` always true) | DataAccessManager.cs:506-519 | CONFIRMED |

## 20. Permission Matrix
Menu-based only. Code enforces nothing: no Session read for authorisation, no role check, no `UserPersmissionValidation`. Direct URL access by any authenticated user (master page presumably redirects unauthenticated users) reaches the page; because Submit is non-functional (BR-M18-052) the practical risk is low today but would become high if Validation were fixed.

## 21. Status Lifecycle
Transfer request: pending → approved (SP-defined). "Reject" option exists (radio value 0) but the live handler never branches on it: `btnSubmit0_Click` always calls the Approve SP (`Type` only Cust/Doc). Reject path exists only in dead `SubmitApproval`. CONFIRMED.

## 22. Error Handling
Swallowed exceptions in list load and validation; save exceptions from DAL are rethrown (no catch in page).

## 23. Audit
`@ApprovedBy` = UserId only. No timestamp param in DAL (SP may set; UNKNOWN).

## 24. Integration
None observed.

## 25. Reports
Only the unreachable `DcStockOutReportViewer` link (`.cs:147`). See reports/README.md.

## 26. Dependencies
CustomerInfoDAL, DoctorDAL, CommonDataLoad, DeStockOutBLL, DAO.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Open page | Customer Transfer list shown | 053 |
| TC-02 | Toggle to Doctor Transfer | doctor grid bound, customer grid emptied | 053 |
| TC-03 | Tick a customer having pending order | untick + "Order Pending!" | 050 |
| TC-04 | Tick customers, press Submit | (as coded) nothing happens — regression check | 052 |
| TC-05 | After (hypothetical) fix, multi-row submit with one failing row | last result masks earlier failure | 051 |
| TC-06 | Non-authorised role opens URL | page renders (no code check) | 054 |
| TC-07 | Select-all customers | each ticked row validated; those with pending orders unticked | 050 |

## 28. Reverse Engineering References
`DoctorCustomerTransferApproval.aspx(.cs)`, `CustomerInfoDAL.cs:170,427`, `DoctorDAL.cs:438`, `CommonDataLoad.cs:1581`, WF-M18-04.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | §12 | MISSING |
| Submit | None existed | dead (Validation on wrong grid) | DEFECT |
| Reject | None existed | not implemented in live path | GAP |

## 30. Known Gaps
GAP-M18-001, 011 (redirect target), 014 (dead submit), 024 (no per-row/all-or-nothing handling), 028 (leftover stock-out code), plus no permission enforcement (GAP-M18-003).

## 31. Change Impact
`UpdateCustomer_Doctor_TransferApprove` grep: only this page. `CommonDataLoad.GetValidationCheck(...,"CustomerTransfer")` is shared with `MasterSetup_UI/Customer_Doctor_Transfer.aspx.cs:322` and `TransferUI/MarketStructure_TransferApprove.aspx.cs:544` (also `TerritoryWiseDepotSetupUncheck` at `MasterSetup_UI/TerritoryWiseDepotSetup.aspx.cs:341`). Fixing `Validation()` would activate a bulk write with no all-or-nothing transaction.
