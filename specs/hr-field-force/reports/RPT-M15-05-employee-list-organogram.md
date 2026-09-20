# RPT-M15-05 Employee list and organogram

- **Employee list (PAGE-612 `MasterSetup_UI/EmployeeRecords.aspx`)**: jQuery `POST EmployeeRecords.aspx/GetEmployeeInformationList(param)` -> `EmployeeInformationDaL.GetEmployeeInformationList` -> `sp_Get_EmployeeInformationList_Prm(@Parm)`; filter built in JS (e.g. ` AND PM.EmployeeStatus='<status>'`) and sent as raw SQL text (GAP-M15-012); columns include AreaName, TerritoryName, RoleType, EmployeeStatus; DataTables grid with copy/excel/pdf/print buttons (client-side export). Code-level permission check on load (BR-M15-080). **The web-method has no auth check.**
- **Organogram (PAGE-625 `Reports_UI/OrganogramReport.aspx`)**: `OrderTrackingDAL.GetOrganogramreportList(@Parm)` -> `sp_GetOrganogramreportList`; filters via ` AND ff.MarketId='<id>'` style fragments (alias `ff` = field-force info); grid only; row edit/redirect links target unrelated `ProductEntry/ProductEdit.aspx` (copy-paste, dead).
- **Legacy lists:** `EmpGenInfView.aspx` (`select * from tblEmpGeneralInfo`), `MiaInfoView.aspx` (`SELECT * FROM tblMIAInfo`) - inline SQL in `EmpGeneralInfoDAL`/`MiaInformationDAL`.
- No `.rpt` files.
