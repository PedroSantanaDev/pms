using pms.app.Models;
using pms.app.UnitOfWork;

namespace pms.app.Seed
{
    public static class ItemSeeder
    {
        public static async Task SeedItemsAsync(IUnitOfWork unitOfWork)
        {
            var items = await unitOfWork.GetRepository<Item>().GetAllAsync();
            if (items != null && items.Count > 0)
            {
                // We already have items, no need to seed
                return;
            }

            var techItemNames = new List<string>
            {
                "Laptop",
                "Smartphone",
                "Tablet",
                "Smartwatch",
                "Headphones",
                "Desktop Computer",
                "Gaming Console",
                "Wireless Earbuds",
                "Fitness Tracker",
                "External Hard Drive",
                "Bluetooth Speaker",
                "Monitor",
                "Camera",
                "VR Headset",
                "Printer",
                "Keyboard",
                "Mouse",
                "Graphics Card",
                "Router",
                "Power Bank",
                "Microphone",
                "USB Flash Drive",
                "Smart TV",
                "Webcam",
                "Game Controller"
            };

            var techItemDescriptions = new List<string>
            {
                "Powerful laptop for work and entertainment",
                "Cutting-edge smartphone with advanced features",
                "Portable tablet for productivity on the go",
                "Sleek smartwatch with fitness tracking capabilities",
                "High-quality headphones for immersive audio experience",
                "Powerful desktop computer for gaming and multimedia",
                "Next-gen gaming console for ultimate gaming experience",
                "Wireless earbuds with noise-canceling technology",
                "Fitness tracker to monitor your health and activity",
                "High-capacity external hard drive for storage",
                "Portable Bluetooth speaker for on-the-go music",
                "High-resolution monitor for crisp visuals",
                "Professional camera for capturing stunning photos",
                "Virtual reality headset for immersive VR experience",
                "High-speed printer for efficient printing",
                "Mechanical keyboard for responsive typing",
                "Ergonomic mouse for comfortable navigation",
                "Dedicated graphics card for gaming and graphics",
                "High-performance router for fast internet connection",
                "Portable power bank for charging devices on the go",
                "Quality microphone for clear audio recording",
                "Portable USB flash drive for data storage",
                "Smart TV with built-in streaming services",
                "HD webcam for video conferencing and streaming",
                "Game controller for gaming consoles and PCs"
            };
            var categories = await unitOfWork.GetRepository<Category>().GetAllAsync();
            var itemsToAdd = new List<Item>();

            DateTime currentDate = DateTime.Now;

            for (int i = 0; i < 25; i++)
            {
                DateTime createdDate = currentDate.AddDays(-7 * i); // Subtract 7 days for each item

                itemsToAdd.Add(new Item
                {
                    SKU = i + 1000, // SKU generation
                    Name = techItemNames[i % techItemNames.Count], // Use modulo to cycle through the tech item names
                    Description = techItemDescriptions[i % techItemDescriptions.Count], // Use modulo to select item description
                    Price = GetRandomPrice(), // Generate random price
                    Status = "Active",
                    Created = createdDate,
                    Category = categories?.Count > 0 ? categories[i] : null,
                    Updated = null,
                });
            }

            await unitOfWork.GetRepository<Item>().AddRangeAsync(itemsToAdd);
        }

        private static decimal GetRandomPrice()
        {
            Random rd = new Random();
            decimal price = 0;
            while (price <= 0) // Ensure the price is greater than 0
            {
                price = Math.Round((decimal)rd.NextDouble() * 1000, 2);
            }
            return price;
        }
    }
}
