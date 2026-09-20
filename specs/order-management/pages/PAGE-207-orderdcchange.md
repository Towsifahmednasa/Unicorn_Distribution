---
page_id: PAGE-207
module: M06
page: OrderDCChange
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "UNKNOWN", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-207 — OrderDCChange (assign DC + route to unassigned orders)

> **Evidence level:** Tier A for the page code (`Solution.Web/MasterSetup_UI/OrderDCChange.aspx.cs` read in full; aspx skimmed by control id) and DAL wrappers (`OrderTrackingDAL.cs:128-150, 300-319`, `SeedDataDAL`). **`sp_Update_OrderDC` and `sp_Get_OrderTrackingList_Latest` bodies are absent → database UNKNOWN.** Note the folder: the page lives in `MasterSetup_UI`, not `SInventory_UI`.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-207 |
| Module | M06 |
| Kind | Bulk update form ("Details List" header; `Title="Order Tracking List"` copy-paste) |
| Class | MasterSetup_UI_OrderDCChange |
| LOC | 487 |

## 2. Purpose
Lists orders that have **no distribution centre assigned** (`mas.ComUnitId is null`), lets the user tick a set of them, choose a DC and a route of that DC and assign them in one action. CONFIRMED (`Parm()` `:426-437`, `btnSearch_Click:328-367`).

## 3. Business Objective
Orders arriving from the field/app without a resolvable DC must be routed to a fulfilling DC (and route) before they can enter the proforma-invoice list (which filters by `ComUnitId`, route, territory — see PAGE-217). INFERRED from consuming filters.

## 4. Navigation
Menu (DB-driven). Row link opens `../SInventory_UI/OrderStatus.aspx?MID=<id>` in a new window (`:322`). "Add" button (`EmpCetegoryAddImageButton_Click`) → `CustomerEntry.aspx` (copy-paste leftover). Success → `successalert(...,'OrderDCChange.aspx')` reload.

## 5. Route
`~/MasterSetup_UI/OrderDCChange.aspx`.

## 6. Source Files
aspx(.cs); `Library.DAL/MasterSetup_DAL/OrderTrackingDAL.cs`, `SeedDataDAL.cs`; user control `IVMarketStructure` (`Approval_UI/IVMarketStructureInvoSearchApp.ascx` family).

## 7. UI Control Inventory
| Control | Role | Required | Notes |
|---|---|---|---|
| ddlDistributionCenter | target DC | Yes (server `:408`, css `is-invalid`) | `SeedDataDAL.GetDistributionCenterDataTableList` (`sp_GET_DistributionCenter`), text ComUnitName / value ComUnitId; AutoPostBack loads routes |
| ddlRouteName | target route | Yes (`:416`) | `sp_GET_DistributionRouteByDCID(dc)` → RouteInformationMasterId/RouteName; cleared when DC blank |
| loadGridView | orders without DC | ≥1 row checked (`:377-406`) | DataKeys[0] = order id; row template has `chkSelect`, `hfOrderId`, many bound columns (order/customer/amount data); header `chkSelectAll`; paging (`loadGridView_PageIndexChanging` reloads) |
| lblOrderCount, lblOrderAmount, lblVAT, lblDiscount, lblAllTotal | Σ of GrossValue / TotalVat / TotalDiscount / TotalNetPayable | — | computed over the loaded table (`:193-218`) |
| btnSearch (handler `btnSearch_Click`) | actually the **Update/Assign** button (name misleading) | — | |
| btnExport | Excel export (`Order_Tracking_List_<timestamp>.xls`) | — | HTML-in-xls export, paging off |
| frmDate/toDate, ddlChemisType, ddlProgramType, ApprovalStatusSelect, IVMarketStructure (group…MIO ddl), custNameTextBox+autocomplete | search filters | — | populated but **never applied** (`Parm()` returns only the fixed clause) |

## 8. Field Mapping
| UI | Call | SP param |
|---|---|---|
| ddlDistributionCenter.SelectedValue | `OrderTrackingDAL.UpdateOrderDC(DCID, …)` | `@DCID` |
| ddlRouteName.SelectedValue | ″ | `@RouteNameId` |
| checked rows' `hfOrderId` joined with "," | ″ | `@OrdId` (comma-separated string) |
| `Session["UserId"]` | ″ | `@ApprovedBy` |
Target columns in `tblOrder` (ComUnitId/Code/Name, DistributionRouteId…): UNKNOWN (SP body absent).

## 9. Dropdown Mapping
Loaded on first request: chemist types (`sp_GET_CustomerTypeAll`), program types (`sp_GET_ProgramTypeListAll`), approval statuses (`sp_GET_ActionStatusList`, value `SoftwareUseId`, text `WebShow`), DCs (`sp_GET_DistributionCenter`); routes on DC change. Each wrapped in empty `try{}catch{}`.

## 10. Button Action Mapping
`btnSearch_Click` (assign), `btnExport_Click`, `resetBtn_Click` (reload), `chkSelectAll_CheckedChanged`, `loadGridView_RowCommand` (EditData → OrderStatus popup).

## 11. Frontend Flow
Page_Load: resolves user-control dropdown references, reads `RoleTypeName/EmpInfoId/RoleTypeId` (values kept in fields but not used), sets dates to today, loads dropdowns, `LoadData(Parm())`. DC change → route list. Assign button → server validation, alert result.

## 12. Backend Flow
`LoadData(parm)` → `OrderTrackingDAL.GetOrderTrackingList(parm)` → `sp_Get_OrderTrackingList_Latest(@Parm)` where `@Parm = " AND mas.ComUnitId is  null  "` (a raw SQL fragment injected into the SP — dynamic SQL inside SP, UNKNOWN) → bind + totals. `btnSearch_Click` → `Validation()` (grid non-empty; ≥1 checked; DC; route) → build id list → `UpdateOrderDC(...)` → `accessManager.UpdateData("sp_Update_OrderDC", …)` → success/failure alert. No confirmation dialog, no per-order result.

## 13. API Mapping
None.

## 14. Service Mapping
None (page → DAL directly: `OrderTrackingDAL`, `SeedDataDAL`, `CommonDataLoad`).

## 15. Repository Mapping
`OrderTrackingDAL.UpdateOrderDC:128`, `GetOrderTrackingList:300`.

## 16. Stored Procedure Mapping
| SP | Body |
|---|---|
| sp_Update_OrderDC | UNKNOWN (write) |
| sp_Get_OrderTrackingList_Latest | UNKNOWN (read; accepts SQL fragment) |
| sp_GET_DistributionCenter, sp_GET_DistributionRouteByDCID, sp_GET_CustomerTypeAll, sp_GET_ProgramTypeListAll, sp_GET_ActionStatusList | UNKNOWN (dropdown reads) |

## 17. Database Mapping
UNKNOWN beyond names; expected `tblOrder` (alias `mas`) updated.

## 18. Validation Matrix
| Rule | Message |
|---|---|
| grid empty | "Table can not be Empty!" |
| none checked | "Please Select at least one row !!!" |
| DC blank | css `is-invalid`, tooltip "please fill out this field" (no alert) |
| route blank | same |
| SP failure | "Operation Faild!" |

## 19. Business Rules
| ID | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M06-066 | The page works only on orders whose DC is unset (`ComUnitId is null`); all other filter controls are decorative. | OrderDCChange.aspx.cs:426-437 | page | CONFIRMED |
| BR-M06-067 | Assignment needs at least one selected order, a DC and a route of that DC; assignment is a set operation on the checked ids (comma list), performed by SP with the user id as `@ApprovedBy`. | .cs:328-424; OrderTrackingDAL.cs:128-141 | page+SP | CONFIRMED (page) / UNKNOWN (SP effect) |
| BR-M06-068 | Route choices are restricted to the routes of the selected DC. | .cs:455-486 | page | CONFIRMED |

## 20. Permission Matrix
Reads `Session["RoleTypeName"]`, `["EmpInfoId"]`, `["RoleTypeId"]` but does not act on them (DIC-restriction to own DC is commented out `:114-118, 471-475`). `Session["UserId"]` written as approver. Any menu-permitted user may reassign DC for any unassigned order. CONFIRMED.

## 21. Status Lifecycle
Order: DC-less (`ComUnitId NULL`) → DC+route assigned (SP) → eligible for invoicing lists. Other fields set by the SP UNKNOWN.

## 22. Error Handling
Empty catches for dropdown loads; failure toast only; SP exception propagates from `UpdateOrderDC` catch (`accessManager.SqlConnectionClose(true)`, result flagged not success).

## 23. Audit
`@ApprovedBy` = UserId passed; persistence UNKNOWN.

## 24. Integration
None visible (orders from app/SAP integration produce the DC-less rows — UNKNOWN).

## 25. Reports
Excel export of the grid.

## 26. Dependencies
Seed SPs, `OrderStatus.aspx`, PAGE-217 consumer.

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-01 | Positive | 3 DC-less orders selected, DC+route chosen | success; orders vanish from list; appear in DC's proforma list | 066,067 |
| TC-02 | Negative | no row ticked | "Please Select at least one row !!!" | 067 |
| TC-03 | Negative | DC chosen, route blank | field highlighted, no update | 067 |
| TC-04 | Edge | change DC after choosing route | route list reloaded | 068 |
| TC-05 | Edge | set date/type filters and press search | list unchanged (filters ignored) | 066 |
| TC-06 | Edge | select 500 orders | comma list passed to SP (limit UNKNOWN) | 067 |

## 28. Reverse Engineering References
`OrderDCChange.aspx(.cs)`, `OrderTrackingDAL.cs`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | — | MISSING |
| Filters | None existed | ignored | GAP-M06-021 |
| SP semantics | None existed | unknown | GAP-M06-022 |

## 30. Known Gaps
SP bodies missing; inert filters; misnamed `btnSearch`; raw SQL fragment `@Parm`; no confirmation; role variables unused; copy-paste titles/links.

## 31. Change Impact
`sp_Get_OrderTrackingList_Latest` is also used by `OrderTrackingList.aspx.cs:204,334` (PAGE-208); `OrderTrackingListDBH` uses the sibling `sp_Get_OrderTrackingList_DBH`; `sp_Update_OrderDC` only from this page (grep of `Library.DAL`: `OrderTrackingDAL.UpdateOrderDC`).
