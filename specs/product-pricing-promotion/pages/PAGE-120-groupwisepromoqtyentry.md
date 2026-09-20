---
page_id: PAGE-120
module: M04
page: GroupWisePromoQtyEntry
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "PARTIAL", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-120 - GroupWisePromoQtyEntry (allocate promotional quantity of a product to the MIOs of a promo group for a month)

> **Evidence level:** Tier A. Read in full: `PromoAlloc/GroupWisePromoQtyEntry.aspx.cs` (460 lines), `Library.DAL/PromoAllocDAL/GroupWisePromoQtyDAL.cs`, `PromoMITagDAL.LoadGroup/LoadProduct/LoadMIOByGroupId`, control and JS regions of the aspx, `GroupWisePromoQtyView.aspx.cs` (for the edit path). `sp_Save_GroupWisePromoQty` body absent => UNKNOWN.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-120 |
| Module | M04 |
| Page name | GroupWisePromoQtyEntry |
| Kind | Entry / Transaction |
| Web folder / master | PromoAlloc / NewMasterPage |
| Page class | PromoAlloc_GroupWisePromoQtyEntry |
| Files | aspx 29,016 bytes; aspx.cs 460 lines |
| Suspected legacy/duplicate | Live (save path = server `btnSave_Click`); contains dead legacy AJAX path (section 13) |

## 2. Purpose
Choose a promo group, a promotional product, a month and year, and give each ticked MIO of the group a quantity (either one common quantity for all or individual). CONFIRMED (labels: Promo Group, Product, Central stock, "Is Common QTY", Amount, month, year; grid Employee Code/Name/Territory/QTY).

## 3. Business Objective
Distribute the limited central-warehouse stock of promotional/gift items (products of product groups 2 and 3) among field officers per month while not allocating more than the warehouse holds. INFERRED (see BR-M04-064: the check is weaker than the label suggests).

## 4. Navigation
Inbound: `GroupWisePromoQtyView.aspx` / `GroupWisePromoQtyList.aspx` (New, and Edit `?MID=` which the page tolerates but cannot load, BR-M04-068). Outbound: success -> `GroupWisePromoQtyView.aspx`.

## 5. Route
`~/PromoAlloc/GroupWisePromoQtyEntry.aspx[?MID=<GWPromoQtyId>]`.

## 6. Source Files
View/code-behind; `GroupWisePromoQtyDAL`, `PromoMITagDAL` (static shared instances `aTargetDal`, `aTargetDal2`); DAO `GroupWisePromoQtyDAO`; `CommonDataLoad` (only a commented-out call).

## 7. UI Control Inventory
| Control | Label | Type | Required (server) | Notes |
|---|---|---|---|---|
| groupname | Promo Group | DropDownList AutoPostBack | YES `:182` | `tblPromoGroup`; change loads MIO grid (`LoadMIOByGroupId`) |
| ddlProduct | Product | DropDownList AutoPostBack | **no server check** (only JS validation in the dead path) | `LoadProduct`: `ProductCode+' : '+ProductName` where `ProductGroupId IN (2,3)`; change loads central stock |
| stockTextBox | Central stock | TextBox (editable!) | parsed with `Convert.ToDecimal` in validation | `SUM(tblCentralStore.Quantity)` for the product |
| ddlmonth | Month | DropDownList | - | 12 months **starting from the current month** (text = month name, value = number), current month selected |
| ddlYear | Year | DropDownList | - | 2015..2050, current year selected |
| chkQty / amount | Is Common QTY / Amount | CheckBox + TextBox (AutoPostBack) | - | copies `amount` to every row's `txtQty` (or clears all when unticked) |
| gv_List | SL, chkSelect (+ header chkSelectAll), Employee Code, Employee Name, Territory Name, QTY (`txtQty`) with hidden `hfMIOId`,`hfEmpInfoId`,`hfTerritoryId` | GridView | see BR-M04-063 | rows = MIOs tagged to the group |
| btnSave / btnUpdate | Submit / Update | LinkButton | - | **both** wired to `btnSave_Click`; Update shown when `?MID` |

## 8. Field Mapping
Per ticked row -> `GroupWisePromoQtyDAO` -> `sp_Save_GroupWisePromoQty` params: `@MIOId`, `@EmpInfoId`, `@TerritoryId`, `@ProductId` (= ddlProduct, null if index 0), `@PromoGroupId`, `@Month` (**month name text**, e.g. "September"), `@Year` (`ddlYear.SelectedIndex > 0 ? int : NULL`; the year list has no placeholder, so index 0 = 2015 is sent as NULL), `@Qty`, `@EntryBy` (= UserId). Table (from legacy inline SQL): `tblGroupWisePromoQty(GWPromoQtyId, Year, Month, PromoGroupId, ProductId, Qty, Date, EntryBy, EntryDate, UpdateBy, UpdateDate)` - note there is no MIO column in that inline SQL, whereas the SP receives MIO/employee/territory: the SP's target table(s) are UNKNOWN (per-MIO quantities are edited on the view page through `sp_Update_PromoEmployeeQty`).

## 9. Dropdown Mapping
Groups, products, months (`GetMonthList`), years (`GetYearList`) as above.

## 10. Button Action Mapping
| Event | Handler | Effect |
|---|---|---|
| groupname change | `:343` | grid <- `LoadMIOByGroupId(groupId)` (`tblPromoMIOTagDetail` join `tblMIOInfo` join `tblEmpGeneralInfo` join `tblTerritory`, `CheckStatus='False'`, Qty '') |
| ddlProduct change | `ddlProduct_OnSelectedIndexChanged` `:451` | `LoadCentralWearhouse(productId)` -> `stockTextBox` = `Qty` |
| chkQty / amount change | `:445`,`:418` -> `NewMethod` | fill / clear `txtQty` of **all** rows |
| chkSelect / chkSelectAll | `:356-416` | `CheckCustInOrder` body is fully commented out (no exclusivity check here) |
| btnSave / btnUpdate | `btnSave_Click` `:262` | `Validation()` -> build list of ticked rows -> `GroupWisePromoQtyDAL.SaveDetail` |

## 11. Frontend Flow
Server postbacks. The aspx also contains jQuery (`Save()`/`FinalSave()`/`GetData()`/`GetStockData()`) posting to the `[WebMethod]`s of this page and `PromoMIOTag.aspx/LoadProduct`; the live buttons do not call them (BR-M04-069).

## 12. Backend Flow
`btnSave_Click`: `Validation()` (BR-M04-063/064) -> loop rows, ticked ones become DTOs (`Month = ddlmonth.SelectedItem.Text`, `Year = ddlYear...`) -> `SaveDetail(list, UserId)`: one `DataAccessManager` transaction, `sp_Save_GroupWisePromoQty` per row via `SaveDataReturnPrimaryKey` (returned PK ignored), `isSuccess = true` unconditionally; exceptions rolled back and re-thrown (no page try/catch => error page). On success "Operation successful!" -> `GroupWisePromoQtyView.aspx`. The failure branch text "Already Exist!" (`:317`) is therefore unreachable in practice.

## 13. API Mapping
`[WebMethod(EnableSession=true)] public static`: `LoadStockQty(id)`, `SaveGroupWiseQty(GroupWisePromoQtyDAO)` (builds date `01-<Month>-<Year>`, inserts/updates `tblGroupWisePromoQty` by **string-concatenated inline SQL**), `LoadGroupWiseQtyById(id)` (concatenated). Called only by the page's legacy JS; no server-side authorisation beyond forms auth.

## 14. Service Mapping
No BLL.

## 15. Repository Mapping
`GroupWisePromoQtyDAL`: `LoadCentralWearhouse`, `SaveDetail`, `SaveGroupWiseQty`, `UpdateGroupWiseQty`, `LoadGroupWiseQty`, `LoadGroupWiseQtyById`, `GetList`, `Update___Info`.

## 16. Stored Procedure Mapping
| SP | Params | Body |
|---|---|---|
| sp_Save_GroupWisePromoQty | @MIOId,@EmpInfoId,@TerritoryId,@ProductId,@PromoGroupId,@Month,@Year,@Qty,@EntryBy | UNKNOWN (insert vs upsert, duplicate handling, month name vs number storage unknown) |
| sp_Get_GroupWisePromoQtyList (view page), sp_Update_PromoEmployeeQty (view page inline edit) | - | UNKNOWN |

## 17. Database Mapping
`tblCentralStore` (SELECT `SUM(Quantity)` grouped by ProductId: `GroupWisePromoQtyDAL.cs:21-25`, no date/batch/expiry/store filter); `tblGroupWisePromoQty` (legacy inline INSERT/UPDATE/SELECT); `tblPromoGroup`, `tblProduct`, `tblPromoMIOTagDetail/Master`, `tblMIOInfo`, `tblEmpGeneralInfo`, `tblTerritory` (SELECT).

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| groupname | required | `:182` | tooltip | BR-M04-063 |
| gv_List | not empty | `:190` | alert "Table can not be Empty!" | BR-M04-063 |
| ticked rows | >=1 | `:217` | alert "Please Select at least one employee !!!" | BR-M04-063 |
| stock vs allocation | `stockTextBox` < qty | `:223` | alert "Assigned stock is greater then main stock  !!!" | BR-M04-064 |
| ticked row qty | not blank | `:229-247` | tooltip | BR-M04-063 |
| product, month/year, qty > 0, duplicates | **not checked** | - | - | BR-M04-065 |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M04-063 | Save needs a promo group, a non-empty grid, at least one ticked MIO, and a quantity on every ticked row (blank => `is-invalid`). Quantities are parsed with `decimal.Parse`/`Convert.ToDecimal` (culture dependent; negatives and zero allowed). | GroupWisePromoQtyEntry.aspx.cs:176-251,283-296 | `Validation`,`btnSave_Click` | CONFIRMED |
| BR-M04-064 | **Stock guard is broken:** the summation loop `break`s as soon as `count > 0`, i.e. after the **first ticked row**, so `qty` = quantity of the first ticked MIO only. The guard `stock < qty` compares that single row with the whole central stock; the total across all ticked MIOs is never compared. Example: stock 100, five ticked MIOs x 50 => passes. `stockTextBox` is an editable box that the user can overwrite; an empty box throws FormatException. | GroupWisePromoQtyEntry.aspx.cs:198-227; aspx:184 | `Validation()` | CONFIRMED |
| BR-M04-065 | Central stock = `SUM(tblCentralStore.Quantity)` for the product across **all** central-store rows (no batch/expiry/plant filter), not net of quantities already allocated to any group/month; the same product/group/month/MIO can be allocated repeatedly because the C# layer has no duplicate check and month, year and product are not required (product null; year 2015 = index 0 => null). | GroupWisePromoQtyDAL.cs:21-25; GroupWisePromoQtyEntry.aspx.cs:289-294 | `LoadCentralWearhouse`,`btnSave_Click` | CONFIRMED |
| BR-M04-066 | Common quantity: with "Is Common QTY" ticked, typing an amount writes it into `txtQty` of **every** row (ticked or not); unticking clears every row. | GroupWisePromoQtyEntry.aspx.cs:418-449 | `NewMethod` | CONFIRMED |
| BR-M04-067 | Month/year selection: month list starts at the current month and spans 12 months (wrapping into next-year month names but with the same selected year list), year list 2015-2050. The month is sent to the SP as **name text**; the legacy WebMethod path stores a date `01-<month>-<year>`. Mixed representation risk. | GroupWisePromoQtyEntry.aspx.cs:105-135,143-148,293 | `GetMonthList`,`btnSave_Click` | CONFIRMED |
| BR-M04-068 | Edit is not implemented on this page: `?MID` only toggles `btnUpdate` visible (`GetOneRecord` is commented out), and `btnUpdate` runs the same `btnSave_Click` (a **new** allocation set is saved). Real editing is inline on `GroupWisePromoQtyView` (`EditData` makes `txtQty` writable, `UpdateData` -> `sp_Update_PromoEmployeeQty`). | GroupWisePromoQtyEntry.aspx.cs:28-38; GroupWisePromoQtyView.aspx.cs:140-199 | - | CONFIRMED |
| BR-M04-069 | Dead legacy AJAX path: `SaveGroupWiseQty`, `LoadGroupWiseQtyById`, `LoadStockQty` WebMethods with inline concatenated SQL are not reachable from the live buttons; they remain callable as public endpoints. | GroupWisePromoQtyEntry.aspx.cs:42-52,138-174; GroupWisePromoQtyDAL.cs:145-195 | - | CONFIRMED |

## 20. Permission Matrix
Session `UserId`. No role check (a commented-out `RoleTypeId=="5"` column toggle exists on the View page). Roles UNKNOWN.

## 21. Status Lifecycle
No status; allocations exist per (group, product, month, MIO). Edits are quantity-only on the view page.

## 22. Error Handling
`Validation` alerts; DAL rollback + rethrow => unhandled error page; loaders in silent try/catch; `stockTextBox` blank => uncaught FormatException.

## 23. Audit
`@EntryBy` = UserId (SP); inline legacy path writes EntryBy/EntryDate and UpdateBy/UpdateDate.

## 24. Integration
None.

## 25. Reports
None (view page lists allocations: PAGE-122).

## 26. Dependencies
DAL `GroupWisePromoQtyDAL`, `PromoMITagDAL`; DAO `GroupWisePromoQtyDAO`; `tblCentralStore` (inventory module owns it).

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Group with 5 MIOs, tick all, common qty 50, stock 100 | Guard passes (only first row compared); 250 allocated | BR-M04-064 |
| TC-02 | First ticked MIO qty 150, stock 100 | "Assigned stock is greater then main stock  !!!" | BR-M04-064 |
| TC-03 | Save without product | Saved with null product (UI/SP dependent) | BR-M04-065 |
| TC-04 | Empty stock box, Save | FormatException | BR-M04-064 |
| TC-05 | Save same data twice | Second save accepted (no C# check) | BR-M04-065 |
| TC-06 | Open `?MID=5`, click Update | New allocation rows, existing unchanged | BR-M04-068 |
| TC-07 | Untick common qty | All qty boxes cleared | BR-M04-066 |
| TC-08 | POST `SaveGroupWiseQty` with quote in Qty | SQL error/injection | GAP-M04-026 |

## 28. Reverse Engineering References
`Solution.Web/PromoAlloc/GroupWisePromoQtyEntry.aspx(.cs)`, `GroupWisePromoQtyView.aspx.cs`, `Library.DAL/PromoAllocDAL/GroupWisePromoQtyDAL.cs`, `PromoMITagDAL.cs`.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | read | SP body absent | MISSING | - |
| "Assigned stock > main stock" control | None existed | checks only first row | n/a | DEFECT | BR-M04-064 |

## 30. Known Gaps
Stock guard defect; no duplicate control; edit unimplemented; inline SQL concatenation in legacy WebMethods; editable stock box; `sp_Save_GroupWisePromoQty` body and target table unknown (GAP-M04-025).

## 31. Change Impact
`tblGroupWisePromoQty` / promo qty tables are read by `GroupWisePromoQtyView`/`List` and by MIO/target flows (`sp_CS_MIOInfo_Rpt`, `sp_Get_GroupWisePromoQtyList`). `LoadCentralWearhouse` shares `tblCentralStore` with the inventory module. Static DAL instances shared per app domain.
