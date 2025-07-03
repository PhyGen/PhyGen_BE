using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace teamseven.PhyGen.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExamTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ExamType__3214EC0759F4F1AF", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Grade__3214EC07726422EF", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Roles__3214EC0736E8263A", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Semesters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GradeId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Semester__3214EC073D10267A", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Semester__GradeI__22751F6C",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    EmailVerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__3214EC0751B15CB7", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Users__RoleId__09A971A2",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Users__UpdatedBy__0A9D95DB",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Chapters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Chapter__3214EC0750862B44", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Chapter__Semeste__2645B050",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriptionCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubscriptionName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SubscriptionPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurationInDays = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Subscrip__3214EC07C6E713D8", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Subscript__Updat__0F624AF8",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserSocialProviders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProviderId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ProfileUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserSoci__3214EC07C47A8C4E", x => x.Id);
                    table.ForeignKey(
                        name: "FK__UserSocia__UserI__14270015",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ChapterId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Lesson__3214EC07A753DB40", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Lesson__ChapterI__2A164134",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionTypeId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentGatewayTransactionId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserSubs__3214EC07715BEBD0", x => x.Id);
                    table.ForeignKey(
                        name: "FK__UserSubsc__Subsc__1AD3FDA4",
                        column: x => x.SubscriptionTypeId,
                        principalTable: "SubscriptionTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__UserSubsc__UserI__19DFD96B",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    ExamTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Exam__3214EC07BBB96DEC", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Exam__CreatedByU__3493CFA7",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Exam__ExamTypeId__339FAB6E",
                        column: x => x.ExamTypeId,
                        principalTable: "ExamTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Exam__LessonId__32AB8735",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionSource = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DifficultyLevel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Question__3214EC07E85416E6", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Question__Create__3E1D39E1",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Question__Lesson__3D2915A8",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    ActionByUserId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ExamHist__3214EC07112EFF0E", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ExamHisto__Actio__395884C4",
                        column: x => x.ActionByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__ExamHisto__ExamI__3864608B",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ExamQuestion__Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ExamQuest__ExamI__47A6A41B",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__ExamQuest__Quest__489AC854",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Solutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Solution__3214EC0789D85D64", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Solution__Create__4F47C5E3",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Solution__Questi__4E53A1AA",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolutionReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolutionId = table.Column<int>(type: "int", nullable: false),
                    ReportedByUserId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Solution__3214EC07C9C74095", x => x.Id);
                    table.ForeignKey(
                        name: "FK__SolutionR__Repor__55009F39",
                        column: x => x.ReportedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__SolutionR__Solut__540C7B00",
                        column: x => x.SolutionId,
                        principalTable: "Solutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolutionsLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolutionId = table.Column<int>(type: "int", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GeneratedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Solution__3214EC0737663E7D", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Solutions__Solut__58D1301D",
                        column: x => x.SolutionId,
                        principalTable: "Solutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_SemesterId",
                table: "Chapters",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamHistory_ExamId",
                table: "ExamHistories",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_IX_ExamHistory_ActionByUserId",
                table: "ExamHistories",
                column: "ActionByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestion_ExamId",
                table: "ExamQuestions",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestion_QuestionId",
                table: "ExamQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_CreatedByUserId",
                table: "Exams",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_ExamTypeId",
                table: "Exams",
                column: "ExamTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_IsDeleted",
                table: "Exams",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_LessonId",
                table: "Exams",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "UQ__ExamType__737584F619DE264A",
                table: "ExamTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Grade__737584F6EABA19DB",
                table: "Grades",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_ChapterId",
                table: "Lessons",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_CreatedByUserId",
                table: "Questions",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_LessonId",
                table: "Questions",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "UQ__Roles__8A2B616076721B42",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Semester_GradeId",
                table: "Semesters",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_SolutionReport_ReportedByUserId",
                table: "SolutionReports",
                column: "ReportedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SolutionReport_SolutionId",
                table: "SolutionReports",
                column: "SolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Solution_CreatedByUserId",
                table: "Solutions",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Solution_IsApproved",
                table: "Solutions",
                column: "IsApproved");

            migrationBuilder.CreateIndex(
                name: "IX_Solution_IsDeleted",
                table: "Solutions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Solution_QuestionId",
                table: "Solutions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SolutionsLink_SolutionId",
                table: "SolutionsLinks",
                column: "SolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionType_SubscriptionCode",
                table: "SubscriptionTypes",
                column: "SubscriptionCode");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionType_UpdatedBy",
                table: "SubscriptionTypes",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UQ__Subscrip__A940962CCEF18247",
                table: "SubscriptionTypes",
                column: "SubscriptionCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsActive",
                table: "Users",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LastLoginAt",
                table: "Users",
                column: "LastLoginAt");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedBy",
                table: "Users",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__A9D105349FB12D7F",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSocialProviders_UserId",
                table: "UserSocialProviders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ_UserSocialProviders_Provider",
                table: "UserSocialProviders",
                columns: new[] { "ProviderName", "ProviderId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptions_IsActive",
                table: "UserSubscriptions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptions_PaymentStatus",
                table: "UserSubscriptions",
                column: "PaymentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptions_SubscriptionTypeId",
                table: "UserSubscriptions",
                column: "SubscriptionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptions_UserId",
                table: "UserSubscriptions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamHistories");

            migrationBuilder.DropTable(
                name: "ExamQuestions");

            migrationBuilder.DropTable(
                name: "SolutionReports");

            migrationBuilder.DropTable(
                name: "SolutionsLinks");

            migrationBuilder.DropTable(
                name: "UserSocialProviders");

            migrationBuilder.DropTable(
                name: "UserSubscriptions");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "Solutions");

            migrationBuilder.DropTable(
                name: "SubscriptionTypes");

            migrationBuilder.DropTable(
                name: "ExamTypes");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Chapters");

            migrationBuilder.DropTable(
                name: "Semesters");

            migrationBuilder.DropTable(
                name: "Grades");
        }
    }
}
