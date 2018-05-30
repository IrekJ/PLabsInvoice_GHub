using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using PLabsInvoice.Models;

namespace PLabsInvoice.DAL
{
    public class InvoiceContext : DbContext 
    {
        public InvoiceContext() : base("InvoiceContext") {}
        public DbSet<Client> Clients { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<LineItem> LineItems { get; set; }

    }
}