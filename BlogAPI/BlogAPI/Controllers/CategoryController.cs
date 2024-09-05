using BlogDAL.DTO;
using BlogDAL.Models;
using BlogDAL.UnitOfWork;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tag = await unitOfWork.GetRepository<Category>().GetAll();
            return Ok(tag);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CategoryDTO categoryDTO)
        {
            var category = categoryDTO.Adapt<Category>();
            var createdCategory = await unitOfWork.GetRepository<Category>().Add(category);
            unitOfWork.SaveChanges();
            return Ok(createdCategory);
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await unitOfWork.GetRepository<Category>().Delete(x => x.CategoryId == id);
            return Ok("Category Deleted");
        }
    }
}
