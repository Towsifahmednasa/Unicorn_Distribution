# RPT-M05-03 — Customer list CSV exports (non-Crystal)

Not Crystal reports; page-generated CSV files from grid data. Listed because they are the main "extract" outputs of the module.

| Page | Trigger | File name | Data source | Columns |
|---|---|---|---|---|
| `MasterSetup_UI/CustomerView.aspx` (PAGE-173) | `btnExportToExcel_Click` (`:580-630`) | `Customer_List_<dd_MMM_yyyy_hh_mm_tt>.csv` (content-type `text/csv`) | `LoadData(Parm())` → `sp_Get_CustMasterList_Approve` with paging disabled | visible grid columns: Customer Code, Customer Name, Salaes Person Name, Owner Name, Mobile NO, Address, Approval Status, Entry Date (hidden columns are not exported) |
| `MasterSetup_UI/CustomerChangeProgramType.aspx` (PAGE-167) | `btnExportToExcel_Click` (`:336-390`) | same name pattern | separate hidden grid `gv_Export` bound to `GetCustomerList(Parm())` | Customer Code/Name, **Provider Type Code**, Provider Type, Owner, Mobile, Address, Zone/Area/Territory/Market codes + Market Name, Division/District/Thana |
| `MasterSetup_UI/CustomerListPending.aspx` (PAGE-169) | `btnExportToExcel_Click` (`:276`) | same pattern (INFERRED clone of PAGE-173) | `GetCustomerList` | as its grid |

## Request parameters
None in query string; filters come from the posted control values via the concatenated `Parm()` (see PAGE-173/167 §8). Session: `UserRoleID`/permissions only used at page load.

## Behaviour
- Requires the grid to already contain rows (`loadGridView.Rows.Count > 0`), else alert "No Data Found!".
- Encoding `Encoding.Default`; cells containing "," are wrapped in quotes (embedded quotes are not escaped, and the quoted branch skips `HtmlDecode`); other cells are HTML-decoded and appended with a trailing comma — CONFIRMED (`CustomerView.aspx.cs:604-614`).
- `Response.End()` terminates the request (ThreadAbort) — normal WebForms pattern.

## Gaps
No row limit; no role scoping; injection via `Parm()` (GAP-M05-009); exports exactly what the user's filter selects (data-leak surface for customer mobile numbers/addresses).
