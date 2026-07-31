using System;

namespace ODataTestService.Models;

public class Order
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreatedAt { get; set; }
}