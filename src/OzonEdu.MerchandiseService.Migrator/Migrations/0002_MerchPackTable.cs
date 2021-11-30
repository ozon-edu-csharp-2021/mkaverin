using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(2)]
    public class MerchPackTable : Migration
    {
        public override void Up()
        {
            if (!TableExists(CommonConstants.NameTableMerchPack))
            {
                Create.Table(CommonConstants.NameTableMerchPack)
                   .WithColumn("id").AsInt64().Identity().PrimaryKey()
                   .WithColumn("merch_type_id").AsInt32().NotNullable()
                   .WithColumn("merch_items").AsCustom("jsonb").WithDefaultValue("{}").NotNullable();
            }
        }

        public override void Down()
        {
            if (TableExists(CommonConstants.NameTableMerchPack))
            {
                Delete.Table(CommonConstants.NameTableMerchPack);
            }
        }
        private bool TableExists(string tableName, string tdmSchema = "public") =>
         Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}