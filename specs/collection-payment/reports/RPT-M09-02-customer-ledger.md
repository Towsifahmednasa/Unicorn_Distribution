# RPT-M09-02 — Customer Ledger family

| Item | Value |
|---|---|
| Filter pages | `SInventory_UI/CustomerLedgerReport.aspx(.cs)` (PAGE-307), `CustomerLedgerReportApps.aspx(.cs)` (PAGE-308, grid version, `EMPID` querystring for app use), `CustomerLedgerReportNew.aspx(.cs)` (PAGE-309) |
| Viewer | `SInventory_RPTVIEW/CustomerLedgerViewer.aspx(.cs)` (PAGE-301) |
| Crystal file | `Solution.Web/Reports/CrystalReports/crpCustomerLedger.rpt`, dataset table name `DataTabledsCustomerLedger` |
| SP | `sp_CustomerLedger(@Cust,@fromdate,@todate)` and `sp_CustomerLedgerNew(...)` via `CustomerLedgerDAL.CustomerLedgerDal/CustomerLedgerDalNew` (`Library.DAL/SInventory_DAL/CustomerLedgerDAL.cs:14-54`) — **bodies NOT in repo**; ledger columns/opening balance/running balance logic UNKNOWN |
| Request params | QueryString `CustomerID`, `fromDate`, `toDate` (viewer). Page: customer chosen through autocomplete text `Code : Name \| Id`; `hfCustomerId` = text before ':' i.e. the **customer code**; From/To optional |

## Filters / behaviour
- Customer required: "Please Insert Valid Customer Code!!" if `hfCustomerId` empty (PAGE-307 :77-90).
- Dates optional; blank strings converted to NULL in the DAL. Dates passed in the URL as `DateTime?.ToString()` (culture-dependent text) and re-converted by the viewer — potential locale mismatch.
- PAGE-308 calls `CustomerLedgerBll(CustomerID, from, to)` and shows a grid instead of Crystal.
- Viewer binds the report inline (`crvCustMasterRpt`); no export code (Crystal toolbar only).

## Data semantics
Not verifiable (SP absent). The ledger presumably combines invoices (debit) and payments incl. discount/AIT (credit) — INFERRED only; compare with `sp_GET_PaymentInvSP` / aging definitions (they add discount and AIT to "paid").

## Risks
Customer id is a code not a key; unhandled exceptions in viewer (`Page_Init` has no try/catch); unrestricted access to any customer's ledger by URL (`CustomerLedgerViewer.aspx?CustomerID=…`) — no session/role check in viewer code.
