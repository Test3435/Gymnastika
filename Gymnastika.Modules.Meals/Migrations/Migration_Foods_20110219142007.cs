
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_Foods_20110219142007 : IDataMigration
    {
        private const string ForeignKeyName = "FK_Categories_Foods";

        public string TableName 
        { 
            get { return "Foods"; }
        }
            
        public string Version 
        { 
            get { return "20110219142007"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(
                TableName,
                t => t.Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("Name")
                    .Column<string>("ImageUri")
                    .Column<decimal>("Calorie")
                    .Column<int>("CategoryId"));

            SchemaBuilder.CreateForeignKey(
                ForeignKeyName,
                TableName,
                new string[] { "CategoryId" },
                "Categories",
                new string[] { "Id" });
        }
        
        public void Down()
        {
            SchemaBuilder.DropTable(TableName);

            SchemaBuilder.DropForeignKey(TableName, ForeignKeyName);
        }
    }
}
  