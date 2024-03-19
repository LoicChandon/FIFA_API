﻿using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Repository
{
    public class UtilisateurManager : BaseRepository<Utilisateur>, IUtilisateurManager
    {
        public UtilisateurManager(FifaDbContext dbContext) : base(dbContext) { }

        public async Task<Utilisateur?> GetByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Utilisateur?> GetByEmailAsync(string email)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.Mail == email);
        }

        public async Task<bool> IsEmailTaken(string email)
        {
            return await DbSet.AnyAsync(u => u.Mail == email);
        }

    }
}