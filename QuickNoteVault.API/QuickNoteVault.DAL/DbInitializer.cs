using Microsoft.EntityFrameworkCore;
using QuickNoteVault.DAL.Entities;

namespace QuickNoteVault.DAL;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.Users.AnyAsync())
        {
            await context.AddRangeAsync(
                new List<UserEntity>()
                {
                    new UserEntity()
                    {
                        FirstName = "John",
                        LastName = "Smith",
                        Email = "john.smith.1099@gmail.com",
                        Password = "j0hny10"
                    },
                    new UserEntity()
                    {
                        FirstName = "Stepan",
                        LastName = "Koval",
                        Email = "stepan3000@gmail.com",
                        Password = "panstepan1"
                    }
                });

            await context.SaveChangesAsync();
        }

        if (!await context.Notes.AnyAsync())
        {
            await context.AddRangeAsync(
                new List<NoteEntity>()
                {
                    new NoteEntity()
                    {
                        Title = "Team Meeting Agenda",
                        Content = "[{\"type\":\"h1\",\"children\":[{\"text\":\"Team Meeting Agenda\"}]}," +
                            "{\"type\":\"p\",\"children\":[{\"text\":\"Date: January 15, 2024\"}]}," +
                            "{\"type\":\"p\",\"children\":[{\"text\":\"Time: 10:00 AM - 11:00 AM\"}]}," +
                            "{\"type\":\"p\",\"children\":[{\"text\":\"Location: Conference Room B\"}]}," +
                            "{\"type\":\"p\",\"listStyleType\":\"disc\",\"listStart\":1,\"indent\":1,\"children\":" +
                            "[{\"text\":\"Project updates\"}]}," +
                            "{\"type\":\"p\",\"listStyleType\":\"disc\",\"listStart\":2,\"indent\":1,\"children\":" +
                            "[{\"text\":\"Discussion on quarterly goals\"}]}," +
                            "{\"type\":\"p\",\"listStyleType\":\"disc\",\"listStart\":3,\"indent\":1,\"children\":" +
                            "[{\"text\":\"Feedback on recent process changes\"}]}," +
                            "{\"type\":\"p\",\"listStyleType\":\"disc\",\"listStart\":4,\"indent\":1,\"children\":" +
                            "[{\"text\":\"Q&A session\"}]}," +
                            "{\"type\":\"p\",\"children\":[{\"text\":\"Please come prepared with your updates and any questions.\"}]}]",
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,
                        UserId = 1,
                        User = null!
                    },
                    new NoteEntity()
                    {
                        Title = "Project Update",
                        Content = "[{\"type\":\"h2\",\"children\":[{\"text\":\"Project Status Update: Website Redesign\"}]}," +
                            "{\"type\":\"p\",\"children\":" +
                            "[{\"text\":\"The website redesign project is progressing on schedule. Below are the key updates:\"}]}," +
                            "{\"type\":\"p\",\"listStyleType\":\"disc\",\"listStart\":1,\"indent\":1,\"children\":" +
                            "[{\"text\":\"Completed user research and wireframing.\"}]}," +
                            "{\"type\":\"p\",\"listStyleType\":\"disc\",\"listStart\":2,\"indent\":1,\"children\":" +
                            "[{\"text\":\"UI design is 75% complete and currently under review.\"}]}," +
                            "{\"type\":\"p\",\"listStyleType\":\"disc\",\"listStart\":3,\"indent\":1,\"children\":" +
                            "[{\"text\":\"Development of the homepage and product pages has started.\"}]}," +
                            "{\"type\":\"p\",\"children\":[{\"text\":\"Challenges:\"}]}," +
                            "{\"type\":\"p\",\"listStyleType\":\"disc\",\"listStart\":1,\"indent\":1,\"children\":" +
                            "[{\"text\":\"Some delays in finalizing content due to stakeholder availability.\"}]}," +
                            "{\"type\":\"p\",\"listStyleType\":\"disc\",\"listStart\":2,\"indent\":1,\"children\":" +
                            "[{\"text\":\"Minor issues with responsive design testing on older devices.\"}]}," +
                            "{\"type\":\"p\",\"children\":[{\"text\":\"Next steps:\"}]}," +
                            "{\"type\":\"p\",\"listStyleType\":\"decimal\",\"listStart\":1,\"indent\":1,\"children\":" +
                            "[{\"text\":\"Finalize the UI design by January 20.\"}]}," +
                            "{\"type\":\"p\",\"listStyleType\":\"decimal\",\"listStart\":2,\"indent\":1,\"children\":" +
                            "[{\"text\":\"Complete the first round of development by February 5.\"}]}," +
                            "{\"type\":\"p\",\"listStyleType\":\"decimal\",\"listStart\":3,\"indent\":1,\"children\":" +
                            "[{\"text\":\"Begin user acceptance testing on February 10.\"}]}]",
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,
                        UserId = 2,
                        User = null!
                    },
                    new NoteEntity()
                    {
                        Title = "Training Session Summary",
                        Content = "[{\"type\":\"h3\",\"children\":[{\"text\":\"Training Session Summary: Effective Communication\"}]}," +
                            "{\"type\":\"p\",\"children\":[{\"text\":\"Date: January 10, 2024\"}]}," +
                            "{\"type\":\"p\",\"children\":[{\"text\":\"Trainer: Sarah Johnson\"}]}," +
                            "{\"type\":\"p\",\"children\":[{\"text\":\"Key topics covered:\"}]}," +
                            "{\"type\":\"p\",\"listStyleType\":\"disc\",\"listStart\":1,\"indent\":1,\"children\":" +
                            "[{\"text\":\"Strategies for clear and concise communication.\"}]}," +
                            "{\"type\":\"p\",\"listStyleType\":\"disc\",\"listStart\":2,\"indent\":1,\"children\":" +
                            "[{\"text\":\"How to give and receive constructive feedback.\"}]}," +
                            "{\"type\":\"p\",\"listStyleType\":\"disc\",\"listStart\":3,\"indent\":1,\"children\":" +
                            "[{\"text\":\"Techniques for active listening.\"}]},{\"type\":\"p\",\"children\":" +
                            "[{\"text\":\"Action items for the team:\"}]}," +
                            "{\"type\":\"p\",\"listStyleType\":\"decimal\",\"listStart\":1,\"indent\":1,\"children\":" +
                            "[{\"text\":\"Implement feedback techniques during team meetings.\"}]}," +
                            "{\"type\":\"p\",\"listStyleType\":\"decimal\",\"listStart\":2,\"indent\":1,\"children\":" +
                            "[{\"text\":\"Practice active listening in one-on-one conversations.\"}]}," +
                            "{\"type\":\"p\",\"children\":" +
                            "[{\"text\":\"For further reading, refer to the training materials shared by Sarah.\"}]}]",
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,
                        UserId = 2,
                        User = null!
                    }
                });

            await context.SaveChangesAsync();
        }
    }
}
