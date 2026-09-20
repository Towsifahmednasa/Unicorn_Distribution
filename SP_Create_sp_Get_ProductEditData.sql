USE [SalesDisDB_Unicorn_DB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ProductEntry.aspx?ID=xx (edit mode): one row with everything the form shows.
-- Replaces the inline SQL in ProductDAL.ProductEditLoad. Column names of tblProduct
-- are unchanged (p.*), so the existing row["..."] mapping keeps working.
CREATE OR ALTER PROCEDURE [dbo].[sp_Get_ProductEditData]
    @ProductId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.*,

        -- Distribution Center: ids (comma list, as the page expects) + names for display
        STUFF((SELECT ',' + CAST(d.ComUnitId AS VARCHAR(20))
               FROM dbo.tblProductDCDetails d WITH (NOLOCK)
               WHERE d.ProductId = p.ProductId
               ORDER BY d.ComUnitId
               FOR XML PATH('')), 1, 1, '')                          AS ProductDCID,
        STUFF((SELECT ', ' + cu.ComUnitCode + ' : ' + cu.ComUnitName
               FROM dbo.tblProductDCDetails d WITH (NOLOCK)
               JOIN dbo.tblCompanyUnit cu WITH (NOLOCK) ON cu.ComUnitId = d.ComUnitId
               WHERE d.ProductId = p.ProductId
               ORDER BY d.ComUnitId
               FOR XML PATH('')), 1, 2, '')                          AS ProductDCNames,

        -- Right-hand dropdown labels
        gg.GenericGroupName,
        tg.TherapeuticGroupName,
        pg.GroupName                                                  AS ProductGroupName,
        cat.CategoryName,
        mf.ManufacName,
        ps.PackSizeName,
        um.StockUOMName,
        pt.ProTypeName,
        sq.ProductSQName                                              AS ProductBrandName,
        pc.PcsPerCase                                                 AS ShippingCartonSize
    FROM dbo.tblProduct p WITH (NOLOCK)
    LEFT JOIN dbo.tblGenericGroup     gg  WITH (NOLOCK) ON gg.GenericGroupId     = p.GenericGroupId
    LEFT JOIN dbo.tblTherapeuticGroup tg  WITH (NOLOCK) ON tg.TherapeuticGroupId = p.TherapueticGroupId
    LEFT JOIN dbo.tblProductGroup     pg  WITH (NOLOCK) ON pg.GroupId            = p.ProductGroupId
    LEFT JOIN dbo.tblProCategory      cat WITH (NOLOCK) ON cat.CategoryId        = p.CategoryId
    LEFT JOIN dbo.tblManufacturer     mf  WITH (NOLOCK) ON mf.ManufacId          = p.ManufacId
    LEFT JOIN dbo.tblPackSize         ps  WITH (NOLOCK) ON ps.PackSizeId         = p.PackSizeId
    LEFT JOIN dbo.tblStockUOM         um  WITH (NOLOCK) ON um.StockUOMId         = p.StockUOMId
    LEFT JOIN dbo.tblProType          pt  WITH (NOLOCK) ON pt.ProTypeId          = p.ProTypeId
    LEFT JOIN dbo.tblProductSQ        sq  WITH (NOLOCK) ON sq.ProductBrandId     = p.ProductBrandId
    LEFT JOIN dbo.tblProductCase      pc  WITH (NOLOCK) ON pc.CaseId             = p.CaseId
    WHERE p.ProductId = @ProductId;
END
GO

-- Test: EXEC dbo.sp_Get_ProductEditData @ProductId = 52;
