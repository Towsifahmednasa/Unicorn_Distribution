---
page_id: PAGE-298
module: M08
page: SndTimesSalesReturnPartial
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-298 — SndTimesSalesReturnPartial (Second-time / post-payment partial return)

> **Evidence level:** Tier A — read `SndTimesSalesReturnPartial.aspx.cs` (lines 1-1345 and 1346-2037: entry, validation, save, calculation), the grid markup (`.aspx:274-470,610-650`), `SalesReturnBLL/SalesReturnDAL` (`PaymentUpdateInvoice`, `InsertInvoiceDetailReturn`, `UpdateDCStock`), `OrderInfoDALSalesReturn.LoadInvoicePartialPayment`, and the SP body `sp_UpdateAndInsertInvoiceDetailSalesReturn` (`SP_SalesReturn_Related.sql:173-228`). `sp_GET_DelivaryInvoiceNoCheckById` body is **not** in the repo. UI coverage PARTIAL: the discount/campaign sections of the aspx (lines 486-610, 660-980) were not read in detail (they feed the hidden campaign totals).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-298 |
| Module | M08 |
| Name / class | SndTimesSalesReturnPartial / `SInventory_UI_SndTimesSalesReturnPartial` (`.aspx.cs:15`) |
| Kind | Entry / Transaction, reached only from `SalesReturnList_new.aspx` (Session["InvoiceId"]) |
| Size | aspx 982 lines, code-behind 2,037 lines |
| Legacy | `LoadAllDataByOrder`, `InitialGrid`, `AddFunc`, `addImageButton_Click`, `SavePayment`, `ChkProductOffer*` are dead/unused (no aspx binding) |

## 2. Purpose
CONFIRMED (class/labels/messages): after a delivery invoice has been **payment-confirmed** (`PaymentInvoiceNo` set) the depot can still record that part of the delivered goods were finally **not accepted/paid** ("second-time return"). The user compares, per line, the quantity accepted (DQty) with the quantity invoiced (TQty) and the previously confirmed payment quantity, chooses a return reason, presses **Calculate**, then **Generate Invoice**.

## 3. Business Objective
INFERRED: correct the receivable and stock when a customer accepts only part of a delivered order at the payment stage; the returned quantity goes back to the batch row in the DC ledger and the invoice header/detail carry the "sndReturn*" figures used by the Delivery Return / rejection reports.

## 4. Navigation
Inbound: `SalesReturnList_new.aspx` → `gotoinvoiceButton_Click` sets `Session["InvoiceId"]` and redirects (`SalesReturnList_new.aspx.cs:349-350`). Outbound: `SalesReturnList_new.aspx` (back), `../SInventory_RPTVIEW/DelivaryInvoiceReturnViewer.aspx?InvNo=` (print).

## 5. Route
`~/SInventory_UI/SndTimesSalesReturnPartial.aspx`.

## 6. Source Files
| Type | Path |
|---|---|
| View / code-behind | Solution.Web/SInventory_UI/SndTimesSalesReturnPartial.aspx(.cs) |
| BLL | Library.BLL/SInventory_BLL/SalesReturnBLL.cs; OrderInfoBLLSalesReturn.cs; OrderInfoBLL.cs; RequisitionBLL |
| DAL | SalesReturnDAL.cs (`PaymentUpdateInvoice` :683-698, `InsertInvoiceDetailReturn` :774-794, `UpdateDCStock` :1504-1510, `ReturnReason`); OrderInfoDALSalesReturn.cs:978-987; BonusCampaignNewDAL.cs:1052-1072 |
| SQL | SP_SalesReturn_Related.sql:173-228 |

## 7. UI Control Inventory
| Control | Type | Required | Default / state | Notes |
|---|---|---|---|---|
| custCodeTextBox, custNameTextBox (AutoComplete `GetCustomer`), custAddressTextBox, comUnitNameTextBox, miaCodeTextBox, miaNameTextBox, districtNameTextBox, areaNameTextBox, marketNameTextBox, custCategoryTextBox | TextBox | no | filled by `GetCustInfo` (:87-115) | header info of the invoice's customer (`CustomerMaster(orderNoTextBox)` — looks up by *order no* text, `:93`) |
| orderNoTextBox, orderDateTextBox, invDateTextBox (today), deliverypersonNameTextBox, deliverypersonMobileTextBox, remarksTextBox | TextBox | orderNo, orderDate required | filled by `LoadInvoice` (:132-161) | `Validation` :49-58 |
| invoiceNoHiddenField, invoiceHiddenField, orderIdHiddenField, hdCustomerMasterId, hdComUnitId, hdMiaId | HiddenField | — | InvoiceNo / InvoiceId / OrderId … | |
| payTypeDDL | DropDownList | no | index 1 then reloaded (`PaymentTypeLoadBLL`) | not used in save |
| gridLineItemGridView | GridView `DataKeyNames=InvoiceDetailId,DCStoreId,Campaign` | rows | bound to `LoadInvoicePartialPayment` | cols: Code, Product Name (multiline, AutoComplete `GetProduct2`), CStock, Unit Price, Unit VAT, Qty, TP, DP%, DAmt, SD, VAT, Net, BQty, **TQty** (readonly = invoiced total qty), **DQty** (editable, default = `PaymentQuantity`; AutoPostBack), **Status** badge (Full/Partial/Reject), **Reason** dropdown (`tblReturnReason`, disabled until Partial/Reject) |
| hfDelTotalQty / hfCampaignType / hfCampaignName | HiddenField in code cell | — | `DelTotalQty` = `tblInvoiceDetail.PaymentQuantity` | previous (payment) quantity |
| lnkCalculate | LinkButton "Calculate" | must be pressed | — | sets `ViewState["IsCalculated"]=true` (:1996-2002) |
| tpTptalTextBox, disTotalTextBox, vatTotalTextBox, grandTotalTextBox, pdTextBox, campaignTPTextBox1 | TextBox | — | totals | `TotalValueCalculation` :1346-1428 |
| submitButton "Generate Invoice" | LinkButton, `OnClientClick=sweetAlertConfirm_Submit` | — | — | `saveButton_Click` |
| invTextBox + LinkButton1 "Print" | TextBox / LinkButton | — | "RTN-"+InvoiceNo | print |

## 8. Field Mapping
| UI | Variable | Handler | BLL | DAL | Param | Table.column |
|---|---|---|---|---|---|---|
| grid DataKey InvoiceDetailId | `InvoiceDetail.InvoiceDetailId` | `SaveInvoiceDetail` :927 | `SalesReturnBLL.PaymentUpdateInvoiceDetail` :584-587 | `InsertInvoiceDetailReturn` :774-794 | `@InvoiceDetailId` | tblInvoiceDetailReturn.InvoiceDetailId |
| invoiceHiddenField | `@InvoiceId` | :964,1063 | same | same | `@InvoiceId` | tblInvoiceDetailReturn.InvoiceId |
| dQtyTextBox | `Quantity`, `TotalQuantity` | :965-973 | same | same | `@Quantity`, `@TotalQuantity` | sndReturnQuantity, sndReturnTotalQuantity |
| hfDelTotalQty | `PreviousQuantity` | :1050-1056 | same | same | `@PreviousQuantity` | PreviousQuantity |
| (0) | `BonusQuantity=0` | :959-960 | | | `@BonusQuantity` | sndReturnBonusQuantity |
| tpTextBox / tpVatTextBox | `TotalPrice`, `TotalPriceVatAmount` | :974-979 | | | `@TotalPrice`, `@TotalPriceVatAmount` | sndReturnTotalPrice, sndReturnTotalPriceVatAmount |
| dpTextBox / dpAmtTextBox | `DiscountPercentage`, `DiscountAmount` | :954-958 | | | `@DiscountPercentage`, `@DiscountAmount` | sndReturnDiscountPercentage, sndReturnDiscountAmount |
| computed | `NetAmount = TP − discount + VAT` | :985-986 | | | `@NetAmount` | sndReturnNetAmount |
| statusLabel | `DeliveryStatus` | :987-988 | | | `@DeliveryStatus` | sndReturnStatus |
| reasonReturnDropDownList text | `ReturnReason` | :938-949 | | | `@ReturnReason` | sndReturnReason |
| header totals | `Invoice.TpTotal/TpDiscount/TpVat/TpGrandTotal`, `DeliveryInvoiceStatus`, `UpdateBy=LoginName`, `InvoiceDate=invDate` | `SaveInvoice` :1201-1275 | `PaymentUpdateInvoice` :572-578 | `SalesReturnDAL.PaymentUpdateInvoice` :683-698 (inline UPDATE) | `@TpTotal…@InvoiceStatus,@SndReturnInvoiceNo,@PaymentBy,@PaymentDate` | tblInvoice.sndReturnTpTotal, sndReturnTpDiscount, sndReturnTpVat, sndReturnTpGrandTotal, sndReturnInvoiceStatus, SndReturnInvoiceNo=`'SRN-'+InvoiceNo`, SndReturnPaymentBy, SndReturnPaymentDate |
| hfDelTotalQty − dQty | stock qty | `SaveInvoiceDetail` :1037-1048 | `SalesReturnBLL.UpdateDCStock` :117 | `SalesReturnDAL.UpdateDCStock` :1504-1510 | `@StockQty`,`@DCStoreId` | tblDCStore.StockQty (+=) |

## 9. Dropdown Mapping
`reasonReturnDropDownList` ← `SalesReturnBLL.ReturnReason` → `SELECT * FROM tblReturnReason` (Text `ReaturnReason`, Value `ReturndReasonId`) `SalesReturnDAL.cs:802-806`. `payTypeDDL` ← `tblPaymentType` (unused).

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| Calculate | `lnkCalculate_Click` :1996 | `CalculateQty()` clamps DQty; `DqtyCalculation()` sets status/reason state, recalculates totals; `IsCalculated=true` |
| DQty edit | `dQtyTextBox_TextChanged` :1958 | clears `IsCalculated`; shows alert if DQty ≥ previous qty and resets to previous qty |
| Generate Invoice | `saveButton_Click` :878 | validate → duplicate check → `SaveInvoice` → `SaveInvoiceDetail` → clear |
| Print | `printButton_Click` :1431 | opens `DelivaryInvoiceReturnViewer.aspx?InvNo=<invTextBox>` |
| Back | `backLinkButton_Click` :128 | `SalesReturnList_new.aspx` |

## 11. Frontend Flow
Page_Load (:20-35): if `Session["InvoiceId"]` exists → `LoadInvoice(id)`, clear the session key; default payment type; date=today. `LoadInvoice` binds `OrderInfoDALSalesReturn.LoadInvoicePartialPayment` (invoice ⟕ detail ⟕ orderDetail ⟕ product ⟕ customer; `DelTotalQty = PaymentQuantity`; `Quantity/TQty = TotalQuantity`), loads reasons per row, runs `DqtyCalculation`. User edits DQty → must click Calculate → Generate Invoice (SweetAlert confirm). No JS calculation.

## 12. Backend Flow
`saveButton_Click` (:878-916, try/catch → alert "Some Networks Error blocked Your Transaction Please Contract Administrator!!"):
1. `Validation()` (§18).
2. `BonusCampaignNewDAL.GetDelivaryInvoiceNoCheckById(invoiceId, "SndReturnInvoiceNo")` → `sp_GET_DelivaryInvoiceNoCheckById(@id,@InvStatus)`; if rows exist → alert "Already Exist!" and clear.
3. `SaveInvoice()` (:1201-1275): header totals from the page totals; status: all rows "Full" ⇒ Full, otherwise Partial, all rows "Reject" ⇒ Reject; `PaymentUpdateInvoice` = inline UPDATE of `tblInvoice.sndReturn*` and `SndReturnInvoiceNo='SRN-'+InvoiceNo` (`invoiceNoHiddenField` is the *invoice* no, despite the `DelivaryInvoiceNo` property name).
4. `SaveInvoiceDetail()` (:917-1071): for each row: build detail; if the original DC row exists (`DCInfoWithDCId(DCStoreId)`) **and status is Partial/Reject** → `UpdateDCStock(hfDelTotalQty − dQty, DCStoreId)` (`tblDCStore.StockQty = StockQty + qty` on the original batch row); then `PaymentUpdateInvoiceDetail` → `sp_UpdateAndInsertInvoiceDetailSalesReturn` inserts `tblInvoiceDetailReturn` (returns `@InvoiceDetailReturnId`).
5. `ViewState["IsCalculated"]=null; Clear();` success `ShowSuccesalert('Generate Invoice Created Successsfully!')`.
No transaction; header update is committed before details.

## 13. API Mapping
None (autocomplete asmx `GetCustomer`, `GetProduct2` only).

## 14. Service Mapping
`SalesReturnBLL.PaymentUpdateInvoice`, `PaymentUpdateInvoiceDetail`, `UpdateDCStock`, `DCInfoWithDCId`, `ReturnReason`, `ProductInfo`, `ProductDiscount`; `OrderInfoBLLSalesReturn.LoadInvoicePartialPayment`; `OrderInfoBLL.LoadOffer/LoadCampaign/LoadInvoice/GetFixedCustomerfromInvoiceTable/GetTradeTerm*` (used by `DqtyCalculation` for campaign discount recalculation).

## 15. Repository Mapping
| DAL | SQL |
|---|---|
| `SalesReturnDAL.PaymentUpdateInvoice` | `UPDATE tblInvoice SET sndReturnTpTotal…SndReturnPaymentDate WHERE InvoiceId=@InvoiceId` (parameterised) |
| `SalesReturnDAL.InsertInvoiceDetailReturn` | `sp_UpdateAndInsertInvoiceDetailSalesReturn` via `ClsCommonInternalDAL.SaveAction` |
| `SalesReturnDAL.UpdateDCStock` | `UPDATE tblDCStore SET StockQty=StockQty+@StockQty WHERE DCStoreId=@DCStoreId` |
| `OrderInfoDALSalesReturn.LoadInvoicePartialPayment` | `SELECT … FROM tblInvoice ⟕ tblInvoiceDetail ⟕ tblOrderDetail ⟕ tblProduct ⟕ tblCustMaster WHERE tblInvoice.InvoiceId=@InvoiceId` |

## 16. Stored Procedure Mapping
| SP | Body | Effect |
|---|---|---|
| `sp_UpdateAndInsertInvoiceDetailSalesReturn` | in repo `SP_SalesReturn_Related.sql:174-228` | `INSERT tblInvoiceDetailReturn(InvoiceId, InvoiceDetailId, PreviousQuantity, sndReturnQuantity, sndReturnBonusQuantity, sndReturnTotalQuantity, sndReturnTotalPrice, sndReturnTotalPriceVatAmount, sndReturnDiscountPercentage, sndReturnDiscountAmount, sndReturnNetAmount, sndReturnStatus, sndReturnReason)`; `@InvoiceDetailReturnId OUTPUT = SCOPE_IDENTITY()` |
| `sp_GET_DelivaryInvoiceNoCheckById` | **not in repo** | duplicate guard — UNKNOWN internals |

## 17. Database Mapping
- tblInvoice (W: sndReturn* columns, SndReturnInvoiceNo, SndReturnPaymentBy/Date) — DDL UNKNOWN.
- tblInvoiceDetailReturn (W, columns above; identity `InvoiceDetailReturnId`).
- **tblDCStore (W stock effect):** `StockQty += (PaymentQuantity − DQty)` on the **original batch (Receive) row** referenced by `tblInvoiceDetail.DCStoreId`; **no** new ledger row.
- R: tblInvoiceDetail (PaymentQuantity, TotalQuantity, DCStoreId, UnitPrice, UnitVatAmount…), tblOrderDetail, tblProduct, tblCustMaster, tblReturnReason.

## 18. Validation Matrix
| Check | Where | Message |
|---|---|---|
| Calculate pressed since last DQty change | `Validation` :44-48 | Please click Calculate before saving. |
| Order no non-empty | :49-53 | Please Input Order Number! |
| Order date non-empty | :54-58 | Please Input Order Date!! |
| Code, TQty and DQty non-empty in every row | :60-68 | Please Input Valid Data!!! |
| Reason chosen when status = "Reject" **or "Parial"** (typo → Partial lines are never checked) | :69-83 | Please Select Reason! |
| DQty ≥ previous payment qty forced back to previous qty | `dQtyTextBox_TextChanged` :1986-1990 / `CalculateQty` :2031-2035 | Cannot be greater then Payment Quantity! |
| DQty > TQty in calc | `DqtyCalculation` :1515-1524, :1951-1954 | Cannot be greater then Total Quantity |
| Already processed | `saveButton_Click` :887-905 | Already Exist! |

## 19. Business Rules
| BR | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M08-019 | The flow is only for invoices already **payment-confirmed** (`PaymentInvoiceNo` not null) and **not yet second-returned** (`SndReturnInvoiceNo` null) — enforced by the list query, plus a server duplicate guard on `SndReturnInvoiceNo`. | SalesReturnList_new.aspx.cs:447-456; SndTimesSalesReturnPartial.aspx.cs:887-905 | list page + SP | CONFIRMED (SP body UNKNOWN) |
| BR-M08-020 | The user must press **Calculate** after any DQty change and before saving. | .aspx.cs:44-48,1961,2001 | code-behind | CONFIRMED |
| BR-M08-021 | DQty (accepted quantity) defaults to the previously paid quantity and cannot exceed it (values ≥ previous are reset to previous). | .aspx.cs:1986-1990,2031-2035; aspx:450 | code-behind | CONFIRMED |
| BR-M08-022 | Line status: DQty = TQty ⇒ **Full**; 0 < DQty < TQty ⇒ **Partial**; DQty = 0 (or DQty > TQty, reset to 0) ⇒ **Reject**. Reason dropdown enabled for Partial/Reject. | .aspx.cs:1501-1530 | code-behind | CONFIRMED |
| BR-M08-023 | A reason is mandatory only for lines whose status literal equals "Reject" or the misspelt "Parial" — Partial lines effectively skip the check. | .aspx.cs:71-72 | code-behind | CONFIRMED (typo) |
| BR-M08-024 | Header status: every line Full ⇒ Full; every line Reject ⇒ Reject; otherwise Partial. Stored in `tblInvoice.sndReturnInvoiceStatus`; `SndReturnInvoiceNo='SRN-'+InvoiceNo`. | .aspx.cs:1240-1267; SalesReturnDAL.cs:683-698 | code-behind + inline SQL | CONFIRMED |
| BR-M08-025 | **Stock effect:** for Partial/Reject lines add `(PaymentQuantity − DQty)` back to the original `tblDCStore` batch row (`StockQty = StockQty + qty`); Full lines: none. Bonus quantity is forced to 0. | .aspx.cs:1037-1048; SalesReturnDAL.cs:1504-1510 | code-behind + inline SQL | CONFIRMED |
| BR-M08-026 | Line snapshot stored in `tblInvoiceDetailReturn` with PreviousQuantity = payment qty and `sndReturnQuantity/TotalQuantity = DQty` (i.e. the *retained* quantity, not the returned quantity). | .aspx.cs:965-973,1050-1063; SP_SalesReturn_Related.sql:191-222 | code-behind + SP | CONFIRMED (semantics INFERRED) |
| BR-M08-027 | Line amounts recomputed on DQty: TP = unit price × DQty; VAT = unit VAT × DQty; Net = TP − discount + VAT; lines with zero TP get VAT/discount/net = 0; campaign/trade-term discount % recomputed from totals (old-trade-policy vs new, fixed customers exempt). | .aspx.cs:1346-1420,1541-1949 | code-behind | CONFIRMED (campaign detail INFERRED) |
| BR-M08-028 | This page posts **no accounting entry**; whether posting occurs downstream from `tblInvoice.sndReturn*` is UNKNOWN. | whole page | — | CONFIRMED absence in page; downstream UNKNOWN |
| BR-M08-029 | Print number shown is `"RTN-"+InvoiceNo` while the stored return number is `"SRN-"+InvoiceNo`. | .aspx.cs:1238; SalesReturnDAL.cs:694 | code-behind | CONFIRMED |
| BR-M08-030 | Product duplicates within the grid are rejected by `ProductCodeValidation` (returns false → "No: Product Already Inserted!!!"), only for manual product edits. | .aspx.cs:746-760,737-742 | code-behind | CONFIRMED |

## 20. Permission Matrix
No role or session check in the page besides `Session["UserId"]`/`Session["LoginName"]` used at save (`.aspx.cs:1230-1232`; NullReference if session lost). Menu-based access only. No DC/territory scoping beyond the list page's filters.

## 21. Status Lifecycle
Invoice-level: `sndReturnInvoiceStatus` ∈ {Full, Partial, Reject} set here (NULL before). Line-level `sndReturnStatus` ∈ {Full, Partial, Reject}. Gate: `SndReturnInvoiceNo IS NULL` (list) — after save the invoice disappears from `SalesReturnList_new`. No reverse transition/cancel found in this module.

## 22. Error Handling
try/catch → generic alert; `Convert.ToDecimal` on empty strings in `TotalValueCalculation` and `SaveInvoiceDetail` throws → generic alert. SP failures propagate. No logging.

## 23. Audit
`SndReturnPaymentBy` = LoginName, `SndReturnPaymentDate` = **invoice date field** (`InvoiceDate = invDateTextBox` today). No timestamp for detail rows; `tblInvoiceDetailReturn` has no user/date columns in the SP.

## 24. Integration
None (no SAP/accounts call here).

## 25. Reports
Print → `DelivaryInvoiceReturnViewer.aspx` → `rptInvoiceForCustomerDelivary.rpt` (RPT-M08-04). The print key does not match the stored numbers (BR-M08-029, GAP-M08-012).

## 26. Dependencies
`SalesReturnList_new.aspx`, `SInventoryWebService.asmx` (`GetCustomer`, `GetProduct2`), tables in §17, sweetalert JS (`sweetAlertConfirm_Submit`, `ShowSuccesalert`, `faildalert` — defined in master).

## 27. Test Cases
| # | Type | Steps | Expected | BR |
|---|---|---|---|---|
| T1 | Positive | Open from list, set DQty < paid qty on one line, choose reason, Calculate, Generate | header status Partial; `tblInvoiceDetailReturn` rows; DCStore row StockQty += diff; list no longer shows invoice | 020-026 |
| T2 | Negative | Generate without Calculate | "Please click Calculate before saving." | 020 |
| T3 | Negative | DQty=0 on a line without reason | "Please Select Reason!" | 022,023 |
| T4 | Edge | Partial line, reason blank | passes validation (typo) | 023 |
| T5 | Edge | DQty > paid qty | reset to paid qty + alert | 021 |
| T6 | Negative | Re-submit same invoice (second tab) | "Already Exist!" | 019 |
| T7 | Edge | All lines DQty = TQty and paid qty = TQty | status Full, no stock change | 022,024,025 |
| T8 | Integration | Print after save | viewer searched with `RTN-<InvoiceNo>` — expect empty report | 029 |

## 28. Reverse Engineering References
`.aspx.cs:20-161,878-1275,1346-1428,1486-2037`; `SalesReturnDAL.cs:683-794,1504-1510`; `OrderInfoDALSalesReturn.cs:978-987`; `SP_SalesReturn_Related.sql:173-228`; `SalesReturnList_new.aspx.cs:336-353,430-466`.

## 29. Spec vs Actual
| Item | Specification | Actual |
|---|---|---|
| All | None existed | as documented |

## 30. Known Gaps
Reason check typo ("Parial"); `dQtyTextBox_TextChanged` alerts even when equal; stock restored by editing the original Receive row (`StockQty +=`) whereas other flows use ledger 'Issue' rows — can push Receive `StockQty > TotalQuantity`, the exact condition `sp_Check_DCStore_StockAnomalies` flags and `sp_Adjust_DCStore_Stock_Only` "repairs" (GAP-M08-006); no posting; header update before details (no transaction); `sp_UpdateAndInsertInvoiceDetailSalesReturn` has no idempotency; 1,300 lines of duplicated campaign-calculation code with hard-coded product codes (`ARD01`, `ARD02`, `FGD02`); `DqtyCalculation` runs `TotalValueCalculation` inside a per-row loop (O(n²) postbacks).

## 31. Change Impact
`SalesReturnDAL.UpdateDCStock`, `PaymentUpdateInvoice*`, `InsertInvoiceDetailReturn` are also used by `SalesReturnList`/other second-time pages in the same DAL (`SalesReturnDAL` is shared with `PaymentPartial`-style pages — not enumerated). Changing `tblInvoice.sndReturn*` semantics affects `DeliveryReturnReport*`/rejection reports (RPT-M08-02). `sp_UpdateAndInsertInvoiceDetailSalesReturn` is called only from `SalesReturnDAL.InsertInvoiceDetailReturn` (grep).
