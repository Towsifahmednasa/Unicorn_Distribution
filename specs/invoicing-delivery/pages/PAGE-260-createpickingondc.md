---
page_id: PAGE-260
module: M07
page: CreatePickingOnDC
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "COMPLETE", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---

# PAGE-260 — CreatePickingOnDC

> **Evidence level:** Tier A - reviewed 2026-09-19 by reading the .aspx, .aspx.cs, the BLL/DAL methods reached, and every SP/SQL body present in the repository. Confidence tags: CONFIRMED (read in implementation), INFERRED (implied by several references), UNKNOWN (not derivable; SP bodies are absent from the repo except where stated). Module: M07 - Proforma, Invoicing & Delivery.


## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-260 |
| Module | M07 - Proforma, Invoicing & Delivery |
| Page name | CreatePickingOnDC |
| Kind | Entry / Transaction (group invoices into a DC picking slip) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_CreatePickingOnDC |
| Size | aspx 206 lines, code-behind 115 lines |
| Suspected legacy/duplicate | Older, simple picking flow (DC picking by invoice number); the newer per-DA/route flow is TopSheetGenerate (PAGE-277) and SCPickingGenerate (PAGE-276). No inbound link found by grep; picking slips are re-printed from `DCPickingPrintingList.aspx` (PAGE-262). |

## 2. Purpose
CONFIRMED (heading "Picking Slip for Distribution Center"): choose a DC and an **invoice date**, Search lists that day's invoices that are not yet on any picking slip; enter a **picking date** and an **area**, tick invoices and press "Create Picking". The system stores a DC picking header (number `DPK-...`) with the ticked invoices as detail rows. The printed picking slip (PAGE-262 / `DCPickingReportViewer`) lists total quantity per product and batch of all invoices on the slip.

## 3. Business Objective
INFERRED: a warehouse/DC store keeper collects goods for many invoices in one pass (pick-pack) before loading; a picking slip is created per area/day so each invoice is picked exactly once. The page changes no stock; stock was already reserved when the invoice was created (tblDCStore Issue rows, BR-M07-013).

## 4. Navigation
Menu DB-driven -> UNKNOWN. Inbound: none (grep). Outbound: none; sibling print list `DCPickingPrintingList.aspx` -> `../SInventory_RPTVIEW/DCPickingReportViewer.aspx?pickNo=<DPK no>`.

## 5. Route
`~/SInventory_UI/CreatePickingOnDC.aspx` (file-based WebForms route; no `MapPageRoute` alias).


## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/CreatePickingOnDC.aspx |
| Code-behind | Solution.Web/SInventory_UI/CreatePickingOnDC.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | InvoiceBLL (Library.BLL/SInventory_BLL/InvoiceBLL.cs) |
| BLL class | RequisitionBLL (Library.BLL/SInventory_BLL/RequisitionBLL.cs) |
| DAL class | InvoiceDAL (Library.DAL/SInventory_DAL/InvoiceDAL.cs) |
| DAL class | RequisitionDAL (Library.DAL/SInventory_DAL/RequisitionDAL.cs) |
| DAO/entity class | DCPicking (Library.DAO/SInventory_Entities/DCPicking.cs) |
| DAO/entity class | DCPickingDetail (Library.DAO/SInventory_Entities/DCPickingDetail.cs) |


## 7. UI Control Inventory
| Control ID | Type | Label | Required | Default | Read-only / Enabled / Visible | Data source | Handler / error |
|---|---|---|---|---|---|---|---|
| dcDropDownList | DropDownList | "DC Name" | YES for Search ("Select DC & Date !!"); not validated at Create | Admin (`Session["UserType"]=="Admin"`): all units `ComUnitCode:ComUnitName`; others: `RequisitionBLL.DCLoad(ddl, Session["ComUnitId"])` | AutoPostBack | see BR-M07-042 | `dcDropDownList_SelectedIndexChanged` -> loads areas |
| dateTextBox (+CalendarExtender dd-MMM-yyyy) | TextBox | "Invoice Date" | YES for Search | empty | editable | typed | - |
| searchButton | Button | "Search" | - | - | - | - | `searchButton_Click` -> `LoadGrid()` |
| picDateTextBox (+calendar) | TextBox | (picking date) | none server-side, `Convert.ToDateTime` at Create | today `dd-MMM-yyyy` on first load | editable | typed | FormatException if empty/invalid |
| areaDropDownList | DropDownList | (area) | none server-side (`Convert.ToInt32(SelectedValue)` at Create) | empty until the DC dropdown fires a change event | enabled | `InvoiceBLL.AreaDropDownLoad(ddl, ComUnitId)` (`InvoiceDAL.cs:1189-1195`) | FormatException if no area selected |
| createButton | Button | "Create Picking" | - | - | - | - | `createButton_Click` -> `Save()` |
| reportListGridView | GridView (columns: Select checkbox `printCheckBox`, Invoice No, Invoice Date, Order No, Order Date, Cust. Code, Customer Name) | - | at least one selection is NOT enforced | - | - | `InvoiceForDCPickingBLL` | - |

## 8. Field Mapping
| UI | DTO | DAL parameter | Table.column |
|---|---|---|---|
| dcDropDownList.SelectedValue | `DCPicking.ComUnitId` | `@ComUnitId` | tblDCPicking.ComUnitId |
| dcDropDownList.SelectedItem.Text split ':' [0] | `DCPicking.ComUnitCode` | (only for number generation) | - |
| picDateTextBox | `DCPicking.DCPicDate` | `@DCPicDate` | tblDCPicking.DCPicDate |
| areaDropDownList.SelectedValue | `DCPicking.AreaId` | `@AreaId` | tblDCPicking.AreaId |
| generated | `DCPicId` (PrimaryKeyMax) / `DCPicNo` | `@DCPicId`, `@DCPicNo` | tblDCPicking.DCPicId / DCPicNo |
| grid cell 1 text (Invoice No) | `DCPickingDetail.InvoiceNo` | `@InvoiceNo` | tblDCPickingDetail.InvoiceNo |
| generated | `DCPicDetailId`, `DCPicId` | `@DCPicDetailId`, `@DCPicId` | tblDCPickingDetail.DCPicDetailId / DCPicId |

## 9. Dropdown Mapping
| Dropdown | Loader | SQL |
|---|---|---|
| dcDropDownList (Admin) | `RequisitionBLL.DCLoad(ddl)` -> `RequisitionDAL.DCLoad` (`RequisitionDAL.cs:35-39`) | `select ComUnitId, (ComUnitCode+':'+ComUnitName) as Com from dbo.tblCompanyUnit` via `LoadDropDownValue` (placeholder "--------Select---------") |
| dcDropDownList (non-admin) | `RequisitionBLL.DCLoad(ddl, ComUnitId)` -> `RequisitionDAL.DCLoad(ddl, comUnitId)` (`RequisitionDAL.cs:53-64`) | `select ComUnitId, ComUnitName from tblCompanyUnit WHERE ComUnitId IN (SELECT CompanyUnitId FROM tblUserCompanyUnit WHERE UserId=@UserId)` with `@UserId` = the value passed as `comUnitId` (**Session["ComUnitId"] is used as a UserId**) |
| areaDropDownList | `InvoiceBLL.AreaDropDownLoad(ddl, dc)` (`InvoiceDAL.cs:1189-1195`) | `SELECT A.* FROM tblArea A LEFT JOIN tblDistrict D ON A.DistrictId=D.DistrictId LEFT JOIN tblCompanyUnit C ON D.ComUnitId=C.ComUnitId WHERE C.ComUnitId='<dc>'` - **string concatenation**, run through the generic SQL executor SP `ExecuteAllSqlQueryByStoreProcedure` (`ClsCommonInternalDAL.LoadDropDownValue`, `:284-310`) |

## 10. Button Action Mapping
| Button | Handler | Chain | DB change |
|---|---|---|---|
| Search | `searchButton_Click` -> `LoadGrid()` (`.aspx.cs:45-69`) | `InvoiceBLL.InvoiceForDCPickingBLL(dc, date)` -> `InvoiceDAL.InvoiceForDCPickingDAL` (`InvoiceDAL.cs:1098-1109`) | none |
| Create Picking | `createButton_Click` -> `Save()` (`.aspx.cs:84-110`) | `InvoiceBLL.DcPickingSaveBLL` (PrimaryKeyMax + `DCPIKNoGenerator` + `DcPickingSaveDAL`), then per ticked row `DcPickingDetailSaveBLL` -> `DcPickingDetailSaveDAL`; then `LoadGrid()` and alert | INSERT tblDCPicking (1 row), INSERT tblDCPickingDetail (n rows) |

## 11. Frontend Flow
UpdatePanel; calendars via AjaxControlToolkit `CalendarExtender`. All logic server-side. After Create, `LoadGrid()` re-runs the search so newly picked invoices disappear (`NOT IN (SELECT InvoiceNo FROM tblDCPickingDetail)`).

## 12. Backend Flow
| Handler | Source | Flow | try/catch | Txn |
|---|---|---|---|---|
| Page_Load | `.aspx.cs:15-38` | `Session["UserType"].ToString() != ""` (NullReference if the key is absent - the `else Response.Redirect("Login.aspx")` is unreachable for null); Admin vs other DC list; picking date = today | no | no |
| LoadGrid | `.aspx.cs:45-69` | DC and date required else alert "Select DC & Date !!"; 0 rows -> alert "No Data Found!!" | no | no |
| Save | `.aspx.cs:84-110` | build header; insert header first; then details for ticked rows; alert | no | **no** |
| dcDropDownList_SelectedIndexChanged | `.aspx.cs:112-115` | areas of chosen DC | no | no |

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.


## 14. Service Mapping
| BLL method | Source |
|---|---|
| RequisitionBLL.DCLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:152 |
| InvoiceBLL.InvoiceForDCPickingBLL | Library.BLL/SInventory_BLL/InvoiceBLL.cs:784 |
| InvoiceBLL.DcPickingSaveBLL | Library.BLL/SInventory_BLL/InvoiceBLL.cs:800 |
| InvoiceBLL.DCPIKNoGenerator | Library.BLL/SInventory_BLL/InvoiceBLL.cs:809 |
| InvoiceBLL.DcPickingDetailSaveBLL | Library.BLL/SInventory_BLL/InvoiceBLL.cs:789 |
| InvoiceBLL.AreaDropDownLoad | Library.BLL/SInventory_BLL/InvoiceBLL.cs:865 |


## 15. Repository Mapping
| DAL | Source | SQL | Params |
|---|---|---|---|
| InvoiceDAL.InvoiceForDCPickingDAL | `InvoiceDAL.cs:1098-1109` | `SELECT * FROM tblInvoice I LEFT JOIN tblCustMaster C ON I.CustomerMasterId=C.CustomerMasterId WHERE I.ComUnitId=@ComUnitId AND I.InvoiceDate=@InvoiceDate AND I.InvoiceNo NOT IN (SELECT InvoiceNo FROM tblDCPickingDetail) ORDER BY I.InvoiceId DESC` | @ComUnitId, @InvoiceDate |
| InvoiceDAL.DcPickingSaveDAL | `InvoiceDAL.cs:1111-1128` | INSERT tblDCPicking(DCPicId, DCPicNo, DCPicDate, ComUnitId, AreaId) | 5 params |
| InvoiceDAL.DcPickingDetailSaveDAL | `InvoiceDAL.cs:1140-1153` | INSERT tblDCPickingDetail(DCPicDetailId, InvoiceNo, DCPicId) | 3 params |
| InvoiceDAL.DcPickingNoCount | `InvoiceDAL.cs:1212-1219` | `SELECT count(DCPicNo) CountNo FROM tblDCPicking WHERE ComUnitId=@ComUnitId` | @ComUnitId |
| InvoiceDAL.AreaDropDownLoad | `InvoiceDAL.cs:1189-1195` | string-concatenated query | - |

## 16. Stored Procedure Mapping
No stored procedure is called (only the generic SQL executor SP `ExecuteAllSqlQueryByStoreProcedure` used by the dropdown loaders).

## 17. Database Mapping
- READ: tblCompanyUnit, tblUserCompanyUnit, tblArea, tblDistrict, tblInvoice, tblCustMaster, tblDCPickingDetail, tblDCPicking (count).
- WRITTEN: `tblDCPicking` (DCPicId, DCPicNo, DCPicDate, ComUnitId, AreaId) and `tblDCPickingDetail` (DCPicDetailId, InvoiceNo, DCPicId). Keys via `ClsPrimaryKeyFind.PrimaryKeyMax` (max+1).
- Downstream READ: `DCPickingReportDetailDataDAL` aggregates `tblInvoiceDetail` by ProductCode/ProductName/BatchNo for invoices on the slip.

## 18. Validation Matrix
| Rule | Layer | Condition | Message |
|---|---|---|---|
| DC and invoice date | server `LoadGrid` | either empty | alert "Select DC & Date !!" |
| Data exists | server | 0 rows | alert "No Data Found!!" |
| Session type | server Page_Load | `Session["UserType"]` empty string -> redirect "Login.aspx" (relative path, would resolve under SInventory_UI) | - |
| Create: at least one invoice, area, DC, picking date | **none** | - | success text "Create Piking Successfully" (typo) always shown |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M07-042 | DC list: Admin (`UserType == "Admin"`) sees every company unit; everyone else gets `RequisitionDAL.DCLoad(ddl, Session["ComUnitId"])`, which queries tblUserCompanyUnit with `UserId = <ComUnitId value>`. The variable is a ComUnitId but is used as a UserId, so non-admin users see units mapped to a user whose id equals their ComUnitId - mostly wrong or empty (INFERRED consequence). The same defect exists in `InvoicePrintingList` and `DCPickingPrintingList`. | `CreatePickingOnDC.aspx.cs:19-33`; `RequisitionDAL.cs:53-64`; `InvoicePrintingList.aspx.cs:18-28`; `DCPickingPrintingList.aspx.cs:18-28` | BLL/DAL | CONFIRMED |
| BR-M07-043 | Search shows invoices of the DC whose `InvoiceDate` equals the entered date and whose InvoiceNo is not present in ANY `tblDCPickingDetail` row (an invoice can be picked once, across all DCs and dates). Delivered, deleted-and-recreated or partially confirmed invoices are not excluded; no status filter. | `InvoiceDAL.cs:1098-1109` | inline SQL | CONFIRMED |
| BR-M07-044 | Create Picking inserts the tblDCPicking header BEFORE reading the ticked rows and without a transaction: with nothing ticked an empty header is created; an exception after the header leaves a header with no/partial details. No check that DC, area, date or at least one invoice is selected. The message "Create Piking Successfully" is returned unconditionally. | `CreatePickingOnDC.aspx.cs:84-110`; `InvoiceBLL.cs:788-806` | code-behind + BLL | CONFIRMED |
| BR-M07-045 | Picking number = `"DPK-" + ComUnitCode + "-" + (COUNT(DCPicNo) of that ComUnit + 1 zero-padded to 8 digits)`; `ComUnitCode` is `dcDropDownList.SelectedItem.Text.Split(':')[0]`, i.e. the code for Admin (text `CODE:Name`) but the **unit name** for non-admin users (text is only ComUnitName). The count-based counter can repeat after deletions/concurrent saves. | `InvoiceBLL.cs:808-846`; `InvoiceDAL.cs:1212-1219`; `.aspx.cs:86-91` | BLL | CONFIRMED |
| BR-M07-046 | The DC picking slip totals, per product/name/batch, `SUM(tblInvoiceDetail.TotalQuantity)` (paid + bonus) of all invoices on the slip; header shows DPK no, date, DC and area. | `InvoiceDAL.cs:1166-1187` | inline SQL | CONFIRMED |

## 20. Permission Matrix
- Session keys: `UserType` (Admin branch; NullReference if missing), `ComUnitId` (misused as user id, BR-M07-042). No other check. The Admin bypass means DC scope is enforced only via the (defective) DC list; the posted `dcDropDownList.SelectedValue` is bound by ASP.NET event validation but there is no server check that the user may act for that DC.
- Master page authentication: MainMasterPage (see M01).

## 21. Status Lifecycle
No status flag exists: "picked" is represented only by the presence of the InvoiceNo in `tblDCPickingDetail` (used by the Search filter). There is no cancel/undo of a picking in this module (no delete code found: grep of `tblDCPicking` writes -> only INSERTs here). Consolidated lifecycle: README.

## 22. Error Handling
No try/catch anywhere; FormatException (`Convert.ToInt32("")`, `Convert.ToDateTime("")`), NullReference on `Session["UserType"]`, and SQL errors surface as unhandled exceptions. Alerts via `alert()`.

## 23. Audit
No audit columns (no CreatedBy/CreatedDate on tblDCPicking/tblDCPickingDetail in the INSERTs). Only DCPicDate (user-entered).

## 24. Integration
None.

## 25. Reports
Picking slip: `DCPickingReportViewer.aspx?pickNo=` (from PAGE-262 print list) -> `rptDcPickingSlip.rpt` (Library.CrystalReports/SInventory_RPT/rptDcPickingSlip.rpt) with datasets mainDataTable, detailDataTable, companyInfoDataTable; see `../reports/RPT-M07-02-top-sheet-picking-challan.md`.

## 26. Dependencies
BLL: RequisitionBLL, InvoiceBLL. DAL: RequisitionDAL, InvoiceDAL, SInventorySql, ClsCommonInternalDAL, ClsPrimaryKeyFind. DAO: DCPicking, DCPickingDetail. Tables: tblCompanyUnit, tblUserCompanyUnit, tblArea, tblDistrict, tblInvoice, tblCustMaster, tblDCPicking, tblDCPickingDetail.

## 27. Test Cases
| ID | Scenario | Steps | Expected | BR |
|---|---|---|---|---|
| TC-260-01 | Admin DC list | login as Admin | all DCs listed | BR-M07-042 |
| TC-260-02 | Non-admin DC list | login as a DC user with ComUnitId=2, UserId=17 | list is based on UserId=2 mapping (likely wrong/empty) | BR-M07-042 |
| TC-260-03 | Search without DC/date | Search | "Select DC & Date !!" | - |
| TC-260-04 | Search no data | valid inputs, no invoices | "No Data Found!!" | - |
| TC-260-05 | Exclusion of picked invoices | pick invoice A, Search again | A absent | BR-M07-043 |
| TC-260-06 | Create with rows | tick 2, area chosen | 1 header + 2 detail rows; number DPK-<code>-0000000N | BR-M07-044/045 |
| TC-260-07 | Create with nothing ticked | click Create | empty header inserted; success text shown | BR-M07-044 |
| TC-260-08 | Create without area | area empty | FormatException | BR-M07-044 |
| TC-260-09 | Number for non-admin | non-admin creates | code part is the unit name | BR-M07-045 |
| TC-260-10 | Slip totals | print slip of two invoices with same batch | quantity summed | BR-M07-046 |
| TC-260-11 | Injection | tamper DC value | area query altered | GAP-M07-021 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/CreatePickingOnDC.aspx`, `Solution.Web/SInventory_UI/CreatePickingOnDC.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).


## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | page present | n/a | MISSING | - |
| DC scoping | None existed | ComUnitId used as UserId | - | GAP | BR-M07-042 |
| Create validation | None existed | none; empty header possible | - | GAP | BR-M07-044 |
| Numbering | None existed | count-based | - | RISK | BR-M07-045 |

## 30. Known Gaps
- Wrong DC scoping for non-admin users (GAP-M07-022).
- No validation and no transaction on Create; unconditional success text (GAP-M07-023).
- Area query built by string concatenation and executed by a generic SQL executor SP (GAP-M07-021).
- Count-based numbering, non-admin code = unit name (GAP-M07-024).
- No audit columns; no undo of a picking; no status.
- Unreachable redirect branch for missing session type.

## 31. Change Impact
- Shares `InvoiceBLL.AreaDropDownLoad`, `DcPickingSaveBLL`, `DcPickingDetailSaveBLL` with `DCPickingPrintingList` (PAGE-262); `RequisitionDAL.DCLoad(ddl, id)` shared with InvoicePrintingList (PAGE-268), DCPickingPrintingList and other requisition pages (grep `DCLoad(`).
- `tblDCPickingDetail` feeds the DC picking slip report.
- Related but distinct: warehouse picking (CreatePickingDetailForWareHouse, PAGE-259) works on requisitions, not invoices.
