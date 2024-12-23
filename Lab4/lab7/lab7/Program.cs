using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы для сессий
builder.Services.AddDistributedMemoryCache();  // Для хранения данных сессий в оперативной памяти 

// Конфигурация сессий
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Устанавливаем время бездействия пользователя, после которого сессия истечёт
    // options.Cookie.HttpOnly = true;  // Делаем куки сессии доступными только через HTTP 
    // options.Cookie.IsEssential = true;  // Указываем, что куки сессии важны для работы приложения
});

// Добавляем контекст базы данных
builder.Services.AddDbContext<AppDbContext>();  

// Регистрируем распределённый кэш
builder.Services.AddDistributedMemoryCache();  


builder.Services.AddSession();  // Позволяет использовать сессии в приложении


builder.Services.AddControllersWithViews();  // Регистрация MVC-паттерна (контроллеры и представления) в приложении

var app = builder.Build();  // Создаём приложение на основе настроек

app.UseSession();  // Подключаем middleware для работы с сессиями

app.UseRouting();  // Включаем маршрутизацию запросов в приложении

app.MapControllerRoute(
    name: "default",  
    pattern: "{controller=Home}/{action=Index}/{id?}"  
);

app.Run();  

