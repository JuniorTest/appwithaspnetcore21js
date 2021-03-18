using SpyStore.Dal.Repos.Base;
using SpyStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpyStore.Dal.Repos.Interfaces
{
    public interface IProductRepo : IRepo<Product>
    {
        IList<Product> Search(string searchString);
        IList<Product> GetProductsForCategory(int id);
        IList<Product> GetFeaturedWithCategoryName();
        Product GetOneWithCategoryName(int id);
    }
}
