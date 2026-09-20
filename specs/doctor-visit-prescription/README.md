# M16 — Doctor / Provider Master, Visits & Prescriptions

Reviewed 2026-09-19. Documentation only; evidence = source paths cited in page specs. No prior specification existed. SP bodies exist in the repo only for `sp_Save_DoctorMaster`, `sp_Update_DoctorMaster`, `sp_Get_DoctorList` (`LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:666-930`).

## Purpose & business overview
Pharma field-force module: maintain the doctor ("provider") master and its lookups (chamber type, category, degree, designation, speciality, patient type, prescription type, special day); review monthly call plans (DCP), doctor call reports (DCR = visits) and prescriptions (RX); approve/reject DCRs; report coverage/prescription performance by doctor, product, brand, MIO, area and zone. DCP/DCR creation is **not** done by any web page (INFERRED mobile app; GAP-M16-062).

## Page catalogue (40 pages)
| Page ID | Page | Kind | Tier | Spec |
|---|---|---|---|---|
| PAGE-636 | DoctorVisitMonitoring | Dashboard | B | [PAGE-636-doctorvisitmonitoring.md](pages/PAGE-636-doctorvisitmonitoring.md) |
| PAGE-637 | ChamberType | Lookup entry | B | [PAGE-637-chambertype.md](pages/PAGE-637-chambertype.md) |
| PAGE-638 | ChamberTypeView | Lookup list | B | [PAGE-638-chambertypeview.md](pages/PAGE-638-chambertypeview.md) |
| PAGE-639 | DoctorCategory | Lookup entry | B | [PAGE-639-doctorcategory.md](pages/PAGE-639-doctorcategory.md) |
| PAGE-640 | DoctorCategoryView | Lookup list | B | [PAGE-640-doctorcategoryview.md](pages/PAGE-640-doctorcategoryview.md) |
| PAGE-641 | DoctorDegreeEntry | Lookup entry | B | [PAGE-641-doctordegreeentry.md](pages/PAGE-641-doctordegreeentry.md) |
| PAGE-642 | DoctorDegreeView | Lookup list | B | [PAGE-642-doctordegreeview.md](pages/PAGE-642-doctordegreeview.md) |
| PAGE-643 | DoctorDesignation | Lookup entry | B | [PAGE-643-doctordesignation.md](pages/PAGE-643-doctordesignation.md) |
| PAGE-644 | DoctorDesignationView | Lookup list | B | [PAGE-644-doctordesignationview.md](pages/PAGE-644-doctordesignationview.md) |
| PAGE-645 | DoctorSpeciality | Lookup entry | B | [PAGE-645-doctorspeciality.md](pages/PAGE-645-doctorspeciality.md) |
| PAGE-646 | DoctorSpecialityView | Lookup list | B | [PAGE-646-doctorspecialityview.md](pages/PAGE-646-doctorspecialityview.md) |
| PAGE-647 | PatientType | Lookup entry | B | [PAGE-647-patienttype.md](pages/PAGE-647-patienttype.md) |
| PAGE-648 | PatientTypeView | Lookup list | B | [PAGE-648-patienttypeview.md](pages/PAGE-648-patienttypeview.md) |
| PAGE-649 | PrescriptionType | Lookup entry | B | [PAGE-649-prescriptiontype.md](pages/PAGE-649-prescriptiontype.md) |
| PAGE-650 | PrescriptionTypeView | Lookup list | B | [PAGE-650-prescriptiontypeview.md](pages/PAGE-650-prescriptiontypeview.md) |
| PAGE-651 | SpecialDaySetup | Lookup entry | B | [PAGE-651-specialdaysetup.md](pages/PAGE-651-specialdaysetup.md) |
| PAGE-652 | SpecialDayView | Lookup list | B | [PAGE-652-specialdayview.md](pages/PAGE-652-specialdayview.md) |
| PAGE-653 | Prescription | Transaction entry (RX) | A | [PAGE-653-prescription.md](pages/PAGE-653-prescription.md) |
| PAGE-654 | PrescriptionApprovalList | Approval (RX, broken) | A | [PAGE-654-prescriptionapprovallist.md](pages/PAGE-654-prescriptionapprovallist.md) |
| PAGE-655 | PrescriptionType | Lookup entry (duplicate) | B | [PAGE-655-prescriptiontype.md](pages/PAGE-655-prescriptiontype.md) |
| PAGE-656 | PrescriptionTypeView | Lookup list (duplicate) | B | [PAGE-656-prescriptiontypeview.md](pages/PAGE-656-prescriptiontypeview.md) |
| PAGE-657 | PrescriptionView | List (RX) | B | [PAGE-657-prescriptionview.md](pages/PAGE-657-prescriptionview.md) |
| PAGE-658 | DCRList | List + approval (DCR) | A | [PAGE-658-dcrlist.md](pages/PAGE-658-dcrlist.md) |
| PAGE-659 | DCRReport | Detail report (DCR) | B | [PAGE-659-dcrreport.md](pages/PAGE-659-dcrreport.md) |
| PAGE-660 | DoctorDailyVisit | List (daily visits) | B | [PAGE-660-doctordailyvisit.md](pages/PAGE-660-doctordailyvisit.md) |
| PAGE-661 | DoctorPlanDetailsView | Detail (DCP) | B | [PAGE-661-doctorplandetailsview.md](pages/PAGE-661-doctorplandetailsview.md) |
| PAGE-662 | DoctorVisit | List (DCP) | A | [PAGE-662-doctorvisit.md](pages/PAGE-662-doctorvisit.md) |
| PAGE-663 | DoctorVisitApprovalList | Approval (DCP, non-functional) | A | [PAGE-663-doctorvisitapprovallist.md](pages/PAGE-663-doctorvisitapprovallist.md) |
| PAGE-664 | DoctorVisitReport | Report (DCP pivot) | B | [PAGE-664-doctorvisitreport.md](pages/PAGE-664-doctorvisitreport.md) |
| PAGE-665 | MISAttendanceAndDCRReport | Report (MIS) | B | [PAGE-665-misattendanceanddcrreport.md](pages/PAGE-665-misattendanceanddcrreport.md) |
| PAGE-666 | SalesmanCustomerVisitReport | Report | B | [PAGE-666-salesmancustomervisitreport.md](pages/PAGE-666-salesmancustomervisitreport.md) |
| PAGE-667 | SalesmanCustomerVisitSummeryReport | Report | B | [PAGE-667-salesmancustomervisitsummeryreport.md](pages/PAGE-667-salesmancustomervisitsummeryreport.md) |
| PAGE-668 | Customer_Doctor_Transfer | Transaction (transfer) | B | [PAGE-668-customer-doctor-transfer.md](pages/PAGE-668-customer-doctor-transfer.md) |
| PAGE-669 | DoctorEntry | Master entry | A | [PAGE-669-doctorentry.md](pages/PAGE-669-doctorentry.md) |
| PAGE-670 | DoctorView | Master list/export | B | [PAGE-670-doctorview.md](pages/PAGE-670-doctorview.md) |
| PAGE-671 | CVRDoctoriseMonthlypt | Report (CVR pivot) | B | [PAGE-671-cvrdoctorisemonthlypt.md](pages/PAGE-671-cvrdoctorisemonthlypt.md) |
| PAGE-672 | DcpDoctoriseMonthlypt | Report (DCP pivot) | B | [PAGE-672-dcpdoctorisemonthlypt.md](pages/PAGE-672-dcpdoctorisemonthlypt.md) |
| PAGE-673 | DcrDoctoriseMonthlypt | Report (DCR pivot) | A | [PAGE-673-dcrdoctorisemonthlypt.md](pages/PAGE-673-dcrdoctorisemonthlypt.md) |
| PAGE-674 | DoctorInfoReport | Report (SMC family) | A | [PAGE-674-doctorinforeport.md](pages/PAGE-674-doctorinforeport.md) |
| PAGE-675 | RXDoctoriseMonthlypt | Report (RX pivot) | A | [PAGE-675-rxdoctorisemonthlypt.md](pages/PAGE-675-rxdoctorisemonthlypt.md) |

Tier A = fully reviewed (9). Tier B = generated spec; pages 636, 657, 660, 661, 664-668, 670-672 were also read for the workflow/report docs and BR index below, but their page files remain generated.

## Business rules index
Full text (source, implementation, confidence) is in the Tier-A page specs: PAGE-669 (BR-001..016), PAGE-658 (026..031), PAGE-662 (033), PAGE-663 (034), PAGE-653 (037..043), PAGE-654 (044..048), PAGE-674 (049..054), PAGE-675 (055..061), PAGE-673 (062..064). Rules 017-025, 032, 035, 036 are defined here (Tier-B pages read).

| BR | One-liner |
|---|---|
| BR-M16-001 | Doctor name mandatory (only server check) |
| BR-M16-002 | No other doctor field validated server-side |
| BR-M16-003 | Chamber needs name+type; phone 11 chars if given |
| BR-M16-004 | DoctorId>0 -> update SP, else save SP |
| BR-M16-005 | DoctorCode = 'D' + max numeric suffix (approved doctors)+1 |
| BR-M16-006 | Web-created doctors saved Approved ('2'), IsFromApp=0 |
| BR-M16-007 | Sales-person tagging CSV ids/codes; SalesPersonId = first |
| BR-M16-008 | Active/Inactive dates = page-load date; update overwrites both |
| BR-M16-009 | Degree list depends on doctor type |
| BR-M16-010 | Update deletes and re-inserts child rows |
| BR-M16-011 | Chambers used by Rx/DCR survive update purge |
| BR-M16-012 | Non-admin field locks (largely ineffective) |
| BR-M16-013 | Market/hierarchy inputs hidden; not editable |
| BR-M16-014 | Any failure reported as "Already Exist!" |
| BR-M16-015 | Bad special-day date stored NULL |
| BR-M16-016 | Program type / SMC type lists filtered by group flags |
| BR-M16-017 | sp_Get_DoctorList: IsDoctor=1 only, order by code desc, status text 0/1/2/3, tagged sales persons resolved (`sql:884-930`) |
| BR-M16-018 | DoctorView filters: default last 30 days by EntryDate; sales person matches SalesPersonId or tagged CSV; hierarchy filters inert (`DoctorView.aspx.cs:417-504`) |
| BR-M16-019 | DoctorView permission: UserRoleID != "2" needs page-permission row; RAdd/REdit control buttons (`DoctorView.aspx.cs:166-198`) |
| BR-M16-020 | DoctorView export: CSV, all pages, no Edit column (`DoctorView.aspx.cs:232-287`) |
| BR-M16-021 | Transfer needs selection + target market; customers blocked if order pending; runs as ApprovedBy=session user (`Customer_Doctor_Transfer.aspx.cs`) |
| BR-M16-022 | Lookup save: update checks duplicate via sp_check_*; insert has no C# check (`DoctorCategoryDal.cs:27-52`) |
| BR-M16-023 | Lookup delete via sp_Delete_* with @DeleteBy (`DoctorCategoryDal.cs:141-143`) |
| BR-M16-024 | Only Active lookup rows feed dropdowns, sp_Get_*_Active (`CommonDataLoad.cs:732-1117`) |
| BR-M16-025 | DoctorModule PrescriptionType save hard-codes user "1" (`DoctorModule_UI/PrescriptionType.aspx.cs:44-50`) |
| BR-M16-026 | DCRList default today; filters date/status/role/employee/platform |
| BR-M16-027 | DCRList and PrescriptionView scoped by RoleTypeName MIO/AM/DZSM/NSM |
| BR-M16-028 | Approve button only on Rejected rows, Reject only on Approved |
| BR-M16-029 | Approve/Reject appends tblDCRApprovalLog via sp_webapi_SaveDCRAppLog (Step+1, MenuId 382) |
| BR-M16-030 | No log row -> silent no-op |
| BR-M16-031 | DCRList hierarchy filters inert (hidden control) |
| BR-M16-032 | DoctorDailyVisit approval filter uses DCR.IsApproved 0/1 (`DoctorDailyVisit.aspx:191-203,491-493`) |
| BR-M16-033 | DCP list defaults to current month/year; raw SQL filter from browser |
| BR-M16-034 | DoctorVisitApprovalList non-functional (Razor URLs in WebForms) |
| BR-M16-035 | Monitoring dashboard drill-down is a stub; two link types unhandled (`DoctorVisitMonitoring.aspx.cs:120-235`) |
| BR-M16-036 | MIS/Salesman reports: robust date parse, EmpInfoId->EmpMasterCode lookup, "[]" on error (`DoctorVisitDAL.cs:154-275`) |
| BR-M16-037..043 | Prescription capture: image mandatory; Validation() dead; chambers per doctor; save flow + image file; approved lock UI-only; no product dedupe; EntryBy = Captured By |
| BR-M16-044..048 | Rx approval: list <> 'Approved'; >=1 row; bulk SP call; undefined `rbValue` defect; status vocabulary mismatch |
| BR-M16-049..054 | DoctorInfoReport: type required; view required (silent); default month; last process date; footer sums; export rules |
| BR-M16-055..061 | Monthly pivots: date-range pivot; 'Select' default; AM area / DZSM zone required; territory bug; text-vs-id; inert DCR/RX radio; unencoded HTML |
| BR-M16-062..064 | DCR page: only 3 report types reachable, hidden filters; hidden columns removed; dead DAL method calling RX SP |

## Status lifecycles
1. **Doctor master** (`tblDoctorMaster.ApprovalStatus`): 0 Pending, 1 Verified, 2 Approved, 3 Rejected (decode in `sp_Get_DoctorList`). Web save inserts '2'. Verify/reject by `Approval_UI/DoctorApprovalList` (other module). Separate `IsActive` flag with Activedate/InactiveDate.
2. **Call plan (DCP) -> DCR -> approval**: monthly plan headers (per employee, `ApprovalStatus`) created outside web; DCP approval page non-functional; DCR (`tbl_DCRInfo`) created by app; web list shows status text; Approved <-> Rejected toggle writes `tblDCRApprovalLog` (Type 'DCR', Status Accepted/Rejected, Step+1). Second flag `IsApproved` (0/1) in Daily Visit list. Reports filter by approval status. See WF-M16-03.
3. **Prescription (RX)**: capture (Prescription.aspx) -> ApprovalStatus (domain UNKNOWN; '2' = approved/locked in UI) -> approval page cannot submit (JS defect) -> RX reports. See WF-M16-04.

## Roles / permissions observed
Session keys used: UserId, EmpInfoId, RoleTypeName (MIO, AM, DZSM, NSM, Admin), RoleTypeId, UserRoleID. Enforced in code: DoctorView page permission (UserRoleID != "2" needs permission row); role-scoped row visibility in DCRList and PrescriptionView; AM must pick Area / DZSM Zone in monthly pivots; hierarchy-control prefill by role. All other pages and WebMethods: menu-only gating, no checks.

## Data model (known)
`tblDoctorMaster` (DoctorId, DoctorCode, DoctorName, SecondaryCode, UPCode, DesignationId, Gender, DoctorTypeId, DoctorCategoryId, ProgramTypeId, SMCTypeId, StationTypeId, MarketId, DivisionId/DistrictId/ThanaId, UnionName, StreetAddress, Reamrks, IsActive, Activedate, InactiveDate, IsFromApp, IsDoctor, ApprovalStatus, SalesPersonId, TaggedSalesPersonIds, TaggedSalesPersonCodes, EntryBy/Date, UpdateBy/Date); children `tblDoctorChemberDetail`, `tblDoctorDegreeDetail`, `tblDoctorSpecialityDetail`, `tblDoctorBrandDetail`, `tblDoctorContactDetail`, `tblDoctorSpecialDayDetail`; lookups `tblDoctorDegree/Speciality/Designation/Chamber/Type/ProgramType`, `tblSMCType`; `tbl_PrescriptionMaster` (+ product detail, names UNKNOWN), `tbl_PrescriptionType`; `tbl_DCRInfo`, `tblDCRApprovalLog`; DCP master/detail (names UNKNOWN); view `View_Webapi_EmployeeFieldForceInfo`. Key SPs: page specs §16 and RPT-M16-01 (17 pivot/report SPs).

## Integrations
Mobile app / webapi SPs (`sp_webapi_*`, `sp_Webapi_Get_ImagePath`), prescription images on server file system, Google Maps link (DCRList), CDN scripts (xlsx). No SAP.

## Reports
No Crystal Reports in M16. See `reports/`: RPT-M16-01 monthly pivots, -02 DoctorInfoReport, -03 visit/DCR/MIS, -04 doctor list, prescription list, monitoring.

## Workflows
`workflows/`: WF-M16-01 doctor master, -02 lookup masters, -03 DCP->DCR->approval, -04 prescription->approval, -05 doctor/customer transfer.

## Known gaps summary
`GAPS-M16.md` (66 items). Top: Rx approval page cannot submit (GAP-018); DCP approval page dead (031); prescription validation dead (011); SQL fragments built in browser (020); `IsDoctor` not set by save SP (002); code-generation race (001); static DAL instances and swallowed exceptions (006, 009).

## Still unverified
All SP bodies except three; DCP/DCR creation channel and initial statuses; approval value domains; pivot/report metric definitions; child-detail SP behaviour (duplicates on update); delete semantics of lookups; table names for DCP and prescription detail; menu placement.
