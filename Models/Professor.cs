/*
    Initializing the Professor Class:
        The professors will be the main users of this application.
        ADD MORE TO THIS?

        New professsor will have to be added to the system manually in SQL.

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000

*/

class Professor(){
    public required string username{get;set;}

    public List<string>? sections{get;set;}
}