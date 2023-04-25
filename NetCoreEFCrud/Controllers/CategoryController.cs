using Microsoft.AspNetCore.Mvc;
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
        context.Categories.Remove(entity);
        int result = context.SaveChanges(); // adonet executeNonQuery sorgu execute kısmı, db bu yansıtır.etkilenen kayıt sayısı döner

        if(result > 0)
        {
          // eğer bir viewden başka bir view veri taşınacak ise bunu mvc de tempdata ile yaparız.
          // viewbag viewdata ise sadece ilgili actiondan kendi view'une veri taşıyacağımız durumda kullanılır. 
          TempData["Mesaj"] = "İşlem başarılı";
        }
        else
        {
          TempData["Mesaj"] = "işlem başarısız. Tekrar deneyiniz";
        }

        // Listeye geri yönlendir
        return RedirectToAction("Index");
      }

      
    }
  }
}
