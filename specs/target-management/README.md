# M14 - Target & DWSP Management

Reviewed 2026-09-19 (reverse-engineered; no prior spec existed). Evidence tags: CONFIRMED / INFERRED / UNKNOWN. Stored-procedure bodies are not in the repository (except where stated), so column-level behaviour of SPs is UNKNOWN.

## 1. Purpose and business overview

The module lets the company plan **sales targets top-down** and then break them into a **day-wise sales plan (DWSP)**:

1. National target per Group (business group) and month - `DWSP/NationalTargetSetup.aspx`
2. Zone (DZSM region) split of the national target - `DWSP/ZoneWiseTargetSetup.aspx`
3. Area (AM) split of the zone target - `DWSP/AreaWiseTargetSetup.aspx`
4. Territory (MIO territory) split of the area target - `DWSP/TerritoryWiseTargetSetup.aspx` (menu version) and `TerritoryWiseTargetSetupApps.aspx` (stand-alone, `?EmpId=`)
5. Day-by-day plan (General / FCB / Campaign amount per calendar day) for a territory - `DWSP/AMDayWiseDWSPSetup.aspx` (stand-alone, `?EmpId=`)
6. Alternative bulk path: monthly **territory target Excel upload** - `SInventory_UI/TargetExcelUpload*.aspx`
7. Legacy/other target stores: MIA (MIO representative) target, monthly company target, product-wise target (`SInventory_UI/MiaTarget*.aspx`, `Target_UI/MonthlyTarget*.aspx`, `Target_UI/ProductWiseTarget*.aspx`).
8. Reporting: DWSP monthly grid (PAGE-548) and target-achievement reports (Zone / Area / Territory: Target vs Order vs Invoice vs Sales with Ach%) (PAGE-549/550).

The cascade is *advisory*: each level reads its parent amount, warns while the running total of the split exceeds it, but the server does not enforce the sum (BR-M14-001, BR-M14-006).

## 2. Page catalogue (24 pages)

| Page ID | Page | Kind | Tier | Spec |
|---|---|---|---|---|
| PAGE-538 | DWSP/AMDayWiseDWSPSetup | Entry (stand-alone, `?EmpId`) | A | [spec](pages/PAGE-538-amdaywisedwspsetup.md) |
| PAGE-539 | DWSP/AreaWiseTargetSetup | Entry | A | [spec](pages/PAGE-539-areawisetargetsetup.md) |
| PAGE-540 | DWSP/AreaWiseTargetSetupView | List + inline update | B | [spec](pages/PAGE-540-areawisetargetsetupview.md) |
| PAGE-541 | DWSP/NationalTargetSetup | Entry | A | [spec](pages/PAGE-541-nationaltargetsetup.md) |
| PAGE-542 | DWSP/NationalTargetSetupView | List + inline update | B | [spec](pages/PAGE-542-nationaltargetsetupview.md) |
| PAGE-543 | DWSP/TerritoryTargetSetupView | List + inline update | B | [spec](pages/PAGE-543-territorytargetsetupview.md) |
| PAGE-544 | DWSP/TerritoryWiseTargetSetup | Entry | B | [spec](pages/PAGE-544-territorywisetargetsetup.md) |
| PAGE-545 | DWSP/TerritoryWiseTargetSetupApps | Entry (stand-alone, `?EmpId`) | A | [spec](pages/PAGE-545-territorywisetargetsetupapps.md) |
| PAGE-546 | DWSP/ZoneWiseTargetSetup | Entry | A | [spec](pages/PAGE-546-zonewisetargetsetup.md) |
| PAGE-547 | DWSP/ZoneWiseTargetSetupView | List + inline update | B | [spec](pages/PAGE-547-zonewisetargetsetupview.md) |
| PAGE-548 | Reports_UI/DWSPMonthlyRpt | Report | A | [spec](pages/PAGE-548-dwspmonthlyrpt.md) |
| PAGE-549 | Reports_UI/TargetAChivementReport | Report (Month/Date, Zone/Area/Territory) | B | [spec](pages/PAGE-549-targetachivementreport.md) |
| PAGE-550 | Reports_UI/TargetAChivementReportNew | Report (Zone/Area/Territory radio) | B | [spec](pages/PAGE-550-targetachivementreportnew.md) |
| PAGE-551 | SInventory_UI/MiaTargetEdit | Edit popup | B | [spec](pages/PAGE-551-miatargetedit.md) |
| PAGE-552 | SInventory_UI/MiaTargetEntry | Entry | B | [spec](pages/PAGE-552-miatargetentry.md) |
| PAGE-553 | SInventory_UI/MiaTargetView | List | B | [spec](pages/PAGE-553-miatargetview.md) |
| PAGE-554 | SInventory_UI/TargetExcelUpload | Excel import | A | [spec](pages/PAGE-554-targetexcelupload.md) |
| PAGE-555 | SInventory_UI/TargetExcelUploadEdit | Edit | B | [spec](pages/PAGE-555-targetexceluploadedit.md) |
| PAGE-556 | SInventory_UI/TargetExcelUploadList | List + delete | B | [spec](pages/PAGE-556-targetexceluploadlist.md) |
| PAGE-557 | Target_UI/CommonDataLoad | JSON lookup endpoints (no UI) | A | [spec](pages/PAGE-557-commondataload.md) |
| PAGE-558 | Target_UI/MonthlyTarget | Entry (PageMethods) | B | [spec](pages/PAGE-558-monthlytarget.md) |
| PAGE-559 | Target_UI/MonthlyTargetView | List (PageMethods) | B | [spec](pages/PAGE-559-monthlytargetview.md) |
| PAGE-560 | Target_UI/ProductWiseTarget | Entry (PageMethods) | B | [spec](pages/PAGE-560-productwisetarget.md) |
| PAGE-561 | Target_UI/ProductWiseTargetList | List (PageMethods) | B | [spec](pages/PAGE-561-productwisetargetlist.md) |

Tier-A choice: the five candidates were the top-scoring pages; PAGE-557 (a pure lookup endpoint) was kept because it is the only page with an auth-relevant finding, and PAGE-541, PAGE-546 and PAGE-554 were swapped in as the write/transaction pages missing from the list (5 + 3 = 8 = list size + 3).

## 3. Business-rule index

| ID | One-liner | Where |
|---|---|---|
| BR-M14-001 | Target cascade National -> Zone -> Area -> Territory -> DWSP; each level reads the parent amount from an SP | 541, 546, 539, 545, 538 |
| BR-M14-002 | Year 2015-2050; month list = 12 rolling months from now (Text=name, Value=number) | all DWSP pages, 548 |
| BR-M14-003 | Month stored as name text (except DWSP/report use number) | 539/541/545/546, 538 |
| BR-M14-004 | National target: Year, Month, Group, Amount required; silent no-op without session | 541 |
| BR-M14-005 | National save: id 0 -> insert (pk>0 else "Already Exist!"), id>0 -> update | 541 |
| BR-M14-006 | Running-sum cap `<=` parent (message says "equal"), not enforced on Save | 546, 539, 545 |
| BR-M14-007 | Zone save validation (all fields, non-empty rows) | 546 |
| BR-M14-008 | Zone save: success if any row pk>0 | 546 |
| BR-M14-009 | Only active zones of the group listed (SP) | 546 |
| BR-M14-010 | Area save via `sp_Save_AreaWiseTargetSetup`, one row per area, Session UserId | 539 |
| BR-M14-011 | DZSM confined to own Group/Zone (client-state only) | 539 |
| BR-M14-012 | All DAL saves are one transaction, rollback on exception; "Already Exist!" unreachable for SaveData | DAL infra |
| BR-M14-013 | Employee context from `?EmpId` (roles 2=AM, 3=DZSM) | 545, 538 |
| BR-M14-014 | Territory save; EntryBy = employee UserId field which is empty on postback | 545 |
| BR-M14-015 | DWSP validation: year, month, territory, rows, non-zero grand total | 538 |
| BR-M14-016 | DWSP total is not reconciled to territory target | 538 |
| BR-M14-017 | DWSP save payload per day, no EntryBy, EmpId from URL | 538 |
| BR-M14-018 | Month day list from SPs; past days editable | 538 |
| BR-M14-019 | DWSP report from `sp_Process_DWSPReport_Territory`; blank approval = `Select` | 548 |
| BR-M14-020 | DZSM must choose a zone to run the DWSP report | 548 |
| BR-M14-021 | Excel target row validation: fields, employee mapped, no duplicate month | 554 |
| BR-M14-022 | Excel save per row (no txn), success = last row id > 0 | 554 |
| BR-M14-023 | "Employee mapped" check calls employee-code SP with the territory code | 554 |
| BR-M14-024 | Target_UI lookup endpoints have no authentication check | 557 |
| BR-M14-025 | Monthly company target: one row per Year+Month (insert silently ignored if present); update only if no other row has same Year+Month | Target_UI/MonthlyTarget (B) |
| BR-M14-026 | Achievement report (New): Zone/Area/Territory type radio; footers sum Target/Order/Invoice/Sales with `Math.Round` and `#,##0` | 550 (B) |
| BR-M14-027 | MIA target: MiaCode and amount required; year = current year on create; edit does not carry Year (writes NULL) | 551/552 (B) |
| BR-M14-028 | Delete of an Excel-loaded target via `sp_DEL_TargetInfo @SL` | 556 (B) |

Sources for 025-028: `Library.DAL/TargetDAL/TargetDAL.cs:16-60`, `Solution.Web/Target_UI/MonthlyTarget.aspx.cs:12-37`; `Solution.Web/Reports_UI/TargetAChivementReportNew.aspx.cs:174-240`; `Solution.Web/SInventory_UI/MiaTargetEntry.aspx.cs:45-70`, `MiaTargetEdit.aspx.cs:16-30`, `Library.DAL/SInventory_DAL/MiaTargetDAL.cs:62-73`; `Solution.Web/SInventory_UI/TargetExcelUploadList.aspx.cs:70-90`, `Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs:1432`. All CONFIRMED (Tier-B pages, rules read from code but page specs not rewritten).

## 4. Status lifecycles

No status/approval flags are set by any M14 page (CONFIRMED). Data states are only "row exists for (level, year, month, entity)". Inline **UpdateData** on the four `*View` pages re-saves an existing row (id > 0 -> update path in the same SP). The DWSP monthly report has an *Approval Status* filter (values from `SeedDataDAL.GetApprovalStatusList`) that is produced elsewhere (UNKNOWN who sets it; no M14 code writes it).

## 5. Roles and permissions observed

| Page group | Session keys used | Enforcement |
|---|---|---|
| National / Zone entry | `UserId` | none beyond master-page login + DB menu |
| Area / Territory entry, views | `RoleTypeName`, `EmpInfoId`, `RoleTypeId` (3 = DZSM, 2 = AM, 5 = higher level) | UI scoping only (locked drop-downs) |
| DWSP Apps pages (538, 545) | none | **no login check at all**; identity = `?EmpId` |
| Reports 548/549/550 | `RoleTypeName`, `EmpInfoId`, `RoleTypeId` | DZSM must pick zone; user-control scoping |
| Excel upload | `LoginName` | none |
| Target_UI/CommonDataLoad | none | none |

## 6. Data model (what is provable)

| Object | Read/Write | Evidence |
|---|---|---|
| `sp_Save_NationaltargetSetup`, `sp_Get_NationalTargetAmount`, `sp_Get_Group_List`, `sp_NTS_Group_Active` | national level | `NationalTargetSetupDal.cs`, `ZoneWiseTargetSetupDal.cs:208` |
| `sp_Save_ZoneWiseTargetSetup`, `sp_Get_ZoneTargetAmount`, `sp_Get_ZoneWiseTargetList`, `sp_Get_Zone_All_Active_ZoneTarget` | zone level | `ZoneWiseTargetSetupDal.cs` |
| `sp_Save_AreaWiseTargetSetup`, `sp_Get_AreaTargetAmount`, `sp_Get_AreaWiseTargetList` | area level | `AreaWiseTargetSetupDal.cs`, `ZoneWiseTargetSetupDal.cs:187` |
| `sp_Save_TerritoryWiseTargetSetup`, `sp_Get_TerritoryWiseTargetList`, `sp_Get_TerritoryWiseTargetSetup` | territory level | `AreaWiseTargetSetupDal.cs` |
| `sp_Save_DWSPMaster`, `sp_Process_DWSPReport_Territory` | DWSP write/report | `AreaWiseTargetSetupDal.cs:101`, `CmnCrystaltoView.cs:1586` |
| `sp_Get_TTargetAChivementReport`, `sp_Get_TTargetAChivementReport_nnn` | achievement reports | `Setup2DAL.cs:131,156` |
| `usp_InsertTerritoryData`, `sp_Get_TerritoryTargetList`, `sp_DEL_TargetInfo`, `sp_Get_Chk_EmpCode`, `sp_Get_Chk_EMPDataExistCheck` | Excel upload | `CompanywisebranchDal.cs:79`, `BonusCampaignNewDAL.cs:46,1441`, `SAP_IntrigationPointDAL.cs` |
| tables `tblMonthlyTarget`, `tblProductWiseSalesTarget`, `tblMIATarget` (inline SQL) | legacy target stores | `TargetDAL.cs`, `ProuctWiseSalesDAL.cs`, `MiaTargetDAL.cs` |
| helper SPs: `sp_Get_RoleTypeByEmpId`, `sp_Get_ZoneinfoByEmpId`, `sp_Get_DZSMinfoByEmpId`, `sp_HigharchyInfoByEmployeeId`, `sp_Get_FirstDate_LastDatebyYearMonth`, `sp_GetDatewithinDateRange`, `sp_Get_TerritoryAll_ByAreaId*`, `sp_Get_Zone_All_Active_ByGroup`, `sp_Get_Zone_ForDSM` | hierarchy lookups | `CommonDal.cs`, `CommonDataLoad.cs` |

Table/column definitions behind the SPs: UNKNOWN.

## 7. Integrations

- Stand-alone `?EmpId=` pages (538, 545) are very probably opened from the field-force mobile app (INFERRED) - see `../integrations/INT-09-mobile-app-shared-database.md`.
- Excel imports - `../integrations/INT-13-excel-file-imports.md`.
- JSON page-method endpoints - `../integrations/INT-12-page-method-json-endpoints.md`.
- No SAP or REST call originates in M14.

## 8. Reports

See [reports/](reports/): RPT-M14-01 DWSP monthly report, RPT-M14-02 target achievement reports. No Crystal `.rpt` files are used by this module.

## 9. Workflows

- [WF-M14-01 target cascade setup](workflows/WF-M14-01-target-cascade-setup.md)
- [WF-M14-02 DWSP day-wise plan and monthly report](workflows/WF-M14-02-dwsp-day-wise-plan.md)
- [WF-M14-03 territory target Excel upload](workflows/WF-M14-03-territory-target-excel-upload.md)

## 10. Known gaps (summary; details in [GAPS-M14.md](GAPS-M14.md))

Unauthenticated `?EmpId` pages; blank `@EntryBy` on territory save; cascade not enforced on save; non-atomic Excel import and unrestricted upload file name; dead edit mode; broken delete in Target_UI list pages; MIA edit wipes Year; SQL string concatenation in legacy target DALs and view-page filters; duplicate `CommonDataLoad` page; all SP bodies missing.

## 11. Still unverified

- Every stored-procedure body (except none in this module): uniqueness rules, approval statuses, tables/columns, report formulas (Ach%).
- The four `*View` pages and `TerritoryWiseTargetSetup` (PAGE-544) are Tier B; inline-update behaviour was only skimmed (row-command `UpdateData` re-saves via the same DAL).
- `IVMarketStructureMarket.ascx` role scoping (504 lines) reviewed only for calls.
- Menu placement/roles (DB-driven); whether the Apps pages are truly hosted in the mobile app.
- PageMethods of Target_UI Monthly/ProductWise pages (client JS) were not read.
