using Application.DTOs;
using Application.Interfaces.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Dashboard
{
    [Authorize(Policy = "DashboardPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryRepo _categoryRepo;

        public CategoryController(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        [HttpGet("issueCategories")]
        public async Task<ActionResult<List<IssueCategoryDto>>> GetAll()
        {
            var result = await _categoryRepo.GetAllCategoriesAsync();
            return Ok(result);
        }

        [HttpGet("top-reported")]
        public async Task<IActionResult> GetTopReportedCategories()
        {
            var result = await _categoryRepo.GetTopReportedCategoriesAsync();
            return Ok(result);
        }

    }
}
