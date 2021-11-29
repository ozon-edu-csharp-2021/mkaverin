using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(6)]
    public class MerchandiseOrderIdIdx : ForwardOnlyMigration
    {
        public override void Up()
        {
            if (TableExists(CommonConstants.NameTableMerchandiseOrder))
            {
                Create.Index("merchandise_order_id_idx")
                    .OnTable(CommonConstants.NameTableMerchandiseOrder)
                    .InSchema("public")
                    .OnColumn("id");
            }
        }
        private bool TableExists(string tableName, string tdmSchema = "public") =>
             Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}