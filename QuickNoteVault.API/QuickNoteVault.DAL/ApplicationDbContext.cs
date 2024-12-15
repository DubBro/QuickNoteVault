using Microsoft.EntityFrameworkCore;
using QuickNoteVault.DAL.Entities;

namespace QuickNoteVault.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        required public DbSet<NoteEntity> Notes { get; set; }
        required public DbSet<UserEntity> Users { get; set; }
    }
}
