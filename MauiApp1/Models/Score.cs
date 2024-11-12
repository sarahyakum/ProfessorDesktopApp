namespace MauiApp1.Models;
/*
    Initializing the Score Class:

       The Score class is attached to each student and keeps all the scores they have received
       from team mates as well as their average.

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000
*/
public class Score{
    public int team{get;set;}
    public string reviewer { get; set; }   
    public int score { get; set; }
    public string criteria { get; set; }
    public string average{ get; set; }
}