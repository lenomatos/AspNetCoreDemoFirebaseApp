using DemoFirebase.Infra.Models.Firestore;
using DemoFirebase.Infra.Repositories.Interfaces;
using DemoFirebaseApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;

namespace DemoFirebaseApp.Controllers
{
    public class EstudanteController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFirebaseService<Estudante> _firebaseService;

        public EstudanteController(ILogger<HomeController> logger, IFirebaseService<Estudante> firebaseService)
        {
            _logger = logger;
            _firebaseService = firebaseService;
        }

        public IActionResult Index()
        {
            var list = _firebaseService.GetAll();
            return View(list);
        }

        public IActionResult Details(string Id)
        {
            var model = new Estudante();
            model.Id = Id;
            model = _firebaseService.Get(model);

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Estudante model)
        {
            if (ModelState.IsValid)
            {
                _firebaseService.Add(model);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(string Id)
        {
            var model = new Estudante();
            model.Id = Id;
            model = _firebaseService.Get(model);

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Estudante model)
        {
            if (ModelState.IsValid)
            {
                //model.Livros = new List<string>();
                //model.Livros.Add("Livro A");
                //model.Livros.Add("Livro B");
                //model.Livros.Add("Livro C");
                _firebaseService.SetMerge(model);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(string Id)
        {
            var model = new Estudante();
            model.Id = Id;
            model = _firebaseService.Get(model);
            _firebaseService.Delete(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult TestSet(string Id)
        {
            var model = new Estudante();
            model.Id = Id;
            model = _firebaseService.Get(model);
            _firebaseService.Delete(model);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
