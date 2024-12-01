using API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Application.Dtos;
using Application.Interfaces;
using Application.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize]
    public class BaseGenericApiController<TEntity, TAddDto, TReturnDto> : BaseApiController
        where TEntity : BaseEntity
        where TAddDto : BaseDto
        where TReturnDto : BaseDto

    {
        private readonly IUnitOfWork _uow;
        private readonly IBaseRepo<TEntity> _Repo;

        public BaseGenericApiController(IUnitOfWork uow)
        {
            _uow = uow;
            _Repo = _uow.BaseRepo<TEntity>();
        }


        [HttpPost("add")]
        public virtual async Task<IActionResult> Add(TAddDto dto)
        {
            dto.Id = 0;
            var x = _uow.Mapper.Map<TEntity>(dto);

            var result = _Repo.Add(x);

            if (!await _uow.SaveAsync()) return BadRequest();

            var map = await _Repo.Map_GetByAsync<TReturnDto>(x => x.Id == result.Id);
            // var map = _uow.Mapper.Map<TReturnDto>(result);

            return Ok(map);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        protected virtual async Task<IActionResult> AddEntity(TEntity entity)
        {
            var result = _Repo.Add(entity);

            if (!await _uow.SaveAsync()) return BadRequest();

            // var map = _uow.Mapper.Map<TReturnDto>(result);
            var map = await _Repo.Map_GetByAsync<TReturnDto>(x => x.Id == result.Id);


            return Ok(map);
        }

        [HttpPut("update")]
        public virtual async Task<IActionResult> Update(TAddDto dto)
        {
            var entity = await _Repo.GetByAsync(x => x.Id == dto.Id);

            if (entity == null) return NotFound();

            var result = _uow.Mapper.Map(dto, entity);

            _Repo.Update(result);

            if (!await _uow.SaveAsync()) return BadRequest("Failed to Update");

            var map = await _Repo.Map_GetByAsync<TReturnDto>(x => x.Id == result.Id);

            return Ok(map);

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        protected virtual async Task<IActionResult> UpdateEntity(TEntity entity)
        {
            _Repo.Update(entity);

            if (!await _uow.SaveAsync()) return BadRequest("Failed to Update");

            var map = await _Repo.Map_GetByAsync<TReturnDto>(x => x.Id == entity.Id);

            return Ok(map);

        }

        [HttpDelete("delete/{id}")]
        public virtual async Task<ActionResult> Delete(int id)
        {
            var entity = await _Repo.GetByAsync(x => x.Id == id);

            if (entity == null) return NotFound();

            entity.IsDeleted = true;

            _Repo.Update(entity);

            if (!await _uow.SaveAsync()) return BadRequest();

            return Ok();


        }

        // [HttpGet]
        // public virtual async Task<IActionResult> Get()
        // {
        //     var result = await _Repo.Map_GetAllAsync<TReturnDto>();

        //     return Ok(result);
        // }


        [HttpGet]
        public virtual async Task<IActionResult> Get()
        {
            var result = await _Repo.Map_GetAllByAsync<TReturnDto>(x => x.IsDeleted == false);

            result = result.OrderByDescending(x => x.Id);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            // var cityId = _hashids.Decode(id)[0];

            var result = await _Repo.Map_GetByAsync<TReturnDto>(x => x.Id == id);

            return Ok(result);
        }


        [HttpGet("getAllPagination")]
        public virtual async Task<ActionResult<IEnumerable<TReturnDto>>> GetAllPagination([FromQuery] PaginationParams paginationParams)
        {
            var result = await _Repo.GetAllQueryableBy<TReturnDto>(x => x.IsDeleted == false, x => x.Id, paginationParams);

            // query =  query.AsSplitQuery();

            PaginationHeader(result);

            return Ok(result);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public void PaginationHeader(PagedList<TReturnDto> result)
        {
            Response.AddPaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
        }

    }
}