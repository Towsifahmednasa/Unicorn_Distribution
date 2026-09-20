# Database functions (UDFs) - definitions absent, calls documented (Part A)

## What is verified

- **No function is defined anywhere in the repository.** A case-insensitive search for `CREATE [OR ALTER] FUNCTION` / `ALTER FUNCTION` across all `*.sql`, `*.cs`, `*.aspx`, `*.config`, `*.txt` and `*.md` files (excluding `PrecompiledWeb`, `graphify-out` and generated `docs/`/`specs/`) returned **zero matches**. The 20 SQL scripts define only stored procedures, `ALTER TABLE` and one `CREATE INDEX` (see `stored-procedures.md`, `tables.md`).
- **Functions are nevertheless called by the application and by the scripts.** Three user-defined functions were found by scanning for schema-qualified calls (`dbo.<name>(`, `[dbo].[<name>](`), `fn_/udf_/ufn_` names, `FROM/JOIN <name>(` and `CROSS/OUTER APPLY <name>(` in `Library.DAL`, `Library.BLL`, `Solution.Web` (`*.cs`, `*.aspx`, `*.master`, `*.asmx`) and all `*.sql`. All three have **no body in the repo**.

## User-defined functions called (bodies not in the repository)

| Function | Kind (INFERRED from call shape) | Called from | Parameters / result as used by caller | Purpose (as far as evidenced) |
|---|---|---|---|---|
| `dbo.MainMenu2(@UserId, @UserRoleID)` | scalar function returning text/HTML (aliased `MenuHTML`) | `Library.DAL/SInventory_DAL/UserDAL.cs:35` `UserDAL.LoadMenu` -> `Library.BLL/SInventory_BLL/UserBLL.cs:67-69` -> `Solution.Web/MasterPages/NewMasterPage.master.cs:73` (`aUserBll.LoadMenu(Session["UserId"], Session["UserRoleID"])`); a commented-out older call is at `Solution.Web/MasterPages/MasterPage.master.cs:37` | statement: `SELECT dbo.MainMenu2(@UserId,@UserRoleID) AS MenuHTML`; both parameters passed as strings via `SInventorySql.DbValue` (target types UNKNOWN); returns one row / one column `MenuHTML` | Generates the role-specific navigation menu HTML for the master page. The brief also records a separate menu SP `sp_GET_MainMenuRole2` (`NewMasterPage.master.cs`); how the two relate is UNKNOWN. **What menu items, permissions tables (`tblMainMenu`, user-permission tables) and filters the function applies is UNKNOWN.** |
| `dbo.Remove_SpecialCharacters(@text)` | scalar function (string in -> string out) | `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:828`, inside `sp_Get_CustMasterList_Approve` (`... dbo.Remove_SpecialCharacters(Address) Address ...`); the only occurrence in scripts, C# or aspx | one string argument (`tblCustMaster.Address`); returned value replaces the address column in the list output | Sanitises the customer address for the grid/report output (strips characters such as line breaks/special symbols - exact character set UNKNOWN). |
| `GetmonthlycustomerSaleDate(@FromDate, @ToDate)` | **table-valued function** (used as `SELECT * FROM GetmonthlycustomerSaleDate(...)`, unqualified - resolves through the caller's default schema, presumably `dbo`) | `Library.DAL/SInventory_DAL/TotalSummaryDAL.cs:90` `TotalSummaryDAL.GetMonthYearWiseSale`, called in a month loop from `Library.BLL/SInventory_BLL/TotalSummaryBLL.cs:76` (report on customer-wise monthly sales) | two DateTime parameters (first day .. last day of a month); result columns UNKNOWN (consumed by index/name in `TotalSummaryBLL` pivoting into a table with a `Total` column) | Monthly customer sales aggregation. Called once per month in a loop -> N round trips; internals UNKNOWN. |

## Database-side objects that look like functions but are not

- `View_Webapi_EmployeeFieldForceInfo` (used in `sp_Get_CustomerApp`) is a **view**, not a function - see `docs/database/views.md`.
- `sp_Sales_DeliveryCOGSPosting` (called by `sp_Sales_CustomerPaymentPosting`, `Live_Deployment_Script_All_Updates.sql:548`) is a stored procedure with no body in the repo.

## Built-in functions with business relevance used in the scripts

Only those that carry behaviour or version requirements are listed (trivial `ISNULL/CAST/CONVERT/GETDATE/ABS/SUM` patterns are omitted):

| Built-in | Where | Why it matters |
|---|---|---|
| `STRING_AGG(...)` | `SP_Update_StockValuation_TransactionDate_Logic.sql:215-216` (`sp_Report_NationalStockValuation_DateRange`) | builds the dynamic PIVOT column list -> requires **SQL Server 2017+** compatibility level 140+ |
| `QUOTENAME(...)` | same, `:215-216` | quoting of depot column names (but the alias uses manual `[` `]` concatenation - injection/escape gap) |
| `FORMAT(date, 'dd MMM yyyy hh:mm tt')` / `FORMAT(date,'dd-MMM-yyyy')` | `sp_Get_CustomerApp`, `sp_Get_CustMasterList_Approve`, `sp_Sales_CustomerPaymentPosting` | CLR-based, slow on large sets; requires SQL 2012+ |
| `STUFF(... FOR XML PATH(''))` | `sp_Get_CustMasterList_Approve`, `sp_Get_DoctorList` | string aggregation of tagged sales persons, chambers, contacts, degrees, specialties (pre-STRING_AGG idiom) |
| `ROW_NUMBER() OVER (...)` | ledger/national reports, ageing report | latest-cost / latest-weighted-average selection |
| `SCOPE_IDENTITY()` | save SPs | returned ids |
| `SYSUTCDATETIME()` | `sp_Up_ProviderDropoutIntrigrationApprove` | approval time stored in **UTC** (other columns use local `GETDATE()`) |
| `COL_LENGTH`, `OBJECT_ID`, `INFORMATION_SCHEMA.COLUMNS` | DDL guards | idempotent schema scripts |
| `ERROR_MESSAGE()` + `RAISERROR` | `sp_Adjust_DCStore_Stock_Only`, `sp_Del_B2BDelete` v2 | error re-raise (severity 16, state 1; original error number lost) |
| `CREATE OR ALTER PROCEDURE` (statement, not function) | `Live_Deployment_Script_All_Updates.sql` | requires SQL Server 2016 SP1+ |

## What is UNKNOWN

1. The definition, parameters' exact data types, determinism and schema of the three UDFs above (`MainMenu2`, `Remove_SpecialCharacters`, `GetmonthlycustomerSaleDate`), and whether any of them read other database objects (menu tables, permissions).
2. Whether other UDFs exist that are called only from stored procedures whose bodies are not in the repo (~1,000 SPs) - such calls are invisible; the list above covers only call sites visible in C#, ASPX and the 20 scripts.
3. Whether calls inside dynamically assembled SQL in C# (string concatenation across methods) invoke additional functions; the scan was pattern-based over literal SQL text.
4. Computed columns or scalar UDFs referenced inside views (`docs/database/views.md`) are not resolvable from the repo.
5. Crystal Reports (`*.rpt`) may embed function calls in their command text; binary `.rpt` files were not parsed.
