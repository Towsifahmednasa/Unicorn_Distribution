---
page_id: PAGE-306
module: M09
page: CustomerInvoiceAgingReport
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "COMPLETE", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-306 — CustomerInvoiceAgingReport (Customer invoice aging & outstanding)

> **Evidence level:** Tier A — `.aspx.cs` (287 lines), `CustomerInvoiceAgingBLL/DAL` and the full body of `sp_Report_CustomerInvoiceAgingOutstanding` (Live_Deployment_Script_All_Updates.sql:129-205) were read. The generated Tier-B file wrongly said the SP body is absent. **Important: this report has no aging buckets** (no 0-30/31-60/… columns): it lists each outstanding invoice with `Credit Days` = days between invoice date and the as-on date. Bucketed aging lives in a different page (`Aging.aspx`, see README/RPT-M09-04) whose SP is not in the repo.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-306 |
| Kind | Report — GridView + Excel (no Crystal) |
| Page class | SInventory_UI_CustomerInvoiceAgingReport |
| Master | NewMasterPage.master; created 2026 (script dated with the LIVE updates) |

## 2. Purpose
For an "as on" date, lists every invoice that still has an outstanding balance (invoice amount minus payments recorded up to that date), with customer, invoice date, invoice amount, paid amount, days elapsed and the responsible sales person; totals shown in a footer; exportable to Excel.

## 3. Business Objective
Let finance/sales follow up unpaid invoices by depot, customer and sales person and see how old they are. INFERRED from title/columns.

## 4. Navigation
Menu path UNKNOWN (DB-driven). Reset → self. A user without `Session["UserId"]` is redirected to `~/Login.aspx` (:23-27).

## 5. Route
`~/SInventory_UI/CustomerInvoiceAgingReport.aspx`.

## 6. Source Files
`CustomerInvoiceAgingReport.aspx(.cs)`; `Library.BLL/SInventory_BLL/CustomerInvoiceAgingBLL`; `Library.DAL/SInventory_DAL/CustomerInvoiceAgingDAL.cs`; SP script above.

## 7. UI Control Inventory
| Control | Label | Required | Default / notes |
|---|---|---|---|
| comUnitDropDownList | DC / Depot | no | all depots (`SELECT ComUnitId, ComUnitCode+' - '+ComUnitName FROM tblCompanyUnit ORDER BY ComUnitName`), first item relabelled "--- All DCs / Depots ---"; **not restricted to the user's depots**; change → reloads customer list |
| asOnDateTextBox | As On Date | **Yes** ("Please select an As On Date.") | today `dd MMMM, yyyy` |
| customerDropDownList | Customer | no | active customers (`IsActive=1`, optionally by ComUnitId) "CustomerCode - CustomerName"; relabelled "--- All Customers ---" |
| fromDateTextBox / toDateTextBox | From / To Date (optional) | no | filter on invoice date |
| salesPersonDropDownList | Sales Person | no | distinct employees (`EmployeeStatus='Active' OR NULL`) who are `tblCustMaster.SalesPersonId`; "--- All Sales Persons ---" |
| searchButton / resetButton / exportButton | Search / Reset / Export | – | |
| loadGridView | Result | – | SL, Customer Name, Invoice No, Invoice Date, Invoice Amount, Paid Amount, Credit Days, Outstanding, Responsible Person; footer "Grand Total" for Invoice, Paid, Outstanding |

## 8. Field Mapping
Page → `GetCustomerInvoiceAging(asOn, from, to, comUnitId, customerMasterId, salesPersonId, onlyDue=false)` → SP params `@AsOnDate,@FromDate,@ToDate,@ComUnitId,@CustomerMasterId,@SalesPersonId,@OnlyDueInvoices` (null when blank). Dates parsed with formats `dd MMMM, yyyy`, `dd MMMM yyyy`, `yyyy-MM-dd`, `dd/MM/yyyy`, `MM/dd/yyyy`, `d-MMM-yyyy`, `dd-MMM-yyyy`, then `DateTime.TryParse`.

## 9. Dropdown Mapping
See §7 (queries in `CustomerInvoiceAgingDAL.cs:30-67`).

## 10. Button Action Mapping
Search → `LoadData`; Export → `exportButton_Click` writes `Customer_Invoice_Aging_Report_<yyyyMMdd_HHmmss>.xls` (HTML table) with title "Customer Invoice Aging & Outstanding Report" and "As On Date"; Reset → reload.

## 11. Frontend Flow
Filters → Search → grid + footer totals accumulated in `loadGridView_RowDataBound` from the formatted cell text (columns 4, 5, 7), footer row uses `#,##0.00`.

## 12. Backend Flow
`LoadData` (:105-151): validate As On Date; parse; `onlyDue` hard-coded `false`; call SP; bind. Errors → `alert('Error retrieving report: …')`. Export uses `Response.End()` (ThreadAbort caught).

## 13. API Mapping
None.

## 14. Service Mapping
`CustomerInvoiceAgingBLL` → `CustomerInvoiceAgingDAL.GetCustomerInvoiceAging` → `ClsCommonInternalDAL.GetDataTableAction("sp_Report_CustomerInvoiceAgingOutstanding", params, "SSIDB")` (timeout 99,000,000 s).

## 15. Repository Mapping
See §14; dropdown queries inline (parameterised).

## 16. Stored Procedure Mapping
`sp_Report_CustomerInvoiceAgingOutstanding` — body IN repo (Live_Deployment_Script_All_Updates.sql:129-205):
- `@AsOnDate` defaults to today if NULL.
- CTE_InvoiceSummary: Σ `tblInvoiceDetail.DeliveryNetAmount` per invoice.
- CTE_PaymentSummary: per invoice Σ(PaymentAmount + Discount + AIT) from `tblCustPayDetail` where `CAST(ISNULL(cpd.custPaymentDate, cp.PaymentDate) AS DATE) <= @AsOnDate`.
- Base: `tblInvoice ⨝ tblCustMaster` (inner) ⟕ summaries ⟕ `tblEmpGeneralInfo` on `cm.SalesPersonId`; filters `InvoiceDate <= @AsOnDate`, optional `>= @FromDate`, `<= @ToDate`, `ComUnitId`, `CustomerMasterId`, `cm.SalesPersonId` (NULL or 0 = all).
- Columns: Invoice Amount = Σ DeliveryNetAmount + DeliveryCharge; Paid = payments (above); **Credit Days = DATEDIFF(DAY, InvoiceDate, @AsOnDate)**; Outstanding = Invoice Amount − Paid; Responsible Person = EmpName or 'N/A'.
- Result: rows with Outstanding > 0.00, `ROW_NUMBER()` SL ordered by InvoiceDate ASC, InvoiceNo ASC.
- `@OnlyDueInvoices` is declared but **never used**.

## 17. Database Mapping
tblInvoice, tblInvoiceDetail (DeliveryNetAmount), tblCustPayDetail, tblCustomerPay (PaymentDate fallback), tblCustMaster, tblEmpGeneralInfo — all SELECT with NOLOCK.

## 18. Validation Matrix
| Field | Rule | Message | BR |
|---|---|---|---|
| As On Date | non-empty | "Please select an As On Date." | BR-M09-045 |
| Any date text | unparseable ⇒ treated as blank (null) | none | – |
| Export | rows > 0 | "No data available to export." | – |
No check From ≤ To.

## 19. Business Rules
| ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M09-045 | Outstanding = (Σ tblInvoiceDetail.DeliveryNetAmount + tblInvoice.DeliveryCharge) − Σ(PaymentAmount + Discount + AIT) of payments dated on/before the As On Date; only invoices dated on/before the As On Date and with Outstanding > 0 are listed (so fully paid, zero-value and not-yet-delivered invoices are excluded). | Live_Deployment_Script_All_Updates.sql:144-204 | CONFIRMED |
| BR-M09-046 | "Credit Days" (age) = `DATEDIFF(DAY, InvoiceDate, AsOnDate)` — a single number per invoice; **there are no bucket boundaries in this report** (no 1-30/31-60 style grouping in SP, page or grid). | :172 ; aspx grid | CONFIRMED |
| BR-M09-047 | Payments count by payment date (`custPaymentDate`, else header `PaymentDate`) ≤ As On Date, so back-dated reports are historically consistent; later payments are ignored. | :160 | CONFIRMED |
| BR-M09-048 | Delivery-stage amounts are used (DeliveryNetAmount), not payment-stage (PaymentNetAmount) used by the collection list (BR-M09-002); returns recorded at payment confirmation or in tblReturnInvoice are therefore not deducted here. Expect differences vs PAGE-311 Due for invoices reduced at payment confirmation. | :147 vs sp_GET_PaymentInvSP:270 | CONFIRMED |
| BR-M09-049 | `@OnlyDueInvoices` has no effect (SP always filters Outstanding > 0); page always passes false. Sales-person filter uses the *customer master's* current SalesPersonId, not the invoice's MIO. Depot list is not restricted by user. | SP, :137 | CONFIRMED |

## 20. Permission Matrix
Session UserId must exist (redirect otherwise, :23-27). No role filter and no depot restriction (all depots/customers selectable). Menu-based visibility.

## 21. Status Lifecycle
Read-only; invoice "paid/unpaid" is computed, not read from `PaymentStatus`.

## 22. Error Handling
try/catch around filter loading, data load and export, each showing an alert with the exception message (leaks internals).

## 23. Audit
None (read-only report).

## 24. Integration
None.

## 25. Reports
GridView + `.xls` export; see RPT-M09-03.

## 26. Dependencies
tblCustPayDetail semantics of PAGE-311 (Discount and AIT count as settlement).

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-306-01 | Invoice 2026-01-01, amount 1000, no payment; As On 2026-01-31 | listed, Credit Days 30, Outstanding 1000 | BR-M09-045/046 |
| TC-306-02 | Payment 400 (+50 discount) dated 2026-01-20 | Paid 450 as of 01-31; as of 01-10 Paid 0 | BR-M09-047 |
| TC-306-03 | Fully paid invoice | not listed | BR-M09-045 |
| TC-306-04 | Filter by customer/sales person/depot | rows restricted | BR-M09-049 |
| TC-306-05 | Invoice dated after As On | not listed | BR-M09-045 |
| TC-306-06 | Invoice reduced at payment confirmation | aging shows higher outstanding than collection list | BR-M09-048 |
| TC-306-07 | Export with rows | xls downloaded; footer totals present | – |
| TC-306-08 | Empty As On Date | message | BR-M09-045 |
| TC-306-09 | Open without session | redirect Login | §20 |

## 28. Reverse Engineering References
`Solution.Web/SInventory_UI/CustomerInvoiceAgingReport.aspx(.cs)`, `Library.DAL/SInventory_DAL/CustomerInvoiceAgingDAL.cs`, `Live_Deployment_Script_All_Updates.sql:129-205`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| "Aging" buckets | (none exist) | not implemented; only Credit Days | NO SPEC (GAP-M09-014) |

## 30. Known Gaps
No buckets despite "Aging" title; unused `@OnlyDueInvoices`; outstanding ignores payment-stage adjustments and tblReturnInvoice; no depot security; NOLOCK reads; page sums formatted text (rounding via display strings); infinite command timeout.

## 31. Change Impact
Only this page calls the SP (grep of `sp_Report_CustomerInvoiceAgingOutstanding`: DAL only). Any change to `tblCustPayDetail.Discount/AIT` semantics changes Paid here.
