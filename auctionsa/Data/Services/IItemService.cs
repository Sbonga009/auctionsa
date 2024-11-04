using auctionsa.Models;

namespace auctionsa.Data.Services
{
  public interface IItemService
  {
    IQueryable<Item> GetItems();
    Task<Item> GetItemDetails(int? id);
    Task CreateItemAsync(Item newItem);
    Task AddBidAsync(Bid bid);
  }
}
