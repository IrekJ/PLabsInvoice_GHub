using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PLabsInvoice.DAL;
using PLabsInvoice.Models;



namespace PLabsInvoice.Controllers.API
{
    public class InvoicesController : ApiController
    {

        private InvoiceContext db = new InvoiceContext();

        // GET: api/Invoices
        public IQueryable<Invoice> GetInvoices()
        {
            Dictionary<string, string> parameteters = Request.GetQueryNameValuePairs().ToDictionary(q => q.Key, q => q.Value);
            if (parameteters.ContainsKey("status"))
            {
                if (parameteters["status"] == "unpaid")
                {
                    return db.Invoices.Where(q => q.Status != "PAID");
                }
            }
            return db.Invoices;
        }


        // GET: api/Invoices/5
        [ResponseType(typeof(Invoice))]
        public async Task<IHttpActionResult> GetInvoice(int id)
        {
            Invoice invoice = await db.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            return Ok(invoice);
        }


        // // PATCH: api/Invoices/5

        public async Task<IHttpActionResult> PatchInvoice(int id, Invoice invoice)
        {
            
            if (id != invoice.InvoiceId)
            {
                return BadRequest();
            }
            if (!InvoiceExists(id))
            {
                return NotFound();
            }

            if (invoice.Status == null)
            {
                return BadRequest();
            }
            Invoice original = await db.Invoices.FindAsync(id);

            original.Status = invoice.Status;
            original.StatusDate = DateTime.Now;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!InvoiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }    
            return Ok(original);
        }

        // PUT: api/Invoices/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutInvoice(int id, Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != invoice.InvoiceId)
            {
                return BadRequest();
            }

            db.Entry(invoice).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Invoices
        [ResponseType(typeof(Invoice))]
        public async Task<IHttpActionResult> PostInvoice(Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Invoices.Add(invoice);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = invoice.InvoiceId }, invoice);
        }

        // DELETE: api/Invoices/5
        [ResponseType(typeof(Invoice))]
        public async Task<IHttpActionResult> DeleteInvoice(int id)
        {
            Invoice invoice = await db.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            db.Invoices.Remove(invoice);
            await db.SaveChangesAsync();

            return Ok(invoice);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InvoiceExists(int id)
        {
            return db.Invoices.Count(e => e.InvoiceId == id) > 0;
        }
    }
}