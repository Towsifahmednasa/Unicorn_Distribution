---
page_id: PAGE-116
module: M04
page: CampaignView
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "PARTIAL", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-116 - CampaignView (campaign list, gateway to the live editor, mobile "Sync Now")

> **Evidence level:** Tier A. Read in full: `CampaignView.aspx.cs` (204 lines), key regions of `CampaignView.aspx`, `BonusCampaignNewDAL.GetBonusCampaignList/SYncBonusCampaign/GetMobileSyncStatus`. SP bodies absent (UNKNOWN).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-116 |
| Module | M04 |
| Page name | CampaignView |
| Kind | List / Query + Action (Sync) |
| Web folder / master | MasterSetup_UI / NewMasterPage.master, `Title="Campaign List"` |
| Page class | MasterSetup_UI_CampaignView |
| Files | aspx 12,710 bytes (2025-07-15), aspx.cs 170 lines (2025-11-13; newest file of the campaign family) |
| Suspected legacy/duplicate | NO - live list; every campaign editor variant (and `DASetup.aspx.cs:324`, `TargetExcelUploadList.aspx.cs:124`) redirects back here |

## 2. Purpose
Shows the campaigns currently in force (or filtered by type/status/validity), lets the user open one for editing (always into `CampaignSetup.aspx`) or start a new one, and offers a **"Sync Now"** action that runs `sp_CampaignUpdateFromPage`. CONFIRMED.

## 3. Business Objective
Give promotion admins one place to see and maintain campaigns and to push campaign changes to the mobile order-capture side ("Mobile Sync Required - Your mobile data is outdated. Please click the button below to sync." modal text, aspx:15-27). The mobile-sync meaning is INFERRED from the modal wording and the SP name; the SP body is not in the repo.

## 4. Navigation
Inbound: success redirects of all campaign editors (`successalert(...,'CampaignView.aspx')`), `DASetup`, `TargetExcelUploadList`, Reset button. Outbound: `CampaignSetup.aspx` (New Entry `:117`) and `CampaignSetup.aspx?MID=<id>` (Edit `:195`). Menu path UNKNOWN.

## 5. Route
`~/MasterSetup_UI/CampaignView.aspx`.

## 6. Source Files
View, code-behind; DAL `BonusCampaignNewDAL`, `SeedDataDAL`; DAO `ResultInfo`.

## 7. UI Control Inventory
| Control | Label | Type | Notes |
|---|---|---|---|
| ddlCampaignType | Campaign Type | DropDownList | `SeedDataDAL.GetCampaignTypeList` (`sp_GET_CampaignType`), placeholder first |
| FromDate / ToDate | From Date / To Date | TextBox (pickadate) | free text; used in SQL fragment |
| ddlActive | Active Status | DropDownList | "" Select One, 1 Active, 0 Inactive |
| btnSearch / resetBtn | Search / Reset | LinkButton | `btnSearch_Click` -> `LoadData()`; Reset redirects to itself |
| EmpCetegoryAddImageButton | New Entry | LinkButton | hidden when `Session["EmpInfoId"]=="496"` |
| loadGridView | SL, Campaign Name, Campaign Type (TypeName), Customer Type, Valid From Date, Valid To Date, Edit | GridView | DataKey = campaign master id; `EditData` command |
| pnl_4 / NoofGSPPopupExtender / btnSyncNow | "Mobile Sync Required" modal, "Sync Now" | Panel + button | modal shown only by `CheckSyncStatus()` whose call is commented out (`:29`), so the modal is normally never shown |

## 8. Field Mapping
Filters -> SQL fragment `@Parm` (see BR-M04-018). Grid columns bound from `sp_Get_BonusCampaignNewMasterList` result columns `CampaignName`, `TypeName`, `CustomerType`, `FromDate`, `Todate`, id key (column names UNKNOWN beyond these).

## 9. Dropdown Mapping
Campaign Type from `sp_GET_CampaignType` (CampainTypeId/TypeName); Active static.

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| Search | `btnSearch_Click` `:184` | rebuild `@Parm`, reload grid |
| Reset | `resetBtn_Click` `:200` | `Response.Redirect("CampaignView.aspx")` |
| New Entry | `:115` | redirect to `CampaignSetup.aspx` |
| Edit (grid) | `loadGridView_RowCommand` `:188` | `CampaignSetup.aspx?MID=<key>` |
| Sync Now | `btnSyncNow_Click` `:84` | `SYncBonusCampaign(UserId)` -> success alert "Operation successful!" / "Something went wrong!"; hides modal |

## 11. Frontend Flow
On load: type list bound, `LoadData()`. Pure server postbacks; the modal is an AjaxControlToolkit `ModalPopupExtender` (`NoofGSPPopupExtender`) shown from server code only.

## 12. Backend Flow
`Page_Load`: `EmpInfoId = Session["EmpInfoId"]` inside try/catch; first load: hide New Entry for empId "496", bind types, `LoadData()`. `LoadData`: `GetBonusCampaignList(parm())`, then removes duplicate rows keyed `CampaignName + "|" + TypeName` scanning from the bottom (keeps the last row of each key in table order). `SYncBonusCampaign` calls `sp_CampaignUpdateFromPage` with `@Ids = ""` via `UpdateData` (one transaction).

## 13. API Mapping
None (no WebMethod).

## 14. Service Mapping
No BLL. Static DAL instance.

## 15. Repository Mapping
`BonusCampaignNewDAL.GetBonusCampaignList`, `SYncBonusCampaign`, `GetMobileSyncStatus` (`usp_CheckCampaignEntryDate`, unused); `SeedDataDAL.GetCampaignTypeList`.

## 16. Stored Procedure Mapping
| SP | Params | Called from | Body |
|---|---|---|---|
| sp_Get_BonusCampaignNewMasterList | @Parm (SQL fragment string) | `GetBonusCampaignList` | UNKNOWN (must concatenate `@Parm` into dynamic SQL; alias `A` = master table) |
| sp_CampaignUpdateFromPage | @Ids = "" | `SYncBonusCampaign` | UNKNOWN |
| usp_CheckCampaignEntryDate | none | `GetMobileSyncStatus` (call commented out) | UNKNOWN; expected to return column `Result` = "yes" to show the modal |
| sp_GET_CampaignType | - | seed | UNKNOWN |

## 17. Database Mapping
Master table aliased `A` with columns `CampainTypeId`, `IsActive`, `FromDate`, `Todate` (from the fragment text). Others UNKNOWN.

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| FromDate/ToDate | none - raw text appended to SQL | `parm()` `:153-182` | - | BR-M04-018 |
| Session["EmpInfoId"] | missing => whole load skipped silently (`catch { }`), grid stays empty | `:22-54` | - | - |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M04-018 | Filter string: `AND A.CampainTypeId='<type>'` if a type is chosen; `AND A.IsActive='<0/1>'` if status chosen; **only when no status is chosen** add `AND CONVERT(date,getdate()) BETWEEN CONVERT(date,A.FromDate) AND CONVERT(date,A.Todate)` (i.e. default list = campaigns valid today, active or not); if both dates given `AND CONVERT(date,A.FromDate) BETWEEN '<from>' AND '<to>'`; if only From given the upper bound is `DateTime.Now`. Choosing Active/Inactive therefore lists campaigns of any date (expired too). Values are string-concatenated into the SP parameter (SQL-injection surface, GAP-M04-010). | CampaignView.aspx.cs:153-182 | `parm()` | CONFIRMED |
| BR-M04-019 | Duplicate suppression: rows with the same `CampaignName` + `TypeName` are collapsed to one grid row (needed because saving several customer types creates several masters, BR-M04-008). The loop runs from the last row upwards, so the kept row (and hence the Edit key and shown Customer Type) is the **last in result order** and editing it re-saves only its own customer type set (`CustomerTypeIdNew` loader in PAGE-111). | CampaignView.aspx.cs:124-146 | `LoadData` | CONFIRMED |
| BR-M04-020 | "New Entry" is hidden for the user whose `Session["EmpInfoId"]` equals the literal `"496"` (hard-coded employee id; view-only user). Direct URL `CampaignSetup.aspx` is not blocked for that user. | CampaignView.aspx.cs:25,31-34 | `Page_Load` | CONFIRMED |
| BR-M04-021 | "Sync Now" runs `sp_CampaignUpdateFromPage @Ids=''` (empty id list = presumably all campaigns) in one transaction; any user who can open the page can trigger it; the automatic "Mobile Sync Required" prompt is disabled (`CheckSyncStatus()` commented out at `:29`, `usp_CheckCampaignEntryDate` unused). SP effect UNKNOWN. | CampaignView.aspx.cs:29,59-102; BonusCampaignNewDAL.cs:148-178,1034-1051 | `btnSyncNow_Click` | CONFIRMED (call) / UNKNOWN (effect) |

## 20. Permission Matrix
Session keys: `EmpInfoId` (hard-coded "496" rule), `UserId` (Sync). No role check; menu-based. Roles UNKNOWN.

## 21. Status Lifecycle
Read-only view of `IsActive` and validity dates (no status change here). See PAGE-111 section 21.

## 22. Error Handling
Whole `Page_Load` in try/catch with empty handler: a missing `EmpInfoId` session value produces an empty page with no message. Sync failure => `faildalert("Something went wrong!")`; exceptions inside `SYncBonusCampaign` are rethrown (unhandled in the click handler, i.e. error page).

## 23. Audit
None on this page (Sync passes `sessionUser` but `SYncBonusCampaign` does not send it to the SP).

## 24. Integration
Mobile sync SP (INFERRED). No other.

## 25. Reports
None (screen list only).

## 26. Dependencies
DAL `BonusCampaignNewDAL`, `SeedDataDAL`; DAO `ResultInfo`; AjaxControlToolkit.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Open with no filters | Campaigns valid today shown once per name+type | BR-M04-018/019 |
| TC-02 | Status = Inactive | Inactive campaigns of any date | BR-M04-018 |
| TC-03 | Only From date | Range From..now | BR-M04-018 |
| TC-04 | Type filter value with `'` | Query error / injection risk | GAP-M04-010 |
| TC-05 | Login as EmpInfoId 496 | No New Entry button | BR-M04-020 |
| TC-06 | Click Sync Now | `sp_CampaignUpdateFromPage` runs; success alert | BR-M04-021 |
| TC-07 | Edit row of multi-type campaign | Opens one master only | BR-M04-019 |

## 28. Reverse Engineering References
`Solution.Web/MasterSetup_UI/CampaignView.aspx(.cs)`, `Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs:20-38,148-178,1034-1051`.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | read | SP bodies absent | MISSING | - |
| Auto sync prompt | None existed | commented out | n/a | DISABLED | BR-M04-021 |

## 30. Known Gaps
SQL fragment injection surface (GAP-M04-010). Hard-coded employee id 496. Sync semantic unknown (GAP-M04-012). Silent failure when session missing. Unused `usp_CheckCampaignEntryDate`.

## 31. Change Impact
All campaign editors redirect here; changing column names returned by `sp_Get_BonusCampaignNewMasterList` breaks the grid. `sp_CampaignUpdateFromPage` is shared only with this page.
