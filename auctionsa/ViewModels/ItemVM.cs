using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace auctionsa.ViewModels
{
  public class ItemVM
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public List<IFormFile> Images { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsSold { get; set; }
    public string Location { get; set; }
    public string PeriodOfOwnership { get; set; }

    public string IdentityUserId { get; set; }
    [ForeignKey("IdentityUserId")]
    public IdentityUser User { get; set; }
  }
}
