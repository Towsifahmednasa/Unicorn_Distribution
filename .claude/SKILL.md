---
name: unicorn-distribution
description: How to work on the Unicorn Distribution ERP (ASP.NET WebForms sales & distribution / field-force system on SQL Server) using the spec-driven workflow in docs/ and specs/. Use for any change, bug report, review, or documentation task in this repository.
---

# Working on Unicorn Distribution

This file tells Claude **HOW to work in this repository**. **WHAT each feature does** lives in `specs/` (one spec per page) and `docs/`. Never copy page requirements into this file; never treat this file as a spec.

> Documentation baseline: produced by reverse engineering on 2026-09-19. Before that date no `docs/`, `specs/`, `.claude/` or `database/` existed. Everything in `docs/` and `specs/` carries a confidence tag (CONFIRMED / INFERRED / UNKNOWN). **UNKNOWN means "go and look", never "assume".**

## 1. Project in one screen

| Item | Fact (source) |
|---|---|
| Kind | ASP.NET **WebForms** website (not MVC), .NET Framework 4.0, VS2010 solution `Solution.sln` |
| Projects | `Solution.Web` (aspx + code-behind + App_Code), `Library.BLL`, `Library.DAL`, `Library.DAO`, `Library.CrystalReports`; stub `MVC_TreeGrid_Template` (not in the solution) |
| Layering | aspx/code-behind → `Library.BLL` (thin) → `Library.DAL` (ADO.NET; SPs **and** inline SQL) → SQL Server `SalesDisDB_Unicorn_DB`. Some pages call DAL directly. |
| Data access | `DataAccessManager` (SP helper), `ClsCommonInternalDAL` (inline SQL helper), `ClsPrimaryKeyFind` (MAX+1 keys) in `Library.DAL` |
| Auth | Forms auth + InProc Session; menu is DB-driven (`sp_GET_MainMenuRole2` → `NewMasterPage.master.cs`); permissions = menu rows per role |
| Reports | Crystal Reports 13 (`Solution.Web/SInventory_RPTVIEW/*Viewer.aspx` load `Reports/CrystalReports/*.rpt`) |
| Front end | server-rendered pages, UpdatePanel/postbacks, jQuery/Bootstrap-era assets, per-page inline scripts; `[WebMethod]` page methods for AJAX |
| Modules | 18 inferred modules M01–M18 (`docs/reverse-engineering/module-inventory.md`); 706 `.aspx` pages |
| Database source in repo | **None** except ~41 SP bodies in root `*.sql` scripts. Schema, views, functions, triggers are not versioned. |
| Tests | None in the repository |

## 2. Where to look (reading order for any task)

1. `specs/README.md` → module → `specs/<module>/README.md` → the page spec `specs/<module>/pages/PAGE-nnn-*.md` (find ids in `docs/reverse-engineering/page-inventory.md`).
2. Business rules: the page spec §19 and `docs/business.md` (ids `BR-Mxx-nnn`); workflows in `specs/<module>/workflows/` and `docs/workflows.md`.
3. `docs/architecture.md`, `docs/coding-standard.md`, `docs/security.md`.
4. Database: `docs/database.md`, `docs/database/stored-procedures.md` (SP interface index + bodies where known), `docs/database/tables.md`, `docs/database/relationships.md`.
5. API/web methods: `docs/api.md`, `docs/reverse-engineering/api-inventory.md`.
6. Open questions: `specs/SPEC-GAPS.md`, `docs/reverse-engineering/gaps.md`. Coverage: `specs/SPEC-COVERAGE.md`.

Tier-B page specs (front matter `tier: B`) are **mechanical extractions** — good leads, not verified requirements. Tier-A specs were reviewed against code. Always re-read the source of the code you touch; a spec never replaces reading it.

## 3. Spec-driven workflow (mandatory order)

```
REQUEST → READ SPEC → READ BUSINESS RULES → READ ARCHITECTURE → READ DB/API DOCS
→ SEARCH EXISTING IMPLEMENTATION → FIND SIMILAR FEATURE → PLAN → IMPLEMENT → TEST
→ VERIFY AGAINST SPEC → SPEC-vs-ACTUAL CHECK → UPDATE DOCUMENTATION → FINAL REPORT
```

1. **Read the relevant spec(s)** and every `BR-` rule they cite. If none exists, say so and create/upgrade the spec first (see §8) — do not code from memory.
2. **Search the existing implementation** (Grep for the page/BLL/DAL/SP names in the spec; check the change-impact section §31 of the page spec and `docs/reverse-engineering/dependency-map.md`).
3. **Find the closest similar feature** (same module first; the Entry/View/Edit trio pattern is the template — see `docs/coding-standard.md`) and copy its structure and naming.
4. **Write a short plan** (files to touch, SP/table changes, rules affected, test cases) and get user confirmation for anything touching data, SPs, money/stock logic or many pages.
5. Implement in the smallest diff that satisfies the spec. No drive-by refactors, no renames, no reformatting of untouched code (the repo has 1,300+ hand-formatted files; noisy diffs destroy reviewability).
6. **Test** (§7), **verify against the spec** rule by rule, run the **spec-vs-actual check** (page spec §29) and **update the docs** (§8). End with a **final report**: what changed, files, rules touched, tests run/not run, spec updates, remaining gaps.

## 4. Conventions to follow (descriptive of this codebase — match the surroundings)

**Naming**
- Page `Foo.aspx` + `Foo.aspx.cs`, class `<Folder>_<Page>` (e.g. `SInventory_UI_CustMasterEntry`). Related trio: `XEntry` / `XView` / `XEdit`.
- Controls carry a type suffix: `xxxTextBox`, `xxxDropDownList`, `xxxLabel`, `submitButton`; message label is usually `MessageLabel`.
- BLL/DAL/DAO pairing by feature: `XBLL`/`XBll` → `XDAL`/`XDal` → entity `X` in `Library.DAO/<Area>_Entities|_DAO`. Field naming in code: `aXxxBll`, `aXxxDal` for instances.
- New code goes in the existing area folders (`SInventory_*` is the main area; `SubDepot_*` parallel stack; `DoctorModule_*`, `MasterSetup_*`, …). Do not create new top-level folders without approval.

**Database**
- Prefer stored procedures named `sp_<Verb>_<Object>` (`sp_Get_`, `sp_Save_`, `sp_Up_`, `sp_Update_`, `sp_Del_`, `sp_Report_`); tables `tbl<Name>` with `…Master`/`…Detail` pairs; views `View_<Name>`; PK usually `<Entity>Id`.
- Always parameterize (`SqlParameter`). **Never** build SQL by concatenating user input, even though legacy code does (`docs/security.md` lists offenders).
- New DB objects are delivered as **idempotent scripts** (`IF NOT EXISTS`/`CREATE OR ALTER`) named like the existing root scripts, and are **never executed by Claude**. State the script + rollback in the plan.
- Multi-DB: main DB via `DataBase.SalesDB`; accounts posting reaches `CSTL_ACCDB_Unicorn`; archive DB configured at runtime. Do not add new cross-database dependencies without approval.

**API / AJAX**
- There is no REST layer. Extend existing patterns only: `[WebMethod] public static` on the page, or `App_Code/SInventoryWebService.cs`. Validate and authorize inside the method — the platform does not.
- Document any new endpoint in `docs/api.md` and the page spec §13.

**UI / front-end**
- Follow the target page's master page (`NewMasterPage.master` is the current one; `MainMasterPage` and `MasterPage` are older) and its existing CSS/JS includes. Reuse `IVMarketStructure*.ascx` controls for hierarchy pickers.
- Server-side validation is mandatory (client validators are advisory). Show messages through the page's message label / existing notification pattern.
- Do not introduce new JS frameworks or NuGet/DLL dependencies without approval (`packages.config` + `DLL/` are pinned to .NET 4.0-era versions).

**Security (never regress)**
- Never commit or print credentials; `Web.config` / `SqlUserAccess.cs` contain live secrets — do not copy, expand, or "move" them without an explicit task. Report, don't rotate.
- Every new page must sit behind the master page auth check or add its own `Session["UserId"]` guard; every data-changing method must check the role/company-unit context (`Session["ComUnitId"]`, `UserRoleID`) the way the neighbouring pages do.
- Validate/whitelist file uploads (extension, size, path); encode output; validate redirect targets.

**Testing**
- No automated tests exist. For each change: derive test cases from the spec rules (page spec §27), run what can be run (build with MSBuild/`aspnet_compiler` if available), and list manual UI checks for the user. Say plainly what was NOT run. Prefer adding a small NUnit-style or SQL check script only if the user asks — do not add a test framework unprompted.

**Documentation**
- Docs are part of the change. Update the affected page spec (sections 19, 27, 29, 30, 31 at minimum), the module README rule index, `docs/api.md`/`docs/database/*` if interfaces changed, and `specs/SPEC-COVERAGE.md` (regenerate with `docs/reverse-engineering/tools/`). Keep confidence tags honest.

## 5. Change control

- Small, reviewable diffs; one concern per change. Touch only files the plan names.
- **Shared code is high blast radius**: `DataAccessManager`, `ClsCommonInternalDAL`, master pages, `IVMarketStructure*.ascx`, `Library.DAL` methods used by ≥3 pages (see change-impact lists), and any SP called by several pages. Ask before editing them; list every impacted page from the spec.
- Many pages exist in near-duplicate variants (`_new`, `_T`, `_final`, `Copy of …`, `Old`). Before editing, determine which variant is live (inbound links in the page spec §4, module README) and edit only that one unless told otherwise. Never delete legacy variants unasked.
- No renames of pages, classes, SPs, tables or columns without an explicit migration plan.
- Commits/PRs only when asked; follow the attribution rules given by the harness.

## 6. Database safety

- **Claude never runs DDL/DML against any database and never connects to one.** All SQL is written to script files for a human to review and run.
- Money and stock paths (`tblDCStore`, `tblCentralStore`, invoice/payment/return posting SPs, voucher posting) are high-risk: require an explicit plan, a rollback, and a before/after data check query in the script header.
- SP bodies are mostly absent from the repo: if a task needs an SP that is not in a script, **ask the user for the definition** (or `sp_helptext` output) instead of guessing its behavior.
- Existing root `*.sql` files are historical live-fix scripts with duplicated/conflicting versions (see `docs/database/stored-procedures.md`); never assume the newest file name is what is deployed.

## 7. Verification checklist (before saying "done")

- [ ] Code builds (or state why it could not be built here).
- [ ] Every rule cited in the spec (`BR-…`) still holds; new/changed rules added with source + confidence.
- [ ] Validation exists server-side; error messages consistent with neighbours.
- [ ] Permissions/session guards match the neighbouring pages; no secret leaked.
- [ ] SQL parameterized; transactions used where multiple tables change (see how the spec's chosen SP/DAL handles it).
- [ ] Impacted pages/SPs from the change-impact list re-read.
- [ ] Spec §29 (spec vs actual) rows updated; new gaps in §30 and `specs/SPEC-GAPS.md`.
- [ ] Test cases listed and run/not-run stated.

## 8. Reverse-engineering workflow (when a page/feature is not yet Tier A)

1. Read `PAGE-nnn` Tier-B spec, then the full `.aspx`, `.aspx.cs`, every BLL/DAL method it reaches, page-local JS and any SP body in scripts.
2. Rewrite the 31 sections with verified content; keep unknowns as `UNKNOWN`; add `BR-Mxx-nnn` rules (next free number in the module README) with source and confidence.
3. Set front matter `tier: A`, `generated: false`, `reviewed: <date>`, and an honest `coverage:` JSON (COMPLETE only if fully verified; database is COMPLETE only when all SQL/SP bodies were read).
4. Update the module README (page catalogue, rules index), `GAPS-Mxx.md`, and regenerate `SPEC-COVERAGE.md`.
5. Tooling in `docs/reverse-engineering/tools/` regenerates inventories and Tier-B specs and never overwrites `tier: A` files.

## 9. Conflict handling

- Spec vs code: **code is the behavioral truth, the spec is the intended truth once approved.** Never silently pick one. Record the conflict in the page spec §29 (status MISMATCH) and `SPEC-GAPS.md`, and ask the user which should win before implementing.
- Code vs database (unknown SP body): stop and ask; do not "fix" either side on a guess.
- Two docs disagree: prefer the one with the newer `reviewed:` date and CONFIRMED tags; flag the other.
- Requirement conflicts with a security or data-safety rule above: stop and ask.

## 10. Do not

- Do not modify `PrecompiledWeb/`, `packages/`, `DLL/`, `graphify-out/`, `bin/`, `obj/`, or generated Crystal `*.cs` designers.
- Do not rely on `graphify-out/` as ground truth (a cached knowledge graph; may be stale).
- Do not invent modules, roles, statuses, or table columns. Say `UNKNOWN`.
