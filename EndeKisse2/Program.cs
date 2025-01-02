using EndeKisse2.Data;
using EndeKisse2.Services;
using EndeKissie2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient < IEmailSender, EmailService>();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
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
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

//var key = Environment.GetEnvironmentVariable("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImJlZnJhcmRpZG9idnF4c2txcHBnIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzU2Njg3MDAsImV4cCI6MjA1MTI0NDcwMH0.bkBOMmX8sNSkB6-uOGO54H6HQuO0irfGnw-2r4sy9zk");

//var options = new Supabase.SupabaseOptions
//{
//    AutoConnectRealtime = true
//};

//try
//{
//    var supabase = new Supabase.Client("https://befrardidobvqxskqppg.supabase.co", key, options);
//    await supabase.InitializeAsync();
//}
//catch(Exception ex)
//{
//    throw;
//}


app.Run();
