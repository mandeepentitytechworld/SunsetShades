using SunsetShades.Context.ViewModel;

namespace SunsetShades.Service.Services.Interface
{
    public interface IProductService
    {
        Task<ProductsViewModel> GetTop20Products(string brand = "all", string category = "sunglases", string size = "all", int page = 1);

        Task<ProductDetailViewModel> GetProductById(int id);

        Task<ProductsViewModel> GetTop20ProductsByBrandId(string brand = "all", string category = "sunglases", string size = "all", int page = 1);

        Task<ProductsViewModel> GetTop20ProductsByCategoryId(string brand = "all", string category = "sunglases", string size = "all", int page = 1);
    }
}
