using DoctorAppointmentSystem.Data;
using DoctorAppointmentSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointmentSystem.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _ctx;
    public HomeController(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<IActionResult> Index() =>
        View(await _ctx.Doctors.Select(d => d.Specialization).Distinct().OrderBy(s => s).ToListAsync());

    public async Task<IActionResult> Doctors(string specialization, string search)
    {
        var q = _ctx.Doctors.AsQueryable();
        if (!string.IsNullOrEmpty(specialization)) q = q.Where(d => d.Specialization == specialization);
        if (!string.IsNullOrEmpty(search))
            q = q.Where(d => d.FirstName.Contains(search) || d.LastName.Contains(search) || d.Specialization.Contains(search));
        ViewBag.Specialization = specialization;
        return View(await q.ToListAsync());
    }

    public async Task<IActionResult> AvailableAppointments(int doctorId)
    {
        ViewBag.Doctor = await _ctx.Doctors.FindAsync(doctorId);
        return View(await _ctx.AppointmentSlots
            .Where(s => s.DoctorId == doctorId && s.IsAvailable && s.StartTime > DateTime.Now)
            .OrderBy(s => s.StartTime)
            .ToListAsync());
    }
}