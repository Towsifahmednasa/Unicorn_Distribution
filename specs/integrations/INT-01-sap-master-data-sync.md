# INT-01 - SAP master-data sync (Employee and Product) - inbound, operator-approved

| Item | Detail |
|---|---|
| Direction | SAP -> ERP (staging -> approved upsert) |
| Transport | **Database hand-off**: `sp_Get_SAP_EmpInfo`, `sp_Get_SAP_ProductInfo` read SAP-staged rows (staging store not in repo; the stock SPs address a separate DB `SAP_API_Data`, INFERRED same feed). How SAP writes to staging (REST adapter, job) is UNKNOWN. |
| Trigger | Manual: user opens `SAP_Integration/SAP_IntrigationPoint.aspx` (PAGE-680) / DIC copy (PAGE-681), picks *Employee Info* or *Product Info*, presses Approve. |
| Payload / fields | Employee: `employee_id, employee_code, name, RoleType, joining_date, mobile_no, FTerritoryName/TTerritoryName, FAreaName/TAreaName, FZoneName/TZoneName, action`. Product: `product_id, product_code, product_name, description, effective_date, status, action`. |
| SPs / tables | Read: `sp_Get_SAP_EmpInfo(@Parm,@Parm2)`, `sp_Get_SAP_ProductInfo(@Parm,@Parm2)`, `sp_Get_SAP_IntrigationPointHeader`. Check: `sp_Chk_SAP_EmpInfoCondition(@femployee_id,@employee_code,@RoleType,@ActionStatus)` returns `Datacheck`, `Msg`. Write: `sp_Upsertdate_EmpInfo(@employee_id,@employee_code,@RoleType,@ActionStatus,@UpdateBy)`, `sp_Upsertdate_ProductInfo(@product_id,@product_code,@status,@UpdateBy)`. Bodies UNKNOWN. Code: `Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:422-610`. |
| Status flags / retry | Staged `action` value (statuses UNKNOWN). No retry: an unapproved row simply stays in the list. Employee approve blocked when check SP returns `Datacheck>0`. |
| Authentication | ERP side: menu access + master-page login only. SAP->staging auth UNKNOWN. |
| Error handling | DAL rollback + rethrow (unhandled yellow page); `Already Exist!` popup unreachable (`SaveData` returns true or throws). Product approve has no pre-check. |
| Risks | No role check on approve; SP rules invisible; static shared DAL instance. Pages: PAGE-680/681. BR-M17-001/006/007. |
