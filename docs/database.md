# Database Documentation

**Source available:** no schema; 41 stored-procedure bodies from 20 SQL scripts; everything else reverse-engineered from C# call sites. Read [reverse-engineering/database-inventory.md](reverse-engineering/database-inventory.md) first for what is and is not known.

| Object class | Verified in repo | Known by reference |
|---|---|---|
| Stored procedures | 41 bodies | 1034 names (1005 `sp_*`-convention) |
| Tables | ~60 touched by scripts (partial columns) | 228 names in inline SQL |
| Views | 0 definitions | 9 (`View_*`) |
| Functions | 0 definitions | 3 called (`dbo.MainMenu2`, `dbo.Remove_SpecialCharacters`, `GetmonthlycustomerSaleDate`) |
| Triggers | 0 | UNKNOWN |
| Constraints / indexes | 1 index, 1 PK, 8 defaults (tblUnitPriceRollback etc.) | UNKNOWN |

## Files
- [database/tables.md](database/tables.md) — script-derived column facts + generated table index (columns observed in code).
- [database/stored-procedures.md](database/stored-procedures.md) — Part A: full analysis of 41 SP bodies (duplicates/conflicts flagged); Part B: generated interface index of all 1034 names.
- [database/views.md](database/views.md), [functions.md](database/functions.md), [triggers.md](database/triggers.md), [relationships.md](database/relationships.md) (inferred joins).

## Data-access patterns
Stored procedures via `DataAccessManager`; inline parameterized SQL via `ClsCommonInternalDAL`/`SInventorySql`; string-concatenated SQL in several DAL methods and pages (SQL-injection risk; see `docs/security.md`). PKs are frequently `MAX+1`. Databases: main `SalesDisDB_Unicorn_DB`, accounts DB (cross-DB voucher posting), SAP staging DB, runtime archive DB (`tblArcDBConnect`).

## Top data-integrity findings (from SP scripts and module reviews)
- Two coexisting stock models (in-place quantity vs ledger of negative issues); `sp_Adjust_DCStore_Stock_Only` can re-inflate stock.
- Voucher `TableID 2018` shared by `ReturnInvoiceId` and `CustReturnId`; returns can be netted against wrong invoices.
- Most writers have no transaction; dynamic SQL concatenated in 7 SPs; customer/doctor save SPs hard-code Approved.
