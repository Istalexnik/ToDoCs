using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoCs.Models
{
    public partial class ToDoItem : ObservableObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Primary key for database, auto-generated

        [ObservableProperty]
        private string title = string.Empty;

        // Initialize properties with default values
        [ObservableProperty]
        private DateTime createdDate = DateTime.Now;

        [ObservableProperty]
        private DateTime editedDate = DateTime.Now;
    }
}
