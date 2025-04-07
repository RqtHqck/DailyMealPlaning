using WebApp.Services.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Для разных сервисов можно использовать разные файлы:
builder.Services.AddScoped<XmlProductService>(provider => 
{
    var fileService = new XmlProductsFileLoader(
        provider.GetRequiredService<IWebHostEnvironment>(),
        "products.xml"
    );
    return new XmlProductService(fileService);
});

builder.Services.AddScoped<XmlCategoryService>(provider => 
{
    var fileService = new XmlProductsFileLoader(
        provider.GetRequiredService<IWebHostEnvironment>(),
        "products.xml"
    );
    return new XmlCategoryService(fileService);
});

builder.Services.AddScoped<XmlMealPlanService>(provider => 
{
    var fileService = new XmlUserFileLoader(
        provider.GetRequiredService<IWebHostEnvironment>(),
        "user.xml"
    );
    return new XmlMealPlanService(fileService);
});

// builder.Services.AddScoped<XmlUserService>(provider => 
// {
//     var fileService = new XmlFileService(
//         provider.GetRequiredService<IWebHostEnvironment>(),
//         "user.xml"
//     );
//     return new XmlUserService(fileService);
// });




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();