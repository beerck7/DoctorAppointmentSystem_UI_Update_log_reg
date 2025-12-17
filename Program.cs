using DoctorAppointmentSystem.Data;
using DoctorAppointmentSystem.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

var cultureInfo = new CultureInfo("pl-PL");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultureInfo),
    SupportedCultures = new List<CultureInfo> { cultureInfo },
    SupportedUICultures = new List<CultureInfo> { cultureInfo }
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var ctx = services.GetRequiredService<ApplicationDbContext>();

        ctx.Database.Migrate();

        if (!ctx.Doctors.Any())
        {
            ctx.Doctors.AddRange(new List<Doctor>
            {
                new() { FirstName = "Jan", LastName = "Kowalski", Specialization = "Kardiolog", Description = "Specjalista chorób serca, EKG, Holter." },
                new() { FirstName = "Anna", LastName = "Nowak", Specialization = "Pediatra", Description = "Leczenie dzieci, bilanse, szczepienia." },
                new() { FirstName = "Piotr", LastName = "Wiśniewski", Specialization = "Dermatolog", Description = "Diagnostyka znamion, trądzik, choroby skóry." },
                new() { FirstName = "Maria", LastName = "Lewandowska", Specialization = "Stomatolog", Description = "Leczenie zachowawcze, protetyka, estetyka." },
                new() { FirstName = "Tomasz", LastName = "Zieliński", Specialization = "Okulista", Description = "Badanie wzroku, dobór okularów, jaskra." }
            });
            ctx.SaveChanges();
        }

        // Check if there are any future appointments
        if (!ctx.AppointmentSlots.Any(s => s.StartTime > DateTime.Now))
        {
            // Optional: Clean up old past appointments to keep DB tidy
            var oldSlots = ctx.AppointmentSlots.Where(s => s.StartTime < DateTime.Now).ToList();
            if (oldSlots.Any())
            {
                ctx.AppointmentSlots.RemoveRange(oldSlots);
                ctx.SaveChanges();
            }

            var doctors = ctx.Doctors.ToList();
            var slots = new List<AppointmentSlot>();

            var startDate = DateTime.Today.AddDays(1);

            for (int i = 0; i < 14; i++)
            {
                var currentDate = startDate.AddDays(i);

                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    continue;
                }

                foreach (var doc in doctors)
                {
                    for (int hour = 9; hour <= 16; hour++)
                    {
                        slots.Add(new AppointmentSlot
                        {
                            DoctorId = doc.Id,
                            StartTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, hour, 0, 0),
                            DurationMinutes = 30,
                            IsAvailable = true
                        });
                    }
                }
            }

            ctx.AppointmentSlots.AddRange(slots);
            ctx.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Wystąpił błąd podczas inicjalizacji bazy danych.");
    }
}

app.Run();