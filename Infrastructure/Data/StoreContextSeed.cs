using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory, IConfiguration config)
        {
            try
            {
                if (!context.ProductBrands.Any())
                {
                    await AddProductBrands(context, config);
                }

                if (!context.ProductTypes.Any())
                {
                    await AddProductTypes(context, config);
                }

                if (!context.Products.Any())
                {
                    await AddProducts(context, config);
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }

        private static async Task AddProductBrands(StoreContext context, IConfiguration config)
        {
            var brandsData = File.ReadAllText(config["Paths:SeedBrands"]);
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
            context.ProductBrands.AddRange(brands);
            await context.SaveChangesAsync();
        }

        private static async Task AddProductTypes(StoreContext context, IConfiguration config)
        {
            var typesData = File.ReadAllText(config["Paths:SeedTypes"]);
            var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
            context.ProductTypes.AddRange(types);
            await context.SaveChangesAsync();
        }

        private static async Task AddProducts(StoreContext context, IConfiguration config)
        {
            var productsData = File.ReadAllText(config["Paths:SeedProducts"]);
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
    }
}