using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using sib_love_site.Areas.Identity.Data;
using sib_love_site.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace sib_love_site.Data
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Answer> Answers { get; set; }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries();
            var data = new Dictionary<string, List<object>>();

            foreach (var entry in entities)
            {
                var entityType = entry.Entity.GetType().Name;

                if (!data.ContainsKey(entityType))
                {
                    data[entityType] = new List<object>();
                }

                if (entry.State == EntityState.Added)
                {
                    data[entityType].Add(entry.Entity);
                }
            }
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "datum.json");
            Dictionary<string, List<object>> existingData = null;

            if (File.Exists(filePath))
            {
                var existingJson = await File.ReadAllTextAsync(filePath, cancellationToken);
                existingData = JsonConvert.DeserializeObject<Dictionary<string, List<object>>>(existingJson);
            }
            existingData ??= new Dictionary<string, List<object>>();

            foreach (var kvp in data)
            {
                var entityType = kvp.Key;
                var entitiesToAdd = kvp.Value;

                if (existingData.ContainsKey(entityType))
                {
                    existingData[entityType].AddRange(entitiesToAdd);
                }
                else
                {
                    existingData[entityType] = new List<object>(entitiesToAdd);
                }
            }

            var json = JsonConvert.SerializeObject(existingData, Formatting.Indented);

            await File.WriteAllTextAsync(filePath, json, cancellationToken);

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}