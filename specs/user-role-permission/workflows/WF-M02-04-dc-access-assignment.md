# WF-M02-04 — DC (distribution-centre) access assignment

**Two paths write `tblUserCompanyUnit`:**
- **A. DCUserPermission (PAGE-020):** pick user → tick DCs / warehouse (id 15) → Save: `DELETE FROM tblUserCompanyUnit WHERE UserId` then `INSERT (UserId,CompanyUnitId,CWHPermission,NationalReportPermission)` per DC; `CWHPermission` for id 15; `NationalReportPermission` on all rows iff exactly 11 non-warehouse DCs are ticked (`DCUserPermission.aspx.cs:82-142`).
- **B. UserSetup (PAGE-015):** for roles 29/14/25/18/20/"DA ASSISTANCE" tick DCs → `sp_Save_UserCompanyUnit(@UserId,@ComUnitId)` each (`UserInfoDAL.cs:453-470`).

**Consumption:** at login `PanalClsDAL.Login` returns the user's company units: `Session["ComUnitId"]` (arbitrary row, GAP-M01-007), `DICCompanyUnitId` (lowest UserComId), `DICUnitId` (CSV) (M01 BR-M01-003/004); downstream DC-scoped pages use them (other modules). `CWHPermission`/`NationalReportPermission` have no C# reader.
**Exceptions:** non-atomic replace; SP B behaviour on edit UNKNOWN; alert text bug. **BR:** 009, 036-041. **Gaps:** 004, 006, 016, 017.
