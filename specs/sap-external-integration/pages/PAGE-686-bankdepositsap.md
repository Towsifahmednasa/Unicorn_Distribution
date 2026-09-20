---
page_id: PAGE-686
module: M17
page: BankDepositSAP
tier: A
generated: false   # upgraded by manual code review (Tier A)
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-686 — BankDepositSAP

> **Evidence level:** Tier A - reviewed manually against `.aspx`, `.aspx.cs`, and the BLL/DAL code they reach (review date 2026-09-19). Claims are tagged CONFIRMED / INFERRED / UNKNOWN. Stored-procedure bodies are NOT in the repository unless a section says so; column-level effects of such SPs stay `UNKNOWN`. Sections 1, 4-10, 13-15 and 28 still carry the mechanically extracted content (re-checked where noted); all other sections were rewritten.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-686 |
| Module | M17 — SAP, Mobile-App & External Integration (INFERRED from folder + file name) |
| Page name | BankDepositSAP |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_BankDepositSAP |
| aspx size | 20253 bytes |
| Code-behind LOC | 387 |
| Inline script size | 751 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
"Bank Deposit (SAP)": for a date range the page **prepares and lists customer bank-deposit postings** (customer code, amount, deposit/value date, bank account no, reference, cheque no) in the layout SAP expects, lets the user **export CSV**, and has a **Final Submit** that builds the SAP JSON payload `{"Posting":[{CustomerCode, Amount, ValueDate, BankAccountNo, Reference, CQNumber}]}` to POST to a SAP REST adapter. **The HTTP call and the "mark as sent" update are commented out**, so Final Submit currently transmits nothing and marks nothing. CONFIRMED: `Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs:236-349`.

## 3. Business Objective
Post collections/bank deposits recorded in the ERP to SAP finance (customer receipts) - intended automatic hand-off; currently semi-manual (CSV export).

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `businesssummaryviewer.aspx`

## 5. Route
`~/SInventory_UI/BankDepositSAP.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/BankDepositSAP.aspx |
| Code-behind | Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | OrderInfoBLL (Library.BLL/SInventory_BLL/OrderInfoBLL.cs) |
| BLL class | TotalSummaryBLL (Library.BLL/SInventory_BLL/TotalSummaryBLL.cs) |
| DAL class | BonusCampaignNewDAL (Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |
| DAL class | TotalSummaryDAL (Library.DAL/SInventory_DAL/TotalSummaryDAL.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| fromDateTextBox | TextBox | Select  From Date | no validator found |  |  |  | OnTextChanged→fromDateTextBox_TextChanged |
| toDateTextBox | TextBox | Select To Date | no validator found |  |  |  |  |
| LinkButton1 | LinkButton |  | no validator found |  |  |  | OnClick→viewRptButton_Click |
| LinkButton2 | LinkButton |  | no validator found |  |  |  | OnClick→btnExportToExcel_Click |
| loadGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Customer Code | no validator found |  |  |  |  |
| lblCustomerCode | Label | <%# Eval("CustomerCode") %> | no validator found |  |  |  |  |
| hfSL | HiddenField |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Amount | no validator found |  |  |  |  |
| lblAmount | Label | <%# Eval("Amount") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Deposit Date | no validator found |  |  |  |  |
| lblDepositDate | Label | <%# Eval("DepositDate") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Bank Account No | no validator found |  |  |  |  |
| lblBankAccountNo | Label | <%# Eval("BankAccountNo") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Reference | no validator found |  |  |  |  |
| lblReference | Label | <%# Eval("Reference") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | CQ Number | no validator found |  |  |  |  |
| lblCQNumber | Label | <%# Eval("CQNumber") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Send to SAP | no validator found |  |  |  |  |
| lblIS_SAP_MigrationDone | Label | <%# Convert.ToBoolean(Eval("IS_SAP_MigrationDone")) ? "Yes" : "No" %> | no validator found |  |  |  |  |
| btnFinalSubmit | LinkButton |  | no validator found |  |  |  | OnClick→btnFinalSubmit_Click, OnClientClick→return sweetAlertConfirm_Submit(this); |
| restbtn | LinkButton |  | no validator found |  |  |  | OnClick→restbtn_Click |
| rptTypeDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→rptTypeDropDownList_SelectedIndexChanged |
| zoneDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| territoryDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| viewRptButton | Button | Search | no validator found |  |  |  | OnClick→viewRptButton_Click |
| excelButton1 | Button | Export to Excel | no validator found |  |  |  | OnClick→btnExportToExcel_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| fromDateTextBox | inputDateTime | SInventory_UI_BankDepositSAP.fromDateTextBox_TextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | Res | SInventory_UI_BankDepositSAP.LoadInfo | TotalSummaryBLL.LoadBankDepositSAP | BonusCampaignNewDAL.BankDeposit_SAP_Process, TotalSummaryDAL.BankDepositPosting_SAP | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | aDataTable | SInventory_UI_BankDepositSAP.LoadInfo | TotalSummaryBLL.LoadBankDepositSAP | BonusCampaignNewDAL.BankDeposit_SAP_Process, TotalSummaryDAL.BankDepositPosting_SAP | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | fromDate | SInventory_UI_BankDepositSAP.excelButton1_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| toDateTextBox | toDate | SInventory_UI_BankDepositSAP.excelButton1_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| lblCustomerCode | customerCode | SInventory_UI_BankDepositSAP.btnExportToExcel_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| lblAmount | amount | SInventory_UI_BankDepositSAP.btnExportToExcel_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| lblDepositDate | depositDate | SInventory_UI_BankDepositSAP.btnExportToExcel_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| lblBankAccountNo | bankAccountNo | SInventory_UI_BankDepositSAP.btnExportToExcel_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| lblReference | reference | SInventory_UI_BankDepositSAP.btnExportToExcel_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| lblCQNumber | cqNumber | SInventory_UI_BankDepositSAP.btnExportToExcel_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | dt | SInventory_UI_BankDepositSAP.btnFinalSubmit_Click | TotalSummaryBLL.LoadBankDepositSAP | TotalSummaryDAL.BankDepositPosting_SAP | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| rptTypeDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| zoneDropDownList | DropDownlist → OrderInfoBLL.LoadZone | inline SQL | Region/RegionCode | tblRegion |
| salesCenterDropDownList | DropDownlist → OrderInfoBLL.LoadSC | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| territoryDropDownList | DropDownlist → OrderInfoBLL.LoadTerritory | inline SQL | AreaName/AreaCode | tblArea, tblInvoice |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| LinkButton1 | LinkButton click | (postback) | SInventory_UI_BankDepositSAP.viewRptButton_Click | Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs:56 | TotalSummaryBLL.LoadBankDepositSAP | BonusCampaignNewDAL.BankDeposit_SAP_Process, TotalSummaryDAL.BankDepositPosting_SAP | sp_SAP_BankDepositPosting, sp_SAP_BankDeposit_SAP_Process | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
|  | LinkButton click | (postback) | SInventory_UI_BankDepositSAP.Unnamed_Click | Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs:369 | — | — | — | read-only / none detected |
| LinkButton2 | LinkButton click | (postback) | SInventory_UI_BankDepositSAP.btnExportToExcel_Click | Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs:162 | — | — | — | read-only / none detected |
| btnFinalSubmit | LinkButton click | return sweetAlertConfirm_Submit(this); | SInventory_UI_BankDepositSAP.btnFinalSubmit_Click | Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs:237 | TotalSummaryBLL.LoadBankDepositSAP | TotalSummaryDAL.BankDepositPosting_SAP | sp_SAP_BankDepositPosting | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| restbtn | LinkButton | — | restbtn_Click | — | — | — | — | UNKNOWN (handler not resolved) |
| viewRptButton | Button click | (postback) | SInventory_UI_BankDepositSAP.viewRptButton_Click | Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs:56 | TotalSummaryBLL.LoadBankDepositSAP | BonusCampaignNewDAL.BankDeposit_SAP_Process, TotalSummaryDAL.BankDepositPosting_SAP | sp_SAP_BankDepositPosting, sp_SAP_BankDeposit_SAP_Process | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| excelButton1 | Button click | (postback) | SInventory_UI_BankDepositSAP.btnExportToExcel_Click | Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs:162 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- `Page_Load` (first): From/To date = today, drop-downs `LoadSC`, `LoadTerritory`, `LoadZone` (`OrderInfoBLL`, Session UserId scoping) - the three drop-downs are not used in the query.
- `fromDateTextBox_TextChanged`: dates before 01-Apr-2022 are reset to `01 April, 2022`.
- `viewRptButton_Click` -> `LoadInfo()`: requires both dates (`Please Select Date Range!!`); calls `BonusCampaignNewDAL.BankDeposit_SAP_Process(from,to)` (`sp_SAP_BankDeposit_SAP_Process`), **then** `TotalSummaryBLL.LoadBankDepositSAP` (`sp_SAP_BankDepositPosting @FrmDate,@ToDate`) and binds `loadGridView`; empty -> `No Data Found!!`. Any exception is swallowed (`catch (Exception) { }`).
- `btnExportToExcel_Click`: writes CSV `Bank Deposit (SAP)_<timestamp>.csv` from the grid labels (`lblCustomerCode, lblAmount, lblDepositDate, lblBankAccountNo, lblReference, lblCQNumber`), quoting values with commas/quotes.
- `btnFinalSubmit_Click`: re-reads `LoadBankDepositSAP`, maps rows to `BankDepositModel`, `JsonConvert.SerializeObject`, sets `apiUrl` (SAP RESTAdapter `eph_mio` endpoint on a `smc-bd.org` host, port 42223) and Basic-auth username/password **hard-coded as string literals (redacted here)** - then the entire `HttpClient.PostAsync` block, the response parsing, and `BankDeposit_SAP_ProcessUpdate(hiddenFieldValues)` (`sp_SAP_BankDepositSendtoSAP @Ids`) are commented out. Result: no side effect.
- `excelButton1_Click` opens `BusinessSummaryViewer.aspx` (unrelated report - copy/paste).

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_BankDepositSAP.Page_Load | Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs:25 | object sender, EventArgs e | SInventory_UI_BankDepositSAP.Page_Load → OrderInfoBLL.LoadSC / OrderInfoBLL.LoadTerritory / OrderInfoBLL.LoadZone → OrderInfoDAL.LoadSC / OrderInfoDAL.LoadTerritory / OrderInfoDAL.LoadZone | no | no |
| SInventory_UI_BankDepositSAP.fromDateTextBox_TextChanged | Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs:37 | object sender, EventArgs e | SInventory_UI_BankDepositSAP.fromDateTextBox_TextChanged | no | no |
| SInventory_UI_BankDepositSAP.DropDownlist | Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs:48 |  | SInventory_UI_BankDepositSAP.DropDownlist → OrderInfoBLL.LoadSC / OrderInfoBLL.LoadTerritory / OrderInfoBLL.LoadZone → OrderInfoDAL.LoadSC / OrderInfoDAL.LoadTerritory / OrderInfoDAL.LoadZone | no | no |
| SInventory_UI_BankDepositSAP.viewRptButton_Click | Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs:56 | object sender, EventArgs e | SInventory_UI_BankDepositSAP.viewRptButton_Click → TotalSummaryBLL.LoadBankDepositSAP → BonusCampaignNewDAL.BankDeposit_SAP_Process / TotalSummaryDAL.BankDepositPosting_SAP → SP: sp_SAP_BankDepositPosting, sp_SAP_BankDeposit_SAP_Process | no | yes |
| SInventory_UI_BankDepositSAP.LoadInfo | Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs:61 |  | SInventory_UI_BankDepositSAP.LoadInfo → TotalSummaryBLL.LoadBankDepositSAP → BonusCampaignNewDAL.BankDeposit_SAP_Process / TotalSummaryDAL.BankDepositPosting_SAP → SP: sp_SAP_BankDepositPosting, sp_SAP_BankDeposit_SAP_Process | yes | yes |
| SInventory_UI_BankDepositSAP.excelButton1_Click | Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs:112 | object sender, EventArgs e | SInventory_UI_BankDepositSAP.excelButton1_Click | no | no |
| SInventory_UI_BankDepositSAP.loadGridView_PageIndexChanging | Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs:135 | object sender, GridViewPageEventArgs e | SInventory_UI_BankDepositSAP.loadGridView_PageIndexChanging | no | no |
| SInventory_UI_BankDepositSAP.rptTypeDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs:140 | object sender, EventArgs e | SInventory_UI_BankDepositSAP.rptTypeDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_BankDepositSAP.btnExportToExcel_Click | Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs:162 | object sender, EventArgs e | SInventory_UI_BankDepositSAP.btnExportToExcel_Click | no | no |
| SInventory_UI_BankDepositSAP.btnFinalSubmit_Click | Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs:237 | object sender, EventArgs e | SInventory_UI_BankDepositSAP.btnFinalSubmit_Click → TotalSummaryBLL.LoadBankDepositSAP → TotalSummaryDAL.BankDepositPosting_SAP → SP: sp_SAP_BankDepositPosting | yes | no |
| SInventory_UI_BankDepositSAP.Unnamed_Click | Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs:369 | object sender, EventArgs e | SInventory_UI_BankDepositSAP.Unnamed_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderInfoBLL.LoadSC | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:91 |
| OrderInfoBLL.LoadTerritory | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:334 |
| OrderInfoBLL.LoadZone | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:338 |
| TotalSummaryBLL.LoadBankDepositSAP | Library.BLL/SInventory_BLL/TotalSummaryBLL.cs:152 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OrderInfoDAL.LoadSC | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1935 | inline SQL: tblCompanyUnit |  |
| OrderInfoDAL.LoadTerritory | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2591 | inline SQL: tblArea, tblInvoice | @ComUnitId |
| OrderInfoDAL.LoadZone | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2613 | inline SQL: tblRegion |  |
| BonusCampaignNewDAL.BankDeposit_SAP_Process | Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs:80 | sp_SAP_BankDeposit_SAP_Process | @fromdate, @todate |
| TotalSummaryDAL.BankDepositPosting_SAP | Library.DAL/SInventory_DAL/TotalSummaryDAL.cs:244 | sp_SAP_BankDepositPosting | @FrmDate, @ToDate |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_SAP_BankDepositPosting | TotalSummaryDAL.BankDepositPosting_SAP | body NOT in repository | write-like name |
| sp_SAP_BankDeposit_SAP_Process | BonusCampaignNewDAL.BankDeposit_SAP_Process | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblArea, tblCompanyUnit, tblInvoice, tblRegion, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Date Range!! | UNKNOWN |
| (handler excelButton1_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Date Range!! | UNKNOWN |
| (handler btnFinalSubmit_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | alert('Error: | UNKNOWN |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M17-014 | **Bank-deposit hand-off.** (1) Date range required (`Please Select Date Range!!`); From date not earlier than 01-Apr-2022 (auto-reset, no message). (2) **Every search** first runs `sp_SAP_BankDeposit_SAP_Process(@fromdate,@todate)` - a write-like processing SP (body UNKNOWN; presumably stages/flags rows for SAP) - then reads `sp_SAP_BankDepositPosting(@FrmDate,@ToDate)`; a read-only search therefore has side effects. (3) Final Submit builds JSON but does not send; `sp_SAP_BankDepositSendtoSAP(@Ids)` (would flag rows as sent) is never called, so no row is ever marked sent by this page and re-running re-processes the same range. (4) Errors in `LoadInfo` are silently swallowed. | `BankDepositSAP.aspx.cs:55-101,236-349`; `BonusCampaignNewDAL.cs:78-135`; `TotalSummaryDAL.cs:245-253` | code-behind + DAL + SP | CONFIRMED |
| BR-M17-013 | **Payload contract (as coded).** `BankDepositDAO { Posting: List<BankDepositModel> }`; `BankDepositModel { CustomerCode, Amount, ValueDate (= DepositDate), BankAccountNo, Reference, CQNumber }`, all strings (NULL -> `""`). | `BankDepositSAP.aspx.cs:254-274,374-386` | code-behind | CONFIRMED |

## 20. Permission Matrix
Session keys: `UserId` (drop-down scoping only). No role check; **SAP credentials are hard-coded in the page** (see §30). Menu-based access.

## 21. Status Lifecycle
Intended lifecycle (evidence: commented code): staged -> exported/posted -> marked sent via `sp_SAP_BankDepositSendtoSAP`. Only the first step is live; sent-flag column/table UNKNOWN.

## 22. Error Handling
- `LoadInfo`: empty catch. `btnFinalSubmit_Click`: catch shows `alert('Error: <message>')` (message text escaped only for quotes).
- Empty results -> `showMessageBox('No Data Found!!')` / `faildalert('No data found!')`.
- No retry/timeout handling (HTTP code disabled).

## 23. Audit
None written by the page; SP audit UNKNOWN. SP bodies are NOT in the repository (only `sp_SAP_B2B*`, `sp_SAP_ChallanSend*`, `sp_SAP_Up_SAP_Challan*` in `SP_NewStockTransferDcToDc_Related.sql` and the two provider-dropout SPs in `Library.DAL/SAP_IntegrationDAL/sql/`).

## 24. Integration
**Outbound SAP finance posting (disabled).** See `specs/integrations/INT-04-sap-bank-deposit-posting.md`.

## 25. Reports
CSV export only (no Crystal). Related report page PAGE-691 `SalesReturnReportSAP`.

## 26. Dependencies
BLL: OrderInfoBLL, TotalSummaryBLL; DAL: BonusCampaignNewDAL, OrderInfoDAL, TotalSummaryDAL; DAO: ResultInfo; Tables (inline): tblArea, tblCompanyUnit, tblInvoice, tblRegion, tblUserCompanyUnit

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-686-01 | Positive | Choose a date range with deposits, View | grid populated (`sp_SAP_BankDepositPosting`) after `sp_SAP_BankDeposit_SAP_Process` ran | BR-M17-014 |
| TC-686-02 | Positive | Export | CSV with 6 columns | BR-M17-014 |
| TC-686-03 | Negative | Clear a date | `Please Select Date Range!!` | BR-M17-014 |
| TC-686-04 | Edge | From date 2021-01-01 | auto-set to 01 April 2022 | BR-M17-014 |
| TC-686-05 | Edge | Final Submit | no HTTP request, no SP call, no message | BR-M17-014 |
| TC-686-06 | Failure | `sp_SAP_BankDeposit_SAP_Process` throws | grid unchanged, no message | BR-M17-014 |
| TC-686-07 | Security | Inspect repo/page source | SAP endpoint credentials present in source (must be rotated) | §30 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/BankDepositSAP.aspx`, `Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | Final Submit -> SAP | n/a | MISSING | - |
| SAP posting | Intended (by name) | disabled | unknown | DEFECT/INCOMPLETE | BR-M17-014 |

## 30. Known Gaps
- **Hard-coded SAP REST endpoint credentials** (Basic-auth username and password literals) in `BankDepositSAP.aspx.cs:276-277`; also over HTTPS to an external SAP host - credentials must be treated as compromised (redacted here).
- Final Submit is a no-op; sent-flag never set; process SP runs on every search (side effect on read).
- Swallowed exceptions; page copies (`excelButton1_Click` opens an unrelated report); unused Territory/Zone/SC drop-downs.
- `sp_SAP_BankDeposit_SAP_Process`, `sp_SAP_BankDepositPosting`, `sp_SAP_BankDepositSendtoSAP` bodies missing (GAP-M17-001).
- `BankDeposit_SAP_Process` DAL uses `UpdateData` (single transaction) but page ignores its result on failure.

## 31. Change Impact
Same SPs used by: none other (grep). `TotalSummaryBLL.LoadBankDepositSAP` also feeds nothing else. `BonusCampaignNewDAL` is a large shared DAL.

