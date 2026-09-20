---
page_id: PAGE-217
module: M06
page: InvoiceCreationByOrder
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-217 — InvoiceCreationByOrder ("Proforma Invoice Creation" list)

> **Evidence level:** Tier A — `Solution.Web/SInventory_UI/InvoiceCreationByOrder.aspx(.cs)`, `OrderInfoDAL` (list/batch/lookup methods), `sp_LoadOrderListForOrderCreationbyTerri` body (`LIVE_DB_UPDATE…sql:1224-1373`), `sp_Check_anomalyInvoiceDetails` (`SP_Fix_Invoice_Anomaly_And_Sync.sql:43-79`), `sp_Delete_ProformaInvoice` (`sp_Delete_ProformaInvoice_Update.sql`). **`sp_Process_ProformaInvoiceByOrderId`, `sp_GET_checkTodaysAlreadyInviceGenerateByCustId`, `sp_Get_NewReceiveableList`, route/DA DDL SPs: bodies absent (UNKNOWN).** aspx JS/markup only partly read (ui/frontend PARTIAL). Cross-module: the proforma this page creates is the M07 (Invoice) entity `tblInvoice`.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-217 |
| Module | M06 (order→invoice entry point; **cross-link M07**) |
| Kind | List + bulk action + entry to invoice form |
| Menu (static master) | `MasterPages/MasterPage.master:1200` "Proforma Invoice Creation" |
| Class | SInventory_UI_InvoiceCreationByOrder (**same class name is reused by** DoctorInvoiceCreationByOrder.aspx.cs and SampleInvoiceCreationByOrder.aspx.cs — cloned pages) |
| Code-behind LOC | 682 |

## 2. Purpose
Shows all customer orders of a distribution centre that are ready to be invoiced, indicates for each whether the customer's credit limit/aging allows invoicing, and lets the dispatcher open the per-order invoice form (PAGE-218) — or (bulk mode) generate proforma invoices for several orders at once for a chosen DA (delivery agent). CONFIRMED.

## 3. Business Objective
Turn accepted orders into proforma invoices while enforcing credit control (blocked button when the customer's exposure would exceed the approved limit or the oldest unpaid invoice is older than the customer's day-limit) and preventing double invoicing. CONFIRMED (SP `Permission` column, `GridView()`).

## 4. Navigation
Menu → this page. Outbound: `gotoinvoiceButton` sets `Session["OrderId"]`, `Session["RootNameId"]=0` and redirects to `InvoiceCreationForCustomerByOrder.aspx` (`.cs:529-532`). Report button opens `../SInventory_RPTVIEW/ProformaReportPrintViewer.aspx` (`:150`). Also referenced by PAGE-218 (Back/redirect), DoctorInvoiceCreationByOrder and SampleInvoiceCreationByOrder.

## 5. Route
`~/SInventory_UI/InvoiceCreationByOrder.aspx`.

## 6. Source Files
aspx/aspx.cs above; `OrderInfoBLL.cs`, `OrderInfoDAL.cs` (LoadSC:978, LoadDisRouteforInvoice:1008, LoadOrderForOrderCreation:196, LoadInvoiceBatchId:264, GenerateInvoiceByOrderId:110, GetInvoNOGetByInvoID:657, GetInvoNOGetByBatchNo:678); `CommonDataLoad.Check_anomalyInvoiceDetails` (`Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1908`); `BonusCampaignNewDAL.checkTodaysAlreadyInviceGenerate:1138`; `ProformaOrInvoiceReturn.DeleteProformaDal` (`Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs:116`); `InvoiceDAL.GetNewReceiveableDAl`; `RouteInformationDAL`.

## 7. UI Control Inventory
| Control | Label | Required | Default / state | Source |
|---|---|---|---|---|
| salesCenterDropDownList | Sales Center (DC) | Yes (`GridView():583` "Please input Sales Center!") | index 1 (2nd item) after load | `LoadSC(ddl, Session["UserId"])`: `tblCompanyUnit WHERE ComUnitId IN (tblUserCompanyUnit of user)` |
| rootDropDownList | Route | No (validation commented out) | — | `LoadDisRouteforInvoice`: distinct `DistributionRouteId, DistributionRoute_Ord` from tblOrder where `IsInvoice=0 AND ActionStatus='2' AND IsPrepareforInvoice=1 AND DistributionRoute_Ord IS NOT NULL AND ComUnitId=@`; AutoPostBack refreshes DA + territory lists |
| ddlTerritoryName | Territory | No | — | `GetTerritoryByRouteInformationDA_DDLId(route)` |
| ddlDAName | DA (delivery agent) | Yes for bulk generate | "Please Select From List" | `GeteRouteInformationDA_DDLId(route)` |
| manufacDropDownList | Manufacturer | — | index 1 | `tblManufacturer` |
| marketDropDownList | Market | — | — | present; not used to filter (`LoadOrderForOrderCreation` ignores market) |
| submitButton ("Search") | — | — | — | `Button1_Click` → `GridView()` |
| orderGridView | Order list | — | footer shows Σ GrossValue | columns: Order No, Order Date, Customer, Sender code/name, Sales Center code/name, MIO code/name, Delivery Date, Remarks, Due link (hidden), **Select checkbox column `Visible=False`** (aspx:352-360), "Go To Invoice" button, "Generate" button (empty handler) |
| batchno / invoiceButton / reportButton | Batch no, bulk "Generate", "Report" | ≥1 checked & DA | — | see BR-024..027 |
| Due modal (`mpe_1`, `gv_Invoice`) | Customer Previous Dues Status | — | opened by `lbDue_Click` (column hidden) | `InvoiceDAL.GetNewReceiveableDAl(" AND mas.[CustomerMasterId]=" + id, null, null)` — concatenated SQL fragment |

## 8. Field Mapping
| UI | Call | SP/SQL param | Effect |
|---|---|---|---|
| salesCenter value | LoadOrderForOrderCreation(comunitId,…) | `@comunitId` | filter `tblOrder.ComUnitId` |
| rootDropDownList value | ″ (`@manufacId`) — **the parameter named manufacId is compared to `DistributionRouteId`** (SP :1358) | route filter (blank/0 = all; also includes orders with NULL route) |
| ddlTerritoryName value | `@TerritoryId` | territory filter (blank/0 = all; also NULL-territory orders) |
| ddlDAName value | `GenerateInvoiceByOrderId(orderId, Session["UserId"], batchNo, DANameId)` | `@OrderId,@UserId,@BatchNo1,@DANameId` of `sp_Process_ProformaInvoiceByOrderId` | creates proforma (body UNKNOWN) |
| batch | `batchn = MAX(int suffix of tblInvoiceBatch.BatchNo)+1`, prefixed `<routeId>-` | tblInvoiceBatch (written by the SP, UNKNOWN) | grouping key for printing |

## 9. Dropdown Mapping
See §7. `Session["RouteId"]`, `Session["MarketId"]` (stores *route id*, misnamed), `Session["STerritoryId"]` are used to remember filters across the round-trip to PAGE-218 (`DropDownlist():419-458`, `GridView():644-645`).

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| Search | Button1_Click `:678` → `GridView()` | reads list via SP; per row colours the action button by `Permission` |
| Go To Invoice | gotoinvoiceButton_Click `:502` | session handoff + redirect to PAGE-218; blocked rows show JS alert "Credit Limit Exceeded! Invoice creation is blocked for this customer." |
| Generate (bulk) | invoiceButton_Click `:213` | calls `sp_Process_ProformaInvoiceByOrderId` per checked order |
| Report | viewRptButton_Click `:141` | builds `Session["paydetailId"]` where-clause by batch no (string-concatenated) and opens ProformaReportPrintViewer |
| Reset | cancelButton_Click | redirect self |

## 11. Frontend Flow
Page init → `DropDownlist()` (all wrapped in empty try/catch): load DCs for user, manufacturers, choose index 1 for both, load routes, restore `Session["RouteId"]`, DA, territory, run `GridView()`. `sweetAlertConfirm_Submit(this)` is used as confirm for Generate. Client-side blocked-row alert for over-limit rows. Modal for due list (dead: due column invisible).

## 12. Backend Flow
`GridView()` → `LoadOrderForOrderCreation` → `sp_LoadOrderListForOrderCreationbyTerri(@comunitId,@manufacId,@TerritoryId)` (BR-019/020) → bind → per row `Permission` (DataKey) '1' ⇒ green "Generate Invoice" with `sweetAlertConfirm_Submit`, else red "Blocked" (BR-021) → footer totals.
`invoiceButton_Click` → `DataValidation()` (BR-024/025) → `batchn = rootDropDownList.value + "-" + MAX(batch)+1` → per checked row: `GenerateInvoiceByOrderId` then `GetInvoNOGetByInvoID(orderId)`; if an invoice exists, `Check_anomalyInvoiceDetails(invoiceId, orderId)`; if rows returned ⇒ `DeleteProforma(invoiceNo)` → `sp_Delete_ProformaInvoice` (BR-027). Whole block is in `try{…}catch(Exception){}` — **all errors swallowed silently**, success alert always shown. No transaction spanning orders.

## 13. API Mapping
None.

## 14. Service Mapping
OrderInfoBLL (LoadSC, LoadManufac, LoadDisRouteforInvoice, LoadOrderForOrderCreation, LoadInvoiceBatchId, GenerateInvoiceByOrderId, GetInvoNOGetByInvoID, GetInvoNOGetByBatchNo), ProformaOrInvoiceReturnBLL.DeleteProforma, InvoiceDAL (receivable list).

## 15. Repository Mapping
See §6. `LoadOrderForOrderCreation` opens `DataBase.SalesDB` and calls the SP with three string parameters (`""`→int 0 by SQL Server implicit conversion).

## 16. Stored Procedure Mapping
| SP | Body | Notes |
|---|---|---|
| sp_LoadOrderListForOrderCreationbyTerri | CONFIRMED `:1224-1373` | list + credit permission (BR-019/020) |
| sp_Check_anomalyInvoiceDetails | CONFIRMED (fix script) | returns a row when `ABS((TpGrandTotal−AdjustAmount)−ΣNetAmount)>10` for invoices after 2022-11-16 |
| sp_Delete_ProformaInvoice | CONFIRMED | only if `DelivaryInvoiceNo IS NULL`: per line delete Issue row from tblDCStore, insert `tblProInvoiceReturnTrack`, archive to `tblInvoiceDetail_DeleterRecord`, delete detail; then reset `tblOrderDetail.Status=NULL`, `tblOrder.IsInvoice=0`, delete `tblReturnAmount`, delete `tblInvoice`, log `tblInvoiceDeleteLog(InvoiceId,InvoiceNo,DeleteDateTime,DeleteBy)` |
| sp_Process_ProformaInvoiceByOrderId | UNKNOWN | creates proforma from order |
| sp_GET_checkTodaysAlreadyInviceGenerateByCustId, sp_Get_NewReceiveableList, sp_GET_RouteInformationDADetailDDL, sp_GET_TerritoryByRouteInformationDDL | UNKNOWN | |

## 17. Database Mapping
Read: tblOrder, tblCompanyUnit, tblMarket, tblCustomerType, tblInvoice, tblInvoiceDetail, tblCustPayDetail, tblCustomerCreditLimit, `CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster/Detail`, tblInvoiceBatch. Written (by page-called SPs): tblInvoice/tblInvoiceDetail/tblInvoiceBatch/tblDCStore (UNKNOWN bodies), and by `sp_Delete_ProformaInvoice`: tblDCStore, tblProInvoiceReturnTrack, tblInvoiceDetail_DeleterRecord, tblInvoiceDetail, tblOrder(Detail), tblReturnAmount, tblInvoice, tblInvoiceDeleteLog.

## 18. Validation Matrix
| Rule | Where | Message |
|---|---|---|
| Sales center required for search | `GridView():583` | "Please input Sales Center!" |
| ≥1 row checked | `DataValidation():198` | "Please Select at least one row !!!" |
| DA required for bulk | `:204` | "Please Select DA Name!!" |
| Already invoiced today (per customer) | `:185-195` | "⚠️ This invoice has already been created for this customer today…" — **unreachable** (BR-025) |
| Credit limit | SP `Permission`; button state | "Credit Limit Exceeded! Invoice creation is blocked for this customer." |

## 19. Business Rules
| ID | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M06-019 | Orders eligible for invoicing: `IsInvoice=0`, (`OrderType='Regular'` OR NULL OR `IsManual=1`), `ComUnitId=@DC`, route (`DistributionRouteId`) and territory match or NULL, `IsPrepareforInvoice=1 or NULL`, `ActionStatus IN ('2','Accepted','Approved')`, `IsSubDepo=0 or NULL`; newest first. Route dropdown, however, offers only `ActionStatus='2'` orders with a non-null `DistributionRoute_Ord`. | LIVE_DB…sql:1356-1364; OrderInfoDAL.cs:1022-1026 | SP + SQL | CONFIRMED |
| BR-M06-020 | Credit permission per order: `Permission=1` if the customer has **no approved active limit for the DC's company** (`LimitAmount=0`); else 1 only if (order Gross + customer Due ≤ LimitAmount) AND (days since oldest unpaid invoice ≤ DayLimit); otherwise 0. Due = Σ delivery TP grand total − (payments + AIT + discount-on-payment) − Σ sales-return vouchers (TableID 2018), per customer+DC; unpaid invoice = residual > 1. Limit rows: `tblCustomerCreditLimit IsActive=1 AND ActionStatus='Approved'`, summed per company. Also `DueAlert` = days since last update of oldest partial/unpaid delivered invoice ≥30 days. | LIVE_DB…sql:1231-1349 | SP | CONFIRMED |
| BR-M06-021 | If `Permission<>1` the row button is "Blocked" (red) and clicking shows an alert; **enforcement is UI-only** — PAGE-218 does not re-check credit (grep shows no credit check in PAGE-218) and the session handoff can be reproduced by any user. | InvoiceCreationByOrder.aspx.cs:620-642; InvoiceCreationForCustomerByOrder.aspx.cs (no limit check) | page | CONFIRMED |
| BR-M06-022 | Defaults: DC and manufacturer both preselect item index 1 (i.e. the first real entry); filters remembered in Session (`RouteId`, `MarketId`(=route), `STerritoryId`). | .cs:408-458, 644-645 | page | CONFIRMED |
| BR-M06-023 | Go-To-Invoice hands the order over through `Session["OrderId"]` (and `RootNameId=0`) — invoicing is one order at a time. | .cs:529-532 | page | CONFIRMED |
| BR-M06-024 | Bulk mode needs ≥1 selected order and a DA; batch number = `<route id>-<MAX(numeric suffix of tblInvoiceBatch.BatchNo)+1>`. **The selection checkbox column is `Visible=False`, so bulk mode cannot be used from the UI as shipped.** | .cs:164-211, 219-223; aspx:352-360; OrderInfoDAL.cs:264-277 | page | CONFIRMED / INFERRED (invisibility effect) |
| BR-M06-025 | "One invoice per customer per day" check is coded but unreachable: the loop `break`s as soon as `count>0` **before** running `checkTodaysAlreadyInviceGenerate`. | .cs:169-196 | page | CONFIRMED |
| BR-M06-026 | Errors during bulk generation are swallowed (`catch{}`) and success is always announced. | .cs:281-284, 276 | page | CONFIRMED |
| BR-M06-027 | Self-heal: after each generated proforma, if `|(TpGrandTotal−AdjustAmount)−ΣNetAmount|>10` (invoices dated after 2022-11-16) the proforma is deleted through `sp_Delete_ProformaInvoice`, which restores stock (deletes Issue rows), resets `tblOrder.IsInvoice=0` and `tblOrderDetail.Status=NULL`, deletes the invoice header + return amounts and logs to `tblInvoiceDeleteLog`. The printing helper `GenerateParameter()` runs the same check and deletes invoices as a **side effect of building a report filter**. | .cs:112-140, 251-262; SP files | page + SP | CONFIRMED |

## 20. Permission Matrix
`Session["UserId"]` used for DC scoping (`LoadSC`) and passed to the proforma SP. `Session["LoginName"]` (via `DeleteProformaDal`, `HttpContext.Session["LoginName"]`) recorded as deleter. No role checks; credit block is advisory (BR-021). CONFIRMED.

## 21. Status Lifecycle
`tblOrder`: (`IsInvoice=0`,`ActionStatus∈{2,Accepted,Approved}`) —[proforma generated by SP]→ `IsInvoice=1` (**setter inside `sp_Process_ProformaInvoiceByOrderId` UNKNOWN for bulk; PAGE-218 uses `UpdateInvoiceStatus`**) —[proforma deleted]→ `IsInvoice=0`. Invoice states: proforma (`DelivaryInvoiceNo IS NULL`) → delivery confirmed (M07).

## 22. Error Handling
Empty catch blocks throughout (`Page_Load`, `DropDownlist`, `invoiceButton_Click`, route/DA loads). Users get no failure feedback.

## 23. Audit
Deletion audited in `tblInvoiceDeleteLog` + `tblInvoiceDetail_DeleterRecord` + `tblProInvoiceReturnTrack`. Creation audit inside UNKNOWN SP (`@UserId` passed).

## 24. Integration
Cross-database read of `CSTL_ACCDB_Unicorn` (accounts) inside the list SP. No SAP call.

## 25. Reports
`ProformaReportPrintViewer.aspx` (Session["paydetailId"] where-clause) — see `reports/RPT-M06-01-order-invoice-print.md`.

## 26. Dependencies
tblCustomerCreditLimit (approved via CustomerCreditLimitApproval), accounts DB, route/DA masters.

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-01 | Positive | DC with 3 approved orders, customer without credit limit | 3 rows, all "Generate Invoice" | 019,020 |
| TC-02 | Negative | Customer limit 100 000, due 90 000, order 20 000 | "Blocked"; click ⇒ alert | 020,021 |
| TC-03 | Edge | Within amount limit but oldest unpaid invoice older than DayLimit | Blocked | 020 |
| TC-04 | Edge | Order with `IsInvoice=1` | not listed | 019 |
| TC-05 | Negative | Direct GET of PAGE-218 with forged Session order id of a blocked customer | invoice can be created (no server re-check) | 021 |
| TC-06 | Edge | Bulk generate (if column made visible) producing an invoice whose header ≠ details by >10 | proforma auto-deleted, order back to IsInvoice=0, log row written | 027 |
| TC-07 | Negative | Bulk with no DA | "Please Select DA Name!!" | 024 |
| TC-08 | Edge | Two customers' orders same day | second not blocked (check unreachable) | 025 |

## 28. Reverse Engineering References
`InvoiceCreationByOrder.aspx(.cs)`, `LIVE_DB_UPDATE…sql:1224-1373`, `SP_Fix_Invoice_Anomaly_And_Sync.sql`, `sp_Delete_ProformaInvoice_Update.sql`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | — | MISSING |
| Credit block | None existed | UI-only | GAP-M06-005 |
| Bulk generation | None existed | unreachable UI | GAP-M06-006 |
| Same-day duplicate check | None existed | dead code | GAP-M06-007 |

## 30. Known Gaps
Credit block bypassable; bulk path unusable; swallowed exceptions; SQL fragment concatenation in `GetNewReceiveableDAl`/`GenerateParameter` (`batchno.Text` inserted into SQL — `.cs:114-116`, injection); `Session["MarketId"]` misnamed; three cloned pages share class name; `manufacId` parameter actually a route id.

## 31. Change Impact
`sp_LoadOrderListForOrderCreationbyTerri` used only by `OrderInfoDAL.LoadOrderForOrderCreation` (callers: this page and AutoInvoiceCreationByOrder [commented]). `sp_Delete_ProformaInvoice` is shared with PAGE-218 (`printButton_Click`), proforma return/delete pages (M07). `tblInvoiceBatch` numbering is shared with `InvoiceBLL.SaveInvoice` (PAGE-218 path) — both use MAX+1.
