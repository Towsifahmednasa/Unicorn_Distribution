# INT-10 - REST base URL configuration (`SqlUserAccess.BASE_URL` / `AppName`)

| Item | Detail |
|---|---|
| Location | `Library.DAL/DataManager/SqlUserAccess.cs:84-85`: `public static string AppName = "CSTL-Development"; public static string BASE_URL = "http://<external IP>:570";` (plain HTTP, hard-coded IP and port). The same file also holds hard-coded SQL credentials (`<redacted>`; only mention of their existence). |
| Usage | **None.** Grep across `*.cs`, `*.aspx`, `*.config`, `*.master` finds no reference to `BASE_URL` or `SqlUserAccess.AppName` other than the declaration. No `HttpClient`/`WebClient` caller uses it. |
| Interpretation | Leftover of a REST/app-API client (INFERRED); dead configuration. |
| Risks | Plain HTTP endpoint and environment name compiled into the assembly; may be used by external code not in the repo (UNKNOWN). |
| Other outbound HTTP found in repo | `App_Code/UserSessionTrackingManager.cs:495-520` calls `https://ipapi.co/<client ip>/json/` (GET, 3 s timeout, User-Agent `SMC-Session-Tracker`, errors ignored) at login (`Login.aspx.cs:48`) to fetch country/region/city; sends the user's public IP to a third party. `BankDepositSAP` HTTP block is disabled (INT-04). |
