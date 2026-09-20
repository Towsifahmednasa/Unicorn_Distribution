# WF-M17-03 - Bank deposit to SAP (currently semi-manual)

Trigger/actors: finance user on `BankDepositSAP.aspx`. BR-M17-013/014. CONFIRMED.

Steps
1. Choose date range (>= 01-Apr-2022 enforced by auto-reset).
2. View -> `BankDeposit_SAP_Process` (`sp_SAP_BankDeposit_SAP_Process`, write-like) -> `LoadBankDepositSAP` (`sp_SAP_BankDepositPosting`) -> grid.
3. Export CSV (`Bank Deposit (SAP)_<ts>.csv`) for manual upload, **or** Final Submit: builds `{"Posting":[...]}` JSON; HTTP POST and `sp_SAP_BankDepositSendtoSAP` are commented out, so nothing is sent or flagged.

Exceptions: search errors swallowed; empty -> message. Postcondition: rows remain unflagged (re-listable). Tests: TC-686-01..07. Pages: 686. See INT-04.
