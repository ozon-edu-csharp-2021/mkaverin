using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(5)]
    public class MerchPackTypeIdx : ForwardOnlyMigration
    {

        public override void Up()
        {
            if (TableExists(CommonConstants.NameTableMerchPack))
            {
                Create.Index("merch_pack_type_idx")
                    .OnTable(CommonConstants.NameTableMerchPack)
                    .InSchema("public")
                    .OnColumn("merch_type_id");
            }
        }
       
        private bool TableExists(string tableName, string tdmSchema = "public") =>
             Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}