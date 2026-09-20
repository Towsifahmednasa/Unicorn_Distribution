# RPT-M06-03 — Order tracking / monitor list pages (grid + Excel export; no .rpt)

Scope: PAGE-208 `OrderTrackingList`, PAGE-209 `OrderTrackingListDBH`, PAGE-210 `OrderTrackingSummary` (all `Solution.Web/MasterSetup_UI/`), PAGE-227 `OrderMonitor` (`SInventory_UI/`), PAGE-230 `OrderStatus`, PAGE-233 `Testing_OrderStatus`. **These are Tier-B pages; only the shared DAL entry points were verified in this pass** (PARTIAL).

| Page | Data entry point (verified unless noted) | Output |
|---|---|---|
| OrderTrackingList | `OrderTrackingDAL.GetOrderTrackingList(parm)` → `sp_Get_OrderTrackingList_Latest(@Parm)` (`OrderTrackingList.aspx.cs:204,334`); `Parm()` builds a **string-concatenated SQL fragment** from DC (`mas.ComUnitId='…'`), customer type (`mas.CustTypeId='…'`), and other filters (many commented out) (`:417-492`) | GridView with paging + Excel (HTML-in-.xls) export |
| OrderTrackingListDBH | `OrderTrackingDAL.GetOrderTrackingDBH(parm)` → `sp_Get_OrderTrackingList_DBH(@Parm)` (`:204`) | same |
| OrderTrackingSummary | `OrderTrackingDAL.GetDatefromMonthYearStuff` → `DynamicDatebyMonthByDateRange` and other SPs (Tier-B map) | UNKNOWN |
| OrderMonitor | `TotalSummaryDAL.OrderMonitoring_Bizmo` → `sp_OrderMonitoringPanel_Bizmotion` (Tier-B map; "Bizmotion" suggests a third-party field-force integration — INFERRED) | UNKNOWN |
| OrderStatus | `?MID=<orderId>` order-information page opened from the tracking list (`OrderDCChange.aspx.cs:322`) | detail view (Tier B) |

All SP bodies are absent (`sp_Get_OrderTrackingList_Latest`, `_DBH`, `sp_OrderMonitoringPanel_Bizmotion`, `DynamicDatebyMonthByDateRange`). Gap: GAP-M06-027.
