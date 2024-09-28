using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// Handler_1

// https://localhost:7256/LVO

app.MapGet("/LVO",  (context) =>
{
    string paramA = context.Request.Query["ParamA"];
    string paramB = context.Request.Query["ParamB"];
    context.Response.ContentType = "text/plain";
    string requestForamat = $"GET-Http-LVO:ParmA ={paramA},ParmB ={paramB}";
    return context.Response.WriteAsync(requestForamat);

});

// Handler_2

app.MapPost("/LVOP",  (context) =>
{
    string paramA = context.Request.Form["ParamA"];
    string paramB = context.Request.Form["ParamB"];
    context.Response.ContentType = "text/plain";
    string requestForamat = $"POST-Http-LVO:ParmA ={paramA},ParmB ={paramB}";
    return context.Response.WriteAsync(requestForamat);

});


// Handler_3

app.MapPut("/LVOPU",  (context) =>
{
    string paramA = context.Request.Query["ParamA"];
    string paramB = context.Request.Query["ParamB"];
    context.Response.ContentType = "text/plain";
    string requestForamat = $"PUT-Http-LVO:ParmA ={paramA},ParmB ={paramB}";
    return context.Response.WriteAsync(requestForamat);

});

// Handler_4

app.MapPost("/SUM", (context) =>
{
    int x = int.Parse(context.Request.Form["X"]);
    int y = int.Parse(context.Request.Form["Y"]);
    context.Response.ContentType = "text/plain";
    string requestForamat = $"{x + y}";
    return context.Response.WriteAsync(requestForamat);

});

// Handler_5 


app.MapGet("/form1", async (context) =>
{
    var filePath = "C:\\Users\\Влад\\source\\repos\\Lab1PartA\\Lab1PartA\\html\\Task5.html";
    string htmlContent = await File.ReadAllTextAsync(filePath);
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(htmlContent);
});


app.MapPost("/multy1",  async (context) =>
{
    var form = await context.Request.ReadFormAsync();
    int x = int.Parse(context.Request.Form["X"]);
    int y = int.Parse(context.Request.Form["Y"]);
    int requestResult = x * y;
    await context.Response.WriteAsync(requestResult.ToString());
});


// Handler_6


app.MapGet("/form2", async (context) =>
{
    var filePath = "C:\\Users\\Влад\\source\\repos\\Lab1PartA\\Lab1PartA\\html\\Task6.html";
    string htmlContent = await File.ReadAllTextAsync(filePath);
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(htmlContent);
});


app.MapPost("/multy2", async (context) =>
{
    var form = await context.Request.ReadFormAsync();
    int x = int.Parse(context.Request.Form["X"]);
    int y = int.Parse(context.Request.Form["Y"]);
    int result = x * y;
    context.Response.ContentType = "text/plain";
    await context.Response.WriteAsync(result.ToString());
});






app.Run();


