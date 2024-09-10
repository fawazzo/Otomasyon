using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Otomasyon.Core.Entities;
using Otomasyon.Core.Repositories;
using Otomasyon.Services; // Ensure this namespace is included
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

public class BasvuruFormuController : Controller
{
    private readonly IBasvuruFormuRepository _repository;
    private readonly IMevcutSinavlarRepository _sinavRepository;
    private readonly IRoleService _roleService;
    private readonly IUserService _userService;

    public BasvuruFormuController(IBasvuruFormuRepository repository, IMevcutSinavlarRepository sinavRepository, IRoleService roleService, IUserService userService)
    {
        _repository = repository;
        _sinavRepository = sinavRepository;
        _roleService = roleService;
        _userService = userService;
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var items = await _repository.ListAllAsync();
        return View(items);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        // Get the logged-in user's TcKimlikNumarasi
        var tcKimlikNumarasi = User.Claims.FirstOrDefault(c => c.Type == "TcKimlikNumarasi")?.Value;

        if (string.IsNullOrEmpty(tcKimlikNumarasi))
        {
            // Handle the case where TcKimlikNumarasi is not available
            return RedirectToAction("Index", "Home"); // Or some other appropriate action
        }

        // Fetch user details based on TcKimlikNumarasi
        var userDetails = await _userService.GetUserDetailsByTcKimlikNumarasiAsync(tcKimlikNumarasi);

        if (userDetails == null)
        {
            // Handle the case where user details could not be found
            return RedirectToAction("Index", "Home"); // Or some other appropriate action
        }

        // Initialize available exams and roles
        var availableExams = await GetAvailableExams();
        var availableRolesWithCapacity = await _roleService.GetAvailableRolesWithCapacityAsync();

        ViewBag.AvailableSinavlar = new SelectList(availableExams, "Value", "Text");

        var filteredRoles = availableRolesWithCapacity
            .Where(role => role.Value > 0)
            .Select(role => new SelectListItem { Value = role.Key, Text = role.Key })
            .ToList();
        ViewBag.AvailableRoles = new SelectList(filteredRoles, "Value", "Text");

        // Create a new BasvuruFormu instance and set user details
        var model = new BasvuruFormu
        {
            TcKimlikNumarasi = tcKimlikNumarasi,
            Adi = userDetails.Adi,
            Soyadi = userDetails.Soyadi,
            Unvan = userDetails.Unvan
        };

        return View(model);
    }



    [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(BasvuruFormu model)
{
    if (ModelState.IsValid)
    {
        // Set the Degerlendirme field to the default value
        model.Degerlendirme = "Değerlendirme aşamasında";

        // Add the new BasvuruFormu to the database
        await _repository.AddAsync(model);

        // Redirect to the Details page for the newly created BasvuruFormu
        return RedirectToAction(nameof(Details), new { id = model.Id });
    }

    // If model state is not valid, repopulate the exams list and roles and return the view with the model
    var availableExams = await GetAvailableExams();
    var availableRolesWithCapacity = await _roleService.GetAvailableRolesWithCapacityAsync();

    // Filter roles with capacity greater than 0
    var filteredRoles = availableRolesWithCapacity
        .Where(role => role.Value > 0)
        .Select(role => new SelectListItem { Value = role.Key, Text = role.Key })
        .ToList();

    ViewBag.AvailableSinavlar = new SelectList(availableExams, "Value", "Text", model.MevcutSinavId);
    ViewBag.AvailableRoles = new SelectList(filteredRoles, "Value", "Text", model.Gorev);

    return View(model);
}


    public async Task<IActionResult> Edit(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        var availableExams = await GetAvailableExams();
        ViewBag.AvailableSinavlar = new SelectList(availableExams, "Value", "Text", item.MevcutSinavId);
        var availableRolesWithCapacity = await _roleService.GetAvailableRolesWithCapacityAsync();
        var filteredRoles = availableRolesWithCapacity
            .Where(role => role.Value > 0)
            .Select(role => new SelectListItem { Value = role.Key, Text = role.Key })
            .ToList();
        ViewBag.AvailableRoles = new SelectList(filteredRoles, "Value", "Text", item.Gorev);

        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(BasvuruFormu model)
    {
        if (ModelState.IsValid)
        {
            // Check if Degerlendirme is null or empty, and if so, set it to "Değerlendirme aşamasında"
            if (string.IsNullOrWhiteSpace(model.Degerlendirme))
            {
                model.Degerlendirme = "Değerlendirme aşamasında";
            }

            // Update the existing BasvuruFormu in the database
            await _repository.UpdateAsync(model);

            // Redirect to the Details page for the updated BasvuruFormu
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }

        // If model state is not valid, repopulate the exams list and roles and return the view with the model
        var availableExams = await GetAvailableExams();
        var availableRolesWithCapacity = await _roleService.GetAvailableRolesWithCapacityAsync();
        var filteredRoles = availableRolesWithCapacity
            .Where(role => role.Value > 0)
            .Select(role => new SelectListItem { Value = role.Key, Text = role.Key })
            .ToList();

        ViewBag.AvailableSinavlar = new SelectList(availableExams, "Value", "Text", model.MevcutSinavId);
        ViewBag.AvailableRoles = new SelectList(filteredRoles, "Value", "Text", model.Gorev);

        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }
        return View(item);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }
        await _repository.DeleteAsync(item);
        return RedirectToAction(nameof(Index));
    }

    private async Task<IEnumerable<SelectListItem>> GetAvailableExams()
    {
        var exams = await _sinavRepository.ListAllAsync();
        var availableExams = exams
            .Where(e => e.dateStart <= DateTime.Now && e.dateEnd >= DateTime.Now)
            .Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.SinavAdi
            }).ToList();
        return availableExams;
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    [Authorize]
    public async Task<IActionResult> Durum()
    {
        var tcKimlikNumarasi = User.Claims.FirstOrDefault(c => c.Type == "TcKimlikNumarasi")?.Value;

        if (tcKimlikNumarasi != null)
        {
            var basvuru = await _repository.GetByTcKimlikNumarasiAsync(tcKimlikNumarasi);
            if (basvuru != null)
            {
                return View(basvuru);
            }
        }

        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateDegerlendirme(int id, string Degerlendirme)
    {
        // Retrieve the BasvuruFormu item by ID
        var basvuru = await _repository.GetByIdAsync(id);
        if (basvuru == null)
        {
            return NotFound();
        }

        // Update the Degerlendirme field
        basvuru.Degerlendirme = Degerlendirme;

        // Save the changes to the database
        await _repository.UpdateAsync(basvuru);

        // Redirect back to the evaluation status page or another relevant page
        return RedirectToAction(nameof(Durum));
    }
}
