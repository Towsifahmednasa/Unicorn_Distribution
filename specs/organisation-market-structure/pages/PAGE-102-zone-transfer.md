---
page_id: PAGE-102
module: M03
page: Zone_Transfer
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-102 — Zone_Transfer

> **Evidence level:** Tier A — page `.aspx`, `.aspx.cs`, the embedded user control (`TransferUI/IVMarketStructureFrom_ToZoneTranfer.ascx(.cs)`), `MarketStructureTransferDAL`, `CommonDataLoad`, `DataAccessManager` were read in full. Stored-procedure bodies are NOT in the repository, so what the SP writes (and whether it re-parents immediately or only stages a request) is `UNKNOWN`. Shared-control behaviour is documented once in `specs/organisation-market-structure/README.md` §Market-structure user controls (referred to below as **[MS-CTRL]**). The four sibling pages (Area/Territory/SubTerritory/Market transfer) are near-copies of this page; see `workflows/WF-M03-01-market-structure-transfer-request.md`.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-102 |
| Module | M03 — Organisation & Market Structure Master (CONFIRMED: folder `TransferUI`, page edits market-structure hierarchy) |
| Page name | Zone_Transfer (breadcrumb text "Zone Transfer", `Solution.Web/TransferUI/Zone_Transfer.aspx:51`) |
| Kind | Entry / Transaction — creates a *zone re-parenting request* (CONFIRMED: writes via `sp_Update_MarketStructure_Transfer`; the request is later approved on PAGE-099) |
| Web folder | TransferUI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | `TransferUI_Area_Transfer` (class name is a copy of Area_Transfer — every transfer page except Market/SubTerritory/Territory reuses a copied class name; they are separate files so no compile clash) |
| aspx size / code-behind LOC | 9313 bytes / 297 lines |
| Inline script | `pageLoad()` only re-initialises `pickadate` and `select2` (`Zone_Transfer.aspx:86-99`) |
| Suspected legacy/duplicate | Near-duplicate of PAGE-095/096/100/101 (same code, five copies). Not dead: linked from menu (menu content UNKNOWN). |

## 2. Purpose
CONFIRMED (labels/code): lets an administrator move one or more **Zones** (DB column `RegionId`/`RegionName`) from one **Group** (the top "National" level) to another Group. The user picks a source Group, presses *Search* to list the active zones under that group as a checkbox list, picks a destination Group, ticks the zones to move and submits.

## 3. Business Objective
INFERRED: keep the sales-force hierarchy (Group → Zone → Area → Territory → Sub-Territory → Market) aligned with organisational restructuring (e.g. a zone is re-assigned to another national/group head) without re-keying master data. Because customers/doctors/orders are tied to territory/market, a controlled transfer (request → approval on PAGE-099) is required; the approval step is implemented in a separate page.

## 4. Navigation
Menu placement is DB-driven (`sp_GET_MainMenuRole2`); exact menu path `UNKNOWN`. Outbound: *Reset* redirects to `Market_Transfer.aspx` (not to itself — CONFIRMED `Zone_Transfer.aspx.cs:278`; all five transfer pages redirect Reset to Market_Transfer). Success alert reloads `Zone_Transfer.aspx` (`.cs:182`). No inbound links found in code.

## 5. Route
`~/TransferUI/Zone_Transfer.aspx` (file-based; no MapPageRoute).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/TransferUI/Zone_Transfer.aspx |
| Code-behind | Solution.Web/TransferUI/Zone_Transfer.aspx.cs |
| User control (used twice: From & To) | Solution.Web/TransferUI/IVMarketStructureFrom_ToZoneTranfer.ascx / .ascx.cs |
| DAL (transfer) | Library.DAL/Transfer_DAL/MarketStructureTransferDAL.cs (namespace `Library.DAL.MasterSetup_DAL`) — `UpdateZone_Transfer` lines 375-441 |
| DAL (lookups) | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs — `GetZone_byGroupId_Active` (1212), `GetGroupInfo_Active/All`, cascade loaders |
| DAO | Library.DAO/Transer_DAO/MarketStructureTranferDAO.cs |
| Infra | Library.DAL/DataManager/DataAccessManager.cs; SalesSolution.Web.Models.ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
Both fieldsets embed the same user control (control tree in **[MS-CTRL]**). In this page's control variant only the **Group** row is visible; Zone/Area/Territory/Sub-Territory/Market rows are wrapped in `runat="server" visible="false"` divs (`IVMarketStructureFrom_ToZoneTranfer.ascx:24,46,59,79,95`) — the dropdown objects exist but are not rendered.

| Control | Label | Required | Default | Read-only/Enabled/Visible | Data source | Error / behaviour |
|---|---|---|---|---|---|---|
| `IVMarketStructure` (From) `GroupSelect` | "Group:" (From fieldset) | Yes for *Search* (`btnSearch_Click` `.cs:199`), not validated on Submit | "Please Select From List" (value "") | Enabled=false when session role is NSM (control code lines 54-57) | `sp_CS_Group_Active` via `CommonDataLoad.GetGroupInfo_Active` | Search with empty group → alert "Please Select Area!" (wrong wording, `.cs:218`) |
| `IVMarketStructureTo` (To) `GroupSelect` | "Group:" (To fieldset) | Yes — Submit (`Validation()` `.cs:99-105`) | placeholder | as above | same | tooltip "please fill out this field", CSS `is-invalid`, focus; no alert text |
| `btnSearch` | Search | – | – | – | – | loads `chkListMarket` |
| `chkMarket` | " Select All / Unselect All" | – | unchecked | AutoPostBack | – | selects/deselects all items (`.cs:281-296`) |
| `chkListMarket` | heading "Territory List" (text is wrong — it lists zones; `Zone_Transfer.aspx:156`) | ≥1 checked (`.cs:90-94`) | empty | 4 columns horizontal | `sp_Get_Zone_All_Active(@GroupId)` → value `RegionId`, text `RegionName` | alert "Please Select at least one Territory !!!" (wrong wording) |
| `btnSave` | Submit | – | – | `OnClientClick="return sweetAlertConfirm_Submit(this);"` (global confirm JS, outside module) | – | – |
| `btnReset` | Reset | – | – | – | – | redirect to Market_Transfer.aspx |
| `progress` / `imgWait` | spinner | – | – | UpdateProgress DisplayAfter=0 | – | – |
Hidden fields inside the control (`hfGroupId, hfZone, hfArea, hfTeritory, hfSubTeritory, hfMarket`) are never set by this page (they are only set by the page in edit-style pages).

## 8. Field Mapping
| UI control | DTO property (`MarketStructureTranferDAO`) | Code | SP parameter | Table.column |
|---|---|---|---|---|
| From `GroupSelect` | `FGroupId` (int?) | `.cs:161` `SelectedIndex>0 ? int.Parse : null` | `@FGroupId` | UNKNOWN |
| From `ZoneSelect` (hidden) | `FRegionId` | `.cs:162` (always null in practice; control hidden, no selection) | `@FRegionId` | UNKNOWN |
| From Area/Territory/SubTerritory (hidden) | `FAreaId`, `FTerritoryId`, `FSubTerritoryId` | `.cs:163-165` | `@FAreaId`, `@FTerritoryId`, `@FSubTerritoryId` | UNKNOWN |
| To `GroupSelect` | `TGroupId` | `.cs:168` | `@TGroupId` | UNKNOWN |
| To Zone/Area/Territory/SubTerritory (hidden) | `TRegionId` … `TSubTerritoryId` | `.cs:169-172` — **not used**: DAL overrides | – | – |
| each ticked `chkListMarket` value | – (string list `param`) | `.cs:125-139`, joined by "," | `@TRegionId` (DAL line 402: `item`) | UNKNOWN |
| constant `"Zone"` | – | `.cs:119` | `@Type` | UNKNOWN |
| `Session["UserId"]` | – | `.cs:179` | `@EntryBy` (string) | UNKNOWN |
Unused DTO members: `FMarketId`, `TMarketId`, `MarketStructureTranferId`, `EntryBy`, `EntryDate`.

## 9. Dropdown Mapping
| Dropdown | Loader | SP | Display/Value | Table(s) |
|---|---|---|---|---|
| Group (From/To) | `LoadInitialDDL` → `CommonDataLoad.GetGroupInfo_Active` (`.ascx.cs:203-222`); NSM/DZSM/AM role users: `GetGroupInfo_All` (`.ascx.cs:72`) | `sp_CS_Group_Active` / `sp_CS_Group_All` | `GroupName` / `GroupId` | UNKNOWN (SP body absent) |
| Zone/Area/Territory/Sub-Territory/Market (hidden) | cascade handlers `.ascx.cs:223-349` | `sp_Get_Zone_All_Active`, `sp_CS_GetArea_ByZoneId_Active`, `sp_CS_GetTerritory_ByAreaId_Active`, `sp_CS_GetSubTerritory_ByTerritoryId_Active`, `sp_CS_GetMarket_BySubTerritoryId_Active` | — | UNKNOWN |
| `chkListMarket` | `btnSearch_Click` → `_dataLoad.GetZone_byGroupId_Active(int)` (`.cs:205`) | `sp_Get_Zone_All_Active(@GroupId)` | `RegionName` / `RegionId` | UNKNOWN |

## 10. Button Action Mapping
| Button | Handler | Chain | SP | DB change |
|---|---|---|---|---|
| Search | `btnSearch_Click` `.cs:195` | `CommonDataLoad.GetZone_byGroupId_Active` | `sp_Get_Zone_All_Active` | read |
| Select all | `chkMarket_CheckedChanged` `.cs:281` | in-memory | – | none |
| Submit | `btnSave_Click` `.cs:110` | `Validation()` → `MarketStructureTransferDAL.UpdateZone_Transfer(dto,"Zone",csv,userId)` | `sp_Update_MarketStructure_Transfer` × N (N = ticked zones) | write; effect UNKNOWN (see WF-M03-01) |
| Reset | `btnReset_Click` `.cs:277` | `Response.Redirect("Market_Transfer.aspx")` | – | none |
Dead handlers: `rbType_SelectedIndexChanged` (empty), `chkSelect_CheckedChanged`/`CheckCustInOrder` (all body commented out), `gv_DocumentUpload_PreRender` (no GridView on page).

## 11. Frontend Flow
1. `Page_Load` (`.cs:23-43`) resolves the 8+6 dropdown references from the two control instances via `FindControl`; no `IsPostBack` logic in the page. The control's own `Page_Load` populates Group lists (see **[MS-CTRL]**).
2. Everything is UpdatePanel (`UpdatePanel2`) partial postbacks; `pageLoad()` re-binds select2 after each async postback. No client-side validation, no AJAX endpoints, no PageMethods. Confirmation dialog `sweetAlertConfirm_Submit` is defined outside the page (master/shared JS) — its content UNKNOWN.
3. Success → `successalert('Operation successful!','Success','Zone_Transfer.aspx')`; failure → `faildalert('Operation Faild!','Faild')` (typo in source).

## 12. Backend Flow
`btnSave_Click`:
1. `Validation()`: reset CSS; count ticked items (`.cs:77-84`); none → `showMessageBox("Please Select at least one Territory !!!")` → false (BR-M03-004); `T_GroupSelect.SelectedValue == ""` → tooltip/CSS/focus → false (BR-M03-005).
2. Build CSV of ticked `RegionId`s (`.cs:125-139`) and DTO (`.cs:158-172`).
3. `UpdateZone_Transfer` (DAL `375-441`): opens `SalesDB` connection; if CSV non-empty, for each id builds the parameter list (`@FGroupId,@FRegionId,@FAreaId,@FTerritoryId,@FSubTerritoryId` from source; `@TGroupId` from destination group; `@TRegionId`=item; `@TAreaId,@TTerritoryId,@TSubTerritoryId,@TMarketId`=NULL; `@EntryBy`; `@Type`="Zone") and calls `accessManager.SaveData("sp_Update_MarketStructure_Transfer", …)`. `finally` → `SqlConnectionClose()` (commit).
4. Transaction: `DataAccessManager` begins one implicit `SqlTransaction` at the first write and commits it in `SqlConnectionClose()`; exception → `SqlConnectionClose(true)` rolls back and the exception is re-thrown (BR-M03-007, BR-M03-101). The page has **no try/catch around the DAL call** (`.cs:179`), so an SP error yields an unhandled exception/yellow screen.
5. `Res.isSuccess` is overwritten on every iteration (BR-M03-009); an empty CSV returns `isSuccess=false` → "Operation Faild!".
try/catch: `btnSearch_Click` swallows all exceptions (`.cs:213`).

## 13. API Mapping
No `[WebMethod]`, no AJAX. (User-control postbacks are ASP.NET auto-postbacks.)

## 14. Service Mapping
No BLL layer — page calls DAL classes directly (`CommonDataLoad`, `MarketStructureTransferDAL`).

## 15. Repository Mapping
| DAL method | Source | SP | Parameters |
|---|---|---|---|
| `MarketStructureTransferDAL.UpdateZone_Transfer` | Library.DAL/Transfer_DAL/MarketStructureTransferDAL.cs:375 | `sp_Update_MarketStructure_Transfer` | `@FGroupId,@FRegionId,@FAreaId,@FTerritoryId,@FSubTerritoryId,@TGroupId,@TRegionId,@TAreaId,@TTerritoryId,@TSubTerritoryId,@TMarketId,@EntryBy,@Type` |
| `CommonDataLoad.GetZone_byGroupId_Active` | CommonDataLoad.cs:1212 | `sp_Get_Zone_All_Active` | `@GroupId` |
| Control loaders | CommonDataLoad.cs (see [MS-CTRL]) | `sp_CS_Group_Active`, `sp_CS_Group_All`, `sp_Get_Zone_AllByGroup`, `sp_HigharchyInfoByEmployeeId`, … | see [MS-CTRL] |

## 16. Stored Procedure Mapping
| SP | Nature | Definition |
|---|---|---|
| `sp_Update_MarketStructure_Transfer` | write (name says "Update"; the approval page reads `MarketStructureTranferId` rows so the SP most likely **inserts a pending transfer row** — INFERRED) | body NOT in repository — table/column effects UNKNOWN |
| `sp_Get_Zone_All_Active` | read | body NOT in repository |
| `sp_CS_Group_Active`, `sp_CS_Group_All`, `sp_HigharchyInfoByEmployeeId` (+cascade SPs) | read | body NOT in repository |

## 17. Database Mapping
No inline SQL on this page. Tables touched by the SPs: `UNKNOWN`. The DAO `MarketStructureTranferDAO` (fields `MarketStructureTranferId`, `F*`/`T*` ids, `EntryBy`, `EntryDate`) and the approval grid key `MarketStructureTranferId` (PAGE-099) imply a transfer-request table; its name is `UNKNOWN`.

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Message | BR |
|---|---|---|---|---|---|---|
| chkListMarket | ≥1 ticked | no | `.cs:90-94` | UNKNOWN | "Please Select at least one Territory !!!" (says Territory; items are Zones) | BR-M03-004 |
| To Group | required | no | `.cs:99-105` | UNKNOWN | tooltip "please fill out this field" | BR-M03-005 |
| From Group | required to Search only | no | `.cs:199` | – | "Please Select Area!" (wrong wording) | BR-M03-006 |
| From ≠ To | not checked | – | – | UNKNOWN | – | BR-M03-010 |
| Duplicate/pending request | not checked in C# | – | – | UNKNOWN | – | – |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M03-001 | Structure dropdown cascade: choosing a level loads only **active** children of that level and clears all lower levels. | IVMarketStructureFrom_ToZoneTranfer.ascx.cs:223-349 | user control | CONFIRMED |
| BR-M03-002 | Users whose `Session["RoleTypeName"]` is AM/DZSM/NSM have upper levels locked to their own hierarchy (from `sp_HigharchyInfoByEmployeeId`); other roles (Admin, MIO, …) are unrestricted in the Transfer variants. | .ascx.cs:29-66 | user control | CONFIRMED |
| BR-M03-003 | When the hidden fields carry ids (edit pages) the control preloads full (active+inactive) lists (`*_All` SPs) instead of active-only lists. | .ascx.cs:67-193 | user control | CONFIRMED (not used by this page) |
| BR-M03-004 | At least one zone must be ticked before submit. | Zone_Transfer.aspx.cs:90-94 | `Validation()` | CONFIRMED |
| BR-M03-005 | Destination Group is mandatory for a zone transfer. | .cs:99-105 | `Validation()` | CONFIRMED |
| BR-M03-006 | Source zone list = **active** zones of the chosen source Group; source Group mandatory to search. | .cs:199-213 | `btnSearch_Click` | CONFIRMED |
| BR-M03-007 | One SP call per ticked zone, all inside one DB transaction (all-or-nothing per submit); `@Type="Zone"`, `@TRegionId`=zone id, `@TGroupId`=destination group, other T-levels NULL, `@EntryBy`=session UserId. | MarketStructureTransferDAL.cs:375-441; DataAccessManager.cs:404-445,122-170 | DAL | CONFIRMED |
| BR-M03-008 | The From-side ids (`FGroupId`… `FSubTerritoryId`) are sent with every SP call as context (source group; others null because their rows are hidden). | .cs:161-165 | page | CONFIRMED |
| BR-M03-009 | Reported result is the **last** iteration's `isSuccess`; earlier results are discarded (transaction still atomic). | DAL line 416 | DAL | CONFIRMED |
| BR-M03-010 | No check that source ≠ destination, no duplicate/pending-request check, no check that the moved zone is inactive/has open documents in C#. | whole page | – | CONFIRMED (absence) |

## 20. Permission Matrix
- Session keys read: `UserId` (`.cs:179`, unguarded — a null session throws `NullReferenceException`); control reads `RoleTypeName`, `EmpInfoId`, `RoleTypeId` inside try/catch.
- Auth: Forms; master page redirects unauthenticated users to Login. **No `<authorization>` element exists in `Solution.Web/Web.config`** (grep), so access control is menu-visibility only. There is no role check in this page: any logged-in user who knows the URL can submit. Role only changes which Group is preselected/locked (BR-M03-002).

## 21. Status Lifecycle
Page itself sets no status column. Request lifecycle (INFERRED from PAGE-099): *Submitted (pending)* → *Approved* on `MarketStructure_TransferApprove`; no reject transition exists in code. See WF-M03-02.

## 22. Error Handling
Validation errors via `alert()`; success/failure via sweet-alert helpers. DAL exceptions are rethrown and **unhandled** (no try/catch in `btnSave_Click`). Lookup failures in the control are swallowed silently (empty catch). `Application_Error` in Global.asax is empty.

## 23. Audit
`@EntryBy` = session UserId string; date stamping presumably inside the SP (`UNKNOWN`). No log of the moved-from parent other than the F* parameters. No logging code in C#.

## 24. Integration
None detected from this page.

## 25. Reports
None.

## 26. Dependencies
DAL: `MarketStructureTransferDAL`, `CommonDataLoad`, `DoctorDAL`/`DoctorDAL` field unused; JS: select2, pickadate, sweetalert helpers (global). Downstream: PAGE-099 approval; effect on customers/doctors/reports UNKNOWN.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Search with From-Group empty | alert "Please Select Area!"; list stays empty | BR-M03-006 |
| TC-02 | Search with valid Group | checkbox list of active zones of that group | BR-M03-006 |
| TC-03 | Submit with nothing ticked | alert "Please Select at least one Territory !!!"; no DB call | BR-M03-004 |
| TC-04 | Submit with zones ticked, To-Group empty | To-Group highlighted; no DB call | BR-M03-005 |
| TC-05 | Submit 3 zones with valid To-Group | 3 SP calls in one transaction; "Operation successful!" | BR-M03-007 |
| TC-06 | Force SP error on 2nd zone | whole batch rolled back; unhandled error page (no catch) | BR-M03-007 |
| TC-07 | From-Group = To-Group | accepted (no rule) — verify SP outcome | BR-M03-010 |
| TC-08 | Login as NSM | Group locked to own group in both fieldsets | BR-M03-002 |
| TC-09 | Select All then untick one | remaining ids submitted | BR-M03-004 |
| TC-10 | Session expired then Submit | NullReferenceException on `Session["UserId"]` | §20 |

## 28. Reverse Engineering References
`Solution.Web/TransferUI/Zone_Transfer.aspx(.cs)`, `IVMarketStructureFrom_ToZoneTranfer.ascx(.cs)`, `Library.DAL/Transfer_DAL/MarketStructureTransferDAL.cs`, `Library.DAL/DataManager/DataAccessManager.cs`.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Zone move semantics | None existed | zone id sent as `@TRegionId`, dest group as `@TGroupId` | SP body absent | UNKNOWN | §16 |
| Validation | None existed | 2 client-independent server checks | – | PARTIAL | §18 |
| Approval linkage | None existed | Page never references approval; only data shape implies it | UNKNOWN | INFERRED | WF-M03-02 |

## 30. Known Gaps
- GAP-M03-001 (SP bodies absent): re-parenting timing/effect UNKNOWN.
- Wrong user messages ("Territory", "Area") and heading "Territory List" for a zone list (`.cs:92,218`, `.aspx:156`).
- No try/catch around DAL; result of only last item; no From≠To or duplicate check; Reset goes to another page.
- Static shared DAL instance (`private static MarketStructureTransferDAL _CustomerInfoDAL`, `.cs:21`) holds a per-instance connection/transaction in `DataAccessManager` — concurrent submits from different users can share one connection (BR-M03-103).
- Dead code: commented `CheckCustInOrder`, unused `rbType`, `gv_DocumentUpload_PreRender`.
- Copy-pasted class name `TransferUI_Area_Transfer`.

## 31. Change Impact
- Sibling pages with the same SP/DAL pattern: PAGE-095, PAGE-096, PAGE-100, PAGE-101 (verified by grep: 5 DAL methods each call `sp_Update_MarketStructure_Transfer`, MarketStructureTransferDAL.cs:277,345,416,486,555).
- `sp_Get_Zone_All_Active` is also used by every `IVMarketStructure*.ascx` cascade (`GetZone_byGroupId_Active`).
- Changing `sp_Update_MarketStructure_Transfer` affects all five transfer pages and the approval semantics of PAGE-099.
