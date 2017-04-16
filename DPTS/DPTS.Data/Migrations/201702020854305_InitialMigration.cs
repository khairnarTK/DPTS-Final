namespace DPTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Hospital = c.String(),
                        CountryId = c.Int(),
                        StateProvinceId = c.Int(),
                        City = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        ZipPostalCode = c.String(),
                        PhoneNumber = c.String(),
                        Website = c.String(),
                        FaxNumber = c.String(),
                        Doctor_Id = c.Int(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .ForeignKey("dbo.StateProvinces", t => t.StateProvinceId)
                .Index(t => t.CountryId)
                .Index(t => t.StateProvinceId);
            
            CreateTable(
                "dbo.AddressMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        AddressId = c.Int(nullable: false),
                        Doctor_DoctorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Doctors", t => t.Doctor_DoctorId)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .Index(t => t.UserId)
                .Index(t => t.AddressId)
                .Index(t => t.Doctor_DoctorId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 256),
                        LastName = c.String(nullable: false, maxLength: 256),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        IsEmailUnsubscribed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        IsPhoneNumberUnsubscribed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        LastIpAddress = c.String(),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        LastLoginDateUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppointmentSchedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DoctorId = c.String(nullable: false, maxLength: 128),
                        PatientId = c.String(nullable: false, maxLength: 128),
                        Subject = c.String(),
                        DiseasesDescription = c.String(),
                        StatusId = c.Int(nullable: false),
                        AppointmentTime = c.String(),
                        AppointmentDate = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppointmentStatus", t => t.StatusId)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .ForeignKey("dbo.AspNetUsers", t => t.PatientId)
                .Index(t => t.DoctorId)
                .Index(t => t.PatientId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.AppointmentStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        DoctorId = c.String(nullable: false, maxLength: 128),
                        DoctorGuid = c.Guid(nullable: false),
                        Gender = c.String(),
                        Qualifications = c.String(),
                        RegistrationNumber = c.String(),
                        YearsOfExperience = c.Int(),
                        ShortProfile = c.String(),
                        Expertise = c.String(),
                        Subscription = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        DateOfBirth = c.String(),
                        Rating = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DoctorId)
                .ForeignKey("dbo.AspNetUsers", t => t.DoctorId)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DoctorId = c.String(nullable: false, maxLength: 128),
                        Day = c.String(nullable: false, maxLength: 10),
                        StartTime = c.String(),
                        EndTime = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.SpecialityMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Doctor_Id = c.String(nullable: false, maxLength: 128),
                        Speciality_Id = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Specialities", t => t.Speciality_Id)
                .ForeignKey("dbo.Doctors", t => t.Doctor_Id)
                .Index(t => t.Doctor_Id)
                .Index(t => t.Speciality_Id);
            
            CreateTable(
                "dbo.Specialities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubSpecialities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SpecialityId = c.Int(nullable: false),
                        Name = c.String(),
                        DisplayOrder = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Specialities", t => t.SpecialityId, cascadeDelete: true)
                .Index(t => t.SpecialityId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TwoLetterIsoCode = c.String(),
                        ThreeLetterIsoCode = c.String(),
                        NumericIsoCode = c.Int(nullable: false),
                        SubjectToVat = c.Boolean(nullable: false),
                        Published = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StateProvinces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryId = c.Int(nullable: false),
                        Name = c.String(),
                        Abbreviation = c.String(),
                        Published = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.DefaultNotificationSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        Name = c.String(),
                        Message = c.String(),
                        Published = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        EmailCategory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmailCategories", t => t.EmailCategory_Id)
                .Index(t => t.EmailCategory_Id);
            
            CreateTable(
                "dbo.EmailCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Published = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DoctorNotificationSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        Name = c.String(),
                        Message = c.String(),
                        DoctorId = c.String(maxLength: 128),
                        Published = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        EmailCategory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .ForeignKey("dbo.EmailCategories", t => t.EmailCategory_Id)
                .Index(t => t.DoctorId)
                .Index(t => t.EmailCategory_Id);
            
            CreateTable(
                "dbo.ReviewComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentForId = c.Int(nullable: false),
                        CommentOwnerId = c.String(),
                        Comment = c.String(),
                        Rating = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsApproved = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        AspNetUser_Id = c.String(maxLength: 128),
                        Doctor_DoctorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUser_Id)
                .ForeignKey("dbo.Doctors", t => t.Doctor_DoctorId)
                .Index(t => t.AspNetUser_Id)
                .Index(t => t.Doctor_DoctorId);
            
            CreateTable(
                "dbo.SentEmailHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderId = c.String(),
                        ReceiverId = c.String(),
                        SenderType = c.String(),
                        ReceiverEmail = c.String(),
                        Email = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        AspNetUser_Id = c.String(maxLength: 128),
                        Doctor_DoctorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUser_Id)
                .ForeignKey("dbo.Doctors", t => t.Doctor_DoctorId)
                .Index(t => t.AspNetUser_Id)
                .Index(t => t.Doctor_DoctorId);
            
            CreateTable(
                "dbo.SentSmsHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderId = c.String(),
                        ReceiverId = c.String(),
                        SenderType = c.String(),
                        ReceiverPhone = c.String(),
                        Text = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        AspNetUser_Id = c.String(maxLength: 128),
                        Doctor_DoctorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUser_Id)
                .ForeignKey("dbo.Doctors", t => t.Doctor_DoctorId)
                .Index(t => t.AspNetUser_Id)
                .Index(t => t.Doctor_DoctorId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SentSmsHistories", "Doctor_DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.SentSmsHistories", "AspNetUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SentEmailHistories", "Doctor_DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.SentEmailHistories", "AspNetUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ReviewComments", "Doctor_DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.ReviewComments", "AspNetUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DoctorNotificationSettings", "EmailCategory_Id", "dbo.EmailCategories");
            DropForeignKey("dbo.DoctorNotificationSettings", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.DefaultNotificationSettings", "EmailCategory_Id", "dbo.EmailCategories");
            DropForeignKey("dbo.StateProvinces", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Addresses", "StateProvinceId", "dbo.StateProvinces");
            DropForeignKey("dbo.Addresses", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.AddressMappings", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Doctors", "DoctorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AppointmentSchedules", "PatientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SpecialityMappings", "Doctor_Id", "dbo.Doctors");
            DropForeignKey("dbo.SubSpecialities", "SpecialityId", "dbo.Specialities");
            DropForeignKey("dbo.SpecialityMappings", "Speciality_Id", "dbo.Specialities");
            DropForeignKey("dbo.Schedules", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.AppointmentSchedules", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.AddressMappings", "Doctor_DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.AppointmentSchedules", "StatusId", "dbo.AppointmentStatus");
            DropForeignKey("dbo.AddressMappings", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.SentSmsHistories", new[] { "Doctor_DoctorId" });
            DropIndex("dbo.SentSmsHistories", new[] { "AspNetUser_Id" });
            DropIndex("dbo.SentEmailHistories", new[] { "Doctor_DoctorId" });
            DropIndex("dbo.SentEmailHistories", new[] { "AspNetUser_Id" });
            DropIndex("dbo.ReviewComments", new[] { "Doctor_DoctorId" });
            DropIndex("dbo.ReviewComments", new[] { "AspNetUser_Id" });
            DropIndex("dbo.DoctorNotificationSettings", new[] { "EmailCategory_Id" });
            DropIndex("dbo.DoctorNotificationSettings", new[] { "DoctorId" });
            DropIndex("dbo.DefaultNotificationSettings", new[] { "EmailCategory_Id" });
            DropIndex("dbo.StateProvinces", new[] { "CountryId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.SubSpecialities", new[] { "SpecialityId" });
            DropIndex("dbo.SpecialityMappings", new[] { "Speciality_Id" });
            DropIndex("dbo.SpecialityMappings", new[] { "Doctor_Id" });
            DropIndex("dbo.Schedules", new[] { "DoctorId" });
            DropIndex("dbo.Doctors", new[] { "DoctorId" });
            DropIndex("dbo.AppointmentSchedules", new[] { "StatusId" });
            DropIndex("dbo.AppointmentSchedules", new[] { "PatientId" });
            DropIndex("dbo.AppointmentSchedules", new[] { "DoctorId" });
            DropIndex("dbo.AddressMappings", new[] { "Doctor_DoctorId" });
            DropIndex("dbo.AddressMappings", new[] { "AddressId" });
            DropIndex("dbo.AddressMappings", new[] { "UserId" });
            DropIndex("dbo.Addresses", new[] { "StateProvinceId" });
            DropIndex("dbo.Addresses", new[] { "CountryId" });
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.SentSmsHistories");
            DropTable("dbo.SentEmailHistories");
            DropTable("dbo.ReviewComments");
            DropTable("dbo.DoctorNotificationSettings");
            DropTable("dbo.EmailCategories");
            DropTable("dbo.DefaultNotificationSettings");
            DropTable("dbo.StateProvinces");
            DropTable("dbo.Countries");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.SubSpecialities");
            DropTable("dbo.Specialities");
            DropTable("dbo.SpecialityMappings");
            DropTable("dbo.Schedules");
            DropTable("dbo.Doctors");
            DropTable("dbo.AppointmentStatus");
            DropTable("dbo.AppointmentSchedules");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AddressMappings");
            DropTable("dbo.Addresses");
        }
    }
}
