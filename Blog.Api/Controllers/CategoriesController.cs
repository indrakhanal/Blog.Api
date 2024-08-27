using Blog.Api.Data;
using Blog.Api.Models.Domain;
using Blog.Api.Models.DTO;
using Blog.Api.Repositories.Implimentations;
using Blog.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IcategoryRepository categoryRepository;

        public CategoriesController(IcategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }


        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            // Map DTO to domain model
            var category = new Category
            {
                Name = request.Name,
                UrlHandler = request.UrlHandler
            };
           
            /*we can directly return category object but we dont want to expose our real object so we need to map it to create a DTO and 
             * send back to the UI. for that we need to add new file and create Dto class
            return Ok(category);
            Domain Model to DTO*/
            await categoryRepository.CreateAsync(category);
            var response = new CategoryDto {
                Name = request.Name, 
                UrlHandler = request.UrlHandler
            };
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            //
            var categories = await categoryRepository.GetAllAsync();
            var response  = new List<CategoryDto>();
            foreach (var category in categories) {
                response.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandler = category.UrlHandler
                });
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var existingCategory = await categoryRepository.GetById(id);
            if (existingCategory == null) { 
                return NotFound();
            }
            var response = new CategoryDto
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                UrlHandler = existingCategory.UrlHandler
            };
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]

        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, UpdateCategoryRequestDto request)
        {
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandler = request.UrlHandler
            };
            category = await categoryRepository.UpdateAsync(category);
            if (category == null) {
                return NotFound();
            }
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandler = category.UrlHandler
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await categoryRepository.DeleteAsync(id);
            if (category is null) 
            { 
                return NotFound(); 
            }
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandler = category.UrlHandler
            };
            return Ok(response);
        }


    }

   
}
