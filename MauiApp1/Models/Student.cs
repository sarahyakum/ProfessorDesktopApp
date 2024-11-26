/*
    Initializing the Student class:
        The student will have their own web applications to handle their side of the interactions. However the professor and student interact
            in multiple ways:
                - Professors should be able to add students to the database
                - Professors should be able to add students to teams in their section
                - Professors should be able to view/ edit the timeslots for students in their section
                - Professors create the peer reviews that the students do in their section
                - Professors should be able to see the scores for the peer reviews, both given and written by the students in their section

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000

*/

using System.Collections.ObjectModel;

namespace MauiApp1.Models;
public class Student{
    public string? name{get;set;}
    public required string netid{get;set;}

    public string? utdid {get;set;}
    public string? section{get;set;}

    public Dictionary<DateOnly,Timeslot>? timeslots{get;set;}

    public List<Score>? scores{get;set;}

}