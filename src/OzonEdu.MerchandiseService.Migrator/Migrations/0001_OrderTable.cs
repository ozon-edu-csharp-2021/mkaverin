using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(1)]
    public class OrderTable : Migration
    {
        public override void Up()
        {
            if (!TableExists("merchandise_order"))
            {
                Create.Table("merchandise_order")
                    .WithColumn("id").AsInt64().Identity().PrimaryKey()
                    .WithColumn("creation_date").AsDateTimeOffset().NotNullable()
                    .WithColumn("employee_id").AsInt32().NotNullable()
                    .WithColumn("merch_pack_id").AsInt32().NotNullable();
            }
        }

        public override void Down()
        {
            Delete.Table("merchandise_order");
        }
        private bool TableExists(string tableName, string tdmSchema = "public") =>
             Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}