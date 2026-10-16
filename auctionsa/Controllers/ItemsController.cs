using auctionsa.Data.Services;
using auctionsa.Models;
using auctionsa.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace auctionsa.Controllers
{
  public class ItemsController : Controller
  {
    private readonly IItemService _iservice;
    private readonly ImageUploadService _imgservice;
    private IWebHostEnvironment _env;
    private readonly UserManager<IdentityUser> _userManager;

    public ItemsController(IItemService service, IWebHostEnvironment env, UserManager<IdentityUser> userManager, ImageUploadService imgservice)
    {
      _iservice = service;
      _env = env;
      _userManager = userManager;
      _imgservice = imgservice;
    }

    public async Task<IActionResult> Index()
    {
      var items = _iservice.GetItems();
      return View(await items.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var item = await _iservice.GetItemDetails(id);
      if (item == null)
      {
        return NotFound();
      }

      var user = await _userManager.GetUserAsync(User);
      if (user != null)
      {
        ViewBag.CurrentUserId = user.Id;
      }

      return View(item);
    }

    public IActionResult Create()
    {
      return View();
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ItemVM newItem)
    {
      if (newItem.Images != null && newItem.Images.Count > 0)
      {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
          ModelState.AddModelError("", "User not found.");
          return View(newItem);
        }

        var imagePaths = await _imgservice.SaveItemImagesAsync(user.Email, newItem.Name, newItem.Images);

        var item = new Item
        {
          Name = newItem.Name,
          Description = newItem.Description,
          Price = newItem.Price,
          Location = newItem.Location,
          PeriodOfOwnership = newItem.PeriodOfOwnership,
          Images = imagePaths,
          IsSold = false,
          StartDate = DateTime.Now,
          EndDate = DateTime.Now.AddHours(48),
          IdentityUserId = user.Id
        };

        await _iservice.CreateItemAsync(item);

        return RedirectToAction(nameof(Index));
      }

      return View(newItem);
    }


    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceBid(int itemId, double bidAmount, string comment)
    {
      var item = await _iservice.GetItemDetails(itemId);
      if (item == null)
      {
        return NotFound();
      }

      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        ModelState.AddModelError("", "User not found.");
        return View("Details", item);
      }
      ViewBag.CurrentUserId = user.Id;

      if (DateTime.Now >= item.EndDate)
      {
        ModelState.AddModelError("", "This auction has ended.");
        return View("Details", item);
      }

      if (item.IdentityUserId == user.Id)
      {
        ModelState.AddModelError("", "You cannot bid on your own item.");
        return View("Details", item);
      }

      var highestBid = item.Bids.Any() ? item.Bids.Max(b => b.Price) : item.Price;
      if (bidAmount <= highestBid)
      {
        ModelState.AddModelError("", "Bid must be higher than the current highest bid.");
        return View("Details", item);
      }

      var bid = new Bid
      {
        ItemId = itemId,
        Price = bidAmount,
        Comment = comment,
        IdentityUserId = user.Id
      };

      await _iservice.AddBidAsync(bid);
      return RedirectToAction("Details", new { id = itemId });
    }

    // GET: Items/Edit/5
    //public async Task<IActionResult> Edit(int? id)
    //{
    //  if (id == null)
    //  {
    //    return NotFound();
    //  }

    //  var item = await _context.Items.FindAsync(id);
    //  if (item == null)
    //  {
    //    return NotFound();
    //  }
    //  ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", item.IdentityUserId);
    //  return View(item);
    //}

    // POST: Items/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Images,Duration,IsSold,Location,PeriodOfOwnership,IdentityUserId")] Item item)
    //{
    //  if (id != item.Id)
    //  {
    //    return NotFound();
    //  }

    //  if (ModelState.IsValid)
    //  {
    //    try
    //    {
    //      _context.Update(item);
    //      await _context.SaveChangesAsync();
    //    }
    //    catch (DbUpdateConcurrencyException)
    //    {
    //      if (!ItemExists(item.Id))
    //      {
    //        return NotFound();
    //      }
    //      else
    //      {
    //        throw;
    //      }
    //    }
    //    return RedirectToAction(nameof(Index));
    //  }
    //  ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", item.IdentityUserId);
    //  return View(item);
    //}

    // GET: Items/Delete/5
    //public async Task<IActionResult> Delete(int? id)
    //{
    //  if (id == null)
    //  {
    //    return NotFound();
    //  }

    //  var item = await _context.Items
    //      .Include(i => i.User)
    //      .FirstOrDefaultAsync(m => m.Id == id);
    //  if (item == null)
    //  {
    //    return NotFound();
    //  }

    //  return View(item);
    //}

    // POST: Items/Delete/5
    //[HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> DeleteConfirmed(int id)
    //{
    //  var item = await _context.Items.FindAsync(id);
    //  if (item != null)
    //  {
    //    _context.Items.Remove(item);
    //  }

    //  await _context.SaveChangesAsync();
    //  return RedirectToAction(nameof(Index));
    //}

    //private bool ItemExists(int id)
    //{
    //  return _context.Items.Any(e => e.Id == id);
    //}
  }
}
