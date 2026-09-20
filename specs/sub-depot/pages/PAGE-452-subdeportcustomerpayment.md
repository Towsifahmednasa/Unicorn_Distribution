---
page_id: PAGE-452
module: M12
page: SubDeportCustomerPayment
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-452 — SubDeportCustomerPayment

> **Evidence level:** Tier A - reviewed 2026-09-19 against the .aspx, .aspx.cs and the BLL/DAL/DAO code they reach. Stored-procedure bodies are NOT in the repository unless stated; such effects are marked `UNKNOWN`. Confidence tags: CONFIRMED (read in code), INFERRED (implied by several references), UNKNOWN.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-452 |
| Module | M12 — Sub-Depot Operations (INFERRED from folder + file name) |
| Page name | SubDeportCustomerPayment |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SubDepot_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SubDepot_UI_SubDeportCustomerPayment |
| aspx size | 14678 bytes |
| Code-behind LOC | 351 |
| Inline script size | 985 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**SubDeportCustomerPayment** (caption `Market Wise Customer Payment`, `SubDeportCustomerPayment.aspx:25`) records customer payments against delivered **sub-depot invoices** (`tblSubInvoiceMaster`). Intended flow: choose the sales center, choose a market, the page lists the market's delivered, not-fully-paid sub-depot invoices with previous payment and due; the user ticks invoices, edits the pay amount per invoice, enters total payment amount, type and reference and saves. Saving writes one payment header + detail per ticked invoice and updates the invoice's `PaymentAmount`/`PaymentStatus` (`Full`/`Partial`). CONFIRMED (code `SubDeportCustomerPayment.aspx.cs:221-295`).

**Important**: the user-visible `Market:` dropdown (`subdeportDropDownList1`) is not referenced by any code-behind and has no data source; the code that lists invoices is bound to the **invisible** `marketDropDownList` (`Visible=false`) - see GAP-M12-017. As shipped, the invoice grid cannot be populated through the visible UI (INFERRED from markup `.aspx:100-104,160-172`).

## 3. Business Objective
Collect sub-depot customer receipts against delivered invoices and keep each invoice's paid amount/status current so that outstanding balances (dues) are correct. CONFIRMED from labels (`Payment Amount`, `Pay Amount`, `Due Amount`) and code; the accounting treatment is UNKNOWN because no journal posting is performed here (BR-M12-036), unlike the main-depot payment page which calls `sp_Sales_CustomerPaymentPosting`.

## 4. Navigation
Menu placement DB-driven (`UNKNOWN`). Reset -> `SubDeportCustomerPayment.aspx` (`.cs:347-350`). Related: `SubdeportPaymentReverse.aspx` (PAGE-439) reverses payments; main equivalent `SInventory_UI/CustomerPayment.aspx`. Master `NewMasterPage.master` (`sweetAlertConfirm_Submit` on Save).

## 5. Route
`~/SubDepot_UI/SubDeportCustomerPayment.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SubDepot_UI/SubDeportCustomerPayment.aspx |
| Code-behind | Solution.Web/SubDepot_UI/SubDeportCustomerPayment.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | CustPaymentBLL (Library.BLL/SInventory_BLL/CustPaymentBLL.cs) |
| DAL class | CustPaymentDAL (Library.DAL/SInventory_DAL/CustPaymentDAL.cs) |
| DAO/entity class | CustPayment (Library.DAO/SInventory_Entities/CustPayment.cs) |
| DAO/entity class | CustPaymentDetail (Library.DAO/SInventory_Entities/CustPaymentDetail.cs) |
| DAO/entity class | CustomerMaster (Library.BLL/SInventory_BLL/CustomerMaster.cs) |

## 7. UI Control Inventory
| Control ID | Label | Required | Default | Read-only / visible | Data source | Handler |
|---|---|---|---|---|---|---|
| salesCenterDropDownList | Sales Center: (*) | none in code; needed for market load | `CustPaymentBLL.LoadSC(ddl, Session["UserId"])` -> user-assigned DCs (`CustPaymentDAL.cs:142-158`) | - | tblCompanyUnit + tblUserCompanyUnit | `salesCenterDropDownList_SelectedIndexChanged` (:49) -> loads hidden `marketDropDownList`, clears grid |
| subdeportDropDownList1 | Market: (*) | **not used by code** | empty (never bound) | visible | none | none (GAP-M12-017) |
| marketDropDownList | (hidden) | drives invoice load | bound from sales center | **Visible=false** | `CustPaymentBLL.LoadMarket` (`View_CustomerMaster` x `tblMarket`, `CustPaymentDAL.cs:185-192`) | `marketDropDownList_SelectedIndexChanged` (:91) -> `LoadSubDeportInvoice` |
| customerTextBox / customerDropDownList / refDtTextBox | (hidden remnants) | - | - | Visible=false | - | `customerTextBox_TextChanged` (:55) uses main `LoadInvoice` (tblInvoice!) |
| paymentDtTextBox | Payment Date: (*) | server: non-empty (BR-M12-033) | today `dd-MMM-yyyy` (`.cs:20`) | ReadOnly=True (datepicker) | - | - |
| paymentAmountTextBox | Payment Amount: (*) | server: non-empty; = sum of pay amounts | computed by `TotalPaymentAmount()` (`F` format) | digits and `.` | - | - |
| payTypeDDL | Payment Type: (*) | server: non-empty | index 1 (`.cs:37,45`) | - | `PaymentTypeLoadBLL` (`tblPaymentType`) | - |
| refNameTextBox | Ref No: | no | empty | - | - | - |
| orderGridView | select, Customer Code/Name, Pro.Invoice No/Date, Del Invoice No/Date, Del Inv Amount, Previous Pay, Due Amount, Pay Amount | >=1 ticked | - | DataKeys `InvoiceId,CustomerMasterId` | `CustPaymentBLL.LoadSubDeportInvoice` | `chkSelect_OnCheckedChanged`, `payAmountTextBox_TextChanged`, `chkSelectAll_CheckedChanged` |
| payAmountTextBox | Pay Amount | server: non-empty for ticked rows | `Eval("Due")` (`.aspx:214`) | AutoPostBack | - | `payAmountTextBox_TextChanged` (:296) |
| saveButton | Save | - | - | JS confirm | - | `saveButton_Click` (:221) |
| Reset | - | - | - | - | - | `Unnamed_Click` |

## 8. Field Mapping
| UI control | Variable | Handler | BLL | DAL | SQL parameter | Table.column |
|---|---|---|---|---|---|---|
| grid DataKeys[1] | `CustPayment.CustomerMasterId` | :247 | `CustPaymentBLL.SubdeportSaveCustPayment` (:91) | `CustPaymentDAL.SaveCustPayment` :78 | `@CustomerMasterId` | `tblCustomerPay.CustomerMasterId` |
| marketDropDownList (hidden) | `MarketId` | :248 | same | same | `@MarketId` | `tblCustomerPay.MarketId` |
| salesCenterDropDownList | `ComUnitId` | :249 | same | same | `@ComUnitId` | `tblCustomerPay.ComUnitId` |
| paymentDtTextBox | `PaymentDate` | :250 | same | same | `@PaymentDate` | `tblCustomerPay.PaymentDate` |
| payAmountTextBox (row) | `PaymentAmount` | :251 | same | same | `@PaymentAmount` | `tblCustomerPay.PaymentAmount` (per-invoice amount, not the grand total) |
| payTypeDDL.SelectedItem.Text | `PayType` | :252 | same | same | `@PayType` | `tblCustomerPay.PayType` (text) |
| refNameTextBox | `RefNo` | :253 | same | same | `@RefNo` | `tblCustomerPay.RefNo` |
| refDtTextBox (hidden) | `RefDate` | :254-257 | same | same | `@RefDate` | `tblCustomerPay.RefDate` (never entered: box hidden) |
| Session LoginName / now | `CreateBy`, `CreateDate` | :258-259 | same | same | `@CreateBy`,`@CreateDate` | `tblCustomerPay.CreateBy/CreateDate` |
| grid DataKeys["InvoiceId"] | `CustPaymentDetail.InvoiceId` | :279 | `SubdeportSaveCustDetail` | `CustPaymentDAL.SubdeportSaveCustDetail` :121 | `@InvoiceId` | `tblCustPayDetail.SubDeportInvoiceId` |
| payAmountTextBox | `CustPaymentDetail.PaymentAmount` | :280 | same | same | `@PaymentAmount` | `tblCustPayDetail.PaymentAmount` |
| generated | `CustPayId`, `CustPayDetailId` (MAX+1) | BLL:91-108 | `PrimaryKeyMax` | - | - | `tblCustomerPay.CustPayId`, `tblCustPayDetail.CustPayId/CustPayDetailId` |
| computed total | status/amount | :262-276 | `SubdeportUpdateInvoicePaymentAmount` | `CustPaymentDAL.cs:167-171` | `@PaymentAmount,@PaymentStatus,@InvoiceId` | `tblSubInvoiceMaster.PaymentAmount`, `.PaymentStatus` |

## 9. Dropdown Mapping
| Dropdown | Loader | SQL | Text/Value | Tables |
|---|---|---|---|---|
| salesCenterDropDownList | `DropDownList()` :43 -> `CustPaymentBLL.LoadSC(ddl,userId)` | `select ComUnitId,ComUnitName from tblCompanyUnit WHERE ComUnitId IN (SELECT CompanyUnitId FROM tblUserCompanyUnit WHERE UserId=@UserId)` | ComUnitName / ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| payTypeDDL | :44 `PaymentTypeLoadBLL` | `select * from tblPaymentType` (`CustPaymentDAL.cs:179-184`) | PaymentTypeName / PaymentTypeId | tblPaymentType |
| marketDropDownList (hidden) | :51 `LoadMarket` | `SELECT DISTINCT CM.MarketId,MarketCode,MarketName FROM tblCompanyUnit Cu JOIN View_CustomerMaster CM ... JOIN tblMarket ... WHERE Cu.ComUnitId=@ComUnitId` | MarketName / MarketId | as listed |
| subdeportDropDownList1 (visible `Market`) | none | - | - | - |

## 10. Button Action Mapping
| Button | Handler | BLL -> DAL | SQL | DB change |
|---|---|---|---|---|
| (market change - hidden) | `marketDropDownList_SelectedIndexChanged` :91 | `LoadSubDeportInvoice` -> `CustPaymentDAL` :220 | SELECT tblSubInvoiceMaster JOIN View_CustomerMaster WHERE DeliveryTpGrandTotal>0 AND MarketId=@ AND ComUnitId=@ AND (PaymentStatus IS NULL OR 'Partial') AND DelivaryInvoiceNo IS NOT NULL AND DeliveryInvoiceStatus IN ('Partial','Full') (BR-M12-032) | read |
| Save | `saveButton_Click` :221 (JS confirm) | per ticked row: `SubdeportUpdateInvoicePaymentAmount` -> UPDATE; `SubdeportSaveCustPayment` -> `PrimaryKeyMax`, INSERT tblCustomerPay, `SubdeportSaveCustDetail` -> INSERT tblCustPayDetail | parameterised inline SQL (`SInventorySql.Execute`) | tblSubInvoiceMaster (UPDATE), tblCustomerPay, tblCustPayDetail (INSERT) |
| pay amount / tick | :296 / :342 | none | - | none |

## 11. Frontend Flow
- `Page_Load`: DC list (user-restricted), payment types, type index 1, payment date today (`.cs:15-22,41-47`).
- Sales center change -> hidden market bound + grid cleared. Market change (hidden control - cannot fire from the UI) -> invoice grid. `Pay Amount` textbox defaults to `Due` (`Eval("Due")`).
- Pay amount TextChanged: if (entered + previous) > invoice amount -> 0 + alert (BR-M12-034); then `TotalPaymentAmount()` sums ticked rows into `Payment Amount` (`ToString("F")`).
- `chkSelectAll` ticks all rows and recomputes; unticking clears the total.
- Save: JS confirm (`sweetAlertConfirm_Submit`) -> postback -> `Validation()` -> loop.
- JS: `pageLoad()` select2/datepicker only. No AJAX.

## 12. Backend Flow
| Handler | Source | Flow | try/catch | Txn |
|---|---|---|---|---|
| `saveButton_Click` | `.cs:221-295` | `Validation()` (BR-M12-033). For each row i: tick? -> build `CustPayment` (CustomerMasterId=DataKeys[1], MarketId=hidden market, ComUnitId, date, amount=row pay, PayType=text, RefNo, CreateBy/Date). `prevamount` = cell[8] if cell[6] != `&nbsp;`. If (pay + prev) == cell[7] (Del Inv Amount) -> `SubdeportUpdateInvoicePaymentAmount(total,'Full',invoiceId)` else `'Partial'`. Then `SubdeportSaveCustPayment(payment, detail)` (header INSERT + detail INSERT). After the loop: alert `Data Saved Successfully !!!!!` and `Clear()` | BLL rethrows; page none | no |
| `payAmountTextBox_TextChanged` | :296-320 | guard BR-M12-034 | no | no |
| `customerTextBox_TextChanged`/`marketDropDownList_SelectedIndexChanged` | :55-116 | invoice load; the first uses the MAIN `LoadInvoice` (tblInvoice, top 10) | no | no |

Order of writes: invoice UPDATE first, then payment INSERTs - a failure in between leaves an invoice marked paid without a payment row. Multiple invoices: each row writes its own `tblCustomerPay` (CustPayId = MAX+1).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| CustPaymentBLL.LoadSC | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:167 |
| CustPaymentBLL.PaymentTypeLoadBLL | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:189 |
| CustPaymentBLL.LoadMarket | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:185 |
| CustPaymentBLL.LoadInvoice | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:193 |
| CustPaymentBLL.CustomerLoad | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:206 |
| CustPaymentBLL.LoadCustomerMaster | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:181 |
| CustPaymentBLL.LoadSubDeportInvoice | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:202 |
| CustPaymentBLL.SubdeportUpdateInvoicePaymentAmount | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:118 |
| CustPaymentBLL.SubdeportSaveCustPayment | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:92 |
| CustPaymentBLL.SubdeportSaveCustDetail | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:147 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CustPaymentDAL.LoadSC | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:137 | inline SQL: tblCompanyUnit |  |
| CustPaymentDAL.SingleParameter | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:344 | delegates / no SQL literal |  |
| CustPaymentDAL.PaymentTypeLoad | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:180 | inline SQL: tblPaymentType |  |
| CustPaymentDAL.LoadMarket | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:186 | inline SQL: View_CustomerMaster, tblCompanyUnit, tblMarket |  |
| CustPaymentDAL.LoadInvoice | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:196 | inline SQL: tblCustPayDetail, tblInvoice, tblInvoiceDetail, tblReturnInvoice |  |
| CustPaymentDAL.CustomerLoad | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:253 | inline SQL: tblCustMaster |  |
| CustPaymentDAL.LoadCustomerMaster | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:174 | inline SQL: View_CustomerMaster, tblCustMaster |  |
| CustPaymentDAL.LoadSubDeportInvoice | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:221 | inline SQL: View_CustomerMaster, tblSubInvoiceMaster | @ComUnitId, @MarketId |
| CustPaymentDAL.SubdeportUpdateInvoicePaymentAmount | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:168 | inline SQL: tblSubInvoiceMaster |  |
| CustPaymentDAL.InvoicePaymentParameters | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:334 | delegates / no SQL literal | @InvoiceId, @PaymentAmount, @PaymentStatus |
| CustPaymentDAL.SaveCustPayment | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:79 | inline SQL: tblCustomerPay |  |
| CustPaymentDAL.CustPaymentParameters | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:296 | delegates / no SQL literal | @ComUnitId, @CreateBy, @CreateDate, @CustPayId, @CustomerMasterId, @MarketId, @PayType, @PaymentAmou |
| CustPaymentDAL.SubdeportSaveCustDetail | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:122 | inline SQL: tblCustPayDetail |  |
| CustPaymentDAL.CustPaymentDetailParameters | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:315 | delegates / no SQL literal | @AIT, @BankAccId, @CashAccId, @CollectionBy, @CustPayDetailId, @CustPayId, @DANameId, @Discount, @In |

## 16. Stored Procedure Mapping
| SP | Use | Body in repo |
|---|---|---|
| (none in the active path) | all SQL is inline parameterised (`SInventorySql`) except `PrimaryKeyMax` selects | n/a |
`sp_Sales_CustomerPaymentPosting` (accounting posting used by the main page, body in `sp_bodies.json`) is **not** called here (BR-M12-036).

## 17. Database Mapping
| Table | Access | Columns | Source |
|---|---|---|---|
| tblSubInvoiceMaster | SELECT, UPDATE | PaymentAmount, PaymentStatus; list uses DeliveryTpGrandTotal, DelivaryInvoiceNo, DeliveryInvoiceStatus, InvoiceNo/Date, UpdateDate, ComUnitId, CustomerMasterId | `CustPaymentDAL.cs:167-171,220-229` |
| tblCustomerPay | INSERT | CustPayId, MarketId, CustomerMasterId, ComUnitId, PaymentDate, PaymentAmount, PayType, RefNo, RefDate, Remarks(null), CreateBy, CreateDate | `CustPaymentDAL.cs:78-102` |
| tblCustPayDetail | INSERT | CustPayDetailId, SubDeportInvoiceId, PaymentAmount, CustPayId | `CustPaymentDAL.cs:121-136` |
| View_CustomerMaster, tblCustMaster, tblMarket, tblCompanyUnit, tblUserCompanyUnit, tblPaymentType | SELECT | look-ups | - |
Note `tblCustPayDetail.InvoiceId` (main invoice) is left NULL for sub-depot payments; the payment reverse and reports must use `SubDeportInvoiceId`.

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Message | BR |
|---|---|---|---|---|---|
| Invoice selection | >=1 ticked | - | yes | `Please Select Invoice from List!!` | BR-M12-033 |
| Pay Amount (ticked row) | non-empty | - | yes | `Please fill out Pay Amount !!` | BR-M12-033 |
| Payment Amount | non-empty | - | yes | `Please Select Payment Amount!!` | BR-M12-033 |
| Sum(pay) == Payment Amount | exact decimal equality | - | yes | `Total Invoice Payment Amount Must Be Equel To Payment Amount` | BR-M12-033 |
| Payment Type | selected | - | yes | `Please Select Payment Type!!` | BR-M12-033 |
| Payment Date | non-empty | ReadOnly | yes | `Please Select Payment Date!!` | BR-M12-033 |
| Pay + Previous <= invoice amount | on change only | post-back | **not on Save** | `Cannot Be Greater then Invoice Quantity ` | BR-M12-034 |
| Negative / zero amount, future date, Ref No for cheque | not validated | - | - | - | - |
| Invoices found | rows exist | - | yes | `No Invoice Found!!` | BR-M12-032 |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M12-032 | Sub-depot payable invoices: tblSubInvoiceMaster with DeliveryTpGrandTotal>0, matching ComUnitId and MarketId, PaymentStatus NULL or 'Partial', DelivaryInvoiceNo NOT NULL and DeliveryInvoiceStatus 'Partial'/'Full', joined to View_CustomerMaster. Due = DeliveryTpGrandTotal - ISNULL(PaymentAmount,0); unlike main depot, sales-return/adjust amounts are NOT deducted. | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:220-229 (main: 205-217) | CustPaymentDAL.LoadSubDeportInvoice | CONFIRMED |
| BR-M12-033 | Payment save validation: at least one invoice ticked (`Please Select Invoice from List!!`); each ticked row has a pay amount (`Please fill out Pay Amount !!`); payment amount total non-empty (`Please Select Payment Amount!!`); sum of ticked pay amounts must equal the payment amount exactly (`Total Invoice Payment Amount Must Be Equel To Payment Amount`); payment type and date present. | Solution.Web/SubDepot_UI/SubDeportCustomerPayment.aspx.cs:146-219 | Validation() | CONFIRMED |
| BR-M12-034 | Over-payment guard: on pay-amount change, if (entered + previous payment) > invoice amount the box is reset to 0 with `Cannot Be Greater then Invoice Quantity ` (sic). Only enforced client-triggered; not repeated on Save. | Solution.Web/SubDepot_UI/SubDeportCustomerPayment.aspx.cs:296-320 | payAmountTextBox_TextChanged | CONFIRMED |
| BR-M12-035 | Payment status: if (pay + previous) equals the Del-invoice amount (grid cell 7) status='Full' else 'Partial'; tblSubInvoiceMaster.PaymentAmount is overwritten with (pay + previous). `previous` is read from cell 8 only when cell 6 (delivery date) is not `&nbsp;`, i.e. the wrong column drives the test. | Solution.Web/SubDepot_UI/SubDeportCustomerPayment.aspx.cs:237-276; Library.DAL/SInventory_DAL/CustPaymentDAL.cs:167-171 | saveButton_Click | CONFIRMED |
| BR-M12-036 | Payment persistence: per ticked invoice one tblCustomerPay row (MarketId, ComUnitId, date, amount, PayType text, RefNo, RefDate, CreateBy=LoginName, CreateDate) and one tblCustPayDetail row keyed by SubDeportInvoiceId. Unlike main depot there is no duplicate-payment check, no AIT/discount/collection-by/bank/cash fields, and no accounting posting (`sp_Sales_CustomerPaymentPosting` is never called). | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:91-108; Library.DAL/SInventory_DAL/CustPaymentDAL.cs:78-136 (main: CustomerPayment.aspx.cs:435-500) | CustPaymentBLL.SubdeportSaveCustPayment | CONFIRMED |
| BR-M12-037 | Sub-depot payment reverse (by order code): finds the sub invoice via tblOrder.OrderCode, sets tblSubInvoiceMaster.PaymentStatus/PaymentAmount to NULL and deletes ONLY the first found payment (tblCustPayDetail then tblCustomerPay by that CustPayId); further payments of the invoice remain. | Solution.Web/SInventory_UI/SubdeportPaymentReverse.aspx.cs:23-66; Library.DAL/SInventory_DAL/PaymentReverseDal.cs:26-47,68-105 | SubdeportPaymentReverse.submitButton_Click1 | CONFIRMED |
| BR-M12-045 | Sub-depot receive and stock-report pages branch on Session UserType: Admin sees all DCs, others only DCs assigned in tblUserCompanyUnit (DCLoad(ddl,userId)). The transfer, adjustment and invoice pages use the unfiltered DC list (`select ... from tblCompanyUnit`) so any logged-in user may pick any DC. | Solution.Web/SubDepot_UI/StockReceiveBySubDepot.aspx.cs:16-31; SubDeportStockReport.aspx.cs:11-27; Library.DAL/SInventory_DAL/RequisitionDAL.cs:35-39,53-64; StockTransferDcToSubDepot.aspx.cs:25 | Page_Load DC dropdown loaders | CONFIRMED |

**Worked calculation (BR-M12-035)**: Del Inv Amount 10,000; previous 4,000; entered 6,000 -> total 10,000 = amount => `Full`, `PaymentAmount=10000`. Entered 5,000 -> 9,000 <> 10,000 => `Partial`, `PaymentAmount=9000`. Entered 7,000 -> guard resets the box to 0 (BR-M12-034); if bypassed, 11,000 <> 10,000 => stored as `Partial` with `PaymentAmount=11000` (over-payment accepted). Comparison is exact decimal equality; no tolerance/rounding.

## 20. Permission Matrix
- Session keys read: `UserId` (DC list), `LoginName` (CreateBy). No role check in code.
- Authorization is menu-based; the DC list is limited to the user's assigned DCs (this page **does** restrict, unlike the transfer pages), but the market/invoice ids posted back are not re-validated against the user's DCs.
- Any user with menu access may record payments for any invoice of an assigned DC; no maker-checker.

## 21. Status Lifecycle
`tblSubInvoiceMaster.PaymentStatus`: NULL (unpaid) -> `Partial` -> `Full`. Set only by `SubdeportUpdateInvoicePaymentAmount` (`CustPaymentDAL.cs:167-171`) at Save; reset to NULL (with PaymentAmount NULL) by `SubdeportPaymentReverse` (BR-M12-037). Invoices already `Full` are excluded from the list. `DeliveryInvoiceStatus` (`Full`/`Partial`) is set upstream by delivery confirmation SPs (UNKNOWN bodies, BR-M12-041) and gates eligibility. Over-payment is representable (status `Partial` with amount > invoice).

## 22. Error Handling
No try/catch in the page. `CustPaymentBLL.SubdeportSaveCustPayment` catches and rethrows. `Convert.ToDecimal/ToInt32` on grid text throws on `&nbsp;` or empty cells; `marketDropDownList.SelectedValue` is empty (control hidden) -> `Convert.ToInt32("")` `FormatException` at Save (`.cs:248`). Messages by JS alert. Empty `Application_Error`.

## 23. Audit
Audit: `tblCustomerPay.CreateBy` (LoginName) and `CreateDate` (now); `tblCustPayDetail` has none in the INSERT; no record of who changed `tblSubInvoiceMaster.PaymentStatus`. Main depot additionally stores `CollectionBy`, `DANameId`, bank/cash account ids on `tblCustPayDetail` (not here).

## 24. Integration
None (no SAP posting, no journal). Main depot posts each payment via `sp_Sales_CustomerPaymentPosting` (`CustPaymentDAL.cs:68-76`); sub-depot payments are therefore invisible to accounting postings (UNKNOWN whether another job reads `tblCustPayDetail.SubDeportInvoiceId`).

## 25. Reports
No report from this page. Money receipt reports in main depot (`MoneyReceiptAfterPayment`) are keyed on `tblCustPayDetail.InvoiceId` - whether sub-depot payments appear is UNKNOWN (GAP-M12-017).

## 26. Dependencies
BLL: `CustPaymentBLL` (SInventory_BLL). DAL: `CustPaymentDAL`, `SInventorySql`, `ClsPrimaryKeyFind`. DAO: `CustPayment`, `CustPaymentDetail`, `CustomerMaster`. Tables per §17. Toolkit `FilteredTextBoxExtender`, pickadate.

## 27. Test Cases
| ID | Scenario | Steps | Expected | BR |
|---|---|---|---|---|
| TC-01 | Load invoices | sales center + market (hidden control) | delivered unpaid/partial sub invoices; Due = amount - previous | BR-M12-032 |
| TC-02 | Full payment | invoice 10,000 unpaid; pay 10,000; total 10,000; Save | `Full`, PaymentAmount 10000, 1 tblCustomerPay + 1 tblCustPayDetail(SubDeportInvoiceId) | BR-M12-035/036 |
| TC-03 | Partial payment | pay 6,000 | `Partial`, PaymentAmount 6000; still listed | BR-M12-035 |
| TC-04 | Second payment completes | previous 6,000; pay 4,000 | `Full`, PaymentAmount 10000 (only if delivery date cell non-blank) | BR-M12-035 |
| TC-05 | Sum mismatch (negative) | total field differs from sum | `Total Invoice Payment Amount Must Be Equel To Payment Amount` | BR-M12-033 |
| TC-06 | Nothing ticked | Save | `Please Select Invoice from List!!` | BR-M12-033 |
| TC-07 | Overpay | pay 12,000 on 10,000 | box reset 0 + alert | BR-M12-034 |
| TC-08 | Edge: delivery date null | invoice with blank Del Invoice Date and previous pay 4,000; pay 6,000 | previous ignored -> `Partial` 6000 instead of `Full` 10000 | BR-M12-035 |
| TC-09 | Duplicate submit | click Save twice | two payments (no duplicate check) | BR-M12-036 |
| TC-10 | Hidden market | use visible `Market` dropdown | no invoices load (defect) | GAP-M12-017 |

## 28. Reverse Engineering References
- `Solution.Web/SubDepot_UI/SubDeportCustomerPayment.aspx`, `Solution.Web/SubDepot_UI/SubDeportCustomerPayment.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | `SubDeportCustomerPayment.aspx(.cs)` | n/a | MISSING | - |
| Invoice selection | None existed | hidden market control triggers load | - | DEFECT (INFERRED) | GAP-M12-017 |
| Payment posting | None existed | none | - | GAP | BR-M12-036 |
| Status calc | None existed | exact-equality Full/Partial, wrong-column previous amount | - | DEFECT | BR-M12-035 |

## 30. Known Gaps
- Visible Market dropdown unbound; logic hangs on hidden controls (INFERRED: page cannot list invoices).
- Previous payment read only when `Cells[6]` (delivery date) is not blank (wrong column).
- No duplicate-payment guard (`CustPaymentDAL.Existence` exists for main and is not used), no journal posting, no AIT/discount/collection party, no return/adjustment deduction from Due.
- Over-payment guard is UI-only; equality test for Full/Partial; invoice UPDATE before payment INSERT with no transaction.
- `customerTextBox_TextChanged` (dead) queries main invoices.

## 31. Change Impact
- `CustPaymentBLL/DAL` Subdeport methods are used only here; `SaveCustPayment(CustPayment)` (`CustPaymentDAL.cs:78`) and `PaymentTypeLoad`/`LoadSC`/`LoadMarket` are shared with main-depot `CustomerPayment.aspx` and others.
- Tables shared with main depot: `tblCustomerPay` (CustPayId sequence shared), `tblCustPayDetail`. Consumers: `SubdeportPaymentReverse` (WF-M12-06). The main aging report SP `sp_Report_CustomerInvoiceAgingOutstanding` (`Live_Deployment_Script_All_Updates.sql:129-~400`) reads only `tblInvoice`/`tblCustPayDetail.InvoiceId` - it never references `tblSubInvoiceMaster` or `SubDeportInvoiceId`, so sub-depot receivables are absent from that report (CONFIRMED by reading the script).

