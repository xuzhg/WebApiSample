using Microsoft.AspNetCore.Mvc;

namespace Question78956264AllowDeepNavigation.Controllers;

public class PeopleController : Controller
{
    public IActionResult List(string ssn)
    {
        return Ok(ssn);
    }

    public IActionResult Get(string odataPath)
    {
        return Ok(odataPath);
    }
}
