# API Documentation

The system is **ASP.NET WebForms; there is no REST API layer**. Programmatic endpoints are `[WebMethod]` page methods (474 found), one ASMX service (`SInventoryWebService`), HTTP handlers (`PictureHandler.ashx`, `SignatureHandler.ashx`, `HandlerDocCV.ashx`) and one page route alias (`user-records`). The complete list with endpoint, class, action, parameters, BLL/DAL, SPs and tables is in [reverse-engineering/api-inventory.md](reverse-engineering/api-inventory.md) (API-001…).

## Conventions (observed)
- Transport: ASP.NET AJAX PageMethods (`Page.aspx/Method`, JSON POST) and ASMX script service; `jsonSerialization maxJsonLength=50000000`.
- Authentication: Forms mode configured but not used; endpoints rely on the session cookie only, **and most do not read Session at all**; `Web.config` has no `<authorization>` (docs/security.md; M13/M14/M15/M17 GAPS).
- Errors: unhandled exceptions surface as ASP.NET AJAX 500 responses; no envelope convention; many methods swallow exceptions.
- Validation: per method, mostly absent; SQL often concatenated (see GAPS).

## Where per-page detail is
Page specs §13 (API Mapping): e.g. `PAGE-464` (AdminDashboard, 28 web methods, also `specs/sales-reports-dashboard/workflows/WF-M13-04`), `DoctorModule_UI/FieldForce`, `CommonDataLoad`, `Setup` (M15/M17/M14), integration endpoints in `specs/integrations/INT-11`, `INT-12`.
