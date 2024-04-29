using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Login.Models
{
    public class TareaCE
    {

        public int Id { get; set; }
        [Required]
        [Display(Name = "Nombre Proyecto")]
        public string NombreProyecto { get; set; }
        [Required]
        [Display(Name = "Nombre Cliente")]
        public string NombreCliente { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        [Display(Name = "Nombre colaborador")]
        public int Colaborador { get; set; }

        public bool Estado { get; set; }


        public string NombreColaborador { get; set; }


    }

    [MetadataType(typeof(TareaCE))]

    public partial class Tarea
    {
        public string NombreColaborador { get; set; }


    }

}