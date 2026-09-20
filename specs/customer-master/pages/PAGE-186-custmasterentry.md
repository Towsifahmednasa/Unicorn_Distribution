---
page_id: PAGE-186
module: M05
page: CustMasterEntry
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-186 — CustMasterEntry (legacy customer master entry)

> **Evidence level:** Tier A. `.aspx.cs`, `CustomerMasterInfoBll`, `CustomerMasterInfoDal`, `SInventorySql`, `ClsCommonInternalDAL`, `ClsPrimaryKeyFind` read in full. All SQL is inline (no SP bodies needed). Table/view DDL (`tblCustMaster`, `View_CustomerMaster`) is not in the repo. This is the SAP-era ("legacy") customer entry that predates `MasterSetup_UI/CustomerEntry.aspx` (PAGE-168).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-186 |
| Module | M05 |
| Page name | CustMasterEntry |
| Kind | Entry form (create only) |
| Folder / master | `SInventory_UI` / `~/MasterPages/MainMasterPage.master` |
| Class | `SInventory_UI_CustMasterEntry` |
| Duplicates | `CustMasterEdit.aspx` (PAGE-185) edits the same fields; `MasterSetup_UI/CustomerEntry.aspx` (PAGE-168) is the current replacement and uses a different column set and SP-based save |

## 2. Purpose
Enter a customer using SAP-style master data: manually typed customer code, category, contact/address details and the legacy sales hierarchy (DZSM region, FE district, territory/area, MIO, market, distribution center), payment term and fixed/regular flag. CONFIRMED (`CustMasterEntry.aspx.cs:224-301`).

## 3. Business Objective
Register a customer whose code comes from the ERP/SAP customer master so that invoicing and legacy sales reports (`View_CustomerMaster`) can group sales by hierarchy. INFERRED.

## 4. Navigation
Menu DB-driven (UNKNOWN). List link `custMasterListImageButton` → `CustomerMasterView.aspx` (`:303-306`). Inbound: `CustMasterView.aspx`, `CustomerMasterView.aspx`, `ActiveCustomerMasterList.aspx`, `FixedCustomerInformation.aspx` "new" buttons → this page.

## 5. Route
`~/SInventory_UI/CustMasterEntry.aspx`.

## 6. Source Files
`Solution.Web/SInventory_UI/CustMasterEntry.aspx(.cs)`; `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs`; `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs`; `SInventorySql.cs`; `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `ClsPrimaryKeyFind.cs`; entity `Library.DAO/SInventory_Entities/CustomerMaster`.

## 7. UI Control Inventory
| Control | Meaning | Required (server) | Default | Behaviour |
|---|---|---|---|---|
| `customerCodeTextBox` | Customer Code (manual) | yes | empty (auto code commented out `:23,291`) | uniqueness checked in BLL |
| `customernameTextBox` | Customer Name | yes | — | — |
| `categoryNameDropDownList` | Category | yes (`SelectedIndex==0`) | — | `SELECT DISTINCT CategoryId,CategoryCode,CategoryName FROM tblCustCategory` |
| `addressTextBox` / `addressTextBox2` | Address / Address 2 | both yes | — | — |
| `contactTextBox` | Mobile/contact | yes | — | no format check |
| `cityTextBox`, `contactPersonTextBox` | City, contact person | yes | — | — |
| `shippingConditionTextBox` | Shipping Condition | yes | "N/A" (`SetShipingCondition`) | — |
| `paymentTypeDropDownList` | Term of payment | no | first item ("Cash", static ListItem) | — |
| `regionNameDropDownList` (+ readonly `dzsmNameTextBox`) | DZSM/Region | yes | — | postback fills DZSM name from `tblRegion` by RegionCode |
| `districtNameDropDownList` (+ `feNameTextBox`) | FE/District | yes | — | fills FE name from `tblDistrict` by DistrictCode |
| `areaNameDropDownList` (+ `territoryNameTextBox`) | Area/Territory | yes | — | from `tblArea` |
| `miaNameDropDownList` (+ `mioNameTextBox`) | MIO/MIA | yes | — | from `tblMIAInfo` |
| `marketNameDropDownList` (+ `marketNameTextBox`) | Market | yes | — | from `tblMarket` by MarketCode |
| `comUnitNameDropDownList` | Distribution center | yes | — | `tblCompanyUnit` (`Name:Code`) |
| `fbDropDownList` | Fixed customer? | no | False(0) / True(1) | `"1"` ⇒ FixedCustomer=true |
| `isActiveCheckBoxList` | Active | — | checked (`SetDefaultType`) | unchecked enables `inActiveDateTextBox` + calendar; required when inactive |
| `submitButton` | Submit | — | — | `submitButton_Click1` |
Error text: modal `alert()` via `showMessageBox`, exact texts in §18.

## 8. Field Mapping
| UI control | Entity property | SQL parameter | `tblCustMaster` column |
|---|---|---|---|
| customerCodeTextBox | CustomerCode | @CustomerCode | CustomerCode |
| categoryNameDropDownList | CategoryId | @CategoryId | CategoryId |
| customernameTextBox | CustomerName | @CustomerName | CustomerName |
| addressTextBox / 2 | Address / Addrees2 | @Address / @Addrees2 | Address / Addrees2 |
| contactTextBox | CellNo | @CellNo | CellNo |
| cityTextBox | City | @City | City |
| contactPersonTextBox | ConPerson | @ConPerson | ConPerson |
| shippingConditionTextBox | ShippingCond | @ShippingCond | ShippingCond |
| marketNameDropDownList value / marketNameTextBox | MarketCode / MarketName | @MarketCode / @MarketName | MarketCode / MarketName |
| miaNameDropDownList value / mioNameTextBox | MIACode / MiaName | @MIACode / @MiaName | MIACode / MIAName |
| areaNameDropDownList | AreaCode | @AreaCode | AreaCode |
| districtNameDropDownList / feNameTextBox | DisCode / FEName | @DisCode / @FEName | DisCode / FEName |
| comUnitNameDropDownList value / text before ':' | ComUnitCode / ComUnitName | @ComUnitCode / @ComUnitName | ComUnitCode / ComUnitName |
| regionNameDropDownList / dzsmNameTextBox | RegionCode / DZSMName | @RegionCode / @DZSMName | RegionCode / DZSMName |
| paymentTypeDropDownList | TermOfPayment | @TermOfPayment | TermOfPayment |
| (none) | CustomerCodeOld | @CustomerCodeOld (null) | CustomerCodeOld |
| fbDropDownList | FixedCustomer | @FixedCustomer | FixedCustomer |
| isActive / inActiveDateTextBox | IsActive / InActiveDate | @IsActive / @InActiveDate | IsActive / InActiveDate |
| (PK) | CustomerMasterId | inserted explicitly | CustomerMasterId = `SELECT ISNULL(MAX(CustomerMasterId),0)+1` (`ClsPrimaryKeyFind.cs:29-45`) |
Source: `CustomerMasterInfoDal.SaveCustometMasterInfo` `:95-129`, `CustomerMasterParameters` `:319-349`. **Not written:** CreateBy/CreateDate, ActionStatus, MarketId, Group/Region ids, ComUnitId, any status (GAP-M05-019).

## 9. Dropdown Mapping
All loaded by inline SQL through `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` (`CustomerMasterInfoDal.cs:16-63`): `tblCompanyUnit` (value ComUnitCode), `tblRegion` (RegionCode), `tblDistrict` (DistrictCode), `tblArea` (AreaCode), `tblMIAInfo` (MiaCode), `tblMarket` (MarketCode), `tblCustCategory` (CategoryId). Name look-ups on selection use `WHERE <Code> = @param` (`:65-93`).

## 10. Button Action Mapping
| Button | Handler | BLL | DAL | SQL | Effect |
|---|---|---|---|---|---|
| submitButton | `submitButton_Click1` `:224` | `CustomerMasterInfoBll.SaveCustMasterInfo` | `HasCustomerMastername`, `SaveCustometMasterInfo` | `select * from tblCustMaster where CustomerCode=@CustomerCode`; `INSERT INTO dbo.tblCustMaster(…)` | one row |
| custMasterListImageButton | redirect | — | — | — | `CustomerMasterView.aspx` |
| 5 select handlers | `*_SelectedIndexChanged/OnTextChanged` | `LoadDZSMName`,`LoadFEName`,… | `SELECT … WHERE Code=@code` | read only |

## 11. Frontend Flow
Pure postback WebForms; each dependent name box refreshes on dropdown change (AutoPostBack). Inactive date box enabled/cleared by `isActiveCheckBoxList_OnCheckedChanged` (`:361-374`). No page JS beyond calendar extender.

## 12. Backend Flow
`submitButton_Click1` → `Validation()` → build `CustomerMaster` (ComUnitName = text before ':' of the selected item; fixed flag) → `CustomerMasterInfoBll.SaveCustMasterInfo`: `HasCustomerMastername` (existence of same `CustomerCode`) → if free: `CustomerMasterId = PrimaryKeyMax("CustomerMasterId","tblCustMaster")` → `SaveCustometMasterInfo` (`SInventorySql.Execute` → `ClsCommonInternalDAL.UpdateDataByUpdateCommandNew`: parameterised text command, `ExecuteNonQuery>0`) → `true`; else `false`. Page shows "Data Save Successfully Customer Master Code  is :  <code> And Customer Master Name is :<name>" and clears, or " Customer Code already exist". No transaction spanning check + max-id + insert; the insert result (bool) is ignored by the BLL (always returns true after calling it).

## 13. API Mapping
None.

## 14. Service Mapping
`CustomerMasterInfoBll` (page-scoped), also instantiates unused `CustomerMasterBLL` (`:286`).

## 15. Repository Mapping
`CustomerMasterInfoDal`: `SaveCustometMasterInfo`, `HasCustomerMastername`, `Get*Info`, `Get*NameById`; helper `SInventorySql.{GetDataTable,Exists,Execute}`; `ClsPrimaryKeyFind.PrimaryKeyMax`.

## 16. Stored Procedure Mapping
No SP named by the page. `ClsCommonInternalDAL` reads use the generic proc `ExecuteAllSqlQueryByStoreProcedure(@Query)` (body NOT in repo — UNKNOWN; runs supplied SQL text) for dropdown loads; save uses `db.GetSqlStringCommand` (text command).

## 17. Database Mapping
Read: `tblCompanyUnit`, `tblRegion`, `tblDistrict`, `tblArea`, `tblMIAInfo`, `tblMarket`, `tblCustCategory`, `tblCustMaster`. Write: `tblCustMaster` (INSERT). Connection string key `SolutionConnectionStringSSIDB` (`ClsCommonInternalDAL.cs:206`); the live config holds a hard-coded `sa` login (`Solution.Web/Web.config`) — value redacted.

## 18. Validation Matrix
Server `Validation()` (`:98-222`), first failure wins, messages verbatim:
| Order | Condition | Message |
|---|---|---|
| 1 | name empty | Please Input Customer Name!! |
| 2 | address empty | Please Input Address!! |
| 3 | contact empty | Please Input Mobail Number!! |
| 4 | `miaNameDropDownList.Text==""` | Please chose Representitive Name !! |
| 5 | region index 0 | Please Input Region  !! |
| 6 | DC index 0 | Please Input DC  !! |
| 7 | district index 0 | Please Input FE  !! |
| 8 | area index 0 | Please Input Area  !! |
| 9 | MIA index 0 | Please Input MIO  !! |
| 10 | market index 0 | Please Input Market  !! |
| 11 | category index 0 | Please Input Category  !! |
| 12-13 | code empty (checked twice) | Please Input Customer Code  !! |
| 14 | name empty (again) | Please Input Customer Name  !! |
| 15 | address empty (again) | Please Input Customer Address  !! |
| 16 | address2 empty | Please Input Customer Address Second  !! |
| 17 | contact empty (again) | Please Input Customer Contact No  !! |
| 18 | city empty | Please Input Customer City  !! |
| 19 | contact person empty | Please Input Contact Person  !! |
| 20 | shipping cond empty | Please Input Shipping Condition  !! |
| 21 | FE name empty | Please Input FE Name  !! |
| 22 | DZSM name empty | Please Input DZSM Name  !! |
| 23 | inactive and date empty | Please select inactive date !! |
| — | validation failed (extra alert) | Please input data in all Textbox |
| BLL | code already exists | " Customer Code already exist" |

## 19. Business Rules
| ID | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M05-014 | Every visible field is mandatory (list in §18), including manually typed customer code; if "inactive" is chosen an inactive date is required. | `CustMasterEntry.aspx.cs:98-222` | page | CONFIRMED |
| BR-M05-015 | Customer code must be unique: exact match on `tblCustMaster.CustomerCode`; on hit nothing is saved and " Customer Code already exist" is shown. Primary key = `MAX(CustomerMasterId)+1` computed by the application (not identity) — race-prone. | `CustomerMasterInfoBll.cs:77-101`; `CustomerMasterInfoDal.cs:170-177`; `ClsPrimaryKeyFind.cs:29-45` | BLL/DAL | CONFIRMED |
| BR-M05-016 | Defaults: shipping condition "N/A", Active checked; inactive date input only enabled when inactive. | `CustMasterEntry.aspx.cs:27-30,45-48,361-374` | page | CONFIRMED |
| BR-M05-017 | Fixed-customer flag = (`fbDropDownList == "1"`); `ComUnitName` = part of the DC dropdown text before `:`. | `:229-234,228` | page | CONFIRMED |
| BR-M05-018 | Legacy insert stores hierarchy as codes/names (Market/MIA/Area/District/Region/ComUnit) and does not set `ActionStatus`, `CreateBy`, `CreateDate` or numeric market-structure ids, so rows created here lack the columns used by the newer approval/list pages (`sp_Get_CustMasterList_Approve` joins by `MarketId`, `CreateBy`). | `CustomerMasterInfoDal.cs:97-127` vs `LIVE_DB…sql:842-869` | DAL | CONFIRMED (columns written) / INFERRED (effect) |

## 20. Permission Matrix
No role/session check in this page (no `Session` access at all). Access = menu only. CONFIRMED.

## 21. Status Lifecycle
None. Only `IsActive` (+ `InActiveDate`) and `FixedCustomer` flags; no approval status is written.

## 22. Error Handling
`showMessageBox` alert on each failure; BLL rethrows exceptions (`throw ex`) — page has no try/catch ⇒ error page. `SInventorySql.Execute` failure surfaces as exception (`UpdateDataByUpdateCommandNew` has no catch) but BLL still reports success only if no exception.

## 23. Audit
None (no CreateBy/CreateDate/UpdateBy columns in the INSERT).

## 24. Integration
Customer code semantics imply SAP customer numbers (INFERRED); no SAP call.

## 25. Reports
Feeds `View_CustomerMaster` (view definition UNKNOWN), used by PAGE-195/180 reports.

## 26. Dependencies
`CustomerMasterInfoBll`, `CustomerMasterInfoDal`, `SInventorySql`, `ClsCommonInternalDAL`, `ClsPrimaryKeyFind`, AjaxControlToolkit calendar.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Fill all fields with new code | row inserted, success alert with code/name | 014,015 |
| TC-02 | Same code twice | second attempt " Customer Code already exist" | 015 |
| TC-03 | Untick Active, no inactive date | "Please select inactive date !!" | 014 |
| TC-04 | Leave Address 2 empty | "Please Input Customer Address Second  !!" | 014 |
| TC-05 | Concurrent inserts | possible duplicate `CustomerMasterId` | 015 |
| TC-06 | Inspect row after insert | ActionStatus/CreateBy null | 018 |

## 28. Reverse Engineering References
`Solution.Web/SInventory_UI/CustMasterEntry.aspx(.cs)`, `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs`, `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | — | MISSING |
| Code generation | none | manual entry; auto-generator commented out | INFO |
| Approval | none | none | INFO |

## 30. Known Gaps
Duplicate validation blocks; race-prone PK/uniqueness; no audit columns; legacy hierarchy differs from modern; insert result ignored; live DB view `View_CustomerMaster` unknown. See GAP-M05-019.

## 31. Change Impact
Same `tblCustMaster` is used by PAGE-168 and the SP-based pages. `CustomerMasterInfoBll` is shared by PAGE-185, 196, 182, 199 (PAGE-187 uses `CustomerMasterBLL`). Changing `SInventorySql`/`ClsCommonInternalDAL` affects all `SInventory_DAL` classes.
