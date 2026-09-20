---
page_id: PAGE-113
module: M04
page: CampaignSetup_Mult
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "PARTIAL", "database": "PARTIAL", "rules": "PARTIAL", "permission": "PARTIAL", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-113 - CampaignSetup_Mult (2025-05 experiment: multi-select main product; NOT linked, functionally broken)

> **Evidence level:** Tier A by full line diff against PAGE-111 (`diff -wB CampaignSetup.aspx.cs CampaignSetup_Mult.aspx.cs` = 59 changed lines, all read) and aspx diff (all substantive differences read). Everything not listed as a delta is **identical to PAGE-111** (header validation, offer mapping, per-customer-type loop, payment flags, edit=replace via `SaveBonusCampaign`). SP bodies absent (UNKNOWN).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-113 |
| Module | M04 |
| Page name | CampaignSetup_Mult |
| Kind | Entry / Transaction (legacy experiment) |
| Web folder / master | MasterSetup_UI / NewMasterPage.master |
| Page class | MasterSetup_UI_CampaignSetup_Mult |
| Files | aspx 975 lines (2025-05-07 14:28), aspx.cs 2012 lines (2025-05-07 14:30) |
| Live/legacy verdict | **LEGACY / ABANDONED EXPERIMENT.** No inbound link. Its code-behind is the live page's code with a half-finished change to the main-product drop-down; `CampaignSetup.aspx.cs` was later edited (2025-09-06) to revert that change and the aspx (2026-07-15) to hide market/customer blocks. INFERRED chronology from timestamps. |

## 2. Purpose
Same as PAGE-111. The experiment turns the "Product" (main trigger product) control of campaign type 1/2 into a **multi-select ListBox** (aspx:308-311, `SelectionMode="Multiple"`), apparently to let one campaign trigger on several products. INFERRED.

## 3. Business Objective
Same as PAGE-111; the multi-product idea is not implemented (see BR-M04-030).

## 4. Navigation
Inbound: none. Outbound: `CampaignView.aspx`, `CampaignSetup.aspx` (Reset). Menu UNKNOWN.

## 5. Route
`~/MasterSetup_UI/CampaignSetup_Mult.aspx[?MID=id]`.

## 6. Source Files
As PAGE-111 (same DAL/DAO/user control). `IVMarketStructure.ascx` is **visible** here (aspx:699-701).

## 7. UI Control Inventory
As PAGE-111 except:

| Control | Delta |
|---|---|
| ddlProduct | `asp:ListBox SelectionMode="Multiple" class="multiple-select"` (was DropDownList), aspx:308-311 |
| IVMarketStructure, gv_Market, divCus, gv_Customer and their buttons | **visible** (PAGE-111 hides them), so BR-M04-004 can be satisfied |
| everything else | identical (payment radio, category, FCFS, ratio, active switch, customer-type multi-select) |

## 8. Field Mapping
Identical to PAGE-111 section 8 (same `SaveBonusCampaign`, same DTO). `BonusProductId = ddlProduct.SelectedIndex > 0 ? int.Parse(ddlProduct.SelectedValue) : null` reads only the **first** selected item and ignores list index 0.

## 9. Dropdown Mapping
| Control | Source (Mult) | Behaviour |
|---|---|---|
| ddlProduct | first block `GetProductForDDL()` bound with `DataValueField="ComUnitId"`, `DataTextField="ComUnitName"` (`.cs:320-333`) - those columns do not belong to a product list, so `DataBind` is expected to throw and the empty `catch` swallows it; second block rebinds `ddlProduct` from `QuotedPriceSetupDAL.GetProductListActiveForALl` (ProductId/ProductName) (`:347-356`); `Items.Insert(-1,"")` throws and is swallowed so no placeholder | ListBox lists all products, none selected |
| ddlProductList | **never populated** (the block that filled it in PAGE-111 was overwritten to fill `ddlProduct`) | offer-product drop-down is empty |
| others | as PAGE-111 | - |

## 10. Button Action Mapping
As PAGE-111. `ddlCampaignType_SelectedIndexChanged` clears `ddlProduct` with `Items.Insert(-1,"")` inside try/catch (`:429-434`) instead of `SelectedValue = ""`.

## 11. Frontend Flow
As PAGE-111; select2 multi-select initialised for both `ddlChemistType` and `ddlProduct` (`.multiple-select`).

## 12. Backend Flow
As PAGE-111 section 12 (per customer-type loop, session map id 4, `SaveBonusCampaign`), except (a) `btnSave_Click` is **not** wrapped in the outer try/catch (PAGE-111 lines 1542-1543 and 1846-1852 are absent) so exceptions reach the ASP.NET error page; (b) `smtp.Send` is commented out (`:1834`).

## 13. API Mapping
`GetCustomer_New` autocomplete (customer block visible).

## 14. Service Mapping
No BLL. Static shared `BonusCampaignNewDAL` instance (GAP-M04-013).

## 15. Repository Mapping
As PAGE-111.

## 16. Stored Procedure Mapping
As PAGE-111 (sp_Del_BonusCampaignNewMaster, sp_Save_BonusCampaignNewMaster, sp_Update_BonusCampaignpkCampaignSetupId, sp_Save_BonusCampaignNewDetail, sp_Save_BonusCampaignMarketDetail, sp_Save_BonusCampaignCustomerDetail + readers). Bodies UNKNOWN.

## 17. Database Mapping
As PAGE-111.

## 18. Validation Matrix
As PAGE-111 (`ddlProduct.SelectedValue == ""` works on a ListBox: no selection => error).

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M04-030 | The multi-select main product is **not** implemented end-to-end: only one product id reaches the master (`SelectedValue` = first selected, and list item 0 is never accepted because `SelectedIndex > 0` is required); the offer-product list `ddlProductList` is empty, so every offer type that needs a product (offers 1,2,3,5) cannot be added - only type-3 offers 6 and 7 (no product) work. The page is unusable for product-based campaigns. INFERRED from reading (page not run). | CampaignSetup_Mult.aspx.cs:315-360,429-434; CampaignSetup_Mult.aspx:308-311,563 | `LoadInitialInfo`, `btnSave_Click` | INFERRED |

Rules BR-M04-001..003, 005..009, 012..015 (PAGE-111) apply unchanged; BR-M04-004 is satisfiable because market/customer blocks are visible; BR-M04-016 differs only in that the outer catch is missing.

## 20. Permission Matrix
Session keys read: `UserId` (as EntryBy). No role/UserType check in code; access is menu-based only (menu rows not in repo). Roles UNKNOWN.

## 21. Status Lifecycle
As PAGE-111 (IsActive + date window; edit = delete + new id).

## 22. Error Handling
No outer try/catch around save (unlike PAGE-111); uncaught exceptions surface as the default error page. Seed loaders swallow all exceptions.

## 23. Audit
`@EntryBy` = `Session["UserId"]` on insert.

## 24. Integration
Mail code present, send commented out (no mail). No other.

## 25. Reports
None.

## 26. Dependencies
As PAGE-111.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Open page | Main-product ListBox lists products; offer-product list empty | BR-M04-030 |
| TC-02 | Select 2 products in ListBox, type 1, save | Only first selected product persisted | BR-M04-030 |
| TC-03 | Select only the first list item | Treated as "no product" | BR-M04-030 |
| TC-04 | Type 3 campaign, offer 7 amount, add a customer, save | Works (no product needed) | BR-M04-005 |
| TC-05 | Force SP error | Unhandled error page | section 22 |

## 28. Reverse Engineering References
`Solution.Web/MasterSetup_UI/CampaignSetup_Mult.aspx(.cs)`; diff vs `CampaignSetup.aspx(.cs)`.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | diffed | SP bodies absent | MISSING | - |
| Multi-product campaign | None existed | UI only, single id persisted | UNKNOWN | INCOMPLETE FEATURE | BR-M04-030 |

## 30. Known Gaps
Abandoned experiment left deployed (GAP-M04-006, GAP-M04-008). No outer error handling. Hard-coded SMTP credential retained in source (GAP-M04-003).

## 31. Change Impact
Shares `BonusCampaignNewDAL.SaveBonusCampaign` and all seed SPs with PAGE-111/114/115/117. Removable independently (no inbound link) once menu rows are checked.
