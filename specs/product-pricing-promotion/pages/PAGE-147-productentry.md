---
page_id: PAGE-147
module: M04
page: ProductEntry
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "PARTIAL", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-147 - ProductEntry (product master: create and edit)

> **Evidence level:** Tier A. Read in full: `ProductEntry.aspx.cs` (566 lines), control/JS regions of `ProductEntry.aspx`, `Library.BLL/SInventory_BLL/ProductBLL.cs`, `Library.DAL/SInventory_DAL/ProductDAL.cs`, `CustomerInfoDAL.SaveProductDist`, `HandlerDocCV.ashx`, `ClsPrimaryKeyFind.PrimaryKeyMax`, `ClsCommonInternalDAL.UpdateDataByUpdateCommandNew`. Product master SQL is inline (fully visible); only the two DC-mapping SPs are bodyless.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-147 |
| Module | M04 |
| Page name | ProductEntry |
| Kind | Entry / Transaction (create + edit via `?ID=`) |
| Web folder / master | SInventory_UI / (uses NewMasterPage, form `enctype=multipart/form-data`) |
| Page class | SInventory_UI_ProductEntry |
| Files | aspx 25,902 bytes; aspx.cs 566 lines |
| Suspected legacy/duplicate | NO - the live editor. `ProductEdit.aspx` (PAGE-146) is a legacy pop-up editor (see section 31) |

## 2. Purpose
Create a product (item) in the master catalogue with its classification (generic group, therapeutic group, product group, product line, category, manufacturer, pack size, UOM, type, brand, shipping carton), image and the **distribution centres (DCs)** that may stock/sell it; edit an existing product. CONFIRMED.

## 3. Business Objective
The product master is the foundation for prices, campaigns, stock and orders. A product must be fully classified before it can be priced (`ProUnitPriceEntry`) or promoted. The DC list restricts availability per DC. INFERRED for the DC restriction (table `tblProductDCDetails`, consumed elsewhere).

## 4. Navigation
Inbound: `ProductView.aspx` (edit -> `ProductEntry.aspx?ID=<id>` `ProductView.aspx.cs:80`, new -> `ProductEntry.aspx`), Reset. Outbound: `ProductView.aspx` (success, Back), reference pages in new tabs (Generic Group, Therapeutic Group, Product Line, Category, Manufacturer, Pack Size, UOM, Product Type, Brand, Shipping Carton entry pages).

## 5. Route
`~/SInventory_UI/ProductEntry.aspx[?ID=<ProductId>]`.

## 6. Source Files
| Type | Path |
|---|---|
| View / code-behind | Solution.Web/SInventory_UI/ProductEntry.aspx(.cs) |
| Upload handler | Solution.Web/SInventory_UI/HandlerDocCV.ashx |
| BLL | Library.BLL/SInventory_BLL/ProductBLL.cs |
| DAL | Library.DAL/SInventory_DAL/ProductDAL.cs; Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs (`SaveProductDist`); Library.DAL/DoctorModule_DAL/SeedDataDAL.cs (`GetProLineDataTableList`, `GetDistributionCenterDataTableList`); Library.DAL/InternalCls/ClsPrimaryKeyFind.cs, ClsCommonInternalDAL.cs |
| DAO | Library.DAO/SInventory_Entities/Product.cs; ResultInfo |

## 7. UI Control Inventory
Required = server-side `Validation()` (create only; `updateButton_Click` has its validation call commented out, `:437`). Each drop-down has a placeholder at index 0 and a refresh button (`*Load_Click`) that re-binds the list after the user adds a value in the linked master page.

| Control | Label | Type | Required (create) | Source / notes |
|---|---|---|---|---|
| productCodeTextBox | Product Code | TextBox | YES `:181` "Please Input Product Code!!" | unique key (BR-M04-041) |
| productNameTextBox | Product Name | TextBox | YES `:188` | - |
| descriptionTextBox | Description | TextBox multiline | YES `:196` | - |
| ddlDistributionCenter | Distribution Center | **ListBox multiple** (select2) | no | `SeedDataDAL.GetDistributionCenterDataTableList` (`ComUnitId`,`ComUnitName`) |
| image-uploadify / hfDocFile / hfDocFileName / img_emp / btnUpload (JS) | Product Images | file input + AJAX | no | client type check + POST to `HandlerDocCV.ashx`; hidden `hfDocFile` gets the stored file name |
| ddlGenericGroup | Generic Group | DropDownList | YES `:205` "Please Input Generic Group!!" | `select * from tblGenericGroup where IsActive=1` |
| ddlTherapeutic | Therapeutic Group | DropDownList | YES `:214` | `tblTherapeuticGroup where IsActive=1` |
| ddlProTypeNew | Product Group | DropDownList AutoPostBack | YES `:222` | `tblProductGroup` (GroupId, GroupName); change => DC auto-select (BR-M04-044) |
| ddlProLine | Product Line | DropDownList | YES `:230` | `sp_GET_ProductLine` |
| categoryNameDropDownList | Category | DropDownList | YES `:239` | `tblProCategory` (via `LoadProductCategory`) |
| manufacDropDownList | Manufacturer | DropDownList | YES `:247` | `tblManufacturer` |
| packSizeDropDownList | Pack Size | DropDownList | YES `:255` | `tblPackSize` |
| stockUOMDropDownList | UOM | DropDownList | YES `:263` | `tblStockUOM where UOMSAPCode is not null` (BR-M04-045) |
| typeDropDownList | Product Type | DropDownList | YES `:270` | `tblProType` |
| productSQDropDownList | Product Brand | DropDownList | YES `:277` | `tblProductSQ` (`ProductSQName`,`ProductBrandId`) |
| shippingCartonSizeDropDownList | Shipping Carton | DropDownList | YES `:284` | `tblProductCase` (`PcsPerCase`,`CaseId`) |
| chkIsActive | Active | checkbox | no | markup `checked` |
| submitButton / updateButton / ResetBtn | Submit / Update / Reset | LinkButton | - | Submit on new, Update on edit; both confirm via `sweetAlertConfirm_*` |

## 8. Field Mapping
UI -> `Product` DTO -> `tblProduct` column (inline SQL, CONFIRMED `ProductDAL.cs:24-45`):

| UI | DTO | Column |
|---|---|---|
| (generated) `PrimaryKeyMax("ProductId","tblProduct")` | ProductId | ProductId (MAX+1, not identity) |
| productCodeTextBox | ProductCode | ProductCode |
| productNameTextBox | ProductName | ProductName |
| descriptionTextBox | Description (null if empty on create) | Description |
| packSizeDropDownList (text) | PackSize | PackSize (denormalised text) |
| categoryNameDropDownList | CategoryId (+CategoryName, not stored) | CategoryId |
| manufacDropDownList | ManufacId | ManufacId |
| stockUOMDropDownList | StockUOMId | StockUOMId |
| typeDropDownList | ProTypeId | ProTypeId |
| productSQDropDownList | ProductBrandId | ProductBrandId |
| shippingCartonSizeDropDownList | CaseId | CaseId (**insert only**) |
| packSizeDropDownList (value) | PackSizeId | PackSizeId (**insert only**) |
| ddlTherapeutic / ddlGenericGroup / ddlProTypeNew / ddlProLine | TherapueticGroupId / GenericGroupId / ProductGroupId / ProductLineID | same names |
| hfDocFile | ProductImage | ProductImage (GUID file name) |
| chkIsActive | IsActive | IsActive |
| ddlDistributionCenter (selected) | comma list `DisArray` | `tblProductDCDetails(ProductId,ComUnitId)` via SPs |

## 9. Dropdown Mapping
See section 7. All bindings via `ProductBLL.Load*` -> `ProductDAL.Load*` -> `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase(ddl, textField, valueField, sql)` (`ClsCommonInternalDAL.cs:15-40`), which sends the SQL **text** to a generic stored procedure `ExecuteAllSqlQueryByStoreProcedure(@Query)` (body UNKNOWN; effectively a remote SQL executor) and inserts the placeholder "--------Select---------" (value empty) at index 0.

## 10. Button Action Mapping
| Control | Handler | Effect |
|---|---|---|
| submitButton | `submitButton_Click1` `:294` | `Validation` -> `ProductBLL.SaveProduct` -> `CustomerInfoDAL.SaveProductDist` -> success alert -> `ProductView.aspx`; duplicate code => "Already Exist!" |
| updateButton | `updateButton_Click` `:435` | `ProductBLL.UpdateDataForProduct` -> `SaveProductDist` -> success; false => "Operation Faild!" |
| `*Load` refresh buttons | `:383-433,503` | re-bind that drop-down only |
| ddlProTypeNew change | `:522` | BR-M04-044 |
| Reset / Back | `:376`, `:366-374` | redirect |

## 11. Frontend Flow
`pageLoad()` inits select2 and `imageuploadify`. `fileValidation()` (aspx:147-172): file chosen; regex `\.(png|jpg|jpeg)$` (case-insensitive) else alert "Invalid file type". `#btnUpload` click => jQuery `$.ajax` POST of the whole `form` `FormData` to `HandlerDocCV.ashx` (progress event bound to `#fileProgress`); success writes `file.dbfilename` into `hfDocFile`, previews `../UploadFile/<name>`. Page_Load re-applies the preview when `hfDocFile` is non-empty (`:24-27`).

## 12. Backend Flow
1. `Page_Load`: set form enctype; preview; first load `LoadProductCategory()` (all lists), then `?ID` => `updateButton` visible + `ProductEditLoad(id)` else `submitButton`.
2. `ProductEditLoad` -> `ProductDAL.ProductEditLoad`: one inline SELECT of `tblProduct` plus `STUFF(... FOR XML PATH)` of `tblProductDCDetails.ComUnitId` (comma list); sets each drop-down `SelectedValue`, image, active flag, DC items.
3. Create: `ProductBLL.SaveProduct`: `HasProductName` (`select top 1 ProductId from tblProduct where ProductCode=@ProductCode`); if free: `ProductId = MAX(ProductId)+1`; `INSERT tblProduct` (parameterised, `UpdateDataByUpdateCommandNew`, own connection `SolutionConnectionStringSSIDB`, auto-commit); **returns true even if the INSERT affected 0 rows** (its bool result is ignored). Then `SaveProductDist(ProductId, DisArray)`: opens `DataAccessManager`, `sp_Delete_ProductDCDetails(@ProductId)`, then `sp_Save_ProductDCDetails(@ProductId,@ComUnitId)` per selected DC (one transaction, committed on close). Product row and DC rows are **separate transactions**.
4. Update: `UpdateProduct` (inline UPDATE, see BR-M04-042) then `SaveProductDist` (delete-all + re-insert).

## 13. API Mapping
`HandlerDocCV.ashx` (generic handler, POST multipart). No WebMethods.

## 14. Service Mapping
`ProductBLL`: `SaveProduct`, `UpdateDataForProduct`, `ProductEditLoad`, `Load*`. `ProductCodeGenerator` exists ("372"+3-digit id) but the call is commented out (`ProductBLL.cs:28`).

## 15. Repository Mapping
`ProductDAL`: `SaveDataForProduct`, `HasProductName`, `UpdateProduct`, `ProductEditLoad`, `Load*`; `CustomerInfoDAL.SaveProductDist`.

## 16. Stored Procedure Mapping
| SP | Params | Called | Body |
|---|---|---|---|
| sp_Delete_ProductDCDetails | @ProductId | `SaveProductDist` | UNKNOWN |
| sp_Save_ProductDCDetails | @ProductId, @ComUnitId | per selected DC | UNKNOWN |
| sp_GET_ProductLine | - | seed | UNKNOWN |
| sp_GET_DistributionCenter (via `GetDistributionCenterDataTableList`) | - | seed | UNKNOWN |

## 17. Database Mapping
| Table | Access | Columns (CONFIRMED) |
|---|---|---|
| tblProduct | INSERT / UPDATE / SELECT | ProductId, ProductCode, ProductName, Description, PackSize, CategoryId, ManufacId, StockUOMId, ProTypeId, ProductBrandId, CaseId, PackSizeId, TherapueticGroupId, GenericGroupId, ProductGroupId, ProductImage, ProductLineID, IsActive |
| tblProductDCDetails | via SPs; SELECT in `ProductEditLoad` | ProductId, ComUnitId |
| tblProCategory, tblManufacturer, tblPackSize, tblStockUOM, tblProType, tblProductSQ, tblProductCase, tblGenericGroup, tblTherapeuticGroup, tblProductGroup | SELECT (drop-downs) | see section 7 |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Message | BR |
|---|---|---|---|---|---|
| ProductCode, Name, Description | required | no | create `:181-202` | alerts "Please Input Product Code!!" etc. | BR-M04-040 |
| 11 drop-downs | not placeholder | no | create `:205-289` | "Please Input Generic Group!!", "...Therapeutic Group!!", "...Product Group!!", "...Product Line!!", "...Category!!", "...Manufacturer!!", "...Pack Size!!", "...  UOM!!", "...Product Type!!", "...Product Brand!!", "...Shipping Carton Size!!" | BR-M04-040 |
| ProductCode | unique | no | `HasProductName` on create only | "Already Exist!" | BR-M04-041 |
| image | png/jpg/jpeg | JS only | **none server-side** | "Invalid file type" | BR-M04-043 |
| update | **no validation at all** | - | `:437` commented | - | BR-M04-042 |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M04-040 | On create a product needs: code, name, description, generic group, therapeutic group, product group, product line, category, manufacturer, pack size, UOM, product type, brand and shipping carton; validation stops at the first failure and is server-side only. Image, DC list and Active are optional. | ProductEntry.aspx.cs:178-292 | `Validation()` | CONFIRMED |
| BR-M04-041 | Product code is the uniqueness key on create (`ProductCode` exact match, case per DB collation); name is not checked. Edit never re-checks uniqueness, so an edit can create a duplicate code. | ProductDAL.cs:70-79; ProductBLL.cs:21; ProductEntry.aspx.cs:437 | `HasProductName` | CONFIRMED |
| BR-M04-042 | **Update writes only** `IsActive, ProductName, ProductCode, Description, PackSize (text), CategoryId, ManufacId, StockUOMId, ProTypeId, ProductBrandId, ProductGroupId, GenericGroupId, TherapueticGroupId, ProductImage, ProductLineID`. It does **not** update `PackSizeId` or `CaseId`, so changing Pack Size (the text `PackSize` changes, the id stays) or Shipping Carton on an existing product is silently ignored; `PackSize` text and `PackSizeId` can then disagree. Update accepts blanks (no `Validation()`), converts drop-down values with `Convert.ToInt32` (placeholder value "" throws). | ProductDAL.cs:172-192; ProductEntry.aspx.cs:435-465 | `UpdateProduct` | CONFIRMED |
| BR-M04-043 | Image upload: only the browser checks extension (png/jpg/jpeg). The handler stores **any** posted file under `~/UploadFile/<Guid N>Product_<original extension>` and returns the name; no size/type/content/authentication check in the handler; the product stores the file name only. | ProductEntry.aspx:147-172; HandlerDocCV.ashx | JS + handler | CONFIRMED |
| BR-M04-044 | Selecting Product Group value `1` in `ddlProTypeNew` (AutoPostBack) selects **every** DC in the list (all-DC product); any other group value **rebinds** the DC list, clearing the current selection (also on edit). Group id 1 is a hard-coded magic value (meaning of group 1 UNKNOWN; INFERRED "all-DC / common product"). Group ids 2 and 3 are treated as promotional products elsewhere (`PromoMITagDAL.LoadProduct`, BR-M04-060). | ProductEntry.aspx.cs:522-565; PromoMITagDAL.cs:158-159 | `ddlProTypeNew_SelectedIndexChanged` | CONFIRMED (code) / INFERRED (meaning) |
| BR-M04-045 | UOM list offers only units that have a SAP code (`UOMSAPCode is not null`), i.e. a product cannot be created with a UOM unmapped to SAP. Because `StockUOMEntry` inserts only `(StockUOMId, StockUOMName)` (`StockUOMDAL.cs:17`) and no C# code writes `UOMSAPCode` (grep), a UOM created in the application is **invisible in this list until a DBA sets `UOMSAPCode` manually**; the code is consumed by the SAP challan SP (`SP_NewStockTransferDcToDc_Related.sql:392`). Therapeutic/generic lists show active rows only. | ProductDAL.cs:216-220,236-248 | `LoadStockUOM` etc. | CONFIRMED |
| BR-M04-046 | DC availability is replaced wholesale on every save: delete all `tblProductDCDetails` rows of the product, insert the selected DCs; an empty selection leaves the product with no DC rows. On create a failed DC save cannot roll back the product insert (separate transactions). | CustomerInfoDAL.cs:205-256; ProductEntry.aspx.cs:326-349 | `SaveProductDist` | CONFIRMED |
| BR-M04-047 | `ProductId` is `MAX(ProductId)+1` computed in a separate query before the INSERT (no identity/lock): concurrent creations can collide (PK violation) or return "success" after a failed insert because the insert result is ignored. | ProductBLL.cs:27-30; ClsPrimaryKeyFind.cs:29-45; ProductDAL.cs:47 | `SaveProduct` | CONFIRMED |
| BR-M04-048 | No inactivation guard: unticking Active only sets `IsActive=0`; there is no check for existing stock, price, campaign or order references. List page shows badge Active/Inactive (`LoadProduct`: `case when IsActive=1`). | ProductDAL.cs:172,66-75 | - | CONFIRMED |

## 20. Permission Matrix
No Session read by this page code except through the master page; `HandlerDocCV.ashx` performs no session check. Menu-based only. Roles UNKNOWN.

## 21. Status Lifecycle
`IsActive` bit only (create: checkbox default true; edit: toggle). No delete function on the page or in `ProductDAL`. Referencing tables are not checked.

## 22. Error Handling
`SaveProduct` rethrows exceptions (`catch (Exception ex) { throw ex; }`, loses stack trace); page has no try/catch around save => default error page. Drop-down loaders in try/catch with empty handlers (`:114-150`).

## 23. Audit
None: `tblProduct` insert/update carries no EntryBy/UpdateBy/date columns (CONFIRMED from the SQL). Image files remain on disk when replaced.

## 24. Integration
`StockUOMId`/`UOMSAPCode` and DC mapping feed SAP-related flows (INFERRED; other modules).

## 25. Reports
None here. Product master listing: `ProductView.aspx` (PAGE-152).

## 26. Dependencies
BLL `ProductBLL`; DAL `ProductDAL`, `CustomerInfoDAL`, `SeedDataDAL`, `ClsCommonInternalDAL`, `ClsPrimaryKeyFind`; DAO `Product`; jQuery, select2, imageuploadify, sweetalert; HandlerDocCV.ashx; writable `~/UploadFile/` folder.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Create with all required fields, 2 DCs | 1 `tblProduct` row (max+1), 2 DC rows | BR-M04-040/046/047 |
| TC-02 | Create with an existing code | "Already Exist!" | BR-M04-041 |
| TC-03 | Create with placeholder UOM | "Please Input  UOM!!" | BR-M04-040 |
| TC-04 | Edit: change Pack Size and Shipping Carton, save | Both unchanged in DB; PackSize text changed | BR-M04-042 |
| TC-05 | Edit: change code to another product's code | Saved (duplicate) | BR-M04-041 |
| TC-06 | Product Group = 1 | All DCs selected | BR-M04-044 |
| TC-07 | Edit then change Product Group to non-1 | DC selection cleared | BR-M04-044 |
| TC-08 | POST a .aspx file to `HandlerDocCV.ashx` | Stored under UploadFile (server does not reject) | BR-M04-043 |
| TC-09 | Create, DC SP failure | Product exists without DCs | BR-M04-046 |
| TC-10 | Untick Active | IsActive=0, no reference check | BR-M04-048 |

## 28. Reverse Engineering References
`Solution.Web/SInventory_UI/ProductEntry.aspx(.cs)`, `HandlerDocCV.ashx`, `Library.BLL/SInventory_BLL/ProductBLL.cs`, `Library.DAL/SInventory_DAL/ProductDAL.cs`, `Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs:205-256`.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | read | inline SQL visible | MISSING | - |
| Edit vs create parity | None existed | edit skips validation and 2 columns | n/a | DIVERGENT | BR-M04-042 |

## 30. Known Gaps
Update ignores PackSizeId/CaseId (GAP-M04-017). Unrestricted file upload (GAP-M04-018). MAX+1 key (GAP-M04-019). No audit columns / no delete / no reference check. Save not atomic across product and DC rows. Magic product-group ids 1/2/3.

## 31. Change Impact
`tblProduct` is read by nearly every module (orders, invoices, stock, reports). `ProductDAL.LoadProduct` feeds `ProductView`. `ProductEdit.aspx` (PAGE-146) also calls `UpdateProduct`; the only references to `ProductEdit.aspx` are copy-pasted private `PopUp(string Id)` helpers in `Reports_UI/OrganogramReport.aspx.cs:89`, `SInventory_UI/CustomerList.aspx.cs:61`, `CustomerTagChangeList.aspx.cs:59`, `MIGOList.aspx.cs:43` and `ProductView.aspx.cs:69` (no call site found for the ones checked; INFERRED dead). `SaveProductDist` also exists as a copy in `Library.DAL/Transfer_DAL/MarketStructureTransferDAL.cs:582`.
