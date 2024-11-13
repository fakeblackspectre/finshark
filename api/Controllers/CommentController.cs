using System;
using api.DTOs.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
  private readonly ICommentRepository _commentRepo;
  private readonly IStockRepository _stockRepo;

  public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
  {
    _commentRepo = commentRepo;
    _stockRepo = stockRepo;
  }

  [HttpGet]
  public async Task<IActionResult> GetAll()
  {
    var comments = await _commentRepo.GetAllAsync();
    var commentDTO = comments.Select(s => s.ToCommentDTO());
    return Ok(commentDTO);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetById([FromRoute] int id)
  {
    var comment = await _commentRepo.GetByIdAsync(id);
    if (comment == null) return NotFound();
    return Ok(comment.ToCommentDTO());
  }

  [HttpPost("{stockId}")]
  public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentRequestDTO commentDTO)
  {
    if (!await _stockRepo.StockExists(stockId)) return BadRequest("Stock does not exist");

    var commentModel = commentDTO.ToCommentFromCreateDTO(stockId);
    await _commentRepo.CreateAsync(commentModel);
    return CreatedAtAction(nameof(GetById), new { Id = commentModel.Id }, commentModel.ToCommentDTO());
  }

  [HttpPut]
  [Route("{id}")]
  public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDTO updateDTO)
  {
    var comment = await _commentRepo.UpdateAsync(id, updateDTO.ToCommentFromUpdateDTO());
    if (comment == null) return NotFound("Comment not found");
    return Ok(comment.ToCommentDTO());
  }

  [HttpDelete]
  [Route("{id}")]
  public async Task<IActionResult> Delete([FromRoute] int id)
  {
    var comment = await _commentRepo.DeleteAsync(id);
    if (comment == null) return NotFound("Comment does not exist");
    return Ok(comment);
  }
}
