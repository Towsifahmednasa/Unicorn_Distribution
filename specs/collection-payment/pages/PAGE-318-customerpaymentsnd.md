---
page_id: PAGE-318
module: M09
page: CustomerPaymentSnd
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-318 — CustomerPaymentSnd (customer payment for "second-time return" invoices)

> **Evidence level:** Tier A — `CustomerPaymentSnd.aspx.cs` (651 lines) read in full; markup compared with PAGE-315. `sp_GET_PaymentInvSPSndReturn`, `sp_Get_RouteInfoforCustPaymentSnd` bodies are NOT in the repo (UNKNOWN). "SND" = second-time (per brief vocabulary); the breadcrumb reads "Customer payment for second return".

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-318 |
| Kind | Entry / Transaction (CONFIRMED) |
| Page class | SInventory_UI_CustomerPaymentSnd |
| Code-behind LOC | 651 |
| Relationship | Clone of PAGE-315 with a different invoice-list SP and route-list SP; PAGE-311's header button linking here is hidden (`CustomerPayment.aspx:366-376`) so reachability is via menu only (UNKNOWN). |

## 2. Purpose
Collect payments for invoices that went through a *second* delivery-return cycle. The invoice list comes from `sp_GET_PaymentInvSPSndReturn` (whereas normal collection excludes `SndReturnInvoiceNo IS NOT NULL` invoices — `sp_GET_PaymentInvSP` line 286), i.e. this page is the only collection path for those invoices. INFERRED from the SP name and the `SndReturnInvoiceNo IS NULL` exclusion in the standard list (CONFIRMED).

## 3. Business Objective
Allow settlement of invoices whose payable amount was recalculated after a second return, which the standard collection list intentionally hides. INFERRED.

## 4. Navigation
Cancel → `CustomerPayment.aspx` (:504). Inbound: hidden link in PAGE-311. Menu UNKNOWN.

## 5. Route
`~/SInventory_UI/CustomerPaymentSnd.aspx`.

## 6. Source Files
`CustomerPaymentSnd.aspx(.cs)`; `OrderInfoDAL.SndTimeReturnLoadPaymentInvSP` (:414-443 → `sp_GET_PaymentInvSPSndReturn`), `SeedDataDAL.GetRouteInfoforCustPaymentSnd`, plus everything in PAGE-315.

## 7. UI Control Inventory
Same control set as PAGE-315 §7 (salesCenterDropDownList, rootDropDownList, ddlTerritoryName, ddlDAName, payTypeDDL, refNameTextBox/refDtTextBox, orderGridView, lblCount, Search/Save/Reset). Differences: route dropdown filled by `sp_Get_RouteInfoforCustPaymentSnd`; Search calls `SndTimeReturnLoadPaymentInvSP`; breadcrumb "Customer payment for second return"; Order Code/Date columns commented out in the grid.

## 8. Field Mapping
Identical to PAGE-315 §8 (PaymentDate = today; PayType from payTypeDDL; RefNo/RefDate from reference boxes; DANameId from ddlDAName; CollectionBy from row dropdown; no AIT/Discount/bank/cash).

## 9. Dropdown Mapping
As PAGE-315 §9 with `GetRouteInfoforCustPaymentSnd(comUnitId)` for routes; CollectionBy auto "MIO" when DA text == "MIO" else "DIC" (`CollectionByChange` :636-651).

## 10. Button Action Mapping
Page-specific validations: `Validation()` :146-213 (≥1 row ticked, pay amount non-empty, payment type selected, DA selected); `LoadGridView` :514-551 (depot, route, territory required; no date sort). Handlers as PAGE-315 §10.
Identical to PAGE-315 §7-§10 (same control IDs, same DTO mapping, same handlers) with two differences: the route dropdown is filled by `sp_Get_RouteInfoforCustPaymentSnd` and Search calls `SndTimeReturnLoadPaymentInvSP(GenerateParam())`. Page-specific validations: `Validation()` :146-213 (≥1 row ticked, pay amount non-empty, payment type selected, DA selected); `LoadGridView` :514-551 (depot, route, territory required; no date sort).

## 11. Frontend Flow
Same as PAGE-315: depot → route → territory/DA → Search → tick rows (pay amount preset) → Save.

## 12. Backend Flow
Same as PAGE-315 `saveButton_Click` (:215-367): duplicate SP → Full/Partial by `(pay + previous) == Confirm Amount` → `UpdateInvoiceFinalPayment` → shared detail list → `SaveCustPayment`; no accounts posting; `LoadGridView()` + "Operation successful!" or "Already Exist!". Note `Validation()` also contains an unused total-amount loop with `Convert.ToDecimal(payAmountTextBox.Text)` (:183-192) that throws on non-numeric text.

## 13. API Mapping
None.

## 14. Service Mapping
`CustPaymentBLL`; `OrderInfoDAL` called directly.

## 15. Repository Mapping
As PAGE-315 §15.

## 16. Stored Procedure Mapping
`sp_GET_PaymentInvSPSndReturn` (UNKNOWN), `sp_GET_PaymentInvSPTPVATAmt` (repo body; INNER JOINs tblOrder), `sp_Check_Duplicate_InvoiceFinalPayment`, `sp_Update_InvoiceFinalPayment` (UNKNOWN), `sp_Get_RouteInfoforCustPaymentSnd` (UNKNOWN), route/territory/DA SPs (UNKNOWN).

## 17. Database Mapping
As PAGE-315.

## 18. Validation Matrix
As PAGE-315 §18.

## 19. Business Rules
| ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M09-037 | Invoices flagged with `SndReturnInvoiceNo` are excluded from the standard collection list (BR-M09-001) and are collectable only through this page via `sp_GET_PaymentInvSPSndReturn` (criteria UNKNOWN). | Live_Deployment_Script_All_Updates.sql:286; OrderInfoDAL.cs:429 | CONFIRMED (exclusion) / UNKNOWN (Snd list criteria) |
| BR-M09-038 | Same save rules as the legacy variant BR-M09-035 (depot+route+territory to search; DA + payment type to save; payment date today; no discount/AIT/bank-cash/voucher). | CustomerPaymentSnd.aspx.cs:146-367 | CONFIRMED |

## 20. Permission Matrix
Session UserId/LoginName; menu-based only.

## 21. Status Lifecycle
As PAGE-315; Snd-specific status columns UNKNOWN.

## 22. Error Handling
As PAGE-315 (empty catch in loaders; none in save).

## 23. Audit
CreateBy, CreateDate on tblCustomerPay.

## 24. Integration
None (no accounts posting).

## 25. Reports
None.

## 26. Dependencies
As PAGE-315.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-318-01 | Search with route/territory of a second-return invoice | invoice listed here but absent from PAGE-311 | BR-M09-037 |
| TC-318-02 | Save without DA/payment type | validation message | BR-M09-038 |
| TC-318-03 | Save valid | payment rows, no voucher | BR-M09-038 |

## 28. Reverse Engineering References
`Solution.Web/SInventory_UI/CustomerPaymentSnd.aspx(.cs)`, `Library.DAL/SInventory_DAL/OrderInfoDAL.cs:414-443`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| Snd list logic | – | SP body not in repo | UNKNOWN (GAP-M09-001) |

## 30. Known Gaps
Same defects as PAGE-315 (shared detail list, no posting, no transaction, UI-only over-payment guard). Snd list criteria and route SP unverifiable. Money collected here is never posted to accounts by this code.

## 31. Change Impact
Shares `SaveCustPayment/SaveCustDetail`, `sp_GET_PaymentInvSPTPVATAmt`, duplicate & update SPs with PAGE-311/314/315. Changing `SndReturnInvoiceNo` handling in `sp_GET_PaymentInvSP` shifts invoices between this page and PAGE-311.
