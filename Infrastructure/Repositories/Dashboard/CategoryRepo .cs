using Application.DTOs;
using Application.Interfaces.Dashboard;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Dashboard
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CategoryRepo(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<IssueCategoryDto>> GetAllCategoriesAsync()
        {
            return await _context.IssueCategories
                .Where(c => !c.IsDeleted)
                .Select(c => new IssueCategoryDto
                {
                    Id = c.Id,
                    Key = c.Key,
                    NameAR = c.NameAR,
                    NameEN = c.NameEN
                }).ToListAsync();
        }

        public async Task<List<TopCategoryDto>> GetTopReportedCategoriesAsync()
        {
            return await _context.IssueReports
                .Include(r => r.IssueCategory)
                .Where(r => !r.IsDeleted)
                .GroupBy(r => r.IssueCategory.Key)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => new TopCategoryDto
                {
                    // Category = g.Key,
                    CategoryAR = g.FirstOrDefault().IssueCategory.NameAR,
                    CategoryEN = g.FirstOrDefault().IssueCategory.NameEN,
                    Count = g.Count()
                })
                .ToListAsync();
        }
    }
}
