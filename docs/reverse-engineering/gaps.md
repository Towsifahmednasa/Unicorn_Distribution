# Reverse-Engineering Gaps

Documentation-level gaps of this reverse-engineering baseline (application gaps are in `specs/SPEC-GAPS.md`).

| ID | Area | Gap | Effect | How to close |
|---|---|---|---|---|
| RE-01 | Database | No schema/DDL, only ~41 SP bodies of ~1,034 referenced names | Table/column/validation/transaction facts for most writes are UNKNOWN | Script the live DB (`sys.sql_modules`, tables, FKs, indexes, views, functions incl. `dbo.MainMenu2`, triggers) into a `database/` folder and re-run inventories |
| RE-02 | Pages | 541 Tier-B pages not reviewed | Purpose, rules, statuses unknown | Upgrade in module priority order (see `.claude/SKILL.md` §8) |
| RE-03 | Menu/roles | Menu/role data live in DB | Module inference by name; permission matrix per role UNKNOWN | Export `tblMainMenu`, `tblMenuDistribution`, role tables |
| RE-04 | Extraction | Regex-based static analysis: dynamic controls, SQL assembled across methods, and helper builders can be missed/mis-attributed; table names from inline SQL only | Tier-B tables/columns are lower bounds | Verify per page when upgrading |
| RE-05 | Reports | `.rpt` internals (formulas, subreports, SP inside) not opened | Report calculation detail UNKNOWN | Open in Crystal Reports designer |
| RE-06 | Runtime | Nothing was executed (no build, no DB) | Behaviour "inferred" items untested | Run the manual test cases per module |
| RE-07 | Static assets | JS libs, CSS, `PrecompiledWeb`, `packages/`, `graphify-out/` not analysed | Out of scope | — |
| RE-08 | MVC template | `MVC_TreeGrid_Template` not in solution; only stub controller documented | — | — |
