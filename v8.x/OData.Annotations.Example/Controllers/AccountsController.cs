using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace OData.Annotations.Example.Controllers
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OwnerId { get; set; }
        public string OwnerIdName { get; set; }
    }

    public class AccountsController() : ODataController
    {
        private static readonly IList<Account> accounts =
        [
            new() { Id = Guid.NewGuid(), Name = "Interstellar Mining Corp", OwnerId = Guid.NewGuid(), OwnerIdName = "User 1" },
            new() { Id = Guid.NewGuid(), Name = "Quantum Communications", OwnerId = Guid.NewGuid(), OwnerIdName = "User 2" },
            new() { Id = Guid.NewGuid(), Name = "Galaxy Graphics", OwnerId = Guid.NewGuid(), OwnerIdName = "User 3" },
            new() { Id = Guid.NewGuid(), Name = "Sam Null test", OwnerId = Guid.NewGuid(), OwnerIdName = null }
        ];

        [HttpGet]
        [EnableQuery]
        public IEnumerable<Account> Get()
        {
            return accounts;
        }
    }
}
