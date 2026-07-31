using Default;

Console.WriteLine("Hello, World!");

var db = new Container(new Uri("http://localhost:5084/main"));

var amount = db.Orders.Sum(x => x.Amount);

Console.WriteLine("amount=" + amount);