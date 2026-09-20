---
page_id: PAGE-296
module: M08
page: SalesReturnList_new
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-296 — SalesReturnList_new (worklist for second-time / partial-dues returns)

> **Evidence level:** Tier A — `SalesReturnList_new.aspx.cs` read in full (717 lines); `.aspx` grid/buttons read (`:296-410`), filter bar via control inventory; `SalesReturnNewDAL.LoadOrderWithInvoiceSP`, `SalesReturnDAL.SavePaymentConformationFull`. The driving SP `sp_BeforesndSalesReturnList`, `sp_PaymentConformationFull`, `sp_GET_forsndReturnTerritoryByRouteInformationDDL`, `SeedDataDAL.GetRouteInfoforSalesReturn2ndTimes` bodies are **not in the repo** → database PARTIAL.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-296 |
| Module | M08 |
| Name / class | SalesReturnList_new / `SInventory_UI_SalesReturnList_new` (`.aspx.cs:14`) |
| Kind | List / worklist with row navigation |
| Size | aspx 532 lines, code-behind 717 lines |

## 2. Purpose
CONFIRMED (grid caption "Go to Partial", status "Partial Dues"): list, for a chosen DC / route / territory / optional invoice number, the delivery invoices that were **already payment-confirmed but not yet second-time-returned**, and send the user to `SndTimesSalesReturnPartial.aspx` for the selected invoice.

## 3. Business Objective
INFERRED: give collectors/depot staff a worklist of invoices with "partial dues" so partially accepted deliveries can be adjusted (PAGE-298), and a bulk "ALL Submit" (currently inoperative) for full confirmation.

## 4. Navigation
Inbound: `SndTimesSalesReturnPartial.aspx` (back). Outbound: `SndTimesSalesReturnPartial.aspx` (Session["InvoiceId"]); `DelivaryInvoiceCreationAuto.aspx` (`cancelButton_Click`); hidden `EmpCetegoryAddImageButton` opens an **external URL with a hard-coded IP address and UserId/LoginName in the query string** (`.aspx.cs:714`).

## 5. Route
`~/SInventory_UI/SalesReturnList_new.aspx`.

## 6. Source Files
| Type | Path |
|---|---|
| View / code-behind | Solution.Web/SInventory_UI/SalesReturnList_new.aspx(.cs) |
| BLL | OrderInfoBLLSalesReturn (LoadSC, LoadManufac, LoadOrderWithInvoice, LoadRouteforReturn); SalesReturnBLL (`SavePaymentConformationFull`) |
| DAL | SalesReturnNewDAL.cs:301-330 (`LoadOrderWithInvoiceSP`), SalesReturnDAL.cs:259-266, SeedDataDAL (route list), RouteInformationDAL (`GetforsndreturnTerritoryByRouteInformationDA_DDLId`) |

## 7. UI Control Inventory
| Control | Type | Required | Default / state | Source |
|---|---|---|---|---|
| salesCenterDropDownList | DropDownList (AutoPostBack) | effectively yes | index 1 of user's DCs (`LoadSC(ddl, UserId)`) | `tblCompanyUnit` ∩ `tblUserCompanyUnit(UserId)` |
| manufacDropDownList | DropDownList | no | index 1 | `LoadManufac` (`tblManufacturer`) |
| marketDropDownList | DropDownList | no | — | legacy; only via `Session["DelMarketId"]` |
| rootDropDownList | DropDownList (AutoPostBack) | yes for results | "Please Select From List" | `SeedDataDAL.GetRouteInfoforSalesReturn2ndTimes(ComUnitId)` (DistributionRouteId/Name) |
| ddlTerritoryName | DropDownList | yes for results | "Please Select From List" | `RouteInformationDAL.GetforsndreturnTerritoryByRouteInformationDA_DDLId(route)` |
| invoicenoTextBox (AutoComplete `GetAllInvoice`) | TextBox | no | empty | free text, becomes `LIKE '%text%'` |
| submitButton (Search) → `Button1_Click` | LinkButton | — | — | rebinds grid |
| orderGridView | GridView `DataKeyNames=ComUnitId,ManufacId,OrderId,InvoiceId,MarketId,CustomerMasterId` | — | bound to `sp_BeforesndSalesReturnList` | cols: SL#, select, Invoice No, Invoice Date, Customer Code, Customer Name, Market, Order Date, Net Amount (`lblTpGrandTotal`), Status, Submit |
| chkSelectAll / chkSelect | CheckBox (AutoPostBack) | — | unchecked | update `lblCount` "Total Net Amount: n" |
| statusDropDownList | DropDownList | — | single item "Partial Dues" **and `Enabled=false`** | preselected from `LoadingSummaryStatus` |
| reasonReturnDropDownList | DropDownList `Visible=False` | — | 13 static reasons (value 11 appears twice) | shown only if status "Reject" (never possible) |
| gotoinvoiceButton "Go to Partial" | LinkButton, `OnClientClick=sweetAlertConfirm_Submit` | — | visible only when status = Partial Dues | `gotoinvoiceButton_Click` |
| btnFinalSubmit "ALL Submit" | LinkButton, confirm | — | — | `btnFinalSubmit_Click` |
| lblCount | Label | — | "Total Net Amount: 0" | |

## 8. Field Mapping
| UI | Variable | Handler | BLL | DAL | SP param | Table |
|---|---|---|---|---|---|---|
| salesCenter, route, territory, invoice text | `GenerateParam()` string appended to WHERE | `GridView()` :467-506 | `SalesReturnNewDAL.LoadOrderWithInvoiceSP` (directly) | :301-330 | `@param` (SQL fragment) | `sp_BeforesndSalesReturnList` → tblOrder/tblInvoice (body UNKNOWN) |
| orderGridView row InvoiceId | `Session["InvoiceId"]` | `gotoinvoiceButton_Click` :349 | — | — | — | — |
| hfInvoiceNo | `SavePaymentConformationFull(InvoiceNo, LoginName, today)` | `btnFinalSubmit_Click` :587 | `SalesReturnBLL.SavePaymentConformationFull` | `SalesReturnDAL.cs:259-266` | `@InvoiceNo,@UpdateBy,@UpdateDate` | `sp_PaymentConformationFull` (body UNKNOWN) |

## 9. Dropdown Mapping
See §7. Static reason list (`reasonReturnDropDownList`): 1 Cash Short, 2 No Order, 3 Wrong Order, 4 Damaged Broken, 5 Quality Issue, 11 Shop Closed, 6 Order Cancelled, 7 Slow Moving, 8 Price Error, 9 Return Others, 10 Date Expired, 11 Partial return-cash short (**duplicate value 11**), 12 Partial return-Excess order, 13 Double order.

## 10. Button Action Mapping
| Button | Effect |
|---|---|
| Search (`Button1_Click`) | resets total label, rebinds grid |
| Route change (`rootDropDownList_SelectedIndexChanged`) | loads territories, clears grid |
| DC change | reloads routes (guarded by `SelectedSalesCenterId`), clears grid |
| Go to Partial | `Session["InvoiceId"]=InvoiceId`; redirect to PAGE-298 |
| ALL Submit | for each ticked row where status = "Full": `sp_PaymentConformationFull`; **never runs** because the only status is "Partial Dues"; message "Delivery Invoice Created Successsfully!" only if `status>0` |
| Cancel | redirect `DelivaryInvoiceCreationAuto.aspx` |

## 11. Frontend Flow
Page_Load: `DropDownlist()` (DCs, manufacturer, first DC, routes for DC), `SessionChoose()`; if `Session["DelMarketId"]` restore market and grid. User picks route → territory → (optional invoice text) → Search. Grid rows show status "Partial" and *Go to Partial* button; ticking rows totals net amount client-postback.

## 12. Backend Flow
`GenerateParam` builds (string concatenation, no escaping):
```
and tblOrder.TerritoryId='<ddlTerritoryName>' and dbo.tblOrder.ComUnitId='<DC>' AND tblOrder.DistributionRouteId='<route>'
[AND tblInvoice.InvoiceNo like'%<invoicenoTextBox>%'] and PaymentInvoiceNo is not null and SndReturnInvoiceNo is null order by tblInvoice.InvoiceNo Asc
```
passed as `@param` to `sp_BeforesndSalesReturnList` (`SalesReturnNewDAL.cs:316`). The grid then sets each row's status from `LoadingSummaryStatus`, shows the *Go to Partial* button for "Partial Dues", and sets `Session["DelMarketId"]`.

## 13. API Mapping
ASMX `GetAllInvoice` autocomplete (not read).

## 14. Service Mapping
`OrderInfoBLLSalesReturn.LoadSC/LoadManufac/LoadOrderWithInvoice`, `SalesReturnBLL.SavePaymentConformationFull`, `SeedDataDAL.GetRouteInfoforSalesReturn2ndTimes`, `RouteInformationDAL.GetforsndreturnTerritoryByRouteInformationDA_DDLId`.

## 15. Repository Mapping
`SalesReturnNewDAL.LoadOrderWithInvoiceSP(param)` → `accessManager.GetDataTable("sp_BeforesndSalesReturnList", [@param])`; `SalesReturnDAL.SavePaymentConformationFull` → `RunStoreProcedure("sp_PaymentConformationFull")`.

## 16. Stored Procedure Mapping
| SP | Body | Notes |
|---|---|---|
| `sp_BeforesndSalesReturnList(@param)` | not in repo | executes the concatenated filter — injection surface |
| `sp_PaymentConformationFull(@InvoiceNo,@UpdateBy,@UpdateDate)` | not in repo | effect UNKNOWN (payment confirmation); unreachable from UI today |
| `sp_GET_forsndReturnTerritoryByRouteInformationDDL` | not in repo | territory list |

## 17. Database Mapping
Read only (via SP): tblOrder, tblInvoice (`PaymentInvoiceNo`, `SndReturnInvoiceNo`, `LoadingSummaryStatus`, `IsAdjustInvoice`, `TpGrandTotal`, `chkStatus`), tblCustMaster, tblMarket, DistributionRoute tables. Writes only through `sp_PaymentConformationFull` (dead). **No stock effect, no posting.**

## 18. Validation Matrix
| Check | Where | Message |
|---|---|---|
| Grid non-empty on ALL Submit | `DataValidation` :626-632 | Data Can not be Empty! |
| ≥1 row ticked | :651-657 | Please Select at least one Row! |
| Territory/route not selected | not validated (empty strings simply match nothing) | — |

## 19. Business Rules
| BR | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M08-046 | The worklist lists invoices of the chosen DC + route + territory that are **payment-confirmed (`PaymentInvoiceNo` not null) and not yet second-returned (`SndReturnInvoiceNo` null)**, sorted by invoice no; optional partial invoice-no search. | .aspx.cs:447-463 | code-behind + SP | CONFIRMED (SP body UNKNOWN) |
| BR-M08-047 | Only status "Partial Dues" exists in the row dropdown (disabled); it exposes *Go to Partial*; an invoice flagged `IsAdjustInvoice=True` forces index 0 and disables the dropdown. | .aspx:341-346; .aspx.cs:475-497,531-567 | UI + code-behind | CONFIRMED |
| BR-M08-048 | "ALL Submit" only acts on ticked rows whose status equals "Full", which cannot occur — the bulk full-confirmation path is dead. | .aspx.cs:576-609 | code-behind | CONFIRMED |
| BR-M08-049 | The DC list is restricted to the user's DCs (`tblUserCompanyUnit`); first DC is auto-selected; routes come from the "second-times sales return" route SP for that DC. | .aspx.cs:123-143,158-167 | code-behind | CONFIRMED |

## 20. Permission Matrix
Uses `Session["UserId"]` to filter DCs (exception swallowed if missing). No role checks. Hidden link exposes UserId/LoginName to an external host (`.aspx.cs:714`).

## 21. Status Lifecycle
Reads `LoadingSummaryStatus` (values seen in code: "Partial Dues", "Full", "Reject", "Completed" — the last three only in commented code). Handoff: after PAGE-298 sets `SndReturnInvoiceNo`, the invoice drops out of this list.

## 22. Error Handling
Nearly all handlers use empty `catch {}` (DropDownlist, route loading) — failures silently produce empty lists; `LoadTerritoryByRoute` alerts "Territory load failed: <ex.Message>".

## 23. Audit
None.

## 24. Integration
None.

## 25. Reports
None.

## 26. Dependencies
PAGE-298; `SInventoryWebService.asmx` (`GetAllInvoice`); route/territory tables; SweetAlert helpers.

## 27. Test Cases
| # | Type | Steps | Expected | BR |
|---|---|---|---|---|
| T1 | Positive | DC, route, territory, Search | invoices with payment no and no SndReturn no listed | 046 |
| T2 | Negative | Invoice search text `'; DROP…` | concatenated into SP param (injection risk) | 046 |
| T3 | Edge | Tick rows | "Total Net Amount" = Σ visible net amounts | — |
| T4 | Positive | Go to Partial | redirects to PAGE-298 with that invoice | 047 |
| T5 | Negative | ALL Submit with rows ticked | nothing happens, no message | 048 |
| T6 | Edge | User with a single mapped DC | DC auto-selected; routes load | 049 |

## 28. Reverse Engineering References
`SalesReturnList_new.aspx.cs` (all); `SalesReturnNewDAL.cs:301-330`; `SalesReturnDAL.cs:259-266`.

## 29. Spec vs Actual
None existed.

## 30. Known Gaps
SQL-fragment injection (`GenerateParam`); dead "Full"/bulk path; duplicated reason value 11; hard-coded external IP link with user id; empty catch blocks; `Session["DelMarketId"]` legacy coupling; static message "Delivery Invoice Created Successsfully!" (copy from delivery page); `SavePayment/SavePaymentMultiple` methods are dead.

## 31. Change Impact
Feeds PAGE-298 through `Session["InvoiceId"]`. `SndReturnInvoiceNo` written by PAGE-298 controls what disappears from this list. `sp_BeforesndSalesReturnList` may be shared with other lists (UNKNOWN).
