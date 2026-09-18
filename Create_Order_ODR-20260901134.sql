USE [SalesDisDB_Unicorn_DB];
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
SET NOCOUNT ON;
GO

BEGIN TRANSACTION;

DECLARE @NewOrderId INT;
DECLARE @OrderCode NVARCHAR(100) = 'ODR-20260901134';

-- ১. অর্ডারটি যদি আগে থেকেই থাকে, তাহলে সেটিকে Invoice করার জন্য প্রস্তুত (IsInvoice = 0) করা
IF EXISTS (SELECT 1 FROM dbo.tblOrder WHERE OrderCode = @OrderCode)
BEGIN
    SELECT @NewOrderId = OrderId FROM dbo.tblOrder WHERE OrderCode = @OrderCode;

    UPDATE dbo.tblOrder 
    SET IsInvoice = 0,
        IsPrepareforInvoice = 1,
        ActionStatus = '2',
        GrossValue = 322550.00,
        TotalNetPayable = 322550.00
    WHERE OrderId = @NewOrderId;

    UPDATE dbo.tblOrderDetail 
    SET [Status] = NULL 
    WHERE OrderId = @NewOrderId;

    PRINT 'Order ' + @OrderCode + ' already exists (OrderId: ' + CAST(@NewOrderId AS NVARCHAR(10)) + '). Status reset to IsInvoice = 0 for invoice generation.';
END
ELSE
BEGIN
    -- ২. নতুন অর্ডার হেডার ইনসার্ট (Header Insert)
    INSERT INTO dbo.tblOrder
    (
        OrderCode, ComUnitId, ComUnitCode, ComUnitName,
        MIOCode, MIOName, ManufacId,
        CustomerCode, CustomerName, GrossValue,
        SubmissionDate, IsInvoice, IsManual,
        OrderSenderType, FixedCustomer, CustomerMasterId,
        EntryDate, ActionStatus, IsFromApp,
        Remarks, OrderType, ServerDateTime,
        TotalVat, TotalDiscount, TotalNetPayable,
        CustTypeId, IsSubDepo, IsPrepareforInvoice
    )
    VALUES
    (
        @OrderCode, 12, 'BD02', 'Dhaka Central Warehouse',
        'EMP000055', 'Office', 1,
        'C0000134', 'Akij Pharmacy - Banani', 322550.00,
        '2026-09-01 00:00:00.000', 0, 1,
        'Web Manual', 0, 134,
        '2026-09-01 12:13:47.793', '2', 0,
        'Transfer from Akij outlate Banani', 'Regular', GETDATE(),
        0.00, 0.00, 322550.00,
        1, 0, 1
    );

    SET @NewOrderId = SCOPE_IDENTITY();

    -- ৩. অর্ডারের ১৪টি আইটেম ইনসার্ট (Line Items Insert)
    INSERT INTO dbo.tblOrderDetail
    (
        ProductId, ProductCode, ProductName, Quantity, TradePrice, TotalTradePrice,
        OrderId, Status, ISGiftProduct, DiscountPercent, DiscountAmount,
        UnitVatAmount, TotalVatAmount, NetAmount, IsCampaignProduct, IsSpDis
    )
    VALUES
    (34, 'HEM-7120-AP', 'Blood Pressure Monitor HEM-7120-AP', 5, 3150.00, 15750.00, @NewOrderId, NULL, 0, 0, 0, 0, 0, 15750.00, 0, 0),
    (35, 'HEM-7121-AP', 'Blood Pressure Monitor HEM-7121-AP', 2, 3300.00, 6600.00, @NewOrderId, NULL, 0, 0, 0, 0, 0, 6600.00, 0, 0),
    (38, 'HEM-7156T-AP', 'Blood Pressure Monitor HEM-7156T-AP', 7, 5000.00, 35000.00, @NewOrderId, NULL, 0, 0, 0, 0, 0, 35000.00, 0, 0),
    (50, 'HEM-CR24-BAP', 'Medium Cuff HEM-CR24-BAP', 6, 900.00, 5400.00, @NewOrderId, NULL, 0, 0, 0, 0, 0, 5400.00, 0, 0),
    (48, 'HHP-CM01-AP', 'AC Adapter HHP-CM01-AP', 20, 850.00, 17000.00, @NewOrderId, NULL, 0, 0, 0, 0, 0, 17000.00, 0, 0),
    (42, 'NE-C28-C1', 'Compressor Nebulizer NE-C28-C1', 9, 4800.00, 43200.00, @NewOrderId, NULL, 0, 0, 0, 0, 0, 43200.00, 0, 0),
    (43, 'NE-C101-AP', 'Compressor Nebulizer NE-C101-AP', 14, 2900.00, 40600.00, @NewOrderId, NULL, 0, 0, 0, 0, 0, 40600.00, 0, 0),
    (55, '9956270-0', 'Connector Hose Pvc, Air Tube', 6, 300.00, 1800.00, @NewOrderId, NULL, 0, 0, 0, 0, 0, 1800.00, 0, 0),
    (51, 'HEM-CS24-BAP', 'Small Cuff HEM-CS24-BAP', 5, 1000.00, 5000.00, @NewOrderId, NULL, 0, 0, 0, 0, 0, 5000.00, 0, 0),
    (36, 'HEM-7142T2-AP2', 'Blood Pressure Monitor HEM-7142T2-AP2', 1, 3700.00, 3700.00, @NewOrderId, NULL, 0, 0, 0, 0, 0, 3700.00, 0, 0),
    (39, 'HEM-7157-AP3 [JPN750]', 'Blood Pressure Monitor HEM-7157-AP3 [JPN750]', 4, 5600.00, 22400.00, @NewOrderId, NULL, 0, 0, 0, 0, 0, 22400.00, 0, 0),
    (45, 'MC-246-AP4', 'Digital Thermometer MC-246-AP4', 10, 520.00, 5200.00, @NewOrderId, NULL, 0, 0, 0, 0, 0, 5200.00, 0, 0),
    (37, 'HEM-7143T1-AP', 'Blood Pressure Monitor HEM-7143T1-AP', 24, 4100.00, 98400.00, @NewOrderId, NULL, 0, 0, 0, 0, 0, 98400.00, 0, 0),
    (46, 'HN-300T2-AP', 'Weight Scale HN-300T2-AP', 5, 4500.00, 22500.00, @NewOrderId, NULL, 0, 0, 0, 0, 0, 22500.00, 0, 0);

    PRINT 'Order ' + @OrderCode + ' and 14 items created successfully with OrderId: ' + CAST(@NewOrderId AS NVARCHAR(10));
END

COMMIT TRANSACTION;

-- ফলাফল যাচাই
SELECT 
    O.OrderId, 
    O.OrderCode, 
    O.CustomerName, 
    O.GrossValue, 
    O.IsInvoice,
    COUNT(OD.OrderDetailId) AS TotalItems
FROM dbo.tblOrder O
INNER JOIN dbo.tblOrderDetail OD ON O.OrderId = OD.OrderId
WHERE O.OrderCode = @OrderCode
GROUP BY O.OrderId, O.OrderCode, O.CustomerName, O.GrossValue, O.IsInvoice;
GO
