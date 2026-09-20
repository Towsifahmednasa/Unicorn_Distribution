# Specs

Feature/page specifications recovered from the running code (nothing was specified before). One spec per `.aspx` page: `specs/<module>/pages/PAGE-nnn-<name>.md` with 31 fixed sections and machine-readable front matter (`tier`, `coverage`).

- **Tier A** (165 pages): reviewed against markup, code-behind and BLL/DAL chain; business rules `BR-Mxx-nnn`.
- **Tier B** (541 pages): mechanical extraction; treat as leads.
- [SPEC-COVERAGE.md](SPEC-COVERAGE.md) — per page/dimension status. [SPEC-GAPS.md](SPEC-GAPS.md) — gaps (574 module gaps + system gaps).

| Module | Name | Pages | Rules | Workflows | Report docs | Gaps |
|---|---|---|---|---|---|---|
| M01 | [Authentication, Session & Navigation](authentication-navigation/README.md) | 11 | 0 | 5 | 1 | 25 |
| M02 | [User, Role & Permission Administration](user-role-permission/README.md) | 16 | 0 | 7 | 0 | 31 |
| M03 | [Organisation & Market Structure Master](organisation-market-structure/README.md) | 75 | 0 | 7 | 0 | 24 |
| M04 | [Product, Pricing & Promotion Master](product-pricing-promotion/README.md) | 64 | 0 | 6 | 0 | 33 |
| M05 | [Customer & Provider Master](customer-master/README.md) | 40 | 0 | 8 | 3 | 32 |
| M06 | [Order Management](order-management/README.md) | 29 | 0 | 6 | 3 | 40 |
| M07 | [Proforma, Invoicing & Delivery](invoicing-delivery/README.md) | 43 | 0 | 5 | 2 | 39 |
| M08 | [Sales Return & Invoice Return](sales-return/README.md) | 20 | 0 | 4 | 5 | 26 |
| M09 | [Collections, Payments & Money Receipts](collection-payment/README.md) | 43 | 0 | 7 | 7 | 40 |
| M10 | [Warehouse, DC Stock & Inventory](warehouse-inventory/README.md) | 56 | 0 | 10 | 10 | 33 |
| M11 | [Stock Transfer & Chalan Receive](stock-transfer/README.md) | 28 | 0 | 4 | 3 | 21 |
| M12 | [Sub-Depot Operations](sub-depot/README.md) | 37 | 0 | 6 | 8 | 31 |
| M13 | [Sales Reports, Analytics & Dashboards](sales-reports-dashboard/README.md) | 75 | 0 | 6 | 8 | 33 |
| M14 | [Target & DWSP Management](target-management/README.md) | 24 | 0 | 3 | 2 | 21 |
| M15 | [HR, Field-Force & Employee Administration](hr-field-force/README.md) | 74 | 0 | 6 | 5 | 27 |
| M16 | [Doctor / Provider Master, Visits & Prescriptions](doctor-visit-prescription/README.md) | 40 | 0 | 5 | 4 | 66 |
| M17 | [SAP, Mobile-App & External Integration](sap-external-integration/README.md) | 19 | 0 | 4 | 0 | 18 |
| M18 | [Approval Workflow](approval-workflow/README.md) | 12 | 0 | 5 | 0 | 34 |

Other: [00-system/](00-system/) (overview, navigation, roles/permissions, status lifecycle, integration map), [integrations/](integrations/README.md).

Workflow for changes: `.claude/SKILL.md`. Regenerate coverage/inventories with `docs/reverse-engineering/tools/README.md`.
