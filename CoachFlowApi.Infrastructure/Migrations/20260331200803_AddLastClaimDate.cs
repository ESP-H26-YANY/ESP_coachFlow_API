using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoachFlowApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLastClaimDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastClaimDate",
                table: "Users",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastClaimDate",
                table: "Users");
        }
    }
}
