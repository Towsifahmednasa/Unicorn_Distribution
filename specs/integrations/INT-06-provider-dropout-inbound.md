# INT-06 - Provider dropout (e-Program) - inbound, approval gate

| Item | Detail |
|---|---|
| Direction | External e-Program system -> ERP |
| Transport | Database table `dbo.tblProviderDropoutIntrigration` (no inserting code in repo; producer UNKNOWN). ERP side: ASP.NET page methods (JSON over POST) in `eProgram_UI/ProviderDropoutRequestList.aspx.cs`. |
| Trigger | User opens PAGE-694 (list on load) and clicks Approve. |
| Payload fields | `providerIDropoutIntrigrationd (BIGINT key), providerId, programShortName, providerCode, providerName, mobileNo, nid, email, outlet, dropoutReason, insertedAt, IsApprove, IsApproveBy, ApproveDate`. |
| SPs / DDL (in repo) | `sp_Get_ProviderDropoutIntrigrationList` (all rows, `ORDER BY insertedAt DESC`), `sp_Up_ProviderDropoutIntrigrationApprove(@providerIDropoutIntrigrationd,@ApproveBy)` (only where `ISNULL(IsApprove,0)=0`; sets IsApprove=1, IsApproveBy, `ApproveDate=SYSUTCDATETIME()`), `alter_tblProviderDropoutIntrigration_AddApprovalColumns.sql` (idempotent `COL_LENGTH` guards). |
| Status / retry | `IsApprove` 0 -> 1, no reject/undo; approve idempotent (0-row update still reports success). No downstream action coded (UNKNOWN). |
| Authentication | Approve needs `Session["UserId"]` (read in DAL). **List method has no session/auth and returns mobile, NID, e-mail** (INFERRED anonymous reachability). |
| Error handling | Try/catch returns `{isSuccess:false,message:ex.Message}` (raw exception text). |
| Refs | PAGE-694, BR-M17-015/016, GAP-M17-006/008. |
