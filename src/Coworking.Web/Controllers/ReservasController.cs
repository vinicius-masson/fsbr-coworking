using Coworking.Aplication.Queries.Reservas.GetReserva;
using Coworking.Common.Response;
using Coworking.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Coworking.Web.Controllers
{
    public class ReservasController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ReservasController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CoworkingAPI");
                var response = await client.GetAsync("reservas");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseWithData<List<GetReservaResponse>>>();

                    var reservas = apiResponse?.Data?.Select(r => new ReservaViewModel
                    {
                        Id = r.Id,
                        DataInicio = r.DataInicioReserva,
                        DataFim = r.DataFimReserva,
                        UsuarioNome = r.UsuarioNome,
                        SalaDescricao = r.SalaDescricao
                    }).ToList();

                    return View(reservas);
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Falha na comunicação com a API: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro inesperado: {ex.Message}");
            }

            return View(new List<ReservaViewModel>());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Salas = GetSalasMock();
            ViewBag.Usuarios = GetUsuariosMock();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReservaViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Salas = GetSalasMock();
                    ViewBag.Usuarios = GetUsuariosMock();
                    return View(model);
                }

                var client = _httpClientFactory.CreateClient("CoworkingAPI");
                var response = await client.PostAsJsonAsync("reservas", model);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                ModelState.AddModelError(string.Empty, "Erro ao criar reserva");
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Falha na comunicação com a API: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro inesperado: {ex.Message}");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CoworkingAPI");
            var response = await client.GetAsync($"reservas/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseWithData<GetReservaResponse>>();

            var model = new UpdateReservaViewModel
            {
                Id = apiResponse.Data.Id,
                SalaId = apiResponse.Data.SalaId,
                UsuarioId = apiResponse.Data.UsuarioId,
                DataInicioReserva = apiResponse.Data.DataInicioReserva,
                DataFimReserva = apiResponse.Data.DataFimReserva,
                Salas = GetSalasMock(),
                Usuarios = GetUsuariosMock()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateReservaViewModel model)
        {
            try
            {
                ModelState.Remove("Salas");
                ModelState.Remove("Usuarios");

                if (!ModelState.IsValid)
                {
                    model.Salas = GetSalasMock();
                    model.Usuarios = GetUsuariosMock();
                    return View(model);
                }

                var client = _httpClientFactory.CreateClient("CoworkingAPI");
                var response = await client.PutAsJsonAsync($"reservas/{model.Id}", model);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                ModelState.AddModelError(string.Empty, "Erro ao atualizar reserva");
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Falha na comunicação com a API: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro inesperado: {ex.Message}");
            }

            model.Salas = GetSalasMock();
            model.Usuarios = GetUsuariosMock();
            return View(model);
        }

        [HttpGet]
        public IActionResult Cancel(Guid id)
        {
            return View(new CancelReservaViewModel { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(CancelReservaViewModel model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CoworkingAPI");

                var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                var response = await client.PatchAsync($"reservas/{model.Id}/cancel", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                var errorContent = await response.Content.ReadAsStringAsync();

                try
                {
                    var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(errorContent);
                    ModelState.AddModelError(string.Empty, problemDetails?.Detail ?? "Erro ao cancelar reserva");
                }
                catch (JsonException)
                {
                    ModelState.AddModelError(string.Empty, errorContent);
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Falha na comunicação com a API: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro inesperado: {ex.Message}");
            }

            return View(model);
        }

        private List<SelectListItem> GetSalasMock()
        {
            return new List<SelectListItem>
        {
            new SelectListItem { Value = "CB8A5465-BA06-48D4-865F-51D0B818F8CB", Text = "Auditório 04" },
            new SelectListItem { Value = "2DC25723-8378-4FA7-A9FA-6F592ADAEDAC", Text = "Auditório 06" },
            new SelectListItem { Value = "B0DC1ED8-E048-4482-B5CF-B8B192DB6CDF", Text = "Sala de Reunião 02" },
            new SelectListItem { Value = "1CE6A2E0-247B-421B-9F9D-BF8BEA59283C", Text = "Sala 8" }
        };
        }

        private List<SelectListItem> GetUsuariosMock()
        {
            return new List<SelectListItem>
        {
            new SelectListItem { Value = "7D3E7C13-FD8D-4268-A154-5FDFD9BA8E66", Text = "João da Silva" },
            new SelectListItem { Value = "A88F9E37-C4D3-4C07-9BFC-90B211705B65", Text = "Vinicius de Carvalho Masson" }
        };
        }
    }
}
