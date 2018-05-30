using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PLabsInvoice.Models
{
    public class Client
    {
        [Display(Name = "Client ID")]
        [Key]
        public int ClientId { get; set; }
        [StringLength(100)]
        //[Display(Name = "Client Name")]
        public string Name { get; set; }
        [StringLength(250)]
        [Display(Name = "Street Address")]
        public string Address { get; set; }
        [StringLength(100)]
        public string City { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [StringLength(50)]
        [Display(Name = "Zip Code")]
        public string Zip { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        [StringLength(250)]
        public string Email { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Invoice> Invoices { get; set; }

    }
}