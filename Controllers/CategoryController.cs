using dotnet8_introduction.Data;
using Microsoft.AspNetCore.Mvc;
using dotnet8_introduction.Model;

namespace dotnet8_introduction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        public IRepository? _repository { get; }
        public CategoryController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("list")]
        public async Task<IActionResult> Get(bool includeProducts = true)
        {
            try
            {
                var results = await _repository!.GetAllCategoriesAsync(includeProducts);
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error 500 - Internal Server Error (database error)"
                );
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var results = await _repository!.GetCategoryByIdAsync(id);
                if (results == null)
                {
                    return NotFound();
                }
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error 500 - Internal Server Error (database error)"
                );
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Category model)
        {
            try
            {
                _repository!.Add(model);
                if (await _repository.SaveChangesAsync())
                {
                    return Created($"/api/category/{model.Id}", model);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error 500 - Internal Server Error (database error)"
                );
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Category model)
        {
            try
            {
                var category = await _repository!.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    return NotFound();
                }
                _repository.Update(model);
                if (await _repository.SaveChangesAsync())
                {
                    return Ok(model);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error 500 - Internal Server Error (database error)"
                );
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _repository!.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    return NotFound();
                }
                _repository.Delete(category);
                if (await _repository.SaveChangesAsync())
                {
                    return Ok(new { message = "Category deleted" });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error 500 - Internal Server Error (database error)"
                );
            }
            return BadRequest();
        }
    }
}
