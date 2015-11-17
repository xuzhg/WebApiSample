using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWithEFSample
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        private static void BuildDataBase()
        {
            WebApiWithEfContext db = new WebApiWithEfContext();
            if (db.People.Any())
            {
                return;
            }

            var people = Enumerable.Range(1, 5).Select(e => new Person
            {
                Id = e,
                Name = "Person_" + e,
                Address = new Address
                {
                    City = "City_" + e
                }
            });

            foreach (var person in people)
            {
                db.People.Add(person);
            }

            db.SaveChanges();
        }
    }
}
