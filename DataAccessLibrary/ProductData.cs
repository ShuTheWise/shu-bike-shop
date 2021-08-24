using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess db;

        public ProductData(ISqlDataAccess db)
        {
            this.db = db;
        }

        public static Type CategoryToType(string category)
        {
            var str = category.TrimEnd('s');
            var typePrefix = char.ToUpper(str[0]) + str.Substring(1);
            return Type.GetType($"DataAccessLibrary.Models.{typePrefix}Model");
        }

        public Task DecrementProductAmount(int id, int amount)
        {
            const string sql = "update products set amount = amount - @amount where id = @Id";
            return db.SaveData(sql, new { id, amount });
        }

        public async Task RemoveProduct(int id)
        {
            string category = await GetCategory(id);

            string sql = "delete from products where id = @id";
            await db.SaveData(sql, id);

            string sql2 = $"delete from {category} where id = @id";
            await db.SaveData(sql2, id);
        }

        public async Task<ProductModel> GetProduct(int id)
        {
            string category = await GetCategory(id);
            string sql2 = $"select * from {category} c left join products p on p.id = c.id where c.id = @id";
            return await db.LoadSingleOrDefault<ProductModel, dynamic>(CategoryToType(category), sql2, new { id });
        }

        public async Task<T> GetProduct<T>(int id) where T : ProductModel
        {
            string category = await GetCategory(id);
            string sql = $"select * from {category} c left join products p on p.id = c.id where c.id = @id";
            return await db.LoadSingleOrDefault<T, dynamic>(CategoryToType(category), sql, new { id });
        }

        private async Task<string> GetCategory(int id)
        {
            const string sql = "select category from products where id = @id";
            var category = await db.LoadSingleOrDefault<string, dynamic>(sql, new { id });
            return category;
        }

        public Task<List<ProductModel>> GetProducts(string category)
        {
            string sql = $"select * from {category} c left join products p where c.id = p.id";
            return db.LoadData<ProductModel, dynamic>(CategoryToType(category), sql, new { category });
        }

        public Task<List<T>> GetProducts<T>() where T : ProductModel
        {
            string category = GetCategory<T>();
            string sql = $"select * from {category} left join products using (id) order by id";
            return db.LoadData<T, dynamic>(CategoryToType(category), sql, new { category });
        }

        public static string GetCategory<T>() where T : ProductModel => typeof(T).Name[0..^5].ToLower() + 's';

        public async Task AddProduct<T, U>(U createModel) where U : ProductCreateModel where T : ProductModel
        {
            var category = GetCategory<T>();

            string sql = $"insert into products (price, amount, category) values (@Price, @Amount, '{category}') returning id";
            int productId = await db.SaveData<U, int>(sql, createModel);

            var childProps = typeof(U).GetProperties().Where(x => x.DeclaringType != typeof(ProductCreateModel));

            var childPropertiesName = string.Join(", ", childProps.Select(p => $"@{p.Name}"));
            var rowNames = string.Join(", ", childProps.Select(p => $"{p.Name.ToLower()}"));

            string sql2 = $"insert into {category} (id, {rowNames}) values ({productId}, {childPropertiesName})";
            await db.SaveData(sql2, createModel);
        }

        public async Task UpdateProduct<T>(int id, dynamic updateModel) where T : ProductModel
        {
            var category = GetCategory<T>();
            var allPropsOfModel = typeof(T).GetProperties();

            //if updating only product
            bool productOnly = typeof(T) == typeof(ProductModel);

            var accessibleParentPropsOfModel = allPropsOfModel.Where(x => x.DeclaringType == typeof(ProductModel) && x.CanWrite && x.CanRead && x.Name.ToLower() != "category" && x.Name.ToLower() != "id").ToArray();
            var accessibleParentPropsNamesOfModel = accessibleParentPropsOfModel.Select(x => x.Name.ToLower()).ToHashSet();

            object o = updateModel;

            var propsOfDynamic = o.GetType().GetProperties();
            var parentPropsOfDynamic = propsOfDynamic.Where(x => accessibleParentPropsNamesOfModel.Contains(x.Name));
           
            if (parentPropsOfDynamic.Any())
            {
                var set = string.Join(", ", parentPropsOfDynamic.Select(p => $"{p.Name} = @{p.Name}"));
                string sql = $"update products set {set} where id = '{id}'";
                await db.SaveData(sql, updateModel);
            }

            if (!productOnly)
            {
                var accessibleChildPropsOfModel = allPropsOfModel.Where(x => x.DeclaringType != typeof(T) && x.CanWrite && x.CanRead).ToArray();
                var accessibleChildPropsNamesOfModel = accessibleChildPropsOfModel.Select(x => x.Name.ToLower()).ToHashSet();
                
                var childPropsOfDynamic = propsOfDynamic.Where(x => !accessibleChildPropsNamesOfModel.Contains(x.Name));
                if (childPropsOfDynamic.Any())
                {
                    var set = string.Join(", ", childPropsOfDynamic.Select(p => $"{p.Name} = @{p.Name}"));
                    string sql = $"update {category} set {set} where id = '{id}'";
                    await db.SaveData(sql, updateModel);
                }
            }
        }
    }
}
