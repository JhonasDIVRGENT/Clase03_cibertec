using Microsoft.AspNetCore.Mvc;
using Clase03.Data;
using Clase03.Models;

namespace Clase03.Controllers
{
    public class VendedorController : Controller
    {
        private readonly VendedorData _vendedorData;

        public VendedorController(VendedorData vendedorData)
        {
            _vendedorData = vendedorData;
        }

        // GET: Vendedor
        public IActionResult Index()
        {
            var vendedores = _vendedorData.Listar();
            return View(vendedores);
        }

        // GET: Vendedor/Details/5
        public IActionResult Details(int id)
        {
            var vendedor = _vendedorData.Listar().FirstOrDefault(v => v.VendedorId == id);
            if (vendedor == null) return NotFound();

            return View(vendedor);
        }

        // GET: Vendedor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vendedor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                ModelState.AddModelError("Nombre", "El nombre es obligatorio.");
                return View();
            }

            _vendedorData.Insertar(nombre);
            return RedirectToAction(nameof(Index));
        }

        // GET: Vendedor/Edit/5
        public IActionResult Edit(int id)
        {
            var vendedor = _vendedorData.Listar().FirstOrDefault(v => v.VendedorId == id);
            if (vendedor == null) return NotFound();

            return View(vendedor);
        }

        // POST: Vendedor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Vendedor vendedor)
        {
            if (id != vendedor.VendedorId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(vendedor);

            _vendedorData.Actualizar(vendedor);
            return RedirectToAction(nameof(Index));
        }

        // GET: Vendedor/Delete/5
        public IActionResult Delete(int id)
        {
            var vendedor = _vendedorData.Listar().FirstOrDefault(v => v.VendedorId == id);
            if (vendedor == null) return NotFound();

            return View(vendedor);
        }

        // POST: Vendedor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _vendedorData.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
