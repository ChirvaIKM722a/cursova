using LightingScada.Services;
using Microsoft.AspNetCore.Mvc;

namespace LightingScada.Controllers;

public class HomeController : Controller
{
    private readonly SensorDataStore _store;

    public HomeController(SensorDataStore store) => _store = store;

    public IActionResult Index() => View();

    [HttpGet]
    public IActionResult GetSensors() =>
        Json(new { connected = _store.IsConnected, sensors = _store.GetAll() });
}
