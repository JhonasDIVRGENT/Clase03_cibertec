using Microsoft.AspNetCore.Mvc;
using Clase03.Data;
using Clase03.Models;
using System.Linq;

namespace Clase03.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProductoData _productoData;

        public ProductoController(ProductoData productoData)
        {
            _productoData = productoData;
        }

        // GET: Producto
        // Agregamos parámetro opcional para búsqueda
        public IActionResult Index(string terminoBusqueda = null)
        {
            var productos = string.IsNullOrWhiteSpace(terminoBusqueda)
                ? _productoData.Listar()
                : _productoData.ListarFiltrado(terminoBusqueda);

            ViewData["TerminoBusqueda"] = terminoBusqueda; // para mantener el texto en la vista
            return View(productos);
        }

        // GET: Producto/Details/5
        public IActionResult Details(int id)
        {
            var producto = _productoData.Listar().FirstOrDefault(p => p.ProductoId == id);
            if (producto == null) return NotFound();

            return View(producto);
        }

        // GET: Producto/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Producto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _productoData.Insertar(producto);
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Producto/Edit/5
        public IActionResult Edit(int id)
        {
            var producto = _productoData.Listar().FirstOrDefault(p => p.ProductoId == id);
            if (producto == null) return NotFound();

            return View(producto);
        }

        // POST: Producto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Producto producto)
        {
            if (id != producto.ProductoId) return BadRequest();

            if (ModelState.IsValid)
            {
                _productoData.Actualizar(producto);
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Producto/Delete/5
        public IActionResult Delete(int id)
        {
            var producto = _productoData.Listar().FirstOrDefault(p => p.ProductoId == id);
            if (producto == null) return NotFound();

            return View(producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _productoData.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
