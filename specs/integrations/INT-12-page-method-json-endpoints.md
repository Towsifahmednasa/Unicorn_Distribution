# INT-12 - Page-method JSON endpoints (`<page>.aspx/<Method>`)

| Item | Detail |
|---|---|
| Direction / protocol | Browser (jQuery `$.ajax`, `contentType: application/json`) -> static `[WebMethod]` on an `.aspx` page; response `{"d": ...}`. Usable by any HTTP client. |
| Endpoint pages in this module | `DoctorModule_UI/CommonDataLoad.aspx` (55 methods), `Target_UI/CommonDataLoad.aspx` (subset duplicate, no callers), `DoctorModule_UI/SeedData.aspx` (13), `DoctorModule_UI/Setup.aspx` (43), `eProgram_UI/ProviderDropoutRequestList.aspx` (2, INT-06), `Reports_UI/DWSPMonthlyRpt.aspx` (`GetMileageClaimList`), `Target_UI/*` pages (own save/load methods). |
| Payload | DataTable -> `JsonConvert.SerializeObject` string (CommonDataLoad, SeedData, most Setup); DTO classes for saves (`Zone`, `Area`, `Territory`, `SubTerritoryDAO`, `TADAMarketruleConfig`); `ResultInfo` for actions. |
| Authentication | None coded. Writes read `Session["UserId"]` and fail without a session; lookups, `GetSubTerritoryList`, `Delete_Prescription`, `Delete_Trainning` and `GetProviderDropoutIntrigrationList` do not (BR-M17-019/020/025). |
| Dead references | ~17 client calls to methods that are commented out/missing in `Setup.aspx.cs`; `Approve_TourPlanList` is non-static (see PAGE-679 section 11). |
| Error handling | None; HTTP 500. |
