-- ==========================================================================
-- Cleanup: tbl_INV_Central_BatchWiseStock legacy duplicate deductions
--
-- Background: the old (pre-fix) tbl_INV_CentralStoreStoIssue SP wrote issue
-- rows with wrong Weighted_Avg_Id refs (ReceiveId/Batch_Stock_Id id-space
-- mix-up). After the SP was fixed and re-run, corrected rows were inserted
-- but the old corrupt rows were never removed: 270 transfers ended up with
-- exactly-doubled deductions, and 7 rows sit on the WRONG product's ledger
-- (which is what zeroed availability for product 5735150 / ReqId=21).
--
-- Safe criterion: delete a negative row ONLY if
--   (a) its Weighted_Avg_Id is NOT its own STI's issue-ledger row, AND
--   (b) the same STI also has a correct-linked negative row.
-- 57 old-style rows that are the ONLY deduction for their STI are kept
-- (their amounts are correct, only the linkage is legacy).
--
-- Also resets the phantom picking of ReqId=21 (picked with zero
-- tblStockInTransfar rows, so the DC could never receive it).
-- ==========================================================================
SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRAN;

-- 1) Backup the rows we are about to delete
SELECT BS.*
INTO tbl_INV_Central_BatchWiseStock_Bak20260927
FROM tbl_INV_Central_BatchWiseStock BS
JOIN tblStockInTransfar ST ON BS.StockInTransfarId = ST.StockInTransfarId
LEFT JOIN tbl_INV_CentralStore_WeightedAvg W
       ON BS.Weighted_Avg_Id = W.Weighted_Avg_Id
      AND W.Mst_Trans_Id = BS.StockInTransfarId
      AND W.Item_Code = ST.ProductCode
WHERE BS.Trans_Qty < 0
  AND W.Weighted_Avg_Id IS NULL                      -- (a) not its own STI's ledger row
  AND EXISTS (                                       -- (b) a correct sibling exists
      SELECT 1
      FROM tbl_INV_Central_BatchWiseStock BS2
      JOIN tbl_INV_CentralStore_WeightedAvg W2 ON BS2.Weighted_Avg_Id = W2.Weighted_Avg_Id
      WHERE BS2.StockInTransfarId = BS.StockInTransfarId
        AND BS2.Trans_Qty < 0
        AND W2.Mst_Trans_Id = BS.StockInTransfarId
        AND W2.Item_Code = ST.ProductCode
        AND BS2.Batch_Stock_Id <> BS.Batch_Stock_Id);

PRINT CONCAT('Backed up: ', @@ROWCOUNT, ' rows');

-- 2) Delete exactly the backed-up rows
DELETE BS
FROM tbl_INV_Central_BatchWiseStock BS
JOIN tbl_INV_Central_BatchWiseStock_Bak20260927 B ON B.Batch_Stock_Id = BS.Batch_Stock_Id;

PRINT CONCAT('Deleted: ', @@ROWCOUNT, ' rows');

-- 3) Reset the phantom picking of ReqId=21 so it can be picked again
UPDATE tblRequsitionChild
SET IsPicking = NULL, IssueQty = 0, PriceAmount = 0, VATAmount = 0, TotalPrice = 0, CaseQty = 0
WHERE ReqId = 21 AND ReqChildId = 389;

UPDATE tblRequisition
SET CreatePicking = NULL, PickingNo = NULL, PickingDate = NULL,
    Submit = NULL, SubmitDate = NULL, IssueChalanNo = NULL, IssuChalanDate = NULL,
    TruckNo = NULL, DriverName = NULL, TotalPrice = NULL, TotalVAT = NULL, GrandTotalPrice = NULL
WHERE ReqId = 21;

COMMIT;

-- 4) Verify: should return 0 doubled transfers
SELECT COUNT(*) AS RemainingDoubledSTIs
FROM (
    SELECT BS.StockInTransfarId
    FROM tbl_INV_Central_BatchWiseStock BS
    JOIN tblStockInTransfar ST ON BS.StockInTransfarId = ST.StockInTransfarId
    WHERE BS.Trans_Qty < 0
    GROUP BY BS.StockInTransfarId
    HAVING SUM(BS.Trans_Qty) = -2 * MIN(ST.Quantity) AND COUNT(*) = 2
) X;

-- 5) Verify: 5735150 batch-net should now equal the ledger Close_Qty (55)
SELECT SUM(BS.Trans_Qty) AS BatchNet_5735150
FROM tbl_INV_Central_BatchWiseStock BS
JOIN tbl_INV_CentralStore_WeightedAvg W ON BS.Weighted_Avg_Id = W.Weighted_Avg_Id
WHERE W.Item_Code = '5735150';
