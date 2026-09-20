---
page_id: PAGE-194
module: M05
page: CustomerMasterEdit
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-194 — CustomerMasterEdit

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-194 |
| Module | M05 — Customer & Provider Master (INFERRED from folder + file name) |
| Page name | CustomerMasterEdit |
| Kind | Edit form (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | none (stand-alone HTML page) |
| Page class | SInventory_UI_CustomerMasterEdit |
| aspx size | 18463 bytes |
| Code-behind LOC | 301 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **CustomerMasterEdit** (Edit form). Page title/heading text: `Edit`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer/provider entry, categories/types, credit limits, tagging, Excel uploads/imports, approvals of customer changes.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/CustMasterView.aspx`, `SInventory_UI/DepotStockAdjustmentsVoucherView.aspx`, `SInventory_UI/SampleTypeConventionView.aspx`

Outbound references: none found

## 5. Route
`~/SInventory_UI/CustomerMasterEdit.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/CustomerMasterEdit.aspx |
| Code-behind | Solution.Web/SInventory_UI/CustomerMasterEdit.aspx.cs |
| CSS include | ../css/custom.css |
| CSS include | ../css/style.css |
| CSS include | ../css/colors/blue.css |
| BLL class | CustomerMasterBLL (Library.BLL/SInventory_BLL/CustomerMasterBLL.cs) |
| DAL class | CustomerMasterDAL (Library.DAL/SInventory_DAL/CustomerMasterDAL.cs) |
| DAO/entity class | CustomerMaster (Library.BLL/SInventory_BLL/CustomerMaster.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| custMastIdHiddenField | HiddenField |  | no validator found |  |  |  |  |
| customerCodeTextBox | TextBox |  | no validator found |  | ReadOnly=true |  |  |
| customernameTextBox | TextBox |  | no validator found |  | ReadOnly=true |  |  |
| addressTextBox | TextBox |  | no validator found |  | ReadOnly=true |  |  |
| addressTextBox2 | TextBox |  | no validator found |  | ReadOnly=true |  |  |
| contactTextBox | TextBox |  | no validator found |  | ReadOnly=true |  |  |
| cityTextBox | TextBox |  | no validator found |  | ReadOnly=true |  |  |
| contactPersonTextBox | TextBox |  | no validator found |  | ReadOnly=true |  |  |
| shippingConditionTextBox | TextBox |  | no validator found |  | ReadOnly=true |  |  |
| feNameTextBox | TextBox |  | no validator found |  | ReadOnly=true |  |  |
| dzsmNameTextBox | TextBox |  | no validator found |  | ReadOnly=true |  |  |
| regionNameDropDownList | DropDownList |  | no validator found |  | Enabled=False | see §9 | onselectedindexchanged→regionNameDropDownList_SelectedIndexChanged |
| comUnitNameDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→comUnitNameDropDownList_SelectedIndexChanged |
| districtNameDropDownList | DropDownList |  | no validator found |  | Enabled=False | see §9 | onselectedindexchanged→districtNameDropDownList_SelectedIndexChanged |
| areaNameDropDownList | DropDownList |  | no validator found |  | Enabled=False | see §9 | onselectedindexchanged→areaNameDropDownList_SelectedIndexChanged |
| miaNameDropDownList | DropDownList |  | no validator found |  | Enabled=False | see §9 | onselectedindexchanged→miaNameDropDownList_SelectedIndexChanged |
| marketNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| categoryNameDropDownList | DropDownList |  | no validator found |  | Enabled=False | see §9 |  |
| paymentTypeDropDownList | DropDownList |  | no validator found |  | Enabled=False | see §9 |  |
| updateButton | Button | Update | no validator found |  |  |  | onclick→updateButton_Click |
| closeButton | Button | Close | no validator found |  |  |  | onclick→closeButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| comUnitNameDropDownList | CUnit | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.Customer, CustomerMasterDAL.CustomerPayment, CustomerMasterDAL.Customerorder, CustomerMasterDAL.Single | UNKNOWN | UNKNOWN | UNKNOWN |
| marketNameDropDownList | CustomerMaster.MarketName | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @MarketName | UNKNOWN (SP body not in repo) | UNKNOWN |
| custMastIdHiddenField | CustomerMaster.CustomerMasterId | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @CustomerMasterId | UNKNOWN (SP body not in repo) | UNKNOWN |
| customerCodeTextBox | CustomerMaster.CustomerCode | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @CustomerCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| customernameTextBox | CustomerMaster.CustomerName | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @CustomerName | UNKNOWN (SP body not in repo) | UNKNOWN |
| categoryNameDropDownList | CustomerMaster.CategoryId | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @CategoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| addressTextBox | CustomerMaster.Address | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @Address | UNKNOWN (SP body not in repo) | UNKNOWN |
| addressTextBox2 | CustomerMaster.Addrees2 | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @Addrees2 | UNKNOWN (SP body not in repo) | UNKNOWN |
| contactTextBox | CustomerMaster.CellNo | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @CellNo | UNKNOWN (SP body not in repo) | UNKNOWN |
| cityTextBox | CustomerMaster.City | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @City | UNKNOWN (SP body not in repo) | UNKNOWN |
| contactPersonTextBox | CustomerMaster.ConPerson | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.Customer, CustomerMasterDAL.CustomerPayment, CustomerMasterDAL.Customerorder, CustomerMasterDAL.Single | UNKNOWN | UNKNOWN | UNKNOWN |
| shippingConditionTextBox | CustomerMaster.ShippingCond | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @ShippingCond | UNKNOWN (SP body not in repo) | UNKNOWN |
| marketNameDropDownList | CustomerMaster.MarketCode | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @MarketCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| miaNameDropDownList | CustomerMaster.MIACode | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @MIACode | UNKNOWN (SP body not in repo) | UNKNOWN |
| miaNameDropDownList | CustomerMaster.MiaName | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @MiaName | UNKNOWN (SP body not in repo) | UNKNOWN |
| areaNameDropDownList | CustomerMaster.AreaCode | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @AreaCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| districtNameDropDownList | CustomerMaster.DisCode | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @DisCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| feNameTextBox | CustomerMaster.FEName | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @FEName | UNKNOWN (SP body not in repo) | UNKNOWN |
| comUnitNameDropDownList | CustomerMaster.ComUnitCode | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @ComUnitCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| regionNameDropDownList | CustomerMaster.RegionCode | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @RegionCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| dzsmNameTextBox | CustomerMaster.DZSMName | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @DZSMName | UNKNOWN (SP body not in repo) | UNKNOWN |
| paymentTypeDropDownList | CustomerMaster.TermOfPayment | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.UpdateCustomerMasterInfo | @TermOfPayment | UNKNOWN (SP body not in repo) | UNKNOWN |
| custMastIdHiddenField | dtcustomerProformaButNoDelevery | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.Customer, CustomerMasterDAL.CustomerPayment, CustomerMasterDAL.Customerorder, CustomerMasterDAL.Single | UNKNOWN | UNKNOWN | UNKNOWN |
| custMastIdHiddenField | dtcustomerPaymentNotFull | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.Customer, CustomerMasterDAL.CustomerPayment, CustomerMasterDAL.Customerorder, CustomerMasterDAL.Single | UNKNOWN | UNKNOWN | UNKNOWN |
| customerCodeTextBox | dtcustomerOrderSubmitted | SInventory_UI_CustomerMasterEdit.updateButton_Click | CustomerMasterBLL.Customer, CustomerMasterBLL.CustomerPayment, CustomerMasterBLL.Customerorder, CustomerMasterBLL.Update | CustomerMasterDAL.Customer, CustomerMasterDAL.CustomerPayment, CustomerMasterDAL.Customerorder, CustomerMasterDAL.Single | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| regionNameDropDownList | LoadDropDown → CustomerMasterBLL.LoadRegionname | inline SQL | RegionName/RegionCode, RegionName/RegionId | tblRegion |
| comUnitNameDropDownList | CompanyInfoLoad → CustomerMasterBLL.LoadCompanyUnit | inline SQL | ComUnitName/ComUnitCode | tblCompanyUnit |
| districtNameDropDownList | CompanyInfoLoad → CustomerMasterBLL.LoadDistrictName | inline SQL | DistrictName/DistrictCode | tblDistrict |
| areaNameDropDownList | CompanyInfoLoad → CustomerMasterBLL.LoadAreaName | inline SQL | AreaName/AreaCode | tblArea |
| miaNameDropDownList | CompanyInfoLoad → CustomerMasterBLL.LoadMiaName | inline SQL | MiaName/MiaCode | tblMIAInfo |
| marketNameDropDownList | CompanyInfoLoad → CustomerMasterBLL.LoadMarketName | inline SQL | MarketName/MarketCode | tblMarket |
| categoryNameDropDownList | LoadDropDown → CustomerMasterBLL.LoadCategoryName | inline SQL | CategoryName/CategoryId | tblCustCategory |
| paymentTypeDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

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
| SInventory_UI_CustomerMasterEdit.Page_Load | Solution.Web/SInventory_UI/CustomerMasterEdit.aspx.cs:15 | object sender, EventArgs e | SInventory_UI_CustomerMasterEdit.Page_Load → CustomerMasterBLL.CustomerMasterEditLoad / CustomerMasterBLL.LoadAreaName / CustomerMasterBLL.LoadCategoryName / CustomerMasterBLL.LoadCompanyUnit / CustomerMasterBLL.LoadDistrictName / CustomerMaster → CustomerMast | no | no |
| SInventory_UI_CustomerMasterEdit.LoadDropDown | Solution.Web/SInventory_UI/CustomerMasterEdit.aspx.cs:25 |  | SInventory_UI_CustomerMasterEdit.LoadDropDown → CustomerMasterBLL.LoadCategoryName / CustomerMasterBLL.LoadRegionname → CustomerMasterDAL.LoadCategoryName / CustomerMasterDAL.LoadRegionname / CustomerMasterDAL.SingleParameter | no | no |
| SInventory_UI_CustomerMasterEdit.updateButton_Click | Solution.Web/SInventory_UI/CustomerMasterEdit.aspx.cs:161 | object sender, EventArgs e | SInventory_UI_CustomerMasterEdit.updateButton_Click → CustomerMasterBLL.Customer / CustomerMasterBLL.CustomerPayment / CustomerMasterBLL.Customerorder / CustomerMasterBLL.UpdateDataForCustomerMaster → CustomerMasterDAL.Customer / CustomerMasterDAL.CustomerPaym | no | no |
| SInventory_UI_CustomerMasterEdit.CompanyInfoLoad | Solution.Web/SInventory_UI/CustomerMasterEdit.aspx.cs:233 | string custMastId | SInventory_UI_CustomerMasterEdit.CompanyInfoLoad → CustomerMasterBLL.CustomerMasterEditLoad / CustomerMasterBLL.LoadAreaName / CustomerMasterBLL.LoadCompanyUnit / CustomerMasterBLL.LoadDistrictName / CustomerMasterBLL.LoadMarketName / CustomerMasterBL → Custom | no | no |
| SInventory_UI_CustomerMasterEdit.closeButton_Click | Solution.Web/SInventory_UI/CustomerMasterEdit.aspx.cs:274 | object sender, EventArgs e | SInventory_UI_CustomerMasterEdit.closeButton_Click | no | no |
| SInventory_UI_CustomerMasterEdit.regionNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/CustomerMasterEdit.aspx.cs:278 | object sender, EventArgs e | SInventory_UI_CustomerMasterEdit.regionNameDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_CustomerMasterEdit.areaNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/CustomerMasterEdit.aspx.cs:282 | object sender, EventArgs e | SInventory_UI_CustomerMasterEdit.areaNameDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_CustomerMasterEdit.comUnitNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/CustomerMasterEdit.aspx.cs:287 | object sender, EventArgs e | SInventory_UI_CustomerMasterEdit.comUnitNameDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_CustomerMasterEdit.districtNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/CustomerMasterEdit.aspx.cs:292 | object sender, EventArgs e | SInventory_UI_CustomerMasterEdit.districtNameDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_CustomerMasterEdit.miaNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/CustomerMasterEdit.aspx.cs:297 | object sender, EventArgs e | SInventory_UI_CustomerMasterEdit.miaNameDropDownList_SelectedIndexChanged | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| CustomerMasterBLL.CustomerMasterEditLoad | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:108 |
| CustomerMasterBLL.LoadCompanyUnit | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:139 |
| CustomerMasterBLL.LoadDistrictName | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:144 |
| CustomerMasterBLL.LoadAreaName | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:131 |
| CustomerMasterBLL.LoadMiaName | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:160 |
| CustomerMasterBLL.LoadMarketName | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:126 |
| CustomerMasterBLL.LoadCategoryName | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:156 |
| CustomerMasterBLL.LoadRegionname | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:148 |
| CustomerMasterBLL.Customer | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:113 |
| CustomerMasterBLL.CustomerPayment | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:117 |
| CustomerMasterBLL.Customerorder | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:121 |
| CustomerMasterBLL.UpdateDataForCustomerMaster | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:68 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CustomerMasterDAL.CustomerMasterEditLoad | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:99 | inline SQL: tblCustMaster | @CustomerMasterId |
| CustomerMasterDAL.LoadCompanyUnit | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:184 | inline SQL: tblCompanyUnit |  |
| CustomerMasterDAL.LoadDistrictName | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:197 | inline SQL: tblDistrict |  |
| CustomerMasterDAL.LoadAreaName | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:178 | inline SQL: tblArea |  |
| CustomerMasterDAL.LoadMiaName | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:221 | inline SQL: tblMIAInfo |  |
| CustomerMasterDAL.LoadMarketName | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:166 | inline SQL: tblMarket |  |
| CustomerMasterDAL.LoadCategoryName | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:215 | inline SQL: tblCustCategory |  |
| CustomerMasterDAL.LoadRegionname | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:203 | inline SQL: tblRegion |  |
| CustomerMasterDAL.SingleParameter | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:386 | delegates / no SQL literal |  |
| CustomerMasterDAL.Customer | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:320 | inline SQL: tblInvoice |  |
| CustomerMasterDAL.CustomerPayment | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:325 | inline SQL: tblInvoice |  |
| CustomerMasterDAL.Customerorder | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:330 | inline SQL: tblOrder |  |
| CustomerMasterDAL.UpdateCustomerMasterInfo | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:138 | UD_CustomerMaster2 | @Addrees2, @Address, @AreaCode, @CategoryId, @CellNo, @City, @ComUnitCode, @ComUnitName, @CustomerCo |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| UD_CustomerMaster2 | CustomerMasterDAL.UpdateCustomerMasterInfo | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblArea, tblCompanyUnit, tblCustCategory, tblCustMaster, tblDistrict, tblInvoice, tblMIAInfo, tblMarket, tblOrder, tblRegion
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
- try/catch present in 0 of 12 methods.
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
BLL: CustomerMasterBLL; DAL: CustomerMasterDAL; DAO: CustomerMaster; Tables (inline): tblArea, tblCompanyUnit, tblCustCategory, tblCustMaster, tblDistrict, tblInvoice, tblMIAInfo, tblMarket, tblOrder, tblRegion

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/CustomerMasterEdit.aspx`, `Solution.Web/SInventory_UI/CustomerMasterEdit.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/CustomerMasterEdit.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 27 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/CustomerMasterEdit.aspx |
| Data access | This document §15–17 | 1 SP names, 10 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.

## 31. Change Impact
- Module: M05 (Customer & Provider Master).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods CustomerMasterDAL.Customer, CustomerMasterDAL.CustomerMasterEditLoad, CustomerMasterDAL.CustomerPayment, CustomerMasterDAL.Customerorder, CustomerMasterDAL.LoadAreaName affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
