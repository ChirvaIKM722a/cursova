using LightingScada.Data;
using LightingScada.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LightingScada.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly SensorDataStore _store;
    private readonly ApplicationDbContext _db;

    public HomeController(SensorDataStore store, ApplicationDbContext db)
    {
        _store = store;
        _db = db;
    }

    // Сторінка мнемосхеми
    public IActionResult Index() => View();

    // Актуальні показання у форматі JSON
    [HttpGet]
    public IActionResult GetSensors() =>
        Json(new { connected = _store.IsConnected, sensors = _store.GetAll() });

    // Зберігає поточні показання з пам'яті в базу даних
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "CanReadSavings")]
    public async Task<IActionResult> Save()
    {
        foreach (var r in _store.GetAll())
        {
            _db.SensorSamples.Add(new SensorSample
            {
                Topic = r.Topic,
                Value = r.Value,
                Unit = r.Unit,
                MeasuredAtUtc = r.MeasuredAtUtc
            });
        }
        await _db.SaveChangesAsync();
        return RedirectToAction("History");
    }

    // Останні 50 збережених записів
    public async Task<IActionResult> History()
    {
        var samples = await _db.SensorSamples
            .OrderByDescending(s => s.Id)
            .Take(50)
            .ToListAsync();
        return View(samples);
    }

    // Сторінка, відкрита для всіх відвідувачів
    [AllowAnonymous]
    public IActionResult Privacy() => View();

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        Content("Сталася помилка під час обробки запиту. Спробуйте пізніше.");
}
