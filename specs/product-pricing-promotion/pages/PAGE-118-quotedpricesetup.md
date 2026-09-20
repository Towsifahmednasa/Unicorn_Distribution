---
page_id: PAGE-118
module: M04
page: QuotedPriceSetup
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "PARTIAL", "database": "PARTIAL", "rules": "PARTIAL", "permission": "PARTIAL", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-118 - QuotedPriceSetup (customer-specific negotiated product prices)

> **Evidence level:** Tier A. Read in full: `QuotedPriceSetup.aspx.cs` (868 lines), `QuotedPriceSetupDAL.cs`, `QuotedPriceView.aspx.cs`, aspx grid/button markup. SP bodies (`sp_Save_QuotedPriceMaster`, `sp_Update_QuotedPriceMaster`, `sp_Save_QuotedPriceDetail`, readers) are not in the repo => database effects UNKNOWN. No consumer of quoted prices was found anywhere else in the repo (grep `QuotedPrice`).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-118 |
| Module | M04 |
| Page name | QuotedPriceSetup |
| Kind | Entry / Transaction (create + edit) |
| Web folder / master | MasterSetup_UI / NewMasterPage.master |
| Page class | MasterSetup_UI_QuotedPriceSetup |
| Files | aspx 30,861 bytes, aspx.cs 868 lines; both last modified 2022-09-08 |
| Suspected legacy/duplicate | Possibly stale (unchanged since 2022) but linked from `QuotedPriceView.aspx`; live use UNKNOWN (menu) |

## 2. Purpose
Define a **quoted price** document for a customer: a description, a policy text, an active period, and for a selected set of products a special price expressed as a discount off the trade price (TP). CONFIRMED (labels: TP, Discount Percent (%), Discount Amount, Final Amount).

## 3. Business Objective
Let sales management grant a key customer a negotiated price list instead of the standard trade price. Downstream application to orders/invoices is not in this repo (UNKNOWN); INFERRED purpose from names.

## 4. Navigation
Inbound: `QuotedPriceView.aspx` (`Response.Redirect("QuotedPriceSetup.aspx")` for new; `QuotedPriceSetup.aspx?MID=<id>` for edit). Outbound: success alert redirects to `QuotedPriceView.aspx`.

## 5. Route
`~/MasterSetup_UI/QuotedPriceSetup.aspx[?MID=<QuotedPriceMasterId>]`.

## 6. Source Files
| Type | Path |
|---|---|
| View / code-behind | Solution.Web/MasterSetup_UI/QuotedPriceSetup.aspx(.cs) |
| User control | IVMarketStructure.ascx (inside hidden `divMarket`) |
| DAL (direct, no BLL) | Library.DAL/MasterSetup_DAL/QuotedPriceSetupDAL.cs |
| DAO | Library.DAO/MasterSetup_DAO/QuotedPriceMasterDAO.cs, QuotedPriceDetailDAO.cs; ResultInfo |
| Web service | App_Code/SInventoryWebService.cs (`GetCustomer` autocomplete) |

## 7. UI Control Inventory
| Control | Label | Type | Required (server) | Default / visibility | Source |
|---|---|---|---|---|---|
| id_mastetID | - | HiddenField | - | from `?MID` | QueryString |
| txtDescription | Description | TextBox multiline | YES (`:198`) | empty | user |
| txtPolicy | Policy | TextBox multiline | YES (`:207`) | empty | user |
| rbType | Customer Wise / Market Wise | RadioButtonList in a `runat=server visible=false` row | - | **hidden**, item 0 (Customer Wise) preselected | static |
| divMarket / IVMarketStructure | market cascade | user control | not validated | hidden (`visible=false`, shown only if Market Wise were chosen) | - |
| txtFromDate / txtToDate | Active From / To | TextBox (pickadate, text) | YES (`:216`,`:225`) | empty | user |
| divCus / custNameTextBox / hfCustomerId | Customer | AutoComplete TextBox + hidden id | see BR-M04-032 | visible | `SInventoryWebService.GetCustomer` |
| btnAdd | add customer | LinkButton | requires selected customer | visible only on new record | `btnAdd_Click` |
| gv_CustomerList | selected customers, Remove | GridView | >=1 on create | empty | in-memory |
| txtCmnPercent | Discount Percent (%) (common) | TextBox `clsDecimal`, AutoPostBack | no | empty | user |
| gv_ProductList | SL, select checkbox (`chkSelect`, header `chkSelectAll`), Product Code, Product Name, Description, **TP** (`txtUnitPrice` label), **Discount Percent (%)** (`txtVat` TextBox, AutoPostBack), **Discount Amount** (`txtDiscountPercent` TextBox, AutoPostBack), **Final Amount** (`txtDiscountShow` label) | GridView | >=1 checked; each checked row needs a Final Amount | all active products loaded (`sp_Get_Product_Active`) | DB |
| btnSave / btnUpdate / restbtn | Submit / Update / Reset | LinkButton | - | Save on new, Update on edit; `restbtn_Click` is an empty handler | - |

Control names are misleading: `txtVat` holds the *discount percent*, `txtDiscountPercent` holds the *discount amount*, `txtDiscountShow` holds the *final price*.

## 8. Field Mapping
| UI | DTO | SP param | Column |
|---|---|---|---|
| id_mastetID | QuotedPriceMasterDAO.QuotedPriceMasterId | @QuotedPriceMasterId | UNKNOWN |
| txtDescription / txtPolicy | Description / Policy | @Description / @Policy | UNKNOWN |
| rbType | IsCustomerWise / IsMarketWise (always Customer Wise: true/false) | @IsCustomerWise / @IsMarketWise | UNKNOWN |
| customer (per list row on create; hfCustomerId on edit) | CustomerMasterId | @CustomerMasterId | UNKNOWN |
| txtFromDate / txtToDate | ActiveFromDate / ActiveToDate (`Convert.ToDateTime`) | @ActiveFromDate / @ActiveToDate | UNKNOWN |
| market cascade selects | GroupId..MarketId | @GroupId,@RegionId,@AreaId,@TerritoryId,@SubTerritoryId,@MarketId | UNKNOWN (all NULL: control hidden) |
| Session["UserId"] | - | @EntryBy | UNKNOWN |
| row product id | QuotedPriceDetailDAO.ProductId | @ProductId | UNKNOWN |
| **Final Amount** (`txtDiscountShow`) | UnitPrice | @UnitPrice | UNKNOWN - **stores the discounted price** |
| **Discount Percent** (`txtVat`) | Vat | @Vat | UNKNOWN - **stores the discount %**, not VAT |

## 9. Dropdown Mapping
No dropdowns. Products from `sp_Get_Product_Active`; customers via auto-complete `SInventoryWebService.GetCustomer` (returns "Name : ... |Id"; page parses `Split('|')[1]` at `:170`).

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| btnAdd | `btnAdd_Click` `:699` | needs `hfCustomerId`; `AddMarket()` appends to `gv_CustomerList`; else alert "Please Select Customer!" |
| rmv_Customer | `:728` | remove row |
| chkSelectAll | `:132` | tick/untick all product rows |
| txtCmnPercent change | `:602` | copy % into every row's `txtVat`, recompute each row |
| txtVat change | `txtUnitPrice_TextChanged` `:447` | `Cal_UnitPrice_to_Percent(row)` |
| txtDiscountPercent change | `lblDiscountPercent_TextChanged` `:524` | `Cal_Percent_to_UnitPrice(row)` |
| btnSave | `btnSave_Click` `:314` | validate; **for each customer in the list** save a master + the checked products |
| btnUpdate | `btnUpdate_Click` `:742` | validate; save one master (`?MID`) + checked products |

## 11. Frontend Flow
No page JS calculations: **all price arithmetic runs server-side on AutoPostBack** (each edit of a % or amount textbox posts back; ASP.NET UpdatePanel). Date fields use pickadate.

## 12. Backend Flow
1. `Page_Load`: bind IVMarketStructure controls; first load: `Customer_gv_Initial()`, `LoadInitialInfo()` (all active products into `gv_ProductList`); `?MID` => `btnUpdate` visible, `btnAdd` hidden, `GetOneRecord(MID)`; else `btnSave` + `btnAdd`.
2. `GetOneRecord`: master via `sp_GET_QuotedPriceMaster_ById` (Description, Policy, ActiveFromDate, ActiveToDate, CustomerMasterId, CustomerName); details via `sp_Get_QuotedPriceDetailById`; for each grid product found in details: tick, `txtDiscountShow=UnitPrice`, `txtVat=Vat`, then `Cal_UnitPrice_to_Percent(i)` **recomputes the final price from the current list price and saved percent**, so the shown price may differ from the stored one if TP changed since (whole method in silent try/catch).
3. Save: see BR-M04-034 (customer loop) - each iteration `QuotedPriceSetupDAL.SaveMasterDetals` (own DataAccessManager transaction): master `sp_Save_QuotedPriceMaster` (returns PK) or `sp_Update_QuotedPriceMaster`, then `sp_Save_QuotedPriceDetail` per detail row.
4. Success => `successalert(..., 'QuotedPriceView.aspx')`; else "Already Exist!".

## 13. API Mapping
Auto-complete web method `GetCustomer` only.

## 14. Service Mapping
No BLL.

## 15. Repository Mapping
`QuotedPriceSetupDAL`: `GetQuotedPriceMasterById`, `GetDetailById`, `GetProductListActive`, `SaveMasterDetals` (also `GetCustomerListActive`, `GetProductListActiveForALl`, `GetQuotedPriceList`).

## 16. Stored Procedure Mapping
sp_Get_Product_Active, sp_GET_QuotedPriceMaster_ById, sp_Get_QuotedPriceDetailById, sp_Save_QuotedPriceMaster, sp_Update_QuotedPriceMaster, sp_Save_QuotedPriceDetail (params @QuotedPriceMasterId,@ProductId,@UnitPrice,@Vat). Bodies UNKNOWN (whether detail save deletes/overwrites existing rows on edit is unknown).

## 17. Database Mapping
Tables UNKNOWN by name (SP-only access). TP source column UNKNOWN (`Eval("UnitPrice")` from `sp_Get_Product_Active`; INFERRED = current `tblUnitPrice.UnitPrice`).

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| txtDescription, txtPolicy, txtFromDate, txtToDate | required | `Validation()` `:198-231` | tooltip "please fill out this field" | BR-M04-031 |
| customers | new: `gv_CustomerList` >=1; edit: `hfCustomerId` non-empty | `:233-255` | alert "Please Add to list Customer!!" / tooltip | BR-M04-032 |
| ticked rows | Final Amount not blank | `:262-281` | alert "Input Discount Percent (%) Or	Discount Amount!!" | BR-M04-033 |
| product selection | >=1 ticked | `:283-304` | alert "Please Select at least one row !!!" | BR-M04-033 |
| dates | From<=To not checked; numeric range of % (0..100) not checked; TP=0 not checked | - | - | BR-M04-035 |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M04-031 | A quoted price needs a description, policy text, active-from and active-to date. | QuotedPriceSetup.aspx.cs:198-231 | `Validation()` | CONFIRMED |
| BR-M04-032 | Customer rule: on **create** at least one customer must be in the list (added with btnAdd, must be chosen from the autocomplete: text must contain `:` else "Input Correct Data !!"); on **edit** the single customer shown in the textbox (`hfCustomerId`) is required. "Market Wise" mode is unreachable (radio row hidden) and even if chosen would still demand a customer. | QuotedPriceSetup.aspx.cs:163-187,233-255,699-718 | `Validation`,`custNameTextBox_TextChanged` | CONFIRMED |
| BR-M04-033 | At least one product row must be ticked and every ticked row must have a Final Amount; otherwise no save. | QuotedPriceSetup.aspx.cs:262-304 | `Validation()` | CONFIRMED |
| BR-M04-034 | **Create fan-out:** one quoted-price master is created **per customer in the list**, each with the same description/policy/dates and product rows. `DtlList` is declared **outside** the customer loop and appended to inside it, so for the 2nd, 3rd... customer the list already holds the earlier customers' rows and grows (N customers => customer k saves k copies of every product row) - duplicate detail rows unless `sp_Save_QuotedPriceDetail` de-duplicates (UNKNOWN). `Res` reflects only the last customer's save; each customer save is a separate transaction. | QuotedPriceSetup.aspx.cs:319-357,427-439 | `btnSave_Click` | CONFIRMED (code) / UNKNOWN (SP effect) |
| BR-M04-035 | **Price arithmetic (server-side, decimal, rounded to 3 places).** Let TP = list price (`txtUnitPrice`), P = Discount Percent (`txtVat`), A = Discount Amount (`txtDiscountPercent`). *Percent -> price* (`Cal_UnitPrice_to_Percent`, `:464-522`): `A = Round(TP * P / 100, 3)`; `Final = Round(TP - (TP - (TP - A)), 3)` = `Round(TP - TP*P/100, 3)`. Blank/invalid inputs count as 0 (silent catch). *Amount -> price* (`Cal_Percent_to_UnitPrice`, `:541-599`): `P = Round(A / TP * 100, 3)` (TP = 0 => DivideByZero swallowed, nothing updates); `Final = Round(TP - A, 3)`. *Common percent* (`txtCmnPercent`): sets every row's P (ticked or not) to the entered value and recomputes all rows. Rounding is banker's rounding (`Math.Round` default, MidpointRounding.ToEven). No limits: P may exceed 100 (negative Final), A may exceed TP. | QuotedPriceSetup.aspx.cs:447-614 | `Cal_*` | CONFIRMED |
| BR-M04-036 | **What is stored:** `UnitPrice` = Final Amount (discounted price), `Vat` = Discount Percent (blank => NULL). The column/param called "Vat" carries a discount percentage, not tax. Anything reading `@Vat` as tax will mis-compute. On edit-load the shown Final Amount is recomputed from *today's* TP and the stored percent (BR-M04-035), so the stored `UnitPrice` is only used as an interim value. | QuotedPriceSetup.aspx.cs:339-352,66-97; QuotedPriceSetupDAL.cs:203-209 | `btnSave_Click`, `GetOneRecord` | CONFIRMED |
| BR-M04-037 | Edit saves the master through `sp_Update_QuotedPriceMaster` and then calls `sp_Save_QuotedPriceDetail` for every ticked row again (no delete in C#). Unticking a product on edit does not remove it unless the SP reconciles (UNKNOWN). The quoted price is not deactivated by date: `ActiveFromDate/ActiveToDate` are stored only. | QuotedPriceSetupDAL.cs:179-210 | `SaveMasterDetals` | CONFIRMED (C#) / UNKNOWN (SP) |

## 20. Permission Matrix
Session `UserId` (EntryBy). No role check; menu-based. Roles UNKNOWN.

## 21. Status Lifecycle
No status column handled. Validity is the from/to date pair only; there is no deactivate/delete action in either the setup or the view page (`QuotedPriceView` only lists and edits).

## 22. Error Handling
`GetOneRecord`, all `Cal_*` methods and `txtUnitPrice_TextChanged` swallow every exception. `btnSave_Click`/`btnUpdate_Click` have no try/catch: `Convert.ToDecimal`/`Convert.ToDateTime` failures surface as the default error page. DAL rethrows after rollback.

## 23. Audit
`@EntryBy = Session["UserId"]` on save/update (a separate update-by is not passed). No history table.

## 24. Integration
None found in repo.

## 25. Reports
None.

## 26. Dependencies
DAL `QuotedPriceSetupDAL`; DAO `QuotedPriceMasterDAO`, `QuotedPriceDetailDAO`; AjaxControlToolkit; web service `SInventoryWebService.GetCustomer`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | TP 200, enter Discount % 10 | Discount Amount 20.000, Final 180.000 | BR-M04-035 |
| TC-02 | TP 200, enter Discount Amount 15 | Discount % 7.500, Final 185.000 | BR-M04-035 |
| TC-03 | TP 0, enter Discount Amount 5 | No change (divide by zero swallowed) | BR-M04-035 |
| TC-04 | TP 100, % 120 | Final -20.000 accepted (no guard) | BR-M04-035 |
| TC-05 | Common % 5 with 3 rows ticked, 2 unticked | All 5 rows get 5% | BR-M04-035 |
| TC-06 | New quote for 3 customers, 2 products | 3 masters; customer 3 gets 6 detail rows (unless SP dedupes) | BR-M04-034 |
| TC-07 | Save without customer | alert "Please Add to list Customer!!" | BR-M04-032 |
| TC-08 | Tick product but empty Final Amount | alert "Input Discount Percent (%) Or Discount Amount!!" | BR-M04-033 |
| TC-09 | Open edit after TP changed | Final Amount differs from stored | BR-M04-036 |
| TC-10 | From date after To date | Saved | BR-M04-031 |

## 28. Reverse Engineering References
`Solution.Web/MasterSetup_UI/QuotedPriceSetup.aspx(.cs)`, `QuotedPriceView.aspx.cs`, `Library.DAL/MasterSetup_DAL/QuotedPriceSetupDAL.cs`.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | read | SP bodies absent | MISSING | - |
| Market-wise quotes | None existed | option hidden, still needs customer | UNKNOWN | INCOMPLETE FEATURE | BR-M04-032 |
| "Vat" column semantics | None existed | holds discount % | UNKNOWN | ANOMALY | BR-M04-036 |

## 30. Known Gaps
Duplicate detail rows in multi-customer create (GAP-M04-014). Column named Vat holds discount % (GAP-M04-015). No consumer of quoted prices in repo (GAP-M04-016). No date/percent validation; no deactivate. All calculations server-side with swallowed errors. No try/catch on save.

## 31. Change Impact
`QuotedPriceSetupDAL.GetProductListActiveForALl` is shared by all campaign pages; `GetProductListActive` (`sp_Get_Product_Active`) only here. Changing SP `sp_Save_QuotedPriceDetail` affects this page and any external consumers of the quoted-price tables (unknown).
