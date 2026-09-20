# Security review (reverse-engineered, documentation only)

Scope: how security controls are actually implemented in this repository today. Nothing here has been changed, fixed or exploited. Secret values are never reproduced (`<redacted>`). Tags: `CONFIRMED` (read in code), `INFERRED`, `UNKNOWN`. Severity ratings in the risk register are `INFERRED` (assessment, not tested). Stored-procedure bodies are absent except ~41 in root scripts, so DB-side controls are `UNKNOWN`.

Related: [architecture.md](architecture.md), [deployment.md](deployment.md), [testing.md](testing.md).

## 1. Authentication

| Item | Finding | Evidence |
|---|---|---|
| Declared mode | `<authentication mode="Forms"><forms loginUrl="~/Login.aspx" timeout="2880" />` (48 h) | `Solution.Web/Web.config` |
| Forms ticket | **Never issued.** No `FormsAuthentication.SetAuthCookie/RedirectFromLoginPage`, no `User.Identity`, no `IsAuthenticated` anywhere in the repo (grep: 0 hits). The Forms module therefore never sees an authenticated user; and because there is no `<authorization>` or `<location>` element, it also never denies anonymous access | grep CONFIRMED |
| Login state | `Session["UserId"]` set by `Login.aspx.cs:34` (InProc session cookie) | CONFIRMED |
| Login flow | `Login.aspx.cs:18-139`: trims username/password, rejects empty, `PanalBLL.Login` -> `PanalClsDAL.Login` (`Library.DAL/PanalCls/PanalClsDAL.cs:15-52`) -> one inline parameterized SELECT on `tblUser` (with role/employee joins) `WHERE LoginName=@LoginName AND Password=@Password AND UserStatus='active'`. Rows > 0 = success; message on failure "User name or password incourect !!!!" (same message for unknown user and wrong password: no user enumeration by message) | CONFIRMED |
| Password check | **Plaintext equality in SQL.** The submitted password is passed unchanged as `@Password` and compared to the `tblUser.Password` column. No hashing, salting or key stretching anywhere in the login path | `PanalClsDAL.cs:35-41` CONFIRMED |
| Password storage | Written as-is: `update tblUser set Password=@Password,IsPasswordChange=1,UpdateBy=@UserId,UpdateDate=GETDATE() where UserId=@UserId` (`Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:78`), inserts in `UserDAL` (`Library.DAL/SInventory_DAL/UserDAL.cs:18-26`). Admin screens read the stored password back into text boxes: `SInventory_UI/UserEdit.aspx.cs:105` (`passwordTextBox.Text = aInformation.Password`, box is not `TextMode=Password`), `DoctorModule_UI/UserSetup.aspx.cs:142`. Therefore the column holds recoverable clear text (`INFERRED` for existing rows, `CONFIRMED` for the code path) | CONFIRMED |
| Default passwords | Selecting a DA user in `UserSetup.aspx.cs:596-606` sets both login name and password text boxes to the DA code (`txtPassword.Text = daCode`): default password equals login | CONFIRMED |
| `EncryptDecrypt` | `Library.DAL/DataManager/EncryptDecrypt.cs`: Rijndael-256 CBC, key from `PasswordDeriveBytes(passphrase, salt, "SHA1", 2 iterations)`; passphrase, salt and IV are **string literals in source** (not reproduced here). **No callers** in the repo (dead code). It is *not* used for passwords. If any deployment did use it, the key is public to anyone with the source | CONFIRMED (no callers) |
| Brute force | No lockout, no attempt counter, no CAPTCHA, no delay in `Login.aspx.cs`; `tblLoginLog` insert is commented out (`:46`). SP/DB triggers `UNKNOWN` | CONFIRMED (code) |
| Session fixation | `Session.Abandon()/SessionID` regeneration is not performed at login. Session id survives login (`INFERRED`) | `Login.aspx.cs` |
| Logout | `NewMasterPage.master.cs:88-95` `logOut_Click` blanks 4 keys to `""` and redirects, without `Session.Abandon()`, without clearing `UserRoleID/EmpInfoId/EmpName/...`, and without `UserSessionTrackingManager.MarkCurrentSessionLoggedOut`. The master gate tests `Session["UserId"] != null` (`:24`), and `""` is not null, so after "logout" a returning user passes the null test and only fails later when the menu call throws (caught -> redirect, `:53-61`) (`INFERRED` effect). `ImageButton1_Click` (`:78-86`) does the reverse: sets `UserId="1"`, `LoginName="admin"`, `UserType="Admin"`, then `Session.Abandon()` (harmless only because of the abandon) | CONFIRMED code, `INFERRED` effect |
| MFA / SSO | None found | grep |
| Login audit | `UserSessionTrackingManager.StartTracking` writes IP/browser/OS/device/city to `tblUserSessionTracking` on success only; failures are not recorded | `App_Code/UserSessionTrackingManager.cs` |

## 2. Session management

| Setting | Value | Note |
|---|---|---|
| Mode | `InProc`, `cookieless="false"` | lost on recycle; no state server |
| Timeout | `sessionState timeout="24000"` minutes (400 h, about 16.7 days) | far beyond normal; sessions accumulate in memory |
| Forms timeout | 2880 min, irrelevant because no ticket is issued | |
| Cookie flags | No `<httpCookies httpOnlyCookies/requireSSL>`, no `requireSSL` on forms; ASP.NET defaults apply (session cookie HttpOnly by default, not `Secure`) | `INFERRED` |
| machineKey | Not configured (auto-generated per app pool); ViewState MAC default on | |
| Session content | Identity, role, company-unit and 93 distinct keys including report SQL parameters and raw image bytes (`Session["ImageBytes"]`) | [architecture.md](architecture.md) section 10 |

## 3. Authorization

Two layers exist; neither is a complete server-side authorization model.

1. **Menu-driven visibility.** The menu is built from the database: `dbo.MainMenu2(@UserId,@UserRoleID)` for `NewMasterPage` (`Library.DAL/SInventory_DAL/UserDAL.cs:33-40`) and `tblMainMenu` joined with `tblMenuDistribution` for `MainMasterPage` (`Library.DAL/Panal_DAL/PanalDAL.cs`). In `MainMasterPage`, `UserId == 1` sees the full menu (`MainMasterPage.master.cs:49`). This only hides links; it does not stop a URL from being requested.
2. **Per-page permission check on 17 pages only** (of 706): `UserPersmissionValidation()` calls `CommonDataLoad.GetPermissionForUserRole(url)` -> SP `sp_GET_MainPermissionByUserRoleandPageUrl(@RoleId,@PageName)` (`Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:17-37`); no row => redirect to `Dashboard_UI/DashboardOne.aspx`. **Role id `"2"` skips the check** (`CustomerApproveList.aspx.cs:80-86`; same in `AdminDashboard.aspx.cs:33-36`), i.e. a hard-coded super-role. The permission flags (`RAdd`, `REdit`) returned by the SP are read only in commented-out code, so only page access is enforced, not add/edit/delete rights. Pages: `Approval_UI/{CustomerApproveList, DAApprovalList, DCPCVPApproval, DoctorApprovalList, ExpenseApprovalList, LeaveApproveList, MillageApprovalList, OrderApproveList, RXApprovalList, TourPlanApproval}`, `Dashboard_UI/AdminDashboard`, `DoctorModule_UI/{AppMonitoringList, AttendanceListApproval, UserRecords}`, `MasterSetup_UI/{CustomerView, DoctorView, EmployeeRecords}`.
3. **Approval-step engine**: `ClsApprovalAction` (`Library.DAL/InternalCls/ClsApprovalAction.cs`) enables approval actions per (login name, page URL) from `tblActionUserWiseApproval`; UI-level (radio buttons enabled/disabled), server enforcement in the approving SP is `UNKNOWN`.

Gaps in the gate itself:

| Aspect | Finding |
|---|---|
| Where the login gate runs | `NewMasterPage.master.cs` `Page_Load` inside `if (!IsPostBack)` (`:19`): postbacks are not gated by the master. Pages that also use `Session["..."]` in code-behind (223 of 706 code-behind files reference `Session["UserId"]`) fail with exceptions rather than a controlled redirect |
| Pages with no master | 133 `.aspx` have no master page; 89 of them also reference no `Session[` in code-behind, so they run **without any login check**. Includes 92 of 93 report viewers (`SInventory_RPTVIEW/*Viewer.aspx`, none references `Session["UserId"]`), and 40 popup edit pages such as `SInventory_UI/UserEdit.aspx`, `OrderEdit.aspx`, `CustMasterEdit.aspx`, `ProductEdit.aspx`, `ProUnitPriceEdit.aspx`, and `MoneyReceiptPreview.aspx`, `CustomerPaymentReportPDF.aspx`, `*ReportApps.aspx` |
| Anonymous access at IIS level | No `<authorization><deny users="?"/>`: ASP.NET does not block anonymous requests to any `.aspx`, `.ashx`, `.asmx`, static file (`INFERRED` from configuration) |
| Page methods and web services | 486 `[WebMethod]` (402 without `EnableSession`, so they cannot read the session at all) on 119 files, plus 3 `.asmx` (`SInventoryWebService`, 23 methods incl. `HelloWorld`). Page methods bypass master-page `Page_Load`; no method-level login/role check was seen in the sampled methods (`Dashboard_UI/AdminDashboard.aspx.cs:134-163`, `DoctorMaster_UI/ChamberType.aspx.cs:27-40`). Autocomplete web services return customer names/codes (`App_Code/SInventoryWebService.cs`) to any caller (`INFERRED`) |
| Data scoping | Company-unit / DC / territory scoping is applied by passing `Session["ComUnitId"]`, `RoleTypeId`, etc. into queries or SPs (per-page); enforcement inside SPs `UNKNOWN`. Client-supplied identifiers (`EmpId`, `EmpInfoId`, `reqId`, `ID`) in QueryString drive several unauthenticated pages (`DWSP/TerritoryWiseTargetSetupApps.aspx.cs:43-47`, `SixTenProjectReportApps.aspx.cs:32`, `ChallanReportViewer.aspx.cs:18`, `UserEdit.aspx.cs:19`) |
| Forced password change | `IsPasswordChange` false => `NewMasterPage.master:1628-1640` renders a static Bootstrap modal only (`ShouldShowForcePasswordChangeModal`, `.master.cs:97-119`); nothing blocks server requests, `MainMasterPage`/`MasterPage` pages have no such modal |

## 4. Roles and claims

Roles are data in `tbl_UserRoleInfo` (`UserRoleID`, links to `tblRoleType.RoleTypeId/RoleType`) and `tblUserType`; login puts `UserRoleID`, `RoleTypeId`, `RoleTypeName`, `UserType` into Session. `roleManager enabled="false"`; no `IsInRole`. Role names/values are `UNKNOWN` from the repo. Hard-coded role/user constants in code: `UserRoleID == "2"` (permission bypass), `UserId == 1` (full menu, `MainMasterPage.master.cs:49`), `UserType == "Admin"` (no company unit stored, `Login.aspx.cs:42`), `UserTypeId 6/7` (distributor/DA logic in the login SELECT), `RoleTypeName` strings ("NSM", "DIC" ... in commented code). Sales-hierarchy visibility uses `RoleTypeName` in `CustomerApproveList.aspx.cs`.

## 5. Password handling and change flow

| Step | Behaviour | Evidence |
|---|---|---|
| Creation | `UserEntry.aspx` -> `UserBLL/UserDAL` inserts clear text; `UserSetup.aspx` (DA users default password = DA code) | `UserDAL.cs:18-26`, `UserSetup.aspx.cs:596-606` |
| Complexity | Only in `ChangePassword.aspx.cs`: regex `^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{12,20}$` (12-20 chars, all four classes); new must differ from current; confirm must match (`:103-160`). Other creation/edit screens (`UserEdit`, `UserSetup`, `EmployeeSetup`, `AccountSettings`) only check non-empty | CONFIRMED |
| Change | No re-entry of the old password; needs only a valid session (`_userId` from `Session["UserId"]`). On success sets `Session["IsPasswordChange"]="True"` and the DB flag `IsPasswordChange=1` | `ChangePassword.aspx.cs:84-101`, `ChangePasswordDAL.cs:78` |
| Current-password read | `select u.Password from tblUser u with (nolock) where u.UserId=` + session id concatenated (`ChangePasswordDAL.cs:109`) | CONFIRMED (value is server-side session data) |
| Reset / forgot | No self-service reset flow found (membership provider declared but points at a non-existent `ApplicationServices` connection string) | grep |
| Expiry/history | None found in code (`IsPasswordChange` is a one-time flag) | |

## 6. Secrets and connection strings

| Location | Finding | Severity note |
|---|---|---|
| `Solution.Web/Web.config` | Live connection string uses SQL login **`sa`** with a hard-coded password `<redacted>`; 14 commented alternates with other hosts/passwords `<redacted>` (different sa passwords for different environments, a public-IP host, machine names) | file is git-ignored/untracked but present on disk and in `PrecompiledWeb/Solution.Web/Web.config` |
| `Library.DAL/DataManager/SqlUserAccess.cs` | Live `DataSource`/`UserName="sa"`/`PassWord` `<redacted>` as C# statics; ~14 commented credential blocks (12 `sa`, 2 for a lower-privileged web login) | **tracked in git** (`git ls-files` lists it) so secrets are in repository history |
| `Library.DAL/MAIN_FUNCTION/DB_Authentication.cs` | Second copy of host/`sa`/password `<redacted>` + 5 commented variants | tracked |
| Compiled DLLs | The above are also embedded in `Library.DAL.dll` (in `Bin/`, `PrecompiledWeb/.../bin`, `Library.DAL/bin/Debug`); 2,830 build-output files are tracked in git | |
| Cipher key | Hard-coded passphrase/salt/IV in `EncryptDecrypt.cs` (unused) | |
| Runtime privilege | The application connects as `sa` (sysadmin) for everything, including `msdb.dbo.sp_start_job` from `ArchiveDbConnectRepository.cs:190-197` | `INFERRED` blast radius: full instance |
| Other | `SqlUserAccess.BASE_URL`/`AppName` mention an external REST host (unused); `Pro_Info/*.xlsx` (customer ledger extracts) and `Solution.Web/ExcelFiles/*` (45 uploaded business spreadsheets) are committed/served | `git ls-files` |

## 7. Input validation

- Server-side validation is ad hoc per page (empty checks, `Convert.To*` with try/catch); no shared validation layer. ASP.NET request validation is downgraded: `requestValidationMode="2.0"` (`Web.config`), and `ValidateRequest="false"` on 1 page.
- `EnableEventValidation="false"` on 34 pages (allows tampered postback values for dropdowns, buttons, grid commands).
- `requestPathInvalidCharacters=""` (disables blocking of `< > * % & : \ ?` in URL path), `maxUrlLength=131072`, `maxRequestLength=4194303` KB and `maxAllowedContentLength=4294967295` (about 4 GB uploads/requests), `executionTimeout=36000` s: large DoS surface.
- JSON limit `maxJsonLength=50000000`.

## 8. SQL-injection posture

Counts are regex-based and approximate (patterns of the form `= '" + variable`, `LIKE '%" + variable`, `+ x.Text/SelectedValue/QueryString`); comments included unless noted.

| Area | Concatenated-SQL indicators | Parameterized indicators |
|---|---|---|
| `Library.DAL` | 953 lines matching `'" + var` in 45 files (334 lines in 28 files after restricting to SQL-shaped comparisons and excluding `//` comments) | 5,801 `new SqlParameter(` occurrences; `AddWithValue` 81 (DAL+Web) |
| `Library.BLL` | 8 / 0 | (delegates to DAL) |
| `Solution.Web` code-behind | 1,601 raw matches in 289 files (556 SQL-shaped lines in 111 files; 73 lines combine an SQL keyword with `.Text/.SelectedValue/QueryString` directly; 537 lines concatenate control/QueryString values between quotes, includes non-SQL strings) | page methods generally pass typed args to DAL |

Notable concatenation examples (all `CONFIRMED` by reading the line):

- `Library.DAL/InternalCls/ClsApprovalAction.cs:17-19` and `:71-73`: `WHERE UA.LoginName='" + userName + "' AND MM.URL LIKE '%" + pageName + "%'"` (`userName` from session/login, `pageName` from URL) executed via `DataContainerDataTable`.
- `Library.DAL/InternalCls/ClsPrimaryKeyFind.cs:16,29`: table/column names concatenated into `MAX()` query; `:52` `IDENT_CURRENT('"+tableName+"')` (callers pass constants).
- `Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:109` (`... where u.UserId=" + Id`) and `GetEmployeeInfoDAL` (`WHERE Emp.EmpInfoId="+Id`).
- `Library.DAL/MasterSetup_DAL/LeaveApplicationDal.cs:26,56`: `pram = pram + " AND EGI.EmpMasterCode = '" + employeeCode + "'"`.
- `Solution.Web/Approval_UI/CustomerApproveList.aspx.cs:459-505`: filters built from dropdown `SelectedValue` (`" AND gr.GroupId='" + GroupSelect.SelectedValue + "' "`) and passed to SP `sp_Get_CustomerApp(@param)`; SP presumably executes the fragment (dynamic SQL, `INFERRED`). Server-side re-validation of the dropdown values relies on event validation, which is disabled on 34 pages.
- `App_Code/SInventoryWebService.cs:33-60`: a session-held SQL fragment (`Session["MOParam"]`) is appended to the autocomplete SQL; the term itself is parameterized.

Structural amplifier: `ClsCommonInternalDAL` text methods send the string as `@Query` to the DB procedure `ExecuteAllSqlQueryByStoreProcedure` (body absent), executing whatever the string contains, with the `sa` login (547 call sites of the `DataContainerDataTable/Reader/SaveDataByInsertCommand/UpdateDataByUpdateCommand/DeleteDataByDeleteCommand` family in DAL). Login itself is parameterized. There is a visible hardening trend: many DALs were converted to `SInventorySql` with old concatenated code left commented (e.g. `StockTransportOrderReportDAL.cs:30-41`).

## 9. XSS

- Request validation weakened (section 7); 243 `Response.Write` calls in 72 files (none traced to a user-input echo in a sampled subset; not exhaustively reviewed), 8 uses of `HtmlEncode/AntiXss` in total in `Solution.Web`. 9 statements assign `Request.QueryString[...]` to `.Text`. Labels usually encode by default, but `Literal`, `innerHTML` building, and `ScriptManager.RegisterStartupScript` with concatenated messages are common (`ChangePassword.aspx.cs:88` builds a script string from constants; `UserEdit.aspx.cs:38` builds `alert('...')` from message strings with an ineffective `Replace("'", "\'")`).
- Menu is server-built HTML injected via `menu.InnerHtml = ...` from a DB scalar function (`NewMasterPage.master.cs:74`): stored XSS possible if menu titles are attacker-controlled (admin only; `INFERRED`).
- External resources: `MainMasterPage.master` loads scripts over plain `http://` (`html5shim`) and from Google CDN; `NewMasterPage.master` loads Font Awesome from cdnjs and Google Fonts. No Content-Security-Policy, no SRI.

## 10. CSRF

WebForms ViewState MAC and event validation provide implicit protection for postbacks only when ViewState is user-bound (`ViewStateUserKey` is not set anywhere); `EnableEventValidation="false"` on 34 pages. No anti-forgery tokens. State-changing GET links exist (e.g. `AreaEdit`/`UserEdit` popups keyed by `?ID=`). Page methods use JSON POST (browsers block cross-site form POSTs with `application/json`, `INFERRED` partial mitigation), ASMX `ScriptService` likewise. Cookies have no `SameSite` setting (default of the framework/browsers).

## 11. File upload handling

| Upload | Extension / size checks | Save path | Risk |
|---|---|---|---|
| 15 Excel import pages (`SInventory_UI/CustomerExcelUpload`, `CustomerPaymentExcelUpload`, `CustomerTagChangeExcelUpload`, `DeliveryExcelUploadOldData`, `DepositSlipExcelUpload(_old)`, `FixedCustomerUpload`, `MIGO_Upload`, `OrderListUpload`, `OrderRequisitionCreation`, `StockTransferOrder`, `TargetExcelUpload`; `TransferUI/CustomerImport`, `DoctorImport`, `MarketInfoImport`) | `id_fu.SaveAs(MapPath("~/ExcelFiles/" + id_fu.FileName))` **executes before** the extension test; only `.xls`/`.xlsx` are then opened via OleDb; other extensions are still saved (e.g. `CustomerExcelUpload.aspx.cs:262-282`). No size or content-type check besides the 4 GB request limits | web root `~/ExcelFiles/` (served statically; 45 files present, filenames from the client, overwrite of same name) | arbitrary-extension file written into the web root by authenticated (master-gated) users; possible executable upload (`.aspx`) - `INFERRED` |
| `SInventory_UI/HandlerDocCV.ashx` (product image, called by `ProductEntry.aspx`) | **None**: `Files[0]` saved as `<guid>Product_<client extension>` | `~/UploadFile/` (33 images present) | **Unauthenticated** (handler, no master, no session check) arbitrary extension upload to the web root - `INFERRED` critical |
| `DoctorModule_UI/Prescription.aspx`, `ExpenseClaim.aspx`, `MileageClaim.aspx`, `MasterSetup_UI/CustomerEntry.aspx`, `NoticeSetup.aspx`, `EmpGeneralInfoEntry.aspx`, `ProductEntry.aspx` | File inputs exist, but no server `SaveAs`/`WriteAllBytes` in the web project (uploads handled client-side/JS or by an external API) - `UNKNOWN` | `PrescriptionImages/` (9 committed images) and `UploadFile/` | writers `UNKNOWN` |
| Download | `Response.AddHeader Content-Disposition` for generated Excel/PDF (192 hits) | memory streams | file names built from constants |

## 12. Sensitive data exposure

- `PictureHandler.ashx` / `SignatureHandler.ashx` (root): serve JPEG bytes from `Session["ImageBytes"]` for the current session only (session-scoped, not IDOR); `SignatureHandler` tests the wrong key (`ImageBytes`) before writing `SigImageBytes` (`SignatureHandler.ashx:11-13`), so it can write a null buffer.
- `PrescriptionImages/` (patient prescription images/screens) and `UploadFile/` are inside the web root, directly URL-addressable without authentication (`INFERRED`; no handler mediates access).
- Customer/ledger and deposit-slip spreadsheets are committed (`Pro_Info/`, `Solution.Web/ExcelFiles/`) and served by IIS from `ExcelFiles/`.
- Client IP, city, browser and device of every login are stored and the client IP is sent to `ipapi.co` (third-party) - privacy/compliance note.
- Unauthenticated report viewers accept business identifiers in QueryString (`reqId`, `InvNo`, dates, `districtId`), returning challans/ledgers/sales (`ChallanReportViewer.aspx.cs:18`), enabling enumeration (`INFERRED`).
- `UserEdit.aspx?ID=<n>` (no master/session check) loads and updates any user including login name, role-type text, status and the plaintext password (`SInventory_UI/UserEdit.aspx.cs:15-22, 104-111`); the `ID` is the sequential `MAX+1` key.

## 13. CORS and transport (HTTPS)

- No CORS configuration in Web.config or code (grep); same-origin only, mobile apps use native HTTP clients.
- No HTTPS enforcement: no `requireSSL`, no rewrite/HSTS, no `Secure` cookie flag configured (`INFERRED` reliance on IIS binding).
- DB transport: `DataAccessManager` uses `Encrypt=True;TrustServerCertificate=True` (encrypts but does not validate the certificate: MITM-able inside the network) (`DataAccessManager.cs:22-32`); the Web.config connection string, `DataAccessManagerOld/Async`, `DB_Connection` do **not** set `Encrypt`, so EntLib/`DB_Manager` traffic uses server defaults (`INFERRED` clear/optional TLS).
- `MainMasterPage.master` still references `http://` script sources.

## 14. Redirect validation in Login

`Login.aspx.cs:83-116`: after a successful login, `Request.QueryString["aspxerrorpath"]` is URL-decoded and used in `Response.Redirect(target, true)` if it does **not start with `http`** (case-insensitive). Protocol-relative and slash variants (`//host`, `/\host`, `\\host`) do not start with `http`, so the check is bypassable (open redirect after login, `INFERRED`); the deny-list is the only validation. Only users with `IsMainDashboard == false` reach this branch. Other redirects use constant paths.

## 15. Error disclosure

No `<customErrors>` => framework default `RemoteOnly` (generic page for remote users, detailed for local). `compilation debug="true"` in Web.config (also in `PrecompiledWeb`). Empty `Application_Error` (and it is in an ignored file). Direct disclosure paths: pages showing `exception.Message` (e.g. `SettingPanel_UI/ArchiveDbConnect.aspx.cs`, `DoctorCategoryDal` sets `ErrorMessage = exception.Message`), SQL error text returned to the UI, and `throw ex;` losing stack. Also `Login.aspx.cs:66-82` swallows exceptions silently.

## 16. Other observations

- `UserSessionTrackingManager` trusts `X-Forwarded-For`/`Client-IP`/`CF-Connecting-IP` headers for the audited IP (spoofable).
- The `ArchiveDbConnect` page runs `msdb.dbo.sp_start_job` and inserts a DB name that later feeds `sp_DeleteArchiveDB*Data(@databaseName)`; the page has no permission check (`INFERRED`, `SettingPanel_UI/ArchiveDbConnect.aspx.cs` has no `Session` reference; only the master gate).
- Legacy DB name `SalesDisDB_New3` in `DB_Manager` paths (see architecture) - data written to an unexpected database is a integrity risk, not confidentiality.

## 17. Risk register (severity is INFERRED)

| ID | Finding | Evidence | Severity | Status |
|---|---|---|---|---|
| SEC-01 | Passwords stored and compared in clear text; admin screens display them | `PanalClsDAL.cs:35`, `ChangePasswordDAL.cs:78`, `UserEdit.aspx.cs:105`, `UserSetup.aspx.cs:142` | Critical | Open |
| SEC-02 | Application connects to SQL Server as `sa` with a password hard-coded in tracked source (`SqlUserAccess.cs`, `DB_Authentication.cs`) and Web.config; 14+ commented alternates with other environment passwords; secrets in git history and compiled DLLs | `Library.DAL/DataManager/SqlUserAccess.cs`, `Library.DAL/MAIN_FUNCTION/DB_Authentication.cs`, `Solution.Web/Web.config` | Critical | Open |
| SEC-03 | No authorization model: Forms auth never issued, no `<authorization>`; 133 pages without master, 89 with no session use at all (incl. `UserEdit.aspx`, 92 report viewers) | `Login.aspx.cs`, Web.config, page scan | Critical | Open |
| SEC-04 | Unauthenticated file upload handler saves any extension into web root | `SInventory_UI/HandlerDocCV.ashx:14-28` | Critical | Open |
| SEC-05 | Excel importers save the upload to `~/ExcelFiles/` before validating the extension (15 pages) | `CustomerExcelUpload.aspx.cs:262-282` and 14 siblings | High | Open |
| SEC-06 | Widespread string-concatenated SQL (334 SQL-shaped DAL lines/28 files; 556 in 111 web files), plus generic SQL-text executor SP run as `sa` | `ClsApprovalAction.cs:17`, `CustomerApproveList.aspx.cs:459-505`, `ClsCommonInternalDAL.cs` | High | Open (partial remediation visible) |
| SEC-07 | 402 `[WebMethod]` without session and 84 with session but no observed authorization; page methods bypass master gate; `.asmx` reachable anonymously | grep, `AdminDashboard.aspx.cs:134` | High | Open |
| SEC-08 | Per-page permission check on only 17 pages; role `"2"` bypass hard-coded; add/edit flags unused; menu-only control elsewhere | `CustomerApproveList.aspx.cs:80-105` | High | Open |
| SEC-09 | Logout does not abandon session and only blanks 4 keys; session timeout 24,000 min; no session-id regeneration at login; tracking logout never recorded | `NewMasterPage.master.cs:88-95`, Web.config | High | Open |
| SEC-10 | No brute-force protection, no lockout, no MFA; default DA password equals login | `Login.aspx.cs`, `UserSetup.aspx.cs:604` | High | Open |
| SEC-11 | Weak password policy except on ChangePassword; no old-password confirmation on change | `ChangePassword.aspx.cs:84-190` | Medium | Open |
| SEC-12 | Open redirect via `aspxerrorpath` (prefix check only) | `Login.aspx.cs:99-113` | Medium | Open |
| SEC-13 | Request validation 2.0, `requestPathInvalidCharacters=""`, EventValidation off on 34 pages, 1 page `ValidateRequest=false`, 4 GB request limits | Web.config, aspx grep | Medium | Open |
| SEC-14 | Prescription and product images and uploaded spreadsheets served from the web root without access control | `PrescriptionImages/`, `UploadFile/`, `ExcelFiles/` | Medium | Open |
| SEC-15 | `TrustServerCertificate=True`; EntLib/Web.config connection without `Encrypt`; no HTTPS enforcement/Secure cookies; `http://` script in `MainMasterPage` | `DataAccessManager.cs:22-29`, Web.config | Medium | Open |
| SEC-16 | Debug build (`debug="true"`), no `customErrors`, empty `Application_Error`, exception text shown, no central logging | Web.config, `Global.asax` | Medium | Open |
| SEC-17 | Login IP/location audit trusts spoofable headers and discloses client IPs to a third party (ipapi.co); activity/logout tracking dead code | `UserSessionTrackingManager.cs` | Low | Open |
| SEC-18 | Unused crypto with hard-coded key (`EncryptDecrypt`), unused `BouncyCastle`, `log4net` referenced but absent | `EncryptDecrypt.cs`, Web.config | Low | Open |
| SEC-19 | Static DAL/BLL fields with shared connection/transaction state may leak data or transactions between concurrent users (366 sites) | `ChamberType.aspx.cs:13`, `DataAccessManager.cs:14-16` | Medium (integrity) | Open |
| SEC-20 | Business data (customer ledgers, deposit slips) and ~2,830 build files committed to source control | `Pro_Info/`, `Solution.Web/ExcelFiles/`, git index | Medium | Open |
| SEC-21 | `MAX+1` key generation races; archive delete SPs invoked with client-selected DB name without permission check | `ClsPrimaryKeyFind.cs:11-46`, `ArchiveDbConnect.aspx.cs` | Medium (integrity) | Open |

`Status = Open` means observed in the current repository at review date 2026-09-19; no remediation has been evidenced for the items above.
