using CalculatedPropertyTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace CalculatedPropertyTest.Controllers;

public class SchoolsController : ODataController
{
    private readonly ApplicationDbContext _context;

    public SchoolsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [EnableQuery]
    public IActionResult Get()
    {
        _context.ChangeTracker.QueryTrackingBehavior
            = QueryTrackingBehavior.NoTracking;

        if (!Request.QueryString.Value.Contains("$select") &&
            !Request.QueryString.Value.Contains("$expand"))
        {
            // If the request not contains $select and $expand,
            // we need to include the LastNames manually
            return Ok(_context.Schools.Select(c => new School
            {
                SchoolId = c.SchoolId,
                SchoolName = c.SchoolName,
                LastNames = string.Join(",", c.Students.Select(s => s.LastName)),
                MailAddress = c.MailAddress,
                Students = c.Students
            }));
        }

        // So, If the request contains $select or $expand, the select and expand extension can handle the LastNames
        return Ok(_context.Schools);
    }

    [EnableQuery]
    public IActionResult Get(int key)
    {
        _context.ChangeTracker.QueryTrackingBehavior
            = QueryTrackingBehavior.NoTracking;

        School school = _context.Schools
            .Include(c => c.Students)
            .FirstOrDefault(c => c.SchoolId == key);
        if (school is null)
        {
            return NotFound($"Cannot find the school with Id '{key}'");
        }

        return Ok(school);
    }

    public IActionResult Post([FromBody]School school)
    {
        int maxId = _context.Schools.Max(s => s.SchoolId);
        school.SchoolId = maxId + 1;
        _context.Schools.Add(school);
        _context.SaveChanges();

        return Created(school);
    }

    public IActionResult Patch(int key, Delta<School> delta)
    {
        School school = _context.Schools.FirstOrDefault(s => s.SchoolId == key);
        if (school is null)
        {
            return NotFound($"Cannot find the school with Id '{key}'");
        }

        delta.Patch(school);
        _context.SaveChanges();
        return Updated(school);
    }

    public IActionResult Delete(int key)
    {
        School school = _context.Schools.FirstOrDefault(s => s.SchoolId == key);
        if (school is null)
        {
            return NotFound($"Cannot find the school with Id '{key}'");
        }

        _context.Schools.Remove(school);
        _context.SaveChanges();
        return StatusCode(StatusCodes.Status204NoContent);
    }
}