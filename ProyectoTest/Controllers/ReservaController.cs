﻿using ProyectoTest.Logica;
using ProyectoTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProyectoTest.Controllers
{
    public class ReservaController : Controller
    {
        private static Usuario oUsuario;
        //VISTA
        //public ActionResult Index()
        //{
        //    if (Session["Usuario"] == null)
        //        return RedirectToAction("Index", "Login");
        //    else
        //        oUsuario = (Usuario)Session["Usuario"];

        //    return View();
        //}

        //VISTA
        public ActionResult Reserva()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");
            else
                oUsuario = (Usuario)Session["Usuario"];            

            return View();
        }        


    }

}