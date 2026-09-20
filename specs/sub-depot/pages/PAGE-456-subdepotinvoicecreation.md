---
page_id: PAGE-456
module: M12
page: SubDepotInvoiceCreation
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-456 — SubDepotInvoiceCreation

> **Evidence level:** Tier A - reviewed 2026-09-19 against the .aspx, .aspx.cs and the BLL/DAL/DAO code they reach. Stored-procedure bodies are NOT in the repository unless stated; such effects are marked `UNKNOWN`. Confidence tags: CONFIRMED (read in code), INFERRED (implied by several references), UNKNOWN.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-456 |
| Module | M12 — Sub-Depot Operations (INFERRED from folder + file name) |
| Page name | SubDepotInvoiceCreation |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SubDepot_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SubDepot_UI_SubDepotInvoiceCreation |
| aspx size | 14536 bytes |
| Code-behind LOC | 216 |
| Inline script size | 985 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**SubDepotInvoiceCreation** (caption `Proforma Invoice Creation`, `.aspx:25`) turns approved orders flagged for sub-depot supply into sub-depot **proforma invoices** in one batch. The user chooses the sales center, sub-depot, route, manufacturer and market, presses the search button to list un-invoiced sub-depot orders, ticks orders and presses `Generate`/invoice. Each ticked order is processed by SP `sp_Process_SubDepoProformaInvoiceByOrderId`, all invoices of the run share one batch id (`06-<n>`); the batch's invoices can then be viewed as a PDF. CONFIRMED (`SubDepotInvoiceCreation.aspx.cs:52-111`).

## 3. Business Objective
Start the sub-depot sales cycle: convert customer orders taken for sub-depot delivery into proforma invoices (`tblSubInvoiceMaster/Detail`), which are later printed, delivered/confirmed (`DelivaryInvoiceList`, PAGE-444) and paid (`SubDeportCustomerPayment`, PAGE-452) - WF-M12-05. Stock allocation, pricing, discount and VAT logic happen inside the SP (body not in repo, UNKNOWN).

## 4. Navigation
Menu placement DB-driven (`UNKNOWN`). Outbound: `SInventory_RPTVIEW/SubDeportInvoiceReportViewer.aspx` (`viewRptButton_Click`, PDF); `gotoinvoiceButton_Click` redirects to `ProformaInvoiceCreation.aspx` after setting `Session["OrderId"]`/`Session["SubDepotId"]` (the main page never reads `SubDepotId` - dead hand-off, GAP-M12-019). Master `NewMasterPage.master`.

## 5. Route
`~/SubDepot_UI/SubDepotInvoiceCreation.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SubDepot_UI/SubDepotInvoiceCreation.aspx |
| Code-behind | Solution.Web/SubDepot_UI/SubDepotInvoiceCreation.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | OrderInfoBLL (Library.BLL/SInventory_BLL/OrderInfoBLL.cs) |
| BLL class | RequisitionBLL (Library.BLL/SInventory_BLL/RequisitionBLL.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |
| DAL class | RequisitionDAL (Library.DAL/SInventory_DAL/RequisitionDAL.cs) |

## 7. UI Control Inventory
| Control ID | Label | Required | Default | Read-only / visible | Data source | Handler |
|---|---|---|---|---|---|---|
| salesCenterDropDownList | Sales Center: | none in code | index 1 (`.cs:121`) | - | `OrderInfoBLL.LoadSC(ddl)` all DCs | `salesCenterDropDownList_SelectedIndexChanged` (:124) |
| dcDropDownList, TERRITORYDropDownList1 | (hidden) | - | - | not used | - | - |
| subdeportDropDownList1 | Sub-Depot : | only tested `!= ""` before searching (`GridView()` :185); **not passed to the query** | first `--Select--` | - | `RequisitionBLL.SubdeportLoad(DC)` (:127) | - |
| rootDropDownList | Route: | drives the query (`root`) | routes for **ComUnitId 6** (`SubLoadDisRouteforInvoice(ddl,6)`, `.cs:122`) | - | `OrderInfoDAL.SubDepoLoadDisRouteforInvoice` | - |
| manufacDropDownList | Manufacture: | drives the query | index 1 (`:120`) | - | `OrderInfoBLL.LoadManufac` | `manufacDropDownList_SelectedIndexChanged` (empty) |
| marketDropDownList | Market: | not used by the order query | remembered `Session["MarketId"]` | - | `OrderInfoBLL.SubdeportLoadMarketOrderWise` (hard-coded territory list) | `marketDropDownList_SelectedIndexChanged` (empty) |
| Button1 (search) | - | - | - | - | `OrderInfoBLL.subDepoOrderLoad(DC, Manufac, Route)` | `Button1_Click` (:212) |
| orderGridView | Order No, Order Date, Customer Code/Name, Market, MIO Code/Name, Territory Code, Gross Value(TP), Customer Type, select, `Go To Invoice` | ticked rows to invoice | - | DataKeys `ComUnitId,ManufacId,OrderId`; footer total of GrossValue | tblOrder x tblCustMaster x tblCompanyUnit x tblMarket | `chkSelectAll_CheckedChanged` (:35) |
| invoiceButton | Generate | >=1 ticked (no check) | - | - | SP call | `invoiceButton_Click` (:52) |
| batchno | Batch No | - | set after run | - | - | - |
| reportButton / viewRptButton | View PDF | - | - | - | session param | `viewRptButton_Click` (:95) |

## 8. Field Mapping
| UI | Variable | Handler | BLL -> DAL | SP parameter | Effect |
|---|---|---|---|---|---|
| grid DataKeys["OrderId"] | `orderId` | :67 | `OrderInfoBLL.GenerateSubInvoiceByOrderId` -> `OrderInfoDAL.GenerateSubInvoiceByOrderId` :21 | `@OrderId` | SP creates `tblSubInvoiceMaster/Detail` rows (UNKNOWN columns) |
| Session["UserId"] | `userId` | :68 | same | `@UserId` | recorded by SP (UNKNOWN) |
| `"06-" + max(BatchNo suffix)+1` | `batchn` | :57-61 | `LoadSubInvoiceBatchId` `OrderInfoDAL.cs:61-73` | `@BatchNo1` | batch id stored by SP in `tblSubInvoiceMasterBatch` (INFERRED from `LoadSubInvoiceBatchId` and `GenerateParameter()` query) |
| rootDropDownList.SelectedValue | `root` | :188-189 | `subDepoOrderLoad` | inline `@Root` | filters `tblOrder.DistributionRouteId` |
| manufacDropDownList | `ManufacId` | same | same | `@ManufacId` | filters orders |
| salesCenterDropDownList | `ComUnitId` | same | same | `@ComUnitId` | filters `tblOrder.ComUnitId` |
Columns written by the SP are UNKNOWN (no body in `sp_bodies.json`).

## 9. Dropdown Mapping
| Dropdown | Loader | SQL | Text/Value | Tables |
|---|---|---|---|---|
| salesCenterDropDownList | `OrderInfoBLL.LoadSC(ddl)` | `SELECT * FROM tblCompanyUnit` (`OrderInfoDAL.cs:972-977`) | ComUnitName / ComUnitId | tblCompanyUnit (no user filter) |
| subdeportDropDownList1 | `RequisitionBLL.SubdeportLoad` | BR-M12-001 | `Code:Name` / SubDepotId | tblSubDepot |
| rootDropDownList | `OrderInfoBLL.SubLoadDisRouteforInvoice(ddl,6)` | `select distinct tblOrder.DistributionRouteId, tblRouteInformationMaster.RouteName ... where IsInvoice=0 and ActionStatus='2' and tblRouteInformationMaster.IsSubDepo=1 and ComUnitId=@ComUnitId(=6) order by RouteName` (`OrderInfoDAL.cs:1033-1055`) | RouteName / DistributionRouteId | tblOrder, tblRouteInformationMaster |
| manufacDropDownList | `OrderInfoBLL.LoadManufac` | `select * from tblManufacturer` | ManufacName / ManufacId | tblManufacturer |
| marketDropDownList | `OrderInfoBLL.SubdeportLoadMarketOrderWise(ddl, DC, subDepot)` | orders joined to `View_CustomerMaster`/`tblMarket` WHERE IsInvoice=0 AND V.ComUnitId=@ AND TerritoryCode IN (a hard-coded list of 14 BL-/KL- codes) (`OrderInfoDAL.cs:1172-1184`); the sub-depot parameter is unused | MarketName / MarketId | tblOrder, View_CustomerMaster, tblMarket |

## 10. Button Action Mapping
| Button | Handler | BLL -> DAL | SQL / SP | DB change |
|---|---|---|---|---|
| search (`Button1`) | :212 -> `GridView()` :183 | `OrderInfoBLL.subDepoOrderLoad` -> `OrderInfoDAL.subDepoOrderLoad` :244 | `SELECT * FROM tblOrder INNER JOIN tblCustMaster, tblCompanyUnit, tblMarket WHERE IsInvoice=0 AND IsSubDepo=1 AND ComUnitId=@ AND ManufacId=@ AND DistributionRouteId=@` (parameterised) | read; also stores `Session["MarketId"]` |
| Generate (`invoiceButton`) | :52 | `LoadSubInvoiceBatchId`; per ticked order `GenerateSubInvoiceByOrderId` | `sp_Process_SubDepoProformaInvoiceByOrderId` | SP writes invoice tables (UNKNOWN) |
| View PDF | :95 | none | builds `WHERE IV.InvoiceNo IN (SELECT InvoiceNo FROM tblSubInvoiceMasterBatch LEFT JOIN tblSubInvoiceMaster ON ... WHERE BatchNo = '<batchno.Text>')` into `Session["paydetailId"]` | none |
| Go To Invoice | :151 | none | sets `Session["OrderId"]`, `Session["SubDepotId"]`; redirect | none |

## 11. Frontend Flow
- `Page_Load` first load: dropdowns, `salesCenterDropDownList_SelectedIndexChanged` (sub-depots + market list), if `Session["MarketId"]` exists select it and load the grid (:13-27).
- Search fills the grid and a footer total of `GrossValue` (`N2`).
- Generate: loop ticked rows -> SP; after the loop reload grid (if `Session["MarketId"]`) and alert `Invoice Generated Successfully.`; `batchno` textbox receives the batch id.
- View PDF opens `SubDeportInvoiceReportViewer.aspx` in a popup which reads `Session["paydetailId"]` (a raw SQL fragment).
- JS: `pageLoad()` select2 init (`.aspx:57-70`).

## 12. Backend Flow
| Handler | Source | Flow | try/catch | Txn |
|---|---|---|---|---|
| `invoiceButton_Click` | `.cs:52-86` | `batchn = "06-" + LoadSubInvoiceBatchId()[0][0]` -> for each ticked order: `GenerateSubInvoiceByOrderId(orderId, Session UserId, batchn)` (SP) -> after loop set `batchno.Text` (inside loop), reload grid, alert success | **`try { ... } catch (Exception ex) { }` - empty catch** | SP-internal only (UNKNOWN) |
| `GridView` | :183-211 | requires `subdeportDropDownList1.SelectedValue != ""` then `subDepoOrderLoad(DC, Manufac, Route)`; sums GrossValue in a try/catch that swallows | partial | no |
| `viewRptButton_Click` | :95-111 | `Session["paydetailId"] = " WHERE IV.InvoiceNo IN (... BatchNo = '<text>')"` | no | no |
The SP return value is ignored: a failed SP still produces the success alert (BR-M12-040). Nothing prevents generating twice for the same orders except `IsInvoice=0` in the search (set by the SP - UNKNOWN).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderInfoBLL.LoadSC | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:91 |
| OrderInfoBLL.LoadManufac | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:117 |
| OrderInfoBLL.SubLoadDisRouteforInvoice | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:109 |
| OrderInfoBLL.subDepoOrderLoad | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:70 |
| RequisitionBLL.SubdeportLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:157 |
| OrderInfoBLL.SubdeportLoadMarketOrderWise | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:87 |
| OrderInfoBLL.LoadSubInvoiceBatchId | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:20 |
| OrderInfoBLL.GenerateSubInvoiceByOrderId | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:25 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OrderInfoDAL.LoadSC | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1935 | inline SQL: tblCompanyUnit |  |
| OrderInfoDAL.LoadManufac | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2219 | inline SQL: tblManufacturer |  |
| OrderInfoDAL.SubDepoLoadDisRouteforInvoice | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2057 | inline SQL: tblOrder, tblRouteInformationMaster | @ComUnitId |
| OrderInfoDAL.subDepoOrderLoad | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:489 | inline SQL: tblCompanyUnit, tblMarket, tblOrder | @ComUnitId, @ManufacId, @Root |
| RequisitionDAL.SubdeportLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:46 | inline SQL: tblSubDepot | @ComUnitId |
| RequisitionDAL.BindDropDown | Library.DAL/SInventory_DAL/RequisitionDAL.cs:761 | delegates / no SQL literal |  |
| OrderInfoDAL.SubdeportLoadMarketOrderWise | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2335 | inline SQL: tblCompanyUnit, tblMarket, tblOrder | @ComUnitId |
| OrderInfoDAL.LoadSubInvoiceBatchId | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:123 | inline SQL: tblSubInvoiceMasterBatch |  |
| OrderInfoDAL.GenerateSubInvoiceByOrderId | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:43 | sp_Process_SubDepoProformaInvoiceByOrderId | @BatchNo1, @OrderId, @UserId |

## 16. Stored Procedure Mapping
| SP | Purpose | R/W | Body in repo |
|---|---|---|---|
| `sp_Process_SubDepoProformaInvoiceByOrderId` (@OrderId,@UserId,@BatchNo1) | create sub-depot proforma from an order, allocate stock/price/discount/VAT (INFERRED) | W | NO - UNKNOWN |
Main-depot twin `sp_Process_ProformaInvoiceByOrderId` (+`@DANameId`) is also not in the repo.

## 17. Database Mapping
| Table | Access | Detail | Source |
|---|---|---|---|
| tblOrder | SELECT | IsInvoice, IsSubDepo, ComUnitId, ManufacId, DistributionRouteId, OrderCode, SubmissionDate, GrossValue, CustomerType, MIO/Territory | `OrderInfoDAL.cs:244-262` |
| tblCustMaster, tblCompanyUnit, tblMarket | SELECT | joins | same |
| tblRouteInformationMaster | SELECT | IsSubDepo=1 routes | :1047-1050 |
| tblSubInvoiceMasterBatch | SELECT (MAX BatchNo), written by SP (INFERRED) | batch numbering | :61-73 |
| tblSubInvoiceMaster / tblSubInvoiceDetail | written by SP (UNKNOWN columns); read by report queries | invoice header/lines (column list visible in `Sub_InvoiceDAL.SaveDataForInvoice/Details` legacy inserts: InvoiceNo, SubDepotId, OrderId, TpTotal, TpVat, TpGrandTotal, Delivery* ...) | `Sub_InvoiceDAL.cs:111-262` |
| tblOrder.IsInvoice | updated by SP (INFERRED) | - | - |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Message | BR |
|---|---|---|---|---|---|
| Sub-depot | must be selected to load orders | - | yes (silent no-op) | none | - |
| Orders ticked | >=1 | - | **not checked** | none | BR-M12-040 |
| Route/manufacturer | used as filters | - | SQL | - | BR-M12-038 |
| Generation result | success | - | none (empty catch) | `Invoice Generated Successfully.` always | BR-M12-040 |
No `RequiredFieldValidator`s on the page (grep). Main depot requires a DA name (`DataValidation`, `InvoiceCreationByOrder.aspx.cs:200-208`).

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M12-001 | A sub-depot is registered under exactly one parent DC (tblSubDepot.ComUnitId). Every sub-depot dropdown is filtered by the DC chosen just before it (`WHERE ComUnitId=@ComUnitId`, label `SubDepotCode:SubDepotName`, value SubDepotId, default item `--------Select---------`). | Library.DAL/SInventory_DAL/RequisitionDAL.cs:45-52,761-770; Library.DAL/SubDepot_DAL/SubDepotDAL.cs:15-19 | RequisitionDAL.SubdeportLoad | CONFIRMED |
| BR-M12-038 | Sub-depot proforma candidates: tblOrder with IsSubDepo=1, IsInvoice=0, chosen DC, manufacturer and distribution route; the route list only offers routes flagged tblRouteInformationMaster.IsSubDepo=1 that still have un-invoiced orders with ActionStatus='2'. Main-depot order lists exclude IsSubDepo=1 orders. | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:244-262,1033-1055; LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:1355-1363 | OrderInfoDAL.subDepoOrderLoad / SubDepoLoadDisRouteforInvoice | CONFIRMED |
| BR-M12-039 | Batch id for a proforma run = literal `06-` + (max numeric suffix of tblSubInvoiceMasterBatch.BatchNo + 1); the `06` prefix and the route-list DC id (6) are hard-coded rather than taken from the selected route/DC. (Main depot uses the selected route id as prefix.) | Solution.Web/SubDepot_UI/SubDepotInvoiceCreation.aspx.cs:57-61,122; Library.DAL/SInventory_DAL/OrderInfoDAL.cs:61-73 (main: InvoiceCreationByOrder.aspx.cs:222-223) | invoiceButton_Click / DropDownlist | CONFIRMED |
| BR-M12-040 | Proforma generation per ticked order calls SP sp_Process_SubDepoProformaInvoiceByOrderId(@OrderId,@UserId,@BatchNo1) (body not in repo). The page wraps the loop in an empty catch and always shows `Invoice Generated Successfully.` Main depot additionally requires a DA name, and auto-deletes invoices that fail sp_check_anomalyinvoicedetails - none of that exists here. | Solution.Web/SubDepot_UI/SubDepotInvoiceCreation.aspx.cs:52-86; Library.DAL/SInventory_DAL/OrderInfoDAL.cs:21-28; main: Solution.Web/SInventory_UI/InvoiceCreationByOrder.aspx.cs:213-260 | invoiceButton_Click | CONFIRMED |
| BR-M12-041 | Delivery confirmation of a sub-depot proforma: status Full -> SP sp_SubdeportDeliveryConformationFull(@InvoiceNo,@UpdateBy,@UpdateDate); Reject -> sp_SubDeportDeliveryConformationReject(+@ReturnReason from dropdown); Partial -> redirect to DelivaryInvoiceCreationForCustomerAuto.aspx, whose files in SubDepot_UI are renamed `.exclude` (page not served). Bodies of both SPs are not in the repo, so stock/valuation effects are UNKNOWN. | Solution.Web/SubDepot_UI/DelivaryInvoiceList.aspx.cs:87-124; Library.DAL/SInventory_DAL/InvoiceDAL.cs:269-276,305-313 | DelivaryInvoiceList.gotoinvoiceButton_Click | CONFIRMED |
| BR-M12-045 | Sub-depot receive and stock-report pages branch on Session UserType: Admin sees all DCs, others only DCs assigned in tblUserCompanyUnit (DCLoad(ddl,userId)). The transfer, adjustment and invoice pages use the unfiltered DC list (`select ... from tblCompanyUnit`) so any logged-in user may pick any DC. | Solution.Web/SubDepot_UI/StockReceiveBySubDepot.aspx.cs:16-31; SubDeportStockReport.aspx.cs:11-27; Library.DAL/SInventory_DAL/RequisitionDAL.cs:35-39,53-64; StockTransferDcToSubDepot.aspx.cs:25 | Page_Load DC dropdown loaders | CONFIRMED |

Calculations on this page: footer total = SUM(GrossValue) (`.cs:197-199`). All pricing/discount/VAT calculations are inside the SP (UNKNOWN). Legacy C# in `Sub_InvoiceBLL` (dead: no page calls `SaveInvoice/SaveInvoiceDetails`) shows the intended formulas: `TotalPrice = qty x UnitPrice` (0 for gift products), `DiscountAmount = TotalPrice x Discount%/100`, `SpecialAmount = TotalPrice x SpecialAmount%/100`, `TotalPriceVatAmount = UnitVatAmount x qty` (0 for gifts), `NetAmount = TotalPrice - DiscountAmount - SpecialAmount + VAT`, FIFO batch allocation by `ExpDate ASC` over `tblSubDepotStore` (`Sub_InvoiceBLL.cs:92-285`, `Sub_InvoiceDAL.cs:480-484`) - INFERRED only, not proven to be what the SP does.

## 20. Permission Matrix
- Session keys read: `UserId` (passed to SP), `MarketId` (remembered). No role/UserType check. DC list unfiltered (`LoadSC(ddl)`), so any user can run proforma generation for any DC/route; route list is limited to DC id 6 by a hard-coded constant.

## 21. Status Lifecycle
Order: `tblOrder.IsInvoice` 0 -> 1 (set by SP, INFERRED) with `ActionStatus='2'` (approved) as pre-condition for routes to appear. Proforma: `tblSubInvoiceMaster` row with `DelivaryInvoiceNo` NULL = proforma pending delivery confirmation (used as such by `DCStockReportDAL.cs:493-500` 'Booked for delivery' and `GetWarning`); `DeliveryInvoiceStatus` `Full`/`Partial`/`Reject` and `PaymentStatus` are set later (BR-M12-041, BR-M12-035). Column-level lifecycle of the SP is UNKNOWN.

## 22. Error Handling
Empty `catch` in `invoiceButton_Click` (all errors hidden), silent no-ops, swallowed footer-total exception; `Convert.ToInt32(Session["UserId"])` throws if session expired (caught & hidden). No logging.

## 23. Audit
Audit: `@UserId` passed to the SP (stored value UNKNOWN); `tblSubInvoiceMaster.UserId` exists in the legacy INSERT (`Sub_InvoiceDAL.cs:111-162`), `CreateDate` also. No page-level logging.

## 24. Integration
None (no SAP/mobile call from this page). Orders originate from the field-force/manual-order flow (`tblOrder.IsFromApp`, M-order modules) - out of M12.

## 25. Reports
`View PDF` -> `SubDeportInvoiceReportViewer.aspx` (RPT-M12-03) using `Session["paydetailId"]`. Related lists: `SubDepotProformaList` (PAGE-457).

## 26. Dependencies
BLL: `OrderInfoBLL`, `RequisitionBLL`. DAL: `OrderInfoDAL`, `RequisitionDAL`, `ClsCommonInternalDAL.RunStoreProcedure`. Tables per §17. Master `NewMasterPage.master`.

## 27. Test Cases
| ID | Scenario | Steps | Expected | BR |
|---|---|---|---|---|
| TC-01 | Route list | open page | only routes with IsSubDepo=1, un-invoiced approved orders, DC id 6 | BR-M12-038/039 |
| TC-02 | Load orders | choose sub-depot, route, manufacturer; search | only `IsSubDepo=1`, `IsInvoice=0` orders of that DC/route/manufacturer | BR-M12-038 |
| TC-03 | Generate | tick 2 orders, Generate | batch `06-n`; success alert; orders vanish after reload (if SP sets IsInvoice) | BR-M12-039/040 |
| TC-04 | SP failure (negative) | force SP error | still shows success (empty catch) | BR-M12-040 |
| TC-05 | Nothing ticked | Generate | success alert with no work | BR-M12-040 |
| TC-06 | Sub-depot ignored (edge) | change sub-depot, same route | same order list | BR-M12-038 |
| TC-07 | Empty batch table | first ever run | `BatchNoInt` NULL -> batch `06-` (INFERRED) | BR-M12-039 |
| TC-08 | View PDF before generating | View PDF | session `paydetailId` missing -> NullReference in viewer | GAP-M12-023 |

## 28. Reverse Engineering References
- `Solution.Web/SubDepot_UI/SubDepotInvoiceCreation.aspx`, `Solution.Web/SubDepot_UI/SubDepotInvoiceCreation.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | `SubDepotInvoiceCreation.aspx(.cs)` | n/a | MISSING | - |
| Proforma logic | None existed | inside `sp_Process_SubDepoProformaInvoiceByOrderId` | SP not in repo | UNKNOWN | GAP-M12-001 |
| Error handling | None existed | swallowed | - | GAP | BR-M12-040 |
| Main-depot parity (DA name, anomaly check) | None existed | absent | - | GAP | BR-M12-040 |

## 30. Known Gaps
- Empty catch and always-success message; hard-coded `06` batch prefix and route DC 6; sub-depot and market dropdowns do not affect the query (decorative); hard-coded territory list in market loader; no ticked-row check; raw SQL fragment in Session for the report; dead hand-off `Session["SubDepotId"]`; `LoadSubInvoiceBatchId` uses MAX+1 (race between users).

## 31. Change Impact
- `OrderInfoDAL.subDepoOrderLoad`/`SubDepoLoadDisRouteforInvoice`/`GenerateSubInvoiceByOrderId`/`LoadSubInvoiceBatchId` are sub-depot only; `LoadSC`, `LoadManufac` shared. Downstream: `SubDepotProformaList`, `DelivaryInvoiceList`, `SubDeportCustomerPayment`, invoice reports, stock report 'Booked' bucket. The SP is the single point that decides stock/price effects for the whole sub-depot sales cycle.

