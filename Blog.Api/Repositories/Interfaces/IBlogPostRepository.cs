using Blog.Api.Models.Domain;

namespace Blog.Api.Repositories.Interfaces
{
    public interface IBlogPostRepository
    {
        public Task<BlogPost> CreateAsync(BlogPost blogPost);

        public Task<IEnumerable<BlogPost>> GetAllAsync();

        public Task<BlogPost?> GetPostById(Guid id);

    }
}
