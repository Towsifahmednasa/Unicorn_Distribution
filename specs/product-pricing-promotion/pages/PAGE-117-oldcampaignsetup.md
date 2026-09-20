---
page_id: PAGE-117
module: M04
page: OLdCampaignSetup
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "PARTIAL", "database": "PARTIAL", "rules": "PARTIAL", "permission": "PARTIAL", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-117 - OLdCampaignSetup (2024-12 snapshot; NOT linked)

> **Evidence level:** Tier A by full diff. `OLdCampaignSetup.aspx.cs` equals `CampaignSetupPT.aspx.cs` except the class name (`diff -wB` = 4 lines); the aspx equals `CampaignSetupPT.aspx` minus the unbound payment radio (only the Page directive and lines 231-249 differ). Diffs against `CampaignSetup.aspx.cs` (215 lines) and `CampaignSetup_new.aspx.cs` (215 lines) read in full. Behaviour not listed = PAGE-111 header/offer logic. SP bodies absent (UNKNOWN). Full behavioural description is in PAGE-115; this file records the identity of the twin.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-117 |
| Module | M04 |
| Page name | OLdCampaignSetup |
| Kind | Entry / Transaction (legacy) |
| Web folder / master | MasterSetup_UI / NewMasterPage.master |
| Page class | MasterSetup_UI_OLdCampaignSetup |
| Files | aspx 936 lines (2024-12-14 00:49), aspx.cs 1859 lines (2024-12-14 00:46) |
| Live/legacy verdict | **LEGACY / BACKUP COPY.** No inbound link. The name says "old"; the file equals the PT prototype without the payment radio and is dated after it, so it is a renamed snapshot of the pre-rewrite page taken before `CampaignSetup.aspx` gained payment-type flags and multi-customer-type saving. INFERRED. |

## 2. Purpose
Campaign setup with single customer type, category, Trade Policy / FCFS, ratio-wise increment, offers, market and customer lists (see PAGE-115).

## 3. Business Objective
As PAGE-115. INFERRED.

## 4. Navigation
Inbound: none. Outbound: `CampaignView.aspx`, `CampaignSetup.aspx` (Reset `.cs:1857`).

## 5. Route
`~/MasterSetup_UI/OLdCampaignSetup.aspx[?MID=id]`.

## 6. Source Files
As PAGE-111; `IVMarketStructure.ascx` visible; DAL `SaveBonusCampaign`.

## 7. UI Control Inventory
As PAGE-115 without the `PaymentOptions` radio: single-select `ddlChemistType` (aspx:154), category, FCFS/Trade Policy, ratio-wise, Active; market/customer blocks visible.

## 8. Field Mapping
As PAGE-115 section 8 (payment flags never set; `CustomerTypeId` single; `.cs:1591,1616-1618,1625`).

## 9. Dropdown Mapping
As PAGE-111 with single customer type.

## 10. Button Action Mapping
As PAGE-111.

## 11. Frontend Flow
As PAGE-111.

## 12. Backend Flow
As PAGE-115: `SaveBonusCampaign` (`.cs:1625`), then active `smtp.Send(mail)` (`.cs:1681`), redirect to `CampaignView.aspx` (`:1696`).

## 13. API Mapping
`GetCustomer_New` autocomplete.

## 14. Service Mapping
No BLL; static DAL (GAP-M04-013).

## 15. Repository Mapping
As PAGE-111.

## 16. Stored Procedure Mapping
As PAGE-115 (map-id session value null). Bodies UNKNOWN.

## 17. Database Mapping
As PAGE-111.

## 18. Validation Matrix
As PAGE-111.

## 19. Business Rules
BR-M04-029 (defined on PAGE-115; identical code) applies. Rules BR-M04-001..003, 005..007, 011, 012, 014, 015 (PAGE-111) apply; BR-M04-004 satisfiable; BR-M04-008 => single master.

| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M04-029 | (see PAGE-115) identical code-behind to PT: payment flags not persisted, edit = delete + new id, active SMTP send | OLdCampaignSetup.aspx.cs:1591,1616-1618,1625,1681 | `btnSave_Click` | CONFIRMED |

## 20. Permission Matrix
Session `UserId` only; no role check; menu-based. Roles UNKNOWN.

## 21. Status Lifecycle
IsActive + date window; edit replaces id.

## 22. Error Handling
As PAGE-115 (outer try/catch -> `faildalert(ex.Message)`; silent seed catches; failure text always "Already Exist!").

## 23. Audit
`@EntryBy` = `Session["UserId"]`.

## 24. Integration
Active Gmail SMTP send with hard-coded credential (`<redacted>`).

## 25. Reports
None.

## 26. Dependencies
As PAGE-111.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Save campaign, inspect IsPTforCOD/IsPTforOther | NULL | BR-M04-029 |
| TC-02 | Edit and save | new `CampgainMasterId` | BR-M04-012 |
| TC-03 | Success save | SMTP send attempted | section 24 |

## 28. Reverse Engineering References
`Solution.Web/MasterSetup_UI/OLdCampaignSetup.aspx(.cs)`; diffs vs `CampaignSetupPT`, `CampaignSetup`, `CampaignSetup_new`.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | diffed | SP bodies absent | MISSING | - |

## 30. Known Gaps
Duplicate legacy page (GAP-M04-006); active SMTP with hard-coded credential (GAP-M04-003).

## 31. Change Impact
Shares `SaveBonusCampaign` with PAGE-111/113/114/115. Independent removal possible after menu check.
