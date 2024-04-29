using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Login.Models;

namespace Login.Controllers
{
    public class AccesoController : Controller
    {

        static string cadena = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=DB_ACCESO;Integrated Security=true";



        // GET: Acceso
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Usuario u)
        {
            bool resgitrado;
            string mensaje;

            if (u.Contraseña == u.ConfirmarContraseña)
            {
                u.Contraseña = ConvertirSha256(u.Contraseña);
            }
            else
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }


            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", cn);
                cmd.Parameters.AddWithValue("Correo", u.Correo);
                cmd.Parameters.AddWithValue("Contraseña", u.Contraseña);
                cmd.Parameters.AddWithValue("Nombre", u.Nombre);
                cmd.Parameters.AddWithValue("Apellido", u.Apellido);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("Registrado", SqlDbType.Bit)
                .Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100)
                .Direction = ParameterDirection.Output;

                cn.Open();
                cmd.ExecuteNonQuery();


                resgitrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            }

            ViewData["Mensaje"] = mensaje;

            if (resgitrado)
            {
                return RedirectToAction("Login", "Acceso");

            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(Usuario u)
        {


            u.Contraseña = ConvertirSha256(u.Contraseña);

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ValidarUsuario", cn);
                cmd.Parameters.AddWithValue("Correo", u.Correo);
                cmd.Parameters.AddWithValue("Contraseña", u.Contraseña);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                u.Id = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            }



            if (u.Id != 0)
            {
                Session["usuario"] = u;
                return RedirectToAction("Tarea", "Home");
            }
            else
            {
                ViewData["Mensaje"] = "Email o Contraseña incorrecta ";
                return View();
            }


        }






        public static string ConvertirSha256(string texto)
        {

            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }



    }
}