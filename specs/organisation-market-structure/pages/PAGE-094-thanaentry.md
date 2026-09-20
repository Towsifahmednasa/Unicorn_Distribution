---
page_id: PAGE-094
module: M03
page: ThanaEntry
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-094 — ThanaEntry

> **Evidence level:** Tier A. `ThanaEntry.aspx` (339 lines incl. JS), `ThanaEntry.aspx.cs` (75 lines), `Library.DAL/Thana_DAL/ThanaDal.cs`, `ThanaDao` read in full; `District_Entry.aspx` JS calls into this page's WebMethods verified by grep. SP bodies absent (`UNKNOWN`). This page is the geography master for **Thana/Upazila** (Division → District → Thana); the same code-behind also serves PAGE-090 District_Entry.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / Module | PAGE-094 / M03 |
| Name | ThanaEntry ("Thana Setup", `.aspx:27`) |
| Kind | Entry — create/edit a Thana (police-station/upazila) under a District |
| Folder / master | Thana_UI / NewMasterPage.master |
| Class | `Thana_UI_ThanaEntry`; static `ThanaDal _Thana` (`.cs:16`) |

## 2. Purpose
CONFIRMED: maintain the **Thana/Upazila** list: pick Division, pick District (filtered by division), type the Thana name, Submit. Edit mode (`?id=`) preloads division/district/name and switches the button caption to "Update".

## 3. Business Objective
INFERRED: Thana is the geographic key used by MarketSetup (PAGE-042: Division→District→Thana on every market) and, in earlier designs, territory↔thana tagging; a clean geography master supports market location reporting.

## 4. Navigation
Back link `Thana_View.aspx` (PAGE-093) (`.aspx:33`); success → `Thana_View.aspx` (`.aspx:285`); Reset = `location.reload()` (`ResetLink`).

## 5. Route
`~/Thana_UI/ThanaEntry.aspx[?id=<ThanaId>]`

## 6. Source Files
`Solution.Web/Thana_UI/ThanaEntry.aspx(.cs)`; `Library.DAL/Thana_DAL/ThanaDal.cs` (namespace `SalesSolution.Web.DataLayer`); `Library.DAO/Thana_DAO/ThanaDao.cs`; shared helper JS (`SelectOption_DtTable_Async_True`, `LoadSelectOption_ById`, `successalert`, `faildalert`, `_errorWithMsg`) defined outside the page (master/shared scripts — content not reviewed).

## 7. UI Control Inventory
| Control | Label | Required | Default | State | Source | Message |
|---|---|---|---|---|---|---|
| `#DivisionDDL` | Division | **Yes** (client) | first item | – | `ThanaEntry.aspx/Get_Division_All_DDL` → `sp_Get_Division_Active_DDL` (`DivisionId`,`DivisionName`) | tooltip "Please fill out of this field!" |
| `#DistrictDDL` | District | **Yes** (client) | empty until division chosen | reloaded on division `change` if value > 0 | `Get_District_All_DDL(id=divisionId)` → `sp_Get_District_Active_DDL(@id)` (`DistrictId`,`DistrictName`) | same |
| `#ThanaName` | Thana/Upazila | **Yes** (client, non-empty) | empty | – | – | same |
| `#btnSave` | Submit / "Update" (set in edit) | – | – | – | – | – |
| Reset button | Reset | – | – | reloads page | – | – |
| `#masterId` (hidden input) | – | – | 0 | set from `?id` | – | – |
`IsActiveChange()` and `#customSwitch1`/`#pacinTxt` in the JS refer to controls that do not exist on this page (leftover).

## 8. Field Mapping
| UI | JSON key (`ThanaDao`) | SP param | Column |
|---|---|---|---|
| `#masterId` | `ThanaId` (string "0" → int) | `@id` | UNKNOWN |
| `#DistrictDDL` | `district_id` | `@district_id` | UNKNOWN |
| `#ThanaName` | `ThanaName` | `@ThanaName` | UNKNOWN |
| Session UserId | – | `@CreatedBy` (insert) / `@UpdateBy` (update) | UNKNOWN |
`Division` is UI-only (not sent).

## 9. Dropdown Mapping
Division → District cascade through two `[WebMethod]`s returning `JsonConvert.SerializeObject(DataTable)`; edit preload calls `GetDisivionAll(divisionId)` then `GetDistrictByDisivion(districtId, divisionId)` after `Get_Thana_By_Id` (`sp_GET_ThanaInfo_ById`, columns `DivisionId, DistrictId, ThanaName`).

## 10. Button Action Mapping
| Button | Handler | Chain | SP | Change |
|---|---|---|---|---|
| Submit/Update | `Save()` → `Validation()` → `FinalSave()` (jQuery `$.ajax` POST `ThanaEntry.aspx/Save_ThanaInfo` body `{"thanaDao":{ThanaId,district_id,ThanaName}}`) | `ThanaDal.SaveThanaInfo(dao, Session["UserId"])` | `sp_check_ThanaInfo`, `sp_UD_ThanaInfo`, `sp_Save_ThanaInfo` | write |
| Reset | `ResetLink()` | `location.reload()` | – | – |

## 11. Frontend Flow
`$(function)` reads `?id` via `getUrlVars()`: `id>0` → `GetThanaEdit(id)` (synchronous `$.ajax`, `async:false`) → fills selects and name, caption "Update"; else `GetDisivionAll(0)`. Division `change` (value>0) → `GetDistrictByDisivion(0, divisionId)`. `Validation()` clears/sets `is-invalid` and tooltips (`ValidationTooltip`); `isValid` is an implicit global. Result handling: `isSuccess` → `successalert('Operation successful!','Success','Thana_View.aspx')`; `isValiCheck` → 'Data cannot be deactivated!'; else 'Operation Faild!'; AJAX error → 'Operation Faild!'.

## 12. Backend Flow
`Save_ThanaInfo` → `ThanaDal.SaveThanaInfo` (`sessionUser` = `Session["UserId"].ToString()`; `EnableSession=true`):
- **Edit** (`ThanaId>0`): `sp_check_ThanaInfo(@id = thana.district_id, @Name = ThanaName)` — note the *district id* is passed as `@id`; rows=0 → `sp_UD_ThanaInfo(@id,@district_id,@ThanaName,@UpdateBy)`, `isSuccess` = result; rows>0 → `isSuccess=false`.
- **New**: `pk = SaveDataReturnPrimaryKey("sp_Save_ThanaInfo")` (`@id,@district_id,@ThanaName,@CreatedBy`); `pk>0` → `isSuccess=true`, else remains false.
- One implicit transaction per call; exception → rollback, `ErrorMessage` set, rethrown → AJAX `error` callback.
`Save_DistictInfo` (same class) is the analogous district save used by `District_Entry.aspx:272`. The four `Get_*` WebMethods return JSON for the dropdowns and edit preload.

## 13. API Mapping
| WebMethod | Session | Purpose | Chain |
|---|---|---|---|
| `Save_ThanaInfo(ThanaDao)` | `EnableSession=true` | save thana | `ThanaDal.SaveThanaInfo` |
| `Save_DistictInfo(ThanaDao)` | `EnableSession=true` | save district (used by PAGE-090) | `ThanaDal.Save_DistictInfo` → `sp_check_DistictInfo`, `sp_UD_DistictInfo`, `sp_Save_DistictInfo` |
| `Get_Division_All_DDL()` | none | active divisions JSON | `GetDivision_All_DDL` → `sp_Get_Division_Active_DDL` |
| `Get_District_All_DDL(id)` | none | active districts of division | `GetDistrict_All_DDL` → `sp_Get_District_Active_DDL` |
| `Get_Thana_By_Id(id)` | none | edit preload | `sp_GET_ThanaInfo_ById` |
| `Get_District_By_Id(id)` | none | district edit preload (PAGE-090) | `sp_GET_DistrictInfo_ById` |
Response shape for Save: `{d:{isSuccess,isDuplicateCheck,isValiCheck,isError,ErrorMessage,Id}}` (`ResultInfo`).

## 14. Service Mapping
No BLL (page → DAL).

## 15. Repository Mapping
`ThanaDal.SaveThanaInfo`, `Save_DistictInfo`, `GetDivision_All_DDL`, `GetDistrict_All_DDL`, `Get_Thana_By_Id_Dal`, `Get_District_By_Id` (`Library.DAL/Thana_DAL/ThanaDal.cs`), plus list methods `Get_Thana_All_List`, `Get_District_All_List`, `GetDivision_All_List` used by PAGE-091/092/093.

## 16. Stored Procedure Mapping
| SP | Nature | Params | Body |
|---|---|---|---|
| `sp_Save_ThanaInfo` | insert, returns pk | `@id,@district_id,@ThanaName,@CreatedBy` | NOT in repo |
| `sp_UD_ThanaInfo` | update | `@id,@district_id,@ThanaName,@UpdateBy` | NOT in repo |
| `sp_check_ThanaInfo` | duplicate check | `@id,@Name` | NOT in repo |
| `sp_GET_ThanaInfo_ById` | read | `@id` | NOT in repo |
| `sp_Get_Division_Active_DDL`, `sp_Get_District_Active_DDL` | read | –, `@id` | NOT in repo |

## 17. Database Mapping
No inline SQL; tables UNKNOWN.

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Message | BR |
|---|---|---|---|---|---|
| Division | `val()==0` → invalid | yes | none | "Please fill out of this field!" | BR-M03-078 |
| District | same | yes | none | same | BR-M03-078 |
| Thana name | non-empty | yes | none (no trim/length/charset) | same | BR-M03-078 |
| Duplicate on edit | SP rows | – | `SaveThanaInfo` | "Operation Faild!" (generic) | BR-M03-080 |
| Duplicate on create | not checked in C# | – | – | – | BR-M03-081 |

## 19. Business Rules
| ID | Rule | Source | Impl | Conf |
|---|---|---|---|---|
| BR-M03-078 | Division, District and Thana/Upazila name are mandatory (client-side only; the WebMethod re-validates nothing, so direct calls can save empty values). | ThanaEntry.aspx:215-249 | JS | CONFIRMED |
| BR-M03-079 | District choices are only the *active* districts of the chosen division; division list = active divisions. | ThanaEntry.aspx:160-166,185-198; ThanaDal (`sp_Get_*_Active_DDL`) | JS/DAL | CONFIRMED |
| BR-M03-080 | On edit, saving is refused when `sp_check_ThanaInfo(@id=<district id>,@Name)` returns rows (i.e. a same-named thana in that district — exact semantics UNKNOWN); the user only sees "Operation Faild!". | ThanaDal.SaveThanaInfo | DAL | CONFIRMED (call) / UNKNOWN (semantics) |
| BR-M03-081 | On create the outcome depends solely on `sp_Save_ThanaInfo` returning a pk>0; C# does no duplicate pre-check; pk≤0 ⇒ generic failure. | ThanaDal.SaveThanaInfo | DAL | CONFIRMED |
| BR-M03-082 | Edit can move a thana to another district (district dropdown is editable and `@district_id` is updated). | ThanaEntry.aspx:307-334; ThanaDal | UI/DAL | CONFIRMED (params) |
| BR-M03-083 | Thana has no active/inactive flag or deactivate rule on this page; the client's `isValiCheck` branch ('Data cannot be deactivated!') is unreachable for this WebMethod (DAL never sets it). | ThanaEntry.aspx:287-290; ThanaDal | JS/DAL | CONFIRMED |

## 20. Permission Matrix
`Save_*` WebMethods use `EnableSession=true` and dereference `Session["UserId"]` (unauthenticated call → `NullReferenceException` = effectively blocked, no explicit check). The four `Get_*` WebMethods are **not** session-guarded and — because `Web.config` has no `<authorization>` element (grep) — can be called anonymously via `POST /Thana_UI/ThanaEntry.aspx/Get_Division_All_DDL` etc. (INFERRED reachable; data = geography lists only). No role check anywhere.

## 21. Status Lifecycle
None (no status/active column handled; see BR-M03-083).

## 22. Error Handling
Server exceptions propagate (DAL rethrows) → jQuery `error` callback → "Operation Faild!". `GetThanaEdit` has no `error` handler. No logging.

## 23. Audit
`@CreatedBy` / `@UpdateBy` = UserId (string). `ThanaDao` also has `CreatedDate/UpdateDate` fields not set by C#.

## 24. Integration
None.

## 25. Reports
None (lists PAGE-091/092/093).

## 26. Dependencies
Shared JS helpers; `sp_check_ThanaInfo`; consumers: MarketSetup (PAGE-042) via `sp_Get_ThanaList_OnlyActive_Bydistrict_id`, Thana_View (PAGE-093).

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Submit empty | three fields flagged, no AJAX | BR-M03-078 |
| TC-02 | Choose division | districts of division only (active) | BR-M03-079 |
| TC-03 | Create new thana | success → Thana_View | BR-M03-081 |
| TC-04 | Edit rename to existing name in district | "Operation Faild!" | BR-M03-080 |
| TC-05 | Edit move to different district | saved with new district | BR-M03-082 |
| TC-06 | POST `Save_ThanaInfo` with empty name (Fiddler) | accepted (no server validation) | BR-M03-078 |
| TC-07 | Anonymous POST `Get_Division_All_DDL` | JSON returned (verify) | §20 |
| TC-08 | Session expired then Save | server NullReference → "Operation Faild!" | §20 |

## 28. Reverse Engineering References
`Solution.Web/Thana_UI/ThanaEntry.aspx(.cs)`, `Solution.Web/Thana_UI/District_Entry.aspx:186-309`, `Library.DAL/Thana_DAL/ThanaDal.cs`, `Library.DAO/Thana_DAO/ThanaDao.cs`.

## 29. Spec vs Actual
| Area | Spec | Actual | DB | Status | Ref |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Server validation | None existed | none | – | GAP | BR-M03-078 |

## 30. Known Gaps
No server-side validation; `sp_check_ThanaInfo` receives the district id as `@id`; generic failure message hides duplicates; anonymous readable WebMethods; sync AJAX; leftover JS for missing controls; no active flag; SP bodies absent (GAP-M03-001).

## 31. Change Impact
`ThanaEntry.aspx/Save_DistictInfo` and `Get_District_By_Id` are consumed by `District_Entry.aspx` (PAGE-090); `sp_Get_District_Active_DDL` etc. may be used by other pages' geography pickers. Thana ids are referenced by MarketSetup (PAGE-042).
