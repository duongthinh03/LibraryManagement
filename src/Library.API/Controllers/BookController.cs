using Library.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search,
            [FromQuery] string? category,
            [FromQuery] string? language,
            [FromQuery] int? fromYear,
            [FromQuery] int? toYear,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDirection,
            [FromQuery] int pageNo = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _bookService.GetAllAsync(
                search,
                category,
                language,
                fromYear,
                toYear,
                sortBy,
                sortDirection,
                pageNo,
                pageSize);
            return Ok(result);
        }
    }
}
