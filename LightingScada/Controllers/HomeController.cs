using LightingScada.Data;
using LightingScada.Services;
using Microsoft.AspNetCore.Mvc;

namespace LightingScada.Controllers;

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
    public IActionResult Save()
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
        _db.SaveChanges();
        return RedirectToAction("History");
    }

    // Останні 50 збережених записів
    public IActionResult History()
    {
        var samples = _db.SensorSamples
            .OrderByDescending(s => s.Id)
            .Take(50)
            .ToList();
        return View(samples);
    }
}
