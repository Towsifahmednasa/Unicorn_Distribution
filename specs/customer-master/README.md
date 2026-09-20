# M05 — Customer & Provider Master

Reviewed 2026-09-19. Documentation only; nothing in the application was changed. Tags: CONFIRMED / INFERRED / UNKNOWN. See `GAPS-M05.md` (32 gaps), `workflows/`, `reports/`.

## 1. Purpose & business overview
Maintains the outlets ("customers"/providers: pharmacies, chemists) and their classification in `tblCustMaster`: market hierarchy, customer type, provider (program) type, pharma platform (SMC type), station types, tagged sales persons, product lines; per-company **credit limits** with maker/checker approval that block invoice generation; bulk maintenance through Excel (legacy upload→verify→transfer, and bulk property update import→approve); classification set-up pages; legacy SAP-era customer pages that still write a code-based column set. Two generations coexist: modern SP-based `MasterSetup_UI` and legacy inline-SQL `SInventory_UI` (GAP-M05-028).

## 2. Page catalogue (40 pages)
Tier A = fully reviewed (link); B = generated only (Tier-B specs, plus facts folded into workflows).
| Page ID | Page | Kind | Tier | Spec |
|---|---|---|---|---|
| PAGE-167 | CustomerChangeProgramType | search + inline edit of provider type | A | [pages/PAGE-167](pages/PAGE-167-customerchangeprogramtype.md) |
| PAGE-168 | CustomerEntry | customer create/edit (SP-based) | A | [pages/PAGE-168](pages/PAGE-168-customerentry.md) |
| PAGE-169 | CustomerListPending | customer list clone of 173 | B | [pages/PAGE-169](pages/PAGE-169-customerlistpending.md) |
| PAGE-170 | CustomerType | customer type form (ajax) | B | [pages/PAGE-170](pages/PAGE-170-customertype.md) |
| PAGE-171 | CustomerTypeSetup | WebMethod host for 170/172 | B | [pages/PAGE-171](pages/PAGE-171-customertypesetup.md) |
| PAGE-172 | CustomerTypeView | customer type list | B | [pages/PAGE-172](pages/PAGE-172-customertypeview.md) |
| PAGE-173 | CustomerView | customer list/export/edit launcher | A | [pages/PAGE-173](pages/PAGE-173-customerview.md) |
| PAGE-174 | ProgramType | provider type form | B | [pages/PAGE-174](pages/PAGE-174-programtype.md) |
| PAGE-175 | ProgramTypeView | provider/SMC type list + WebMethod host | B | [pages/PAGE-175](pages/PAGE-175-programtypeview.md) |
| PAGE-176 | SMCType | pharma platform form | B | [pages/PAGE-176](pages/PAGE-176-smctype.md) |
| PAGE-177 | SMCTypeView | pharma platform list | B | [pages/PAGE-177](pages/PAGE-177-smctypeview.md) |
| PAGE-178 | StationTypeEntry | station type form | B | [pages/PAGE-178](pages/PAGE-178-stationtypeentry.md) |
| PAGE-179 | StationTypeView | station type list | B | [pages/PAGE-179](pages/PAGE-179-stationtypeview.md) |
| PAGE-180 | CustomerMasterViewer | report exporter (RPT-01) | B | [pages/PAGE-180](pages/PAGE-180-customermasterviewer.md) |
| PAGE-181 | CustVerUnverExcelViewer | report exporter (RPT-02) | B | [pages/PAGE-181](pages/PAGE-181-custverunverexcelviewer.md) |
| PAGE-182 | ActiveCustomerMasterList | legacy list | B | [pages/PAGE-182](pages/PAGE-182-activecustomermasterlist.md) |
| PAGE-183 | CustCategoryEdit | legacy category edit | B | [pages/PAGE-183](pages/PAGE-183-custcategoryedit.md) |
| PAGE-184 | CustCategoryView | legacy category list | B | [pages/PAGE-184](pages/PAGE-184-custcategoryview.md) |
| PAGE-185 | CustMasterEdit | legacy edit (all guards commented) | B | [pages/PAGE-185](pages/PAGE-185-custmasteredit.md) |
| PAGE-186 | CustMasterEntry | legacy entry (manual code) | A | [pages/PAGE-186](pages/PAGE-186-custmasterentry.md) |
| PAGE-187 | CustMasterView | legacy lookup by code | B | [pages/PAGE-187](pages/PAGE-187-custmasterview.md) |
| PAGE-188 | CustomerCategoryEntry | legacy category entry | B | [pages/PAGE-188](pages/PAGE-188-customercategoryentry.md) |
| PAGE-189 | CustomerCreditLimitApproval | credit-limit approve/reject | A | [pages/PAGE-189](pages/PAGE-189-customercreditlimitapproval.md) |
| PAGE-190 | CustomerCreditLimitEntry | credit-limit create/edit | A | [pages/PAGE-190](pages/PAGE-190-customercreditlimitentry.md) |
| PAGE-191 | CustomerCreditView | credit-limit list/edit/soft delete | B | [pages/PAGE-191](pages/PAGE-191-customercreditview.md) |
| PAGE-192 | CustomerExcelUpload | Excel upload stage 1 | A | [pages/PAGE-192](pages/PAGE-192-customerexcelupload.md) |
| PAGE-193 | CustomerList | upload batches: verify/transfer/delete | B | [pages/PAGE-193](pages/PAGE-193-customerlist.md) |
| PAGE-194 | CustomerMasterEdit | legacy edit (guards active, `UD_CustomerMaster2`) | B | [pages/PAGE-194](pages/PAGE-194-customermasteredit.md) |
| PAGE-195 | CustomerMasterReport | report launcher (RPT-01) | B | [pages/PAGE-195](pages/PAGE-195-customermasterreport.md) |
| PAGE-196 | CustomerMasterView | legacy list | B | [pages/PAGE-196](pages/PAGE-196-customermasterview.md) |
| PAGE-197 | CustomerTagChangeExcelUpload | tag-change upload (clone of 192) | B | [pages/PAGE-197](pages/PAGE-197-customertagchangeexcelupload.md) |
| PAGE-198 | CustomerTagChangeList | tag-change batches verify/transfer | B | [pages/PAGE-198](pages/PAGE-198-customertagchangelist.md) |
| PAGE-199 | FixedCustomerInformation | fixed-customer list + sales report | B | [pages/PAGE-199](pages/PAGE-199-fixedcustomerinformation.md) |
| PAGE-200 | FixedCustomerUpload | fixed/regular flag upload | B | [pages/PAGE-200](pages/PAGE-200-fixedcustomerupload.md) |
| PAGE-201 | MultiCustomerEdit | bulk re-assignment | B | [pages/PAGE-201](pages/PAGE-201-multicustomeredit.md) |
| PAGE-202 | CustomerDocTransferApproval | customer/doctor market-transfer approval | B | [pages/PAGE-202](pages/PAGE-202-customerdoctransferapproval.md) |
| PAGE-203 | CustomerImport | bulk customer property import | A | [pages/PAGE-203](pages/PAGE-203-customerimport.md) |
| PAGE-204 | CustomerProviderTypeApprove | approve customer import batch | B | [pages/PAGE-204](pages/PAGE-204-customerprovidertypeapprove.md) |
| PAGE-205 | DoctorImport | bulk doctor property import (+ doctor SPs) | A | [pages/PAGE-205](pages/PAGE-205-doctorimport.md) |
| PAGE-206 | DoctorUpdationApprove | approve doctor import batch | B | [pages/PAGE-206](pages/PAGE-206-doctorupdationapprove.md) |

## 3. Business rules index
| BR | One-liner | Where |
|---|---|---|
| 001 | Only name, type, mobile, address mandatory on CustomerEntry | 168 |
| 002 | Mobile exactly 11 chars | 168 |
| 003 | Same handler creates/updates by MID; user = UserId | 168 |
| 004 | Code `C`+max approved suffix+1, SP-generated | 168 |
| 005 | Web-created customer auto-Approved (`ActionStatus='2'`) | 168 |
| 006 | Geography/station types derived from market, override form | 168 |
| 007 | Tagged sales persons ids/codes; first = SalesPersonId | 168 |
| 008 | Save atomic; update rebuilds product lines | 168 |
| 009 | Update never changes code/status/DC | 168 |
| 010 | No duplicate check; "Already Exist!" generic | 168 |
| 011 | Non-admin restriction dead code | 168 |
| 012 | Provider/platform dropdown filtered by customer flags | 168 |
| 013 | Inactive shown with "(Inactive)" | 168 |
| 014 | Legacy entry: all fields mandatory | 186 |
| 015 | Unique customer code; PK = MAX+1 in app | 186 |
| 016 | Defaults (shipping "N/A", Active); inactive date rule | 186 |
| 017 | Fixed flag, ComUnitName parsing | 186 |
| 018 | Legacy insert omits status/audit/ids | 186 |
| 019 | Credit limit needs company, customer, amount>0, days>0 | 190 |
| 020 | One active limit per company+customer | 190 |
| 021 | Create/edit ⇒ `Posted` | 190 |
| 022 | Approval queue = Posted | 189 |
| 023 | Batch approve/reject, no role/SoD | 189 |
| 024 | Enforce only active+Approved; rows summed | 189 |
| 025 | Permission = no limit OR (order+due ≤ limit AND age ≤ days) | 189 |
| 026 | Due / oldest-unpaid definitions | 189 |
| 027 | Block is UI-level only | 189 |
| 028 | RemainingCreditDays on invoice print | 189 |
| 029 | Credit delete = IsActive 0 | 189 |
| 030 | Excel parsing rules (.xls/.xlsx, first sheet) | 192 |
| 031 | Upload requires date, manufacturer, rows, 20 non-blank columns | 192 |
| 032 | Batch header `CMEF-n`, flags false | 192 |
| 033 | Row failures swallowed | 192 |
| 034 | Verify/transfer/delete gates | 192 |
| 035 | Property import = pending batch | 203 |
| 036 | Update type radio → ConvertType; 5 columns | 203 |
| 037 | Batch atomic | 203 |
| 038 | No code validation; blank batch possible | 203 |
| 039 | Import approval: headers only, approve only | 203 |
| 040 | Doctor import columns/type mapping | 205 |
| 041 | Doctor approval chain | 205 |
| 042 | Provider-type search needs a type; some filters ignored | 167 |
| 043 | Provider-type change immediate | 167 |
| 044 | Only customer-flagged provider types | 167 |
| 045 | Default 30-day window, no mandatory scope | 173 |
| 046 | Add/Edit by RAdd/REdit; role 2 bypass | 173 |
| 047 | ActionStatus 0/1/2/3 labels | 173 |
| 048 | Duplicate mobile flagged in approval queue | 173 |
| 049 | Salesperson filter primary or tagged | 173 |
| 050 | Doctor code `D`+max+1, auto-approved | 205 |
| 051 | Doctor edit rebuilds child rows | 205 |
| 052 | DIC DC preset but not filtered | 167 |
| 053 | Customer type: dup name check on edit; default-check dead | WF-07 |
| 054 | Program/SMC/station type save patterns (dup ⇒ failure) | WF-07 |
| 055 | Legacy category: `CUS-nnn` code, unique name | WF-08 |
| 056 | Bulk reassign skips customers with invoices | WF-08 |
| 057 | Fixed/regular flag upload sequence | WF-08 |
| 058 | Legacy edit guards differ between 185 and 194; proc-name mismatch | WF-08 |
| 059 | Tag-change batches `CTME-n`, own tables/SPs | WF-04 |

## 4. Status lifecycles
- **Customer** `tblCustMaster.ActionStatus`: `0` Pending → `1` Verified → `2` Approved / `3` Rejected (labels CONFIRMED in `sp_Get_CustMasterList_Approve`); web create ⇒ `2`; transitions of 0/1/3 by approval flow (UNKNOWN SP). `IsActive` separate.
- **Credit limit**: `Posted` → `Approved` | `Reject`; edit ⇒ `Posted`; delete ⇒ `IsActive=0`.
- **Excel batch**: Uploaded → Verified (`VerifyedAll`) → Transferred (`Transfer`) | Deleted (before transfer).
- **Property import batch**: Pending → Approved (INFERRED).

## 5. Roles & permissions observed
Almost no in-code enforcement: menu-based only. Exceptions: PAGE-173 (`UserRoleID`, `RAdd/REdit`), DIC/DZSM/AM DC-dropdown locks (cosmetic), `CustomerMasterReport` reads `Session["UserType"]`. Credit approval, imports, approvals: none.

## 6. Data model
Tables: `tblCustMaster` (core; `ActionStatus`, `TaggedSalesPersonIds/Codes`, `SalesPersonId`, geography ids and legacy code columns), `tblCustProductLine`, `tblCustomerType`, `tblProgramType`, `tblSMCType`, `tblStationType`, `tblCustCategory`, `tblCustomerCreditLimit`, `tblCompanyInfo`, `tblCustomerApprovalLog`, `tblCustomerMasterExcelFileMaster/Detail`, `tblCustomerMasterTagChangeExcelFileMaster/Detail`, `tblDoctorMaster` (+details), views `View_CustomerMaster`, `View_CustomerForModification`.
SPs with bodies read (`LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql`): `sp_Save_CustomerMaster`, `sp_Update_CustomerMaster`, `sp_UD_CustomerMaster`, `sp_Get_CustMasterList_Approve`, `sp_Get_CustomerApp`, `sp_Save_DoctorMaster`, `sp_Update_DoctorMaster`, `sp_Get_DoctorList`, `sp_LoadOrderListForOrderCreationbyTerri`. All others UNKNOWN.

## 7. Integrations
Excel exchange (SAP-format files); order-to-invoice consumes credit limits; approval-workflow module (PAGE-695) works the pending queue; mobile API (creation path UNKNOWN); cross-DB `CSTL_ACCDB_Unicorn` in enforcement.

## 8. Reports
`reports/RPT-M05-01` customer master (Crystal→Excel), `RPT-M05-02` verified/unverified upload rows, `RPT-M05-03` CSV exports.

## 9. Workflows
WF-M05-01 create/edit · 02 approval queue · 03 credit limit · 04 Excel upload/verify/transfer · 05 bulk property import/approval · 06 provider-type change · 07 classification set-up · 08 legacy maintenance (all in `workflows/`).

## 10. Known gaps (summary)
Injection via concatenated filters and generic SQL executor with `sa` login; racy code generation; auto-approval; images/platform not saved; credit enforcement UI-only and releasable by edit/reject; silent failures in Excel upload; blind import approvals; legacy/modern duplication. Details: `GAPS-M05.md`.

## 11. Still unverified
Menu/roles; SP bodies (verify/transfer/import/approve/seed); table/view DDL and constraints; mobile customer creation and status transitions; contents of `ExcelFiles/`; Tier-B pages beyond mechanical extraction (PAGE-169–172, 174–179, 182–185, 187–188, 191, 193–202, 204, 206).
