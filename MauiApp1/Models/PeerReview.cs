/*
    Initializing the Criteria Class:

        The Criteria will be used in the Peer Reviews. The Professor will be able to create and add the criteria for each section.
        Then upon creation of the peer reviews, the students will receive the criteria to rate their team members on.

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000
*/

namespace MauiApp1.Models;

public class PeerReview{
    public string section{ get; set; }
    public string type{ get; set; }
    public DateTime startDate{ get; set; }
    public DateTime endDate{ get; set; }
}