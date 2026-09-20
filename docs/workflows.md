# Workflows

104 traced workflows (page → handler → BLL → DAL → SP/SQL; data and status changes). Each file is under `specs/<module>/workflows/`.

## End-to-end business chain (evidence: module READMEs)

```
Master data (M03/M04/M05) -> Order (M06) -> Proforma/Invoice (M07) -> Picking / Challan / Top sheet / Loading (M07) -> Delivery confirmation
   -> Payment & voucher (M09)  |  Return (M08)  |  Stock effects (M10/M11/M12) -> SAP hand-off (M17)
```

## Index

| Module | Workflow file | Title |
|---|---|---|
| M01 | [WF-M01-01-login-and-session-bootstrap](../specs/authentication-navigation/workflows/WF-M01-01-login-and-session-bootstrap.md) | WF-M01-01 — Login and session bootstrap |
| M01 | [WF-M01-02-db-driven-menu-rendering](../specs/authentication-navigation/workflows/WF-M01-02-db-driven-menu-rendering.md) | WF-M01-02 — Page guard and DB-driven menu rendering (NewMasterPage) |
| M01 | [WF-M01-03-forced-password-change-and-change-password](../specs/authentication-navigation/workflows/WF-M01-03-forced-password-change-and-change-password.md) | WF-M01-03 — Forced password change and self-service password change |
| M01 | [WF-M01-04-legacy-menu-administration](../specs/authentication-navigation/workflows/WF-M01-04-legacy-menu-administration.md) | WF-M01-04 — Legacy menu administration (tblMainMenu / tblMenuDistribution) |
| M01 | [WF-M01-05-logout-and-session-end](../specs/authentication-navigation/workflows/WF-M01-05-logout-and-session-end.md) | WF-M01-05 — Logout and session end |
| M02 | [WF-M02-01-user-provisioning](../specs/user-role-permission/workflows/WF-M02-01-user-provisioning.md) | WF-M02-01 — User provisioning (create / edit login account) |
| M02 | [WF-M02-02-role-creation](../specs/user-role-permission/workflows/WF-M02-02-role-creation.md) | WF-M02-02 — Role creation / edit |
| M02 | [WF-M02-03-role-menu-permission](../specs/user-role-permission/workflows/WF-M02-03-role-menu-permission.md) | WF-M02-03 — Role menu permission (Permission / Add / View / Edit / Delete) |
| M02 | [WF-M02-04-dc-access-assignment](../specs/user-role-permission/workflows/WF-M02-04-dc-access-assignment.md) | WF-M02-04 — DC (distribution-centre) access assignment |
| M02 | [WF-M02-05-approval-chain-mapping](../specs/user-role-permission/workflows/WF-M02-05-approval-chain-mapping.md) | WF-M02-05 — Approval chain mapping (role types per menu) |
| M02 | [WF-M02-06-approver-assignment-legacy](../specs/user-role-permission/workflows/WF-M02-06-approver-assignment-legacy.md) | WF-M02-06 — Approver assignment (legacy `tblAppSetup`) |
| M02 | [WF-M02-07-order-placement-delegation](../specs/user-role-permission/workflows/WF-M02-07-order-placement-delegation.md) | WF-M02-07 — Order placement delegation per territory |
| M03 | [WF-M03-01-market-structure-transfer-request](../specs/organisation-market-structure/workflows/WF-M03-01-market-structure-transfer-request.md) | WF-M03-01 — Market-structure transfer request (Zone / Area / Territory / Sub-Territory / Market) |
| M03 | [WF-M03-02-market-structure-transfer-approval](../specs/organisation-market-structure/workflows/WF-M03-02-market-structure-transfer-approval.md) | WF-M03-02 — Market-structure transfer approval |
| M03 | [WF-M03-03-bulk-market-creation-upload-and-approval](../specs/organisation-market-structure/workflows/WF-M03-03-bulk-market-creation-upload-and-approval.md) | WF-M03-03 — Bulk market creation / update by Excel upload and approval |
| M03 | [WF-M03-04-hierarchy-node-setup-and-deactivation](../specs/organisation-market-structure/workflows/WF-M03-04-hierarchy-node-setup-and-deactivation.md) | WF-M03-04 — Hierarchy node setup (Group → Zone → Area → Territory → Sub-Territory → Market) and deactivation |
| M03 | [WF-M03-05-route-information-setup](../specs/organisation-market-structure/workflows/WF-M03-05-route-information-setup.md) | WF-M03-05 — Route information setup |
| M03 | [WF-M03-06-territory-wise-depot-mapping](../specs/organisation-market-structure/workflows/WF-M03-06-territory-wise-depot-mapping.md) | WF-M03-06 — Territory-wise depot mapping |
| M03 | [WF-M03-07-financial-year-opening-and-data-purge](../specs/organisation-market-structure/workflows/WF-M03-07-financial-year-opening-and-data-purge.md) | WF-M03-07 — Financial-year opening balance and data purge/archive |
| M04 | [WF-M04-01-campaign-setup-and-maintenance](../specs/product-pricing-promotion/workflows/WF-M04-01-campaign-setup-and-maintenance.md) | WF-M04-01 Campaign setup and maintenance |
| M04 | [WF-M04-02-quoted-price-setup](../specs/product-pricing-promotion/workflows/WF-M04-02-quoted-price-setup.md) | WF-M04-02 Quoted price setup |
| M04 | [WF-M04-03-unit-price-change-with-archive](../specs/product-pricing-promotion/workflows/WF-M04-03-unit-price-change-with-archive.md) | WF-M04-03 Unit price creation and change with archive |
| M04 | [WF-M04-04-product-creation-dc-availability](../specs/product-pricing-promotion/workflows/WF-M04-04-product-creation-dc-availability.md) | WF-M04-04 Product creation / edit and DC availability |
| M04 | [WF-M04-05-promo-group-tagging-and-quantity-allocation](../specs/product-pricing-promotion/workflows/WF-M04-05-promo-group-tagging-and-quantity-allocation.md) | WF-M04-05 Promo group, MIO tagging and quantity allocation |
| M04 | [WF-M04-06-price-discount-application-at-invoicing](../specs/product-pricing-promotion/workflows/WF-M04-06-price-discount-application-at-invoicing.md) | WF-M04-06 Price, trade policy, special discount and campaign application at invoicing (cross-module consumptio |
| M05 | [WF-M05-01-customer-master-create-edit](../specs/customer-master/workflows/WF-M05-01-customer-master-create-edit.md) | WF-M05-01 — Customer master: create, edit, find |
| M05 | [WF-M05-02-customer-approval-status-queue](../specs/customer-master/workflows/WF-M05-02-customer-approval-status-queue.md) | WF-M05-02 — Customer approval status and pending queue |
| M05 | [WF-M05-03-credit-limit-lifecycle](../specs/customer-master/workflows/WF-M05-03-credit-limit-lifecycle.md) | WF-M05-03 — Customer credit limit: entry → approval → enforcement |
| M05 | [WF-M05-04-excel-customer-upload-verify-transfer](../specs/customer-master/workflows/WF-M05-04-excel-customer-upload-verify-transfer.md) | WF-M05-04 — Excel customer upload → verify → transfer (legacy pipeline, incl. tag-change variant) |
| M05 | [WF-M05-05-bulk-property-import-and-approval](../specs/customer-master/workflows/WF-M05-05-bulk-property-import-and-approval.md) | WF-M05-05 — Bulk customer / doctor property update: import → approve → apply |
| M05 | [WF-M05-06-customer-provider-type-change](../specs/customer-master/workflows/WF-M05-06-customer-provider-type-change.md) | WF-M05-06 — Change a customer's provider (program) type in place |
| M05 | [WF-M05-07-classification-master-data-setup](../specs/customer-master/workflows/WF-M05-07-classification-master-data-setup.md) | WF-M05-07 — Classification master data: customer type, provider (program) type, pharma platform (SMC type), st |
| M05 | [WF-M05-08-legacy-customer-maintenance](../specs/customer-master/workflows/WF-M05-08-legacy-customer-maintenance.md) | WF-M05-08 — Legacy (SAP-era) customer maintenance: entry, edit, bulk reassign, fixed/regular flag, category |
| M06 | [WF-M06-01-manual-order-entry-and-edit](../specs/order-management/workflows/WF-M06-01-manual-order-entry-and-edit.md) | WF-M06-01 — Manual (web) order entry and edit |
| M06 | [WF-M06-02-order-to-proforma-invoice](../specs/order-management/workflows/WF-M06-02-order-to-proforma-invoice.md) | WF-M06-02 — Order → proforma invoice (credit check, stock issue, batch tag) |
| M06 | [WF-M06-03-delivery-confirmation](../specs/order-management/workflows/WF-M06-03-delivery-confirmation.md) | WF-M06-03 — Delivery confirmation of a proforma invoice (Full / Partial / Reject) |
| M06 | [WF-M06-04-special-order-approval](../specs/order-management/workflows/WF-M06-04-special-order-approval.md) | WF-M06-04 — Special-order approval (M18 engine consumer) |
| M06 | [WF-M06-05-excel-order-upload-and-pending-order-import](../specs/order-management/workflows/WF-M06-05-excel-order-upload-and-pending-order-import.md) | WF-M06-05 — Excel order upload → order generation; pending-order re-import |
| M06 | [WF-M06-06-order-dc-assignment-edit-and-delete](../specs/order-management/workflows/WF-M06-06-order-dc-assignment-edit-and-delete.md) | WF-M06-06 — Order maintenance: assign DC/route, change DC, delete |
| M07 | [WF-M07-01-proforma-to-delivery-lifecycle](../specs/invoicing-delivery/workflows/WF-M07-01-proforma-to-delivery-lifecycle.md) | WF-M07-01 - Proforma -> confirmation -> delivery (customer invoice lifecycle) |
| M07 | [WF-M07-02-manual-invoice-and-stock-allocation](../specs/invoicing-delivery/workflows/WF-M07-02-manual-invoice-and-stock-allocation.md) | WF-M07-02 - Invoice save and FEFO stock allocation (InvoiceBLL path) |
| M07 | [WF-M07-03-proforma-delete-and-anomaly-clean](../specs/invoicing-delivery/workflows/WF-M07-03-proforma-delete-and-anomaly-clean.md) | WF-M07-03 - Proforma deletion and anomaly auto-clean |
| M07 | [WF-M07-04-requisition-picking-issue-challan](../specs/invoicing-delivery/workflows/WF-M07-04-requisition-picking-issue-challan.md) | WF-M07-04 - Requisition picking, central-store issue and challan (CLN) |
| M07 | [WF-M07-05-top-sheet-and-dc-picking](../specs/invoicing-delivery/workflows/WF-M07-05-top-sheet-and-dc-picking.md) | WF-M07-05 - DC picking slip and top sheet (outbound preparation) |
| M08 | [WF-M08-01-market-sales-return-entry-and-posting](../specs/sales-return/workflows/WF-M08-01-market-sales-return-entry-and-posting.md) | WF-M08-01 — Market sales return: entry, stock receipt and accounting posting |
| M08 | [WF-M08-02-delivery-invoice-return](../specs/sales-return/workflows/WF-M08-02-delivery-invoice-return.md) | WF-M08-02 — Delivery invoice return (return against a delivered invoice) |
| M08 | [WF-M08-03-second-time-partial-return](../specs/sales-return/workflows/WF-M08-03-second-time-partial-return.md) | WF-M08-03 — Second-time (post-payment) partial return |
| M08 | [WF-M08-04-invoice-rollback-delete](../specs/sales-return/workflows/WF-M08-04-invoice-rollback-delete.md) | WF-M08-04 — Invoice roll-back (delete undelivered invoice) and legacy return-invoice with freeze stock |
| M09 | [WF-M09-01-customer-payment-collection](../specs/collection-payment/workflows/WF-M09-01-customer-payment-collection.md) | WF-M09-01 — Customer payment collection, TP/VAT split and accounts voucher posting |
| M09 | [WF-M09-02-payment-stage-confirmation](../specs/collection-payment/workflows/WF-M09-02-payment-stage-confirmation.md) | WF-M09-02 — Payment-stage confirmation (partial delivery / customer return) making an invoice collectable |
| M09 | [WF-M09-03-bulk-payment-excel-upload](../specs/collection-payment/workflows/WF-M09-03-bulk-payment-excel-upload.md) | WF-M09-03 — Bulk customer payment upload from Excel |
| M09 | [WF-M09-04-payment-reversal-and-deletion](../specs/collection-payment/workflows/WF-M09-04-payment-reversal-and-deletion.md) | WF-M09-04 — Reversing or deleting a customer payment |
| M09 | [WF-M09-05-deposit-slip-upload-and-list](../specs/collection-payment/workflows/WF-M09-05-deposit-slip-upload-and-list.md) | WF-M09-05 — Bank deposit slip upload, listing and deletion (deposit reconciliation) |
| M09 | [WF-M09-06-money-receipt-printing](../specs/collection-payment/workflows/WF-M09-06-money-receipt-printing.md) | WF-M09-06 — Money receipt issue and print |
| M09 | [WF-M09-07-accounts-posting-audit](../specs/collection-payment/workflows/WF-M09-07-accounts-posting-audit.md) | WF-M09-07 — Sales-to-accounts voucher posting audit (customer payment portion) |
| M10 | [WF-M10-01-warehouse-stock-in-to-approval](../specs/warehouse-inventory/workflows/WF-M10-01-warehouse-stock-in-to-approval.md) | WF-M10-01 Warehouse stock-in: entry -> review -> approval -> stock-out list |
| M10 | [WF-M10-02-material-conversion-receipt](../specs/warehouse-inventory/workflows/WF-M10-02-material-conversion-receipt.md) | WF-M10-02 Material conversion receipt (production output -> warehouse stock-in) |
| M10 | [WF-M10-03-warehouse-stock-out](../specs/warehouse-inventory/workflows/WF-M10-03-warehouse-stock-out.md) | WF-M10-03 Warehouse stock-out (issue from an approved stock-in) |
| M10 | [WF-M10-04-stock-condition-freeze-and-release](../specs/warehouse-inventory/workflows/WF-M10-04-stock-condition-freeze-and-release.md) | WF-M10-04 Stock condition freeze (quarantine) and release |
| M10 | [WF-M10-05-warehouse-stock-adjustment](../specs/warehouse-inventory/workflows/WF-M10-05-warehouse-stock-adjustment.md) | WF-M10-05 Central warehouse stock adjustment (and its reversal) |
| M10 | [WF-M10-06-depot-stock-adjustment-voucher](../specs/warehouse-inventory/workflows/WF-M10-06-depot-stock-adjustment-voucher.md) | WF-M10-06 Depot stock adjustment voucher (DC stock-out) with approval |
| M10 | [WF-M10-07-sample-stock-conversion](../specs/warehouse-inventory/workflows/WF-M10-07-sample-stock-conversion.md) | WF-M10-07 Sample stock conversion (DC) and sample stock for warehouse |
| M10 | [WF-M10-08-stock-valuation-reporting](../specs/warehouse-inventory/workflows/WF-M10-08-stock-valuation-reporting.md) | WF-M10-08 Stock valuation reporting (DC ledger and national valuation) |
| M10 | [WF-M10-09-central-store-picking-weighted-average-issue](../specs/warehouse-inventory/workflows/WF-M10-09-central-store-picking-weighted-average-issue.md) | WF-M10-09 Central store issue for STO picking (weighted-average ledger) |
| M10 | [WF-M10-10-dc-stock-anomaly-check-and-reconcile](../specs/warehouse-inventory/workflows/WF-M10-10-dc-stock-anomaly-check-and-reconcile.md) | WF-M10-10 DC store anomaly check and reconcile (operational SP scripts) |
| M11 | [WF-M11-01-sto-warehouse-to-dc](../specs/stock-transfer/workflows/WF-M11-01-sto-warehouse-to-dc.md) | WF-M11-01 — Stock Transport Order: warehouse → DC (request, pick/issue, DC receive) |
| M11 | [WF-M11-02-dc-to-dc-b2b-transfer](../specs/stock-transfer/workflows/WF-M11-02-dc-to-dc-b2b-transfer.md) | WF-M11-02 — DC ↔ DC (B2B) transfer: send, in-transit, receive, delete |
| M11 | [WF-M11-03-dc-to-warehouse-return](../specs/stock-transfer/workflows/WF-M11-03-dc-to-warehouse-return.md) | WF-M11-03 — DC → Warehouse return challan: create, transfer, warehouse receive |
| M11 | [WF-M11-04-sap-b2b-challan-and-handoff](../specs/stock-transfer/workflows/WF-M11-04-sap-b2b-challan-and-handoff.md) | WF-M11-04 — SAP-origin B2B challan and SAP hand-off states |
| M12 | [WF-M12-01-dc-to-subdepot-transfer](../specs/sub-depot/workflows/WF-M12-01-dc-to-subdepot-transfer.md) | WF-M12-01 DC -> Sub-depot stock transfer (send and receive) |
| M12 | [WF-M12-02-subdepot-to-dc-return](../specs/sub-depot/workflows/WF-M12-02-subdepot-to-dc-return.md) | WF-M12-02 Sub-depot -> DC return transfer |
| M12 | [WF-M12-03-stock-out-adjustment-approval](../specs/sub-depot/workflows/WF-M12-03-stock-out-adjustment-approval.md) | WF-M12-03 Sub-depot stock out / adjustment and approval |
| M12 | [WF-M12-04-freeze-and-release](../specs/sub-depot/workflows/WF-M12-04-freeze-and-release.md) | WF-M12-04 Freeze (quarantine) and release |
| M12 | [WF-M12-05-subdepot-invoicing-payment](../specs/sub-depot/workflows/WF-M12-05-subdepot-invoicing-payment.md) | WF-M12-05 Sub-depot invoicing, delivery and payment |
| M12 | [WF-M12-06-invoice-payment-reversal](../specs/sub-depot/workflows/WF-M12-06-invoice-payment-reversal.md) | WF-M12-06 Reversals: invoice delete, payment reverse, sales-return delete |
| M13 | [WF-M13-01-sales-operation-rollback](../specs/sales-reports-dashboard/workflows/WF-M13-01-sales-operation-rollback.md) | WF-M13-01 Sales operation rollback (proforma / delivery / payment) |
| M13 | [WF-M13-02-logistics-requisition-rollback](../specs/sales-reports-dashboard/workflows/WF-M13-02-logistics-requisition-rollback.md) | WF-M13-02 Logistics requisition rollback (STO / picking / challan) |
| M13 | [WF-M13-03-business-summary-mis-reporting](../specs/sales-reports-dashboard/workflows/WF-M13-03-business-summary-mis-reporting.md) | WF-M13-03 Business Summary MIS reporting (role-scoped, SP-driven) |
| M13 | [WF-M13-04-admin-dashboard-data-load](../specs/sales-reports-dashboard/workflows/WF-M13-04-admin-dashboard-data-load.md) | WF-M13-04 Admin dashboard data load and endpoint catalogue |
| M13 | [WF-M13-05-report-viewer-launch](../specs/sales-reports-dashboard/workflows/WF-M13-05-report-viewer-launch.md) | WF-M13-05 List page -> popup viewer -> Crystal export |
| M13 | [WF-M13-06-dynamic-sales-report-drilldown](../specs/sales-reports-dashboard/workflows/WF-M13-06-dynamic-sales-report-drilldown.md) | WF-M13-06 Dynamic sales report grid and drill-down modals |
| M14 | [WF-M14-01-target-cascade-setup](../specs/target-management/workflows/WF-M14-01-target-cascade-setup.md) | WF-M14-01 - Target cascade setup (National -> Zone -> Area -> Territory) |
| M14 | [WF-M14-02-dwsp-day-wise-plan](../specs/target-management/workflows/WF-M14-02-dwsp-day-wise-plan.md) | WF-M14-02 - DWSP day-wise plan and monthly report |
| M14 | [WF-M14-03-territory-target-excel-upload](../specs/target-management/workflows/WF-M14-03-territory-target-excel-upload.md) | WF-M14-03 - Territory target Excel upload |
| M15 | [WF-M15-01-employee-onboarding](../specs/hr-field-force/workflows/WF-M15-01-employee-onboarding.md) | WF-M15-01 Employee onboarding (employee + login + market assignment) |
| M15 | [WF-M15-02-leave-application-and-approval](../specs/hr-field-force/workflows/WF-M15-02-leave-application-and-approval.md) | WF-M15-02 Leave set-up, application and approval |
| M15 | [WF-M15-03-yearly-leave-process](../specs/hr-field-force/workflows/WF-M15-03-yearly-leave-process.md) | WF-M15-03 Yearly leave balance processing |
| M15 | [WF-M15-04-tour-plan-setup-and-approval](../specs/hr-field-force/workflows/WF-M15-04-tour-plan-setup-and-approval.md) | WF-M15-04 Tour plan set-up, approval and reporting |
| M15 | [WF-M15-05-claims-tada-expense-mileage-approval](../specs/hr-field-force/workflows/WF-M15-05-claims-tada-expense-mileage-approval.md) | WF-M15-05 Field-force claims (DA/TADA, expense, mileage) - entry, approval, reporting |
| M15 | [WF-M15-06-attendance-and-approval](../specs/hr-field-force/workflows/WF-M15-06-attendance-and-approval.md) | WF-M15-06 Attendance, shifts and attendance approval |
| M16 | [WF-M16-01-doctor-master-create-update](../specs/doctor-visit-prescription/workflows/WF-M16-01-doctor-master-create-update.md) | WF-M16-01 — Doctor master create / update |
| M16 | [WF-M16-02-doctor-lookup-master-maintenance](../specs/doctor-visit-prescription/workflows/WF-M16-02-doctor-lookup-master-maintenance.md) | WF-M16-02 — Doctor lookup master maintenance (chamber type, category, degree, designation, speciality, patient |
| M16 | [WF-M16-03-visit-plan-dcr-approval](../specs/doctor-visit-prescription/workflows/WF-M16-03-visit-plan-dcr-approval.md) | WF-M16-03 — Doctor visit: call plan (DCP) → call report (DCR) → approval → reporting |
| M16 | [WF-M16-04-prescription-capture-approval](../specs/doctor-visit-prescription/workflows/WF-M16-04-prescription-capture-approval.md) | WF-M16-04 — Prescription (RX) capture → list → approval |
| M16 | [WF-M16-05-doctor-customer-market-transfer](../specs/doctor-visit-prescription/workflows/WF-M16-05-doctor-customer-market-transfer.md) | WF-M16-05 — Doctor / customer transfer to another market |
| M17 | [WF-M17-01-sap-stock-receive](../specs/sap-external-integration/workflows/WF-M17-01-sap-stock-receive.md) | WF-M17-01 - SAP stock movement (challan) receive |
| M17 | [WF-M17-02-sap-master-data-approval](../specs/sap-external-integration/workflows/WF-M17-02-sap-master-data-approval.md) | WF-M17-02 - SAP employee / product master approval |
| M17 | [WF-M17-03-bank-deposit-to-sap](../specs/sap-external-integration/workflows/WF-M17-03-bank-deposit-to-sap.md) | WF-M17-03 - Bank deposit to SAP (currently semi-manual) |
| M17 | [WF-M17-04-provider-dropout-approval](../specs/sap-external-integration/workflows/WF-M17-04-provider-dropout-approval.md) | WF-M17-04 - Provider dropout approval |
| M18 | [WF-M18-01-generic-approval-log-engine](../specs/approval-workflow/workflows/WF-M18-01-generic-approval-log-engine.md) | WF-M18-01 — Generic role-chain approval-log engine (shared by all M18 pages) |
| M18 | [WF-M18-02-field-force-plan-claim-order-approval](../specs/approval-workflow/workflows/WF-M18-02-field-force-plan-claim-order-approval.md) | WF-M18-02 — Field-force plan / claim / leave / order approval (multi-level Verified → Accepted) |
| M18 | [WF-M18-03-customer-doctor-master-approval](../specs/approval-workflow/workflows/WF-M18-03-customer-doctor-master-approval.md) | WF-M18-03 — Customer and Doctor master-data approval |
| M18 | [WF-M18-04-customer-doctor-transfer-approval](../specs/approval-workflow/workflows/WF-M18-04-customer-doctor-transfer-approval.md) | WF-M18-04 — Customer / Doctor market-transfer approval |
| M18 | [WF-M18-05-tblaction-user-action-engine](../specs/approval-workflow/workflows/WF-M18-05-tblaction-user-action-engine.md) | WF-M18-05 — Per-user action-permission engine (`tblAction*`, `ClsApprovalAction`) |
