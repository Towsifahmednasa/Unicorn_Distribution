# System overview

One-page orientation for developers. Sources: [../../docs/architecture.md](../../docs/architecture.md), [../../docs/reverse-engineering/system-inventory.md](../../docs/reverse-engineering/system-inventory.md), [../../docs/reverse-engineering/module-inventory.md](../../docs/reverse-engineering/module-inventory.md). Tags: CONFIRMED / INFERRED / UNKNOWN.

## What it is

An ASP.NET WebForms (.NET Framework 4.0, Visual Studio 2010, Web Site project) sales and distribution ERP with a pharma field-force layer: orders, proforma/invoice, delivery, collections, DC/warehouse and sub-depot stock, transfers, returns, targets, HR/field force, doctor visits and prescriptions, SAP hand-offs, dashboards and about 112 Crystal reports. About 706 pages, 4 class libraries, one SQL Server database `SalesDisDB_Unicorn_DB` (plus accounts and ERP databases reached by cross-db names). Business logic is largely in stored procedures whose bodies are mostly not in this repo. Android apps (APKs in `Solution.Web/APK_File/`) use `*Apps.aspx` pages and WebMethods.

## Actors (INFERRED from code and vocabulary)

Admin users (UserType Admin, `IsMainDashboard` dashboard), head-office sales/accounts/warehouse staff, depot (DC) and sub-depot users, sales hierarchy (NSM/RSM/ASM/AM/DZSM approvers), field officers (MIO/MIA) and distributor assistants (DA, login = DA code) using the mobile apps, doctors/providers as master data. Role names are database data (UNKNOWN); authorization is mainly menu-driven ([security](../../docs/security.md)).

## Module map

| Id | Module | Spec |
|---|---|---|
| M01 | Authentication, session, navigation | [README](../authentication-navigation/README.md) |
| M02 | User, role, permission administration | [README](../user-role-permission/README.md) |
| M03 | Organisation and market structure master | [README](../organisation-market-structure/README.md) |
| M04 | Product, pricing, promotion master | [README](../product-pricing-promotion/README.md) |
| M05 | Customer and provider master | [README](../customer-master/README.md) |
| M06 | Order management | [README](../order-management/README.md) |
| M07 | Proforma, invoicing, delivery | [README](../invoicing-delivery/README.md) |
| M08 | Sales return and invoice return | [README](../sales-return/README.md) |
| M09 | Collections, payments, money receipts | [README](../collection-payment/README.md) |
| M10 | Warehouse, DC stock, inventory | [README](../warehouse-inventory/README.md) |
| M11 | Stock transfer and chalan receive | [README](../stock-transfer/README.md) |
| M12 | Sub-depot operations | [README](../sub-depot/README.md) |
| M13 | Sales reports, analytics, dashboards | [README](../sales-reports-dashboard/README.md) |
| M14 | Target and DWSP management | [README](../target-management/README.md) |
| M15 | HR, field force, employee administration | [README](../hr-field-force/README.md) |
| M16 | Doctor master, visits, prescriptions | [README](../doctor-visit-prescription/README.md) |
| M17 | SAP, mobile-app, external integration | [README](../sap-external-integration/README.md) |
| M18 | Approval workflow | [README](../approval-workflow/README.md) |

(Module READMEs are produced per module; a link may not resolve until that module is documented.)

## Architecture summary

`Browser/APK -> IIS (Solution.Web: master page + .aspx.cs, WebMethods, Crystal viewers) -> Library.BLL (SInventory/SubDepot/Panal only) -> Library.DAL -> SQL Server`, entities in `Library.DAO`, reports in `Library.CrystalReports` and `Solution.Web/Reports/CrystalReports`. 274 pages call the DAL directly. Four data-access styles coexist (`DataAccessManager`, EntLib `ClsCommonInternalDAL` with generic SQL-text SP, `SInventorySql`, `DB_Manager`); PKs are often `MAX+1` in code. Login: inline SQL on `tblUser` with plaintext password compare, Session-only state (Forms auth configured but never issued), InProc session. Menu built in DB (`dbo.MainMenu2`). No background jobs, no logging, no automated tests.

## Main data flow: order to cash (as evidenced by page/table names)

1. Master data: company/unit/market structure (M03), products and unit price (M04), customers (M05; `tblCustMaster`).
2. Order: manual, Excel upload (`OrderListUpload`), requisition or mobile app -> `tblOrder`/`tblOrderDetail` with `ActionStatus`, `IsInvoice`, `IsPrepareforInvoice` (M06); approval (M18).
3. Proforma and invoice: `ProformaInvoiceCreation`, invoice by order (manual/auto) -> `tblInvoice`, `tblInvoiceDetail`, DC stock `tblDCStore` reduced (M07, M10); invoice number and `MAX+1` key in `InvoiceBLL.SaveInvoice`.
4. Picking, challan/top sheet, `LoadingSummary` -> delivery status Full Dues / Cash / Rejection; final submit runs `sp_DeliveryConformationFull_New`, `sp_UP_LoadingSummary`, `sp_Sales_DeliverySalesPosting` as separate calls (`LoadingSummary.aspx.cs:602-660`) posting vouchers to `CSTL_ACCDB_Unicorn`.
5. Collection: `CustomerPayment`, money receipt, deposit-slip upload, ledger and aging (M09); payment posting `sp_Sales_CustomerPaymentPosting`.
6. Returns: `SalesReturn`, return invoices, delivery-return posting (M08) restoring DC stock.
7. Supply side: stock transfer DC to DC/warehouse/sub-depot, STO, chalan receive (M11, M12), material conversion and SAP MIGO (M10, M17).

## Where to look

| Need | Location |
|---|---|
| Login and session | `Solution.Web/Login.aspx.cs`, `MasterPages/NewMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |
| Data access core | `Library.DAL/DataManager/`, `Library.DAL/InternalCls/`, `Library.DAL/MAIN_FUNCTION/` |
| Invoicing/delivery | `Library.BLL/SInventory_BLL/InvoiceBLL.cs`, `Library.DAL/SInventory_DAL/InvoiceDAL.cs`, `Solution.Web/SInventory_UI/LoadingSummary.aspx.cs` |
| Reports | `Solution.Web/SInventory_RPTVIEW/`, `Solution.Web/Reports/CrystalReports/`, `Library.CrystalReports/` |
| SP bodies in repo | root `*.sql`, `Library.DAL/SAP_IntegrationDAL/sql/` |
| Page-level specs | `specs/<slug>/pages/PAGE-NNN-*.md`; inventories in `docs/reverse-engineering/` |
| Security, deployment, testing, conventions | [security](../../docs/security.md), [deployment](../../docs/deployment.md), [testing](../../docs/testing.md), [coding-standard](../../docs/coding-standard.md) |
