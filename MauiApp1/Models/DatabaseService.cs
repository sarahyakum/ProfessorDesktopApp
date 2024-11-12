/*
    Connects the professor's front end with the database. Instantiates the connection with the connection string. 
    Connects the various procedures from the database to the front end system.
    The procedures include:
        Time Tracking: Editing timeslot, getting student emails
        General: Professor login, change password, insert the number of teams, deleting a team, changing student's teams, getting sections,
            adding students to teams, adding section
        Peer Review: Getting section criteria, creating criteria, editing criteria, deleting criteria, creating peer reviews, viewing average scores, 
            viewing individual scores, editing scores, reusing previous criteria, getting incomplete reviews, getting student emails

    The procedures handle most of the validation for the database and return a string of the status of the procedure. 

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on ....
        NETID: sny200000
*/

using System.Data;
using MySqlConnector;

namespace MauiApp1.Models;

class DatabaseService{
    // Connection String to the databse, must change to reflect how the database is used on your device and your password.
    string connectionString = "Server=localhost;Database=seniordesignproject;User=root;Password=sarahyakum;";


    // Professor Login connection to check the inputted values against the database
    // Also returns whether the professor needs to change their password because it is a first time login
    public async Task<string> Login(string username, string password){

        string error_message = string.Empty;
        using (var conn = new MySqlConnection(connectionString)){
        try{
            await conn.OpenAsync();
            using(var cmd = new MySqlCommand("check_professor_login", conn)){
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@prof_input_username", username);
                cmd.Parameters.AddWithValue("@prof_input_password", password);

                var result = new MySqlParameter("@error_message", MySqlDbType.VarChar);
                result.Direction= System.Data.ParameterDirection.Output;
                result.Size = 255;
                cmd.Parameters.Add(result);

                await cmd.ExecuteNonQueryAsync();

                error_message = result.Value?.ToString() ?? string.Empty;



            }
            return error_message;
        }
        catch(Exception ex){
            return "Error: " + ex.Message;
        }
       }

    }
    

    // Gets the sections that a professor teaches 
    public async Task<List<Section>> GetSections(string netId){


        List<Section> sections= new List<Section>();
        
        using (var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("professor_get_sections", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@prof_netID", netId);

                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()){
                                            while (await reader.ReadAsync())
                    {
                        sections.Add(new Section
                        {
                            code = reader.GetString("SecCode"), // Match column names with your query
                            name = reader.GetString("SecName")
                        });
                    }

                    }

                }
            }
            catch (Exception ex){
                Console.Write(ex.Message);
            
            }
            return sections;


        }
    }

    // Checks whether the inputted username, old password, and new password are within the constraints:
        // Username and old password must match, new password cannot be the same as the old or the UTDID
    public async Task<string> ChangePassword(string netid, string oldPassword, string newPassword){

        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(var cmd = new MySqlCommand("change_professor_password", conn)){
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@prof_username", netid);
                    cmd.Parameters.AddWithValue("@old_professor_password", oldPassword);
                    cmd.Parameters.AddWithValue("@new_professor_password", newPassword);

                    var result = new MySqlParameter("@error_message", MySqlDbType.VarChar);
                    result.Direction= System.Data.ParameterDirection.Output;
                    result.Size = 255;
                    cmd.Parameters.Add(result);

                    await cmd.ExecuteNonQueryAsync();

                    error_message = result.Value?.ToString() ?? string.Empty;
                

                }
                return error_message;
                

            }
            catch(Exception ex){
                return "Error: " + ex.Message;
            }
        }

    }


    // Gets the NetIDs of all of the students in the section that the professor teaches
    public async Task<List<Student>> GetStudents(string code){


        List<Student> students= new List<Student>();
        
        using (var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("get_section_students", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Connection = conn;

                    cmd.Parameters.AddWithValue("@section_code", code);

                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        while (await reader.ReadAsync())
                            {
                                students.Add(new Student
                                {
                                    netid=reader.GetString("StuNetID"),
                                    section = code
                                });
                            }

                        }

                }
            }
            catch (Exception ex){
                Console.Write(ex.Message);
            
            }
            return students;


        }
    }

    // Based on the student's NetID, returns their Name and UTDID
    public async Task<Student> GetStudentsInfo(string netid){


        Student student = new Student();
        string studentNetid = netid;
        string studentName = "";
        string studentUtdId = "";
        using (var conn = new MySqlConnection(connectionString)){
            string query = "SELECT StuName, StuUTDID FROM Student WHERE StuNetID = @StuNetID";
            
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand(query, conn)){
                    

                    cmd.Parameters.AddWithValue("@StuNetID", studentNetid);

                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        if (await reader.ReadAsync()){
                            studentName = reader["StuName"] != DBNull.Value ? reader["StuName"].ToString() ?? string.Empty : string.Empty;
                            studentName = reader["StuName"] != DBNull.Value ? reader["StuUTDID"].ToString() ?? string.Empty : string.Empty;
                        }

                    }
                }
            }
            catch (Exception ex){
                Console.Write(ex.Message);
            
            }
            
        }
        student.name = studentName;
        student.netid = studentNetid;
        student.utdid = studentUtdId;

        return student;

    }

    //get timeslot for student by date
    public async Task<List<Timeslot>> GetTimeslots(DateTime date, string netId){
        List<Timeslot> timeslots = new List<Timeslot>();
        using(var conn = new MySqlConnection(connectionString)){
           
            try{
                using (MySqlCommand cmd = new MySqlCommand("student_timeslot_by_date", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@stu_netID", netId);
                    cmd.Parameters.AddWithValue("@input_date", date);
                     using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        while (await reader.ReadAsync())
                        {
                            timeslots.Add(new Timeslot
                            {
                                
                                desciption = reader.GetString("TSDate"),
                                duration = reader.GetString("TSDuration")
                                
                            });
                        }
                    }
                }
            }
            catch (Exception ex){
                    Console.Write(ex.Message);

            }
        }
        return timeslots;
    }


    // Gets the teams for the section that the professor teaches
    public async Task<List<Team>> GetTeams(string code){

        
        using (var conn = new MySqlConnection(connectionString)){
            List<Team> teams = new List<Team>();
            
            
            
            string query = "SELECT TeamNum from Team where SecCode=@SecCode";
            
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand(query, conn)){
                    

                    cmd.Parameters.AddWithValue("@SecCode", code);

                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        while (await reader.ReadAsync()){
                            teams.Add(new Team{
                                number=Convert.ToInt32(reader["TeamNum"]),
                                section = code

                            }); 
                        }
                    }
                }
            }
            catch (Exception ex){
                Console.Write(ex.Message);
            
            }

            return teams;
            
        }
        
    }

    //Get Team members for each team
    public async Task<List<Student>> GetTeamMembers(string section, int team_num){
        int number = team_num;

        
        using (var conn = new MySqlConnection(connectionString)){
            List<Student> students = new List<Student>();
            
            
            
            string query = "SELECT StuNetID from MemberOf where TeamNum=@TeamNum";
            
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand(query, conn)){
                    

                    cmd.Parameters.AddWithValue("@TeamNum", team_num);

                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        while (await reader.ReadAsync()){
                            students.Add(new Student{
                                netid= reader.GetString("StuNetID")

                            });
                            
                        }
                    }
                }
            }
            catch (Exception ex){
                Console.Write(ex.Message);
            
            }

            return students;
            
        }
    }
    //Creates criteria for a review type in a specific section
    public async Task<string> CreateCriteria(string netid, List<string> criteriaSetup){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand("professor_create_criteria", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@professor_netID", netid);
                    cmd.Parameters.AddWithValue("@section_code", criteriaSetup[0]);
                    cmd.Parameters.AddWithValue("@criteria_name", criteriaSetup[1]);
                    cmd.Parameters.AddWithValue("@criteria_description", criteriaSetup[2]);
                    cmd.Parameters.AddWithValue("@review_type", criteriaSetup[3]);

                    var result = new MySqlParameter("@error_message", MySqlDbType.VarChar);
                    result.Direction= System.Data.ParameterDirection.Output;
                    result.Size = 255;
                    cmd.Parameters.Add(result);

                    await cmd.ExecuteNonQueryAsync();

                    error_message = result.Value?.ToString() ?? string.Empty;



                }
                return error_message;

            }
            catch(Exception ex){
                return "Error: " + ex.Message;
            }
        }
    }

     //Creates a peer review for a specific section with availability dates
     public async Task<string> CreatePeerReview(string netid, List<string> prDetails, List<DateTime> dates){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand("create_peer_reviews", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@professor_netID", netid);
                    cmd.Parameters.AddWithValue("@section_code", prDetails[0]);
                    cmd.Parameters.AddWithValue("@review_type", prDetails[1]);
                    cmd.Parameters.AddWithValue("@start_date", dates[0]);
                    cmd.Parameters.AddWithValue("@end_date", dates[1]);

                    var result = new MySqlParameter("@error_message", MySqlDbType.VarChar);
                    result.Direction= System.Data.ParameterDirection.Output;
                    result.Size = 255;
                    cmd.Parameters.Add(result);

                    await cmd.ExecuteNonQueryAsync();

                    error_message = result.Value?.ToString() ?? string.Empty;



                }
                return error_message;

            }
            catch(Exception ex){
                return "Error: " + ex.Message;
            }
        }
    }

   //Allows for professor to modify individual scores for a student if needed
    public async Task<string> EditScores(string netid, List<string> reviewInfo, int newScore){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("edit_scores_given", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@professor_netID", netid);
                    cmd.Parameters.AddWithValue("@section_code",reviewInfo[0]);
                    cmd.Parameters.AddWithValue("@reviewer_netID", reviewInfo[1]);
                    cmd.Parameters.AddWithValue("@reviewee_netID", reviewInfo[2]);
                    cmd.Parameters.AddWithValue("@criteria_name", reviewInfo[3]);
                    cmd.Parameters.AddWithValue("@new_score", newScore);
                    cmd.Parameters.AddWithValue("@review_type", reviewInfo[4]);

                    var result = new MySqlParameter("@error_message", MySqlDbType.VarChar);
                    result.Direction= System.Data.ParameterDirection.Output;
                    result.Size = 255;
                    cmd.Parameters.Add(result);

                    await cmd.ExecuteNonQueryAsync();

                    error_message = result.Value?.ToString() ?? string.Empty;


                }
                return error_message;
            }
            catch(Exception ex){
                return "Error: " + ex.Message;
            }
        }
    }

    //Inserts the amount of teams a professor will have for a section
    public async Task<string> InsertTeamNums(string netid, string section, int numTeams){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("professor_insert_num_teams", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@professor_netID", netid);
                    cmd.Parameters.AddWithValue("@section_code",section);
                    cmd.Parameters.AddWithValue("@num_teams", numTeams);


                    var result = new MySqlParameter("@error_message", MySqlDbType.VarChar);
                    result.Direction= System.Data.ParameterDirection.Output;
                    result.Size = 255;
                    cmd.Parameters.Add(result);

                    await cmd.ExecuteNonQueryAsync();

                    error_message = result.Value?.ToString() ?? string.Empty;


                }
                return error_message;
            }
            catch(Exception ex){
                return "Error: " + ex.Message;
            }
        }
    }

    //Deletes a team for a section based on given team number
    public async Task<string> DeleteTeam(string netid, string section, int teamNum){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("professor_delete_team", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@professor_netID", netid);
                    cmd.Parameters.AddWithValue("@section_code",section);
                    cmd.Parameters.AddWithValue("@team_num", teamNum);


                    var result = new MySqlParameter("@error_message", MySqlDbType.VarChar);
                    result.Direction= System.Data.ParameterDirection.Output;
                    result.Size = 255;
                    cmd.Parameters.Add(result);

                    await cmd.ExecuteNonQueryAsync();

                    error_message = result.Value?.ToString() ?? string.Empty;


                }
                return error_message;
            }
            catch(Exception ex){
                return "Error: " + ex.Message;
            }
        }
    }

    //Gets criteria based
    public async Task<List<Criteria>> GetSectionsCriteria(string secCode){
        List<Criteria> criterias = new List<Criteria>();

        string query = "SELECT CriteriaName, CriteriaDescription, ReviewType FROM Criteria WHERE SecCode=@SecCode";
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand(query, conn)){
                    cmd.Parameters.AddWithValue("@SecCode", secCode);

                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        while(await reader.ReadAsync()){
                            criterias.Add(new Criteria{
                                section = secCode,
                                name = reader.GetString("CriteriaName"),
                                description = reader.GetString("CriteriaDescription"),
                                reviewType = reader.GetString("ReviewType")
                               

                            });
                        }
                    }
                }
            }
            catch (Exception ex){
                Console.Write(ex.Message);
            }
        }
        return criterias;
    }
   
    //Gets the peer reviews for a section
    public async Task<List<PeerReview>> GetPeerReviews(string secCode) {
        List<PeerReview> peerReviews = new List<PeerReview>();
        string query = " SELECT DISTINCT ReviewType, StartDate, EndDate FROM PeerReview WHERE SecCode=@SecCode";
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand(query, conn)){
                    cmd.Parameters.AddWithValue("@SecCode", secCode);

                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        while(await reader.ReadAsync()){
                            peerReviews.Add(new PeerReview{
                                section = secCode,
                                type = reader.GetString("ReviewType"),
                                startDate = reader.GetDateTime("StartDate"),
                                endDate = reader.GetDateTime("EndDate")

                               

                            });
                        }
                    }
                }
            }
            catch (Exception ex){
                Console.Write(ex.Message);
            }
        }
        return peerReviews;
    }
    //Retrieves criteria ID for a section in order to edit it
     public async Task<string> GetCriteriaID(string netid, string section, string reviewType){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("get_section_criteriaid", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@professor_netID", netid);
                    cmd.Parameters.AddWithValue("@section_code",section);
                    cmd.Parameters.AddWithValue("@review_type", reviewType);


                    var result = new MySqlParameter("@error_message", MySqlDbType.VarChar);
                    result.Direction= System.Data.ParameterDirection.Output;
                    result.Size = 255;
                    cmd.Parameters.Add(result);

                    await cmd.ExecuteNonQueryAsync();

                    error_message = result.Value?.ToString() ?? string.Empty;


                }
                return error_message;
            }
            catch(Exception ex){
                return "Error: " + ex.Message;
            }
        }
    }

    //Edits a specific criteria created
    public async Task<string> EditCriteria(string netid, string section, int criteriaID, List<string> criteriaInfo){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("professor_edit_criteria", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@professor_netID", netid);
                    cmd.Parameters.AddWithValue("@section_code",section);
                    cmd.Parameters.AddWithValue("@criteria_id", criteriaID);
                    cmd.Parameters.AddWithValue("@criteria_name", criteriaInfo[0]);
                    cmd.Parameters.AddWithValue("@criteria_description", criteriaInfo[1]);
                    cmd.Parameters.AddWithValue("@review_type", criteriaInfo[2]);


                    var result = new MySqlParameter("@error_message", MySqlDbType.VarChar);
                    result.Direction= System.Data.ParameterDirection.Output;
                    result.Size = 255;
                    cmd.Parameters.Add(result);

                    await cmd.ExecuteNonQueryAsync();

                    error_message = result.Value?.ToString() ?? string.Empty;


                }
                return error_message;
            }
            catch(Exception ex){
                return "Error: " + ex.Message;
            }
        }
    }
   
    //Deletes a specific criteria
    public async Task<string> DeleteCriteria(string netid, string section, string criteriaName, string reviewType){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("professor_delete_criteria", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@professor_netID", netid);
                    cmd.Parameters.AddWithValue("@section_code",section);
                    cmd.Parameters.AddWithValue("@criteria_name", criteriaName);
                    cmd.Parameters.AddWithValue("@review_type", reviewType);
                    var result = new MySqlParameter("@error_message", MySqlDbType.VarChar);
                    result.Direction= System.Data.ParameterDirection.Output;
                    result.Size = 255;
                    cmd.Parameters.Add(result);

                    await cmd.ExecuteNonQueryAsync();

                    error_message = result.Value?.ToString() ?? string.Empty;


                }
                return error_message;
            }
            catch(Exception ex){
                return "Error: " + ex.Message;
            }
        }
    }
    
    //Changes a students assigned team
    public async Task<string> ChangeStuTeam(string profNetid, string section, string stuNetid, int newTeam){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("professor_change_student_team", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@professor_netID", profNetid);
                    cmd.Parameters.AddWithValue("@section_code",section);
                    cmd.Parameters.AddWithValue("@student_netID", stuNetid);
                    cmd.Parameters.AddWithValue("@new_team", newTeam);

                    var result = new MySqlParameter("@error_message", MySqlDbType.VarChar);
                    result.Direction= System.Data.ParameterDirection.Output;
                    result.Size = 255;
                    cmd.Parameters.Add(result);

                    await cmd.ExecuteNonQueryAsync();

                    error_message = result.Value?.ToString() ?? string.Empty;


                }
                return error_message;
            }
            catch(Exception ex){
                return "Error: " + ex.Message;
            }
        }
    }
    
    //Allows professor to reuse criteria for different types of reviews
    public async Task<string> ReuseCriteria(string netid, string section, string oldType, string newType){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("reuse_criteria", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@professor_netID", netid);
                    cmd.Parameters.AddWithValue("@section_code",section);
                    cmd.Parameters.AddWithValue("@old_criteria_type", oldType);
                    cmd.Parameters.AddWithValue("@new_criteria_type", newType);

                    var result = new MySqlParameter("@error_message", MySqlDbType.VarChar);
                    result.Direction= System.Data.ParameterDirection.Output;
                    result.Size = 255;
                    cmd.Parameters.Add(result);

                    await cmd.ExecuteNonQueryAsync();

                    error_message = result.Value?.ToString() ?? string.Empty;

                }
                return error_message;
            }
            catch(Exception ex){
                return "Error: " + ex.Message;
            }
        }
    }
    
    //Adds students to a professors class
    public async Task<string> AddStudents(string netid, string utdid, string name, string section){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("professor_add_students", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@student_netID",netid);
                    cmd.Parameters.AddWithValue("@student_UTDID",utdid);
                    cmd.Parameters.AddWithValue("@student_name", name);
                    cmd.Parameters.AddWithValue("@section_code", section);

                    var result = new MySqlParameter("@error_message", MySqlDbType.VarChar);
                    
                    result.Direction= System.Data.ParameterDirection.Output;
                    result.Size = 255;
                    cmd.Parameters.Add(result);

                    await cmd.ExecuteNonQueryAsync();

                    error_message = result.Value?.ToString() ?? string.Empty;


                }
                return error_message;
            }
            catch(Exception ex){
                return "Error: " + ex.Message;
            }
        }
    }
    
    //Adds a student to a team
     public async Task<string> AddNewTeamMember(int teamNum, string netid, string section){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("add_student_to_team", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@team_num",teamNum);
                    cmd.Parameters.AddWithValue("@student_netID", netid);
                    cmd.Parameters.AddWithValue("@section_code", section);

                    var result = new MySqlParameter("@error_message", MySqlDbType.VarChar);
                    result.Direction= System.Data.ParameterDirection.Output;
                    result.Size = 255;
                    cmd.Parameters.Add(result);

                    await cmd.ExecuteNonQueryAsync();

                    error_message = result.Value?.ToString() ?? string.Empty;


                }
                return error_message;
            }
            catch(Exception ex){
                return "Error: " + ex.Message;
            }
        }
    }
    
    //Creates view for each teams peer review scores
    public async Task<List<Score>> GetReviews(string profID, string section, string stuID, string reviewType){
        string error_message = string.Empty;
        List<Score> scores= new List<Score>();
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("professor_view_individual_scores", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@professor_netID",profID);
                    cmd.Parameters.AddWithValue("@section_code", section);
                    cmd.Parameters.AddWithValue("@student_netID", stuID);
                    cmd.Parameters.AddWithValue("@review_type", reviewType);
                    
                    var result = new MySqlParameter("@error_message", MySqlDbType.VarChar);
                    result.Direction= System.Data.ParameterDirection.Output;
                    result.Size = 255;
                    cmd.Parameters.Add(result);


                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        while (await reader.ReadAsync()){
                            scores.Add(new Score{
                                reviewer = reader.GetString("ReviewerName"),
                                score = reader.GetInt32("Score"),
                                criteria = reader.GetString("CriteriaName")

                            });
                            
                        }}

                    await cmd.ExecuteNonQueryAsync();

                    error_message = result.Value?.ToString() ?? string.Empty;
                }
                Console.Write(error_message);
            }
            catch(Exception ex){
                Console.Write(ex.Message);
            }
        }
        return scores;
    }
    
    //Add a new section for a professor

    //Retrieves sections start and end date

    //Retrieves the students with unfinished reviews

    //Edits the timeslot of a student

    //Retrieves email addresses for students who have not logged any hours

    //Retrieves email addresses for student who have not submitted their peer reviews



}