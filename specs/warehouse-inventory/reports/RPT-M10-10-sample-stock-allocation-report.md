# RPT-M10-10 Sample stock allocation report

**Page:** `Reports_UI/SampleStockAllocationRpt.aspx` (PAGE-342) - not a stock-table report: it lists sample stock allocated to field officers (MIO) by year/month.
**Data:** `CmnCrystaltoView.GetSampleStockRptList(EmpId=ddlMIO, From=ddlYear, To=ddlmonth)` (`Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1751-1761`) calls SP `sp_Get_SampleStockReport(@EmpId,@Month,@Year)` (note the argument names are swapped: `@Month = To`, `@Year = From` - call site passes year in `From`, so mapping is correct only if the caller's variables are read as such). MIO list via `sp_CS_MIOInfo_Rpt`. Bodies absent.
**Output:** grid (Excel/print helpers in page); no `.rpt` referenced.
**Relation to M10:** none of the M10 sample-conversion pages (PAGE-379/376) feed this SP as far as code shows (UNKNOWN).
