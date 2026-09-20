---
page_id: PAGE-264
module: M07
page: DelivaryInvoiceCreationAfterSalesConfirm
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---

# PAGE-264 — DelivaryInvoiceCreationAfterSalesConfirm

> **Evidence level:** Tier A - reviewed 2026-09-19 by reading the .aspx, .aspx.cs, the BLL/DAL methods reached, and every SP/SQL body present in the repository. Confidence tags: CONFIRMED (read in implementation), INFERRED (implied by several references), UNKNOWN (not derivable; SP bodies are absent from the repo except where stated). Module: M07 - Proforma, Invoicing & Delivery.


## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-264 |
| Module | M07 - Proforma, Invoicing & Delivery |
| Page name | DelivaryInvoiceCreationAfterSalesConfirm |
| Kind | Entry / Transaction - second-stage delivery confirmation ("payment confirmation") over invoices that already passed sales confirmation (LoadingSummary). **Largely disabled by markup (see BR-M07-021, 023).** |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master (`DelivaryInvoiceCreationAfterSalesConfirm.aspx:1`) |
| Page class | SInventory_UI_DelivaryInvoiceCreationAfterSalesConfirm |
| Size | aspx 529 lines, code-behind 693 lines (about 40 percent commented-out or dead code) |
| Suspected legacy/duplicate | Partly: `SavePayment`, `SavePaymentMultiple`, `NewMethod`, `EmpCetegoryAddImageButton_Click` and the commented `SaveFullInvoice`/`SaveRejectInvoice` blocks are legacy of the older `DelivaryInvoiceCreationAuto` page (excluded from the build: `DelivaryInvoiceCreationAuto.aspx.exclude`). Twin of `LoadingSummary.aspx` (copy/paste, see PAGE-271). |

## 2. Purpose
CONFIRMED (headings): breadcrumb "Return"; button "Update Old Data"; Sales Center filter, Search, Reset, "Total Net Amount" counter, grid of invoices with checkbox, invoice no, invoice date, customer code/name, market, order date, net amount, status and a per-row "Go to Partial" button, and an "ALL Submit" button. Intended purpose: after sales confirmation, list the invoices of a sales center that have **no payment invoice yet** (`PaymentInvoiceNo IS NULL`), let the user mark them and either confirm payment in full (`sp_PaymentConformationFull`) or jump to the partial-payment screen (`PaymentPartial.aspx`).

## 3. Business Objective
INFERRED: give the DC accounts/logistics user a worklist to finalise delivery invoices after the loading-summary "sales confirm" step, and drive the invoice from "delivered" to "payment confirmed" (Full) or "partial dues" (PaymentPartial, module M09). The full path is currently unreachable because the only status option in the row dropdown is "Partial Dues" and the dropdown is disabled (BR-M07-021, BR-M07-023); operationally only the "Go to Partial" hand-off works.

## 4. Navigation
Menu DB-driven -> UNKNOWN. Inbound: the M07 spec inventory finds none by markup; `LoadingSummary` (PAGE-271) does not link here. Outbound: `PaymentPartial.aspx` (row button, `.aspx.cs:283`; session `InvoiceId`, see `../collection-payment/pages/PAGE-334-paymentpartial.md`), `DelivaryInvoiceCreationAuto.aspx` (Reset button, `.aspx.cs:44` - **file is `.exclude`d, dead redirect**), external window `http://13.76.141.111:58122/SInventory_UI/DelivaryInvoiceCreationAuto.aspx?UId=<UserId>&UName=<LoginName>` ("Update Old Data", `.aspx.cs:687-692`; hard-coded host and identity passed on the query string).

## 5. Route
`~/SInventory_UI/DelivaryInvoiceCreationAfterSalesConfirm.aspx` (file-based WebForms route; no `MapPageRoute` alias).


## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/DelivaryInvoiceCreationAfterSalesConfirm.aspx |
| Code-behind | Solution.Web/SInventory_UI/DelivaryInvoiceCreationAfterSalesConfirm.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL | OrderInfoBLL (`LoadSC`, `LoadManufac`, `LoadOrderWithInvoice`), InvoiceBLL (`SavePaymentConformationFull` :47), CustPaymentBLL (dead helpers) |
| DAL | OrderInfoDAL (`LoadOrderWithInvoiceSP` `OrderInfoDAL.cs:322-351`), InvoiceDAL (`SavePaymentConformationFull` `InvoiceDAL.cs:232-240`), `SeedDataDAL.GetRouteInfoforReturn`, `RouteInformationDAL` |

## 7. UI Control Inventory
| Control ID | Type | Label | Required | Default | Read-only / Enabled / Visible | Data source | Handler / error |
|---|---|---|---|---|---|---|---|
| salesCenterDropDownList | DropDownList | "Sales Center:" | YES - `GridView()` alert "Please input Sales Center!" when empty | `SelectedIndex = 1` | enabled, AutoPostBack | `OrderInfoBLL.LoadSC(ddl, Session["UserId"])` (tblCompanyUnit x tblUserCompanyUnit) | `salesCenterDropDownList_SelectedIndexChanged` - loads routes via `SeedDataDAL.GetRouteInfoforReturn(ComUnitId)`, clears grid |
| manufacDropDownList ("Manufacturer"), marketDropDownList, dd, rootDropDownList ("Route"), ddlTerritoryName, invoicenoTextBox ("Invoice No") | various | - | none | manufac index 1; others empty | **Visible="False"** wrapper divs (`runat="server" visible="False"`) or `display:none` | manufacturers / routes / territory by route / invoice autocomplete `GetAllInvoice` | not part of the active query (`GenerateParam` uses only the sales center; the invoice-no branch needs a value the user cannot enter) |
| submitButton "Search" | LinkButton | "Search" | - | - | enabled | - | `Button1_Click` -> `GridView()` |
| Reset link | LinkButton | "Reset" | - | - | enabled | - | `cancelButton_Click` -> redirect `DelivaryInvoiceCreationAuto.aspx` (excluded page) |
| lblCount | Label | "Total Net Amount : 0" | - | "Total Net Amount: 0" | - | recomputed by check events | `chkSelectAll_CheckedChanged`, `chkSelect_CheckedChanged` |
| orderGridView | GridView `DataKeyNames="ComUnitId,ManufacId,OrderId,InvoiceId,MarketId,CustomerMasterId"` | SL#, [checkbox], Invoice No, Invoice Date, Customer Code, Customer Name, Market, Order Date, Net Amount, Status, Submit | - | rows from `sp_DeliveryInvoiceCreationList_New` | - | see BR-M07-020 | - |
| chkSelectAll / chkSelect | CheckBox | header/row | at least one required at submit | unchecked | AutoPostBack | - | see BR-M07-023/024 |
| hfIsAdjustInvoice, hfInvoiceNo, hfchkStatus | HiddenField | - | - | from SP columns `IsAdjustInvoice`, `InvoiceNo`, `chkStatus` | hidden | SP | - |
| statusDropDownList | DropDownList | "Status" | - | single item `Partial Dues` ("Partial", `Selected="True"`) | **Enabled="false"**, AutoPostBack | markup: only `<asp:ListItem Selected="True" Value="Partial Dues">Partial</asp:ListItem>` (Full/Full Dues/Reject items commented out at `.aspx:342-345`) | `statusDropDownList_OnTextChanged` |
| reasonReturnDropDownList | DropDownList | (return reason) | - | first | `Visible="False"`; shown only when status == "Reject" (impossible - no such item) | 14 static reasons: Cash Short, No Order, Wrong Order, Damaged Broken, Quality Issue, Shop Closed(11), Order Cancelled, Slow Moving, Price Error, Return Others, Date Expired, Partial return-cash short (11 - duplicate value), Partial return-Excess order (12), Double order (13) | - |
| gotoinvoiceButton | LinkButton | "Go to Partial" | - | - | `Visible="false"`, made visible when status = "Partial Dues"; `OnClientClick="return sweetAlertConfirm_Submit(this)"` | - | `gotoinvoiceButton_Click` |
| btnFinalSubmit | LinkButton | "ALL Submit" | see §18 | - | enabled; confirm dialog | - | `btnFinalSubmit_Click` |
| EmpCetegoryAddImageButton | LinkButton | "Update Old Data" | - | - | enabled | - | opens external URL (see §4) |

## 8. Field Mapping
| UI | DataKey / field | Handler | BLL -> DAL -> SP param | Column |
|---|---|---|---|---|
| hfInvoiceNo.Value | InvoiceNo (SP column) | `btnFinalSubmit_Click` (`.aspx.cs:562-566`) | `InvoiceBLL.SavePaymentConformationFull(InvoiceNo, LoginName, dd-MMM-yyyy)` -> `InvoiceDAL.SavePaymentConformationFull` -> `sp_PaymentConformationFull` `@InvoiceNo`, `@UpdateBy`, `@UpdateDate` | UNKNOWN (body absent) |
| Session["LoginName"] | - | same | `@UpdateBy` | UNKNOWN |
| DateTime.Now `dd-MMM-yyyy` | - | same | `@UpdateDate` (string) | UNKNOWN |
| DataKeys["InvoiceId"] | - | `gotoinvoiceButton_Click` | `Session["InvoiceId"]` -> `PaymentPartial.aspx` | n/a |

## 9. Dropdown Mapping
| Dropdown | Loader | Source | Notes |
|---|---|---|---|
| salesCenterDropDownList | `OrderInfoBLL.LoadSC(ddl, UserId)` | inline SQL, tblCompanyUnit filtered by tblUserCompanyUnit | per-user scope |
| manufacDropDownList | `OrderInfoBLL.LoadManufac` | tblManufacturer | hidden |
| rootDropDownList | `SeedDataDAL.GetRouteInfoforReturn(ComUnitId)` -> `sp_Get_RouteInfoforReturn` (absent) | SP | hidden; DisplayValue DistributionRouteName / DistributionRouteId; placeholder "Please Select From List" |
| ddlTerritoryName | `RouteInformationDAL.GetTerritoryByRouteInformationDA_DDLId` -> `sp_GET_TerritoryByRouteInformationDDL` (absent) | SP | hidden |
| statusDropDownList | static markup | one item | see §7 |

## 10. Button Action Mapping
| Button | Handler | BLL / DAL / SP | DB change |
|---|---|---|---|
| Search | `Button1_Click` -> `GridView()` | `OrderInfoDAL.LoadOrderWithInvoiceSP(param)` -> `sp_DeliveryInvoiceCreationList_New(@param)` (absent) | none |
| ALL Submit | `btnFinalSubmit_Click` `.aspx.cs:546-598` | for checked rows with status "Full" -> `sp_PaymentConformationFull`; **status "Full" cannot be selected** | none in practice |
| Go to Partial | `gotoinvoiceButton_Click` `.aspx.cs:269-347` | none (redirect) | session key only |
| Reset | `cancelButton_Click` | redirect | none |

## 11. Frontend Flow
UpdatePanel + UpdateProgress. Page_Load(!PostBack): `DropDownlist()` (SC index 1, manufacturer 1), route load, `SessionChoose()` (reads `Session["DelMarketId"]`), and if that session value exists it rebinds the grid and resets the total. Check events recompute the "Total Net Amount" label as the sum of `lblTpGrandTotal` of ticked rows (server round-trip per click). `sweetAlertConfirm_Submit` (SweetAlert, defined in the master) confirms submit/partial clicks. No page-local JS functions other than the master-provided ones (`pageLoad()` at `.aspx:122` initialises select2/datepicker).

## 12. Backend Flow
| Handler | Source | Flow | try/catch | Txn |
|---|---|---|---|---|
| Page_Load | `.aspx.cs:20-40` | load lists; restore DelMarket | no | no |
| DropDownlist | `.aspx.cs:65-83` | wrapped in `try{}catch(Exception ex){}` (silently ignores a missing session) | swallow | no |
| salesCenterDropDownList_SelectedIndexChanged | `.aspx.cs:85-130` | nested try/catch swallow; loads routes | swallow | no |
| GridView | `.aspx.cs:399-464` | validates SC; `LoadOrderWithInvoiceSP(GenerateParam())`; per row: if `IsAdjustInvoice == "True"` -> `SelectedIndex=0; Enabled=false`; then `statusDropDownList.SelectedValue = row["LoadingSummaryStatus"]`; make "Go to Partial" visible when SelectedValue == "Partial Dues"; sets `Session["DelMarketId"]` | no | no |
| btnFinalSubmit_Click | `.aspx.cs:546-598` | `DataValidation()` then loop over rows | no | none; each SP call separate |
| DataValidation | `.aspx.cs:600-636` | grid empty -> faildalert "Data Can not be Empty!"; no checked row -> "Please Select at least one Row!" | - | - |

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.


## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderInfoBLL.LoadSC | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:91 |
| OrderInfoBLL.LoadManufac | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:117 |
| OrderInfoBLL.LoadOrderWithInvoice | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:143 |
| OrderInfoBLL.LoadRouteforReturn | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:133 |
| CustPaymentBLL.UpdateInvoicePaymentAmount | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:114 |
| CustPaymentBLL.SaveCustPayment | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:33 |
| CustPaymentBLL.SaveCustDetail | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:127 |
| InvoiceBLL.SavePaymentConformationFull | Library.BLL/SInventory_BLL/InvoiceBLL.cs:48 |


## 15. Repository Mapping
| DAL | Source | SQL/SP | Params |
|---|---|---|---|
| OrderInfoDAL.LoadOrderWithInvoiceSP | `OrderInfoDAL.cs:322-351` | `GetDataTable("sp_DeliveryInvoiceCreationList_New", @param)` - `@param` is a **raw SQL fragment** built by the page | `@param` |
| InvoiceDAL.SavePaymentConformationFull | `InvoiceDAL.cs:232-240` | `RunStoreProcedure("sp_PaymentConformationFull")` | @InvoiceNo, @UpdateBy, @UpdateDate |
| OrderInfoBLL.LoadOrderWithInvoice | `OrderInfoDAL.cs:278-308` -> `LoadOrderWithInvoiceSP` | same SP | used by `SessionChoose()` |

## 16. Stored Procedure Mapping
| SP | Called from | Body | Known |
|---|---|---|---|
| sp_DeliveryInvoiceCreationList_New | `OrderInfoDAL.LoadOrderWithInvoiceSP` | absent | returns at least: ComUnitId, ManufacId, OrderId, InvoiceId, MarketId, CustomerMasterId, InvoiceNo, InvoiceDate, CustomerCode, CustomerName, MarketName, SubmissionDate, TpGrandTotal, IsAdjustInvoice, chkStatus, LoadingSummaryStatus (from grid bindings); consumes a dynamic WHERE fragment (BR-M07-020) |
| sp_PaymentConformationFull | `InvoiceDAL.SavePaymentConformationFull` | absent | effect UNKNOWN; name suggests it sets PaymentInvoiceNo/PaymentStatus (INFERRED) |
| sp_Get_RouteInfoforReturn, sp_GET_TerritoryByRouteInformationDDL | dropdown loaders | absent | UNKNOWN |

## 17. Database Mapping
- READ (inline): tblCompanyUnit, tblUserCompanyUnit, tblManufacturer, tblMarket, tblOrder, tblInvoice, tblRouteInformationMaster, tblCustomerPay/tblCustPayDetail (dead helpers).
- READ via SP: see §16 (base tables not derivable; the WHERE fragment names `tblOrder.ComUnitId`, `tblInvoice.InvoiceNo`, `PaymentInvoiceNo`, `tblOrder.DistributionRouteId`, `tblInvoice.InvoiceDate` -> the SP joins tblOrder and tblInvoice; INFERRED).
- WRITTEN: only via `sp_PaymentConformationFull` (unreachable). Dead helpers `SavePayment`/`SavePaymentMultiple` would write tblCustomerPay/tblCustPayDetail through `CustPaymentBLL.SaveCustPayment` and `UpdateInvoicePaymentAmount(...,"Full",...)` but are never called (grep in this file).

## 18. Validation Matrix
| Rule | Layer | Condition | Message |
|---|---|---|---|
| Sales center chosen | server `GridView()` | `salesCenterDropDownList.SelectedValue` empty | faildalert "Please input Sales Center!" |
| Grid not empty | server `DataValidation` | `orderGridView.Rows.Count == 0` | faildalert "Data Can not be Empty!" |
| At least one selection | server | no `chkSelect.Checked` | faildalert "Please Select at least one Row!" |
| Status must be "Full" for action | implicit | `statusDropDownList.SelectedValue == "Full"` (cannot occur) | none - silent no-op |
| Success | server | `status > 0` | ShowSuccesalert "Delivery Invoice Created Successsfully!" (typo in source) |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M07-020 | The worklist = invoices of the selected sales center whose `PaymentInvoiceNo IS NULL`, ordered by InvoiceNo; the filter is a concatenated SQL fragment (`" and dbo.tblOrder.ComUnitId='<SC>' and PaymentInvoiceNo is null order by tblInvoice.InvoiceNo Asc"`; with an invoice-no value it additionally adds `tblOrder.DistributionRouteId='<route>' AND tblInvoice.InvoiceNo like '%<text>%'`). Date, route and territory filters are not applied. | `DelivaryInvoiceCreationAfterSalesConfirm.aspx.cs:363-397` | page builds fragment -> SP `@param` | CONFIRMED (fragment); SP body UNKNOWN |
| BR-M07-021 | Row status is taken from the SP column `LoadingSummaryStatus` into a dropdown that contains only `Partial Dues`; any other value (e.g. "Full Dues", "Cash", "Rejection") makes `SelectedValue` throw ArgumentOutOfRangeException and the whole grid load fails. `IsAdjustInvoice == "True"` rows are forced to index 0 and disabled. The dropdown itself is `Enabled="false"`. | `.aspx.cs:439-457`; `.aspx:341-346` | code-behind + markup | CONFIRMED |
| BR-M07-022 | "Go to Partial" is offered only for rows whose status is `Partial Dues`; clicking stores `Session["InvoiceId"]` and redirects to `PaymentPartial.aspx` (partial payment/dues capture, module M09). | `.aspx.cs:269-285, 448-456, 538-541` | code-behind | CONFIRMED |
| BR-M07-023 | "ALL Submit" requires a non-empty grid and at least one ticked row, but the only action inside the loop is `sp_PaymentConformationFull` for rows whose status is `"Full"`; since that status is not selectable (and the Full/Reject items are commented out), no SP is called, `status` stays 0 and no success message is shown. Payment confirmation is thus effectively performed on `PaymentPartial.aspx` only. | `.aspx.cs:546-598` | code-behind | CONFIRMED |
| BR-M07-024 | "Total Net Amount" = sum of `TpGrandTotal` of the ticked rows (non-numeric labels count 0). | `.aspx.cs:132-146, 643-685` | code-behind | CONFIRMED |
| BR-M07-025 | Reset redirects to `DelivaryInvoiceCreationAuto.aspx`, which exists only as `.exclude` files (not compiled) -> HTTP 404 in the deployed site (INFERRED: depends on deployment of `.exclude` files). | `.aspx.cs:42-45`; file list `SInventory_UI/DelivaryInvoiceCreationAuto.aspx*.exclude` | code-behind | CONFIRMED (file state) / INFERRED (404) |

## 20. Permission Matrix
- Session keys: `UserId` (SC list), `LoginName` (SP `@UpdateBy`, dead helpers), `DelMarketId` (page state).
- No role check; access is menu-based only. Any authenticated user with the URL can act on any invoice shown; scope is limited by the user's sales centers (tblUserCompanyUnit) only on the dropdown (the SP filter uses whichever sales center value is posted - the posted DropDownList value is validated by ASP.NET event validation only against the bound list).

## 21. Status Lifecycle
Status columns involved (values seen in code): `tblInvoice.LoadingSummaryStatus` ("Full Dues", "Partial Dues", "Cash", "Rejection" - set by `sp_UP_LoadingSummary`, PAGE-271); `tblInvoice.PaymentInvoiceNo` (NULL = not yet payment-confirmed; `"RTN-"+DelivaryInvoiceNo` written by `InvoiceDAL.PaymentUpdateInvoice`, `InvoiceDAL.cs:793-808`); `PaymentInvoiceStatus` ("Reject" filtered elsewhere: `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:1335-1355`); `PaymentStatus` ("Partial"). Transition performed by this page: none reachable (BR-M07-023). Full lifecycle: README "Status lifecycles".

## 22. Error Handling
Silent swallow in `DropDownlist()` and `salesCenterDropDownList_SelectedIndexChanged` (empty catch). `GridView()` and `btnFinalSubmit_Click` have no try/catch -> exceptions (e.g., BR-M07-021) reach the ASP.NET error page. Messages via SweetAlert (`faildalert`, `ShowSuccesalert`) and `alert()`.

## 23. Audit
Audit: `@UpdateBy` = LoginName and `@UpdateDate` = today are sent to `sp_PaymentConformationFull` (stored columns UNKNOWN). No local logging.

## 24. Integration
External: hard-coded host `13.76.141.111:58122` opened for "Update Old Data" with `UId`/`UName` in the query string (identity leaks via URL/Referer, and the target's authentication is UNKNOWN). Accounting/payments: dead helpers reference `CustPaymentBLL`. No SAP integration.

## 25. Reports
None here. Downstream print/list outputs are documented in `../reports/`.

## 26. Dependencies
BLL: OrderInfoBLL, InvoiceBLL, CustPaymentBLL. DAL: OrderInfoDAL, InvoiceDAL, SeedDataDAL, RouteInformationDAL. DAO: CustPayment, CustPaymentDetail, CustomerMaster. Web service: `SInventoryWebService.GetAllInvoice` (hidden). SPs: 4 (all absent).

## 27. Test Cases
| ID | Scenario | Steps | Expected | BR |
|---|---|---|---|---|
| TC-264-01 | Default load | open as user with one sales center | SC preselected; no rows until Search | BR-M07-020 |
| TC-264-02 | Search without SC | clear SC (if placeholder selected), Search | faildalert "Please input Sales Center!" | - |
| TC-264-03 | Search valid | select SC, Search | rows with `PaymentInvoiceNo` null | BR-M07-020 |
| TC-264-04 | Row with status not Partial Dues | data row where `LoadingSummaryStatus='Full Dues'` | ArgumentOutOfRangeException | BR-M07-021 |
| TC-264-05 | Adjust invoice | row with `IsAdjustInvoice=True` | dropdown index 0, disabled | BR-M07-021 |
| TC-264-06 | Go to Partial | click on a Partial Dues row | redirect to PaymentPartial.aspx with `Session["InvoiceId"]` | BR-M07-022 |
| TC-264-07 | ALL Submit no rows | click | faildalert "Data Can not be Empty!" | BR-M07-023 |
| TC-264-08 | ALL Submit nothing ticked | click | faildalert "Please Select at least one Row!" | BR-M07-023 |
| TC-264-09 | ALL Submit ticked | tick rows, click | no DB call, no message | BR-M07-023 |
| TC-264-10 | Net total | tick two rows | label = sum of amounts | BR-M07-024 |
| TC-264-11 | Reset | click | redirect to excluded page | BR-M07-025 |
| TC-264-12 | Injection | (security test) SC value tampered | `@param` fragment altered | GAP-M07-009 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/DelivaryInvoiceCreationAfterSalesConfirm.aspx`, `Solution.Web/SInventory_UI/DelivaryInvoiceCreationAfterSalesConfirm.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).


## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | page present | n/a | MISSING | - |
| Full payment confirmation | None existed | unreachable | SP absent | GAP | BR-M07-023 |
| Worklist filter | None existed | SC + PaymentInvoiceNo null | SP absent | UNKNOWN | BR-M07-020 |
| Status set | None existed | Partial Dues only | - | GAP | BR-M07-021 |

## 30. Known Gaps
- SQL injection surface: `GenerateParam` concatenates dropdown/textbox values into the `@param` fragment executed inside `sp_DeliveryInvoiceCreationList_New` (GAP-M07-009).
- Full-payment path dead; status dropdown disabled with a single item; SelectedValue assignment throws for other statuses (GAP-M07-010).
- Reset redirect points to an excluded page; hard-coded external IP with identity in URL (GAP-M07-011).
- Success message typo "Successsfully"; duplicated reason value 11.
- Large dead code blocks (SavePayment, SavePaymentMultiple, commented submit logic).
- Silent exception swallowing hides route/session failures.
- `sp_DeliveryInvoiceCreationList_New`, `sp_PaymentConformationFull`, `sp_Get_RouteInfoforReturn`, `sp_GET_TerritoryByRouteInformationDDL` bodies absent (GAP-M07-012).

## 31. Change Impact
- Twin page: `LoadingSummary.aspx` (PAGE-271) - both use `sp_DeliveryInvoiceCreationList_New` / `sp_LoadingSummary` patterns and the same status vocabulary.
- `OrderInfoDAL.LoadOrderWithInvoiceSP` is also used by `OrderInfoBLL.LoadOrderWithInvoice` (SessionChoose) and by other delivery pages (grep `LoadOrderWithInvoiceSP`).
- `InvoiceDAL.SavePaymentConformationFull` shared with any page calling `InvoiceBLL.SavePaymentConformationFull`.
- Downstream: `PaymentPartial.aspx` (M09 `../collection-payment/pages/PAGE-334-paymentpartial.md`), `SalesReturnList_new.aspx` reads `PaymentInvoiceNo` non-null rows.
