using Microsoft.AspNetCore.Mvc;
using MvcFrontend.ApiService;

namespace MvcFrontend.Controllers;

public class ServiceAEntityController : Controller
{
    private readonly ServiceA _api;

    public ServiceAEntityController(ServiceA api)
    {
        _api = api;
    }

    // GET: ServiceAEntity
    public async Task<IActionResult> Index()
    {
        return View(await _api.GetServiceAEntities());
    }

    // GET: ServiceAEntity/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var serviceAEntityDto = await _api.GetServiceAEntity(id);

        return View(serviceAEntityDto);
    }

    // GET: ServiceAEntity/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ServiceAEntity/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description")] ServiceAEntityDto serviceAEntityDto)
    {
        if (ModelState.IsValid)
        {
            await _api.CreateServiceAEntity(serviceAEntityDto);
            return RedirectToAction(nameof(Index));
        }

        return View(serviceAEntityDto);
    }

    // GET: ServiceAEntity/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var serviceAEntityDto = await _api.GetServiceAEntity(id);

        return View(serviceAEntityDto);
    }

    // POST: ServiceAEntity/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] ServiceAEntityDto serviceAEntityDto)
    {
        if (id != serviceAEntityDto.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await _api.UpdateServiceAEntity(id, serviceAEntityDto);

            return RedirectToAction(nameof(Index));
        }

        return View(serviceAEntityDto);
    }

    // GET: ServiceAEntity/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var serviceAEntityDto = await _api.GetServiceAEntity(id);

        return View(serviceAEntityDto);
    }

    // POST: ServiceAEntity/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _api.DeleteServiceAEntity(id);

        return RedirectToAction(nameof(Index));
    }
}