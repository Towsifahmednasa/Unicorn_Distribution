# RPT-M08-05 — Sales Rejection report and Replace Note report

| Item | Sales Rejection | Replace Note |
|---|---|---|
| List pages | `SalesRejectionReport.aspx` (PAGE-293, grid via `sp_Get_SalesRejectionReportListLAtest`), `SalesRejecionReport.aspx` (PAGE-292, launcher) | `ReplaceNoteReport.aspx` (PAGE-289; `CmnCrystaltoView.cs:424` → SP `sp_Get_ReplaceNoteReport`) |
| Viewer | `SInventory_RPTVIEW/SalesRejectionReportViewer.aspx` (PAGE-281) | no separate viewer confirmed (UNKNOWN) |
| .rpt | `rptSalesRejection.rpt` (Excel session flag) or `crpSalesRejection.rpt` | UNKNOWN |
| Parameters | QueryString `fromDate/toDate/districtId/NationalReport`; **`Session["param"]` (pre-built SQL fragment) and `Session["Excel"]`** | UNKNOWN |
| Data source | `SalesRejecionReportDAl(param)` / `CmnCrystaltoView.GetSalesRejectionReportDAL(Parm, Parm2)` (`:218`) → SPs not in repo | SP not in repo |
| Notes | The viewer trusts `Session["param"]` (concatenated SQL). These pages are Tier B; only structure was reviewed. | |
