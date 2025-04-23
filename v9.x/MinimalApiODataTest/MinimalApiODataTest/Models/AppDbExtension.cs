namespace MinimalApiODataTest.Models
{
    static class AppDbExtension
    {
        public static void MakeSureDbCreated(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDb>();

                if (context.Customers.Count() == 0)
                {
                    #region Customers and Orders

                    var customers = new List<Customer>
                {
                    new Customer { Id = 1, Name = "Alice", FavoriteColor = Color.Red, HomeAddress = new Address { City = "Redmond", Street = "156 AVE NE" },
                        Orders = [
                            new Order { Id = 11, Amount = 9},
                            new Order { Id = 12, Amount = 19},
                        ] },
                    new Customer { Id = 2, Name = "Johnson", FavoriteColor = Color.Blue, HomeAddress = new Address {  City = "Bellevue", Street = "Main St NE" },
                        Orders = [
                            new Order { Id = 21, Amount =8},
                            new Order { Id = 22, Amount = 76},
                        ] },
                    new Customer { Id = 3, Name = "Peter", FavoriteColor = Color.Green, HomeAddress = new Address { City = "Banff", Street = "183 St NE" },
                        Orders = [
                            new Order { Id = 32, Amount = 7 }
                        ] },

                    new Customer { Id = 4, Name = "Sam", FavoriteColor = Color.Red, HomeAddress = new Address { City = "Hollewye", Street = "Main St NE" },
                        Orders = [
                            new Order { Id = 41, Amount = 5 },
                            new Order { Id = 42, Amount = 32}
                        ] }
                };


                    foreach (var s in customers)
                    {
                        context.Customers.Add(s);
                        foreach (var o in s.Orders)
                        {
                            context.Orders.Add(o);
                        }
                    }
                    #endregion

                    context.SaveChanges();
                }
            }
        }
    }
}
