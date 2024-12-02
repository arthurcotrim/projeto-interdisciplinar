using Gerenciamento.Models;
using Gerenciamento.Repositories.Address;
using Gerenciamento.Repositories.Person;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;

namespace Gerenciamento.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPersonRepository _personRepository;
        private readonly IAddressRepository _addressRepository;
        public HomeController(IPersonRepository personRepository, IAddressRepository addressRepository)
        {
            _personRepository = personRepository;
            _addressRepository = addressRepository;
        }

        public async Task<IActionResult> Index()
        {
            var peopleList = await _personRepository.GetAllPersons();
            ViewData["peopleList"] = peopleList;
            return View();
        }

        public async Task<IActionResult> Dados(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError("", "Id inválido.");
                return View(id);
            }

            var retorno = await _addressRepository.GetAddressByPersonId(id);

            if (retorno == null)
            {
                ModelState.AddModelError("", "Nenhum usuário encontrado com o Id fornecidos.");
                return View(new DataAddressViewModel());
            }

            return View(retorno);
        }

        [HttpPost]
        public async Task<IActionResult> Dados(AddressViewModel addressView)
        {
            if (addressView == null)
            {
                ModelState.AddModelError("", "Preencha os dados novamente.");

                return View(addressView);
            }

            if (ModelState.IsValid)
            {
                var result = await _addressRepository.AddAddress(addressView);

                if (!string.IsNullOrEmpty(result))
                {
                    ModelState.AddModelError("", result);
                    return View(addressView); // Retorne a view com os erros
                }

                return RedirectToAction("Dados", new { id = addressView.PersonId });
            }

            return View(addressView);
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(PersonViewModel model)
        {
            if (ModelState.IsValid)
            {
                int newPersonId = await _personRepository.AddPerson(model);

                if (newPersonId > 0)
                {
                    return RedirectToAction("Dados", new { id = newPersonId });
                }
                else if (newPersonId == -1)
                {
                    ModelState.AddModelError("", "Usuário já cadastrado.");
                }
                else
                {
                    ModelState.AddModelError("", "Erro ao criar o registro.");
                }
            }
            return View(model);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAddress(int id, int personId)
        {
            var result = await _addressRepository.DeleteAddress(id, personId);

            if (!string.IsNullOrEmpty(result))
            {
                return BadRequest("Dados inválidos.");
            }

            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}