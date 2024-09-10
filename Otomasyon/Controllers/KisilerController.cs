using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Otomasyon.Core.Entities;
using Otomasyon.Core.Repositories;
using System.Threading.Tasks;

public class KisilerController : Controller
{
    private readonly IKisilerRepository _repository;

    public KisilerController(IKisilerRepository repository)
    {
        _repository = repository;
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var items = await _repository.ListAllAsync();
        return View(items);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Prevent CSRF attacks
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Kisiler model)
    {
        if (ModelState.IsValid)
        {
            await _repository.AddAsync(model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Prevent CSRF attacks
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(Kisiler model)
    {
        if (ModelState.IsValid)
        {
            await _repository.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    [Authorize(Roles = "Admin")]
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
    [ValidateAntiForgeryToken] // Prevent CSRF attacks
    [Authorize(Roles = "Admin")]
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

    // Add Details Action Method
    public async Task<IActionResult> Details(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }
        return View(item);
    }

    // API method to get Kisi by TcKimlikNumarasi
    [HttpGet("api/Kisiler/GetKisiByTc/{tcKimlikNumarasi}")]
    public async Task<IActionResult> GetKisiByTc(string tcKimlikNumarasi)
    {
        var kisi = await _repository.GetKisiByTcAsync(tcKimlikNumarasi);
        if (kisi == null)
        {
            return NotFound();
        }
        return Ok(kisi);
    }
}