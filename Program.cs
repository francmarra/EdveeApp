var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // Sessões ativadas

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // Sessões ativadas aqui
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Conta}/{action=Login}/{id?}");

app.Run();
