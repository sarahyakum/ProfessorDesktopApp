namespace MauiApp1.ViewModels{
    public class StudentHoursViewModel
    {
        public List<StudentHours> Students { get; set; }

        public StudentHoursViewModel()
        {
            // Sample Data
            Students = new List<StudentHours>
            {
                new StudentHours
                {
                    StudentName = "John Doe",
                    HoursByDate = new Dictionary<DateTime, double>
                    {
                        { new DateTime(2024, 10, 19), 5 },
                        { new DateTime(2024, 10, 20), 5 },
                        { new DateTime(2024, 10, 21), 4 },
                        { new DateTime(2024, 10, 22), 5 },
                        { new DateTime(2024, 10, 23), 5 },
                    }
                },
                new StudentHours
                {
                    StudentName = "Jane Smith",
                    HoursByDate = new Dictionary<DateTime, double>
                    {
                        { new DateTime(2024, 10, 19), 5 },
                        { new DateTime(2024, 10, 20), 6 },
                        { new DateTime(2024, 10, 21), 7 },
                        { new DateTime(2024, 10, 22), 5 },
                        { new DateTime(2024, 10, 23), 5 },
                    }
                }
                ,
                new StudentHours
                {
                    StudentName = "Sarah",
                    HoursByDate = new Dictionary<DateTime, double>
                    {
                        { new DateTime(2024, 10, 19), 5 },
                        { new DateTime(2024, 10, 20), 6 },
                        { new DateTime(2024, 10, 21), 7 },
                        { new DateTime(2024, 10, 22), 5 },
                        { new DateTime(2024, 10, 23), 5 },
                    }
                }
                ,
                new StudentHours
                {
                    StudentName = "Kiara",
                    HoursByDate = new Dictionary<DateTime, double>
                    {
                        { new DateTime(2024, 10, 19), 5 },
                        { new DateTime(2024, 10, 20), 6 },
                        { new DateTime(2024, 10, 21), 7 },
                        { new DateTime(2024, 10, 22), 5 },
                        { new DateTime(2024, 10, 23), 5 },
                    }
                }
                ,
                new StudentHours
                {
                    StudentName = "Emma",
                    HoursByDate = new Dictionary<DateTime, double>
                    {
                        { new DateTime(2024, 10, 19), 5 },
                        { new DateTime(2024, 10, 20), 6 },
                        { new DateTime(2024, 10, 21), 7 },
                        { new DateTime(2024, 10, 22), 5 },
                        { new DateTime(2024, 10, 23), 5 },
                    }
                }
                ,
                new StudentHours
                {
                    StudentName = "Darya",
                    HoursByDate = new Dictionary<DateTime, double>
                    {
                        { new DateTime(2024, 10, 19), 5 },
                        { new DateTime(2024, 10, 20), 6 },
                        { new DateTime(2024, 10, 21), 7 },
                        { new DateTime(2024, 10, 22), 5 },
                        { new DateTime(2024, 10, 23), 5 },
                    }
                }
            };
        }
    }


}
