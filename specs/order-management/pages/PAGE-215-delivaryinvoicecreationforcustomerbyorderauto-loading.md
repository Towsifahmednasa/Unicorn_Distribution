---
page_id: PAGE-215
module: M06
page: DelivaryInvoiceCreationForCustomerByOrderAuto_Loading
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-215 — DelivaryInvoiceCreationForCustomerByOrderAuto_Loading (delivery confirmation of a proforma invoice)

> **Evidence level:** Tier A — `Solution.Web/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto_Loading.aspx.cs` read for Page_Load/validation/save/SaveInvoice/SaveInvoiceDetail/SavePayment/DqtyCalculation-start (lines 15-160, 895-1350, 1596-1660; the ~350 lines of campaign/trade-policy recomputation in `LoadInvoice`/`DqtyCalculation` (≈240-330, 1660-1830) only skimmed → rules PARTIAL); `InvoiceDAL.UpdateInvoice/UpdateInvoiceDetail/PaymentUpdateInvoice*/ReturnToDCStore`. `sp_Sales_DeliverySalesPosting` and `sp_GET_DelivaryInvoiceNoCheckById` bodies UNKNOWN. **Semantically this is M07 (Delivery/Invoice) functionality filed under M06 by the classifier; cross-link `specs/invoice-management` if that module is documented.** Sibling: PAGE-214.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-215 |
| Module | M06 by classifier; functionally M07 |
| Kind | Entry / Transaction |
| Class | SInventory_UI_DelivaryInvoiceCreationForCustomerByOrderAuto_Loading |
| LOC | code-behind 2010 |
| Reached from | `LoadingSummary.aspx.cs:271-272` (sets `Session["InvoiceId"]` then redirects here) |

## 2. Purpose
For one proforma invoice loaded from the loading summary, the user enters per line the quantity that was actually **delivered** (Full / Partial / Reject), gives a return reason for short-delivered lines and confirms. The page stamps the delivery numbers/status on the invoice, returns undelivered quantity to DC stock and posts the delivery journal. CONFIRMED.

## 3. Business Objective
Reconcile what was invoiced against what the customer accepted, restore stock for undelivered goods and recognise sales revenue only for the delivered value. INFERRED (accounting posting SP body unknown).

## 4. Navigation
Inbound: `LoadingSummary.aspx` (row action; Session["InvoiceId"]). After save the form is cleared and stays on page; no redirect. Back link → `LoadingSummary.aspx` (`.cs:129`).

## 5. Route
`~/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto_Loading.aspx` (session-driven; no querystring).

## 6. Source Files
aspx/aspx.cs; `InvoiceBLL`, `OrderInfoBLL`, `InvoiceDAL`, `BonusCampaignNewDAL.GetDelivaryInvoiceNoCheckById`, `CustPaymentBLL` (dead `SavePayment`).

## 7. UI Control Inventory
Same grid family as PAGE-218: per line `codeTextBox`, `nameTextBox`, `unitPriceTextBox`, `upVatTextBox`, `qtyTextBox`, `tpTextBox`, `tpVatTextBox`, `dpTextBox`, `dpAmtTextBox`, `npTextBox`, `bQtyTextBox`, `tQtyTextBox` (invoiced qty), `dQtyTextBox` (delivered qty, editable, postback `dQtyTextBox_TextChanged`), `statusLabel` (Full/Partial/Reject badge), `reasonReturnDropDownList` (`aInvoiceBll.ReturnReason` list; enabled only for Partial/Reject), plus header labels/textboxes (order no/date, customer block, `invDateTextBox`, `deliverypersonName/Mobile`, `remarksTextBox`), totals (`tpTptalTextBox`, `disTotalTextBox`, `vatTotalTextBox`, `grandTotalTextBox`, `pdTextBox`, `campaignTPTextBox1`), hidden `invoiceHiddenField`, `invoiceNoHiddenField`, `orderIdHiddenField`, `hdCustomerMasterId`, `hdComUnitId`, `hdMiaId`, `invTextBox` (shows `DEL-<invoice no>`), `saveButton`.

## 8. Field Mapping
| UI | DAL | Column |
|---|---|---|
| totals (`tpTptal/disTotal/vatTotal/grandTotal`) | `InvoiceDAL.UpdateInvoice` | tblInvoice.DeliveryTpTotal / DeliveryTpDiscount / DeliveryTpVat / DeliveryTpGrandTotal |
| status Full/Partial/Reject | ″ | tblInvoice.DeliveryInvoiceStatus |
| `DEL-`+invoice no | ″ | tblInvoice.DelivaryInvoiceNo |
| pdTextBox | ″ | tblInvoice.DelivarySpecialAmount |
| session user / now / invoice date | ″ | tblInvoice.UpdateBy / UpdateDatetime / UpdateDate |
| same totals + status | `PaymentUpdateInvoice` | tblInvoice.PaymentTpTotal/PaymentTpDiscount/PaymentTpVat/PaymentTpGrandTotal, PaymentInvoiceStatus, PaymentInvoiceNo=`RTN-`+no, PaymentBy, PaymentDate |
| dQty, tp, vat, discount, net, line status, reason | `UpdateInvoiceDetail` | tblInvoiceDetail.DeliveryQuantity/BonusQuantity(0)/TotalQuantity/TotalPrice/TotalPriceVatAmount/DiscountPercentage/DiscountAmount/NetAmount/DeliveryStatus/DelivarySpecialAmount(0)/ReturnReason |
| same | `PaymentUpdateInvoiceDetail` | tblInvoiceDetail.Payment* columns |
| tQty−dQty (Partial/Reject) | `ReturnToDCStore(qty, DCStoreId, InvoiceDetailId)` | INSERT tblDCStore (Trans_Type 'Receive', StockCondition 'DeliveryReturn', ReturnFromInvoiceDetailId) |

## 9. Dropdown Mapping
`reasonReturnDropDownList` ← `InvoiceBLL.ReturnReason` → DAL `ReturnReason(ddl)` (source table UNKNOWN); `payTypeDDL` ← `PaymentTypeLoadBLL`.

## 10. Button Action Mapping
`saveButton_Click:910` → Validation → duplicate guard → `SaveInvoice` → `SaveInvoiceDetail` → `PostDeliveryConfPartialJournal` → Clear + alert; `dQtyTextBox_TextChanged` → `DqtyCalculation`.

## 11. Frontend Flow
`Page_Load` (first load): if `Session["InvoiceId"]` present → `LoadInvoice(id)` (loads `tblInvoice` + detail rows via `OrderInfoBLL.LoadInvoice`, binds grid, `TotalValueCalculation`, fills reason lists); clear session key; payment type list; today's date. Editing a delivered qty recomputes line status, TP, VAT, discount (trade-policy/offer/campaign re-evaluation using `LoadOffer`, `LoadCampaign`, `GetTradeTerm(Old)`, `getEzeventCampaignCheck`, `getEsomiumCampaignCheck(Seacoral)`) and totals — all server-side postbacks.

## 12. Backend Flow
`saveButton_Click`: `Validation()`; then `GetDelivaryInvoiceNoCheckById(invoiceId,"DelivaryInvoiceNo")` (`sp_GET_DelivaryInvoiceNoCheckById`): if rows returned ⇒ alert "Already Exist!" + Clear (duplicate-delivery guard); else `SaveInvoice()` (header update + status roll-up), `SaveInvoiceDetail()` (per line update + stock return), `InvoiceBLL.PostDeliveryConfPartialJournal(invoiceId)` → `sp_Sales_DeliverySalesPosting(@InvoiceId)` (accounts posting, body UNKNOWN), success "Delivery confirmation Created Successsfully!". try/catch shows generic "Some Networks Error blocked Your Transaction Please Contract Administrator!!" — the real exception is discarded. No transaction.
`SaveInvoice`: `DelivaryInvoiceNo = invoiceNoHiddenField`; header totals from the page; status roll-up: all lines "Full" ⇒ `Full`; else `Partial`; all lines "Reject" ⇒ `Reject`; then `UpdateInvoice` (DAL adds `"DEL-"` prefix) and `PaymentUpdateInvoice` (`"RTN-"` prefix).
`SaveInvoiceDetail`: per line: delivered qty used as `Quantity`/`TotalQuantity`; NetAmount = TotalPrice − DiscountAmount + VAT; `DeliveryStatus` from `statusLabel`; if the invoice line's DC store row exists and status is Partial/Reject → `ReturnToDCStore(tQty − dQty, DCStoreId, InvoiceDetailId)`; `UpdateInvoiceDetail` + `PaymentUpdateInvoiceDetail`.

## 13. API Mapping
None.

## 14. Service Mapping
InvoiceBLL (UpdateInvoice, PaymentUpdateInvoice, UpdateInvoiceDetail, PaymentUpdateInvoiceDetail, ReturnToDCStore, DCInfoWithDCId, PostDeliveryConfPartialJournal, ReturnReason), OrderInfoBLL (LoadInvoice, LoadOffer, LoadCampaign, GetTradeTerm(Old), GetFixedCustomerfromInvoiceTable, ProductVat, ProductDiscount).

## 15. Repository Mapping
`InvoiceDAL.UpdateInvoice:772`, `PaymentUpdateInvoice:793`, `UpdateInvoiceDetail:810`, `PaymentUpdateInvoiceDetail:844`, `PostDeliveryConfPartialJournal:833`, `ReturnToDCStore:1827` (all inline SQL except the posting SP).

## 16. Stored Procedure Mapping
`sp_Sales_DeliverySalesPosting` (UNKNOWN; writes accounts vouchers in `CSTL_ACCDB_Unicorn` presumably — INFERRED), `sp_GET_DelivaryInvoiceNoCheckById` (UNKNOWN; read).

## 17. Database Mapping
Written: `tblInvoice` (Delivery*/Payment*), `tblInvoiceDetail` (Delivery*/Payment*), `tblDCStore` (Receive rows), accounts DB via SP. Read: tblInvoice(Detail), tblDCStore, tblOrder(Detail), tblTradePolicy*, tblFocMaster…

## 18. Validation Matrix
| Rule | Where | Message |
|---|---|---|
| order no non-empty | `Validation:~41` | "Please Input Order Number!!" (INFERRED same as PAGE-214 lines 41-59) |
| no blank code/tQty/dQty | ″ | "Please Input Valid Data!!!" |
| reason required when status Reject or **"Parial"** | `:72-80` | "Please Select Reason !!" — misspelled literal ⇒ real "Partial" lines skip the check |
| already delivered | `saveButton_Click:918-944` | "Already Exist!" |
| delivered > invoiced | `DqtyCalculation`: dQty reset to 0 ⇒ Reject | none shown |

## 19. Business Rules
| ID | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M06-049 | Per-line delivery status from `dQty` vs invoiced `tQty`: equal ⇒ **Full**; 0<dQty<tQty ⇒ **Partial**; dQty=0 ⇒ **Reject**; dQty>tQty ⇒ dQty forced to 0 and **Reject**. | .cs (DqtyCalculation, same as PAGE-214 :1464-1518) | page | CONFIRMED (214 code read; 215 assumed identical — INFERRED for 215 lines) |
| BR-M06-050 | Invoice-level status: all lines Full ⇒ Full; all lines Reject ⇒ Reject; otherwise Partial. | .cs:1270-1297 | page | CONFIRMED |
| BR-M06-051 | A delivery confirmation can be saved only once per invoice (guard SP `sp_GET_DelivaryInvoiceNoCheckById`); the delivery number is `DEL-`+proforma number and a parallel "payment" record `RTN-`+number is written with identical figures. | .cs:918-944; InvoiceDAL.cs:772-809 | page+SQL | CONFIRMED (guard call) / UNKNOWN (SP semantics) |
| BR-M06-052 | Reason for short/rejected delivery is required — but the check tests `"Parial"`, so Partial lines are not enforced. | .cs:72-80 | page | CONFIRMED |
| BR-M06-053 | Undelivered quantity (tQty − dQty) of Partial/Reject lines is put back into DC stock as a new `Receive` row with `StockCondition='DeliveryReturn'` referencing the original receive row and invoice detail. | .cs:1085-1087; InvoiceDAL.cs:1827-1862 | page+SQL | CONFIRMED |
| BR-M06-054 | Delivery value figures on the invoice = delivered qty × price, with discount and VAT recomputed for delivered qty; delivery journal posted by SP after saving. | .cs:1029-1093, 929 | page+SP | CONFIRMED (call) / UNKNOWN (posting) |
| BR-M06-055 | Delivery figures are duplicated into the `Payment*` column set at confirmation time (PaymentTpTotal…, PaymentInvoiceNo). | .cs:1299-1300, 1093 | page | CONFIRMED |
| BR-M06-056 | Product-specific FOC/offer helpers keyed on hard-coded codes (`ARD01`≥2, `ARD02`≥8, `FGD02`≥4) and campaign names influence discount recalculation; a copy-paste defect tests `ProductCode2` instead of `ProductCode3` for `FGD02`. | .cs:1596-1626 | page | CONFIRMED |

## 20. Permission Matrix
`Session["UserId"]`, `Session["LoginName"]` stored as updater. No role check; access is menu + Session["InvoiceId"] handoff only.

## 21. Status Lifecycle
`tblInvoice.DelivaryInvoiceNo NULL → 'DEL-…'`; `DeliveryInvoiceStatus NULL → Full|Partial|Reject`; line `DeliveryStatus` same; stock returns as Receive rows. Reversal path: `sp_DeleteDeliveryInvoice` (`ProformaOrInvoiceReturn.cs:130`, body UNKNOWN).

## 22. Error Handling
Generic message hides cause; partial failure leaves header updated but details/posting missing (no transaction) — duplicate guard may then block a retry ("Already Exist!") while `sp_Sales_DeliverySalesPosting` never ran.

## 23. Audit
`UpdateBy`, `UpdateDatetime`, `PaymentBy`, `PaymentDate`.

## 24. Integration
Accounts posting via SP into `CSTL_ACCDB_Unicorn` (INFERRED); no SAP.

## 25. Reports
None here; `LoadingSummary` prints loading sheets (M07).

## 26. Dependencies
LoadingSummary.aspx, tblDCStore, accounts DB, trade-policy tables, hard-coded campaign names.

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-01 | Positive | dQty = tQty all lines | invoice Full; no stock return; journal posted | 049,050,054 |
| TC-02 | Positive | one line dQty < tQty | line Partial; `tblDCStore` Receive row = difference; reason list enabled | 049,053 |
| TC-03 | Negative | Partial line, no reason | (defect) accepted | 052 |
| TC-04 | Negative | Reject line, no reason | "Please Select Reason !!" | 052 |
| TC-05 | Edge | dQty > tQty | dQty set 0, line Reject | 049 |
| TC-06 | Negative | Save twice | second "Already Exist!" | 051 |
| TC-07 | Edge | all lines dQty 0 | invoice Reject | 050 |
| TC-08 | Edge | DB error after header update | generic error; retry blocked; posting missing | 051 |

## 28. Reverse Engineering References
`…Auto_Loading.aspx(.cs)`, `InvoiceDAL.cs`, `LoadingSummary.aspx.cs:255-275`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | — | MISSING |
| Reason enforcement | None existed | typo disables Partial | GAP-M06-017 |
| Atomicity | None existed | none | GAP-M06-010 |

## 30. Known Gaps
"Parial" typo; no transaction; generic error; duplication with PAGE-214/`…AutoForSalesCnfrm.*.exclude`; hard-coded product codes; `Payment*` duplication semantics unknown; posting SP unknown.

## 31. Change Impact
`InvoiceDAL.UpdateInvoice/UpdateInvoiceDetail/ReturnToDCStore` /`PostDeliveryConfPartialJournal` are called (grep of `Solution.Web`) from PAGE-214, this page, `PaymentPartial.aspx.cs`, `SndTimesSalesReturnPartial.aspx.cs` and `ReturnInvoiceCreationForCustomer.aspx.cs`; a change affects all five. `sp_Sales_DeliverySalesPosting` posting affects customer ledger and sales reports (M08/M09).
