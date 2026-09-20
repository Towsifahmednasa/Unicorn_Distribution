# WF-M05-01 — Customer master: create, edit, find

**Scope:** the current (SP-based) customer maintenance process on `MasterSetup_UI`. Evidence: `CustomerEntry.aspx.cs`, `CustomerInfoDAL.cs`, `DataAccessManager.cs`, `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql` (bodies of `sp_Save_CustomerMaster`, `sp_Update_CustomerMaster`, `sp_Get_CustMasterList_Approve`). Confidence tagged per step.

## Trigger & actors
- Trigger: a sales-admin/master-data user needs a new outlet, or must correct/re-tag an existing one.
- Actors: any authenticated user who has the page in their menu (no role check in code — PAGE-168 §20). The search list additionally honours `RAdd/REdit` permissions (PAGE-173).

## Preconditions
- Market structure (Group → Region → Area → Territory → SubTerritory → Market), Thana/District/Division, customer types, provider types, station types, DC/routes, product lines and salesperson master data exist (maintained in other modules / WF-M05-07).
- For edit: an existing `tblCustMaster.CustomerMasterId`.

## Steps
1. **Find (edit case)** — `CustomerView.aspx` (PAGE-173) `btnSearch_Click` → `Parm()` builds filter text → `CustomerInfoDAL.GetCustomerList` → `sp_Get_CustMasterList_Approve(@Parm)` → grid. The edit link opens `CustomerEntry.aspx?mid=<id>` in a new tab. (CONFIRMED)
2. **Open form** — `CustomerEntry.Page_Load` → `LoadInitialInfo` binds all dropdowns; with `MID` → `GetOneRecord` → `CustomerInfoDAL.GetCustomerSetupById` → `sp_GET_CustMaster_ById(@id)` (body UNKNOWN; returns one row per product line plus image-path columns). The sales-person list is pre-selected from `TaggedSalesPersonIds` (else `SalesPersonId`). (CONFIRMED)
3. **Fill** — the user picks the market through the `IVMarketStructure` cascade and division/district/thana; multi-selects salespersons and product lines. (CONFIRMED)
4. **Validate** — `Validation()` (BR-M05-001/002): name, type, mobile (11 chars), address are required; nothing else. Failure ⇒ inline styling/popup and stop.
5. **Save** — `btnSave_Click` builds `CustMasterInfoDAO`: tagged ids = comma list of selected `EmpInfoId`, codes = text before `:`, primary `EmpInfoId` = first selected (BR-M05-007) → `CustomerInfoDAL.SaveInfo` (single DB transaction, BR-M05-008):
   - **Create** (`CustomerMasterId=0`): `sp_Save_CustomerMaster` with `@EntryBy=UserId`. Inside: derive geography ids from the market; station types from `tblMarketStationDetail` (roles 1/2/3); DC name/code from `tblCompanyUnit`; **code = `'C'` + (MAX numeric suffix among ActionStatus='2' rows + 1)** (BR-M05-004); INSERT with **`ActionStatus='2'`** (BR-M05-005) and `CreateBy/CreateDate`; return `SCOPE_IDENTITY()`.
   - **Update**: `sp_Update_CustomerMaster` with `@UpdateBy`: rewrite master fields (not code, not ActionStatus, not DC — BR-M05-009); delete the customer's `tblCustProductLine` rows.
   - Then for each selected product line: `sp_Save_CustProductLineDetail(@CustomerMasterId,@ProductLineID)` (body UNKNOWN).
6. **Outcome** — success ⇒ `successalert` and redirect to `CustomerView.aspx`; any non-success return ⇒ "Already Exist!" (no duplicate detection exists — BR-M05-010); exception ⇒ rollback + unhandled error page.

## Data created / changed
| Table | Change |
|---|---|
| `tblCustMaster` | INSERT (create) or UPDATE (edit); audit CreateBy/Date, UpdateBy/Date |
| `tblCustProductLine` | delete-all + insert selected (edit) / insert (create) |
| Status | new customer is immediately `ActionStatus='2'` (Approved); `IsActive` from checkbox |

## Branches / exceptions
- Validation failure: no DB call.
- `MID` for a missing record: `GetOneRecord` swallows errors, form appears blank; Save then runs an update against a non-existent id (0 rows changed, still reports success) — INFERRED.
- Concurrency: two simultaneous creates may compute the same code (no lock) — INFERRED.
- Market omitted: geography/station types stay as posted/null.
- SP failure ⇒ whole transaction rolled back.

## Postconditions
Customer visible in `CustomerView`, autocomplete (`GetCustomernew`, `GetCustomer_ALL_new`), orders and the mobile app; `SalesPersonId` and `TaggedSalesPersonIds` control who is matched by salesperson filters (BR-M05-049).

## Related BRs
BR-M05-001…013, 045…049.

## Related pages
PAGE-168 CustomerEntry, PAGE-173 CustomerView, PAGE-169 CustomerListPending, PAGE-167 CustomerChangeProgramType, `Approval_UI/CustomerApproveList.aspx` (PAGE-695).

## Tests
See PAGE-168 §27 (TC-01…TC-12) and PAGE-173 §27. End-to-end: create a customer with market M and two salespersons → search by salesperson → edit the address → verify code and status unchanged.
