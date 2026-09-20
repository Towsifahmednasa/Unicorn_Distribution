# WF-M16-01 — Doctor master create / update

## Trigger & actors
Back-office user (any authenticated user with the menu entry; no code-enforced role) opens `MasterSetup_UI/DoctorView.aspx`, presses Add (new) or Edit (existing) → `MasterSetup_UI/DoctorEntry.aspx[?mid=<DoctorId>]`.

## Preconditions
- Lookup masters populated (WF-M16-02): designation, category, doctor type (+degrees), speciality, chamber type, contact type, special day, program type, SMC type ("Pharma Platform"), brands, sales persons.
- For edit: existing `tblDoctorMaster.DoctorId`.

## Steps
1. `DoctorView.aspx.cs:301-333` → opens `DoctorEntry.aspx` (Add: redirect; Edit: `window.open('DoctorEntry.aspx?mid=' + id)`). (PAGE-670 → PAGE-669)
2. `DoctorEntry.Page_Load` → `LoadInitialInfo()` (dropdowns via `CommonDataLoad`/`SeedDataDAL` → `sp_Get_*_Active`); edit: `GetOneRecord` → `DoctorDAL.GetDoctorSetupById` → `sp_GET_DoctorMaster_ById`, `GetContactById`, `GetSpecialDayById`.
3. User edits form; chambers/contacts/special days are collected in in-page grids (`AddChamber`, `AddContact`, `AddSpecialDay`; chamber validation BR-M16-003).
4. Submit → `btnSave_Click` → `Validation()` (name only, BR-M16-001) → DAO build (tags BR-M16-007, dates BR-M16-008).
5. `DoctorDAL.SaveInfo` (`DoctorDAL.cs:114`):
   - create: `sp_Save_DoctorMaster` (`LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:672`) → generates `DoctorCode`, inserts row with `ApprovalStatus='2'`, `IsFromApp=0`, returns `SCOPE_IDENTITY()`.
   - update: `sp_Update_DoctorMaster` (`:735`) → updates master; deletes brand/degree/speciality/special-day/contact rows and all chambers not used by prescriptions/DCRs.
   - then inserts chambers (`sp_Save_BDoctorChemberDetail`), degrees (`sp_Save_DoctorDegreeDetail`), specialities (`sp_Save_DoctorSpecialityDetail`), brands (`sp_Save_DoctorBrandDetail`), contacts (`sp_Save_DoctorContactDetail_New`), special days (`sp_Save_DoctorSpecialDayDetails`).
   - `DataAccessManager` opens an implicit transaction on the first write; committed by `SqlConnectionClose()`; rollback only via the catch path.
6. Page shows `successalert(..., 'DoctorView.aspx')` or `faildalert('Already Exist!')` (BR-M16-014).

## Data created / changed
| Table | Create | Update |
|---|---|---|
| `tblDoctorMaster` | insert 1 row (`EntryBy`, `EntryDate`, `ApprovalStatus='2'`, `IsFromApp=0`, tags) | update all listed columns + `UpdateBy/UpdateDate` |
| `tblDoctorChemberDetail` | insert per grid row | delete except chambers referenced by `tbl_PrescriptionMaster`/`tbl_DCRInfo`, then insert per grid row |
| `tblDoctorDegreeDetail`, `tblDoctorSpecialityDetail`, `tblDoctorBrandDetail`, `tblDoctorContactDetail`, `tblDoctorSpecialDayDetail` | insert | delete all + insert |
Status transitions: none on update; a created doctor is immediately `Approved` (`'2'`). Doctor Verified/Rejected transitions are done by `Approval_UI/DoctorApprovalList` (another page; `SaveDoctor_ApplogDAL` → `sp_webapi_SaveDoctorAppLog`).

## Branches / exceptions
- Empty name → no save (BR-M16-001).
- DAL exception → rollback + rethrow → unhandled error page (page has no try/catch around `SaveInfo`).
- `isSuccess==false` (only when `pk<=0`) → "Already Exist!" (misleading).
- Chamber referenced by DCR/Rx and also present in grid → potential duplicate chamber row after update (depends on `sp_Save_BDoctorChemberDetail`, UNKNOWN).
- Concurrent creates → possible duplicate `DoctorCode` (BR-M16-005).

## Postconditions
Doctor exists with master + child rows; visible in `sp_Get_DoctorList` only if `IsDoctor=1` (the Save SP does not set it — GAP-M16-002).

## Related BR ids
BR-M16-001..016, BR-M16-017, BR-M16-018, BR-M16-019, BR-M16-020.

## Related pages
PAGE-669 DoctorEntry, PAGE-670 DoctorView, lookup pages PAGE-637..652 (WF-M16-02), PAGE-668 (WF-M16-05).

## Tests
See PAGE-669 §27 (T1-T12); plus: create doctor then open DoctorView with default filter (last 30 days by `EntryDate`) — row must be present; edit → change tags A,B → B only, verify `SalesPersonId`/`TaggedSalesPersonIds`; edit inactive doctor and verify dates (BR-M16-008).
