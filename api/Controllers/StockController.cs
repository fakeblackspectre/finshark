﻿using System;
using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
  private readonly ApplicationDbContext _context;
  private readonly IStockRepository _stockRepo;
  public StockController(ApplicationDbContext context, IStockRepository stockRepo)
  {
    _stockRepo = stockRepo;
    _context = context;
  }

  [HttpGet]
  public async Task<IActionResult> GetAll() {
    var stocks = await _stockRepo.GetAllAsync();
    var stockDTO = stocks.Select(s => s.ToStockDTO());
    return Ok(stocks);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetById([FromRoute] int id) {
    var stock = await _stockRepo.GetByIdAsync(id);
    if (stock == null) return NotFound();
    return Ok(stock.ToStockDTO());
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateStockRequestDTO stockDTO) {
    var stockModel = stockDTO.ToStockFromCreateDTO();
    await _stockRepo.CreateAsync(stockModel);
    return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDTO());
  }

  [HttpPut]
  [Route("{id}")]
  public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateDTO) {
    var stockModel = await _stockRepo.UpdateAsync(id, updateDTO);
    if (stockModel == null) return NotFound();
    return Ok(stockModel);
  }

  [HttpDelete]
  [Route("{id}")]
  public async Task<IActionResult> Delete([FromRoute] int id) {
    var stockModel = await _stockRepo.DeleteAsync(id);
    if (stockModel == null) return NotFound();
    return NoContent();
  }
}