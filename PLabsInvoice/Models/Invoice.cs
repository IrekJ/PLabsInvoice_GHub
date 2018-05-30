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
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        //[JsonIgnore]
        //[IgnoreDataMember]
        public int ClientId { get; set; }

        [Display(Name = "Invoice #")]
        public int Number { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        [NotMapped]
        public decimal Total { get; set; }

        public string Status { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? StatusDate { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<LineItem> LineItems { get; set; }
    }
}