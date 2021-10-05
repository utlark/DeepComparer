using System;

namespace DeepComparer
{
    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
    }
    class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
        public double House { get; set; }
    }

    class Program
    {
        static void Main()
        {
            var first =
                new Person
                {
                    FirstName = "Иван",
                    LastName = "Иванов",
                    Address = new Address
                    {
                        City = "Екатеринбург",
                        Street = "Ленина",
                        House = 1
                    }
                };

            var second =
                new Person
                {
                    FirstName = "Иван",
                    LastName = "Сидоров",
                    Address = new Address
                    {
                        City = "Екатеринбург",
                        Street = "Малышева",
                        House = 4
                    }
                };

            var comparer = new DeepComparer();
            var differences = comparer.Compare<Person>(first, second);

            foreach (var item in differences)
                Console.WriteLine(item);

            Console.ReadLine();
        }
    }
}
