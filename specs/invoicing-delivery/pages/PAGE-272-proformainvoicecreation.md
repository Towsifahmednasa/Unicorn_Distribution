---
page_id: PAGE-272
module: M07
page: ProformaInvoiceCreation
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---

# PAGE-272 — ProformaInvoiceCreation

> **Evidence level:** Tier A - reviewed 2026-09-19 by reading the .aspx, .aspx.cs, the BLL/DAL methods reached, and every SP/SQL body present in the repository. Confidence tags: CONFIRMED (read in implementation), INFERRED (implied by several references), UNKNOWN (not derivable; SP bodies are absent from the repo except where stated). Module: M07 - Proforma, Invoicing & Delivery.


## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-272 |
| Module | M07 - Proforma, Invoicing & Delivery (CONFIRMED: folder `SInventory_UI`, invoice generation from orders) |
| Page name | ProformaInvoiceCreation |
| Kind | Entry / Transaction (bulk "Process Invoice" of selected orders) - but see BR-M07-002: the order grid is never populated, so the page is effectively **non-functional/legacy** |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master (`Solution.Web/SInventory_UI/ProformaInvoiceCreation.aspx:1`) |
| Page class | SInventory_UI_ProformaInvoiceCreation |
| aspx size | 11287 bytes |
| Code-behind LOC | 181 (`Solution.Web/SInventory_UI/ProformaInvoiceCreation.aspx.cs`) |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | YES (INFERRED): `GridView()` body is entirely commented out (`ProformaInvoiceCreation.aspx.cs:103-126`); the only inbound reference is a redirect from `SubDepot_UI/SubDepotInvoiceCreation.aspx.cs:160`. The by-order invoice pages of M06 (`../order-management/pages/PAGE-217-invoicecreationbyorder.md`, `PAGE-218-invoicecreationforcustomerbyorder.md`) are the live path. |

## 2. Purpose
CONFIRMED (labels/code): title "Proforma Invoice Creation" (`ProformaInvoiceCreation.aspx:15`). Intended purpose: let a distribution-center user pick a sales center (DC), manufacturer and market, list the not-yet-invoiced orders, tick several of them and press **Process Invoice**, which asks the database to generate a proforma invoice for each ticked order by calling `sp_AutoInvoiceGeneration` once per order (`ProformaInvoiceCreation.aspx.cs:155`, `InvoiceDAL.cs:297-303`).
**As shipped, the list step is dead:** `Button1_Click` and `Page_Load` call `GridView()`, whose body only contains commented-out code, so `orderGridView` never receives data and the loop in `btnSubmit0_Click` iterates zero rows (BR-M07-002).

## 3. Business Objective
INFERRED business objective: convert customer orders (`tblOrder`, `IsInvoice=0`) into proforma invoices in bulk, without opening each order (the manual alternative is the M06 by-order page). A proforma invoice is the first invoice stage (`tblInvoice.DelivaryInvoiceNo IS NULL`) that reserves DC stock; delivery confirmation later turns it into a delivery invoice (see `../workflows/WF-M07-01-proforma-to-delivery-lifecycle.md`). What `sp_AutoInvoiceGeneration` actually writes (header, details, stock issue, VAT/discount math) is **UNKNOWN - body not in repository**.

## 4. Navigation
Menu placement is DB-driven (`tblMainMenu`, `tblMenuDistribution`, `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu row is not in the repository -> path UNKNOWN. Note the page uses **MainMasterPage.master**, not NewMasterPage.master used by newer M07 pages.

Inbound (CONFIRMED by grep): `Solution.Web/SubDepot_UI/SubDepotInvoiceCreation.aspx.cs:160` (`Response.Redirect("ProformaInvoiceCreation.aspx")`).
Outbound: hidden grid column "Go To Invoice" (`Visible="False"`, `ProformaInvoiceCreation.aspx:174`) would redirect to `InvoiceCreationForCustomerByOrder.aspx` with `Session["OrderId"]` (`ProformaInvoiceCreation.aspx.cs:54-63`); unreachable because the column is invisible and the grid is empty.

## 5. Route
`~/SInventory_UI/ProformaInvoiceCreation.aspx` (file-based WebForms route; no `MapPageRoute` alias).



## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/ProformaInvoiceCreation.aspx |
| Code-behind | Solution.Web/SInventory_UI/ProformaInvoiceCreation.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | InvoiceBLL (Library.BLL/SInventory_BLL/InvoiceBLL.cs) |
| BLL class | OrderInfoBLL (Library.BLL/SInventory_BLL/OrderInfoBLL.cs) |
| DAL class | InvoiceDAL (Library.DAL/SInventory_DAL/InvoiceDAL.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |



## 7. UI Control Inventory
| Control ID | Type | Label | Required | Default | Read-only / Enabled / Visible | Data source | Events -> handler |
|---|---|---|---|---|---|---|---|
| salesCenterDropDownList | DropDownList | "Sales Center" | no validator; no server check | `SelectedIndex = 1` after load (`.aspx.cs:34`) - i.e. first real item, item 0 is the blank placeholder | Enabled | `OrderInfoBLL.LoadSC(ddl, Session["UserId"])` -> tblCompanyUnit filtered by tblUserCompanyUnit(UserId) (`OrderInfoDAL.cs:978-993`) | AutoPostBack -> `salesCenterDropDownList_SelectedIndexChanged` (reloads markets, clears grid) |
| manufacDropDownList | DropDownList | "Manufacture" | none | `SelectedIndex = 1` (`.aspx.cs:33`) | Enabled | `OrderInfoBLL.LoadManufac` -> tblManufacturer | AutoPostBack -> empty handler (`.aspx.cs:43`) |
| marketDropDownList | DropDownList | "Market" | none | first item / `Session["MarketId"]` if set | Enabled | `OrderInfoBLL.LoadMarketOrderWise(ddl, ComUnitId)` -> distinct markets of un-invoiced orders (`IsInvoice = 0`) whose customer belongs to the sales center (`OrderInfoDAL.cs:1148-1160`) | AutoPostBack -> empty handler (`.aspx.cs:47`, body commented) |
| Button1 | Button | "Search" | - | - | Enabled | - | `Button1_Click` -> `GridView()` (no-op) |
| btnSubmit | Button | "Process Invoice" | none - no confirm dialog, no "at least one row" check | - | Enabled | - | `btnSubmit0_Click` |
| orderGridView | GridView (`DataKeyNames="ComUnitId,ManufacId,OrderId,OrderCode"`, footer on) | columns Order No, Order Date (dd-MMM-yyyy), Customer Code, Customer Name, Market, MIO Code, MIO Name, Gross Value(TP), [Go To Invoice - `Visible="False"`], checkbox | - | never bound (BR-M07-002) | - | none in effect | - |
| chkSelectAll (header) / chkSelect (row) | CheckBox | - | - | unchecked | AutoPostBack on header only | - | `chkSelectAll_CheckedChanged` ticks/unticks all rows |
| progress | UpdateProgress | spinner | - | - | - | - | - |

Sales Center list is per-user (BR-M07-004); the Manufacturer filter is displayed but not passed to any active query.

## 8. Field Mapping
No field-to-DTO mapping exists; the page posts a single string per order to the SP.

| UI source | Handler | BLL | DAL | SP parameter | Effect |
|---|---|---|---|---|---|
| `orderGridView.DataKeys[i][3]` (= `OrderCode`) | `btnSubmit0_Click` (`.aspx.cs:155`) | `InvoiceBLL.SaveProformaInvoice(invoiceNo, updateby)` (`InvoiceBLL.cs:78`) | `InvoiceDAL.SaveProformaInvoice` (`InvoiceDAL.cs:297`) | `@OrderCodeInPut` (parameter is misleadingly named `InvoiceNo` in C#) | UNKNOWN (body absent) |
| `Session["UserId"]` | same | same | same | `@UserId` | UNKNOWN |

## 9. Dropdown Mapping
| Dropdown | Loader | SQL | Display / Value | Tables |
|---|---|---|---|---|
| salesCenterDropDownList | `DropDownlist()` -> `OrderInfoBLL.LoadSC(ddl, userId)` | inline: `select ComUnitId, ComUnitName from tblCompanyUnit WHERE ComUnitId IN (SELECT CompanyUnitId FROM tblUserCompanyUnit WHERE UserId=@UserId)` (`OrderInfoDAL.cs:986-993`) | ComUnitName / ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| manufacDropDownList | `OrderInfoBLL.LoadManufac` (`OrderInfoDAL.cs:2219`) | inline SELECT on tblManufacturer | ManufacName / ManufacId | tblManufacturer |
| marketDropDownList | `salesCenterDropDownList_SelectedIndexChanged` -> `LoadMarketOrderWise` | inline (see §7) with `@ComUnitId` | MarketName / MarketId | tblOrder, View_CustomerMaster, tblCompanyUnit, tblMarket |

## 10. Button Action Mapping
| Button | Event | Handler | Service | Repository | SP | DB change |
|---|---|---|---|---|---|---|
| Button1 "Search" | click | `Button1_Click` -> `GridView()` | none | none | none | none (no-op) |
| btnSubmit "Process Invoice" | click (no client confirm) | `btnSubmit0_Click` (`.aspx.cs:144`): for each row i with `chkSelect.Checked` -> `aInvoiceBll.SaveProformaInvoice(DataKeys[i][3], Session["UserId"])`; afterwards ALWAYS `showMessageBox("Invoice Generated Successfully")` and `GridView()` | `InvoiceBLL.SaveProformaInvoice` | `InvoiceDAL.SaveProformaInvoice` -> `ClsCommonInternalDAL.RunStoreProcedure` (`ClsCommonInternalDAL.cs:551`, timeout 99000000 s, no transaction wrapper) | `sp_AutoInvoiceGeneration(@OrderCodeInPut,@UserId)` | UNKNOWN |
| gotoinvoiceButton (hidden) | click | `gotoinvoiceButton_Click` -> `Session["OrderId"]=...; Redirect("InvoiceCreationForCustomerByOrder.aspx")` | - | - | - | none |

## 11. Frontend Flow
No client script. ASP.NET postbacks inside an UpdatePanel with UpdateProgress spinner. Flow: Page_Load(!PostBack) -> `DropDownlist()` (sales centers, manufacturers, select index 1) -> `salesCenterDropDownList_SelectedIndexChanged(sender,e)` (loads markets, clears grid) -> `SessionChoose()` (body commented out) -> if `Session["MarketId"]` exists select it and call `GridView()` (no-op). Nothing ever sets `Session["MarketId"]` on this page (the assignment at `.aspx.cs:111-113` is commented out), so the market restore path only fires if another page set it.

## 12. Backend Flow
| Handler | Source | Branches | try/catch | Txn |
|---|---|---|---|---|
| Page_Load | `.aspx.cs:15-28` | `!IsPostBack` only | no | no |
| DropDownlist | `.aspx.cs:30-35` | reads `Session["UserId"].ToString()` (NullReference if session expired, unhandled) | no | no |
| salesCenterDropDownList_SelectedIndexChanged | `.aspx.cs:37-42` | reload markets; clear grid | no | no |
| chkSelectAll_CheckedChanged | `.aspx.cs:86-101` | copies header check state to every row | no | no |
| GridView | `.aspx.cs:103-126` | all statements commented; wrapped in `try { } catch (Exception) { }` that swallows | yes (empty catch) | no |
| btnSubmit0_Click | `.aspx.cs:144-183` | loop rows -> SP call per checked row; the `if (status == 1)` guard is commented out (`.aspx.cs:159`) so the success message is unconditional | no | no - each SP call is its own connection/command |

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.



## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderInfoBLL.LoadSC | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:91 |
| OrderInfoBLL.LoadManufac | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:117 |
| OrderInfoBLL.LoadMarketOrderWise | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:79 |
| InvoiceBLL.SaveProformaInvoice | Library.BLL/SInventory_BLL/InvoiceBLL.cs:79 |



## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OrderInfoDAL.LoadSC | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1935 | inline SQL: tblCompanyUnit |  |
| OrderInfoDAL.LoadManufac | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2219 | inline SQL: tblManufacturer |  |
| OrderInfoDAL.LoadMarketOrderWise | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2287 | inline SQL: tblCompanyUnit, tblMarket, tblOrder | @ComUnitId |
| InvoiceDAL.SaveProformaInvoice | Library.DAL/SInventory_DAL/InvoiceDAL.cs:297 | sp_AutoInvoiceGeneration | @OrderCodeInPut, @UserId |



## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Known / unknown |
|---|---|---|---|
| sp_AutoInvoiceGeneration | `InvoiceDAL.SaveProformaInvoice` (`InvoiceDAL.cs:297-303`); also `SalesReturnDAL.cs:329` | **body NOT in repository** (not in `sp_bodies.json`, not in any root `*.sql`) | Parameters `@OrderCodeInPut` (order code string), `@UserId` (session UserId as string). Header/detail creation, VAT/discount/total calculation, stock issue (tblDCStore Issue rows), `tblOrder.IsInvoice` / `tblOrderDetail.Status` update: all UNKNOWN. |

## 17. Database Mapping
- READ (inline SQL): tblCompanyUnit, tblUserCompanyUnit, tblManufacturer, tblOrder, View_CustomerMaster, tblMarket.
- WRITTEN by this page's own SQL: none.
- WRITTEN via `sp_AutoInvoiceGeneration`: UNKNOWN. By analogy with the manual/by-order path (`InvoiceBLL.SaveInvoice` -> tblInvoice, tblInvoiceBatch, tblInvoiceDetail, tblDCStore 'Issue' rows; see BR-M07-011..018) and with `sp_Delete_ProformaInvoice`, which reverses `tblOrder.IsInvoice`, `tblOrderDetail.Status`, `tblDCStore`, the SP very probably touches the same tables - INFERRED only, do not rely on it.

## 18. Validation Matrix
| Field / rule | Client | Server | Message |
|---|---|---|---|
| Sales center / manufacturer / market | none | none | - |
| At least one order ticked | none | none (zero ticked rows still shows success) | "Invoice Generated Successfully" (`.aspx.cs:161`) |
| SP result | none | `status` captured but ignored (`.aspx.cs:159` commented) | - |
| Session expiry | - | `Session["UserId"].ToString()` NRE, no message | - |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M07-001 | "Process Invoice" invokes `sp_AutoInvoiceGeneration(@OrderCodeInPut = order code of the row, @UserId = session UserId)` once per ticked row; invoice creation logic itself lives in the database and is UNKNOWN. | `ProformaInvoiceCreation.aspx.cs:150-157`; `InvoiceDAL.cs:297-303` | code-behind loop + DAL | CONFIRMED (call) / UNKNOWN (effect) |
| BR-M07-002 | The order list is never loaded: `GridView()` has no active statements, so `orderGridView` stays empty and Process Invoice iterates nothing (page is dead code in practice). | `ProformaInvoiceCreation.aspx.cs:103-126,140-143` | code-behind | CONFIRMED |
| BR-M07-003 | Success message "Invoice Generated Successfully" is shown after every Process Invoice click regardless of SP outcome or number of rows (status check commented out). | `ProformaInvoiceCreation.aspx.cs:159-163` | code-behind | CONFIRMED |
| BR-M07-004 | Sales Center choices are limited to the units assigned to the logged-in user in `tblUserCompanyUnit`; market choices are limited to markets that still have un-invoiced orders (`IsInvoice = 0`) for that unit. | `OrderInfoDAL.cs:986-993, 1151-1155` | inline SQL | CONFIRMED |
| BR-M07-005 | Defaults: first real Sales Center (index 1) and first real Manufacturer (index 1) are preselected. | `ProformaInvoiceCreation.aspx.cs:33-34` | code-behind | CONFIRMED |

## 20. Permission Matrix
- Authentication: Forms auth + master page (`MainMasterPage.master`) redirects to Login when `Session["UserId"]` missing (only if the master enforces it - not re-verified here; see M01).
- Session keys read: `UserId` (Sales Center filter and SP `@UserId`), `MarketId`, `OrderId` (both set elsewhere).
- No role/UserType check in code. Authorization = menu-only (DB-driven). No DC-level check that the ticked orders belong to the user's DC (there are no orders in the grid, moot today).

## 21. Status Lifecycle
Order/invoice status touched (by the SP, UNKNOWN): the page's filter condition implies `tblOrder.IsInvoice = 0` -> 1. The page itself sets no status. Proforma lifecycle (verified elsewhere in M07): see README section "Status lifecycles" and `../workflows/WF-M07-01-proforma-to-delivery-lifecycle.md`.

## 22. Error Handling
No try/catch except the empty catch in `GridView()`. SP exceptions propagate as unhandled -> ASP.NET error page (Global.asax `Application_Error` is empty). `showMessageBox` escapes nothing (`message.Replace("'", "\'")` is a no-op in C#).

## 23. Audit
No audit columns written by the page. `@UserId` passed to the SP -> whether tblInvoice.UserId/CreateBy are set is UNKNOWN. No page-level logging.

## 24. Integration
None from the page. (`sp_AutoInvoiceGeneration` may touch accounting; UNKNOWN.)

## 25. Reports
None on this page.

## 26. Dependencies
BLL: InvoiceBLL, OrderInfoBLL. DAL: InvoiceDAL, OrderInfoDAL, ClsCommonInternalDAL. DAO: none. Tables (inline): tblCompanyUnit, tblUserCompanyUnit, tblManufacturer, tblOrder, View_CustomerMaster, tblMarket. SP: sp_AutoInvoiceGeneration (absent).

## 27. Test Cases
| ID | Scenario | Steps | Expected | BR |
|---|---|---|---|---|
| TC-272-01 | Page load | open as user with 2 sales centers | Sales Center defaults to first real unit; Manufacturer index 1; grid empty | BR-M07-005, 004 |
| TC-272-02 | Sales center change | select other unit | market list reloads to markets with `IsInvoice=0` orders; grid cleared | BR-M07-004 |
| TC-272-03 | Search | click Search | still an empty grid (no-op) - **documents defect** | BR-M07-002 |
| TC-272-04 | Process with nothing ticked | click Process Invoice | alert "Invoice Generated Successfully", no DB write | BR-M07-003 |
| TC-272-05 | Direct session-less access | request URL without session | NullReference/redirect per master page | - |
| TC-272-06 | (blocked) Process ticked order | requires grid population; would call `sp_AutoInvoiceGeneration` per ticked order | cannot be executed until grid is restored; effects UNKNOWN | BR-M07-001 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/ProformaInvoiceCreation.aspx`, `Solution.Web/SInventory_UI/ProformaInvoiceCreation.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).



## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | page present | n/a | MISSING | - |
| Order list | None existed | not loaded (commented out) | n/a | GAP | BR-M07-002 |
| Invoice generation | None existed | delegated to SP | body absent | UNKNOWN | BR-M07-001 |
| Success feedback | None existed | unconditional message | n/a | GAP | BR-M07-003 |

## 30. Known Gaps
- Dead/legacy page: `GridView()`, `SessionChoose()`, market handler bodies commented out (GAP-M07-001).
- Unconditional success message; SP failures and zero-row runs indistinguishable (GAP-M07-002).
- `sp_AutoInvoiceGeneration` body absent: VAT/discount/total rules, stock effects and idempotency (duplicate invoice for an already-invoiced order) are UNKNOWN (GAP-M07-003).
- `RunStoreProcedure` returns 1 only when `ExecuteNonQuery() > 0`; with `SET NOCOUNT ON` inside the SP the return would be -1 -> 0 (INFERRED; `status` is ignored anyway).
- Manufacturer dropdown is decorative (not in any query).
- Session-expiry NRE at `Session["UserId"].ToString()`.

## 31. Change Impact
- Shares `InvoiceDAL.SaveProformaInvoice` / `sp_AutoInvoiceGeneration` with `SalesReturnDAL` (grep `AutoInvoiceGeneration` in `Library.DAL/SInventory_DAL/InvoiceDAL.cs`, `SalesReturnDAL.cs`).
- `OrderInfoDAL.LoadSC/LoadManufac/LoadMarketOrderWise` are shared by many SInventory pages (delivery, top sheet, loading).
- Re-enabling `GridView()` would need `OrderInfoBLL.LoadOrderForOrderCreation` (referenced only in comments).
- Cross-links: M06 `../order-management/pages/PAGE-217-invoicecreationbyorder.md`, `../order-management/pages/PAGE-218-invoicecreationforcustomerbyorder.md`; sub-depot variant `../sub-depot/`.
