using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Login.Permisos;
using Login.Models;
using System.Data;
using System.Web.WebSockets;
using System.Diagnostics.CodeAnalysis;

namespace Login.Controllers
{

    [ValidarSesionAtribute]

    public class HomeController : Controller
    {



        public ActionResult Usuario()
        {
            try
            {
                using (var db = new DB_ACCESOEntities())
                {
                    return PartialView(db.Usuario.ToList());
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Tarea()
        {
            using (var db = new DB_ACCESOEntities())
            {
                var data = from a in db.Tarea
                           join c in db.Usuario on a.Colaborador equals c.Id
                           select new TareaCE
                           {
                               Id = a.Id,
                               NombreProyecto = a.NombreProyecto,
                               NombreCliente = a.NombreCliente,
                               Descripcion = a.Descripcion,
                               NombreColaborador = c.Nombre + " " + c.Apellido,
                               Estado = a.Estado.Equals(false)
                           };
                ViewBag.Mensaje = TempData["Mensaje"] as string;
                ViewBag.Error = TempData["Error"] as string;
                return View(data.ToList());
            }

        }

        public ActionResult AgregarTarea()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarTarea(Tarea t)
        {

            try
            {
                using (var db = new DB_ACCESOEntities())
                {
                    db.Tarea.Add(t);
                    db.SaveChanges();
                    TempData["Mensaje"] = "Tarea agregada exitosamente";
                    return RedirectToAction("Tarea");

                }


            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Error al registrar Tarea -" + ex.Message);

                return View();
            }
        }

        public ActionResult Editar(int id)
        {
            try
            {
                using (var db = new DB_ACCESOEntities())
                {
                    Tarea tarea = db.Tarea.Find(id);
                    return View(tarea);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Editar(Tarea t)
        {
            try
            {
                using (var db = new DB_ACCESOEntities())
                {
                    Tarea tarea = db.Tarea.Find(t.Id);
                    tarea.NombreProyecto = t.NombreProyecto;
                    tarea.NombreCliente = t.NombreCliente;
                    tarea.Descripcion = t.Descripcion;
                    tarea.NombreColaborador = t.NombreColaborador;

                    db.SaveChanges();

                    return RedirectToAction("Tarea");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Eliminar(int id)
        {
            try
            {
                using(var db = new DB_ACCESOEntities())
                {
                    Tarea t = db.Tarea.Find(id);
                    db.Tarea.Remove(t);
                    db.SaveChanges();
                    TempData["Mensaje"] = "Tarea eliminada exitosamente";
                    return RedirectToAction("tarea");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Integrantes()
        {
            using (var db = new DB_ACCESOEntities())
            {
                var data = from a in db.Usuario
                           select new UsuarioCE
                           {
                               Id = a.Id,
                               Correo = a.Correo,
                               Nombre = a.Nombre,
                               Apellido = a.Apellido,
                           };
                ViewBag.Mensaje = TempData["Mensaje"] as string;
                ViewBag.Error = TempData["Error"] as string;
                return View(data.ToList());
            }

        }


        public ActionResult EliminarUsuario(int id)
        {
            try
            {
                using (var db = new DB_ACCESOEntities())
                {
                    Usuario u = db.Usuario.Find(id);
                    db.Usuario.Remove(u);
                    db.SaveChanges();
                    TempData["Mensaje"] = "Usuario eliminado exitosamente";
                    return RedirectToAction("Integrantes");
                }


            }
            catch (Exception)
            {

                TempData["Error"] = "Error al eliminar usuario, el usuario pertenece a un proyecto";

                return RedirectToAction("Integrantes");

            }
        }

        public ActionResult EstadoTarea(int id)
        {
            try
            {
                using (var db = new DB_ACCESOEntities())
                {
                    Tarea tarea = db.Tarea.Find(id);
                    if(tarea.Estado == true)
                    {
                        tarea.Estado= false;
                    }
                    else
                    {
                        tarea.Estado = true;
                    }


                    db.SaveChanges();

                    return RedirectToAction("Tarea");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }






























        public ActionResult CerrarSesion()
        {
            Session["usuario"] = null;
            return RedirectToAction("Login", "Acceso");
        }
    }
}