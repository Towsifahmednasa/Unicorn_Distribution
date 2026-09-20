---
page_id: PAGE-114
module: M04
page: CampaignSetup_new
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "PARTIAL", "database": "PARTIAL", "rules": "PARTIAL", "permission": "PARTIAL", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-114 - CampaignSetup_new (2023-02 generation; NOT linked)

> **Evidence level:** Tier A by full diff against `OLdCampaignSetup.aspx.cs` (215 changed lines read) and against `CampaignSetup_final.aspx.cs` (887 lines read), plus aspx feature grep. Behaviour not listed = PAGE-111 header/offer logic. SP bodies absent (UNKNOWN).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-114 |
| Module | M04 |
| Page name | CampaignSetup_new |
| Kind | Entry / Transaction (legacy) |
| Web folder / master | MasterSetup_UI / NewMasterPage.master |
| Page class | MasterSetup_UI_CampaignSetup_new |
| Files | aspx 1095 lines (2023-02-26), aspx.cs 1863 lines (2023-02-26) |
| Live/legacy verdict | **LEGACY (oldest of the family; precursor of `_final`).** No inbound link. INFERRED from timestamps and feature superset (`_final` = `_new` + persisted multi-product/manual-ratio). |

## 2. Purpose
Campaign setup with a single customer type, campaign type/offers/market/customer lists and a *draft* "Multiple Product Add" modal. No campaign category, FCFS, ratio-wise increment or payment-type fields.

## 3. Business Objective
Same as PAGE-111 in its first form. INFERRED.

## 4. Navigation
Inbound: none. Outbound: `CampaignView.aspx`, `CampaignSetup.aspx` (Reset `:1781`).

## 5. Route
`~/MasterSetup_UI/CampaignSetup_new.aspx[?MID=id]`.

## 6. Source Files
As PAGE-111 (DAL `SaveBonusCampaign`), user control visible, DAO `BonusCampaignNewMasterDAO`.

## 7. UI Control Inventory
Deltas vs PAGE-111: `ddlChemistType` single DropDownList (aspx:158); **absent:** ddlCampaignCategory, chkFCFS, chkIsRatioWiseIncrement, rbPaymentOptions; **present but non-persistent:** `divrbType` (visible for campaign type 1, aspx:298), `rbType` radio (aspx:304; `rbType_SelectedIndexChanged` -> `AddMultipleProductAdd()` at `.cs:1784-1794`), modal `mpe_1` with `gv_MultipleProductAdd` (aspx:652,957), handlers `LinkButton1_Click`, `ddlProduct_SelectedIndexChanged`, `txtProductQty_TextChanged` (`.cs:1838-1853`). Market/customer blocks visible.

## 8. Field Mapping
`SaveBonusCampaign` (edit = replace) with header params; `CampaignCategoryId`, `IsFCFS`, `IsRatioWiseIncrement`, payment flags are left null in the DTO and sent as NULL (`.cs:1549`; `BonusCampaignNewDAL.cs:207-229`). `CustomerTypeId = ddlChemistType.SelectedIndex > 0 ? int.Parse(SelectedValue) : null`. Details as PAGE-111 without the ratio flag. **The multi-product grid is never read by `btnSave_Click`** (no parameter exists in `SaveBonusCampaign`).

## 9. Dropdown Mapping
As PAGE-111 minus category and with single-select customer type.

## 10. Button Action Mapping
As PAGE-111 plus the multi-product handlers listed in section 7.

## 11. Frontend Flow
As PAGE-111; modal popup toggled server-side (`mpe_1`).

## 12. Backend Flow
`btnSave_Click`: validation, single master DTO, `SaveBonusCampaign(aMaster, DtlList, MarketList, DtlCustList, UserId)` (`.cs:1549`), then mail with **active** `smtp.Send(mail)` (`:1605`) and "Operation successful!" redirect to `CampaignView.aspx`.

## 13. API Mapping
`GetCustomer_New` autocomplete.

## 14. Service Mapping
No BLL; static DAL (GAP-M04-013).

## 15. Repository Mapping
As PAGE-111.

## 16. Stored Procedure Mapping
As PAGE-111 (`SaveBonusCampaign` sequence). `@CampgainMasterMapId` = null (session key never set by this page, `BonusCampaignNewDAL.cs:246,289`). Bodies UNKNOWN.

## 17. Database Mapping
As PAGE-111; category / FCFS / ratio / payment columns are written NULL by this page.

## 18. Validation Matrix
As PAGE-111 minus the category rule; empty offer / market messages use `showMessageBox` alerts.

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M04-028 | This variant's "Multiple Product Add" UI (radio + modal grid) is a **non-persistent stub**: the grid contents are not passed to `SaveBonusCampaign`, so anything entered is lost on save; campaigns created here lack category, FCFS, ratio-wise, payment-type and multi-product properties (NULL/false), and edit = replace with a new id (BR-M04-012). | CampaignSetup_new.aspx.cs:1549,1784-1853; BonusCampaignNewDAL.cs:180-229 | `btnSave_Click`, `SaveBonusCampaign` | CONFIRMED |

Rules BR-M04-001..003, 005..008, 012..015 (PAGE-111) apply (BR-M04-008: single customer type gives one master). BR-M04-004 is satisfiable (market/customer visible).

## 20. Permission Matrix
Session keys read: `UserId` (as EntryBy). No role/UserType check in code; access is menu-based only (menu rows not in repo). Roles UNKNOWN.

## 21. Status Lifecycle
IsActive + date window; edit replaces id.

## 22. Error Handling
Outer try/catch in `btnSave_Click` shows `faildalert(ex.Message)` (PAGE-111 section 22 applies); seed/edit loaders swallow every exception. Failure message is always "Already Exist!".

## 23. Audit
`@EntryBy` = `Session["UserId"]`.

## 24. Integration
Success e-mail via Gmail SMTP: **`smtp.Send(mail)` is ACTIVE** (hard-coded SMTP account and password in source, `<redacted>`; recipient/Bcc are developer mailboxes).

## 25. Reports
None.

## 26. Dependencies
As PAGE-111 + AjaxControlToolkit ModalPopupExtender.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Add rows in the multi-product modal then save | Rows not persisted | BR-M04-028 |
| TC-02 | Save valid campaign, inspect category/FCFS columns | NULL | BR-M04-028 |
| TC-03 | Edit and save | new `CampgainMasterId` | BR-M04-012 |
| TC-04 | Save success | SMTP send attempted | section 24 |

## 28. Reverse Engineering References
`Solution.Web/MasterSetup_UI/CampaignSetup_new.aspx(.cs)`; diffs vs `OLdCampaignSetup.aspx.cs` and `CampaignSetup_final.aspx.cs`.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | diffed | SP bodies absent | MISSING | - |
| Multi-product | None existed | UI stub only | n/a | INCOMPLETE FEATURE | BR-M04-028 |

## 30. Known Gaps
Legacy page deployed (GAP-M04-006); non-persistent stub; live SMTP with embedded credential (GAP-M04-003).

## 31. Change Impact
Shares `SaveBonusCampaign` and seed SPs with PAGE-111/113/115/117; independent removal possible after menu check.
