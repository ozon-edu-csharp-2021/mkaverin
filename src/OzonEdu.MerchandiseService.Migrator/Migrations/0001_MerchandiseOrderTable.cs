using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(1)]
    public class MerchandiseOrderTable : Migration
    {
        private readonly string NameTable = "merchandise_order";
        public override void Up()
        {
            if (!TableExists(NameTable))
            {
                Create.Table(NameTable)
                    .WithColumn("id").AsInt64().Identity().PrimaryKey()
                    .WithColumn("creation_date").AsDateTimeOffset().NotNullable()
                    .WithColumn("employee_email").AsString().NotNullable()
                    .WithColumn("manager_email").AsString().NotNullable()
                    .WithColumn("merch_pack_id").AsInt64().NotNullable()
                    .WithColumn("source_id").AsInt64().NotNullable()
                    .WithColumn("status_id").AsInt64().NotNullable()
                    .WithColumn("delivery_date").AsDateTimeOffset().Nullable();
            }
        }

        public override void Down()
        {
            if (TableExists(NameTable))
            {
                Delete.Table(NameTable);
            }
        }
        private bool TableExists(string tableName, string tdmSchema = "public") =>
             Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}