using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tornado.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtTimestamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "CreatedAt",
                table: "Videos",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "CreatedAt",
                table: "VideoRatings",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "CreatedAt",
                table: "VideoComments",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "CreatedAt",
                table: "VideoCommentChains",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "CreatedAt",
                table: "UserProfiles",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "CreatedAt",
                table: "CommentRatings",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "CreatedAt",
                table: "Channels",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "VideoRatings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "VideoComments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "VideoCommentChains");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CommentRatings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Channels");
        }
    }
}
