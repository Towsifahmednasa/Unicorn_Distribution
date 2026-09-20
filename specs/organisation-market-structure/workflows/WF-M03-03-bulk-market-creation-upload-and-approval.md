# WF-M03-03 — Bulk market creation / update by Excel upload and approval

> Traced from `Solution.Web/TransferUI/MarketInfoImport.aspx(.cs)` (PAGE-098), `MarketInfoApprove.aspx(.cs)` (PAGE-097), `Library.DAL/MarketUpload_DAL/MarketUploadDAL.cs`, `Library.DAL/MasterSetup_DAL/OrderTrackingDAL.cs` (`GetMarketInfoApprovalList` :408, `SaveMarket_ApplogDAL` :723). Bodies of `sp_Save_MarketPropMaster`, `sp_Save_MarketPropDetail`, `sp_Save_MarketPropToTable`, `sp_GetMarketInfoApprovalList` are **not in the repo**; only the staging shape and hand-offs are known.

## Trigger & actors
- Trigger: many markets (and territory assignments) must be created/updated at once (e.g. new field-force launch).
- Actors: uploader (any menu-authorised user), approver (any user with `MarketInfoApprove.aspx`; no segregation in code).

## Preconditions
Excel file based on template `Solution.Web/Approval_UI/MarketTranferFormat.xls` (columns `territorycode, marketcode, MarketName, Division, District, Thana, RegionalHeadStationType, DZSMStationType, AMStationType, MIOStationType`); server has Jet 4.0 (.xls) / ACE 12.0 (.xlsx) OLEDB providers and write access to `Solution.Web/ExcelFiles/`.

## Steps
1. **Upload** (`btnUpload_OnClick`) → file saved to `~/ExcelFiles/<name>` **before** extension check → OLEDB read of the first sheet → rows with empty first column dropped → preview grid + "File Name:… [ n records Found!]" (BR-M03-097/098). Any error → "Excel file is not a correct format !". [CONFIRMED]
2. **Choose mode** `rbType`: `MarketInsert` (default) / `MarketUpdate` / `TerritoryUpdate` (BR-M03-096).
3. **Submit** (`submitButton_Click`): `Validation()` warns on an empty grid but never blocks (BR-M03-099) → rows read back from grid labels → `SaveMarketInfoForExcel`: `sp_Save_MarketPropMaster(@TypeId="0",@EntryBy,@EntryDate,@ConvertType,@MarketPropMasterId="0")` → pk; then one `sp_Save_MarketPropDetail` per row (`@MarketPropMasterId=pk` + the ten Excel fields); one implicit transaction; rollback + rethrow on error (BR-M03-101). Success → alert, reload; failure → "Already Exist!". [CONFIRMED]
4. **Review** on `MarketInfoApprove.aspx`: page load → `OrderTrackingDAL.GetMarketInfoApprovalList("")` → `sp_GetMarketInfoApprovalList(@Parm="")` → grid (`hfCustPropMasterId` per row). [CONFIRMED]
5. **Approve** (row command `ApproveData`) → `OrderTrackingDAL.SaveMarket_ApplogDAL(MarketPropMasterId)` → `SaveData("sp_Save_MarketPropToTable",{@MarketPropMasterId,@EntryBy=Session UserId,@EntryDate})` → success alert "Operation successful!" and list reload, failure → "Already Exist!". This SP is the (UNKNOWN) step that turns staged rows into live Market/Territory data according to `ConvertType`. [CONFIRMED call / UNKNOWN effect]
6. A hidden "Transfer" button on the import page calls the same SP but is unreachable (BR-M03-100).

## Data created / changed
| Data | Change |
|---|---|
| Staging master + detail (tables UNKNOWN; names `MarketProp*` INFERRED) | created at step 3 |
| `Solution.Web/ExcelFiles/<file>` | file written at step 1 (kept) |
| Live Market/Territory/station-type tables | changed by `sp_Save_MarketPropToTable` at step 5 (UNKNOWN) |

## Status transitions
`Staged` → `Applied` (approve). No reject/delete of a staged batch in C#; `Approve` list filter/status UNKNOWN.

## Branches / exceptions
Bad/missing headers → binding exception (reported as format error). Duplicate market/unknown territory/division/district/thana text → not validated in C#; SP behaviour UNKNOWN. Same uploader and approver allowed. No approval-side validation of content in C#.

## Postconditions
Staged batch visible on approval page; after approval, live data updated (UNKNOWN).

## Related BR / pages
BR-M03-095…100, 101. PAGE-098, PAGE-097 (Tier B), MarketSetup PAGE-042 for the interactive equivalent (with rules BR-M03-032…040 that this path may bypass).

## Tests
| ID | Scenario | Expected |
|---|---|---|
| WF3-T1 | Valid template, 50 rows, Market Insert | 1 master + 50 detail rows staged; approve → markets exist |
| WF3-T2 | Non-Excel file upload | saved to ExcelFiles then rejected |
| WF3-T3 | Submit without file | blank row staged |
| WF3-T4 | Missing `Thana` header | format error |
| WF3-T5 | Approve twice | second approval outcome UNKNOWN (check SP idempotence) |
| WF3-T6 | Uploader approves own batch | allowed |
