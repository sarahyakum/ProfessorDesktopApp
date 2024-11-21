/*
    Initializing the Peer Review Class:

        The Peer Review created with this class will set up the dates and 
        given criteria of a section for the students to submit reviews

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000
*/

namespace MauiApp1.Models;

public class PeerReview{
    public string section{ get; set; }
    public string type{ get; set; }
    public DateOnly startDate{ get; set; }
    public DateOnly endDate{ get; set; }
}