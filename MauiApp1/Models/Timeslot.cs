/*
Timeslot class

    Individual daily timeslot associated with a student to create their timesheet

Written by Sarah Yakum
sny200000


*/
public class Timeslot{
    public string studentName { get; set; }
    public string netId{ get; set; }
    public Dictionary<DateTime, string> HoursByDate { get; set; }
    public Dictionary<DateTime, string> DescByDate { get; set; }

}