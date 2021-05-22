using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Pago
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id_Pago { get; set; }


        [DataType(DataType.Date)]
        public DateTime FechaTransaccion { get; set; }



        public int IdCajero { get; set; }
        

        public Cajero Cajero { get; set; }


        public int IdCaja { get; set; }
        
        public Caja Caja { get; set; }


        public string IdEstudiante { get; set; }
        
        




    }
}