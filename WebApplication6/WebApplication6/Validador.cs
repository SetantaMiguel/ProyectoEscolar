using Microsoft.AspNet.Identity;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;
using WebApplication6.viewModels;

namespace WebApplication6
{
    public class Validador
    {
        public class BaseRespuesta
        {
            public bool Ok { get; set; }

            public string Mensaje { get; set; }
        }

        public static bool PuedeEntrar(string id,string NombrePermiso)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            bool res = false;
            var usuario = db.Users.Find(id);
            var UserIdRol = usuario.Roles.FirstOrDefault().RoleId;
            var permisos = db.tbpermisosyroles.Include(x => x.Permisos).Where(x => x.Id_Rol == UserIdRol);
            foreach (var item in permisos.ToList())
            {
                if (item.Permisos.NombrePermiso==NombrePermiso)
                {
                    res = true;
                    return res;
                }
            }
            return res;
        }



        public static bool PuedeEntrar2(object permisos, string NombrePermiso)
        {
            //Variable Retorno
            bool res = false;
            var list = (IQueryable<PermisosYRoles>)permisos;
            if (permisos==null)
            {
                return false;
            }
            foreach (var item in list)
            {
                if (item.Permisos.NombrePermiso==NombrePermiso)
                {
                    res = true;
                    return res;
                }
            }
            return res;
        }

    }
}