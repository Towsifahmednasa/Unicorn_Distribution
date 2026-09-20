---
page_id: PAGE-334
module: M09
page: PaymentPartial
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-334 — PaymentPartial (Payment-stage delivery confirmation / return entry)

> **Evidence level:** Tier A — `.aspx.cs` lines 1-360, 861-1310, 1342-1470, 1490-2008, `InvoiceDAL.PaymentUpdateInvoice / PaymentUpdateInvoiceDetail / UpdateDCStock`, `InvoiceBLL`, `OrderInfoDAL.LoadInvoicePartialPayment` read; markup control list checked (987 lines, not every style block read). `sp_GET_DelivaryInvoiceNoCheckById` body is not in the repo (UNKNOWN). **Despite the name, this page does NOT record customer money.** It records, at the "payment/return confirmation" stage, how many units of each delivered invoice line the customer actually accepted (full / partial / reject), stamps the invoice with a `PaymentInvoiceNo` ("RTN-…"), returns rejected quantity to depot stock, and thereby makes the invoice appear in the collection list of PAGE-311.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-334 |
| Module | M09 (placed in Collections because the "Payment*" columns it fills feed collection) |
| Page name | PaymentPartial |
| Kind | Entry / Transaction — invoice payment-stage confirmation (CONFIRMED) |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_PaymentPartial |
| Code-behind LOC | 2008 |
| Legacy/duplicate | none found; contains large commented-out blocks and an unused `SavePayment()` (:1264-1307) |

## 2. Purpose
For one delivery invoice (id arriving in `Session["InvoiceId"]`), shows every invoice line with the sales-confirmed/delivered quantity and lets the user enter the quantity finally accepted (DQty). The page derives each line's status (Full / Partial / Reject), recomputes TP, discount, VAT and net amount for the accepted quantity, requires a return reason for short-accepted lines, and on Save stamps the payment-stage fields of `tblInvoice` / `tblInvoiceDetail` and puts un-accepted stock back. CONFIRMED (`:877-1263`).

## 3. Business Objective
Produce the final billable ("Confirm Amount") of an invoice after customer acceptance/returns so that collections (PAGE-311) and receivables use the corrected figure, while returned goods go back to the depot stock. INFERRED from `PaymentNetAmount` use in `sp_GET_PaymentInvSP` and the `UpdateDCStock` call.

## 4. Navigation
Inbound: `DelivaryInvoiceCreationAfterSalesConfirm.aspx`, `SalesReturnList_new.aspx` (they set `Session["InvoiceId"]`). Outbound: `Back` → `DelivaryInvoiceCreationAfterSalesConfirm.aspx` (:131); Print → popup `../SInventory_RPTVIEW/DelivaryInvoiceReturnViewer.aspx?InvNo=RTN-…` (:1437). Menu path UNKNOWN (DB-driven).

## 5. Route
`~/SInventory_UI/PaymentPartial.aspx`.

## 6. Source Files
| Type | Path |
|---|---|
| View / Code-behind | Solution.Web/SInventory_UI/PaymentPartial.aspx(.cs) |
| BLL | Library.BLL/SInventory_BLL/InvoiceBLL.cs (PaymentUpdateInvoice :699, PaymentUpdateInvoiceDetail :716, UpdateDCStock :128, DCInfoWithDCId :137, CustomerMaster :655, ReturnReason :672), OrderInfoBLL, CustPaymentBLL, RequisitionBLL |
| DAL | InvoiceDAL.cs (PaymentUpdateInvoice :793-809, PaymentUpdateInvoiceDetail :844-864, UpdateDCStock :1817-1825), OrderInfoDAL.LoadInvoicePartialPayment :1224, BonusCampaignNewDAL.GetDelivaryInvoiceNoCheckById :1052 |
| DAO | Invoice, InvoiceDetail, DCStockNew, CustPayment, CustPaymentDetail |

## 7. UI Control Inventory
| Control | Label | Required | Default / state | Notes |
|---|---|---|---|---|
| custCodeTextBox, custNameTextBox, custAddressTextBox, comUnitNameTextBox, miaCodeTextBox, miaNameTextBox, districtNameTextBox (FE name), areaNameTextBox (territory), marketNameTextBox, custCategoryTextBox | Customer/MIO/market header | – | filled from `InvoiceBLL.CustomerMaster(orderNo)` (:94-107) | display |
| orderNoTextBox | Order No | Yes (:43) | from invoice | message "Please Input Order Number!!" |
| orderDateTextBox | Order Date | Yes (:48) | from invoice `OrderDate` | "Please Input Order Date!!" |
| invDateTextBox | Invoice Date | – | today (:39) | becomes `PaymentDate` |
| deliverypersonNameTextBox / deliverypersonMobileTextBox / remarksTextBox | Delivery person / mobile / remarks | – | from invoice | display |
| gridLineItemGridView (DataKeys InvoiceDetailId, DCStoreId, Campaign) | Line items | ≥1 | bound from `LoadInvoicePartialPayment` | columns: Code, Product Name, UP, UVAT, Qty (RO), TP, DP, DAmt, SD, TPVAT, NP, BQty, TQty, DQty, Status, Reason |
| – dQtyTextBox (AutoPostBack) | DQty (accepted qty) | non-empty (:65) | delivered qty | drives status |
| – statusLabel | Status | – | Full / Partial / Reject | set by `DqtyCalculation` |
| – reasonReturnDropDownList | Reason | mandatory when status Reject or "Parial" (sic) | disabled until status needs it | source tblReturnReason |
| tpTptalTextBox, disTotalTextBox, pdTextBox (RO), vatTotalTextBox, grandTotalTextBox | TP / Discount / Special Discount / VAT / Grand Total | – | computed | |
| lnkCalculate | Calculate | must be clicked before save | – | sets `ViewState["IsCalculatedDelReturn"]` |
| submitButton / Button1 ("Invoice Generate") | Save | – | `confirm('Are you sure you want to Save Delivery Invoice ?')` | `saveButton_Click` |
| invTextBox, printButton | Print Inv No | – | "RTN-"+delivery invoice no after save | |
| hidden: invoiceHiddenField, invoiceNoHiddenField, orderIdHiddenField, hdCustomerMasterId, hdComUnitId, hdMiaId, payTypeDDL | | | | |

## 8. Field Mapping
| UI | DTO | DAL SQL | Column |
|---|---|---|---|
| tpTptalTextBox | Invoice.TpTotal | `PaymentUpdateInvoice` | tblInvoice.PaymentTpTotal |
| disTotalTextBox | Invoice.TpDiscount | | PaymentTpDiscount |
| vatTotalTextBox | Invoice.TpVat | | PaymentTpVat |
| grandTotalTextBox | Invoice.TpGrandTotal | | PaymentTpGrandTotal |
| computed status (Full/Partial/Reject) | Invoice.DeliveryInvoiceStatus | | PaymentInvoiceStatus |
| "RTN-" + invoiceNo | Invoice.DelivaryInvoiceNo | | PaymentInvoiceNo |
| Session LoginName | Invoice.UpdateBy | | PaymentBy |
| invDateTextBox | Invoice.InvoiceDate | | PaymentDate |
| row dQtyTextBox | InvoiceDetail.Quantity / TotalQuantity | `PaymentUpdateInvoiceDetail` | tblInvoiceDetail.PaymentQuantity / PaymentTotalQuantity |
| row tpTextBox | TotalPrice | | PaymentTotalPrice |
| row tpVatTextBox | TotalPriceVatAmount | | PaymentTotalPriceVatAmount |
| row dpTextBox / dpAmtTextBox | DiscountPercentage / DiscountAmount | | PaymentDiscountPercentage / PaymentDiscountAmount |
| (TotalPrice − DiscountAmount) + TotalPriceVatAmount | NetAmount | | PaymentNetAmount |
| row statusLabel | DeliveryStatus | | PaymentStatus |
| reason text | ReturnReason | | PaymentReturnReason |
| 0 constant | BonusQuantity | | PaymentBonusQuantity |
| hfDelTotalQty − dQty | stock delta | `UpdateDCStock(qty, DCStoreId)` | tblDCStore.StockQty += qty |

## 9. Dropdown Mapping
reasonReturnDropDownList: text/value from `SELECT * FROM tblReturnReason` (ReaturnReason / ReturndReasonId) per row (:157). payTypeDDL: tblPaymentType (hidden, unused in save; `PaymentTypeId` forced to 0 in the Invoice DTO, not persisted by `PaymentUpdateInvoice`).

## 10. Button Action Mapping
| Control | Handler | Effect |
|---|---|---|
| lnkCalculate | `lnkCalculate_Click` :1342 | `DqtyCalculation()` then flag calculated |
| dQtyTextBox | `dQtyTextBox_TextChanged` :1975 | if `DelTotalQty <= typed` → "Cannot be greater then Sales confirmation Quantity" and resets to DelTotalQty |
| Save | `saveButton_Click` :877 | §12 |
| Print | `printButton_Click` :1435 | opens DelivaryInvoiceReturnViewer |
| Back | `backLinkButton_Click` :129 | redirect |

## 11. Frontend Flow
Load: if `Session["InvoiceId"]` present → `LoadInvoice(id)` (then session key nulled) → grid bound, return-reason dropdown per row, `DqtyCalculation()`. User edits DQty per line (AutoPostBack), presses Calculate, then Save. Select2/pickadate initialisation via master. All calculations are server-side (no page-local JS beyond confirm dialog).

## 12. Backend Flow
`saveButton_Click` (:877-915): `Validation()` → `GetDelivaryInvoiceNoCheckById(invoiceId,"PaymentInvoiceNo")` (`sp_GET_DelivaryInvoiceNoCheckById`); if it returns **0 rows** → `SaveInvoice()` then `SaveInvoiceDetail()`, `Clear()`, success alert; else alert "Already Exist!" + `Clear()`. Any exception → "Some Networks Error blocked Your Transaction Please Contract Administrator!!".
- `SaveInvoice()` (:1189-1263): header status: all lines "Full" → "Full"; else "Partial"; all lines "Reject" → "Reject". `aInvoiceBll.PaymentUpdateInvoice(...)` UPDATE tblInvoice SET PaymentTp*, PaymentInvoiceStatus, PaymentInvoiceNo="RTN-"+DelivaryInvoiceNo, PaymentBy, PaymentDate. Returns 0 always (never assigned) — harmless since ids come from hidden fields.
- `SaveInvoiceDetail()` (:916-1059): per line builds the DTO (values from grid text boxes; NetAmount = TP − discount + VAT), loads DC store info (`DCInfoWithDCId`), for lines "Partial"/"Reject" calls `UpdateDCStock(hfDelTotalQty − DQty, DCStoreId)` (adds the *not-accepted* quantity back to `tblDCStore.StockQty`), then `PaymentUpdateInvoiceDetail` UPDATE tblInvoiceDetail.
- No transaction; each UPDATE is a separate command.
`DqtyCalculation()` (:1490-1973) per row: `dq==tq` → Full; `dq<tq && dq!=0` → Partial; `dq>tq` → dq reset to "0", Reject; else (0) → Reject and TPVAT=0. If `dq<=tq`: TP = UnitPrice × dq, then discount %/amount chosen (BR-M09-030), VAT = UnitVAT × dq (F2), NP = TP − DAmt + VAT.

## 13. API Mapping
None (postback only).

## 14. Service Mapping
InvoiceBLL, OrderInfoBLL, BonusCampaignNewDAL (DAL called directly from code-behind at :884-886), CustPaymentBLL (only in unused `SavePayment`).

## 15. Repository Mapping
See §6 DAL list. `UpdateDCStock`: `UPDATE tblDCStore SET StockQty=StockQty + @StockQty WHERE DCStoreId=@DCStoreId` (relative update). `LoadInvoicePartialPayment`: tblInvoice ⟕ tblInvoiceDetail ⟕ tblOrderDetail ⟕ tblProduct ⟕ tblCustMaster by InvoiceId; DelTotalQty = tblInvoiceDetail.DeliveryQuantity.

## 16. Stored Procedure Mapping
`sp_GET_DelivaryInvoiceNoCheckById(@id,@InvStatus)` — body NOT in repo (UNKNOWN); used as "already confirmed?" gate with @InvStatus='PaymentInvoiceNo'. Other data access is inline SQL.

## 17. Database Mapping
tblInvoice (UPDATE Payment* columns), tblInvoiceDetail (UPDATE Payment* columns), tblDCStore (UPDATE StockQty), reads: tblOrder / tblOrderDetail / tblProduct / tblCustMaster / tblReturnReason / View_CustomerMaster / tblDCStore / offer & trade-term tables (via OrderInfoBLL: LoadOffer, LoadCampaign, GetFixedCustomerfromInvoiceTable, GetTradeTerm, GetTradeTermOld, ProductDiscount, getEzeventCampaignCheck, getEsomiumCampaignCheck, getEsomiumCampaignCheckSeacoral — bodies not traced in detail here).

## 18. Validation Matrix
| Field | Rule | Backend | Message | BR |
|---|---|---|---|---|
| Order No | non-empty | :43 | "Please Input Order Number!!" | BR-M09-021 |
| Order Date | non-empty | :48 | "Please Input Order Date!!" | BR-M09-021 |
| Calculate | must be clicked (ViewState flag) | :56 | alert('Please click Calculate before saving.') | BR-M09-022 |
| Code / TQty / DQty | non-empty per row | :62-70 | "Please Input Valid Data!!!" | BR-M09-021 |
| Reason | required when status text is "Reject" or "Parial" (misspelt; real status is "Partial", so Partial lines are NOT forced to give a reason) | :71-84 | "Please Select Reason !!" | BR-M09-024 |
| DQty vs TQty | DQty > TQty resets to 0 / Reject; `dq<=tq` else message | :1519, :1968 | "Cannot be greater then Total Quantity" | BR-M09-025 |
| DQty vs delivered | `DelTotalQty <= typed` | :2001 | "Cannot be greater then Sales confirmation Quantity" | BR-M09-025 |
| Duplicate confirmation | `PaymentInvoiceNo` already stamped | :886-905 | "Already Exist!" | BR-M09-021 |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M09-021 | An invoice can be payment-confirmed only once: if `sp_GET_DelivaryInvoiceNoCheckById(invoiceId,'PaymentInvoiceNo')` returns any row the save is refused ("Already Exist!"). Order no/date and all line codes/quantities must be present. | PaymentPartial.aspx.cs:41-70, :886-905 | Validation, saveButton_Click | CONFIRMED (gate call) / UNKNOWN (SP semantics) |
| BR-M09-022 | The user must press Calculate before saving (ViewState flag `IsCalculatedDelReturn`); the flag is never reset after edits, so later DQty edits do not force recalculation. | :56-60, :1342-1348 | Validation | CONFIRMED |
| BR-M09-023 | Line status: DQty == TQty → Full; 0 < DQty < TQty → Partial; DQty = 0 → Reject; DQty > TQty → DQty forced to 0 and Reject. Header status: all Full → Full; all Reject → Reject; otherwise Partial. | :1505-1544, :1244-1255 | DqtyCalculation, SaveInvoice | CONFIRMED |
| BR-M09-024 | A return reason is required for lines whose status is "Reject" (or the misspelt "Parial"); Partial lines are effectively exempt. Reason stored in PaymentReturnReason (empty string if none). | :71-84, :937-948 | Validation, SaveInvoiceDetail | CONFIRMED |
| BR-M09-025 | Accepted quantity may not exceed the quantity being confirmed (TQty) or, per `dQtyTextBox_TextChanged`, the delivered quantity (`DelTotalQty <= typed` is rejected — note `<=`, so typing exactly the delivered quantity also triggers the message). | :1519-1521, :1968-1971, :2001-2005 | DqtyCalculation, dQtyTextBox_TextChanged | CONFIRMED (code) / INFERRED (comparison is a bug) |
| BR-M09-026 | Not-accepted quantity returns to depot stock: for Partial/Reject lines `tblDCStore.StockQty += (DelTotalQty − DQty)`. | :1036-1047; InvoiceDAL.cs:1817 | SaveInvoiceDetail | CONFIRMED |
| BR-M09-027 | Amounts per line for accepted quantity: TP = UnitPrice × DQty; VAT = UnitVAT × DQty (F2); discount amount = TP × pct/100 (F2); NP = TP − discount + VAT; saved PaymentNetAmount = (TP − DiscountAmount) + VAT; bonus qty fixed 0; special amount fixed 0. Header totals are sums of lines with TP>0 (`TotalValueCalculation` :1350). | :1922-1951, :984-985 | DqtyCalculation, TotalValueCalculation | CONFIRMED |
| BR-M09-028 | Stamps the invoice: PaymentInvoiceNo = "RTN-" + delivery invoice no; PaymentDate = invoice-date box (today); PaymentBy = login. The presence of PaymentInvoiceNo is what makes the invoice eligible for collection (BR-M09-001). | InvoiceDAL.cs:793-809 | PaymentUpdateInvoice | CONFIRMED |
| BR-M09-029 | Zero-TP lines have VAT, discount amount and NP forced to 0 in totals. | :1417-1423 | TotalValueCalculation | CONFIRMED |
| BR-M09-030 | Discount % decision (when not a fixed customer): if `OldTradePolicy` → `GetTradeTermOld(TP total)`; else campaign match by invoice-detail (Ezevent / Esomium / Seacoral) → campaign DiscountPercentage (rounded 2dp); else `GetTradeTerm(TP total, CustTypeId, PaymentType, SubmissionDate)`; fixed customer → 0. Line discount % not applied when the SD box = "1" or bonus flag "True". | :1612-1860 | DqtyCalculation | CONFIRMED (flow) / UNKNOWN (lookup table contents) |

## 20. Permission Matrix
Session: InvoiceId (input), UserId, LoginName. Master redirect on missing UserId. No role check; menu-based. Any logged-in user with the URL and a session `InvoiceId` can post.

## 21. Status Lifecycle
tblInvoice.PaymentInvoiceStatus ∈ {Full, Partial, Reject} set here together with PaymentInvoiceNo; tblInvoiceDetail.PaymentStatus per line same domain. Only transition performed: (no PaymentInvoiceNo) → (stamped); re-entry blocked by BR-M09-021. Downstream: PAGE-311 collects only stamped invoices with Due>0; PAGE-335 reset does not clear these fields.

## 22. Error Handling
`saveButton_Click` try/catch shows generic message (exception swallowed, no log). Other handlers (parse of decimals from text boxes) have no guards → FormatException on blank/invalid input (`Convert.ToDecimal`).

## 23. Audit
PaymentBy / PaymentDate on tblInvoice. No per-line audit, no stock-movement record other than the StockQty update.

## 24. Integration
None.

## 25. Reports
Print → `DelivaryInvoiceReturnViewer.aspx?InvNo=RTN-…` (Crystal viewer, not analysed here).

## 26. Dependencies
InvoiceBLL/DAL, OrderInfoBLL/DAL, BonusCampaignNewDAL, tblDCStore.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-334-01 | Save without pressing Calculate | Alert, no write | BR-M09-022 |
| TC-334-02 | All lines DQty = TQty | header Full, PaymentInvoiceNo=RTN-…, invoice appears in PAGE-311 list | BR-M09-023, 028 |
| TC-334-03 | One line DQty=TQty−2, reason chosen | line Partial; stock += 2 | BR-M09-023, 026 |
| TC-334-04 | Partial line with no reason | currently saves (misspelt check) — record as defect | BR-M09-024 |
| TC-334-05 | Reject line without reason | "Please Select Reason !!" | BR-M09-024 |
| TC-334-06 | DQty > TQty | reset to 0, Reject | BR-M09-025 |
| TC-334-07 | Save same invoice twice | second: "Already Exist!" | BR-M09-021 |
| TC-334-08 | Change DQty after Calculate and save | totals may be stale | BR-M09-022 |
| TC-334-09 | Verify PaymentNetAmount = (TP − disc) + VAT | equality | BR-M09-027 |

## 28. Reverse Engineering References
`Solution.Web/SInventory_UI/PaymentPartial.aspx(.cs)`, `Library.DAL/SInventory_DAL/InvoiceDAL.cs:793-864,1817`, `Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1224`, `Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs:1052`.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Name vs behaviour | – | "PaymentPartial" is invoice payment-stage confirmation/return, not cash collection | – | NO SPEC | §1 |

## 30. Known Gaps
- Misleading name; unused `SavePayment()` would record a whole-invoice "Cash" payment and set PaymentAmount/"Full" — dead code (GAP-M09-010).
- Reason validation compares to "Parial" (typo) (GAP-M09-011); `<=` comparison in dQty handler; Calculate flag never reset.
- Non-transactional multi-table update; stock add-back runs before/independent of header update; `SaveInvoice` returns 0.
- Hard-coded product codes (ARD01, ARD02, FGD02) and campaign logic embedded in code-behind.
- Catch-all message hides root cause; `DAL` accessed directly from the page.
- `sp_GET_DelivaryInvoiceNoCheckById` body missing (GAP-M09-001).

## 31. Change Impact
`InvoiceDAL.PaymentUpdateInvoice/PaymentUpdateInvoiceDetail` populate the Payment* columns used by `sp_GET_PaymentInvSP*`, `sp_Sales_CustomerPaymentPosting` (COGS test uses Delivery* not Payment*), aging and collection reports. Changing the write formula (BR-M09-027) alters receivables everywhere. Referenced from DelivaryInvoiceCreationAfterSalesConfirm and SalesReturnList_new.
