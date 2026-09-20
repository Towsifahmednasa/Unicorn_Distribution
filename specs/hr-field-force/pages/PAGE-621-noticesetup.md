---
page_id: PAGE-621
module: M15
page: NoticeSetup
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-621 — NoticeSetup

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, reachable DAL/DAO code and page-local JS. Stored-procedure bodies are NOT in the repository (none of this module's SPs are in the root `*.sql` scripts), so SP-internal behaviour is marked `UNKNOWN`. Sections 1, 4-6, 13-15 and 28 keep the generated (static-analysis) content, which was spot-checked; sections carrying `Verified` notes were checked line by line.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-621 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | NoticeSetup |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | NoticeBoard_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | NoticeBoard_UI_NoticeSetup |
| aspx size | 51364 bytes |
| Code-behind LOC | 509 |
| Inline script size | 30187 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
CONFIRMED: create/edit a **notice-board announcement**: title, announcement text, display period (From/To date), optional image, and its **audience** - a list of market-structure rows (group / region / area / territory / sub-territory / market) and/or user roles. Records list: `NoticeRecords.aspx` (PAGE-620); the notice board and mobile app show notices (`NoticeBoard.aspx`, `NoticeDetailsApp.aspx`).

## 3. Business Objective
CONFIRMED by fields: push company announcements to the right field staff by geography and/or role for a limited period.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `NoticeBoard_UI/NoticeRecords.aspx`

Outbound references: `noticerecords.aspx`

## 5. Route
`~/NoticeBoard_UI/NoticeSetup.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/NoticeBoard_UI/NoticeSetup.aspx |
| Code-behind | Solution.Web/NoticeBoard_UI/NoticeSetup.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/MasterSetup_UI/IVMarketStructureForTeritory.ascx |
| JS include | //cdn.ckeditor.com/4.14.1/standard/ckeditor.js |
| DAL class | AppPrimaryDAL (Library.DAL/DoctorModule_DAL/AppPrimaryDAL.cs) |
| DAL class | NoticeDal (Library.DAL/DoctorVisit_DAL/NoticeDal.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |
| DAL class | Setup2DAL (Library.DAL/DoctorModule_DAL/Setup2DAL.cs) |
| DAO/entity class | BonusCampaignMarketDetailDAO (Library.DAO/MasterSetup_DAO/BonusCampaignMarketDetailDAO.cs) |
| DAO/entity class | ImagePath (Library.DAO/DoctorModule_DAO/ImagePath.cs) |
| DAO/entity class | NoticeMaster (Library.DAO/DoctorModule_DAO/NoticeMaster.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| id_mastetID | HiddenField |  | no validator found |  |  |  |  |
| txtTitle | TextBox |  | no validator found |  |  |  |  |
| Announcement | TextBox |  | no validator found |  |  |  |  |
| frmDate | TextBox | Select Date | no validator found |  |  |  |  |
| toDate | TextBox | Select Date | no validator found |  |  |  |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| btnAddtoListMarket | LinkButton |  | no validator found |  |  |  | OnClick→btnAddtoListMarket_Click |
| gv_Market | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | SL# | no validator found |  |  |  |  |
| hfGroupId | HiddenField |  | no validator found |  |  |  |  |
| hfRegionId | HiddenField |  | no validator found |  |  |  |  |
| hfAreaId | HiddenField |  | no validator found |  |  |  |  |
| hfTerritoryId | HiddenField |  | no validator found |  |  |  |  |
| hfSubTerritoryId | HiddenField |  | no validator found |  |  |  |  |
| hfMarketId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Group | no validator found |  |  |  |  |
| lbl_GroupName | Label | <%#Eval("GroupName") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Zone | no validator found |  |  |  |  |
| lbl_RegionName | Label | <%#Eval("RegionName") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Area  | no validator found |  |  |  |  |
| lbl_AreaName | Label | <%#Eval("AreaName") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Territory | no validator found |  |  |  |  |
| lbl_TerritoryName | Label | <%#Eval("TerritoryName") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Sub-Territory | no validator found |  | Visible=false |  |  |
| lbl_SubTerritoryName | Label | <%#Eval("SubTerritoryName") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Market | no validator found |  | Visible=false |  |  |
| lbl_MarketName | Label | <%#Eval("MarketName") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Remove | no validator found |  |  |  |  |
| MarketdeleteImageButton | LinkButton |  | no validator found |  |  |  | OnClick→MarketdeleteImageButton_Click |
| UserRoleSelect | ListBox |  | no validator found |  |  |  |  |
| ShopImgUploadForm | input |  | no validator found |  |  |  | onchange→ImageToBase64ShopImg(this) |
| hfimgShow | HiddenField |  | no validator found |  |  |  |  |
| hfimgeBase64Str | HiddenField |  | no validator found |  |  |  |  |
| outputimage | Image |  | no validator found |  |  |  |  |
| btnSave | LinkButton |  | no validator found |  | Visible=false |  | OnClientClick→return sweetAlertConfirm_Submit(this);, OnClick→btnSave_Click |
| btnUpdate | LinkButton |  | no validator found |  | Visible=false |  | OnClientClick→return sweetAlertConfirm_Update(this);, OnClick→btnSave_Click |
| btnReset | LinkButton |  | no validator found |  |  |  | OnClick→btnReset_Click |
| gv_Seen | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | SL# | no validator found |  |  |  |  |
| (no id) | TemplateField | Seen By | no validator found |  |  |  |  |
| lbl_EmpName | Label | <%#Eval("EmpName") %> | no validator found |  |  |  |  |
| masterId | input | 0 | no validator found |  |  |  |  |
| (no id) | a |  | no validator found |  |  |  | onclick→DeleteFile(\"" + srandomid + "\",\"" + sfilename +                             "\") |
| HfGroupId | input | ' + dtGroupId + ' | no validator found |  |  |  |  |
| HfZoneId | input | ' + dtZoneId + ' | no validator found |  |  |  |  |
| HfAreaId | input | ' + dtAreaId + ' | no validator found |  |  |  |  |
| HfterritoryId | input | ' + dtterritoryId + ' | no validator found |  |  |  |  |
| HfmarketId | input | ' + dtmarketId + ' | no validator found |  |  |  |  |
| HfGroupId | input | ' + dtGroupId + ' | no validator found |  |  |  |  |
| HfZoneId | input | ' + dtZoneId + ' | no validator found |  |  |  |  |
| HfAreaId | input | ' + dtAreaId + ' | no validator found |  |  |  |  |
| HfterritoryId | input | ' + dtterritoryId + ' | no validator found |  |  |  |  |
| HfmarketId | input | ' + dtmarketId + ' | no validator found |  |  |  |  |
| HfProductId | input | ' + result[i].ProductId + ' | no validator found |  |  |  |  |
| (no id) | a |  | no validator found |  |  |  | onclick→DeleteFile(\"" + result[i].NoticeId + "\",\"" + result[i].ImageName +                             "\") |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| hfimgShow | ImageUrl | NoticeBoard_UI_NoticeSetup.Page_Load | — | SeedDataDAL.GetUserRoleList, Setup2DAL.GetNoticeDetailMarketById, Setup2DAL.GetNoticeSeenbyById, Setup2DAL.GetNoticeSetu | UNKNOWN | UNKNOWN | UNKNOWN |
| hfGroupId | BonusCampaignMarketDetailDAO.GroupId | NoticeBoard_UI_NoticeSetup.btnSave_Click | (none — page calls DAL directly) | NoticeDal.SaveNotice | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfRegionId | BonusCampaignMarketDetailDAO.RegionId | NoticeBoard_UI_NoticeSetup.btnSave_Click | (none — page calls DAL directly) | NoticeDal.SaveNotice | @RegionId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfAreaId | BonusCampaignMarketDetailDAO.AreaId | NoticeBoard_UI_NoticeSetup.btnSave_Click | (none — page calls DAL directly) | NoticeDal.SaveNotice | @AreaId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfTerritoryId | BonusCampaignMarketDetailDAO.TerritoryId | NoticeBoard_UI_NoticeSetup.btnSave_Click | (none — page calls DAL directly) | NoticeDal.SaveNotice | @TerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfSubTerritoryId | BonusCampaignMarketDetailDAO.SubTerritoryId | NoticeBoard_UI_NoticeSetup.btnSave_Click | (none — page calls DAL directly) | NoticeDal.SaveNotice | @SubTerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfMarketId | BonusCampaignMarketDetailDAO.MarketId | NoticeBoard_UI_NoticeSetup.btnSave_Click | (none — page calls DAL directly) | NoticeDal.SaveNotice | @MarketId | UNKNOWN (SP body not in repo) | UNKNOWN |
| id_mastetID | NoticeMaster.NoticeId | NoticeBoard_UI_NoticeSetup.btnSave_Click | (none — page calls DAL directly) | NoticeDal.SaveNotice | @NoticeId | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtTitle | NoticeMaster.NoticeTitle | NoticeBoard_UI_NoticeSetup.btnSave_Click | (none — page calls DAL directly) | NoticeDal.SaveNotice | @NoticeTitle | UNKNOWN (SP body not in repo) | UNKNOWN |
| Announcement | NoticeMaster.Announcement | NoticeBoard_UI_NoticeSetup.btnSave_Click | (none — page calls DAL directly) | NoticeDal.SaveNotice | @Announcement | UNKNOWN (SP body not in repo) | UNKNOWN |
| frmDate | NoticeMaster.FromDate | NoticeBoard_UI_NoticeSetup.btnSave_Click | (none — page calls DAL directly) | NoticeDal.SaveNotice | @FromDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| toDate | NoticeMaster.ToDate | NoticeBoard_UI_NoticeSetup.btnSave_Click | (none — page calls DAL directly) | NoticeDal.SaveNotice | @ToDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfimgeBase64Str | NoticeMaster.ImageString | NoticeBoard_UI_NoticeSetup.btnSave_Click | — | AppPrimaryDAL.GetImagePath, NoticeDal.SaveNotice | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| UserRoleSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnAddtoListMarket | LinkButton click | (postback) | NoticeBoard_UI_NoticeSetup.btnAddtoListMarket_Click | Solution.Web/NoticeBoard_UI/NoticeSetup.aspx.cs:175 | — | — | — | read-only / none detected |
| MarketdeleteImageButton | LinkButton click | (postback) | NoticeBoard_UI_NoticeSetup.MarketdeleteImageButton_Click | Solution.Web/NoticeBoard_UI/NoticeSetup.aspx.cs:193 | — | — | — | read-only / none detected |
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | NoticeBoard_UI_NoticeSetup.btnSave_Click | Solution.Web/NoticeBoard_UI/NoticeSetup.aspx.cs:418 | — | AppPrimaryDAL.GetImagePath, NoticeDal.SaveNotice | sp_Save_NoticeMaster, sp_Save_NoticeUserRoleDetail, sp_Save_Notice_MarketDetail, sp_Update_NoticeMaster, sp_Webapi_Get_I | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnUpdate | LinkButton click | return sweetAlertConfirm_Update(this); | NoticeBoard_UI_NoticeSetup.btnSave_Click | Solution.Web/NoticeBoard_UI/NoticeSetup.aspx.cs:418 | — | AppPrimaryDAL.GetImagePath, NoticeDal.SaveNotice | sp_Save_NoticeMaster, sp_Save_NoticeUserRoleDetail, sp_Save_Notice_MarketDetail, sp_Update_NoticeMaster, sp_Webapi_Get_I | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnReset | LinkButton click | (postback) | NoticeBoard_UI_NoticeSetup.btnReset_Click | Solution.Web/NoticeBoard_UI/NoticeSetup.aspx.cs:506 | — | — | — | read-only / none detected |

## 11. Frontend Flow
`Page_Load` (`NoticeSetup.aspx.cs:20-66`) loads user roles (multi-select) and the market cascade controls; `?id=` -> `GetOneRecord` (`:68-`) reading the notice, image file (`File.ReadAllBytes` of the stored path -> base64 preview), audience CSV/rows. Market rows are added with `btnAddtoListMarket_Click`/`AddMarket()` into `gv_Market` and removed with `MarketdeleteImageButton_Click`; the image is chosen client-side and carried as base64 in hidden field `hfimgeBase64Str`. Save -> `btnSave_Click` (`:417`); success -> `NoticeRecords.aspx`, failure `Already Exist!`.

## 12. Backend Flow
`btnSave_Click` -> `Validation()` (`:365-410`) -> market rows from `gv_Market` hidden fields into `BonusCampaignMarketDetailDAO` list; role CSV from `UserRoleSelect` -> `NoticeMaster {NoticeId, NoticeTitle, Announcement, FromDate, ToDate (DateTime.Parse), ImageString}` -> `NoticeDal.SaveNotice` (`Library.DAL/DoctorVisit_DAL/NoticeDal.cs:22`): id>0 -> `sp_Update_NoticeMaster(@UpdateBy)`, else `sp_Save_NoticeMaster(@EntryBy)` (PK); pk>0 -> `isSuccess=true` (forced, even if the update SP returned false), `sp_Save_Notice_MarketDetail` per market row, `sp_Save_NoticeUserRoleDetail(@NoticeId,@UserRoleID)` per role, then image `<prefix><id>.jpg` written to `AppPrimaryDAL.GetImagePath("Notice")` (`Directory.CreateDirectory` + `File.WriteAllBytes`, unguarded).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SeedDataDAL.GetUserRoleList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:707 | sp_Get_UserRoleInfo |  |
| Setup2DAL.GetNoticeSetupById | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:5155 | sp_Get_Notice_ById | @id |
| Setup2DAL.GetNoticeDetailMarketById | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:5094 | sp_GET_NoticeDetailMarket_ById | @id |
| Setup2DAL.GetNoticeSeenbyById | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:5115 | sp_GET_NoticeSeen_ById | @id |
| NoticeDal.SaveNotice | Library.DAL/DoctorVisit_DAL/NoticeDal.cs:23 | sp_Save_NoticeMaster, sp_Save_NoticeUserRoleDetail, sp_Save_Notice_MarketDetail, | @Announcement, @AreaId, @EntryBy, @FromDate, @GroupId, @MarketId, @NoticeId, @NoticeTitle, @RegionId |
| AppPrimaryDAL.GetImagePath | Library.DAL/DoctorModule_DAL/AppPrimaryDAL.cs:58 | sp_Webapi_Get_ImagePath | @type |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GET_NoticeDetailMarket_ById | Setup2DAL.GetNoticeDetailMarketById | body NOT in repository | read-like name (INFERRED) |
| sp_GET_NoticeSeen_ById | Setup2DAL.GetNoticeSeenbyById | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Notice_ById | Setup2DAL.GetNoticeSetupById | body NOT in repository | read-like name (INFERRED) |
| sp_Get_UserRoleInfo | SeedDataDAL.GetUserRoleList | body NOT in repository | read-like name (INFERRED) |
| sp_Save_NoticeMaster | NoticeDal.SaveNotice | body NOT in repository | write-like name |
| sp_Save_NoticeUserRoleDetail | NoticeDal.SaveNotice | body NOT in repository | write-like name |
| sp_Save_Notice_MarketDetail | NoticeDal.SaveNotice | body NOT in repository | write-like name |
| sp_Update_NoticeMaster | NoticeDal.SaveNotice | body NOT in repository | write-like name |
| sp_Webapi_Get_ImagePath | AppPrimaryDAL.GetImagePath | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| txtTitle | required | `Validation()` `:365-` | tooltip | BR-M15-055 |
| frmDate / toDate | required | same | tooltip | BR-M15-055 |
| audience | `gv_Market` rows > 0 **or** a role selected | `:402` | `please Add to List Market Structure or User Role!!` | BR-M15-056 |
| To >= From, image type/size, announcement text | **not validated** | - | - | GAP-M15-019 |

## 19. Business Rules
| BR ID | Description | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M15-055 | A notice needs title, From date and To date; announcement text optional; no ordering check between dates. | `NoticeSetup.aspx.cs:365-395` | code-behind | CONFIRMED |
| BR-M15-056 | A notice must target at least one market-structure row or at least one user role (either, or both). | `:400-405` | code-behind | CONFIRMED |
| BR-M15-057 | An attached image is stored as a `.jpg` file named `<ImagePreName><NoticeId>.jpg` in the folder configured for image type `Notice` (not in the DB); update rewrites the same file. | `NoticeDal.cs:88-110` | DAL | CONFIRMED |

## 20. Permission Matrix
`Session["UserId"]` for audit only; no role/permission check; menu-only.

## 21. Status Lifecycle
No status column in code; visibility governed by From/To dates (consumer SPs UNKNOWN).

## 22. Error Handling
DAL rollback + rethrow; image write not caught (a bad path/base64 throws after DB rows are written and committed? - the DAL commits at connection close in `finally`, but an exception path triggers rollback; net effect UNKNOWN); generic `Already Exist!`.

## 23. Audit
`@EntryBy` / `@UpdateBy` = `Session["UserId"]`.

## 24. Integration
sp_Webapi_Get_ImagePath

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: AppPrimaryDAL, NoticeDal, SeedDataDAL, Setup2DAL; DAO: BonusCampaignMarketDetailDAO, ImagePath, NoticeMaster, ResultInfo; Tables (inline): —

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-621-01 | Title, dates, one role, Save | Notice created | BR-M15-055, 056 |
| TC-621-02 | No market row and no role | Alert `please Add to List Market Structure or User Role!!` | BR-M15-056 |
| TC-621-03 | With image | jpg file appears in configured folder | BR-M15-057 |
| TC-621-04 | To date before From (edge) | Saved (no check) | GAP-M15-019 |
| TC-621-05 | Edit notice and Save (edge) | Audience rows re-inserted; check duplicates | GAP-M15-019 |

## 28. Reverse Engineering References
- `Solution.Web/NoticeBoard_UI/NoticeSetup.aspx`, `Solution.Web/NoticeBoard_UI/NoticeSetup.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | NoticeSetup.aspx(.cs) | n/a | MISSING | - |
| Update result | None existed | forced success | UNKNOWN | ACTUAL-DEFECT | GAP-M15-019 |

## 30. Known Gaps
- **GAP-M15-019**.

## 31. Change Impact
`NoticeDal` shared with NoticeRecords/NoticeBoard/mobile notice APIs; image folder config (`AppPrimaryDAL.GetImagePath`) shared with expense/mileage/prescription images.

