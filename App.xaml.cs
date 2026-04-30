using Microsoft.EntityFrameworkCore;
using RPGManager.Data;
using System.Windows;

namespace RPGManager;

public partial class App : Application
{
   protected override void OnStartup(StartupEventArgs e)
   {
            base.OnStartup(e);
            using var db = DbContextFactory.Create();
            db.Database.Migrate();
    }
}

