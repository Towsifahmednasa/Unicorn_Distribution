---
page_id: PAGE-185
module: M05
page: CustMasterEdit
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-185 — CustMasterEdit

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-185 |
| Module | M05 — Customer & Provider Master (INFERRED from folder + file name) |
| Page name | CustMasterEdit |
| Kind | Edit form (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | none (stand-alone HTML page) |
| Page class | SInventory_UI_CustMasterEdit |
| aspx size | 24429 bytes |
| Code-behind LOC | 436 |
| Inline script size | 214 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **CustMasterEdit** (Edit form). Page title/heading text: `Edit`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer/provider entry, categories/types, credit limits, tagging, Excel uploads/imports, approvals of customer changes.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/ActiveCustomerMasterList.aspx`, `SInventory_UI/CustomerMasterView.aspx`, `SInventory_UI/FixedCustomerInformation.aspx`

Outbound references: none found

## 5. Route
`~/SInventory_UI/CustMasterEdit.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/CustMasterEdit.aspx |
| Code-behind | Solution.Web/SInventory_UI/CustMasterEdit.aspx.cs |
| JS include | https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js |
| JS include | https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.jquery.min.js |
| CSS include | ../css/custom.css |
| CSS include | ../css/style.css |
| CSS include | ../css/colors/blue.css |
| CSS include | https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.min.css |
| BLL class | CustomerMasterInfoBll (Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs) |
| DAL class | CustomerMasterInfoDal (Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs) |
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
| shippingConditionTextBox | TextBox |  | no validator found |  |  |  |  |
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
| isActiveCheckBoxList | CheckBox |  | no validator found |  |  |  | OnCheckedChanged→isActiveCheckBoxList_OnCheckedChanged |
| inActiveDateTextBox | TextBox |  | no validator found |  | Enabled=False |  |  |
| imgDatse | ImageButton |  | no validator found |  |  |  |  |
| updateButton | Button | Update | no validator found |  |  |  | onclick→updateButton_Click |
| closeButton | Button | Close | no validator found |  |  |  | onclick→closeButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| comUnitNameDropDownList | CUnit | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.Customer, CustomerMasterInfoDal.CustomerPayment, CustomerMasterInfoDal.SingleParameter, CustomerMa | UNKNOWN | UNKNOWN | UNKNOWN |
| custMastIdHiddenField | CustomerMaster.CustomerMasterId | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @CustomerMasterId | UNKNOWN (SP body not in repo) | UNKNOWN |
| customerCodeTextBox | CustomerMaster.CustomerCode | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @CustomerCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| customernameTextBox | CustomerMaster.CustomerName | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @CustomerName | UNKNOWN (SP body not in repo) | UNKNOWN |
| categoryNameDropDownList | CustomerMaster.CategoryId | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @CategoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| addressTextBox | CustomerMaster.Address | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @Address | UNKNOWN (SP body not in repo) | UNKNOWN |
| addressTextBox2 | CustomerMaster.Addrees2 | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @Addrees2 | UNKNOWN (SP body not in repo) | UNKNOWN |
| contactTextBox | CustomerMaster.CellNo | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @CellNo | UNKNOWN (SP body not in repo) | UNKNOWN |
| cityTextBox | CustomerMaster.City | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @City | UNKNOWN (SP body not in repo) | UNKNOWN |
| contactPersonTextBox | CustomerMaster.ConPerson | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @ConPerson | UNKNOWN (SP body not in repo) | UNKNOWN |
| shippingConditionTextBox | CustomerMaster.ShippingCond | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @ShippingCond | UNKNOWN (SP body not in repo) | UNKNOWN |
| marketNameDropDownList | CustomerMaster.MarketCode | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @MarketCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| marketNameTextBox | CustomerMaster.MarketName | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @MarketName | UNKNOWN (SP body not in repo) | UNKNOWN |
| miaNameDropDownList | CustomerMaster.MIACode | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @MIACode | UNKNOWN (SP body not in repo) | UNKNOWN |
| mioNameTextBox | CustomerMaster.MiaName | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @MiaName | UNKNOWN (SP body not in repo) | UNKNOWN |
| areaNameDropDownList | CustomerMaster.AreaCode | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @AreaCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| districtNameDropDownList | CustomerMaster.DisCode | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @DisCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| feNameTextBox | CustomerMaster.FEName | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @FEName | UNKNOWN (SP body not in repo) | UNKNOWN |
| comUnitNameDropDownList | CustomerMaster.ComUnitCode | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @ComUnitCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| regionNameDropDownList | CustomerMaster.RegionCode | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @RegionCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| dzsmNameTextBox | CustomerMaster.DZSMName | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @DZSMName | UNKNOWN (SP body not in repo) | UNKNOWN |
| paymentTypeDropDownList | CustomerMaster.TermOfPayment | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @TermOfPayment | UNKNOWN (SP body not in repo) | UNKNOWN |
| inActiveDateTextBox | CustomerMaster.InActiveDate | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @InActiveDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| isActiveCheckBoxList | CustomerMaster.IsActive | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | @IsActive | UNKNOWN (SP body not in repo) | UNKNOWN |
| custMastIdHiddenField | dtcustomerProformaButNoDelevery | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.Customer, CustomerMasterInfoDal.CustomerPayment, CustomerMasterInfoDal.SingleParameter, CustomerMa | UNKNOWN | UNKNOWN | UNKNOWN |
| custMastIdHiddenField | dtcustomerPaymentNotFull | SInventory_UI_CustMasterEdit.updateButton_Click | CustomerMasterInfoBll.Customer, CustomerMasterInfoBll.CustomerPayment, CustomerMasterInfoBll.UpdateDataForCustomerMaster | CustomerMasterInfoDal.Customer, CustomerMasterInfoDal.CustomerPayment, CustomerMasterInfoDal.SingleParameter, CustomerMa | UNKNOWN | UNKNOWN | UNKNOWN |
| areaNameDropDownList | TeritoryDataTable | SInventory_UI_CustMasterEdit.CompanyInfoLoad | CustomerMasterInfoBll.CustomerMasterEditLoad, CustomerMasterInfoBll.LoadMarketName, CustomerMasterInfoBll.LoadMiaName, C | CustomerMasterInfoDal.CustomerMasterEditLoad, CustomerMasterInfoDal.GetMarketNameById, CustomerMasterInfoDal.GetMiaNameB | UNKNOWN | UNKNOWN | UNKNOWN |
| miaNameDropDownList | MiaDataTable | SInventory_UI_CustMasterEdit.CompanyInfoLoad | CustomerMasterInfoBll.CustomerMasterEditLoad, CustomerMasterInfoBll.LoadMarketName, CustomerMasterInfoBll.LoadMiaName, C | CustomerMasterInfoDal.CustomerMasterEditLoad, CustomerMasterInfoDal.GetMarketNameById, CustomerMasterInfoDal.GetMiaNameB | UNKNOWN | UNKNOWN | UNKNOWN |
| marketNameDropDownList | marketDataTable | SInventory_UI_CustMasterEdit.CompanyInfoLoad | CustomerMasterInfoBll.CustomerMasterEditLoad, CustomerMasterInfoBll.LoadMarketName, CustomerMasterInfoBll.LoadMiaName, C | CustomerMasterInfoDal.CustomerMasterEditLoad, CustomerMasterInfoDal.GetMarketNameById, CustomerMasterInfoDal.GetMiaNameB | UNKNOWN | UNKNOWN | UNKNOWN |
| regionNameDropDownList | aDataTable | SInventory_UI_CustMasterEdit.regionNameDropDownList_SelectedIndexChanged | CustomerMasterInfoBll.LoadDZSMName | CustomerMasterInfoDal.GetDZSMnameById, CustomerMasterInfoDal.SingleParameter | UNKNOWN | UNKNOWN | UNKNOWN |
| areaNameDropDownList | aDataTable | SInventory_UI_CustMasterEdit.areaNameDropDownList_SelectedIndexChanged | CustomerMasterInfoBll.LoadTeritoryName | CustomerMasterInfoDal.GetTeritorynameById, CustomerMasterInfoDal.SingleParameter | UNKNOWN | UNKNOWN | UNKNOWN |
| districtNameDropDownList | aDataTable | SInventory_UI_CustMasterEdit.districtNameDropDownList_SelectedIndexChanged | CustomerMasterInfoBll.LoadFEName | CustomerMasterInfoDal.GetFEnameById, CustomerMasterInfoDal.SingleParameter | UNKNOWN | UNKNOWN | UNKNOWN |
| miaNameDropDownList | aDataTable | SInventory_UI_CustMasterEdit.miaNameDropDownList_SelectedIndexChanged | CustomerMasterInfoBll.LoadMiaName | CustomerMasterInfoDal.GetMiaNameById, CustomerMasterInfoDal.SingleParameter | UNKNOWN | UNKNOWN | UNKNOWN |
| marketNameDropDownList | aDataTable | SInventory_UI_CustMasterEdit.marketNameDropDownList_OnTextChanged | CustomerMasterInfoBll.LoadMarketName | CustomerMasterInfoDal.GetMarketNameById, CustomerMasterInfoDal.SingleParameter | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| comUnitNameDropDownList | LoadDropDown → CustomerMasterInfoBll.LoadDistributionCenterName | inline SQL | ComUnitName/ComUnitCode | tblCompanyUnit |
| regionNameDropDownList | LoadDropDown → CustomerMasterInfoBll.LoadDZSMInfo | inline SQL | RegionName/RegionCode | tblRegion |
| districtNameDropDownList | LoadDropDown → CustomerMasterInfoBll.LoadFEInfo | inline SQL | DistrictName/DistrictCode | tblDistrict |
| areaNameDropDownList | LoadDropDown → CustomerMasterInfoBll.LoadTerritoryInfo | inline SQL | AreaName/AreaCode | tblArea |
| miaNameDropDownList | LoadDropDown → CustomerMasterInfoBll.LoadMiaInfo | inline SQL | MiaName/MiaCode | tblMIAInfo |
| marketNameDropDownList | LoadDropDown → CustomerMasterInfoBll.LoadMaketInfo | inline SQL | MarketName/MarketCode | tblMarket |
| categoryNameDropDownList | LoadDropDown → CustomerMasterInfoBll.LoadCategoryName | inline SQL | CategoryName/CategoryId | tblCustCategory |
| paymentTypeDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| fbDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_CustMasterEdit.Page_Load | Solution.Web/SInventory_UI/CustMasterEdit.aspx.cs:17 | object sender, EventArgs e | SInventory_UI_CustMasterEdit.Page_Load → CustomerMasterInfoBll.CustomerMasterEditLoad / CustomerMasterInfoBll.LoadCategoryName / CustomerMasterInfoBll.LoadDZSMInfo / CustomerMasterInfoBll.LoadDistributionCenterName / CustomerMasterInfoBll.Lo → CustomerMasterIn | no | no |
| SInventory_UI_CustMasterEdit.LoadDropDown | Solution.Web/SInventory_UI/CustMasterEdit.aspx.cs:33 |  | SInventory_UI_CustMasterEdit.LoadDropDown → CustomerMasterInfoBll.LoadCategoryName / CustomerMasterInfoBll.LoadDZSMInfo / CustomerMasterInfoBll.LoadDistributionCenterName / CustomerMasterInfoBll.LoadFEInfo / CustomerMasterInfoBll.LoadMaketInfo  → CustomerMaste | no | no |
| SInventory_UI_CustMasterEdit.updateButton_Click | Solution.Web/SInventory_UI/CustMasterEdit.aspx.cs:183 | object sender, EventArgs e | SInventory_UI_CustMasterEdit.updateButton_Click → CustomerMasterInfoBll.Customer / CustomerMasterInfoBll.CustomerPayment / CustomerMasterInfoBll.UpdateDataForCustomerMaster → CustomerMasterInfoDal.Customer / CustomerMasterInfoDal.CustomerPayment / CustomerMast | no | no |
| SInventory_UI_CustMasterEdit.CompanyInfoLoad | Solution.Web/SInventory_UI/CustMasterEdit.aspx.cs:286 | string custMastId | SInventory_UI_CustMasterEdit.CompanyInfoLoad → CustomerMasterInfoBll.CustomerMasterEditLoad / CustomerMasterInfoBll.LoadMarketName / CustomerMasterInfoBll.LoadMiaName / CustomerMasterInfoBll.LoadTeritoryName → CustomerMasterInfoDal.CustomerMasterEditLoad / Cus | no | no |
| SInventory_UI_CustMasterEdit.closeButton_Click | Solution.Web/SInventory_UI/CustMasterEdit.aspx.cs:364 | object sender, EventArgs e | SInventory_UI_CustMasterEdit.closeButton_Click | no | no |
| SInventory_UI_CustMasterEdit.regionNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/CustMasterEdit.aspx.cs:368 | object sender, EventArgs e | SInventory_UI_CustMasterEdit.regionNameDropDownList_SelectedIndexChanged → CustomerMasterInfoBll.LoadDZSMName → CustomerMasterInfoDal.GetDZSMnameById / CustomerMasterInfoDal.SingleParameter | no | no |
| SInventory_UI_CustMasterEdit.areaNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/CustMasterEdit.aspx.cs:379 | object sender, EventArgs e | SInventory_UI_CustMasterEdit.areaNameDropDownList_SelectedIndexChanged → CustomerMasterInfoBll.LoadTeritoryName → CustomerMasterInfoDal.GetTeritorynameById / CustomerMasterInfoDal.SingleParameter | no | no |
| SInventory_UI_CustMasterEdit.districtNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/CustMasterEdit.aspx.cs:389 | object sender, EventArgs e | SInventory_UI_CustMasterEdit.districtNameDropDownList_SelectedIndexChanged → CustomerMasterInfoBll.LoadFEName → CustomerMasterInfoDal.GetFEnameById / CustomerMasterInfoDal.SingleParameter | no | no |
| SInventory_UI_CustMasterEdit.miaNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/CustMasterEdit.aspx.cs:400 | object sender, EventArgs e | SInventory_UI_CustMasterEdit.miaNameDropDownList_SelectedIndexChanged → CustomerMasterInfoBll.LoadMiaName → CustomerMasterInfoDal.GetMiaNameById / CustomerMasterInfoDal.SingleParameter | no | no |
| SInventory_UI_CustMasterEdit.marketNameDropDownList_OnTextChanged | Solution.Web/SInventory_UI/CustMasterEdit.aspx.cs:411 | object sender, EventArgs e | SInventory_UI_CustMasterEdit.marketNameDropDownList_OnTextChanged → CustomerMasterInfoBll.LoadMarketName → CustomerMasterInfoDal.GetMarketNameById / CustomerMasterInfoDal.SingleParameter | no | no |
| SInventory_UI_CustMasterEdit.isActiveCheckBoxList_OnCheckedChanged | Solution.Web/SInventory_UI/CustMasterEdit.aspx.cs:422 | object sender, EventArgs e | SInventory_UI_CustMasterEdit.isActiveCheckBoxList_OnCheckedChanged | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| CustomerMasterInfoBll.CustomerMasterEditLoad | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:104 |
| CustomerMasterInfoBll.LoadTeritoryName | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:63 |
| CustomerMasterInfoBll.LoadMiaName | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:68 |
| CustomerMasterInfoBll.LoadMarketName | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:73 |
| CustomerMasterInfoBll.LoadDistributionCenterName | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:23 |
| CustomerMasterInfoBll.LoadDZSMInfo | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:28 |
| CustomerMasterInfoBll.LoadFEInfo | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:33 |
| CustomerMasterInfoBll.LoadTerritoryInfo | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:38 |
| CustomerMasterInfoBll.LoadMiaInfo | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:43 |
| CustomerMasterInfoBll.LoadMaketInfo | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:48 |
| CustomerMasterInfoBll.LoadCategoryName | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:18 |
| CustomerMasterInfoBll.Customer | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:121 |
| CustomerMasterInfoBll.CustomerPayment | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:126 |
| CustomerMasterInfoBll.UpdateDataForCustomerMaster | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:131 |
| CustomerMasterInfoBll.LoadDZSMName | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:53 |
| CustomerMasterInfoBll.LoadFEName | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:58 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CustomerMasterInfoDal.CustomerMasterEditLoad | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:132 | inline SQL: tblCustMaster |  |
| CustomerMasterInfoDal.SingleParameter | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:352 | delegates / no SQL literal |  |
| CustomerMasterInfoDal.GetTeritorynameById | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:78 | inline SQL: tblArea |  |
| CustomerMasterInfoDal.GetMiaNameById | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:84 | inline SQL: tblMIAInfo |  |
| CustomerMasterInfoDal.GetMarketNameById | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:90 | inline SQL: tblMarket |  |
| CustomerMasterInfoDal.LoadCompanyUnit | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:17 | inline SQL: tblCompanyUnit |  |
| CustomerMasterInfoDal.GetDZSMname | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:24 | inline SQL: tblRegion |  |
| CustomerMasterInfoDal.GetFEInfo | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:38 | inline SQL: tblDistrict |  |
| CustomerMasterInfoDal.GetTerritoryInfo | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:45 | inline SQL: tblArea |  |
| CustomerMasterInfoDal.GetMiaInfo | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:52 | inline SQL: tblMIAInfo |  |
| CustomerMasterInfoDal.GetMaketInfo | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:59 | inline SQL: tblMarket |  |
| CustomerMasterInfoDal.LoadCategoryName | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:31 | inline SQL: tblCustCategory |  |
| CustomerMasterInfoDal.Customer | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:206 | inline SQL: tblInvoice |  |
| CustomerMasterInfoDal.CustomerPayment | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:212 | inline SQL: tblInvoice |  |
| CustomerMasterInfoDal.UpdateCustomerMasterInfo | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:218 | UD_CustomerMaster | @Addrees2, @Address, @AreaCode, @CategoryId, @CellNo, @City, @ComUnitCode, @ComUnitName, @ConPerson, |
| CustomerMasterInfoDal.GetDZSMnameById | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:66 | inline SQL: tblRegion |  |
| CustomerMasterInfoDal.GetFEnameById | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:72 | inline SQL: tblDistrict |  |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| UD_CustomerMaster | CustomerMasterInfoDal.UpdateCustomerMasterInfo | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblArea, tblCompanyUnit, tblCustCategory, tblCustMaster, tblDistrict, tblInvoice, tblMIAInfo, tblMarket, tblRegion
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Address!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Area  !! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Category  !! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Contact Person  !! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Customer Address  !! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Customer Address Second  !! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Customer City  !! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Customer Code  !! | UNKNOWN |
| (handler updateButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Data Update Successfully!!! Please Reload | UNKNOWN |
| (handler updateButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please input data in all Textbox | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: LoginName
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 14 methods.
- Redirect targets: none
- User messages: Data Update Successfully!!! Please Reload; Please Input Address!!; Please Input Area  !!; Please Input Category  !!; Please Input Contact Person  !!; Please Input Customer Address  !!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: CustomerMasterInfoBll; DAL: CustomerMasterInfoDal; DAO: CustomerMaster; Tables (inline): tblArea, tblCompanyUnit, tblCustCategory, tblCustMaster, tblDistrict, tblInvoice, tblMIAInfo, tblMarket, tblRegion

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/CustMasterEdit.aspx`, `Solution.Web/SInventory_UI/CustMasterEdit.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/CustMasterEdit.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 34 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/CustMasterEdit.aspx |
| Data access | This document §15–17 | 1 SP names, 9 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.

## 31. Change Impact
- Module: M05 (Customer & Provider Master).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods CustomerMasterInfoDal.Customer, CustomerMasterInfoDal.CustomerMasterEditLoad, CustomerMasterInfoDal.CustomerPayment, CustomerMasterInfoDal.GetDZSMname, CustomerMasterInfoDal.GetDZSMnameById affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
