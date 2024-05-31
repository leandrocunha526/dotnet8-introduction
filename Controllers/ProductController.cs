using dotnet8_introduction.Data;
using dotnet8_introduction.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet8_introduction.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        public IRepository _repository { get; }
        public ProductController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("list")]
        public async Task<IActionResult> Get([FromQuery] int? limit)
        {
            try
            {
                var results = await _repository!.GetAllProductsAsync(includeCategory: true);
                if (limit.HasValue)
                {
                    results = results.Take(limit.Value).ToArray();
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
        [HttpGet("average-price")]
        public async Task<ActionResult> GetAveragePrice()
        {
            var averagePrice = await _repository!.GetAveragePriceAsync();

            if (averagePrice == 0)
            {
                return Ok(new { AveragePrice = averagePrice, Message = "No products available." });
            }

            return Ok(new { AveragePrice = averagePrice, Message = "Average price calculated successfully." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var results = await _repository!.GetProductByIdAsync(id, includeCategory: true);
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
        public async Task<IActionResult> Post(Product model)
        {
            try
            {
                _repository!.Add(model);
                if (await _repository.SaveChangesAsync())
                {
                    return Created($"/api/product/{model.Id}", model);
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
        public async Task<IActionResult> Put(int id, Product model)
        {
            try
            {
                var product = await _repository!.GetProductByIdAsync(id, includeCategory: false);
                if (product == null)
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
                var product = await _repository!.GetProductByIdAsync(id, includeCategory: false);
                if (product == null)
                {
                    return NotFound();
                }

                _repository.Delete(product);

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(new { message = "Product deleted successfully." });
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
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string searchTerm)
        {
            try
            {
                var results = await _repository!.SearchProductsAsync(searchTerm, includeCategory: true);
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error 500 - Internal Server Error (database error)");
            }
        }
    }
}
