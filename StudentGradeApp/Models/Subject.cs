namespace StudentGradeApp.Models;
public class Subject
{
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;

    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
}