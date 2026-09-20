# WF-M17-01 - SAP stock movement (challan) receive

Trigger/actors: SAP stages a stock movement (`SAP_API_Data`); DIC/DC or head-office user approves. BR-M17-002..005, 008-010, 018. Confidence: CONFIRMED wiring, SP bodies UNKNOWN (except B2B script).

Preconditions: challan present in staging; SAP product codes map to `tblProduct.SAP_Code`; from/to plants map to `tblCompanyUnit.SAP_Code`.

Steps
1. `SAP_IntrigationPointDIC.aspx` load -> `GetSAP_StockReceivePendingDataDIC` (`sp_Get_SAP_DICStockReceivePendingData @Parm=DICCompanyUnitId`) -> grid; rows hidden if `sp_Get_SAP_HideChallanByChallanNo` returns rows.
2. `Preview >>` -> `Session["StockMovementMasterId"]` -> `SAP_StockReceiveDIC.aspx`: `sp_Get_SAP_StockReceivePendingDataById`; rows red / Approve hidden per BR-M17-004.
3. `ApproveButton_Click`: validate quantities > 0 -> per line `sp_SAP_Up_StockReceiveQty` -> `sp_SAP_StockReceive(@ChallanNo,@ApproveBy)`.
4. Inside receive (INFERRED, script evidence): B2B SPs create ERP challan (`tblChalanInfo`, `tblChalanDetail`, FEFO deduction of `tblDCStore`) and destination `tblDCStore` 'Available' rows.
5. Redirect to list; challan disappears. Later, DC receipt of ERP challans triggers INT-03 confirmation.

Data changed: staged quantities, ERP challan/stock tables (per INT-02). Exceptions: SP error -> rollback + yellow page; partial line updates persist. Postcondition: ERP stock equals SAP challan (subject to BR-M17-009 defects). Tests: TC-681-01..05, TC-682-01..08. Pages: 680-683.
