-- ==============================================================================
-- SCRIPT FOR LIVE SERVER DEPLOYMENT: Customer Payment Update
-- Database: SalesDisDB_Unicorn_DB
-- Description: Adds Remarks column to tblCustomerPay table for customer payment remarks.
-- ==============================================================================

USE [SalesDisDB_Unicorn_DB]
GO

IF NOT EXISTS (
    SELECT 1 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'tblCustomerPay' 
      AND COLUMN_NAME = 'Remarks'
)
BEGIN
    ALTER TABLE tblCustomerPay ADD Remarks NVARCHAR(500) NULL;
    PRINT 'SUCCESS: Column [Remarks] added to table [tblCustomerPay].';
END
ELSE
BEGIN
    PRINT 'INFO: Column [Remarks] already exists in table [tblCustomerPay].';
END
GO
