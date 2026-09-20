---
page_id: PAGE-658
module: M16
page: DCRList
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-658 — DCRList

> **Evidence level:** Tier A — `DCRList.aspx`, `DCRList.aspx.cs` (413 lines), `IVMasterStructureForDCR.ascx.cs`, `DoctorVisitDAL.cs`, `CustomerInfoDAL.SaveDCR_Applog_DAL`, `OrderSaveApprovalLogDAO`. SP bodies (`sp_Get_DCRInfoListForView`, `sp_webapi_SaveDCRAppLog`) absent. Reviewed 2026-09-19.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-658 |
| Module | M16 |
| Page | DCRList (Doctor Call Report list with approve/reject toggle) |
| Folder | DoctorVisit_UI |
| User control | `IVMasterStructureForDCR.ascx` (registered as `IVMarketStructure`; forced hidden) |

## 2. Purpose
List the doctor call reports (DCR = one row per doctor visit reported by a field user), filter them, open a DCR detail, and let a manager **accept or reject** a DCR. CONFIRMED (`loadGridView_RowCommand`).

## 3. Business Objective
Managers validate that visits reported by MIO/MIA are genuine before they count in DCR/coverage reports (monthly pivots take an `@ApprovalStatus` parameter). INFERRED.

## 4. Navigation
Menu row DB-driven (`UNKNOWN`). View button → `DCRReport.aspx?id=<DcrId>` (`DCRList.aspx.cs:70-76`). `MenuId = 382` is hard-coded in the approval log (`:116`) which suggests the DB menu id of this page (INFERRED).

## 5. Route
`~/DoctorVisit_UI/DCRList.aspx`.

## 6. Source Files
`Solution.Web/DoctorVisit_UI/DCRList.aspx`, `.aspx.cs`, `IVMasterStructureForDCR.ascx(.cs)`, `Library.DAL/DoctorVisit_DAL/DoctorVisitDAL.cs:297-315` (`GetDCRList`), `Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs:1341-1400` (`SaveDCR_Applog_DAL`), `Library.DAO/MasterSetup_DAO/CustMasterInfoDAO.cs:270-304` (`OrderSaveApprovalLogDAO`), `CommonDataLoad.GetEmpMarketStructure_Active`, `SeedDataDAL.GetApprovalStatusList/GetSMCTypeListParam`, `SetupDAL.Get_UserRoleInfo`.

## 7. UI Control Inventory
| Control | Label | Required | Default | Notes |
|---|---|---|---|---|
| FromDate / ToDate (TextBox, datepicker) | From Date / To Date | No | today ("dd MMMM, yyyy") both | filter on `CONVERT(date,DCR.EntryDate)` |
| ApprovalStatusSelect | Approval Status | No | placeholder | `SeedDataDAL.GetApprovalStatusList` → `sp_GET_ActionStatusList` (value `SoftwareUseId`, text `WebShow`) |
| EmployeeIdSelect | Employee | No | placeholder | `CommonDataLoad.GetEmployeeList_Active` → `sp_CS_GetEmpGeneralInfo_All` |
| UserRoleSelect | User Role | No | placeholder | `SetupDAL.Get_UserRoleInfo` → `sp_Get_UserRoleInfo` (UserRoleID/RoleName) |
| ddlPharmaPlatform | Pharma Platform | No | placeholder | `GetSMCTypeListParam(" and GRP.forDotor=1")` |
| IVMarketStructure (UC) | Zone/Area/Territory | – | – | `Visible=false` (`DCRList.aspx.cs:38`) ⇒ never rendered ⇒ its values are always empty (BR-M16-031) |
| Search / Reset | – | – | – | `btnSearch_Click` → `LoadData`; Reset → `Response.Redirect("DCRList.aspx")` |
| loadGridView (key `DcrId`) | columns: DCR Date, Doctor Name, Sales Person Name, Doctor Address, Visit Type (`TourTypeName`), Visited With, Chamber, Comments (`Remarks`), Role, Location (Google-maps link from `POutLoc`), Visited Address, Doctor Street Address, Approval Status, Action | – | – | Action buttons: View (`EditData`), Approve (`ApproveData`), Reject (`RejectData`) |

## 8. Field Mapping
Grid ← `sp_Get_DCRInfoListForView`: `DcrId, DcrDate, DoctorName, DCRSenderName, DoctorAddress, TourTypeName, VisitedWith, ChamberName, Remarks, RoleName, POutLoc, StreetAddress, DoctorStreet, ApprovalStatus` (from BoundField names). Approval write mapping in §12.

## 9. Dropdown Mapping
See §7. All bound in `LoadInitialInfo()` inside try/catch (silent).

## 10. Button Action Mapping
| Button | Command | Visibility | Effect |
|---|---|---|---|
| View | `EditData` | always | redirect to `DCRReport.aspx?id=<DcrId>` |
| Approve (✓) | `ApproveData` | only if row `ApprovalStatus` text `== "Rejected"` (aspx:284-286) | writes approval log with Status `Accepted` |
| Reject (✗) | `RejectData` | only if row `ApprovalStatus` text `== "Approved"` (aspx:287-289) | writes approval log with Status `Rejected` |
(Hence a DCR in any other state, e.g. "Pending" — text values UNKNOWN — shows no approve/reject buttons.)

## 11. Frontend Flow
Page_Load (whole body in `try{}catch(Exception){}`): find hidden hierarchy dropdowns, hide the user control, read `RoleTypeName`, `EmpInfoId`, `RoleTypeId` from Session; first load sets both dates to today, binds dropdowns and `LoadData()`. DataTables (client) is initialised over the GridView with Excel/PDF/print buttons (`aspx:317-350`). Google Maps link opens `http://maps.google.com/?q=<POutLoc>` (plain http).

## 12. Backend Flow
**Listing** — `LoadData()` → `DoctorVisitDAL.GetDCRList(param())` → `sp_Get_DCRInfoListForView(@param)`. `param()` (`:174-283`) concatenates raw SQL text:
- dates: both set → `AND CONVERT(date,DCR.EntryDate) BETWEEN '<from>' AND '<to>'`; only From → up to today `dd-MMM-yyyy`. (Only To given ⇒ no date filter.)
- `ApprovalStatusSelect` → `AND DCR.ApprovalStatus='<v>'`; `UserRoleSelect` → `AND us.UserRoleID='<v>'`; `EmployeeIdSelect` → `AND dtl.EmpInfoId='<v>'`; `ddlPharmaPlatform` → `AND DCR.SmcTypeId_Dcr='<v>'`; zone/area/territory → `DCR.RegionId/AreaId/TerritoryId` (hidden control ⇒ empty).
- **Role scoping** from `CommonDataLoad.GetEmpMarketStructure_Active(EmpInfoId)` (`sp_Get_EmployeeFieldForceInfo_EmpId`): `MIO` ⇒ `AND View_Webapi_EmployeeFieldForceInfo.MIOEmpId=<MIOEmpId>`; `AM` ⇒ `...ASMEMPId=<ASMEMPId>`; `DZSM` ⇒ `...RSMEMPId=<RSMEMPId>`; `NSM` ⇒ `...NSMEMPId=<NSMEMPId>`; any other role (Admin, HO users) ⇒ no scoping. `dtMarket.Rows[0]` is dereferenced without a row check (IndexOutOfRange ⇒ unhandled if the employee has no field-force row).
- A `[WebMethod] GetDCRList(string param)` (`:359-413`) duplicates the same role scoping for a client-supplied `param` (no caller found in the aspx ⇒ apparently unused; INFERRED).

**Approve / Reject** (`loadGridView_RowCommand:78-131`):
1. `status = "Accepted"` (ApproveData) or `"Rejected"`; `dcrId = DataKeys[row]["DcrId"]`.
2. `GetLatestDCRApprovalLog(dcrId)` — inline parameterised SQL over the `SalesDB` connection string from `ConfigurationManager` (`:133-161`): `SELECT TOP 1 ISNULL(L.DCRApprovalId,0), DCR.DcrId, ISNULL(L.ToEmpId,0), ISNULL(L.Step,0), ISNULL(L.GroupId,DCR.GroupId), ISNULL(L.RegionId,DCR.RegionId), ISNULL(L.AreaId,DCR.AreaId), ISNULL(L.TerritoryId,DCR.TerritoryId) FROM tbl_DCRInfo DCR LEFT JOIN tblDCRApprovalLog L ON L.TableId=DCR.DcrId WHERE DCR.DcrId=@DcrId ORDER BY L.DCRApprovalId DESC`.
3. If a row is returned (always, for an existing DCR): build `OrderSaveApprovalLogDAO`: `OrderApprovalId=DCRApprovalId`, `TableId=dcrId`, Group/Region/Area/Territory from above, `FromEmpId=Session EmpInfoId (0 if empty)`, `ToEmpId=previous ToEmpId`, `Step=previous Step+1`, `Type="DCR"`, `Status`, `Date=EntryDateS=ApproveDateS=DateTime.Now`, `EntryByS=EntryByApp=Session UserId`, `MenuId=382` (other fields null: `ApproveByS`, `Comments`, `ToGroupId…`).
4. `CustomerInfoDAL.SaveDCR_Applog_DAL` → `SaveData("sp_webapi_SaveDCRAppLog", 26 params: @DCRApprovalId,@Date,@FromEmpId,@ToEmpId,@TableId,@Status,@Comments,@Type,@Step,@GroupId,@RegionId,@AreaId,@TerritoryId,@ToGroupId..@ToTerritoryId,@EntryByS,@EntryDateS,@EntryTimeS,@ApproveByS,@ApproveDateS,@ApproveTimeS,@EntryByApp,@EntryDateApp,@EntryTimeApp,@ApproveByApp,@ApproveDateApp,@ApproveTimeApp,@MenuId)`).
5. Success → `ShowSuccesalert("Operation Approved/Rejected successful!")` and `LoadData()`; failure → `faildalert("Operation failed!")`. The SP presumably updates `tbl_DCRInfo.ApprovalStatus` (UNKNOWN: how "Accepted" maps to the list text "Approved").

## 13. API Mapping
`[WebMethod] DoctorVisit_UI_DCRList.GetDCRList(param)` (see §12) — no auth check.

## 14. Service Mapping
No BLL; page → `DoctorVisitDAL`, `CustomerInfoDAL` (created per click), `CommonDataLoad`.

## 15. Repository Mapping
`DoctorVisitDAL.GetDCRList`; `CustomerInfoDAL.SaveDCR_Applog_DAL` (`sp_webapi_SaveDCRAppLog`); inline SQL in code-behind (bypasses DAL, opens its own `SqlConnection` — connection string held in Web.config, `<redacted>`).

## 16. Stored Procedure Mapping
`sp_Get_DCRInfoListForView` (absent), `sp_webapi_SaveDCRAppLog` (absent), `sp_GET_ActionStatusList`, `sp_CS_GetEmpGeneralInfo_All`, `sp_Get_UserRoleInfo`, `sp_GET_SMCTypeListParm`, `sp_Get_EmployeeFieldForceInfo_EmpId` (all absent).

## 17. Database Mapping
Read: `tbl_DCRInfo` (columns seen: `DcrId, GroupId, RegionId, AreaId, TerritoryId, EntryDate, ApprovalStatus, SmcTypeId_Dcr, DoctorId, ChemberId`), `tblDCRApprovalLog` (`DCRApprovalId, TableId, ToEmpId, Step, GroupId, RegionId, AreaId, TerritoryId`), view `View_Webapi_EmployeeFieldForceInfo`. Written: `tblDCRApprovalLog` (and presumably `tbl_DCRInfo.ApprovalStatus`) through the SP (UNKNOWN).

## 18. Validation Matrix
No input validation (dates are free text concatenated into SQL; `Convert.ToInt32` on DataKeys). Action buttons are conditional only through markup visibility; the RowCommand does **not** re-check the current status server-side, so a crafted postback can approve an already-approved DCR (extra log step).

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M16-026 | Default view = DCRs entered today; filters: date range on `DCR.EntryDate`, approval status, user role, employee, pharma platform (`SmcTypeId_Dcr`) | `Solution.Web/DoctorVisit_UI/DCRList.aspx.cs:45-46,174-217` | code-behind | CONFIRMED |
| BR-M16-027 | Visibility is scoped by session `RoleTypeName`: MIO→own MIOEmpId, AM→ASM, DZSM→RSM, NSM→NSM; other roles see all | `DCRList.aspx.cs:240-279` | `param()` | CONFIRMED |
| BR-M16-028 | Approve button is available only on rows currently "Rejected"; Reject only on rows currently "Approved" (toggle; no button for other states) | `DCRList.aspx:284-289` | markup | CONFIRMED |
| BR-M16-029 | Approve/Reject appends a `tblDCRApprovalLog` entry (`Type='DCR'`, Status `Accepted`/`Rejected`, `Step = last Step + 1`, To = previous `ToEmpId`, From = current employee, `MenuId=382`, entry/approve dates = now, by = session user) through `sp_webapi_SaveDCRAppLog` | `DCRList.aspx.cs:78-131`; `CustomerInfoDAL.cs:1341-1400` | RowCommand | CONFIRMED |
| BR-M16-030 | If no DCR row is found for the key the action silently does nothing (no message) | `DCRList.aspx.cs:86` | `if (dtApp.Rows.Count > 0)` | CONFIRMED |
| BR-M16-031 | Zone/Area/Territory filters are inert because the hierarchy user control is hidden (`Visible=false`), so `F_*Select.SelectedValue` is always empty | `DCRList.aspx.cs:38,219-238` | code | INFERRED |

## 20. Permission Matrix
No `GetPermissionForUserRole` call; no check that the acting user is the approver (`ToEmpId`) or higher in the hierarchy; any user who can open the page and see a row can approve/reject it. Row visibility scoped by role type (BR-M16-027). Session keys used: `RoleTypeName, EmpInfoId, RoleTypeId, UserId` (all via try-less `.ToString()`; failure of `Page_Load` is swallowed but postback handlers are not).

## 21. Status Lifecycle
DCR (`tbl_DCRInfo.ApprovalStatus`, text shown via `sp_Get_DCRInfoListForView`; filter values from `sp_GET_ActionStatusList.SoftwareUseId`; the four-value convention seen on doctor master is 0 Pending / 1 Verified / 2 Approved / 3 Rejected — for DCR the mapping is `UNKNOWN`).
Transitions coded on this page: `Approved →(Reject click) Rejected` and `Rejected →(Approve click) Accepted/Approved`. Initial state and the first approval (Pending → Approved) are created outside these web pages (mobile app / webapi SPs; `EntryByApp`, `sp_webapi_*`) — INFERRED. `Step` in the log increments per action; `ToEmpId` is copied, so no routing to the next approver is performed by this page.
Second flag: `DoctorDailyVisit` (PAGE-660) filters on `DCR.IsApproved` ('0'/'1'), a different column (see BR-M16-032).

## 22. Error Handling
`Page_Load` swallows every exception; `RowCommand` and `param()` do not (unhandled exception page). DAL: rollback and rethrow.

## 23. Audit
`tblDCRApprovalLog` rows (actor session UserId/EmpInfoId, timestamps, step). Old status is not recorded in the log row (only new `Status`).

## 24. Integration
Mobile app writes the DCR (INFERRED); this page reuses the "webapi" approval-log SP shared with mobile approvals. Google Maps link.

## 25. Reports
Detail: DCRReport (PAGE-659). Related lists: DoctorDailyVisit (PAGE-660), monthly pivots (PAGE-673/671).

## 26. Dependencies
DataTables + buttons (client), AjaxControlToolkit UpdatePanel, `Library.DAL.*`.

## 27. Test Cases
| # | Case | Expected | BR |
|---|---|---|---|
| T1 | Open as MIO | only DCRs of own MIOEmpId, today | 026,027 |
| T2 | Open as Admin | all DCRs, today | 027 |
| T3 | Row "Approved" | only Reject + View buttons | 028 |
| T4 | Click Reject | new log row Step+1, Status Rejected, row shows Rejected, Approve button now visible | 029 |
| T5 | Click Approve on Rejected row | Status `Accepted`; list shows the mapped text | 029 |
| T6 | Replay Approve postback on an already-approved DCR | server accepts, adds another log step | §18 |
| T7 | Filter by zone | no effect (hidden control) | 031 |
| T8 | Employee without field-force row opens page | unhandled `IndexOutOfRangeException` for MIO/AM/DZSM/NSM | 027 |
| T9 | Only "To Date" filled | no date filter | 026 |

## 28. Reverse Engineering References
Files in §6.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Approval routing | None existed | log step increments but `ToEmpId` not advanced | SP absent | UNKNOWN | BR-M16-029 |

## 30. Known Gaps
GAP-M16-024 (SQL string concatenation of filters, incl. dates), GAP-M16-025 (no server-side status/permission check on approve/reject), GAP-M16-026 (inert hierarchy filter), GAP-M16-027 (inline SQL with own connection in code-behind), GAP-M16-028 (unused duplicated WebMethod), GAP-M16-029 (`Rows[0]` unguarded), GAP-M16-030 (approval text vocabulary `Accepted` vs `Approved` unresolved).

## 31. Change Impact
`sp_Get_DCRInfoListForView` used only here (PAGE-658) by name; `sp_webapi_SaveDCRAppLog` shared with mobile API and any other page calling `SaveDCR_Applog_DAL`. `tbl_DCRInfo`/`tblDCRApprovalLog` shared with DoctorEntry purge guard (`sp_Update_DoctorMaster:807`), DCR reports (PAGE-659/660/665/671/673/674), monthly pivots.
