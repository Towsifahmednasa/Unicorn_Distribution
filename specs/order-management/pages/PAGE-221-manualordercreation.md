---
page_id: PAGE-221
module: M06
page: ManualOrderCreation
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "COMPLETE", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-221 — ManualOrderCreation

> **Evidence level:** Tier A — read in full: `Solution.Web/SInventory_UI/ManualOrderCreation.aspx(.cs)`, `Library.BLL/SInventory_BLL/OrderListBLL.cs`, `Library.DAL/SInventory_DAL/OrderListDAL.cs`, `OrderInfoDAL.LoadOrderWithDetail`, and the three SP bodies in `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql`. `status` is PARTIAL because `tblOrder.ActionStatus` value vocabulary is defined outside the repo (`tblAction*`/mobile app SPs). Product lookup (`DCStoreBLL.ProductInfoNew`) was traced by name only (see GAP-M06-014).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-221 |
| Module | M06 — Order Management |
| Page name | ManualOrderCreation (create AND edit of a "Web Manual" sales order) |
| Kind | Entry / Transaction (CONFIRMED) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_ManualOrderCreation |
| Code-behind LOC | 1043 (aspx 454) |
| Suspected legacy/duplicate | Menu link is commented out in `Solution.Web/MasterPages/MasterPage.master:1208`; reachable from `OrderListView.aspx` (New Order button + Edit) — INFERRED that the DB-driven menu (`NewMasterPage`) may still expose it |

## 2. Purpose
Lets a back-office user key in a sales order on behalf of a customer/MIO (field officer), choose the distribution centre (DC) that will fulfil it, add product lines with quantity and trade price (TP) / VAT, and save it as an order that is immediately ready for proforma-invoice creation. The same page re-opens an existing, not-yet-invoiced order (`?OrderId=` or `?ID=`) and rewrites it. CONFIRMED (`ManualOrderCreation.aspx.cs:24-35, 776-784`; SP `sp_Save_ManualOrderMaster`).

## 3. Business Objective
Capture orders that did not arrive from the MIO mobile app or Excel upload (phone/walk-in/transfer orders — e.g. the data script `Create_Order_ODR-20260901134.sql` recreates such an order: remark "Transfer from Akij outlate Banani", `OrderSenderType='Web Manual'`). Because the SP inserts `ActionStatus='2'` and `IsPrepareforInvoice=1`, these orders skip approval and appear on the proforma-creation list (PAGE-217). INFERRED for the business intent; CONFIRMED for the state written.

## 4. Navigation
Menu is DB-driven (unknown rows). Inbound: `OrderListView.aspx` ("New" button `OrderListView.aspx:16`, row Edit `:48`, preview-modal Edit link `OrderListView.aspx.cs:109`). Outbound: `Response.Redirect("OrderListView.aspx")` after an *edit* save (`:847`); `resetbtn` reloads `ManualOrderCreation.aspx` (`:1041`); dead handler `miaImageButton_Click` → `OrderRequisitionView.aspx` (`:1000`).

## 5. Route
`~/SInventory_UI/ManualOrderCreation.aspx[?OrderId=<id>|?ID=<id>]`. `OrderId` wins over `ID` (`:26`).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/ManualOrderCreation.aspx |
| Code-behind | Solution.Web/SInventory_UI/ManualOrderCreation.aspx.cs |
| BLL | Library.BLL/SInventory_BLL/OrderListBLL.cs, OrderInfoBLL.cs, DCStoreBLL.cs |
| DAL | Library.DAL/SInventory_DAL/OrderListDAL.cs, OrderInfoDAL.cs, DCStoreDAL.cs |
| DAO | Library.DAO/SInventory_Entities/OrderInfoMaster.cs, OrderInfoDetail.cs |
| SQL bodies | LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:939 (sp_Save_ManualOrderMaster), :1114 (sp_Update_ManualOrderMaster), :1375 (sp_Save_ManualOrderDetail) |

## 7. UI Control Inventory
No ASP validators exist on the page; all validation is JS (`validateOrderForm`) + server (`Validation()`).
| Control | Label | Required | Default | Read-only / visibility | Data source | Error / behaviour |
|---|---|---|---|---|---|---|
| custCodeTextBox (+AutoCompleteExtender) | "Type Customer Name, Code or Mobile No..." | Yes (JS `:344`, server `:863`) | empty | postback on text change | autocomplete service (UNKNOWN endpoint) → value `Code:Name|Id` parsed at `:1006-1037` | "Please Select a valid Customer!" / "Input Correct Data !!" |
| hfCustomerId | — | — | — | hidden | parsed from text after `|` | — |
| ddlSalesPerson | Sales person (MIO) | Yes (JS `:351`, server `:881`) | "-- Select Sales Person --" (value "") | AutoPostBack → reloads DC list | `tblEmpGeneralInfo` ALL employees ordered by name, text `EmpMasterCode : EmpName` (`OrderListDAL.cs:221`) | "Please Select Sales Person (MIO)!" |
| lblAddress / lblCellNo / custNameLabel / mioCodeLabel / mioNameLabel / teritory / FCBLabel3 / marketNameLabel | customer info panel `pnlCustomerDetails` | — | shown only when customer resolves | labels | `View_CustomerMaster` by CustomerCode (`OrderListDAL.cs:237`) | panel hidden if no row |
| orderDateTextBox | order date | No (falls back to `DateTime.Now`) | today, format `dd MMMM, yyyy` (`:187`) | pickadate | — | unparseable → tries formats `dd MMMM, yyyy`, `dd-MMM-yyyy`, `dd/MM/yyyy`, `yyyy-MM-dd` (`:700-703`) |
| dcDropDownList | Distribution Center | Yes (JS `:362`, server `:888`) | list by sales person (see BR-007) | — | `tblCompanyUnit` (`ComUnitCode:ComUnitName`) or user-DC mapping | "Please Select Distribution Center!" |
| remarksTextBox | optional remarks | No | empty | — | — | stored in `tblOrder.Remarks` |
| orderNoTextBox | order no. | — | `ORD-000000n` proposal (new) / existing code (edit) | (not ReadOnly in markup grep; treat as editable) | `OrderListBLL.OrdNo()` | SP may replace it (BR-008) |
| hfOrderId | — | — | from querystring | hidden | — | non-empty ⇒ edit mode |
| productGridView row: productCodeTextBox / productNameTextBox (AutoComplete) | Product Code / Name | Yes per row (server `:907`) | empty | postback on change → fills pack, price, VAT | `DCStoreBLL.ProductInfoNew(code)` (tblProduct+tblUnitPrice) | "Please select valid Product in row #n" |
| packSizeTextBox, vatTextBox, TotaltpTextBox, TotaltpVatTextBox | pack, unit VAT, totals | — | — | ReadOnly | JS recalculates totals | — |
| reqQtyTextBox | Req. Qty | Yes, decimal > 0 (server `:915`) | empty | `onkeyup=calculateRow` | user | "Please enter valid Req. Qty in row #n" |
| tpTextBox | Unit TP (Tk) | — | from tblUnitPrice | editable (user can override price) | `ProductInfoNew.UnitPrice` | none (no ceiling/floor) |
| unitpriceHiddenField / productidHiddenField | — | productId ≤0 rows are skipped on save | — | hidden | — | — |
| IsGiftProductDropDownList / IsCampaignProductDropDownList | — | — | "False" | column `Visible=false` | static True/False | saved via `bool.TryParse` (BR-013) |
| btnRowAdd / btnRowDelete | add/remove row | — | — | LinkButton postbacks (rebuild whole grid from controls) | — | delete of last row re-inits an empty grid |
| submitButton | "Save"/"Update Order" | — | — | `OnClientClick=validateOrderForm()`; disabled + "Order Locked (Invoiced)" when edit target already invoiced | — | see BR-001 |
| resetbtn | reset | — | — | redirect to self | — | — |
| lblSumQty/lblSumTP/lblSumVAT/lblSumNetPayable | grand-total badges | — | — | JS only (`calculateGrandTotals`) | — | not posted |

## 8. Field Mapping
| UI control | DTO field | Handler | BLL → DAL | SP parameter | Table.column (from SP body) |
|---|---|---|---|---|---|
| orderNoTextBox | OrderInfoMaster.OrderCode | SaveAllData `:718` | OrderListBLL.SaveOrderMaster → OrderListDAL.SaveOrderMaster | @OrderCode (InputOutput NVARCHAR(50)) | tblOrder.OrderCode (may be regenerated) |
| teritory.Text (`TerritoryCode` of customer) | teritory | `:725` | ″ | @TerritoryCode | tblOrder.TerritoryCode |
| dcDropDownList value / text split by ':' | ComUnitId, ComUnitCode (left), ComUnitName (right) | `:676-695` | ″ | @ComUnitId/@ComUnitCode/@ComUnitName | tblOrder.ComUnitId/ComUnitCode/ComUnitName |
| ddlSalesPerson text split by ':' / value | MIOCode, MIOName, MIOId | `:650-664` | ″ | @MIOCode/@MIOName/@MIOId (MIOId NULL if 0) | tblOrder.MIOCode, MIOName; **@MIOId is accepted but never written** (not in INSERT column list, SP `:1005-1090`) |
| constant 1 | ManufacId | `:726` | ″ | @ManufacId | tblOrder.ManufacId |
| custCodeTextBox / custNameLabel | CustomerCode / CustomerName | `:727-729` | ″ | @CustomerCode/@CustomerName | tblOrder.CustomerCode/CustomerName; SP also looks up tblCustMaster → CustomerMasterId, MarketId, CustTypeId, ProgramTypeId, DistributionRouteId, SMC type and Group/Region/Area/Territory/SubTerritory ids+names (`*_Ord` columns) |
| Σ tp×qty, Σ vat×qty | GrossValue, TotalVat, TotalDiscount=0, TotalNetPayable | `:735-765` | ″ (`TotalNetPayable>0 ? it : GrossValue`) | @GrossValue/@TotalVat/@TotalDiscount/@TotalNetPayable | tblOrder.GrossValue/TotalVat/TotalDiscount/TotalNetPayable |
| orderDateTextBox | SubmissionDate | `:697-704` | ″ | @SubmissionDate | tblOrder.SubmissionDate |
| FCBLabel3 ("1"/"true") | FCB | `:706-714` | ″ | @FixedCustomer | tblOrder.FixedCustomer |
| remarksTextBox | Remarks | `:732` | ″ | @Remarks | tblOrder.Remarks |
| (constants inside SP) | — | — | — | — | OrderType='Regular', IsManual=1, IsFromApp=0, IsInvoice=0, IsPrepareforInvoice=1, IsSubDepo=0, ActionStatus='2', OrderSenderType='Web Manual', EntryDate=ServerDateTime=GETDATE() |
| row productidHiddenField | ProductId | `:809-810,826` | OrderListBLL.SaveOrderDetail → OrderListDAL.SaveOrderDetail | @ProductId | tblOrderDetail.ProductId |
| row code/name | ProductCode/ProductName | `:824-825` | ″ | @ProductCode/@ProductName | tblOrderDetail.ProductCode/ProductName |
| reqQtyTextBox | Quantity | `:827` | ″ | @Quantity | tblOrderDetail.Quantity |
| tpTextBox (else hidden unit price) | TradePrice | `:813-816,829` | ″ | @TradePrice | tblOrderDetail.TradePrice |
| qty×TP | TotalTradePrice | `:830` | ″ | @TotalTradePrice | tblOrderDetail.TotalTradePrice |
| vatTextBox | VatAmountPerUnit / TotalVatAmount (=qty×vat) | `:831-832` | ″ | @UnitVatAmount/@TotalVatAmount | tblOrderDetail.UnitVatAmount/TotalVatAmount |
| TP+VAT | NetAmount | `:833` | ″ (`NetAmount>0 ? : TotalTradePrice`) | @NetAmount | tblOrderDetail.NetAmount |
| gift / campaign dropdown text | IsgiftProduct / IsCampaignProduct (string) | `:835-839` | `bool.TryParse` (`OrderListDAL.cs:192-193`) | @ISGiftProduct/@IsCampaignProduct | tblOrderDetail.ISGiftProduct/IsCampaignProduct; SP forces IsSpDis=0, DiscountPercent=0, DiscountAmount=0 |

## 9. Dropdown Mapping
| Dropdown | Loader | Source | Text / Value | Notes |
|---|---|---|---|---|
| ddlSalesPerson | DropDownLoad `:179` → `OrderListDAL.LoadSalesPerson` | `SELECT EmpInfoId, EmpMasterCode+' : '+EmpName FROM tblEmpGeneralInfo ORDER BY EmpName` | EmpDisplayName / EmpInfoId | no role/status filter → includes every employee record |
| dcDropDownList (default) | `OrderListDAL.DCLoad` | `SELECT ComUnitId, ComUnitCode+':'+ComUnitName FROM tblCompanyUnit` | Com / ComUnitId | inserted "-- Select Distribution Center --" |
| dcDropDownList (by sales person) | `LoadDCBySalesPerson` → `OrderListDAL.GetDCBySalesPerson` | `tblUserCompanyUnit ⨝ tblUser(EmpInfoId=@) ⨝ tblCompanyUnit` DISTINCT | `ComUnitCode : ComUnitName` / ComUnitId | 1 row ⇒ auto-selected, >1 ⇒ prompt, 0 ⇒ fallback to all DCs. NB: text contains " : " (spaces) whereas save splits on ':' and trims — OK |
| IsGift/IsCampaignProductDropDownList | static True/False items in aspx | — | — | hidden column |

## 10. Button Action Mapping
| Button | Handler | DB effect |
|---|---|---|
| btnRowAdd | ImageButton1_Click `:482` | none (rebuilds grid from posted controls, appends blank row) |
| btnRowDelete | ImageButton2_Click `:486` | none |
| submitButton | submitButton_Click `:924` → Validation `:861` → SaveAllData `:634` | INSERT/UPDATE tblOrder, DELETE+INSERT tblOrderDetail (via 3 SPs) |
| resetbtn | resetbtn_Click `:1039` | none |

## 11. Frontend Flow
1. `Page_Load` (first load): `DropDownLoad()` (sales-person + all DCs), `Todate()`. If `OrderId`/`ID` present → `LoadOrderForEdit`, else `InitialGrid()` + `OrdNo()` (`:19-35`).
2. Customer autocomplete → `custCodeTextBox_TextChanged` (`:1006`) parses `Code:Name|Id`, calls `CustomerInfo` which fills the info panel and pre-selects sales person / DC (BR-007).
3. Product code/name text change → postback → `GetProductInGrid` fills name, pack, unit price (TP) and unit VAT.
4. JS (`aspx:342-451`): `calculateRow` (qty×TP, qty×VAT per row), `calculateGrandTotals` (sum qty, TP, VAT, net = TP+VAT, shown in badges), `validateOrderForm` (customer, sales person, DC, at least one row with qty>0), `pageLoad` re-inits pickadate + select2 after each partial postback.
5. Submit: `validateOrderForm()` client gate, then server `submitButton_Click`.

## 12. Backend Flow
`submitButton_Click`: if `hfOrderId` non-empty → `LoadOrderWithDetail(id)`; if `IsInvoice` is 1/true → alert and stop (BR-001). → `Validation()` (BR-002..005) → `SaveAllData()` (inside try/catch; **no DB transaction**):
1. Requires sales person again; derives MIO code/name from the dropdown text; customer name fallback lookup; DC id/code/name split on ':'.
2. Totals loop (BR-006).
3. New: `OrderListBLL.SaveOrderMaster(dto, out orderId)` → `OrderListDAL.SaveOrderMaster` opens connection → `sp_Save_ManualOrderMaster` (OrderCode InputOutput, OrderId Output). Edit: `UpdateOrderMaster` → `sp_Update_ManualOrderMaster` (deletes all `tblOrderDetail` for the order).
4. If `!masterSaved || orderId<=0` → alert "Error saving Order. Please check details and try again." and stop.
5. Builds detail list (skips rows with productId ≤0 or qty ≤0), `OrderListBLL.SaveOrderDetail(list)` loops `sp_Save_ManualOrderDetail` once per line, each with its own connection; **return values ignored** (BLL always returns "Data Saved Successfully").
6. Edit ⇒ `Response.Redirect("OrderListView.aspx")`; new ⇒ `Clear()` + alert "Order <code> Created Successfully!".
7. Any exception ⇒ alert "Error: " + message (raw exception text shown to user).

## 13. API Mapping
No `[WebMethod]`/handler on this page. Customer/product autocompletes are `AutoCompleteExtender`s pointing at service paths not read here (UNKNOWN — see GAP-M06-014).

## 14. Service Mapping
OrderListBLL: OrdNo (`:25`), OrdnNoGenerator (`:63`), SaveOrderMaster (`:16`), UpdateOrderMaster (`:21`), SaveOrderDetail (`:98`), CustomerInfo (`:94`), LoadSalesPerson (`:54`), GetDCBySalesPerson (`:59`), DCLoad (`:111`). OrderInfoBLL.LoadOrderWithDetail (`OrderInfoBLL.cs:184`). DCStoreBLL.ProductInfoNew (`DCStoreBLL.cs:94`).

## 15. Repository Mapping
`OrderListDAL.cs`: SaveOrderMaster `:90`, UpdateOrderMaster `:147`, SaveOrderDetail `:183`, LoadSalesPerson `:219`, CustomerInfo `:235` (`View_CustomerMaster`), OrderManualId `:244` (`SELECT isnull(MAX(SUBSTRING(OrderCode,5,15)),0)+1 FROM tblOrder WHERE IsManual='True'`), DCLoad `:250`, GetDCBySalesPerson `:257`. `OrderInfoDAL.LoadOrderWithDetail(string)` `:602` (tblOrder ⟕ tblCustMaster ⟕ tblCompanyUnit ⟕ tblMarket ⟕ tblOrderDetail ⨝ tblProduct, with computed `IsCampaignProduct = CASE WHEN DiscountAmount>0 THEN 0 ELSE 1 END`). All connections via `DataAccessManager` with SP execution `SaveData` (`DataAccessManager.cs:491`, no transaction).

## 16. Stored Procedure Mapping
| SP | Body | Read | Write |
|---|---|---|---|
| sp_Save_ManualOrderMaster | CONFIRMED `:939-1112` | tblCustMaster, tblSMCType, tblMarket, tblSubTerritory, tblTerritory, tblArea, tblRegion, tbl_Group, `MAX(OrderId)` of tblOrder | INSERT tblOrder |
| sp_Update_ManualOrderMaster | CONFIRMED `:1114-1214` | same lookups | UPDATE tblOrder (by OrderId) + DELETE tblOrderDetail (by OrderId) |
| sp_Save_ManualOrderDetail | CONFIRMED `:1375-…` | — | INSERT tblOrderDetail |

## 17. Database Mapping
Written: `tblOrder`, `tblOrderDetail`. Read: `View_CustomerMaster`, `tblEmpGeneralInfo`, `tblCompanyUnit`, `tblUser`, `tblUserCompanyUnit`, `tblCustMaster`, `tblProduct`, `tblUnitPrice` (product lookup, INFERRED), and the hierarchy tables above. Details of columns in §8/§16.

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Error message | BR |
|---|---|---|---|---|---|
| Customer | non-empty code and resolvable name | JS `:344` | `Validation():863-879` | "Please Select a valid Customer!" | BR-003 |
| Sales person | index>0, value not "0" | JS | `Validation():881`, `SaveAllData:643` | "Please Select Sales Person (MIO)!" | BR-002 |
| DC | value non-empty | JS | `:888` | "Please Select Distribution Center!" | BR-004 |
| Rows | ≥1 row; each row product code+name present; qty parses >0 | JS: only ≥1 row with qty>0 | server per-row | "Please Add at least one Product item!" / "Please select valid Product in row #n" / "Please enter valid Req. Qty in row #n" | BR-005 |
| Edit target | not invoiced | button disabled at load | `:926-943` | "Invoice has already been generated for this Order! Order cannot be updated." | BR-001 |
| Product/customer text | must contain ':' | — | `:982-996, 1009-1036` | "Input Correct Data!!" / "Input Correct Data !!" | — |
| Price, qty upper bounds, stock, credit limit, duplicate product, date in past | **not validated** | — | — | — | GAP-M06-012 |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M06-001 | An order whose `IsInvoice` is 1/true cannot be edited: submit button disabled ("Order Locked (Invoiced)") on load and re-checked on submit. The SP itself has no such check. | ManualOrderCreation.aspx.cs:46-54, 926-943; SP `:1114-1214` | page only | CONFIRMED |
| BR-M06-002 | Sales person (MIO) is mandatory. | .cs:643-648, 881-886; aspx JS:351 | page | CONFIRMED |
| BR-M06-003 | Customer is mandatory and must resolve to a name in `View_CustomerMaster`. | .cs:863-879 | page | CONFIRMED |
| BR-M06-004 | Distribution centre is mandatory. | .cs:888-893; JS:362 | page | CONFIRMED |
| BR-M06-005 | ≥1 product row; every row needs code+name and qty>0; on save rows with productId≤0 or qty≤0 are silently dropped (so a row with typed name but no resolved product id disappears). | .cs:895-921, 820-821 | page | CONFIRMED |
| BR-M06-006 | Totals: Gross=Σ(TP×qty); TotalVat=Σ(unitVAT×qty); Discount=0; Net=Gross+VAT; line net=qty×TP+qty×unitVAT. No discount, campaign or trade-policy logic is applied at manual order entry (SP writes DiscountPercent/Amount=0, IsSpDis=0). No rounding is applied (decimal). | .cs:735-765, 826-833; SP detail `:1375+` | page + SP | CONFIRMED |
| BR-M06-007 | Defaults from customer: if customer has tagged sales persons: 1 ⇒ select it; >1 ⇒ select `SalesPersonId` only if it is in the tag list, else blank; none tagged ⇒ `SalesPersonId` then `MIOEmpInfoId`. DC list = DCs mapped to the chosen sales person's user (`tblUserCompanyUnit`); single ⇒ auto-select; multiple ⇒ manual; none ⇒ all DCs. | .cs:190-308; OrderListDAL.cs:257-268 | page + inline SQL | CONFIRMED |
| BR-M06-008 | Order number: page proposes `ORD-` + 7-digit zero-padded `MAX(SUBSTRING(OrderCode,5,15))+1` over manual orders. The SP replaces the code with `ODR-<yyyyMMdd><MAX(OrderId)+1>` **only when** the incoming code is NULL/'' or matches `LIKE 'ORD-000000%'` (i.e. proposals 1–9 only); any other incoming value (including proposals ≥10, or an edit) is stored as posted. Real codes in production are of the `ODR-yyyymmddN` form (see `Create_Order_ODR-20260901134.sql`). | OrderListDAL.cs:244-248; OrderListBLL.cs:25-93; SP :999-1004 | page + SP | CONFIRMED (code) / INFERRED (effect on live sequence) |
| BR-M06-009 | A new web-manual order is born approved and invoice-ready: OrderType='Regular', IsManual=1, IsFromApp=0, IsInvoice=0, IsPrepareforInvoice=1, IsSubDepo=0, ActionStatus='2', OrderSenderType='Web Manual'. It therefore satisfies the invoice-list filter of `sp_LoadOrderListForOrderCreationbyTerri` (:1356-1362). | SP :1058-1101 | SP | CONFIRMED |
| BR-M06-010 | Market hierarchy (market, sub-territory, territory, area, region, group, customer type, program type, route, SMC type) is snapshotted from `tblCustMaster` into the order at save/update; on update each is kept if lookup returns NULL (`ISNULL(@x, col)`). | SP :954-997, 1170-1207 | SP | CONFIRMED |
| BR-M06-011 | Editing rewrites the whole order: header updated (and forced back to ActionStatus='2', IsPrepareforInvoice=1, IsSubDepo=0, OrderType='Regular'); ALL detail rows deleted, then lines re-inserted by separate calls (no transaction). | SP :1173-1213; .cs:776-844 | SP + page | CONFIRMED |
| BR-M06-012 | Unit TP is preloaded from the price list but the user may overwrite it; the saved TradePrice is `tpTextBox` if non-empty else the hidden list price. There is no upper/lower limit. | .cs:813-816; aspx grid | page | CONFIRMED |
| BR-M06-013 | Gift/Campaign flags default false and are written as posted (`bool.TryParse`). In edit mode the campaign flag is pre-filled from a computed column (`1` when line DiscountAmount = 0), which is the reverse of the natural meaning, so re-saving an existing order may set `IsCampaignProduct=True` on non-discounted lines. | OrderInfoDAL.cs:604; .cs:134-137; OrderListDAL.cs:190-193 | SQL + page | CONFIRMED (query) / INFERRED (effect) |

## 20. Permission Matrix
No `Session` role/permission check in this page (grep of `.cs`: no `Session[` usage). Access = menu placement only (DB-driven). No filter restricts which customers/sales persons/DCs a user may select (sales-person list = all employees; DC list narrowed only by sales person mapping). Authentication via master-page/Forms. CONFIRMED.

## 21. Status Lifecycle
`tblOrder`: new → `ActionStatus='2'`, `IsInvoice=0`, `IsPrepareforInvoice=1` (SP :1058-1101) → edit keeps the same (SP :1184-1195) → invoiced when PAGE-218 sets `IsInvoice='True'` (`OrderInfoDAL.UpdateInvoiceStatus:1127`) → back to `IsInvoice=0` if the proforma is deleted (`sp_Delete_ProformaInvoice`, `sp_Delete_ProformaInvoice_Update.sql:191-192`). Meaning of `ActionStatus` '2' vs 'Accepted'/'Approved': UNKNOWN mapping (three literals accepted by `sp_LoadOrderListForOrderCreationbyTerri:1362`). See README §status.

## 22. Error Handling
Single try/catch in `SaveAllData` showing `"Error: " + ex.Message` (leaks internals). Detail-line SP failures inside `SaveOrderDetail` propagate as exceptions only if the SP throws (each call is independent → header may exist without lines). `showMessageBox` escapes `'` incorrectly (`message.Replace("'", "\'")` is a no-op in C#) so messages containing an apostrophe break the alert script.

## 23. Audit
`tblOrder.EntryDate` and `ServerDateTime` = GETDATE() on insert (SP). No creating user is stored (no `EntryBy`/`CreatedBy` column in the INSERT); `OrderSenderType='Web Manual'` only marks the channel. Updates store no modifier/time. No log table.

## 24. Integration
None from this page. Orders created here later feed the proforma/delivery flow (WF-M06-02) and SAP/other integrations are UNKNOWN.

## 25. Reports
None (Preview modal on OrderListView only).

## 26. Dependencies
Tables/views: tblOrder, tblOrderDetail, View_CustomerMaster, tblCustMaster, tblEmpGeneralInfo, tblUser, tblUserCompanyUnit, tblCompanyUnit, tblProduct/tblUnitPrice (lookup), tblMarket…tbl_Group. JS: jQuery, select2, pickadate, Bootstrap; AjaxControlToolkit AutoComplete.

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-01 | Positive | New order: customer with one tagged MIO + one mapped DC, 2 lines qty 3 TP 100 VAT 5 | MIO & DC preselected; saved Gross=600, VAT=30, Net=630; ActionStatus '2', IsManual 1, appears in PAGE-217 list | 006,007,009 |
| TC-02 | Negative | Submit with no sales person | alert "Please Select Sales Person (MIO)!", no DB write | 002 |
| TC-03 | Negative | Row with qty 0 | "Please enter valid Req. Qty in row #1" | 005 |
| TC-04 | Negative | Edit an invoiced order (`?OrderId=`) | warning + submit disabled "Order Locked (Invoiced)" | 001 |
| TC-05 | Edge | Edit order: remove one row, re-save | old details deleted, remaining lines re-inserted, header ActionStatus='2' | 011 |
| TC-06 | Edge | Customer with 2 tagged MIOs and no SalesPersonId in tags | sales person blank, all DCs listed | 007 |
| TC-07 | Edge | Overwrite TP to 1 | saved TradePrice=1 (no guard) | 012 |
| TC-08 | Edge | Create 10th manual order after sequence proposals ≥10 | verify stored OrderCode (BR-008) — expect page proposal stored unless SP branch triggered; also verify `OrderManualId` still executes when an `ODR-` code exists | 008 |
| TC-09 | Negative | Tamper `tpTextBox`/qty via HTTP (bypass JS) with negative price | accepted (no server bound) | 012 |

## 28. Reverse Engineering References
`Solution.Web/SInventory_UI/ManualOrderCreation.aspx(.cs)`, `OrderListBLL.cs`, `OrderListDAL.cs`, `OrderInfoDAL.cs:602`, `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:939-1214,1375+`, `Create_Order_ODR-20260901134.sql`.

## 29. Spec vs Actual
| Area | Specification | Actual code | Actual DB | Status | Ref |
|---|---|---|---|---|---|
| Prior spec | None existed | ManualOrderCreation.aspx(.cs) | n/a | MISSING | — |
| Order numbering | None existed | two generators that disagree (BR-008) | SP :999 | MISMATCH-INTERNAL | GAP-M06-001 |
| Approval bypass | None existed | manual orders auto-'2' | SP :1058-1101 | DOCUMENTED | BR-009 |
| MIOId persistence | None existed | passes @MIOId | not stored | DEFECT (parameter ignored) | GAP-M06-002 |

## 30. Known Gaps
- Order-code generators inconsistent: `LIKE 'ORD-000000%'` matches only sequence 1–9; `OrderManualId` takes MAX of a *string* over substring(5,15) of both `ORD-` and `ODR-yyyymmddN` codes and adds 1 in int arithmetic (the 11-digit `ODR-` suffix exceeds `int` range → INFERRED conversion error when the create page loads). GAP-M06-001.
- `@MIOId` ignored by both SPs; MIO stored only as code/name text. GAP-M06-002.
- No transaction across header + detail (partial saves possible). GAP-M06-003.
- No creator/modifier audit; no price/qty/stock/credit validation; free-text overwrite of TP. GAP-M06-004/012.
- Sales-person dropdown lists every employee; no role filter. GAP-M06-013.
- Hidden gift/campaign columns + inverted computed campaign flag on edit. BR-013.

## 31. Change Impact
`sp_Save_ManualOrderMaster` / `sp_Update_ManualOrderMaster` / `sp_Save_ManualOrderDetail` are used by this page only (grep of `Library.DAL`: `OrderListDAL` only). `OrderInfoDAL.LoadOrderWithDetail(string)` is shared with PAGE-218 (invoice), PAGE-226 (preview), DelivaryInvoice* pages, OrderStatus; changing its columns (esp. the computed `IsCampaignProduct`) affects invoice-creation logic. `tblOrder` columns written here (`ActionStatus`, `IsPrepareforInvoice`, `IsManual`) drive `sp_LoadOrderListForOrderCreationbyTerri` (PAGE-217) and `OrderInfoDAL.LoadDisRouteforInvoice`.
