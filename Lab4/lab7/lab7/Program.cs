using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ��������� ������� ��� ������
builder.Services.AddDistributedMemoryCache();  // ��� �������� ������ ������ � ����������� ������ 

// ������������ ������
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // ������������� ����� ����������� ������������, ����� �������� ������ �������
    // options.Cookie.HttpOnly = true;  // ������ ���� ������ ���������� ������ ����� HTTP 
    // options.Cookie.IsEssential = true;  // ���������, ��� ���� ������ ����� ��� ������ ����������
});

// ��������� �������� ���� ������
builder.Services.AddDbContext<AppDbContext>();  

// ������������ ������������� ���
builder.Services.AddDistributedMemoryCache();  


builder.Services.AddSession();  // ��������� ������������ ������ � ����������


builder.Services.AddControllersWithViews();  // ����������� MVC-�������� (����������� � �������������) � ����������

var app = builder.Build();  // ������ ���������� �� ������ ��������

app.UseSession();  // ���������� middleware ��� ������ � ��������

app.UseRouting();  // �������� ������������� �������� � ����������

app.MapControllerRoute(
    name: "default",  
    pattern: "{controller=Home}/{action=Index}/{id?}"  
);

app.Run();  

