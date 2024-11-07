/*
    Initializing the Section Class:
        The Section class corresponds to a unique class section.
        Each section will have a unique section code and name.
        Each section should have a unique professor and a list of students who attend the class.

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: ...

*/

namespace MauiApp1.Models;

public class Section{
    public string code { get; set; }
    public string name { get; set; }

    public Team number{ get; set; }
}