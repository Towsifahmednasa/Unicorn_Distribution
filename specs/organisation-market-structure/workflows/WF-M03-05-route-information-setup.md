# WF-M03-05 — Route information setup

Traced from `MasterSetup_UI/RouteInformationEntry.aspx(.cs)` (PAGE-062, detail there), `RouteInformationList.aspx.cs` (PAGE-063), `Library.DAL/MasterSetup_DAL/RouteInformationDAL.cs`. SP bodies absent (UNKNOWN).

**Trigger/actors:** depot/logistics admin defining a delivery route. Depot choices limited to the user's `tblUserCompanyUnit` units; list page scopes non-role-5 users to `Session["DICUnitId"]` (BR-M03-067).

**Preconditions:** depot, DA master (`sp_Get_DDLDANameByDCID`), route types, weekdays, and active markets exist.

**Steps**
1. List page (`LoadData`) → `sp_Get_RouteInformationMasterList(@Parameter)`; New/Edit (`?MID=`) → entry page.
2. Entry: choose DC (reloads DAs), name, sub-depot flag; add DAs to list; add markets one at a time via structure control + distance (list sorted by distance, reorderable, total auto-summed) (BR-M03-049…062).
3. Fill total days, route type, TA, DA amounts, route weekdays.
4. Save → `Validation()` → `SaveRouteInformation`: `sp_check_RouteInformationMArket` (market already on another route?) → master save (`sp_Save_RouteInformationMaster` / `sp_UD_RouteInformationMaster`) → children `sp_Save_RouteInformationDADetail`, `…MarketDetail`, `…WeekNameDetails`; one transaction (BR-M03-063…065, 101).

**Data changed:** route master + DA, market-stop and weekday detail rows (tables UNKNOWN).
**Status:** none (exists/not); no deactivate/delete.
**Exceptions:** market on another route → message (inconsistent wording, BR-M03-064); numeric parse error unhandled; edit re-inserts children (delete semantics UNKNOWN).
**Postcondition:** redirect to list.
**Tests:** add market twice → blocked; stops 30/10/20 → order 10,20,30 and total 60; entry vs edit duplicate messages; non-admin list scoping.
