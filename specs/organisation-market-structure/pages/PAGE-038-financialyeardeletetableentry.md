---
page_id: PAGE-038
module: M03
page: FinancialYearDeleteTableEntry
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-038 — FinancialYearDeleteTableEntry

> **Evidence level:** Tier A. `FinancialYearDeleteTableEntry.aspx` (150 lines) and `.aspx.cs` (526 lines) read in full; `FinancialYearDeleteTableDAL` (146 lines) and the relevant `UserInfoDAL` methods (`CreateOpeningBalance`, `GetArchiveFinancialYearById`, `IsOpeningBalanceExistsForFinancialYear`, `GetArchiveFinancialYearList`, `GetArchiveDatabaseList`) read. All SP bodies absent (`UNKNOWN`) — so what is *actually deleted/archived/carried forward* is only known from messages in C#. Process: `workflows/WF-M03-07-financial-year-opening-and-data-purge.md`. **High-risk destructive page.**

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / Module | PAGE-038 / M03 (financial-year master) |
| Name | FinancialYearDeleteTableEntry (breadcrumb "Delete Table Data", `.aspx:9`) |
| Kind | Entry / Transaction — year-end batch process launcher |
| Folder / master | DoctorModule_UI / NewMasterPage.master |
| Class | `DoctorModule_UI_FinancialYearDeleteTableEntry`; static `UserInfoDAL _userDal`, static `FinancialYearDeleteTableDAL _deleteTableDal` (`.cs:8-9`) |

## 2. Purpose
CONFIRMED (labels/messages): three year-end operations for a selected **financial year**: (a) **Opening Process** – create opening balances for the year; (b) **Production Table** ("Delete" mode) – delete (per the on-screen messages: archive into `…DeleteArchive` tables and then delete) the selected transactional table's rows dated within the financial year from the live database; (c) **Archive Table** – delete the selected table's rows for that date range from a chosen *archive database*.

## 3. Business Objective
INFERRED: keep the live database small at year-end: roll balances forward (opening), retire old transactional data (Order, Invoice, RX, DCR, TourPlan, Attendance, Expense) by first copying to archive structures, then purging.

## 4. Navigation
Menu path UNKNOWN (DB-driven). No inbound/outbound links; success alert on Opening reloads the page after 2 s (`ShowSuccessAlert(...,true)` `.cs:510-522`).

## 5. Route
`~/DoctorModule_UI/FinancialYearDeleteTableEntry.aspx`

## 6. Source Files
`Solution.Web/DoctorModule_UI/FinancialYearDeleteTableEntry.aspx(.cs)`; `Library.DAL/DoctorModule_DAL/FinancialYearDeleteTableDAL.cs`; `Library.DAL/DoctorModule_DAL/UserInfoDAL.cs` (lines 39-160); `Library.DAL/DataManager/DataAccessManager.cs`, `DataBase.cs`. Related list/entry pages (Tier B): PAGE-039 FinancialYearEntry, PAGE-040 FinancialYearView.

## 7. UI Control Inventory
| Control | Label | Required | Default | State | Source | Message |
|---|---|---|---|---|---|---|
| `ddlArchiveFinancialYear` (AutoPostBack) | Financial Year | Yes (all modes) | placeholder | – | `sp_GET_ArchiveFinancialYearList` → `FinancialYearId`/`FinancialYearDesc` | "Please Select Financial Year!" (Opening) / included in combined delete message |
| `ddlArchiveDatabase` (inside `pnlArchiveDatabaseRow`) | Database Name | Yes in *Archive* mode only | placeholder | shown only for Archive (JS toggle + server `Style["display"]`) | `sp_GET_ArchiveDbConnect_ByFinancialYearDesc(@FinancialYearDesc)` → `DataBaseName` | "Please Select Financial Year, Database Name and Delete Table!" |
| `rblProcessType` (not AutoPostBack) | Process Type: *Opening Process* (`Opening`, default), *Production Table* (`Delete`), *Archive Table* (`Archive`) | one | Opening | client-side toggle (`toggleProcessModeControls`, `.aspx:113-122`) | static | – |
| `ddlDeleteTable` (inside `pnlDeleteTableRow`) | Delete Table | Yes in Delete/Archive modes | placeholder | hidden in Opening mode | static list built in code: Order, Invoice, RX, DCR, TourPlan, Attendance, EXPENSE, DA, Leave, Mileage Claim (`.cs:70-86`) | "Please Select Financial Year and Delete Table!" |
| `btnOpening` | "Opening" | – | disabled until FY chosen | enabled only if **no** completed opening exists for the FY; text shows "Opening (Select FY)" / "Opening" / "Opening (Already Done)" | – | – |
| `btnDelete` | "Delete" / "Archive" / "Delete (Select FY)" / "Delete (Please Process Opening)" | – | disabled | enabled only if a completed opening **exists** for the FY; caption "Archive" (yellow) in Archive mode | – | – |
No confirmation dialog (no `OnClientClick`) on either button.

## 8. Field Mapping
| UI | Code | Parameter |
|---|---|---|
| FY id | `int.TryParse(ddlArchiveFinancialYear.SelectedValue)` → `tblFinancialYear.StartDate/EndDate` (inline SQL) | `@FromDate`, `@ToDate` (`.Date`) |
| FY id (string) | `CreateOpeningBalance(ddlArchiveFinancialYear.SelectedValue,…)` | `@FinancialYear` = **FinancialYearId string** (not description) |
| Database Name | `ddlArchiveDatabase.SelectedValue` | `@DatabaseName` (Archive mode) |
| Delete Table | `TryExecuteDeleteTableProcess(...)` selects DAL method | – |

## 9. Dropdown Mapping
Financial year: `UserInfoDAL.GetArchiveFinancialYearList` (`sp_GET_ArchiveFinancialYearList`); database list: `GetArchiveDatabaseList(desc)` (`sp_GET_ArchiveDbConnect_ByFinancialYearDesc`) reloaded on FY change using `SelectedItem.Text` (description); table list static in code.

## 10. Button Action Mapping
| Button | Handler | Chain | SP / SQL | Change |
|---|---|---|---|---|
| FY change | `ddlArchiveFinancialYear_SelectedIndexChanged` `.cs:88` | `LoadArchiveDatabase`; `UserInfoDAL.IsOpeningBalanceExistsForFinancialYear` | inline `SELECT COUNT(1) FROM dbo.tblOpeningBalanceFinancialYearLog WHERE FinancialYear=@FinancialYear AND Status='Completed'` | read |
| Opening | `btnOpening_Click` `.cs:153` | validate mode+FY → `TryGetSelectedFinancialYearRange` (inline `SELECT … FROM dbo.tblFinancialYear WHERE FinancialYearId=@FinancialYearId`) → `UserInfoDAL.CreateOpeningBalance` | `sp_opeingBalanceCreate(@FinancialYear,@FromDate,@ToDate)` (sic) | write (UNKNOWN: opening balances) |
| Delete / Archive | `btnDelete_Click` `.cs:198` | validate → range → `TryExecuteDeleteTableProcess` → `FinancialYearDeleteTableDAL.*` | see §16 | **destructive** |

## 11. Frontend Flow
Mode radio is client-side only (jQuery show/hide) and re-bound on every partial postback via `pageLoad()`; server mirrors it in `ApplyProcessModeSelection()` (`.cs:463-493`, called at the end of `Page_Load` on every request). FY change posts back (UpdatePanel) and switches button states. Success alerts use `ShowSuccesalert(...)` (Opening also reloads), failures `faildalert(...)`.

## 12. Backend Flow
`Page_Load` (first load): load FY list, archive DB list (empty), delete-table list; disable both buttons (captions "(Select FY)"); always `ApplyProcessModeSelection()`.
**FY change**: reload DB list; if FY chosen → `IsOpeningBalanceExistsForFinancialYear(FinancialYearId)`: exists ⇒ Opening disabled ("Opening (Already Done)"), Delete enabled and an alert "Opening balance already exists for this financial year. Opening process is disabled, delete process is enabled."; none ⇒ Opening enabled, Delete disabled ("Delete (Please Process Opening)"). Exception ⇒ Opening enabled, Delete disabled ("Delete (Error)"). No FY ⇒ both disabled.
**Opening**: requires Opening radio; FY selected; FY date range found and `end >= start`; then `CreateOpeningBalance` (single implicit transaction; exception caught inside DAL → `isSuccess=false`, `ErrorMessage`) → success message "Opening balance created successfully for {start:dd-MMM-yyyy} to {end:dd-MMM-yyyy}." + reload after 2 s, else "Opening balance creation failed." or the SQL error text.
**Delete/Archive**: refuses if Opening radio selected ("Please select Delete Table or Archive Table."); requires FY, table (and DB in Archive mode); range check; then per table:
- *Production mode* (`Delete`): `sp_DeleteArchive<Table>Data(@FromDate,@ToDate)` executed on **`DataBase.SalesDB`** via `ExecuteDelete` (the `databaseName` argument of the public methods is ignored, `FinancialYearDeleteTableDAL.cs:48-111`).
- *Archive mode*: `sp_DeleteArchiveDB<Table>Data(@DatabaseName,@FromDate,@ToDate)` executed on SalesDB via `ExecuteDeleteArch` (`ResolveDatabaseName` falls back to SalesDB if blank).
- Tables supported: Order, Invoice, RX, DCR, TourPlan, Attendance, EXPENSE. **DA, Leave, Mileage Claim** are offered in the dropdown but return the message "Delete logic is configured only for Order, Invoice, RX, DCR, TourPlan, Attendance and Expense."
- DAL methods swallow exceptions: `SqlConnectionClose(true)` (rollback), `isSuccess=false`, `ErrorMessage=exception.Message` (SQL text is shown to the user); the page additionally wraps everything in `try/catch → ShowFailAlert(ex.Message)`.
Success text describes intent, e.g. Production/Invoice: "Invoice data archived into tblInvoiceDeleteArchive/tblInvoiceDetailDeleteArchive and deleted by tblInvoice.InvoiceDate between …" (message only — actual SP behaviour UNKNOWN).

## 13. API Mapping
None.

## 14. Service Mapping
No BLL.

## 15. Repository Mapping
| DAL method | Source | SP / SQL |
|---|---|---|
| `UserInfoDAL.GetArchiveFinancialYearList` | UserInfoDAL.cs:126 | `sp_GET_ArchiveFinancialYearList` |
| `UserInfoDAL.GetArchiveDatabaseList` | :145 | `sp_GET_ArchiveDbConnect_ByFinancialYearDesc` |
| `UserInfoDAL.GetArchiveFinancialYearById` | :69 | inline `SELECT FinancialYearId,FinancialYearDesc,StartDate,EndDate FROM dbo.tblFinancialYear WHERE FinancialYearId=@FinancialYearId` |
| `UserInfoDAL.IsOpeningBalanceExistsForFinancialYear` | :98 | inline `SELECT COUNT(1) … tblOpeningBalanceFinancialYearLog … Status='Completed'` |
| `UserInfoDAL.CreateOpeningBalance` | :39 | `sp_opeingBalanceCreate` |
| `FinancialYearDeleteTableDAL.DeleteArchiveOrder/Invoice/Rx/DCR/Attendance/TourPlan/ExpenseData` | FinancialYearDeleteTableDAL.cs:48-82 | `sp_DeleteArchive{Order,Invoice,Rx,DCR,Attendance,TourPlan,Expense}Data` |
| `FinancialYearDeleteTableDAL.DeleteArchiveDB*Data` | :16-37 | `sp_DeleteArchiveDB{Order,Invoice,Rx,DCR,TourPlan,Attendance,Expense}Data` |

## 16. Stored Procedure Mapping
All **bodies NOT in repository**. Parameters: Production: `@FromDate,@ToDate`; Archive: `@DatabaseName,@FromDate,@ToDate`; Opening: `@FinancialYear,@FromDate,@ToDate`; lists as §15. Tables named only in on-screen messages (INFERRED from message text, not code): `tblOrder, tblOrderDetail, tblInvoice, tblInvoiceDetail, tbl_PrescriptionMaster, tbl_PrescriptionProductDetail, tblPrescriptionApprovalLog, tbl_DCRInfo, tbl_DcrDetails, tbl_DcrBrandDetails, tbl_DcrVisitedWithDetails, tbl_TourPlanMaster, tbl_TourPlanInfo, tblMarketAttendance_Master_webapi, tbl_ExpenseClaim, tbl_ExpenseClaimDetails` and the `…DeleteArchive` copies.

## 17. Database Mapping
Inline SQL reads: `dbo.tblFinancialYear` (`FinancialYearId, FinancialYearDesc, StartDate, EndDate`), `dbo.tblOpeningBalanceFinancialYearLog` (`FinancialYear, Status` — 'Completed' means an opening was finished). Writes only via SPs (UNKNOWN). Connection: `DataAccessManager` opened against `DataBase.SalesDB` (name `<redacted>`; credentials are hard-coded in `SqlUserAccess.cs` — see GAPS).

## 18. Validation Matrix
| Rule | Where | Message | BR |
|---|---|---|---|
| Opening needs Opening radio | `.cs:155-159` | "Please select Opening Process." | BR-M03-070 |
| Opening needs FY | `.cs:161-165` | "Please Select Financial Year!" | BR-M03-070 |
| Delete needs non-Opening radio | `.cs:200-204` | "Please select Delete Table or Archive Table." | BR-M03-071 |
| Delete needs FY + table (+DB when Archive) | `.cs:206-214` | "Please Select Financial Year and Delete Table!" / "…, Database Name and Delete Table!" | BR-M03-071 |
| FY range valid | `.cs:290-316` | "Invalid Financial Year selected." / "Financial year date range not found." / "Financial year date range is invalid." | BR-M03-072 |
| Table supported | `.cs:318-450` | "Delete logic is configured only for Order, Invoice, RX, DCR, TourPlan, Attendance and Expense." | BR-M03-073 |
| Opening ⇒ delete order | UI state only | button caption | BR-M03-068/069 |

## 19. Business Rules
| ID | Rule | Source | Impl | Conf |
|---|---|---|---|---|
| BR-M03-068 | Opening balance for a financial year can be created **only if** `tblOpeningBalanceFinancialYearLog` has no row with `Status='Completed'` for that FY (enforced by disabling the button after FY selection — the server handler itself does not re-check). | FinancialYearDeleteTableEntry.aspx.cs:88-127,153-196; UserInfoDAL.cs:98-121 | page/DAL | CONFIRMED |
| BR-M03-069 | Purge/archive is enabled **only after** a completed opening exists for that FY (enabled state set in the FY change handler; `btnDelete_Click` does not re-check). | .cs:88-127,198-263 | page | CONFIRMED |
| BR-M03-070 | Opening requires the Opening process radio and a financial year. | .cs:155-165 | page | CONFIRMED |
| BR-M03-071 | Delete/Archive requires FY + table (+ archive DB in Archive mode) and a non-Opening process. | .cs:200-214 | page | CONFIRMED |
| BR-M03-072 | Date range comes from `tblFinancialYear.StartDate/EndDate` for the chosen FY and must satisfy `end.Date >= start.Date`; all SPs receive whole dates (`.Date`, i.e. inclusive-day semantics depend on the SP). | .cs:265-316; FinancialYearDeleteTableDAL.cs | page/DAL | CONFIRMED |
| BR-M03-073 | Only Order, Invoice, RX, DCR, TourPlan, Attendance and Expense can be purged/archived; DA, Leave, Mileage Claim appear in the list but are rejected. | .cs:70-86,318-450 | page | CONFIRMED |
| BR-M03-074 | "Production Table" mode calls `sp_DeleteArchive<Table>Data(@FromDate,@ToDate)` on the live SalesDB (the selected archive DB name is ignored). | FinancialYearDeleteTableDAL.cs:48-111 | DAL | CONFIRMED |
| BR-M03-075 | "Archive Table" mode calls `sp_DeleteArchiveDB<Table>Data(@DatabaseName,@FromDate,@ToDate)`; messages say the rows are removed from `<db>.dbo.<table>` **by date column** (Order: `SubmissionDate`, Invoice: `InvoiceDate`, RX: `PrescriptionDate`, DCR: `DcrDate`, Attendance: `AttendanceDate`, Expense: `EntryDate`; TourPlan: unspecified) — message text only. | .cs:318-450 | page messages | CONFIRMED (text) / UNKNOWN (SP) |
| BR-M03-076 | Each operation is one DB transaction (single SP call); on failure the transaction is rolled back and the SQL error text is displayed to the user. | FinancialYearDeleteTableDAL.cs:83-140; UserInfoDAL.cs:39-66 | DAL | CONFIRMED |
| BR-M03-077 | No confirmation prompt, no second-person approval, no audit record written by C# for these irreversible operations. | .aspx:93-98; whole page | – | CONFIRMED (absence) |

## 20. Permission Matrix
No `Session[...]` key is read at all on this page (no user id passed to the SPs). No role check. Access = menu visibility only; the master page's login redirect applies; `Web.config` has no `<authorization>`. Any authenticated user with the URL can run year-end deletes.

## 21. Status Lifecycle
Financial-year processing state (from `tblOpeningBalanceFinancialYearLog.Status`): *(none)* → *Completed* (written by `sp_opeingBalanceCreate` — INFERRED). Purge availability is gated by *Completed*. Other status values UNKNOWN.

## 22. Error Handling
Page-level try/catch around Opening/Delete → `ShowFailAlert(ex.Message)` (raw exception text); DAL catch converts to `ErrorMessage`. FY-change handler catch re-enables Opening on error. No logging.

## 23. Audit
None in C# (no user id, no log table write). The SPs may log (UNKNOWN); `tblOpeningBalanceFinancialYearLog` is read-only from this page.

## 24. Integration
Archive databases (per FY) are selected by name from `sp_GET_ArchiveDbConnect_ByFinancialYearDesc` — cross-database access presumably by 3-part names inside SPs (UNKNOWN).

## 25. Reports
None.

## 26. Dependencies
`tblFinancialYear` (maintained by PAGE-039/040), opening balance SP, archive databases, all purged transactional tables (Order/Invoice/RX/DCR/TourPlan/Attendance/Expense modules).

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Load page | both buttons disabled, Opening mode | §12 |
| TC-02 | Select FY without completed opening | Opening enabled, Delete disabled | BR-M03-068/069 |
| TC-03 | Select FY with completed opening | alert; Opening disabled; Delete enabled | BR-M03-068/069 |
| TC-04 | Run Opening | success message with dates, page reload | BR-M03-072/076 |
| TC-05 | Production mode, no table | "Please Select Financial Year and Delete Table!" | BR-M03-071 |
| TC-06 | Archive mode, no DB | "…Database Name and Delete Table!" | BR-M03-071 |
| TC-07 | Table = Leave/DA/Mileage Claim | "Delete logic is configured only for …" | BR-M03-073 |
| TC-08 | Production mode Order | `sp_DeleteArchiveOrderData` on SalesDB; ignore DB dropdown | BR-M03-074 |
| TC-09 | Archive mode Invoice | `sp_DeleteArchiveDBInvoiceData(@DatabaseName…)` | BR-M03-075 |
| TC-10 | SP error | rollback; SQL text shown | BR-M03-076 |
| TC-11 | Forge POST to `btnDelete` for FY without opening | server has no re-check — verify | BR-M03-069 |
| TC-12 | FY with EndDate < StartDate | "Financial year date range is invalid." | BR-M03-072 |

## 28. Reverse Engineering References
`Solution.Web/DoctorModule_UI/FinancialYearDeleteTableEntry.aspx(.cs)`, `Library.DAL/DoctorModule_DAL/FinancialYearDeleteTableDAL.cs`, `UserInfoDAL.cs:39-160`.

## 29. Spec vs Actual
| Area | Spec | Actual | DB | Status | Ref |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| "Delete" archives first? | None existed | only in success text | SP absent | UNKNOWN | BR-M03-075 |
| Mode naming | None existed | radio "Production Table" ↔ `Delete`, "Archive Table" ↔ `Archive` | – | CONFIRMED | .aspx:64-66 |

## 30. Known Gaps
Destructive, un-audited, un-confirmed operations; button-state-only gating (server handlers do not re-check opening/eligibility); `databaseName` ignored in production mode; FY string passed as id not description; no current-year protection (FY list SP may restrict — UNKNOWN); unsupported tables listed; hard-coded credentials in `SqlUserAccess.cs` used by every DAL call; typo `sp_opeingBalanceCreate`; SP bodies absent (GAP-M03-001).

## 31. Change Impact
Purges data used by Order/Invoice/RX/DCR/TourPlan/Attendance/Expense modules; `UserInfoDAL` is shared with user-setup pages; `tblFinancialYear` is maintained by PAGE-039/040 and read by many pages outside this module.
