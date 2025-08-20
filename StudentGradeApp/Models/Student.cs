namespace StudentGradeApp.Models;

public class Student
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;

    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
