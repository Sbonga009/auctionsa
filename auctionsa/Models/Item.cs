using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace auctionsa.Models
{
  public class Item
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Images { get; set; }
    public double Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsSold { get; set; }
    public string Location { get; set; }
    public string PeriodOfOwnership { get; set; }

    public string IdentityUserId { get; set; }
    [ForeignKey("IdentityUserId")]
    public IdentityUser User { get; set; }

    public List<Bid> Bids { get; set; }
  }

  public class Bid
  {
    public int Id { get; set; }
    public double Price { get; set; }
    public string? Comment { get; set; }
    public string IdentityUserId { get; set; }
    [ForeignKey("IdentityUserId")]
    public IdentityUser User { get; set; }
    public int? ItemId { get; set; }
    [ForeignKey("ItemId")]
    public Item Item { get; set; }
  }

}
