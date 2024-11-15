﻿using System;
using api.DTOs.Comment;
using api.DTOs.Stock;
using api.Models;

namespace api.Mappers;

public static class CommentMapper
{
  public static CommentDTO ToCommentDTO(this Comment commentModel)
  {
    return new CommentDTO
    {
      Id = commentModel.Id,
      Title = commentModel.Title,
      Content = commentModel.Content,
      CreatedOn = commentModel.CreatedOn,
      CreatedBy = commentModel.AppUser.UserName,
      StockId = commentModel.StockId
    };
  }

  public static Comment ToCommentFromCreateDTO(this CreateCommentRequestDTO commentDTO, int stockId)
  {
    return new Comment
    {
      Title = commentDTO.Title,
      Content = commentDTO.Content,
      StockId = stockId
    };
  }

  public static Comment ToCommentFromUpdateDTO(this UpdateCommentRequestDTO commentDTO)
  {
    return new Comment
    {
      Title = commentDTO.Title,
      Content = commentDTO.Content,
    };
  }
}
