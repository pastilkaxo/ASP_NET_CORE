var builder = WebApplication.CreateBuilder(args); // создает обьект веб приложения

// Add services to the container.
builder.Services.AddControllersWithViews(); // поддержка mvc контроллеров с представлениями

var app = builder.Build(); // создает экземпляр приложения

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
} // включает обработчик исключений для неразработческой среды

app.UseStaticFiles(); // обслуживание статических ресурсов из wwwroot

app.UseRouting(); // включает маршрутизацию

app.UseAuthorization(); // добавляет промежуточное ПО для авторизации; проверяет права доступа к ресурсам

app.MapControllerRoute( // маршрут по умолчанию
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
