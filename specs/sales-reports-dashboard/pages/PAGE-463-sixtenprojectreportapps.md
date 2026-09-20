---
page_id: PAGE-463
module: M13
page: SixTenProjectReportApps
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-463 — SixTenProjectReportApps

> **Evidence level:** Tier A - `.aspx`, `.aspx.cs` and the reachable BLL/DAL code were read; SP bodies are NOT in the repository (except where stated), so SP-internal behaviour is `UNKNOWN`. Confidence tags: CONFIRMED / INFERRED / UNKNOWN. Reviewed 2026-09-19.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-463 |
| Module | M13 — Sales Reports, Analytics & Dashboards (INFERRED from folder + file name) |
| Page name | SixTenProjectReportApps |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | (root) |
| Master page | none (stand-alone HTML page) |
| Page class | SixTenProjectReportApps |
| aspx size | 15900 bytes |
| Code-behind LOC | 529 |
| Inline script size | 1890 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**SixTenProjectReportApps** is a stand-alone (no master page) mobile-app web-view page - "Six-Ten" project monitoring: per employee it shows DCP (doctor call plans), DCR (call reports), RX (prescriptions), CVP/CVR counts, customer coverage, GP Sales and Total Sales (Net TP), with four KPI tiles.

## 3. Business Objective
Give sales managers (AM/DZSM/NSM/MIO) a compact monitoring view of field activity versus sales for a date range and approval status. INFERRED from the labels ("Customer Visit Plan (CVP)", "GP Sales (Net TP)", "Total Sales (Net TP)").

## 4. Navigation
No inbound references in the web project (invoked externally, by URL `SixTenProjectReportApps.aspx?EmpInfoId=<id>`, presumably from the Android app in `Solution.Web/APK_File` - INFERRED). Menu: none (no master page).

## 5. Route
`~/SixTenProjectReportApps.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SixTenProjectReportApps.aspx |
| Code-behind | Solution.Web/SixTenProjectReportApps.aspx.cs |
| JS include | https://cdn.jsdelivr.net/npm/jquery@3.7.1/dist/jquery.min.js |
| JS include | https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js |
| JS include | https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js |
| JS include | https://cdn.jsdelivr.net/npm/pickadate@3.6.4/lib/picker.js |
| JS include | https://cdn.jsdelivr.net/npm/pickadate@3.6.4/lib/picker.date.js |
| CSS include | https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css |
| CSS include | https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@6.6.0/css/all.min.css |
| CSS include | https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css |
| CSS include | https://cdn.jsdelivr.net/npm/select2-bootstrap-5-theme@1.3.0/dist/select2-bootstrap-5-theme.min.css |
| CSS include | https://cdn.jsdelivr.net/npm/pickadate@3.6.4/lib/themes/default.css |
| CSS include | https://cdn.jsdelivr.net/npm/pickadate@3.6.4/lib/themes/default.date.css |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | DoctorMonitoringDAL (Library.DAL/Doctor_Monitoring_DAL/DoctorMonitoringDAL.cs) |

## 7. UI Control Inventory
Generated inventory:

| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| GroupSelect | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→GroupSelect_SelectedIndexChanged |
| hfMarket | HiddenField |  | no validator found |  |  |  |  |
| hfSubTeritory | HiddenField |  | no validator found |  |  |  |  |
| hfTeritory | HiddenField |  | no validator found |  |  |  |  |
| hfArea | HiddenField |  | no validator found |  |  |  |  |
| hfZone | HiddenField |  | no validator found |  |  |  |  |
| hfGroupId | HiddenField |  | no validator found |  |  |  |  |
| ZoneSelect | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→ZoneSelect_SelectedIndexChanged |
| AreaSelect | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→AreaSelect_SelectedIndexChanged |
| TeritorySelect | DropDownList |  | no validator found |  |  | see §9 |  |
| FromDate | TextBox | Select Date | no validator found |  |  |  |  |
| ToDate | TextBox | Select Date | no validator found |  |  |  |  |
| ApprovalStatusSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| btnSearchModal | LinkButton |  | no validator found |  |  |  | OnClick→btnSearch_Click |
| lblKpiDCP | Label | — | no validator found |  |  |  |  |
| lblKpiDCR | Label | — | no validator found |  |  |  |  |
| lblKpiRX | Label | — | no validator found |  |  |  |  |
| lblKpiSales | Label | — | no validator found |  |  |  |  |
| lblResultInfo | Label |  | no validator found |  |  |  |  |
| loadGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| hfEmpInfoId | HiddenField |  | no validator found |  |  |  |  |
| hfRoleType | HiddenField |  | no validator found |  |  |  |  |
| (no id) | BoundField | ID | no validator found |  |  |  |  |
| (no id) | TemplateField | Territory | no validator found |  |  |  |  |
| lblTerritory | Label | <%#Eval("Territory")%> | no validator found |  |  |  |  |
| (no id) | BoundField | Name Of the Employee | no validator found |  |  |  |  |
| (no id) | BoundField | User Role | no validator found |  |  |  |  |
| (no id) | TemplateField | Base HQ | no validator found |  |  |  |  |
| hfTerritoryCode | Label | <%#Eval("TerritoryCode")%> | no validator found |  |  |  |  |
| (no id) | TemplateField | DCP | no validator found |  |  |  |  |
| lblDCP | Label | <%# Eval("DCP") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | DCR | no validator found |  |  |  |  |
| lblDCR | Label | <%# Eval("DCR") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | RX | no validator found |  |  |  |  |
| lblRX | Label | <%# Eval("RX") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Customer Visit Plan (CVP) | no validator found |  |  |  |  |
| lblCustomerCoverage | Label | <%# Eval("CustomerCoverage") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Customer Visit Report (CVR) | no validator found |  |  |  |  |
| lblCVRCount | Label | <%# Eval("CVRCount") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Customer Coverage | no validator found |  |  |  |  |
| lnkCustomerCsoveragenn | Label | <%#Eval("CustomerCoverageNew")%> | no validator found |  |  |  |  |
| (no id) | TemplateField | GP Sales (Net TP) | no validator found |  |  |  |  |
| lblGPSales | Label | <%# Eval("GPSales") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Total Sales (Net TP) | no validator found |  |  |  |  |
| lblTSales | Label | <%# Eval("TSalesNetTP") %> | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

**Verified**: GroupSelect/ZoneSelect/AreaSelect/TeritorySelect (cascading, AutoPostBack), FromDate/ToDate (`type=date`), ApprovalStatusSelect, btnSearch(Modal); KPI labels `lblKpiDCP/lblKpiDCR/lblKpiRX/lblKpiSales` (default "-"); grid `loadGridView` columns SL, ID, Territory, Name Of the Employee, User Role, Base HQ, DCP, DCR, RX, CVP, CVR, Customer Coverage, GP Sales (Net TP), Total Sales (Net TP). Hidden fields hfGroupId/hfZone/hfArea/hfTeritory lock scope. No validators.

## 8. Field Mapping
GroupSelect/ZoneSelect/AreaSelect/TeritorySelect -> `GroupId, ZonId, Area, Terr` -> `DoctorMonitoringDAL.GetDoctorVisitMonitoringApprovalList(param="", FromDate, ToDate, ...)` -> `sp_Get_AllDoctorVisitMonitoringApprovalList(@param,@frmDate,@toDate,@GroupId,@ZonId,@AreaId,@TerrId)`. FromDate/ToDate texts are passed as strings.

## 9. Dropdown Mapping
Cascade via `_dataLoad` (CommonDataLoad): `sp_Get_Zone_AllByGroup`, `sp_Get_Zone_All_Active`, `sp_Get_Zone_ForDSM`, `sp_CS_GetArea_ByZoneId_*`, `sp_CS_GetTerritory_ByAreaId_*`, `sp_CS_Group_*`.

## 10. Button Action Mapping
btnSearch_Click -> `LoadData()`; dropdown SelectedIndexChanged handlers refill children.

## 11. Frontend Flow
`Page_Load` first load: `LoadInitialDDL()`, read `EmpInfoId` from QueryString, load employee role, derive scope lists, lock selectors (AM: group/zone/area locked with territory list; DZSM: group+zone locked; NSM: group locked; MIO: all three locked) using `sp_HigharchyInfoByEmployeeId`. Search updates grid and KPI tiles.

## 12. Backend Flow
`LoadData`: SP -> bind -> sum DCP/DCR/RX (int) and TSalesNetTP (decimal) -> tiles `N0`/`N2` (BR-M13-036). `ToIntSafe/ToDecSafe` swallow bad values as 0.

## 13. API Mapping
No `[WebMethod]`, ASMX, WCF or Web API endpoint on this page; all interaction is WebForms postback (ScriptManager/UpdatePanel). CONFIRMED (no `[WebMethod]` in the code-behind).

## 14. Service Mapping
None (DAL called directly from code-behind; `CommonDataLoad` for hierarchy).

## 15. Repository Mapping
DoctorMonitoringDAL.GetDoctorVisitMonitoringApprovalList; CommonDataLoad (hierarchy/zone/area/territory loaders).

## 16. Stored Procedure Mapping
Called SPs: `sp_Get_AllDoctorVisitMonitoringApprovalList`, `sp_HigharchyInfoByEmployeeId`, `sp_EmpGeneralInfoByEmployeeId`, zone/area/territory/group loaders (see generated table below). All bodies NOT in repo.

| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_GetArea_ByZoneId_Active | CommonDataLoad.GetArea_ByZoneId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetArea_ByZoneId_All | CommonDataLoad.GetArea_ByZoneId_All | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetArea_ByZoneId_ForAMOnly | CommonDataLoad.GetArea_ByZoneId_AllForAMOnly | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetTerritory_ByAreaId_Active | CommonDataLoad.GetTerritory_ByAreaId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetTerritory_ByAreaId_All | CommonDataLoad.GetTerritory_ByAreaId_All | body NOT in repository | read-like name (INFERRED) |
| sp_CS_Group_Active | CommonDataLoad.GetGroupInfo_Active | body NOT in repository | write-like name |
| sp_CS_Group_All | CommonDataLoad.GetGroupInfo_All | body NOT in repository | write-like name |
| sp_EmpGeneralInfoByEmployeeId | CommonDataLoad.Get_EmpGeneralInfoByEmployeeId | body NOT in repository | read-like name (INFERRED) |
| sp_Get_AllDoctorVisitMonitoringApprovalList | DoctorMonitoringDAL.GetDoctorVisitMonitoringApprovalList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_AllByGroup | CommonDataLoad.GetZone_byGroupId_All | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_All_Active | CommonDataLoad.GetZone_byGroupId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_ForDSM | CommonDataLoad.GetZone_byGroupId_forDSM | body NOT in repository | read-like name (INFERRED) |
| sp_HigharchyInfoByEmployeeId | CommonDataLoad.GetHigharcyInfoByEmployeeId | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
No inline SQL. Tables UNKNOWN. Read-only.

## 18. Validation Matrix
No validation on dates or scope; if `EmpInfoId` is missing/unknown the `Page_Load` returns silently and the page shows empty selectors.

## 19. Business Rules
| BR id | Rule | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M13-035 | SixTenProjectReportApps (mobile/web-view page, no master page) takes EmpInfoId from the QueryString, loads that employee's RoleTypeId/RoleType via Get_EmpGeneralInfoByEmployeeId and derives the visible hierarchy (AM->AreaId, MIO->TerritoryId, DZSM->RegionId, NSM->GroupId); no Session or login check exists on the page, so the scope is fully caller-controlled. | `Solution.Web/SixTenProjectReportApps.aspx.cs:32` | SixTenProjectReportApps.aspx.cs Page_Load | CONFIRMED |
| BR-M13-036 | KPI tiles are sums over the SP result: lblKpiDCP = SUM(DCP), lblKpiDCR = SUM(DCR), lblKpiRX = SUM(RX), lblKpiSales = SUM(TSalesNetTP); non-numeric/NULL cells count as 0 (ToIntSafe/ToDecSafe; decimals truncated to int for counts). Data = sp_Get_AllDoctorVisitMonitoringApprovalList(@param,@frmDate,@toDate,@GroupId,@ZonId,@AreaId,@TerrId) with @param always "". | `Solution.Web/SixTenProjectReportApps.aspx.cs:467` | SixTenProjectReportApps.aspx.cs LoadData; DoctorMonitoringDAL.GetDoctorVisitMonitoringApprovalList | CONFIRMED |

## 20. Permission Matrix
- **No login/session check.** The page has no master page, takes `EmpInfoId` from the QueryString and derives role and scope from it (BR-M13-035). Web.config has Forms authentication but no `<authorization>` rules, so nothing forces a login (INFERRED).
- Role narrowing is UI-only (disabled dropdowns + hidden fields); the POSTed dropdown values are what the SP receives.

## 21. Status Lifecycle
Read-only. Approval status filter values (`ApprovalStatusSelect`) are passed nowhere in `LoadData` (`param` is always "") - the control has no effect (CONFIRMED).

## 22. Error Handling
No try/catch; `ToIntSafe/ToDecSafe` are tolerant parsers.

## 23. Audit
None.

## 24. Integration
Consumed by the mobile app (INFERRED); no outbound integration.

## 25. Reports
On-screen KPI + grid. See workflow `WF-M13-03` for the scoped-report pattern.

## 26. Dependencies
DoctorMonitoringDAL, CommonDataLoad, master-less HTML page.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Open with EmpInfoId of a DZSM | group+zone locked to DZSM scope | BR-M13-035 |
| TC-02 | Open with someone else's EmpInfoId, no login | data returned (gap) | GAP-M13-005 |
| TC-03 | Search | tiles equal column sums | BR-M13-036 |
| TC-04 | Row with NULL RX | counted as 0 | BR-M13-036 |
| TC-05 | Change ApprovalStatusSelect | no change in result | BR-M13-036 |

## 28. Reverse Engineering References
- `Solution.Web/SixTenProjectReportApps.aspx(.cs)`; `Library.DAL/Doctor_Monitoring_DAL/DoctorMonitoringDAL.cs:15`

## 29. Spec vs Actual
| Area | Specification | Actual code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | see §2-§26 | n/a | MISSING | - |

## 30. Known Gaps
- Identity/scope from QueryString (impersonation, GAP-M13-005).
- Approval-status filter is dead.
- SP body unknown; "Six-Ten" naming/business meaning UNKNOWN.

## 31. Change Impact
- Module M13; SP `sp_Get_AllDoctorVisitMonitoringApprovalList` also serves the doctor-visit approval pages (other module).
