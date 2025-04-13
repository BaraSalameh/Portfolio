﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DataAccess.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public DbSet<Education> Education { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Link> Link { get; set; }
        public DbSet<LKP_EducationLevel> LKP_EducationLevel { get; set; }
        public DbSet<LKP_LanguageLevel> LKP_LanguageLevel { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}