using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(5)]
    public class MerchPackTypeIdx : ForwardOnlyMigration
    {
        private readonly string NameTable = "merch_pack";

        public override void Up()
        {
            if (TableExists(NameTable))
            {
                Create.Index("merch_pack_type_idx")
                    .OnTable(NameTable)
                    .InSchema("public")
                    .OnColumn("merch_type_id");
            }
        }
       
        private bool TableExists(string tableName, string tdmSchema = "public") =>
             Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}