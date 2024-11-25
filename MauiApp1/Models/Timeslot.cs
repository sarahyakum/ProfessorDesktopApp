/*
Timeslot class

    Individual daily timeslot associated with a student to create their timesheet

Written by Sarah Yakum
sny200000


*/


namespace MauiApp1.Models;
public class Timeslot{
    public required DateTime date{get;set;}
    public required string duration {get;set;}

    public required string desciption {get;set;}
}