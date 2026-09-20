# WF-M13-06 Dynamic sales report grid and drill-down modals

_Module M13, reverse-engineered 2026-09-19 (documentation only). Confidence tags as in the page specs._

## Trigger and actors
Sales manager opens `DynamicSalesReport.aspx`, sets dates and scope, presses Show.

## Steps
1. `btnShowGrid_Click`: toggle visible columns from `cblHeader`, call `CmnCrystaltoView.GetDynamicSalesReportListDAL` -> SP `GetDynamicSalesReportList` (BR-M13-041); bind `gvSelectedItems`.
2. Click a number -> handler reads `hfTerritoryId` of the row into `_hfTerritoryId`, shows the ModalPopupExtender and runs `Load*()` (BR-M13-042): invoice lists -> `sp_Get_ProformaInvoiceReportList`; collections -> `sp_Get_AllSalesReportListParam2` / `sp_Get_NewReceiveableList`; doctors -> `sp_Get_DoctorList`; DCR -> `sp_Get_DCRInfoListForView`; Rx -> `sp_Get_PrescriptionList`.
3. Each `*Parm(territory)` builds an `AND ...` fragment from the current selectors and the clicked territory.
4. Modal Export buttons write Excel (ClosedXML/EPPlus).

## Branches / defects
HQ/Ex-HQ/OS lists identical; Campaign links empty (BR-M13-043); From-date floor (BR-M13-044).

## Related
PAGE-505; RPT-M13-07; GAP-M13-006, -008, -023.

