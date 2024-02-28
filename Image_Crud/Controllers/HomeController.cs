using Humanizer;
using Image_Crud.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Image_Crud.Controllers
{
	public class HomeController : Controller
	{
		private readonly IMGContext context;
		private readonly IWebHostEnvironment web;

		public HomeController(IMGContext Context , IWebHostEnvironment web)
        {
			context = Context;
			this.web = web;
		}

        public IActionResult Index()
		{
			var show = context.Products.ToList();
			return View(show);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Images images)
		{
			if (ModelState.IsValid)
			{
				string Folder = Path.Combine(web.WebRootPath, "Images");
				string File = images.Image.FileName;

				string FilePath = Path.Combine(Folder, File);

				images.Image.CopyTo(new FileStream(FilePath, FileMode.Create));

				Product pro = new Product()
				{
					Name = images.Name,
					Quantity = images.Quantity,
					Image = File
				};

				context.Products.Add(pro);
				context.SaveChanges();
				return RedirectToAction("Index");
			}
            else
            {
                ViewBag.message = "File Is Not Inserted";
            }
            return View();

		}

		public IActionResult Edit(int id)
		{
			var show = context.Products.Find(id);
			return View(show);
		}
        [HttpPost]
        public IActionResult Edit(int id, Images images)
        {
            var show = context.Products.Find(id);

            if (show != null)
            {
                show.Name = images.Name;
                show.Quantity = images.Quantity;

                if (images.Image != null)
                {
                    string Folder = Path.Combine(web.WebRootPath, "Images");
                    string File = images.Image.FileName;
                    string FilePath = Path.Combine(Folder, File);

                    // If the new file path is different from the old one, delete the old file
                    if (FilePath != show.Image)
                    {
                        string oldFilePath = Path.Combine(web.WebRootPath, "Images", show.Image);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Using statement ensures FileStream is properly disposed
                    using (var stream = new FileStream(FilePath, FileMode.Create))
                    {
                        images.Image.CopyTo(stream);
                    }

                    show.Image = File;
                }

                context.Products.Update(show);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Details(int id)
		{
			var show = context.Products.Find(id);
			return View(show);
		}

		public IActionResult Delete (int id)
		{
            var show = context.Products.Find(id);
            return View(show);
        }
		[HttpPost]
		public IActionResult Delete (int id,Product product)
		{
            context.Products.Remove(product);
			context.SaveChanges();
			return RedirectToAction("Index");
        }
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}