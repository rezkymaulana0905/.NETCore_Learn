using Microsoft.EntityFrameworkCore;
using PTC.Models;

namespace PTC.Data;

public partial class PtcContext : DbContext
{
    public PtcContext()
    {
    }

    public PtcContext(DbContextOptions<PtcContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DdedmEmployee> DdedmEmployees { get; set; }
    public virtual DbSet<EmployeeClocking> EmployeeClockings { get; set; }
    public virtual DbSet<EmpScanRecord> EmpScanRecord { get; set; }
    public virtual DbSet<TrxTmsEmpPcDtKpFlt> TrxTmsEmpPcDtKpsFlt { get; set; }
    public virtual DbSet<TrxTmsEmpPcDtKpFltAct> TrxTmsEmpPcDtKpsFltAct { get; set; }
    public virtual DbSet<GuestReg> RegGuest { get; set; }
    public virtual DbSet<GuestAttndnc> GuestAttndnc { get; set; }
    public virtual DbSet<GuestCategory> GuestCategory {  get; set; }
    public virtual DbSet<GuestScanRecord> GuestScanRecord { get; set; }
    public virtual DbSet<VhcSpplier> VhcSpplier { get; set; }
    public virtual DbSet<PsgSpplier> PsgSpplier { get; set; }
    public virtual DbSet<SpplierTransactionRecord> SpplierTransactionRecord { get; set; }
    public virtual DbSet<User> User { get; set; }
    public virtual DbSet<PasswordDump> PasswordDump { get; set; }
    public virtual DbSet<UserRecord> UserRecord { get; set; }
    public virtual DbSet<SsoUser> SsoUsers { get; set; }
    public virtual DbSet<UserRole> UserRole { get; set; }
    public virtual DbSet<UserLock> UserLock { get; set; }
    public virtual DbSet<UserPasswordAttempt> UserPasswordAttempt { get; set; }
    public virtual DbSet<WrkPermDesc> WrkPermDesc { get; set; }
    public virtual DbSet<WrkPermWorker> WrkPermWorker { get; set; }
    public virtual DbSet<WrkPermAttndnc> WrkPermAttndnc { get; set; }
    public virtual DbSet<WrkPermScanRecord> WrkPermScanRecord { get; set; }
    public virtual DbSet<CountryModel> Country { get; set; }
    public virtual DbSet<GuestIdTemp> GuestIdTemp { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GuestIdTemp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("USER_ID");
            entity.ToTable("GUEST_ID_TEMP");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Date).HasColumnName("DATE");
        });
        modelBuilder.Entity<UserPasswordAttempt>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("USER_ID");
            entity.ToTable("USER_PASSWORD_ATTEMPT");
            entity.Property(e => e.UserId).HasColumnName("USER_ID");
            entity.Property(e => e.Attempt).HasColumnName("ATTEMPT");
        });
        modelBuilder.Entity<UserLock>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("USER_ID");
            entity.ToTable("USER_LOCK");
            entity.Property(e => e.UserId).HasColumnName("USER_ID");
            entity.Property(e => e.CreateDate).HasColumnName("CREATED_DATE");
        });
        modelBuilder.Entity<WrkPermScanRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ID");
            entity.ToTable("WRKPERM_SCAN_RECORD");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Username).HasColumnName("USERNAME");
            entity.Property(e => e.LoginId).HasColumnName("LOGIN_ID");
            entity.Property(e => e.TransactionId).HasColumnName("TRANSACTION_ID");
            entity.Property(e => e.Type).HasColumnName("TYPE");
            entity.Property(e => e.ScanTime).HasColumnName("SCAN_TIME");
        });
        modelBuilder.Entity<SpplierTransactionRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ID");
            entity.ToTable("SPPLIER_TRANSACTION_RECORD");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Username).HasColumnName("USERNAME");
            entity.Property(e => e.LoginId).HasColumnName("LOGIN_ID");
            entity.Property(e => e.TransactionId).HasColumnName("TRANSACTION_ID");
            entity.Property(e => e.Type).HasColumnName("TYPE");
            entity.Property(e => e.ScanTime).HasColumnName("SCAN_TIME");
        });
        modelBuilder.Entity<EmpScanRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ID");
            entity.ToTable("TRXTMS_EMP_SCAN_RECORD");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Username).HasColumnName("USERNAME");
            entity.Property(e => e.LoginId).HasColumnName("LOGIN_ID");
            entity.Property(e => e.TransactionId).HasColumnName("TRANSACTION_ID");
            entity.Property(e => e.Type).HasColumnName("TYPE");
            entity.Property(e => e.ScanTime).HasColumnName("SCAN_TIME");
        });
        modelBuilder.Entity<GuestScanRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ID");
            entity.ToTable("GUEST_SCAN_RECORD");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Username).HasColumnName("USERNAME");
            entity.Property(e => e.LoginId).HasColumnName("LOGIN_ID");
            entity.Property(e => e.TransactionId).HasColumnName("TRANSACTION_ID");
            entity.Property(e => e.Type).HasColumnName("TYPE");
            entity.Property(e => e.ScanTime).HasColumnName("SCAN_TIME");
        });
        modelBuilder.Entity<PasswordDump>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ID");
            entity.ToTable("USER_PASSWORD_DUMP");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.UserId).HasColumnName("USER_ID");
            entity.Property(e => e.Password).HasColumnName("PASSWORD");
            entity.Property(e => e.CreateDate).HasColumnName("CREATE_DATE");
        });
        modelBuilder.Entity<GuestCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ID");
            entity.ToTable("GUEST_CATEGORY");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CategoryName).HasColumnName("CATEGORY_NAME");
        });
        modelBuilder.Entity<CountryModel>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("CODE");
            entity.ToTable("COUNTRY");
            entity.Property(e => e.Code).HasColumnName("CODE");
            entity.Property(e => e.Name).HasColumnName("NAME");
        });
        modelBuilder.Entity<EmployeeClocking>(entity =>
        {
            entity.HasKey(e => e.EmpId).HasName("EMP_ID");
            entity.ToTable("TRXTMS_TIME_CLOCKING");
            entity.Property(e => e.EmpId).HasColumnName("EMP_ID");
            entity.Property(e => e.ClockingDate).HasColumnName("CLOCKING_DATE");
            entity.Property(e => e.ClockingTime).HasColumnName("CLOCKING_TIME");
            entity.Property(e => e.InOut).HasColumnName("IN_OUT");
            entity.Property(e => e.ShiftGroupId).HasColumnName("SHIFTGROUP_ID");
            entity.Property(e => e.ShiftId).HasColumnName("SHIFT_ID");
            entity.Property(e => e.TimeReasonId).HasColumnName("TIMEREASON_ID");
            entity.Property(e => e.Processed).HasColumnName("PROCESSED");
            entity.Property(e => e.CreateDate).HasColumnName("CREATE_DATE");
            entity.Property(e => e.UpdateDate).HasColumnName("UPDATE_DATE");
        });
        modelBuilder.Entity<SsoUser>(entity =>
        {
            entity.HasKey(e => e.EmpId).HasName("EMP_ID");
            entity.ToTable("SSO_USER");
            entity.Property(e => e.EmpId).HasColumnName("EMP_ID");
            entity.Property(e => e.Username).HasColumnName("USERNAME");
            entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");
        });
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ID");
            entity.ToTable("USER_ROLE");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.RoleName).HasColumnName("ROLE_NAME");
        });
        modelBuilder.Entity<WrkPermDesc>(entity =>
        {
            entity.HasKey(e => e.RegNum).HasName("REG_NUM");
            entity.ToTable("WRKPERM_DESC");
            entity.Property(e => e.RegNum).HasColumnName("REG_NUM");
            entity.Property(e => e.Title).HasColumnName("TITLE");
            entity.Property(e => e.CompName).HasColumnName("COMP_NAME");
            entity.Property(e => e.Location).HasColumnName("LOCATION");
            entity.Property(e => e.Start).HasColumnName("START_DATE");
            entity.Property(e => e.End).HasColumnName("END_DATE");
        });
        modelBuilder.Entity<WrkPermWorker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ID");
            entity.ToTable("WRKPERM_WORKER");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.RegNum).HasColumnName("REG_NUM");
            entity.Property(e => e.Name).HasColumnName("NAME");
            entity.Property(e => e.Speciality).HasColumnName("SPECIALITY");
            entity.Property(e => e.Certificate).HasColumnName("CERTIFICATION");
            entity.Property(e => e.NationalId).HasColumnName("NATIONAL_ID");
        });
        modelBuilder.Entity<WrkPermAttndnc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ID");
            entity.ToTable("WRKPERM_ATTNDNC");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.RegNum).HasColumnName("REG_NUM");
            entity.Property(e => e.WorkerId).HasColumnName("WORKER_ID");
            entity.Property(e => e.Date).HasColumnName("DATE");
            entity.Property(e => e.InTime).HasColumnName("TIME_IN");
            entity.Property(e => e.OutTime).HasColumnName("TIME_OUT");
            entity.Property(e => e.Flag).HasColumnName("FLAG");
        });
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ID");
            entity.ToTable("PTC_USER");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Username).HasColumnName("USERNAME");
            entity.Property(e => e.Password).HasColumnName("PASSWORD");
        });
        modelBuilder.Entity<UserRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ID");
            entity.ToTable("PTC_USER_RECORD");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Username).HasColumnName("USERNAME");
            entity.Property(e => e.Address).HasColumnName("ADDRESS");
            entity.Property(e => e.LoginTime).HasColumnName("LOGIN_TIME");
        });
        modelBuilder.Entity<GuestReg>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ID");
            entity.ToTable("GUEST_REG", tb => tb.HasTrigger("UPDATEDATE"));
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasColumnName("NAME");
            entity.Property(e => e.Email).HasColumnName("EMAIL");
            entity.Property(e => e.CountryCode).HasColumnName("COUNTRY_CODE");
            entity.Property(e => e.CategoryId).HasColumnName("CATEGORY_ID");
            entity.Property(e => e.Company).HasColumnName("COMPANY");
            entity.Property(e => e.CompanyType).HasColumnName("COMPANY_TYPE");
            entity.Property(e => e.DeptSect).HasColumnName("DEPT_SECT");
            entity.Property(e => e.NationalId).HasColumnName("NATIONAL_ID");
            entity.Property(e => e.Total).HasColumnName("TOTAL");
            entity.Property(e => e.Requirement).HasColumnName("REQUIREMENT");
            entity.Property(e => e.ReqId).HasColumnName("REQ_ID");
            entity.Property(e => e.MetId).HasColumnName("MET_ID");
            entity.Property(e => e.ImageData).HasColumnName("IMAGE_DATA");
            entity.Property(e => e.StartDate).HasColumnName("START_DATE");
            entity.Property(e => e.EndDate).HasColumnName("END_DATE");
            entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT");
        });
        modelBuilder.Entity<GuestAttndnc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ID");
            entity.ToTable("GUEST_ATTNDNC");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TransactionId).HasColumnName("TRANSACTION_ID");
            entity.Property(e => e.Date).HasColumnName("DATE");
            entity.Property(e => e.Total).HasColumnName("TOTAL");
            entity.Property(e => e.TimeIn).HasColumnName("TIME_IN");
            entity.Property(e => e.TimeOut).HasColumnName("TIME_OUT");
            entity.Property(e => e.Flag).HasColumnName("FLAG");
        });
        modelBuilder.Entity<VhcSpplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ID");

            entity.ToTable("VHC_SPPLIER");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.VehicleId).HasColumnName("VEHICLE_ID");
            entity.Property(e => e.VehicleType).HasColumnName("VEHICLE_TYPE");
            entity.Property(e => e.Company).HasColumnName("COMPANY");
            entity.Property(e => e.PassangerCount).HasColumnName("PASSANGER_COUNT");
            entity.Property(e => e.InTime).HasColumnName("IN_TIME");
            entity.Property(e => e.Flag).HasColumnName("FLAG");
            entity.Property(e => e.ConfirmOutTime).HasColumnName("CONFIRM_OUT_TIME");
            entity.Property(e => e.VehicleImg).HasColumnName("VEHICLE_IMG");
        });
        modelBuilder.Entity<PsgSpplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ID");

            entity.ToTable("PSG_SPPLIER");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasColumnName("NAME");
            entity.Property(e => e.VhcId).HasColumnName("VHC_ID");
            entity.Property(e => e.Flag).HasColumnName("FLAG");
        });
        modelBuilder.Entity<DdedmEmployee>(entity =>
        {
            entity.HasKey(e => e.EmpId).HasName("PK__DDEDM_EM__16EBFA269A66E613");

            entity.ToTable("DDEDM_EMPLOYEE");

            entity.Property(e => e.EmpId)
                .HasMaxLength(20)
                .HasColumnName("EMP_ID");
            entity.Property(e => e.Active).HasColumnName("ACTIVE");
            entity.Property(e => e.CurrentPhone)
                .HasMaxLength(20)
                .HasColumnName("CURRENT_PHONE");
            entity.Property(e => e.Department)
                .HasMaxLength(20)
                .HasColumnName("DEPARTMENT");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("EMAIL");
            entity.Property(e => e.GradeId)
                .HasMaxLength(5)
                .HasColumnName("GRADE_ID");
            entity.Property(e => e.MobilePhone)
                .HasMaxLength(20)
                .HasColumnName("MOBILE_PHONE");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("NAME");
            entity.Property(e => e.PrivateEmail)
                .HasMaxLength(50)
                .HasColumnName("PRIVATE_EMAIL");
            entity.Property(e => e.SectionStru)
                .HasMaxLength(50)
                .HasColumnName("SECTION_STRU");
        });
        modelBuilder.Entity<TrxTmsEmpPcDtKpFlt>(entity =>
        {
            entity.HasKey(e => e.SeqNo).HasName("PK__TRXTMS_E__04B917664F5755A5");

            entity.ToTable("TRXTMS_EMP_PCDTKP_FLT");

            entity.Property(e => e.SeqNo).HasColumnName("SEQ_NO");
            entity.Property(e => e.Date).HasColumnName("DATE");
            entity.Property(e => e.Department)
                .HasMaxLength(20)
                .HasColumnName("DEPARTMENT");
            entity.Property(e => e.EmpId)
                .HasMaxLength(20)
                .HasColumnName("EMP_ID");
            entity.Property(e => e.Flag).HasColumnName("FLAG");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("NAME");
            entity.Property(e => e.Reason)
                .HasMaxLength(50)
                .HasColumnName("REASON");
            entity.Property(e => e.TimeOut)
                .HasPrecision(0)
                .HasColumnName("TIME_OUT");
            entity.Property(e => e.TimeReturn)
                .HasPrecision(0)
                .HasColumnName("TIME_RETURN");
            entity.Property(e => e.TimereasonId)
                .HasMaxLength(5)
                .HasColumnName("TIMEREASON_ID");
            entity.Property(e => e.CreateBy)
            .HasColumnName("CREATE_BY");
        });
        modelBuilder.Entity<TrxTmsEmpPcDtKpFltAct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TRXTMS_E__3214EC27E7A29CD2");

            entity.ToTable("TRXTMS_EMP_PCDTKP_FLT_ACT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.SeqNo).HasColumnName("SEQ_NO");
            entity.Property(e => e.Date).HasColumnName("DATE");
            entity.Property(e => e.Department)
                .HasMaxLength(20)
                .HasColumnName("DEPARTMENT");
            entity.Property(e => e.EmpId)
                .HasMaxLength(20)
                .HasColumnName("EMP_ID");
            entity.Property(e => e.Flag).HasColumnName("FLAG");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("NAME");
            entity.Property(e => e.Reason)
                .HasMaxLength(50)
                .HasColumnName("REASON");
            entity.Property(e => e.TimeOut)
                .HasPrecision(0)
                .HasColumnName("TIME_OUT");
            entity.Property(e => e.TimeReturn)
                .HasPrecision(0)
                .HasColumnName("TIME_RETURN");
            entity.Property(e => e.TimereasonId)
                .HasMaxLength(5)
                .HasColumnName("TIMEREASON_ID");
            entity.Property(e => e.CreateBy)
            .HasColumnName("CREATE_BY");
        });
        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
