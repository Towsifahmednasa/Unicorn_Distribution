# Reverse-engineering tooling (read-only static analysis)

Python 3 scripts used to produce the inventories and Tier-B page specs. They never modify application source.

Run order (from this folder; `ROOT` is hard-coded in `gen_common.py` — edit it if the repo moves):
1. `python extract.py <repo_root> facts.json` — parses every `.aspx/.ascx/.master` and `.cs` (methods, calls, SP names, inline SQL tables/columns, control→property assignments). ~5 min.
2. `python gen_specs.py` — (re)writes Tier-B page specs; **never overwrites a file whose front matter has `tier: A`**.
3. `python gen_docs.py` — inventories (`docs/reverse-engineering/*`), `docs/database/views.md`, `relationships.md`, and generated blocks (`blocks/`) that were merged into `docs/database/stored-procedures.md` / `tables.md`.
4. `python gen_coverage.py` — `specs/SPEC-COVERAGE.md` from page front matter.
5. `python gen_final.py` — aggregate docs (business, workflows, modules, SPEC-GAPS, …) from module READMEs/GAPS.

Caveats: regex-based; dynamic controls, SQL assembled across methods and helper parameter builders can be missed. `facts.json` (≈10 MB) is not committed. Generated files that were later hand-edited (Tier A pages, module READMEs) must not be regenerated blindly — `gen_final.py` overwrites the aggregate docs listed above.
