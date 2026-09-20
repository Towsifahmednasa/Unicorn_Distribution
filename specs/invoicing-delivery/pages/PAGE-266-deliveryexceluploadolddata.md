---
page_id: PAGE-266
module: M07
page: DeliveryExcelUploadOldData
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---

# PAGE-266 — DeliveryExcelUploadOldData

> **Evidence level:** Tier A - reviewed 2026-09-19 by reading the .aspx, .aspx.cs, the BLL/DAL methods reached, and every SP/SQL body present in the repository. Confidence tags: CONFIRMED (read in implementation), INFERRED (implied by several references), UNKNOWN (not derivable; SP bodies are absent from the repo except where stated). Module: M07 - Proforma, Invoicing & Delivery.


## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-266 |
| Module | M07 — Proforma, Invoicing & Delivery (INFERRED from folder + file name) |
| Page name | DeliveryExcelUploadOldData |
| Kind | Upload / Import (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_DeliveryExcelUploadOldData |
| aspx size | 26113 bytes |
| Code-behind LOC | 719 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |


## 2. Purpose
CONFIRMED (code/labels): upload an Excel file whose columns are `ProformaInvoiceNo` and `Amount`; the rows appear in a grid ("Invoice No", "Amount"); "Submit" confirms each listed proforma invoice as delivered using the "old data" SP. The page is a clone of a requisition entry page (requisition/warehouse/DC fields and save helpers remain in code but the visible flow is upload -> submit).

## 3. Business Objective
INFERRED: back-fill delivery confirmation for historical/legacy invoices in bulk (catch-up after migration) without going through Loading Summary. Effect on stock/accounting is UNKNOWN (`sp_DeliveryConformationFull_OldData` absent); the journal-posting call used by LoadingSummary is NOT made here.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `orderrequisitionview.aspx`


## 5. Route
`~/SInventory_UI/DeliveryExcelUploadOldData.aspx` (file-based WebForms route; no `MapPageRoute` alias).


## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/DeliveryExcelUploadOldData.aspx |
| Code-behind | Solution.Web/SInventory_UI/DeliveryExcelUploadOldData.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | DCStoreBLL (Library.BLL/SInventory_BLL/DCStoreBLL.cs) |
| BLL class | InvoiceBLL (Library.BLL/SInventory_BLL/InvoiceBLL.cs) |
| BLL class | RequisitionBLL (Library.BLL/SInventory_BLL/RequisitionBLL.cs) |
| DAL class | DCStoreDAL (Library.DAL/SInventory_DAL/DCStoreDAL.cs) |
| DAL class | InvoiceDAL (Library.DAL/SInventory_DAL/InvoiceDAL.cs) |
| DAL class | RequisitionDAL (Library.DAL/SInventory_DAL/RequisitionDAL.cs) |
| DAO/entity class | Requesition (Library.DAO/SInventory_Entities/Requesition.cs) |
| DAO/entity class | RequsitionChild (Library.DAO/SInventory_Entities/RequsitionChild.cs) |


## 7. UI Control Inventory
| Control | Type | Label | Required | Default | Notes |
|---|---|---|---|---|---|
| id_fu | FileUpload | (file) | none | - | any file; no extension filter |
| btnUpload / lbl_up_status | Button / Label | "Upload" / "<n> record Found!" | - | - | `btnUpload_Click` -> `ExcelToGrid` |
| IsFileUploaded | HiddenField | - | - | "true" after upload | never checked at submit |
| productGridView | GridView | "Invoice No" (TextBox `productCodeTextBox` bound to `ProformaInvoiceNo`), "Amount" (TextBox `productNameTextBox` bound to `Amount`, digits/'.' only) | none | one blank row on load | textboxes editable |
| submitButton | LinkButton | (Submit) | - | - | confirm dialog `sweetAlertConfirm_Submit`; `submitButton_Click` |
| Reset link | LinkButton | "Reset" | - | - | `Unnamed_Click` redirect to self |
| requisition leftovers (`manufacturerDropDownList`, `wareHouseDropDownList`, `dcDropDownList`, `reqNoTextBox`, `reqDateTextBox`, `hiddenField`) | various | - | - | loaded in `Page_Load` (`DropDownLoad`, `ReqNo`, `GetOneRecord(MasID)`) | not part of the submit flow; markup partly inside `<%-- --%>` |

## 8. Field Mapping
| UI | Chain | SP param |
|---|---|---|
| each row `productCodeTextBox.Text` (Invoice No) | `InvoiceBLL.SaveFullInvoiceOldData` -> `InvoiceDAL.SaveFullInvoiceOldData` (`InvoiceDAL.cs:261-267`) | `sp_DeliveryConformationFull_OldData @InvoiceNo` |
| Session["LoginName"] | same | `@UpdateBy` |
| today `dd-MMM-yyyy` | same | `@UpdateDate` |
| Amount | not used | - |

## 9. Dropdown Mapping
Requisition leftovers only (`LoadmanufacturerName`, `WareHouseLoad`, `DCLoad`); not relevant to the flow.

## 10. Button Action Mapping
| Button | Handler | Chain | DB change |
|---|---|---|---|
| Upload | `btnUpload_Click` `.aspx.cs:647-650` -> `ExcelToGrid` (`:651-700`): saves file to `~/ExcelFiles/<FileName>`, OLEDB Jet 4.0 (.xls) or ACE 12.0 (.xlsx), first sheet, `SELECT * From [sheet]`, imports rows whose first cell is non-empty, binds grid | none in DB; file written to web root |
| Submit | `submitButton_Click` `:578-596`: loop rows -> `SaveFullInvoiceOldData`; if last `status == 1` alert "Delivery Invoice Save Successfully" and reset grid | UNKNOWN (SP absent) |

## 11. Frontend Flow
Client: SweetAlert confirm on Submit; FilteredTextBoxExtender on Amount. All logic postback.

## 12. Backend Flow
`Page_Load` (`:19-34`) initialises requisition leftovers and `MasID` edit mode. `ExcelToGrid`: no try/catch; unsupported extension leaves `oledbConn` null -> NullReferenceException. `submitButton_Click`: no validation, no try/catch, no transaction.

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.


## 14. Service Mapping
| BLL method | Source |
|---|---|
| RequisitionBLL.LoadmanufacturerName | Library.BLL/SInventory_BLL/RequisitionBLL.cs:138 |
| RequisitionBLL.WareHouseLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:134 |
| RequisitionBLL.DCLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:152 |
| RequisitionBLL.GetDataInfoByIdBll | Library.BLL/SInventory_BLL/RequisitionBLL.cs:49 |
| RequisitionBLL.GetDataInfoByIdBllDtls | Library.BLL/SInventory_BLL/RequisitionBLL.cs:54 |
| RequisitionBLL.ReqNo | Library.BLL/SInventory_BLL/RequisitionBLL.cs:73 |
| RequisitionBLL.ReqnNoGenerator | Library.BLL/SInventory_BLL/RequisitionBLL.cs:81 |
| DCStoreBLL.ProductInfo | Library.BLL/SInventory_BLL/DCStoreBLL.cs:90 |
| DCStoreBLL.GetProductStock | Library.BLL/SInventory_BLL/DCStoreBLL.cs:98 |
| RequisitionBLL.SaveRequsition | Library.BLL/SInventory_BLL/RequisitionBLL.cs:20 |
| RequisitionBLL.ReqId | Library.BLL/SInventory_BLL/RequisitionBLL.cs:67 |
| RequisitionBLL.SaveRequsitionChild | Library.BLL/SInventory_BLL/RequisitionBLL.cs:33 |
| RequisitionBLL.ReqChildId | Library.BLL/SInventory_BLL/RequisitionBLL.cs:59 |
| RequisitionBLL.UpdateManufacturerInfo | Library.BLL/SInventory_BLL/RequisitionBLL.cs:28 |
| RequisitionBLL.DeleteRequisitionDtls | Library.BLL/SInventory_BLL/RequisitionBLL.cs:182 |
| InvoiceBLL.SaveFullInvoiceOldData | Library.BLL/SInventory_BLL/InvoiceBLL.cs:63 |


## 15. Repository Mapping
`InvoiceDAL.SaveFullInvoiceOldData` -> `RunStoreProcedure("sp_DeliveryConformationFull_OldData")` (params above).

## 16. Stored Procedure Mapping
`sp_DeliveryConformationFull_OldData` - body absent; UNKNOWN. (Live `sp_DeliveryConformationFull_New` is the counterpart used by LoadingSummary.)

## 17. Database Mapping
Written: UNKNOWN (SP). Read (leftovers): tblManufacturer, tblWearhouse, tblCompanyUnit, tblRequisition, tblRequsitionChild, tblProduct, tblUnitPrice, tbl_INV_CentralStore_WeightedAvg. File system: `~/ExcelFiles/`.

## 18. Validation Matrix
| Rule | Layer | Message |
|---|---|---|
| Any file | none | - |
| Valid extension | implicit (.xls/.xlsx else NRE) | - |
| Non-empty first cell | server (rows skipped) | - |
| Submit: file uploaded / rows present / invoice exists / amount matches | **none** | - |

## 19. Business Rules
| BR ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M07-058 | Upload accepts `.xls` (Jet OLEDB) or `.xlsx` (ACE OLEDB) - extension taken from the client file name; file saved under `~/ExcelFiles/` with the client-supplied name; only the first worksheet is read; rows with an empty first column are dropped; the grid needs columns named `ProformaInvoiceNo` and `Amount`. | `DeliveryExcelUploadOldData.aspx.cs:651-700`; `.aspx:165-190` | CONFIRMED |
| BR-M07-059 | Submit calls `sp_DeliveryConformationFull_OldData(InvoiceNo, LoginName, today)` once per grid row, using the (editable) Invoice No text; the Amount column is ignored server-side. | `.aspx.cs:578-596` | CONFIRMED |
| BR-M07-060 | Success message depends only on the return of the LAST row's SP call; earlier rows' failures are invisible; rows already confirmed before a failure are not rolled back. | `.aspx.cs:578-596` | CONFIRMED |
| BR-M07-061 | This path performs no accounting journal call (`PostDeliveryConfFullJournal` is not invoked), unlike LoadingSummary Sales Confirm; whether the SP posts internally is UNKNOWN. | `.aspx.cs:578-596`; compare `LoadingSummary.aspx.cs:615-637` | CONFIRMED (call absent) / UNKNOWN (SP) |

## 20. Permission Matrix
No Session/role check except `Session["LoginName"]` (NullReference if absent). Menu-only access to a bulk delivery-confirmation function.

## 21. Status Lifecycle
Intended transition: proforma (`DelivaryInvoiceNo IS NULL`) -> delivery-confirmed (INFERRED via `sp_DeliveryConformationFull_OldData`). No status column set by the page itself.

## 22. Error Handling
Unhandled exceptions on upload; swallowed exceptions in dead email helpers. Message via `alert()`.

## 23. Audit
`@UpdateBy` = LoginName, `@UpdateDate` = today. The uploaded file itself is retained in `~/ExcelFiles/`.

## 24. Integration
Excel via OLEDB providers (Jet/ACE must exist on the server). Dead SMTP helpers (`email()`, `email2()`, `SenMailForApprved`) with hard-coded credentials and addresses (values not recorded; GAP-M07-025) - never called from the active flow (grep in file).

## 25. Reports
No report file referenced from this page.


## 26. Dependencies
BLL: InvoiceBLL, RequisitionBLL, DCStoreBLL (leftovers). DAL: InvoiceDAL. Libraries: System.Data.OleDb, System.Net.Mail.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-266-01 | upload valid .xlsx with 3 rows | grid 3 rows, "3 record Found!" | 058 |
| TC-266-02 | upload .csv/.txt | NullReferenceException | 058 |
| TC-266-03 | row with blank first cell | dropped | 058 |
| TC-266-04 | submit 3 rows | 3 SP calls; message if last returns 1 | 059/060 |
| TC-266-05 | second row invalid | first row committed, message depends on last | 060 |
| TC-266-06 | invoice already confirmed | behaviour UNKNOWN (SP) | 059 |
| TC-266-07 | accounting | no journal from page | 061 |
| TC-266-08 | duplicate file name | overwritten in ExcelFiles | 058 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/DeliveryExcelUploadOldData.aspx`, `Solution.Web/SInventory_UI/DeliveryExcelUploadOldData.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).


## 29. Spec vs Actual
| Area | Specification | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | - | MISSING |
| Validation | None existed | none | GAP |
| Journal posting | None existed | not called | UNKNOWN |

## 30. Known Gaps
GAP-M07-025 (credentials), GAP-M07-026 (upload), GAP-M07-027 (no validation), GAP-M07-012 (SP absent); clone of requisition page (dead code); sibling `PostDeliveryConfFullJournal` not called.

## 31. Change Impact
`InvoiceDAL.SaveFullInvoiceOldData` used only by this page (grep in Solution.Web); `sp_DeliveryConformationFull_OldData` sibling of `sp_DeliveryConformationFull_New`. Related pages: LoadingSummary (PAGE-271), DelivaryInvoiceCreationAfterSalesConfirm "Update Old Data" (external link).
