# WF-M02-07 — Order placement delegation per territory

**Trigger:** admin opens `Reports_UI/OrderPermission.aspx` (PAGE-017).
1. Zone (`sp_Get_ZoneListOrdPer`) → Area (`sp_Get_AreaListOrdPer`); Search → `sp_GET_EmployeeInfoOrdPermission(@Parm=" AND ar.RegionId='..' AND ar.AreaId='..'")`.
2. Per row candidates from `sp_Get_AMDZSMListByTerritoryId`; existing permission preselected.
3. Tick rows, choose permitted employee, From/To datetime-local; Submit → `Validation2` (≥1 ticked; other checks only colour rows).
4. For each ticked row `Setup2DAL.Save_OrderPermission` → `sp_Save_OrderPermission(@TerritoryId,@PermittedEmpId,@FrmDate,@ToDate,@EntryBy=LoginName)`; the success message comes from the last row; grid reloaded.

**Enforcement:** no reader of the saved data in this repository (UNKNOWN). **BR:** 017-023. **Gaps:** 018, 019, 020, 021.
