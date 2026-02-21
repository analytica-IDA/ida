using Microsoft.EntityFrameworkCore;

namespace backend.Data.Seeders
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.Migrate();
        }
    }
}
