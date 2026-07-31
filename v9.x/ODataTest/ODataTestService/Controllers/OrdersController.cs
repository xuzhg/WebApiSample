using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ODataTestService.Models;

namespace ODataTestService.Controllers;

//[Route("/main/[controller]")]
public class OrdersController : ODataController
{
    private readonly MainContext _db;

    public OrdersController(MainContext db)
    {
        _db = db;
    }

    [EnableQuery]
    [HttpGet("/main/orders")]
    public IQueryable<Order> Get()
    {
        return _db.Orders;
    }
}