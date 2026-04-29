using Microsoft.EntityFrameworkCore;

namespace RPGManager.Data
{
    public static class DbContextFactory
    {
        public static DndDBContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DndDBContext>();
            optionsBuilder.UseSqlite("Data Source=campaign.db");
            return new DndDBContext(optionsBuilder.Options);
        }
    }
}