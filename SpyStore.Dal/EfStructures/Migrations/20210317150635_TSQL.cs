using Microsoft.EntityFrameworkCore.Migrations;
using SpyStore.Dal.EfStructures.MigrationHelpers;

namespace SpyStore.Dal.EfStructures.Migrations
{
    public partial class TSQL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE FUNCTION Store.GetOrderTotal ( @OrderId INT )
                RETURNS MONEY WITH SCHEMABINDING
                BEGIN
                    DECLARE @Result MONEY;
                    SELECT @Result = SUM([Quantity]*[UnitCost]) FROM Store.OrderDetails
                    WHERE OrderId = @OrderId;
                    RETURN coalesce(@Result,0)
                END");

            migrationBuilder.Sql(@"CREATE PROCEDURE [Store].[PurchaseItemsInCart] (@customerId int = 0, @orderId int OUTPUT)
                        AS
                        BEGIN
                          SET NOCOUNT ON;
                          INSERT INTO Store.Orders (CustomerId, OrderDate, ShipDate)
                            VALUES (@customerId, GETDATE(), GETDATE());
                          SET @orderId = SCOPE_IDENTITY();
                          DECLARE @TranName varchar(20);
                          SELECT
                            @TranName = 'CommitOrder';
                          BEGIN TRANSACTION @TranName;
                            BEGIN TRY
                              INSERT INTO Store.OrderDetails (OrderId, ProductId, Quantity, UnitCost)
                                SELECT
                                  @orderId,
                                  scr.ProductId,
                                  scr.Quantity,
                                  p.CurrentPrice
                                FROM Store.ShoppingCartRecords scr
                                INNER JOIN Store.Products p
                                  ON p.Id = scr.ProductId
                                WHERE scr.CustomerId = @customerId;
                              DELETE FROM Store.ShoppingCartRecords
                              WHERE CustomerId = @customerId;
                            COMMIT TRANSACTION @TranName;
                          END TRY
                          BEGIN CATCH
                            ROLLBACK TRANSACTION @TranName;
                            SET @OrderId = -1;
                          END CATCH;
                        END;");

            migrationBuilder.Sql(@"CREATE VIEW [Store].[OrderDetailWithProductInfo]
                        AS
                        SELECT
                          od.Id,
                          od.TimeStamp,
                          od.OrderId,
                          od.ProductId,
                          od.Quantity,
                          od.UnitCost,
                          od.Quantity * od.UnitCost AS LineItemTotal,
                          p.ModelName,
                          p.Description,
                          p.ModelNumber,
                          p.ProductImage,
                          p.ProductImageLarge,
                          p.ProductImageThumb,
                          p.CategoryId,
                          p.UnitsInStock,
                          p.CurrentPrice,
                          c.CategoryName
                        FROM Store.OrderDetails od
                        INNER JOIN Store.Orders o
                          ON o.Id = od.OrderId
                        INNER JOIN Store.Products AS p
                          ON od.ProductId = p.Id
                        INNER JOIN Store.Categories AS c
                          ON p.CategoryId = c.id");

            migrationBuilder.Sql(@"CREATE VIEW [Store].[CartRecordWithProductInfo]
                            AS
                            SELECT
                              scr.Id,
                              scr.TimeStamp,
                              scr.DateCreated,
                              scr.CustomerId,
                              scr.Quantity,
                              scr.LineItemTotal,
                              scr.ProductId,
                              p.ModelName,
                              p.Description,
                              p.ModelNumber,
                              p.ProductImage,
                              p.ProductImageLarge,
                              p.ProductImageThumb,
                              p.CategoryId,
                              p.UnitsInStock,
                              p.CurrentPrice,
                              c.CategoryName
                            FROM Store.ShoppingCartRecords scr
                            INNER JOIN Store.Products p
                              ON p.Id = scr.ProductId
                            INNER JOIN Store.Categories c
                              ON c.Id = p.CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION [Store].[GetOrderTotal]");
            migrationBuilder.Sql(@"DROP PROCEDURE [Store].[PurchaseItemsInCart]");
            migrationBuilder.Sql(@"DROP VIEW [Store].[OrderDetailWithProductInfo]");
            migrationBuilder.Sql(@"DROP VIEW [Store].[CartRecordWithProductInfo]");
        }
    }

}
