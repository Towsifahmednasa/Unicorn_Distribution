---
page_id: PAGE-315
module: M09
page: CustomerPaymentPPPP
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-315 — CustomerPaymentPPPP (legacy route/territory-based customer payment)

> **Evidence level:** Tier A — full `.aspx.cs` (651 lines) read by diff against PAGE-318 (`CustomerPaymentSnd.aspx.cs`, read in full) plus the `.aspx` header/grid columns. The two code-behind files differ only in (1) class name, (2) route-dropdown SP, (3) the invoice-list SP; the .aspx files differ in breadcrumb text and visible Order Code/Date columns. SP bodies not in repo except `sp_GET_PaymentInvSP` / `sp_GET_PaymentInvSPTPVATAmt`.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-315 |
| Kind | Entry / Transaction — earlier generation of PAGE-311 (CONFIRMED by identical structure) |
| Page class | **`SInventory_UI_CustomerPayment`** — the same class name as PAGE-311 (declared in `CustomerPaymentPPPP.aspx` `Inherits=`); risk of type collision if both are ever compiled into one assembly (INFERRED) |
| Code-behind LOC | 651 |
| Status | Superseded by PAGE-311; no menu evidence of use (UNKNOWN) |

## 2. Purpose
Record customer collections against due invoices selected by **depot + distribution route + territory** (all three mandatory), for a DA (delivery assistant) chosen by name, in the older simple form (pay amount only; no discount, AIT, bank/cash accounts, receipt modal, accounts posting).

## 3. Business Objective
Same as PAGE-311 (reduce invoice dues), without accounting integration. INFERRED.

## 4. Navigation
Cancel → `CustomerPayment.aspx` (:504). No inbound link found in code. Menu UNKNOWN.

## 5. Route
`~/SInventory_UI/CustomerPaymentPPPP.aspx`.

## 6. Source Files
`Solution.Web/SInventory_UI/CustomerPaymentPPPP.aspx(.cs)`; `CustPaymentBLL`, `OrderInfoDAL`, `SeedDataDAL.GetRouteInfoforCustPayment`, `RouteInformationDAL`; DAO CustPayment/CustPaymentDetail.

## 7. UI Control Inventory
| Control | Label | Required | Notes |
|---|---|---|---|
| salesCenterDropDownList | Sales Center | `LoadGridView` :519 | user depots |
| rootDropDownList | Route | :527 "Please input Route!" | `sp_Get_RouteInfoforCustPayment` |
| ddlTerritoryName | Territory | :537 "Please input Territory!" | `sp_GET_forPaymentTerritoryByRouteInformationDDL` |
| ddlDAName | DA Name | **required on save** :203 "Please Select DA Name!!" | `sp_GET_RouteInformationDADetailDDL` |
| payTypeDDL | Payment Type | required :198 "Please Select Payment Type!!" | tblPaymentType |
| refNameTextBox / refDtTextBox | Reference No / date | optional | |
| orderGridView | Due invoice list | – | checkbox, Customer ID/Name, Order Code, Order Date, Invoice No/Date, Confirm Amount, Previous Pay, Due Amount, Pay Amount, Collection By (dropdown DIC/MIO/X. MIO), Adjust (hidden) |
| lblCount | Total Pay Amount | – | |
| Save / Search / Reset buttons | | | |

## 8. Field Mapping
As PAGE-311 §8 except: PaymentDate = today (no date input); PayType = payTypeDDL text; RefNo/RefDate from reference boxes; no AIT/Discount/BankAccId/CashAccId (columns left at defaults 0 by DAO); DANameId = `ddlDAName`; CollectionBy = row dropdown text; TP/VAT from hidden fields.

## 9. Dropdown Mapping
Route: `GetRouteInfoforCustPayment(comUnitId)`; territory & DA by route id; CollectionBy auto = "MIO" when DA display text equals "MIO" else "DIC" (`CollectionByChange` :636-651).

## 10. Button Action Mapping
Search → `LoadGridView` → `sp_GET_PaymentInvSP(GenerateParam)`; row check / pay amount → `PayAmountChange` + `CalculateTotal`; Save → `saveButton_Click`; header check-all.

## 11. Frontend Flow
Depot → routes; route → territory + DA lists; Search → list (NOT sorted by date in this version); tick rows; pay amount textbox defaults to Due; Save.

## 12. Backend Flow
`saveButton_Click` (:215-367): same as PAGE-311 minus discount/AIT/accounts: per ticked row header (`PaymentDate = today`) → duplicate SP → status Full/Partial (`pay + previous == confirm`) → `UpdateInvoiceFinalPayment(invoice, pay, status, login)` → accumulate detail → `SaveCustPayment(header, sharedList)`; **no `PaymentJournalPosting`**. After save `LoadGridView()` + success alert; else "Already Exist!".

## 13. API Mapping
None.

## 14. Service Mapping
CustPaymentBLL, OrderInfoDAL (direct).

## 15. Repository Mapping
As PAGE-311 §15 (SaveCustPayment/SaveCustDetail/UpdateInvoiceFinalPayment/Check/List/TPVAT).

## 16. Stored Procedure Mapping
`sp_GET_PaymentInvSP` (body in Live_Deployment_Script_All_Updates.sql:212-289; here `@param` = `and ord.TerritoryId='…' and ord.ComUnitId='…' AND ord.DistributionRouteId='…'`), `sp_GET_PaymentInvSPTPVATAmt` (:296-333), `sp_Check_Duplicate_InvoiceFinalPayment`, `sp_Update_InvoiceFinalPayment` (both UNKNOWN), route/territory/DA SPs (UNKNOWN).

## 17. Database Mapping
tblCustomerPay, tblCustPayDetail (INSERT); tblInvoice (via SP); reads as PAGE-311.

## 18. Validation Matrix
| Rule | Message | BR |
|---|---|---|
| ≥1 invoice ticked | "Please Select Invoice from List!!" | BR-M09-035 |
| Pay amount non-empty for ticked rows | "Please fill out Pay Amount !!" | BR-M09-035 |
| Payment type selected | "Please Select Payment Type!!" | BR-M09-035 |
| DA selected | "Please Select DA Name!!" | BR-M09-035 |
| Depot, Route, Territory before search | "Please input Sales Center!" / "Please input Route!" / "Please input Territory!" | BR-M09-035 |
| Over-payment reset | "Cannot Be Greater then Invoice Quantity " (UI event only) | BR-M09-004 (same rule) |

## 19. Business Rules
| ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M09-035 | Legacy variant: search needs depot + route + territory; save needs DA name and payment type; `PaymentDate` is always today; payments are stored without discount, AIT, cash/bank account or accounts-voucher posting. Status Full/Partial and duplicate guards are the same as BR-M09-007/010. TP/VAT split is BR-M09-006. | CustomerPaymentPPPP.aspx.cs:146-367, :514-564 | CONFIRMED |
| BR-M09-036 | CollectionBy default: "MIO" if the selected DA name text is exactly "MIO", else "DIC" (no role-based logic). | :636-651 | CONFIRMED |

## 20. Permission Matrix
Session: UserId, LoginName. Menu-based only; no role check.

## 21. Status Lifecycle
Full / Partial computed as in PAGE-311; persisted by `sp_Update_InvoiceFinalPayment` (UNKNOWN).

## 22. Error Handling
Try/catch swallow in dropdown loaders; none in save.

## 23. Audit
CreateBy, CreateDate on tblCustomerPay.

## 24. Integration
None (no accounts posting).

## 25. Reports
None.

## 26. Dependencies
As PAGE-311 (no cross-DB).

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-315-01 | Search without territory | "Please input Territory!" | BR-M09-035 |
| TC-315-02 | Save without DA | "Please Select DA Name!!" | BR-M09-035 |
| TC-315-03 | Save valid | rows inserted, no voucher | BR-M09-035 |
| TC-315-04 | DA text "MIO" | Collection By preset MIO | BR-M09-036 |

## 28. Reverse Engineering References
`Solution.Web/SInventory_UI/CustomerPaymentPPPP.aspx(.cs)`; diff vs `CustomerPaymentSnd.aspx.cs`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| Relationship to PAGE-311 | – | older duplicate | NO SPEC (GAP-M09-013) |

## 30. Known Gaps
Duplicate/legacy page with same class name as PAGE-311; shared-list bug identical to PAGE-311; no accounting posting; over-payment only UI-checked; dynamic SQL (`@param`) built from dropdown values; no transaction.

## 31. Change Impact
Shares `SaveCustPayment/SaveCustDetail`, `sp_GET_PaymentInvSP`, duplicate SP, `UpdateInvoiceFinalPayment` with PAGE-311/312/314/318. Deleting this page has no code dependents found (grep for "CustomerPaymentPPPP" in `.cs`: none).
