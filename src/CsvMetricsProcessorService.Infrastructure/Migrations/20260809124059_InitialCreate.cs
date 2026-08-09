using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CsvMetricsProcessorService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "metrics_results",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    delta_date = table.Column<double>(type: "double precision", nullable: false),
                    min_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    avg_execution_time = table.Column<double>(type: "double precision", nullable: false),
                    avg_value = table.Column<double>(type: "double precision", nullable: false),
                    median_value = table.Column<double>(type: "double precision", nullable: false),
                    max_value = table.Column<double>(type: "double precision", nullable: false),
                    min_value = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metrics_results", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "metrics_values",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    execution_time = table.Column<double>(type: "double precision", nullable: false),
                    value = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metrics_values", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_metrics_results_avg_execution_time",
                table: "metrics_results",
                column: "avg_execution_time");

            migrationBuilder.CreateIndex(
                name: "IX_metrics_results_avg_value",
                table: "metrics_results",
                column: "avg_value");

            migrationBuilder.CreateIndex(
                name: "IX_metrics_results_file_name",
                table: "metrics_results",
                column: "file_name");

            migrationBuilder.CreateIndex(
                name: "IX_metrics_results_min_date",
                table: "metrics_results",
                column: "min_date");

            migrationBuilder.CreateIndex(
                name: "IX_metrics_values_file_name_date",
                table: "metrics_values",
                columns: new[] { "file_name", "date" },
                descending: new[] { false, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "metrics_results");

            migrationBuilder.DropTable(
                name: "metrics_values");
        }
    }
}
