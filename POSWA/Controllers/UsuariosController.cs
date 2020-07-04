using POSWA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace POSWA.Controllers
{
    public class UsuariosController: ApiController
    {
        POSContext db = new POSContext();

        public HttpResponseMessage GetUsuarios()
        {
          var user =  db.Usuarios.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        public HttpResponseMessage GetOneUsuario(string email, string password)
        {
            var user = db.Usuarios.Where(a => a.Nombre == email).FirstOrDefault();
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

    }
}