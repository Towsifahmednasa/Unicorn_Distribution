# M04 reports

Three report families, all Crystal Reports opened in a pop-up from a launcher page. No export code is active (commented `ExportToHttpResponse` in the trade-price viewer); export is via the Crystal viewer toolbar.

## RPT-M04-01 Product cost price / trade price / price reports
| Launcher | Viewer | .rpt | Dataset table(s) | Data source |
|---|---|---|---|---|
| `SInventory_UI/ProductCostPriceReport.aspx` (PAGE-144, button opens viewer, no filters) | `SInventory_RPTVIEW/ProductCostPriceViewer.aspx` (PAGE-127) | `Library.CrystalReports/SInventory_RPT/rptProductCostPrice.rpt` (class `rptProductCostPrice`) | `unitPriceDetailDataTable`, `companyInfoDataTable` | `UnitPriceBLL.LoadProductUnitPrice` -> `ProductUnitPriceDAL.LoadUnitPriceView`: inline SELECT `(UnitPrice+VATAmountPerUnit) TPVat, *` from `tblUnitPrice` left-joined `tblProduct, tblProductSQ, tblPackSize, tblProType, tblProCategory, tblManufacturer, tblStockUOM, tblProductCase` where `tblUnitPrice.IsActive='1'`; company info `StockTransportOrderReportBLL.CompanyInfoBLL` |
| `SInventory_UI/ProductPriceReport.aspx` (PAGE-148; drop-down `reportType`: "PR" Product Information Report; "PPR" option commented out) | `SInventory_RPTVIEW/ProductTreadPriceViewer.aspx` (PAGE-128), QueryString `reportType` | PR: `Solution.Web/Reports/CrystalReports/rptProductTradePrice.rpt` (copy also in Library.CrystalReports); PPR: `rptProductPriceReport.rpt` | PR: `unitPriceDetailDataTable`; PPR: `ActiveUnitPriceDetailDataTable` (+ `companyInfoDataTable`) | PR: same `LoadUnitPriceView`; PPR: `GetProductPriceReportInfo` = `SELECT ProductCode,ProductName,PackSize,CostPrice,UnitPrice,VATPercentage,VATAmountPerUnit,IsActive,ActiveDate,InActiveDate FROM tblUnitPrice ORDER BY IsActive DESC` (all rows; PPR unreachable from the launcher) |
Notes: viewers use `Page_Init`; empty result shows "No Data Found!!"; no authorisation beyond menu; cost price shown to anyone with the URL.

## RPT-M04-02 Special discount report
Launcher `SInventory_UI/DiscountSpecialReport.aspx` (PAGE-130): From date required ("Select From Date"), To defaults to From. Opens `SInventory_RPTVIEW/DiscountReportViewer.aspx?fromDate=&toDate=` (PAGE-126) -> `ProductDiscountBLL.LoadProductDiscount(from,to)` = `SELECT (MIACode+'-'+MIAName) MIAName, * FROM tblProductDiscount LEFT JOIN tblCustMaster ... LEFT JOIN tblProduct ... WHERE ActiveDate BETWEEN @FromDate AND @ToDate` (parameterised, string dates) -> dataset table `spDiscDataTable` + `companyInfoDataTable` -> `rptSpecialDiscount.rpt` (`Library.CrystalReports/SInventory_RPT`). Filters only on ActiveDate, not on validity overlap. Source table cannot be maintained from the app (BR-M04-074).

## Screen lists (not Crystal)
Campaign list (PAGE-116), quoted price list (PAGE-119), unit price list (PAGE-158), product list (PAGE-152), group-wise promo qty list/view (PAGE-121/122; `sp_CS_MIOInfo_Rpt`, `sp_Get_GroupWisePromoQtyList`).
