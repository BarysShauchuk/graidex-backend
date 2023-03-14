namespace Graidex.Domain.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Faculty { get; set; }
        public string Teacher { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Test> Tests { get; set; }

        public Subject(
            int id,
            string title,
            string faculty,
            string teacher,
            ICollection<Student> students,
            ICollection<Test> tests)
        {
            Id = id;
            Title = title;
            Faculty = faculty;
            Teacher = teacher;
            Students = students;
            Tests = tests;
        }
    }
}
