using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using BKProjekt.Areas.Identity.Data;

namespace BKProjekt.Models
{
    public class Borrow
    {
        public int Id { get; set; }
        public int BookId {  get; set; }
        public string UserId { get; set; }
        [Display(Name = "Czytelnik")]
        public string UserName { get; set; }
        
        [Required]
        [Display(Name = "Data Wypożyczenia")]
        public DateTime BorrowDate { get; set; }
        [Display(Name = "Data Oddania")]
        public DateTime? ReturnDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        // Navigation properties
        [ForeignKey("BookId")]
        public Book? Book { get; set; }
        [Display(Name ="Tytuł")]
        public string? Title { get; set; }

        public ICollection<Book>? Books { get; set; }
    }
}
