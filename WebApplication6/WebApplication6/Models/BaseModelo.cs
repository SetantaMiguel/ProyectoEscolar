using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WebApplication6.Models
{
    public class BaseModelo
    {
        public int PaginaActual { get; set; }

        public int TotalRegistros { get; set; }

        public int RegistrosPorPagina { get; set; }

        public string Vista { get; set; }

        public int Controller { get; set; }
        public RouteValueDictionary ValoresQueryString { get; set; }
    }
}