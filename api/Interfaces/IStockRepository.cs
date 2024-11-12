﻿using System;
using api.DTOs.Stock;
using api.Models;

namespace api.Interfaces;

public interface IStockRepository
{
  Task<List<Stock>> GetAllAsync();
  Task<Stock?> GetByIdAsync(int id);
  Task<Stock> CreateAsync(Stock stockModel);
  Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO stockDTO);
  Task<Stock?> DeleteAsync(int id);
  Task<bool> StockExists(int id);
}