# Coding standard (as practised in this repository)

This document is **descriptive**: it records how code is actually written in `D:\Projects\Unicorn\Distribution`, marks each observation as **Convention** (dominant, repeated pattern) or **Inconsistency** (competes with the dominant one), and gives counts as evidence. It is not an aspirational rule set and nothing here changes the code. Counts come from greps over `Library.*` and `Solution.Web` excluding `obj/`, `bin/`, `PrecompiledWeb/`.

Related: [architecture.md](architecture.md), [security.md](security.md), [testing.md](testing.md).

## 1. Solution, folder and namespace naming

| Element | Pattern | Type | Evidence |
|---|---|---|---|
| Web folder | `<Area>_UI` (`SInventory_UI` 325 pages, `MasterSetup_UI`, `DoctorModule_UI`, `DoctorMaster_UI`, `SubDepot_UI`, `Approval_UI`, `Reports_UI`, `TransferUI`, `Target_UI`, `Thana_UI`, `LeaveProcess_UI`, `NoticeBoard_UI`, `eProgram_UI`, `Dashboard_UI`, `SettingPanel_UI`) plus report viewers in `SInventory_RPTVIEW` | Convention (with exceptions: `DWSP`, `PromoAlloc`, `UserPermission`, `UserTracking`, `CommonUI`, `Chart` have no `_UI` suffix) | folder list |
| Library folders | Same area name suffixed by layer: `SInventory_BLL/_DAL/_Entities`, `SubDepot_BLL/_DAL/_DAO`, `DoctorModule_DAL/_DAO`, `MasterSetup_DAL/_DAO`; `MAIN_FUNCTION`, `InternalCls`, `DataManager` for infrastructure | Convention, with inconsistent DAO suffix (`_Entities`, `_DAO`, `_Dao`, `Panal_Entities`, `Transer_DAO` misspelling) | `Library.DAO/*` |
| Namespaces | `Library.BLL.SInventory_BLL`, `Library.DAL.SInventory_DAL`, `Library.DAO.SInventory_Entities` (folder = namespace). Exceptions: DALs of the newer doctor/HR modules use `SalesSolution.Web.DataLayer` / `SalesSolution.Web.Models` (`CommonDataLoad.cs`, `ResultInfo`) | Inconsistency (two vendor lineages: original `SInventory` and a later "SalesSolution" style) | `CommonDataLoad.cs:9`, `DoctorCategoryDal.cs:10` |
| Page class | Generated from path: `<Folder>_<PageName>`, e.g. `SInventory_UI_AreaEntry`, `MasterSetup_UI_CampaignSetup`; all 706 pages use `CodeFile=` (Web Site model, not `CodeBehind`) | Convention (forced by Web Site project) | aspx `Inherits=` / `CodeFile=` |
| Page name suffixes | `Entry` (50 pages), `View` (92), `Edit` (32), `List` (51), `Report` (88), `Viewer` (80), `Setup` (34), `Approval` (11), `Upload` (8); mobile web-view pages end in `Apps` | Convention | file counts |
| Disabled/legacy pages | Rename to `*.aspx.exclude` + `*.aspx.cs.exclude` (59 files), or keep `Copy of X.aspx`, `X - Copy.aspx`, `X_old.aspx`, `OLdCampaignSetup.aspx`, `Testing_OrderStatus.aspx` | Inconsistency (no version control discipline; dead pages remain in tree) | `find -name '*.exclude'` |
| File encoding | Mostly UTF-8 with BOM, CRLF; Bengali comments in some code (`DataAccessManager.cs`) | Mixed | file heads |

## 2. Class naming and BLL/DAL/DAO pairing

| Rule | Detail | Type |
|---|---|---|
| Triple per entity | `XxxBLL` (68 files `BLL`, 26 `Bll` in `SInventory_BLL`) + `XxxDAL` (69 `DAL`, 40 `Dal` in `SInventory_DAL`) + entity `Xxx`/`XxxInfo` in `Library.DAO/SInventory_Entities` (e.g. `AreaBLL`, `AreaDAL`, `AreaInfo`) | Convention; suffix casing (`BLL/Bll`, `DAL/Dal`) mixed = Inconsistency |
| Page holds BLL field | `AreaBLL areaBll = new AreaBLL();` in the page class (also `static` in 366 places on pages/DAL-facing code) | Convention / Inconsistency (static shares state) |
| Instance variable naming | Prefix `a` + type: `aOrderInfoBll`, `aInvoiceDal`, `aCommonInternalDal` (about 980 declarations); alternatives `_a...` (22), `_s...` (57 e.g. `_seedRepo`), plain names (`setup`, `accessManager`, `aChartDal`) | Convention (older code) vs newer underscore-prefixed fields |
| DAO shape | Public auto-properties / fields, no logic; return types `ResultInfo` (`isSuccess`, `ErrorMessage`) in newer modules | Convention |
| BLL presence | BLL exists only for `SInventory`, `SubDepot`, `Panal`. Doctor, HR, MasterSetup, Target, DWSP, Transfer, Promo, UserRole, Chart pages call DAL directly (274 code-behind files reference `Library.DAL`) | Inconsistency |
| Result signalling | Older: BLL returns a user message `string` ("Data Save Successfully ...", `AreaBLL.SaveArea`) or `bool`; newer: `ResultInfo`; DB_Manager/`ClsCommonInternalDAL.SaveAction` return `bool/int` 0 on failure | Inconsistency |
| Generated code | `Library.CrystalReports`: `dsXxx.xsd` typed datasets, `crpXxx.rpt` + wrapper `rptXxx` | Convention (tooling) |

## 3. Control and variable naming on pages

| Pattern | Examples | Frequency | Type |
|---|---|---|---|
| **Type suffix, camelCase** (`<meaning><ControlType>`) | `areaNameTextBox`, `districtDropDownList`, `submitButton`, `loadGridView`, `MessageLabel`, `areaInfoIdHiddenField`, `orderGridView` | IDs ending `TextBox` 1,857, `Button` 594, `DropDownList` 542, `GridView` 364, `ImageButton` 263, `Label` 216, `HiddenField` 89, `LinkButton` 52 | Convention (dominant) |
| Hungarian prefix (newer pages) | `txt_Password`, `txtConfirm`, `ddlFY`, `btnSave`, `lblPasswordError`, `hfInvoiceNo`, `gvArcDbConnect`, `chkSelect`, `pnl...` | `lbl` 447, `hf` 429, `btn` 350, `chk` 226, `ddl` 210, `txt` 200, `gv` 162, `pnl` 60 | Inconsistency (newer pages / different authors; both styles are common) |
| Standard IDs reused across pages | grid `loadGridView` (list/view pages), status label `MessageLabel`/`msgLabel`, popup `hf...IdHiddenField` | Convention | aspx |
| Event handlers | VS default `controlID_Click`, sometimes `Click1` after control rename (`submitButton_Click1`), `RowCommand` with `CommandName="EditData"` | Convention |
| Local method names | `Clear()`, `<Thing>Load()`, `PopUp(id)`, `ShowMessageBox(string)` (defined separately in 351 pages), `Validation()`/`DataValidation()` | Convention (copy-paste per page) |
| Session access | `Session["UserId"]` string keys repeated inline (475 uses of UserId), no wrapper class | Convention (weakly typed) |
| Comments | Commented-out blocks left in place (old query versions, alternate connection strings); English and some Bengali | Convention of the repo (no cleanup) |
| `#region` | 46 occurrences in the whole code base | rarely used |

## 4. Typical Entry / View / Edit trio (how a new master page is added)

Complete trios present (Entry + View + Edit in the same folder): Area, CompanyInfo, CompanyUnit, CustMaster, Department, District, Ingridents, Manufacturer, MarketInfo, MiaInfo, MiaTarget, PackSize, ProCategory, Product, ProductSQ, ProType, ProUnitPrice, Region, ShippingCartonSize, StockUOM, User (all in `Solution.Web/SInventory_UI/`).

Reference trio: **Area** (`AreaEntry.aspx`, `AreaView.aspx`, `AreaEdit.aspx`; `AreaBLL`; `AreaDAL`; `AreaInfo`).

| Piece | Template | Example |
|---|---|---|
| `XxxEntry.aspx` | `MasterPageFile="~/MasterPages/NewMasterPage.master"`, content in `ContentPlaceHolder1`, `UpdatePanel`, table layout with `TDLeft/TDRight` cells, `Submit` button and an `ImageButton` linking to the View page. Code-behind: `Page_Load` -> `if (!IsPostBack)` load dropdowns (`XxxBLL.LoadX(ddl)` which binds to `DropDownList` inside the DAL); `submitButton_Click`: `if (requiredTextBox.Text != "")` build the `Xxx` entity from controls, call `aBll.SaveXxx(entity)`, put the returned string into `MessageLabel.Text`, `Clear()`; else `MessageLabel.Text = "Please input data in all Textbox"` | `SInventory_UI/AreaEntry.aspx.cs:11-56` |
| `XxxView.aspx` | Master page, `loadGridView` bound in `!IsPostBack` from `aBll.LoadXxxInfo()`; `RowCommand` `"EditData"` reads `DataKeys[rowindex][0]` and opens the Edit page as a popup with `ScriptManager.RegisterStartupScript(... window.open('XxxEdit.aspx?ID=' + id ...))`; add-button `Response.Redirect("XxxEntry.aspx")`; reload button rebinds | `AreaView.aspx.cs:12-45` |
| `XxxEdit.aspx` | **No master page** (bare HTML page, popup), `Request.QueryString["ID"]` copied to a `HiddenField` in `!IsPostBack`, then `aBll.XxxEditLoad(id)` fills controls; `updateButton_Click` builds entity with `Convert.ToInt32(hiddenField.Value)`, calls `UpdateDataForXxx`, sets `MessageLabel` "Data Update Successfully!!! Please Reload" (green) / "Data Not Update!!!" (red) | `AreaEdit.aspx.cs:12-70` |
| `XxxBLL` | `SaveXxx`: existence check (`HasXxxName`), `ClsPrimaryKeyFind.PrimaryKeyMax("XxxId","tblXxx")`, DAL save, returns a message; `try { ... } catch (Exception ex) { throw ex; } finally { }` | `AreaBLL.cs:15-40` |
| `XxxDAL` | Field `ClsCommonInternalDAL aCommonInternalDal`; methods `SaveXxxInfo` (parameterized `insert into tblXxx (...) values (@...)`, `List<SqlParameter>` with `SInventorySql.DbValue(x)`), `HasXxx`, `LoadXxxView` (join query returning `DataTable`), `XxxEditLoad` (row -> entity), `UpdateXxxInfo` | `AreaDAL.cs:16-92` |
| `Xxx`/`XxxInfo` | property bag in `Library.DAO/SInventory_Entities` | `AreaInfo.cs` |
| Menu | Add a row to `tblMainMenu` (menu URL) and grant it per user/role in `tblMenuDistribution` via the permission pages; menu is DB-driven so no code registration is needed (`UNKNOWN` exact menu SP) | `UserDAL.LoadMenu`, `PanalDAL` |
| Newer variant (doctor/HR modules) | Page keeps only markup + jQuery `$.ajax` to `[WebMethod] public static` methods (`EnableSession=true` to read `UserId`), DAL called directly with `DataAccessManager`, result returned as `ResultInfo`; SP set `sp_Save_X` / `sp_Update_X` / `sp_check_X` / `sp_Get_XList` | `DoctorMaster_UI/ChamberType.aspx.cs:27-40`, `DoctorCategoryDal.cs:15-70` |
| Template stub | `CommonUI/TemplatePage.aspx` (+ `.cs`) | `Solution.Web/CommonUI/` |

## 5. Stored-procedure and SQL naming

| Pattern | Count (distinct literals in code, case-insensitive) | Notes |
|---|---|---|
| `sp_Get_*` / `sp_GET_*` (mixed case) | 495 | reads; e.g. `sp_Get_DoctorCategoryList`, `sp_GET_MainMenuRole2` |
| `sp_Save_*` | 123 | inserts (return new key) |
| `sp_Update_*` | 63 | updates; `sp_Up_*` only 4, `sp_UP_*` and `sp_UD*` (30) also exist: prefix meaning of `sp_UD`/`sp_CS` (46) is `UNKNOWN` |
| `sp_Delete_*` | 47 (`sp_Del_*` 4) | deletes |
| `sp_check_*` / `sp_Check_*` | 55 | duplicate / existence checks |
| `sp_Webapi_*` / `sp_webapi_*` | 29+ | procedures for the mobile/web API |
| `sp_RPT_*`, `sp_Rpt_*`, `sp_Report_*` | 19 + 5 | report feeds |
| Others | `sp_I_*` (5), `sp_Process*`, `sp_SAP_*`, `Dynamic*` (no `sp_` prefix, dynamic pivots), `ExecuteAllSqlQueryByStoreProcedure` (generic), `spInsertReturnInvoice_new` (`sp`, no underscore), `tbl_INV_CentralStoreStoIssue` (a procedure named with a `tbl_` prefix, root script) | Inconsistency: at least 6 prefixes and 3 casings for the same verbs |
| Tables | `tbl<Name>` (`tblArea`, `tblInvoice`, `tblInvoiceDetail`); some `tbl_<Module>_<Name>` (`tbl_UserRoleInfo`, `tbl_INV_BatchWiseStock`, `tbl_MC_*`); child tables `...Child` / `...Detail` | Convention with exceptions |
| Views | `View_Webapi_*` (e.g. `View_Webapi_EmployeeFieldForceInfo`, referenced in commented code) | Convention |
| Column keys | `<Entity>Id` (30 `AreaId`, 29 `GroupId`, 28 `RegionId` mentions in DAO), an `ID` suffix appears in 31 places against 777 `Id`; surrogate `SL` for menu/serial tables (`tblMainMenu.SL`, `tblArcDBConnect.SL`); `<Entity>Code` for business codes; audit columns `EntryBy`, `EntryDate`, `UpdateBy`, `UpdateDate`; `IsActive`, `ActiveDate`, `IsPasswordChange` flags | Convention |
| Primary keys | Application-generated `MAX(id)+1` via `ClsPrimaryKeyFind` (150 sites) **or** SP identity returned by `SaveDataReturnPrimaryKey` | Inconsistency (two mechanisms; older entities use MAX+1) |
| Parameters | `@ParamName` matching entity property; `List<SqlParameter>`; older code `SqlParameter("@X ", ...)` with stray trailing spaces (e.g. `"@CategoryId "`) | Convention with typos |
| Inline SQL | Three styles: (a) parameterized via `SInventorySql`/`...UpdateCommandNew`; (b) concatenated strings sent through `ExecuteAllSqlQueryByStoreProcedure`; (c) `pram` WHERE-fragments built on the page and passed to SPs | Inconsistency (SQL-shaped concatenation on 334 DAL lines and 556 web lines; see [security.md](security.md)) |
| Cross-database | `db..table` names hard-coded in SQL text (`CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster`) | Convention for accounts/ERP posting |

## 6. Data-access and transaction style

| Practice | Type |
|---|---|
| `DataAccessManager`: `try { SqlConnectionOpen(DataBase.SalesDB); ...write... } catch { SqlConnectionClose(true); throw; } finally { SqlConnectionClose(); }` (open in `try`, close in `finally`; commit on close) | Convention for newer DALs (76 files) |
| `ClsCommonInternalDAL`: pass `"SSIDB"` as database key; single statement per call, no explicit transaction | Convention for older DALs (119 files) |
| `SInventorySql` for parameterized inline SQL (84 files) | Emerging convention |
| `DB_Manager` + `CreateConnection_DAL()` opened by BLL and closed later (8 files) | Legacy, inconsistent |
| Method naming: Load* (returns `DataTable` or fills `DropDownList`), Save*, Update*, Delete*, Has* (existence), `...DAL`/`...BLL` suffix on some methods (`GetDeptoWiseOrderDAL`, `MoneyReceiptReportBLL`) | Mixed |
| `DropDownList` binding inside DAL/BLL (`LoadDropDownValue(ddl, displayField, valueField, query, dbName)`) with first item `"--------Select---------"` | Convention |
| Timeouts: literal huge values (`99000000`, `12000000`, `8000`) or 60/90 in newer code | Inconsistency |

## 7. Error handling and messages

| Practice | Evidence | Type |
|---|---|---|
| `try { ... } catch (Exception ex) { throw ex; }` in BLL/DAL | 507 `throw ex/exception;` versus 620 bare `throw;` | Inconsistency (stack loss in the older style) |
| Empty `catch { }` around non-critical loads on pages | about 609 | Convention (silently swallow) |
| User messages: `MessageLabel.Text = "..."` with green/red `ForeColor` (older) or `ScriptManager.RegisterStartupScript(... "successalert('msg','Success','Page.aspx')" ...)` (newer, SweetAlert helper, 84 pages) or `ShowMessageBox("...")` = `alert()` (351 pages define it) | Three styles coexist |
| Standard phrasing (with original typos): "Data Save Successfully!!", "Data Update Successfully!!! Please Reload", "Data Not Update!!!", "Please input data in all Textbox", "Operation successful!" (131 uses), "User name or password incourect !!!!" | Convention (typos preserved) |
| Redirect to `../Dashboard_UI/DashboardOne.aspx` or `../Login.aspx` from `catch` blocks in `Page_Load` (permission failure, session loss) | Convention |

## 8. Front-end conventions

- Two UI generations: `NewMasterPage` (Bootstrap 5 template in `VerticalAsset/`, 435 pages) and `MainMasterPage` (older `css/style.css` template, 130 pages). Do not assume one theme per module.
- jQuery `$.ajax({ url: 'Page.aspx/Method', type: 'POST', contentType: 'application/json' ...})` for page methods (e.g. `ChamberType.aspx:251`, `AdminDashboard.aspx:3071`); DataTables (`VerticalAsset/MyDt`), SweetAlert, Select2, Chart.js/ApexCharts/Highcharts as bundled plugins.
- Inline `<style>`/`<script>` blocks inside pages are normal (e.g. `Login.aspx` embeds its own CSS block); shared JS is only `CustomScript/_myCusGen_Func.js`, `_QuickDataAccess.js`.
- Cascading market-structure filters via the 24 `IV*` user controls.

## 9. Report conventions

- List page `XxxReport.aspx` (filters) opens `SInventory_RPTVIEW/XxxReportViewer.aspx` (no master) with QueryString and/or Session parameters; viewer names tables in a `DataSet` to match the `.rpt`; `.rpt` files named `crp<Name>.rpt`, wrapper classes `rpt<Name>`, datasets `ds<Name>` (see [architecture.md](architecture.md) section 9). Excel exports are separate list-page buttons using EPPlus/ClosedXML.

## 10. Where conventions conflict (checklist for reviewers)

1. Control-naming style (`xxxTextBox` vs `txtXxx`) - both accepted in existing code.
2. Data-access stack (`DataAccessManager` vs `ClsCommonInternalDAL` vs `SInventorySql` vs `DB_Manager`) - four live styles.
3. PK generation (`MAX+1` vs identity).
4. SP prefix and casing (`sp_Get`/`sp_GET`, `Update`/`Up`/`UD`).
5. Message display (label, `alert`, `successalert`).
6. BLL present versus direct DAL calls.
7. `static` versus instance DAL/BLL fields.
8. Disabled pages kept as `.exclude` / `Copy of`.
