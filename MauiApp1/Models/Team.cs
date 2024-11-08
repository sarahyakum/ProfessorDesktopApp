/*
    Initializing the Team Class:
        Teams are dependent on the section. They should be used to separate the students to make peer reviews easier.

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000

*/

namespace MauiApp1.Models;

public class Team(){

    public string section{get;set;}
    public int number{ get; set; }

    public List<Student> members{ get; set; }

}