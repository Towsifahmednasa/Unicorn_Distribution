---
page_id: PAGE-611
module: M15
page: YearlyLeaveProcess
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "MISSING", "rules": "PARTIAL", "permission": "COMPLETE", "status": "MISSING", "tests": "PARTIAL"}
---
# PAGE-611 — YearlyLeaveProcess

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, reachable DAL/DAO code and page-local JS. Stored-procedure bodies are NOT in the repository (none of this module's SPs are in the root `*.sql` scripts), so SP-internal behaviour is marked `UNKNOWN`. Sections 1, 4-6, 13-15 and 28 keep the generated (static-analysis) content, which was spot-checked; sections carrying `Verified` notes were checked line by line.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-611 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | YearlyLeaveProcess |
| Kind | Page (INFERRED from name) |
| Web folder | LeaveProcess_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | LeaveProcess_UI_YearlyLeaveProcess |
| aspx size | 6426 bytes |
| Code-behind LOC | 26 |
| Inline script size | 4047 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
CONFIRMED (page text): one-button **yearly leave balance process** - "This process will set employee yearly leave balance based on leave type and yearly leave allocation." Button `Process Leave`.

## 3. Business Objective
CONFIRMED by text: at year start, HR generates every employee's leave balances from the leave types (PAGE-577) and allocations/rules (PAGE-578). How is UNKNOWN.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/LeaveProcess_UI/YearlyLeaveProcess.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/LeaveProcess_UI/YearlyLeaveProcess.aspx |
| Code-behind | Solution.Web/LeaveProcess_UI/YearlyLeaveProcess.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | YearlyLeaveprocessDal (Library.DAL/MasterSetup_DAL/YearlyLeaveprocessDal.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| btnSave | button |  | no validator found |  |  |  | onclick→Save() |
| masterId | input | 0 | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
`Save()` -> `FinalSave()` (`YearlyLeaveProcess.aspx`): builds unused `jsonData["year"]='2022'`; shows spinner `#coverScreen`; `$.ajax POST 'YearlyLeaveProcess.aspx/SaveYearlyLeave'` with body `{}`; `result.d.isSuccess` true -> `successalert('Operation successful!','Success','YearlyLeaveProcess.aspx')` else `faildalert('Operation Faild!')`; `error` -> same failure alert. `Validation()` JS is a stub returning true. No confirmation dialog. Leftover functions (`GetData`, `IsActiveChange`) reference MVC `@Url.Action` and are dead.

## 12. Backend Flow
`SaveYearlyleave()` (`YearlyLeaveProcess.aspx.cs`, `[WebMethod] static`) -> `YearlyLeaveprocessDal.SaveYearlyleave(Convert.ToInt32(Session["UserId"]))` -> `accessManager.SaveData("sp_Process_YearlyLeaveBalance", [@ProcessBy])` -> `ResultInfo{isSuccess}`; catch: `SqlConnectionClose(true)` (rollback) and rethrow; finally close/commit. Static DAL instance.

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| LeaveProcess_UI_YearlyLeaveProcess.SaveYearlyleave | Solution.Web/LeaveProcess_UI/YearlyLeaveProcess.aspx.cs:23 |  | ~/LeaveProcess_UI/YearlyLeaveProcess.aspx/SaveYearlyleave | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| YearlyLeaveprocessDal.SaveYearlyleave | Library.DAL/MasterSetup_DAL/YearlyLeaveprocessDal.cs:17 | sp_Process_YearlyLeaveBalance | @ProcessBy |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Process_YearlyLeaveBalance | YearlyLeaveprocessDal.SaveYearlyleave | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| (none) | No input; no year; no precondition; no duplicate-run check in C# | - | - | BR-M15-063, 064 |
| Session | `Session["UserId"]` must exist | web-method | NullReference -> HTTP 500 -> `Operation Faild!` | BR-M15-063 |

## 19. Business Rules
| BR ID | Description | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M15-063 | The process is global and parameterless: a single call `sp_Process_YearlyLeaveBalance(@ProcessBy)` for all employees; the year is not chosen by the user (the JS 2022 value is unused). Balance formula/allocation/carry-forward: UNKNOWN. | `YearlyLeaveprocessDal.cs`; `YearlyLeaveProcess.aspx` | DAL + SP | CONFIRMED (call), UNKNOWN (logic) |
| BR-M15-064 | No guard against repeated execution or unauthorised callers in the web tier: any session can trigger it; button has no confirmation. | `YearlyLeaveProcess.aspx.cs`; markup | web tier | CONFIRMED |

## 20. Permission Matrix
Session: `UserId` only. No role/permission check; menu-only. Any authenticated user can POST the page-method.

## 21. Status Lifecycle
None in web code (UNKNOWN inside SP; possibly writes leave-balance rows for a fiscal year).

## 22. Error Handling
Rollback+rethrow -> client shows `Operation Faild!`; success determined only by `SaveData` returning true (which returns true whenever the SP executes).

## 23. Audit
`@ProcessBy` = `Session["UserId"]`; further audit UNKNOWN.

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: YearlyLeaveprocessDal; DAO: ResultInfo; Tables (inline): —

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-611-01 | Click Process Leave once | Balances created for all employees per SP (verify in DB/report RPT-M15-01) | BR-M15-063 |
| TC-611-02 | Click twice | Must not double balances - UNKNOWN | GAP-M15-016 |
| TC-611-03 | POST page-method as non-HR user | Executes (no check) | BR-M15-064 |
| TC-611-04 | Expired session | HTTP 500 / failure alert | BR-M15-063 |

## 28. Reverse Engineering References
- `Solution.Web/LeaveProcess_UI/YearlyLeaveProcess.aspx`, `Solution.Web/LeaveProcess_UI/YearlyLeaveProcess.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | YearlyLeaveProcess.aspx(.cs) | n/a | MISSING | - |
| Year selection | None existed | none | UNKNOWN | ACTUAL-GAP | GAP-M15-016 |
| Processing logic | None existed | in SP | absent | UNKNOWN | GAP-M15-001 |

## 30. Known Gaps
- **GAP-M15-016**, **GAP-M15-009**, GAP-M15-015 (method-name case), GAP-M15-001.

## 31. Change Impact
`sp_Process_YearlyLeaveBalance` determines all leave balances used by PAGE-608 (balance lookup), PAGE-622 (report) and mobile leave; changing leave types/config (PAGE-577/578) only takes effect on the next run.

