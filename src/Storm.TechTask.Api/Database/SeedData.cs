using Microsoft.EntityFrameworkCore;

using Storm.TechTask.Core.ProjectAggregate;
using Storm.TechTask.Infrastructure.Repository;

namespace Storm.TechTask.Api.Database
{
    public static class SeedData
    {
        public static readonly Project TestProject1 = new Project("Test Project", ProjectCategory.Development, false, ProjectStatus.Open);
        public static readonly ToDoItem ToDoItem1 = new ToDoItem
        {
            Title = "Get Sample Working",
            Description = "Try to get the sample to build."
        };
        public static readonly ToDoItem ToDoItem2 = new ToDoItem
        {
            Title = "Review Solution",
            Description = "Review the different projects in the solution and how they relate to one another."
        };
        public static readonly ToDoItem ToDoItem3 = new ToDoItem
        {
            Title = "Run and Review Tests",
            Description = "Make sure all the tests run and review what they are doing."
        };

        public static void Initialize(IHostEnvironment environment, IServiceProvider serviceProvider)
        {
            if (environment.EnvironmentName != "ApiTest")
            {
                using (var dbContext = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), null))
                {
                    // Look for any Projects.
                    if (dbContext.Set<Project>().Any())
                    {
                        return;   // DB has been seeded
                    }

                    PopulateTestData(dbContext);
                }
            }
        }

        public static void PopulateTestData(AppDbContext dbContext)
        {
            foreach (var item in dbContext.Set<ToDoItem>())
            {
                dbContext.Remove(item);
            }
            foreach (var project in dbContext.Set<Project>())
            {
                dbContext.Remove(project);
            }
            dbContext.SaveChanges();

            if (TestProject1.Items.Count() == 0)
            {
                TestProject1.AddItem(ToDoItem1.Title, ToDoItem1.Description);
                TestProject1.AddItem(ToDoItem2.Title, ToDoItem2.Description);
                TestProject1.AddItem(ToDoItem3.Title, ToDoItem3.Description);
            }

            dbContext.Add(TestProject1);

            dbContext.SaveChanges();
        }
    }
}
