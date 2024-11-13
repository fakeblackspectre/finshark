﻿using System;
using api.Data;
using api.DTOs.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository : IStockRepository
{
  private readonly ApplicationDbContext _context;

  public StockRepository(ApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<Stock> CreateAsync(Stock stockModel)
  {
    await _context.Stocks.AddAsync(stockModel);
    await _context.SaveChangesAsync();
    return stockModel;
  }

  public async Task<Stock?> DeleteAsync(int id)
  {
    var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
    if (stockModel == null) return null;
    _context.Stocks.Remove(stockModel);
    await _context.SaveChangesAsync();
    return stockModel;
  }

  public async Task<List<Stock>> GetAllAsync(QueryObject query)
  {
    var stocks = _context.Stocks.Include(c => c.Comments).AsQueryable();

    if (!string.IsNullOrWhiteSpace(query.CompanyName))
      stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
    if (!string.IsNullOrWhiteSpace(query.Symbol))
      stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
    if (!string.IsNullOrWhiteSpace(query.SortBy))
    {
      if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
      {
        stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
      }
    }
    var skipNumber = (query.PageNumber - 1) * query.PageSize;

    return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
  }

  public async Task<Stock?> GetByIdAsync(int id)
  {
    return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
  }

  public Task<bool> StockExists(int id)
  {
    return _context.Stocks.AnyAsync(s => s.Id == id);
  }

  public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO stockDTO)
  {
    var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
    if (existingStock == null) return null;

    existingStock.Symbol = stockDTO.Symbol;
    existingStock.CompanyName = stockDTO.CompanyName;
    existingStock.Purchase = stockDTO.Purchase;
    existingStock.LastDiv = stockDTO.LastDiv;
    existingStock.Industry = stockDTO.Industry;
    existingStock.MarketCap = stockDTO.MarketCap;

    await _context.SaveChangesAsync();
    return existingStock;
  }
}
