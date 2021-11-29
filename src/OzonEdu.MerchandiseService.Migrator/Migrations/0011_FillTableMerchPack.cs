using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(11)]
    public class FillTableMerchPack : Migration
    {
        public override void Up()
        {
            if (TableExists(CommonConstants.NameTableMerchPack))
            {
                Insert.IntoTable(CommonConstants.NameTableMerchPack)
                    .Row(new { id = 10, merch_type_id = 10, merch_items = "{\"12341\":2,\"23722\":1}" })
                    .Row(new { id = 20, merch_type_id = 20, merch_items = "{\"12341\":2,\"23722\":1}" })
                    .Row(new { id = 30, merch_type_id = 30, merch_items = "{\"1141\":1,\"23722\":1}" })
                    .Row(new { id = 40, merch_type_id = 40, merch_items = "{\"12341\":2,\"23722\":1}" })
                    .Row(new { id = 50, merch_type_id = 50, merch_items = "{\"12\":3,\"12372\":1}" });
            }
        }
        public override void Down()
        {
            if (TableExists(CommonConstants.NameTableMerchPack))
            {
                Delete.FromTable(CommonConstants.NameTableMerchPack)
                    .Row(new { id = 10 })
                    .Row(new { id = 20 })
                    .Row(new { id = 30 })
                    .Row(new { id = 40 })
                    .Row(new { id = 50 });
            }
        }
        private bool TableExists(string tableName, string tdmSchema = "public") =>
             Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}