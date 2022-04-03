using Bacola_MVC_Updated_.Data;
using Bacola_MVC_Updated_.Exceptions;
using Bacola_MVC_Updated_.Models;
using Bacola_MVC_Updated_.Utilities.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bacola_MVC_Updated_.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            var categories = await _context.Categories
                .Skip((page - 1) * take)
                .Take(take)
                .AsNoTracking()
                .OrderByDescending(m => m.Id)
                .ToListAsync();
            int count = await GetPageCount(take);

            Paginate<Categories> result = new Paginate<Categories>(categories, page, count);

            return View(result);
        }
        private async Task<int> GetPageCount(int take)
        {
            var count = await _context.Categories.CountAsync();
            return (int)Math.Ceiling((decimal)count / take);
        }
        public IActionResult Detail(int id)
        {
            var category = _context.Categories.FirstOrDefault(m => m.Id == id);
            return Json(new
            {
                categoryName = category.Name,
                action = "Detail",
                Id = id
            });
        }
        public async Task<IActionResult> Edit(int id)
        {
            Categories category = await _context.Categories.Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Categories category)
        {
            
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (id != category.Id) return BadRequest();
            Categories dbCategory =await _context.Categories.AsNoTracking().Where(m=>!m.IsDeleted&&m.Id==category.Id).FirstOrDefaultAsync();
            if (dbCategory.Name.ToLower().Trim() == category.Name.ToLower().Trim())
            {
                return RedirectToAction(nameof(Index));
            }
            /*throw new CustomException("Error");*/
            bool isExist = _context.Categories.Any(m => m.Name.Trim().ToLower() == category.Name.Trim().ToLower());
            if (isExist)
            {
                ModelState.AddModelError("Name", "This category already exists");
                return View();
            }
            /*dbCategory.Name = category.Name;*/
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Categories dbCategory = await _context.Categories.Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();
            if (dbCategory == null) return NotFound();
            /*_context.Categories.Remove(dbCategory);*/
            dbCategory.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categories category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool isExist = _context.Categories.Any(m => m.Name.Trim().ToLower() == category.Name.Trim().ToLower());
            if (isExist)
            {
                ModelState.AddModelError("Name", "This category already exists");
                return View();
            }

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
