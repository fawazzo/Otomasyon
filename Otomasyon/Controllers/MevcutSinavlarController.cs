using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Otomasyon.Core.Entities;
using Otomasyon.Infrastructure.Data;

[Authorize(Roles = "Admin")]
public class MevcutSinavlarController : Controller
{
    private readonly ApplicationDbContext _context;

    public MevcutSinavlarController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: MevcutSinavlar
    public async Task<IActionResult> Index()
    {
        return View(await _context.MevcutSinavlar.ToListAsync());
    }

    // GET: MevcutSinavlar/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var mevcutSinavlar = await _context.MevcutSinavlar
            .FirstOrDefaultAsync(m => m.Id == id);
        if (mevcutSinavlar == null)
        {
            return NotFound();
        }

        return View(mevcutSinavlar);
    }

    // GET: MevcutSinavlar/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: MevcutSinavlar/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MevcutSinavlar mevcutSinavlar)
    {
        if (ModelState.IsValid)
        {
            _context.Add(mevcutSinavlar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(mevcutSinavlar);
    }

    // GET: MevcutSinavlar/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var mevcutSinavlar = await _context.MevcutSinavlar.FindAsync(id);
        if (mevcutSinavlar == null)
        {
            return NotFound();
        }
        return View(mevcutSinavlar);
    }

    // POST: MevcutSinavlar/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MevcutSinavlar mevcutSinavlar)
    {
        if (id != mevcutSinavlar.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(mevcutSinavlar);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MevcutSinavlarExists(mevcutSinavlar.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(mevcutSinavlar);
    }

    // GET: MevcutSinavlar/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var mevcutSinavlar = await _context.MevcutSinavlar
            .FirstOrDefaultAsync(m => m.Id == id);
        if (mevcutSinavlar == null)
        {
            return NotFound();
        }

        return View(mevcutSinavlar);
    }

    // POST: MevcutSinavlar/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var mevcutSinavlar = await _context.MevcutSinavlar.FindAsync(id);
        _context.MevcutSinavlar.Remove(mevcutSinavlar);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MevcutSinavlarExists(int id)
    {
        return _context.MevcutSinavlar.Any(e => e.Id == id);
    }
}
