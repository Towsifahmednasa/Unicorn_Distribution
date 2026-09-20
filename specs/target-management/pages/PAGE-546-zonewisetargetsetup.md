---
page_id: PAGE-546
module: M14
page: ZoneWiseTargetSetup
tier: A
generated: false   # upgraded by manual code review (Tier A)
reviewed: 2026-09-19
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-546 — ZoneWiseTargetSetup

> **Evidence level:** Tier A - reviewed manually against `.aspx`, `.aspx.cs`, and the BLL/DAL code they reach (review date 2026-09-19). Claims are tagged CONFIRMED / INFERRED / UNKNOWN. Stored-procedure bodies are NOT in the repository unless a section says so; column-level effects of such SPs stay `UNKNOWN`. Sections 1, 4-10, 13-15 and 28 still carry the mechanically extracted content (re-checked where noted); all other sections were rewritten.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-546 |
| Module | M14 — Target & DWSP Management (INFERRED from folder + file name) |
| Page name | ZoneWiseTargetSetup |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | DWSP |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DWSP_ZoneWiseTargetSetup |
| aspx size | 14467 bytes |
| Code-behind LOC | 408 |
| Inline script size | 1374 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
Lets a national/head-office planner **split the national monthly target of a Group across the group's active Zones** (DZSM regions). The user picks Year, Month and Group; the page shows the saved national amount (read-only label) and a grid with one editable amount per active zone; Save writes one zone target per row. CONFIRMED: `Solution.Web/DWSP/ZoneWiseTargetSetup.aspx.cs:128-273,289-340`.

## 3. Business Objective
Distribute the national target down to zone level (step 2 of the cascade, BR-M14-001) so that area/territory/day targets can be planned. Business intent INFERRED from names and data flow.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DWSP/ZoneWiseTargetSetupView.aspx`

Outbound references: `zonewisetargetsetupview.aspx`

## 5. Route
`~/DWSP/ZoneWiseTargetSetup.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DWSP/ZoneWiseTargetSetup.aspx |
| Code-behind | Solution.Web/DWSP/ZoneWiseTargetSetup.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | NationalTargetSetupDal (Library.DAL/DWSP_DAL/NationalTargetSetupDal.cs) |
| DAL class | ZoneWiseTargetSetupDal (Library.DAL/DWSP_DAL/ZoneWiseTargetSetupDal.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |
| DAO/entity class | ZoneWiseTargetDao (Library.DAO/DWSP_DAO/ZoneWiseTargetDao.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| id_mastetID | HiddenField |  | no validator found |  |  |  |  |
| ddlYear | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→ddlmonth_SelectedIndexChanged |
| ddlmonth | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→ddlmonth_SelectedIndexChanged |
| groupname | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→groupname_SelectedIndexChanged |
| lblNationalTargetAmount | Label |  | no validator found |  |  |  |  |
| gv_List | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL# | no validator found |  |  |  |  |
| HFZoneId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | TemplateField |  | no validator found |  |  |  |  |
| chkSelectAll | CheckBox |  | no validator found |  |  |  | OnCheckedChanged→chkSelectAll_CheckedChanged |
| chkSelect | CheckBox |  | no validator found |  |  |  | OnCheckedChanged→chkSelect_CheckedChanged |
| (no id) | BoundField | Zone | no validator found |  |  |  |  |
| (no id) | TemplateField | Amount | no validator found |  |  |  |  |
| txtAmount | TextBox | <%#Eval("Amount") %> | no validator found |  |  |  | OnTextChanged→txtAmount_OnTextChanged |
| FilteredTssextBoxExtender1 | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| btnSave | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Submit(this); |
| btnUpdate | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Update(this); |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| ddlYear | ZoneWiseTargetDao.Year | DWSP_ZoneWiseTargetSetup.btnSave_Click | (none — page calls DAL directly) | ZoneWiseTargetSetupDal.SaveZoneWiseTarget | @Year | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlmonth | ZoneWiseTargetDao.Month | DWSP_ZoneWiseTargetSetup.btnSave_Click | (none — page calls DAL directly) | ZoneWiseTargetSetupDal.SaveZoneWiseTarget | @Month | UNKNOWN (SP body not in repo) | UNKNOWN |
| groupname | ZoneWiseTargetDao.GroupId | DWSP_ZoneWiseTargetSetup.btnSave_Click | (none — page calls DAL directly) | ZoneWiseTargetSetupDal.SaveZoneWiseTarget | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| lblNationalTargetAmount | ZoneWiseTargetDao.TargetAmount | DWSP_ZoneWiseTargetSetup.btnSave_Click | (none — page calls DAL directly) | ZoneWiseTargetSetupDal.SaveZoneWiseTarget | @TargetAmount | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtAmount | ZoneWiseTargetDao.Amount | DWSP_ZoneWiseTargetSetup.btnSave_Click | (none — page calls DAL directly) | ZoneWiseTargetSetupDal.SaveZoneWiseTarget | @Amount | UNKNOWN (SP body not in repo) | UNKNOWN |
| groupname | ds | DWSP_ZoneWiseTargetSetup.groupname_SelectedIndexChanged | — | ZoneWiseTargetSetupDal.Get_NationalTargetAmount_All_ByGroup, ZoneWiseTargetSetupDal.Get_ZoneActive_All_ByGroup | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlmonth | dtTargetAmount | DWSP_ZoneWiseTargetSetup.NewMethod | — | ZoneWiseTargetSetupDal.Get_NationalTargetAmount_All_ByGroup | UNKNOWN | UNKNOWN | UNKNOWN |
| lblNationalTargetAmount | target | DWSP_ZoneWiseTargetSetup.txtAmount_OnTextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

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
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | DWSP_ZoneWiseTargetSetup.btnSave_Click | Solution.Web/DWSP/ZoneWiseTargetSetup.aspx.cs:229 | — | ZoneWiseTargetSetupDal.SaveZoneWiseTarget | sp_Save_ZoneWiseTargetSetup | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnUpdate | LinkButton click | return sweetAlertConfirm_Update(this); | DWSP_ZoneWiseTargetSetup.btnSave_Click | Solution.Web/DWSP/ZoneWiseTargetSetup.aspx.cs:229 | — | ZoneWiseTargetSetupDal.SaveZoneWiseTarget | sp_Save_ZoneWiseTargetSetup | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
|  | LinkButton click | (postback) | DWSP_ZoneWiseTargetSetup.Unnamed_Click | Solution.Web/DWSP/ZoneWiseTargetSetup.aspx.cs:275 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- `Page_Load` (first load): `LoadInitialInfo()` -> Group list (`NationalTargetSetupDal.Get_Group_All`), Year/Month lists. `?MID=` shows `btnUpdate` (same button handler `btnSave_Click`), otherwise `btnSave` (`ZoneWiseTargetSetup.aspx.cs:29-48`).
- `groupname_SelectedIndexChanged` (AutoPostBack): clears grid and national label, loads active zones of the group (`ZoneWiseTargetSetupDal.Get_ZoneActive_All_ByGroup` -> `sp_Get_Zone_All_Active_ZoneTarget`), binds `gv_List`, calls `NewMethod()` to read the national target (`sp_Get_NationalTargetAmount`, params `@GroupId,@month(text),@Year`) into `lblNationalTargetAmount`, then sums pre-filled `txtAmount` values into the footer.
- `ddlmonth/ddlYear` change -> only `NewMethod()` (label refresh; grid is NOT reloaded).
- `txtAmount_OnTextChanged` (AutoPostBack per row): running-sum check (BR-M14-006).
- Reset button handler `Unnamed_Click` is empty (no reset).
- Client: SweetAlert confirm, numeric-only filter (`ValidChars="0123456789"` - integers only, no decimals), redirect on success to `ZoneWiseTargetSetupView.aspx`.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| DWSP_ZoneWiseTargetSetup.Page_Load | Solution.Web/DWSP/ZoneWiseTargetSetup.aspx.cs:30 | object sender, EventArgs e | DWSP_ZoneWiseTargetSetup.Page_Load → NationalTargetSetupDal.Get_Group_All → SP: sp_NTS_Group_Active | no | no |
| DWSP_ZoneWiseTargetSetup.LoadInitialInfo | Solution.Web/DWSP/ZoneWiseTargetSetup.aspx.cs:50 |  | DWSP_ZoneWiseTargetSetup.LoadInitialInfo → NationalTargetSetupDal.Get_Group_All → SP: sp_NTS_Group_Active | yes | no |
| DWSP_ZoneWiseTargetSetup.btnSave_Click | Solution.Web/DWSP/ZoneWiseTargetSetup.aspx.cs:229 | object sender, EventArgs e | DWSP_ZoneWiseTargetSetup.btnSave_Click → ZoneWiseTargetSetupDal.SaveZoneWiseTarget → SP: sp_Save_ZoneWiseTargetSetup | no | yes |
| DWSP_ZoneWiseTargetSetup.Unnamed_Click | Solution.Web/DWSP/ZoneWiseTargetSetup.aspx.cs:275 | object sender, EventArgs e | DWSP_ZoneWiseTargetSetup.Unnamed_Click | no | no |
| DWSP_ZoneWiseTargetSetup.gv_DocumentUpload_PreRender | Solution.Web/DWSP/ZoneWiseTargetSetup.aspx.cs:278 | object sender, EventArgs e | DWSP_ZoneWiseTargetSetup.gv_DocumentUpload_PreRender | no | no |
| DWSP_ZoneWiseTargetSetup.groupname_SelectedIndexChanged | Solution.Web/DWSP/ZoneWiseTargetSetup.aspx.cs:290 | object sender, EventArgs e | DWSP_ZoneWiseTargetSetup.groupname_SelectedIndexChanged → ZoneWiseTargetSetupDal.Get_NationalTargetAmount_All_ByGroup / ZoneWiseTargetSetupDal.Get_ZoneActive_All_ByGroup → SP: sp_Get_NationalTargetAmount, sp_Get_Zone_All_Active_ZoneTarget | no | no |
| DWSP_ZoneWiseTargetSetup.NewMethod | Solution.Web/DWSP/ZoneWiseTargetSetup.aspx.cs:327 |  | DWSP_ZoneWiseTargetSetup.NewMethod → ZoneWiseTargetSetupDal.Get_NationalTargetAmount_All_ByGroup → SP: sp_Get_NationalTargetAmount | no | no |
| DWSP_ZoneWiseTargetSetup.txtAmount_OnTextChanged | Solution.Web/DWSP/ZoneWiseTargetSetup.aspx.cs:343 | object sender, EventArgs e | DWSP_ZoneWiseTargetSetup.txtAmount_OnTextChanged | no | no |
| DWSP_ZoneWiseTargetSetup.ddlmonth_SelectedIndexChanged | Solution.Web/DWSP/ZoneWiseTargetSetup.aspx.cs:405 | object sender, EventArgs e | DWSP_ZoneWiseTargetSetup.ddlmonth_SelectedIndexChanged → ZoneWiseTargetSetupDal.Get_NationalTargetAmount_All_ByGroup → SP: sp_Get_NationalTargetAmount | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| NationalTargetSetupDal.Get_Group_All | Library.DAL/DWSP_DAL/NationalTargetSetupDal.cs:71 | sp_NTS_Group_Active |  |
| ZoneWiseTargetSetupDal.SaveZoneWiseTarget | Library.DAL/DWSP_DAL/ZoneWiseTargetSetupDal.cs:17 | sp_Save_ZoneWiseTargetSetup | @Amount, @EntryBy, @GroupId, @Month, @RegionId, @TargetAmount, @Year, @ZoneWTSetupId |
| ZoneWiseTargetSetupDal.Get_ZoneActive_All_ByGroup | Library.DAL/DWSP_DAL/ZoneWiseTargetSetupDal.cs:70 | sp_Get_Zone_All_Active_ZoneTarget | @GroupId |
| ZoneWiseTargetSetupDal.Get_NationalTargetAmount_All_ByGroup | Library.DAL/DWSP_DAL/ZoneWiseTargetSetupDal.cs:209 | sp_Get_NationalTargetAmount | @GroupId, @Year, @month |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_NationalTargetAmount | ZoneWiseTargetSetupDal.Get_NationalTargetAmount_All_ByGroup | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_All_Active_ZoneTarget | ZoneWiseTargetSetupDal.Get_ZoneActive_All_ByGroup | body NOT in repository | read-like name (INFERRED) |
| sp_NTS_Group_Active | NationalTargetSetupDal.Get_Group_All | body NOT in repository | write-like name |
| sp_Save_ZoneWiseTargetSetup | ZoneWiseTargetSetupDal.SaveZoneWiseTarget | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadInitialInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Table can not be Empty! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm mb-3 clsDecimal is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','ZoneWiseTargetSetupView.aspx'); | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Amount must be equal with target amount | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Target Amount | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm mb-3 clsDecimal is-invalid | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M14-002 | Year drop-down is a fixed list 2015-2050, default = current year. Month drop-down lists 12 entries starting from the *current calendar month* and wrapping (e.g. in September: September ... August); each item's Text is the month name and Value is the calendar month number; the current month is pre-selected. | `DWSP/AreaWiseTargetSetup.aspx.cs:187-215` (same `GetYearList`/`GetMonthList` copy in every DWSP page) | code-behind | CONFIRMED |
| BR-M14-003 | Except on the DWSP day-wise page, the month is persisted as the month **name** text (`ddlmonth.SelectedItem.Text`, e.g. `September`) plus a text year; the DWSP page and the monthly report use the month **number**. The two conventions meet only inside SPs whose bodies are not in the repo. | `DWSP/AreaWiseTargetSetup.aspx.cs:317-352`; `DWSP/AMDayWiseDWSPSetup.aspx.cs:666-680` | code-behind | CONFIRMED |
| BR-M14-006 | **Running-sum cap (advisory).** On every amount edit the page walks the grid top-down accumulating `temValue += amount`; if the running total is `<=` the national target the footer shows it, otherwise the message `Amount must be equal with target amount` is shown and *that* row's box is cleared (and footer cleared) - the loop continues so later rows may still pass. The message says "equal" but only `<=` is enforced, and **Save does not re-check the total** (no `sum == target` or `sum <= target` test on the server) - a user can save less than, or (by editing without postback) more than, the national target. If the national label is empty the message `Please Select Target Amount` clears all amounts. | `ZoneWiseTargetSetup.aspx.cs:342-405` | code-behind (UI only) | CONFIRMED |
| BR-M14-007 | **Zone save validation.** Required: Year, Month, Group, national-target label non-empty, >=1 grid row (`Table can not be Empty!`), every row's amount non-empty (blank -> invalid). Rows saved: `ZoneWTSetupId=0`, `@TargetAmount` = national amount, `@RegionId` = row `HFZoneId`, `@Amount` = row amount, `@EntryBy` = `Session["UserId"]`. | `ZoneWiseTargetSetup.aspx.cs:128-273`; `ZoneWiseTargetSetupDal.cs:16-66` | code-behind + DAL | CONFIRMED |
| BR-M14-008 | **Zone save semantics.** For each row: `ZoneWTSetupId > 0` -> `SaveData`, else `SaveDataReturnPrimaryKey` and `isSuccess = true` if `pk > 0`. `isSuccess` is initialised false and only ever set true, so the page reports success if **any** row returned `pk > 0`, even if other rows returned 0 (duplicates skipped silently by the SP). All rows share one transaction (BR-M14-012). | `ZoneWiseTargetSetupDal.cs:34-49` | DAL | CONFIRMED |
| BR-M14-009 | **Only active zones of the chosen group are listed** (`sp_Get_Zone_All_Active_ZoneTarget @GroupId`); definition of "active" is inside the SP (UNKNOWN). | `ZoneWiseTargetSetupDal.cs:69-86` | SP | INFERRED |

## 20. Permission Matrix
- Session keys read: `UserId` (audit value on save). No role check; no scoping of groups/zones by the caller's role (unlike the Area page, BR-M14-011). CONFIRMED by absence of `RoleTypeName`/`RoleTypeId` reads in this file.
- Any logged-in user with the menu entry can allocate zone targets. Which roles: UNKNOWN.

## 21. Status Lifecycle
No status field is set by the page. Presence of a row per (Group, Year, Month, Region) is the only state; duplicate handling inside `sp_Save_ZoneWiseTargetSetup` (UNKNOWN). No approval/lock step.

## 22. Error Handling
- Validation failures: CSS `is-invalid` + tooltip; `showMessageBox("Table can not be Empty!")` for empty grid; `showMessageBox("Please Select Target Amount")` / `("Amount must be equal with target amount")` from the running-sum handler.
- Data-load failures in `LoadInitialInfo` are swallowed (`catch (Exception ex) { }`), so an empty Group list is silent.
- `NewMethod()`/save exceptions are not caught (only DAL rollback + rethrow).
- Success: `successalert(..., 'ZoneWiseTargetSetupView.aspx')`; failure: `faildalert('Already Exist!')`.

## 23. Audit
Audit columns: the page passes an `@EntryBy`-style value to the SP (see §8) but which audit columns/tables the SP populates is `UNKNOWN` (SP body not in repo). `UpdateBy/UpdateDate/EntryDate` exist on the DAO (`Library.DAO/DWSP_DAO/*.cs`) but are never set by the pages.

## 24. Integration
None (database only).

## 25. Reports
No report from this page; list page PAGE-547.

## 26. Dependencies
BLL: —; DAL: NationalTargetSetupDal, ZoneWiseTargetSetupDal; DAO: ResultInfo, ZoneWiseTargetDao; Tables (inline): —

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-546-01 | Positive | Select a Group that has a saved national target; enter zone amounts summing to the target; Save | one `sp_Save_ZoneWiseTargetSetup` call per zone in a single transaction; redirect to view | BR-M14-007/008/012 |
| TC-546-02 | Negative | Group with no national target | label empty; editing an amount shows "Please Select Target Amount" and clears grid | BR-M14-006 |
| TC-546-03 | Negative | Enter amounts whose running total exceeds the target | offending box cleared with "Amount must be equal with target amount" | BR-M14-006 |
| TC-546-04 | Edge | Enter amounts summing to *less* than the target and Save | Saves (no server check) | BR-M14-006 |
| TC-546-05 | Negative | Leave one zone amount blank | that box `is-invalid`, no DB write | BR-M14-007 |
| TC-546-06 | Edge | Re-save same Group/Month/Year | success if any row returns pk>0; otherwise `Already Exist!` | BR-M14-008 |
| TC-546-07 | Edge | Change Month after loading grid | label updates, grid amounts do not reload | BR-M14-006 |
| TC-546-08 | Negative | Type `12.5` in amount | FilteredTextBox blocks `.` (integers only) | - |

## 28. Reverse Engineering References
- `Solution.Web/DWSP/ZoneWiseTargetSetup.aspx`, `Solution.Web/DWSP/ZoneWiseTargetSetup.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | `ZoneWiseTargetSetup.aspx(.cs)` | n/a | MISSING | - |
| Sum(zone) = national | None existed | UI cap only (`<=`), message says "equal"; no server check | not known | UNKNOWN | BR-M14-006 |
| Edit (`?MID`) | None existed | `btnUpdate` maps to same handler; no load of existing row | - | DEFECT | §30 |

## 30. Known Gaps
- Running-sum cap not enforced on Save (BR-M14-006); message text wrong ("equal").
- `btnUpdate`/`?MID` edit mode: no record is loaded and `ZoneWTSetupId` is always 0, so "update" is really another insert.
- Partial success hidden (BR-M14-008).
- Empty catch blocks in `LoadInitialInfo` (lines 49-70).
- Integers only for amounts (`ValidChars="0123456789"`) although DAO/SP use decimals.
- Unused static fields `aTargetDal`, `aTargetDal2` (PromoAlloc DAL) - dead code.
- SP bodies missing: `sp_Save_ZoneWiseTargetSetup`, `sp_Get_Zone_All_Active_ZoneTarget`, `sp_Get_NationalTargetAmount`, `sp_NTS_Group_Active` (GAP-M14-001).

## 31. Change Impact
- Reads national target written by PAGE-541; writes zone rows read by Area page (`sp_Get_ZoneTargetAmount`, PAGE-539) and listed by PAGE-547.
- `ZoneWiseTargetSetupDal` is also used by PAGE-538/539/545 (`Get_TerriTargetAmount_*`, `Get_FirstDate_LastDatebyYearMonth`, `GetDatewithinDateRange`).

