# INT-03 - ERP challan send / receipt confirmation to SAP - outbound

| Item | Detail |
|---|---|
| Direction | ERP -> SAP |
| Transport | (a) **DB-side REST**: after a DC receives a challan the ERP executes SP `MakeRESTRequest(@StoNo, @RcvBy)` (`SAP_IntrigationPointDAL.MakeRESTRequestWithUpdateChallan`, `SAP_IntrigationPointDAL.cs:548-577`). The SP body is UNKNOWN; its name implies the DB itself issues an HTTP call (INFERRED). (b) Challan send payload SP `sp_SAP_ChallanSendDetailByChalanId(@ChalanId)` -> `product_code (tblProduct.SAP_Code), batch_no, quantity, UoM (tblStockUOM.UOMSAPCode), remarks ''`; flag SPs `sp_SAP_Up_SAP_ChallanSendByChalanId` (`tblChalanInfo.SAP_ChallanSend=1`) and `sp_SAP_Up_SAP_ChallanConfirmByChalanId` (`SAP_Challan_ConfirmationSend=1`) - bodies in `SP_NewStockTransferDcToDc_Related.sql`; **no C# caller**, so an external job/DB job is assumed (INFERRED). |
| Trigger | Save on `SInventory_UI/ReceiveProductByChalanByDC.aspx.cs:148` and `TransferReceiveProductByChalanByDC.aspx.cs:118` (after `SaveDCStoreFreeze2` loop). |
| Payload | `@StoNo` = challan number text box, `@RcvBy` = `Session["UserId"]`. |
| Status flags | `tblChalanInfo.SAP_ChallanSend`, `SAP_Challan_ConfirmationSend` (0/1). Monitored on `SAPDashboard.aspx` grid "Send to SAP" / "Confirmation to SAP" (`sp_RPT_ChallanStatusByDate`, columns `SAP_ChallanSend`, `SAP_ChallanRecevie`). |
| Retry | None in code. |
| Authentication | UNKNOWN (inside SP/external job). |
| Error handling | `try { MakeRESTRequestWithUpdateChallan } catch { }` - **errors swallowed**; user still sees "Data Save Successfully!!" (BR-M17-012). Failed confirmations are only visible on the SAP dashboard. |
| Pages | ReceiveProductByChalanByDC, TransferReceiveProductByChalanByDC, PAGE-692 SAPDashboard. |
