﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Login.Models
{
    public class UsuarioCE
    {

        public int Id { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public string ConfirmarContraseña { get; set; }
    }
}