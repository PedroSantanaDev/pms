using pms.app.Models;
using pms.app.UnitOfWork;

namespace pms.app.Seed
{
    public static class CustomerItemSeeder
    {
        public static async Task SeedCustomerItemsAsync(IUnitOfWork unitOfWork)
        {
            var customerItems = await unitOfWork.GetRepository<CustomerItem>().GetAllAsync();
            if (customerItems != null && customerItems.Count > 0)
            {
                // We already have customer items, no need to seed
                return;
            }

            // Get customers and items
            var customers = await unitOfWork.GetRepository<Customer>().GetAllAsync();
            var items = await unitOfWork.GetRepository<Item>().GetAllAsync();

            // If there are no customers or items, just return
            if (customers == null || customers.Count == 0 || items == null || items.Count == 0)
            {
                return;
            }

            var random = new Random();

            DateTime currentDate = DateTime.Now;

            var counter = 0;
            foreach (var customer in customers)
            {
                var customerItemsToAdd = new List<CustomerItem>();

                DateTime assignedDate = currentDate.AddDays(-7 * counter); // Subtract 7 days for each item

                // Choose a random number of items to assign to the customer
                int itemCount = random.Next(1, 6); // Choose a random number between 1 and 5

                // Shuffle the items to assign random ones
                var shuffledItems = items.OrderBy(x => random.Next()).ToList();

                // Select the first itemCount items to assign to the customer
                var itemsToAssign = shuffledItems.Take(itemCount);

                foreach (var item in itemsToAssign)
                {
                    // Create a new CustomerItem object and add it to the list
                    customerItemsToAdd.Add(new CustomerItem
                    {
                        CustomerId = customer.Id,
                        Customer = customer,
                        ItemId = item.Id,
                        Item = item,
                        Quantity = random.Next(1, 11), // Choose quantity between 1 and 10
                        AssignedDate = assignedDate
                    });
                }
                customer.CustomerItems = customerItemsToAdd;

                await unitOfWork.GetRepository<Customer>().UpdateAsync(customer);
                counter++;
            }
        }
    }
}
