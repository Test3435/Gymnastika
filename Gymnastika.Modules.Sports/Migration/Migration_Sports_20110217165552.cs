using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_Sports_20110217165552 : IDataMigration
    {
        public string TableName
        {
            get { return "Sports"; }
        }

        public string Version
        {
            get { return "20110217165552"; }
        }

        public SchemaBuilder SchemaBuilder { get; set; }

        public void Up()
        {
            SchemaBuilder.CreateTable(TableName,
                t => t.Column<int>("Id", c => c.PrimaryKey().Identity())
                      .Column<string>("Name")
                      .Column<string>("SmallImageUri")
                      .Column<string>("LargeImageUri")
                      .Column<string>("BriefIntroduction")
                      .Column<string>("DetailIntroduction")
                      .Column<int>("CaloriePerHour")
                      .Column<int>("CategoryId"));
            
            //SchemaBuilder
                      //.CreateForeignKey("FK_SPTCAT", TableName, new string[] { "CategoryId" }, "SportsCategories", new string[] { "Id" });
        }

        public void Down()
        {
            SchemaBuilder.DropTable(TableName);
        }
    }
}