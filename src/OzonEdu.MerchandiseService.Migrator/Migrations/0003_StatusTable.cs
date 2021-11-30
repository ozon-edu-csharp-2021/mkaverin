using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(3)]
    public class StatusTable : Migration
    {
    
        public override void Up()
        {
            if (!TableExists(CommonConstants.NameTableStatus))
            {
                Create.Table(CommonConstants.NameTableStatus)
                    .WithColumn("id").AsInt64().Identity().PrimaryKey()
                    .WithColumn("name").AsString().NotNullable();
            }
        }

        public override void Down()
        {
            if (TableExists(CommonConstants.NameTableStatus))
            {
                Delete.Table(CommonConstants.NameTableStatus);
            }
        }
        private bool TableExists(string tableName, string tdmSchema = "public") =>
             Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}