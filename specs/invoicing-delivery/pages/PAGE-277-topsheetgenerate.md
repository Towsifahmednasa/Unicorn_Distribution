---
page_id: PAGE-277
module: M07
page: TopSheetGenerate
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---

# PAGE-277 — TopSheetGenerate

> **Evidence level:** Tier A - reviewed 2026-09-19 by reading the .aspx, .aspx.cs, the BLL/DAL methods reached, and every SP/SQL body present in the repository. Confidence tags: CONFIRMED (read in implementation), INFERRED (implied by several references), UNKNOWN (not derivable; SP bodies are absent from the repo except where stated). Module: M07 - Proforma, Invoicing & Delivery.


## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-277 |
| Module | M07 - Proforma, Invoicing & Delivery |
| Page name | TopSheetGenerate |
| Kind | Entry / Transaction with **destructive read side effect** (Search deletes anomalous proforma invoices) plus report launcher (Top Sheet, Invoice print) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_TopSheetGenerate |
| Size | aspx 210 lines, code-behind 378 lines |
| Suspected legacy/duplicate | Siblings: `DelivaryTopSheetGenerate.aspx` (delivery invoices, PAGE-265) and `SCPickingGenerate.aspx` (PAGE-276); `opsheetButton_Click` (`.aspx.cs:198`) has no button in the markup (dead). Market/manufacturer/territory controls and the old search branches are commented out. |

## 2. Purpose
CONFIRMED (labels): "Sales Center", (hidden Manufacturer, Market), "Invoice Date", "Distribution Route", "DA Name", "Delivery Man", buttons Search, "View TopSheet Report", "Print Invoice Report", and a grid of the day's invoices per Delivery Assistant (DA) with customer, invoice no/date, territory (`areacode`), total amount and a per-row "View Report". The user picks a DC, an invoice date and a DA; ticks invoices; the page assigns a running **top-sheet code**, logs the delivery man against it and opens the top sheet (loading/route sheet) and/or the invoice prints.

## 3. Business Objective
INFERRED business objective: prepare the outbound delivery paperwork per delivery assistant: a top sheet lists every proforma invoice loaded on one vehicle/DA for the day (with a market-wise picking summary) so goods can be loaded and handed over. The generated `TopSheetGenCode` links the printed sheet to the DA name and creator (`sp_Save_TopSheetGenReportCode(@EntryBy,@DeliveryMan)`). The page also acts as a **data-quality gate**: any invoice whose header total disagrees with its detail total is automatically deleted and returned to stock so it can be re-invoiced (BR-M07-035/036).

## 4. Navigation
Menu DB-driven -> UNKNOWN. Outbound (popups): `../SInventory_RPTVIEW/TopSheetReportViewer.aspx?InvNo=<InvoiceId,...>&Code=<code>` (`.aspx.cs:288-292`), `../SInventory_RPTVIEW/ProformaReportPrintViewer.aspx?InvNo=<InvoiceId,...>` (`.aspx.cs:331-336`), `../SInventory_RPTVIEW/InvoiceReportViewer.aspx?InvNo=<InvoiceNo>` (row button, `.aspx.cs:182-196`). Inbound: none found by markup.

## 5. Route
`~/SInventory_UI/TopSheetGenerate.aspx` (file-based WebForms route; no `MapPageRoute` alias).


## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/TopSheetGenerate.aspx |
| Code-behind | Solution.Web/SInventory_UI/TopSheetGenerate.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL | OtherStockActionBLL (`LoadmanufacturerName`, `DCLoad`), OrderInfoBLL (`LoadMarket`), StockConditionFreezeBLL (`LoadInvoiceByDAName` `StockConditionFreezeBLL.cs:107`, `LoadPendingTerritory`), ProformaOrInvoiceReturnBLL (`DeleteProforma` `:184`) |
| DAL | StockConditionFreezeDAL (`LoadInvoiceByDAName` `:161-180`), ProformaOrInvoiceReturn (`DeleteProformaDal` `:116-121`), CommonDataLoad (`Check_anomalyInvoiceDetails` `CommonDataLoad.cs:1908-1928`), SeedDataDAL (`Save_TopSheetGenReportCodeInfo` `:162`, `GetTopSheetCodeSetupById` `:252`, `GetDistributionRouteListByDCID` `:806`), RouteInformationDAL (`GetDANameDDL` `:17`) |
| SP bodies in repo | `sp_Delete_ProformaInvoice` -> `sp_Delete_ProformaInvoice_Update.sql:7`; `sp_Check_anomalyInvoiceDetails` -> `SP_Fix_Invoice_Anomaly_And_Sync.sql:42` |

## 7. UI Control Inventory
| Control ID | Type | Label | Required | Default | Read-only / Enabled / Visible | Data source | Handler / error |
|---|---|---|---|---|---|---|---|
| dcDropDownList1 | DropDownList | "Sales Center:" | YES (Search: "Please Select All Parameters!") | first DC of the user (no placeholder: `OtherStockActionDAL.DCLoad(ddl, UserId)` `OtherStockActionDAL.cs:93-105`) | AutoPostBack | tblCompanyUnit LEFT JOIN tblUserCompanyUnit WHERE UserId | `dcDropDownList1_SelectedIndexChanged` - loads markets, DA names, routes; clears grid |
| manufacturerDropDownList, MarketDropDownList1, TERRITORYDropDownList1, DropDownList1 | DropDownList | "Manufacturer :", "Market :" | none | manufacturer index 1 | **hidden** (`runat="server" visible="false"`); Market change reloads territories (`LoadPendingTerritory`) | tblManufacturer, markets, pending territories | Market/Manufacturer changes only clear the grid |
| InvoiceDateTextBox | TextBox (datepicker) | "Invoice Date :" | YES | `DateTime.Now.ToString("dd MMMM, yyyy")` | AutoPostBack, handler empty | typed/picker | `InvoiceDateTextBox_TextChanged` (no-op) |
| ddlRoute | DropDownList | "Distribution Route :" | none | placeholder "Please Select From List" | enabled | `SeedDataDAL.GetDistributionRouteListByDCID` -> `sp_GET_DistributionRouteByDCID` (RouteInformationMasterId / RouteName) | **not used by Search or reports** (`Session["Route"] = 0`) |
| ddlDAName | DropDownList | "DA Name :" | YES for Search and for View TopSheet Report ("Please Select DA Name!") | placeholder | AutoPostBack -> triggers Search | `RouteInformationDAL.GetDANameDDL(depoId)` -> `sp_Get_DDLDANameByDCID` (Value / TextField) | `ddlDAName_SelectedIndexChanged` -> `SearchButton_Click(null,null)` |
| txtDeliveryMan | TextBox | "Delivery Man" | used only if no DA is selected | empty | - | typed | value is effectively dead: DA is required before the report path runs |
| SearchButton | LinkButton | "Search" | - | - | - | - | `SearchButton_Click` |
| reportButton | LinkButton | "View TopSheet Report" | DA + >=1 ticked row | - | - | - | `viewRptButton_Click` |
| lblInvoice | LinkButton | "Print Invoice Report" | >=1 ticked row | - | - | - | `lblInvoice_Click` |
| loadGridView | GridView `DataKeyNames="InvoiceId,InvoiceNo"` | #SL, [checkbox], Customer Code, Customer Name, Invoice No, Invoice Date, Territory Code (`areacode`), Total Amount (`TpGrandTotal`), Print Invoice | - | rows from `LoadInvoiceByDAName` | hidden fields hfOrderId/hfInvoiceNo/hfInvoiceId per row | - | - |
| chkSelectAll / chkSelect | CheckBox | - | - | unchecked | header AutoPostBack | - | `chkSelectAll_CheckedChanged` |
| gotoinvoiceButton | LinkButton | "View Report" (row) | - | - | - | - | `gotoinvoiceButton_Click` |
| hiddiv | div | (button panel) | - | `hiddiv.Visible = true` on load (the user-id 9/11 restriction is commented out, `.aspx.cs:25-28`) | visible for everyone | - | - |

## 8. Field Mapping
| UI | Handler | BLL -> DAL -> parameter | Table.column |
|---|---|---|---|
| dcDropDownList1.SelectedValue | `SearchButton_Click` | `StockConditionFreezeBLL.LoadInvoiceByDAName` -> `@ComUnitId` | tblInvoice.ComUnitId |
| ddlDAName.SelectedValue | same | `@DANameId` | tblInvoice.Inv_DANameId |
| InvoiceDateTextBox.Text | same | `@InvoiceDate` (string; `CONVERT(DATE,InvoiceDate)=@InvoiceDate`) | tblInvoice.InvoiceDate |
| hfInvoiceId, hfOrderId | same | `CommonDataLoad.Check_anomalyInvoiceDetails` -> `sp_Check_anomalyInvoiceDetails @InvoiceId,@orderId` | tblInvoice.InvoiceId / OrderId |
| hfInvoiceNo | same | `ProformaOrInvoiceReturnBLL.DeleteProforma(InvoiceNo)` -> `sp_Delete_ProformaInvoice @InvoiceCode,@User(=Session["LoginName"])` | tblInvoice.InvoiceNo |
| Session["UserId"], ddlDAName text | `viewRptButton_Click` | `SeedDataDAL.Save_TopSheetGenReportCodeInfo` -> `sp_Save_TopSheetGenReportCode @EntryBy,@DeliveryMan` (returns PK via `SaveDataReturnPrimaryKey`) | UNKNOWN (top sheet code table) |
| pk | same | `SeedDataDAL.GetTopSheetCodeSetupById(pk)` -> `sp_GET_ToSheetcode_ById @id` -> column `TopSheetGenCode` | UNKNOWN |
| ticked hfInvoiceId (comma joined) | same | query string `InvNo=<ids>&Code=<TopSheetGenCode>` | - |

## 9. Dropdown Mapping
| Dropdown | Loader | SQL |
|---|---|---|
| dcDropDownList1 | `OtherStockActionBLL.DCLoad(ddl, UserId)` (`OtherStockActionDAL.cs:93-105`) | `SELECT * FROM tblCompanyUnit LEFT JOIN tblUserCompanyUnit ... WHERE UserId=@UserId` |
| manufacturerDropDownList | `LoadmanufacturerName` | `select * from tblManufacturer` |
| MarketDropDownList1 | `OrderInfoBLL.LoadMarket(ddl, ComUnitId)` | inline (OrderInfoDAL) |
| ddlRoute | `_seedRepo.GetDistributionRouteListByDCID(ComUnitId)` | `sp_GET_DistributionRouteByDCID` (absent); wrapped in swallow `try/catch` |
| ddlDAName | `LoadDANameByDC(ComUnitId)` -> `RouteInformationDAL.GetDANameDDL` | `sp_Get_DDLDANameByDCID` (absent); swallow `try/catch` |

## 10. Button Action Mapping
| Button | Handler | Chain | DB change |
|---|---|---|---|
| Search / DA change | `SearchButton_Click` (`.aspx.cs:101-180`) | `LoadInvoiceByDAName` (inline SQL) then per row `sp_Check_anomalyInvoiceDetails` and, if flagged, `sp_Delete_ProformaInvoice` | **deletes invoices + restores stock** (BR-M07-035..039) |
| View TopSheet Report | `viewRptButton_Click` (`.aspx.cs:217-305`) | sets session keys (`ProformaTopSheet=1`, `dcDropDownList1`, `MarketDropDownList1`, `InvoiceDateTextBox`, `Route=0`), `sp_Save_TopSheetGenReportCode`, `sp_GET_ToSheetcode_ById`, popup `TopSheetReportViewer` | inserts a top-sheet code row (UNKNOWN table) each click |
| Print Invoice Report | `lblInvoice_Click` (`.aspx.cs:313-350`) | `Session["paydetailId"]=null`; popup `ProformaReportPrintViewer?InvNo=<ids>` | none |
| View Report (row) | `gotoinvoiceButton_Click` | popup `InvoiceReportViewer?InvNo=<InvoiceNo>` | none |

## 11. Frontend Flow
No page JS beyond master helpers (datepicker `pageLoad`). Popups use `window.open(url,null,'height=700,width=950,...')` via `ScriptManager.RegisterStartupScript`. Session keys are handed to viewer pages instead of query-string parameters (the viewers read them back: `TopSheetReportViewer.aspx.cs:22-33`).

## 12. Backend Flow
| Handler | Source | Flow | try/catch | Txn |
|---|---|---|---|---|
| Page_Load | `.aspx.cs:19-30` | `LoadDropDown()`; date = now; show `hiddiv` | no | no |
| LoadDropDown | `.aspx.cs:32-41` | manufacturer index 1 (NullReference/ArgumentOutOfRange if list empty); DC list; triggers `dcDropDownList1_SelectedIndexChanged(null,null)` | no | no |
| dcDropDownList1_SelectedIndexChanged | `.aspx.cs:53-77` | markets; clear grid; DA list; routes (swallow) | swallow around routes | no |
| SearchButton_Click | `.aspx.cs:101-180` | (a) parameter check DC/date/DA else faildalert "Please Select All Parameters!"; (b) `LoadInvoiceByDAName` -> bind; (c) for each row anomaly check and delete; (d) if 0 rows faildalert " No Data Found!" | no | no - each SP is its own statement; `sp_Delete_ProformaInvoice` itself has no transaction |
| viewRptButton_Click | `.aspx.cs:217-305` | DA required else faildalert "Please Select DA Name!"; collects ticked InvoiceIds; if none faildalert "Please Select at Least one row from Table!"; **saves code before checking that rows are ticked** | outer `try{}catch(Exception){}` swallow (silent failure) | no |
| lblInvoice_Click | `.aspx.cs:313-350` | ticked ids -> viewer popup; none -> faildalert | swallow | no |

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.


## 14. Service Mapping
| BLL method | Source |
|---|---|
| OtherStockActionBLL.LoadmanufacturerName | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:20 |
| OtherStockActionBLL.DCLoad | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:28 |
| OrderInfoBLL.LoadMarket | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:310 |
| StockConditionFreezeBLL.LoadPendingTerritory | Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs:152 |
| StockConditionFreezeBLL.LoadInvoiceByDAName | Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs:108 |
| ProformaOrInvoiceReturnBLL.DeleteProforma | Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs:185 |


## 15. Repository Mapping
| DAL | Source | SQL/SP | Params |
|---|---|---|---|
| StockConditionFreezeDAL.LoadInvoiceByDAName | `StockConditionFreezeDAL.cs:161-180` | `SELECT ord.TerritoryCode_Ord + ' : ' + ord.TerritoryName_Ord areacode, tblD.ManufacId AS TpGrandTotal, * FROM tblInvoice I (nolock) INNER JOIN (SELECT D.InvoiceId, SUM(NetAmount) ManufacId FROM tblInvoice I INNER JOIN tblInvoiceDetail D ... INNER JOIN tblProduct P ... GROUP BY D.InvoiceId) tblD ON I.InvoiceId = tblD.InvoiceId INNER JOIN tblCustMaster C ON I.CustomerMasterId=C.CustomerMasterId INNER JOIN tblOrder ord ON I.OrderId=ord.OrderId WHERE I.ComUnitId=@ComUnitId AND I.Inv_DANameId=@DANameId AND CONVERT(DATE,InvoiceDate)=@InvoiceDate ORDER BY OrderNo` | @ComUnitId, @DANameId, @InvoiceDate |
| CommonDataLoad.Check_anomalyInvoiceDetails | `CommonDataLoad.cs:1908-1928` | `sp_Check_anomalyInvoiceDetails` | @InvoiceId, @orderId |
| ProformaOrInvoiceReturn.DeleteProformaDal | `ProformaOrInvoiceReturn.cs:116-121` | `sp_Delete_ProformaInvoice` | @InvoiceCode, @User = `Session["LoginName"]` (read directly from `HttpContext` inside the DAL) |
| SeedDataDAL.Save_TopSheetGenReportCodeInfo | `SeedDataDAL.cs:162-` | `SaveDataReturnPrimaryKey("sp_Save_TopSheetGenReportCode")` | @EntryBy, @DeliveryMan |
| SeedDataDAL.GetTopSheetCodeSetupById | `SeedDataDAL.cs:252-268` | `sp_GET_ToSheetcode_ById` | @id |

## 16. Stored Procedure Mapping
| SP | Body | What is known |
|---|---|---|
| sp_Check_anomalyInvoiceDetails | **in repo**: `SP_Fix_Invoice_Anomaly_And_Sync.sql:42-72` (the live copy may differ; the script is an ALTER) | returns a row (TpGrandTotal, NetAmount, InvoiceNo, invoicedate) iff invoice date > 2022-11-16 AND ABS((TpGrandTotal - ISNULL(AdjustAmount,0)) - SUM(tblInvoiceDetail.NetAmount)) > 10 for the given InvoiceId+OrderId |
| sp_Delete_ProformaInvoice | **in repo**: `sp_Delete_ProformaInvoice_Update.sql:7-206` (ALTER script; live copy unverified) | see BR-M07-037..039 |
| sp_Save_TopSheetGenReportCode, sp_GET_ToSheetcode_ById, sp_GET_DistributionRouteByDCID, sp_Get_DDLDANameByDCID | absent | UNKNOWN; code proves: `@EntryBy`(UserId) and `@DeliveryMan`(DA name text) go in; a numeric primary key comes back; `TopSheetGenCode` is a column of the by-id result |

## 17. Database Mapping
- READ (inline SQL): tblInvoice, tblInvoiceDetail, tblProduct, tblCustMaster, tblOrder (search); tblCompanyUnit, tblUserCompanyUnit, tblManufacturer, tblMarket (dropdowns).
- READ/WRITE by `sp_Delete_ProformaInvoice` (body read): reads tblInvoice, tblInvoiceDetail; DELETEs tblDCStore (Issue rows), tblInvoiceDetail, tblReturnAmount, tblInvoice; UPDATEs tblDCStore (Receive rows), tblOrderDetail.Status, tblOrder.IsInvoice; INSERTs tblProInvoiceReturnTrack, tblInvoiceDetail_DeleterRecord, tblInvoiceDeleteLog.
- WRITTEN via `sp_Save_TopSheetGenReportCode`: a top-sheet code table (name UNKNOWN).

## 18. Validation Matrix
| Rule | Layer | Condition | Message |
|---|---|---|---|
| DC, date, DA required to search | server `SearchButton_Click` | any empty | faildalert "Please Select All Parameters!" |
| No data | server | 0 rows | faildalert " No Data Found!" (leading space) |
| DA required for top sheet | server `viewRptButton_Click` | `ddlDAName.SelectedValue == ""` | faildalert "Please Select DA Name!" |
| At least one ticked row | server | InvoiceIds empty | faildalert "Please Select at Least one row from Table!" (checked AFTER code creation) |
| Date format | none | `Convert.ToDateTime(InvoiceDateTextBox.Text)` for the viewer date | FormatException swallowed by `try/catch` |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M07-034 | Search lists the selected DC's invoices for the chosen date whose `Inv_DANameId` equals the selected DA; "Total Amount" shown is SUM(tblInvoiceDetail.NetAmount) per invoice (the SQL aliases that sum as `TpGrandTotal`, shadowing the header column). Route, market, manufacturer and territory are not filters. Delivered/confirmed invoices are NOT excluded (no `DelivaryInvoiceNo` filter). | `TopSheetGenerate.aspx.cs:141-150`; `StockConditionFreezeDAL.cs:161-180` | inline SQL | CONFIRMED |
| BR-M07-035 | **Search is destructive**: after binding the grid, every returned invoice is tested with `sp_Check_anomalyInvoiceDetails`; if it returns a row the invoice is deleted with `sp_Delete_ProformaInvoice(InvoiceNo, LoginName)` right away. The grid was bound before the delete, so deleted invoices remain visible until the next search. The same pattern exists in M06 (`InvoiceCreationByOrder.aspx.cs:131,259`, `InvoiceCreationForCustomerByOrder.aspx.cs:3140`) and `ProformaorInvoiceReturn.aspx.cs:57` (manual delete). | `TopSheetGenerate.aspx.cs:151-166` | code-behind | CONFIRMED |
| BR-M07-036 | Anomaly criterion: invoice date after 2022-11-16 and `ABS((tblInvoice.TpGrandTotal - ISNULL(AdjustAmount,0)) - SUM(tblInvoiceDetail.NetAmount)) > 10`. Because header `TpGrandTotal` is written as `SUM(NetAmount) + DeliveryCharge` (`InvoiceDAL.cs:507-535`), an invoice whose DeliveryCharge or AdjustAmount differs from the other by more than 10 currency units is flagged and (through BR-M07-035) auto-deleted (INFERRED risk; the anomaly script's tolerance and AdjustAmount handling were added by `SP_Fix_Invoice_Anomaly_And_Sync.sql`, which also contains a one-time UPDATE that re-syncs headers with `TpGrandTotal = SUM(NetAmount)` without DeliveryCharge). | `SP_Fix_Invoice_Anomaly_And_Sync.sql:12-72`; `InvoiceDAL.cs:507-535` | SP + inline SQL | CONFIRMED (criterion) / INFERRED (DeliveryCharge false positive) |
| BR-M07-037 | `sp_Delete_ProformaInvoice` acts only if the invoice exists with `DelivaryInvoiceNo IS NULL` (i.e., still a proforma); otherwise it does nothing and silently returns. | `sp_Delete_ProformaInvoice_Update.sql:15` | SP | CONFIRMED |
| BR-M07-038 | Stock reversal per detail row (cursor over tblInvoiceDetail): if `DCStoreId > 0`, DELETE TOP (1) the tblDCStore `Issue` row with `ReferenceDCStoreId = DCStoreId AND Trans_Type='Issue' AND StockQty = -TotalQuantity` (an exact quantity match is required - an Issue row of a different quantity is left behind, orphaning stock; see `sp_Check_DCStore_StockAnomalies.sql`), and repair any Receive row where `StockQty > TotalQuantity` by setting StockQty = TotalQuantity. The Receive row is otherwise not credited (ledger model: deleting the Issue row restores availability). | `sp_Delete_ProformaInvoice_Update.sql:110-129` | SP | CONFIRMED |
| BR-M07-039 | Delete bookkeeping: per detail INSERT tblProInvoiceReturnTrack(ReturnTotalQuantity, DCStoreId, ReturnDate, ReturnExecutionDateTime, InvoiceDetailId, InvoiceId) and a full copy of the line into tblInvoiceDetail_DeleterRecord, then DELETE the detail; afterwards `tblOrderDetail.Status = NULL` for all lines of the last-seen OrderId, `tblOrder.IsInvoice = 0`, DELETE tblReturnAmount for the invoice, DELETE tblInvoice, INSERT tblInvoiceDeleteLog(InvoiceId, InvoiceNo, DeleteDateTime, DeleteBy=@User). No BEGIN TRAN/TRY-CATCH (partial deletes possible); an invoice with zero detail rows leaves `@OrderIdForUpdate` NULL so the order is not reset; the log does not receive ComUnitId/DelInvoiceNo although the deleted-invoice report selects `mas.ComUnitId` and `mas.DelInvoiceNo` (see GAP-M07-016). tblInvoiceBatch rows are not removed. | `sp_Delete_ProformaInvoice_Update.sql:130-203` | SP | CONFIRMED |
| BR-M07-040 | Top sheet needs a DA (name text goes to `@DeliveryMan`) and >=1 ticked invoice; **a new top-sheet code is created on every click of View TopSheet Report, even when no row is ticked** (code created before the ticked-row check). Report parameters: `InvNo` = comma list of InvoiceIds, `Code` = `TopSheetGenCode`, session `ProformaTopSheet=1`, `dcDropDownList1`, `Route=0`, `InvoiceDateTextBox`. | `.aspx.cs:217-305` | code-behind + SPs | CONFIRMED |
| BR-M07-041 | "Print Invoice Report" and the top-sheet viewers receive InvoiceIds while the report main-data queries filter `IV.InvoiceNo IN ('<text>')` (`InvoiceDAL.InvoiceMainDatainvNo` `:1461+`, `InvoiceMainDataForReport` `:1302+`, `InvoiceNoCollectionFormate` splits on ':' not ','): a comma list of numeric ids does not match invoice numbers of the form `INV-...`. Detail datasets (`InvoiceDetailDataForReportNew_`, `MarketPickinReportNew_`) use `I.InvoiceId IN (...)` and are correct. Effect on the printed output is INFERRED (main header data may be empty) - verify in a runtime test. | `TopSheetGenerate.aspx.cs:288-336`; `TopSheetReportViewer.aspx.cs:25-49`; `ProformaReportPrintViewer.aspx.cs:101-113`; `InvoiceDAL.cs:1302,1461,2078,2112`; `InvoiceBLL.cs:753-776` | inline SQL | CONFIRMED (query text) / INFERRED (output) |

## 20. Permission Matrix
- Session keys read: `UserId` (DC list, `@EntryBy`), `LoginName` (`@User` for deletes, read inside the DAL), plus keys written for viewers (`ProformaTopSheet`, `dcDropDownList1`, `manufacturerDropDownList`, `MarketDropDownList1`, `InvoiceDateTextBox`, `Route`, `Terr`, `paydetailId`).
- No role or UserType check. The commented line `//if (Session["UserId"] == "9" || "11")` shows the button panel was once restricted to two user ids; now every user with the menu item can run Search (with delete side effect).
- The DA/DC scoping relies on the dropdown lists; nothing re-verifies that the posted DC belongs to the user (SQL is parameterised, so no injection here).

## 21. Status Lifecycle
Invoice states touched: proforma invoice (DelivaryInvoiceNo NULL) -> **deleted** (row removed, logged in tblInvoiceDeleteLog / tblInvoiceDetail_DeleterRecord, order `IsInvoice` 1->0 and `tblOrderDetail.Status` -> NULL, so the order re-appears for invoicing). No status column is written on a printed invoice. Top-sheet code creation is an insert only. Consolidated lifecycle: README.

## 22. Error Handling
Route/DA loads swallow all exceptions (empty catch); `viewRptButton_Click` swallows everything (a failed SP produces no message and no popup); Search has no try/catch (SP errors bubble). Alerts via SweetAlert helpers `faildalert`.

## 23. Audit
Delete audit: tblInvoiceDeleteLog (InvoiceId, InvoiceNo, DeleteDateTime, DeleteBy), tblInvoiceDetail_DeleterRecord (full line copy), tblProInvoiceReturnTrack (qty returned to stock). Top sheet: `@EntryBy` = UserId, `@DeliveryMan`. Automatic deletions are logged with the user who happened to press Search.

## 24. Integration
Report launch only. No SAP/accounting call.

## 25. Reports
`TopSheetReportViewer.aspx` -> `rptTopSheet.rpt` (datasets mainDataTable, detailDataTable, companyInfoDataTable, marketwisepickingDataTable), `ProformaReportPrintViewer.aspx` -> `rptInvoiceForCustomer_Uncr.rpt`, `InvoiceReportViewer.aspx` -> `rptInvoiceForCustomer_Uncr.rpt`. Details in `../reports/RPT-M07-02-top-sheet-picking-challan.md` and `RPT-M07-01-invoice-and-proforma-print.md`.

## 26. Dependencies
BLL: OtherStockActionBLL, OrderInfoBLL, StockConditionFreezeBLL, ProformaOrInvoiceReturnBLL. DAL: StockConditionFreezeDAL, ProformaOrInvoiceReturn, CommonDataLoad, SeedDataDAL, RouteInformationDAL, SInventorySql. SPs: 6 (2 bodies in repo). Tables: tblInvoice, tblInvoiceDetail, tblOrder, tblOrderDetail, tblDCStore, tblCustMaster, tblProduct, tblReturnAmount, tblInvoiceDeleteLog, tblInvoiceDetail_DeleterRecord, tblProInvoiceReturnTrack.

## 27. Test Cases
| ID | Scenario | Steps | Expected | BR |
|---|---|---|---|---|
| TC-277-01 | Missing DA | select DC + date, no DA, Search | faildalert "Please Select All Parameters!" | BR-M07-034 |
| TC-277-02 | Search happy | DC, date, DA with 3 clean invoices | grid of 3, total amount = detail net sum | BR-M07-034 |
| TC-277-03 | Search with anomaly | invoice with header 1000 vs details 850 (diff 150) | invoice deleted, Issue rows removed, order `IsInvoice=0`, log row; grid still shows it once | BR-M07-035..039 |
| TC-277-04 | Tolerance | header vs details differ by exactly 10 / 10.01 | 10 -> kept, 10.01 -> deleted | BR-M07-036 |
| TC-277-05 | Old invoice | invoice date 2022-11-16 with mismatch | not flagged | BR-M07-036 |
| TC-277-06 | Delivery charge | invoice with DeliveryCharge 50, no adjustment | flagged (INFERRED) - confirm at runtime | BR-M07-036 |
| TC-277-07 | Confirmed invoice | anomalous invoice with DelivaryInvoiceNo set | SP does nothing | BR-M07-037 |
| TC-277-08 | Stock reversal | invoice with 2 batch lines, one Issue row quantity mismatched | matching Issue rows deleted, the mismatched one orphaned | BR-M07-038 |
| TC-277-09 | Top sheet without DA | click View TopSheet Report | "Please Select DA Name!" | BR-M07-040 |
| TC-277-10 | Top sheet no rows ticked | DA chosen, none ticked | new code still created; "Please Select at Least one row from Table!" | BR-M07-040 |
| TC-277-11 | Top sheet happy | tick rows | popup with Code; sheet lists selected invoices and market-wise picking | BR-M07-040/041 |
| TC-277-12 | Print invoice report | tick rows | popup; verify headers populate (invoice ids vs numbers) | BR-M07-041 |
| TC-277-13 | Session loss | expire session, click View TopSheet | silent failure (swallow) | - |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/TopSheetGenerate.aspx`, `Solution.Web/SInventory_UI/TopSheetGenerate.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).


## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | page present | n/a | MISSING | - |
| Search semantics | None existed | deletes anomalous invoices | SP body in repo | GAP | BR-M07-035 |
| Anomaly tolerance | None existed | 10 currency units, after 2022-11-16 | script | DOCUMENTED | BR-M07-036 |
| Deleted-invoice report data | None existed | report selects columns the SP does not populate | log table columns UNKNOWN | GAP | GAP-M07-016 |

## 30. Known Gaps
- Read action (Search) performs irreversible deletes of invoices with no confirmation and no per-user authority (GAP-M07-015).
- `sp_Delete_ProformaInvoice`: no transaction; exact-quantity Issue-row match can orphan stock; zero-detail invoices leave the order flagged invoiced; tblInvoiceBatch left behind; log lacks ComUnitId/DelInvoiceNo (GAP-M07-016, GAP-M07-017).
- Anomaly rule may flag invoices with delivery charge or adjustment (GAP-M07-018).
- Top-sheet code created before validation; `viewRptButton_Click` swallows all exceptions (GAP-M07-019).
- Report parameters use InvoiceIds while report SQL keys on InvoiceNo (GAP-M07-020).
- Grid total alias `TpGrandTotal` (sum of details) shadows the header column in the same result set (`SELECT ..., tblD.ManufacId AS TpGrandTotal, *` -> duplicate column names).
- Session state used as viewer parameters (cross-tab collisions); `hiddiv` restriction removed.
- Four SP bodies absent (GAP-M07-012).

## 31. Change Impact
- `sp_Delete_ProformaInvoice` / `sp_Check_anomalyInvoiceDetails` are shared with M06 pages `InvoiceCreationByOrder` (`:131,259`), `InvoiceCreationForCustomerByOrder` (`:3140`), `ProformaorInvoiceReturn.aspx.cs:57` and this page - a change to either SP changes all of them.
- `DelivaryTopSheetGenerate` (PAGE-265) uses different viewers (`DelivaryInvoiceReturnViewer`, `DelivaryTopSheetInvoiceReturnViewer` / `rptDelivaryTopSheet.rpt`); `TopSheetReportViewer` is opened from this page (other callers not exhaustively searched).
- `StockConditionFreezeDAL.LoadInvoiceByDAName` and `SeedDataDAL.Save_TopSheetGenReportCodeInfo` are called only from this page in `Solution.Web` (grep).
- `tblDCStore` Issue rows are also produced by every invoice-creation path (BR-M07-013).
- Cross-links: M06 `../order-management/pages/PAGE-217-invoicecreationbyorder.md`, `PAGE-218-invoicecreationforcustomerbyorder.md`.
