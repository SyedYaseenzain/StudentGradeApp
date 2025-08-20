using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentGradeApp.Models
{
    public class Grade
    {
        public int GradeId { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }

        [Required]
        [Range(0, 100)]
        public int GradeValue { get; set; }   // 👈 match DB column name

        [NotMapped]
        public string Remarks => GradeValue >= 75 ? "PASS" : "FAIL";
    }
}