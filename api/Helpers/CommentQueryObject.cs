﻿using System;

namespace api.Helpers;

public class CommentQueryObject
{
  public string symbol { get; set; }
  public bool IsDescending { get; set; } = true;
}
