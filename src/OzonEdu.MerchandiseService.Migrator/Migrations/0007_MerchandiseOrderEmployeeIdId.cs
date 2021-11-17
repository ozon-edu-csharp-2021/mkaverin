using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(7)]
    public class MerchandiseOrderEmployeeIdId : ForwardOnlyMigration
    {
        private readonly string NameTable = "merchandise_order";
        public override void Up()
        {
            if (TableExists(NameTable))
            {
                Create.Index("merchandise_order_employee_id_idx")
                    .OnTable(NameTable)
                    .InSchema("public")
                    .OnColumn("employee_id");
            }
        }
        private bool TableExists(string tableName, string tdmSchema = "public") =>
             Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}