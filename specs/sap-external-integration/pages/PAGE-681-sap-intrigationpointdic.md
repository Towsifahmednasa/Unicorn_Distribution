---
page_id: PAGE-681
module: M17
page: SAP_IntrigationPointDIC
tier: A
generated: false   # upgraded by manual code review (Tier A)
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-681 — SAP_IntrigationPointDIC

> **Evidence level:** Tier A - reviewed manually against `.aspx`, `.aspx.cs`, and the BLL/DAL code they reach (review date 2026-09-19). Claims are tagged CONFIRMED / INFERRED / UNKNOWN. Stored-procedure bodies are NOT in the repository unless a section says so; column-level effects of such SPs stay `UNKNOWN`. Sections 1, 4-10, 13-15 and 28 still carry the mechanically extracted content (re-checked where noted); all other sections were rewritten.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-681 |
| Module | M17 — SAP, Mobile-App & External Integration (INFERRED from folder + file name) |
| Page name | SAP_IntrigationPointDIC |
| Kind | Page (INFERRED from name) |
| Web folder | SAP_Integration |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SAP_Integration_SAP_IntrigationPointDIC |
| aspx size | 11874 bytes |
| Code-behind LOC | 366 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
DIC ("company-unit variant") copy of PAGE-680. The `Stock Receive` radio is pre-selected and the first load immediately calls `rbType_SelectedIndexChanged(null,null)` (`SAP_IntrigationPointDIC.aspx.cs:44`), so the page opens on the **list of SAP stock-movement challans pending receipt for the logged-in DIC company unit**. `Preview >>` opens PAGE-683. CONFIRMED.

## 3. Business Objective
Let a DIC-role user (own company unit) review and accept SAP challans destined for their unit; Employee/Product tabs behave as on PAGE-680.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SAP_Integration/SAP_StockReceiveDIC.aspx`

Outbound references: `sap_stockreceivedic.aspx`

## 5. Route
`~/SAP_Integration/SAP_IntrigationPointDIC.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SAP_Integration/SAP_IntrigationPointDIC.aspx |
| Code-behind | Solution.Web/SAP_Integration/SAP_IntrigationPointDIC.aspx.cs |
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
| gv_ProductInfo | GridView |  | no validator found |  |  |  | onrowcommand→gv_ProductInfo_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
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
| lbApprove | LinkButton |  | no validator found |  |  |  |  |
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
| rbType | selectedValue | SAP_Integration_SAP_IntrigationPointDIC.LoadInfoNewMethod | — | SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL, SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL, SAP_IntrigationPointDAL.GetSAP | UNKNOWN | UNKNOWN | UNKNOWN |
| hfstatus | dtCheck | SAP_Integration_SAP_IntrigationPointDIC.LoadInfoNewMethod | — | SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL, SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL, SAP_IntrigationPointDAL.GetSAP | UNKNOWN | UNKNOWN | UNKNOWN |
| hfemployee_id | Res | SAP_Integration_SAP_IntrigationPointDIC.gv_EmpInfo_RowCommand | — | SAP_IntrigationPointDAL.CheckSAP_EmpInfoDAL, SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL, SAP_IntrigationPointDAL.GetSAP_P | UNKNOWN | UNKNOWN | UNKNOWN |
| hfproduct_id | Res | SAP_Integration_SAP_IntrigationPointDIC.gv_ProductInfo_RowCommand | — | SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL, SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL, SAP_IntrigationPointDAL.GetSAP | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| rbType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| PreviewButton | Button click | (postback) | SAP_Integration_SAP_IntrigationPointDIC.previewButton_Click | Solution.Web/SAP_Integration/SAP_IntrigationPointDIC.aspx.cs:163 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- First load: header grid + automatic Stock-Receive load (`GetSAP_StockReceivePendingDataDIC`).
- Same radio/list mechanics as PAGE-680; difference list: (1) pre-selected Stock Receive, (2) `previewButton_Click` -> `SAP_StockReceiveDIC.aspx`, (3) hide-challan lookup wrapped in try/catch, (4) `gv_ProductInfo_RowCommand` is repaired to the `ApproveData` command form and the separate `lbApprove_Click` handler is removed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SAP_Integration_SAP_IntrigationPointDIC.Page_Load | Solution.Web/SAP_Integration/SAP_IntrigationPointDIC.aspx.cs:18 | object sender, EventArgs e | SAP_Integration_SAP_IntrigationPointDIC.Page_Load → SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL / SAP_IntrigationPointDAL.GetSAP_IntrigationPointHeaderDAL / SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL / SAP_IntrigationPointDAL.GetSAP_StockReceivePendin → SP: s | no | no |
| SAP_Integration_SAP_IntrigationPointDIC.gv_DocumentUpload_PreRender | Solution.Web/SAP_Integration/SAP_IntrigationPointDIC.aspx.cs:52 | object sender, EventArgs e | SAP_Integration_SAP_IntrigationPointDIC.gv_DocumentUpload_PreRender | no | no |
| SAP_Integration_SAP_IntrigationPointDIC.rbType_SelectedIndexChanged | Solution.Web/SAP_Integration/SAP_IntrigationPointDIC.aspx.cs:63 | object sender, EventArgs e | SAP_Integration_SAP_IntrigationPointDIC.rbType_SelectedIndexChanged → SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL / SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL / SAP_IntrigationPointDAL.GetSAP_StockReceivePendingDataDIC / SAP_IntrigationPointDAL.HideChallanByC | no | no |
| SAP_Integration_SAP_IntrigationPointDIC.LoadInfoNewMethod | Solution.Web/SAP_Integration/SAP_IntrigationPointDIC.aspx.cs:68 |  | SAP_Integration_SAP_IntrigationPointDIC.LoadInfoNewMethod → SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL / SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL / SAP_IntrigationPointDAL.GetSAP_StockReceivePendingDataDIC / SAP_IntrigationPointDAL.HideChallanByChallanNo → | yes | no |
| SAP_Integration_SAP_IntrigationPointDIC.previewButton_Click | Solution.Web/SAP_Integration/SAP_IntrigationPointDIC.aspx.cs:163 | object sender, EventArgs e | SAP_Integration_SAP_IntrigationPointDIC.previewButton_Click | no | no |
| SAP_Integration_SAP_IntrigationPointDIC.gv_StockReceive_RowCommand | Solution.Web/SAP_Integration/SAP_IntrigationPointDIC.aspx.cs:174 | object sender, GridViewCommandEventArgs e | SAP_Integration_SAP_IntrigationPointDIC.gv_StockReceive_RowCommand → SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL / SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL / SAP_IntrigationPointDAL.GetSAP_StockReceivePendingDataDIC / SAP_IntrigationPointDAL.HideChallanByCh | no | yes |
| SAP_Integration_SAP_IntrigationPointDIC.gv_StockReceive_PreRender | Solution.Web/SAP_Integration/SAP_IntrigationPointDIC.aspx.cs:204 | object sender, EventArgs e | SAP_Integration_SAP_IntrigationPointDIC.gv_StockReceive_PreRender | no | no |
| SAP_Integration_SAP_IntrigationPointDIC.gv_StockReceive_RowDataBound | Solution.Web/SAP_Integration/SAP_IntrigationPointDIC.aspx.cs:215 | object sender, GridViewRowEventArgs e | SAP_Integration_SAP_IntrigationPointDIC.gv_StockReceive_RowDataBound | no | no |
| SAP_Integration_SAP_IntrigationPointDIC.gv_EmpInfo_RowCommand | Solution.Web/SAP_Integration/SAP_IntrigationPointDIC.aspx.cs:222 | object sender, GridViewCommandEventArgs e | SAP_Integration_SAP_IntrigationPointDIC.gv_EmpInfo_RowCommand → SAP_IntrigationPointDAL.CheckSAP_EmpInfoDAL / SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL / SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL / SAP_IntrigationPointDAL.GetSAP_StockReceivePendingDataDIC  | no | yes |
| SAP_Integration_SAP_IntrigationPointDIC.AllConditionCheckForEmp | Solution.Web/SAP_Integration/SAP_IntrigationPointDIC.aspx.cs:309 | string femployee_id, string employee_code, string RoleType,  | SAP_Integration_SAP_IntrigationPointDIC.AllConditionCheckForEmp → SAP_IntrigationPointDAL.CheckSAP_EmpInfoDAL → SP: sp_Chk_SAP_EmpInfoCondition | yes | no |
| SAP_Integration_SAP_IntrigationPointDIC.gv_ProductInfo_RowCommand | Solution.Web/SAP_Integration/SAP_IntrigationPointDIC.aspx.cs:334 | object sender, GridViewCommandEventArgs e | SAP_Integration_SAP_IntrigationPointDIC.gv_ProductInfo_RowCommand → SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL / SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL / SAP_IntrigationPointDAL.GetSAP_StockReceivePendingDataDIC / SAP_IntrigationPointDAL.HideChallanByCha | no | yes |

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
| SAP_IntrigationPointDAL.GetSAP_StockReceivePendingDataDIC | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:270 | sp_Get_SAP_DICStockReceivePendingData | @Parm |
| SAP_IntrigationPointDAL.HideChallanByChallanNo | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:400 | sp_Get_SAP_HideChallanByChallanNo | @ChallanNo |
| SAP_IntrigationPointDAL.SaveStockReceive | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:308 | sp_SAP_StockReceive | @ApproveBy, @ChallanNo |
| SAP_IntrigationPointDAL.SaveEmpInfoDAL | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:514 | sp_Upsertdate_EmpInfo | @ActionStatus, @RoleType, @UpdateBy, @employee_code, @employee_id |
| SAP_IntrigationPointDAL.CheckSAP_EmpInfoDAL | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:466 | sp_Chk_SAP_EmpInfoCondition | @ActionStatus, @RoleType, @employee_code, @femployee_id |
| SAP_IntrigationPointDAL.SaveProductInfoDAL | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:580 | sp_Upsertdate_ProductInfo | @UpdateBy, @product_code, @product_id, @status |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Chk_SAP_EmpInfoCondition | SAP_IntrigationPointDAL.CheckSAP_EmpInfoDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_SAP_DICStockReceivePendingData | SAP_IntrigationPointDAL.GetSAP_StockReceivePendingDataDIC | body NOT in repository | read-like name (INFERRED) |
| sp_Get_SAP_EmpInfo | SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_SAP_HideChallanByChallanNo | SAP_IntrigationPointDAL.HideChallanByChallanNo | body NOT in repository | read-like name (INFERRED) |
| sp_Get_SAP_IntrigationPointHeader | SAP_IntrigationPointDAL.GetSAP_IntrigationPointHeaderDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_SAP_ProductInfo | SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL | body NOT in repository | read-like name (INFERRED) |
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

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M17-018 | **DIC scoping.** `GetSAP_StockReceivePendingDataDIC` sends `@Parm = DICCompanyUnitId` when `Session["RoleTypeName"] == "DIC"`, otherwise `@Parm = "0"` (`ComId` defaults 0; the `Convert.ToInt32` failure is swallowed). Hence a non-DIC user opening this page passes `0` (behaviour of SP with 0 UNKNOWN). `Session["RoleTypeName"]` null -> NullReferenceException (unhandled). `GetDepositCodeByDIC` uses the same pattern for deposit-code lookups on the Excel upload pages. | `SAP_IntrigationPointDAL.cs:20-57,269-306` | DAL | CONFIRMED |
| BR-M17-002 | Pending list and hide-challan rule as PAGE-680 (DIC SP `sp_Get_SAP_DICStockReceivePendingData`). | `SAP_IntrigationPointDIC.aspx.cs:118-152` | code-behind | CONFIRMED |
| BR-M17-003 | List-level approve (`ApproveData` on `gv_StockReceive`) -> `sp_SAP_StockReceive`; unvalidated. | `SAP_IntrigationPointDIC.aspx.cs:172-200` | code-behind | CONFIRMED |
| BR-M17-006 | Employee sync with pre-check (same as PAGE-680). | `SAP_IntrigationPointDIC.aspx.cs:220-260` | code-behind | CONFIRMED |
| BR-M17-007 | Product sync without pre-check (same as PAGE-680). | `SAP_IntrigationPointDIC.aspx.cs:330-358` | code-behind | CONFIRMED |

## 20. Permission Matrix
Session keys: `UserId` (audit), `RoleTypeName`, `DICCompanyUnitId` (in DAL). Only the unit filter (BR-M17-018) restricts data; no explicit check that the user *is* DIC; menu-based access.

## 21. Status Lifecycle
As PAGE-680 (staged `action` value; statuses UNKNOWN).

## 22. Error Handling
As PAGE-680; additionally the per-row hide-challan call is guarded by try/catch, hiding SP failures.

## 23. Audit
As PAGE-680. SP bodies are NOT in the repository (only `sp_SAP_B2B*`, `sp_SAP_ChallanSend*`, `sp_SAP_Up_SAP_Challan*` in `SP_NewStockTransferDcToDc_Related.sql` and the two provider-dropout SPs in `Library.DAL/SAP_IntegrationDAL/sql/`).

## 24. Integration
SAP <-> ERP: see INT-01 / INT-02.

## 25. Reports
None.

## 26. Dependencies
BLL: —; DAL: SAP_IntrigationPointDAL; DAO: ResultInfo; Tables (inline): —

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-681-01 | Positive | DIC user opens page | Stock Receive list for own `DICCompanyUnitId` | BR-M17-018 |
| TC-681-02 | Negative | non-DIC user opens page | SP called with `@Parm=0` | BR-M17-018 |
| TC-681-03 | Positive | Click Preview on a challan | redirect to `SAP_StockReceiveDIC.aspx`, `Session["StockMovementMasterId"]` set | BR-M17-002 |
| TC-681-04 | Edge | Hide-challan SP throws | row stays visible (try/catch) | BR-M17-002 |
| TC-681-05 | Edge | Session `RoleTypeName` missing | NullReferenceException | BR-M17-018 |

## 28. Reverse Engineering References
- `Solution.Web/SAP_Integration/SAP_IntrigationPointDIC.aspx`, `Solution.Web/SAP_Integration/SAP_IntrigationPointDIC.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | `SAP_IntrigationPointDIC.aspx(.cs)` | n/a | MISSING | - |
| DIC data scoping | None existed | `@Parm` = DIC company unit | UNKNOWN | UNKNOWN | BR-M17-018 |

## 30. Known Gaps
As PAGE-680, plus: page and code are a near-verbatim copy (drift risk); unit filter relies on a Session role string; `ComId` conversion errors swallowed; `RoleTypeName` NullReference when unset.

## 31. Change Impact
Same DAL as PAGE-680; `sp_Get_SAP_DICStockReceivePendingData` and `sp_Get_DepositCodeByDIC` are shared with the deposit/customer-payment Excel upload pages.

