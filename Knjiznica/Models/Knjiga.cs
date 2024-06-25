using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Knjiznica.Models
{
    [Table("Knjiga")]
    public class Knjiga
    {
        [Key]
        [Display(Name = "ID Knjige")]

        public int KnjigaId { get; set; }


        [Required(ErrorMessage = "Naslov knjige je obavezan!")]
        [StringLength(30, MinimumLength =2, ErrorMessage ="Mora biti duljine minimalno 2 i maksimalno 30 znakova ")]
        [Display(Name = "Naslov njige")]
        public string NaslovKnjige { get; set; }

        [Required(ErrorMessage = "Ime autora je obavezno!")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Mora biti duljine minimalno 2 i maksimalno 30 znakova ")]
        [Display(Name = "Ime autora")]
        public string ImeAutora { get; set; }

        public string NaslovIAutor
        {
            get
            {
                return NaslovKnjige + " - " + ImeAutora; 
            }
        }


        [Required(ErrorMessage = "Datum objave knjige je obavezan!")]
        [Display(Name = "Datum objave knjige")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}", ApplyFormatInEditMode =true)]
        public DateTime GodinaObjave { get; set; }

        [Required(ErrorMessage = "Broj stranica je obavezan!")]
        [Display(Name = "Broj stranica")]
        public int BrojStranica { get; set; }

        [Required(ErrorMessage = "Izdavacka kuća je obavezna!")]
        [Display(Name = "Izdavačka Kuća")]
        //[ForeignKey("IzdavackaKucaId")]
        public IzdavackaKuca IzdavackaKucaId { get; set; }
        
        [Display(Name ="Tip knjige")]
        [Column("tipoviKnjiga_sifra")]
        [ForeignKey("ZadaniTipKnjige")]
        public string SifraTip { get; set; }

        public virtual TipKnjige ZadaniTipKnjige { get; set; }
        
        [Display(Name ="Fotografija")]
        [Column("slika")]
        public string SlikaPutanja { get; set; }
        
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}