using AutoMapper;
using Course.Application.Contracts.Persistence;
using Course.Application.Dto;
using Course.Domain.Entities;
using Course.Infrastructure.Persistence;

namespace Course.Infrastructure.Repositories;

public class CourseRepository : RepositoryBase<Test, TestDto>, ICourseRepository
{
    private readonly CourseContext _context;

    public CourseRepository(
        CourseContext context,
        IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }

    
}