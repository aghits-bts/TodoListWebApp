using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TodoListWebApp.Models
{
    public class TodoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        private int id;

        [Required]
        [StringLength(500)]
        private string description { get; set; } = null!;

        [Required]
        [StringLength(20)]
        private string importance { get; set; } = null!;

        private bool completed;

        [Required]
        [StringLength(100)]
        private string category { get; set; } = null!;

        private DateTime dueDate;

        // public accessor methods
        [Display(Name = "Priority")]
        public string Importance { get => this.importance; set => this.importance = value; }

        public int Id { get => this.id; set => this.id = value; }
        public string Description { get => this.description; set => this.description = value; }
        public bool Completed { get => this.completed; set => this.completed = value; }

        public string Category { get => this.category; set => this.category = value; }
        public DateTime DueDate { get => this.dueDate; set => this.dueDate = value; }
    }
}
