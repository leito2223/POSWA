using OpenHtmlToPdf;
using POSWA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.UI.WebControls;

namespace POSWA.Controllers
{
    public class ProformasController : ApiController
    {
        POSContext db = new POSContext();
        object resp;
        public HttpResponseMessage GetProformas()
        {
            //var proforma = db.EncProforma.Select(d => new
            //{
            //    d.NumProforma,
            //    Cliente = db.Clientes.Where(a => a.CodCliente == d.CodCliente).Select( c => new { c.CodCliente, c.Nombre,c.Email, c.Telefono, c.Direccion}).FirstOrDefault(),
            //    Usuario = db.Usuarios.Where(a => a.CodUsuario == d.CodUsuario).Select( u => new { u.CodUsuario, u.Nombre }).ToList(),
            //    d.FecFactura,
            //    d.SubTotal,
            //    d.Impuesto,
            //    d.Descuento,
            //    d.TipoPago,
            //    DetProform = db.DetProforma.Select(det => new { 
            //        det.Codpro,
            //        det.Nompro,
            //        det.NumLinea,
            //        det.Precio

            //    }).ToList()
            //}
            //).ToList();

            //var EncProforma = db.EncProforma.Select(d => new
            //{

            //        d.NumProforma,
            //        Cliente = db.Clientes.Where(a => a.CodCliente == d.CodCliente).Select( c => new { c.CodCliente, c.Nombre,c.Email, c.Telefono, c.Direccion}).FirstOrDefault(),
            //        Usuario = db.Usuarios.Where(a => a.CodUsuario == d.CodUsuario).Select( u => new { u.CodUsuario, u.Nombre }).ToList(),
            //        d.FecFactura,
            //        d.SubTotal,
            //        d.Impuesto,
            //        d.Descuento,
            //        d.TipoPago,

            //}).ToList();

            var EncVtas = db.EncProforma.ToList();
          
            return Request.CreateResponse(HttpStatusCode.OK, EncVtas);
        }
        [EnableCors("*", "*", "GET"), HttpGet]
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
                DetProform = db.DetProforma.Where(a => a.NumProforma == d.NumProforma).Select(det => new {
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
        [EnableCors("*", "*", "POST"), HttpPost]
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
                        det.Nompro = det.Nompro; //db.Productos.Where(a => a.Codpro == det.Codpro).Select(u => u.Nompro).FirstOrDefault();
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
        [EnableCors("*", "*", "PUT"), HttpPut]
        
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

        [EnableCors("*", "*", "POST"), HttpPost]
        [Route("api/Proformas/EnvioCorreos")]
        public HttpResponseMessage GetCorreos([FromBody] string html, int para)
        {

            try
            {


                // Parametros2 param = db.Parametros2.FirstOrDefault();
                var user = db.Clientes.Where(a => a.CodCliente == para).FirstOrDefault();
                List<Object[]> adjuntos = new List<Object[]>();
                var pdf = Pdf.From(html).WithObjectSetting("web.defaultEncoding", "utf-8").Content();


                MemoryStream newStream = new MemoryStream(pdf);

                newStream.Flush();
                newStream.Position = 0;
                adjuntos.Add(new object[] { (Stream)newStream, "Factura_Proforma.pdf" });
                //  adjuntos.Add(new object[] { pdf, "Liquidacion.pdf" });
                bool respuesta = SendV2(user.Email, "l.arce22@hotmail.com", "", WebConfigurationManager.AppSettings["UserName"], user.Nombre, "Factura Proforma", html, WebConfigurationManager.AppSettings["HostName"], int.Parse(WebConfigurationManager.AppSettings["Port"].ToString()), false, WebConfigurationManager.AppSettings["UserName"], WebConfigurationManager.AppSettings["Password"], adjuntos);

                return Request.CreateResponse(HttpStatusCode.OK, respuesta);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        public bool SendV2(string para, string copia, string copiaOculta, string de, string displayName, string asunto,
       string html, string HostServer, int Puerto, bool EnableSSL, string UserName, string Password, List<Object[]> ArchivosAdjuntos = null)
        {
            try
            {

                MailMessage mail = new MailMessage();
                mail.Subject = asunto;
                mail.Body = html;
                mail.IsBodyHtml = true;
                // * mail.From = new MailAddress(WebConfigurationManager.AppSettings["UserName"], displayName);
                mail.From = new MailAddress(de, displayName);

                var paraList = para.Split(';');
                foreach (var p in paraList)
                {
                    if (p.Trim().Length > 0)
                        mail.To.Add(p.Trim());
                }
                var ccList = copia.Split(';');
                foreach (var cc in ccList)
                {
                    if (cc.Trim().Length > 0)
                        mail.CC.Add(cc.Trim());
                }
                var ccoList = copiaOculta.Split(';');
                foreach (var cco in ccoList)
                {
                    if (cco.Trim().Length > 0)
                        mail.Bcc.Add(cco.Trim());
                }

                if (ArchivosAdjuntos != null)
                {
                    foreach (var archivo in ArchivosAdjuntos)
                    {
                        //  if (!string.IsNullOrEmpty(archivo))
                        mail.Attachments.Add(new Attachment((Stream)archivo[0], archivo[1].ToString()));
                    }
                }
                //if (ArchivosAdjuntos != null)
                //{
                //    foreach (var archivo in ArchivosAdjuntos)
                //    {
                //        //if (!string.IsNullOrEmpty(archivo))
                //        System.Net.Mail.Attachment Data = new System.Net.Mail.Attachment(archivo);
                //        mail.Attachments.Add(Data);
                //    }
                //}


                SmtpClient client = new SmtpClient();
                client.Host = HostServer; // WebConfigurationManager.AppSettings["HostName"];
                client.Port = Puerto; // int.Parse(WebConfigurationManager.AppSettings["Port"].ToString());
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = EnableSSL; // bool.Parse(WebConfigurationManager.AppSettings["EnableSsl"]);
                client.Credentials = new NetworkCredential(UserName, Password);

                client.Send(mail);
                client.Dispose();
                mail.Dispose();

                return true;

            }
            catch (Exception ex)
            {
                return false;
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