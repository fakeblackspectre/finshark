﻿using System;
using api.Helpers;
using api.Models;

namespace api.Interfaces;

public interface ICommentRepository
{
  Task<List<Comment>> GetAllAsync(CommentQueryObject queryObject);
  Task<Comment?> GetByIdAsync(int id);
  Task<Comment> CreateAsync(Comment commentModel);
  Task<bool> CommentExists(int id);
  Task<Comment?> UpdateAsync(int id, Comment commentModel);
  Task<Comment?> DeleteAsync(int id);
}
