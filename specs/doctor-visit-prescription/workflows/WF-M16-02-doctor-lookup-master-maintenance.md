# WF-M16-02 — Doctor lookup master maintenance (chamber type, category, degree, designation, speciality, patient type, prescription type, special day)

## Trigger & actors
Back-office user opens a `*View.aspx` list page in `Solution.Web/DoctorMaster_UI/` (or `DoctorModule_UI/PrescriptionTypeView.aspx`), adds/edits via the matching entry page (`?id=<Id>`), or deletes a row. No role check in code (menu-only).

## Preconditions
None. Active rows feed the dropdowns of DoctorEntry (WF-M16-01), Prescription (WF-M16-04) and report pages.

## Pages / DAL / SP map (all SP bodies absent from the repo)
| Master | Entry page | List page | DAL (`Library.DAL/DoctorMaster_DAL`) | Save/Update/Check/Delete/List/Get SPs |
|---|---|---|---|---|
| Chamber type | `ChamberType.aspx` (PAGE-637) | `ChamberTypeView.aspx` (638) | `DoctorChamberDal` | `sp_Save_DoctorChamber`, `sp_Update_DoctorChamber`, `sp_check_DoctorChamber`, `sp_Delete_DoctorChamber`, `sp_Get_DoctorChamberList`, `sp_Get_DoctorChamber_ById` |
| Doctor category | `DoctorCategory.aspx` (639) | `DoctorCategoryView.aspx` (640) | `DoctorCategoryDal` | `sp_Save_DoctorCategory`, `sp_Update_DoctorCategory`, `sp_check_DoctorCategory`, `sp_Delete_DoctorCategory`, `sp_Get_DoctorCategoryList`, `sp_Get_Doctorcategory_ById` |
| Degree | `DoctorDegreeEntry.aspx` (641) | `DoctorDegreeView.aspx` (642) | `DoctorDegreeDal` | `sp_Save_DoctorDegree`, `sp_Update_DoctorDegree`, `sp_check_DoctorDegree` (name + DoctorTypeId), `sp_Get_DoctorDegreeList`, `sp_Get_DoctorDegree_ById` (no delete page method) |
| Designation | `DoctorDesignation.aspx` (643) | `DoctorDesignationView.aspx` (644) | `DoctorDesignationDal` | `sp_Save/Update/check_DoctorDesignation`, `sp_Delete_DoctorDesignation`, `sp_Get_DoctorDesignationList`, `sp_Get_DoctorDesignation_ById` |
| Speciality | `DoctorSpeciality.aspx` (645) | `DoctorSpecialityView.aspx` (646) | `DoctorSpecialityDal` | `sp_Save/Update/check_DoctorSpeciality`, `sp_Delete_DoctorSpeciality`, `sp_Get_DoctorSpecialityList`, `sp_Get_DoctorSpeciality_ById` |
| Patient type | `PatientType.aspx` (647) | `PatientTypeView.aspx` (648) | `PatientTypeDal` | `sp_Save/Update_DoctorPatientType`, `sp_check_DoctorPatientType`, `sp_Delete_DoctorpatientType`, `sp_Get_DoctorPatientList`, `sp_Get_DoctorPatientType_ById` |
| Prescription type | `DoctorMaster_UI/PrescriptionType.aspx` (649) and **duplicate** `DoctorModule_UI/PrescriptionType.aspx` (655) | `PrescriptionTypeView.aspx` (650, 656) | `PrescriptionTypeDal` (649/650); `SetupDAL.SavePrescription/GetPrescriptionForEdit/GetPrescriptionTypeList` (655/656) | `sp_Save/Update_PrescriptionType`, `sp_check_PrescriptionType`, `sp_Delete_PrescriptionType`, `sp_Get_PrescriptionTypeList`, `sp_Get_PrescriptionType_ById` |
| Special day | `SpecialDaySetup.aspx` (651) | `SpecialDayView.aspx` (652) | `DoctorSpecailDayDal` | `sp_Save/Update_DoctorSpecialDay`, `sp_check_DoctorSpecialDay`, `sp_Delete_DoctorSpecailDay`, `sp_Get_DoctorSpecailDayList`, `sp_Get_DoctorSpeacialDay_ById` |
Dropdown feeds (Active only, `CommonDataLoad`/`DoctorInformationDal`): `sp_Get_ChamberType_Active`, `sp_Get_doctorCateList`, `sp_Get_Degree_All_Active`, `sp_Get_Designation_All_Active`, `sp_Get_DoctorSpeciality_Active`, `sp_Get_DoctorSpecialDay_All_Active`, `sp_Get_DoctorType_Active` (+ `sp_Get_DoctorProgramType_Active`).

## Steps (uniform pattern, verified in `DoctorCategoryDal.cs:20-80`, `PrescriptionTypeDal.cs:66-130`)
1. Entry page `Page_Load` sets hidden `masterId` = `?id` or `0` (`ChamberType.aspx.cs:12-24` pattern). Edit loads via WebMethod `Get…ForEdit(id)` → `sp_Get_*_ById`.
2. Save WebMethod (e.g. `Save_DoctorChamber`, `Save_PatientTypeDay`, `Save_PrescriptionType`) passes `Session["UserId"]` to the DAL `Save…`.
3. DAL: if Id>0 → call `sp_check_*` (with the same Id and new name); **if it returns 0 rows → `sp_Update_*` (@UpdateBy) else `isSuccess=false`** (duplicate name). If Id==0 → `SaveDataReturnPrimaryKey(sp_Save_*)` (@EntryBy); `pk>0 ⇒ success`; **no `sp_check_*` call on insert** (duplicates on insert depend on the Save SP, UNKNOWN).
4. Delete WebMethod (`Delete_Doctorchamber`, `DeleteDoctorcategory`, …) → `DeleteData(sp_Delete_*, @<Id>, @DeleteBy=session user)`; soft vs hard delete and referential checks UNKNOWN.
5. List page loads JSON via `Get_*` WebMethod.

## Data created / changed
Lookup tables (names UNKNOWN from repo; e.g. `tblDoctorChamber`, `tblDoctorDegree`, `tblDoctorSpeciality`, `tblDoctorDesignation`, `tbl_PrescriptionType`, `tblDoctorType`, `tblDoctorProgramType` appear inside `sp_Get_DoctorList` and legacy inline SQL) with `IsActive`, `Activedate`, `EntryBy`/`UpdateBy`. Status: `IsActive` flag only.

## Branches / exceptions
- Duplicate on update → `isSuccess=false` (message text per page JS, UNKNOWN).
- DAL exceptions rethrown → HTTP 500 to the AJAX call.
- `DoctorModule_UI/PrescriptionType.aspx.cs` calls `SetupDAL.SavePrescription(prescription, 1.ToString())` — **user id hard-coded "1"**, so audit shows user 1 for every save (BR-M16-025).
- `SqlParameter` names with trailing spaces (`"@CategoryId "`, `"@IsActive "`) in several DALs (ADO.NET normally tolerates? UNKNOWN, works in production presumably).

## Postconditions
Active masters appear in dropdowns; inactive ones disappear from dropdowns but remain on historic records.

## Related BR ids
BR-M16-022, 023, 024, 025.

## Related pages
PAGE-637..652 (this folder; Tier B), PAGE-655/656.

## Tests
Create category "X" → appears in DoctorEntry Doctor Category; rename "X" to an existing name → rejected; create duplicate name (insert) → verify SP behaviour; delete a category used by a doctor → verify behaviour (referential rule UNKNOWN); save PrescriptionType from the DoctorModule page → audit user = 1.
