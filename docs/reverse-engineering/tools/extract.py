"""Reverse-engineering extractor: read-only scan of the WebForms solution -> facts.json
Usage: python extract.py <repo_root> <out_json>
Static regex/tokenizer analysis only. Nothing in the repo is modified."""
import os, re, sys, json, collections

ROOT = sys.argv[1]
OUT = sys.argv[2]
SKIP = {'bin', 'obj', '.vs', 'packages', 'PrecompiledWeb', 'graphify-out', '.git',
        'crystalreportviewers13', 'assets', 'scripts', 'css', 'fonts', 'images', 'VerticalAsset',
        'LoginAssets', 'TempPE', 'APK_File', 'PrescriptionImages', 'ExcelFiles', 'DLL', 'App_Data'}

def rd(p):
    for enc in ('utf-8-sig', 'cp1252', 'latin-1'):
        try:
            with open(p, encoding=enc) as f:
                return f.read()
        except Exception:
            continue
    return ''

def walk(base, exts):
    for dp, dns, fns in os.walk(base):
        dns[:] = [d for d in dns if d not in SKIP]
        for fn in fns:
            if os.path.splitext(fn)[1].lower() in exts:
                yield os.path.join(dp, fn)

def rel(p):
    return os.path.relpath(p, ROOT).replace('\\', '/')

# ---------- C# tokenizer: returns (code_no_comments, masked_no_comments_no_strings, literals[(pos,text)])
def tokenize(t):
    n = len(t); code = []; masked = []; lits = []
    i = 0
    while i < n:
        c = t[i]; d = t[i + 1] if i + 1 < n else ''
        if c == '/' and d == '/':
            j = t.find('\n', i)
            j = n if j < 0 else j
            code.append(' ' * (j - i)); masked.append(' ' * (j - i)); i = j
        elif c == '/' and d == '*':
            j = t.find('*/', i + 2)
            j = n if j < 0 else j + 2
            seg = t[i:j]
            blank = re.sub(r'[^\n]', ' ', seg)
            code.append(blank); masked.append(blank); i = j
        elif c == '@' and d == '"' or (c == '$' and d == '"') or (c == '$' and d == '@') or (c == '@' and d == '$'):
            # verbatim / interpolated (treated as verbatim if @ present)
            verb = '@' in t[i:i + 3]
            k = i
            while t[k] != '"':
                k += 1
            j = k + 1
            while j < n:
                if verb:
                    if t[j] == '"':
                        if j + 1 < n and t[j + 1] == '"':
                            j += 2; continue
                        break
                else:
                    if t[j] == '\\':
                        j += 2; continue
                    if t[j] == '"':
                        break
                j += 1
            j = min(j + 1, n)
            seg = t[i:j]
            lits.append((len(''.join(code)), seg[k - i + 1:-1]))
            code.append(seg); masked.append(re.sub(r'[^\n]', ' ', seg)); i = j
        elif c == '"':
            j = i + 1
            while j < n:
                if t[j] == '\\':
                    j += 2; continue
                if t[j] == '"':
                    break
                if t[j] == '\n':
                    break
                j += 1
            j = min(j + 1, n)
            seg = t[i:j]
            lits.append((len(''.join(code)), seg[1:-1]))
            code.append(seg); masked.append(re.sub(r'[^\n]', ' ', seg)); i = j
        elif c == "'":
            j = i + 1
            while j < n and t[j] != "'":
                if t[j] == '\\':
                    j += 1
                j += 1
            j = min(j + 1, n)
            seg = t[i:j]
            code.append(seg); masked.append(' ' * len(seg)); i = j
        else:
            code.append(c); masked.append(c); i += 1
    return ''.join(code), ''.join(masked), lits

KW = {'if', 'for', 'foreach', 'while', 'switch', 'catch', 'using', 'lock', 'else', 'do', 'try', 'finally',
      'return', 'new', 'get', 'set', 'fixed', 'checked', 'unchecked', 'delegate', 'add', 'remove'}
METH_RE = re.compile(r'(?:^|[\s\]])(?:[\w<>\[\],\.\?]+\s+)*?(\w+)\s*\(([^{};]*)\)\s*(?::\s*(?:base|this)\s*\([^)]*\))?\s*(?:where\s[^{]*)?$', re.S)
CLS_RE = re.compile(r'\b(?:class|interface|struct)\s+(\w+)')
NS_RE = re.compile(r'\bnamespace\s+([\w\.]+)')
LOCALS_RE = re.compile(r'\b([A-Z][A-Za-z0-9_]*)\s+(\w+)\s*(?:=\s*new\s+([A-Z][A-Za-z0-9_]*)|;|=)')
VAR_NEW_RE = re.compile(r'\bvar\s+(\w+)\s*=\s*new\s+([A-Z][A-Za-z0-9_]*)')

BADTBL = {'dbo', 'sys', 'msdb', 'data', 'date', 'information', 'list', 'stored', 'successfully', 'eroor', 'btn', 'deposit', 'failed', 'freeze', 'logic', 'operation', 'process', 'stock', 'the', 'master', 'details', 'detail'}
SQLKW = {'select', 'where', 'set', 'values', 'as', 'on', 'and', 'or', 'not', 'null', 'top', 'distinct', 'from',
         'inner', 'left', 'right', 'outer', 'join', 'group', 'order', 'by', 'having', 'union', 'all', 'case', 'when',
         'then', 'else', 'end', 'in', 'exists', 'is', 'like', 'between', 'with', 'nolock', 'the', 'a', 'table', 'if'}
TBL_RE = re.compile(r'(?i)\b(?:FROM|JOIN|INTO|UPDATE|DELETE\s+FROM|DELETE|TRUNCATE\s+TABLE)\s+(?:\[?dbo\]?\.)?\[?([A-Za-z_][\w]*)\]?')
EXEC_RE = re.compile(r'(?i)\bEXEC(?:UTE)?\s+(?:\[?dbo\]?\.)?\[?([A-Za-z_][\w]*)\]?')
DBCALL_RE = re.compile(r'\b(GetSqlDataReader|GetDataTable\w*|GetDataSet\w*|SaveDataReturnPrimaryKey|SaveData\w*|UpdateData\w*|DeleteData\w*|'
                       r'RunStoreProcedure|SaveAction|UpdateAction|DeleteAction|GetDataTableAction|GetDataTableUsingReader|ExecuteNonQuery\w*|'
                       r'ExecuteStoredProcedure\w*|ExecuteProcedure\w*|\w*Async)\s*\(\s*(?:@?")([^"]+)"')
SPLIT_RE = re.compile(r'(?:new\s+SqlCommand\s*\(|CommandText\s*=)\s*(?:@?")([^"]+)"')
SPNAME_RE = re.compile(r'^(?:sp|SP|Sp|usp|USP|proc|Proc)[_A-Z][\w\.]*$|^sp[A-Z]\w*$')
IDENT_RE = re.compile(r'^[A-Za-z_][\w\.]*$')

def parse_cs(path):
    text = rd(path)
    code, masked, lits = tokenize(text)
    # line starts
    stack = []  # dicts
    classes = {}
    ns = ''
    last = 0
    res = []
    for i, ch in enumerate(masked):
        if ch == '{':
            header = masked[last:i]
            top = stack[-1]['kind'] if stack else None
            node = {'kind': 'block', 'start': i}
            hs = re.sub(r'\[[^\]]*\]', ' ', header).strip()
            m = NS_RE.search(hs)
            if m and top in (None, 'ns'):
                node.update(kind='ns', name=m.group(1))
            else:
                m = CLS_RE.search(hs)
                if m and top in (None, 'ns', 'class'):
                    node.update(kind='class', name=m.group(1),
                                base=(re.search(r':\s*([\w\.<>, ]+)', hs[m.end():]) or [None, ''])[1] if ':' in hs[m.end():] else '')
                elif top == 'class' and '(' in hs and ')' in hs and '=' not in hs.split('(')[0]:
                    mm = METH_RE.search(hs)
                    if mm and mm.group(1) not in KW:
                        node.update(kind='method', name=mm.group(1), params=' '.join(mm.group(2).split()))
                        node['header'] = hs
            stack.append(node)
            last = i + 1
        elif ch == '}':
            if stack:
                node = stack.pop()
                node['end'] = i
                if node['kind'] == 'method':
                    cls = next((s['name'] for s in reversed(stack) if s['kind'] == 'class'), None)
                    res.append((cls, node))
            last = i + 1
        elif ch == ';':
            last = i + 1
    line_starts = [0]
    for m in re.finditer('\n', code):
        line_starts.append(m.end())
    import bisect
    def line_of(pos):
        return bisect.bisect_right(line_starts, pos)
    # classes present
    cls_info = collections.OrderedDict()
    for m in re.finditer(r'\bclass\s+(\w+)(?:\s*:\s*([\w\.<>, ]+))?', masked):
        cls_info.setdefault(m.group(1), (m.group(2) or '').strip())
    # variable -> type across file
    vt = {}
    for m in LOCALS_RE.finditer(masked):
        t, v, nt = m.group(1), m.group(2), m.group(3)
        if t in ('Session', 'String', 'Response', 'Request', 'Convert', 'Math', 'DateTime', 'Exception', 'Label', 'TextBox', 'DropDownList'):
            pass
        vt[v] = t
    for m in VAR_NEW_RE.finditer(masked):
        vt[m.group(1)] = m.group(2)
    methods = []
    for cls, node in res:
        b0, b1 = node['start'], node['end']
        body = code[b0:b1]
        blits = [l for (p, l) in lits if b0 <= p < b1]
        calls = collections.OrderedDict()
        for m in re.finditer(r'\b(?:this\.)?([A-Za-z_]\w*)\s*\.\s*([A-Za-z_]\w*)\s*\(', masked[b0:b1]):
            calls[(m.group(1), m.group(2))] = 1
        local_calls = set(m.group(1) for m in re.finditer(r'(?<![\.\w])([A-Za-z_]\w*)\s*\(', masked[b0:b1])) - KW
        sps = []
        for m in DBCALL_RE.finditer(body):
            nm = m.group(2).strip()
            if IDENT_RE.match(nm) and not re.search(r'(?i)\b(select|insert|update|delete)\b\s', nm):
                sps.append(nm)
            else:
                pass
        for m in SPLIT_RE.finditer(body):
            nm = m.group(1).strip()
            if IDENT_RE.match(nm):
                sps.append(nm)
        for l in blits:
            ls = l.strip()
            if SPNAME_RE.match(ls):
                sps.append(ls)
        sql_join = ' '.join(blits)
        for m in EXEC_RE.finditer(sql_join):
            sps.append(m.group(1))
        tables = set(); inserts = []; updates = []
        sql_lits = [l for l in blits if re.search(r'(?i)\b(select|insert|update|delete|from|join|into|set|where|exec)\b', l) and not re.search(r'(?i)\b(please|successfully|failed|error)\b', l)]
        sql_join = ' '.join(sql_lits)
        has_sql = bool(re.search(r'(?i)\b(select|insert\s+into|update|delete)\b', sql_join)) and bool(re.search(r'(?i)\b(from|into|update|delete|set)\b', sql_join))
        if has_sql:
            for m in TBL_RE.finditer(sql_join):
                nm = m.group(1)
                if nm.lower() in SQLKW or nm.lower() in BADTBL:
                    continue
                if not (re.match(r'(?i)^(tbl|view_|vw|v_|tmp|temp|inv|acc|hr|sys_|udf|fn_)', nm) or '_' in nm or any(ch.isupper() for ch in nm[1:])):
                    continue
                tables.add(nm)
            for m in re.finditer(r'(?is)INSERT\s+INTO\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?\s*\(([^)]*)\)', sql_join):
                inserts.append([m.group(1), [c.strip(' []\n\t') for c in m.group(2).split(',') if c.strip()]])
            for m in re.finditer(r'(?is)UPDATE\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?\s+SET\s+(.*?)(?:\bWHERE\b|$)', sql_join):
                updates.append([m.group(1), re.findall(r'\[?(\w+)\]?\s*=', m.group(2))])
        twrite = sorted(set(m.group(1) for m in re.finditer(r'(?is)\b(?:INSERT\s+INTO|UPDATE|DELETE\s+FROM|DELETE|TRUNCATE\s+TABLE)\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?', sql_join)) & tables)
        tread = sorted(set(m.group(1) for m in re.finditer(r'(?is)\b(?:FROM|JOIN)\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?', sql_join)) & tables)
        al = {}
        for tn, an in re.findall(r'(?is)\b(?:FROM|JOIN)\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?\s+(?:AS\s+)?(\w+)\b', sql_join):
            if an.lower() not in SQLKW and an.lower() not in ('on', 'with', 'where', 'inner', 'left', 'right', 'join', 'group', 'order', 'union', 'cross', 'full', 'outer', 'nolock'):
                al[an] = tn
        for tn in tables:
            al.setdefault(tn, tn)
        joins = []
        for x, y in re.findall(r'(?is)\bON\s+\(?\s*(\w+\.\w+)\s*=\s*(\w+\.\w+)', sql_join)[:12]:
            xa, xc = x.split('.'); ya, yc = y.split('.')
            if xa in al and ya in al and al[xa] != al[ya]:
                joins.append([al[xa] + '.' + xc, al[ya] + '.' + yc])
        sqlparams = sorted(set(re.findall(r'new\s+SqlParameter\s*\(\s*"(@?\w+)"', body)) | set(re.findall(r'Parameters\.Add\w*\s*\(\s*"(@?\w+)"', body)))
        sess = sorted(set(re.findall(r'Session\s*\[\s*"([^"]+)"\s*\]', body)))
        redirs = sorted(set(re.findall(r'Response\.Redirect\s*\(\s*(?:@)?"([^"]+)"', body)))
        rpts = sorted(set(re.findall(r'"([^"]*\.rpt)"', body)) | set(re.findall(r'\bnew\s+(\w+(?:Rpt|RPT|_RPT\w*|Report))\s*\(', body)))
        js = sorted(set(re.findall(r'(?:RegisterStartupScript|RegisterClientScriptBlock|Response\.Write)\s*\([^;]*?"([^"]{0,140})', body)))[:3]
        msgs = sorted(set(l.strip() for l in blits if 12 <= len(l.strip()) <= 140 and re.search(r'(?i)\b(please|success|error|fail|invalid|already|required|not allowed|cannot|can not|must|select|enter|input|exceed|insufficient|not found|denied)\b', l) and not re.search(r'(?i)\b(select .* from|insert into|update .* set)\b', l)))[:8]
        raw_ctl = sorted(set(re.findall(r'\b(\w+)\.(?:Text|SelectedValue|SelectedItem|Checked|Value)\b', masked[b0:b1])))
        mb = masked[b0:b1]
        assigns = []
        for m in re.finditer(r'(\w+)\s*=(?!=)\s*([^;{},]*?)\b([A-Za-z_]\w*)\.(Text|SelectedValue|Checked|Value|SelectedItem\.Text|SelectedItem\.Value)\b', mb):
            if m.group(3) not in ('Session', 'Request', 'Convert', 'DBNull', 'ViewState'):
                assigns.append([m.group(1), m.group(3), m.group(4)])
        param_prop = []
        for m in re.finditer(r'new\s+SqlParameter\s*\(\s*"(@\w+)"\s*,', body):
            seg = body[m.end():m.end() + 220]
            cut = re.search(r'new\s+SqlParameter|;|\}\s*\)', seg)
            seg = seg[:cut.start()] if cut else seg
            for mm in re.finditer(r'\b(\w+)\.(\w+)\b', seg):
                if mm.group(1) not in ('DBNull', 'Convert', 'String', 'string', 'DateTime', 'Math', 'Decimal', 'Int32', 'Session', 'Guid') and mm.group(2) not in ('Value', 'ToString', 'Trim', 'ToShortDateString'):
                    param_prop.append([m.group(1), mm.group(1) + '.' + mm.group(2)]); break
            else:
                mm = re.search(r'\b(?:Convert\.\w+\()?\s*(\w+)\s*\)?\s*$', seg.strip().rstrip(')'))
                if mm and not mm.group(1).isdigit():
                    param_prop.append([m.group(1), mm.group(1)])
        colmap = []
        for m in re.finditer(r'(?is)INSERT\s+INTO\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?\s*\(([^)]*)\)\s*VALUES\s*\((.*?)\)\s*(?:;|$|SELECT|\'|SET|IF)', sql_join):
            cols = [c.strip(' []\n\t') for c in m.group(2).split(',') if c.strip()]
            vals = [v.strip() for v in re.split(r',(?![^()]*\))', m.group(3))]
            if len(cols) == len(vals):
                for c, v in zip(cols, vals):
                    pm = re.search(r'@\w+', v)
                    if pm:
                        colmap.append([m.group(1), c, pm.group(0)])
        for m in re.finditer(r'(?is)UPDATE\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?\s+SET\s+(.*?)(?:\bWHERE\b|$)', sql_join):
            for c, pn in re.findall(r'\[?(\w+)\]?\s*=\s*(@\w+)', m.group(2)):
                colmap.append([m.group(1), c, pn])
        ddl_loads = [[a, b, c] for a, b, c in re.findall(r'(\w*(?:DropDown|Ddl|Load|Bind|Fill)\w*)\s*\(\s*[^,()]+,\s*"(\w+)"\s*,\s*"(\w+)"', body)]
        ddl_loads += [['DataTextField/DataValueField', a, b] for a, b in zip(re.findall(r'\.DataTextField\s*=\s*"(\w+)"', body), re.findall(r'\.DataValueField\s*=\s*"(\w+)"', body))]
        ctl_args = []
        for m in re.finditer(r'\b(\w+)\s*\.\s*(\w+)\s*\(', mb):
            i0 = m.end(); depth = 1; j = i0
            while j < len(mb) and depth:
                depth += (mb[j] == '(') - (mb[j] == ')'); j += 1
            args = re.findall(r'\b([A-Za-z_]\w*)\b(?!\s*[\(\.])', mb[i0:j - 1])
            ids = [a for a in args if a in set() or re.search(r'(?i)(dropdown|ddl|list|listbox|combo|grid|gv|textbox|txt|label|lbl|hidden|hdn|check|chk|rbl|repeater)', a) or a[:1].islower()]
            if ids:
                ctl_args.append([m.group(1), m.group(2), sorted(set(ids))[:6]])
        news = sorted(set(re.findall(r'\bnew\s+([A-Z]\w*)\s*[\(\{]', mb)))
        methods.append({
            'class': cls, 'name': node['name'], 'params': node.get('params', ''), 'line': line_of(b0),
            'inserts': inserts, 'updates': updates, 'sqlparams': sqlparams, 'twrite': twrite, 'tread': tread, 'joins': joins,
            'assigns': assigns, 'param_prop': param_prop, 'colmap': colmap, 'ddl_loads': ddl_loads, 'ctl_args': ctl_args, 'news': news,
            'end_line': line_of(b1), 'loc': line_of(b1) - line_of(b0) + 1,
            'calls': [list(k) for k in calls.keys()], 'local_calls': sorted(local_calls),
            'sps': sorted(set(sps)), 'tables': sorted(tables), 'sess': sess, 'redirects': redirs,
            'reports': rpts, 'messages': msgs, 'controls': raw_ctl[:40],
            'webmethod': bool(re.search(r'\[\s*(?:System\.Web\.Services\.)?WebMethod|\[\s*(?:System\.Web\.Script\.Services\.)?ScriptMethod|\[\s*Http(?:Get|Post)', masked[max(0, b0 - 200):b0]) or re.search(r'\[\s*(?:System\.Web\.Services\.)?WebMethod', code[max(0, b0 - 260):b0]) is not None),
            'try': 'try' in local_calls or bool(re.search(r'\btry\s*\{', masked[b0:b1])),
            'txn': bool(re.search(r'(?i)BeginTransaction|SqlTransaction|TransactionScope|SqlConnectionClose\(true\)', body)),
        })
    props = collections.OrderedDict()
    if 'Library.DAO' in path.replace('\\', '/'):
        cur = None
        for m in re.finditer(r'\bclass\s+(\w+)|public\s+([\w<>\?\[\], \.]+?)\s+(\w+)\s*(\{\s*get|;|=)', masked):
            if m.group(1):
                cur = m.group(1); props.setdefault(cur, [])
            elif cur and m.group(2) not in ('class', 'static', 'return', 'new', 'const'):
                props[cur].append([m.group(2).strip(), m.group(3)])
    return {'path': rel(path), 'classes': cls_info, 'vars': vt, 'methods': methods, 'loc': code.count('\n') + 1, 'props': props}

# ---------- ASPX
ATTR_RE = re.compile(r'([\w:\-\.]+)\s*=\s*(?:"([^"]*)"|\'([^\']*)\')', re.S)
TAG_RE = re.compile(r'<(asp:\w+|input|select|textarea|button|a|form|table|img|iframe|uc\d*:\w+|ucr:\w+|[a-z]+:\w+)\b((?:[^>"\']|"[^"]*"|\'[^\']*\')*)>', re.S | re.I)
DIR_RE = re.compile(r'<%@\s*(\w+)\s+((?:[^%]|%(?!>))*)%>', re.S)

def parse_aspx(path):
    t = rd(path)
    d = {'path': rel(path), 'ext': os.path.splitext(path)[1].lower(), 'size': len(t)}
    m = DIR_RE.search(t)
    if m:
        d['directive'] = m.group(1)
        d['dattrs'] = {k: (a or b) for k, a, b in ATTR_RE.findall(m.group(2))}
    else:
        d['directive'] = ''; d['dattrs'] = {}
    d['registers'] = [{k: (a or b) for k, a, b in ATTR_RE.findall(x)} for x in re.findall(r'<%@\s*Register\s+((?:[^%]|%(?!>))*)%>', t)]
    ctrls = []
    for m in TAG_RE.finditer(t):
        tag, at = m.group(1), m.group(2)
        a = {k: (x or y) for k, x, y in ATTR_RE.findall(at)}
        low = tag.lower()
        if low.startswith('asp:'):
            typ = tag[4:]
            if typ in ('Content', 'Panel', 'ContentPlaceHolder', 'ScriptManager', 'UpdatePanel', 'UpdateProgress', 'Literal', 'PlaceHolder', 'View', 'MultiView', 'ContentTemplate', 'TemplateField', 'Columns', 'BoundField', 'ItemTemplate', 'HeaderTemplate', 'FooterTemplate') and not a.get('ID') and typ not in ('BoundField', 'TemplateField'):
                continue
        else:
            typ = tag
            if low in ('a', 'img', 'iframe', 'table', 'form') and not a.get('id') and not a.get('ID'):
                if not (low == 'a' and a.get('href')):
                    continue
        cid = a.get('ID') or a.get('id') or ''
        ev = {k: v for k, v in a.items() if re.match(r'(?i)^on(click|selectedindexchanged|textchanged|checkedchanged|rowcommand|rowdatabound|rowdeleting|rowediting|rowupdating|pageindexchanging|sorting|command|itemcommand|itemdatabound|databound|load|init|prerender|clientclick|change|blur|focus|keyup|keydown|keypress|mouseover|submit|dblclick)\w*$', k)}
        keep = {k: v for k, v in a.items() if k in ('Text', 'HeaderText', 'DataField', 'DataFormatString', 'CssClass', 'class', 'type', 'name', 'value', 'placeholder', 'ValidationGroup', 'Enabled', 'Visible', 'ReadOnly', 'AutoPostBack', 'TextMode', 'ControlToValidate', 'ErrorMessage', 'ToolTip', 'CommandName', 'DataKeyNames', 'AllowPaging', 'PageSize', 'NavigateUrl', 'href', 'SelectedValue', 'MaxLength', 'CausesValidation', 'ValidationExpression', 'InitialValue', 'Checked', 'disabled', 'readonly', 'required', 'title', 'for', 'ControlToCompare', 'Operator', 'MinimumValue', 'MaximumValue', 'Type', 'runat', 'DataTextField', 'DataValueField', 'DataSourceID', 'SortExpression')}
        ctrls.append({'tag': typ, 'id': cid, 'attrs': keep, 'events': ev, 'line': t.count('\n', 0, m.start()) + 1})
    d['controls'] = ctrls
    d['labels'] = [re.sub(r'\s+', ' ', re.sub(r'<[^>]+>', ' ', x)).strip() for x in re.findall(r'<label[^>]*>(.*?)</label>', t, re.S | re.I)][:80]
    d['headings'] = [re.sub(r'\s+', ' ', re.sub(r'<[^>]+>', ' ', x)).strip() for x in re.findall(r'<h[1-6][^>]*>(.*?)</h[1-6]>', t, re.S | re.I)][:10]
    scripts = re.findall(r'<script\b[^>]*>(.*?)</script>', t, re.S | re.I)
    js = '\n'.join(scripts)
    d['script_src'] = re.findall(r'<script[^>]+src\s*=\s*"([^"]+)"', t, re.I)
    d['link_css'] = re.findall(r'<link[^>]+href\s*=\s*"([^"]+\.css[^"]*)"', t, re.I)
    d['js_functions'] = sorted(set(re.findall(r'function\s+([A-Za-z_$][\w$]*)\s*\(', js)))
    d['js_ajax'] = sorted(set(re.findall(r'url\s*:\s*["\']([^"\']+)["\']', js)) | set(re.findall(r'\$\.(?:post|get|getJSON)\s*\(\s*["\']([^"\']+)["\']', js)))
    d['js_pagemethods'] = sorted(set(re.findall(r'PageMethods\.(\w+)', js)))
    d['js_events'] = sorted(set(re.findall(r'\$\(\s*["\']([#\.][\w\-]+)["\']\s*\)\s*\.\s*(?:on\(\s*["\'](\w+)["\']|(click|change|keyup|blur|focus|keypress|submit)\s*\()', js) and [f"{a}:{b or c}" for a, b, c in re.findall(r'\$\(\s*["\']([#\.][\w\-]+)["\']\s*\)\s*\.\s*(?:on\(\s*["\'](\w+)["\']|(click|change|keyup|blur|focus|keypress|submit)\s*\()', js)]))[:40]
    d['js_len'] = len(js)
    d['has_modal'] = bool(re.search(r'(?i)class="[^"]*\bmodal\b|\.modal\(|ModalPopupExtender', t))
    d['has_export'] = bool(re.search(r'(?i)excel|export|\.xls|\.pdf|ExportTo|print', t))
    d['has_upload'] = bool(re.search(r'(?i)FileUpload|type="file"', t))
    d['has_grid'] = bool(re.search(r'asp:GridView|asp:Repeater|asp:ListView|asp:DataList|<table', t, re.I))
    d['viewer'] = bool(re.search(r'CrystalReportViewer', t))
    m = re.search(r'<title>(.*?)</title>', t, re.S | re.I)
    d['html_title'] = m.group(1).strip() if m else ''
    d['server_script'] = bool(re.search(r'<script[^>]*runat\s*=\s*"server"', t, re.I))
    return d

def main():
    facts = {'pages': [], 'cs': {}, 'cs_files': []}
    web = os.path.join(ROOT, 'Solution.Web')
    for p in walk(web, {'.aspx', '.ascx', '.master', '.asmx', '.ashx', '.asax'}):
        facts['pages'].append(parse_aspx(p))
    for base in ('Solution.Web', 'Library.BLL', 'Library.DAL', 'Library.DAO', 'Library.CrystalReports', 'MVC_TreeGrid_Template'):
        for p in walk(os.path.join(ROOT, base), {'.cs'}):
            if p.endswith(('.Designer.cs', 'AssemblyInfo.cs')) or '\\TemporaryGeneratedFile' in p or 'Reference.cs' in p:
                continue
            try:
                facts['cs'][rel(p)] = parse_cs(p)
            except Exception as ex:
                facts['cs'][rel(p)] = {'error': str(ex), 'path': rel(p), 'classes': {}, 'vars': {}, 'methods': [], 'loc': 0}
    # views (cshtml)
    facts['cshtml'] = [rel(p) for p in walk(os.path.join(ROOT, 'MVC_TreeGrid_Template'), {'.cshtml'})]
    # menu in NewMasterPage (static)
    with open(OUT, 'w', encoding='utf-8') as f:
        json.dump(facts, f)
    print('pages', len(facts['pages']), 'cs', len(facts['cs']))

main()
