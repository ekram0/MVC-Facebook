using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC_Facebook.Data.Migrations
{
    public partial class edit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_CommentOwnerId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_AspNetUsers_LikeOwnerId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_OwnerId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_OwnerId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "LikeOwnerId",
                table: "Likes",
                newName: "LikeOwnerID");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_LikeOwnerId",
                table: "Likes",
                newName: "IX_Likes_LikeOwnerID");

            migrationBuilder.RenameColumn(
                name: "CommentOwnerId",
                table: "Comments",
                newName: "CommentOwnerID");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CommentOwnerId",
                table: "Comments",
                newName: "IX_Comments_CommentOwnerID");

            migrationBuilder.AddColumn<string>(
                name: "PostOwnerID",
                table: "Posts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    FriendshipId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderID = table.Column<string>(nullable: true),
                    ReceiverID = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.FriendshipId);
                    table.ForeignKey(
                        name: "FK_Friendships_AspNetUsers_ReceiverID",
                        column: x => x.ReceiverID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friendships_AspNetUsers_SenderID",
                        column: x => x.SenderID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PostOwnerID",
                table: "Posts",
                column: "PostOwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_ReceiverID",
                table: "Friendships",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_SenderID",
                table: "Friendships",
                column: "SenderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_CommentOwnerID",
                table: "Comments",
                column: "CommentOwnerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_AspNetUsers_LikeOwnerID",
                table: "Likes",
                column: "LikeOwnerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_PostOwnerID",
                table: "Posts",
                column: "PostOwnerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_CommentOwnerID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_AspNetUsers_LikeOwnerID",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_PostOwnerID",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropIndex(
                name: "IX_Posts_PostOwnerID",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PostOwnerID",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "LikeOwnerID",
                table: "Likes",
                newName: "LikeOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_LikeOwnerID",
                table: "Likes",
                newName: "IX_Likes_LikeOwnerId");

            migrationBuilder.RenameColumn(
                name: "CommentOwnerID",
                table: "Comments",
                newName: "CommentOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CommentOwnerID",
                table: "Comments",
                newName: "IX_Comments_CommentOwnerId");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Likes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_OwnerId",
                table: "Posts",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_CommentOwnerId",
                table: "Comments",
                column: "CommentOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_AspNetUsers_LikeOwnerId",
                table: "Likes",
                column: "LikeOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_OwnerId",
                table: "Posts",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
