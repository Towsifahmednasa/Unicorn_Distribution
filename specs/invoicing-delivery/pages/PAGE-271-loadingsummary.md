---
page_id: PAGE-271
module: M07
page: LoadingSummary
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---

# PAGE-271 — LoadingSummary

> **Evidence level:** Tier A - reviewed 2026-09-19 by reading the .aspx, .aspx.cs, the BLL/DAL methods reached, and every SP/SQL body present in the repository. Confidence tags: CONFIRMED (read in implementation), INFERRED (implied by several references), UNKNOWN (not derivable; SP bodies are absent from the repo except where stated). Module: M07 - Proforma, Invoicing & Delivery.


## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-271 |
| Module | M07 - Proforma, Invoicing & Delivery |
| Page name | LoadingSummary |
| Kind | **Entry / Transaction** (the generated spec said "Report / Dashboard" - wrong): sales-confirmation / loading-summary status update of proforma invoices, with delivery confirmation and accounting posting for "Full Dues" |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_LoadingSummary |
| Size | aspx 589 lines, code-behind 815 lines |
| Suspected legacy/duplicate | Copy/paste twin of `DelivaryInvoiceCreationAfterSalesConfirm` (PAGE-264): same `SavePayment`, `SavePaymentMultiple`, `NewMethod` dead helpers. Live page (redirects to `DelivaryInvoiceCreationForCustomerByOrderAuto_Loading.aspx`). |

## 2. Purpose
CONFIRMED (labels/code): filter by **Sales Center** and **Invoice Date**, "Search" lists the day's proforma invoices that are not yet delivery-confirmed (`DelivaryInvoiceNo IS NULL`), each with a status dropdown (Sales Confirm = "Full Dues", Partial Confirm = "Partial Dues", or "Cash"/"Rejection" for specific rows), an "Is Common Status" checkbox that applies one status to all rows, and "ALL Submit". "Sales Confirm" performs the delivery confirmation and posts the sales journal; "Partial Confirm" hands the invoice to the partial screen.

## 3. Business Objective
INFERRED (from status names, SP names and downstream `SP_Audit_Sales_Accounts_Voucher_Posting.sql:113-174`): after goods are loaded and delivered, the DC user confirms per invoice whether the delivery was full (sales confirmed -> delivery invoice number/quantities set, revenue and COGS vouchers posted with TableID 2014), partial (quantities are edited on the partial screen), cash, or rejected. This is the pivot between the proforma world (stock reserved in tblDCStore) and the delivery/accounting world (`DeliveryInvoiceStatus`, `DeliveryTpGrandTotal`, vouchers in `CSTL_ACCDB_Unicorn`).

## 4. Navigation
Menu DB-driven -> UNKNOWN. Outbound: `DelivaryInvoiceCreationForCustomerByOrderAuto_Loading.aspx` (row "Go to Partial", `.aspx.cs:272`; see `../order-management/pages/PAGE-215-delivaryinvoicecreationforcustomerbyorderauto-loading.md`), external URL on the hidden "Update Old Data" button (`Visible="false"`, `.aspx:99`; `.aspx.cs:761-767`), self (Reset, `.aspx.cs:49`).

## 5. Route
`~/SInventory_UI/LoadingSummary.aspx` (file-based WebForms route; no `MapPageRoute` alias).


## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/LoadingSummary.aspx |
| Code-behind | Solution.Web/SInventory_UI/LoadingSummary.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL | OrderInfoBLL (LoadSC, LoadManufac, LoadDeliveryDisRouteforInvoice, LoadOrderWithInvoice), InvoiceBLL (SaveFullInvoice :37, UP_LoadingSummaryInvoice :53, PostDeliveryConfFullJournal :42), ProformaOrInvoiceReturnBLL (unused field) |
| DAL | OrderInfoDAL.LoadSummaryList (`OrderInfoDAL.cs:477-503`), InvoiceDAL (`:213-221`, `:222-230`, `:242-250`), SeedDataDAL.GettblInvoiceStatusddlALL (`SeedDataDAL.cs:115-145`), RouteInformationDAL.GetDelTerritoryByRouteInformationDA_DDLId (`RouteInformationDAL.cs:365-385`), ClsCommonInternalDAL.UpdateAction (`:450-464`) |

## 7. UI Control Inventory
| Control ID | Type | Label | Required | Default | Read-only / Enabled / Visible | Data source | Handler / error |
|---|---|---|---|---|---|---|---|
| salesCenterDropDownList | DropDownList | "Sales Center:" | YES: `Button1_Click` requires SelectedValue != "" | `SelectedIndex = 1` | enabled, AutoPostBack | `OrderInfoBLL.LoadSC(ddl, Session["UserId"])` | `salesCenterDropDownList_SelectedIndexChanged` -> `getRouteInfo()` (route list for that SC and date) and clears grid |
| DateTextBox | TextBox (datepicker) | "Invoice Date:" | YES (`DateTextBox.Text != ""`) | `DateTime.Now.ToString("dd MMMM, yyyy")` | AutoPostBack | typed/picker | `DateTextBox_TextChanged` -> `getRouteInfo()` |
| manufacDropDownList, marketDropDownList, rootDropDownList ("Route"), ddlTerritoryName ("Territory Name"), invoicenoTextBox ("Invoice No") | various | - | none | placeholder `--------Select---------` (empty value) | **hidden** (`runat="server" visible="False"`) | tblManufacturer / `LoadDeliveryDisRouteforInvoice` (routes of un-delivered invoices of that day, `OrderInfoDAL.cs:1056-1084`) / `sp_GET_DelTerritoryByRouteInformationDDL` | Route and Territory still feed `GenerateParam()` but are always empty because their controls are never rendered/changed |
| Search (submitButton) / Reset | LinkButton | "Search" / "Reset" | - | - | enabled | - | `Button1_Click` / `cancelButton_Click` (redirect self) |
| lblCount | Label | "Total Net Amount : 0" | - | - | - | check events | sum of ticked `lblTpGrandTotal` |
| chk_Common | CheckBox | " Is Common Status" | - | unchecked | AutoPostBack | - | `chk_Common_OnCheckedChanged` -> `ISCommonDate()` |
| ddlStatus | DropDownList | (common status) | - | first | `Enabled=false` initially; enabled when chk_Common checked (`ISCommonDate`) - note `chk_Common_OnCheckedChanged` first sets `ddlStatus.Enabled = false` and then `ISCommonDate()` re-enables it when checked | markup: `Full Dues` ("Sales Confirm"), `Partial Dues` ("Partial Confirm") | `ddlStatus_SelectedIndexChanged` |
| orderGridView | GridView `DataKeyNames="ComUnitId,ManufacId,OrderId,InvoiceId,MarketId,CustomerMasterId"` | SL#, [checkbox], Invoice No, Invoice Date, Customer Code, Customer Name, Order Sender Name, Market, Amount, Status, Submit | - | rows from `sp_LoadingSummary` | - | - | - |
| hfIsAdjustInvoice, hfIsPartialCheck, hfInvoiceId, hfInvoiceNo | HiddenField | - | - | SP columns `IsAdjustInvoice`, `hfIsPartialCheck`(`Partial Show`/`Partial Off`), `InvoiceId`, `InvoiceNo` | hidden | SP | - |
| lblTpGrandTotal | Label | Amount | - | `TpGrandTotal` | - | SP | - |
| statusDropDownList | DropDownList | "Status" | - | markup items `Full Dues` (Sales Confirm), `Partial Dues` (Partial Confirm) | AutoPostBack; items replaced from `sp_GET_InvoiceStatusddlAll` for special rows (BR-M07-027) | markup / SP | `statusDropDownList_SelectedIndexChanged` toggles the "Go to Partial" button; `statusDropDownList_OnTextChanged` (leftover, references `Reject`) |
| gotoinvoiceButton | LinkButton | "Go to Partial" | - | - | `Visible=false`; visible when status == "Partial Dues"; confirm dialog | - | `gotoinvoiceButton_Click` |
| btnFinalSubmit | LinkButton | "ALL Submit" | see §18 | - | confirm dialog | - | `btnFinalSubmit_Click` |

## 8. Field Mapping
| UI | Handler | BLL -> DAL -> SP param | Column |
|---|---|---|---|
| hfInvoiceNo.Value | `btnFinalSubmit_Click` | `InvoiceBLL.SaveFullInvoice(InvoiceNo, LoginName, dd-MMM-yyyy)` -> `sp_DeliveryConformationFull_New @InvoiceNo,@UpdateBy,@UpdateDate` | UNKNOWN (body absent) |
| hfInvoiceId.Value + statusDropDownList.SelectedValue | same | `InvoiceBLL.UP_LoadingSummaryInvoice(InvoiceId, LoginName, status)` -> `sp_UP_LoadingSummary @InvoiceId,@UpdateBy,@LoadingSummaryStatus` | UNKNOWN; column named `LoadingSummaryStatus` (read back by PAGE-264/`SalesReturnList_new`) |
| DataKeys["InvoiceId"] | same | `InvoiceBLL.PostDeliveryConfFullJournal(InvoiceId)` -> `ClsCommonInternalDAL.UpdateAction("sp_Sales_DeliverySalesPosting",@InvoiceId)` | accounting vouchers (TableID 2014 in the audit script) - UNKNOWN body |
| DateTextBox, salesCenter | `GenerateParam()` | `OrderInfoDAL.LoadSummaryList(param)` -> `sp_LoadingSummary(@param)` | dynamic SQL fragment |

## 9. Dropdown Mapping
| Dropdown | Loader | SQL |
|---|---|---|
| salesCenterDropDownList | `OrderInfoBLL.LoadSC(ddl, UserId)` | tblCompanyUnit filtered by tblUserCompanyUnit |
| rootDropDownList | `OrderInfoBLL.LoadDeliveryDisRouteforInvoice(ddl, ComUnitId, date)` (`OrderInfoDAL.cs:1056-1084`) | `select distinct ord.DistributionRouteId, ord.DistributionRoute_Ord from tblInvoice Inv inner join tblOrder ord on ord.OrderId=Inv.OrderId where DelivaryInvoiceNo is null and Inv.ComUnitId=@ComUnitId and convert(date, Inv.InvoiceDate)=@InvoiceDate` (parameterised; placeholder inserted) |
| ddlTerritoryName | `RouteInformationDAL.GetDelTerritoryByRouteInformationDA_DDLId(routeId, date)` | `sp_GET_DelTerritoryByRouteInformationDDL` (absent) |
| statusDropDownList (special rows) | `SeedDataDAL.GettblInvoiceStatusddlALL(isForSalesConfirm, IsShowforPartial, IsShowforRejection, IsforReturn)` | `sp_GET_InvoiceStatusddlAll` (absent); Text/Value pairs |

## 10. Button Action Mapping
| Button | Handler | Chain | DB change |
|---|---|---|---|
| Search | `Button1_Click` (`.aspx.cs:280-303`) | `GridView()` -> `LoadSummaryList(GenerateParam())` -> `sp_LoadingSummary` | none |
| ALL Submit | `btnFinalSubmit_Click` (`.aspx.cs:603-672`) | per ticked row by status - see BR-M07-029/030 | tblInvoice (via SPs), accounting vouchers |
| Go to Partial | `gotoinvoiceButton_Click` (`.aspx.cs:258-278`) | `Session["InvoiceId"]=...; Redirect(...Auto_Loading.aspx)` | none |
| Is Common Status | `ISCommonDate()` (`.aspx.cs:344-415`) | copies ddlStatus to every row's status dropdown; shows Go-to-Partial for Partial Dues | none |

## 11. Frontend Flow
UpdatePanel; SweetAlert confirm (`sweetAlertConfirm_Submit`); datepicker via master. `ISCommonDate` is server-side. Check events recompute the amount label on each postback. The date box posts back to reload the route list only.

## 12. Backend Flow
| Handler | Source | Flow | try/catch | Txn |
|---|---|---|---|---|
| Page_Load | `.aspx.cs:23-45` | date=now; lists; restore `Session["DelMarketId"]` and re-search | no | no |
| GridView | `.aspx.cs:417-545` | SC required; `LoadSummaryList`; per row reload status list from `sp_GET_InvoiceStatusddlAll` when `hfIsAdjustInvoice=="True"` (partial shown) or `hfIsPartialCheck=="Partial Off"` (rejection shown); `Session["DelMarketId"]=marketDropDownList.SelectedValue` | swallow per row | no |
| btnFinalSubmit_Click | `.aspx.cs:603-672` | `DataValidation()`; loop rows; per ticked row branch by status; `if (status == 1)` show success and `GridView()` | inner none | none - separate connection per SP |
| DataValidation | `.aspx.cs:674-710` | empty grid -> "Data Can not be Empty!"; nothing ticked -> "Please Select at least one Row!" | - | - |

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.


## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderInfoBLL.LoadSC | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:91 |
| OrderInfoBLL.LoadManufac | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:117 |
| OrderInfoBLL.LoadDeliveryDisRouteforInvoice | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:128 |
| OrderInfoBLL.LoadOrderWithInvoice | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:143 |
| CustPaymentBLL.UpdateInvoicePaymentAmount | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:114 |
| CustPaymentBLL.SaveCustPayment | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:33 |
| CustPaymentBLL.SaveCustDetail | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:127 |
| InvoiceBLL.SaveFullInvoice | Library.BLL/SInventory_BLL/InvoiceBLL.cs:38 |
| InvoiceBLL.UP_LoadingSummaryInvoice | Library.BLL/SInventory_BLL/InvoiceBLL.cs:54 |
| InvoiceBLL.PostDeliveryConfFullJournal | Library.BLL/SInventory_BLL/InvoiceBLL.cs:43 |


## 15. Repository Mapping
| DAL | Source | SQL/SP | Params |
|---|---|---|---|
| OrderInfoDAL.LoadSummaryList | `OrderInfoDAL.cs:477-503` | `sp_LoadingSummary` | `@param` = raw WHERE fragment |
| InvoiceDAL.SaveFullInvoice | `InvoiceDAL.cs:213-220` | `RunStoreProcedure("sp_DeliveryConformationFull_New")` | @InvoiceNo, @UpdateBy, @UpdateDate |
| InvoiceDAL.UP_LoadingSummaryInvoice | `InvoiceDAL.cs:242-250` | `RunStoreProcedure("sp_UP_LoadingSummary")` | @InvoiceId (actually InvoiceId text), @UpdateBy, @LoadingSummaryStatus |
| InvoiceDAL.PostDeliveryConfFullJournal | `InvoiceDAL.cs:222-230` | `UpdateAction("sp_Sales_DeliverySalesPosting")` (exceptions swallowed -> false) | @InvoiceId |
| SeedDataDAL.GettblInvoiceStatusddlALL | `SeedDataDAL.cs:115-145` | `sp_GET_InvoiceStatusddlAll` | @isForSalesConfirm, @IsShowforPartial, @IsShowforRejection, @IsforReturn |

## 16. Stored Procedure Mapping
| SP | Called from | Body | Known |
|---|---|---|---|
| sp_LoadingSummary | LoadSummaryList | absent | returns grid columns (InvoiceNo, InvoiceDate, CustomerCode, CustomerName, OrderSenderName, MarketName, TpGrandTotal, IsAdjustInvoice, hfIsPartialCheck, InvoiceId, ComUnitId, ManufacId, OrderId, MarketId, CustomerMasterId) |
| sp_DeliveryConformationFull_New | SaveFullInvoice | absent | UNKNOWN effect; downstream code shows `tblInvoice.DelivaryInvoiceNo`, `DeliveryInvoiceStatus`('Full'/'Partial'), `DeliveryTpGrandTotal`, `DeliveryDate` exist (`SP_Audit_Sales_Accounts_Voucher_Posting.sql:113-124`, `LIVE_DB_UPDATE...sql:1335`) - INFERRED that this SP sets them |
| sp_UP_LoadingSummary | UP_LoadingSummaryInvoice | absent | sets `LoadingSummaryStatus` (INFERRED from parameter name and reads in PAGE-264/`SalesReturnList_new.aspx.cs:487`) |
| sp_Sales_DeliverySalesPosting | PostDeliveryConfFullJournal | absent | posts sales (and inventory valuation) voucher for TableID 2014 (INFERRED from audit script) |
| sp_GET_InvoiceStatusddlAll, sp_GET_DelTerritoryByRouteInformationDDL | dropdowns | absent | UNKNOWN |

## 17. Database Mapping
- READ (inline): tblCompanyUnit, tblUserCompanyUnit, tblManufacturer, tblMarket, tblOrder, tblInvoice (route dropdown).
- READ via SP: `sp_LoadingSummary` base tables UNKNOWN (fragment names tblOrder.DistributionRouteId, tblOrder.TerritoryId, tblOrder.ComUnitId, tblInvoice.InvoiceDate, tblInvoice.InvoiceNo, DelivaryInvoiceNo).
- WRITTEN via SPs (bodies absent): tblInvoice status/delivery columns (INFERRED), accounting voucher tables in `CSTL_ACCDB_Unicorn` (INFERRED: `tblDebitCreditVoucherMaster/Detail`, TableID 2014).

## 18. Validation Matrix
| Rule | Layer | Condition | Message |
|---|---|---|---|
| Sales Center and Date | server `Button1_Click` | SC or date empty | faildalert "Please Select all Filtering Criteria!" |
| SC on load | `GridView()` | SC empty | faildalert "Please input Sales Center!" |
| Grid not empty | `DataValidation` | rows == 0 | faildalert "Data Can not be Empty!" |
| Any row ticked | `DataValidation` | none ticked | faildalert "Please Select at least one Row!" |
| Success | `status == 1` after the last call | - | ShowSuccesalert "Delivery confirmation Created Successsfully!" (typo in source) |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M07-026 | Search needs Sales Center and Invoice Date. The list SP receives a fragment built as: `[AND tblOrder.DistributionRouteId='<route>'] [AND tblOrder.TerritoryId='<terr>'] [AND tblOrder.ComUnitId='<SC>'] [AND CONVERT(Date, tblInvoice.InvoiceDate) = CONVERT(Date,'<date text>')] AND DelivaryInvoiceNo IS NULL ORDER BY tblInvoice.InvoiceNo ASC`. So only same-day, not-yet-delivery-confirmed invoices are listed. Route/territory are always empty (hidden controls). | `LoadingSummary.aspx.cs:306-333` | page builds SQL fragment | CONFIRMED (fragment) / UNKNOWN (SP) |
| BR-M07-027 | Status choices: default `Full Dues` ("Sales Confirm") / `Partial Dues` ("Partial Confirm"). Rows with `IsAdjustInvoice = True` get the list from `sp_GET_InvoiceStatusddlAll(isForSalesConfirm=1, IsShowforPartial=1, IsShowforRejection=0, IsforReturn=0)`; rows with `hfIsPartialCheck = "Partial Off"` get `(1,0,1,0)` i.e. partial hidden and rejection shown. Resulting value strings handled: `Full Dues`, `Cash`, `Rejection`, `Partial Dues`. | `.aspx.cs:462-541`; `.aspx:381-388` | code-behind + SP | CONFIRMED (branches) / UNKNOWN (list contents) |
| BR-M07-028 | "Is Common Status": when ticked, `ddlStatus` value is copied into every row's status dropdown (raises an exception, swallowed, if the value is not in a row's list); unticking resets every row to index 0. | `.aspx.cs:335-415` | code-behind (try/catch swallow) | CONFIRMED |
| BR-M07-029 | **Sales Confirm ("Full Dues")** per ticked row runs, in this order and without a transaction: (1) `sp_DeliveryConformationFull_New(InvoiceNo, LoginName, today)`; (2) `sp_UP_LoadingSummary(InvoiceId, LoginName, "Full Dues")`; (3) `sp_Sales_DeliverySalesPosting(InvoiceId)` accounting posting; if step 3 returns true the grid is re-searched. Step 3 exceptions are swallowed (`UpdateAction` catch -> false) so posting failures are invisible; steps 1-2 leave the invoice confirmed without a journal in that case. | `.aspx.cs:615-637`; `InvoiceDAL.cs:213-230, 242-250`; `ClsCommonInternalDAL.cs:450-464` | code-behind + SPs | CONFIRMED (order) / UNKNOWN (SP effects) |
| BR-M07-030 | **"Cash"** and **"Rejection"** rows only run `sp_UP_LoadingSummary(InvoiceId, LoginName, status)` (status flag), with no delivery confirmation call and no journal. | `.aspx.cs:639-657` | code-behind | CONFIRMED |
| BR-M07-031 | **"Partial Dues"** has no submit action in "ALL Submit"; each such row shows "Go to Partial" which saves `Session["InvoiceId"]` and redirects to `DelivaryInvoiceCreationForCustomerByOrderAuto_Loading.aspx`, where quantities/dues are adjusted (M06). | `.aspx.cs:258-278, 780-798` | code-behind | CONFIRMED |
| BR-M07-032 | The success alert depends only on the `status` variable left by the LAST SP call of the loop (`RunStoreProcedure` returns 1 only when `ExecuteNonQuery() > 0`); errors in earlier rows are invisible, and an SP using `SET NOCOUNT ON` would return -1 -> 0 -> no success message even though the work was done (INFERRED). | `.aspx.cs:603-672`; `ClsCommonInternalDAL.cs:551-586` | code-behind + DAL | CONFIRMED (control flow) / INFERRED (NOCOUNT) |
| BR-M07-033 | Defect: after a successful "Full Dues" journal posting `GridView()` is called INSIDE the row loop; it rebinds `orderGridView` (unchecked rows, fewer rows) while the loop keeps indexing `rowindex` against the rebound grid, so remaining ticked rows are skipped or mismatched. Since posted invoices drop out of the list (`DelivaryInvoiceNo IS NULL`), a multi-row "Full Dues" submit processes only the first row reliably (INFERRED consequence; the rebind itself is CONFIRMED). | `.aspx.cs:628-636, 417-424` | code-behind | CONFIRMED (rebind) / INFERRED (skipping) |

## 20. Permission Matrix
- Session: `UserId` (SC list), `LoginName` (SP `@UpdateBy` on every write), `DelMarketId`.
- No role/UserType check. Access is menu-based only; any user with the menu item can confirm any invoice of any sales center posted in the parameter (the fragment uses the posted SC value; only the ASP.NET event validation of the bound dropdown restricts it).
- Confirm dialogs are client-side only.

## 21. Status Lifecycle
Status values and transitions verified in code (values stored by SPs whose bodies are absent):
- `LoadingSummaryStatus`: (none) -> `Full Dues` | `Cash` | `Rejection` via `sp_UP_LoadingSummary`; `Partial Dues` is chosen but not persisted here (handled by the partial screen).
- Delivery confirmation ("Sales Confirm"): `DelivaryInvoiceNo IS NULL` (proforma) -> not null (delivery invoice) via `sp_DeliveryConformationFull_New` (INFERRED); `DeliveryInvoiceStatus` 'Full'/'Partial' seen in `SP_Audit_Sales_Accounts_Voucher_Posting.sql:174`.
- Accounting: voucher rows keyed `TableID = 2014`, `CommonPK_ID = InvoiceId` appear after posting (audit script).
- After this page the invoice appears on PAGE-264's worklist (`PaymentInvoiceNo IS NULL`).
README "Status lifecycles" consolidates this.

## 22. Error Handling
`GridView()` per-row try/catch swallows; `ISCommonDate` swallows; `UpdateAction` swallows accounting errors; SP call exceptions elsewhere unhandled. Messages: SweetAlert `faildalert` / `ShowSuccesalert`.

## 23. Audit
`@UpdateBy` = LoginName, `@UpdateDate` = today (string) passed to the delivery confirmation; `sp_UP_LoadingSummary` also gets `@UpdateBy`. Stored audit columns UNKNOWN (tblInvoice.UpdateDate is read by `LIVE_DB_UPDATE...sql:1345`, so it exists). No local log.

## 24. Integration
Accounting integration: `sp_Sales_DeliverySalesPosting` writes to the accounting DB `CSTL_ACCDB_Unicorn` (cross-database name appears in audit and report queries: `SP_Audit_Sales_Accounts_Voucher_Posting.sql:145-165`). External URL "Update Old Data" (hidden). No SAP.

## 25. Reports
Reports opened from downstream pages: delivery invoice `rptInvoiceForCustomerDelivary.rpt`; see `../reports/`. This page opens none.

## 26. Dependencies
BLL: OrderInfoBLL, InvoiceBLL, ProformaOrInvoiceReturnBLL. DAL: OrderInfoDAL, InvoiceDAL, SeedDataDAL, RouteInformationDAL, ClsCommonInternalDAL. DAO: CustPayment, CustPaymentDetail (dead helpers). SPs: 6 (all absent).

## 27. Test Cases
| ID | Scenario | Steps | Expected | BR |
|---|---|---|---|---|
| TC-271-01 | Missing filter | clear date, Search | faildalert "Please Select all Filtering Criteria!" | BR-M07-026 |
| TC-271-02 | List | SC + date with 3 proforma invoices | only invoices dated that day with `DelivaryInvoiceNo` null | BR-M07-026 |
| TC-271-03 | Status list default | load | Sales Confirm / Partial Confirm | BR-M07-027 |
| TC-271-04 | Adjust invoice | row `IsAdjustInvoice=True` | list from `sp_GET_InvoiceStatusddlAll(1,1,0,0)` | BR-M07-027 |
| TC-271-05 | Partial Off | row `hfIsPartialCheck='Partial Off'` | partial hidden, rejection shown | BR-M07-027 |
| TC-271-06 | Common status | tick, choose Partial Confirm | all rows switch and show Go-to-Partial | BR-M07-028 |
| TC-271-07 | Sales confirm one | tick 1 row Full Dues, ALL Submit | 3 calls in order; success alert; row disappears | BR-M07-029 |
| TC-271-08 | Sales confirm several | tick 3 rows | **defect**: only first reliably processed | BR-M07-033 |
| TC-271-09 | Cash / Rejection | tick row with those statuses | only `sp_UP_LoadingSummary` | BR-M07-030 |
| TC-271-10 | Partial | click Go to Partial | redirect with `Session["InvoiceId"]` | BR-M07-031 |
| TC-271-11 | Nothing ticked | ALL Submit | "Please Select at least one Row!" | - |
| TC-271-12 | Journal failure | make `sp_Sales_DeliverySalesPosting` fail | no message, invoice confirmed without voucher | BR-M07-029 |
| TC-271-13 | Injection | tamper DateTextBox text | fragment altered | GAP-M07-009 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/LoadingSummary.aspx`, `Solution.Web/SInventory_UI/LoadingSummary.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).


## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | page present | n/a | MISSING | - |
| Delivery confirm chain | None existed | 3 SP calls no txn | SP bodies absent | UNKNOWN | BR-M07-029 |
| Multi-row submit | None existed | grid rebound inside loop | - | GAP | BR-M07-033 |
| Route/territory filters | None existed | hidden, always empty | - | GAP | BR-M07-026 |

## 30. Known Gaps
- SQL injection surface via `GenerateParam()` (date text, dropdown values) executed inside `sp_LoadingSummary` (GAP-M07-009).
- Non-atomic 3-step confirmation; swallowed journal errors (GAP-M07-013).
- Grid rebinding inside submit loop (GAP-M07-014).
- Copy/paste dead code (`SavePayment*`), commented UI, typo "Successsfully"; hidden filters.
- Date filter uses culture-dependent text `dd MMMM, yyyy` parsed by SQL `CONVERT(Date, '<text>')` (INFERRED language dependency).
- Success flag from last call only.
- Six SP bodies absent (GAP-M07-012).

## 31. Change Impact
- Twin: PAGE-264 (`DelivaryInvoiceCreationAfterSalesConfirm`), and by-order loading page PAGE-215 (M06).
- `InvoiceDAL.SaveFullInvoice` -> `sp_DeliveryConformationFull_New` also used by other delivery pages (grep `SaveFullInvoice`); `sp_UP_LoadingSummary` counterpart in `SalesReturnDAL.cs:274-284` (sales return module).
- Downstream: PAGE-264 worklist, `PaymentPartial.aspx` (M09), accounting audit script, delivery invoice report.
- Any change to status vocabulary ("Full Dues", "Partial Dues", "Cash", "Rejection") must be mirrored in `sp_GET_InvoiceStatusddlAll`, PAGE-264 and `SalesReturnList_new.aspx.cs:487`.
