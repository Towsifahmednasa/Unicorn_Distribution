# M17 - SAP, Mobile-App & External Integration

Reviewed 2026-09-19. Evidence tags CONFIRMED / INFERRED / UNKNOWN. SP bodies are absent except: `sp_SAP_B2B*`, `sp_SAP_ChallanSend*`, `sp_SAP_Up_SAP_Challan*` (in `SP_NewStockTransferDcToDc_Related.sql`) and the two provider-dropout SPs (`Library.DAL/SAP_IntegrationDAL/sql/`).

## 1. Purpose
Operator screens and endpoints through which the ERP exchanges data with SAP (master data, stock movements, challan confirmation, bank deposits, MIGO files), with an external e-Program (provider dropouts), with API-captured customers, and with the field-force mobile app; plus the AJAX/JSON lookup endpoints (`CommonDataLoad`, `SeedData`, `Setup` page methods, ASMX). Integration-by-integration detail: [`../integrations/README.md`](../integrations/README.md).

## 2. Page catalogue (19 pages)

| Page ID | Page | Kind | Tier | Spec |
|---|---|---|---|---|
| PAGE-676 | DoctorModule_UI/AppMonitoringList | List (app usage monitor) | B | [spec](pages/PAGE-676-appmonitoringlist.md) |
| PAGE-677 | DoctorModule_UI/CommonDataLoad | JSON lookups (55) | A | [spec](pages/PAGE-677-commondataload.md) |
| PAGE-678 | DoctorModule_UI/SeedData | JSON lookups (13) | A | [spec](pages/PAGE-678-seeddata.md) |
| PAGE-679 | DoctorModule_UI/Setup | JSON set-up/approval methods (43) | A | [spec](pages/PAGE-679-setup.md) |
| PAGE-680 | SAP_Integration/SAP_IntrigationPoint | SAP console | A | [spec](pages/PAGE-680-sap-intrigationpoint.md) |
| PAGE-681 | SAP_Integration/SAP_IntrigationPointDIC | SAP console (DIC) | A | [spec](pages/PAGE-681-sap-intrigationpointdic.md) |
| PAGE-682 | SAP_Integration/SAP_StockReceive | Challan approve | A | [spec](pages/PAGE-682-sap-stockreceive.md) |
| PAGE-683 | SAP_Integration/SAP_StockReceiveDIC | Challan approve (DIC) | B | [spec](pages/PAGE-683-sap-stockreceivedic.md) |
| PAGE-684 | SInventory_RPTVIEW/MIGOReportViewer | Crystal viewer | B | [spec](pages/PAGE-684-migoreportviewer.md) |
| PAGE-685 | SInventory_UI/ApiCustomerList | List/promote | B | [spec](pages/PAGE-685-apicustomerlist.md) |
| PAGE-686 | SInventory_UI/BankDepositSAP | SAP posting | A | [spec](pages/PAGE-686-bankdepositsap.md) |
| PAGE-687 | SInventory_UI/ImportedApiCustomer | Edit | B | [spec](pages/PAGE-687-importedapicustomer.md) |
| PAGE-688 | SInventory_UI/MIGO_Upload | Excel import | B | [spec](pages/PAGE-688-migo-upload.md) |
| PAGE-689 | SInventory_UI/MIGOList | List/transfer/delete | B | [spec](pages/PAGE-689-migolist.md) |
| PAGE-690 | SInventory_UI/MIGOReport | Report launcher | B | [spec](pages/PAGE-690-migoreport.md) |
| PAGE-691 | SInventory_UI/SalesReturnReportSAP | Report | B | [spec](pages/PAGE-691-salesreturnreportsap.md) |
| PAGE-692 | SInventory_UI/SAPDashboard | Status dashboard | B | [spec](pages/PAGE-692-sapdashboard.md) |
| PAGE-693 | SInventory_UI/ViewMIGODetailList | Detail list | B | [spec](pages/PAGE-693-viewmigodetaillist.md) |
| PAGE-694 | eProgram_UI/ProviderDropoutRequestList | List + approve | A | [spec](pages/PAGE-694-providerdropoutrequestlist.md) |

Tier-A: 5 listed candidates + PAGE-682, 686, 694 (list size + 3).

## 3. Business-rule index

| ID | One-liner | Where |
|---|---|---|
| BR-M17-001 | Header panel from `sp_Get_SAP_IntrigationPointHeader` | 680 |
| BR-M17-002 | Pending SAP challan list; already handled challans hidden | 680/681/682 |
| BR-M17-003 | List-level approve -> `sp_SAP_StockReceive`, no validation | 680/681 |
| BR-M17-004 | Red row + hidden Approve when code empty / qty 0 / B2B stock check 0 | 682 |
| BR-M17-005 | Approve: qty non-empty > 0; per-line qty overwrite then receive, not atomic | 682 |
| BR-M17-006 | Employee sync with `sp_Chk_SAP_EmpInfoCondition` pre-check | 680/681 |
| BR-M17-007 | Product sync, no pre-check | 680/681 |
| BR-M17-008 | B2B challan master from SAP movement (SQL script) | INT-02 |
| BR-M17-009 | B2B detail FEFO deduction; Quantity=@Qty and stale-variable defects | INT-02 |
| BR-M17-010 | B2B receive inserts `tblDCStore` Available/Receive | INT-02 |
| BR-M17-011 | Outbound challan payload + send/confirm flags | INT-03 |
| BR-M17-012 | `MakeRESTRequest` after DC receive, errors swallowed | INT-03 |
| BR-M17-013 | Bank deposit JSON payload contract | 686 |
| BR-M17-014 | Bank deposit: date rule, process SP on every search, send disabled | 686 |
| BR-M17-015 | Provider dropout approve (idempotent, UTC) | 694 |
| BR-M17-016 | Provider dropout list ordering and DDL | 694 |
| BR-M17-017 | MIGO upload/transfer/delete (delete only if StockUpload=false) | 688/689 (B) - `MIGO_Upload.aspx.cs:70-160`, `MIGOList.aspx.cs:40-60` |
| BR-M17-018 | DIC scoping by `DICCompanyUnitId` | 681 |
| BR-M17-019 | Page-method endpoints have no auth checks | 677/678/679 |
| BR-M17-020 | Sub-territory scoping by role; anonymous gets all | 679 |
| BR-M17-021 | API customer promote rules | 685 (B) - `ApiCustomerList.aspx.cs:57-150` |
| BR-M17-022 | SAP dashboard statuses (IDoc / send / confirm) | 692 (B) - `SAPDashboard.aspx.cs:101-140` |
| BR-M17-023 | ASMX customer autocomplete scoped by ComUnitId | `SInventoryWebService.cs:102-140` |
| BR-M17-024 | AppMonitoring page permission check | 676 (B) - `AppMonitoringList.aspx.cs:48-75` |
| BR-M17-025 | Approvals/deletes in Setup page methods; delete without session | 679 |
| BR-M17-026 | User/employee lookups exposed | 677 |
| BR-M17-027 | Field-force roster lookups exposed | 678 |

## 4. Status lifecycles
- SAP challan: staged (`action`) -> received by `sp_SAP_StockReceive` -> hidden (INFERRED). ERP challan: `SAP_ChallanSend` 0/1, `SAP_Challan_ConfirmationSend` 0/1.
- Provider dropout: `IsApprove` 0 -> 1.
- API customer: `AddtoMainCustomer` 0 -> 1. MIGO: `StockUpload` 0 -> 1.
- Bank deposit: intended staged -> sent (sent step disabled).
- Customer app approval ActionStatus 0 Pending / 1 Verified / 2 Approved / 3 Rejected.

## 5. Roles / permissions
Session keys: `UserId`, `RoleTypeName`, `DICCompanyUnitId`, `EmpInfoId`, `RoleTypeId`, `ComUnitId`, `UserType`, `LoginName`, `UserRoleID`. Enforced role logic: DIC unit filter, Setup sub-territory scoping, AppMonitoring page permission, ASMX customer scoping. Everything else is menu-based; page methods and ASMX have no checks.

## 6. Data model
SAP staging DB `SAP_API_Data` (`tblSAP_StockMovementMaster/Detail`), `tblChalanInfo` (`SAP_ChallanSend`, `SAP_Challan_ConfirmationSend`), `tblChalanDetail`, `tblDCStore`, `tblWHStockInMaster`, `tblProduct.SAP_Code`, `tblCompanyUnit.SAP_Code`, `tblStockUOM.UOMSAPCode`, `tblProviderDropoutIntrigration`, `tbltempCustMaster`, `tblMIGOMaster/Detail`, `tblCustMaster`. SPs listed in the INT files.

## 7. Integrations / reports / workflows
Integrations: `../integrations/`. Reports: [reports/README.md](reports/README.md). Workflows: [WF-M17-01](workflows/WF-M17-01-sap-stock-receive.md), [WF-M17-02](workflows/WF-M17-02-sap-master-data-approval.md), [WF-M17-03](workflows/WF-M17-03-bank-deposit-to-sap.md), [WF-M17-04](workflows/WF-M17-04-provider-dropout-approval.md).

## 8. Known gaps / unverified
See [GAPS-M17.md](GAPS-M17.md). Unverified: all SP bodies except those listed; how SAP feeds staging; mobile-app side; Tier-B pages (683-685, 687-693, 676) only skimmed; anonymous reachability is INFERRED from missing `<authorization>`.
