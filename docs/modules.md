# Modules

Module boundaries are INFERRED from folders, page/class/SP naming and code structure (no module registry exists). Full inventory: `docs/reverse-engineering/module-inventory.md`.

## M01 — Authentication, Session & Navigation

- Pages: 11; business rules: 0; workflows: 5; report docs: 1; gaps: 25
- Spec: [specs/authentication-navigation/README.md](../specs/authentication-navigation/README.md)

This module is the front door of the ERP: it authenticates a user against `tblUser`, builds the ASP.NET session that all other modules read (user, role, role type, company unit, employee display data, dashboard preference), renders the role-driven navigation menu, forces a password change when flagged, records a best-effort login-session audit row, and lets a user change their own password. It also contains a few legacy/utility pages (legacy menu admin, demo/template pages, a field-employee location viewer).

## M02 — User, Role & Permission Administration

- Pages: 16; business rules: 0; workflows: 7; report docs: 0; gaps: 31
- Spec: [specs/user-role-permission/README.md](../specs/user-role-permission/README.md)

M02 lets administrators (a) create/maintain login accounts and tie them to employees or DAs, roles, dashboard preference, mobile access and DC scope; (b) maintain roles and role→menu permissions (Permission/Add/View/Edit/Delete, Web/App); (c) assign users to distribution centres (`tblUserCompanyUnit`); (d) define approval chains by role type per menu and approver users per approval page; (e) delegate order placement per territory for a period. Most stored procedures behind these screens are **not in the repository**, so persistence details are UNKNOWN.

## M03 — Organisation & Market Structure Master

- Pages: 75; business rules: 0; workflows: 7; report docs: 0; gaps: 24
- Spec: [specs/organisation-market-structure/README.md](../specs/organisation-market-structure/README.md)

Maintains the organisational backbone used by the field-force/distribution ERP:
- **Sales hierarchy** (as coded): `Group` (screens titled "National") → `Zone` (DB/code name `Region`; same level) → `Area` → `Territory` → `Sub-Territory` → `Market`. Roles bound to levels (INFERRED from control scoping): NSM=Group, DZSM=Zone, AM=Area, MIO=Territory. There is no extra "Region" tier between Zone and Area.
- **Geography**: Division → District → Thana/Upazila (each Market points to a Thana).
- **Distribution structure**: Sales Center / Depot (= `tblCompanyUnit`, "DC"), routes (`RouteInformation*`, legacy `Router*`), territory-wise depot mapping.
- **Structure change control**: transfer requests (5 levels) + approval; bulk market import + approval.
- **Financial year** master and year-end opening/purge.
- Legacy `SInventory_UI` company/region/zone/area/district/market/department pages (older BLL+inline-SQL model).

## M04 — Product, Pricing & Promotion Master

- Pages: 64; business rules: 0; workflows: 6; report docs: 0; gaps: 33
- Spec: [specs/product-pricing-promotion/README.md](../specs/product-pricing-promotion/README.md)

M04 holds the master data that every sale rests on: the **product catalogue** and its classification (category, type, brand, generic/therapeutic group, product line, pack size, UOM, shipping carton, manufacturer, supplier), the **unit price** record (cost, trade price, VAT, MRP) with an archive of superseded prices, **campaigns** (bonus / discount / trade-policy / FCFS offers targeted by customer type, product line, validity and payment mode), **quoted prices** (customer-specific discounted prices), and **promo allocation** (promo groups, MIO membership, monthly promo quantities). The *application* of prices, trade policy, special discounts and campaigns to orders/invoices happens in the order/invoice modules and (for campaigns) outside this repo; the consumption points found are documented in WF-M04-06.

## M05 — Customer & Provider Master

- Pages: 40; business rules: 0; workflows: 8; report docs: 3; gaps: 32
- Spec: [specs/customer-master/README.md](../specs/customer-master/README.md)

Maintains the outlets ("customers"/providers: pharmacies, chemists) and their classification in `tblCustMaster`: market hierarchy, customer type, provider (program) type, pharma platform (SMC type), station types, tagged sales persons, product lines; per-company **credit limits** with maker/checker approval that block invoice generation; bulk maintenance through Excel (legacy upload→verify→transfer, and bulk property update import→approve); classification set-up pages; legacy SAP-era customer pages that still write a code-based column set. Two generations coexist: modern SP-based `MasterSetup_UI` and legacy inline-SQL `SInventory_UI` (GAP-M05-028).

## M06 — Order Management

- Pages: 29; business rules: 0; workflows: 6; report docs: 3; gaps: 40
- Spec: [specs/order-management/README.md](../specs/order-management/README.md)

The module covers the life of a customer sales order **before** delivery: order capture (manual web entry, Excel upload, mobile/app orders that arrive as `tblOrder` rows), optional exception approval, assignment to a distribution centre (DC) / route, credit-controlled conversion to a proforma invoice, and confirmation of delivery (Full/Partial/Reject) which the classifier also placed here (functionally M07). Monitoring/tracking pages, requisition pages (DC → warehouse stock requests) and a delete/audit report complete the module.

## M07 — Proforma, Invoicing & Delivery

- Pages: 43; business rules: 0; workflows: 5; report docs: 2; gaps: 39
- Spec: [specs/invoicing-delivery/README.md](../specs/invoicing-delivery/README.md)

Covers everything between a customer order and goods delivered at a Distribution Center (DC): proforma invoice creation (bulk/manual; the live by-order creation pages belong to **M06** `../order-management/`), invoice printing, DC picking slips, top sheets per Delivery Assistant (DA), loading summary / sales confirmation (delivery confirmation + accounting posting), payment-confirmation worklist, requisition based warehouse picking and issue challans (central warehouse -> DC), challan reports, and old-data delivery confirmation by Excel. Partial payment (`PaymentPartial`) is **M09** `../collection-payment/pages/PAGE-334-paymentpartial.md`; sales return is a separate module; `LogisticeRollBackOperation` is documented under `../sales-reports-dashboard/pages/PAGE-514-logisticerollbackoperation.md`.

## M08 — Sales Return & Invoice Return

- Pages: 20; business rules: 0; workflows: 4; report docs: 5; gaps: 26
- Spec: [specs/sales-return/README.md](../specs/sales-return/README.md)

M08 covers every way goods and money come back from customers in the distribution ERP:
1. **Market sales return** ("Return From Market", PAGE-294): return without an original invoice; stock is received into the depot ledger and two vouchers are auto-posted.
2. **Delivery invoice return** (PAGE-287): return against a delivered invoice; stock re-enters at the original batch; vouchers posted; an approval step exists in the data model but is bypassed (status 'Posted').
3. **Second-time (post-payment) partial return** (PAGE-296 -> PAGE-298): after payment confirmation, the accepted quantity per line is corrected; stock is restored in place; **no posting**.
4. **Invoice roll-back** (PAGE-288): undelivered invoices are deleted and the stock reservation is released.
5. **Legacy return invoice with freeze stock** (PAGE-290).
6. Report/list pages for all of the above (sales return, delivery return, rejection, replace note).
The **deduction charge** (return-delivery charge recovered from the customer) exists in both the delivery and market flows and is booked as income (GL flag 1032).

## M09 — Collections, Payments & Money Receipts

- Pages: 43; business rules: 0; workflows: 7; report docs: 7; gaps: 40
- Spec: [specs/collection-payment/README.md](../specs/collection-payment/README.md)

After a distributor invoice is delivered and (optionally) reduced at "payment confirmation" (`PaymentPartial`), this module records the money coming back: depot users enter **customer collections** against invoices (`CustomerPayment`), optionally with **discount** and **AIT** deductions, choosing a **cash or bank ledger account**. The system splits each collection into **TP** and **VAT** parts, updates invoice payment status, **auto-posts an approved credit voucher into the accounting database** (`CSTL_ACCDB_Unicorn`), and issues a **money receipt**. Related activities: bulk Excel uploads of collections, reversal/deletion of wrongly entered collections, **bank deposit slips** (cash banked by MIO/DIC, uploaded from Excel and listed), and a family of **ledger / receivable / aging / collection reports**.

## M10 — Warehouse, DC Stock & Inventory

- Pages: 56; business rules: 0; workflows: 10; report docs: 10; gaps: 33
- Spec: [specs/warehouse-inventory/README.md](../specs/warehouse-inventory/README.md)

M10 covers stock held in the **Central Warehouse (CW)** and in **Distribution Centers (DC/depots)**: receiving goods (manual stock-in, production output via material conversion), maker/checker approval, issue/write-off, adjustments, quarantine (freeze by stock condition) and release, sample conversions, batch correction, opening balances, and the stock, valuation and monitoring reports. Pages live in `Solution.Web/SInventory_UI`, `SInventory_RPTVIEW` (Crystal viewers) and one in `Reports_UI`; logic in `Library.BLL/DAL/SInventory_*`. Most DAL code builds SQL by string concatenation and runs it through the generic SP `ExecuteAllSqlQueryByStoreProcedure`.

## M11 — Stock Transfer & Chalan Receive

- Pages: 28; business rules: 0; workflows: 4; report docs: 3; gaps: 21
- Spec: [specs/stock-transfer/README.md](../specs/stock-transfer/README.md)

M11 moves stock between organisational units and records the documents (challans) that go with it:
- **STO (warehouse to DC)**: request (PAGE-420) -> pick/issue (M06/M07 pages) -> DC receipt (PAGE-413) -> SAP notification.
- **DC to DC (B2B)**: challan with immediate stock issue at the sender (PAGE-412), receipt at the destination (PAGE-423), list/delete (PAGE-407).
- **DC to warehouse return**: challan (PAGE-408), stock deduction on "Transfer" (PAGE-409), warehouse receipt (PAGE-414).
- **SAP-origin B2B challans** created and flagged by SAP-related stored procedures (no C# caller).
- Reports: challan prints, STO print, and "in-transit" reports that are actually customer receivables.

## M12 — Sub-Depot Operations

- Pages: 37; business rules: 0; workflows: 6; report docs: 8; gaps: 31
- Spec: [specs/sub-depot/README.md](../specs/sub-depot/README.md)

A **sub-depot** is a satellite stock point under one parent DC (`tblSubDepot.ComUnitId`, BR-M12-001). M12 is a **parallel stack** to the main DC stack: the DC sends stock to a sub-depot (chalan), the sub-depot receives it into its own store (`tblSubDepotStore`), invoices customers of sub-depot routes (`tblSubInvoiceMaster/Detail`), collects payments, quarantines/releases damaged and returned stock (`tblSubDepotStoreFreeze`), adjusts stock (direct or by approved voucher) and can return stock to the DC. Orders/routes are flagged `IsSubDepo` so main-depot lists exclude them (BR-M12-038).
Code: `Solution.Web/SubDepot_UI` (19 pages), sub-depot pages in `SInventory_UI` (7) and viewers in `SInventory_RPTVIEW` (11); `Library.BLL/DAL/DAO` `SubDepot_*` (plus shared `OrderInfoDAL`, `CustPaymentDAL`, `ChalanDAL`, `OtherStockActionDAL`, `DCStockReportDAL`). All `SubDepot_DAL` SQL is string-concatenated and executed through the wrapper SP `ExecuteAllSqlQueryByStoreProcedure` (`Library.DAL/InternalCls/ClsCommonInternalDAL.cs:397-421`).

## M13 — Sales Reports, Analytics & Dashboards

- Pages: 75; business rules: 0; workflows: 6; report docs: 8; gaps: 33
- Spec: [specs/sales-reports-dashboard/README.md](../specs/sales-reports-dashboard/README.md)

M13 holds the management-information layer of the sales & distribution ERP: 75 pages = the post-login **AdminDashboard** (28 AJAX endpoints), the **Business Summary / MIS** report family (many near-duplicate variants), sales reports by company/DC/region/zone/district/territory/MIA/MIO/customer/product, GP/COGS reports, trend/comparison reports, the field-activity **DynamicSalesReport**, 26 popup **viewer** pages (`SInventory_RPTVIEW`) that export Crystal reports to Excel/screen, an app web-view (**SixTenProjectReportApps**, DoctorApps) and two **data-modifying admin operations**: `SalesOperationRollback` (proforma / delivery / payment) and `LogisticeRollBackOperation` (STO / picking / challan).
Business value: management sees invoiced vs delivered ("sales") vs returned vs (invoice-minus-return-minus-sales) "outstanding" amounts per depot/zone/area/territory/MIO; sales-force activity (DCR/Rx) vs sales; GP on realised collections; and can undo mistakes in the order-to-cash chain.

## M14 — Target & DWSP Management

- Pages: 24; business rules: 0; workflows: 3; report docs: 2; gaps: 21
- Spec: [specs/target-management/README.md](../specs/target-management/README.md)

The module lets the company plan **sales targets top-down** and then break them into a **day-wise sales plan (DWSP)**:

## M15 — HR, Field-Force & Employee Administration

- Pages: 74; business rules: 0; workflows: 6; report docs: 5; gaps: 27
- Spec: [specs/hr-field-force/README.md](../specs/hr-field-force/README.md)

Employee master and login creation, sales-force hierarchy assignment (MIO/AM/DZSM/NSM), attendance and shift set-up, leave (types, configuration, applications, yearly balance), holidays, tour-plan set-up and approval, expense / TADA (daily allowance) / mileage claims and their rule tables, monthly allowances, training, transport modes, notice board, department/designation masters, and HR reports. Users: HR/admin (masters, employees), field managers (approvals), field staff (mostly via the mobile app - attendance, tour plans and claims are *created* by the app; the web pages here list, correct, approve and report).

## M16 — Doctor / Provider Master, Visits & Prescriptions

- Pages: 40; business rules: 0; workflows: 5; report docs: 4; gaps: 66
- Spec: [specs/doctor-visit-prescription/README.md](../specs/doctor-visit-prescription/README.md)

Pharma field-force module: maintain the doctor ("provider") master and its lookups (chamber type, category, degree, designation, speciality, patient type, prescription type, special day); review monthly call plans (DCP), doctor call reports (DCR = visits) and prescriptions (RX); approve/reject DCRs; report coverage/prescription performance by doctor, product, brand, MIO, area and zone. DCP/DCR creation is **not** done by any web page (INFERRED mobile app; GAP-M16-062).

## M17 — SAP, Mobile-App & External Integration

- Pages: 19; business rules: 0; workflows: 4; report docs: 0; gaps: 18
- Spec: [specs/sap-external-integration/README.md](../specs/sap-external-integration/README.md)

Operator screens and endpoints through which the ERP exchanges data with SAP (master data, stock movements, challan confirmation, bank deposits, MIGO files), with an external e-Program (provider dropouts), with API-captured customers, and with the field-force mobile app; plus the AJAX/JSON lookup endpoints (`CommonDataLoad`, `SeedData`, `Setup` page methods, ASMX). Integration-by-integration detail: [`../specs/integrations/README.md`](../specs/integrations/README.md).

## M18 — Approval Workflow

- Pages: 12; business rules: 0; workflows: 5; report docs: 0; gaps: 34
- Spec: [specs/approval-workflow/README.md](../specs/approval-workflow/README.md)

The module gives managers in the sales hierarchy a queue per subject type from which they approve/verify or reject items raised by field staff or created in the field: customers and doctors (master data), monthly tour plans and DCP/CVP visit plans, DCRs and prescriptions (RX), leave applications, expense / DA / mileage claims and field-force orders, plus a bulk market-transfer approval of customers/doctors. Approval is multi-level: the list SP returns each subject's latest approval-log row (`Step`, `ToRoleTypeId`, `ToEmpId`); only the user whose role equals `ToRoleTypeId` (or a gate-exempt role) sees the approve/reject buttons; each action calls a per-domain "save app-log" SP with `Step = current + 1`, a status word (`Verified`, `Accepted`, `Rejected`) and the acting employee. The chain itself is configured on `UserPermission/ApprovalStepMap.aspx` (M02, PAGE-026). Next-approver computation and header-status changes happen inside SPs that are not in the repository.

