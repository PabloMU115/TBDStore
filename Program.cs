using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
//using PaypalServerSdk.Standard;
//using PaypalServerSdk.Standard.Authentication;
using TBD.Data;
using TBD.Models;
using TBD.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("LocalConnection")
    ?? throw new InvalidOperationException("Connection string 'LocalConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Se a�ade el servicio de los roles
builder.Services.AddDefaultIdentity<Usuario>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//// Se a�ade el servicio que env�a correos
//builder.Services.AddTransient<IEmailSender, EmailSender>();

////Servicio de PayPal
//builder.Services.AddSingleton<PaypalServerSdkClient>(sp =>
//{
//    return new PaypalServerSdkClient.Builder()
//        .ClientCredentialsAuth(
//            new ClientCredentialsAuthModel.Builder(
//                "OAuthClientId",
//                "OAuthClientSecret"
//            )
//            .Build())
//        .Environment(PaypalServerSdk.Standard.Environment.Sandbox)
//        .LoggingConfig(config => config
//            .LogLevel(LogLevel.Information)
//            .RequestConfig(reqConfig => reqConfig.Body(true))
//            .ResponseConfig(respConfig => respConfig.Headers(true))
//        )
//        .Build();
//});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Tienda}/{id?}");
app.MapRazorPages();

app.MapGet("/", context =>
{
    context.Response.Redirect("/Tienda");
    return Task.CompletedTask;
});

//Esta porcion verifica que siempre existan categorias
//disponibles para los productos en la base de datos.
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

    var roles = new[] { "admin", "usuario" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

//Esta porcion verifica que siempre existan roles
//disponibles para los usuarios en la base de datos.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

    var categorias = new[] { "Electrónica", "Ropa", "Hogar", "Juguetes" };

    foreach (var nombre in categorias)
    {
        if (!context.Categorias.Any(c => c.NombreCategoria == nombre))
        {
            var ranges = new List<(int Min, int Max)>
            {
                (65, 90),   // A-Z
                (97, 122)   // a-z
            };

            var random = new Random();
            var selectedRange = ranges[random.Next(ranges.Count)];
            int min = selectedRange.Min;
            int max = selectedRange.Max;

            int codigo = random.Next(min, max + 1);
            char caracter = (char)codigo;

            string id = "C" + random.Next(10, 100) + caracter;

            context.Categorias.Add(new Categoria { IdCategoria = id , NombreCategoria = nombre });
        }
    }

    await context.SaveChangesAsync();
}


//Esta porcion se asegura de que, en caso de que no se encuentre ningun
//usuario registrado en la base de datos, se agregue al menos uno predefinido
//con el rol de admin ya asignado.
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetService<UserManager<Usuario>>();

    string email = "admin@admin.com";
    //Asegurarse de seguir la convencion para la creacion de una contrase�a valida
    //(Una letra mayuscula, letras, numeros y un caracter especial)
    string password = "Test1234.";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new Usuario
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            NombreCompleto = "Admin adminoso",
            Estado = 1,
            FechaCreacion = DateTime.Now.Date.ToString("yyyy-MM-dd"),
            FechaEliminacion = ""
        };

        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, "ADMIN");
    }
}

app.Run();
