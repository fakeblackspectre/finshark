using System;
using api.DTOs.Comment;
using api.Interfaces;
using api.Mappers;
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
  public async Task<IActionResult> GetAll() {
    var comments = await _commentRepo.GetAllAsync();
    var commentDTO = comments.Select(s => s.ToCommentDTO());
    return Ok(commentDTO);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetById([FromRoute] int id) {
    var comment = await _commentRepo.GetByIdAsync(id);
    if (comment == null) return NotFound();
    return Ok(comment.ToCommentDTO());
  }

  [HttpPost("{stockId}")]
  public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentRequestDTO commentDTO) {
    if (!await _stockRepo.StockExists(stockId)) {
      return BadRequest("Stock does not exist");
    }
    var commentModel = commentDTO.ToCommentFromCreate(stockId);
    await _commentRepo.CreateAsync(commentModel);
    return CreatedAtAction(nameof(GetById), new { Id = commentModel }, commentModel.ToCommentDTO());
  }
}
