using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tornado.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMetrics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VideoMetrics_VideoId",
                table: "VideoMetrics");

            migrationBuilder.DropIndex(
                name: "IX_ChannelMetrics_ChannelId",
                table: "ChannelMetrics");

            migrationBuilder.CreateIndex(
                name: "IX_VideoMetrics_VideoId",
                table: "VideoMetrics",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelMetrics_ChannelId",
                table: "ChannelMetrics",
                column: "ChannelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VideoMetrics_VideoId",
                table: "VideoMetrics");

            migrationBuilder.DropIndex(
                name: "IX_ChannelMetrics_ChannelId",
                table: "ChannelMetrics");

            migrationBuilder.CreateIndex(
                name: "IX_VideoMetrics_VideoId",
                table: "VideoMetrics",
                column: "VideoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChannelMetrics_ChannelId",
                table: "ChannelMetrics",
                column: "ChannelId",
                unique: true);
        }
    }
}
