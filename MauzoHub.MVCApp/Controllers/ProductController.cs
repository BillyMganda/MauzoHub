using MauzoHub.MVCApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace MauzoHub.MVCApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProducts();
            return View(products);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _productService.GetProductById(id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateProduct(product);
                return RedirectToAction("Index");
            }

            return View(product);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteProduct(id);
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.CreateProduct(product);
                return RedirectToAction("Index");
            }

            return View(product);
        }
    }
}
