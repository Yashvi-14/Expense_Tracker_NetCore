using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Expense_Tracker.Models
{
    public partial class Categories
    {
        
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Type { get; set; }

        [System.Runtime.Serialization.IgnoreDataMember]
        public virtual ICollection<Transactions> Transactions { get; set; }
        
        public string? TitleWithIcon
        {
            get
            {
                return this.Icon + "   " + this.Title;
            }
        }

        public Categories()
        {
            Transactions = new HashSet<Transactions>();
        }

    }
}
