using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTC.Migrations
{
    /// <inheritdoc />
    public partial class TestMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COUNTRY",
                columns: table => new
                {
                    CODE = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CODE", x => x.CODE);
                });

            migrationBuilder.CreateTable(
                name: "DDEDM_EMPLOYEE",
                columns: table => new
                {
                    EMP_ID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ACTIVE = table.Column<bool>(type: "bit", nullable: true),
                    NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PRIVATE_EMAIL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CURRENT_PHONE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MOBILE_PHONE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GRADE_ID = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    SECTION_STRU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DEPARTMENT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DDEDM_EM__16EBFA269A66E613", x => x.EMP_ID);
                });

            migrationBuilder.CreateTable(
                name: "GUEST_ATTNDNC",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TRANSACTION_ID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    TOTAL = table.Column<int>(type: "int", nullable: false),
                    TIME_IN = table.Column<TimeOnly>(type: "time", nullable: false),
                    TIME_OUT = table.Column<TimeOnly>(type: "time", nullable: true),
                    FLAG = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ID", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GUEST_CATEGORY",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CATEGORY_NAME = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ID", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GUEST_ID_TEMP",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("USER_ID", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GUEST_REG",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COUNTRY_CODE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CATEGORY_ID = table.Column<int>(type: "int", nullable: false),
                    COMPANY = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    COMPANY_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DEPT_SECT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NATIONAL_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TOTAL = table.Column<int>(type: "int", nullable: false),
                    REQUIREMENT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    REQ_ID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MET_ID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IMAGE_DATA = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    START_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    END_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ID", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GUEST_SCAN_RECORD",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USERNAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LOGIN_ID = table.Column<int>(type: "int", nullable: false),
                    TRANSACTION_ID = table.Column<int>(type: "int", nullable: false),
                    TYPE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SCAN_TIME = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ID", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PSG_SPPLIER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VHC_ID = table.Column<int>(type: "int", nullable: false),
                    FLAG = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ID", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PTC_USER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USERNAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PASSWORD = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ID", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PTC_USER_RECORD",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USERNAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LOGIN_TIME = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ID", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SPPLIER_TRANSACTION_RECORD",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USERNAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LOGIN_ID = table.Column<int>(type: "int", nullable: false),
                    TRANSACTION_ID = table.Column<int>(type: "int", nullable: false),
                    TYPE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SCAN_TIME = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ID", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SSO_USER",
                columns: table => new
                {
                    EMP_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    USERNAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ROLE_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("EMP_ID", x => x.EMP_ID);
                });

            migrationBuilder.CreateTable(
                name: "TRXTMS_EMP_PCDTKP_FLT",
                columns: table => new
                {
                    SEQ_NO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FLAG = table.Column<bool>(type: "bit", nullable: false),
                    EMP_ID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DEPARTMENT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    TIME_OUT = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true),
                    TIME_RETURN = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true),
                    TIMEREASON_ID = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    REASON = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CREATE_BY = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TRXTMS_E__04B917664F5755A5", x => x.SEQ_NO);
                });

            migrationBuilder.CreateTable(
                name: "TRXTMS_EMP_PCDTKP_FLT_ACT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SEQ_NO = table.Column<int>(type: "int", nullable: true),
                    FLAG = table.Column<bool>(type: "bit", nullable: false),
                    EMP_ID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DEPARTMENT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    TIME_OUT = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true),
                    TIME_RETURN = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true),
                    TIMEREASON_ID = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    REASON = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CREATE_BY = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TRXTMS_E__3214EC27E7A29CD2", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TRXTMS_EMP_SCAN_RECORD",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USERNAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LOGIN_ID = table.Column<int>(type: "int", nullable: false),
                    TRANSACTION_ID = table.Column<int>(type: "int", nullable: false),
                    TYPE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SCAN_TIME = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ID", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TRXTMS_TIME_CLOCKING",
                columns: table => new
                {
                    EMP_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CLOCKING_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    CLOCKING_TIME = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IN_OUT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SHIFTGROUP_ID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SHIFT_ID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TIMEREASON_ID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PROCESSED = table.Column<bool>(type: "bit", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("EMP_ID", x => x.EMP_ID);
                });

            migrationBuilder.CreateTable(
                name: "USER_LOCK",
                columns: table => new
                {
                    USER_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("USER_ID", x => x.USER_ID);
                });

            migrationBuilder.CreateTable(
                name: "USER_PASSWORD_ATTEMPT",
                columns: table => new
                {
                    USER_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ATTEMPT = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("USER_ID", x => x.USER_ID);
                });

            migrationBuilder.CreateTable(
                name: "USER_PASSWORD_DUMP",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USER_ID = table.Column<int>(type: "int", nullable: false),
                    PASSWORD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ID", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USER_ROLE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ROLE_NAME = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ID", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "VHC_SPPLIER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VEHICLE_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VEHICLE_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COMPANY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PASSANGER_COUNT = table.Column<int>(type: "int", nullable: false),
                    IN_TIME = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FLAG = table.Column<bool>(type: "bit", nullable: false),
                    CONFIRM_OUT_TIME = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VEHICLE_IMG = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ID", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WRKPERM_ATTNDNC",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    REG_NUM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WORKER_ID = table.Column<int>(type: "int", nullable: false),
                    DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    TIME_IN = table.Column<TimeOnly>(type: "time", nullable: false),
                    TIME_OUT = table.Column<TimeOnly>(type: "time", nullable: true),
                    FLAG = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ID", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WRKPERM_DESC",
                columns: table => new
                {
                    REG_NUM = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TITLE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    COMP_NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LOCATION = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    START_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    END_DATE = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("REG_NUM", x => x.REG_NUM);
                });

            migrationBuilder.CreateTable(
                name: "WRKPERM_SCAN_RECORD",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USERNAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LOGIN_ID = table.Column<int>(type: "int", nullable: false),
                    TRANSACTION_ID = table.Column<int>(type: "int", nullable: false),
                    TYPE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SCAN_TIME = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ID", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WRKPERM_WORKER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    REG_NUM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SPECIALITY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CERTIFICATION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NATIONAL_ID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ID", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COUNTRY");

            migrationBuilder.DropTable(
                name: "DDEDM_EMPLOYEE");

            migrationBuilder.DropTable(
                name: "GUEST_ATTNDNC");

            migrationBuilder.DropTable(
                name: "GUEST_CATEGORY");

            migrationBuilder.DropTable(
                name: "GUEST_ID_TEMP");

            migrationBuilder.DropTable(
                name: "GUEST_REG");

            migrationBuilder.DropTable(
                name: "GUEST_SCAN_RECORD");

            migrationBuilder.DropTable(
                name: "PSG_SPPLIER");

            migrationBuilder.DropTable(
                name: "PTC_USER");

            migrationBuilder.DropTable(
                name: "PTC_USER_RECORD");

            migrationBuilder.DropTable(
                name: "SPPLIER_TRANSACTION_RECORD");

            migrationBuilder.DropTable(
                name: "SSO_USER");

            migrationBuilder.DropTable(
                name: "TRXTMS_EMP_PCDTKP_FLT");

            migrationBuilder.DropTable(
                name: "TRXTMS_EMP_PCDTKP_FLT_ACT");

            migrationBuilder.DropTable(
                name: "TRXTMS_EMP_SCAN_RECORD");

            migrationBuilder.DropTable(
                name: "TRXTMS_TIME_CLOCKING");

            migrationBuilder.DropTable(
                name: "USER_LOCK");

            migrationBuilder.DropTable(
                name: "USER_PASSWORD_ATTEMPT");

            migrationBuilder.DropTable(
                name: "USER_PASSWORD_DUMP");

            migrationBuilder.DropTable(
                name: "USER_ROLE");

            migrationBuilder.DropTable(
                name: "VHC_SPPLIER");

            migrationBuilder.DropTable(
                name: "WRKPERM_ATTNDNC");

            migrationBuilder.DropTable(
                name: "WRKPERM_DESC");

            migrationBuilder.DropTable(
                name: "WRKPERM_SCAN_RECORD");

            migrationBuilder.DropTable(
                name: "WRKPERM_WORKER");
        }
    }
}
