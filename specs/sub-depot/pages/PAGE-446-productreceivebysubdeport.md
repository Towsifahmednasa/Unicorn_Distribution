---
page_id: PAGE-446
module: M12
page: ProductReceiveBySubDeport
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-446 — ProductReceiveBySubDeport

> **Evidence level:** Tier A - reviewed 2026-09-19 against the .aspx, .aspx.cs and the BLL/DAL/DAO code they reach. Stored-procedure bodies are NOT in the repository unless stated; such effects are marked `UNKNOWN`. Confidence tags: CONFIRMED (read in code), INFERRED (implied by several references), UNKNOWN.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-446 |
| Module | M12 — Sub-Depot Operations (INFERRED from folder + file name) |
| Page name | ProductReceiveBySubDeport |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SubDepot_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SubDepot_UI_ProductReceiveBySubDeport |
| aspx size | 10108 bytes |
| Code-behind LOC | 165 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**ProductReceiveBySubDeport** is the sub-depot's goods-receipt screen for a DC->Sub-depot chalan. It opens from the pending-chalan list (`StockReceiveBySubDepot.aspx`, hyperlink `ProductReceiveBySubDeport.aspx?ChalanId={SChalanId}`) and shows the chalan header (no, date, truck, driver) and lines (product, pack, batch, quantity sent, expiry, mfg date). Per line the user confirms `RcvQty` and `UnRcvQty` (damage); Submit creates the sub-depot stock rows (and a `Restricted` freeze row for the un-received/damaged part), marks the chalan delivered and pops up the receive report. CONFIRMED (`ProductReceiveBySubDeport.aspx.cs:46-118`).

## 3. Business Objective
Confirm physical receipt at the sub-depot: make the transferred goods available for sale at the sub-depot (`Available` stock) and quarantine damaged/short quantities (`Restricted`), closing the in-transit state of the chalan (WF-M12-01). INFERRED from labels (`RcvQty`, `UnRcvQty`) and the tables written.

## 4. Navigation
Reached from `StockReceiveBySubDepot.aspx` (grid `HyperLinkField`, `StockReceiveBySubDepot.aspx:154-155`) with `?ChalanId=<SChalanId>`; `Back To List` -> `StockReceiveBySubDepot.aspx` (`.cs:161-164`); after save opens `../SInventory_RPTVIEW/SubDeportTransferStockReceiveReportViewer.aspx?ChalanId=<chalanNo>` (RPT-M12-02). Master `MainMasterPage.master`. Menu path UNKNOWN (DB-driven).

## 5. Route
`~/SubDepot_UI/ProductReceiveBySubDeport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SubDepot_UI/ProductReceiveBySubDeport.aspx |
| Code-behind | Solution.Web/SubDepot_UI/ProductReceiveBySubDeport.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | SubDepotChalanBLL (Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs) |
| DAL class | SubDepotChalanDAL (Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs) |
| DAO/entity class | DCStockNew (Library.DAO/SInventory_Entities/DCStockNew.cs) |
| DAO/entity class | DCStoreFreezeDAO (Library.DAO/SInventory_Entities/DCStoreFreezeDAO.cs) |

## 7. UI Control Inventory
| Control ID | Label | Required | Default | Read-only / visible | Data source | Handler |
|---|---|---|---|---|---|---|
| clnNoTextBox | Chalan No | - | `tblSubDepotChalanInfo.ChalanNo` | (markup) | `SubDepotChalanBLL.GetChalanReceieve(ChalanId)` (`.cs:28-38`) | - |
| clnDateTextBox | Chalan Date | - | ChalanDate `dd-MMM-yyyy` | - | same | - |
| rcvDateTextBox | Receive Date | server: `Convert.ToDateTime` must parse | today `dd-MMM-yyyy` (:34) | - | system | - |
| driverNameTextBox / truckTextBox | Driver / Truck | - | from chalan (`DriverName`, `TrackNo`) | - | same | - |
| hdComUnitId (hidden) | - | - | `SubDepotId` (:37) | hidden | same | - |
| hdReqId (hidden) | - | - | ChalanNo | hidden | same | - |
| rcvGridView | ProductCode, ProductName, PackSize, BatchNo, Quantity, ExpDate, ReceiveDate(=ChalanDate), RcvQty, UnRcvQty, AccessQty, Mfgdate | - | - | DataKeys `SChalanId,SChalanDetailsId,DCStoreId` | joined chalan lines (`SubDepotChalanDAL.cs:176-186`) | - |
| rcvQtyTextBox | RcvQty | server: numeric (`Convert.ToDecimal`) | `Eval("Quantity")` | AutoPostBack | - | `rcvQtyTextBox_TextChanged` (:150) sets damage = sent - rcv |
| damageTextBox | UnRcvQty | server: numeric | `0` | AutoPostBack | - | `damageTextBox_TextChanged` (:139) sets rcv = sent - damage |
| accessQtyTextBox | AccessQty | - | 0 | not read by code | - | none (dead) |
| submitButton `Submit` | - | - | - | `OnClientClick="return confirm('Are you sure you want to Save ?');"` (`.aspx:128`) | - | `submitButton_Click` (:115) -> `Save()` |
| backLinkButton | Back To List | - | - | - | - | :161 |

## 8. Field Mapping
| UI control / source | DTO field (`DCStockNew`) | Handler | BLL | DAL | Table.column |
|---|---|---|---|---|---|
| cell0 ProductCode | `ProductCode` | `Save()` :55 | `SubDepotChalanBLL.DCStockIn2` (:26) | `SubDepotChalanDAL.DCStockInDALMain` :42 | `tblSubDepotStore.ProductCode` |
| cell1 | `ProductName` | :56 | same | same | `.ProductName` |
| cell2 | `PackSize` | :57 | same | same | `.PackSize` |
| cell3 | `BatchNo` | :58 | same | same | `.BatchNo` |
| cell4 (qty sent) | `TotalQuantity` | :59 | same | same | `.TotalQuantity` |
| rcvQtyTextBox | `StockQty` | :60 | same | same | `.StockQty` |
| damageTextBox | `DamageQty` | :61 | same | same | `.DamageQty` |
| cell5 | `ExpDate` | :62 | same | same | `.ExpDate` |
| cell10 | `mfgdate` | :63 | same | same | `.MfgDate` |
| cell6 (chalan date) | `ReceiveDate` | :64 | same | same | `.ReceiveDate` |
| rcvDateTextBox | `StockRcvDate` | :65 | same | same | `.StockRcvDate` |
| clnNoTextBox / clnDateTextBox | `ChalanNo`, `ChalanDate` | :66-67 | same | same | `.ChalanNo`, `.ChalanDate` |
| hdComUnitId | `ComUnitId` (= SubDepotId) | :68 | same | same | `.SubDepotId` |
| DataKeys[1] | `ChalanDetailsId` | :69 | same | same | `.SChalanDetailsId` |
| DataKeys[0] | `ChalanId` | :70 | `ChalanUpdate(ChalanId)` | `SubDepotChalanDAL.ChalanUpdate` :30 | `tblSubDepotChalanInfo.IsDeliver='True'` |
| DataKeys[2] | `DCStoreId` | :71 | same | same | `.DCStoreId` |
| generated | `SubDCStoreId` = MAX(SubDCStoreId)+1 | BLL:31 | `PrimaryKeyMax` | - | `.SubDCStoreId` |
| constant | `StockCondition` | - | - | DAL:86 | `.StockCondition='Available'` |
| damage>0: damage qty | freeze `TotalQuantity` and `StockQty`; `DamageQty`=0 | :84-86 | `SaveDCStoreFreeze2` (:37) | `SubDepotChalanDAL.SaveDCStoreFreeze2` :35 | `tblSubDepotStoreFreeze.TotalQuantity/StockQty/DamageQty` |
| SubDCStoreId returned | freeze `DCStoreId` -> col `SubDCStoreId` | :77 | same | DAL:38 | `tblSubDepotStoreFreeze.SubDCStoreId` |
| hdComUnitId | freeze `ComUnitId` -> col `SubDepotId` | :92 | same | same | `.SubDepotId` |
| constant `Restricted` | `StockCondition` | :93 | same | same | `.StockCondition` |
| DataKeys[1] | `ChalanDetailsId` | :94 | same | same | `.SChalanDetailsId` |
| generated | `SDStoreFreezeId` = MAX+1 | BLL:42 | `PrimaryKeyMax` | - | `.SDStoreFreezeId` |

## 9. Dropdown Mapping
No dropdowns. All values come from the chalan query `SubDepotChalanDAL.GetChalanReceieve` (`tblSubDepotChalanInfo` LEFT JOIN `tblSubDepot`, `tblSubDepotChalanDetail`, `tblProduct`, `tblDCStore`; `ChalanDate AS ReceiveDate`; `WHERE IsDeliver='False' AND SChalanId=<id>`).

## 10. Button Action Mapping
| Button | Handler | BLL -> DAL | SQL | DB change |
|---|---|---|---|---|
| Submit (after JS `confirm`) | `submitButton_Click` :115 -> `Save()` :46 | per line: `DCStockIn2` -> `PrimaryKeyMax("SubDCStoreId","tblSubDepotStore")`, `DCStockInDALMain`, `ChalanUpdate`; if damage>0: `SaveDCStoreFreeze2` -> `PrimaryKeyMax("SDStoreFreezeId","tblSubDepotStoreFreeze")`, INSERT | INSERT tblSubDepotStore (17 columns + 'Available'); UPDATE tblSubDepotChalanInfo SET IsDeliver='True' WHERE SChalanId; INSERT tblSubDepotStoreFreeze | 3 tables |
| Back To List | :161 | none | none | none |
After the loop: alert `Data Save Successfully!!`, `Popup(hdReqId.Value)` (report window), `Clear()`.

## 11. Frontend Flow
- `Page_Load` (first load): `LoadInfo(ChalanId)` -> header + hidden ids; `LoadGrid(ChalanId)` (two identical queries).
- `rcvQtyTextBox`/`damageTextBox` post-backs keep `rcv + damage = sent` (BR-M12-015).
- Submit: browser `confirm` -> post-back -> `Save()`; `Popup` script opens the receive report window (`window.open`, 950x700).
- No AJAX; JS only the `confirm`.

## 12. Backend Flow
| Handler | Source | Flow | try/catch | Txn |
|---|---|---|---|---|
| `Page_Load` | :19-27 | `Request.QueryString["ChalanId"]` (no validation) -> header/lines; if the chalan is not `IsDeliver='False'` the query returns no rows and `aTable.Rows[0]` throws | no | no |
| `Save` | :46-106 | for each grid row: read rcv/damage textboxes (`Convert.ToDecimal(text.Trim())`, throws on blank/non-numeric), parse dates from cell text, `DCStockIn2` (new tblSubDepotStore row + flag chalan delivered), if damage>0 `SaveDCStoreFreeze2`. Then alert, popup, `Clear()` | no | no |
| `damageTextBox_TextChanged` / `rcvQtyTextBox_TextChanged` | :139-160 | mutual recalculation | no | no |
There is no check that the chalan is still undelivered at Save (two browser tabs/users can both post), no comparison of `rcv + damage` with `sent`, and the delivered flag is written after the first line's stock row (BR-M12-017).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| SubDepotChalanBLL.GetChalanReceieve | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:62 |
| SubDepotChalanBLL.DCStockIn2 | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:27 |
| SubDepotChalanBLL.SaveDCStoreFreeze2 | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:38 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SubDepotChalanDAL.GetChalanReceieve | Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:177 | inline SQL: tblDCStore, tblProduct, tblSubDepot, tblSubDepotChalanDetail |  |
| SubDepotChalanDAL.DCStockInDALMain | Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:43 | inline SQL: tblSubDepotStore |  |
| SubDepotChalanDAL.ChalanUpdate | Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:31 | inline SQL: tblSubDepotChalanInfo |  |
| SubDepotChalanDAL.SaveDCStoreFreeze2 | Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:36 | inline SQL: tblSubDepotStoreFreeze |  |

## 16. Stored Procedure Mapping
`ExecuteAllSqlQueryByStoreProcedure(@Query)` wrapper for all INSERT/UPDATE (body not in repo - UNKNOWN). No business SP.

## 17. Database Mapping
| Table | Access | Columns | Source |
|---|---|---|---|
| tblSubDepotChalanInfo | SELECT, UPDATE | header fields; `IsDeliver='True'` | `SubDepotChalanDAL.cs:30-34,176-186` |
| tblSubDepotChalanDetail | SELECT | line fields | :176-186 |
| tblDCStore | SELECT | ExpDate, MfgDate, PackSize of the source DC batch (join by DCStoreId) | :183 |
| tblSubDepotStore | INSERT | SubDCStoreId, DCStoreId, ProductCode, ProductName, PackSize, BatchNo, TotalQuantity, ExpDate, MfgDate, ReceiveDate, ChalanNo, ChalanDate, SubDepotId, StockQty, DamageQty, StockRcvDate, SChalanDetailsId, StockCondition | `SubDepotChalanDAL.cs:42-90` |
| tblSubDepotStoreFreeze | INSERT | SDStoreFreezeId, StorageLocation(''), TotalQuantity, ProductCode, ProductName, PackSize, BatchNo, ExpDate, ReceiveDate, ChalanNo, ChalanDate, StockQty, DamageQty, StockRcvDate, StockCondition, SubDepotId, SubDCStoreId, SChalanDetailsId | `SubDepotChalanDAL.cs:35-41` |
| tblProduct, tblSubDepot | SELECT | look-up | - |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Message | BR |
|---|---|---|---|---|---|
| Submit | user confirmation | JS `confirm('Are you sure you want to Save ?')` | - | - | - |
| RcvQty / UnRcvQty | numeric | (aspx has no extender on this old page) | `Convert.ToDecimal` throws | (unhandled error) | BR-M12-015 |
| rcv + damage = sent | consistency | client post-back arithmetic only | **none** | - | BR-M12-015 |
| Chalan still pending | IsDeliver False | at load only | not at save | (IndexOutOfRange at load) | BR-M12-013/017 |
| Receive date | parseable | - | `Convert.ToDateTime` | - | - |
No message text is defined by this page except the success alert `Data Save Successfully!!`.

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M12-013 | Sub-depot receive list shows chalans of the selected sub-depot with IsDeliver='False' OR NULL (list) - the receive screen itself reloads only IsDeliver='False' (a NULL flag chalan appears in the list but the receive page cannot load it). | Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:169-186; Solution.Web/SubDepot_UI/StockReceiveBySubDepot.aspx.cs:35-51,60-71; ProductReceiveBySubDeport.aspx.cs:28-38 | ChalanLoadInReceive / GetChalanReceieve | CONFIRMED |
| BR-M12-014 | Sub-depot receive creates one NEW tblSubDepotStore row per chalan line: StockQty = received qty, TotalQuantity = quantity sent, DamageQty = damaged qty, MfgDate/ExpDate from the DC batch, StockCondition = 'Available', SubDepotId from the chalan's sub-depot, SChalanDetailsId = chalan line. No cost/valuation column is written. | Solution.Web/SubDepot_UI/ProductReceiveBySubDeport.aspx.cs:46-73; Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:26-36; Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:42-90 | ProductReceiveBySubDeport.Save -> SubDepotChalanBLL.DCStockIn2 | CONFIRMED |
| BR-M12-015 | Received qty and damaged/un-received qty textboxes drive each other on post-back (rcv = sent - damage; damage = sent - rcv). There is no server-side check that rcv + damage = sent, that values are numeric, or that neither is negative/greater than sent. | Solution.Web/SubDepot_UI/ProductReceiveBySubDeport.aspx.cs:139-160,51-61; Solution.Web/SInventory_UI/SubdeportTransferReceiveProductByChalanByDC.aspx.cs:151-172 | damageTextBox_TextChanged / rcvQtyTextBox_TextChanged | CONFIRMED |
| BR-M12-016 | Damaged/un-received qty > 0 on receive creates a freeze row (INSERT tblSubDepotStoreFreeze; StockCondition 'Restricted'; StockQty = TotalQuantity = damage qty; SubDCStoreId = the store row just created; SubDepotId = chalan sub-depot). Restricted stock is therefore excluded from `Available` sellable stock but counted in the stock report as RQty. | Solution.Web/SubDepot_UI/ProductReceiveBySubDeport.aspx.cs:74-99; Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:37-47; Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:35-41; Library.DAL/SInventory_DAL/DCStockReportDAL.cs:530-540 | SubDepotChalanBLL.SaveDCStoreFreeze2 | CONFIRMED |
| BR-M12-017 | A chalan is flagged delivered (IsDeliver='True') as a side-effect of saving the FIRST received line (ChalanUpdate is called inside DCStockIn2 for every line). There is no partial-receipt state, no cancel/reject path and no sent-vs-received reconciliation. | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:26-36; Library.BLL/SInventory_BLL/ChalanBLL.cs:36-46; Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:30-34 | DCStockIn2 / DCStockInSub | CONFIRMED |

Calculations: `RcvQty = sent - UnRcvQty`, `UnRcvQty = sent - RcvQty` (post-backs, decimal). Valuation: none (no price/cost is read or stored at receipt, BR-M12-014).

## 20. Permission Matrix
- Session keys read in page code: none. Access = menu only; the `ChalanId` query-string value is not checked against the user's DC/sub-depot (any user can open any pending chalan id). The list page (`StockReceiveBySubDepot`) restricts DCs by `UserType` (BR-M12-045) but this page does not re-check.

## 21. Status Lifecycle
Chalan state `IsDeliver`: `False` (in transit, set at creation by PAGE-450) -> `True` (received) - written by `SubDepotChalanDAL.ChalanUpdate` inside `DCStockIn2` (BR-M12-017). Stock rows created at receipt start as `Available` (received qty) and, for damage, `Restricted` in the freeze table. There is no `Rejected`/`Partially received` state and no reversal in M12 code.

## 22. Error Handling
No try/catch. Blank/non-numeric qty -> `FormatException`; missing chalan -> `IndexOutOfRangeException` (`aTable.Rows[0]`). A failure mid-loop leaves earlier lines received and the chalan already flagged delivered. Alerts through JS. `Global.asax` error handler empty.

## 23. Audit
No audit: `tblSubDepotStore` INSERT has no CreateBy/CreateDate; the receiving user is not recorded (only `StockRcvDate` = date entered). DB defaults, if any, UNKNOWN.

## 24. Integration
None. (DC side of the main B2B flow has SAP confirm procedures `sp_SAP_B2B*` - not used for sub-depot chalans.)

## 25. Reports
After save: `SubDeportTransferStockReceiveReportViewer.aspx?ChalanId=<chalanNo>` -> `rptChalanReceive.rpt` fed by `ChalanDAL.SubdeportDCStoreReport(chalanNo)` (RPT-M12-02).

## 26. Dependencies
BLL: `SubDepotChalanBLL`, `RequisitionBLL`(unused instance). DAL: `SubDepotChalanDAL`, `ClsPrimaryKeyFind`. DAO: `DCStockNew`, `DCStoreFreezeDAO`. Tables per §17. Master `MainMasterPage.master`.

## 27. Test Cases
| ID | Scenario | Steps | Expected | BR |
|---|---|---|---|---|
| TC-01 | Full receipt | open chalan (1 line, sent 10), keep defaults, Submit + OK | 1 tblSubDepotStore row StockQty 10, TotalQuantity 10, DamageQty 0, `Available`; chalan IsDeliver=True; report popup; no freeze row | BR-M12-014/017 |
| TC-02 | Damage | set UnRcvQty 2 | RcvQty auto 8; store row StockQty 8, DamageQty 2; freeze row `Restricted` StockQty 2 | BR-M12-015/016 |
| TC-03 | Rcv > sent (negative) | type RcvQty 12 | UnRcvQty becomes -2; saved without error (no validation) | BR-M12-015 |
| TC-04 | Blank qty | clear RcvQty | `FormatException` on Submit | BR-M12-015 |
| TC-05 | Re-open received chalan | open `?ChalanId=<already received>` | `IndexOutOfRangeException` (no rows) | BR-M12-013 |
| TC-06 | Double submit (edge) | two tabs open on the same pending chalan; both Submit | stock rows created twice (no server re-check) | BR-M12-017 |
| TC-07 | Multi-line partial failure | force error on line 2 | line 1 stock stored and chalan flagged delivered | BR-M12-017 |
| TC-08 | List vs detail | chalan with IsDeliver NULL | listed but page fails to load | BR-M12-013 |

## 28. Reverse Engineering References
- `Solution.Web/SubDepot_UI/ProductReceiveBySubDeport.aspx`, `Solution.Web/SubDepot_UI/ProductReceiveBySubDeport.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | `ProductReceiveBySubDeport.aspx(.cs)` | n/a | MISSING | - |
| Reconciliation sent vs received | None existed | none | - | GAP | BR-M12-015 |
| Delivered flag | None existed | after first line | - | CONFIRMED | BR-M12-017 |
| Valuation | None existed | no cost stored | - | CONFIRMED | BR-M12-014 |

## 30. Known Gaps
- No server validation (qty, reconciliation, still-pending); non-atomic multi-table writes; delivered flag set on first line; concurrency (double receive); `AccessQty` dead control; SQL by string concatenation; no audit user; chalan id unauthenticated in query string; `RequisitionBLL` field unused. Freeze row stores `DCStoreId` param name for a sub-depot store id (naming trap: `DCStoreFreezeDAO.DCStoreId` = SubDCStoreId).

## 31. Change Impact
- `SubDepotChalanBLL.DCStockIn2/SaveDCStoreFreeze2` used by this page only (and identical copies in `SubDepoAdjustBLL`). Rows created here feed: Sub-depot stock report (Available/Restricted/In-transit buckets), `SubDeportStockFreez` (release), delivery/proforma stock deduction SPs (UNKNOWN), stock-out approvals. Changing `StockCondition` literals affects `DCStockReportDAL.cs:485-540`.

