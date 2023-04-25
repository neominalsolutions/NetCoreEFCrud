

using NetCoreEFCrud.Entities;

namespace NetCoreEFCrud.Models
{
  public class CategoryIndexViewModel
  {
    public List<Category> Categories { get; set; } = new List<Category>();
    public List<Supplier> Suppliers { get; set; } = new List<Supplier>();

  }
}
