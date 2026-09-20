# WF-M17-02 - SAP employee / product master approval

Trigger/actors: SAP stages employee or product change; head-office user approves on `SAP_IntrigationPoint.aspx`. BR-M17-006, 007. 

Steps
1. Choose *Employee Info* -> `sp_Get_SAP_EmpInfo`; or *Product Info* -> `sp_Get_SAP_ProductInfo` (`LoadInfoNewMethod`).
2. Employee Approve -> `AllConditionCheckForEmp` -> `sp_Chk_SAP_EmpInfoCondition`; `Datacheck>0` -> popup with `Msg`, stop.
3. `sp_Upsertdate_EmpInfo(...@UpdateBy=UserId)` (or `sp_Upsertdate_ProductInfo` without check) -> success popup, reload.

Data: ERP employee/territory and product master (tables UNKNOWN). Exceptions: DAL rollback + rethrow. Postcondition: staged row leaves list (INFERRED). Tests: TC-680-02..05. Pages: 680, 681.
