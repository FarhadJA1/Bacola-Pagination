using Bacola_MVC_Updated_.Data;
using Bacola_MVC_Updated_.Models;
using Bacola_MVC_Updated_.ViewModels.Admin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bacola_MVC_Updated_.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class IcecreamController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly AppDbContext _context;
        public IcecreamController(AppDbContext context, IWebHostEnvironment environment) { 
        
            _context = context;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            List<Icecream> icecreams = await _context.Icecreams.ToListAsync();
            return View(icecreams);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Icecream icecream)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!icecream.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Photo not found");
                return View();
            }
            if (icecream.Photo.Length / 1024 > 350)
            {
                ModelState.AddModelError("Photo", "File size is invalid");
                return View();
            }


            string fileName = Guid.NewGuid().ToString() + "_" + icecream.Photo.FileName;
            string path = Path.Combine(_environment.WebRootPath, "assets/img", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await icecream.Photo.CopyToAsync(stream);
            }
            icecream.Image = fileName;
            
            await _context.Icecreams.AddAsync(icecream);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dbicecream = await _context.Icecreams.FindAsync(id);
            if (dbicecream == null) return NotFound();
            string path = Path.Combine(_environment.WebRootPath, "assets/img", dbicecream.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _context.Icecreams.Remove(dbicecream);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            Icecream icecream = await _context.Icecreams.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (icecream == null) return NotFound();
            return View(icecream);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Icecream icecream)
        {
            if (!icecream.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Photo not found");
                return View();
            }
            if (icecream.Photo.Length / 1024 > 350)
            {
                ModelState.AddModelError("Photo", "File size is invalid");
                return View();
            }

            if (id != icecream.Id) return BadRequest();
            Icecream dbicecream = await _context.Icecreams.Where(m => m.Id == icecream.Id).FirstOrDefaultAsync();

            string fileName = Guid.NewGuid().ToString() + "_" + icecream.Photo.FileName;
            string path = Path.Combine(_environment.WebRootPath, "assets/img", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await icecream.Photo.CopyToAsync(stream);
            }

            dbicecream.Image = fileName;
            dbicecream.Description = icecream.Description;
            dbicecream.DiscountTitle = icecream.DiscountTitle;
            dbicecream.MainTitle = icecream.MainTitle;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int id)
        {
            Icecream icecream = await _context.Icecreams.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (icecream == null) return NotFound();
            return View(icecream);
        }


    }
}
