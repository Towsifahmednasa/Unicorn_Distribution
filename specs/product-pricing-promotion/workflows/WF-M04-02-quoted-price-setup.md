# WF-M04-02 Quoted price setup

**Trigger / actors:** sales/pricing administrator from `QuotedPriceView.aspx` (PAGE-119). **Preconditions:** active products (`sp_Get_Product_Active`) with list price (TP) shown.

## Steps
1. `QuotedPriceSetup.aspx` (PAGE-118) loads all active products; edit `?MID` loads `sp_GET_QuotedPriceMaster_ById` + `sp_Get_QuotedPriceDetailById` and recalculates final prices from current TP (BR-M04-036).
2. Create: enter description, policy, dates; pick customers via autocomplete (`GetCustomer`) and "Add" (BR-M04-032).
3. Tick products; enter discount % (`txtVat`) or discount amount (`txtDiscountPercent`) per row or a common % (`txtCmnPercent`): server postback computes per BR-M04-035 (`Final = round(TP - TP*P/100, 3)`; or `P = round(A/TP*100,3)`, `Final = round(TP-A,3)`).
4. Save -> `Validation` (BR-M04-031/033) -> for each customer `QuotedPriceSetupDAL.SaveMasterDetals`: `sp_Save_QuotedPriceMaster` (or `sp_Update_QuotedPriceMaster`) then `sp_Save_QuotedPriceDetail(@QuotedPriceMasterId,@ProductId,@UnitPrice=Final,@Vat=P)` per ticked product. Cumulative detail list defect for customers 2..N (BR-M04-034).
5. Success alert -> `QuotedPriceView.aspx`.

## Data / exceptions / postconditions
Quoted-price master and detail tables (UNKNOWN names). No status; validity by dates only. Failure -> "Already Exist!" or error page (no try/catch). Consumer of quoted prices not found in repo (GAP-M04-016). **Related:** BR-M04-031..037, PAGE-118/119. **Tests:** TP 200 with 10% -> 180.000; amount 15 -> 7.500%; 3 customers x 2 products -> duplicate rows check; TP 0 amount entry no change.
