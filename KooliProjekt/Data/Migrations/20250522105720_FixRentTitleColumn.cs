using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace KooliProjekt.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class FixRentTitleColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First, check if the duplicate title column exists using raw SQL
            // We'll use SQL Server's system tables to check for duplicate columns
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 
                    FROM sys.columns 
                    WHERE Name = N'Title' 
                      AND Object_ID = Object_ID(N'Rent')
                    GROUP BY Name
                    HAVING COUNT(*) > 1
                )
                BEGIN
                    -- Rename one of the duplicate columns to a temporary name
                    EXEC sp_rename 'Rent.Title', 'Title_Temp', 'COLUMN';
                    
                    -- Keep only one Title column by dropping the other one
                    -- This will keep the renamed column to avoid data loss
                    DECLARE @sql nvarchar(max);
                    SET @sql = 'ALTER TABLE [Rent] DROP COLUMN [Title]';
                    EXEC sp_executesql @sql;
                    
                    -- Rename the temporary column back to Title
                    EXEC sp_rename 'Rent.Title_Temp', 'Title', 'COLUMN';
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No down migration needed
        }
    }
}
