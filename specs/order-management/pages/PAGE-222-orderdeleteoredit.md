---
page_id: PAGE-222
module: M06
page: OrderDeleteorEdit
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "UNKNOWN", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-222 — OrderDeleteorEdit ("Order Delete")

> **Evidence level:** Tier A for the page code (`OrderDeleteorEdit.aspx.cs` read in full, aspx grepped), BLL/DAL wrappers read. **`sp_DeleteOrder` and `sp_GetOrderInvoiceIsZero` bodies are absent → what a delete really removes/refuses is UNKNOWN (database: UNKNOWN).** The "Proforma Already Generated" message is the page's interpretation of a `false` return.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-222 |
| Module | M06 |
| Kind | Edit/Delete form |
| Menu | `MasterPages/MasterPage.master:1206` "Order Delete" |
| LOC | code-behind 152; page comment/naming carries leftovers of a customer-master screen (`CustMasterNewImageButton_Click` → `CustMasterEntry.aspx`, `rptImageButton_Click` → `CustomerMasterViewer.aspx`) — copy-paste origin |

## 2. Purpose
Lets an authorised user pick a not-yet-invoiced order from a dropdown and delete it (confirm dialog). A grid variant (search by order code, per-row Edit/Delete) is coded but its search box is hidden (`custcodenameTextBox Visible="false"`, aspx:48). CONFIRMED.

## 3. Business Objective
Remove wrongly captured or duplicate orders before they are invoiced. INFERRED.

## 4. Navigation
Menu → page. Row Edit opens `OrderEdit.aspx?ID=<orderId>` in a popup (PAGE-223). After successful delete: JS `successalert(...,'OrderDeleteorEdit.aspx')` refresh.

## 5. Route
`~/SInventory_UI/OrderDeleteorEdit.aspx`.

## 6. Source Files
aspx(.cs); `Library.BLL/SInventory_BLL/CustomerMasterBLL` → `CustomerMasterDAL.DeleteRequisition:334`, `LoadOrderView:89`; `CommonDataLoad.GetOrderInvoiceIsZero:1954`.

## 7. UI Control Inventory
| Control | Behaviour |
|---|---|
| ddlOrder | value=OrderId, text=OrderCode; source `sp_GetOrderInvoiceIsZero` (name suggests orders with `IsInvoice=0` — INFERRED); first item "Please Select From List" (empty). **No required validator**. |
| searchButton (LinkButton, text unknown; handler `Button1_Click`) | `OnClientClick=sweetAlertConfirm_Delete` → deletes the selected order |
| custcodenameTextBox | hidden |
| loadGridView | (hidden path) Edit (`EditData`) / Delete (`DelData`, confirm) per row; data from `LoadOrderView(orderCode)`: `select * from tblOrder where IsInvoice=0 and OrderCode=@OrderCode` |
| reset | `Unnamed_Click` → reload page |

## 8. Field Mapping
`ddlOrder.SelectedValue` → `Convert.ToInt32` → `DeleteRequisition(OrdID)` → `sp_DeleteOrder(@OrdID, @LoginName)` where **`@LoginName` is filled with `Session["UserId"]`** (not the login name) — `CustomerMasterDAL.cs:336-340`.

## 9. Dropdown Mapping
`ddlOrder` ← `LoadDropDown()` (`:26-46`) ← `sp_GetOrderInvoiceIsZero`; failures swallowed by empty catch.

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| main delete | `Button1_Click:122` | `DeleteRequisition(selected)`; true ⇒ success alert+reload; false ⇒ "Can not delete!!Proforma Already Generated!" |
| grid Delete | `loadGridView_RowCommand:82` | same for the row's OrderId; reload grid; "Delete Successfully" / "Cant delete!!Proforma Already Generated" |
| grid Edit | `PopUp` → `OrderEdit.aspx?ID=` | change DC (PAGE-223) |

## 11. Frontend Flow
Page_Load loads the order dropdown once; `sweetAlertConfirm_Delete` (shared JS in master/layout) asks for confirmation; server delete; toast.

## 12. Backend Flow
`Button1_Click` → `CustomerMasterBLL.DeleteRequisition` → `CustomerMasterDAL.DeleteRequisition` → `aCommonInternalDal.DeleteAction("sp_DeleteOrder", {@OrdID, @LoginName=Session["UserId"]})` → bool. No try/catch: an empty selection makes `Convert.ToInt32("")` throw `FormatException` (unhandled).

## 13. API Mapping
None.

## 14. Service Mapping
`CustomerMasterBLL.DeleteRequisition/LoadOrderView`.

## 15. Repository Mapping
See §6. Note the method is named `DeleteRequisition` though it deletes an **order** (`sp_DeleteOrder`); an unrelated `RequisitionDAL.DeleteRequisition(reqId)` also exists.

## 16. Stored Procedure Mapping
`sp_DeleteOrder` (UNKNOWN body; write), `sp_GetOrderInvoiceIsZero` (UNKNOWN; read).

## 17. Database Mapping
`tblOrder` (inline read in `LoadOrderView`); writes by SP: UNKNOWN (presumably `tblOrder`/`tblOrderDetail` and a delete log — not verifiable).

## 18. Validation Matrix
| Rule | Where | Message |
|---|---|---|
| Not invoiced (delete refused) | inside `sp_DeleteOrder` (UNKNOWN); UI reads `false` | "Can not delete!!Proforma Already Generated!" |
| Order search returns rows | `CustomerMasterLoad` | "No Order Found!!" |
| Order selected | **none** | — (FormatException) |

## 19. Business Rules
| ID | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M06-063 | Only orders listed by `sp_GetOrderInvoiceIsZero` (INFERRED: not invoiced) are offered for deletion; the list view of the grid is `IsInvoice=0` by inline SQL. | OrderDeleteorEdit.aspx.cs:32; CustomerMasterDAL.cs:91 | SP+SQL | CONFIRMED (grid) / INFERRED (dropdown) |
| BR-M06-064 | A delete that the SP refuses (returns false) is reported as "Proforma Already Generated"; the actual refusal conditions live in the SP (UNKNOWN). | .cs:85-94, 124-134 | page | CONFIRMED (message) / UNKNOWN (condition) |
| BR-M06-065 | The deleting user is passed as `Session["UserId"]` into a parameter named `@LoginName`. | CustomerMasterDAL.cs:338 | DAL | CONFIRMED |

## 20. Permission Matrix
No role/session check in code beyond DAL reading `Session["UserId"]` (NullReference if session expired). Menu-based only.

## 21. Status Lifecycle
Order removed (state after delete UNKNOWN: hard delete vs soft flag).

## 22. Error Handling
No try/catch; SP exceptions propagate; user text for refusals only.

## 23. Audit
Deleter id passed to SP. The Delete Order report (`DeleteOrderReportDal.GetDeleteOrderReport`, `Library.DAL/SInventory_DAL/DeleteOrderReportDal.cs:28-33`) reads archive table `tblOrderDel` (OrderCode, ComUnitCode/Name, MIOCode/Name, CustomerCode/Name, GrossValue, SubmissionDate, **DeleteBy, DeleteDate**), so `sp_DeleteOrder` very probably copies the order there before deleting — INFERRED (SP body absent).

## 24. Integration
None.

## 25. Reports
`DeleteOrderReport.aspx` → `DeleteOrderReportViewer.aspx` lists deleted orders from `tblOrderDel` — see `reports/RPT-M06-02-delete-order-report.md`.

## 26. Dependencies
sp_DeleteOrder, sp_GetOrderInvoiceIsZero, PAGE-223.

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-01 | Positive | delete un-invoiced order | success alert, order gone from dropdown | 063 |
| TC-02 | Negative | invoiced order forced via crafted postback | SP refusal message (verify) | 064 |
| TC-03 | Negative | click delete with no selection | unhandled FormatException (defect) | — |
| TC-04 | Edge | expired session | NullReferenceException in DAL | 065 |
| TC-05 | Edge | verify audit row in delete log after delete | UNKNOWN — confirm | — |

## 28. Reverse Engineering References
`OrderDeleteorEdit.aspx(.cs)`, `CustomerMasterDAL.cs:89,334`, `CommonDataLoad.cs:1954`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | — | MISSING |
| Delete semantics | None existed | SP unknown | GAP-M06-020 |

## 30. Known Gaps
SP bodies missing; no selection validation; copy-paste leftovers; UserId sent as LoginName; hidden grid path unused.

## 31. Change Impact
`CustomerMasterDAL.DeleteRequisition` → `sp_DeleteOrder` shared with any caller of `CustomerMasterBLL.DeleteRequisition` (only this page found in `Solution.Web` by name grep of `OrderDeleteorEdit`; other callers not exhaustively searched).
