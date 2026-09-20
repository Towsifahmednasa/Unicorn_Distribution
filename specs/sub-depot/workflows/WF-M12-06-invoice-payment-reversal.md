# WF-M12-06 Reversals: invoice delete, payment reverse, sales-return delete
- PAGE-440: Proforma -> `sp_Delete_ProformaInvoice_SubDeport`; Invoice -> freeze pre-check on MAIN tables (ineffective) -> `sp_DeleteDeliveryInvoiceSubdepo` (BR-M12-042).
- PAGE-439: by order code: `tblSubInvoiceMaster.PaymentStatus/PaymentAmount = NULL`; delete first `tblCustPayDetail`+`tblCustomerPay` only (BR-M12-037).
- PAGE-437: `ReturnInvoiceDAL.DeleteData` cascade incl. `tblDCStoreFreeze` (BR-M12-044).
Exceptions: partial reversal with multiple payments; no stock restore visible. Tests: reverse a fully paid invoice with two payments -> one payment remains but status reset.
