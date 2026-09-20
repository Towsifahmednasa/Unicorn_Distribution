# WF-M05-08 — Legacy (SAP-era) customer maintenance: entry, edit, bulk reassign, fixed/regular flag, category

**Scope:** the `SInventory_UI` customer pages that predate `MasterSetup_UI/CustomerEntry`: `CustMasterEntry` (PAGE-186), `CustMasterEdit` (PAGE-185), `CustomerMasterEdit` (PAGE-194), `CustomerMasterView`/`CustMasterView`/`ActiveCustomerMasterList` (PAGE-196/187/182), `MultiCustomerEdit` (PAGE-201), `FixedCustomerUpload` (PAGE-200), `FixedCustomerInformation` (PAGE-199), `CustomerCategoryEntry`/`CustCategoryEdit`/`CustCategoryView` (PAGE-188/183/184). They write the **code-based** columns of `tblCustMaster` (MarketCode, MIACode, AreaCode, DisCode, ComUnitCode, RegionCode, …) instead of ids. Evidence: page code-behinds, `CustomerMasterInfoBll/Dal`, `MultiCustomerEditDAL`, `FixedCustomerUploadDal`, `CustomerCategoryBLL/DAL`. Bodies of SPs `UD_CustomerMaster`, `sp_I_FixedCustomer`, `sp_UD_FixedCustomer`, `sp_UD_RegularCustomer` are not in the repo (`sp_UD_CustomerMaster` is — see mismatch below).

## Trigger & actors
Legacy back-office users maintaining SAP-numbered customers; any user with the page in the menu (no role checks; only `CustomerMasterReport` reads `Session["UserType"]`).

## Steps
1. **Category** — `CustomerCategoryEntry`: name required ("Please input data in all Textbox"); `CustomerCategoryBLL.SaveDataForCustomerCategory`: reject if same name exists (`select top 1 … where CategoryName=@…`) → "CustomerCategory Name already exist"; else `CategoryId = MAX+1`, `CategoryCode = "CUS-" + id zero-padded to 3 digits`, INSERT `tblCustCategory`. Edit page updates name only (`UPDATE tblCustCategory SET CategoryName`). (BR-M05-055)
2. **New customer** — PAGE-186 flow (BR-M05-014…018): manual code, uniqueness by code, max-id PK, INSERT of code-based columns.
3. **Edit customer** — PAGE-185 (`CustMasterEdit`, via `CustomerMasterInfoBll.UpdateDataForCustomerMaster` → `CustomerMasterInfoDal.UpdateCustomerMasterInfo` → proc **`UD_CustomerMaster`**) and PAGE-194 (`CustomerMasterEdit`, `CustomerMasterDAL` → proc **`UD_CustomerMaster2`**). Same validation list as entry. Edit guards differ between the twins: PAGE-194 refuses the update with "Delivery Pending !!" when the customer has an invoice with `TpTotal>0 AND DeliveryInvoiceStatus IS NULL`, and with "Payment Pending !!" when it has an invoice with `PaymentStatus IS NULL OR 'Partial'` and `DeliveryTpTotal>0` (`CustomerMasterEdit.aspx.cs:192-223`; the open-order guard is commented out), whereas PAGE-185 has **all** guards commented out (`CustMasterEdit.aspx.cs:245-277`) although the queries still run — so the same edit is blocked on one page and allowed on the other. The repo defines `sp_UD_CustomerMaster` (sets ConPerson, FixedCustomer, IsActive, CategoryId, name, address, cell, address2, city, shipping condition, market/MIA/area/dis/FE/DC/region/DZSM names+codes, term of payment, tagged sales persons) — the C# calls `UD_CustomerMaster` (no `sp_` prefix); `UpdateAction` swallows exceptions and returns false, so if the proc does not exist the UI just shows "Data Not Update!!!". (BR-M05-058; GAP-M05-019)
4. **Find** — `CustomerMasterView` / `ActiveCustomerMasterList` / `CustMasterView` / `FixedCustomerInformation`: filter by code-based dropdowns via string-built WHERE on `View_CustomerForModification` / `View_CustomerMaster`; open entry/edit; report buttons open Crystal exports (RPT-M05-01) or the fixed-customer sales report.
5. **Bulk reassign** — `MultiCustomerEdit` (PAGE-201): filter customers by Region/DC/District/Area/MIA/Market codes (`View_CustomerMaster`, whitelist + parameterised via `FragmentValueRegex`), tick rows, choose new codes, **Replace**: for each ticked customer, if `tblInvoice` has any row for it, skip (collect code) else `UPDATE tblCustMaster SET <whitelisted columns>` (RegionCode, ComUnitCode/Name, DisCode, AreaCode, MIACode, MarketCode/Name). Message: "All Data Updated" or "Data Updated,Customer Code <list> is not updated". (BR-M05-056)
6. **Fixed / regular flag** — `FixedCustomerUpload` (PAGE-200): Excel with customer codes in first column (same OLE-DB reader as PAGE-192), required: no blank code, document date, ≥1 row. **Submit**: per row `sp_I_FixedCustomer(@date,@code)` (returns id via `SaveAction`, errors swallowed) and if id>0 `sp_UD_FixedCustomer(@code)`. **Regular** button: per row `sp_UD_RegularCustomer(@code)` with **no validation**; success message shown if the last row succeeded. (BR-M05-057)

## Data created / changed
`tblCustMaster` (insert/update code-based columns, FixedCustomer, IsActive/InActiveDate), `tblCustCategory`, fixed-customer register table (via `sp_I_FixedCustomer`, UNKNOWN name).

## Branches / exceptions
- Duplicate code ⇒ blocked on entry only; the equivalent check in the edit path is commented out (`CustomerMasterInfoBll.cs:130-141`).
- Customers with invoices are protected only in bulk reassign.
- Legacy inserts leave `ActionStatus/CreateBy/MarketId/ids` empty (BR-M05-018) so they do not appear correctly in modern lists.

## Postconditions
Legacy views (`View_CustomerMaster`, `View_CustomerForModification`) reflect changes; modern list `CustomerView` may miss rows lacking `CreateBy/MarketId` joins (INFERRED from LEFT JOINs).

## Related BRs
BR-M05-014 … 018, 055 … 058.

## Related pages
PAGE-182, 183, 184, 185, 186, 187, 188, 194, 196, 199, 200, 201.

## Tests
1. Category "A" twice ⇒ second rejected; codes `CUS-001`, `CUS-002`. 2. Bulk reassign 3 customers where 1 has an invoice ⇒ 2 updated, message lists the third. 3. Fixed upload with blank code ⇒ "Field Cannot be Blank !!". 4. Edit a customer with an undelivered invoice: PAGE-194 ⇒ "Delivery Pending !!"; PAGE-185 ⇒ saved (guards disabled). 5. Edit with missing proc `UD_CustomerMaster` ⇒ "Data Not Update!!!".
