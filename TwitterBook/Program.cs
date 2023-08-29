using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TwitterBook.Data;
using TwitterBook.Installers;
using TwitterBook.MappingProfiles;
using TwitterBook.Options;

var builder = WebApplication.CreateBuilder(args);

InstallerExtensions.InstallServicesInAssembly(builder.Services, builder.Configuration);
// var config = new MapperConfiguration(cfg =>
// {
//     cfg.AddProfile(new DomainToResponseProfile());
// });
// var mapper = config.CreateMapper();
// builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

SwaggerOptions swaggerOptions = new SwaggerOptions();
builder.Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

app.UseSwagger(option => option.RouteTemplate = swaggerOptions.JsonRoute);
app.UseSwaggerUI(swaggerUiOptions =>
{
    swaggerUiOptions.DocumentTitle = "TwitterBook";
    swaggerUiOptions.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);
    swaggerUiOptions.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using var scope = app.Services.CreateScope();
var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
if (!await roleManager.RoleExistsAsync("Admin"))
{
    var adminRole = new IdentityRole("Admin");
    await roleManager.CreateAsync(adminRole);
}
if (!await roleManager.RoleExistsAsync("Poster"))
{
    var posterRole = new IdentityRole("Poster");
    await roleManager.CreateAsync(posterRole);
}
app.Run();

public partial class Program
{
}