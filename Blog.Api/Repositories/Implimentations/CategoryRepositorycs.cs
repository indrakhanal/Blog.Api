using Blog.Api.Data;
using Blog.Api.Models.Domain;
using Blog.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Repositories.Implimentations
{
    public class CategoryRepositorycs:IcategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepositorycs(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Category> CreateAsync(Category category) {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetById(Guid id)
        {
           return await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(z => z.Id == category.Id);
            if (existingCategory != null) { 
                dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
                dbContext.SaveChanges();
                return category;
            }
            return null;
        }
        public async Task<Category?> DeleteAsync(Guid id)
        {
            var deletCat = await dbContext.Categories.FirstOrDefaultAsync(z => z.Id == id);
            if (deletCat is null) {
                return null;
            }
            dbContext.Categories.Remove(deletCat);
            await dbContext.SaveChangesAsync();
            return deletCat;

        }
    }
}
