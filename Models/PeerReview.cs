/*
    Initializing the Peer Review Class:

        The Peer Review created with this class will set up the dates and 
        given criteria of a section for the students to submit reviews

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000
*/

namespace CS4485_Team75.Models;

public class PeerReview{
    public required string section { get; set; }
    public required string type { get; set; }
    public required DateOnly startDate{ get; set; }
    public required DateOnly endDate{ get; set; }
}