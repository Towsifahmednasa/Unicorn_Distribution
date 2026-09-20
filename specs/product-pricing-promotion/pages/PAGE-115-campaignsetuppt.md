---
page_id: PAGE-115
module: M04
page: CampaignSetupPT
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "PARTIAL", "database": "PARTIAL", "rules": "PARTIAL", "permission": "PARTIAL", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-115 - CampaignSetupPT (2024-11 prototype; NOT linked)

> **Evidence level:** Tier A by full diff. `CampaignSetupPT.aspx.cs` and `OLdCampaignSetup.aspx.cs` are **identical except the class name** (`diff -wB` = 4 lines). Diffs of this code against `CampaignSetup.aspx.cs` (215 lines) and `CampaignSetup_new.aspx.cs` (215 lines) were read in full. Behaviour not listed = PAGE-111 header/offer logic. SP bodies absent (UNKNOWN).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-115 |
| Module | M04 |
| Page name | CampaignSetupPT |
| Kind | Entry / Transaction (legacy) |
| Web folder / master | MasterSetup_UI / NewMasterPage.master |
| Page class | MasterSetup_UI_CampaignSetupPT |
| Files | aspx 955 lines (2024-11-23 10:58), aspx.cs 1859 lines (2024-11-23 10:48) |
| Live/legacy verdict | **LEGACY / PROTOTYPE.** No inbound link. Adds an unbound "Payment Type" radio (COD / Other) to the OLd layout - apparently the first unfinished step toward the payment-type feature that `CampaignSetup.aspx` later implemented with `rbPaymentOptions` and `IsPTforCOD/IsPTforOther`. INFERRED from timestamps (PT 2024-11-23, OLd 2024-12-14, live 2025-09). |

## 2. Purpose
Campaign setup with single customer type, campaign category, Trade Policy / FCFS flags, ratio-wise increment, offers, market and customer lists. No payment-type persistence, no multi-customer-type loop, no multi-product / manual-ratio structures.

## 3. Business Objective
Same as PAGE-111 in its 2024 form (before COD/NCOD payment targeting and multi-customer-type saving were added). INFERRED.

## 4. Navigation
Inbound: none. Outbound: `CampaignView.aspx`, `CampaignSetup.aspx` (Reset `.cs:1857`).

## 5. Route
`~/MasterSetup_UI/CampaignSetupPT.aspx[?MID=id]`.

## 6. Source Files
As PAGE-111; `IVMarketStructure.ascx` visible; DAL `SaveBonusCampaign`.

## 7. UI Control Inventory
Deltas vs PAGE-111: `ddlChemistType` **single** DropDownList (aspx:154); `rbPaymentOptions` absent; market/customer blocks **visible**; category, FCFS, Trade Policy, ratio-wise increment, Active present. Extra control: `PaymentOptions` RadioButtonList (COD, Other; aspx:237-247) - **not referenced by the code-behind** (grep `PaymentOptions` in `.cs` = 0), i.e. cosmetic; no default, no validation, no save.

## 8. Field Mapping
`SaveBonusCampaign` with `CustomerTypeId = ddlChemistType.SelectedIndex > 0 ? int.Parse(SelectedValue) : null` (`.cs:1591`), `CampaignCategoryId`, `IsTradePolicy`, `IsFCFS` (`:1616`), `IsRatioWiseIncrement` (`:1618`), `IsActive`. **IsPTforCOD/IsPTforOther are never set** (NULL). Details as PAGE-111 incl. the `IsRatioWiseIncrementPro` rule (`:1530-1545`).

## 9. Dropdown Mapping
As PAGE-111 (customer type single with placeholder at index 0).

## 10. Button Action Mapping
As PAGE-111.

## 11. Frontend Flow
As PAGE-111; the payment radio has no script.

## 12. Backend Flow
`btnSave_Click`: `Validation()`, single master DTO, `SaveBonusCampaign(...)` (`:1625`), mail with **active** `smtp.Send(mail)` (`:1681`), "Operation successful!" redirect to `CampaignView.aspx`. No per-type loop, no session map id.

## 13. API Mapping
`GetCustomer_New` autocomplete.

## 14. Service Mapping
No BLL; static DAL (GAP-M04-013).

## 15. Repository Mapping
As PAGE-111.

## 16. Stored Procedure Mapping
As PAGE-111. `SaveBonusCampaign` still executes `sp_Update_BonusCampaignpkCampaignSetupId` with `@CampgainMasterMapId = Session["CampgainMasterMapId"]`, which this variant never sets (null) (`BonusCampaignNewDAL.cs:246,289`). Effect UNKNOWN.

## 17. Database Mapping
As PAGE-111.

## 18. Validation Matrix
As PAGE-111 (single customer type: `ddlChemistType.SelectedValue == ""`).

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M04-029 | `CampaignSetupPT` and `OLdCampaignSetup` share identical code-behind: single customer type; category/TradePolicy/FCFS/ratio-wise persisted; payment-type flags **not** persisted (NULL) even though `CampaignSetupPT.aspx` renders a COD/Other radio `PaymentOptions` (aspx:237-247) that no code reads; edit = delete + new id (BR-M04-012); session map id passed as null. Both call `smtp.Send` (active). | CampaignSetupPT.aspx.cs = OLdCampaignSetup.aspx.cs (diff); .cs:1591,1616-1618,1625,1681; CampaignSetupPT.aspx:237 | `btnSave_Click` | CONFIRMED |

Rules BR-M04-001..003, 005..007, 011, 012, 014, 015 (PAGE-111) apply; BR-M04-004 satisfiable; BR-M04-008 => single master.

## 20. Permission Matrix
Session keys read: `UserId` (as EntryBy). No role/UserType check in code; access is menu-based only. Roles UNKNOWN.

## 21. Status Lifecycle
IsActive + date window; edit replaces id.

## 22. Error Handling
Outer try/catch in `btnSave_Click` shows `faildalert(ex.Message)`; seed/edit loaders swallow every exception. Failure message is always "Already Exist!".

## 23. Audit
`@EntryBy` = `Session["UserId"]`.

## 24. Integration
Success e-mail via Gmail SMTP: **`smtp.Send(mail)` is ACTIVE** (hard-coded SMTP account and password in source, `<redacted>`; recipient/Bcc are developer mailboxes).

## 25. Reports
None.

## 26. Dependencies
As PAGE-111.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Save campaign, inspect IsPTforCOD/IsPTforOther | NULL | BR-M04-029 |
| TC-02 | Edit and save | new `CampgainMasterId` | BR-M04-012 |
| TC-03 | Save success | Gmail SMTP send attempted | section 24 |
| TC-04 | Choose "COD" in the Payment Type radio, save | No effect on saved data | BR-M04-029 |

## 28. Reverse Engineering References
`Solution.Web/MasterSetup_UI/CampaignSetupPT.aspx(.cs)`; diffs vs `OLdCampaignSetup`, `CampaignSetup`, `CampaignSetup_new`.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | diffed | SP bodies absent | MISSING | - |
| Payment-type targeting | None existed | UI radio unbound | UNKNOWN | ABSENT | BR-M04-029 |

## 30. Known Gaps
Duplicate legacy page (GAP-M04-006); active SMTP with hard-coded credential (GAP-M04-003); unbound UI element.

## 31. Change Impact
Shares `SaveBonusCampaign` with PAGE-111/113/114 and its twin PAGE-117. Independent removal possible after menu check.
