using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(1)]
    public class MerchandiseOrderTable : Migration
    {
        public override void Up()
        {
            if (!TableExists(CommonConstants.NameTableMerchandiseOrder))
            {
                Create.Table(CommonConstants.NameTableMerchandiseOrder)
                    .WithColumn("id").AsInt64().Identity().PrimaryKey()
                    .WithColumn("creation_date").AsDateTimeOffset().NotNullable()
                    .WithColumn("employee_email").AsString().NotNullable()
                    .WithColumn("employee_name").AsString().NotNullable()
                    .WithColumn("manager_email").AsString().NotNullable()
                    .WithColumn("manager_name").AsString().NotNullable()
                    .WithColumn("clothing_size").AsInt32().NotNullable()
                    .WithColumn("merch_pack_id").AsInt64().NotNullable()
                    .WithColumn("source_id").AsInt64().NotNullable()
                    .WithColumn("status_id").AsInt64().NotNullable()
                    .WithColumn("delivery_date").AsDateTimeOffset().Nullable();
            }
        }

        public override void Down()
        {
            if (TableExists(CommonConstants.NameTableMerchandiseOrder))
            {
                Delete.Table(CommonConstants.NameTableMerchandiseOrder);
            }
        }
        private bool TableExists(string tableName, string tdmSchema = "public") =>
         Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}