# GAPS - M17 SAP, Mobile-App & External Integration

| GAP ID | Module | Page | Description | Impact | Source | Status |
|---|---|---|---|---|---|---|
| GAP-M17-001 | M17 | 680-683, 686, 694, 679 | SP bodies missing: `sp_SAP_StockReceive, sp_SAP_Up_StockReceiveQty, sp_Get_SAP_*, sp_Upsertdate_EmpInfo/ProductInfo, sp_Chk_SAP_EmpInfoCondition, sp_SAP_BankDeposit*, MakeRESTRequest, sp_RPT_*StatusByDate, sp_Webapi_*, sp_webapi_Save*AppLog, sp_Approve*Information, AppMonitoringViewList_new` and others | Rules, tables, status values unverifiable | `SAP_IntrigationPointDAL.cs` etc. | Open |
| GAP-M17-002 | M17 | 686 | Hard-coded SAP RESTAdapter Basic-auth credentials in source (`<redacted>`); HTTP POST and sent-flag update commented out | Credential leak; Final Submit is a no-op, deposits never marked sent | `BankDepositSAP.aspx.cs:276-291` | Open |
| GAP-M17-003 | M17 | 686 | Search runs write-like `sp_SAP_BankDeposit_SAP_Process` every time; errors swallowed | Read has side effects; silent failures | `BankDepositSAP.aspx.cs:60-101` | Open |
| GAP-M17-004 | M17 | 677/678/679/694, ASMX | Page methods and ASMX lack authentication; anonymous reachability inferred (no `<authorization>`); include user/employee lists, provider PII, `Delete_Prescription`, `Delete_Trainning`, unfiltered sub-territory list | Data disclosure / unauthorised delete | `Setup.aspx.cs:360,1054,1638`, `CommonDataLoad.aspx.cs`, `ProviderDropoutRequestList.aspx.cs:18`, `Web.config:80-82` | Open |
| GAP-M17-005 | M17 | 682 | Approve overwrites SAP quantities per line then receives, non-atomic, no upper bound vs shipped/stock; original SAP qty lost | Stock discrepancies vs SAP | `SAP_StockReceive.aspx.cs:119-170` | Open |
| GAP-M17-006 | M17 | 694 | `tblProviderDropoutIntrigration` base DDL and inserter absent; approve idempotent reports success for missing id; no reject/downstream action | Unknown producer contract | `sql/*.sql` | Open |
| GAP-M17-007 | M17 | INT-02 | `sp_SAP_B2BChallanDetailInsert`: inserts `Quantity=@Qty` for every lot; stale `@StockQuantity` when no stock lot found; `MAX+1` keys; `sp_SAP_B2B*` have no C# caller | Wrong challan quantities / phantom stock movement | `SP_NewStockTransferDcToDc_Related.sql` (proc 5) | Open |
| GAP-M17-008 | M17 | 694 | List returns whole table incl. mobile/NID/e-mail with no auth/paging; raw exception messages returned | PII exposure | `ProviderDropoutRequestList.aspx.cs` | Open |
| GAP-M17-009 | M17 | INT-03 | `MakeRESTRequest` failures swallowed after DC receive; no retry; send/confirm flag SPs have no C# caller | SAP confirmations can be lost silently | `ReceiveProductByChalanByDC.aspx.cs:148` | Open |
| GAP-M17-010 | M17 | 679 | ~17 client-referenced methods missing; `Approve_TourPlanList` non-static; ~75% commented legacy code | Broken screens | `Setup.aspx.cs:136` | Open |
| GAP-M17-011 | M17 | 680/681 | Product approve no pre-check; buggy `gv_ProductInfo_RowCommand`; near-duplicate DIC pages; N+1 hide-challan calls | Master-data risk, drift | `SAP_IntrigationPoint.aspx.cs` | Open |
| GAP-M17-012 | M17 | INT-10 | `SqlUserAccess.BASE_URL`/`AppName` unused; also hard-coded DB credentials in `SqlUserAccess.cs`/`Web.config` | Dead config; secrets in source | `SqlUserAccess.cs:84-85` | Open |
| GAP-M17-013 | M17 | 688/689 | MIGO upload non-atomic, file saved with client name; `MIGOList.Parameter()` concatenated and malformed quotes; delete non-transactional | Partial loads, injection | `MIGO_Upload.aspx.cs`, `MIGOList.aspx.cs:100-115` | Open (Tier B) |
| GAP-M17-014 | M17 | 685/687 | API customer promote is two separate statements; validation only non-empty | Duplicate/partial promotion | `ApiCustomerList.aspx.cs` | Open (Tier B) |
| GAP-M17-015 | M17 | 676 | Filter fragments concatenated into `@Parameter` of `AppMonitoringViewList_new` | Injection risk | `AppMonitoringList.aspx.cs:100-120` | Open |
| GAP-M17-016 | M17 | ASMX | `GetPreBatch`, `GetAllInvoice` concatenate Session values; `GetCustomernew` appends Session-held SQL fragment | Injection if session tainted | `SInventoryWebService.cs:261-300,31-60` | Open |
| GAP-M17-017 | M17 | INT-14 | Login sends client IP to ipapi.co; SQL Agent job start via web login | Privacy, privilege | `UserSessionTrackingManager.cs:495` | Open |
| GAP-M17-018 | M17 | Tier B pages | 676, 683-685, 687-693 not reviewed line by line | Unverified | - | Unverified |
