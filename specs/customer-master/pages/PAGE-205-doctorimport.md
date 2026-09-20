---
page_id: PAGE-205
module: M05
page: DoctorImport
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-205 — DoctorImport ("Doctor Updation")

> **Evidence level:** Tier A. Full diff against `CustomerImport` (PAGE-203) performed (`diff` of `.aspx` and `.aspx.cs`); `BonusCampaignNewDAL.SaveDoctorInfoForExcel` (`:862-937`), `OrderTrackingDAL` doctor approval methods and `DoctorUpdationApprove` (PAGE-206) read. Bodies of `sp_Save_DoctorPropUpdateMaster`, `sp_Save_DoctorPropUpdateDetail`, `sp_GetDoctorProviderTypeApproveList`, `sp_Update_DoctorPropUpdate` NOT in repo (UNKNOWN). Doctor-master SPs `sp_Save_DoctorMaster`, `sp_Update_DoctorMaster`, `sp_Get_DoctorList` (bodies read, `LIVE_DB…sql:671-929`) belong to the doctor entry pages but are documented here in §16/§19 because they define doctor code generation and tagging that this import ultimately affects.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-205 |
| Module | M05 (doctors are "providers" of the same market structure) |
| Page | DoctorImport (breadcrumb "Doctor Updation") |
| Kind | Upload / Import → pending batch (stage 1 of 2) |
| Folder / class | `TransferUI` / `TransferUI_DoctorImport` |
| Twin | PAGE-203 CustomerImport (identical structure; see that spec for shared details) |

## 2. Purpose
Bulk-change the **market**, **provider type**, **doctor type** or **pharma platform** of many doctors from an Excel file, saved as a pending batch for approval on `DoctorUpdationApprove.aspx` (PAGE-206). CONFIRMED (staging) / INFERRED (effect).

## 3. Business Objective
Re-classify or re-territory doctors in bulk without editing each doctor record. INFERRED.

## 4. Navigation
"Go to Approval List" → `DoctorUpdationApprove.aspx`; template `../ExcelFiles/DoctorFormat.xls`; success/Reset → `DoctorImport.aspx`.

## 5. Route
`~/TransferUI/DoctorImport.aspx[?MasID=]` (`MasID` leads to the same dead requisition loader as PAGE-203).

## 6. Source Files
`Solution.Web/TransferUI/DoctorImport.aspx(.cs)`; `Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs:862-937`; DAOs `CustomerPropUpdateMasterDAO/DetailDAO` (shared, `Library.DAO/DoctorModule_DAO/`); approval `Solution.Web/TransferUI/DoctorUpdationApprove.aspx(.cs)`, `OrderTrackingDAL.cs:386-405,691-719`.

## 7. UI Control Inventory
Differences from PAGE-203 (everything else identical):
| Control | Doctor variant |
|---|---|
| `rbType` items | `Market Update` (default), `Provider Type`, **`Customer Type` value shown as "Doctor Type Update"**, `Pharma Platform Type` (order swapped vs PAGE-203) — `DoctorImport.aspx:147-152` |
| Grid columns | "Doctor Code" (`Eval("DoctorCode")`), "Provider Type Code", "Doctor Type Code" (label ID still `lblCustomerTypeCode`, `Eval("DoctorTypeCode")`), "Market Code", "Pharma Platform Code" |
| Template link | `DoctorFormat.xls` |
| Back button | "Go to Approval List" → PAGE-206 |

## 8. Field Mapping
As PAGE-203 §8 with: DoctorCode → `CustCode` → `@CustCode`; DoctorTypeCode → `CustTypeCode` → `@CustTypeCode`; SPs `sp_Save_DoctorPropUpdateMaster` (`@CustPropMasterId,@TypeId=1,@ConvertType,@EntryBy`) and `sp_Save_DoctorPropUpdateDetail` (`@CustPropMasterId,@CustPropUpdateDetailId,@CustCode,@ProviderType,@CustTypeCode,@MarketCode,@PharmaPlatformCode`). Note the DAO/param names still say "Cust". Source `DoctorImport.aspx.cs:522-560`, `BonusCampaignNewDAL.cs:862-937`.

## 9. Dropdown Mapping
None.

## 10. Button Action Mapping
| Button | Handler | DAL | SP | Effect |
|---|---|---|---|---|
| btnUpload | `ExcelToGrid` `:628` | — | — | preview |
| submitButton | `submitButton_Click` | `SaveDoctorInfoForExcel` | `sp_Save_DoctorPropUpdateMaster`, `sp_Save_DoctorPropUpdateDetail` | pending doctor batch |
| approve (PAGE-206) | `loadGridView_RowCommand("ApproveData")` | `OrderTrackingDAL.SaveDoctorProvider_ApplogDAL` | `sp_Update_DoctorPropUpdate(@CustPropMasterId,@UpdateBy)` | apply batch (UNKNOWN) |

## 11. Frontend Flow
Same as PAGE-203. `ExcelToGrid` here has **no** empty-file check and **no** try/catch (differs from PAGE-203): choosing no file, or a non-Excel extension, throws an unhandled exception (NRE on `id_fu.PostedFile` / null `oledbConn`).

## 12. Backend Flow
Identical to PAGE-203 except `SaveDoctorInfoForExcel`. The DAL is a copy-paste of the customer method and is atomic (single `DataAccessManager` transaction; `SqlConnectionClose(true)` on exception).

## 13. API Mapping
None.

## 14. Service Mapping
None (DAL direct: `BonusCampaignNewDAL`).

## 15. Repository Mapping
`BonusCampaignNewDAL.SaveDoctorInfoForExcel`; approvals `OrderTrackingDAL.GetDoctorProviderApprovalList` (`sp_GetDoctorProviderTypeApproveList`), `SaveDoctorProvider_ApplogDAL` (`sp_Update_DoctorPropUpdate`).

## 16. Stored Procedure Mapping
| SP | Body | Notes |
|---|---|---|
| `sp_Save_DoctorPropUpdateMaster/Detail`, `sp_GetDoctorProviderTypeApproveList`, `sp_Update_DoctorPropUpdate` | NOT in repo | UNKNOWN |
| `sp_Save_DoctorMaster` | READ `LIVE_DB…sql:672-725` | INSERT `tblDoctorMaster`: code = `'D'` + (MAX numeric suffix of `DoctorCode` where `ApprovalStatus='2'`)+1; `ApprovalStatus='2'` (auto-approved), `IsFromApp=0`, `EntryBy=@EntryBy`, `EntryDate=GETDATE()`; if `@TaggedSalesPersonIds` null and `@SalesPersonId` given ⇒ tagged = SalesPersonId; returns `SCOPE_IDENTITY()`. Columns written: DoctorName, DoctorCode, SecondaryCode, DesignationId, Gender, IsActive, Activedate, EntryBy, EntryDate, DivisionId, DistrictId, ThanaId, IsFromApp, ApprovalStatus, UPCode, DoctorTypeId, MarketId, UnionName, Reamrks, StationTypeId, ProgramTypeId, DoctorCategoryId, InactiveDate, SMCTypeId, SalesPersonId, TaggedSalesPersonIds, TaggedSalesPersonCodes, StreetAddress. |
| `sp_Update_DoctorMaster` | READ `:735-811` | UPDATE same columns (+UpdateBy/UpdateDate, `SalesPersonId`, tagged fields) by `@DoctorId`, then DELETE child rows `tblDoctorBrandDetail`, `tblDoctorDegreeDetail`, `tblDoctorSpecialityDetail`, `tblDoctorSpecialDayDetail`, `tblDoctorContactDetail` and `tblDoctorChemberDetail` except chambers referenced by `tbl_PrescriptionMaster` or `tbl_DCRInfo`; the calling DAL re-inserts details. |
| `sp_Get_DoctorList` | READ `:884-929` | Dynamic SQL list (`@Parm` appended): tagged sales persons string (`STUFF … FOR XML`), chamber info, contacts, degree, speciality, program types, approval status label (0 Pending, 1 Verified, 2 Approved, 3 Rejected), `WHERE isnull(DM.IsDoctor,0)=1`, `ORDER BY DM.DoctorCode DESC`. |

## 17. Database Mapping
Import stage: UNKNOWN tables. Doctor master: `tblDoctorMaster` and detail tables above; joins `tblDoctorDesignation`, `tblProgramType`, `tblDoctorType`, `tblMarket…tbl_Group`, `tblSMCType`, `tblUser`.

## 18. Validation Matrix
As PAGE-203 minus the empty-file check. `Validation()` (grid non-empty) alerts "Tabel Can not be Empty!" but still returns true.

## 19. Business Rules
| ID | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M05-035…039 | Same staging, atomicity, no-validation and approval semantics as PAGE-203 (apply to doctors). | `DoctorImport.aspx.cs`; `BonusCampaignNewDAL.cs:862-937` | — | CONFIRMED / INFERRED |
| BR-M05-040 | Doctor sheet columns: DoctorCode, ProviderTypeCode, DoctorTypeCode, MarketCode, PharmaPlatformCode (bound by header name); radio `Customer Type` means *Doctor Type* here. | `DoctorImport.aspx.cs:87-103`; `DoctorImport.aspx:147-152,205,218` | page | CONFIRMED |
| BR-M05-041 | Doctor approval PAGE-206 lists `sp_GetDoctorProviderTypeApproveList("")` batches and approves with `sp_Update_DoctorPropUpdate`; headers only, no reject. | `DoctorUpdationApprove.aspx.cs:26-63` | PAGE-206 | CONFIRMED |
| BR-M05-050 | Doctor code generation and auto-approval: `D<n>` with n=max suffix of approved doctors +1, new doctors saved with `ApprovalStatus='2'`, `IsFromApp=0` (same race/robustness caveats as customer code, BR-M05-004). | `LIVE_DB…sql:700-720` | `sp_Save_DoctorMaster` | CONFIRMED |
| BR-M05-051 | Doctor edit rebuilds detail collections (brand/degree/speciality/special-day/contact deleted then re-inserted by DAL) and keeps only chambers that are referenced by prescriptions/DCR; primary sales person and tagged sales persons follow the customer convention (BR-M05-007). | `LIVE_DB…sql:763-809`; `DoctorDAL.cs:153-280` | `sp_Update_DoctorMaster` | CONFIRMED |

## 20. Permission Matrix
Same as PAGE-203: `Session["UserId"]` only, no role check; PAGE-206 also unrestricted. CONFIRMED.

## 21. Status Lifecycle
Import batch: Pending → Approved (INFERRED). Doctor master `ApprovalStatus` `'0'/'1'/'2'/'3'` = Pending/Verified/Approved/Rejected (labels in `sp_Get_DoctorList`); web-created doctors start at `'2'`.

## 22. Error Handling
Weaker than PAGE-203 (no try/catch around upload). Save failure ⇒ "Already Exist!" or unhandled exception after rollback.

## 23. Audit
As PAGE-203 (master EntryBy/date; approver UpdateBy).

## 24. Integration
None external.

## 25. Reports
None.

## 26. Dependencies
Same as PAGE-203.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Doctor Type Update batch with 2 rows | batch saved with ConvertType "Customer Type" (value), rows with DoctorCode/DoctorTypeCode | 040 |
| TC-02 | Click Upload with no file | unhandled exception (no guard) | — |
| TC-03 | Submit blank grid | blank detail row batch saved | 038 |
| TC-04 | Approve batch in PAGE-206 | success alert; effect UNKNOWN | 041 |
| TC-05 | Create doctor in DoctorEntry (other module) | `DoctorCode='D'+n`, ApprovalStatus '2' | 050 |
| TC-06 | Edit doctor with chamber used in a DCR | chamber row retained, others rebuilt | 051 |

## 28. Reverse Engineering References
Files in §6; `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:671-929`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | — | MISSING |
| Radio label | "Doctor Type Update" | value `Customer Type` | INFO |

## 30. Known Gaps
See PAGE-203 §30; additionally missing upload guards, DAO/SP naming ("CustProp…") reused for doctors, and value/label mismatch of the radio option. GAP-M05-016/017/025.

## 31. Change Impact
Shares DAO classes with PAGE-203; `sp_Save_DoctorMaster/sp_Update_DoctorMaster/sp_Get_DoctorList` are called from `DoctorDAL.cs`, `SetupDAL.cs`, `Setup2DAL.cs` and used by Doctor entry/view, approval list and transfer pages (`Approval_UI/DoctorApprovalList.aspx`, `MasterSetup_UI/DoctorEntry.aspx`, …).
