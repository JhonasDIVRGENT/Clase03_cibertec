var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registrar SqlData 
builder.Services.AddSingleton<Clase03.Data.SqlData>();

// Registrar tus Data (repositorios ADO.NET)
// Si tienes más, los vas agregando igual
builder.Services.AddScoped<Clase03.Data.VendedorData>();
builder.Services.AddScoped<Clase03.Data.ProductoData>(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
