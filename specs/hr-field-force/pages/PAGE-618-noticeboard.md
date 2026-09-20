---
page_id: PAGE-618
module: M15
page: NoticeBoard
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "PARTIAL", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-618 — NoticeBoard

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-618 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | NoticeBoard |
| Kind | Page (INFERRED from name) |
| Web folder | NoticeBoard_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | NoticeBoard_UI_NoticeBoard |
| aspx size | 368 bytes |
| Code-behind LOC | 119 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **NoticeBoard** (Page). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Employee master, attendance, leave, holiday, shifts, tour plan, expense/TADA/mileage claims, training, notice board, employee-related reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/NoticeBoard_UI/NoticeBoard.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/NoticeBoard_UI/NoticeBoard.aspx |
| Code-behind | Solution.Web/NoticeBoard_UI/NoticeBoard.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | NoticeDal (Library.DAL/DoctorVisit_DAL/NoticeDal.cs) |
| DAO/entity class | NoticeMaster (Library.DAO/DoctorModule_DAO/NoticeMaster.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
No server/HTML controls with an ID were extracted (viewer-only or code-generated page).

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: none
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: no
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| NoticeBoard_UI_NoticeBoard.Page_Load | Solution.Web/NoticeBoard_UI/NoticeBoard.aspx.cs:18 | object sender, EventArgs e | NoticeBoard_UI_NoticeBoard.Page_Load | no | no |
| NoticeBoard_UI_NoticeBoard.GetNoticeMasterList | Solution.Web/NoticeBoard_UI/NoticeBoard.aspx.cs:83 |  | NoticeBoard_UI_NoticeBoard.GetNoticeMasterList → NoticeDal.GetNoticeMasterList → SP: sp_Get_NoticeMaster | no | no |
| NoticeBoard_UI_NoticeBoard.Delete_NoticeMaster | Solution.Web/NoticeBoard_UI/NoticeBoard.aspx.cs:92 | int Id | NoticeBoard_UI_NoticeBoard.Delete_NoticeMaster → NoticeDal.Delete_NoticeMaster → SP: sp_Delete_NoticeMaster | no | yes |
| NoticeBoard_UI_NoticeBoard.GetNoticeMasterEditData | Solution.Web/NoticeBoard_UI/NoticeBoard.aspx.cs:98 | int id | NoticeBoard_UI_NoticeBoard.GetNoticeMasterEditData → NoticeDal.GetNoticeMasterForEdit → SP: sp_Get_NoticeMaster_ById | no | no |
| NoticeBoard_UI_NoticeBoard.GetNotticeDetailsEditForEdit | Solution.Web/NoticeBoard_UI/NoticeBoard.aspx.cs:112 | int id | NoticeBoard_UI_NoticeBoard.GetNotticeDetailsEditForEdit → NoticeDal.Get_NoticeDetailsByNoticeId → SP: sp_Get_Noticedetails_By_NoticeId | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| NoticeBoard_UI_NoticeBoard.GetNoticeMasterList | Solution.Web/NoticeBoard_UI/NoticeBoard.aspx.cs:83 |  | ~/NoticeBoard_UI/NoticeBoard.aspx/GetNoticeMasterList | via BLL |
| NoticeBoard_UI_NoticeBoard.Delete_NoticeMaster | Solution.Web/NoticeBoard_UI/NoticeBoard.aspx.cs:92 | int Id | ~/NoticeBoard_UI/NoticeBoard.aspx/Delete_NoticeMaster | via BLL |
| NoticeBoard_UI_NoticeBoard.GetNoticeMasterEditData | Solution.Web/NoticeBoard_UI/NoticeBoard.aspx.cs:98 | int id | ~/NoticeBoard_UI/NoticeBoard.aspx/GetNoticeMasterEditData | via BLL |
| NoticeBoard_UI_NoticeBoard.GetNotticeDetailsEditForEdit | Solution.Web/NoticeBoard_UI/NoticeBoard.aspx.cs:112 | int id | ~/NoticeBoard_UI/NoticeBoard.aspx/GetNotticeDetailsEditForEdit | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| NoticeDal.GetNoticeMasterList | Library.DAL/DoctorVisit_DAL/NoticeDal.cs:267 | sp_Get_NoticeMaster |  |
| NoticeDal.Delete_NoticeMaster | Library.DAL/DoctorVisit_DAL/NoticeDal.cs:310 | sp_Delete_NoticeMaster | @NoticeId |
| NoticeDal.GetNoticeMasterForEdit | Library.DAL/DoctorVisit_DAL/NoticeDal.cs:343 | sp_Get_NoticeMaster_ById | @id |
| NoticeDal.Get_NoticeDetailsByNoticeId | Library.DAL/DoctorVisit_DAL/NoticeDal.cs:408 | sp_Get_Noticedetails_By_NoticeId | @id |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Delete_NoticeMaster | NoticeDal.Delete_NoticeMaster | body NOT in repository | write-like name |
| sp_Get_NoticeMaster | NoticeDal.GetNoticeMasterList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_NoticeMaster_ById | NoticeDal.GetNoticeMasterForEdit | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Noticedetails_By_NoticeId | NoticeDal.Get_NoticeDetailsByNoticeId | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 5 methods.
- Redirect targets: none
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: NoticeDal; DAO: NoticeMaster, ResultInfo; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/NoticeBoard_UI/NoticeBoard.aspx`, `Solution.Web/NoticeBoard_UI/NoticeBoard.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/NoticeBoard_UI/NoticeBoard.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 2 controls parsed | n/a | PARTIAL | Solution.Web/NoticeBoard_UI/NoticeBoard.aspx |
| Data access | This document §15–17 | 4 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 4 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M15 (HR, Field-Force & Employee Administration).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods NoticeDal.Delete_NoticeMaster, NoticeDal.GetNoticeMasterForEdit, NoticeDal.GetNoticeMasterList, NoticeDal.Get_NoticeDetailsByNoticeId affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
