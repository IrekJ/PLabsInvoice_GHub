using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PLabsInvoice.DAL;
using PLabsInvoice.Models;

namespace PLabsInvoice.Controllers
{
    public class ClientsController : Controller
    {
        private InvoiceContext db = new InvoiceContext();


        public ActionResult AltClient()
        {
            return View();
        }

        // GET: Clients
        public ActionResult Index()
        {
            return View(db.Clients.ToList());
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClientId,Name,Address,City,State,Zip,Phone,Email")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("AltClient", "Clients");
            }

            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClientId,Name,Address,City,State,Zip,Phone,Email")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AltClient","Clients");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
            db.SaveChanges();
            return RedirectToAction("AltClient", "Clients");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult getClientList()
        {
            try
            {
                List<object> lReslut = new List<object>();
                List<Client> clients = db.Clients.ToList();

                foreach(Client c in clients)
                {
                    lReslut.Add(new { clientId = c.ClientId, name = c.Name });
                }


                return Json(lReslut, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult getClientById(int? id)
        {
            try
            {
                List<object> lReslut = new List<object>();
                Client client = db.Clients.Find(id);
                if (client != null)
                {
                    lReslut.Add(new { id= client.ClientId, name=client.Name,address=client.Address,city=client.City,state=client.State,zip=client.Zip,phone=client.Phone,email=client.Email });
                }
                return Json(lReslut, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult getInvoicesByClient(int? id)
        {
            try
            {
                List<object> lResult = new List<object>();
                List<Invoice> invoices = db.Invoices.Where(q => q.ClientId == id).ToList();
                LineItem[] lineItems;
                foreach (Invoice inv in invoices)
                {
                    lineItems = db.LineItems.Where(q => q.InvoiceId == inv.InvoiceId).ToArray();
                    for (int i = 0; i< lineItems.Length; i++)
                    {
                        inv.Total += lineItems[i].Qty * lineItems[i].UnitPrice;
                    }
                    string invStatDate = (inv.StatusDate.HasValue ? inv.StatusDate.Value.ToString("C2", System.Globalization.CultureInfo.CurrentCulture) : "");
                    lResult.Add(new {
                        invoiceId = inv.InvoiceId,
                        clientId = inv.ClientId,
                        invoiceNumber = inv.Number,
                        invoiceDate = inv.Date.ToShortDateString(),
                        invoiceStatus= (inv.Status ==null? "": inv.Status),
                        invoiceStatusDate = invStatDate,                        
                        invoiceTotal = inv.Total.ToString("C2", System.Globalization.CultureInfo.CurrentCulture) });
                }
                return Json(lResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult getLineItemsByInvoice(int? id)
        {
            try
            {
                List<object> lResult = new List<object>();
                LineItem[] lineItems = db.LineItems.Where(q => q.InvoiceId == id ).OrderBy(o => o.LineNumber ).ToArray();
                for (int i = 0; i < lineItems.Length ; i++)
                {
                    lineItems[i].LIneTotal = lineItems[i].UnitPrice * lineItems[i].Qty;
                    lResult.Add(new
                    {
                        lineId = lineItems[i].LineItemId,
                        lineNumber = lineItems[i].LineNumber,
                        descriptin = lineItems[i].Description,
                        qty = lineItems[i].Qty.ToString("#,##0.##"),
                        unitPrice = lineItems[i].UnitPrice.ToString("C2", System.Globalization.CultureInfo.CurrentCulture),
                        lineTotal = lineItems[i].LIneTotal.ToString("C2", System.Globalization.CultureInfo.CurrentCulture)
                    });
                }
                return Json(lResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }

        }
    }
}
