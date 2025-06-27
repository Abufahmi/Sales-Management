using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sales.Context.Contracts;
using Sales.Library.Entities;
using Sales.Library.Services;

namespace Sales.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [Authorize]
    [HttpPost]
    [Route("CreateCategory")]
    public async Task<IActionResult> CreateCategory(Category category)
    {
        var response = await categoryService.CreateCategoryAsync(category);
        if (response.IsSuccess && response.TItem != null)
        {
            return Ok(response.TItem);
        }
        else if (response.ErrorMessage != null)
        {
            return BadRequest(response.ErrorMessage);
        }
        return BadRequest();
    }

    [Authorize]
    [HttpDelete]
    [Route("DeleteCategoryById/{id}")]
    public async Task<IActionResult> DeleteCategoryById(int id)
    {
        var response = await categoryService.DeleteCategoryByIdAsync(id);
        if (response.IsSuccess)
        {
            return Ok(true);
        }
        else if (response.ErrorMessage != null)
        {
            return BadRequest(response.ErrorMessage);
        }
        return BadRequest();
    }

    [Authorize]
    [HttpDelete]
    [Route("DeleteCategoryList/{serialize}")]
    public async Task<IActionResult> DeleteCategoryList(string serialize)
    {
        var ids = SerializationService<int>.DeSerializeList(serialize);
        if (ids == null || ids.Count == 0)
        {
            return BadRequest("Parameters required.");
        }

        var response = await categoryService.DeleteCategoryListAsync(ids);
        if (response.IsSuccess)
        {
            return Ok(true);
        }
        else if (response.ErrorMessage != null)
        {
            return BadRequest(response.ErrorMessage);
        }
        return BadRequest();
    }

    [HttpGet]
    [Route("GetCategoryById/{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var response = await categoryService.GetCategoryByIdAsync(id);
        if (response.IsSuccess && response.TItem != null)
        {
            return Ok(response.TItem);
        }
        else if (response.ErrorMessage != null)
        {
            return BadRequest(response.ErrorMessage);
        }
        return BadRequest();
    }


    [HttpGet]
    [Route("GetCategoryList")]
    public async Task<IActionResult> GetCategoryList()
    {
        var response = await categoryService.GetCategoryListAsync();
        if (response.IsSuccess && response.TList != null)
        {
            return Ok(response.TList);
        }
        else if (response.ErrorMessage != null)
        {
            return BadRequest(response.ErrorMessage);
        }
        return BadRequest();
    }

    [Authorize]
    [HttpPut]
    [Route("UpdateCategory")]
    public async Task<IActionResult> UpdateCategory(Category category)
    {
        var response = await categoryService.UpdateCategoryAsync(category);
        if (response.IsSuccess)
        {
            return Ok();
        }
        else if (response.ErrorMessage != null)
        {
            return BadRequest(response.ErrorMessage);
        }
        return BadRequest();
    }
}
