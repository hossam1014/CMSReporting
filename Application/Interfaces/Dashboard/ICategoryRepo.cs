﻿using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Dashboard
{
  public interface ICategoryRepo
    {
        Task<List<IssueCategoryDto>> GetAllCategoriesAsync();
        Task<List<TopCategoryDto>> GetTopReportedCategoriesAsync();

    }
}
