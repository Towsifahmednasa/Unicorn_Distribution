# RPT-M05-02 — Verified / Unverified customer upload rows (Crystal → Excel)

## Pages
| Role | Page | Path |
|---|---|---|
| Launcher (counts as link buttons) | `CustomerList.aspx` (PAGE-193) — customer upload batches; `CustomerTagChangeList.aspx` (PAGE-198) — tag-change batches | `Solution.Web/SInventory_UI/` |
| Viewer / exporter (PAGE-181) | `CustVerUnverExcelViewer.aspx` | `Solution.Web/SInventory_RPTVIEW/CustVerUnverExcelViewer.aspx(.cs)` |

## Report file and dataset
- `.rpt`: `crpCustomerMaster.rpt` (same layout as RPT-M05-01) from `Solution.Web/Reports/CrystalReports/`.
- Datasets: `custMasterDetailTable` (staged rows aliased to the customer-master column names) + `companyInfoDataTable` (company header, `CompanyId='1'`).
- Output: `ExportToHttpResponse(ExcelRecord, …, "CustomerMaster_Report")` — Excel download (no on-screen viewer).

## Data source
Inline SQL in `ExcelUpForMIGODAL.GetVerificationReport` (`:302-309`) for `status` 1/2 (customer upload) and the equivalent in `ExcelUpForCustTagChangeDAL` for 3/4 (tag change):
`SELECT BRANCH AS ComUnitCode, BRANCHDES AS ComUnitName, CustomerCode, CUSTOMERNAME AS CustomerName, ADDRESS1 AS Address, ADDRESS2 AS Addrees2, CITY AS City, CONTACTPERSON AS ConPerson, CONTACTNUMBER AS CellNo, MIOCode AS MiaCode, MIOName AS MiaName, TerritoryCode AS AreaCode, FECode AS DistrictCode, FEName, DZSMCode AS RegionCode, DZSMName, SHIPPINGCOND AS ShippingCond, SHIPPINGPOINT AS MarketCode, MarketName, TERMOFPAYMENT AS TermOfPayment FROM dbo.tblCustomerMasterExcelFileDetail WHERE MasterID=@MasterID AND Verifyed=@Verifyed` (tag change: `tblCustomerMasterTagChangeExcelFileDetail`). Parameterised.

## Request parameters
QueryString `status` and `id`: `1` = verified rows of customer upload batch `id`; `2` = unverified; `3` = verified tag-change; `4` = unverified tag-change (`CustVerUnverExcelViewer.aspx.cs`). Any other value ⇒ nothing is exported (blank page). `id` = batch master id (`CustomerMasterExcelFileMasterID` / `CustomerTagChangeExcelFileMasterID`).

## Filters on the list page
List filtered by manufacturer only (`Transfer=0` batches). Links show the counts of verified / unverified rows (`COUNT(*)`), populated by `PlaceVerfiyUnverify` (N+1 queries per grid row).

## Output / export
Excel with the staged rows so users can fix unverified customers and re-upload. The class name in the code-behind is `SInventory_RPTVIEW_CustomerMasterViewer` (copied from the master viewer), only its file name differs.

## Business logic / notes
Report reflects staging status only; the meaning of `Verifyed` is set by `sp_VerifyCustomer` (body UNKNOWN). No session/role check in the viewer.

## Gaps
Copy/paste class naming; four near-identical blocks; no `status` validation (GAP-M05-024).
