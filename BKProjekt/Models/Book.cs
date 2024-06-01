using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace BKProjekt.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Display(Name = "Tytuł")]
        [Required]
        [StringLength(50, ErrorMessage = "Tytuł nie może być dłuższy niż 50 znaków.", MinimumLength = 1)]
        public string? Title { get; set; }
        [Display(Name = "Krótki opis")]
        [Required]
        [StringLength(500, ErrorMessage = "Opis nie może być dłuższy niż 500 znaków.", MinimumLength = 1)]
        public string? Description { get; set; }
        [Display(Name = "Nazwisko autora")]
        [Required]
        [StringLength(50, ErrorMessage = "Nazwisko autora nie może być dłuższe niż 50 znaków.", MinimumLength = 1)]
        public string? Author { get; set; }
        [Display(Name = "Ilość stron")]
        [Required]
        [Range(1, 5000, ErrorMessage = "Wpisana wartość wykracza poza doswoloną ilość stron (5000)")]
        public int TotalPages { get; set; }

        [Display(Name = "Czy jest wypożyczona?")]
        public string Status { get; set; } = "Nie";

        public ICollection<Borrow>? Borrows { get; set; }
    }
    
}
