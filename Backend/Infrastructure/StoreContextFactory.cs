using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure;

// Denne klassen brukes KUN av "dotnet ef"-kommandoene (migrations, database update)
// -- den har ingenting å si for hvordan appen din faktisk kjører i produksjon/
// vanlig bruk. Den er bare en "instruksjon" for verktøyet: "sånn lager du en
// StoreContext når du trenger å se på databasestrukturen".
public class StoreContextFactory : IDesignTimeDbContextFactory<StoreContext>
{
    public StoreContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<StoreContext>();
        optionsBuilder.UseSqlite("Data Source=klinikk.db");
        return new StoreContext(optionsBuilder.Options);
    }
}