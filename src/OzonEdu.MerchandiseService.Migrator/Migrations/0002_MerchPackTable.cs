using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(2)]
    public class MerchPackTable : Migration
    {
        private readonly string NameTable = "merch_pack";

        public override void Up()
        {
            if (!TableExists(NameTable))
            {
                Create.Table(NameTable)
                   .WithColumn("id").AsInt64().Identity().PrimaryKey()
                   .WithColumn("merch_type_id").AsInt32().NotNullable()
                   .WithColumn("merch_items").AsCustom("jsonb").WithDefaultValue("{}").NotNullable();
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