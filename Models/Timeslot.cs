/*
Timeslot class

    Individual daily timeslot associated with a student to create their timesheet

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in November 1, 2024
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