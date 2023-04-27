using DotNetEFCrud.Entitites;
using DotNetEFCrud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetEFCrud.Controllers
{
    public class CategoryController : Controller
    {
        private readonly NorthwindContext context;

        public CategoryController(NorthwindContext context)
        {
            this.context = context;
        }
        [HttpGet("kategoriler", Name = "listCategory")]
        public IActionResult Index()
        {
            //select * from categories
            List<Category> cList = this.context.Categories.ToList();
            List<Supplier> sList = this.context.Suppliers.ToList();



            var model = new CategoryIndexViewModel
            {
                Categories = cList,
                Suppliers = sList
            };

            //var model = new CategoryIndexViewModel();
            //model.Categories = cList;
            //model.Suppliers = sList;

            return View(model);
        }




        [HttpGet("kategori-guncelle/{id:int}", Name = "updateCategory")]
        public IActionResult Update(int id)
        {
            var entity = context.Categories.Find(id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new CategoryUpdateInputModel
            {
                Id = entity.CategoryId,
                Name = entity.CategoryName,
                Description = entity.Description
            };

            return View(model);
        }

        [HttpPost("kategori-guncelle/{id:int}", Name = "updateCategory")]
        [ValidateAntiForgeryToken]//Form üzerinden form bilgilerinin 3.kişiler tarafından değiştirilmesini engeller. (XSRF/CSRF)
        public IActionResult Update(CategoryUpdateInputModel uCategory)
        {
            var entity = context.Categories.Find(uCategory.Id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Description = uCategory.Description;
            entity.CategoryName = uCategory.Name;

            context.Categories.Update(entity);
            int result = context.SaveChanges();

            if (result > 0)
            {
                TempData["IsSucceded"] = true;
                TempData["Message"] = "Güncelleme İşlemi Başarılı.";
            }
            else
            {
                ViewBag.IsSucceded = false;
                ViewBag.Message = "Güncelleme İşlemi Başarısız. Tekrar Deneyiniz.";
                return View();
            }
            return RedirectToAction("Index");
            //return RedirectToAction("listCategory");
        }




        [HttpGet("kategori-sil/{id:int}", Name = "deleteCategory")]
        public IActionResult Delete(int id)
        {
            var entity = context.Categories.Find(id);
            if (entity is null)
            {
                return NotFound(); //Kayıt bulunamadı sayfasına yönlendir.
            }
            else
            {
                //Diposable Çalışır
                //using(var tra = context.Database.BeginTransaction())
                //{
                //    try
                //    {
                //        tra.Commit();
                //    }
                //    catch (Exception)
                //    {
                //        tra.Rollback();
                //        throw;
                //    }
                //}


                try
                {
                    context.Categories.Remove(entity);
                    int result = context.SaveChanges();//Adonet executeNonQuery sorgu execute kısmı, db bu yansıtır. Etkilenen kayıt sayısı döner.

                    if (result > 0)
                    {
                        //Eğer bir viewden başka bir view veri taşınacak ise bunu mvc de tempdata ile yaparız.
                        //Viewbag Viewdata ise sadece ilgili actiondan kendi view'une veri taşıyacağımız durumda kullanılır.
                        TempData["IsSucceded"] = true;
                        TempData["Message"] = "Silme İşlemi Başarılı!";
                    }
                    else
                    {
                        TempData["IsSucceded"] = false;
                        TempData["Message"] = "Silme İşlemi Başarısız. Tekrar Deneyiniz.";

                    }
                }
                catch (DbUpdateException ex)
                {
                    ViewBag.Mesaj = "Ürünü olan kategori silinemez.";
                    //Listeye geri yönlendir
                    return View();
                }



            }
            //Listeye geri yönlendir
            return RedirectToAction("Index");

        }




        [HttpGet("kategori-ekle",Name ="addCategory")]
        public IActionResult Add() {
            var model = new CategoryAddInputModel();
            
            return View(model);
        }

        [HttpPost("kategori-ekle",Name ="addCategory")]
        [ValidateAntiForgeryToken]
        public IActionResult Add(CategoryAddInputModel aCategory) 
        {
            //if (ModelState.IsValid)
            //{
            //    bool isExist = false;
            //    if (isExist)
            //    {
            //        ModelState.AddModelError("Name", "Aynı");
            //    }
            //    else
            //    {
            //        //DB işlemleri yapılır.
            //        //DBden aynı isimde kategori var mı yok mu kontrolü yapılır. Aynı iismde kategori varsa burada hata verilebilir.
            //    }
            //}

            if(ModelState.IsValid)
            {
                var newCategory = new Category
                {
                    CategoryName = aCategory.CategoryName,
                    Description = aCategory.Description,
                };
                context.Categories.Add(newCategory);
                var result = context.SaveChanges();

                if (result > 0)
                {
                    TempData["IsSucceded"] = true;
                    TempData["Message"] = "Ekleme İşlemi Başarılı.";
                }
                else
                {
                    TempData["IsSucceded"] = false;
                    TempData["Message"] = "Bir sorun meydana geldi. Lütfen daha sonra tekrar deneyiniz.";
                    return View();
                }

                return RedirectToAction("Index");
            }

            return View();
           
        }
    }
}
