using Gerenciamento.Models;
using Gerenciamento.Repositories.Person;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciamento.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonRepository _personRepository;

        public PersonController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError("", "Nenhum usuário encontrado com o Id fornecido.");

                return View(new PersonViewModel()); // Retorne a view com um novo modelo
            }

            var person = await _personRepository.GetPersonById(id);

            if (person == null)
            {
                ModelState.AddModelError("", "Nenhum usuário encontrado com o Id fornecido.");
                return View(new PersonViewModel()); // Retorne a view com um novo modelo
            }

            return View(person);
        }

        [HttpPost]
        public async Task<IActionResult> Index(PersonViewModel personViewModel)
        {
            if (personViewModel == null)
            {
                ModelState.AddModelError("", "Preencha os dados novamente.");
                
                return View(personViewModel);
            }

            if (ModelState.IsValid)
            {
                var result = await _personRepository.UpdatePerson(personViewModel);

                if (!string.IsNullOrEmpty(result))
                {
                    ModelState.AddModelError("", result);
                    return View(personViewModel); // Retorne a view com os erros
                }

                return RedirectToAction("Index", new { id = personViewModel.Id });
            }

            return View(personViewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var result = await _personRepository.DeletePerson(id);

            if (!string.IsNullOrEmpty(result))
            {
                return BadRequest("Dados inválidos.");
            }

            return Ok();
        }
    }
}
