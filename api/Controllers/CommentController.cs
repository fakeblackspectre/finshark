﻿using System;
using api.DTOs.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
  private readonly ICommentRepository _commentRepo;
  private readonly IStockRepository _stockRepo;
  private readonly UserManager<AppUser> _userManager;

  public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
  {
    _commentRepo = commentRepo;
    _stockRepo = stockRepo;
    _userManager = userManager;
  }

  [HttpGet]
  public async Task<IActionResult> GetAll()
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    var comments = await _commentRepo.GetAllAsync();
    var commentDTO = comments.Select(s => s.ToCommentDTO());
    return Ok(commentDTO);
  }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById([FromRoute] int id)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    var comment = await _commentRepo.GetByIdAsync(id);
    if (comment == null) return NotFound();
    return Ok(comment.ToCommentDTO());
  }

  [HttpPost("{stockId:int}")]
  public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentRequestDTO commentDTO)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    if (!await _stockRepo.StockExists(stockId)) return BadRequest("Stock does not exist");

    var username = User.GetUsername();
    var appUser = await _userManager.FindByNameAsync(username);

    var commentModel = commentDTO.ToCommentFromCreateDTO(stockId);
    commentModel.AppUserId = appUser.Id;
    await _commentRepo.CreateAsync(commentModel);
    return CreatedAtAction(nameof(GetById), new { Id = commentModel.Id }, commentModel.ToCommentDTO());
  }

  [HttpPut]
  [Route("{id:int}")]
  public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDTO updateDTO)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    var comment = await _commentRepo.UpdateAsync(id, updateDTO.ToCommentFromUpdateDTO());
    if (comment == null) return NotFound("Comment not found");
    return Ok(comment.ToCommentDTO());
  }

  [HttpDelete]
  [Route("{id:int}")]
  public async Task<IActionResult> Delete([FromRoute] int id)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    var comment = await _commentRepo.DeleteAsync(id);
    if (comment == null) return NotFound("Comment does not exist");
    return Ok(comment);
  }
}
