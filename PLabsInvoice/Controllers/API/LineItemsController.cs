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
    public class LineItemsController : ApiController
    {
        private InvoiceContext db = new InvoiceContext();

        // GET: api/LineItems
        public IQueryable<LineItem> GetLineItems()
        {
            return db.LineItems;
        }

        // GET: api/LineItems/5
        [ResponseType(typeof(LineItem))]
        public async Task<IHttpActionResult> GetLineItem(int id)
        {
            LineItem lineItem = await db.LineItems.FindAsync(id);
            if (lineItem == null)
            {
                return NotFound();
            }

            return Ok(lineItem);
        }

        // PUT: api/LineItems/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLineItem(int id, LineItem lineItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lineItem.LineItemId)
            {
                return BadRequest();
            }

            db.Entry(lineItem).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LineItemExists(id))
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

        // POST: api/LineItems
        [ResponseType(typeof(LineItem))]
        public async Task<IHttpActionResult> PostLineItem(LineItem lineItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LineItems.Add(lineItem);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = lineItem.LineItemId }, lineItem);
        }

        // DELETE: api/LineItems/5
        [ResponseType(typeof(LineItem))]
        public async Task<IHttpActionResult> DeleteLineItem(int id)
        {
            LineItem lineItem = await db.LineItems.FindAsync(id);
            if (lineItem == null)
            {
                return NotFound();
            }

            db.LineItems.Remove(lineItem);
            await db.SaveChangesAsync();

            return Ok(lineItem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LineItemExists(int id)
        {
            return db.LineItems.Count(e => e.LineItemId == id) > 0;
        }
    }
}