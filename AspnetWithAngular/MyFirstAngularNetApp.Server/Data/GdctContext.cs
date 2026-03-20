using Microsoft.EntityFrameworkCore;
using MyFirstAngularNetApp.Server.Models;
using System;
using Attribute = MyFirstAngularNetApp.Server.Models.Attribute;

namespace MyFirstAngularNetApp.Server.Data;

public partial class GdctContext(DbContextOptions<GdctContext> options) : DbContext(options)
{
    /*  public GdctContext()
      {
      }

      public GdctContext(DbContextOptions<GdctContext> options)
          : base(options)
      {
      }
    */

   // public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public virtual DbSet<MyFirstAngularNetApp.Server.Models.Attribute> GDCTAttributes { get; set; }
    public virtual DbSet<Attribute> Attributes { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryGroup> CategoryGroups { get; set; }

    public virtual DbSet<CombineReport> CombineReports { get; set; }
    public virtual DbSet<FiscalYear> FiscalYears { get; set; }

    public virtual DbSet<Gdctdatum> Gdctdata { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<ReportingPeriod> ReportingPeriods { get; set; }

    public virtual DbSet<Submission> Submissions { get; set; }
    public virtual DbSet<SubmissionApprover> SubmissionApprovers { get; set; }
    public virtual DbSet<SubmissionSetting> SubmissionSettings { get; set; }

    public virtual DbSet<Template> Templates { get; set; }

    public virtual DbSet<UnitofMeasure> UnitofMeasures { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Sector> Sectors { get; set; }

    public virtual DbSet<NavMenu> NavMenus { get; set; }

    public virtual DbSet<NavMenuRole> NavMenusRoles { get; set; }

    public virtual DbSet<LookupMapping> LookupMappings { get; set; }

    public virtual DbSet<PublishLog> PublishLogs { get; set; }
    public virtual DbSet<AppConfig> AppConfigs { get; set; }
    public virtual DbSet<RecordStatus> RecordStatuses { get; set; }
    public virtual DbSet<SubmissionStatus> SubmissionStatuses { get; set; }
    
    public virtual DbSet<TemplateStatus> TemplateStatuses { get; set; }

    public virtual DbSet<MessageTemplate> MessageTemplates { get; set; }

    public virtual DbSet<ReportTemplate> ReportTemplates { get; set; }
    public virtual DbSet<ReportTemplateCategory> ReportTemplateCategories { get; set; }
    public virtual DbSet<ReportTemplateData> ReportTemplateData { get; set; }
    public virtual DbSet<ReportAttributeMapping> ReportAttributeMappings { get; set; }
    public virtual DbSet<ReportAttributeMappingDetail> ReportAttributeMappingDetails { get; set; }
    public virtual DbSet<ReportExcludeAttribute> ReportExcludeAttributes { get; set; }
    public virtual DbSet<ImportReport> ImportReports { get; set; }

    public virtual DbSet<ResetPasswordRequest> ResetPasswordRequest { get; set; }

    public virtual DbSet<UserReport> UserReports { get; set; }
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<AppConfig>(entity =>
        {
            entity.ToTable("AppConfig");

            entity.Property(e => e.ConfigItem).HasMaxLength(50);
        });

     /*   modelBuilder.Entity<RefreshToken>(e =>
        {
            e.HasIndex(rt => rt.TokenHash).IsUnique();
            e.Property(rt => rt.TokenHash).IsRequired();
            e.Property(rt => rt.TokenSalt).IsRequired();

            e.HasOne(rt => rt.User)
             .WithMany(u => u.RefreshTokens)
             .HasForeignKey(rt => rt.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        */
        modelBuilder.Entity<Attribute>(entity =>
        {
            entity.ToTable("Attribute"); // Update the table name to "Attribute"

            entity.HasKey(e => e.AttributeId); // Specify AttributeId as the primary key

            entity.Property(e => e.AttributeId).ValueGeneratedNever();
            entity.Property(e => e.AttributeName).HasMaxLength(200);
            entity.HasIndex(e => e.AttributeName, "UNIK_AttributeName").IsUnique();

            entity.Ignore(e => e.Id); // Ignore the inherited Id property
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).ValueGeneratedNever();
            entity.Property(e => e.CategoryName).HasMaxLength(200);

            entity.HasOne(d => d.CategoryGroup).WithMany(p => p.Categories)
                .HasForeignKey(d => d.CategoryGroupId)
                .HasConstraintName("FK_Category_CategoryGroup");

            entity.HasOne(d => d.UnitofMeasure).WithMany(p => p.Categories)
                .HasForeignKey(d => d.UnitofMeasureId)
                .HasConstraintName("FK_Category_UnitofMeasure");
            
            entity.Property(e => e.EditFlag).HasDefaultValue(true);
            entity.Ignore(e => e.Id); // Ignore the inherited Id property
        });

        modelBuilder.Entity<CategoryGroup>(entity =>
        {
            entity.ToTable("CategoryGroup");

            entity.HasIndex(e => e.CategoryGroupName, "UNIK_CategoryGroup").IsUnique();

            entity.Property(e => e.CategoryGroupName)
                .HasMaxLength(200)
                .HasColumnName("CategoryGroupName");

        });

        modelBuilder.Entity<FiscalYear>(entity =>
        {
            entity.ToTable("FiscalYear");

            entity.HasIndex(e => e.FiscalYearName, "UNIK_FiscalYear").IsUnique();

            entity.Property(e => e.FiscalYearName)
                .HasMaxLength(50)
                .HasColumnName("FiscalYearName");
        });

        modelBuilder.Entity<Gdctdatum>(entity =>
        {
            entity.ToTable("GDCTData");

            //entity.HasIndex(e => e.OrgId, "OrgId_GDCTData");
            entity.Property(e => e.CategoryName).HasMaxLength(200);
            entity.Property(e => e.AttributeName).HasMaxLength(200);
            entity.Property(e => e.DataValue).HasMaxLength(300);
            entity.Property(e => e.Worksheet).HasMaxLength(50);

            entity.HasOne(d => d.GDCTAttribute).WithMany(p => p.Gdctdata)
                .HasForeignKey(d => d.AttributeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GDCTData_Attribute");

            entity.HasOne(d => d.Category).WithMany(p => p.Gdctdata)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GDCTData_Category");

            entity.HasOne(d => d.Template).WithMany(p => p.Gdctdata)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("FK_GDCTData_Template");

            entity.HasOne(d => d.Organization).WithMany(p => p.Gdctdata)
                .HasForeignKey(d => d.OrgId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("FK_GDCTData_Organization");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Organizations_Id");
            entity.HasIndex(e => e.FacilityNo, "FacilityNo_Organizations").IsUnique();

            entity.Property(e => e.Address1).HasMaxLength(300);
            entity.Property(e => e.Address2).HasMaxLength(300);
            entity.Property(e => e.BoardChairEmail).HasMaxLength(50);
            entity.Property(e => e.BoardChairName).HasMaxLength(100);
            entity.Property(e => e.BoardChairPosition).HasMaxLength(100);
            entity.Property(e => e.BoardChairTele).HasMaxLength(50);
            entity.Property(e => e.BoardCoChairEmail).HasMaxLength(50);
            entity.Property(e => e.BoardCoChairName).HasMaxLength(100);
            entity.Property(e => e.BoardCoChairPosition).HasMaxLength(100);
            entity.Property(e => e.BoardCoChairTele).HasMaxLength(50);
            entity.Property(e => e.CFOEmail)
                .HasMaxLength(50)
                .HasColumnName("CFOEmail");
            entity.Property(e => e.CFOName)
                .HasMaxLength(100)
                .HasColumnName("CFOName");
            entity.Property(e => e.CFOPosition)
                .HasMaxLength(100)
                .HasColumnName("CFOPosition");
            entity.Property(e => e.CFOTele)
                .HasMaxLength(50)
                .HasColumnName("CFOTele");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.FinanceEmail).HasMaxLength(50);
            entity.Property(e => e.FinanceName).HasMaxLength(100);
            entity.Property(e => e.FinancePosition).HasMaxLength(100);
            entity.Property(e => e.FinanceTele).HasMaxLength(50);
            entity.Property(e => e.IFIS)
                .HasMaxLength(50)
                .HasColumnName("IFIS");
            entity.Property(e => e.LHINName)
                .HasMaxLength(200)
                .HasColumnName("LHINName");
            entity.Property(e => e.OrganizationLegalName).HasMaxLength(200);
            entity.Property(e => e.OrganizationName).HasMaxLength(200);
            entity.Property(e => e.PostalCode).HasMaxLength(50);

            entity.HasOne(d => d.Region).WithMany(p => p.Organizations)
               .HasForeignKey(d => d.RegionId)
               .HasConstraintName("FK_Organizations_Region");
            entity.HasOne(d => d.Sector).WithMany(p => p.Organizations)
                .HasForeignKey(d => d.SectorId)
                .HasConstraintName("FK_Organizations_Sector");
        });

        modelBuilder.Entity<ReportingPeriod>(entity =>
        {
            entity.ToTable("ReportingPeriod");

            entity.Property(e => e.ReportingPeriodName)
                .HasMaxLength(50)
                .HasColumnName("ReportingPeriodName");
        });

        modelBuilder.Entity<Submission>(entity =>
        {
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.SubmissionLocation).HasMaxLength(300);
            entity.Property(e => e.SubmissionName).HasMaxLength(300);
            entity.Property(e => e.SubmissionStatus).HasDefaultValue(1);
            entity.Property(e => e.SubmittedDate).HasColumnType("datetime");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Org).WithMany(p => p.Submissions)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.OrgId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Submissions_Organizations");
            entity.HasOne(d => d.Template).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Submissions_Template");
            entity.HasOne(d => d.SubmissionRequestor).WithMany(p => p.SubmissionRequestors)
                .HasForeignKey(d => d.RequestorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Submission_UserInfo");
            entity.HasOne(d => d.SubmissionSetting).WithMany(p => p.Submissions)
               .HasForeignKey(d => d.SubmissionSettingId)
               .HasConstraintName("FK_Submissions_SubmissionSetting");
            entity.HasOne(d => d.SubmissionStatusNavigation).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.SubmissionStatus)
                .HasConstraintName("FK_Submissions_SubmissionStatus");
        });

        modelBuilder.Entity<ResetPasswordRequest>(entity =>
        {
            entity.ToTable("ResetPasswordRequest");
        });

            modelBuilder.Entity<Template>(entity =>
        {
            entity.ToTable("Template");

            entity.HasKey(e => e.Id).HasName("PK_Template_Id");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.TemplateName).HasMaxLength(100);
            entity.Property(e => e.Jsonschema).HasColumnName("JSONSchema");
            
            entity.HasOne(d => d.FiscalYear).WithMany(p => p.Templates)
                .HasForeignKey(d => d.FiscalYearId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Template_FiscalYear");
            
            entity.HasOne(d => d.ReportingPeriod).WithMany(p => p.Templates)
                .HasForeignKey(d => d.ReportingPeriodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Template_ReportingPeriod");
            
            entity.Property(e => e.TemplateLocation).HasMaxLength(300);

            entity.HasOne(d => d.TemplateReviewer).WithMany(p => p.TemplateReviewers)
                .HasForeignKey(d => d.TemplateReviewerId)
                .HasConstraintName("FK_Template_UserInfo_TemplateReviewerId");

            entity.HasOne(d => d.TemplateApprover).WithMany(p => p.TemplateApprovers)
                .HasForeignKey(d => d.TemplateApproverId)
                .HasConstraintName("FK_Template_UserInfo_TemplateApproverId");
            
            entity.Property(e => e.TemplateStatus).HasDefaultValue(1);
            entity.HasOne(d => d.Requestor).WithMany(p => p.Requestors)
                .HasForeignKey(d => d.RequestorId)
                .HasConstraintName("FK_Template_UserInfo_RequestorId");
            
            entity.HasOne(d => d.Sector).WithMany(p => p.Templates)
                .HasForeignKey(d => d.SectorId)
                .HasConstraintName("FK_Template_Sector");

            entity.HasOne(d => d.TemplateStatusNavigation).WithMany(p => p.Templates)
                .HasForeignKey(d => d.TemplateStatus)
                .HasConstraintName("FK_Template_TemplateStatus");
        });

        modelBuilder.Entity<SubmissionApprover>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Approver");
            entity.ToTable("SubmissionApprover");
            entity.HasOne(d => d.SubmissionSetting).WithMany(p => p.SubmissionApprovers)
                .HasForeignKey(d => d.SubmissionSettingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubmissionApprover_SubmissionSetting");

            entity.HasOne(d => d.User).WithMany(p => p.SubmissionApprovers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubmissionApprover_UserInfo");
        });
        modelBuilder.Entity<SubmissionSetting>(entity =>
        {
            entity.ToTable("SubmissionSetting");
            entity.Property(e => e.SubmissionName).HasMaxLength(200);
            entity.HasOne(d => d.SubmissionReviewer).WithMany(p => p.SubmissionSettings)
                .HasForeignKey(d => d.SubmissionReviewerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubmissionSetting_UserInfo");
            entity.HasOne(d => d.Template).WithMany(p => p.SubmissionSettings)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_SubmissionSetting_Template");
        });

        modelBuilder.Entity<SubmissionSetting>()
            .HasOne(o => o.Template)
            .WithMany(c => c.SubmissionSettings)
            .HasForeignKey(o => o.TemplateId)      // FK property on Template
            .OnDelete(DeleteBehavior.Restrict);    // or Cascade/ClientCascade as needed

        modelBuilder.Entity<SubmissionSetting>().HasMany(s => s.SubmissionApprovers)
            .WithOne(a => a.SubmissionSetting)
            .HasForeignKey(a => a.SubmissionSettingId)
            .OnDelete(DeleteBehavior.ClientSetNull);


        modelBuilder.Entity<UnitofMeasure>(entity =>
        {
            entity.ToTable("UnitofMeasure");

            entity.HasIndex(e => e.UnitofMeasureName, "IX_UnitofMeasure").IsUnique();

            entity.Property(e => e.UnitofMeasureName)
                .HasMaxLength(50)
                .HasColumnName("UnitofMeasureName");

            entity.Property(e => e.DataType)
                .HasMaxLength(50)
                .HasColumnName("DataType");

            entity.Property(e => e.ValidationRule)
                .HasMaxLength(100)
                .HasColumnName("ValidationRule");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("UserInfo");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(100);
            
            entity.HasOne(d => d.UserRole).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserInfo_UserRole");
        });

        modelBuilder.Entity<RefreshToken>()
                 .HasIndex(rt => rt.TokenHash)
                 .IsUnique();

        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId);



        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRole");

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.ToTable("Region");

            entity.Property(e => e.RegionName).HasMaxLength(50);
        });

        modelBuilder.Entity<Sector>(entity =>
        {
            entity.ToTable("Sector");

            entity.Property(e => e.SectorName).HasMaxLength(100);
        });

        modelBuilder.Entity<NavMenu>(entity =>
        {
            entity.ToTable("NavMenu");
            entity.Property(e => e.MenuItemName).HasMaxLength(200);
            entity.Property(e => e.NavMenuIcon).HasMaxLength(300);
            entity.Property(e => e.NavUrl).HasMaxLength(300);
            entity.Property(e => e.PageName).HasMaxLength(200);
        });

        modelBuilder.Entity<NavMenuRole>(entity =>
        {
            entity.ToTable("NavMenuRole");
            entity.HasOne(d => d.NavMenu).WithMany(p => p.NavMenuRoles)
                .HasForeignKey(d => d.NavMenuId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NavMenuRole_NavMenu");
            entity.HasOne(d => d.UserRole).WithMany(p => p.NavMenuRoles)
                .HasForeignKey(d => d.UserRoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NavMenuRole_UserRole");
        });

        modelBuilder.Entity<LookupMapping>(entity =>
        {
            entity.ToTable("LookupMapping");
            entity.Property(e => e.MappedField).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(10);
            entity.Property(e => e.VariableName).HasMaxLength(50);
            entity.Property(e => e.Worksheet).HasMaxLength(50);
            entity.HasOne(d => d.Template).WithMany(p => p.LookupMappings)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LookupMapping_Template");
        });

        modelBuilder.Entity<PublishLog>(entity =>
        {
            entity.Property(e => e.PublishedBy).HasMaxLength(50);

            entity.HasOne(d => d.Org).WithMany(p => p.PublishLogs)
                .HasPrincipalKey(p => p.FacilityNo)
                .HasForeignKey(d => d.OrgId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PublishLogs_Organizations");

            entity.HasOne(d => d.Template).WithMany(p => p.PublishLogs)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PublishLogs_Template");
        });

        modelBuilder.Entity<GdctdataCalculation>(entity =>
        {
            entity.ToTable("GDCTDataCalculation");

            entity.Property(e => e.CellFormula).HasMaxLength(1000);
            entity.Property(e => e.CellLocation).HasMaxLength(50);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataValue).HasMaxLength(300);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(10);
            entity.Property(e => e.Worksheet).HasMaxLength(50);

            entity.HasOne(d => d.Attribute).WithMany(p => p.GdctdataCalculations)
                .HasForeignKey(d => d.AttributeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GDCTDataCalculation_Attribute");

            entity.HasOne(d => d.Category).WithMany(p => p.GdctdataCalculations)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GDCTDataCalculation_Category");

            entity.HasOne(d => d.Org).WithMany(p => p.GdctdataCalculations)
                .HasForeignKey(d => d.OrgId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GDCTDataCalculation_Organizations");

            entity.HasOne(d => d.Template).WithMany(p => p.GdctdataCalculations)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GDCTDataCalculation_Template");
        });

        modelBuilder.Entity<SubmissionStatus>(entity =>
        {
            entity.ToTable("SubmissionStatus");
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<TemplateStatus>(entity =>
        {
            entity.ToTable("TemplateStatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<RecordStatus>(entity =>
        {
            entity.ToTable("RecordStatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<MessageTemplate>(entity =>
        {
            entity.ToTable("MessageTemplate");

            entity.Property(e => e.Body).HasColumnType("text");
            entity.Property(e => e.Comment).HasMaxLength(300);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.MessageTemplateKey).HasMaxLength(100);
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);
            entity.Property(e => e.Status).HasDefaultValue(1);
            entity.Property(e => e.Subject).HasMaxLength(300);
            entity.Property(e => e.ChannelId).HasColumnType("int");
        });

        modelBuilder.Entity<CombineReport>(entity =>
        {
            entity.ToTable("CombineReport");

            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ReportName).HasMaxLength(50);
        });

        modelBuilder.Entity<ReportTemplate>(entity =>
        {
            entity.ToTable("ReportTemplate");

            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.WorkSheet).HasMaxLength(50);
        });

        modelBuilder.Entity<ReportTemplateCategory>(entity =>
        {
            entity.ToTable("ReportTemplateCategory");

            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
        });

        modelBuilder.Entity<ReportTemplateData>(entity =>
        {
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.TemplateId).HasMaxLength(50);
        });

        modelBuilder.Entity<ReportAttributeMapping>(entity =>
        {
            entity.ToTable("ReportAttributeMapping");

            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.DisplayAttributeName).HasMaxLength(500);
            entity.Property(e => e.MappingName).HasMaxLength(200);
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
        });

        modelBuilder.Entity<ReportAttributeMappingDetail>(entity =>
        {
            entity.ToTable("ReportAttributeMappingDetail");

            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);

            entity.HasOne(d => d.ReportAttributeMapping).WithMany(p => p.ReportAttributeMappingDetails)
                .HasForeignKey(d => d.ReportAttributeMappingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportAttributeMappingDetail_ReportAttributeMapping");
        });

        modelBuilder.Entity<ReportExcludeAttribute>(entity =>
        {
            entity.ToTable("ReportExcludeAttribute");
            entity.Property(e => e.AttributeId);
            entity.Property(e => e.Comments).HasMaxLength(500);
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
        });

        modelBuilder.Entity<ImportReport>(entity =>
        {
            entity.ToTable("ImportReport");
            entity.Property(e => e.ImportReportName).HasMaxLength(200);
            entity.Property(e => e.Summary).HasMaxLength(1000);
            entity.Property(e => e.ImportLogLocation).HasMaxLength(1000);
            entity.Property(e => e.TableName).HasMaxLength(100);
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
        });

        modelBuilder.Entity<UserReport>(entity =>
        {
            entity.ToTable("UserReport");

            entity.Property(e => e.ReportName).HasMaxLength(200);
            entity.Property(e => e.ReportDescription).HasMaxLength(300);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}