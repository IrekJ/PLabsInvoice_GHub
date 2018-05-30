using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PLabsInvoice.Models
{
    public class LineItem
    {
        [Key]
        public int LineItemId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public int InvoiceId { get; set; }
        public int LineNumber { get; set; }
        public string Description { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitPrice { get; set; }
        [NotMapped]
        public decimal LIneTotal { get; set; }

    }
}