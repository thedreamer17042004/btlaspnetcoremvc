using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Asp.netApp.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "attributes",
                columns: table => new
                {
                    AttributeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attributes", x => x.AttributeId);
                });

            migrationBuilder.CreateTable(
                name: "brands",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedAt = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brands", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedAt = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Canonical = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Current = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedAt = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Permissions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedAt = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "attribute_options",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attribute_options", x => x.Id);
                    table.ForeignKey(
                        name: "FK_attribute_options_attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "attributes",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<float>(type: "real", nullable: false),
                    SalePrice = table.Column<float>(type: "real", nullable: false),
                    Publish = table.Column<bool>(type: "bit", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    Album = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedAt = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_products_brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_products_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "attribute_languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    AttributeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attribute_languages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_attribute_languages_attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "attributes",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_attribute_languages_languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "category_languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_languages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_category_languages_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_category_languages_languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Images = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedAt = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_accounts_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "attribute_option_languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeOptionId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    OptionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attribute_option_languages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_attribute_option_languages_attribute_options_AttributeOptionId",
                        column: x => x.AttributeOptionId,
                        principalTable: "attribute_options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_attribute_option_languages_languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "attribute_option_products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeOptionId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attribute_option_products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_attribute_option_products_attribute_options_AttributeOptionId",
                        column: x => x.AttributeOptionId,
                        principalTable: "attribute_options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_attribute_option_products_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_languages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_product_languages_languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_languages_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "password_resets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Token = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_password_resets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_password_resets_accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "RoleId", "Active", "CreatedAt", "CreatedBy", "DeletedAt", "Permissions", "RoleName", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, false, new DateTime(2024, 8, 11, 13, 56, 57, 8, DateTimeKind.Local).AddTicks(849), null, null, "[\"ALL\"]", "ADMIN", null, null },
                    { 2, false, new DateTime(2024, 8, 11, 13, 56, 57, 8, DateTimeKind.Local).AddTicks(868), null, null, "[\"PRODUCT_MANAGEMENT\", \"LANGUAGE_MANAGEMENT\"]", "STAFF", null, null },
                    { 3, false, new DateTime(2024, 8, 11, 13, 56, 57, 8, DateTimeKind.Local).AddTicks(869), null, null, "[\"BRAND_MANAGEMENT\", \"LANGUAGE_MANAGEMENT\"]", "USER", null, null },
                    { 4, false, new DateTime(2024, 8, 11, 13, 56, 57, 8, DateTimeKind.Local).AddTicks(870), null, null, "[\"PRODUCT_MANAGEMENT\", \"POST_MANAGEMENT\", \"LANGUAGE_MANAGEMENT\"]", "MANAGER", null, null }
                });

            migrationBuilder.InsertData(
                table: "accounts",
                columns: new[] { "AccountId", "Active", "Address", "Birthday", "CreatedAt", "CreatedBy", "DeletedAt", "Email", "FirstName", "Fullname", "Gender", "Images", "LastName", "Password", "Phone", "RoleId", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[,]
                {
                    { "1", true, null, null, new DateTime(2024, 8, 11, 13, 56, 57, 8, DateTimeKind.Local).AddTicks(1073), null, null, "admin@example.com", "nam", "admin", null, null, "van", "e10adc3949ba59abbe56e057f20f883e", null, 1, null, null, "nam" },
                    { "2", true, null, null, new DateTime(2024, 8, 11, 13, 56, 57, 8, DateTimeKind.Local).AddTicks(1078), null, null, "khoi@gmail.com", "nam", "khoi", null, null, "van", "e10adc3949ba59abbe56e057f20f883e", null, 2, null, null, "khoi" },
                    { "3", true, null, null, new DateTime(2024, 8, 11, 13, 56, 57, 8, DateTimeKind.Local).AddTicks(1080), null, null, "viet@gmail.com", "nam", "viet", null, null, "van", "e10adc3949ba59abbe56e057f20f883e", null, 3, null, null, "viet" },
                    { "4", true, null, null, new DateTime(2024, 8, 11, 13, 56, 57, 8, DateTimeKind.Local).AddTicks(1082), null, null, "manh@gmail.com", "nam", "manh", null, null, "van", "e10adc3949ba59abbe56e057f20f883e", null, 4, null, null, "manh" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_RoleId",
                table: "accounts",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_attribute_languages_AttributeId",
                table: "attribute_languages",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_attribute_languages_LanguageId",
                table: "attribute_languages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_attribute_option_languages_AttributeOptionId",
                table: "attribute_option_languages",
                column: "AttributeOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_attribute_option_languages_LanguageId",
                table: "attribute_option_languages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_attribute_option_products_AttributeOptionId",
                table: "attribute_option_products",
                column: "AttributeOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_attribute_option_products_ProductId",
                table: "attribute_option_products",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_attribute_options_AttributeId",
                table: "attribute_options",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_category_languages_CategoryId",
                table: "category_languages",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_category_languages_LanguageId",
                table: "category_languages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_password_resets_UserId",
                table: "password_resets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_product_languages_LanguageId",
                table: "product_languages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_product_languages_ProductId",
                table: "product_languages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_products_BrandId",
                table: "products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_products_CategoryId",
                table: "products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attribute_languages");

            migrationBuilder.DropTable(
                name: "attribute_option_languages");

            migrationBuilder.DropTable(
                name: "attribute_option_products");

            migrationBuilder.DropTable(
                name: "category_languages");

            migrationBuilder.DropTable(
                name: "password_resets");

            migrationBuilder.DropTable(
                name: "product_languages");

            migrationBuilder.DropTable(
                name: "attribute_options");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "languages");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "attributes");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "brands");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
