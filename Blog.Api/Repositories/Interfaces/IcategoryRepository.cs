using Blog.Api.Models.Domain;

namespace Blog.Api.Repositories.Interfaces
{
    public interface IcategoryRepository
    {
        public Task<Category> CreateAsync(Category category);
        public Task<IEnumerable<Category>> GetAllAsync();

        public Task<Category?> GetById(Guid id);

        public Task<Category?> UpdateAsync(Category category);
        public Task<Category?> DeleteAsync(Guid id);
    }
}
