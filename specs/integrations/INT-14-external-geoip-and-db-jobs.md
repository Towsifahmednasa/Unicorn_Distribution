# INT-14 - Other outbound touchpoints: GeoIP lookup and SQL Agent backup job

| Integration | Evidence | Detail |
|---|---|---|
| ipapi.co GeoIP | `Solution.Web/App_Code/UserSessionTrackingManager.cs:479-520`, called from `Login.aspx.cs:48` | HTTPS GET `https://ipapi.co/<ip>/json/`; only for non-private IPs; timeout 3 s; fields read: `country_name, region, city`; result stored in session-tracking record; failure silently ignored; no API key. Sends users' public IPs to a third party. |
| SQL Agent backup job | `Solution.Web/App_Code/ArchiveDbConnectRepository.cs:20-110`, page `SettingPanel_UI/ArchiveDbConnect.aspx.cs` | Inserts `dbo.tblArcDBConnect (FY, DataBaseName)` then `EXEC msdb.dbo.sp_start_job @job_name` (default `Society_DB_Backup_Job`, overridable by app settings `DatabaseBackupJobName`, `DatabaseBackupConnectionStringName`). Requires the web DB login to have msdb job rights. |
