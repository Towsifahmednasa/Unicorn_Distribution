# RPT-M01-01 — Employee location tracking (map + list + Excel)

| Item | Value |
|---|---|
| Viewer / list page | `Solution.Web/UserTracking/UserTrackingList.aspx` (PAGE-011) |
| Crystal `.rpt` | none (no `Solution.Web/Reports/CrystalReports` or `Library.CrystalReports` file is referenced) |
| Data source | SP `sp_Get_UserLocationTracking(@empid int, @trackDate datetime)` via `CommonDataLoad.GetUserLocationTracking` (`Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:164-181`); body **not in repo** — UNKNOWN |
| Request parameters | POST JSON `{empId, trackDate}` to `UserTrackingList.aspx/GetUserLocationTrackig`; no QueryString/Session parameters |
| Filters (UI) | Employee (select2 from `ExpenseClaim.aspx/GetEmployeeList_Active`), Date (pickadate) |
| Columns consumed by the UI | `AddressName`, `Time`, `LatValue`, `LongValue` (other SP columns UNKNOWN) |
| Output | Leaflet map (OSM tiles): start marker, intermediate markers, end marker, polyline; table Sl/Time/Location |
| Export | client-side: table `innerHTML` wrapped as HTML and downloaded as `Employee_Location_Report.xls` (`application/vnd.ms-excel`), no server round-trip |
| Client logic | drop blank/"null" addresses; sort by parsed time-of-day; unparsable times sort as 0 |
| Security | endpoint has no session/role check (PAGE-011 §20) |
| Business rules | BR-M01-030, BR-M01-031 |

## Not a report (for the avoidance of doubt)
`tblUserSessionTracking` (login sessions: IP, geo, browser, OS, device, login/last-activity/logout) is written by `UserSessionTrackingManager` but **no page in the repository reads it**, so there is no login-audit report. `tblLoginLog` is never written.
