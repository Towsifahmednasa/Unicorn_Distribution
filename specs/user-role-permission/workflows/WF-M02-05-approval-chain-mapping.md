# WF-M02-05 — Approval chain mapping (role types per menu)

**Trigger:** admin uses `ApprovalStepMap.aspx` — PAGE-026.
1. Load role types (`sp_GET_RoleType`) and menus (`sp_GET_MainMenuByType('')`).
2. Choose menu and From role → `LoadApprovalMap` → `sp_GET_ApprovalMapLoad(@FromRoleId,@MenuId)`; empty → the from role is pre-inserted as step 1.
3. Add To-role + Order (client-side uniqueness of order and role).
4. Save → `SaveMenu`: `sp_Save_ApprovalMapMaster(@MenuId,@FromRoleId,@MenuName=null)` → pk → `sp_Save_ApprovalMapDetail(@ApprovalMapMasterId,@Order,@ToRoleId)` per step; returns "True" (no transaction, no auth).

**Consumption (INFERRED):** approval list pages compare `ToRoleTypeId` with `Session["RoleTypeId"]` (e.g. `CustomerApproveList.aspx.cs:532-548`); SP-side use UNKNOWN. **Exceptions:** row-index bug after Remove; re-save may duplicate. **BR:** 024-027. **Gaps:** 001, 004, 005, 013, 014, 015.
