"""Shared loading, module classification and call-trace resolution for the doc generators."""
import json, os, re, collections, sys

ROOT = r'D:\Projects\Unicorn\Distribution'
SP_DIR = os.path.dirname(os.path.abspath(__file__))
F = json.load(open(os.path.join(SP_DIR, 'facts.json')))

MODULES = [
 ('M01', 'authentication-navigation', 'Authentication, Session & Navigation', 'Login, session bootstrap, master pages / DB-driven menu, landing pages, password change, profile, session tracking.'),
 ('M02', 'user-role-permission', 'User, Role & Permission Administration', 'User accounts, roles, menu-permission distribution, stock-condition/DC permissions, approval-step maps, settings panel.'),
 ('M03', 'organisation-market-structure', 'Organisation & Market Structure Master', 'Company/branch/unit, geography (division/district/thana), sales hierarchy (national/zone/region/area/territory/market), routes, depot-territory mapping and structure transfers.'),
 ('M04', 'product-pricing-promotion', 'Product, Pricing & Promotion Master', 'Products and classification, units/pack sizes, manufacturers/suppliers, unit prices, discounts/bonus/campaigns, promo allocation groups.'),
 ('M05', 'customer-master', 'Customer & Provider Master', 'Customer/provider entry, categories/types, credit limits, tagging, Excel uploads/imports, approvals of customer changes.'),
 ('M06', 'order-management', 'Order Management', 'Manual/imported orders, requisitions, order approval, monitoring, tracking, DC change, deletion/edit.'),
 ('M07', 'invoicing-delivery', 'Proforma, Invoicing & Delivery', 'Proforma and invoice creation (manual/auto/by-order), challans, top sheets, picking, loading, delivery assistants, invoice printing.'),
 ('M08', 'sales-return', 'Sales Return & Invoice Return', 'Sales returns, return invoices, delivery returns, replacement notes, return posting.'),
 ('M09', 'collection-payment', 'Collections, Payments & Money Receipts', 'Customer payments, money receipts, deposit slips, payment reversal/partial, customer ledger, aging and receivables.'),
 ('M10', 'warehouse-inventory', 'Warehouse, DC Stock & Inventory', 'Warehouse/DC stock in/out, adjustments, freezes, opening balances, sample stock, material conversion, stock valuation and monitoring.'),
 ('M11', 'stock-transfer', 'Stock Transfer & Chalan Receive', 'DC-to-DC, DC-to-Warehouse, B2B, stock transport orders, chalan receive, in-transit tracking.'),
 ('M12', 'sub-depot', 'Sub-Depot Operations', 'Sub-depot info, stock in/out/transfer, invoicing, payments, top sheets, adjustments and freeze.'),
 ('M13', 'sales-reports-dashboard', 'Sales Reports, Analytics & Dashboards', 'Sales/business/summary/trend reports, Crystal report viewers and dashboards not owned by a transactional module.'),
 ('M14', 'target-management', 'Target & DWSP Management', 'National/zone/area/territory/MIO targets, monthly and product-wise targets, DWSP, target achievement reports.'),
 ('M15', 'hr-field-force', 'HR, Field-Force & Employee Administration', 'Employee master, attendance, leave, holiday, shifts, tour plan, expense/TADA/mileage claims, training, notice board, employee-related reports.'),
 ('M16', 'doctor-visit-prescription', 'Doctor / Provider Master, Visits & Prescriptions', 'Doctor master data, doctor visits/DCR/DCP, prescriptions (RX), doctor reports.'),
 ('M17', 'sap-external-integration', 'SAP, Mobile-App & External Integration', 'SAP hand-off pages, MIGO, provider dropout integration, API-customer import, mobile-app data-load web methods.'),
 ('M18', 'approval-workflow', 'Approval Workflow', 'Approval list pages for customers, doctors, orders, leave, expense, tour plan, DCR, RX and related.'),
]
MOD = {m[0]: m for m in MODULES}

def kebab(s):
    s = re.sub(r'[^A-Za-z0-9]+', '-', s).strip('-').lower()
    return s or 'page'

# ---------- module classification (INFERRED from folder + file name) ----------
def classify(path):
    p = path.replace('\\', '/')
    parts = p.split('/')
    folder = parts[1] if len(parts) > 2 else '(root)'
    name = os.path.splitext(os.path.basename(p))[0]
    n = name.lower()
    def has(*ks): return any(k in n for k in ks)
    if folder == '(root)':
        return 'M01' if n == 'login' else 'M13'
    if folder in ('CommonUI', 'UserProfile_UI', 'UserTracking', 'MasterPages'): return 'M01'
    if folder in ('UserPermission', 'SettingPanel_UI'): return 'M02'
    if folder == 'Approval_UI': return 'M18'
    if folder in ('DWSP', 'Target_UI'): return 'M14'
    if folder == 'Thana_UI': return 'M03'
    if folder == 'TransferUI': return 'M05' if has('customer', 'doctor') else 'M03'
    if folder == 'SubDepot_UI': return 'M12'
    if folder in ('SAP_Integration', 'eProgram_UI'): return 'M17'
    if folder == 'Money_Receipt_UI': return 'M09'
    if folder == 'PromoAlloc': return 'M04'
    if folder in ('NoticeBoard_UI', 'LeaveProcess_UI'): return 'M15'
    if folder == 'Dashboard_UI': return 'M16' if has('doctorvisit') else 'M13'
    if folder in ('DoctorMaster_UI', 'DoctorVisit_UI'): return 'M16'
    if folder == 'App_Code':
        return 'M17' if has('webservice') else 'M01'
    if folder == 'Reports':
        return 'M13'
    if folder == 'Reports_UI':
        if has('dwsp', 'target'): return 'M14'
        if has('expense', 'leave', 'tourplan', 'organogram'): return 'M15'
        if has('sample'): return 'M10'
        if has('orderpermission'): return 'M02'
        return 'M16'
    if folder == 'MasterSetup_UI':
        if has('campaign', 'quotedprice'): return 'M04'
        if has('doctor'): return 'M16'
        if has('employee', 'tour', 'usersetupforemp'): return 'M15' if not has('usersetup') else 'M02'
        if has('order'): return 'M06'
        if n in ('dalist', 'dasetup'): return 'M07'
        if has('routeinformation', 'territorywisedepot'): return 'M03'
        if has('customer', 'programtype', 'smctype', 'stationtype'): return 'M05'
        if has('inventorywebservice'): return 'M17'
        return 'M03'
    if folder == 'DoctorModule_UI':
        if has('usersetup', 'userrole', 'userrecords'): return 'M02'
        if has('prescription'): return 'M16'
        if has('productline', 'tharapeutic', 'genericgroup', 'groupsetup'): return 'M04'
        if has('seeddata', 'commondataload', 'appmonitoring'): return 'M17'
        if n == 'setup': return 'M17'
        if has('financialyear', 'depotwisearea'): return 'M03'
        if has('tada'): return 'M15'
        if has('national', 'nsm', 'dzsm', 'rsm', 'asm', 'zone', 'area', 'territory', 'market', 'mio', 'amrecords', 'amsetup', 'router'): return 'M03'
        return 'M15'
    if folder == 'SInventory_RPTVIEW':
        rules = [
            (('subdep',), 'M12'), (('proforma', 'invoice', 'delivary', 'topsheet', 'picking', 'challan', 'loading', 'marketwisepicking'), 'M07'),
            (('stockreceive', 'transfer', 'transfar', 'tochalan', 'intransit', 'dctodc', 'dctowh', 'stocktransfar'), 'M11'),
            (('return', 'rejection'), 'M08'), (('moneyreceipt', 'payment', 'ledger', 'collection', 'aging'), 'M09'),
            (('stock', 'inventory', 'whstock', 'wearhouse', 'warehouse', 'opening', 'wh'), 'M10'),
            (('customermaster', 'custver'), 'M05'), (('tourplan', 'employeeexpense'), 'M15'), (('migo',), 'M17'),
            (('productcost', 'producttread', 'discount'), 'M04'), (('deleteorder',), 'M06'),
        ]
        for ks, m in rules:
            if has(*ks): return m
        return 'M13'
    if folder == 'SInventory_UI':
        if has('salesreport', 'totalsummary', 'businesssummary', 'branchtotal'): return 'M13'
        if has('subdep', 'subdeport'): return 'M12'
        if has('sap', 'migo', 'apicustomer', 'importedapi'): return 'M17'
        if has('salesreturn', 'returninvoice', 'deliveryreturn', 'deliverysalesreturn', 'invoicereturn', 'replacenote', 'backtoreturn', 'sndtimes', 'salesrejec'): return 'M08'
        if has('stocktransfer', 'dctodc', 'depotowh', 'depowh', 'b2b', 'transferstock', 'transferreceive', 'receiveproductbychalan', 'stockrcv', 'stockreceive', 'intransit', 'stocktransportorder', 'transferstockreceive'): return 'M11'
        if has('payment', 'moneyreceipt', 'deposit', 'collection', 'ledger', 'aging', 'receiveable', 'outstanding', 'dhb_delivery'): return 'M09'
        if has('creditlimit', 'custcategory', 'customercategory', 'custmaster', 'customermaster', 'customerlist', 'customercredit', 'customerexcel', 'customertag', 'fixedcustomer', 'multicustomer', 'activecustomer'): return 'M05'
        if has('order', 'requisition', 'issuerequisition'): return 'M06'
        if has('proforma', 'invoice', 'delivary', 'delivery', 'challan', 'topsheet', 'picking', 'loading', 'autoinvoice'): return 'M07'
        if has('product', 'procategory', 'protype', 'packsize', 'stockuom', 'ingrident', 'manufacturer', 'supplier', 'shippingcarton', 'unitprice', 'conditionalpurpose', 'discountspecial', 'pricereport'): return 'M04'
        if has('stock', 'warehouse', 'wh', 'adjust', 'freeze', 'opening', 'materialconversion', 'countrystock', 'dcstock', 'sample', 'distributioncenter', 'monthlyinventory', 'monthlywarehouse', 'batch', 'othersto'): return 'M10'
        if has('miatarget', 'target'): return 'M14'
        if has('emp', 'miainfo'): return 'M15'
        if has('userentry', 'useredit', 'userview', 'dcuserpermission', 'approvalsetup', 'accountsettings'): return 'M02'
        if has('zone', 'region', 'area', 'district', 'companyinfo', 'companyunit', 'branch', 'department', 'marketinfo', 'territory'): return 'M03'
        return 'M13'
    return 'M13'

def page_kind(path):
    n = os.path.splitext(os.path.basename(path))[0]
    l = n.lower()
    ext = os.path.splitext(path)[1].lower()
    if ext == '.master': return 'Master page'
    if ext == '.ascx': return 'User control'
    if ext == '.asmx': return 'Web service'
    if ext == '.ashx': return 'HTTP handler'
    if ext == '.asax': return 'Application'
    if 'viewer' in l or 'rptview' in path.lower(): return 'Report viewer'
    if re.search(r'report|summary|aging|ledger|monitoring|trend|rpt|dashboard', l): return 'Report / Dashboard'
    if re.search(r'approv', l): return 'Approval'
    if re.search(r'upload|import', l): return 'Upload / Import'
    if re.search(r'edit', l): return 'Edit form'
    if re.search(r'view|list|records|listing', l): return 'List / View'
    if re.search(r'entry|setup|creation|create|generate|transfer|receive|payment|receipt|adjust', l): return 'Entry / Transaction'
    return 'Page'

LEGACY_RE = re.compile(r'(?i)(copy of|- copy|_old|old\b|oldcampaign|testing_|_test|_new\b|_final|ppp|new2|_t$|_mult)')

# ---------- class / method index for trace ----------
LIBS = ('Library.BLL/', 'Library.DAL/', 'Library.DAO/', 'Solution.Web/App_Code/', 'Library.CrystalReports/')
class_idx = collections.defaultdict(list)   # class name -> [(path, method dict)]
meth_by_name = collections.defaultdict(list)  # method name -> [(path, class, method)]
file_of_class = {}
dao_props = {}
for pth, c in F['cs'].items():
    for cls in c['classes']:
        file_of_class.setdefault(cls, pth)
    for cls, pr in c.get('props', {}).items():
        dao_props[cls] = pr
    for m in c['methods']:
        if pth.startswith(LIBS[:4]):
            class_idx[m['class']].append((pth, m))
            meth_by_name[m['name']].append((pth, m['class'], m))

def layer_of(path):
    if path.startswith('Library.BLL'): return 'Service (BLL)'
    if path.startswith('Library.DAL'): return 'Repository (DAL)'
    if path.startswith('Solution.Web/App_Code'): return 'App_Code service'
    if path.startswith('Solution.Web'): return 'Page code-behind'
    return 'Other'

def resolve_call(path, var, meth, unresolved):
    c = F['cs'].get(path)
    vt = c['vars'] if c else {}
    typ = vt.get(var) or var
    cands = [(p, m) for p, m in class_idx.get(typ, []) if m['name'] == meth]
    if cands:
        return cands, 'CONFIRMED'
    # unique method name fallback among BLL/DAL classes
    alts = meth_by_name.get(meth, [])
    alts = [(p, m) for p, cls, m in alts if p.startswith(('Library.BLL', 'Library.DAL', 'Solution.Web/App_Code'))]
    if len(alts) == 1 and typ not in ('Response', 'Session', 'Request', 'Convert', 'String', 'DateTime', 'Math'):
        return alts, 'INFERRED'
    return [], None

INFRA = {'DataAccessManager', 'DataAccessManagerAsync', 'DataAccessManagerOld', 'SInventorySql', 'ClsCommonInternalDAL', 'ClsPrimaryKeyFind', 'DB_Manager', 'clsNum2Word', 'EncryptDecrypt', 'DB_Connection', 'DB_Authentication'}

def trace(path, mname, depth=4, line=None):
    """Return list of nodes reachable from a method: dict(path, cls, name, line, layer, via, conf, depth, parent_idx)."""
    c = F['cs'].get(path)
    if not c: return []
    ms = [m for m in c['methods'] if m['name'] == mname and (line is None or m['line'] == line)]
    nodes = []
    seen = set()
    def add(p, m, parent, d, conf):
        if m['class'] in INFRA and d > 0: return
        key = (p, m['class'], m['name'], m['line'])
        if key in seen: return
        seen.add(key)
        idx = len(nodes)
        nodes.append({'path': p, 'cls': m['class'], 'name': m['name'], 'line': m['line'], 'layer': layer_of(p), 'depth': d, 'parent': parent, 'conf': conf, 'm': m})
        if d >= depth or len(nodes) > 80: return
        for var, meth in m['calls']:
            cands, cf = resolve_call(p, var, meth, None)
            for p2, m2 in cands[:3]:
                add(p2, m2, idx, d + 1, cf)
        for lc in m['local_calls']:
            for mm in F['cs'][p]['methods']:
                if mm['name'] == lc and mm['class'] == m['class'] and mm is not m:
                    add(p, mm, idx, d + 1, 'CONFIRMED')
    for m in ms[:1]:
        add(path, m, None, 0, 'CONFIRMED')
    return nodes

def node_label(n):
    return f"{n['cls']}.{n['name']} ({n['path']}:{n['line']})"

def chain_of(nodes, i):
    out = []
    while i is not None:
        out.append(nodes[i]); i = nodes[i]['parent']
    return list(reversed(out))

def esc(s):
    return str(s).replace('|', '\\|').replace('\n', ' ').replace('\r', ' ')

def table(headers, rows):
    out = ['| ' + ' | '.join(headers) + ' |', '|' + '|'.join(['---'] * len(headers)) + '|']
    for r in rows:
        out.append('| ' + ' | '.join(esc(x) for x in r) + ' |')
    return '\n'.join(out)

def write(path, text):
    full = os.path.join(ROOT, path)
    os.makedirs(os.path.dirname(full), exist_ok=True)
    with open(full, 'w', encoding='utf-8', newline='\n') as f:
        f.write(text)

# ---------- pages ----------
PAGES = []
for p in F['pages']:
    p['module'] = classify(p['path'])
    p['kind'] = page_kind(p['path'])
    p['name'] = os.path.splitext(os.path.basename(p['path']))[0]
    p['folder'] = p['path'].split('/')[1] if p['path'].count('/') > 1 else '(root)'
    p['cs'] = p['path'] + '.cs' if p['path'] + '.cs' in F['cs'] else None
    PAGES.append(p)
ASPX = sorted([p for p in PAGES if p['ext'] == '.aspx'], key=lambda p: (p['module'], p['folder'], p['name'].lower()))
COMPONENTS = sorted([p for p in PAGES if p['ext'] != '.aspx'], key=lambda p: p['path'])
for i, p in enumerate(ASPX, 1):
    p['id'] = f'PAGE-{i:03d}'
    p['file'] = f"specs/{MOD[p['module']][1]}/pages/{p['id']}-{kebab(p['name'])}.md"
for i, p in enumerate(COMPONENTS, 1):
    p['id'] = f'COMP-{i:03d}'
PAGE_BY_PATH = {p['path']: p for p in PAGES}
PAGE_BY_BASENAME = collections.defaultdict(list)
for p in ASPX:
    PAGE_BY_BASENAME[(p['name'] + '.aspx').lower()].append(p)

def route_of(p):
    return '~/' + p['path'].split('Solution.Web/', 1)[1]

# page -> referenced *.aspx via string literals (navigation graph)
NAV_OUT = collections.defaultdict(set)
NAV_IN = collections.defaultdict(set)
for p in ASPX:
    if not p['cs']: continue
    try:
        with open(os.path.join(ROOT, p['cs']), encoding='utf-8-sig', errors='replace') as f:
            t = f.read()
    except Exception:
        continue
    for m in re.finditer(r'["\']([\w\./~\-]*?([A-Za-z0-9_\- ]+)\.aspx)(?:\?[^"\']*)?["\']', t):
        b = (m.group(2) + '.aspx').lower()
        if b != (p['name'] + '.aspx').lower() and b in PAGE_BY_BASENAME:
            NAV_OUT[p['path']].add(b)
    for c in p['controls']:
        h = c['attrs'].get('NavigateUrl') or c['attrs'].get('href') or ''
        m = re.search(r'([A-Za-z0-9_\- ]+\.aspx)', h)
        if m and m.group(1).lower() in PAGE_BY_BASENAME and m.group(1).lower() != (p['name'] + '.aspx').lower():
            NAV_OUT[p['path']].add(m.group(1).lower())
for src, outs in NAV_OUT.items():
    for b in outs:
        for tgt in PAGE_BY_BASENAME[b]:
            NAV_IN[tgt['path']].add(src)

# ---------- SP / table reverse indexes via traces of every handler ----------
def handlers_of(p):
    if not p['cs']: return []
    c = F['cs'][p['cs']]
    res = []
    for m in c['methods']:
        n = m['name']
        if re.match(r'^(Page_\w+|\w+_\w+)$', n) or m['webmethod']:
            res.append(m)
    return res

CACHE = {}
def page_traces(p):
    if p['path'] in CACHE: return CACHE[p['path']]
    out = {}
    if p['cs']:
        c = F['cs'][p['cs']]
        for m in c['methods']:
            out[(m['name'], m['line'])] = trace(p['cs'], m['name'], line=m['line'])
    CACHE[p['path']] = out
    return out
