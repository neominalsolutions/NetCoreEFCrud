using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreEFCrud.Entities;
using NetCoreEFCrud.Models;

namespace NetCoreEFCrud.Controllers
{
  public class CategoryController : Controller
  {
    private readonly NORTHWNDContext context;

    public CategoryController(NORTHWNDContext context)
    {
      this.context = context;
    }

    [HttpGet("kategoriler", Name ="listCategory")]

    public IActionResult Index()
    {
      //List<Category> clist2 = new List<Category>();
      //foreach (var item in this.context.Categories.ToList())
      //{
      //  if (item.CategoryName.Contains("a"))
      //  {
      //    clist2.Add(item);
      //  }
      //}

      // select * from categories
      // lambda expression
      List<Category> clist = this.context.Categories.ToList();
      List<Supplier> slist = this.context.Suppliers.ToList();
      //this.context.Categories.add

      //var ca = new Category();
      //ca.Products.ToList();

      //var p = new Product();
      //p.Category.CategoryName

      var model = new CategoryIndexViewModel
      {
        Categories = clist,
        Suppliers = slist
      };

      //var model1 = new CategoryIndexViewModel();
      //model1.Categories = clist;
      //model1.Suppliers = slist;


      return View(model);
    }

    [HttpGet("kategori-guncelle/{id:int}", Name ="updateCategory")]
    public IActionResult Update(int id)
    {
      var entity = context.Categories.Find(id);

      if(entity is null)
      {
        return NotFound(); // 404 döndür
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
    [ValidateAntiForgeryToken] // form üzerinden form bilgilerinin 3 kişiler tarafından değiştirilmesini engeller. (XSRF/CSRF)
    public IActionResult Update(CategoryUpdateInputModel model)
    {
      

      var entity = context.Categories.Find(model.Id);

      if(entity is not null)
      {
        // update işlemi yap
        entity.CategoryName = model.Name;
        entity.Description = model.Description;
        //context.Categories.Update(entity);
        int result = context.SaveChanges(); // kayıt sonucu dönerse başarılı

        if(result > 0)
        {
          TempData["IsSucceded"] = true;
          TempData["Message"] = "İşlem Başarılı";

          //return Redirect("/kategoriler");
          return RedirectToRoute("listCategory");
        }
        else
        {
          ViewBag.IsSucceded = false;
          ViewBag.Message = "İşlem Hatalı. Tekrar Deneyiniz";

          return View(); // hata mesajını kendi view üzerinde göstersin.
        }

      }
      else
      {
        return NotFound(); // 404 Sayfa bulunamadı
      }
     
    }


    [HttpGet("kategori-sil/{id:int}",Name = "deleteCategory")]
    public IActionResult Delete(int id)
    {
      var entity = context.Categories.Find(id);
      if(entity is null)
      {
        return NotFound(); // kayıt bulunamadı sayfasına yönlendir. 404 döner
      }
      else
      {

        // diposable çalışır.
        //using (var tra = context.Database.BeginTransaction())
        //{
        //  try
        //  {

        //    tra.Commit();
        //  }
        //  catch (Exception)
        //  {
        //    tra.Rollback();

        //    throw;
        //  }
        //}


        try
        {
          context.Categories.Remove(entity);
          int result = context.SaveChanges(); // adonet executeNonQuery sorgu execute kısmı, db bu yansıtır.etkilenen kayıt sayısı döner

          if (result > 0)
          {
            // eğer bir viewden başka bir view veri taşınacak ise bunu mvc de tempdata ile yaparız.
            // viewbag viewdata ise sadece ilgili actiondan kendi view'une veri taşıyacağımız durumda kullanılır. 
            TempData["Message"] = "İşlem başarılı";
            TempData["IsSucceded"] = true;
          }
          else
          {
            TempData["Message"] = "işlem başarısız. Tekrar deneyiniz";
            TempData["IsSucceded"] = false;
          }

        }
        catch (DbUpdateException ex)
        {
          ViewBag.Message = "Ürünü olan kategoriyi silemezsiniz";
          ViewBag.IsSucceded = false;
          return View();
        }

       
        // Listeye geri yönlendir
        return RedirectToAction("Index");
      }

      
    }
  }
}
