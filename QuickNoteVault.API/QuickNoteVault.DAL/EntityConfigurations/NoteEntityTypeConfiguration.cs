using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickNoteVault.DAL.Entities;

namespace QuickNoteVault.DAL.EntityConfigurations
{
    public class NoteEntityTypeConfiguration : IEntityTypeConfiguration<NoteEntity>
    {
        public void Configure(EntityTypeBuilder<NoteEntity> builder)
        {
            builder.ToTable("Note");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(n => n.Content)
                  .IsRequired();

            builder.Property(n => n.CreatedAt)
                   .IsRequired();

            builder.HasOne(n => n.User)
                  .WithMany(u => u.Notes)
                  .HasForeignKey(n => n.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
