using POSWA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace POSWA.Controllers
{
    public class ProformasController : ApiController
    {
        POSContext db = new POSContext();
        object resp;
        public HttpResponseMessage GetProformas()
        {
            var proforma = db.EncProforma.Select(d => new
            {
                d.NumProforma,
                Cliente = db.Clientes.Where(a => a.CodCliente == d.CodCliente).Select( c => new { c.CodCliente, c.Nombre,c.Email, c.Telefono, c.Direccion}).FirstOrDefault(),
                Usuario = db.Usuarios.Where(a => a.CodUsuario == d.CodUsuario).Select( u => new { u.CodUsuario, u.Nombre }).ToList(),
                d.FecFactura,
                d.SubTotal,
                d.Impuesto,
                d.Descuento,
                d.TipoPago,
                DetProform = db.DetProforma.Select(det => new { 
                    det.Codpro,
                    det.Nompro,
                    det.NumLinea,
                    det.Precio
                    
                }).ToList()
            }

            ).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, proforma);
        }

        public IHttpActionResult GetOneProforma(string id)
        {
            var proforma = db.EncProforma.Where(p => p.NumProforma == id).Select(d => new
            {
                d.NumProforma,
                Cliente = db.Clientes.Where(a => a.CodCliente == d.CodCliente).Select(c => new { c.CodCliente, c.Nombre, c.Email, c.Telefono, c.Direccion }).FirstOrDefault(),
                Usuario = db.Usuarios.Where(a => a.CodUsuario == d.CodUsuario).Select(u => new { u.CodUsuario, u.Nombre }).ToList(),
                d.FecFactura,
                d.SubTotal,
                d.Impuesto,
                d.Descuento,
                d.TipoPago,
                DetProform = db.DetProforma.Select(det => new {
                    det.Codpro,
                    det.Nompro,
                    det.NumLinea,
                    det.Precio

                }).ToList()
            }

           ).FirstOrDefault();

            if (proforma == null)
            {
                return NotFound();
            }



            var obj = new
            {
                proforma
            };
            return Ok(proforma);
        }
        public async Task<HttpResponseMessage> PostAsync([FromBody] Proforma proforma)
        {
                    var transaccion = db.Database.BeginTransaction();
            try
            {
                EncProforma Consulta = db.EncProforma.Where(a => a.NumProforma == proforma.encProforma.NumProforma).FirstOrDefault();

                if (Consulta == null)
                {





                    proforma.encProforma.FecFactura = DateTime.Now;

                    db.EncProforma.Add(proforma.encProforma);

                    await db.SaveChangesAsync();

                    foreach(var det in proforma.detProforma)
                    {
                        det.NumProforma = proforma.encProforma.NumProforma;
                        det.Nompro = db.Productos.Where(a => a.Codpro == det.Codpro).Select(u => u.Nompro).FirstOrDefault();
                        db.DetProforma.Add(det);
                        db.SaveChanges();

                    }
                    transaccion.Commit();
                    
                    resp = new
                    {
                        Success = true
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, resp);
                }
                else
                {
                    throw new Exception("Producto ya existe");
                }
            }
            catch (Exception ex)
            {
                    transaccion.Rollback();
                // EncuestasRegistroController.GuardarBitacoraTxt(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        // [Route("api/Clientes/CambiarClave")]
        [HttpPut]
        public async Task<HttpResponseMessage> PutAsync([FromBody] Proforma cambio)
        {
            var transaccion = db.Database.BeginTransaction();
            try
            {
                if (cambio.encProforma.NumProforma == null || cambio.encProforma.NumProforma == "")
                    throw new Exception("Proforma no existe");

                EncProforma pc = db.EncProforma.Where(a => a.NumProforma == cambio.encProforma.NumProforma).FirstOrDefault();
                if (pc == null)
                    throw new Exception("Proforma no existe");


                db.Entry(pc).State = EntityState.Modified;

                pc.CodCliente = cambio.encProforma.CodCliente;
                pc.CodUsuario = pc.CodUsuario;
                pc.FecFactura = DateTime.Now;
                pc.SubTotal = cambio.encProforma.SubTotal;
                pc.Descuento = cambio.encProforma.Descuento;
                pc.Impuesto = cambio.encProforma.Impuesto;

                var dt = db.DetProforma.Where(a => a.NumProforma == pc.NumProforma).ToList();


                foreach(var item in dt )
                {
                    db.DetProforma.Remove(item);
                    await db.SaveChangesAsync();
                }


                foreach (var det in cambio.detProforma)
                {
                    det.NumProforma = cambio.encProforma.NumProforma;
                    det.Nompro = db.Productos.Where(a => a.Codpro == det.Codpro).Select(u => u.Nompro).FirstOrDefault();
                    db.DetProforma.Add(det);
                    db.SaveChanges();

                }
                db.SaveChanges();

                resp = new
                {
                    Success = true
                };
                transaccion.Commit();

                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                transaccion.Rollback();
                // EncuestasRegistroController.GuardarBitacoraTxt(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        //[HttpDelete]// [Route("api/Productos/Eliminar")]

        //public async Task<IHttpActionResult> Eliminar(string id)
        //{
        //    Productos cliente = db.Productos.Where(a => a.Codpro == id).FirstOrDefault();
        //    if (cliente == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Productos.Remove(cliente);


        //    await db.SaveChangesAsync();

        //    return Ok("OK");
        //}
    }
}