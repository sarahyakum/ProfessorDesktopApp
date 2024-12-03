/*
Timeslot class

    Individual daily timeslot associated with a student to create their timesheet

Written by Sarah Yakum
sny200000


*/
public class Timeslot{
    public string? studentName { get; set; }
    public string? netId{ get; set; }
    public DateOnly date{ get; set; }
    
    public string? hours { get; set; }
    public string? description { get; set; }
    public string? total { get; set; }

} 