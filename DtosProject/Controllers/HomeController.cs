using System.Diagnostics;
using DtosProject.Models;
using DtosProject.DTOs;
using DtosProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace DtosProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ClientService _clientService;

        public HomeController(ILogger<HomeController> logger, ClientService clientService)
        {
            _logger = logger;
            _clientService = clientService;
        }

        public async Task<IActionResult> Index(string? searchTerm, int? minAge, int? maxAge)
        {
            var searchDto = new SearchClientsDTO
            {
                SearchTerm = searchTerm,
                MinAge = minAge,
                MaxAge = maxAge
            };

            var clients = await _clientService.SearchClientsAsync(searchDto);

            ViewBag.SearchTerm = searchTerm;
            ViewBag.MinAge = minAge;
            ViewBag.MaxAge = maxAge;

            return View(clients);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateClientDTO clientDto)
        {
            if (ModelState.IsValid)
            {
                await _clientService.AddClientAsync(clientDto);
                return RedirectToAction(nameof(Index));
            }
            return View(clientDto);
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateClientDTO
            {
                Id = client.Id,
                FullName = client.FullName,
                Email = client.Email,
                Age = client.Age
            };

            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateClientDTO clientDto)
        {
            if (id != clientDto.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var result = await _clientService.UpdateClientAsync(clientDto);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            return View(clientDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clientService.DeleteClientAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}