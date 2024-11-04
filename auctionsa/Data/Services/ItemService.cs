using auctionsa.Models;
using Microsoft.EntityFrameworkCore;

namespace auctionsa.Data.Services
{
  public class ItemService : IItemService
  {
    private readonly ApplicationDbContext _context;

    public ItemService(ApplicationDbContext context)
    {
      _context = context;
    }

    public IQueryable<Item> GetItems()
    {
      var items = _context.Items.Include(i => i.User);
      return items;
    }

    public async Task<Item> GetItemDetails(int? id)
    {
      var item = await _context.Items
        .Include(i => i.User)
        .Include(i => i.Bids)
        .ThenInclude(c => c.User)
        .SingleOrDefaultAsync(i => i.Id == id);

      return item!;
    }

    public async Task CreateItemAsync(Item newItem)
    {
      _context.Items.Add(newItem);
      await _context.SaveChangesAsync();
    }

    public async Task AddBidAsync(Bid bid)
    {
      _context.Bids.Add(bid);
      await _context.SaveChangesAsync();
    }

  }
}
