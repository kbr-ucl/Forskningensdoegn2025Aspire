using Microsoft.AspNetCore.Mvc;
using MvcFrontend.ApiService;

namespace MvcFrontend.Controllers;

public class ServiceBEntityController : Controller
{
    private readonly ServiceB _api;

    public ServiceBEntityController(ServiceB api)
    {
        _api = api;
    }

    // GET: ServiceBEntity
    public async Task<IActionResult> Index()
    {
        return View(await _api.GetServiceBEntities());
    }

    // GET: ServiceBEntity/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var serviceBEntityDto = await _api.GetServiceBEntity(id);

        return View(serviceBEntityDto);
    }

    // GET: ServiceBEntity/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ServiceBEntity/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description")] ServiceBEntityDto serviceBEntityDto)
    {
        if (ModelState.IsValid)
        {
            await _api.CreateServiceBEntity(serviceBEntityDto);
            return RedirectToAction(nameof(Index));
        }

        return View(serviceBEntityDto);
    }

    // GET: ServiceBEntity/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var serviceBEntityDto = await _api.GetServiceBEntity(id);

        return View(serviceBEntityDto);
    }

    // POST: ServiceBEntity/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] ServiceBEntityDto serviceBEntityDto)
    {
        if (id != serviceBEntityDto.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await _api.UpdateServiceBEntity(id, serviceBEntityDto);

            return RedirectToAction(nameof(Index));
        }

        return View(serviceBEntityDto);
    }

    // GET: ServiceBEntity/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var serviceBEntityDto = await _api.GetServiceBEntity(id);

        return View(serviceBEntityDto);
    }

    // POST: ServiceBEntity/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _api.DeleteServiceBEntity(id);

        return RedirectToAction(nameof(Index));
    }
}