using Microsoft.EntityFrameworkCore;
using Edveeeeeee.Data; // se tiveres a pasta Data


//11

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // Sessões ativadas
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EdveeDb;Trusted_Connection=True;"));


builder.Services.AddHttpContextAccessor();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(db);
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // Sessões ativadas aqui
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Conta}/{action=Login}/{id?}");

app.Run();
