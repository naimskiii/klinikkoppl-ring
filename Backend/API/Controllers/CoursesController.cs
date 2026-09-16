namespace API.Controllers;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
public class CoursesController : BaseController
{
    private readonly StoreContext _context;

    public CoursesController(StoreContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Course>>> GetCourses()
    {
         return  await _context.Courses.ToListAsync();
    
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Course>> GetCourse(Guid id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
        {
            return NotFound();
        }
        return Ok(course);
    }
}
