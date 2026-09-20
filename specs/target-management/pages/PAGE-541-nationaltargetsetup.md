---
page_id: PAGE-541
module: M14
page: NationalTargetSetup
tier: A
generated: false   # upgraded by manual code review (Tier A)
reviewed: 2026-09-19
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-541 — NationalTargetSetup

> **Evidence level:** Tier A - reviewed manually against `.aspx`, `.aspx.cs`, and the BLL/DAL code they reach (review date 2026-09-19). Claims are tagged CONFIRMED / INFERRED / UNKNOWN. Stored-procedure bodies are NOT in the repository unless a section says so; column-level effects of such SPs stay `UNKNOWN`. Sections 1, 4-10, 13-15 and 28 still carry the mechanically extracted content (re-checked where noted); all other sections were rewritten.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-541 |
| Module | M14 — Target & DWSP Management (INFERRED from folder + file name) |
| Page name | NationalTargetSetup |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | DWSP |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DWSP_NationalTargetSetup |
| aspx size | 11937 bytes |
| Code-behind LOC | 213 |
| Inline script size | 1375 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
Lets a head-office user record the **national (product-group level) sales target amount for one month** - one amount per Group (business group/division from `sp_NTS_Group_Active`), month and year. This amount is the top of the target cascade: the Zone page later reads it (`sp_Get_NationalTargetAmount`) and splits it across zones. CONFIRMED (fields and handlers: `Solution.Web/DWSP/NationalTargetSetup.aspx.cs:101-205`).

## 3. Business Objective
Set the monthly national sales target per group so that lower levels (zone -> area -> territory -> day) can be planned against it (see BR-M14-001). Business rationale INFERRED from the downstream pages that read this figure (`Solution.Web/DWSP/ZoneWiseTargetSetup.aspx.cs:326-338`).

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DWSP/NationalTargetSetupView.aspx`

Outbound references: `nationaltargetsetupview.aspx`

## 5. Route
`~/DWSP/NationalTargetSetup.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DWSP/NationalTargetSetup.aspx |
| Code-behind | Solution.Web/DWSP/NationalTargetSetup.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | NationalTargetSetupDal (Library.DAL/DWSP_DAL/NationalTargetSetupDal.cs) |
| DAO/entity class | NationalTargetDao (Library.DAO/DWSP_DAO/NationalTargetDao.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| id_mastetID | HiddenField |  | no validator found |  |  |  |  |
| ddlYear | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlmonth | DropDownList |  | no validator found |  |  | see §9 |  |
| groupname | DropDownList |  | no validator found |  |  | see §9 |  |
| txtAmount | TextBox |  | no validator found |  |  |  |  |
| FilteredTextBoxExtender18 | ajaxToolkit:FilteredTextBoxExtender |  | no validator found |  |  |  |  |
| btnSave | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Submit(this); |
| btnUpdate | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Update(this); |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| id_mastetID | NationalTargetDao.NatargetSpId | DWSP_NationalTargetSetup.DataPrepare | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlYear | NationalTargetDao.Year | DWSP_NationalTargetSetup.DataPrepare | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlmonth | NationalTargetDao.Month | DWSP_NationalTargetSetup.DataPrepare | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| groupname | NationalTargetDao.GroupId | DWSP_NationalTargetSetup.DataPrepare | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| txtAmount | NationalTargetDao.Amount | DWSP_NationalTargetSetup.DataPrepare | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlYear | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlmonth | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| groupname | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | DWSP_NationalTargetSetup.btnSave_Click | Solution.Web/DWSP/NationalTargetSetup.aspx.cs:180 | — | NationalTargetSetupDal.SaveNationalTargetSetup | sp_Save_NationaltargetSetup | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnUpdate | LinkButton click | return sweetAlertConfirm_Update(this); | DWSP_NationalTargetSetup.btnSave_Click | Solution.Web/DWSP/NationalTargetSetup.aspx.cs:180 | — | NationalTargetSetupDal.SaveNationalTargetSetup | sp_Save_NationaltargetSetup | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
|  | LinkButton click | (postback) | DWSP_NationalTargetSetup.Unnamed_Click | Solution.Web/DWSP/NationalTargetSetup.aspx.cs:208 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Page init (`Page_Load`, first load only): `LoadInitialInfo()` fills Year (2015-2050) and Month (12 rolling months) and the Group list (`NationalTargetSetupDal.Get_Group_All` -> `sp_NTS_Group_Active`, item 0 = "Please Select From List"). If `?MID=` is present `btnUpdate` is shown and `id_mastetID` is set to it, otherwise `btnSave` is shown (`NationalTargetSetup.aspx.cs:18-37`). **No code loads the existing record for `MID`** - edit mode only carries the id into the hidden field (CONFIRMED: no read of `MID` beyond line 24).
- Only client script: SweetAlert confirm (`sweetAlertConfirm_Submit/_Update`) before postback, `successalert(msg,'Success',url)` redirect, `faildalert`.
- Full postback (no AJAX endpoints, no PageMethods). Amount textbox has a client-side numeric filter (FilteredTextBoxExtender in markup).

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| DWSP_NationalTargetSetup.Page_Load | Solution.Web/DWSP/NationalTargetSetup.aspx.cs:19 | object sender, EventArgs e | DWSP_NationalTargetSetup.Page_Load → NationalTargetSetupDal.Get_Group_All → SP: sp_NTS_Group_Active | no | no |
| DWSP_NationalTargetSetup.LoadInitialInfo | Solution.Web/DWSP/NationalTargetSetup.aspx.cs:40 |  | DWSP_NationalTargetSetup.LoadInitialInfo → NationalTargetSetupDal.Get_Group_All → SP: sp_NTS_Group_Active | yes | no |
| DWSP_NationalTargetSetup.btnSave_Click | Solution.Web/DWSP/NationalTargetSetup.aspx.cs:180 | object sender, EventArgs e | DWSP_NationalTargetSetup.btnSave_Click → NationalTargetSetupDal.SaveNationalTargetSetup → SP: sp_Save_NationaltargetSetup | no | yes |
| DWSP_NationalTargetSetup.Unnamed_Click | Solution.Web/DWSP/NationalTargetSetup.aspx.cs:208 | object sender, EventArgs e | DWSP_NationalTargetSetup.Unnamed_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| NationalTargetSetupDal.Get_Group_All | Library.DAL/DWSP_DAL/NationalTargetSetupDal.cs:71 | sp_NTS_Group_Active |  |
| NationalTargetSetupDal.SaveNationalTargetSetup | Library.DAL/DWSP_DAL/NationalTargetSetupDal.cs:17 | sp_Save_NationaltargetSetup | @Amount, @EntryBy, @GroupId, @Month, @NatargetSpId |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_NTS_Group_Active | NationalTargetSetupDal.Get_Group_All | body NOT in repository | write-like name |
| sp_Save_NationaltargetSetup | NationalTargetSetupDal.SaveNationalTargetSetup | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadInitialInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm mb-3 is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','NationalTargetSetupView.aspx'); | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M14-001 | **Target cascade.** Targets are planned top-down: National (Group, month, year) -> Zone (`sp_Get_NationalTargetAmount` supplies the parent figure on the Zone page) -> Area (`sp_Get_ZoneTargetAmount`) -> Territory (`sp_Get_AreaTargetAmount`) -> employee/territory day-wise DWSP (`sp_Get_TerritoryWiseTargetSetup`). Each lower page shows the parent amount read-only and asks the user to split it. The cascade is only *advisory* in the UI (see BR-M14-006). | `DWSP/ZoneWiseTargetSetup.aspx.cs:326-338`; `DWSP/AreaWiseTargetSetup.aspx.cs:536-555`; `DWSP/TerritoryWiseTargetSetupApps.aspx.cs:562-583`; `DWSP/AMDayWiseDWSPSetup.aspx.cs:556-577`; DAL `Library.DAL/DWSP_DAL/ZoneWiseTargetSetupDal.cs:88-227` | code-behind + DAL (SP bodies UNKNOWN) | CONFIRMED (parent-lookup wiring); INFERRED (business meaning) |
| BR-M14-002 | Year drop-down is a fixed list 2015-2050, default = current year. Month drop-down lists 12 entries starting from the *current calendar month* and wrapping (e.g. in September: September ... August); each item's Text is the month name and Value is the calendar month number; the current month is pre-selected. | `DWSP/AreaWiseTargetSetup.aspx.cs:187-215` (same `GetYearList`/`GetMonthList` copy in every DWSP page) | code-behind | CONFIRMED |
| BR-M14-003 | Except on the DWSP day-wise page, the month is persisted as the month **name** text (`ddlmonth.SelectedItem.Text`, e.g. `September`) plus a text year; the DWSP page and the monthly report use the month **number**. The two conventions meet only inside SPs whose bodies are not in the repo. | `DWSP/AreaWiseTargetSetup.aspx.cs:317-352`; `DWSP/AMDayWiseDWSPSetup.aspx.cs:666-680` | code-behind | CONFIRMED |
| BR-M14-004 | **National target save rules.** Required: Year, Month, Group, Amount (`txtAmount.Text.Trim() != ""`); first failing control gets the `is-invalid` CSS class + tooltip "please fill out this field" and save stops. Amount is parsed with `decimal.Parse` (non-numeric text throws an unhandled `FormatException`; no try/catch). If `Session["UserId"]` is null the click silently does nothing (empty `else`). | `NationalTargetSetup.aspx.cs:101-135,158-172,179-205` | code-behind | CONFIRMED |
| BR-M14-005 | **National save semantics.** `NatargetSpId == 0` -> `SaveDataReturnPrimaryKey("sp_Save_NationaltargetSetup")`; success only if returned id `> 0`, else the page shows `Already Exist!` (duplicate detection therefore lives inside the SP: UNKNOWN). `NatargetSpId > 0` -> `UpdateData(...)` (never reached from this page in practice because edit mode does not load/parse the id into a record - see §30). Parameters: `@NatargetSpId,@GroupId,@Year ` (note the trailing space in the name), `@Month,@Amount,@EntryBy`. | `Library.DAL/DWSP_DAL/NationalTargetSetupDal.cs:16-68` | DAL | CONFIRMED |
| BR-M14-012 | **Atomic writes.** Every DAL save opens one connection, lazily begins one transaction on the first write and commits in `SqlConnectionClose()`; on exception the DAL calls `SqlConnectionClose(true)` (rollback) and rethrows. So multi-row saves on the other DWSP pages are all-or-nothing, and the `Already Exist!` branch of `SaveData`-based DAL methods is effectively unreachable (`SaveData` returns `true` or throws). | `Library.DAL/DataManager/DataAccessManager.cs:122-146,405-515` | DAL infra | CONFIRMED |

## 20. Permission Matrix
- Session keys read: `UserId` (only in `btnSave_Click`; used as `@EntryBy`). No role/permission check in code (CONFIRMED - `RoleTypeName` is not read).
- Access is therefore *menu-based only* plus the master page's Session check (`NewMasterPage.master.cs:99`). Any logged-in role that knows the URL can post a national target. Which roles get the menu item: UNKNOWN (DB-driven).

## 21. Status Lifecycle
No status column is written or read by this page (CONFIRMED). A target row is simply present/absent per (Group, Year, Month); duplicates are rejected by `sp_Save_NationaltargetSetup` (UNKNOWN mechanism, surfaced as `Already Exist!`). No approval workflow for national targets in the code.

## 22. Error Handling
- `Validation()` failures are UI-only (CSS class + tooltip); no message text.
- DAL exceptions are rethrown (`throw exception;`) and **not caught by the page** -> ASP.NET yellow error page / `Application_Error` (empty in `Global.asax`).
- Success -> `successalert('Operation successful!','Success','NationalTargetSetupView.aspx')`; failure (pk = 0) -> `faildalert('Already Exist!','Faild')`.

## 23. Audit
Audit columns: the page passes an `@EntryBy`-style value to the SP (see §8) but which audit columns/tables the SP populates is `UNKNOWN` (SP body not in repo). `UpdateBy/UpdateDate/EntryDate` exist on the DAO (`Library.DAO/DWSP_DAO/*.cs`) but are never set by the pages.

## 24. Integration
None. Pure data entry into the SQL Server database. Its output is consumed by the Zone target page (BR-M14-001) and, indirectly, by target-achievement reports (RPT-M14-02).

## 25. Reports
No report from this page. Listing is on `NationalTargetSetupView.aspx` (PAGE-542).

## 26. Dependencies
BLL: —; DAL: NationalTargetSetupDal; DAO: NationalTargetDao, ResultInfo; Tables (inline): —

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-541-01 | Positive | Choose Year 2026, Month September, Group X, Amount 1000000, Save (new) | `sp_Save_NationaltargetSetup` executed with `@NatargetSpId=0`; redirect to `NationalTargetSetupView.aspx` | BR-M14-004/005 |
| TC-541-02 | Negative | Amount empty | Amount box gets `is-invalid`, no DB call | BR-M14-004 |
| TC-541-03 | Negative | Group left at "Please Select From List" | Group control marked invalid, no DB call | BR-M14-004 |
| TC-541-04 | Negative | Amount `abc` (bypass client filter) | Unhandled `FormatException` (defect) | BR-M14-004 |
| TC-541-05 | Edge | Save the same Group+Month+Year twice | Second save returns id 0 -> `Already Exist!` (if SP enforces uniqueness; UNKNOWN) | BR-M14-005 |
| TC-541-06 | Edge | Open `?MID=5` | Update button shown but form is empty (no record load) | BR-M14-005 |
| TC-541-07 | Security | Session expired then click Save | Nothing happens (silent `else`) | BR-M14-004 |
| TC-541-08 | Cascade | After saving, open Zone page for same Group/Month/Year | Parent "National Target" label shows the saved amount | BR-M14-001 |

## 28. Reverse Engineering References
- `Solution.Web/DWSP/NationalTargetSetup.aspx`, `Solution.Web/DWSP/NationalTargetSetup.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | `NationalTargetSetup.aspx(.cs)` | n/a | MISSING | - |
| Required fields | None existed | Year, Month, Group, Amount | SP constraints UNKNOWN | UNKNOWN | BR-M14-004 |
| Duplicate handling | None existed | id 0 => `Already Exist!` | SP body not in repo | UNKNOWN | BR-M14-005 |
| Edit mode | None existed | Update button shown for `?MID=` but record never loaded | n/a | DEFECT | §30 |

## 30. Known Gaps
- GAP-M14-004: `?MID=` edit mode does not load the record; `btnUpdate` posts an empty form (`NationalTargetSetup.aspx.cs:18-37`).
- Non-numeric amount is not caught (`decimal.Parse`, line ~168).
- Silent no-op when session is missing (lines 181-205).
- `@Year ` parameter name has a trailing space (`NationalTargetSetupDal.cs:28`) - works with ADO.NET trimming? UNKNOWN; flagged.
- 3 SPs (`sp_Save_NationaltargetSetup`, `sp_NTS_Group_Active`, plus the view page's `sp_Get_Group_List`) have no body in the repo (GAP-M14-001).
- Page calls DAL directly, bypassing the BLL layer.

## 31. Change Impact
- Writes the table(s) behind `sp_Save_NationaltargetSetup` - read by `sp_Get_NationalTargetAmount` (Zone target page PAGE-546) and listed by PAGE-542.
- `NationalTargetSetupDal.Get_Group_All` (`sp_NTS_Group_Active`) is also used (via `CommonDal.Get_Group_All`) by PAGE-538/539/545/546 - changing the SP changes every DWSP page.
- Any change to month-name storage (BR-M14-003) must be mirrored in the Zone/Area/Territory pages and SPs.

