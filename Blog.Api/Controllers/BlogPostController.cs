using Blog.Api.Models.Domain;
using Blog.Api.Models.DTO;
using Blog.Api.Repositories.Implimentations;
using Blog.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IcategoryRepository _categoryRepository;

        public BlogPostController(IBlogPostRepository _blogPostRepository,
            IcategoryRepository _categoryRepository)
        {
            this._blogPostRepository = _blogPostRepository;
            this._categoryRepository = _categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto blogPostDto)
        {
            var blogPost = new BlogPost
            {
                Title = blogPostDto.Title,
                ShortDescription = blogPostDto.ShortDescription,
                Content = blogPostDto.Content,
                FeatureImageUrl = blogPostDto.FeatureImageUrl,
                UrlHandlerget = blogPostDto.UrlHandlerget,
                PublishedDate = blogPostDto.PublishedDate,
                Author = blogPostDto.Author,
                IsVisible = blogPostDto.IsVisible,
                Categories = new List<Category>()
            };

            foreach (var catGuid in blogPostDto.Categories)
            {
                var existingCat = await _categoryRepository.GetById(catGuid);
                if (existingCat != null)
                {
                    blogPost.Categories.Add(existingCat);
                }
            }

            blogPost = await _blogPostRepository.CreateAsync(blogPost);
            var response = new BlogPostDto
            {
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                FeatureImageUrl = blogPost.FeatureImageUrl,
                UrlHandlerget = blogPost.UrlHandlerget,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandler = x.UrlHandler,
                }).ToList()
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogPost()
        {
            var blogPosts = await _blogPostRepository.GetAllAsync();
            var response = new List<BlogPostDto>();
            foreach (var blogpost in blogPosts)
            {
                response.Add(new BlogPostDto
                {
                    Id = blogpost.Id,
                    Title = blogpost.Title,
                    ShortDescription = blogpost.ShortDescription,
                    Content = blogpost.Content,
                    FeatureImageUrl = blogpost.FeatureImageUrl,
                    PublishedDate = blogpost.PublishedDate,
                    Author = blogpost.Author,
                    IsVisible = blogpost.IsVisible,
                    Categories = blogpost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandler = x.UrlHandler,
                    }).ToList()
                });
            }
            return Ok(response);

        }


        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            var existingBlog = await _blogPostRepository.GetPostById(id);
            if (existingBlog == null)
            {
                return NotFound();
            }
            var response = new BlogPostDto
            {
                Id = existingBlog.Id,
                Title = existingBlog.Title,
                ShortDescription = existingBlog.ShortDescription,
                Content = existingBlog.Content,
                FeatureImageUrl = existingBlog.FeatureImageUrl,
                PublishedDate = existingBlog.PublishedDate,
                Author = existingBlog.Author,
                IsVisible = existingBlog.IsVisible,
                Categories = existingBlog.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandler = x.UrlHandler,
                }).ToList()

            };
            return Ok(response);
        }
    }
}
