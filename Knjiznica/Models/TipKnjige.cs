using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Knjiznica.Models
{
    [Table("tipoviKnjiga")]
    public class TipKnjige
    {
        [Key]
        [Display(Name = "Šifra")]
        [Required(ErrorMessage = "{0} je obavezna")]
        [StringLength(10, ErrorMessage ="{0} mora biti maksimalno duljine {1} znakova")]
        public string sifra { get; set; }

        [Display(Name = "Naziv")]
        [Required(ErrorMessage = "{0} je obavezan")]
        [StringLength(10, ErrorMessage = "{0} mora biti maksimalno duljine {1} znakova")]
        public string naziv { get; set; }


    }
}