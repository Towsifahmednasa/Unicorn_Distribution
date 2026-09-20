---
page_id: PAGE-011
module: M01
page: UserTrackingList
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-011 — UserTrackingList ("Location Tracking")

> **Evidence level:** Tier A for page logic (aspx + JS + code-behind + `CommonDataLoad.GetUserLocationTracking`). SP `sp_Get_UserLocationTracking` body is not in the repository (database PARTIAL). **This page is NOT the viewer of the login/session tracking table** written at login (`tblUserSessionTracking`); it shows an employee's field GPS trail for a day.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-011 |
| Module | M01 (by folder `UserTracking`); functionally a field-force monitoring report |
| Kind | Search + map + list (report-like) |
| Master | `~/MasterPages/NewMasterPage.master` |
| Class | `UserTracking_UserTrackingList` |
| Size | aspx 328 lines, code-behind 28 |

## 2. Purpose
Pick an employee and a date and see the places recorded for that employee that day on an OpenStreetMap (Leaflet) with start/intermediate/end markers and a polyline, plus a Sl/Time/Location table exportable to Excel.

## 3. Business Objective
Let management review where a field employee (mobile-app user) was during a day. INFERRED (labels "Location Tracking", "Employee Name", "Started at / Ended at").

## 4. Navigation
No inbound link found in code (menu row is DB data — UNKNOWN). Outbound: none.

## 5. Route
`~/UserTracking/UserTrackingList.aspx`; AJAX endpoint `UserTrackingList.aspx/GetUserLocationTrackig` (sic).

## 6. Source Files
`Solution.Web/UserTracking/UserTrackingList.aspx(.cs)`, `Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:164-181`, `Solution.Web/CustomScript/_QuickDataAccess.js:77-97` (`_getEmployeeList_Active`), `Solution.Web/DoctorModule_UI/ExpenseClaim.aspx.cs:92` (employee list WebMethod). External: Leaflet 1.7.1 from `unpkg.com`, OSM tiles.

## 7. UI Control Inventory
| Control | Type | Label | Required | Notes |
|---|---|---|---|---|
| `#ddlEmpName` | `<select>` (select2) | Employee Name * | client: value must be non-zero | options loaded via `_getEmployeeList_Active` (option `0` = "--- Select Employee ---") |
| `#dashboardDate2` | text + pickadate | Date | client: non-empty | free text |
| Search button | `GetUserLocation()` | Search | – | – |
| Reset button | `ResetClick()` → `window.location.reload()` | Reset | – | – |
| `#mapid` | div 400px | – | – | Leaflet map |
| Export to Excel | `exportTableToExcel()` | – | – | client-side HTML-as-xls Blob `Employee_Location_Report.xls` |
| `#tblUserLocation` | table Sl / Time / Location | – | – | "No data found" row when empty |

## 8. Field Mapping
| UI | JS var | AJAX param | Server | SQL param |
|---|---|---|---|---|
| `#ddlEmpName` | `empId` | `empId` (int) | `GetUserLocationTrackig(int empId, DateTime trackDate)` | `@empid` |
| `#dashboardDate2` | `trackDate` | `trackDate` (string→DateTime by model binding) | same | `@trackDate` |

## 9. Dropdown Mapping
`ddlEmpName`: `ExpenseClaim.aspx/GetEmployeeList_Active` → `CommonDataLoad.GetEmployeeList_Active` (value `EmpInfoId`, text `EmpName`). SP/SQL for it is in the DoctorModule area (not read here).

## 10. Button Action Mapping
Search → `$.ajax POST UserTrackingList.aspx/GetUserLocationTrackig` (JSON). Only fires when `empId != 0` and `trackDate != ""`; otherwise nothing happens and no message is shown.

## 11. Frontend Flow
`ready`: load employee list (synchronous `async:false`), init pickadate. On response: parse `data.d` JSON → drop rows whose `AddressName` is empty/"null" → sort by `Time` using `GetTimeSortValue` (accepts `h:mm[:ss][AM|PM]`, other formats sort as 0) → `LoadMap` (first row = start marker "Started at :", last = "Ended at :" when >1 row, polyline through all, view zoom 13 on the start; default map centre Dhaka 23.810331, 90.412521) → `BindLocationTable`. Table cells are built by string concatenation without HTML-encoding `Time`/`AddressName` (DOM-XSS sink if location text is attacker-controlled; INFERRED risk).

## 12. Backend Flow
`[WebMethod] static GetUserLocationTrackig` → `CommonDataLoad.GetUserLocationTracking(empId, trackDate)` → open `DataBase.SalesDB`, `GetDataTable("sp_Get_UserLocationTracking", @empid, @trackDate)`, close in `finally` → `JsonConvert.SerializeObject(DataTable)`. No try/catch (rethrow), no session read.

## 13. API Mapping
| Endpoint | Method | Inputs | Output |
|---|---|---|---|
| `UserTrackingList.aspx/GetUserLocationTrackig` | POST WebMethod | empId, trackDate | JSON string of DataTable (columns used by JS: `AddressName`, `Time`, `LatValue`, `LongValue`) |

## 14. Service Mapping
None (page → DAL).

## 15. Repository Mapping
`CommonDataLoad.GetUserLocationTracking` (`CommonDataLoad.cs:164`).

## 16. Stored Procedure Mapping
`sp_Get_UserLocationTracking(@empid,@trackDate)` — body not in repo; result columns beyond those used by JS UNKNOWN.

## 17. Database Mapping
UNKNOWN tables (SP body absent). Presumably a GPS/location table fed by the mobile app — INFERRED.

## 18. Validation Matrix
| Field | Rule | FE | BE | Message | BR |
|---|---|---|---|---|---|
| Employee | must be selected (`!= 0`) | yes | no | none (silent) | BR-M01-030 |
| Date | non-empty | yes | no (`DateTime` binding fails on garbage) | none | BR-M01-030 |

## 19. Business Rules
| ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M01-030 | A trail is requested only when an employee and a date are chosen; locations without an address name are discarded; the trail is ordered by the time-of-day string; first/last points are labelled start/end. | `UserTrackingList.aspx:136-176,258-296` | CONFIRMED |
| BR-M01-031 | Excel export is a client-side HTML table saved as `.xls` (`Employee_Location_Report.xls`) — no server export. | `UserTrackingList.aspx:297-316` | CONFIRMED |


## 20. Permission Matrix
`Session` is not read in the code-behind. The master-page guard runs only on the page's own first GET; the WebMethod is a static page method: it is invoked directly at `.../UserTrackingList.aspx/GetUserLocationTrackig` without executing the master page, and contains no session/role check → **callable without a session** by anyone who can reach the server (CONFIRMED absence of check; no `<authorization>` in `Web.config`). Any authenticated user can also query any employee id (no scoping to the caller's hierarchy).

## 21. Status Lifecycle
None.

## 22. Error Handling
None server-side; AJAX has no `error` callback (silent failure).

## 23. Audit
None (no record of who viewed whose location).

## 24. Integration
Browser loads Leaflet/CSS from `unpkg.com` with SRI hashes and tiles from `tile.openstreetmap.org` (external dependencies; needs internet).

## 25. Reports
Behaves as a report (map + table + Excel). No Crystal `.rpt`. See `reports/RPT-M01-01-user-location-tracking.md`.

## 26. Dependencies
`CommonDataLoad`, `DataAccessManager`, `Newtonsoft.Json`, `_QuickDataAccess.js`, select2, pickadate, Leaflet.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Employee + date with points | map with start/end markers, table sorted by time | 030 |
| TC-02 | No employee chosen | nothing happens | 030 |
| TC-03 | Points with blank addresses only | "No data found", map cleared | 030 |
| TC-04 | Export | `.xls` with table HTML | 031 |
| TC-05 | POST endpoint with no session cookie | JSON returned (no auth) | – |
| TC-06 | Times in 24h `HH:mm:ss` | sorted correctly; unusual formats sort as 0 | 030 |

## 28. Reverse Engineering References
`Solution.Web/UserTracking/UserTrackingList.aspx(.cs)`, `Library.DAL/DoctorModule_DAL/CommonDataLoad.cs`.

## 29. Spec vs Actual
| Area | Specification | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| Session-tracking viewer | none | no page reads `tblUserSessionTracking` | GAP-M01-008 |

## 30. Known Gaps
GAP-M01-021 (unauthenticated data endpoint, no scoping, unencoded output, external CDN), GAP-M01-008 (login session tracking has no viewer).

## 31. Change Impact
`CommonDataLoad` is a large shared DAL class (`GetPermissionForUserRole` etc.). `sp_Get_UserLocationTracking` used only here (grep). `_getEmployeeList_Active` shared by many pages.
