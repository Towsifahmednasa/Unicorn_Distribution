# WF-M12-05 Sub-depot invoicing, delivery and payment
1. Order with `IsSubDepo=1` on a sub-depot route (`RouteInformationEntry` chkIsSubDepo, `RouteInformationEntry.aspx.cs:809`).
2. PAGE-456: search `OrderInfoDAL.subDepoOrderLoad` (BR-M12-038); Generate -> `sp_Process_SubDepoProformaInvoiceByOrderId` batch `06-n` (BR-M12-039/040); PDF RPT-M12-03.
3. PAGE-457/462 lists: proforma print, top sheet (RPT-M12-05), picking (PAGE-447, RPT-M12-05).
4. PAGE-444 delivery confirmation: Full -> `sp_SubdeportDeliveryConformationFull`; Reject -> `sp_SubDeportDeliveryConformationReject`; Partial -> excluded page (BR-M12-041); PAGE-445 delivery invoice/top sheet (RPT-M12-04).
5. PAGE-452 payment: list `LoadSubDeportInvoice` (BR-M12-032) -> validate (033/034) -> `SubdeportUpdateInvoicePaymentAmount` + INSERT `tblCustomerPay`/`tblCustPayDetail.SubDeportInvoiceId` (035/036).
**State**: order IsInvoice 0->1; proforma DelivaryInvoiceNo NULL -> set; DeliveryInvoiceStatus Full/Partial/Reject; PaymentStatus NULL->Partial->Full. Stock deduction points UNKNOWN (SPs). Exceptions: swallowed SP errors, payment page hidden-control defect. Tests: PAGE-456 TC-01..08, PAGE-452 TC-01..10.
