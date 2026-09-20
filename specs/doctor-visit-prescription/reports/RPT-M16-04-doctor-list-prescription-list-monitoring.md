# RPT-M16-04 — Doctor list (CSV), Prescription list and Doctor Visit Monitoring dashboard

## A. Doctor list / export — `MasterSetup_UI/DoctorView.aspx` (PAGE-670, Tier B; read for this document)
- Data: `DoctorDAL.GetDoctorList(param)` → `sp_Get_DoctorList(@Parm)` — **body in repo** (`LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:884-930`): dynamic SQL over `tblDoctorMaster DM` with joins to designation, program type, doctor type, market/route/company unit ("Dept"), station type (MIO role 1), sub-territory → territory → area → region → group, SMC type, entry/update users; subqueries (`STUFF … FOR XML PATH`) for chamber info (`Chamber Type/Name/Phone/Address`), contacts, degrees, specialities, program types; `where DM.DoctorId is not null and isnull(DM.IsDoctor,0)=1 <@Parm> ORDER BY DM.DoctorCode DESC`; `ApprovalStatusWeb` = Pending/Verified/Approved/Rejected for `'0'/'1'/'2'/'3'`; `SalesPersonName` = tagged sales persons `EmpMasterCode : EmpName` (comma list) else the single `SalesPersonId` employee.
- Filters (`Parm()` `DoctorView.aspx.cs:417-504`): area/territory/sub-territory/market (from the hierarchy control — which is forced `Visible=false` at `:49`, so these are inert), program type, pharma platform, sales person (`DM.SalesPersonId=<id> OR (','+ISNULL(Tagged,'')+',') LIKE '%,<id>,%'`), doctor type, degree (`DM.DoctorId in (select DoctorId from tblDoctorDegreeDetail where DegId=…)`), active status, approval status, doctor (autocomplete id), `CONVERT(date,DM.EntryDate) BETWEEN from AND to` (default from = today-30 days, to = today).
- Export: `btnExportToExcel_Click` streams **CSV** (`Doctor_List_<dd_MMM_yyyy_hh_mm_tt>.csv`, `Encoding.Default`), all pages, all visible columns except "Edit" (`:232-287`); empty grid ⇒ "No Data Found!".
- Permission: `UserPersmissionValidation` (`:166-198`): users whose `Session["UserRoleID"]` ≠ "2" need a row from `sp_GET_MainPermissionByUserRoleandPageUrl` for this URL (else redirect to `Dashboard_UI/DashboardOne.aspx`); `RAdd` controls the Add button; `REdit` controls the last (Edit) grid column. Role id 2 bypasses (likely Admin — INFERRED).
- Defects: `custNameTextBox_TextChanged` tests `Contains(':')` but splits on `'|'` (IndexOutOfRange when no `|`); filters are SQL-concatenated; dates are compared as text.

## B. Prescription list — `DoctorModule_UI/PrescriptionView.aspx` (PAGE-657, Tier B; read for this document)
- Data: `SetupDAL.Get_PrescriptionList(param)` → `sp_Get_PrescriptionList(@param)` (body absent). Default `and PM.PrescriptionId IS NOT NULL` + date range on `PM.PrescriptionDate` (default today..today) + approval status, `us.UserRoleID`, `em.EmpInfoId`, `PM.SmcTypeId_RX`, `PM.RegionId/AreaId/TerritoryId` (hierarchy control `IVMasterStructureForDCR.ascx`, visible) + role scoping MIO/AM/DZSM/NSM via `View_Webapi_EmployeeFieldForceInfo` (same pattern as DCRList; `dtMarket.Rows[0]` unguarded).
- Also exposes `[WebMethod] Get_PrescriptionList(param)` (client-built SQL) — no auth check.
- Edit → `Prescription.aspx?id=`; Add → `Prescription.aspx`. No export in code-behind.

## C. Doctor Visit Monitoring dashboard — `Dashboard_UI/DoctorVisitMonitoring.aspx` (PAGE-636, Tier B; read for this document)
- Data: `DoctorMonitoringDAL.GetDoctorVisitMonitoringApprovalList(param, FromDate, ToDate, GroupId, ZonId, Area, Terr)` → `sp_Get_AllDoctorVisitMonitoringApprovalList(@param,@frmDate,@toDate,@GroupId,@ZonId,@AreaId,@TerrId)` (body absent). `param` is always `""`. Default period = current month (first..last day). Status dropdown is loaded but **not used**.
- Grid columns: SL, ID, Territory, Name Of the Employee, User Role, Base HQ, then link buttons **DCP, DCR, RX, Customer Coverage, Customer Visit Plan (CVP), Customer Visit Report (CVR), GP Sales (Net TP), Total Sales (Net TP)** (command argument `<ID>|<type>`).
- Drill-down `ShowModal` → `GetDataByType(id,type)` handles `DCP, DCR, RX, CustomerCoverage, GPSales, TSalesNetTP` but each returns only a placeholder string (`"DCP data for ID: <id>"` …); types `CustomerCoverageNew` and `CVRCount` raised by the grid are not handled (`"Invalid type."`) — the modal is a stub. `GetKPIByEmpId` returns hard-coded sample KPI rows (unused).
- Role behaviour: for `DZSM` the Zone dropdown is disabled; for `AM` Zone, Area and Group are disabled (relying on the hierarchy control prefill); no other checks.

## Business rules / gaps
BR-M16-017..020 (doctor list), BR-M16-035 (monitoring), BR-M16-044.. (prescription list scoping shares BR-M16-027 pattern); GAP-M16-052..054, GAP-M16-058.

## Tests
Doctor list: default filter shows doctors created in last 30 days; filter by sales person tagged as second tag; export CSV column set equals visible columns minus Edit; non-role-2 user without permission row is redirected. Prescription list: MIO sees only own prescriptions. Dashboard: click CVR link ⇒ "Invalid type." (stub).
