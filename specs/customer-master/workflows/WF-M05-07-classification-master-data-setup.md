# WF-M05-07 — Classification master data: customer type, provider (program) type, pharma platform (SMC type), station type

**Scope:** the small setup pages whose data feeds the customer/doctor forms. Evidence: `CustomerTypeSetup.aspx.cs` (WebMethod host) + `CustomerType.aspx` / `CustomerTypeView.aspx` (client pages), `CustomerTypeDal.cs`, `ProgramTypeView.aspx.cs` (WebMethod host for program type and SMC type) + `ProgramType.aspx`/`SMCType.aspx`/`SMCTypeView.aspx`, `ProgramDal.cs`, `StationTypeEntry.aspx.cs`/`StationDal.cs`. All SP bodies (`sp_Save_*Info`, `sp_UD_*Info`, `sp_check_*`, `sp_GET_*`) are **not** in the repo; only the C# control flow is documented.

## Trigger & actors
Master-data admin creates/edits a classification value so it appears in customer/doctor dropdowns. Any user with the page in the menu (no role check).

## Steps (customer type as reference; program/SMC/station types follow the same pattern)
1. `CustomerType.aspx` (client form): loads customer categories from `../DoctorModule_UI/Setup.aspx/GetCustomerCategory`; `Save()` → `Validation()` (name required; the category check `=="" && ==null && =="0"` can never be true, so category is effectively optional client-side) → `$.ajax` POST `CustomerTypeSetup.aspx/Save_DepartmentInfo` with `{CustomerTypeId, CustomerTypee, CustomerCategoryId, IsActive, IsCampaign, IsDefault}`.
2. `Save_DepartmentInfo` → `CustomerTypeDal.Save_DepartmentInfo(dto, Session["UserId"])` (`CustomerTypeDal.cs:17-102`):
   - **New** (`CustomerTypeId=0`): `sp_Save_CustomerTypeInfo` (adds `@EntryBy`); id ≤ 0 ⇒ `isDuplicateCheck=true` ("Already Exist!"), else success.
   - **Edit**: `sp_check_CustomerType(@id,@CustomerType)` (duplicate name excluding self, per name); if a row is returned ⇒ `isDuplicateCheck=true`; else `sp_checkIsDefault_CustomerType(@id)` is executed but its result is never used (the "Is Default Already Exist!" branch is unreachable, `:47-64`), then `sp_UD_CustomerTypeInfo` (adds `@UpdateBy`).
3. Client shows: success ⇒ `successalert(...,'CustomerTypeView.aspx')`; duplicate ⇒ "Already Exist!"; validation flag ⇒ "Is Default Already Exist!"; else "Operation Faild!".
4. `CustomerTypeView.aspx`: `GetDepartmentList` (→ `sp_GET_CustomerInfo(@Parameter)`) renders Category / Type Code / Customer Type / flags; edit link → `CustomerType.aspx?id=` → `GetDepartmentEditData` (`sp_GET_CustomerType_ById`). Activate/Inactivate and Delete buttons post to `/CustomerType/ActiveInactive_departmentInfo` and `/Department/Delete_EmployeeDepartment` — MVC-style URLs that do not exist in this WebForms app (no controller/route), so these actions appear non-functional (INFERRED). The WebMethods `ActiveInactive_departmentInfo` (`sp_ActiveInactive_customerType_ById`) and `Delete_EmployeeDepartment` (calls `sp_Delete_DepartmnetInfo(@DeptId)` — a *department* delete copied into the customer-type DAL) exist on `CustomerTypeSetup.aspx` and are callable directly.
5. **Program type / SMC type** (`ProgramTypeView.aspx` hosts `Save_DepartmentInfo`, `Save_SMCTypeInfo`, `GetDepartmentList`, `GetSMCTypeList`, `GetDepartmentEditData`, `GetSMCTypeEditData`; `EnableSession=true`): `ProgramDal.Save_DepartmentInfo`: fields `ProgramTypeName, IsActive, IsCustomer, IsDoctor, IsDefault`; edit ⇒ `sp_check_Programtype` (duplicate ⇒ returns `isSuccess=false`, shown as failure) then `sp_UD_ProgramTypeInfo`; new ⇒ `sp_Save_ProgramTypeInfo`. SMC type analogous (`sp_check_SMCtype`, `sp_UD_SMCTypeInfo`, `sp_Save_SMCTypeInfo`) with flags such as `forCustomer` (INFERRED from the `GRP.forCustomer=1` filter used by customer pages).
6. **Station type** (`StationTypeEntry.aspx`/`StationTypeView.aspx`, `StationDal`): `StationTypeName, StartTime, EndTime, IsActive`; edit ⇒ `sp_check_StationType`, `sp_UD_StationTypeInfo`; new ⇒ `sp_Save_StationTypeInfo`; list `sp_GET_StationTypeInfo`.

## Data created / changed
`tblCustomerType` (columns incl. CustomerType, CustTypeCode, CustomerCategoryId, IsActive, IsCampaign, IsDefault, Entry/Update/Approve audit — from the DAO), `tblProgramType` (IsCustomer/IsDoctor/IsDefault), `tblSMCType`, `tblStationType` — plus audit columns. Exact DDL UNKNOWN.

## Branches / exceptions
- Duplicate name ⇒ "Already Exist!"; default-flag conflict message unreachable (BR-M05-053).
- Errors in DAL rethrow after rollback; client shows "Operation Faild!".
- Classification values referenced by customers can be inactivated/deleted through direct WebMethod calls (no referential guard visible).

## Postconditions
New/updated classification appears in dropdowns loaded by `SeedDataDAL` (`sp_GET_CustomerType`, `sp_GET_ProgramTypeListParm`, `sp_GET_SMCTypeListParm`, `sp_GET_StationTypeList`) used by PAGE-168/167/173.

## Related BRs
BR-M05-012, 044, 053, 054.

## Related pages
PAGE-170/171/172 (customer type), PAGE-174/175 (program type), PAGE-176/177 (SMC type), PAGE-178/179 (station type).

## Tests
1. New customer type "X" ⇒ appears in CustomerEntry dropdown. 2. Duplicate "X" on edit of another id ⇒ "Already Exist!". 3. Set IsDefault on two types ⇒ no error (default check dead). 4. Call `Delete_EmployeeDepartment` with an id directly ⇒ verify which table is affected (security test).
