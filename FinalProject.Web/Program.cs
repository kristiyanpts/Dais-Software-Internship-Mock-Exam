using FinalProject.Repository;
using FinalProject.Repository.Implementations.FavoriteWorkspace;
using FinalProject.Repository.Implementations.Reservation;
using FinalProject.Repository.Implementations.User;
using FinalProject.Repository.Implementations.Workspace;
using FinalProject.Repository.Interfaces.FavoriteWorkspace;
using FinalProject.Repository.Interfaces.Reservation;
using FinalProject.Repository.Interfaces.User;
using FinalProject.Repository.Interfaces.Workspace;
using FinalProject.Services.Implementations.Authentication;
using FinalProject.Services.Implementations.FavoriteWorkspace;
using FinalProject.Services.Implementations.Reservation;
using FinalProject.Services.Implementations.User;
using FinalProject.Services.Implementations.Workspace;
using FinalProject.Services.Interfaces.Authentication;
using FinalProject.Services.Interfaces.FavoriteWorkspace;
using FinalProject.Services.Interfaces.Reservation;
using FinalProject.Services.Interfaces.User;
using FinalProject.Services.Interfaces.Workspace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Connection to database
builder.Services.AddSingleton<ConnectionFactory>();

// Add repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
builder.Services.AddScoped<IFavoriteWorkspaceRepository, FavoriteWorkspaceRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

// Add services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IWorkspaceService, WorkspaceService>();
builder.Services.AddScoped<IFavoriteWorkspaceService, FavoriteWorkspaceService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
