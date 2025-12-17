using System.Security.Claims;
using DoctorAppointmentSystem.Data;
using DoctorAppointmentSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointmentSystem.Controllers;

public class AppointmentController : Controller
{
    private readonly ApplicationDbContext _ctx;

    public AppointmentController(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    [Authorize]
    public async Task<IActionResult> MyAppointments()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var appointments = await _ctx.AppointmentSlots
            .Include(s => s.Doctor)
            .Where(s => s.PatientId == userId)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
        return View(appointments);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var slot = await _ctx.AppointmentSlots.FirstOrDefaultAsync(s => s.Id == id && s.PatientId == userId);

        if (slot != null)
        {
            slot.IsAvailable = true;
            slot.PatientId = null;
            slot.PatientFirstName = null;
            slot.PatientLastName = null;
            slot.PatientEmail = null;
            slot.PatientPhone = null;
            slot.PatientAddress = null;
            slot.ReservationDate = null;
            await _ctx.SaveChangesAsync();
        }

        return RedirectToAction("MyAppointments");
    }

    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var slot = await _ctx.AppointmentSlots.FirstOrDefaultAsync(s => s.Id == id && s.PatientId == userId);

        if (slot == null)
        {
            return NotFound();
        }

        return View(slot);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AppointmentSlot slot)
    {
        if (id != slot.Id)
        {
            return NotFound();
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var dbSlot = await _ctx.AppointmentSlots.FirstOrDefaultAsync(s => s.Id == id && s.PatientId == userId);

        if (dbSlot == null)
        {
            return NotFound();
        }

        // Only update editable fields
        dbSlot.PatientFirstName = slot.PatientFirstName;
        dbSlot.PatientLastName = slot.PatientLastName;
        dbSlot.PatientEmail = slot.PatientEmail;
        dbSlot.PatientPhone = slot.PatientPhone;
        dbSlot.PatientAddress = slot.PatientAddress;

        await _ctx.SaveChangesAsync();
        return RedirectToAction(nameof(MyAppointments));
    }

    public async Task<IActionResult> BookAppointment(int slotId)
    {
        var slot = await _ctx.AppointmentSlots.Include(s => s.Doctor).FirstOrDefaultAsync(s => s.Id == slotId && s.IsAvailable);
        if (slot == null) return NotFound();
        ViewBag.Slot = slot;

        var vm = new BookAppointmentViewModel { Id = slot.Id, DoctorId = slot.DoctorId };

        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
             var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
             var user = await _ctx.Users.FindAsync(userId);
             if (user != null)
             {
                 vm.PatientFirstName = user.FirstName;
                 vm.PatientLastName = user.LastName;
                 vm.PatientEmail = user.Email;
                 vm.PatientAddress = user.Address;
             }
        }

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BookAppointment(int slotId, BookAppointmentViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Slot = await _ctx.AppointmentSlots.Include(s => s.Doctor).FirstOrDefaultAsync(s => s.Id == slotId);
            return View(vm);
        }
        var slot = await _ctx.AppointmentSlots.FirstOrDefaultAsync(s => s.Id == slotId);
        if (slot == null || !slot.IsAvailable) return RedirectToAction("Index", "Home"); // Or somewhere else

        slot.PatientFirstName = vm.PatientFirstName;
        slot.PatientLastName = vm.PatientLastName;
        slot.PatientEmail = vm.PatientEmail;
        slot.PatientPhone = vm.PatientPhone;
        slot.PatientAddress = vm.PatientAddress;
        slot.IsAvailable = false;
        slot.ReservationDate = DateTime.Now;

        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            slot.PatientId = userId;
        }

        await _ctx.SaveChangesAsync();
        return RedirectToAction("BookingConfirmation", new { slotId = slot.Id });
    }

    public async Task<IActionResult> BookingConfirmation(int slotId) =>
        View(await _ctx.AppointmentSlots.Include(s => s.Doctor).FirstOrDefaultAsync(s => s.Id == slotId));
}
