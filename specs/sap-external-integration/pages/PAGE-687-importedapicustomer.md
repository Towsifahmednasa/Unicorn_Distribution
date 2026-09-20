---
page_id: PAGE-687
module: M17
page: ImportedApiCustomer
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-687 — ImportedApiCustomer

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-687 |
| Module | M17 — SAP, Mobile-App & External Integration (INFERRED from folder + file name) |
| Page name | ImportedApiCustomer |
| Kind | Upload / Import (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | none (stand-alone HTML page) |
| Page class | SInventory_UI_ImportedApiCustomer |
| aspx size | 21071 bytes |
| Code-behind LOC | 418 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **ImportedApiCustomer** (Upload / Import). Page title/heading text: `Edit`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: SAP hand-off pages, MIGO, provider dropout integration, API-customer import, mobile-app data-load web methods.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/ApiCustomerList.aspx`

Outbound references: none found

## 5. Route
`~/SInventory_UI/ImportedApiCustomer.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/ImportedApiCustomer.aspx |
| Code-behind | Solution.Web/SInventory_UI/ImportedApiCustomer.aspx.cs |
| CSS include | ../css/custom.css |
| CSS include | ../css/style.css |
| CSS include | ../css/colors/blue.css |
| BLL class | ImportedApiCustomerBll (Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs) |
| DAL class | ImportedApiCustomerDal (Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs) |
| DAO/entity class | CustomerMaster (Library.BLL/SInventory_BLL/CustomerMaster.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| custMastIdHiddenField | HiddenField |  | no validator found |  |  |  |  |
| customerCodeTextBox | TextBox |  | no validator found |  | ReadOnly=true |  |  |
| customernameTextBox | TextBox |  | no validator found |  |  |  |  |
| addressTextBox | TextBox |  | no validator found |  |  |  |  |
| addressTextBox2 | TextBox |  | no validator found |  |  |  |  |
| contactTextBox | TextBox |  | no validator found |  |  |  |  |
| cityTextBox | TextBox |  | no validator found |  |  |  |  |
| contactPersonTextBox | TextBox |  | no validator found |  |  |  |  |
| shippingConditionTextBox | TextBox |  | no validator found |  | ReadOnly=true |  |  |
| comUnitNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| regionNameDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→regionNameDropDownList_SelectedIndexChanged |
| dzsmNameTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| districtNameDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→districtNameDropDownList_SelectedIndexChanged |
| feNameTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| areaNameDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→areaNameDropDownList_SelectedIndexChanged |
| territoryNameTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| miaNameDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→miaNameDropDownList_SelectedIndexChanged |
| mioNameTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| marketNameDropDownList | DropDownList |  | no validator found |  |  | see §9 | OnTextChanged→marketNameDropDownList_OnTextChanged |
| marketNameTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| categoryNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| paymentTypeDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| fbDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| updateButton | Button | Update | no validator found |  |  |  | onclick→updateButton_Click |
| closeButton | Button | Close | no validator found |  |  |  | onclick→closeButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| areaNameDropDownList | TeritoryDataTable | SInventory_UI_ImportedApiCustomer.ApiCustomerInfoLoad | ImportedApiCustomerBll.CustomerMasterEditLoad, ImportedApiCustomerBll.LoadMarketName, ImportedApiCustomerBll.LoadMiaName | ImportedApiCustomerDal.CustomerMasterEditLoad, ImportedApiCustomerDal.GetMarketNameById, ImportedApiCustomerDal.GetMiaNa | UNKNOWN | UNKNOWN | UNKNOWN |
| miaNameDropDownList | MiaDataTable | SInventory_UI_ImportedApiCustomer.ApiCustomerInfoLoad | ImportedApiCustomerBll.CustomerMasterEditLoad, ImportedApiCustomerBll.LoadMarketName, ImportedApiCustomerBll.LoadMiaName | ImportedApiCustomerDal.CustomerMasterEditLoad, ImportedApiCustomerDal.GetMarketNameById, ImportedApiCustomerDal.GetMiaNa | UNKNOWN | UNKNOWN | UNKNOWN |
| marketNameDropDownList | marketDataTable | SInventory_UI_ImportedApiCustomer.ApiCustomerInfoLoad | ImportedApiCustomerBll.CustomerMasterEditLoad, ImportedApiCustomerBll.LoadMarketName, ImportedApiCustomerBll.LoadMiaName | ImportedApiCustomerDal.CustomerMasterEditLoad, ImportedApiCustomerDal.GetMarketNameById, ImportedApiCustomerDal.GetMiaNa | UNKNOWN | UNKNOWN | UNKNOWN |
| regionNameDropDownList | aDataTable | SInventory_UI_ImportedApiCustomer.regionNameDropDownList_SelectedIndexChanged | ImportedApiCustomerBll.LoadDZSMName | ImportedApiCustomerDal.GetDZSMnameById | UNKNOWN | UNKNOWN | UNKNOWN |
| areaNameDropDownList | aDataTable | SInventory_UI_ImportedApiCustomer.areaNameDropDownList_SelectedIndexChanged | ImportedApiCustomerBll.LoadTeritoryName | ImportedApiCustomerDal.GetTeritorynameById | UNKNOWN | UNKNOWN | UNKNOWN |
| districtNameDropDownList | aDataTable | SInventory_UI_ImportedApiCustomer.districtNameDropDownList_SelectedIndexChanged | ImportedApiCustomerBll.LoadFEName | ImportedApiCustomerDal.GetFEnameById | UNKNOWN | UNKNOWN | UNKNOWN |
| miaNameDropDownList | aDataTable | SInventory_UI_ImportedApiCustomer.miaNameDropDownList_SelectedIndexChanged | ImportedApiCustomerBll.LoadMiaName | ImportedApiCustomerDal.GetMiaNameById | UNKNOWN | UNKNOWN | UNKNOWN |
| marketNameDropDownList | aDataTable | SInventory_UI_ImportedApiCustomer.marketNameDropDownList_OnTextChanged | ImportedApiCustomerBll.LoadMarketName | ImportedApiCustomerDal.GetMarketNameById | UNKNOWN | UNKNOWN | UNKNOWN |
| comUnitNameDropDownList | CUnit | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | UNKNOWN | UNKNOWN | UNKNOWN |
| custMastIdHiddenField | CustomerMaster.CustomerMasterId | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | @ApiCustomerId | UNKNOWN (SP body not in repo) | UNKNOWN |
| customerCodeTextBox | CustomerMaster.CustomerCode | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | UNKNOWN | UNKNOWN | UNKNOWN |
| customernameTextBox | CustomerMaster.CustomerName | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | @CustomerName | UNKNOWN (SP body not in repo) | UNKNOWN |
| addressTextBox | CustomerMaster.Address | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | @Address | UNKNOWN (SP body not in repo) | UNKNOWN |
| addressTextBox2 | CustomerMaster.Addrees2 | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | @Addrees2 | UNKNOWN (SP body not in repo) | UNKNOWN |
| contactTextBox | CustomerMaster.CellNo | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | @CellNo | UNKNOWN (SP body not in repo) | UNKNOWN |
| cityTextBox | CustomerMaster.City | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | @City | UNKNOWN (SP body not in repo) | UNKNOWN |
| contactPersonTextBox | CustomerMaster.ConPerson | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | @ConPerson | UNKNOWN (SP body not in repo) | UNKNOWN |
| marketNameDropDownList | CustomerMaster.MarketCode | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | @MarketCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| marketNameTextBox | CustomerMaster.MarketName | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | @MarketName | UNKNOWN (SP body not in repo) | UNKNOWN |
| miaNameDropDownList | CustomerMaster.MIACode | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | @MIACode | UNKNOWN (SP body not in repo) | UNKNOWN |
| mioNameTextBox | CustomerMaster.MiaName | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | @MiaName | UNKNOWN (SP body not in repo) | UNKNOWN |
| areaNameDropDownList | CustomerMaster.AreaCode | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | @AreaCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| districtNameDropDownList | CustomerMaster.DisCode | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | @DisCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| feNameTextBox | CustomerMaster.FEName | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | @FEName | UNKNOWN (SP body not in repo) | UNKNOWN |
| comUnitNameDropDownList | CustomerMaster.ComUnitCode | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | @ComUnitCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| regionNameDropDownList | CustomerMaster.RegionCode | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | @RegionCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| dzsmNameTextBox | CustomerMaster.DZSMName | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | @DZSMName | UNKNOWN (SP body not in repo) | UNKNOWN |
| paymentTypeDropDownList | CustomerMaster.TermOfPayment | SInventory_UI_ImportedApiCustomer.updateButton_Click | ImportedApiCustomerBll.UpdateApiCustomerInfo | ImportedApiCustomerDal.UpdateApiCustomerInformation | @TermOfPayment | UNKNOWN (SP body not in repo) | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| comUnitNameDropDownList | LoadDropDownList → ImportedApiCustomerBll.LoadDistributionCenterName | inline SQL | ComUnitName/ComUnitCode | tblCompanyUnit |
| regionNameDropDownList | LoadDropDownList → ImportedApiCustomerBll.LoadDZSMInfo | inline SQL | RegionName/RegionCode | tblRegion |
| districtNameDropDownList | LoadDropDownList → ImportedApiCustomerBll.LoadFEInfo | inline SQL | DistrictName/DistrictCode | tblDistrict |
| areaNameDropDownList | LoadDropDownList → ImportedApiCustomerBll.LoadTerritoryInfo | inline SQL | AreaName/AreaCode | tblArea |
| miaNameDropDownList | LoadDropDownList → ImportedApiCustomerBll.LoadMiaInfo | inline SQL | MiaName/MiaCode | tblMIAInfo |
| marketNameDropDownList | LoadDropDownList → ImportedApiCustomerBll.LoadMaketInfo | inline SQL | MarketName/MarketCode | tblMarket |
| categoryNameDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| paymentTypeDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| fbDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: none
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_ImportedApiCustomer.Page_Load | Solution.Web/SInventory_UI/ImportedApiCustomer.aspx.cs:16 | object sender, EventArgs e | SInventory_UI_ImportedApiCustomer.Page_Load → ImportedApiCustomerBll.CustomerMasterEditLoad / ImportedApiCustomerBll.LoadDZSMInfo / ImportedApiCustomerBll.LoadDistributionCenterName / ImportedApiCustomerBll.LoadFEInfo / ImportedApiCustomerBll.Loa → ImportedApi | no | no |
| SInventory_UI_ImportedApiCustomer.LoadDropDownList | Solution.Web/SInventory_UI/ImportedApiCustomer.aspx.cs:26 |  | SInventory_UI_ImportedApiCustomer.LoadDropDownList → ImportedApiCustomerBll.LoadDZSMInfo / ImportedApiCustomerBll.LoadDistributionCenterName / ImportedApiCustomerBll.LoadFEInfo / ImportedApiCustomerBll.LoadMaketInfo / ImportedApiCustomerBll.LoadMiaInfo  → Impo | no | no |
| SInventory_UI_ImportedApiCustomer.ApiCustomerInfoLoad | Solution.Web/SInventory_UI/ImportedApiCustomer.aspx.cs:37 | string customerId | SInventory_UI_ImportedApiCustomer.ApiCustomerInfoLoad → ImportedApiCustomerBll.CustomerMasterEditLoad / ImportedApiCustomerBll.LoadMarketName / ImportedApiCustomerBll.LoadMiaName / ImportedApiCustomerBll.LoadTeritoryName → ImportedApiCustomerDal.CustomerMaster | no | no |
| SInventory_UI_ImportedApiCustomer.closeButton_Click | Solution.Web/SInventory_UI/ImportedApiCustomer.aspx.cs:184 | object sender, EventArgs e | SInventory_UI_ImportedApiCustomer.closeButton_Click | no | no |
| SInventory_UI_ImportedApiCustomer.regionNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/ImportedApiCustomer.aspx.cs:190 | object sender, EventArgs e | SInventory_UI_ImportedApiCustomer.regionNameDropDownList_SelectedIndexChanged → ImportedApiCustomerBll.LoadDZSMName → ImportedApiCustomerDal.GetDZSMnameById | no | no |
| SInventory_UI_ImportedApiCustomer.areaNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/ImportedApiCustomer.aspx.cs:201 | object sender, EventArgs e | SInventory_UI_ImportedApiCustomer.areaNameDropDownList_SelectedIndexChanged → ImportedApiCustomerBll.LoadTeritoryName → ImportedApiCustomerDal.GetTeritorynameById | no | no |
| SInventory_UI_ImportedApiCustomer.districtNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/ImportedApiCustomer.aspx.cs:211 | object sender, EventArgs e | SInventory_UI_ImportedApiCustomer.districtNameDropDownList_SelectedIndexChanged → ImportedApiCustomerBll.LoadFEName → ImportedApiCustomerDal.GetFEnameById | no | no |
| SInventory_UI_ImportedApiCustomer.miaNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/ImportedApiCustomer.aspx.cs:222 | object sender, EventArgs e | SInventory_UI_ImportedApiCustomer.miaNameDropDownList_SelectedIndexChanged → ImportedApiCustomerBll.LoadMiaName → ImportedApiCustomerDal.GetMiaNameById | no | no |
| SInventory_UI_ImportedApiCustomer.marketNameDropDownList_OnTextChanged | Solution.Web/SInventory_UI/ImportedApiCustomer.aspx.cs:233 | object sender, EventArgs e | SInventory_UI_ImportedApiCustomer.marketNameDropDownList_OnTextChanged → ImportedApiCustomerBll.LoadMarketName → ImportedApiCustomerDal.GetMarketNameById | no | no |
| SInventory_UI_ImportedApiCustomer.updateButton_Click | Solution.Web/SInventory_UI/ImportedApiCustomer.aspx.cs:371 | object sender, EventArgs e | SInventory_UI_ImportedApiCustomer.updateButton_Click → ImportedApiCustomerBll.UpdateApiCustomerInfo → ImportedApiCustomerDal.UpdateApiCustomerInformation → SP: sp_UD_ApiCustomerMaster | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| ImportedApiCustomerBll.CustomerMasterEditLoad | Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs:60 |
| ImportedApiCustomerBll.LoadTeritoryName | Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs:75 |
| ImportedApiCustomerBll.LoadMiaName | Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs:80 |
| ImportedApiCustomerBll.LoadMarketName | Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs:85 |
| ImportedApiCustomerBll.LoadDistributionCenterName | Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs:30 |
| ImportedApiCustomerBll.LoadDZSMInfo | Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs:35 |
| ImportedApiCustomerBll.LoadFEInfo | Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs:40 |
| ImportedApiCustomerBll.LoadTerritoryInfo | Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs:45 |
| ImportedApiCustomerBll.LoadMiaInfo | Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs:50 |
| ImportedApiCustomerBll.LoadMaketInfo | Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs:55 |
| ImportedApiCustomerBll.LoadDZSMName | Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs:65 |
| ImportedApiCustomerBll.LoadFEName | Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs:70 |
| ImportedApiCustomerBll.UpdateApiCustomerInfo | Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs:90 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| ImportedApiCustomerDal.CustomerMasterEditLoad | Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs:79 | inline SQL: tbltempCustMaster | @TempCustomerMasterId |
| ImportedApiCustomerDal.GetTeritorynameById | Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs:137 | inline SQL: tblArea | @AreaCode |
| ImportedApiCustomerDal.GetMiaNameById | Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs:146 | inline SQL: tblMIAInfo | @MiaCode |
| ImportedApiCustomerDal.GetMarketNameById | Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs:155 | inline SQL: tblMarket | @MarketCode |
| ImportedApiCustomerDal.LoadCompanyUnit | Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs:28 | inline SQL: tblCompanyUnit |  |
| ImportedApiCustomerDal.GetDZSMname | Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs:35 | inline SQL: tblRegion |  |
| ImportedApiCustomerDal.GetFEInfo | Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs:49 | inline SQL: tblDistrict |  |
| ImportedApiCustomerDal.GetTerritoryInfo | Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs:56 | inline SQL: tblArea |  |
| ImportedApiCustomerDal.GetMiaInfo | Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs:63 | inline SQL: tblMIAInfo |  |
| ImportedApiCustomerDal.GetMaketInfo | Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs:70 | inline SQL: tblMarket |  |
| ImportedApiCustomerDal.GetDZSMnameById | Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs:119 | inline SQL: tblRegion | @RegionCode |
| ImportedApiCustomerDal.GetFEnameById | Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs:128 | inline SQL: tblDistrict | @DistrictCode |
| ImportedApiCustomerDal.UpdateApiCustomerInformation | Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs:164 | sp_UD_ApiCustomerMaster | @Addrees2, @Address, @ApiCustomerId, @AreaCode, @CellNo, @City, @ComUnitCode, @ComUnitName, @ConPers |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_UD_ApiCustomerMaster | ImportedApiCustomerDal.UpdateApiCustomerInformation | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblArea, tblCompanyUnit, tblDistrict, tblMIAInfo, tblMarket, tblRegion, tbltempCustMaster
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Address!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input City name!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Contact Number!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Contact person Name  !! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Customer Address Second  !! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Customer Code  !! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Customer Name!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input DC  !! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 12 methods.
- Redirect targets: none
- User messages: Please Input Address!!; Please Input City name!!; Please Input Contact Number!!; Please Input Contact person Name  !!; Please Input Customer Address Second  !!; Please Input Customer Code  !!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
ImportedApiCustomerBll, ImportedApiCustomerDal, sp_UD_ApiCustomerMaster

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: ImportedApiCustomerBll; DAL: ImportedApiCustomerDal; DAO: CustomerMaster; Tables (inline): tblArea, tblCompanyUnit, tblDistrict, tblMIAInfo, tblMarket, tblRegion, tbltempCustMaster

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/ImportedApiCustomer.aspx`, `Solution.Web/SInventory_UI/ImportedApiCustomer.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/ImportedApiCustomer.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 31 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/ImportedApiCustomer.aspx |
| Data access | This document §15–17 | 1 SP names, 7 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.

## 31. Change Impact
- Module: M17 (SAP, Mobile-App & External Integration).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods ImportedApiCustomerDal.CustomerMasterEditLoad, ImportedApiCustomerDal.GetDZSMname, ImportedApiCustomerDal.GetDZSMnameById, ImportedApiCustomerDal.GetFEInfo, ImportedApiCustomerDal.GetFEnameById affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
