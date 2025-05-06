var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // Sess�es ativadas

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // Sess�es ativadas aqui
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Conta}/{action=Login}/{id?}");

app.Run();
