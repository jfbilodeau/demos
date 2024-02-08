using Microsoft.Extensions.Configuration.AzureAppConfiguration;

var builder = WebApplication.CreateBuilder(args);

var appConfigConnectionString = 
    builder.Configuration.GetConnectionString("AppConfig");

builder.Configuration.AddAzureAppConfiguration(options => {
    options.Connect(appConfigConnectionString)
        .Select("HrSystem:Web:*", LabelFilter.Null)
        .ConfigureRefresh(refresh => {
            refresh.Register("HrSystem:Web:Message", true)
                .SetCacheExpiration(TimeSpan.FromSeconds(5));
        }
    );
});

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAzureAppConfiguration();
builder.Services.Configure<Settings>(
    builder.Configuration.GetSection("HrSystem:Web")
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseAzureAppConfiguration();

app.MapRazorPages();

app.Run();
