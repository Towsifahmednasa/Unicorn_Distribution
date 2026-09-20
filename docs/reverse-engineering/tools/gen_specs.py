"""Generate Tier-B (mechanically extracted) page specifications. Tier-A pages are enriched later by reviewers."""
import sys, os, re, json, collections
from gen_common import *

INPUT_TYPES = {'TextBox', 'DropDownList', 'ListBox', 'CheckBox', 'CheckBoxList', 'RadioButton', 'RadioButtonList', 'FileUpload', 'HiddenField', 'Calendar', 'TextArea'}
BTN_TYPES = {'Button', 'LinkButton', 'ImageButton'}
GRID_TYPES = {'GridView', 'Repeater', 'ListView', 'DataList', 'DetailsView', 'FormView'}
VALID_TYPES = {'RequiredFieldValidator', 'RegularExpressionValidator', 'CompareValidator', 'RangeValidator', 'CustomValidator', 'ValidationSummary'}
WRITE_SP_RE = re.compile(r'(?i)(save|insert|update|delete|del_|up_|post|create|approve|reject|process|rollback|adjust|remove|add|confirm|cancel|reverse|generate)')

def analyze(p):
    a = {'handlers': [], 'reach': [], 'own_sql_tables_r': set(), 'own_sql_tables_w': set(), 'sps': set(), 'tr': set(), 'tw': set(),
         'bll': set(), 'dal': set(), 'sess': set(), 'msgs': [], 'reports': set(), 'redirects': set(), 'colmap': [], 'wm': [], 'inserts': [],
         'string_sql': [], 'dao': set(), 'direct_dal': False, 'txn': False, 'try': 0}
    if not p['cs']:
        return a
    c = F['cs'][p['cs']]
    traces = page_traces(p)
    for m in c['methods']:
        nodes = traces.get((m['name'], m['line']), [])
        h = {'m': m, 'nodes': nodes, 'sps': set(), 'tr': set(), 'tw': set(), 'bll': [], 'dal': [], 'colmap': [], 'param_prop': [], 'ddl': [], 'dao': set()}
        for n in nodes:
            mm = n['m']
            h['sps'].update(mm['sps'])
            h['tr'].update(mm['tread']); h['tw'].update(mm['twrite'])
            h['colmap'] += [tuple(x) + (n['path'], n['cls'], n['name']) for x in mm['colmap']]
            h['param_prop'] += [(x[0], x[1], n['cls'], n['name'], n['path']) for x in mm['param_prop']]
            h['ddl'] += [tuple(x) + (n['cls'], n['name']) for x in mm['ddl_loads']]
            if n['layer'].startswith('Service'): h['bll'].append(n)
            if n['layer'].startswith('Repository'): h['dal'].append(n)
            for nw in mm['news']:
                if nw in dao_props: h['dao'].add(nw)
        a['handlers'].append(h)
        a['sps'] |= h['sps']; a['tr'] |= h['tr']; a['tw'] |= h['tw']
        a['bll'] |= {n['cls'] for n in h['bll']}; a['dal'] |= {n['cls'] for n in h['dal']}
        a['dao'] |= h['dao']
        a['sess'] |= set(m['sess'])
        a['msgs'] += [(m['name'], x) for x in m['messages']]
        a['reports'] |= set(m['reports'])
        a['redirects'] |= set(m['redirects'])
        a['txn'] |= any(n['m']['txn'] for n in nodes)
        if m['webmethod']:
            a['wm'].append(m)
        if m['tables'] and not m['sqlparams'] and m['controls']:
            a['string_sql'].append(m['name'])
        if m['calls'] and any(F['cs'][p['cs']]['vars'].get(v, '').endswith(('DAL', 'Dal')) for v, _ in m['calls']):
            a['direct_dal'] = True
        for n in nodes:
            a['sess'] |= set(n['m']['sess'])
    return a

def fld(v):
    return v if v else 'UNKNOWN'

def ctl_label(c):
    at = c['attrs']
    return at.get('Text') or at.get('HeaderText') or at.get('placeholder') or at.get('title') or at.get('value') or ''

def summarize_chain(h):
    """One-line chain for a handler."""
    parts = [f"{h['m']['class']}.{h['m']['name']}"]
    if h['bll']: parts.append(' / '.join(sorted({f"{n['cls']}.{n['name']}" for n in h['bll']}))[:200])
    if h['dal']: parts.append(' / '.join(sorted({f"{n['cls']}.{n['name']}" for n in h['dal']}))[:200])
    if h['sps']: parts.append('SP: ' + ', '.join(sorted(h['sps']))[:200])
    return ' → '.join(parts)

def gen_page(p, a, tier='B'):
    mod = MOD[p['module']]
    cs = F['cs'].get(p['cs']) if p['cs'] else None
    ctrls = p['controls']
    by_id = {c['id']: c for c in ctrls if c['id']}
    validators = collections.defaultdict(list)
    for c in ctrls:
        if c['tag'] in VALID_TYPES and c['attrs'].get('ControlToValidate'):
            validators[c['attrs']['ControlToValidate']].append(c)
    handler_by_name = {h['m']['name']: h for h in a['handlers']}
    L = []
    W = L.append
    has_js = bool(p['js_len'] or p['js_functions'])
    cov = {
        'ui': 'PARTIAL' if ctrls else 'UNKNOWN',
        'frontend': 'PARTIAL' if has_js or any(c['events'].get('OnClientClick') or c['attrs'].get('onclick') for c in ctrls) else 'COMPLETE',
        'backend': 'PARTIAL' if cs else 'MISSING',
        'api': 'PARTIAL' if (a['wm'] or p['js_ajax'] or p['js_pagemethods']) else 'COMPLETE',
        'database': 'PARTIAL' if (a['sps'] or a['tr'] or a['tw']) else 'UNKNOWN',
        'rules': 'UNKNOWN', 'permission': 'PARTIAL' if a['sess'] else 'UNKNOWN', 'status': 'UNKNOWN', 'tests': 'MISSING'}
    W('---')
    W(f"page_id: {p['id']}")
    W(f"module: {mod[0]}")
    W(f"page: {p['name']}")
    W(f"tier: {tier}")
    W("generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A")
    W("coverage: " + json.dumps(cov))
    W('---')
    W(f"# {p['id']} — {p['name']}")
    W('')
    W('> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. '
      'Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.')
    W('')
    W('## 1. Page Information')
    W(table(['Attribute', 'Value'], [
        ['Page ID', p['id']], ['Module', f"{mod[0]} — {mod[2]} (INFERRED from folder + file name)"], ['Page name', p['name']], ['Kind', p['kind'] + ' (INFERRED from name)'],
        ['Web folder', p['folder']], ['Master page', p['dattrs'].get('MasterPageFile', 'none (stand-alone HTML page)')],
        ['Page class', p['dattrs'].get('Inherits', 'UNKNOWN')], ['aspx size', f"{p['size']} bytes"],
        ['Code-behind LOC', cs['loc'] if cs else 'n/a'], ['Inline script size', f"{p['js_len']} chars"],
        ['Suspected legacy/duplicate', 'YES (name pattern; INFERRED)' if LEGACY_RE.search(p['name']) else 'no evidence']]))
    W('')
    W('## 2. Purpose')
    t = p['html_title'] or (p['headings'][0] if p['headings'] else '') or p['dattrs'].get('Title', '')
    W(f"INFERRED from name/markup: **{p['name']}** ({p['kind']}). Page title/heading text: `{t or 'none'}`. Detailed purpose: UNKNOWN (Tier B).")
    W('')
    W('## 3. Business Objective')
    W(f"UNKNOWN for Tier B. Module context: {mod[3]}")
    W('')
    W('## 4. Navigation')
    inb = sorted(NAV_IN.get(p['path'], []))
    outb = sorted(NAV_OUT.get(p['path'], []))
    W('Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.')
    W('')
    W('Inbound references (other pages whose code/markup mention this page): ' + (', '.join(f"`{x.split('Solution.Web/')[1]}`" for x in inb[:12]) + (' …' if len(inb) > 12 else '') if inb else 'none found'))
    W('')
    W('Outbound references: ' + (', '.join(f"`{x}`" for x in outb[:12]) if outb else 'none found'))
    W('')
    W('## 5. Route')
    W(f"`{route_of(p)}` (file-based WebForms route; no `MapPageRoute` alias" + (" except `user-records`" if p['name'] == 'UserRecords' else '') + ').')
    W('')
    W('## 6. Source Files')
    rows = [['View', p['path']]]
    if p['cs']: rows.append(['Code-behind', p['cs']])
    if p['dattrs'].get('MasterPageFile'): rows.append(['Master', 'Solution.Web/' + p['dattrs']['MasterPageFile'].lstrip('~/')])
    for r in p['registers']:
        if r.get('Src'): rows.append(['User control', r['Src']])
    for s in p['script_src'][:10]: rows.append(['JS include', s])
    for s in p['link_css'][:6]: rows.append(['CSS include', s])
    for cl in sorted(a['bll']): rows.append(['BLL class', f"{cl} ({file_of_class.get(cl, '?')})"])
    for cl in sorted(a['dal']): rows.append(['DAL class', f"{cl} ({file_of_class.get(cl, '?')})"])
    for cl in sorted(a['dao'])[:12]: rows.append(['DAO/entity class', f"{cl} ({file_of_class.get(cl, '?')})"])
    for r in sorted(a['reports']): rows.append(['Report (.rpt)', r])
    W(table(['Type', 'Path'], rows))
    W('')
    # ---- 7 UI controls
    W('## 7. UI Control Inventory')
    rows = []
    shown = 0
    for c in ctrls:
        tg = c['tag']
        if tg in VALID_TYPES or tg in ('form', 'table', 'img', 'iframe', 'ScriptManager', 'UpdatePanel', 'Content', 'ContentPlaceHolder', 'Panel', 'View', 'MultiView', 'PlaceHolder', 'Literal'):
            continue
        if tg == 'a' and not c['attrs'].get('href'): continue
        if not c['id'] and tg not in ('BoundField', 'TemplateField', 'a'): continue
        vs = validators.get(c['id'], [])
        req = 'yes (' + ','.join(v['tag'].replace('Validator', '') for v in vs) + ')' if vs else ('yes (html)' if 'required' in c['attrs'] else 'no validator found')
        at = c['attrs']
        state = []
        for k in ('ReadOnly', 'readonly', 'Enabled', 'disabled', 'Visible'):
            if k in at: state.append(f"{k}={at[k]}")
        ds = ' '.join(f"{k}={at[k]}" for k in ('DataSourceID', 'DataTextField', 'DataValueField') if k in at)
        evs = ', '.join(f"{k}→{v}" for k, v in c['events'].items())
        rows.append([c['id'] or '(no id)', tg, ctl_label(c), req, at.get('SelectedValue') or at.get('Checked') or at.get('InitialValue') or '', ' '.join(state), ds or ('see §9' if tg == 'DropDownList' else ''), evs or ''])
        shown += 1
        if shown >= 150: break
    if rows:
        W(table(['Control ID', 'Type', 'Label / Text', 'Required', 'Default', 'Read-only / Enabled / Visible', 'Data source', 'Events → handler'], rows))
        if len(ctrls) > 150: W(f"\n_Truncated at 150 rows; {len(ctrls)} controls parsed._")
    else:
        W('No server/HTML controls with an ID were extracted (viewer-only or code-generated page).')
    W('')
    W('Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.')
    W('')
    # ---- 8 field mapping
    W('## 8. Field Mapping')
    fm = []
    for h in a['handlers']:
        m = h['m']
        for tgt, ctl, member in m['assigns']:
            if ctl not in by_id and not any(ctl == c['id'] for c in ctrls):
                continue
            dto = next((d for d in sorted(h['dao']) if any(pr[1] == tgt for pr in dao_props.get(d, []))), '(local variable)')
            pp = [x for x in h['param_prop'] if x[1].split('.')[-1] == tgt]
            if pp:
                for (sp_param, prop, dcls, dmeth, dpath) in pp[:2]:
                    cm = [x for x in h['colmap'] if x[2] == sp_param and x[4] == dpath and x[6] == dmeth]
                    tbl = ', '.join(sorted({x[0] for x in cm})) if cm else 'UNKNOWN (SP body not in repo)'
                    col = ', '.join(sorted({x[1] for x in cm})) or 'UNKNOWN'
                    svc = ', '.join(sorted({f"{n['cls']}.{n['name']}" for n in h['bll']}))[:120] or '(none — page calls DAL directly)'
                    fm.append([ctl, f"{dto}.{tgt}" if dto != '(local variable)' else tgt, f"{m['class']}.{m['name']}", svc, f"{dcls}.{dmeth}", sp_param, tbl, col])
            else:
                fm.append([ctl, f"{dto}.{tgt}" if dto != '(local variable)' else tgt, f"{m['class']}.{m['name']}", ', '.join(sorted({f"{n['cls']}.{n['name']}" for n in h['bll']}))[:120] or '—', ', '.join(sorted({f"{n['cls']}.{n['name']}" for n in h['dal']}))[:120] or '—', 'UNKNOWN', 'UNKNOWN', 'UNKNOWN'])
    if fm:
        seen = set(); uniq = []
        for r in fm:
            k = tuple(r)
            if k not in seen:
                seen.add(k); uniq.append(r)
        W(table(['UI Control', 'DTO.Property / variable', 'Controller (handler)', 'Service (BLL)', 'Repository (DAL)', 'SP / SQL parameter', 'Table', 'Column'], uniq[:120]))
        W('\nMapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.')
    else:
        W('No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.')
    W('')
    # ---- 9 dropdown mapping
    W('## 9. Dropdown Mapping')
    dd = [c for c in ctrls if c['tag'] in ('DropDownList', 'ListBox', 'CheckBoxList', 'RadioButtonList') or c['tag'] == 'select']
    rows = []
    for c in dd:
        src = []
        for h in a['handlers']:
            for var, meth, ids in h['m']['ctl_args']:
                if c['id'] in ids:
                    src.append((h, var, meth))
        if src:
            h, var, meth = src[0]
            tv = F['cs'][p['cs']]['vars'].get(var, var)
            tgt_idx = [i for i, n in enumerate(h['nodes']) if n['name'] == meth and n['layer'] != 'Page code-behind']
            def in_sub(i):
                while i is not None:
                    if i in tgt_idx: return True
                    i = h['nodes'][i]['parent']
                return False
            sub = [n for i, n in enumerate(h['nodes']) if in_sub(i)]
            sp = sorted({s for n in sub for s in n['m']['sps']})
            trd = sorted({t for n in sub for t in n['m']['tread']})
            ddls = [x for n in sub for x in n['m']['ddl_loads']]
            disp = ', '.join(sorted({f"{x[1]}/{x[2]}" for x in ddls}))[:80] or 'UNKNOWN'
            srcs = ', '.join(sp) if sp else ('inline SQL' if trd else 'UNKNOWN')
            rows.append([c['id'], f"{h['m']['name']} → {tv}.{meth}", srcs, disp, ', '.join(trd) or 'UNKNOWN'])
        else:
            rows.append([c['id'], 'no loader call found passing this control (static ListItems or bound indirectly)', 'UNKNOWN', c['attrs'].get('DataTextField', 'UNKNOWN') + '/' + c['attrs'].get('DataValueField', 'UNKNOWN'), 'UNKNOWN'])
    W(table(['Dropdown', 'Loader (handler → BLL/DAL call)', 'SP / SQL source', 'Display / Value field', 'Table(s) read'], rows) if rows else 'No dropdown controls.')
    W('')
    # ---- 10 button mapping
    W('## 10. Button Action Mapping')
    rows = []
    for c in ctrls:
        if c['tag'] in BTN_TYPES or (c['tag'] == 'input' and c['attrs'].get('type') in ('button', 'submit', 'image')) or c['tag'] == 'button' or ('OnClick' in c['events'] and c['tag'] not in INPUT_TYPES):
            hn = c['events'].get('OnClick') or c['events'].get('OnCommand') or ''
            h = handler_by_name.get(hn)
            js = c['events'].get('OnClientClick') or c['attrs'].get('onclick') or ''
            if h:
                svc = ', '.join(sorted({f"{n['cls']}.{n['name']}" for n in h['bll']}))[:100] or '—'
                rep = ', '.join(sorted({f"{n['cls']}.{n['name']}" for n in h['dal']}))[:100] or '—'
                dbc = []
                if h['tw']: dbc.append('writes ' + ', '.join(sorted(h['tw'])[:6]))
                elif h['sps']: dbc.append('SP executed — effect on tables UNKNOWN (body not in repo)' + ('; write-like name' if any(WRITE_SP_RE.search(s) for s in h['sps']) else ''))
                rows.append([c['id'] or ctl_label(c), c['tag'] + ' click', js or '(postback)', f"{h['m']['class']}.{hn}", f"{p['cs']}:{h['m']['line']}", svc, rep, ', '.join(sorted(h['sps']))[:120] or '—', '; '.join(dbc) or ('read-only / none detected')])
            elif hn or js:
                rows.append([c['id'] or ctl_label(c), c['tag'], js or '—', hn or '—', '—', '—', '—', '—', 'UNKNOWN (handler not resolved)'])
    W(table(['Button', 'Event', 'JS', 'Controller.Action', 'Source', 'Service (BLL)', 'Repository (DAL)', 'Stored procedure', 'DB change'], rows) if rows else 'No buttons with server handlers found.')
    W('')
    # ---- 11 frontend flow
    W('## 11. Frontend Flow')
    W(f"- Client script functions in page: {', '.join(p['js_functions'][:30]) or 'none'}")
    W(f"- jQuery/DOM bindings: {', '.join(p['js_events'][:20]) or 'none detected'}")
    W(f"- AJAX endpoints (`url:`): {', '.join(p['js_ajax'][:10]) or 'none'}")
    W(f"- `PageMethods` calls: {', '.join(p['js_pagemethods'][:10]) or 'none'}")
    W(f"- Modal dialogs: {'yes' if p['has_modal'] else 'no'}; Export/print hints: {'yes' if p['has_export'] else 'no'}; File upload: {'yes' if p['has_upload'] else 'no'}; Grid/table markup: {'yes' if p['has_grid'] else 'no'}")
    W('- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.')
    W('')
    # ---- 12 backend flow
    W('## 12. Backend Flow')
    if a['handlers']:
        rows = []
        for h in a['handlers']:
            m = h['m']
            if m['name'].startswith('Page_') or '_' in m['name'] or m['webmethod'] or h['sps'] or h['tr'] or h['tw']:
                rows.append([f"{m['class']}.{m['name']}", f"{p['cs']}:{m['line']}", m['params'][:60], summarize_chain(h)[:260], 'yes' if m['try'] else 'no', 'yes' if any(n['m']['txn'] for n in h['nodes']) else 'no'])
        W(table(['Handler', 'Source', 'Params', 'Call chain (page → BLL → DAL → SP)', 'try/catch', 'Txn'], rows[:60]))
        if len(rows) > 60: W(f"\n_Truncated: {len(rows)} handlers._")
    else:
        W('No code-behind.')
    W('')
    W('Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).')
    W('')
    # ---- 13 API mapping
    W('## 13. API Mapping')
    rows = [[f"{m['class']}.{m['name']}", f"{p['cs']}:{m['line']}", m['params'][:80], route_of(p) + '/' + m['name'], ', '.join(m['sps'])[:100] or ', '.join(m['tables'])[:100] or 'via BLL'] for m in a['wm']]
    W(table(['[WebMethod] / static page method', 'Source', 'Parameters', 'Endpoint (PageMethod URL)', 'Direct data access'], rows) if rows else 'This page exposes no `[WebMethod]`. Client AJAX targets: ' + (', '.join(p['js_ajax'][:8]) or 'none') + '.')
    W('')
    W('## 14. Service Mapping')
    svc = collections.OrderedDict()
    for h in a['handlers']:
        for n in h['bll']:
            svc.setdefault(f"{n['cls']}.{n['name']}", n['path'] + ':' + str(n['line']))
    W(table(['BLL method', 'Source'], list(svc.items())[:60]) if svc else 'No BLL class is reached from this page (page calls DAL directly or has no data access).')
    W('')
    W('## 15. Repository Mapping')
    rp = collections.OrderedDict()
    for h in a['handlers']:
        for n in h['dal']:
            rp.setdefault(f"{n['cls']}.{n['name']}", (n['path'] + ':' + str(n['line']), ', '.join(n['m']['sps'])[:80] or ('inline SQL: ' + ', '.join((n['m']['twrite'] + n['m']['tread'])[:4]) if n['m']['tables'] else 'delegates / no SQL literal'), ', '.join(n['m']['sqlparams'])[:100]))
    W(table(['DAL method', 'Source', 'SP / SQL', 'SQL parameters'], [[k] + list(v) for k, v in list(rp.items())[:80]]) if rp else 'No DAL method reached.')
    W('')
    W('## 16. Stored Procedure Mapping')
    sps = sorted(a['sps'])
    if sps:
        rows = []
        for s in sps:
            callers = [f"{n['cls']}.{n['name']}" for h in a['handlers'] for n in h['nodes'] if s in n['m']['sps']]
            body = SP_BODY.get(s.lower()) if 'SP_BODY' in globals() else None
            rows.append([s, ', '.join(sorted(set(callers)))[:100], body or 'body NOT in repository', 'write-like name' if WRITE_SP_RE.search(s) else 'read-like name (INFERRED)'])
        W(table(['Stored procedure', 'Called from', 'Definition', 'Nature (from name)'], rows[:100]))
    else:
        W('No stored-procedure name resolved from this page (inline SQL or no DB access).')
    W('')
    W('## 17. Database Mapping')
    W(f"- Tables/views READ (inline SQL reachable): {', '.join(sorted(a['tr'])) or 'none detected'}")
    W(f"- Tables WRITTEN (inline SQL reachable): {', '.join(sorted(a['tw'])) or 'none detected'}")
    W('- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).')
    cols = [(t, c2, pn) for h in a['handlers'] for (t, c2, pn, *_r) in h['colmap']]
    if cols:
        W('')
        W(table(['Table', 'Column', 'Bound parameter'], sorted(set(cols))[:60]))
    W('')
    W('## 18. Validation Matrix')
    rows = []
    for c in ctrls:
        if c['tag'] in VALID_TYPES and c['tag'] != 'ValidationSummary':
            at = c['attrs']
            rows.append([at.get('ControlToValidate', c['id']), c['tag'] + (f" ({at.get('ValidationExpression')})" if at.get('ValidationExpression') else ''), 'yes (validator)', 'UNKNOWN', 'UNKNOWN', at.get('ErrorMessage', 'UNKNOWN'), 'UNKNOWN'])
    for who, msg in a['msgs'][:25]:
        rows.append(['(handler ' + who + ')', 'message literal in code-behind (condition not extracted)', 'no', 'yes (server-side message)', 'UNKNOWN', msg, 'UNKNOWN'])
    W(table(['Field', 'Rule', 'Frontend', 'Backend', 'Database', 'Error message', 'BR ID'], rows) if rows else 'No validators or validation messages extracted.')
    W('')
    W('## 19. Business Rules')
    W('None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.')
    W('')
    W('## 20. Permission Matrix')
    W(f"- Session keys read: {', '.join(sorted(a['sess'])) or 'none'}")
    W('- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).')
    W(f"- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).")
    W('')
    W('## 21. Status Lifecycle')
    W('UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.')
    W('')
    W('## 22. Error Handling')
    W(f"- try/catch present in {sum(1 for h in a['handlers'] if h['m']['try'])} of {len(a['handlers'])} methods.")
    W(f"- Redirect targets: {', '.join(sorted(a['redirects'])) or 'none'}")
    W(f"- User messages: {'; '.join(sorted({m for _, m in a['msgs']})[:6]) or 'none extracted'}")
    W('- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).')
    W('')
    W('## 23. Audit')
    aud = sorted({c2 for h in a['handlers'] for (t, c2, *_r) in h['colmap'] if re.search(r'(?i)(entry|create|update|modif|approv|delete)(by|date|time|user)|userid|logdate', c2)})
    W(f"Audit-like columns bound in inline SQL: {', '.join(aud) or 'none detected (SP-based writes: UNKNOWN)'}. Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).")
    W('')
    W('## 24. Integration')
    integ = sorted({x for x in list(a['bll']) + list(a['dal']) + list(a['sps']) if re.search(r'(?i)sap|api|webapi|app\b|sms|mail|mobile', x)})
    W(', '.join(integ) if integ else 'No integration-related classes or SPs detected from this page.')
    W('')
    W('## 25. Reports')
    W(', '.join(sorted(a['reports'])) if a['reports'] else ('This page is a Crystal Reports viewer.' if p['viewer'] else 'No report file referenced from this page.'))
    W('')
    W('## 26. Dependencies')
    W(f"BLL: {', '.join(sorted(a['bll'])) or '—'}; DAL: {', '.join(sorted(a['dal'])) or '—'}; DAO: {', '.join(sorted(a['dao'])[:10]) or '—'}; Tables (inline): {', '.join(sorted(a['tr'] | a['tw'])[:15]) or '—'}")
    W('')
    W('## 27. Test Cases')
    tcs = [['TC-01', 'Open page as an authorised user', 'Page renders without server error; controls in §7 present'],
           ['TC-02', 'Open page with no session', 'Redirected to Login.aspx (if master page in use); verify per page']]
    n = 3
    btns = [c for c in ctrls if c['tag'] in BTN_TYPES and c['events'].get('OnClick')]
    for r in btns:
        tcs.append([f'TC-{n:02d}', f"Click {r['id']} with required fields empty", 'Validator/server message shown; no DB write']); n += 1
        tcs.append([f'TC-{n:02d}', f"Click {r['id']} with valid data", 'Handler runs; rows written to ' + (', '.join(sorted(a['tw'])[:4]) or 'tables written by the SPs in §16 (UNKNOWN)')]); n += 1
        if n > 14: break
    W('GENERATED TEMPLATE — none executed. No automated tests exist in the repository.')
    W('')
    W(table(['ID', 'Scenario', 'Expected (to be confirmed)'], tcs))
    W('')
    W('## 28. Reverse Engineering References')
    refs = [f"`{p['path']}`"] + ([f"`{p['cs']}`"] if p['cs'] else [])
    W('- ' + ', '.join(refs))
    W('- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).')
    W('')
    W('## 29. Spec vs Actual')
    W(table(['Area', 'Specification', 'Actual Code', 'Actual DB', 'Status', 'Reference'], [
        ['Prior spec', 'None existed before this reverse-engineering (no specs/ directory)', p['path'], 'n/a', 'MISSING', '—'],
        ['UI controls', 'This document §7', f"{len(ctrls)} controls parsed", 'n/a', 'PARTIAL', p['path']],
        ['Data access', 'This document §15–17', f"{len(a['sps'])} SP names, {len(a['tr'] | a['tw'])} inline-SQL tables", 'SP bodies not in repo' if a['sps'] else 'n/a', 'UNKNOWN' if a['sps'] else 'PARTIAL', '§16'],
        ['Business rules', 'none', 'not extracted', 'not extracted', 'UNKNOWN', '§19']]))
    W('')
    W('## 30. Known Gaps')
    gaps = ['Business objective, business rules and status lifecycle not reviewed (Tier B).']
    if a['sps']: gaps.append(f"{len(a['sps'])} stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.")
    if a['direct_dal']: gaps.append('Page code-behind calls a DAL class directly (bypasses BLL).')
    if a['string_sql']: gaps.append('Inline SQL in code-behind without SqlParameter (possible string-built SQL — verify): ' + ', '.join(sorted(set(a['string_sql']))[:5]))
    if LEGACY_RE.search(p['name']): gaps.append('Name suggests a legacy/duplicate/test variant; confirm whether it is still reachable from the menu.')
    if not a['msgs'] and not any(c['tag'] in VALID_TYPES for c in ctrls) and any(c['tag'] in BTN_TYPES for c in ctrls): gaps.append('No client or server validation messages found for a page with action buttons.')
    for g in gaps: W('- ' + g)
    W('')
    W('## 31. Change Impact')
    W(f"- Module: {mod[0]} ({mod[2]}).")
    shared = sorted({f"{q['id']}" for s in sps for q in SP_PAGES.get(s, []) if q['id'] != p['id']}) if 'SP_PAGES' in globals() else []
    W(f"- Other pages sharing this page's stored procedures: {', '.join(shared[:15]) or 'none detected'}" + (' …' if len(shared) > 15 else ''))
    W(f"- Changing DAL methods {', '.join(sorted(rp)[:5]) or '—'} affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.")
    return '\n'.join(L) + '\n', cov, a

def score(p, a):
    s = len(a['handlers']) + (F['cs'][p['cs']]['loc'] / 60 if p['cs'] else 0) + len(a['sps']) * 2 + len(a['tw']) * 3 + len(p['controls']) / 15
    if any(WRITE_SP_RE.search(x) for x in a['sps']): s += 6
    if p['kind'] in ('Report viewer',): s *= 0.3
    if p['kind'] == 'List / View': s *= 0.7
    return round(s, 1)

if __name__ == '__main__':
    SP_PAGES = collections.defaultdict(list)
    SP_BODY = {}
    bodies = json.load(open(os.path.join(SP_DIR, 'sp_bodies.json'))) if os.path.exists(os.path.join(SP_DIR, 'sp_bodies.json')) else {}
    SP_BODY = bodies
    analyses = {}
    for p in ASPX:
        analyses[p['path']] = analyze(p)
        for s in analyses[p['path']]['sps']:
            SP_PAGES[s].append(p)
    out = {}
    cov_all = {}
    for p in ASPX:
        a = analyses[p['path']]
        text, cov, _ = gen_page(p, a)
        target = os.path.join(ROOT, p['file'])
        # Never overwrite a reviewed (tier A) spec
        if os.path.exists(target) and re.search(r'(?m)^tier: A', open(target, encoding='utf-8').read(800)):
            continue
        write(p['file'], text)
        out[p['path']] = {'id': p['id'], 'score': score(p, a), 'module': p['module'], 'file': p['file'], 'nsp': len(a['sps']), 'ntw': len(a['tw']), 'handlers': len(a['handlers']), 'kind': p['kind']}
    json.dump(out, open(os.path.join(SP_DIR, 'page_scores.json'), 'w'))
    print('wrote', len(out), 'page specs')
