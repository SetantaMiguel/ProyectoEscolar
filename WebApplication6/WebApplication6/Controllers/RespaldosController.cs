using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data.SqlClient;

namespace WebApplication6.Controllers
{
    public class RespaldosController : Controller
    {
        // GET: Respaldos
        public ActionResult Index()
        {
            return View();
        }

        public void Backup_Click(object sender, EventArgs e)
        {


            SqlConnection sqlconn = new SqlConnection("Data Source=.;Initial Catalog=SistemaAsC;Integrated Security=True;Pooling=False");
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            // Backup destibation
            string backupDestination = "C:\\Program Files\\Microsoft SQL Server\\MSSQL14.MSSQLSERVER\\MSSQL\\Backup";
            // check if backup folder exist, otherwise create it.

            try
            {
                sqlconn.Open();
                sqlcmd = new SqlCommand("BACKUP DATABASE SistemaAsC TO DISK = 'C:\\Program Files\\Microsoft SQL Server\\MSSQL14.MSSQLSERVER\\MSSQL\\Backup\\Backup1.bak';", sqlconn);
                sqlcmd.ExecuteNonQuery();
                //Close connection
                sqlconn.Close();
                Response.Write("Backup creado Exitosamente");
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        public void Backup_Diferencial(object sender, EventArgs e)
        {


            SqlConnection sqlconn = new SqlConnection("Data Source=.;Initial Catalog=SistemaAsC;Integrated Security=True;Pooling=False");
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            // Backup destibation
            string backupDestination = "C:\\Program Files\\Microsoft SQL Server\\MSSQL14.MSSQLSERVER\\MSSQL\\Backup";
            // check if backup folder exist, otherwise create it.

            try
            {
                sqlconn.Open();
                sqlcmd = new SqlCommand("BACKUP DATABASE SistemaAsC TO DISK = 'C:\\Program Files\\Microsoft SQL Server\\MSSQL14.MSSQLSERVER\\MSSQL\\Backup\\BackupDiferencial.bak' WITH DIFFERENTIAL;", sqlconn);
                sqlcmd.ExecuteNonQuery();
                //Close connection
                sqlconn.Close();
                Response.Write("Backup Diferencial Creado Exitosamente");
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        public void Backup_Log(object sender, EventArgs e)
        {


            SqlConnection sqlconn = new SqlConnection("Data Source=.;Initial Catalog=SistemaAsC;Integrated Security=True;Pooling=False");
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            // Backup destibation
            string backupDestination = "C:\\Program Files\\Microsoft SQL Server\\MSSQL14.MSSQLSERVER\\MSSQL\\Backup";
            // check if backup folder exist, otherwise create it.

            try
            {
                sqlconn.Open();
                sqlcmd = new SqlCommand("BACKUP LOG SistemaAsC TO DISK = 'C:\\Program Files\\Microsoft SQL Server\\MSSQL14.MSSQLSERVER\\MSSQL\\Backup\\BackupLog.log'; ", sqlconn);
                sqlcmd.ExecuteNonQuery();
                //Close connection
                sqlconn.Close();
                Response.Write("Log de Transacciones Creado Exitosamente");
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}