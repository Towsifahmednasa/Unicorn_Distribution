---
page_id: PAGE-125
module: M04
page: PromoMIOTag
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "PARTIAL", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-125 - PromoMIOTag (assign field officers / MIOs to a promo group)

> **Evidence level:** Tier A. Read in full: `PromoAlloc/PromoMIOTag.aspx.cs` (327 lines), `Library.DAL/PromoAllocDAL/PromoMITagDAL.cs` (all inline SQL), `CommonDataLoad.GetMIOTagValidationCheck`, JS regions of `PromoMIOTag.aspx`. SP bodies (`sp_Save_PromoMIOTagMaster`, `sp_Update_PromoMIOTagMaster`, `sp_Save_PromoMIOTagDetail`, `sp_check_Vali_PromoMIOTag`) absent => UNKNOWN.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-125 |
| Module | M04 |
| Page name | PromoMIOTag |
| Kind | Entry / Transaction |
| Web folder / master | PromoAlloc / NewMasterPage |
| Page class | PromoAlloc_PromoMIOTag |
| Files | aspx 19,373 bytes; aspx.cs 327 lines |
| Suspected legacy/duplicate | Page is live (linked from `PromoGroupList.aspx`); it contains a **dead second implementation** (jQuery `Save()` + `[WebMethod] SaveMIOTag`) described in BR-M04-061 |

## 2. Purpose
Select a **promo group** and tick the active MIOs (market information officers = field sales officers) who belong to it. CONFIRMED (labels, grid: Employee Code, Employee Name, Territory Name).

## 3. Business Objective
Promo groups (`tblPromoGroup`, PG-100x codes) carry promotional stock quantities per month (`GroupWisePromoQtyEntry`). Tagging MIOs to a group defines who may receive/allocate that promotional quantity. INFERRED from the downstream page `GroupWisePromoQtyEntry.groupname_SelectedIndexChanged` which loads MIOs via `LoadMIOByGroupId`.

## 4. Navigation
Inbound: `PromoAlloc/PromoGroupList.aspx`. Outbound: after save `successalert(..., 'PromoMIOTag.aspx')`; Reset -> `PromoMIOTag.aspx`. Menu path UNKNOWN.

## 5. Route
`~/PromoAlloc/PromoMIOTag.aspx` (no querystring used by the server code).

## 6. Source Files
View/code-behind; `PromoMITagDAL`; `CommonDataLoad` (`GetMIOTagValidationCheck`); DAO `Library.DAO/PromoAlloc_DAO` (`PromoMIOTagMaster`, `PromoMIOTagDetail`); `Newtonsoft.Json`.

## 7. UI Control Inventory
| Control | Label | Type | Required | Notes |
|---|---|---|---|---|
| groupname | Promo Group | DropDownList AutoPostBack | YES `:210` (tooltip "please fill out this field") | `SELECT * FROM tblPromoGroup` (PromoGroupId / PromoGroupName) |
| id_mastetID | - | HiddenField | - | set by group change from `ds.Rows[0]["MIOTagMasterId"]` |
| gv_List | SL, [chkSelectAll header + chkSelect row], Employee Code, Employee Name, Territory Name (hidden `hfMIOId`, `hfEmpInfoId`) | GridView | at least one ticked (`:243`) | bound in `groupname_SelectedIndexChanged` |
| chkSelectAll / chkSelect | select | CheckBox AutoPostBack | - | each tick runs the exclusivity check (BR-M04-058) |
| btnSave / btnUpdate | Submit / Update | LinkButton | - | both call `btnSave_Click`; `btnUpdate` is `Visible=false` and never enabled by code (`btnSave` is the visible one) |
| Reset | Reset | LinkButton | - | `Unnamed_Click` -> reload page |

## 8. Field Mapping
`groupname` -> `PromoMIOTagMaster.PromoGroupId` -> `@PromoGroupId`; `id_mastetID` -> `MIOTagId` -> `@MIOTagId`; `Session["UserId"]` -> `@EntryBy`; each ticked row -> `PromoMIOTagDetail.MIOId` (`hfMIOId`) and `EmpInfoId` (`hfEmpInfoId`) -> `sp_Save_PromoMIOTagDetail(@MIOTagId, @MIOId, @EmpInfoId)`. Tables (from other repo SQL): `tblPromoMIOTagMaster(MIOTagId, PromoGroupId, EntryBy, EntryDate)`, `tblPromoMIOTagDetail(MIOTagMasterId, MIOId, [EmpInfoId])` (`PromoMITagDAL.cs:43-65,162-196`).

## 9. Dropdown Mapping
`groupname`: `PromoMITagDAL.LoadGroup()` = `SELECT * FROM dbo.tblPromoGroup` (inline, via `ExecuteAllSqlQueryByStoreProcedure`-style helper `DataContainerDataTable(query,"SSIDB")`).

## 10. Button Action Mapping
| Event | Handler | Effect |
|---|---|---|
| groupname change | `groupname_SelectedIndexChanged` `:190` | `LoadMIO(groupId)`; sets `id_mastetID` from the **first row's** `MIOTagMasterId`; binds `gv_List` |
| chkSelect / chkSelectAll | `:98`,`:86` | `CheckCustInOrder(row)` for each newly ticked row |
| btnSave | `btnSave_Click` `:262` | validate; collect ticked rows; `PromoMITagDAL.SaveBonusCampaign(master, list, UserId)` (method name is a copy-paste leftover; it saves promo tags) |

## 11. Frontend Flow
Server-postback grid. The page also carries jQuery code (`GetData`, `Save`, `FinalSave`, `LoadGroup`) that targets a `#dtTble` table that does not exist in the current markup; nothing calls `Save()` from the current buttons => dead (BR-M04-061).

## 12. Backend Flow
`groupname_SelectedIndexChanged` -> `LoadMIO(groupId)` = UNION of (a) **active** MIOs (`tblMIOInfo.IsActive=1`) not yet in any detail row of a tag master for this group (`CheckStatus='False'`, `MIOTagMasterId='0'`) and (b) active MIOs already tagged for the group (`CheckStatus='True'`, real `MIOTagMasterId`), ordered by EmpInfoId, joined to `tblEmpGeneralInfo` (code, name) and `tblTerritory` (territory).
`btnSave_Click`: `Validation()`; collect ticked rows; `MIOTagId = id_mastetID` (0 when empty); `SaveBonusCampaign`: one transaction: if `MIOTagId>0` `sp_Update_PromoMIOTagMaster` else `sp_Save_PromoMIOTagMaster` (returns PK); then for each ticked row `sp_Save_PromoMIOTagDetail(@MIOTagId,@MIOId,@EmpInfoId)`; success => success alert, else "Already Exist!".

## 13. API Mapping
Page methods (all `[WebMethod(EnableSession = true)]`, public static, no authentication check beyond forms auth): `LoadGroup()`, `LoadProduct()` (`tblProduct` where `ProductGroupId IN (2,3)`; also used by `GroupWisePromoQtyEntry` JS), `LoadMIO(string id)`, `SaveMIOTag(PromoMIOTagMaster)`. Only `LoadProduct` is used by another page; the others are legacy JS endpoints.

## 14. Service Mapping
No BLL: DAL used directly.

## 15. Repository Mapping
`PromoMITagDAL`: `LoadGroup`, `LoadProduct`, `LoadMIOTag`, `LoadMIO`, `LoadMIOByGroupId`, `SaveMIOTagMaster`, `SaveMIOTagDetail`, `SaveBonusCampaign`, `DeleteData`.

## 16. Stored Procedure Mapping
| SP | Params | Called from | Body |
|---|---|---|---|
| sp_Save_PromoMIOTagMaster | @MIOTagId, @PromoGroupId, @EntryBy | `SaveBonusCampaign` | UNKNOWN |
| sp_Update_PromoMIOTagMaster | same | `SaveBonusCampaign` | UNKNOWN |
| sp_Save_PromoMIOTagDetail | @MIOTagId, @MIOId, @EmpInfoId | per ticked MIO | UNKNOWN (unknown whether it removes previously saved unticked MIOs) |
| sp_check_Vali_PromoMIOTag | @PromoGroupId, @EmpInfoId, @PageName='PromoMIOTag' | `CheckCustInOrder` | UNKNOWN; returns >=1 row when the employee is "Already Declared in Promo Group!" |

## 17. Database Mapping
| Table | Access | Note |
|---|---|---|
| tblPromoGroup | SELECT | dropdown |
| tblPromoMIOTagMaster | SELECT/INSERT (SP), SELECT (inline) | PK `MIOTagId`, `PromoGroupId` |
| tblPromoMIOTagDetail | SELECT; DELETE by master id (legacy inline `DeleteData`); INSERT (SP / legacy inline) | `MIOTagMasterId`, `MIOId` |
| tblMIOInfo, tblEmpGeneralInfo, tblTerritory | SELECT | MIO list |
| tblProduct | SELECT | `LoadProduct` |

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| groupname | required | `Validation()` `:210` | tooltip | BR-M04-059 |
| gv_List | not empty | `:218` | alert "Table can not be Empty!" | BR-M04-059 |
| ticked rows | >=1 | `:226-247` | alert "Please Select at least one employee !!!" | BR-M04-059 |
| employee already in another group | SP check on tick | `CheckCustInOrder` `:113-146` | faildalert "Already Declared in Promo Group!" (row is unticked) | BR-M04-058 |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M04-058 | Each time an MIO is ticked the page calls `sp_check_Vali_PromoMIOTag(group, employee, 'PromoMIOTag')`; a non-empty result unticks the row with "Already Declared in Promo Group!" - i.e. an MIO may be tagged to only one promo group (the exact scope of the check is inside the SP, UNKNOWN). The check runs **only on tick events**, not on save, and exceptions in it are swallowed (row stays ticked). `chkSelectAll` triggers it row by row in one loop. | PromoMIOTag.aspx.cs:86-146; CommonDataLoad.cs:1603-1621 | `CheckCustInOrder` | CONFIRMED (call) / UNKNOWN (rule detail) |
| BR-M04-059 | Save requires a promo group, a non-empty MIO table and at least one ticked MIO. Only ticked MIOs are sent; unticking a previously tagged MIO and saving relies on the detail SP to remove it (UNKNOWN - `sp_Save_PromoMIOTagDetail` is called only for ticked rows and there is no delete call in the live path). | PromoMIOTag.aspx.cs:204-251,262-320; PromoMITagDAL.cs:68-144 | `Validation`,`SaveBonusCampaign` | CONFIRMED (C#) / UNKNOWN (SP) |
| BR-M04-060 | Only **active** MIOs (`tblMIOInfo.IsActive=1`) are listed; the "available" list excludes MIOs already tagged to *this* group; the group's existing members appear pre-ticked. Promo product list (used by group-wise qty entry) is limited to products with `ProductGroupId IN (2,3)`. | PromoMITagDAL.cs:158-159,167-185 | `LoadMIO`,`LoadProduct` | CONFIRMED |
| BR-M04-061 | The live save path can create a **second master row for the same group**: `id_mastetID` comes from the first grid row (ordered by EmpInfoId); if that row is an untagged MIO its `MIOTagMasterId` is `'0'`, so `MIOTagId=0` and `sp_Save_PromoMIOTagMaster` is called again even though the group already has a master. (The dead WebMethod `SaveMIOTag` avoided this by looking up the master with `LoadMIOTag`, deleting all detail rows and re-inserting.) Effect depends on the SP (UNKNOWN); INFERRED risk. | PromoMIOTag.aspx.cs:190-201,300; PromoMITagDAL.cs:167-184; PromoMIOTag.aspx.cs:162-188 | `groupname_SelectedIndexChanged`,`btnSave_Click` | INFERRED |

## 20. Permission Matrix
Session `UserId` (EntryBy). No role check. The static WebMethods are callable by any authenticated (forms) client. Roles UNKNOWN.

## 21. Status Lifecycle
No status. Membership is present/absent in the detail table; MIO `IsActive` filters the list.

## 22. Error Handling
Tick-check errors swallowed; save errors re-thrown by the DAL after rollback; no page try/catch => error page.

## 23. Audit
`EntryBy`, `EntryDate` on the master (legacy inline insert); SP path passes `@EntryBy`. No detail audit.

## 24. Integration
None.

## 25. Reports
None.

## 26. Dependencies
DAL `PromoMITagDAL`, `CommonDataLoad`; DAO PromoAlloc_DAO; Newtonsoft.Json; ExecuteAllSql helper (via `ClsCommonInternalDAL.DataContainerDataTable`).

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Select group with no members, tick 3 MIOs, save | master + 3 detail rows | BR-M04-059 |
| TC-02 | Tick an MIO tagged to another group | Unticked with "Already Declared in Promo Group!" | BR-M04-058 |
| TC-03 | Save with none ticked | "Please Select at least one employee !!!" | BR-M04-059 |
| TC-04 | Open existing group whose first-ordered MIO is untagged, add one MIO, save | Possible second master (check DB) | BR-M04-061 |
| TC-05 | Untick a member and save | Member removal depends on SP (verify) | BR-M04-059 |
| TC-06 | Inactive MIO | Not listed | BR-M04-060 |
| TC-07 | POST `PromoMIOTag.aspx/LoadMIO` with `id` containing `'` | SQL error / injection (string concatenation) | GAP-M04-026 |

## 28. Reverse Engineering References
`Solution.Web/PromoAlloc/PromoMIOTag.aspx(.cs)`, `Library.DAL/PromoAllocDAL/PromoMITagDAL.cs`, `Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1603`.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | read | SP bodies absent | MISSING | - |
| Save path | None existed | two implementations, one dead | UNKNOWN | DUPLICATED | BR-M04-061 |

## 30. Known Gaps
Inline SQL concatenating request values (`LoadMIO`, `LoadMIOTag`, `DeleteData`, `SaveMIOTagDetail`) (GAP-M04-026). Dead WebMethod path. Possible duplicate master. Exclusivity rule only on tick. Misnamed DAL method.

## 31. Change Impact
`tblPromoMIOTagMaster/Detail` are read by `GroupWisePromoQtyEntry` (PAGE-120 via `LoadMIOByGroupId`) and validated by `sp_check_Vali_PromoMIOTag` (also referenced, commented out, in PAGE-120). Public `LoadProduct` WebMethod is called by `GroupWisePromoQtyEntry.aspx` JS.
