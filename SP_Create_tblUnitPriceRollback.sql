-- Product Unit Price: Inactive korle row tblUnitPrice theke delete hoye ekhane insert hobe
-- Used by: SInventory_UI/ProUnitPriceEntry.aspx -> ProductUnitPriceDAL.MoveUnitPriceToRollback

IF OBJECT_ID('dbo.tblUnitPriceRollback', 'U') IS NULL
BEGIN
    -- Column types copied from tblUnitPrice (CAST drops IDENTITY from UnitPriceId)
    SELECT TOP 0
        CAST(UnitPriceId AS INT) AS UnitPriceId,
        ProductId, ProductCode, ProductName, PackSize,
        CostPrice, UnitPrice, VATPercentage, VATAmountPerUnit, MRPPrice,
        IsActive, ActiveDate, InActiveDate
    INTO dbo.tblUnitPriceRollback
    FROM dbo.tblUnitPrice;

    ALTER TABLE dbo.tblUnitPriceRollback ADD
        RollbackId   INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_tblUnitPriceRollback PRIMARY KEY,
        RollbackBy   NVARCHAR(100) NULL,
        RollbackDate DATETIME NOT NULL CONSTRAINT DF_tblUnitPriceRollback_RollbackDate DEFAULT (GETDATE());

    CREATE INDEX IX_tblUnitPriceRollback_ProductId ON dbo.tblUnitPriceRollback (ProductId);
END
GO
