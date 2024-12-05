/*
    Initializing the Team Class:
        Teams are dependent on the section. They should be used to separate the students to make peer reviews easier.

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in November 1, 2024
        NetID: sny200000

*/

using System.Collections.ObjectModel;

namespace CS4485_Team75.Models;

public class Team(){

    public required string section {get;set;}
    public required int number{ get; set; }
    public List<Student>? members{ get; set; }

}