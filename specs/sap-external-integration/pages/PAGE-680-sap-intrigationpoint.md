---
page_id: PAGE-680
module: M17
page: SAP_IntrigationPoint
tier: A
generated: false   # upgraded by manual code review (Tier A)
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-680 — SAP_IntrigationPoint

> **Evidence level:** Tier A - reviewed manually against `.aspx`, `.aspx.cs`, and the BLL/DAL code they reach (review date 2026-09-19). Claims are tagged CONFIRMED / INFERRED / UNKNOWN. Stored-procedure bodies are NOT in the repository unless a section says so; column-level effects of such SPs stay `UNKNOWN`. Sections 1, 4-10, 13-15 and 28 still carry the mechanically extracted content (re-checked where noted); all other sections were rewritten.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-680 |
| Module | M17 — SAP, Mobile-App & External Integration (INFERRED from folder + file name) |
| Page name | SAP_IntrigationPoint |
| Kind | Page (INFERRED from name) |
| Web folder | SAP_Integration |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SAP_Integration_SAP_IntrigationPoint |
| aspx size | 18269 bytes |
| Code-behind LOC | 398 |
| Inline script size | 6345 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
"SAP Integration Point" console for the **head office / company user**: shows what SAP has staged for the ERP and lets the user accept it - (a) **Product master changes** (`Product Info`), (b) **Employee master changes** (`Employee Info`: new joiner / transfer / exit with from-to territory, area, zone), and (c) **Stock movement challans** from SAP (`Stock Receive`, radio item hidden with `display:none` on this page; it is pre-selected on the DIC copy PAGE-681). CONFIRMED: `Solution.Web/SAP_Integration/SAP_IntrigationPoint.aspx(.cs)`.

## 3. Business Objective
Keep ERP master data (products, employees/territory assignments) and stock in step with SAP: SAP pushes into staging (`SAP_API_Data` database / `sp_Get_SAP_*` views, source UNKNOWN), a human approves, the ERP tables are updated by `sp_Upsertdate_*` / `sp_SAP_StockReceive`. See `specs/integrations/INT-01` and `INT-02`.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SAP_Integration/SAP_StockReceive.aspx`, `SAP_Integration/SAP_StockReceiveDIC.aspx`

Outbound references: `sap_stockreceive.aspx`

## 5. Route
`~/SAP_Integration/SAP_IntrigationPoint.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SAP_Integration/SAP_IntrigationPoint.aspx |
| Code-behind | Solution.Web/SAP_Integration/SAP_IntrigationPoint.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | SAP_IntrigationPointDAL (Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| id_mastetID | HiddenField |  | no validator found |  |  |  |  |
| gv_HeaderInfo | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | SL# | no validator found |  |  |  |  |
| (no id) | BoundField | Title | no validator found |  |  |  |  |
| (no id) | BoundField | Value | no validator found |  |  |  |  |
| rbType | RadioButtonList |  | no validator found |  |  |  | OnSelectedIndexChanged→rbType_SelectedIndexChanged |
| gv_EmpInfo | GridView |  | no validator found |  |  |  | onrowcommand→gv_EmpInfo_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| hfemployee_code | HiddenField |  | no validator found |  |  |  |  |
| hfRoleType | HiddenField |  | no validator found |  |  |  |  |
| hfemployee_id | HiddenField |  | no validator found |  |  |  |  |
| hfaction | HiddenField |  | no validator found |  |  |  |  |
| (no id) | BoundField | EMP SAP Code | no validator found |  |  |  |  |
| (no id) | BoundField | EMP Name | no validator found |  |  |  |  |
| (no id) | BoundField | Role Type | no validator found |  |  |  |  |
| (no id) | BoundField | Joining Date | no validator found |  |  |  |  |
| (no id) | BoundField | Mobile No | no validator found |  |  |  |  |
| (no id) | BoundField | From Territory | no validator found |  |  |  |  |
| (no id) | BoundField | To Territory | no validator found |  |  |  |  |
| (no id) | BoundField | From Area | no validator found |  |  |  |  |
| (no id) | BoundField | To Area | no validator found |  |  |  |  |
| (no id) | BoundField | From Zone | no validator found |  |  |  |  |
| (no id) | BoundField | To Zone | no validator found |  |  |  |  |
| (no id) | BoundField | Action Status | no validator found |  |  |  |  |
| (no id) | TemplateField | Actions | no validator found |  |  |  |  |
| lbApprove | LinkButton |  | no validator found |  |  |  |  |
| gv_ProductInfo | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| hfstatus | HiddenField |  | no validator found |  |  |  |  |
| hfproduct_code | HiddenField |  | no validator found |  |  |  |  |
| hfproduct_id | HiddenField |  | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Description | no validator found |  |  |  |  |
| (no id) | BoundField | Entry Date | no validator found |  |  |  |  |
| (no id) | BoundField | Action Status | no validator found |  |  |  |  |
| (no id) | TemplateField | Actions | no validator found |  |  |  |  |
| lbApprove | LinkButton |  | no validator found |  |  |  | OnClick→lbApprove_Click |
| gv_StockReceive | GridView |  | no validator found |  |  |  | OnRowCommand→gv_StockReceive_RowCommand, OnRowDataBound→gv_StockReceive_RowDataBound, OnPreRender→gv_StockReceive_PreRender |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| hfstatus | HiddenField |  | no validator found |  |  |  |  |
| (no id) | BoundField | Challan No | no validator found |  |  |  |  |
| (no id) | BoundField | Challan Date | no validator found |  |  |  |  |
| (no id) | BoundField | From | no validator found |  |  |  |  |
| (no id) | BoundField | To | no validator found |  |  |  |  |
| (no id) | BoundField | Action Status | no validator found |  |  |  |  |
| (no id) | BoundField | Description | no validator found |  |  |  |  |
| (no id) | TemplateField | Actions | no validator found |  |  |  |  |
| lbApprove | LinkButton |  | no validator found |  |  |  |  |
| PreviewButton | Button | Preview >> | no validator found |  |  |  | OnClick→previewButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| rbType | selectedValue | SAP_Integration_SAP_IntrigationPoint.LoadInfoNewMethod | — | SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL, SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL, SAP_IntrigationPointDAL.GetSAP | UNKNOWN | UNKNOWN | UNKNOWN |
| hfstatus | dtCheck | SAP_Integration_SAP_IntrigationPoint.LoadInfoNewMethod | — | SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL, SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL, SAP_IntrigationPointDAL.GetSAP | UNKNOWN | UNKNOWN | UNKNOWN |
| hfemployee_id | Res | SAP_Integration_SAP_IntrigationPoint.gv_EmpInfo_RowCommand | — | SAP_IntrigationPointDAL.CheckSAP_EmpInfoDAL, SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL, SAP_IntrigationPointDAL.GetSAP_P | UNKNOWN | UNKNOWN | UNKNOWN |
| hfproduct_id | Res | SAP_Integration_SAP_IntrigationPoint.gv_ProductInfo_RowCommand | — | SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL, SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL, SAP_IntrigationPointDAL.GetSAP | UNKNOWN | UNKNOWN | UNKNOWN |
| hfproduct_id | Res | SAP_Integration_SAP_IntrigationPoint.lbApprove_Click | — | SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL, SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL, SAP_IntrigationPointDAL.GetSAP | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| rbType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| lbApprove | LinkButton click | (postback) | SAP_Integration_SAP_IntrigationPoint.lbApprove_Click | Solution.Web/SAP_Integration/SAP_IntrigationPoint.aspx.cs:360 | — | SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL, SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL, SAP_Intrig | sp_Get_SAP_EmpInfo, sp_Get_SAP_HideChallanByChallanNo, sp_Get_SAP_ProductInfo, sp_Get_SAP_StockReceivePendingData, sp_Up | SP executed — effect on tables UNKNOWN (body not in repo) |
| PreviewButton | Button click | (postback) | SAP_Integration_SAP_IntrigationPoint.previewButton_Click | Solution.Web/SAP_Integration/SAP_IntrigationPoint.aspx.cs:154 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- First load only: header grid (BR-M17-001). Nothing else is loaded until the radio list changes.
- `rbType_SelectedIndexChanged` (AutoPostBack) -> `LoadInfoNewMethod()`: clears the three grids, then binds one of `gv_ProductInfo`, `gv_EmpInfo`, `gv_StockReceive`; empty result => `faildalert('No data found!')`.
- Row actions (LinkButton `Approve`): postback; SweetAlert popups `ShowSuccesalert('Operation successful!')` / `faildalert`.
- Stock Receive rows: `Preview >>` button -> `previewButton_Click` stores `Session["StockMovementMasterId"]` and redirects to `SAP_StockReceive.aspx` (PAGE-682).
- Client script: only the alert helpers (`ShowSuccesalert`, `faildalert`, `successalert`); no AJAX.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SAP_Integration_SAP_IntrigationPoint.Page_Load | Solution.Web/SAP_Integration/SAP_IntrigationPoint.aspx.cs:18 | object sender, EventArgs e | SAP_Integration_SAP_IntrigationPoint.Page_Load → SAP_IntrigationPointDAL.GetSAP_IntrigationPointHeaderDAL → SP: sp_Get_SAP_IntrigationPointHeader | no | no |
| SAP_Integration_SAP_IntrigationPoint.gv_DocumentUpload_PreRender | Solution.Web/SAP_Integration/SAP_IntrigationPoint.aspx.cs:50 | object sender, EventArgs e | SAP_Integration_SAP_IntrigationPoint.gv_DocumentUpload_PreRender | no | no |
| SAP_Integration_SAP_IntrigationPoint.rbType_SelectedIndexChanged | Solution.Web/SAP_Integration/SAP_IntrigationPoint.aspx.cs:61 | object sender, EventArgs e | SAP_Integration_SAP_IntrigationPoint.rbType_SelectedIndexChanged → SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL / SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL / SAP_IntrigationPointDAL.GetSAP_StockReceivePendingData / SAP_IntrigationPointDAL.HideChallanByChallan | no | no |
| SAP_Integration_SAP_IntrigationPoint.LoadInfoNewMethod | Solution.Web/SAP_Integration/SAP_IntrigationPoint.aspx.cs:66 |  | SAP_Integration_SAP_IntrigationPoint.LoadInfoNewMethod → SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL / SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL / SAP_IntrigationPointDAL.GetSAP_StockReceivePendingData / SAP_IntrigationPointDAL.HideChallanByChallanNo → SP: s | no | no |
| SAP_Integration_SAP_IntrigationPoint.previewButton_Click | Solution.Web/SAP_Integration/SAP_IntrigationPoint.aspx.cs:154 | object sender, EventArgs e | SAP_Integration_SAP_IntrigationPoint.previewButton_Click | no | no |
| SAP_Integration_SAP_IntrigationPoint.gv_StockReceive_RowCommand | Solution.Web/SAP_Integration/SAP_IntrigationPoint.aspx.cs:165 | object sender, GridViewCommandEventArgs e | SAP_Integration_SAP_IntrigationPoint.gv_StockReceive_RowCommand → SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL / SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL / SAP_IntrigationPointDAL.GetSAP_StockReceivePendingData / SAP_IntrigationPointDAL.HideChallanByChallanN | no | yes |
| SAP_Integration_SAP_IntrigationPoint.gv_StockReceive_PreRender | Solution.Web/SAP_Integration/SAP_IntrigationPoint.aspx.cs:195 | object sender, EventArgs e | SAP_Integration_SAP_IntrigationPoint.gv_StockReceive_PreRender | no | no |
| SAP_Integration_SAP_IntrigationPoint.gv_StockReceive_RowDataBound | Solution.Web/SAP_Integration/SAP_IntrigationPoint.aspx.cs:206 | object sender, GridViewRowEventArgs e | SAP_Integration_SAP_IntrigationPoint.gv_StockReceive_RowDataBound | no | no |
| SAP_Integration_SAP_IntrigationPoint.gv_EmpInfo_RowCommand | Solution.Web/SAP_Integration/SAP_IntrigationPoint.aspx.cs:213 | object sender, GridViewCommandEventArgs e | SAP_Integration_SAP_IntrigationPoint.gv_EmpInfo_RowCommand → SAP_IntrigationPointDAL.CheckSAP_EmpInfoDAL / SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL / SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL / SAP_IntrigationPointDAL.GetSAP_StockReceivePendingData / SAP_ | no | yes |
| SAP_Integration_SAP_IntrigationPoint.AllConditionCheckForEmp | Solution.Web/SAP_Integration/SAP_IntrigationPoint.aspx.cs:300 | string femployee_id, string employee_code, string RoleType,  | SAP_Integration_SAP_IntrigationPoint.AllConditionCheckForEmp → SAP_IntrigationPointDAL.CheckSAP_EmpInfoDAL → SP: sp_Chk_SAP_EmpInfoCondition | yes | no |
| SAP_Integration_SAP_IntrigationPoint.gv_ProductInfo_RowCommand | Solution.Web/SAP_Integration/SAP_IntrigationPoint.aspx.cs:325 | object sender, GridViewCommandEventArgs e | SAP_Integration_SAP_IntrigationPoint.gv_ProductInfo_RowCommand → SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL / SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL / SAP_IntrigationPointDAL.GetSAP_StockReceivePendingData / SAP_IntrigationPointDAL.HideChallanByChallanNo | no | yes |
| SAP_Integration_SAP_IntrigationPoint.lbApprove_Click | Solution.Web/SAP_Integration/SAP_IntrigationPoint.aspx.cs:360 | object sender, EventArgs e | SAP_Integration_SAP_IntrigationPoint.lbApprove_Click → SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL / SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL / SAP_IntrigationPointDAL.GetSAP_StockReceivePendingData / SAP_IntrigationPointDAL.HideChallanByChallanNo / SA → SP | no | yes |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SAP_IntrigationPointDAL.GetSAP_IntrigationPointHeaderDAL | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:60 | sp_Get_SAP_IntrigationPointHeader | @Parm, @Parm2 |
| SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:445 | sp_Get_SAP_ProductInfo | @Parm, @Parm2 |
| SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:423 | sp_Get_SAP_EmpInfo | @Parm, @Parm2 |
| SAP_IntrigationPointDAL.GetSAP_StockReceivePendingData | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:246 | sp_Get_SAP_StockReceivePendingData | @Parm |
| SAP_IntrigationPointDAL.HideChallanByChallanNo | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:400 | sp_Get_SAP_HideChallanByChallanNo | @ChallanNo |
| SAP_IntrigationPointDAL.SaveStockReceive | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:308 | sp_SAP_StockReceive | @ApproveBy, @ChallanNo |
| SAP_IntrigationPointDAL.SaveEmpInfoDAL | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:514 | sp_Upsertdate_EmpInfo | @ActionStatus, @RoleType, @UpdateBy, @employee_code, @employee_id |
| SAP_IntrigationPointDAL.CheckSAP_EmpInfoDAL | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:466 | sp_Chk_SAP_EmpInfoCondition | @ActionStatus, @RoleType, @employee_code, @femployee_id |
| SAP_IntrigationPointDAL.SaveProductInfoDAL | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:580 | sp_Upsertdate_ProductInfo | @UpdateBy, @product_code, @product_id, @status |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Chk_SAP_EmpInfoCondition | SAP_IntrigationPointDAL.CheckSAP_EmpInfoDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_SAP_EmpInfo | SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_SAP_HideChallanByChallanNo | SAP_IntrigationPointDAL.HideChallanByChallanNo | body NOT in repository | read-like name (INFERRED) |
| sp_Get_SAP_IntrigationPointHeader | SAP_IntrigationPointDAL.GetSAP_IntrigationPointHeaderDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_SAP_ProductInfo | SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_SAP_StockReceivePendingData | SAP_IntrigationPointDAL.GetSAP_StockReceivePendingData | body NOT in repository | read-like name (INFERRED) |
| sp_SAP_StockReceive | SAP_IntrigationPointDAL.SaveStockReceive | body NOT in repository | read-like name (INFERRED) |
| sp_Upsertdate_EmpInfo | SAP_IntrigationPointDAL.SaveEmpInfoDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Upsertdate_ProductInfo | SAP_IntrigationPointDAL.SaveProductInfoDAL | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler gv_StockReceive_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success'); | UNKNOWN |
| (handler gv_StockReceive_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler gv_EmpInfo_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success'); | UNKNOWN |
| (handler gv_EmpInfo_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler gv_ProductInfo_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success'); | UNKNOWN |
| (handler gv_ProductInfo_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler lbApprove_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ShowSuccesalert('Operation successful!', 'Success'); | UNKNOWN |
| (handler lbApprove_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | faildalert('Already Exist!', 'Faild'); | UNKNOWN |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M17-001 | **Header panel.** On first load the page shows a Title/Value summary from `sp_Get_SAP_IntrigationPointHeader(@Parm='',@Parm2='')` (content of the summary UNKNOWN). Empty result => empty grid, no message. | `SAP_IntrigationPoint.aspx.cs:17-47`; `SAP_IntrigationPointDAL.cs:59-79` | code-behind + SP | CONFIRMED (wiring) |
| BR-M17-002 | **Pending SAP challans list.** `rbType = "Stock Receive"` loads `sp_Get_SAP_StockReceivePendingData(@Parm='')` (all challans staged by SAP); the DIC variant (PAGE-681) calls `sp_Get_SAP_DICStockReceivePendingData(@Parm=DICCompanyUnitId)` (BR-M17-018). No rows => popup `No data found!`. Challan rows for which `sp_Get_SAP_HideChallanByChallanNo(@ChallanNo)` returns any row are hidden (`Rows[i].Visible=false`) - the SP body is unknown; by name/usage it hides challans already received (INFERRED). In the DIC page that lookup is wrapped in try/catch (errors ignored). | `SAP_IntrigationPoint.aspx.cs:120-151`; `SAP_IntrigationPointDIC.aspx.cs:122-152`; DAL `:245-306,399-421` | code-behind + SP | CONFIRMED |
| BR-M17-003 | **Approve challan from list** (`ApproveData` command) calls `SaveStockReceive(challanNo)` -> `sp_SAP_StockReceive(@ChallanNo, @ApproveBy=Session UserId)`; pre-check `AllConditionCheck` is commented out, so there is no validation. On `isSuccess` false the popup says `Already Exist!` - but `SaveData` returns true or throws, so that branch is effectively unreachable (same DataAccessManager behaviour as BR-M14-012). On success the list is reloaded. *(In practice the Approve button in the list is commented out in markup; users use `Preview >>` -> PAGE-682/683 to approve.)* | `SAP_IntrigationPoint.aspx.cs:164-193`; `SAP_IntrigationPoint.aspx:232-240`; DAL `:307-338` | code-behind + DAL | CONFIRMED |
| BR-M17-006 | **Employee Info sync (SAP -> ERP).** Grid `sp_Get_SAP_EmpInfo(@Parm='',@Parm2='')` shows SAP employee events (code, name, role type, joining date, mobile, from/to Territory/Area/Zone, action status). `Approve` first runs `sp_Chk_SAP_EmpInfoCondition(@femployee_id,@employee_code,@RoleType,@ActionStatus)`; if the first row has `Datacheck > 0` the page shows the SP's `Msg` (popup) and stops; otherwise `sp_Upsertdate_EmpInfo(@employee_id,@employee_code,@RoleType,@ActionStatus,@UpdateBy=Session UserId)` is executed and the list reloaded. | `SAP_IntrigationPoint.aspx.cs:212-245,299-323`; DAL `:422-545` | code-behind + SP | CONFIRMED (flow); UNKNOWN (SP rules) |
| BR-M17-007 | **Product Info sync (SAP -> ERP).** Grid `sp_Get_SAP_ProductInfo`; `Approve` (`lbApprove_Click`) runs `sp_Upsertdate_ProductInfo(@product_id,@product_code,@status,@UpdateBy=Session UserId)` with **no pre-check** (`AllConditionCheck` commented out). A second handler `gv_ProductInfo_RowCommand` reads its hidden fields from `gv_EmpInfo` (copy/paste bug) and is not wired to the LinkButton (it uses `OnClick=lbApprove_Click`). | `SAP_IntrigationPoint.aspx.cs:324-397`; `SAP_IntrigationPoint.aspx:198-210` | code-behind + SP | CONFIRMED |

## 20. Permission Matrix
- Session keys read: `UserId` (audit param in every approve). For the plain (non-DIC) page there is no role condition (CONFIRMED). The DAL `GetSAP_StockReceivePendingDataDIC`/`GetDepositCodeByDIC` read `RoleTypeName` and `DICCompanyUnitId` (DIC page only).
- Menu-based access only; the page lives under `SAP_Integration/` with the standard master page `NewMasterPage.master`.
- Approve buttons perform master-data upserts with no additional role check: any user who can open the page can accept SAP employee/product changes.

## 21. Status Lifecycle
SAP staging rows carry an `action` (Action Status) value shown in the grids and passed as `@ActionStatus` to `sp_Upsertdate_EmpInfo`; the allowed values and the transitions written by the SPs are UNKNOWN (SP bodies missing). What the page proves:
- Employee: staged row -> (check via `sp_Chk_SAP_EmpInfoCondition`) -> `sp_Upsertdate_EmpInfo` -> row disappears from the next `sp_Get_SAP_EmpInfo` result (INFERRED from reload).
- Product: staged row -> `sp_Upsertdate_ProductInfo(@status)` -> disappears (INFERRED).
- Challan: pending -> `sp_SAP_StockReceive` -> hidden via `sp_Get_SAP_HideChallanByChallanNo` (INFERRED).

## 22. Error Handling
- DAL layer: on exception `SqlConnectionClose(true)` (rollback), `isSuccess=false`, exception rethrown -> not caught by these pages -> yellow error page.
- Page-level: only `AllConditionCheckForEmp` messages (SP `Msg`) and generic popups (`No data found!`, `Operation successful!`, `Already Exist!`).
- `AllConditionCheck` / `AllConditionCheckForProduct` are stubs (`new DataTable()`), always pass.

## 23. Audit
Audit: `@UpdateBy`/`@ApproveBy` = `Session["UserId"]` passed to the SPs; storage columns UNKNOWN. SP bodies are NOT in the repository (only `sp_SAP_B2B*`, `sp_SAP_ChallanSend*`, `sp_SAP_Up_SAP_Challan*` in `SP_NewStockTransferDcToDc_Related.sql` and the two provider-dropout SPs in `Library.DAL/SAP_IntegrationDAL/sql/`).

## 24. Integration
**SAP <-> ERP integration.** This page is the operator console for the inbound half. Full description: `specs/integrations/INT-01-sap-master-data-sync.md` (employee/product) and `INT-02-sap-stock-movement-receive.md` (challans). How SAP data reaches staging is not visible in the repo (INFERRED: a separate SAP-side/REST adapter writes `SAP_API_Data`; the stock SPs query `SAP_API_Data..tblSAP_StockMovementMaster/Detail`).

## 25. Reports
None (grid only).

## 26. Dependencies
BLL: —; DAL: SAP_IntrigationPointDAL; DAO: ResultInfo; Tables (inline): —

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-680-01 | Positive | Open page | header grid from `sp_Get_SAP_IntrigationPointHeader` | BR-M17-001 |
| TC-680-02 | Positive | Choose Employee Info, approve a new-joiner row | `sp_Chk_SAP_EmpInfoCondition` then `sp_Upsertdate_EmpInfo`; success popup; list reload | BR-M17-006 |
| TC-680-03 | Negative | Approve a row for which the check SP returns `Datacheck>0` | popup with SP `Msg`; no upsert | BR-M17-006 |
| TC-680-04 | Positive | Choose Product Info, approve | `sp_Upsertdate_ProductInfo` executes without pre-check | BR-M17-007 |
| TC-680-05 | Edge | Product/Employee list empty | popup `No data found!` | BR-M17-002 |
| TC-680-06 | Edge | (DB) stock challan already received | row hidden from list | BR-M17-002 |
| TC-680-07 | Security | Any logged-in user approves employee change | allowed (no role check) | permission |
| TC-680-08 | Failure | Break DB during approve | exception page; DAL rolled back | - |

## 28. Reverse Engineering References
- `Solution.Web/SAP_Integration/SAP_IntrigationPoint.aspx`, `Solution.Web/SAP_Integration/SAP_IntrigationPoint.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | `SAP_IntrigationPoint.aspx(.cs)` | n/a | MISSING | - |
| Product approve pre-check | None existed | commented out | UNKNOWN | RISK | BR-M17-007 |
| SAP staging schema | None existed | - | not in repo | UNKNOWN | GAP-M17-001 |

## 30. Known Gaps
- Product approval has no validation; wrong handler (`gv_ProductInfo_RowCommand`) reads `gv_EmpInfo` controls (dead/buggy).
- Duplicate page copies (`SAP_IntrigationPointDIC`, `SAP_StockReceive`, `SAP_StockReceiveDIC`) differ by a handful of lines.
- `Stock Receive` radio hidden but coded; `HideChallanByChallanNo` executed once per grid row (N+1 queries, each opening a connection).
- Static DAL instance shared across requests (`private static SAP_IntrigationPointDAL _DAL`); DAL uses a per-instance `DataAccessManager` field with connection state - **not thread-safe under concurrent requests** (INFERRED: `accessManager` holds `sqlConnection`/`sqlTransaction` as instance fields, `DataAccessManager.cs:104-146`).
- "Already Exist!" message for every failure.
- SP bodies missing (GAP-M17-001).

## 31. Change Impact
Shares `SAP_IntrigationPointDAL` with PAGE-681/682/683/694 and the Excel-upload pages (`TargetExcelUpload`, `DepositSlipExcelUpload`, `CustomerPaymentExcelUpload`). `sp_Upsertdate_EmpInfo`/`ProductInfo` change ERP master data used all over the system.

