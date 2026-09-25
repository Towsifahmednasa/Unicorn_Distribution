# 🧪 Test Plan — `saveButton_Click` TransactionScope (InvoiceCreationForCustomerByOrder.aspx)

Ei document ta tumi nijei run korbe. Sob test **dev DB** te korbe (Web.config e ekhon `TOWSIF\MSSQLSERVER2019` / `SalesDisDB_Unicorn_DB`), kokhono production e na.

## 📌 Assumptions (ami code theke inference korechi, tumi verify koro)

| # | Assumption | Kivabe confirm korbe |
|---|---|---|
| A1 | "single connection" na, ekhon **onek connection ek ek kore open-close** hoy (`SInventorySql.Execute`, `PrimaryKeyMax`, `InvoiceNoCount` prottek call e notun connection). `TransactionScope` egulo ke ekta transaction e join kore. Pooling e same physical connection reuse hole MSDTC lage na | Test 3 |
| A2 | `tblInvoice.InvoiceId` ar `tblInvoiceDetail.InvoiceDetailId` **IDENTITY** (INSERT e ei column nai). Rollback er por ID te gap thakbe, eta **fail na** | `SELECT COLUMNPROPERTY(OBJECT_ID('tblInvoice'),'InvoiceId','IsIdentity')` |
| A3 | `tblDCStore.DCStoreId` MAX+1 (identity na), rollback er por gap hobe na | code: `UpdateDCStoreQuantity` |
| A4 | `tblOrderDetail` e `OrderId` column ache | `SELECT TOP 1 * FROM tblOrderDetail` |
| A5 | `InvoiceNo` = `INV-<code><MAX(InvoiceId)+1 padded>` (`InvoiceDAL.InvoiceNoCount`). Rollback er por retry te same invoice no ashbe | Test 6 |
| A6 | Runtime .NET Framework 4.x (Web.config `targetFramework="4.0"`), SQL Server 2019 | `SELECT @@VERSION` |

---

## 1️⃣ Test Environment Setup

### 1.1 Ki ki lagbe
- [ ] SQL Server 2019 dev instance + SSMS
- [ ] Website chalanor jonno VS / IIS Express (project e .csproj nai, Website open kore run)
- [ ] **2ta eligible order** (Order A = Test 1 er jonno, Order B = rollback test gulor jonno). Test 1 successful hole order ta consume hoye jay, tai alada order dorkar
- [ ] Order er kom pokkhe ekta line e `currentStock > 0` (na hole line `Undelivered` hoye jay, invoice detail hoy na)

### 1.2 Eligible order khuje ber kora
```sql
USE SalesDisDB_Unicorn_DB;
SELECT TOP 20 o.OrderId, o.OrderCode, o.IsInvoice
FROM tblOrder o
WHERE NOT EXISTS (SELECT 1 FROM tblInvoice i WHERE i.OrderId = o.OrderId)  -- OrderExists() guard ei query
ORDER BY o.OrderId DESC;
```
UI (`InvoiceCreationForCustomerByOrder.aspx`) e jei order load hoy, tar `OrderId` note koro: **Order A = ______ , Order B = ______**

### 1.3 MSDTC status (informational)
```powershell
Get-Service MSDTC          # Status = Running kina
```
MSDTC **chalu thaka mane ei na je lagche**. Transaction promote hoyeche kina Test 3 e dekhbe.

MSDTC network access on korte hole (shudhu dev, Test 3 e error ashle): `dcomcnfg` → Component Services → Computers → My Computer → Distributed Transaction Coordinator → Local DTC → Properties → Security → tick **Network DTC Access**, **Allow Inbound**, **Allow Outbound** → OK → service restart.

### 1.4 Pre-test backup (must)
```sql
BACKUP DATABASE SalesDisDB_Unicorn_DB
TO DISK = N'D:\Backup\SalesDisDB_Unicorn_DB_PreTxTest.bak'
WITH COPY_ONLY, INIT, COMPRESSION;
```
Restore (Test 1 / Test 6 er cleanup e lagbe):
```sql
USE master;
ALTER DATABASE SalesDisDB_Unicorn_DB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
RESTORE DATABASE SalesDisDB_Unicorn_DB
FROM DISK = N'D:\Backup\SalesDisDB_Unicorn_DB_PreTxTest.bak' WITH REPLACE;
ALTER DATABASE SalesDisDB_Unicorn_DB SET MULTI_USER;
```
> Website ke restore er age stop koro (IIS Express / VS debug bondho), na hole connection thakle restore atke jabe.

### 1.5 Snapshot / Compare script (SSMS e **same query window** e rakho)
Ei script **2 bar** chalabe: prothom bar = BEFORE snapshot save, dwitiyo bar = compare. Notun test e ashar age `DROP TABLE ##Snap;` koro.
```sql
USE SalesDisDB_Unicorn_DB;
IF OBJECT_ID('tempdb..##Now') IS NOT NULL DROP TABLE ##Now;

SELECT t, cnt INTO ##Now FROM (
    SELECT 'tblInvoice' AS t, COUNT_BIG(*) AS cnt FROM tblInvoice
    UNION ALL SELECT 'tblInvoiceDetail',  COUNT_BIG(*) FROM tblInvoiceDetail
    UNION ALL SELECT 'tblInvoiceBatch',   COUNT_BIG(*) FROM tblInvoiceBatch
    UNION ALL SELECT 'tblDCStore (all)',  COUNT_BIG(*) FROM tblDCStore
    UNION ALL SELECT 'tblDCStore (Issue)',COUNT_BIG(*) FROM tblDCStore WHERE Trans_Type = 'Issue'
    UNION ALL SELECT 'tblDCStore MAX(DCStoreId)', ISNULL(MAX(DCStoreId),0) FROM tblDCStore
    UNION ALL SELECT 'tblOrder IsInvoice=1', COUNT_BIG(*) FROM tblOrder WHERE CAST(IsInvoice AS VARCHAR(5)) IN ('1','True')
    UNION ALL SELECT 'tblOrderDetail Undelivered', COUNT_BIG(*) FROM tblOrderDetail WHERE Status = 'Undelivered'
    UNION ALL SELECT 'orphan tblInvoiceDetail', COUNT_BIG(*) FROM tblInvoiceDetail d
              WHERE NOT EXISTS (SELECT 1 FROM tblInvoice i WHERE i.InvoiceId = d.InvoiceId)
    UNION ALL SELECT 'orphan tblInvoiceBatch', COUNT_BIG(*) FROM tblInvoiceBatch b
              WHERE NOT EXISTS (SELECT 1 FROM tblInvoice i WHERE CAST(i.InvoiceId AS VARCHAR(20)) = CAST(b.InvoiceId AS VARCHAR(20)))
) x;

IF OBJECT_ID('tempdb..##Snap') IS NULL
BEGIN
    SELECT * INTO ##Snap FROM ##Now;
    SELECT 'SNAPSHOT SAVED - ekhon UI theke save koro, tarpor abar ei script chalao' AS msg;
END
ELSE
    SELECT s.t, s.cnt AS [Before], n.cnt AS [After], n.cnt - s.cnt AS Diff
    FROM ##Snap s JOIN ##Now n ON n.t = s.t;
```
Per-order state (Order id boshao):
```sql
DECLARE @OrderId INT = <ORDER_ID>;
SELECT OrderId, IsInvoice FROM tblOrder WHERE OrderId = @OrderId;
SELECT OrderDetailId, Status FROM tblOrderDetail WHERE OrderId = @OrderId;
SELECT COUNT(*) AS InvoiceCntForOrder FROM tblInvoice WHERE OrderId = @OrderId;
```
`sp_Deletenvoice` baseline (Test 2 e lagbe):
```sql
SELECT OBJECT_NAME(object_id, database_id) AS sp, execution_count, last_execution_time
FROM sys.dm_exec_procedure_stats
WHERE database_id = DB_ID() AND OBJECT_NAME(object_id, database_id) = 'sp_Deletenvoice';
-- 0 row = cache e kokhono cholenai (0 execution dhoro)
```

### 1.6 Temporary code rule
Prottek temporary change e comment dao `// TXTEST`. Sob test sesh e:
- [ ] `Ctrl+Shift+F` → `TXTEST` → **0 result** (sob temp code remove)

### 1.7 Smoke check (Test 0)
- [ ] Page open koro. **Compile error / "assembly System.Transactions not found"** ashle Web.config assemblies line ta check koro
- [ ] Order load kore page thik dekhay

---

## 2️⃣ Test Cases

### ✅ Test 1 — Happy Path (Normal Save)

**Objective:** Sob write commit hoy, MSDTC error ashe na.
**Prerequisite:** Order A, temp code nai (`TXTEST` = 0).

**Steps**
1. [ ] Snapshot script chalao (1st run → `SNAPSHOT SAVED`).
2. [ ] UI e Order A load koro, **Save** click koro.
3. [ ] "Invoice Created Successsfully!" alert dekhbe. (Ekhane "Successsfully" e 3ta `s` code er typo, existing)
4. [ ] Snapshot script abar chalao (compare).
5. [ ] Detail verify query chalao:
```sql
DECLARE @OrderId INT = <ORDER_A>;
DECLARE @InvId  INT = (SELECT InvoiceId FROM tblInvoice WHERE OrderId = @OrderId);
DECLARE @MaxBefore BIGINT = (SELECT cnt FROM ##Snap WHERE t = 'tblDCStore MAX(DCStoreId)');

SELECT * FROM tblInvoice       WHERE InvoiceId = @InvId;
SELECT * FROM tblInvoiceBatch  WHERE CAST(InvoiceId AS VARCHAR(20)) = CAST(@InvId AS VARCHAR(20));
SELECT * FROM tblInvoiceDetail WHERE InvoiceId = @InvId;

-- Header total = detail sum (UpdateInvoiceHeaderTotals kaj korlo kina)
SELECT i.TpTotal, d.SumTotalPrice, i.TpDiscount, d.SumDisc, i.TpVat, d.SumVat,
       i.TpGrandTotal, d.SumNet + ISNULL(i.DeliveryCharge,0) AS ExpectedGrand
FROM tblInvoice i
CROSS APPLY (SELECT SUM(TotalPrice) SumTotalPrice, SUM(DiscountAmount) SumDisc,
                    SUM(TotalPriceVatAmount) SumVat, SUM(NetAmount) SumNet
             FROM tblInvoiceDetail WHERE InvoiceId = i.InvoiceId) d
WHERE i.InvoiceId = @InvId;

-- DCStore Issue rows (negative StockQty)
SELECT DCStoreId, ProductCode, BatchNo, StockQty, Trans_Type, ReferenceDCStoreId
FROM tblDCStore WHERE DCStoreId > @MaxBefore ORDER BY DCStoreId;
SELECT -SUM(StockQty)  AS IssuedQty   FROM tblDCStore WHERE DCStoreId > @MaxBefore;
SELECT SUM(TotalQuantity) AS InvoicedQty FROM tblInvoiceDetail WHERE InvoiceId = @InvId;

SELECT OrderId, IsInvoice FROM tblOrder WHERE OrderId = @OrderId;
SELECT OrderDetailId, Status FROM tblOrderDetail WHERE OrderId = @OrderId;
```

**Expected (👀 kী dekhbe)**
| Item | Expected |
|---|---|
| `tblInvoice` Diff | **+1** |
| `tblInvoiceBatch` Diff | **+1** |
| `tblInvoiceDetail` Diff | **+N** (N ≥ stock thaka line sonkha; batch split hole beshi) |
| `tblDCStore (Issue)` Diff | **= tblInvoiceDetail Diff** (code e proti detail row er sathe 1ta Issue row, INFERRED) |
| `tblOrder IsInvoice=1` Diff | **+1**, `IsInvoice` = 1/True |
| `orphan *` Diff | **0** |
| Header vs detail | `TpTotal = SumTotalPrice`, `TpGrandTotal = ExpectedGrand` |
| `IssuedQty` | `= InvoicedQty` |
| `SaveReturnAmount` | `tblReturnAmount` e notun row **nai** (comment kora) |

**❌ Fail hole (kী hole fail)**
- Kono table Diff 0 (commit hoyni) ba beshi/kom
- Success alert er jaygay "Invoice generation failed: ..." dekhay
- Alert e MSDTC / distributed transaction / `Timeout expired` message
- Page **hang** kore ba 30s+ por timeout (Test 3 dekho)
- Invoice detail Diff ≠ DCStore Issue Diff → report koro (ami code check korbo)

**Pass criteria:** sob Expected row match + kono error nai.
**Cleanup:** DB restore (1.4). Ba Order A er data rakhte chaile Test 6 er retry check er jonno rekhe dao.

---

### ✅ Test 2 — Rollback Test

**Objective:** Sob step complete howar por o exception dile **kichu-i thake na**.
**Prerequisite:** Order B, snapshot fresh (`DROP TABLE ##Snap;`).

**Temporary code** (`aOrderInfoBll.UpdateInvoiceStatus(...)` line er **thik porey**, `scope.Complete()` er age):
```csharp
aOrderInfoBll.UpdateInvoiceStatus(orderIdHiddenField.Value);
throw new Exception("test"); // TXTEST
```
> 💡 Tumi bolechile `UpdateInvoiceHeaderTotals` er por. Kintu `UpdateInvoiceStatus` (`tblOrder.IsInvoice`) tar **por** cholay, oi jayga theke throw korle `IsInvoice` toh kokhono update-i hoyni, tai rollback proof hoy na. `UpdateInvoiceStatus` er por throw korle **4ta step-i** age theke commit-er moto hoye rollback hoy.

**Steps**
1. [ ] `sp_Deletenvoice` baseline query chalao, `execution_count` note: `____`
2. [ ] Snapshot 1st run (BEFORE).
3. [ ] Order B load kore **Save** click.
4. [ ] Alert dekhbe: `Invoice generation failed: test` (success alert **na**).
5. [ ] Snapshot 2nd run (compare) + per-order state query chalao.
6. [ ] `sp_Deletenvoice` query abar chalao.

**Expected**
| Item | Expected |
|---|---|
| `tblInvoice`, `tblInvoiceBatch`, `tblInvoiceDetail` Diff | **0** |
| `tblDCStore (all)`, `(Issue)` Diff | **0**, `MAX(DCStoreId)` unchanged |
| `tblOrder IsInvoice=1` Diff | **0**, Order B er `IsInvoice` age jemon chilo temon |
| `tblOrderDetail Undelivered` Diff | **0**, line status unchanged |
| `orphan *` Diff | **0** |
| `InvoiceCntForOrder` | **0** |
| `sp_Deletenvoice execution_count` | **unchanged** (code e `DeleteInvoice` call soriyechi) |
| Alert | `Invoice generation failed: test` |

**❌ Fail hole**
- Kono table e +1 ba beshi row (rollback hoyni)
- `IsInvoice` change hoye gache
- `sp_Deletenvoice` count barse
- Success alert dekhay

**ℹ️ Known side effect (fail na):** `SaveInvoice()` `invTextBox.Text` e invoice no set kore, exception er por o page render e ei no dekhabe, kintu DB te ei invoice nai. Confusing, kintu bug na. Fix chaile bolo.

**Pass criteria:** sob Diff = 0, `sp_Deletenvoice` unchanged, `InvoiceCntForOrder = 0`.
**Cleanup:** `throw` line delete kore `TXTEST` remove koro. Order B abar reuse kora jabe.

---

### ✅ Test 3 — MSDTC Detection

**Objective:** Transaction distributed (MSDTC) e promote hocche kina ber kora, ar error ashle ki kora.

**Part A — Silent promotion check** (MSDTC chalu thakleo promote hote pare, error ashbe na, kintu slow/fragile)
Temporary code, `scope.Complete();` er **age**:
```csharp
// TXTEST
System.IO.File.AppendAllText(Server.MapPath("~/txtest.log"),
    DateTime.Now + " promoted=" +
    (Transaction.Current.TransactionInformation.DistributedIdentifier != Guid.Empty) + Environment.NewLine);
```
Steps: [ ] Order B diye Save koro (Test 2 er throw **remove** kore, tai ei Save **real commit** korbe — tai Order B er jaygay **Order C** ba restore-er por Order B use koro) → [ ] `txtest.log` (website root) kholo.

| Result | Meaning |
|---|---|
| `promoted=False` | ✅ Lightweight transaction, MSDTC lagche na. Plan A thik ache |
| `promoted=True` | ⚠️ MSDTC use hocche. Ekhon kaj korle o production e MSDTC enable lagbe. Amake report koro, Plan B (Section 5) niye decide korbo |

Extra cross-check: `dcomcnfg` → Component Services → Distributed Transaction Coordinator → Local DTC → **Transaction Statistics**. Save er age/por **Total / Committed** count barle MSDTC use hoyeche.

**Part B — Error message identification**
Kono error page e/alert e (`Invoice generation failed: ...`) ei dhoroner message ashle MSDTC problem (exact text version bhede alada hote pare, amake **exact text copy** kore dao):
- `MSDTC on server '<name>' is unavailable.`
- `Network access for Distributed Transaction Manager (MSDTC) has been disabled...`
- `The partner transaction manager has disabled its support for remote/network transactions. (Exception from HRESULT: 0x8004D025)`
- `Communication with the underlying transaction manager has failed.`
- `The operation is not valid for the state of the transaction.` / `Transaction Timeout` (promote hoye timeout)

**Pass:** Part A te `promoted=False` + Part B er moto kono error nai.

**Error ashle ki korbe**
1. [ ] Exact message + full stack trace (browser alert shudhu message dey; stack er jonno temporary `catch` e `ex.ToString()` log koro) save koro.
2. [ ] `Get-Service MSDTC` → Running kina, na hole `Start-Service MSDTC`.
3. [ ] 1.3 er moto Network DTC Access on kore service restart, abar try koro.
4. [ ] Kaj korle o `promoted=True` hole → Plan B (Section 5), amake janao.

**Cleanup:** `TXTEST` remove, `txtest.log` delete.

> ⚠️ Alada ekta symptom: **page hang / `Timeout expired. The timeout period elapsed...`** — mane ekta connection er uncommitted write ke arekta connection (different physical connection) read korte chaiche ar lock e atke gache (`SelectInvoiceID`, `PrimaryKeyMax`). Eta MSDTC message na, kintu ekoi root cause (multiple connection). Same treat koro, report koro.

---

### ✅ Test 4 — `dtl == false` Branch

**Objective:** `SaveInvoiceDetail` false dile throw → rollback → fail alert (success na).
**Prerequisite:** Order B, snapshot fresh. Test 2 er `throw` remove kora.

**Temporary code** — `SaveInvoiceDetail()` er sheshe:
```csharp
    aInvoiceBll.SaveInvoiceDetails(aInvoiceDetailsList, hdComUnitId.Value);
}

return false; // TXTEST (chilo: return true;)
```
> 💡 `SaveInvoiceDetails` (tblInvoiceDetail + tblDCStore insert) **age-i** cholay, tarpor `false` return hoy. Tai ei test proof kore je already-inserted detail/DCStore row o rollback hoy.

**Steps**
1. [ ] Snapshot BEFORE. 2. [ ] Order B Save. 3. [ ] Alert dekho. 4. [ ] Snapshot compare + per-order state.

**Expected**
- Alert: `Invoice generation failed: Please generate Invoice again!` ✅
- "Invoice Created Successsfully!" alert **dekhabe na**
- Form **clear hobe na** (catch e `Clear()` nai; age `else` e chilo) — eta expected
- Test 2 er moto sob Diff = 0 (**special:** `tblInvoiceDetail` ar `tblDCStore (Issue)` Diff o 0, karon insert hoyechilo)
- `IsInvoice` unchanged

**❌ Fail hole**
- Success alert dekhay
- `tblInvoiceDetail` / `tblDCStore` e row theke jay (partial commit)
- Alert e onno message (jemon NullReference) mane onno jaygay fail hocche, message amake dao

**Pass criteria:** Alert + Diff 0 + success alert nai.
**Cleanup:** `return true;` ferot dao, `TXTEST` remove.

---

### ✅ Test 5 — Timeout Boundary (optional)

**Objective:** Timeout hole transaction abort hoy ar kichu commit hoy na.
Real 5 minute wait korar dorkar nai, temporary choto kore test koro.

**Temporary code**
```csharp
var txOpt = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                                     Timeout = TimeSpan.FromSeconds(10) }; // TXTEST (chilo 5 minute)
using (var scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
{
    int invId = SaveInvoice();
    System.Threading.Thread.Sleep(15000); // TXTEST
    ...
```
**Steps:** [ ] Snapshot BEFORE → [ ] Order B Save → [ ] ~15 sec wait → [ ] alert + Snapshot compare.

**Expected**
- Alert: `Invoice generation failed: ...` (message e typically *transaction aborted / timeout / "The transaction has aborted"*; exact text version bhede alada, amake copy kore dao)
- Sob Diff = 0, `InvoiceCntForOrder = 0`

**❌ Fail hole:** kono row theke jay (invoice header thake) ba success alert.

**Pass criteria:** error + Diff 0.
**Cleanup:** `Timeout` `TimeSpan.FromMinutes(5)` e ferot dao (**must**), `Sleep` remove, `TXTEST` remove.

> Note: alada 30s `SqlCommand` timeout ache prottek query er, egulo TransactionScope timeout theke independent.

---

### ✅ Test 6 — Post-rollback Consistency

**Objective:** Fail er por orphan nai, retry thik moto kaj kore.
**Prerequisite:** Test 2/4/5 er poreo Order B **ekhono uninvoiced**, sob temp code remove.

**Steps**
1. [ ] Snapshot script chalao — `orphan tblInvoiceDetail` / `orphan tblInvoiceBatch` Diff (Test 2/4/5 er BEFORE er sathe) = **0**.
2. [ ] Extra orphan checks:
```sql
DECLARE @OrderId INT = <ORDER_B>;
-- Invoice header ache kintu detail nai (half-saved)
SELECT i.InvoiceId, i.InvoiceNo FROM tblInvoice i
WHERE i.OrderId = @OrderId AND NOT EXISTS (SELECT 1 FROM tblInvoiceDetail d WHERE d.InvoiceId = i.InvoiceId);
-- Issue row jar sathe invoice detail nai (rollback er por thaka uchit na)
SELECT TOP 20 s.DCStoreId, s.ProductCode, s.StockQty
FROM tblDCStore s
WHERE s.Trans_Type = 'Issue' AND s.DCStoreId > (SELECT cnt FROM ##Snap WHERE t = 'tblDCStore MAX(DCStoreId)')
ORDER BY s.DCStoreId;
```
   Duitai **0 row** hote hobe.
3. [ ] Fail attempt e UI te je `invTextBox` no dekhiyechilo, **note** koro: `INV-...______`
4. [ ] Snapshot fresh (`DROP TABLE ##Snap;` → run) → Order B te **normal Save** (temp code nai).
5. [ ] Success alert + Test 1 er Expected table match.
6. [ ] Invoice no compare: `SELECT InvoiceNo, InvoiceId FROM tblInvoice WHERE OrderId = <ORDER_B>;`

**Expected**
- Retry **success**, Test 1 er moto Diff
- `InvoiceNo` = step 3 er note kora no (A5, rollback e MAX(InvoiceId) barenai)
- `InvoiceId` e **gap thakte pare** (IDENTITY, A2) → eta ✅ **fail na**

**❌ Fail hole**
- Step 2 te kono row
- Retry te "Invoice already Generated !!" (`OrderExists` true → rollback hoyni)
- Retry te duplicate key error
- Invoice no alada (info, fail na, kintu amake bolo)

**Pass criteria:** orphan 0 + retry success.
**Cleanup:** DB restore (1.4) ba Order B rekhe dao.

---

## 3️⃣ Test Summary

| # | Test | Expected | Status |
|---|------|----------|--------|
| 0 | Page load / build | Compile error nai | ⬜ |
| 1 | Happy path | Sob table commit, no MSDTC error | ⬜ |
| 2 | Rollback (throw after UpdateInvoiceStatus) | Sob Diff 0, `sp_Deletenvoice` unchanged | ⬜ |
| 3A | Promotion check | `promoted=False` | ⬜ |
| 3B | MSDTC error identify | Kono error nai | ⬜ |
| 4 | `dtl == false` | Fail alert, rollback, no success alert | ⬜ |
| 5 | Timeout (optional) | Abort, Diff 0 | ⬜ |
| 6 | Consistency + retry | Orphan 0, retry success | ⬜ |
| — | `TXTEST` = 0 result, Timeout = 5 min, `return true` restored | Cleaned | ⬜ |

Legend: ⬜ not run · ✅ pass · ❌ fail

---

## 4️⃣ Report Back Criteria — Amake (Claude ke) kokhon janabe

Ei situation gulote **exact message / screenshot / query output** soho janao:
- [ ] Test 1 e kono error alert (vitor **full alert text**)
- [ ] `promoted=True` (Test 3A)
- [ ] MSDTC related kono message (Test 3B)
- [ ] Page hang / `Timeout expired` jekono test e
- [ ] Rollback test e kono table e Diff ≠ 0 (kon table + koto)
- [ ] `sp_Deletenvoice execution_count` barse
- [ ] Invoice detail Diff ≠ DCStore Issue Diff (Test 1)
- [ ] Retry fail (Test 6)
- [ ] Onno kono unexpected alert message (jemon NullReference, `Invalid object`)

Report e ei 4ta ta dio: (1) kon test, (2) exact alert text, (3) compare script output, (4) SQL Server version (`SELECT @@VERSION`).

---

## 5️⃣ Fallback Plan (Plan B) — `TransactionScope` → `SqlTransaction`

**Kokhon:** Test 3A `promoted=True` hole ba MSDTC error/lock timeout hole.

**Idea:** Save er somoy ekta connection + `SqlTransaction` request er `HttpContext.Items` e rakho; DAL helper gulo oi current transaction thakle seta-i use kore, na thakle ager moto notun connection kholey.

> ⚠️ Eta **sketch**, ami compile/run kori nai. Website e C# 5 compiler dhore classic syntax use korechi. Koto file bodlate hobe ta niche.

`Library.DAL/SInventory_DAL/TxContext.cs` (notun):
```csharp
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace Library.DAL.SInventory_DAL
{
    public static class TxContext
    {
        private const string Key = "SInv.Tx";

        public static SqlTransaction Current
        {
            get { return HttpContext.Current.Items[Key] as SqlTransaction; }
        }

        public static void Begin()
        {
            string cs = ConfigurationManager.ConnectionStrings["SolutionConnectionStringSSIDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(cs);
            conn.Open();
            HttpContext.Current.Items[Key] = conn.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public static void Commit()   { End(true);  }
        public static void Rollback() { End(false); }

        private static void End(bool commit)
        {
            SqlTransaction tx = Current;
            if (tx == null) return;
            SqlConnection conn = tx.Connection;
            try { if (commit) tx.Commit(); else tx.Rollback(); }
            finally { conn.Close(); HttpContext.Current.Items.Remove(Key); }
        }
    }
}
```
`SInventorySql.Execute` / `GetDataTable` e hook (ekhon connection ei class e na, `ClsCommonInternalDAL.UpdateDataByUpdateCommandNew` / `DataAccessManager` e khule; **oi 2ta method e** ekoi check boshate hobe):
```csharp
SqlTransaction tx = TxContext.Current;
SqlConnection conn = (tx != null) ? tx.Connection : OpenNewConnection();
using (SqlCommand cmd = new SqlCommand(query, conn, tx))   // tx == null hole normal
{
    cmd.Parameters.AddRange(parameters.ToArray());
    cmd.ExecuteNonQuery();                                 // Execute()
}
// tx == null hole ager moto conn.Close() korbe; tx != null hole connection KHULLE RAKHBE
// (OpenNewConnection = ager code jekhane connection khole, ekhane pseudo-name)
```
`saveButton_Click` (TransactionScope er jaygay):
```csharp
try
{
    TxContext.Begin();
    int invId = SaveInvoice();
    ...                                    // SaveInvoiceDetail, UpdateInvoiceHeaderTotals, UpdateInvoiceStatus
    TxContext.Commit();
}
catch (Exception ex)
{
    TxContext.Rollback();
    ...                                    // fail alert
}
```
**Kon jaygay hook lagbe (grep kore ber korte hobe, ami korbo):**
1. `SInventorySql.Execute` / `GetDataTable` (→ `ClsCommonInternalDAL.UpdateDataByUpdateCommandNew`, `DataAccessManager.SqlConnectionOpen/Close`)
2. `ClsPrimaryKeyFind.PrimaryKeyMax` (→ `ClsCommonInternalDAL.DataContainerDataTable`)
3. `InvoiceDAL.InvoiceNoCount`, `LoadInvoiceBatchId`, `BatchWiseProductQty`, `ProductInfo`, `Isgift` (jei jei method ei save flow te read kore ar `DataContainerDataTable` / `SInventorySql.GetDataTable` use kore)

Ei duita choke hook korle prai sob flow cover hoy, tai file count kom, kintu **shared connection helper** bodlate hobe, tai onno page (jegulo ei helper use kore) o affect hote pare, regression test lagbe.

---

## 6️⃣ Final Checklist (sob test sesh e)

- [ ] `TXTEST` search → 0 result
- [ ] `Timeout = TimeSpan.FromMinutes(5)` ache
- [ ] `SaveInvoiceDetail` `return true;`
- [ ] `throw new Exception("test")` nai
- [ ] `txtest.log` delete
- [ ] DB restore (jodi Test 1/6 er data rakhbe na)
- [ ] `git diff -- Solution.Web/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx.cs Solution.Web/Web.config` e shudhu TransactionScope + `System.Transactions` change thake (repo te onno uncommitted file o ache, segulo alada)
