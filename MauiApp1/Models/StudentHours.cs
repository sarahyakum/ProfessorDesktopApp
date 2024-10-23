public class StudentHours
{
    public string StudentName { get; set; }
    public Dictionary<DateTime, double> HoursByDate { get; set; } // Hours worked by date
    public double CumulativeHours => HoursByDate.Values.Sum(); // Sum of hours worked
}
