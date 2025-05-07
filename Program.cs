using Microsoft.EntityFrameworkCore;
using Edveeeeeee.Data; // se tiveres a pasta Data




var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // Sess�es ativadas
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=edvee.db"));


var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // Sess�es ativadas aqui
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Conta}/{action=Login}/{id?}");

app.Run();
