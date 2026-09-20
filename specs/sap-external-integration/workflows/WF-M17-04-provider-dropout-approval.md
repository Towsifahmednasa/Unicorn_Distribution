# WF-M17-04 - Provider dropout approval

Trigger/actors: external system inserts rows in `tblProviderDropoutIntrigration` (producer UNKNOWN); ERP user approves on `eProgram_UI/ProviderDropoutRequestList.aspx`. BR-M17-015/016. CONFIRMED.

Steps
1. Page load -> AJAX `GetProviderDropoutIntrigrationList` -> `sp_Get_ProviderDropoutIntrigrationList` -> table.
2. Approve click -> AJAX `ApproveProviderDropoutIntrigration(id)` -> `SAP_IntrigationPointDAL.ApproveProviderDropoutIntrigrationDAL` -> `sp_Up_ProviderDropoutIntrigrationApprove(@id,@ApproveBy=UserId)` -> `IsApprove=1, IsApproveBy, ApproveDate=UTC` where not yet approved.
3. Button becomes "Approved"; list reloads.

Exceptions: no session -> NullReference message; repeated approve is a silent no-op success. Downstream effect UNKNOWN. Tests: TC-694-01..07. Pages: 694. See INT-06.
