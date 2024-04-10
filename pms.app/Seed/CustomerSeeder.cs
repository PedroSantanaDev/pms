using pms.app.Enums;
using pms.app.Models;
using pms.app.UnitOfWork;

namespace pms.app.Seed
{
    public static class CustomerSeeder
    {
        public static async Task SeedCustomersAsync(IUnitOfWork unitOfWork)
        {

            var customers = await unitOfWork.GetRepository<Customer>().GetAllAsync();
            if (customers != null && customers.Count > 0)
            {
                // We already have customers, no need to seed
                return;
            }

            var customersList = new List<string>
            {
                "Tech Innovations",
                "Digital World",
                "Gadget Emporium",
                "ElectroTech Solutions",
                "Tech Haven",
                "Byte Shack",
                "Nerdy Nexus",
                "Circuit Central",
                "Geek Galaxy",
                "Digital Dreams",
                "Tech Junction",
                "Gizmo Garage",
                "MicroTech Mart",
                "Virtual Ventures",
                "Electro Depot",
                "Cyber Solutions",
                "Pixel Palace",
                "Quantum Quarters",
                "Bit Bazaar",
                "Smart Shop",
                "Tech Oasis",
                "Data Drive",
                "Innovative IT",
                "Code Corner",
                "Logic Lounge"
            };

            var cities = new List<string>
            {
                "New York",
                "Los Angeles",
                "Chicago",
                "Houston",
                "Phoenix",
            };

            var addresses = new List<string>
            {
                "123 Main St",
                "456 Elm St",
                "789 Oak St",
                "321 Pine St",
                "987 Maple St",
            };

            var customersToAdd = new List<Customer>();

            DateTime currentDate = DateTime.Now;

            for (int i = 0; i < 25; i++)
            {
                DateTime createdDate = currentDate.AddDays(-7 * i); // Subtract 7 days for each customer

                // Get random city and address
                string city = cities[i % cities.Count];
                string address = addresses[i % addresses.Count];

                customersToAdd.Add(new Customer
                {
                    Name = customersList[i % customersList.Count], // Use modulo to cycle through the tech store names
                    Email = $"info@{customersList[i % customersList.Count].Replace(" ", "").ToLower()}.com", // Generate email the store name
                    Phone = GenerateRandomPhoneNumber(), // Generate a random number
                    Address = address,
                    City = city,
                    Status = Status.Statuses.Active.ToString(),
                    Created = createdDate,
                    Updated = null
                });
            }

            await unitOfWork.GetRepository<Customer>().AddRangeAsync(customersToAdd);
        }

        private static string GenerateRandomPhoneNumber()
        {
            Random random = new Random();
            // Format: (xxx) xxx-xxxx
            return $"({random.Next(100, 999)}) {random.Next(100, 999)}-{random.Next(1000, 9999)}";
        }
    }
}
