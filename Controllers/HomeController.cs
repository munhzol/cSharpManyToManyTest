using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Products.Models;

namespace Products.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;

        public HomeController(MyContext context) {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index() {
            return RedirectToAction("Products");
        }

        [HttpGet("products")]
        public IActionResult Products() {
            ViewBag.Products = _context.Products.ToList();
            return View();
        }

        [HttpPost("products/new")]
        public IActionResult CreateProducts(Product product) {

            ViewBag.Products = _context.Products.ToList();

            if(ModelState.IsValid) {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Products");
            }

            return View("Products");
        }

        [HttpGet("products/{pid}")]
        public IActionResult ProductDetail(int pid) {
            ViewBag.pid = pid;

            Product currentProduct = _context.Products
                .Include(p => p.Categories)
                .ThenInclude(cat => cat.AddedProduct)
                .FirstOrDefault(p => p.ProductID == pid);

            ViewBag.Product = currentProduct;

            ViewBag.Categories = _context.Categories
            .ToList()
            .Where(c => !currentProduct.Categories.Any(c1 => c1.CategoryID == c.CategoryID));

            return View();
        }

        [HttpPost("/products/add/category")]
        public IActionResult AddCategoryToProduct(int productID, int categoryID) {

            Association association = new Association() {
                ProductID = productID,
                CategoryID = categoryID
            };

            _context.Associations.Add(association);
            _context.SaveChanges();

            return Redirect($"/products/{productID}");
        }
        

        
        [HttpGet("categories")]
        public IActionResult Categories() {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost("categories/new")]
        public IActionResult CreateCategories(Category category) {

            ViewBag.Categories = _context.Categories.ToList();

            if(ModelState.IsValid) {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Categories");
            }

            return View("Categories");
        }

        [HttpGet("categories/{cid}")]
        public IActionResult CategoriesDetail(int cid) {
            ViewBag.cid = cid;

            Category currentCategory = _context.Categories
                .Include(c => c.Products)
                .ThenInclude(pro => pro.AddedCategory)
                .FirstOrDefault(c => c.CategoryID == cid);

            ViewBag.Category = currentCategory;

            ViewBag.Products = _context.Products
            .ToList()
            .Where(p => !currentCategory.Products.Any(p1 => p1.ProductID == p.ProductID));

            return View();
        }

        
        [HttpPost("categories/add/product")]
        public IActionResult AddProductToCategory(int productID, int categoryID) {

            Association association = new Association() {
                ProductID = productID,
                CategoryID = categoryID
            };

            _context.Associations.Add(association);
            _context.SaveChanges();

            return Redirect($"/categories/{categoryID}");
        }

        [HttpGet("remove/{productID}/{categoryID}")]
        public IActionResult RemoveProductFromCategories(int productID, int categoryID) {

            // User RetrievedUser = dbContext.Users.SingleOrDefault(user => user.UserId == userId);
            Association association = _context.Associations
                .SingleOrDefault(a=>a.ProductID==productID && a.CategoryID == categoryID);
            _context.Associations.Remove(association);
            _context.SaveChanges();

            return Redirect($"/categories/{categoryID}");
        }

    }
}
