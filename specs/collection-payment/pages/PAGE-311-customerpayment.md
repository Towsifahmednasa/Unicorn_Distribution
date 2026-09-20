---
page_id: PAGE-311
module: M09
page: CustomerPayment
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-311 — CustomerPayment (Customer Payment / Collection Entry)

> **Evidence level:** Tier A — `.aspx`, `.aspx.cs`, `CustPaymentBLL`, `CustPaymentDAL`, `OrderInfoDAL`, `SInventoryWebService.GetCustomerPaymentSearch` and the repo-resident SP bodies (`sp_GET_PaymentInvSP`, `sp_GET_PaymentInvSPTPVATAmt`, `sp_Sales_CustomerPaymentPosting`, `sp_Get_MoneyReceiptReportAfterPaymentList`) were read in full. SP bodies NOT in the repo (`sp_Update_InvoiceFinalPayment`, `sp_Check_Duplicate_InvoiceFinalPayment`, `sp_Sales_DeliveryCOGSPosting`, `sp_Get_RouteInfoforCustPayment`, `sp_GET_RouteInformationDADetailDDL`, `sp_GET_forPaymentTerritoryByRouteInformationDDL`) remain `UNKNOWN`. No spec existed before this reverse-engineering. Confidence tags: CONFIRMED / INFERRED / UNKNOWN.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-311 |
| Module | M09 — Collections, Payments & Money Receipts |
| Page name | CustomerPayment |
| Kind | Entry / Transaction (CONFIRMED — writes tblCustomerPay / tblCustPayDetail, posts to accounts DB) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_CustomerPayment |
| Code-behind LOC | 1253 (`Solution.Web/SInventory_UI/CustomerPayment.aspx.cs`); markup 1059 lines |
| Legacy/duplicate siblings | `CustomerPayment - Copy.aspx` (PAGE-312), `CustomerPaymentPPPP.aspx` (PAGE-315; declares the *same class name* `SInventory_UI_CustomerPayment`), `CustomerPaymentSnd.aspx` (PAGE-318) |

## 2. Purpose
Lets a depot user record customer collections (cash or bank) against one or many due invoices of one customer, optionally with a discount and an AIT (advance income tax) deduction per invoice, and then print a money receipt. Behind the scenes it (a) updates the invoice payment status, (b) inserts payment header/detail rows, and (c) auto-posts a credit-voucher (cash/bank Dr, discount Dr, AIT Dr, accounts-receivable Cr) into the accounting database. CONFIRMED (`CustomerPayment.aspx.cs:354-547`, `CustPaymentDAL.cs:66-120`, `Live_Deployment_Script_All_Updates.sql:403-551`).

## 3. Business Objective
Reduce the customer's receivable on each invoice as money is collected, keep TP (trade price) and VAT components of collections separately, book the collection to the correct cash/bank ledger and the customer's subsidiary ledger in accounting, and hand the customer a receipt. INFERRED from labels ("Collection / Disburse Amount", "Print Money Receipt") and code.

## 4. Navigation
Menu placement is DB-driven (`sp_GET_MainMenuRole2`); exact menu path UNKNOWN. Inbound: `CustomerPaymentSnd.aspx` Reset → `CustomerPayment.aspx`, `CustomerPayment.aspx.cs:730` (self-redirect on Reset). The header buttons linking to `CustomerPaymentSnd.aspx` and `CustomerPaymentExcelUpload.aspx` are inside a `runat=server visible=False` block (`CustomerPayment.aspx:366-376`) and are never rendered. Outbound: `../SInventory_RPTVIEW/MoneyReceiptViewerForAfterPayment.aspx` (popup window, `CustomerPayment.aspx.cs:806-810`).

## 5. Route
`~/SInventory_UI/CustomerPayment.aspx` (file route; no MapPageRoute alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/CustomerPayment.aspx |
| Code-behind | Solution.Web/SInventory_UI/CustomerPayment.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| Web service | Solution.Web/App_Code/SInventoryWebService.cs:371-393 (`GetCustomerPaymentSearch`) |
| BLL | Library.BLL/SInventory_BLL/CustPaymentBLL.cs; OrderInfoBLL |
| DAL | Library.DAL/SInventory_DAL/CustPaymentDAL.cs, OrderInfoDAL.cs, SInventorySql.cs; RouteInformationDAL; SeedDataDAL (route dropdown) |
| DAO | Library.DAO/SInventory_Entities/CustPayment.cs, CustPaymentDetail.cs |
| SQL scripts in repo | Live_Deployment_Script_All_Updates.sql (SP bodies); SP_Fix_CustomerPayment_Live.sql (tblCustomerPay.Remarks column); SP_Create_sp_Get_MoneyReceiptReportAfterPaymentList.sql; SP_Audit_Sales_Accounts_Voucher_Posting.sql |

## 7. UI Control Inventory
Controls visible to the user (others exist in `visible=False` blocks and are listed as "hidden").
| Control ID | Label | Required | Default | Read-only / visibility (code) | Data source | Error message |
|---|---|---|---|---|---|---|
| salesCenterDropDownList | Sales Center | Yes (`*`); server check `Validation()` :285 and `LoadGridView` :819 | first real item (`SelectedIndex=1`, :114) or `Session["SalesCenter"]` (:35) | AutoPostBack; items = depots mapped to user (`tblUserCompanyUnit`, `CustPaymentDAL.LoadSC:150`) | tblCompanyUnit ⨝ tblUserCompanyUnit | "Please select Sales Center!!" / "Please select Sales Center!" |
| txtCustomerSearch | Customer | Yes (name or code, :292) | empty | AutoPostBack; AutoCompleteExtender → `SInventoryWebService.GetCustomerPaymentSearch` (min prefix 1, top 10, active customers only) | tblCustMaster | "Please select Customer!!" |
| hfCustomerSearchId | (hidden) | – | – | set from text `Name : Code \| Id` (split on `\|`, :164-168); cleared if no `\|` | – | – |
| submitButton | Search | – | – | loads due invoices | – | – |
| Reset link | Reset | – | – | `Response.Redirect("CustomerPayment.aspx")` (:730) | – | – |
| paymentDateTextBox (+ CalendarExtender cePaymentDate) | Payment Date * | Yes (:299) | today `dd-MMM-yyyy` (:27) | editable, any date | – | "Please select Payment Date!!" |
| checkNoTextBox | Cheque No | No | empty | – | – | – |
| checkDateTextBox (+ceCheckDate) | Cheque Date | No | empty | – | – | – |
| remarksTextBox | Remarks | No | empty | – | – | – |
| txtDisburseAmount (+ FilteredTextBoxExtender `0123456789.`) | Collection / Disburse Amount | No | empty | `oninput/onkeyup` JS `disbursePaymentAmount()` allocates top-to-bottom | – | – |
| lblCount | Total Pay Amount | – | "Total Pay Amount : 0" | Label updated by `CalculateTotal()` / JS | – | – |
| orderGridView (DataKeys `InvoiceId,CompanyId,PaymentTypeName`) | Due Invoice List | – | – | bound from `sp_GET_PaymentInvSP`, sorted `InvoiceDate ASC, InvoiceId ASC` | see §16 | – |
| – chkSelectAll (header) | select all | – | unchecked | AutoPostBack; fills every row with Due/0.00/0.00 (:208-281) | – | – |
| – chkSelect | row check | – | unchecked | AutoPostBack (:898) | – | – |
| – hfCustomerMasterId / hfMarketId / hfDistributionRouteId / hfInvoiceId / hfTP_Pay / hfVat_Pay | hidden row data | – | from SP columns | hfTP_Pay/hfVat_Pay overwritten by `PayAmountChange` | – | – |
| – BoundField CustomerCode, CustomerName, InvoiceNo, InvoiceDate | Customer ID / Customer Name / Invoice No / Invoice Date | – | – | read-only (TerritoryName_Ord column Visible=False) | – | – |
| – lblTotalDelivery | Confirm Amount | – | `TotalDelivery` (= Σ PaymentNetAmount + DeliveryCharge) | label | – | – |
| – lblPaymentAmount | Previous Pay | – | `PaymentAmount` (= Σ(pay+discount+AIT) of earlier payments) | label | – | – |
| – lblDue | Due Amount | – | `Due` | label | – | – |
| – payAmountTextBox (+ FilteredTextBox `0123456789.`) | Pay Amount | Yes when row checked (:326) | `Due` | AutoPostBack; text | – | "Please fill out Pay Amount !!" |
| – discountTextBox | Discount | No | "0.00" | AutoPostBack | – | – |
| – aitTextBox | AIT / VAT | No | "0.00" | AutoPostBack (stored in `tblCustPayDetail.AIT`; label says "AIT / VAT" but column is AIT — see §30) | – | – |
| – isBank / ddlBank | Is Bank / Bank Account | one of Bank/Cash required per checked row (:337-344) | auto by invoice `PaymentTypeName` | ddlBank filled by `LoadBankAccount(CompanyId)` when isBank checked | CSTL_ACCDB_Unicorn..tblBankAccount ⨝ tblBankInformation ⨝ tblBankAccountUnitdtl | "Please select Bank or Cash account for checked invoice at row N." |
| – isCash / ddlCash | Is Cash / Cash Account | as above | as above | ddlCash filled by `LoadCashAccount(CompanyId)`, first item auto-selected | CSTL_ACCDB_Unicorn..tblCashAccount ⨝ tblMultipleCompanySameCashAccount | same |
| – ddlCollectionBy | Collection By | defaulted | DIC / MIO / X. MIO by role (:1077-1120) | editable | static list | – |
| – chkAdjust, AdjustableAmount | (hidden, Visible=false) | – | – | dead: IsAdjust never persisted (`SaveCustDetail` omits it) | – | – |
| masterButton | ALL Submit | – | – | `OnClientClick=sweetAlertConfirm_Submit(this)` then `saveButton_Click` | – | – |
| moneyModal (btnMoneyReceiptPrint / btnSavePayment) | "Print Money Receipt" / "Close (Don't Print)" | – | – | shown after successful save (`$('#moneyModal').modal('show')` :536) | – | – |
| Hidden (visible=False block): rootDropDownList, ddlTerritoryName, ddlDAName, marketDropDownList, customerDropDownList, refDtTextBox, ImageButton0 | Route / Territory / DA Name / Market / Customer | – | – | server-side only; `ddlDAName.SelectedValue` is still read at save (:475) | SPs in §16 | – |
| Hidden by CSS `display:none`: payTypeDDL, refNameTextBox | Payment Type / Reference No | – | payTypeDDL.SelectedIndex=1 | still posted; `PayType = payTypeDDL.SelectedItem.Text` (:413) | tblPaymentType | – |
| hfCustPayId | (hidden) | – | comma list of new CustPayIds | used to filter the money-receipt (:533, :750) | – | – |

## 8. Field Mapping
| UI control | DTO field | Handler / DAL | SQL param | Table.column |
|---|---|---|---|---|
| salesCenterDropDownList | CustPayment.ComUnitId | :400 | @ComUnitId | tblCustomerPay.ComUnitId |
| row hfCustomerMasterId | CustPayment.CustomerMasterId | :397 | @CustomerMasterId | tblCustomerPay.CustomerMasterId |
| row hfMarketId | CustPayment.MarketId | :398 | @MarketId (DBNull if null; 0 stays 0) | tblCustomerPay.MarketId |
| row hfDistributionRouteId | CustPayment.DistributionRouteId | :399 | **not persisted** (SaveCustPayment has no route column) | – |
| paymentDateTextBox | CustPayment.PaymentDate | :402-410 (TryParse, else today) | @PaymentDate | tblCustomerPay.PaymentDate; also copied to tblCustPayDetail.custPaymentDate by `SaveCustDetail` (`@PDate` subquery) |
| row payAmountTextBox | CustPayment.PaymentAmount | :412 | @PaymentAmount | tblCustomerPay.PaymentAmount (row pay only, excludes discount/AIT) |
| payTypeDDL (text) | CustPayment.PayType | :413 | @PayType | tblCustomerPay.PayType |
| checkNoTextBox else refNameTextBox | CustPayment.RefNo | :415-417 | @RefNo | tblCustomerPay.RefNo |
| checkDateTextBox else refDtTextBox | CustPayment.RefDate | :419-430 | @RefDate | tblCustomerPay.RefDate |
| remarksTextBox | CustPayment.Remarks | :432 | @Remarks | tblCustomerPay.Remarks (column added by `SP_Fix_CustomerPayment_Live.sql`, NVARCHAR(500) NULL) |
| Session["LoginName"] / DateTime.Now | CreateBy / CreateDate | :440-441 | @CreateBy, @CreateDate | tblCustomerPay.CreateBy, CreateDate |
| row DataKeys["InvoiceId"] | CustPaymentDetail.InvoiceId | :467 | @InvoiceId | tblCustPayDetail.InvoiceId |
| row payAmountTextBox | CustPaymentDetail.PaymentAmount | :468 | @PaymentAmount | tblCustPayDetail.PaymentAmount |
| row hfTP_Pay | CustPaymentDetail.TPAmount | :446,469 | @TPAmount | tblCustPayDetail.TPAmount |
| row hfVat_Pay | CustPaymentDetail.VATAmount | :447,470 | @VATAmount | tblCustPayDetail.VATAmount |
| row aitTextBox | CustPaymentDetail.AIT | :450,471 | @AIT | tblCustPayDetail.AIT |
| row discountTextBox | CustPaymentDetail.Discount | :452,472 | @Discount | tblCustPayDetail.Discount |
| row chkAdjust | CustPaymentDetail.IsAdjust | :473 | not in INSERT | – (dead) |
| ddlDAName | CustPaymentDetail.DANameId | :475 (0 if empty; ddl is hidden/unloaded → typically 0) | @DANameId | tblCustPayDetail.DANameId |
| row ddlCollectionBy | CustPaymentDetail.CollectionBy | :479-480 (default "DIC") | @CollectionBy | tblCustPayDetail.CollectionBy |
| row ddlBank (if isBank) | CustPaymentDetail.BankAccId | :487-500 | @BankAccId | tblCustPayDetail.BankAccId (0 when not bank) |
| row ddlCash (if isCash) | CustPaymentDetail.CashAccId | :495-501 | @CashAccId | tblCustPayDetail.CashAccId (0 when not cash) |
| PK generation | CustPayId / CustPayDetailId | `CustPaymentBLL.SaveCustPayment:38`, `SaveCustDetail:133` → `ClsPrimaryKeyFind.PrimaryKeyMax` = `SELECT ISNULL(MAX(col),0)+1` | – | tblCustomerPay.CustPayId, tblCustPayDetail.CustPayDetailId (no identity/sequence, race-prone) |

## 9. Dropdown Mapping
| Dropdown | Value / Text | Source | Notes |
|---|---|---|---|
| salesCenterDropDownList | ComUnitId / ComUnitName | `CustPaymentDAL.LoadSC(ddl,userId)` :150 — depots assigned to the user in tblUserCompanyUnit | option 0 = placeholder; page selects index 1 by default |
| rootDropDownList (hidden) | DistributionRouteId / DistributionRouteName | `sp_Get_RouteInfoforCustPayment` (body UNKNOWN) | used only when `Session["Route"]` restored |
| ddlTerritoryName (hidden) | TerritoryId / TerritoryName | `sp_GET_forPaymentTerritoryByRouteInformationDDL` (UNKNOWN) | adds filter `ord.TerritoryId` |
| ddlDAName (hidden) | DANameId / DAName | `sp_GET_RouteInformationDADetailDDL` (UNKNOWN) | |
| payTypeDDL (hidden) | PaymentTypeId / PaymentTypeName | `select * from tblPaymentType` (:182) | index 1 preselected |
| ddlBank (row) | BankAccountId / "Bank (Account - No)" | `LoadBankAccount(companyId)` :351 — only accounts that exist in tblChartOfAccounts and are mapped to the company unit; **companyId concatenated into SQL** | |
| ddlCash (row) | CashAccountId / AccountName | `LoadCashAccount(companyId)` :362 (same concatenation) | |
| ddlCollectionBy (row) | DIC / MIO / X. MIO | static ListItems | default from `GetDefaultCollectionBy()` |

## 10. Button Action Mapping
| Control | Handler | Effect |
|---|---|---|
| submitButton (Search) | `submitButton_Click` :733 → `LoadGridView` :814 | validates depot+customer, stores `Session["SalesCenter"]`, sets `Session["Route"]=0`, `Session["Territory"]=0`, runs `sp_GET_PaymentInvSP(GenerateParam())` |
| chkSelectAll / chkSelect | :208 / :898 | fills Pay=Due, Discount=0.00, AIT=0.00, ticks Cash or Bank (by invoice PaymentTypeName == "Cash"), loads account list, runs `PayAmountChange` |
| payAmountTextBox / discountTextBox / aitTextBox | :565 / :588 / :577 | `PayAmountChange(row)` + `CalculateTotal()` |
| btnDisburse / txtDisburseAmount | JS + `ApplyDisbursement` :978 | allocates amount to checked rows top-to-bottom |
| isBank / isCash | :1164 / :1191 | loads account list; if row not ticked: clears and "You should select row item first !!" |
| masterButton (ALL Submit) | `saveButton_Click` :354 | see §12 |
| btnMoneyReceiptPrint | :550 | opens money receipt viewer (Session["InvoiceIds"]) then reloads grid, clears form |
| btnSavePayment | :558 | reload grid, clear form, hide modal |

## 11. Frontend Flow
1. Page_Load (first hit): payment date = today; if `Session["SalesCenter"]` exists, restores depot/route/territory and immediately runs the due-invoice query (:29-70); otherwise loads depots and selects index 1 (:74, :114).
2. User picks depot; types customer (autocomplete returns `Code : Name | Id`; text-changed splits and stores the id in `hfCustomerSearchId`).
3. Search loads due invoices ordered oldest first.
4. Per row: ticking a row auto-fills pay amount with the full Due (AutoPostBack), chooses Cash vs Bank from the invoice's payment type and loads the account list.
5. Optional bulk allocation: typing an amount in Collection/Disburse Amount runs client-side JS `disbursePaymentAmount()` (`CustomerPayment.aspx:779-827`): walks rows top→bottom; for checked rows writes `min(remaining, due)`, others "0.00"; updates the total label. `oninput`/`onkeyup` triggers it.
6. ALL Submit → SweetAlert confirm → postback → save (§12); success shows Bootstrap modal offering "Print Money Receipt".
7. jQuery: Select2 (`.mySelect2`) and pickadate re-initialised in `pageLoad()`; modal backdrop cleanup on `hidden.bs.modal`.
All inputs sit in nested UpdatePanels (partial postbacks).

## 12. Backend Flow
`saveButton_Click` (:354-547), only if `Validation()` (:283-352) passes:
1. Loop all grid rows (index i); for each **checked** row build a *new* `CustPayment` header.
2. Read previous payment (`lblPaymentAmount`) and confirm amount (`lblTotalDelivery`) from grid labels (client-supplied viewstate, not re-read from DB).
3. `aDal.CheckInvoiceCustpayment(payAmount, invoiceId)` → `sp_Check_Duplicate_InvoiceFinalPayment` (body UNKNOWN). If it returns rows → `save=false` for this row (no write).
4. Else: `_TP_Pay/_Vat_Pay` from hidden fields; `totalSettled = pay + AIT + discount`; if `totalSettled + prevamount == TotalDelivery` → status "Full" else "Partial"; **`UpdateInvoiceFinalPayment(invoiceId, totalSettled, status, LoginName)`** → `sp_Update_InvoiceFinalPayment` (UNKNOWN body) is executed *before* any payment row is saved; return value stored in `save`.
5. Build a `CustPaymentDetail` (pay, TP, VAT, AIT, discount, DA, collectionBy, bank/cash id) and **append to the shared list `aCustPaymentDetails`** (declared once outside the loop, :362).
6. If `save`: `CustPaymentBLL.SaveCustPayment(header, aCustPaymentDetails)` (:32-54): new CustPayId = MAX+1; `INSERT tblCustomerPay`; then for *every* detail in the accumulated list: CustPayId := this header, new CustPayDetailId = MAX+1, `SaveCustDetail` which inserts only if no row exists with same InvoiceId + same payment date + same PaymentAmount (`CustPaymentDAL.cs:105-118`).
7. If saved: record `CustPayId` in `custPayIdList`, then for every detail in the list call `PaymentJournalPosting(custPayId, detail.InvoiceId)` → `sp_Sales_CustomerPaymentPosting` (accounts voucher). The boolean result is **ignored**.
8. After the loop: `save` (value from the *last processed checked row*) true → `hfCustPayId = join(ids)`; modal shown. false → alert "Already Exist!".
9. Money receipt: `btnMoneyReceiptPrint_Click` → `MoneyRecitPrint()`; builds an SQL fragment (`AND INV.InvoiceId IN (...)` + `AND custPay.CustPayId IN (hfCustPayId)` + `AND CONVERT(date, custPay.custPaymentDate)=CONVERT(date,GETDATE())`, :741-788) into `Session["InvoiceIds"]` and opens the PDF viewer.
Transactions: none. try/catch: none in save path (exceptions from BLL/DAL propagate; `UpdateAction` swallows SP errors). Logging: none.

Consequence of steps 5–7 (CONFIRMED by reading): for the k-th checked row, header k receives details 0..k; details 0..k-1 are silently skipped by the duplicate guard (same date/amount), and `PaymentJournalPosting` is re-invoked for earlier invoices with the *new* CustPayId (SP filters by both ids so posts nothing) but still re-evaluates and may re-execute `sp_Sales_DeliveryCOGSPosting` for earlier fully paid invoices (BR-M09-016, GAP-M09-004).

## 13. API Mapping
| Endpoint | Type | Purpose | Auth |
|---|---|---|---|
| `/SInventory_UI/SInventoryWebService.asmx/GetCustomerPaymentSearch` | ASMX ScriptMethod (session enabled) | customer autocomplete: `TOP 10 CustomerCode : CustomerName \| CustomerMasterId` where IsActive=1 and name/code LIKE `%prefix%` (parameterised) | none in method |
No other API.

## 14. Service Mapping
`CustPaymentBLL`: UpdateInvoiceFinalPayment, PaymentJournalPosting, SaveCustPayment(list), LoadSC, PaymentTypeLoadBLL, LoadBankAccount, LoadCashAccount, LoadCustomerMaster. `OrderInfoBLL.LoadSC` is not used by this page. `OrderInfoDAL` used directly from code-behind (bypasses BLL): LoadPaymentInvSP, LoadPaymentInvSPTPVATAmt, CheckInvoiceCustpayment. `SeedDataDAL.GetRouteInfoforCustPayment`, `RouteInformationDAL.GeteRouteInformationDA_DDLId / GetforPaymentTerritoryByRouteInformationDA_DDLId`.

## 15. Repository Mapping
| DAL method | SQL |
|---|---|
| CustPaymentDAL.SaveCustPayment | inline INSERT tblCustomerPay (12 columns) via `SInventorySql.Execute` → `UpdateDataByUpdateCommandNew` (`db.ExecuteNonQuery>0`) |
| CustPaymentDAL.SaveCustDetail | inline batch: `@PDate` from header, duplicate count, conditional INSERT tblCustPayDetail (13 columns) |
| CustPaymentDAL.UpdateInvoiceFinalPayment | SP `sp_Update_InvoiceFinalPayment(@InvoiceId,@PaymentAmount,@ptStatus,@FinalPaymentBy)` |
| CustPaymentDAL.PaymentJournalPosting | SP `sp_Sales_CustomerPaymentPosting(@CustPayId,@InvoiceId)` via `ClsCommonInternalDAL.UpdateAction` (catches all exceptions → false) |
| OrderInfoDAL.LoadPaymentInvSP / LoadPaymentInvSPTPVATAmt / CheckInvoiceCustpayment | SP `sp_GET_PaymentInvSP(@param)`, `sp_GET_PaymentInvSPTPVATAmt(@InvoiceId,@PayAmount)`, `sp_Check_Duplicate_InvoiceFinalPayment(@PaymentAmount,@InvoiceId)` |
| CustPaymentDAL.LoadBankAccount / LoadCashAccount | inline SELECT on CSTL_ACCDB_Unicorn (companyId string-concatenated) |

## 16. Stored Procedure Mapping
| SP | Body in repo? | Read/Write | What is known |
|---|---|---|---|
| sp_GET_PaymentInvSP | YES — Live_Deployment_Script_All_Updates.sql:212-289 | read (dynamic SQL, `@param` appended) | due invoice list; formulas in BR-M09-001/002 |
| sp_GET_PaymentInvSPTPVATAmt | YES — :296-333 | read | returns TP_Pay / Vat_Pay remaining for an invoice (BR-M09-006) |
| sp_Sales_CustomerPaymentPosting | YES — :403-551 | write (cross-DB) | voucher posting (BR-M09-012..017) |
| sp_Update_InvoiceFinalPayment | NO | write | UNKNOWN (presumably updates tblInvoice.PaymentStatus/PaymentAmount/FinalPaymentBy — INFERRED from parameter names only) |
| sp_Check_Duplicate_InvoiceFinalPayment | NO | read | UNKNOWN (returns rows ⇒ save blocked) |
| sp_Sales_DeliveryCOGSPosting | NO (called from posting SP) | write | UNKNOWN |
| sp_Get_RouteInfoforCustPayment, sp_GET_RouteInformationDADetailDDL, sp_GET_forPaymentTerritoryByRouteInformationDDL | NO | read | dropdown feeders |
| sp_Get_MoneyReceiptReportAfterPaymentList | YES — SP_Create_sp_Get_MoneyReceiptReportAfterPaymentList.sql | read | receipt query (see RPT-M09-01) |

## 17. Database Mapping
| Table (DB) | Access | Columns / notes |
|---|---|---|
| tblCustomerPay (SalesDisDB) | INSERT | CustPayId, MarketId, CustomerMasterId, ComUnitId, PaymentDate, PaymentAmount, PayType, RefNo, RefDate, Remarks, CreateBy, CreateDate |
| tblCustPayDetail | INSERT / SELECT | CustPayDetailId, InvoiceId, PaymentAmount, CustPayId, TPAmount, VATAmount, CollectionBy, DANameId, custPaymentDate, BankAccId, CashAccId, AIT, Discount |
| tblInvoice | SELECT (list SP), UPDATE (sp_Update_InvoiceFinalPayment, columns UNKNOWN) | InvoiceId, InvoiceNo, InvoiceDate, DeliveryCharge, PaymentInvoiceNo, PaymentDate, SndReturnInvoiceNo, PaymentTypeId, ComUnitId, OrderId, DeliveryTpGrandTotal |
| tblInvoiceDetail | SELECT | PaymentNetAmount, PaymentTotalPriceVatAmount, PaymentTotalPrice |
| tblReturnInvoice | SELECT | TPGrandTotal (shown as AdjustableAmount only) |
| tblOrder, tblCustMaster, tblCompanyUnit, tblPaymentType, tblUserCompanyUnit | SELECT | |
| CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster / Detail | INSERT (via SP) | see BR-M09-012 |
| CSTL_ACCDB_Unicorn..tblSubsidiaryInfo, tblReferenceInfo, tblFinancialYear, tblChartOfAccounts, tblBankAccount, tblBankInformation, tblBankAccountUnitdtl, tblCashAccount, tblMultipleCompanySameCashAccount | SELECT | |
| CSTL_ERPDB_Unicorn..tblCompanyInfo, tbl_AccIntegrationTable, Table_Integration_AccountsConnectivity | SELECT | GL mapping |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| Sales Center | required, not "0" | no | `Validation()` :285 | – | "Please select Sales Center!!" | BR-M09-020 |
| Customer | hidden id or text required | no | :292 | – | "Please select Customer!!" | BR-M09-020 |
| Payment Date | non-blank (format not validated here; TryParse falls back to today :403-410) | no | :299 | – | "Please select Payment Date!!" | BR-M09-019 |
| Invoice rows | ≥1 checked | no | :305-318 | – | "Please Select Invoice from List!!" | BR-M09-003 |
| Pay Amount | not empty for checked rows (0, ".", "1.2.3" NOT rejected) | FilteredTextBox chars only | :326 | – | "Please fill out Pay Amount !!" | BR-M09-004 |
| Bank/Cash | exactly one account selected per checked row (bank flag+account ≠ ""/"0" or cash flag+account ≠ ""/"0") | no | :337-344 | – | "Please select Bank or Cash account for checked invoice at row N." | BR-M09-008 |
| Over-payment | pay+discount+AIT+previous ≤ Confirm Amount | – | **only in `PayAmountChange` (:674-682) on UI events; NOT re-checked in `Validation()`/save** | sp_Check_Duplicate… UNKNOWN | "Cannot Be Greater then Invoice Quantity " (sic) | BR-M09-004 |
| Duplicate | same invoice + amount already paid | – | `CheckInvoiceCustpayment` :435; DAL guard same invoice/date/amount | – | "Already Exist!" | BR-M09-010 |
| Disburse amount | numeric ≥ 0 else ignored | FilteredTextBox | :981 | – | – | BR-M09-005 |
| Row not ticked when Bank/Cash ticked | – | – | :1187 | – | "You should select row item first !!" | BR-M09-008 |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M09-001 | An invoice appears in the collection list only if it has a payment/return confirmation (`PaymentInvoiceNo IS NOT NULL`), is not a second-time-return invoice (`SndReturnInvoiceNo IS NULL`), has invoice-detail rows, and Due > 0, where Due = (Σ tblInvoiceDetail.PaymentNetAmount + INV.DeliveryCharge) − (Σ PaymentAmount + Σ Discount + Σ AIT from tblCustPayDetail). Sorted InvoiceDate ASC. Invoices without an order are shown with order code "Opening". | Live_Deployment_Script_All_Updates.sql:220-286 | sp_GET_PaymentInvSP | CONFIRMED |
| BR-M09-002 | "Confirm Amount" (payable) = Σ PaymentNetAmount + DeliveryCharge (payment-stage amounts, not delivery-stage). Returns registered in tblReturnInvoice are shown as AdjustableAmount but **not** deducted from Due. | :237-240 | SP | CONFIRMED |
| BR-M09-003 | Selecting a row auto-fills Pay Amount = Due, Discount = 0.00, AIT = 0.00; at least one row must be selected to save. | CustomerPayment.aspx.cs:918-932, :314 | chkSelect_CheckedChanged, Validation | CONFIRMED |
| BR-M09-004 | A row's payment + discount + AIT + previously settled amount must not exceed the Confirm Amount; on breach the row's pay/discount/AIT are reset to 0 / 0.00 / 0.00 with a message. Enforced only in postback event handlers (not on final submit). | :612,:674-682 | PayAmountChange | CONFIRMED |
| BR-M09-005 | Bulk "Disburse" amount is allocated to checked rows in grid order (oldest invoice first): each row gets min(remaining, Due); rows after exhaustion get 0.00; unchecked rows are set to 0.00. Any excess over total Due is silently dropped — there is **no advance / on-account / overpayment** handling on this page. | :978-1026 and aspx JS :779-827 | ApplyDisbursement / disbursePaymentAmount | CONFIRMED |
| BR-M09-006 | TP/VAT split of a settlement `main = pay+discount+AIT`, using remaining VAT `vatPay = Σ PaymentTotalPriceVatAmount − Σ VATAmount(prior)` and remaining TP `tpPay = (ConfirmAmount − Σ VAT) − Σ TPAmount(prior)`: if vatPay>0 → VAT first: if vatPay−main>0 then VAT=main,TP=0 else VAT=vatPay,TP=main−vatPay; if vatPay==0 → TP=main (VAT=0); if vatPay<0 no assignment (hidden fields keep the list-SP values). Result stored in tblCustPayDetail.TPAmount/VATAmount. Invoices with no tblOrder row return no TPVAT row (INNER JOIN) so the list-SP seed (TP_Pay, and Vat_Pay = −Σ prior VAT) is stored instead. | :622-664; SQL :296-333, :223-224 | PayAmountChange, sp_GET_PaymentInvSPTPVATAmt | CONFIRMED |
| BR-M09-007 | Payment status sent to `sp_Update_InvoiceFinalPayment` is "Full" if (pay+AIT+discount + previous) == Confirm Amount (exact decimal equality), otherwise "Partial". The amount passed is this settlement only (`totalSettled`), not the cumulative total. What the SP stores is UNKNOWN. | :453-464 | saveButton_Click | CONFIRMED (call) / UNKNOWN (effect) |
| BR-M09-008 | Every ticked row needs a cash or bank GL account. Default: if the invoice's payment type name (trimmed) equals "Cash" → Cash ticked & cash accounts loaded; otherwise Bank ticked & bank accounts loaded. Accounts offered are those mapped in the accounts DB to the invoice's company (`CompanyId` DataKey). Bank ID stored in BankAccId, cash in CashAccId (0 when unused). | :242-255, :337-344, :1218-1253; CustPaymentDAL.cs:351-371 | Validation, SetBankOrCashAccount | CONFIRMED |
| BR-M09-009 | "Collection By" default: `X. MIO` if role type name / designation / user type equals "X. MIO"; else `MIO` if RoleTypeId=1 or any of role/designation/user type contains "MIO"; else `DIC` if RoleTypeId ∈ {8,13} or contains "DIC"; else DA-name text if it contains "MIO"; else `DIC`. Saved default "DIC" when the dropdown value is empty. | :1077-1120, :480 | GetDefaultCollectionBy | CONFIRMED |
| BR-M09-010 | Duplicate protection: (a) `sp_Check_Duplicate_InvoiceFinalPayment(amount, invoiceId)` returning rows blocks the row (UNKNOWN criteria); (b) `SaveCustDetail` skips insert when a detail already exists for the same invoice, same calendar date and same PaymentAmount. Two legitimate equal-amount instalments on the same day for one invoice therefore cannot both be stored. | :435-437; CustPaymentDAL.cs:105-118 | saveButton_Click, SaveCustDetail | CONFIRMED (b) / UNKNOWN (a) |
| BR-M09-011 | One tblCustomerPay header is created per ticked invoice row (header.PaymentAmount = that row's pay excluding discount/AIT); RefNo = cheque no, else the (hidden, empty) reference field; RefDate = cheque date, else hidden ref date; PayType = selected payment-type text; CreateBy = Session LoginName. | :395-432 | saveButton_Click | CONFIRMED |
| BR-M09-012 | For each payment-detail with (PaymentAmount + Discount + AIT) > 0, an *approved* voucher is auto-created in CSTL_ACCDB_Unicorn: master (VoucherCategoryID=2, currency 1 @1.0, Remarks "Auto Posting", EntryBy "admin", ApprovedBy "Auto Process", ActionStatus "Approved", TotalAmount = pay+discount+AIT, CommonPK_ID = CustPayId, TableID = tbl_AccIntegrationTable.IntegrationTableId for 'tblCustomerPay' — audit script assumes 2017, narration "An amount of BDT x … has been received from customer "Name (Code)" against Invoice No. "…" on dd-mm-yyyy."). Detail lines: Dr Cash/Bank GL = pay; Dr Discount GL = discount (if>0); Dr AIT GL = AIT (if>0); Cr Accounts-Receivable GL = pay+AIT+discount with customer subsidiary ledger id. Balanced by construction. | Live_Deployment_Script_All_Updates.sql:462-522 | sp_Sales_CustomerPaymentPosting | CONFIRMED |
| BR-M09-013 | Voucher type/code: Bank → `CreditVoucherBank`, code prefix `CVB-`; Cash → `CreditVoucherCash`, prefix `CVC-`; code = prefix + yyMMdd + (MAX(SUBSTRING(VoucherCode,11,6)) or 10000) + 1 over all vouchers with the same PostingDate. If neither bank nor cash id is set, VoucherType becomes NULL (the CASE has no ELSE, overwriting the 'Journal'/'JVP-' default) and the code has no prefix. | :453-460, :445-451 | SP | CONFIRMED |
| BR-M09-014 | GL resolution: bank GL = tblChartOfAccounts.AccountId where BankAccountId = @BankId; cash GL where CashAccountId = @CashId; discount GL = Table_Integration_AccountsConnectivity(PageId=163, FlagId=1023); AIT GL = (163, 1024); receivable GL = (163, 2). Customer subsidiary = tblSubsidiaryInfo(SubsidiaryTypeId=1, S_Info_Id=CustomerMasterId); reference = tblReferenceInfo(ReferenceTypeId=1016, R_Info_Id=InvoiceId). | :420-430, :486-522 | SP | CONFIRMED |
| BR-M09-015 | Financial year = the tblFinancialYear row of the invoice's company whose Start/End contains the payment date (INNER JOIN). If the payment date falls in no financial year (or company/unit mapping missing) the cursor is empty and **no voucher is posted, silently**. | :426-431 | SP | CONFIRMED |
| BR-M09-016 | COGS: after posting, DueAmount = (INV.DeliveryTpGrandTotal + DeliveryCharge) − Σ(PaymentAmount+Discount+AIT); if ≤ 0, `sp_Sales_DeliveryCOGSPosting @InvoiceId` is executed (body UNKNOWN). Note this uses delivery-stage totals whereas the collection list uses payment-stage totals (BR-M09-002), so an invoice reduced at payment confirmation may show Due=0 in the UI but Due>0 here (COGS not posted). | :532-549 | SP | CONFIRMED |
| BR-M09-017 | Posting is not atomic with the payment: `PaymentJournalPosting` is called after commit of the payment rows; `UpdateAction` catches every exception and returns false, and the page ignores the result. Failure ⇒ payment saved with no voucher and no message. | :520; ClsCommonInternalDAL.cs:450-464 | saveButton_Click | CONFIRMED |
| BR-M09-018 | Money-receipt no. (report SP): `masPay.RefNo` when non-empty else `MR-` + 8-digit zero-padded CustPayId; the receipt lists payment amount, TP and VAT amounts per detail. | SP_Create_sp_Get_MoneyReceiptReportAfterPaymentList.sql | sp_Get_MoneyReceiptReportAfterPaymentList | CONFIRMED |
| BR-M09-019 | Payment date is user-editable (any date, incl. back/future), defaults to today; it decides the accounting financial year and the voucher PostingDate and is copied to tblCustPayDetail.custPaymentDate. | :27, :402-410 | Page_Load, saveButton_Click | CONFIRMED |
| BR-M09-020 | A depot and a customer (or customer text) are mandatory to search and to save. Search filters: `ord.ComUnitId`, `ord.CustomerMasterId` (or code/name LIKE), optional territory/route (restored from session). | :851-892 | GenerateParam | CONFIRMED |

## 20. Permission Matrix
- Session keys read: UserId (depot list, :32,:111), LoginName (CreateBy, FinalPaymentBy), RoleTypeName / RoleTypeId / UserType / DesigName (Collection-By default only), SalesCenter/Route/Territory (page state).
- Authentication: Forms auth; `NewMasterPage.master.cs:24-59` redirects to `../Login.aspx` when `Session["UserId"]` is missing.
- Authorization: **menu-based only**. No role check, no depot ownership check on the customer/invoice ids posted back (hidden fields), no limit on discount/AIT. Depot list is restricted to `tblUserCompanyUnit` rows of the user, but `salesCenterDropDownList.SelectedValue` is trusted at save (:400).

## 21. Status Lifecycle
- Invoice payment status: values "Full"/"Partial" are computed here (BR-M09-007) and passed to `sp_Update_InvoiceFinalPayment`; the column(s) written are UNKNOWN. Other code reads `tblInvoice.PaymentStatus IS NULL OR = 'Partial'` as "collectable" (`sp_GET_PaymentInvSPPaymentAmount`, `CustPaymentDAL.LoadInvoice`), and `PaymentReverse` resets `PaymentStatus`/`PaymentAmount` to NULL.
- Payment header/detail rows have no status column (no draft/approval): a saved row is final until reversed/deleted by PAGE-335/PAGE-313.
- Accounting voucher is created directly `ActionStatus='Approved'`.

## 22. Error Handling
- `showMessageBox()` → `alert()` (note `message.Replace("'", "\'")` is a no-op in C#; quotes in messages would break the script).
- Save path has no try/catch: DB exceptions surface as ASP.NET error page; partially-executed steps stay committed (§12).
- `DropDownList()`, route/territory loaders and `txtCustomerSearch_TextChanged` swallow exceptions (empty catch).
- Global.asax `Application_Error` empty (no central log).

## 23. Audit
tblCustomerPay.CreateBy / CreateDate; sp_Update_InvoiceFinalPayment receives `@FinalPaymentBy` (login). Voucher master EntryBy is hard-coded `'admin'`, ApprovedBy `'Auto Process'` (no user traceability in accounts). No update/delete audit on this page.

## 24. Integration
Cross-database write into the accounts DB `CSTL_ACCDB_Unicorn` (same SQL Server, three-part names) and reads of the ERP master DB `CSTL_ERPDB_Unicorn` (company, integration mapping). Integration mapping tables: `tbl_AccIntegrationTable` (table id), `Table_Integration_AccountsConnectivity` (PageId 163 = customer payment). Read-only verification script: `SP_Audit_Sales_Accounts_Voucher_Posting.sql` (see WF-M09-06). No SAP call from this page.

## 25. Reports
Money receipt: `../SInventory_RPTVIEW/MoneyReceiptViewerForAfterPayment.aspx` → `rptMoneyReceipt.rpt` (see RPT-M09-01).

## 26. Dependencies
BLL: CustPaymentBLL. DAL: CustPaymentDAL, OrderInfoDAL, SeedDataDAL, RouteInformationDAL, ClsPrimaryKeyFind, ClsCommonInternalDAL. DAO: CustPayment, CustPaymentDetail. Client: AjaxControlToolkit (AutoComplete, Calendar, FilteredTextBox), Select2, pickadate, Bootstrap modal, SweetAlert (`sweetAlertConfirm_Submit`). Cross-DB: CSTL_ACCDB_Unicorn, CSTL_ERPDB_Unicorn.

## 27. Test Cases
Proposed (none exist in repo).
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-311-01 | Search customer with 2 due invoices; tick both | Pay=Due per row, Cash ticked if invoice payment type "Cash" else Bank; account dropdown filled | BR-M09-003, 008 |
| TC-311-02 | Enter Disburse amount less than sum of Due | Oldest invoice filled first, remainder to next, later rows 0.00 | BR-M09-005 |
| TC-311-03 | Enter Disburse amount greater than sum of Due | Each row = Due; excess ignored, no error | BR-M09-005 |
| TC-311-04 | Set Pay + Discount + AIT + Previous > Confirm Amount | Row reset to 0/0.00/0.00 and message | BR-M09-004 |
| TC-311-05 | Pay exactly Due, cash account chosen, save | tblCustomerPay+tblCustPayDetail inserted; status "Full"; voucher `CVC-…` with Dr Cash, Cr AR (Cr = pay) | BR-M09-007, 012, 013 |
| TC-311-06 | Partial pay with discount 100 and AIT 50 | detail Discount=100, AIT=50; voucher Dr Cash pay, Dr Discount GL 100, Dr AIT GL 50, Cr AR pay+150; status "Partial" unless previous+150+pay == Confirm | BR-M09-007, 012 |
| TC-311-07 | Invoice with remaining VAT 200 and Confirm Amount 1000, no prior payment: pay 150, then (separate case) pay 500 | pay 150 → VATAmount=150, TPAmount=0; pay 500 → VATAmount=200, TPAmount=300 | BR-M09-006 |
| TC-311-08 | Tick row but leave bank & cash unselected | "Please select Bank or Cash account … row N." | BR-M09-008 |
| TC-311-09 | Save two rows in one submit | Two headers created; verify each header has exactly one detail and no COGS double posting (regression for shared-list defect) | BR-M09-011, 016 |
| TC-311-10 | Save same invoice+amount twice on the same day | Second blocked ("Already Exist!") | BR-M09-010 |
| TC-311-11 | Payment date outside any accounts financial year | Payment rows saved, no voucher, no message | BR-M09-015, 017 |
| TC-311-12 | Payment with neither bank nor cash id (tamper) | voucher VoucherType NULL, no prefix | BR-M09-013 |
| TC-311-13 | Invoice with no tblOrder (Opening) | shown as "Opening"; TP/VAT seeded from list SP | BR-M09-001, 006 |
| TC-311-14 | Open page with no session | redirected to Login | §20 |
| TC-311-15 | Print Money Receipt after save | PDF with receipt no = cheque no or `MR-0000nnnn` | BR-M09-018 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/CustomerPayment.aspx`, `CustomerPayment.aspx.cs`; `Library.BLL/SInventory_BLL/CustPaymentBLL.cs`; `Library.DAL/SInventory_DAL/CustPaymentDAL.cs`, `OrderInfoDAL.cs:352-474`; `Solution.Web/App_Code/SInventoryWebService.cs:371-393`; `Live_Deployment_Script_All_Updates.sql:212-551`; `SP_Fix_CustomerPayment_Live.sql`; `SP_Audit_Sales_Accounts_Voucher_Posting.sql:148-219` (customer-payment audit block).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Payment allocation | None existed | Per-invoice, full/partial by equality; no advance | tblCustPayDetail per invoice | NO SPEC | BR-M09-005/007 |
| Accounts posting | None existed | Auto approved voucher via cross-DB SP; result ignored | vouchers in CSTL_ACCDB_Unicorn | NO SPEC | BR-M09-012..017 |
| Audit of posting | none | `sp_Audit_Sales_Accounts_Voucher_Posting` checks Dr=Cr and value ±1.00 | – | NO SPEC | GAPS |

## 30. Known Gaps
- **Shared detail list across loop** (`:362`, `:513`): later headers re-receive earlier details; earlier details are only skipped by the date+amount duplicate guard; posting/COGS re-invoked for earlier invoices (GAP-M09-004).
- No DB transaction; `sp_Update_InvoiceFinalPayment` runs before payment insert; posting failure silent (GAP-M09-005).
- Over-payment only checked on UI events, not on submit; `lblPaymentAmount`/`lblTotalDelivery` read from client-controlled viewstate (GAP-M09-006).
- SQL built by string concatenation: `GenerateParam` (customer hidden field/text), `Parm()` uses `hfCustPayId` in the receipt query, `LoadBankAccount/LoadCashAccount` concatenate `companyId`, `sp_GET_PaymentInvSP` executes `@param` dynamically (GAP-M09-007).
- `Session["Route"]=0` / `Session["Territory"]=0` written in `LoadGridView` then `rootDropDownList.SelectedValue = Session["Route"]` on next `Page_Load` (:42,:50) — value "0" is probably not in the list ⇒ likely ArgumentOutOfRangeException on revisit (INFERRED) (GAP-M09-008).
- `PaymentAmount` per header excludes discount/AIT; receipt/ledger totals must add them (GAP-M09-009).
- `IsAdjust`, `DistributionRouteId` collected but never persisted; hidden DA control makes `DANameId` usually 0.
- Label "AIT / VAT" but stored as AIT only.
- Voucher `EntryBy='admin'` hard-coded; voucher code counter not locked (duplicate code race); PK via MAX+1 (race).
- No advance/on-account receipt, no overpayment handling.

## 31. Change Impact
- `CustPaymentBLL.SaveCustPayment/SaveCustDetail` and `CustPaymentDAL.SaveCustDetail` are shared by CustomerPaymentPPPP, CustomerPaymentSnd, CustomerPaymentExcelUpload, CustomerPayment - Copy, MarketWiseCustomerPayment, PaymentPartial (dead call), DelivaryInvoiceCreationAfterSalesConfirm, DelivaryInvoiceCreationForCustomerByOrderAuto(+_Loading), LoadingSummary, SalesReturnList_new, SndTimesSalesReturnPartial (grep of `SaveCustPayment` in Solution.Web). Changing SQL there changes all of them.
- `sp_GET_PaymentInvSP` / `sp_GET_PaymentInvSPTPVATAmt` are shared with CustomerPaymentPPPP/Snd (Snd uses `sp_GET_PaymentInvSPSndReturn`), CustomerPaymentExcelUpload (TPVAT), CustomerPayment - Copy.
- `sp_Sales_CustomerPaymentPosting` is called only from this page (`PaymentJournalPosting` grep: CustomerPayment.aspx.cs only); other collection entry pages do **not** post to accounts.
- `sp_Get_MoneyReceiptReportAfterPaymentList` is shared with MoneyReceiptAfterPayment (PAGE-330) and the viewer PAGE-304.
- tblCustPayDetail is read by aging (PAGE-306), receivable, ledger, deposit and money-receipt reports; new columns/semantics (TPAmount, VATAmount, AIT, Discount) affect all.
