using POSWA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.Http.Cors;

namespace POSWA.Controllers
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*")]
    public class ParametrosController: ApiController
    {
        POSContext db = new POSContext();
        object resp;
        [EnableCors("*", "*", "GET"), HttpGet]
        public HttpResponseMessage GetProducto()
        {


            var parametros = db.Parametros.FirstOrDefault();
            return Request.CreateResponse(HttpStatusCode.OK, parametros);
        }
    }
}