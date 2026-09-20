# WF-M02-06 — Approver assignment (legacy `tblAppSetup`)

1. Admin opens `ApprovalSetupEntry.aspx` (PAGE-019); picks an approval menu (`tblMainMenu.IsApprovalPage='1'`), adds (active user, e-mail) rows.
2. Submit: `DELETE FROM tblAppSetup WHERE SL=@SL`, then `INSERT` per row with `EntryBy=LoginName`, `EntryDate=now` (`ApprovalSetupEntry.aspx.cs:163-184`).
3. Consumers: `DataTrue()` → `GetMenuIdByMenuName(file)` (`tblMainMenu.URL LIKE`) → `GetAssignedAppUser(SL, Session["UserId"])`. Live only in `SubDepot_UI/SubDepotStockOutApproval` (first load; postback unguarded); commented out in `DcStockOutApproval`, `WarehouseStockInApproval`, `DoctorCustomerTransferApproval`.

**Exceptions:** blank user row throws after the delete; wrong join in `LoadAppSetup`; concatenated SQL in two DALs. **BR:** 046-049. **Gaps:** 001, 004, 015, 024, 025.
