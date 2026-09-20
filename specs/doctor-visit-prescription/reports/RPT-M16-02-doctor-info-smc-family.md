# RPT-M16-02 — Doctor Info Report ("SMC Family Doctor Report")

## Viewer / list page
`Solution.Web/Reports_UI/DoctorInfoReport.aspx` (+ `.aspx.cs`) — PAGE-674 (Tier A spec: `pages/PAGE-674-doctorinforeport.md`). Uses `Reports_UI/IVMasterStructureForDoctorReport.ascx` as hierarchy control (registered as `IVMarketStructure`).

## Crystal `.rpt` / dataset
None. Output is six ASP.NET GridViews rendered as HTML and an Excel-compatible HTML export.

## Data source
`Library.DAL/DoctorInfo_DAL/DoctorInfoReportDal.cs` → SPs (all bodies absent):
| View (radio) | SP | Params |
|---|---|---|
| Summary / Zone Wise (`gv_Zone`) | `sp_Rpt_ZonewiseDoctorWeek` | `@frmDate,@toDate,@Parameter(""),@Zone` |
| Summary / Area Wise (`gv_Area`) | `sp_Rpt_AreawiseDoctorWeek` | + `@Area` |
| Summary / MIO Wise (`GridView1`) | `sp_Rpt_MIOwiseDoctorWeek` | + `@Teritory` |
| Summary / Doctor Wise (`GridView2`) | `sp_Rpt_DoctorwiseDoctorWeek` | `@Zone,@Area,@Teritory` |
| Details (`GridView3`) | `sp_Rpt_SMCFamilyDoctorReport_ProcessData` | `@Zone,@Area,@Teritory` |
| Provider view (option commented out; `loadGridView`) | `sp_RPT_DoctorInfoReport` | `@frmDate,@toDate,@Parameter` |
| Banner "Last Process Date" | `sp_GET_SMCFamilyDoctorLastProcessDate(@id)` | – |
Empty Zone/Area/Territory are sent as `DBNull`. Dates are passed as the raw text ("dd MMMM, yyyy").

## Filters on the list page
Report type (Summary / Details), Summary view (Zone / Area / MIO / Doctor), From/To date (default current month), Zone/Area/Territory (shown per view), "Is only download" (Details).

## Output columns (from markup)
- Zone/Area/MIO summary: Zone, Area, Territory (`MIOCode`), MIO, **Assign** (`AssignDocCount`), **Coverage** (`DCRDocCount`), **Total DCP** (`DCPTotalCount`), **Repeat** (`RepatCount`), **No. of Prescriber** (`NORXCount`), **Prescription** (`RXCount`), then per week 1-4: Coverage (`nWeekV`), DCP (`nWeekDCP`), Repeat (`nWeekR`), No. of Prescriber (`nWeekNP`), Prescription (`nWeekP`); footer = column sums computed in code-behind.
- Doctor wise: + Doctor, Provider (`ProgramTypeName`), Doctor Type, Degree, Speciality, DCP/DCR/RX totals and weekly DCP/V/P.
- Details: doctor attributes + `1_DCP,1_DCR,1_RX … 31_DCP,31_DCR,31_RX` + total (`to_*`), footer sums.
Business meaning of each metric is defined inside the SP (UNKNOWN).

## Request parameters
Form fields only (no QueryString). Session not read (no role logic).

## Export
`btnExportToExcel_Click`: `.xls` (HTML table with title "… SMC Family Doctor Report (Date Range : from- to) Print Date: …") named `<type> <dd_MMM_yyyy_hh_mm_tt>.xls`; Details export requires a Zone ("Please Select Zone!!"); errors ⇒ "No Data Found!!". Legacy client `exporttocsv()` unused.

## Business rules / gaps
BR-M16-049..054; GAP-M16-043..048.

## Tests
See PAGE-674 §27. Verify footer sums equal column sums of the exported file; verify Details day columns correspond to the chosen date range; verify "Last Process Date" changes when the out-of-repo processing job runs.
