using pms.app.Models;
using pms.app.UnitOfWork;

namespace pms.app.Seed
{
    public static class CategorySeeder
    {
        public static async Task SeedCategoriesAsync(IUnitOfWork unitOfWork)
        {
            var categories = await unitOfWork.GetRepository<Category>().GetAllAsync();
            if (categories != null && categories.Count > 0)
            {
                // We already have categories, no need to seed
                return;
            }


            var categoryNames = new List<string>
            {
                "Laptops", "Smartphones", "Tablets", "Desktop PCs", "Computer Accessories",
                "Gaming Consoles", "Smartwatches", "Wireless Headphones", "Bluetooth Speakers",
                "Home Automation", "Drones", "Camera Gear", "Network Devices", "Wearables",
                "Virtual Reality", "Augmented Reality", "Software", "Storage Devices",
                "Monitors", "Printers", "Scanners", "Computer Components", "Cables & Adapters",
                "Office Equipment", "Tech Gadgets"
            };


            var categoryDescriptions = new List<string>
            {
                "Explore a wide range of laptops suitable for work, gaming, and everyday use.",
                "Discover the latest smartphones with cutting-edge features and stunning designs.",
                "Find the perfect tablet for productivity, entertainment, and creativity.",
                "Build or upgrade your desktop PC with high-performance components and accessories.",
                "Enhance your computing experience with essential accessories like keyboards, mice, and more.",
                "Experience immersive gaming with the latest gaming consoles and accessories.",
                "Stay connected and organized with stylish and feature-rich smartwatches.",
                "Enjoy wireless freedom and superior audio quality with our selection of wireless headphones.",
                "Listen to music anytime, anywhere with our range of Bluetooth speakers.",
                "Make your home smarter and more convenient with our home automation products.",
                "Capture stunning aerial footage and explore the skies with our drone collection.",
                "Level up your photography and videography game with professional camera gear.",
                "Stay connected and secure with our range of network devices and accessories.",
                "Stay active and monitor your health with our selection of wearables and fitness trackers.",
                "Immerse yourself in virtual worlds with our virtual reality headsets and accessories.",
                "Enhance your reality with augmented reality devices and applications.",
                "Boost productivity and creativity with our selection of software solutions.",
                "Expand your digital storage capacity with our range of storage devices.",
                "Experience crystal-clear visuals with our high-quality monitors.",
                "Print, scan, and copy documents with ease using our printers and scanners.",
                "Digitize documents and images with our selection of reliable scanners.",
                "Build or upgrade your PC with quality computer components and accessories.",
                "Connect your devices seamlessly with our high-quality cables and adapters.",
                "Equip your office with essential equipment for maximum efficiency.",
                "Discover a variety of tech gadgets and accessories to enhance your daily life."
            };

            var categoriesToAdd = new List<Category>();
            // Start with the current date and time
            DateTime currentDate = DateTime.Now;

            for (int i = 0; i < categoryNames.Count; i++)
            {
                // Decrement the current date by a certain number of days for each category
                DateTime createdDate = currentDate.AddDays(-7 * i); // Subtract 7 days for each category

                categoriesToAdd.Add(new Category
                {
                    Name = categoryNames[i],
                    Description = categoryDescriptions[i],
                    Created = createdDate, // Use the generated created date
                });
            }

            await unitOfWork.GetRepository<Category>().AddRangeAsync(categoriesToAdd);
        }
    }
}
