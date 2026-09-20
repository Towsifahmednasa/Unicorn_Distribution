---
page_id: PAGE-314
module: M09
page: CustomerPaymentExcelUpload
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-314 — CustomerPaymentExcelUpload (bulk collection upload by invoice number)

> **Evidence level:** Tier A — full `.aspx.cs` (976 lines), grid markup (lines 232-335), `OrderInfoDAL.LoadPaymentInvSPWithPaymentAmount`, `CustPaymentBLL/DAL`, and the repo body of `sp_GET_PaymentInvSPPaymentAmount` (Live_Deployment_Script_All_Updates.sql:340-396) were read. Bodies of `sp_Update_InvoiceFinalPayment`, `sp_Check_Duplicate_InvoiceFinalPayment`, `sp_Get_DepositCodeByDIC`, `sp_Get_SAP_EmpSApCodebyTerritory`, `sp_I_Customer`, `sp_I_Diposit_New` are NOT in the repo. The page is a copy-paste hybrid of the deposit-slip upload page; only the payment path (`saveButton_Click`) is wired.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-314 |
| Kind | Upload / Import (CONFIRMED) |
| Page class | SInventory_UI_CustomerPaymentExcelUpload |
| Master | ~/MasterPages/NewMasterPage.master |
| Code-behind LOC | 976 |
| Notes | Dead deposit-slip code (`submitButton_Click`, `XLDataGridToDbByRow`, `CheckValidation`) inherited from DepositSlip/customer upload pages; not reachable (submit button commented out at aspx:546). |

## 2. Purpose
Loads an Excel/CSV-like sheet with `InvoiceNo | Amount | CollectionBy` per row, looks each invoice up in the due list with the amount pre-filled, lets the user tick/adjust rows and saves them as customer payments in bulk. CONFIRMED (`ExcelToGrid` :516-626, `saveButton_Click` :661-806).

## 3. Business Objective
Speed up back-office capture of many collections (e.g. daily MIO/DIC collection sheets) without entering invoices one-by-one. INFERRED.

## 4. Navigation
Header button "Excel Upload" on PAGE-311 exists but is hidden (`visible=False`), so entry is only via menu (UNKNOWN path). Success redirects (`successalert(...,'CustomerPaymentExcelUpload.aspx')`, :795). Refresh/Home helper buttons target `CustomerExcelUpload.aspx` / `HomePage.aspx` (unrelated pages).

## 5. Route
`~/SInventory_UI/CustomerPaymentExcelUpload.aspx`.

## 6. Source Files
View/code-behind as above; BLL `CustPaymentBLL`; DAL `CustPaymentDAL`, `OrderInfoDAL` (:29-60 `LoadPaymentInvSPWithPaymentAmount`, :352 check, :445 TP/VAT), `SAP_IntrigationPointDAL`, `CompanywisebranchDal`, `ExcelUpForMIGODAL`, `SeedDataDAL`, `RouteInformationDAL`; DAO CustPayment, CustPaymentDetail, CompanyWiseDepositDao.

## 7. UI Control Inventory
| Control | Label | Required | Default / state | Source |
|---|---|---|---|---|
| salesCenterDropDownList | Sales Center | no server check | index 1 | user depots |
| rootDropDownList | Route | no | placeholder | `sp_Get_RouteInfoforCustPayment` |
| ddlDAName | DA Name | **Yes** (`Validation` :398) "Please Select DA Name!!" | placeholder | route DA SP |
| txtDepositCode (RO), txtTotalAmount | Deposit Code / Total Amount | not used by payment path | code from `sp_Get_DepositCodeByDIC` | deposit residue |
| id_fu, btnUpload | Upload File | required (exception message otherwise) | – | writes file to `~/ExcelFiles/<name>` |
| orderGridView (DataKeys InvoiceId) | | | rows `chkSelect Checked="true"` | columns Customer ID, Customer Name, Order Code, Order/Submission Date, Invoice No, Invoice Date, Confirm Amount, Previous Pay, Due Amount, Pay Amount (Text=PayNewAmount), Collection By (label), hidden Adjust/AdjustableAmount |
| lbl_up_status, lblCount | status / "Total Pay Amount" | – | "N record Found!" | |
| masterButton | ALL Submit | `sweetAlertConfirm_Submit` | – | `saveButton_Click` |
| cancelButton | Reset | – | redirect to self | |

## 8. Field Mapping
| UI / source | DTO | Column |
|---|---|---|
| grid hfCustomerMasterId / hfMarketId / hfComUnitId | CustPayment.CustomerMasterId / MarketId / ComUnitId | tblCustomerPay.* |
| now (`DateTime.Now`, date only) | PaymentDate | tblCustomerPay.PaymentDate (no date column in sheet) |
| Pay Amount textbox | PaymentAmount | tblCustomerPay.PaymentAmount; tblCustPayDetail.PaymentAmount |
| constant "Cash" | PayType | tblCustomerPay.PayType |
| constant "" / now | RefNo / RefDate | tblCustomerPay.RefNo / RefDate |
| hfTP_Pay / hfVat_Pay | TPAmount / VATAmount | tblCustPayDetail.TPAmount / VATAmount |
| ddlDAName | DANameId | tblCustPayDetail.DANameId |
| lblCollectionBy (sheet col 3) | CollectionBy | tblCustPayDetail.CollectionBy |
| – | AIT / Discount / BankAccId / CashAccId | 0 / 0 / 0 / 0 (never set) |
Excel columns: A = Invoice No (rows with empty A skipped), B = amount (formatted F2), C = Collection By.

## 9. Dropdown Mapping
Depot, route, DA as in PAGE-311; no payment-type or account dropdown.

## 10. Button Action Mapping
btnUpload → `ExcelToGrid`; grid check / pay-amount → `PayAmountChange`; ALL Submit → `saveButton_Click` (`Validation()` first); Reset → reload.

## 11. Frontend Flow
Upload → server reads first sheet through OLE DB (Jet 4.0 for .xls, ACE 12.0 for .xlsx), for every row calls `LoadPaymentInvSPWithPaymentAmount(" and  InvoiceNo='<A>'", "<B F2>", "<C>")`, merges results into one table, binds, calls `PayAmountChange(i)` on every row and totals. User reviews and submits.

## 12. Backend Flow
`saveButton_Click`: `Validation()` (§18) → for each ticked row: header (ComUnitId from the *invoice's* company unit hidden field, PayType "Cash", RefNo "", PaymentDate today) → `sp_Check_Duplicate_InvoiceFinalPayment` → status Full/Partial by `(pay + previous) == Confirm Amount` → `UpdateInvoiceFinalPayment(invoiceId, pay, status, login)` → detail added to the shared list → `SaveCustPayment(header, list)` (same accumulating-list behaviour as PAGE-311). **No `PaymentJournalPosting` call → no accounts voucher, no cash/bank account, no discount/AIT.** Success → SweetAlert redirect; else "Already Exist!". No transaction.

## 13. API Mapping
None.

## 14. Service Mapping
CustPaymentBLL.SaveCustPayment / UpdateInvoiceFinalPayment; OrderInfoDAL called directly.

## 15. Repository Mapping
`SaveCustPayment`, `SaveCustDetail` as PAGE-311 §15; `LoadPaymentInvSPWithPaymentAmount` → `sp_GET_PaymentInvSPPaymentAmount(@param,@PaymentAmount,@CollectionBy)`.

## 16. Stored Procedure Mapping
| SP | Body | Notes |
|---|---|---|
| sp_GET_PaymentInvSPPaymentAmount | YES (:340-396) | dynamic SQL: **`@PaymentAmount` and `@CollectionBy` are concatenated unescaped into the SELECT** (`' + @PaymentAmount + ' AS PayNewAmount`, `'''+@CollectionBy+''' AS CollectionBy`); returns invoices with `PaymentInvoiceNo IS NOT NULL AND (PaymentStatus IS NULL OR PaymentStatus='Partial')`; Due = (Σ PaymentNetAmount + DeliveryCharge) − (Σ pay+discount+AIT); TP_Pay and Vat_Pay hard-coded 0 |
| sp_GET_PaymentInvSPTPVATAmt | YES | as PAGE-311 |
| sp_Update_InvoiceFinalPayment, sp_Check_Duplicate_InvoiceFinalPayment | NO | UNKNOWN |
| sp_Get_DepositCodeByDIC, sp_Get_SAP_EmpSApCodebyTerritory, sp_I_Diposit_New, sp_I_Customer | NO | only dead code |

## 17. Database Mapping
tblCustomerPay INSERT, tblCustPayDetail INSERT, tblInvoice UPDATE (via SP), reads as above; server file system `~/ExcelFiles/`.

## 18. Validation Matrix
| Rule | Where | Message | BR |
|---|---|---|---|
| DA Name selected | Validation :398 | "Please Select DA Name!!" | BR-M09-032 |
| ≥1 row ticked | :404-417 | "Please Select Invoice from List!!" | BR-M09-032 |
| Pay Amount not "" and not "0" | :426-436 | "Please fill out Pay Amount !!" | BR-M09-032 |
| Collection By present and ∈ {MIO, DIC} | :438-449 | "Please fill out Collection By MIO or DIC!!" | BR-M09-031 |
| pay + previous ≤ Confirm Amount (server, per ticked row) | :459-495 | "Cannot Be Greater then Confirm Amount!!" | BR-M09-032 |
| Upload errors | catch :622-625 | "Please fill up all required data !!" + exception text | – |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M09-031 | Sheet layout: col A invoice number, col B amount to collect, col C collection channel which must be exactly `MIO` or `DIC` (case-sensitive) for every ticked row. | CustomerPaymentExcelUpload.aspx.cs:573-600, :438-449 | ExcelToGrid, Validation | CONFIRMED |
| BR-M09-032 | Bulk save requires a DA name, ≥1 ticked row, non-zero amounts, and (unlike PAGE-311) re-checks `pay + previous ≤ Confirm Amount` on the server before saving. | :398-495 | Validation | CONFIRMED |
| BR-M09-033 | Only invoices with `PaymentInvoiceNo` stamped and `PaymentStatus IS NULL OR 'Partial'` are matched; an invoice number not in that set contributes no grid row (silently dropped). | Live_Deployment_Script_All_Updates.sql:391-392 | sp_GET_PaymentInvSPPaymentAmount | CONFIRMED |
| BR-M09-034 | Bulk payments are recorded as PayType "Cash", RefNo "", payment date = today, with no bank/cash ledger id, no discount, no AIT, and **without accounts-voucher posting** (no call to `sp_Sales_CustomerPaymentPosting`). Accounting therefore does not see these collections unless posted by another process (UNKNOWN). | :710-717, :753-762 | saveButton_Click | CONFIRMED |

## 20. Permission Matrix
Session: UserId (depot list), LoginName; DICCompanyUnitId/RoleTypeName inside `GetDepositCodeByDIC` (deposit residue). Menu-based only; no role check.

## 21. Status Lifecycle
As PAGE-311 (Full/Partial sent to `sp_Update_InvoiceFinalPayment`); no draft state; upload grid is transient (not persisted).

## 22. Error Handling
Upload wrapped in try/catch with message; save path has none. Deposit-code lookup on Page_Load throws if `Session["RoleTypeName"]` is null (unguarded).

## 23. Audit
CreateBy / CreateDate on tblCustomerPay; the uploaded file is left in `~/ExcelFiles/` under its original name (overwrites; no cleanup).

## 24. Integration
Reads Excel via OLE DB (server needs Jet/ACE drivers). No accounts or SAP posting for payments.

## 25. Reports
None.

## 26. Dependencies
AjaxControlToolkit, OLE DB providers, SweetAlert, tblInvoice/tblCustPayDetail.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-314-01 | Upload sheet with 3 valid invoices, CollectionBy MIO/DIC | 3 rows loaded with sheet amounts | BR-M09-031, 033 |
| TC-314-02 | CollectionBy = "mio" or blank | Blocked on save | BR-M09-031 |
| TC-314-03 | Invoice already fully paid | row absent | BR-M09-033 |
| TC-314-04 | Amount > due | blocked on submit | BR-M09-032 |
| TC-314-05 | Save without DA | "Please Select DA Name!!" | BR-M09-032 |
| TC-314-06 | Save valid | headers/details inserted, PayType Cash, **no voucher** | BR-M09-034 |
| TC-314-07 | Sheet cell containing `' ; --` in CollectionBy | verify injection not possible (currently executes in dynamic SQL) | GAP-M09-007 |
| TC-314-08 | Duplicate upload same day | "Already Exist!" for equal invoice+amount | BR-M09-010 |

## 28. Reverse Engineering References
`Solution.Web/SInventory_UI/CustomerPaymentExcelUpload.aspx(.cs)`, `Library.DAL/SInventory_DAL/OrderInfoDAL.cs:29-60`, `Live_Deployment_Script_All_Updates.sql:340-396`.

## 29. Spec vs Actual
| Area | Specification | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| Accounting parity with manual entry | – | manual page posts vouchers; upload page does not | NO SPEC / DIVERGENCE (GAP-M09-012) |

## 30. Known Gaps
SQL injection through Excel cells (amount and collection-by concatenated into dynamic SQL; invoice number concatenated in C#); no accounts posting; dead deposit code & unrelated redirects; `FilePath` uses client-supplied file name (path traversal / overwrite risk, `id_fu.FileName`); `Convert.ToDateTime(...ToString("dd-MMM-yyyy"))` culture-dependent; `id_fu` not checked for extension before OLE DB open (`oledbConn` null → NRE for other extensions).

## 31. Change Impact
Shares `SaveCustPayment`/`SaveCustDetail`, `sp_GET_PaymentInvSPTPVATAmt` and duplicate SP with PAGE-311/315/318. `sp_GET_PaymentInvSPPaymentAmount` is used only here.
