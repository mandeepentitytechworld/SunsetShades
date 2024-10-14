using Microsoft.EntityFrameworkCore;
using SunsetShades.Context;
using SunsetShades.Context.Model;
using SunsetShades.Context.ViewModel;
using SunsetShades.Service.Services.Interface;

namespace SunsetShades.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly EFDbContext eFDbContext;

        public ProductService(EFDbContext eFDbContext)
        {
            this.eFDbContext = eFDbContext;
        }

        public async Task<ProductsViewModel> GetTop20Products(string brand = "all", string category = "sunglases", string size = "all", int page = 1)
        {
            var result = new ProductsViewModel();
            result.ResponseMessage = new ResponseMessage();
            result.Products = new List<ProductViewModel>();
            result.Brands = new List<BrandViewModel>();
            result.Categories = new List<CategoryViewModel>();
            int skip = 0;
            int take = 20;
            skip = (page - 1) * take;

            try
            {
                if (Connection.Open())
                {
                    var dbProducts = await eFDbContext.Product.Select(c => c)
                        .Join(eFDbContext.Brands,
                        product => product.BrandId, brand => brand.Id,
                        (product, brand) => new
                        {
                            Id = product.Id,
                            ProductName = product.ProductName,
                            Price = product.Price,
                            Size = product.Size,
                            Image = product.Image,
                            BrandId = product.BrandId,
                            BrandName = brand.BrandName,
                            CategoryId = product.CategoryId,
                            FriendlyName = product.FriendlyName,
                            Description = product.Description,
                            FrameColor = product.FrameColor,
                            GlassColor = product.GlassColor,
                            Material = product.Material,
                        })
                        .Join(eFDbContext.Categories,
                        product => product.CategoryId, category => category.Id,
                        (product, category) => new
                        {
                            CategoryId = category.Id,
                            CategoryName = category.Name,
                            Id = product.Id,
                            ProductName = product.ProductName,
                            Price = product.Price,
                            Size = product.Size,
                            Image = product.Image,
                            BrandId = product.BrandId,
                            BrandName = product.BrandName,
                            FriendlyName = product.FriendlyName,
                            Description = product.Description,
                            FrameColor = product.FrameColor,
                            GlassColor = product.GlassColor,
                            Material = product.Material,
                        })
                        .Skip(skip).Take(take).ToListAsync();

                    foreach (var item in dbProducts)
                    {
                        var product = new ProductViewModel()
                        {
                            Id = item.Id,
                            ProductName = item.ProductName,
                            Price = item.Price,
                            Size = item.Size,
                            Image = item.Image,
                            BrandId = item.BrandId,
                            BrandName = item.BrandName,
                            FrindlyName = item.FriendlyName,
                            Description = item.Description,
                            FrameColor = item.FrameColor,
                            GlassColor = item.GlassColor,
                            Material = item.Material,
                        };

                        product.Category = new CategoryViewModel()
                        {
                            CategoryId = item.CategoryId,
                            CategoryName = item.CategoryName,
                        };

                        result.Products.Add(product);
                    }

                    var dbBrands = await eFDbContext.Brands.ToListAsync();
                    foreach (var dbBrand in dbBrands)
                    {
                        var brnd = new BrandViewModel()
                        {
                            BrandId = dbBrand.Id,
                            BrandName = dbBrand.BrandName,
                        };

                        result.Brands.Add(brnd);
                    }

                    var dbCategories = await eFDbContext.Categories.ToListAsync();
                    foreach (var cat in dbCategories)
                    {
                        var categry = new CategoryViewModel()
                        {
                            CategoryId = cat.Id,
                            CategoryName = cat.Name,
                        };

                        result.Categories.Add(categry);
                    }

                    result.Category = category.ToUpper();
                    result.Brand = brand.ToUpper();
                    result.Size = size.ToUpper();
                }
                else
                {
                    result.ResponseMessage.Message = "Not conected to database";
                    result.ResponseMessage.IsValid = false;
                }
            }
            catch (Exception ex)
            {
                result.ResponseMessage.Message = "Internal server error.";
                result.ResponseMessage.IsValid = false;
            }

            return result;
        }

        public async Task<ProductDetailViewModel> GetProductById(int id)
        {
            var result = new ProductDetailViewModel();
            result.ResponseMessage = new ResponseMessage();

            try
            {
                if (Connection.Open())
                {
                    var dbProduct = await eFDbContext.Product
                        .Where(c => c.Id == id).Select(c => c)
                        .Join(eFDbContext.Brands,
                        product => product.BrandId, brand => brand.Id,
                        (product, brand) => new
                        {
                            Id = product.Id,
                            ProductName = product.ProductName,
                            Price = product.Price,
                            Size = product.Size,
                            Image = product.Image,
                            BrandId = product.BrandId,
                            BrandName = brand.BrandName,
                            CategoryId = product.CategoryId,
                            FriendlyName = product.FriendlyName,
                            Description = product.Description,
                            FrameColor = product.FrameColor,
                            GlassColor = product.GlassColor,
                            Material = product.Material,
                        })
                        .Join(eFDbContext.Categories,
                        product => product.CategoryId, category => category.Id,
                        (product, category) => new
                        {
                            CategoryId = category.Id,
                            CategoryName = category.Name,
                            Id = product.Id,
                            ProductName = product.ProductName,
                            Price = product.Price,
                            Size = product.Size,
                            Image = product.Image,
                            BrandId = product.BrandId,
                            BrandName = product.BrandName,
                            FriendlyName = product.FriendlyName,
                            Description = product.Description,
                            FrameColor = product.FrameColor,
                            GlassColor = product.GlassColor,
                            Material = product.Material,
                        })
                        .FirstOrDefaultAsync();

                    if (dbProduct == null)
                    {
                        result.ResponseMessage.Message = "Invalid Request";
                        result.ResponseMessage.IsValid = false;

                        return result;
                    }

                    result.ProductDetail = new ProductViewModel()
                    {
                        Id = dbProduct.Id,
                        ProductName = dbProduct.ProductName,
                        Price = dbProduct.Price,
                        Size = dbProduct.Size,
                        Image = dbProduct.Image,
                        BrandId = dbProduct.BrandId,
                        BrandName = dbProduct.BrandName,
                        FrindlyName = dbProduct.FriendlyName,
                        Description = dbProduct.Description,
                        FrameColor = dbProduct.FrameColor,
                        GlassColor = dbProduct.GlassColor,
                        Material = dbProduct.Material,
                    };

                    result.ProductDetail.Category = new CategoryViewModel
                    {
                        CategoryId = dbProduct.CategoryId,
                        CategoryName = dbProduct.CategoryName,
                    };
                }
                else
                {
                    result.ResponseMessage.Message = "Not conected to database";
                    result.ResponseMessage.IsValid = false;
                }
            }
            catch (Exception)
            {
                result.ResponseMessage.Message = "Internal server error.";
                result.ResponseMessage.IsValid = false;
            }

            return result;
        }

        public async Task<ProductsViewModel> GetTop20ProductsByBrandId(string brand = "all", string category = "sunglases", string size = "all", int page = 1)
        {
            var result = new ProductsViewModel();
            result.ResponseMessage = new ResponseMessage();
            result.Products = new List<ProductViewModel>();
            result.Brands = new List<BrandViewModel>();
            result.Categories = new List<CategoryViewModel>();
            int skip = 0;
            int take = 20;
            skip = (page - 1) * take;

            try
            {
                if (Connection.Open())
                {
                    int brandId = Convert.ToInt32(brand);

                    var dbProducts = await eFDbContext.Product.Where(c => c.BrandId == brandId).Select(c => c)
                        .Join(eFDbContext.Brands,
                        product => product.BrandId, brand => brand.Id,
                        (product, brand) => new
                        {
                            Id = product.Id,
                            ProductName = product.ProductName,
                            Price = product.Price,
                            Size = product.Size,
                            Image = product.Image,
                            BrandId = product.BrandId,
                            BrandName = brand.BrandName,
                            CategoryId = product.CategoryId,
                            FriendlyName = product.FriendlyName,
                            Description = product.Description,
                            FrameColor = product.FrameColor,
                            GlassColor = product.GlassColor,
                            Material = product.Material,
                        })
                        .Join(eFDbContext.Categories,
                        product => product.CategoryId, category => category.Id,
                        (product, category) => new
                        {
                            CategoryId = category.Id,
                            CategoryName = category.Name,
                            Id = product.Id,
                            ProductName = product.ProductName,
                            Price = product.Price,
                            Size = product.Size,
                            Image = product.Image,
                            BrandId = product.BrandId,
                            BrandName = product.BrandName,
                            FriendlyName = product.FriendlyName,
                            Description = product.Description,
                            FrameColor = product.FrameColor,
                            GlassColor = product.GlassColor,
                            Material = product.Material,
                        })
                        .Skip(skip).Take(take).ToListAsync();

                    foreach (var item in dbProducts)
                    {
                        var product = new ProductViewModel()
                        {
                            Id = item.Id,
                            ProductName = item.ProductName,
                            Price = item.Price,
                            Size = item.Size,
                            Image = item.Image,
                            BrandId = item.BrandId,
                            BrandName = item.BrandName,
                            FrindlyName = item.FriendlyName,
                            Description = item.Description,
                            FrameColor = item.FrameColor,
                            GlassColor = item.GlassColor,
                            Material = item.Material,
                        };

                        product.Category = new CategoryViewModel()
                        {
                            CategoryId = item.CategoryId,
                            CategoryName = item.CategoryName,
                        };

                        result.Products.Add(product);
                    }

                    var dbBrands = await eFDbContext.Brands.ToListAsync();
                    foreach (var dbBrand in dbBrands)
                    {
                        var brnd = new BrandViewModel()
                        {
                            BrandId = dbBrand.Id,
                            BrandName = dbBrand.BrandName,
                        };

                        result.Brands.Add(brnd);
                    }

                    var dbCategories = await eFDbContext.Categories.ToListAsync();
                    foreach (var cat in dbCategories)
                    {
                        var categry = new CategoryViewModel()
                        {
                            CategoryId = cat.Id,
                            CategoryName = cat.Name,
                        };

                        result.Categories.Add(categry);
                    }

                    result.Category = category.ToUpper();
                    result.Brand = dbBrands.Where(c => c.Id == Convert.ToInt32(brand)).Select(c => c.BrandName).FirstOrDefault().ToUpper();
                    result.Size = size.ToUpper();
                }
                else
                {
                    result.ResponseMessage.Message = "Not conected to database";
                    result.ResponseMessage.IsValid = false;
                }
            }
            catch (Exception ex)
            {
                result.ResponseMessage.Message = "Internal server error.";
                result.ResponseMessage.IsValid = false;
            }

            return result;
        }


        public async Task<ProductsViewModel> GetTop20ProductsByCategoryId(string brand = "all", string category = "sunglases", string size = "all", int page = 1)
        {
            var result = new ProductsViewModel();
            result.ResponseMessage = new ResponseMessage();
            result.Products = new List<ProductViewModel>();
            result.Brands = new List<BrandViewModel>();
            result.Categories = new List<CategoryViewModel>();
            int skip = 0;
            int take = 20;
            skip = (page - 1) * take;

            try
            {
                if (Connection.Open())
                {
                    int categoryId = Convert.ToInt32(category);

                    var dbProducts = await eFDbContext.Product.Where(c => c.CategoryId == categoryId).Select(c => c)
                        .Join(eFDbContext.Brands,
                        product => product.BrandId, brand => brand.Id,
                        (product, brand) => new
                        {
                            Id = product.Id,
                            ProductName = product.ProductName,
                            Price = product.Price,
                            Size = product.Size,
                            Image = product.Image,
                            BrandId = product.BrandId,
                            BrandName = brand.BrandName,
                            CategoryId = product.CategoryId,
                            FriendlyName = product.FriendlyName,
                            Description = product.Description,
                            FrameColor = product.FrameColor,
                            GlassColor = product.GlassColor,
                            Material = product.Material,
                        })
                        .Join(eFDbContext.Categories,
                        product => product.CategoryId, category => category.Id,
                        (product, category) => new
                        {
                            CategoryId = category.Id,
                            CategoryName = category.Name,
                            Id = product.Id,
                            ProductName = product.ProductName,
                            Price = product.Price,
                            Size = product.Size,
                            Image = product.Image,
                            BrandId = product.BrandId,
                            BrandName = product.BrandName,
                            FriendlyName = product.FriendlyName,
                            Description = product.Description,
                            FrameColor = product.FrameColor,
                            GlassColor = product.GlassColor,
                            Material = product.Material,
                        })
                        .Skip(skip).Take(take).ToListAsync();

                    foreach (var item in dbProducts)
                    {
                        var product = new ProductViewModel()
                        {
                            Id = item.Id,
                            ProductName = item.ProductName,
                            Price = item.Price,
                            Size = item.Size,
                            Image = item.Image,
                            BrandId = item.BrandId,
                            BrandName = item.BrandName,
                            FrindlyName = item.FriendlyName,
                            Description = item.Description,
                            FrameColor = item.FrameColor,
                            GlassColor = item.GlassColor,
                            Material = item.Material,
                        };

                        product.Category = new CategoryViewModel()
                        {
                            CategoryId = item.CategoryId,
                            CategoryName = item.CategoryName,
                        };

                        result.Products.Add(product);
                    }

                    var dbBrands = await eFDbContext.Brands.ToListAsync();
                    foreach (var dbBrand in dbBrands)
                    {
                        var brnd = new BrandViewModel()
                        {
                            BrandId = dbBrand.Id,
                            BrandName = dbBrand.BrandName,
                        };

                        result.Brands.Add(brnd);
                    }

                    var dbCategories = await eFDbContext.Categories.ToListAsync();
                    foreach (var cat in dbCategories)
                    {
                        var categry = new CategoryViewModel()
                        {
                            CategoryId = cat.Id,
                            CategoryName = cat.Name,
                        };

                        result.Categories.Add(categry);
                    }

                    result.Category = dbCategories.Where(c => c.Id == Convert.ToInt32(category)).Select(c => c.Name).FirstOrDefault().ToUpper(); ;
                    result.Brand = brand.ToUpper(); ;
                    result.Size = size.ToUpper();
                }
                else
                {
                    result.ResponseMessage.Message = "Not conected to database";
                    result.ResponseMessage.IsValid = false;
                }
            }
            catch (Exception ex)
            {
                result.ResponseMessage.Message = "Internal server error.";
                result.ResponseMessage.IsValid = false;
            }

            return result;
        }
    }
}
