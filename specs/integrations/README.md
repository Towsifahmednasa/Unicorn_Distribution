# Integrations index

Proven from code (2026-09-19). Nothing here was tested against live systems; SP bodies are mostly not in the repo. No secrets recorded: hard-coded credentials exist in `Library.DAL/DataManager/SqlUserAccess.cs`, `Solution.Web/Web.config` and `Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs` (all `<redacted>`).

| ID | Integration | Direction | Transport | State |
|---|---|---|---|---|
| [INT-01](INT-01-sap-master-data-sync.md) | SAP employee/product master sync | SAP -> ERP | staging tables + operator approve | live (staging producer UNKNOWN) |
| [INT-02](INT-02-sap-stock-movement-receive.md) | SAP stock movement / STO / B2B challan receive | SAP -> ERP | `SAP_API_Data` DB + SPs | live |
| [INT-03](INT-03-sap-challan-send-confirm-outbound.md) | Challan send + receipt confirmation | ERP -> SAP | `MakeRESTRequest` SP + flags | live, errors swallowed |
| [INT-04](INT-04-sap-bank-deposit-posting.md) | Bank deposit posting | ERP -> SAP | HTTPS/JSON (disabled), CSV | HTTP disabled |
| [INT-05](INT-05-sap-idoc-status-and-reports.md) | SAP status dashboard / return report | read-only | DB | live |
| [INT-06](INT-06-provider-dropout-inbound.md) | Provider dropout requests | external -> ERP | table + page methods | live |
| [INT-07](INT-07-migo-excel-import.md) | MIGO Excel import | file -> ERP | upload + OLEDB | live |
| [INT-08](INT-08-api-customer-staging-import.md) | API customer staging import | external -> ERP | staging table | live |
| [INT-09](INT-09-mobile-app-shared-database.md) | Field-force mobile app | bidirectional | shared DB, WebView pages | INFERRED |
| [INT-10](INT-10-rest-base-url-config.md) | `BASE_URL`/`AppName` config | - | none | dead config |
| [INT-11](INT-11-asmx-autocomplete-webservice.md) | `SInventoryWebService` ASMX | browser -> ERP | ASMX/script service | live |
| [INT-12](INT-12-page-method-json-endpoints.md) | Page-method JSON endpoints | browser -> ERP | ASP.NET page methods | live, unauthenticated |
| [INT-13](INT-13-excel-file-imports.md) | Excel imports (target, deposit, payment, customer) | file -> ERP | upload + OLEDB | live |
| [INT-14](INT-14-external-geoip-and-db-jobs.md) | ipapi.co GeoIP, SQL Agent backup job | ERP -> external | HTTPS / msdb | live |

Cross-cutting: authentication is Forms + InProc session; there is no `<authorization>` element, so endpoints not rendering a master page (ASMX, page methods) are only protected if they read Session (BR-M17-019). Writes go through `DataAccessManager`: one lazy transaction per DAL call, commit on close, rollback on exception, so "Already Exist!" branches after `SaveData` are unreachable.
