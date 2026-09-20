---
page_id: PAGE-669
module: M16
page: DoctorEntry
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-669 — DoctorEntry

> **Evidence level:** Tier A — `.aspx`, `.aspx.cs`, `IVMarketStructure.ascx.cs`, `DoctorDAL.cs`, `DoctorMasterDAO`, `DataAccessManager.cs` and the repo SP bodies `sp_Save_DoctorMaster`, `sp_Update_DoctorMaster`, `sp_Get_DoctorList` (`LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:666-930`) were read in full. Child-detail SPs and `sp_GET_DoctorMaster_ById` bodies are NOT in the repository (`UNKNOWN`). Reviewed 2026-09-19.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-669 |
| Module | M16 — Doctor / Provider Master, Visits & Prescriptions |
| Page name | DoctorEntry (title "Doctor Information") |
| Kind | Master-data entry / edit (create + update in one page) |
| Web folder | MasterSetup_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | MasterSetup_UI_DoctorEntry (`Solution.Web/MasterSetup_UI/DoctorEntry.aspx.cs`, 1230 lines) |
| User controls | `IVMarketStructure.ascx` (forced `Visible=false`, see BR-M16-013) |
| Legacy/duplicate | No duplicate page found; `SetupDAL.Get_DoctorList` legacy inline SQL exists but is unrelated to this page |

## 2. Purpose
Create a new doctor (prescriber / "provider") record, or edit an existing one (`?MID=<DoctorId>`), including designation, category, doctor type, degrees, specialities, promoted brands, chambers (name/type/address/phone), contact numbers, special days (birthday etc.), program type, "Pharma Platform" (SMC type), tagged sales person(s), active flag. **CONFIRMED** from labels/handlers (`DoctorEntry.aspx.cs:651-877`).

## 3. Business Objective
Maintain the doctor master used by the field force for doctor call plans (DCP), doctor call reports (DCR), prescription (RX) capture and the doctor reports of this module. Tagging one or more sales persons to a doctor decides which MIO/MIA "owns" the doctor (INFERRED from `TaggedSalesPersonIds` usage in `sp_Get_DoctorList` and `DoctorView` filter).

## 4. Navigation
Menu placement is DB-driven (`sp_GET_MainMenuRole2` → `NewMasterPage.master.cs`); the menu row is `UNKNOWN`. Inbound: `MasterSetup_UI/DoctorView.aspx` (Add button → `DoctorEntry.aspx`; Edit row → `window.open('DoctorEntry.aspx?mid=<id>')`, `DoctorView.aspx.cs:301-316,330-333`). After a successful save the page redirects (via `successalert(...,'DoctorView.aspx')`) to `DoctorView.aspx` (`DoctorEntry.aspx.cs:866`). "Back to List" link → `DoctorView.aspx`.

## 5. Route
`~/MasterSetup_UI/DoctorEntry.aspx` and `~/MasterSetup_UI/DoctorEntry.aspx?MID=<DoctorId>` (query key read as `MID`, case-insensitive; `Request.QueryString["MID"]`, `DoctorEntry.aspx.cs:48`). No `MapPageRoute`.

## 6. Source Files
| Type | Path |
|---|---|
| View | `Solution.Web/MasterSetup_UI/DoctorEntry.aspx` |
| Code-behind | `Solution.Web/MasterSetup_UI/DoctorEntry.aspx.cs` |
| User control | `Solution.Web/MasterSetup_UI/IVMarketStructure.ascx(.cs)` |
| DAL | `Library.DAL/MasterSetup_DAL/DoctorDAL.cs` (`SaveInfo`, `GetDoctorSetupById`, `GetContactById`, `GetSpecialDayById`) |
| DAL (lookups) | `Library.DAL/DoctorModule_DAL/CommonDataLoad.cs`, `Library.DAL/DoctorModule_DAL/SeedDataDAL.cs` |
| DAO | `Library.DAO/MasterSetup_DAO/DoctorMasterDAO.cs`, `DoctorChemberDetailDAO`, `DoctorSpecialDayDAO`, `DoctorContactDetailDAO` |
| Data access core | `Library.DAL/DataManager/DataAccessManager.cs` (implicit transaction, see §12) |
| SP bodies in repo | `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql` lines 666-725 (`sp_Save_DoctorMaster`), 729-811 (`sp_Update_DoctorMaster`), 878-930 (`sp_Get_DoctorList`) |

## 7. UI Control Inventory
"Required" = server-side check that blocks save. Only **Doctor name** is checked (BR-M16-001/002); red `*` next to Designation/Gender/Doctor Type are visual only.

| Control ID | Label | Required | Default | Read-only / Visible conditions | Data source / notes |
|---|---|---|---|---|---|
| id_mastetID (HiddenField) | – | – | "" (new) / `MID` (edit) | – | Drives update vs insert (`DoctorEntry.aspx.cs:52,749`) |
| DoctorName (TextBox) | Doctor name | **Yes** (`Validation()` → `is-invalid` css + tooltip "please fill out this field", focus; no message box) | empty | – | `DoctorMaster.DoctorName` |
| docCat (DropDownList) | Doctor Category | No | "Please Select From List" | – | `CommonDataLoad.GetDocCat_Active` → `sp_Get_doctorCateList` (CategoryId/CategoryName) |
| SecondaryCode (TextBox) | Secondary Code | No | empty | **Hidden** (parent div `visible=false`, aspx:79); ReadOnly for non-Admin (code:243) | – |
| UPCode (TextBox) | UP Code | No | empty | **Hidden** (aspx:95) | – |
| designationSelect (DDL) | Designation | No | first item | – | `GetDesignation_Active` → `sp_Get_Designation_All_Active` (DesignationId/DesignationName) |
| GenderSelect (DDL) | Gender | No | "Select from list" (index 0 → NULL) | – | static: Male, Female, Other; saved as the item **text** (`GenderSelect.SelectedItem.Text`, code:759) |
| DoctorTypeSelect (DDL, AutoPostBack) | Doctor Type | No | first item | postback reloads degrees (BR-M16-009) | `GetDoctorType_Active` → `sp_Get_DoctorType_Active` |
| ddlSalesPerson (ListBox multi) | Sales Person | No | none | – | `GetDoctor_SalesPerson_Active` → `sp_GetDoctor_SalesPerson_Active` (EmpInfoId / EmpName "CODE : Name") |
| ddlProgramType (DDL) | Program Type / Provider | No | first item | disabled for non-Admin in edit (code:250, but control is not hidden) | `SeedDataDAL.GetProgramTypeListParam(" and GRP.IsDoctor=1")` → `sp_GET_ProgramTypeListParm` |
| ddlPharmaPlatform (DDL) | Pharma Platform | No | first item | disabled for non-Admin in edit | `GetSMCTypeListParam(" and GRP.forDotor=1")` → `sp_GET_SMCTypeListParm` |
| ddlStationType (DDL) | (Station Type) | No | first item | **Hidden** (aspx:265) | `SeedDataDAL.GetStationTypeList` |
| BrandSelect (ListBox multi) | Brand | No | none | – | `GetDoctorBrand_Active` → `sp_Get_DoctorBrand_Active` (ProductBrandId / ProductSQName) |
| degreeSelect (ListBox multi) | Degree | No | empty until Doctor Type chosen | – | `GetDegree_ActiveDoctorTypeId` → `sp_Get_Degree_All_ActiveByDoctorTypeId` |
| doctorSpecialitySelect (ListBox multi) | Speciality | No | none | – | `DoctorSpeciality_Active` → `sp_Get_DoctorSpeciality_Active` |
| txaStreetAddress (TextBox multi-line) | Street Address | No | empty | – | `DoctorMaster.StreetAddress` |
| IVMarketStructure (UC) | Market Structure | No | – | `Visible=false` in Page_Load (code:40) and `display:none` | Market/territory hierarchy is **not editable** (BR-M16-013) |
| ddlDivision / ddlDistrict / ddlThana | Division/District/Thana | No | first item | **Hidden** (aspx:373-402); cascade postbacks exist | `SeedDataDAL.GetDivisionList/GetDistrictList/GetThanaList` |
| txtUnion | Union Name | No | empty | **Hidden** (aspx:433) | – |
| ChamberTypeSelect (DDL) | Chamber type | yes for "Add chamber" | first item | – | `GetChamberType_Active` → `sp_Get_ChamberType_Active` (ChamberId/ChamberName) |
| txtChamberName | Chamber Name | yes for "Add chamber" | empty | – | – |
| txtChamberAddress | Chamber Address | No | empty | – | – |
| txtPhone | Chamber Phone | No; if entered must be exactly 11 chars | empty | `MaxLength=11`, `FilteredTextBoxExtender` digits only | msg "Mobile NO must be 11 digits!" via `showpop6` |
| gv_Chamber (GridView) | Chamber list | – | empty | remove via `deleteChamber` | in-memory list posted on Save (viewstate) |
| ddlContactType / txtContact / gv_Contact | Contact list | No validation | – | – | `GetContactType` → `sp_Get_ContactType_Active` |
| ddlSpecialDay / txtSpecialDate / gv_Special | Special days | No validation | – | datepicker text | `GetSpecialDay_Active` → `sp_Get_DoctorSpecialDay_All_Active` |
| txtActiveDate (TextBox) | (Active/Inactive date) | – | `DateTime.Now` "dd-MMMM-yyyy" on first load | `Visible=false` (holds value in viewstate) | See BR-M16-008 |
| txtRemarks | Remarks | No | empty | **Hidden** (aspx:796); ReadOnly non-Admin | – |
| chkIsActive (HTML checkbox runat=server) | Active | – | checked | – | `IsActive` |
| btnSave / btnUpdate (LinkButton) | Submit / Update | – | exactly one visible: `btnSave` if no `MID`, `btnUpdate` if `MID` (code:50,57) | both use `btnSave_Click`; client `sweetAlertConfirm_Submit/Update` (NewMasterPage.master:441,486) | – |
| Reset (LinkButton, no handler) | Reset | – | – | – | Has no OnClick; effect UNKNOWN (page does nothing server-side) |

## 8. Field Mapping
Path: control → `DoctorMasterDAO` → `DoctorDAL.SaveInfo` → SP param → `tblDoctorMaster` column. Column names are confirmed for create/update from the repo SP bodies.

| UI control | DAO property | SP param (both `sp_Save_DoctorMaster` / `sp_Update_DoctorMaster`) | Table.column (CONFIRMED) |
|---|---|---|---|
| id_mastetID | `DoctorId` (0 if empty) | `@DoctorId` (unused by Save; WHERE key in Update) | `tblDoctorMaster.DoctorId` |
| DoctorName | `DoctorName` | `@DoctorName` | `DoctorName` |
| (auto) | – | – | `DoctorCode` = `'D'+(MAX numeric suffix of DoctorCode among ApprovalStatus='2')+1` (Save only) |
| SecondaryCode | `SecondaryCode` | `@SecondaryCode` | `SecondaryCode` |
| UPCode | `UPCode` | `@UPCode` | `UPCode` |
| designationSelect | `DesignationId` | `@DesignationId` | `DesignationId` |
| GenderSelect | `Gender` (text) | `@Gender` | `Gender` |
| DoctorTypeSelect | `DoctorTypeId` | `@DoctorTypeId` | `DoctorTypeId` |
| ddlSalesPerson (first selected) | `SalesPersonId` | `@SalesPersonId` | `SalesPersonId` |
| ddlSalesPerson (all selected, CSV EmpInfoId) | `TaggedSalesPersonIds` | `@TaggedSalesPersonIds nvarchar(500)` | `TaggedSalesPersonIds` (column added by same script, lines 14-17, `NVARCHAR(MAX)`) |
| ddlSalesPerson (CSV of text before ':' = EmpMasterCode) | `TaggedSalesPersonCodes` | `@TaggedSalesPersonCodes nvarchar(500)` | `TaggedSalesPersonCodes` |
| txtRemarks | `Reamrks` (sic) | `@Reamrks` | `Reamrks` |
| txaStreetAddress | `StreetAddress` | `@StreetAddress` | `StreetAddress` |
| chkIsActive | `IsActive` | `@IsActive` | `IsActive` |
| MarketSelect (hidden) | `MarketId` | `@MarketId` | `MarketId` |
| ddlStationType | `StationTypeId` | `@StationTypeId` | `StationTypeId` |
| ddlProgramType | `ProgramTypeId` | `@ProgramTypeId` | `ProgramTypeId` |
| ddlPharmaPlatform | `SMCTypeId` | `@SMCTypeId` | `SMCTypeId` |
| ddlDivision/District/Thana | `DivisionId/DistrictId/ThanaId` | `@DivisionId/@DistrictId/@ThanaId` | same names |
| docCat | `DoctorCategoryId` | `@DoctorCategoryId` | `DoctorCategoryId` |
| txtUnion | `UnionName` | `@UnionName` | `UnionName` |
| txtActiveDate + chkIsActive | `Activedate` (if active) / `InactiveDate` (if inactive) | `@Activedate` / `@InactiveDate` | `Activedate` / `InactiveDate` |
| session `UserId` | – | Save: `@EntryBy`; Update: `@UpdateBy` | `EntryBy`+`EntryDate=GETDATE()` / `UpdateBy`+`UpdateDate=GETDATE()` |
| (constants in SP) | – | – | Save sets `IsFromApp=0`, `ApprovalStatus='2'` (BR-M16-006); does **not** set `IsDoctor` (see §30) |

Child tables (SP bodies absent; table names from `sp_Update_DoctorMaster:796-809`):
| Grid / list | DAO | SP called per item | Table (deleted on update) |
|---|---|---|---|
| Degree ListBox (CSV of DegreeId) | string | `sp_Save_DoctorDegreeDetail(@DoctorId,@DegId)` | `tblDoctorDegreeDetail` |
| Speciality ListBox | string | `sp_Save_DoctorSpecialityDetail(@DoctorId,@SpecialityId)` | `tblDoctorSpecialityDetail` |
| Brand ListBox | string | `sp_Save_DoctorBrandDetail(@DoctorId,@BrandId)` | `tblDoctorBrandDetail` |
| gv_Contact | `DoctorContactDetailDAO` | `sp_Save_DoctorContactDetail_New(@DoctorId,@ContactTypeId,@ContactType,@Contact)` | `tblDoctorContactDetail` |
| gv_Special | `DoctorSpecialDayDAO` | `sp_Save_DoctorSpecialDayDetails(@DoctorId,@SpecialDayId,@SpecialDate)` | `tblDoctorSpecialDayDetail` |
| gv_Chamber | `DoctorChemberDetailDAO` | `sp_Save_BDoctorChemberDetail(@DoctorId,@ChamberTypeId,@Name,@Address,@Phone)` | `tblDoctorChemberDetail` (ChemberId PK, DoctorId, ChamberTypeId, Name, Phone, Address — from `sp_Get_DoctorList`) |

## 9. Dropdown Mapping
See §7 "Data source" column; all loaders are `try{...}catch(Exception){}` (silent, `DoctorEntry.aspx.cs:294-532`). Each list is filled once (non-postback). Division→District→Thana cascade uses `SeedDataDAL.GetDistrictList(divisionId)` / `GetThanaList(districtId)`. The Sales Person ListBox is bound **twice** (lines 313-323 with placeholder item, then 522-531 again without placeholder – the second bind replaces the first, so the "Please Select From List" item is lost; harmless).

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| Submit / Update | `btnSave_Click` (code:651) | validate name → collect grids → `DoctorDAL.SaveInfo` → success alert + redirect to DoctorView, else `faildalert("Already Exist!")` |
| Add To List (chamber) | `addButtonChamber_Click` | `ChemberValidation()` then `AddChamber()` appends to `gv_Chamber` |
| Remove (chamber/contact/special) | `deleteChamber_Click`, `deleteContact_Click`, `deleteSpecial_Click` | rebuild grid without the row |
| Add (contact / special day) | `lblContactAdd_Click`, `btnSpecialAdd_Click` | append row; **no validation** (empty contact / no date allowed; `ddlSpecialDay.SelectedItem.Text` used even when placeholder selected) |
| Reset | none | no OnClick handler |

## 11. Frontend Flow
Page_Load (first request): sets `txtActiveDate` = today; `LoadInitialInfo()` binds every dropdown and empty grids; if `MID` present → `btnUpdate` visible and `GetOneRecord(MID)`; else `btnSave` visible. `GetOneRecord` (code:62-288) loads master row via `sp_GET_DoctorMaster_ById`, fills controls, re-runs division/district cascades, sets hidden market hidden-fields (`hfGroupId, hfZone, hfArea, hfTeritory, hfSubTeritory, hfMarket`) consumed by the user control's own `Page_Load` (`IVMarketStructure.ascx.cs:15-148`), selects multi-select items by CSV, binds `gv_Chamber` to the same DataTable, loads contacts (`sp_GET_DoctorContactDetail_ById`) and special days (`sp_GET_DoctorSpecialDayDetail_ById`, wrapped in silent try/catch). Whole `GetOneRecord` is wrapped in `try{}catch(Exception){}` — any failure (e.g. a stored Gender text not in the list makes `SelectedValue` throw) leaves the form half-filled with no message. After the try, non-Admin restrictions are applied (BR-M16-012). UpdatePanel (`UpdatePanel2`) + spinner; sweetalert confirm wrappers in the master page.

## 12. Backend Flow
`btnSave_Click`:
1. `Validation()` → if `DoctorName.Text==""` mark invalid and return (no save).
2. Read the three grids into DAOs (empty strings → null; special date `DateTime.Parse(...).Date` in try/catch → null).
3. Build `DoctorMasterDAO` (nulls for unselected drops; sales person tags BR-M16-007; dates BR-M16-008); build CSV strings for degree/speciality/brand.
4. `DoctorDAL.SaveInfo(master, degrees, specialities, brands, chambers, specials, contacts, Session["UserId"])` (`DoctorDAL.cs:114-319`): opens `SalesDB`; if `DoctorId>0` → `UpdateData("sp_Update_DoctorMaster")` (adds `@UpdateBy`) else `SaveDataReturnPrimaryKey("sp_Save_DoctorMaster")` (adds `@EntryBy`; result of `SELECT SCOPE_IDENTITY()`; `pk>0` ⇒ success). If success and `pk>0`: loop chambers → `sp_Save_BDoctorChemberDetail`; degrees; specialities; brands; contacts; special days. `isSuccess` is **overwritten by each child SaveData** (always true unless exception).
5. `DataAccessManager` starts a transaction lazily on the first write and commits in `SqlConnectionClose()` (`DataAccessManager.cs:122-165, 407-450`); on exception the DAL calls `SqlConnectionClose(true)` (rollback) then rethrows. So master + children are one implicit transaction (INFERRED from reading the manager; not exercised).
6. Result: success ⇒ `successalert('Operation successful!','Success','DoctorView.aspx')`; `isSuccess==false` ⇒ `faildalert('Already Exist!')` (message is wrong for non-duplicate failures, BR-M16-014). Exceptions from the DAL are **not caught** in the page (unhandled → ASP.NET error page).

## 13. API Mapping
No `[WebMethod]`/JSON endpoints on this page. Postbacks only (UpdatePanel).

## 14. Service Mapping
No BLL layer; the page calls `DoctorDAL`, `CommonDataLoad`, `SeedDataDAL` directly (static `DoctorDAL _DAL` shared across requests, see §30).

## 15. Repository Mapping
`DoctorDAL.GetDoctorSetupById → sp_GET_DoctorMaster_ById(@id)`; `GetContactById → sp_GET_DoctorContactDetail_ById(@id)`; `GetSpecialDayById → sp_GET_DoctorSpecialDayDetail_ById(@id)`; `SaveInfo` (above). `CommonDataLoad.*_Active`, `SeedDataDAL.Get*List*` for dropdowns.

## 16. Stored Procedure Mapping
| SP | Role | Body in repo? |
|---|---|---|
| `sp_Save_DoctorMaster` | insert doctor (auto code, Approved) | **YES** `LIVE_DB_UPDATE_...sql:672-725` |
| `sp_Update_DoctorMaster` | update doctor + purge child rows | **YES** `:735-811` |
| `sp_Get_DoctorList` | list (used by DoctorView, not this page) | **YES** `:884-930` |
| `sp_GET_DoctorMaster_ById`, `sp_GET_DoctorContactDetail_ById`, `sp_GET_DoctorSpecialDayDetail_ById` | edit-load | no |
| `sp_Save_BDoctorChemberDetail`, `sp_Save_DoctorDegreeDetail`, `sp_Save_DoctorSpecialityDetail`, `sp_Save_DoctorBrandDetail`, `sp_Save_DoctorContactDetail_New`, `sp_Save_DoctorSpecialDayDetails` | child inserts | no |
| Lookup loaders (`sp_Get_*_Active`, `sp_GET_ProgramTypeListParm`, `sp_GET_SMCTypeListParm`, `sp_GetDoctor_SalesPerson_Active`, `sp_Get_Degree_All_ActiveByDoctorTypeId`, Division/District/Thana/StationType lists) | dropdowns | no |

`sp_Save_DoctorMaster` detail: params listed in §8; `SELECT @CustCodeint = MAX(CONVERT(INT,SUBSTRING(DoctorCode,2,LEN(DoctorCode)+1)))+1 FROM tblDoctorMaster WHERE ApprovalStatus='2'`; `@CustCode='D'+CONVERT(NVARCHAR(MAX),@CustCodeint)`; if `@TaggedSalesPersonIds IS NULL AND @SalesPersonId IS NOT NULL` then `@TaggedSalesPersonIds = CAST(@SalesPersonId AS NVARCHAR(50))`; single INSERT (no transaction/TRY), then `SELECT SCOPE_IDENTITY()`.
`sp_Update_DoctorMaster` detail: same tagged fallback; one UPDATE of every master column listed in §8 (unconditionally, including `Activedate` and `InactiveDate`, `SalesPersonId`, `TaggedSalesPersonIds`, `TaggedSalesPersonCodes`) `WHERE DoctorId=@DoctorId`; then `DELETE` from `tblDoctorBrandDetail`, `tblDoctorDegreeDetail`, `tblDoctorSpecialityDetail`, `tblDoctorSpecialDayDetail`, `tblDoctorContactDetail` for the doctor; then `DELETE tblDoctorChemberDetail WHERE DoctorId=@DoctorId AND ChemberId NOT IN (SELECT ChemberId FROM tbl_PrescriptionMaster WHERE DoctorId=@DoctorId UNION ALL SELECT ChemberId FROM tbl_DCRInfo WHERE DoctorId=@DoctorId)`.
`sp_Get_DoctorList`: dynamic SQL; `@Parm` appended to `WHERE DM.DoctorId is not null and isnull(DM.IsDoctor,0)=1`; `ORDER BY DM.DoctorCode DESC`; joins detailed in the README; `TaggedSalesPersonIds` are resolved to `EmpMasterCode : EmpName` list via `STUFF(... FOR XML PATH)` with `(',' + Tagged + ',') LIKE '%,'+EmpInfoId+',%'`, fallback to `SalesPersonId` employee (`EGI`) when tagged list is empty; approval status decoded `0 Pending, 1 Verified, 2 Approved, 3 Rejected` into `ApprovalStatusWeb`.

## 17. Database Mapping
Written: `tblDoctorMaster` (insert/update), `tblDoctorChemberDetail`, `tblDoctorDegreeDetail`, `tblDoctorSpecialityDetail`, `tblDoctorBrandDetail`, `tblDoctorContactDetail`, `tblDoctorSpecialDayDetail` (delete+insert). Read for the purge guard: `tbl_PrescriptionMaster`, `tbl_DCRInfo`. Read for list: many (README §Data model). Child SP insert column mapping `UNKNOWN`.

## 18. Validation Matrix
| Rule | Where | Message / effect |
|---|---|---|
| Doctor name non-empty | server `Validation()` (`DoctorEntry.aspx.cs:589-595`) | css `is-invalid`, tooltip "please fill out this field"; no alert; save stops |
| Chamber name non-empty | server `ChemberValidation()` (:619-625) | same tooltip |
| Chamber type selected | :626-632 | same |
| Phone 11 chars if entered | :633-645 | "Mobile NO must be 11 digits!" (`showpop6`) |
| Phone digits only, max length 11 | client (FilteredTextBoxExtender, MaxLength) | – |
| Everything else (designation, gender, type, market, sales person, dates, contacts) | none | accepted empty |
| Duplicate doctor | none in C#/SP | no duplicate check exists (`sp_Save_DoctorMaster` has none) |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M16-001 | Doctor name is mandatory; save is blocked (no popup) when empty | `Solution.Web/MasterSetup_UI/DoctorEntry.aspx.cs:582-608` | `Validation()` | CONFIRMED |
| BR-M16-002 | No other field is validated server-side (designation, gender, doctor type, category, market, sales person all optional) despite red asterisks | `DoctorEntry.aspx.cs:582-608,747-823` | absence of checks | CONFIRMED |
| BR-M16-003 | A chamber row needs name and chamber type; phone optional but exactly 11 characters when supplied | `DoctorEntry.aspx.cs:611-648` | `ChemberValidation()` | CONFIRMED |
| BR-M16-004 | `DoctorId>0` (from `MID`) ⇒ update path (`sp_Update_DoctorMaster`, `@UpdateBy`=session UserId); else insert path (`sp_Save_DoctorMaster`, `@EntryBy`=session UserId) | `Library.DAL/MasterSetup_DAL/DoctorDAL.cs:153-174` | `SaveInfo` | CONFIRMED |
| BR-M16-005 | New doctor code = `'D'` + (highest numeric suffix among doctors whose `ApprovalStatus='2'`) + 1; concurrent saves can get the same code; doctors in other approval states are ignored; first doctor would yield NULL code; non-numeric suffix causes conversion error | `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:702-703` | `sp_Save_DoctorMaster` | CONFIRMED |
| BR-M16-006 | A doctor created through this page is stored immediately as Approved (`ApprovalStatus='2'`) with `IsFromApp=0` | `LIVE_DB_...sql:719` | `sp_Save_DoctorMaster` | CONFIRMED |
| BR-M16-007 | Sales-person tagging: `TaggedSalesPersonIds` = CSV of selected EmpInfoIds; `TaggedSalesPersonCodes` = CSV of the text before `:` of each selected item; `SalesPersonId` = first selected. If Tagged is NULL and SalesPersonId given, SP sets Tagged=SalesPersonId. When nothing is selected the page sends `""` (not NULL) so the fallback does not run | `DoctorEntry.aspx.cs:763-776`; SP `:705-708,:763-766` | page + SPs | CONFIRMED |
| BR-M16-008 | Active doctor ⇒ `Activedate` = page-load date; inactive ⇒ `InactiveDate` = page-load date; the update SP overwrites **both** columns each time (the other becomes NULL; date is "today when the edit page was opened", not the original) | `DoctorEntry.aspx.cs:45,814-823`; SP `:774,:789` | page + `sp_Update_DoctorMaster` | CONFIRMED |
| BR-M16-009 | Degree choices depend on Doctor Type (`sp_Get_Degree_All_ActiveByDoctorTypeId`); changing type clears the degree list | `DoctorEntry.aspx.cs:1041-1068`; `CommonDataLoad.cs:1110-1117` | postback | CONFIRMED |
| BR-M16-010 | Update replaces child data: brand, degree, speciality, special-day, contact rows of the doctor are deleted and re-inserted from the form; chambers are deleted (except BR-M16-011) and re-inserted from the form | SP `:796-809`; `DoctorDAL.cs:181-280` | `sp_Update_DoctorMaster` + `SaveInfo` | CONFIRMED |
| BR-M16-011 | A chamber referenced by any prescription (`tbl_PrescriptionMaster.ChemberId`) or DCR (`tbl_DCRInfo.ChemberId`) of that doctor is NOT deleted on update | SP `:802-809` | `sp_Update_DoctorMaster` | CONFIRMED |
| BR-M16-012 | For non-Admin `RoleTypeName`, in edit mode SecondaryCode/UPCode/Remarks/Union become read-only and StationType/ProgramType/PharmaPlatform/Division/District/Thana are disabled (most are hidden anyway; ProgramType and PharmaPlatform are visible and become disabled ⇒ a disabled DropDownList does not post, so the previous selection is kept via viewstate) | `DoctorEntry.aspx.cs:238-286` | `GetOneRecord` tail | CONFIRMED (effect on postback INFERRED) |
| BR-M16-013 | Market/hierarchy, division/district/thana, station type, union, remarks, secondary code and UP code inputs are hidden; a new doctor is therefore saved with these NULL and an edit round-trips the loaded values through viewstate. How a new doctor gets a market/territory is not visible in this page (sales-person tag / mobile app / transfer page — UNKNOWN) | `DoctorEntry.aspx:79,95,265,373-433,796`; `DoctorEntry.aspx.cs:40` | markup + code | INFERRED |
| BR-M16-014 | Any `isSuccess==false` from the DAL is reported as "Already Exist!" although no duplicate check exists in code/SP | `DoctorEntry.aspx.cs:864-872` | `btnSave_Click` | CONFIRMED |
| BR-M16-015 | A special-day row with an unparsable/empty date is stored with NULL date, silently | `DoctorEntry.aspx.cs:705-713` | `btnSave_Click` | CONFIRMED |
| BR-M16-016 | Program Type list restricted to program types whose group has `IsDoctor=1`; Pharma Platform (SMC type) list restricted to groups with `forDotor=1` | `DoctorEntry.aspx.cs:491,508` | `LoadInitialInfo` | CONFIRMED (filter text only; SP body absent) |

## 20. Permission Matrix
Enforced in code: only the non-Admin field-lock in BR-M16-012 (`Session["RoleTypeName"]`, wrapped in try/catch). **No page-level permission check** (`GetPermissionForUserRole` is not called on this page, unlike `DoctorView`). Menu visibility (DB `sp_GET_MainMenuRole2`) is the only gate; direct URL access by any authenticated user is possible (Forms auth only, `Web.config`). `Session["UserId"]` is dereferenced without null check (`btnSave_Click:863`) ⇒ NullReference if session expired.

## 21. Status Lifecycle
`tblDoctorMaster.ApprovalStatus`: this page's insert path writes `'2'` (Approved). Decode (from `sp_Get_DoctorList`): `0 Pending`, `1 Verified`, `2 Approved`, `3 Rejected`. Other transitions (Verified/Rejected) are performed by `Approval_UI/DoctorApprovalList.aspx` via `SaveDoctor_ApplogDAL` / `sp_webapi_SaveDoctorAppLog` (outside this page; see README). Update SP does not touch `ApprovalStatus`. `IsActive`/`Activedate`/`InactiveDate` is the separate active flag (BR-M16-008).

## 22. Error Handling
Dropdown loaders and `GetOneRecord` swallow all exceptions (silent). `SaveInfo` rethrows after rollback; the page does not catch ⇒ default ASP.NET error page. Validation feedback via css/tooltips and `showpop6`/sweetalert.

## 23. Audit
`EntryBy`/`EntryDate` (Save) and `UpdateBy`/`UpdateDate` (Update) on `tblDoctorMaster` (session UserId). Child-table audit columns `UNKNOWN`. No change-history table.

## 24. Integration
Doctor data feeds mobile app (`IsFromApp` flag, `sp_webapi_*` SPs) — INFERRED; no direct integration code on this page.

## 25. Reports
Downstream consumers: DoctorInfoReport (PAGE-674), monthly pivot reports (PAGE-671/672/673/675), DCR/DCP/RX lists (via doctor id).

## 26. Dependencies
`AjaxControlToolkit` (`FilteredTextBoxExtender`), `select2`/datepicker/sweetalert in master page, `Library.DAL.*`.

## 27. Test Cases
| # | Case | Expected | BR |
|---|---|---|---|
| T1 | Submit with empty doctor name | no save; name box red | BR-M16-001 |
| T2 | Submit with only a name | doctor saved, code `D<n+1>`, Approved, appears in DoctorView only if `IsDoctor` defaults to 1 (verify) | 002,005,006 |
| T3 | Add chamber with 10-digit phone | "Mobile NO must be 11 digits!" | 003 |
| T4 | Add chamber without type | type box invalid | 003 |
| T5 | Select 2 sales persons A,B | `SalesPersonId`=A; `TaggedSalesPersonIds`="A,B"; codes CSV | 007 |
| T6 | Select none | `TaggedSalesPersonIds`="" and `SalesPersonId` NULL | 007 |
| T7 | Edit an inactive doctor and save unchanged | `Activedate` becomes NULL, `InactiveDate`=today | 008 |
| T8 | Edit doctor who has DCR at chamber X; remove X from grid; save | chamber X survives; other removed chambers deleted | 010,011 |
| T9 | Change Doctor Type | degree list refreshed, previous selection lost | 009 |
| T10 | Two users save new doctors simultaneously | possible duplicate `DoctorCode` (race) | 005 |
| T11 | Session expired then submit | unhandled NullReference | §20 |
| T12 | Non-admin edits doctor | Program Type & Pharma Platform disabled | 012 |

## 28. Reverse Engineering References
`DoctorEntry.aspx`, `DoctorEntry.aspx.cs`, `IVMarketStructure.ascx.cs`, `DoctorDAL.cs`, `DoctorMasterDAO.cs`, `DataAccessManager.cs`, `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:666-930`.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Mandatory fields | None existed | Only doctor name enforced | – | n/a | BR-M16-001/002 |
| Doctor code | None existed | `D`+max+1 among approved | SP body read | documented | BR-M16-005 |
| Child SP behavior | None existed | – | bodies absent | UNKNOWN | §16 |

## 30. Known Gaps
See `specs/doctor-visit-prescription/GAPS-M16.md`: GAP-M16-001 (code generation race/NULL), -002 (`IsDoctor` not set by `sp_Save_DoctorMaster` while `sp_Get_DoctorList` filters `IsDoctor=1`; new doctors may not appear unless column has a default), -003 (hidden market inputs), -004 (Activedate/InactiveDate overwritten), -005 (misleading "Already Exist!"), -006 (static DAL/DataAccessManager shared across requests), -007 (chamber re-insert duplicates chambers preserved by the purge guard unless `sp_Save_BDoctorChemberDetail` dedupes; UNKNOWN), -008 (no page permission), -009 (silent catch blocks), -010 (`Reset` button has no handler).

## 31. Change Impact
`sp_Get_DoctorList` is also used by DoctorView (PAGE-670), Customer_Doctor_Transfer (PAGE-668), `Approval_UI/DoctorApprovalList`, `TransferUI/*` pages (they instantiate `DoctorDAL`). `DoctorDAL.SaveInfo` is used only by this page. Changing `tblDoctorMaster` columns affects reports PAGE-671..675, `sp_Rpt_*DoctorWeek`, DCR/RX SPs (`UNKNOWN`). `CommonDataLoad.*_Active` loaders are shared with DoctorView, Prescription pages and report pages.
