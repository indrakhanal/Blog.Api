using Blog.Api.Data;
using Blog.Api.Models.Domain;
using Blog.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Repositories.Implimentations
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BlogPostRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await _dbContext.BlogPosts.AddAsync(blogPost);
            await _dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _dbContext.BlogPosts.Include(x=>x.Categories).ToListAsync();

        }

        public async Task<BlogPost?> GetPostById(Guid id)
        {
            return await _dbContext.BlogPosts.FirstOrDefaultAsync(x=>x.Id == id);
        }
    }
}
