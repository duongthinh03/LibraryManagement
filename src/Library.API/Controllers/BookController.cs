using Library.Application.Common.Authorization;
using Library.Application.Common.Requests;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = Permissions.BookView)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequest request)
        {
            var result = await _bookService.GetAllAsync(request);
            return Ok(result);
        }
    }
}
