using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(2)]
    public class StockTable : Migration
    {
        public override void Up()
        {
            if (!TableExists("stocks"))
            {
                Create.Table("stocks")
                   .WithColumn("id").AsInt64().Identity().PrimaryKey()
                   .WithColumn("sku_id").AsInt64().NotNullable()
                   .WithColumn("quantity").AsInt32().NotNullable()
                   .WithColumn("minimal_quantity").AsInt32().NotNullable();
            }
        }

        public override void Down()
        {
            Delete.Table("stocks");
        }
        private bool TableExists(string tableName, string tdmSchema = "public") =>
             Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}