var builder = WebApplication.CreateBuilder(args); // ������� ������ ��� ����������

// Add services to the container.
builder.Services.AddControllersWithViews(); // ��������� mvc ������������ � ���������������

var app = builder.Build(); // ������� ��������� ����������

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
} // �������� ���������� ���������� ��� ���������������� �����

app.UseStaticFiles(); // ������������ ����������� �������� �� wwwroot

app.UseRouting(); // �������� �������������

app.UseAuthorization(); // ��������� ������������� �� ��� �����������; ��������� ����� ������� � ��������

app.MapControllerRoute( // ������� �� ���������
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
