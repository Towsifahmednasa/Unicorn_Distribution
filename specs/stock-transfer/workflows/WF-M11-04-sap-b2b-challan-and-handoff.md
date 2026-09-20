# WF-M11-04 — SAP-origin B2B challan and SAP hand-off states

**Confidence:** CONFIRMED for SP bodies (`SP_NewStockTransferDcToDc_Related.sql`); callers/scheduler UNKNOWN (no C# code calls these SPs).

## Trigger & actors
An external integration process (UNKNOWN — SQL Agent job or SAP interface service) handles SAP stock movement (STO) documents: `SAP_API_Data..tblSAP_StockMovementMaster/Detail`. ERP users then see the challan in the B2B list / receive pages and the SAP dashboard (`SAPDashboard.aspx`).

## Preconditions
SAP data staged in `SAP_API_Data`; `tblCompanyUnit.SAP_Code` set for from/to plants; `tblProduct.SAP_Code` maps to `SUBSTRING(product_code,13,…)`; active `tblUnitPrice`; challan not already in `tblWHStockInMaster`.

## Steps
| # | Step | SP |
|---|---|---|
| 1 | Create challan header from SAP movement | `sp_SAP_B2BChallanMasterInsert(@ChallanNo)` → INSERT `tblChalanInfo` (`Note='From SAP'`, `IsDeliver='False'`, `ManufacId=1`, `ChalanDate=GETDATE()`, values = Σ qty × price/VAT, missing → 1); returns new `ChalanId` |
| 2 | Create details and consume source stock | `sp_SAP_B2BChallanDetailInsert(@MasterId,@ChallanNo)` → FIFO by `ExpDate`, `UPDATE tblDCStore SET StockQty …` (decrement), INSERT `tblChalanDetail` (see defects) |
| 3 | Receive at destination DC | `sp_SAP_B2BStockReceiveByDc(@MasterId)` → INSERT `tblDCStore` Receive rows (`Available`, no cost/reference) at the ToComUnit; (or the user receives through PAGE-423 using the challan) |
| 4 | Send challan detail to SAP | `sp_SAP_ChallanSendDetailByChalanId(@ChalanId)` returns `product_code (SAP code), batch_no, quantity, UoM, remarks=''` |
| 5 | Mark sent | `sp_SAP_Up_SAP_ChallanSendByChalanId` → `tblChalanInfo.SAP_ChallanSend = 1` |
| 6 | Mark receipt confirmation sent | `sp_SAP_Up_SAP_ChallanConfirmByChalanId` → `tblChalanInfo.SAP_Challan_ConfirmationSend = 1` |
| 7 | Monitor | `SAPDashboard.aspx` → `CmnCrystaltoView.GetChallanStatusByDateDAL` → `sp_RPT_ChallanStatusByDate` (body absent) → grid columns `ChalanDate, ChalanNo, SAP_ChallanSend, SAP_ChallanRecevie` |

## Data created / changed
tblChalanInfo (header, SAP flags), tblChalanDetail, tblDCStore (source decrement; destination Receive rows).

## Stock effects
Source: in-place decrement (FIFO by expiry) — **no Issue ledger row** (differs from WF-M11-02). Destination: Receive rows with `TotalQuantity=StockQty=chalan qty` (no damage split, no cost). Shortages produce phantom detail rows (GAP-M11-004).

## Valuation impact
`ISNULL(UnitPrice,1)` and `ISNULL(VATAmountPerUnit,1)` default to 1 in challan totals; destination rows carry no cost.

## Accounting posting implications
None in these SPs.

## SAP hand-off states (summary)
| State | Where | Set by | Meaning |
|---|---|---|---|
| Challan exists `Note='From SAP'`, `IsDeliver='False'` | tblChalanInfo | `sp_SAP_B2BChallanMasterInsert` | SAP STO imported, pending receive |
| `SAP_ChallanSend=1` | tblChalanInfo | `sp_SAP_Up_SAP_ChallanSendByChalanId` | challan lines transmitted to SAP |
| `SAP_Challan_ConfirmationSend=1` | tblChalanInfo | `sp_SAP_Up_SAP_ChallanConfirmByChalanId` | receipt confirmation transmitted to SAP (dashboard label "Confirmation to SAP"; dashboard field name `SAP_ChallanRecevie` differs) |
| `IsDeliver='True'` | tblChalanInfo | `ChalanDAL.ChalanUpdate` (PAGE-423) — **not** set by `sp_SAP_B2BStockReceiveByDc` | received in ERP |
| STO receipt notify | `MakeRESTRequest(@StoNo,@RcvBy)` | PAGE-413 / PAGE-423 save | fire-and-forget REST call from SQL; result not stored |

## Branches / exceptions
Master insert may create several headers (one per group of date/units); the SP returns only the last id. Missing SAP price/unit/product mapping silently drops lines (inner joins/`UP.IsActive=1`). `sp_SAP_B2BStockReceiveByDc` does not close the challan → a later manual receive (PAGE-423) can double-receive.

## Postconditions
Challan tracked with SAP flags; stock moved per SP semantics.

## Related BR ids
BR-M11-012, 013, 014, 035.

## Related pages
PAGE-412 (documents SP set), 423/424 (receive), SAPDashboard.aspx (other module), PAGE-407 (list).

## Tests
Run the detail SP with stock < SAP qty (expect phantom rows); run both flag SPs and verify dashboard columns; receive via `sp_SAP_B2BStockReceiveByDc` and then via PAGE-423 (expect duplicate).
