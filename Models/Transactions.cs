using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Expense_Tracker.Models
{
    public partial class Transactions
    {
        public int TransactionId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Amount should be greater than 0.")]
        public int Amount { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; } = DateTime.Now ;
        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        public int? CategoryId { get; set; }

        [JsonIgnore]
        
        public  Categories ? Categories { get; set; }
       /* public Categories? Category { get; set; }*/

        public string? CategoryTitleWithIcon
        {
            get
            {
                return Categories == null ? "" : Categories.Icon + " " + Categories.Title;
            }
        }


        public string? FormattedAmount
        {
            get
            {
                return ((Categories == null || Categories.Type == "Expense") ? "- " : "+ ") + Amount.ToString("C0");
            }
        }


    }
}
