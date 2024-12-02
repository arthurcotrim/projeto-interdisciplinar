using Gerenciamento.Models;
using Gerenciamento.Repositories.Address;
using Gerenciamento.Repositories.Person;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciamento.Controllers
{
    public class AddressController : Controller
    {
        private readonly IAddressRepository _addressRepository;
        public AddressController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id, int? uid)
        {
            if (id == null)
            {
                ModelState.AddModelError("", "Id inválido.");
                return View(id);
            }

            var retorno = await _addressRepository.GetAddressById(id, uid);

            if (retorno == null)
            {
                ModelState.AddModelError("", "Endereço não encontrado. Cadastre ou verifique os dados.");

                return View(new DataOneAddressViewModel());
            }

            return View(retorno);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AddressViewModel addressViewModel)
        {
            if (addressViewModel == null)
            {
                ModelState.AddModelError("", "Preencha os dados novamente.");

                return View(addressViewModel);
            }

            if (ModelState.IsValid)
            {
                var result = await _addressRepository.UpdateAddress(addressViewModel);

                if (!string.IsNullOrEmpty(result))
                {
                    ModelState.AddModelError("", result);
                    return View(addressViewModel); // Retorne a view com os erros
                }

                return RedirectToAction("Index", new { id = addressViewModel.Id , uid = addressViewModel .PersonId});
            }

            return View(addressViewModel);
        }
    }
}
