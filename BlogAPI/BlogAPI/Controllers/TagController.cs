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
    public class TagController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public TagController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tag = await unitOfWork.GetRepository<Tag>().GetAll();
            return Ok(tag);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] TagDTO tagDto)
        {
            var tag = tagDto.Adapt<Tag>();
            var createdTag = await unitOfWork.GetRepository<Tag>().Add(tag);
            unitOfWork.SaveChanges();
            return Ok(createdTag);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await unitOfWork.GetRepository<Tag>().Delete(x => x.TagId == id);
            return Ok("Tag Deleted");
        }

    }
}
