namespace Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context, ILogger Logger)
    {
        try
        {
            if (!context.Courses.Any())
            {
                var courses = new List<Course>
                {
                    new Course { Title = "Hygiene og smittevern", Description = "Grunnleggende hygienerutiner.", Image = "hygiene.jpg" },
                    new Course { Title = "Klargjøring av behandlingsrom", Description = "Steg for steg mellom hver pasient.", Image = "klargjoring.jpg" },
                    new Course { Title = "Trygg assistanse i stolen", Description = "Positionering og samarbeid.", Image = "assistanse.jpg" }
                };

                context.Courses.AddRange(courses);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}