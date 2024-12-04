/*
    Connects the professor's front end with the database. Instantiates the connection with the connection string. 
    Connects the various procedures from the database to the front end system.
    
    Login/ Change Password Procedures: 
        Login, ChangePassword

    Sections Procedures: 
        GetSections, AddSection, EditSection, DeleteSection, GetCourseTimeFrame

    Students Procedures: 
        GetStudents, GetStudentsInfo, GetStudentandInfo, AddStudents, EditStudent, DeleteStudent

    Teams Procedures: 
        GetTeams, GetTeamNumber, GetTeamMembers, GetUnassignedStudents, InsertTeamNum, EditTeamNumber, DeleteTeam, 
        ChangeStuTeam, RemoveFromTeam, AddNewTeamMember, CheckTeamExists

    Time Tracking Procedures: 
        GetTimeslots, GetTimeslot, GetWeeklyTimeslots

    Peer Review Procedures: 
        Criteria:
            CreateCriteria, GetSectionCriteria, GetAllCriteriaID, GetCriteriaID, CheckCriteriaInPR, EditCriteria, DeleteCriteria

        Peer Review:
            CreatePeerReview, GetPeerReviews, EditPRDates, DeletePR, EditScores, CheckPeerReviewStatus, GetReviews

    Student Emails Procedures: 
        GetPREmails, GetTTEmails

    The procedures handle most of the validation for the database and return a string of the status of the procedure. 

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on October 24, 2024
        NETID: sny200000

    Written by Emma Hockett for CS 4485.0W1, Senior Design Project, Started on November 12, 2024
        NETID: ech210001
*/

using System.Collections.ObjectModel;
using System.Data;
using MySqlConnector;

namespace CS4485_Team75.Models;

class DatabaseService{
    // Connection String to the databse, must change to reflect how the database is used on your device and your password.
    string connectionString = "Server=localhost;Database=seniordesignproject;User=root;Password=seniordesignproject;";

    /* *************************************************************************************
    *   Procedure Calls for Professor Login/ Login Information                             *
    ****************************************************************************************/

    // Professor Login connection to check the inputted values against the database
    // Written by Sarah Yakum (sny200000)
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

    // Checks whether the inputted username, old password, and new password are within the constraints:
    // Written by Sarah Yakum (sny200000)
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
    
    

    /* *************************************************************************************
    *   Procedure Calls for Professor Sections                                             *
    ****************************************************************************************/

    // Gets the sections that a professor teaches 
    // Written by Sarah Yakum (sny200000)
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
                            DateTime startDate = reader.GetDateTime("StartDate");
                            DateTime endDate = reader.GetDateTime("EndDate");

                            sections.Add(new Section
                            {
                                code = reader.GetString("SecCode"), // Match column names with your query
                                name = reader.GetString("SecName"),
                                startDate = DateOnly.FromDateTime(startDate),
                                endDate = DateOnly.FromDateTime(endDate)
    
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

    //Add a new section for a professor
    // Written by Sarah Yakum (sny200000)
    public async Task<string> AddSection(string netid, List<string> sectionInfo, List<DateOnly> dates){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand("professor_add_section", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@professor_netID", netid);
                    cmd.Parameters.AddWithValue("@section_code", sectionInfo[1]);
                    cmd.Parameters.AddWithValue("@section_name", sectionInfo[0]);
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

    //Allows the professor to edit a section
    // Written by Emma Hockett (ech210001)
    public async Task<string> EditSection(string netid, List<string> sectionInfo, List<DateOnly> dates){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand("professor_edit_section", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@original_section_code", netid);
                    cmd.Parameters.AddWithValue("@updated_name", sectionInfo[0]);
                    cmd.Parameters.AddWithValue("@updated_code", sectionInfo[1]);
                    cmd.Parameters.AddWithValue("@updated_start_date", dates[0]);
                    cmd.Parameters.AddWithValue("@updated_end_date", dates[1]);

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

    //Allows the professor to edit a section
    // Written by Emma Hockett (ech210001)
    public async Task<string> DeleteSection(string sectionCode){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand("professor_delete_section", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@section_code", sectionCode);

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

    
    //Retrieves sections start and end date
    // Written by Sarah Yakum (sny200000)
    public async Task<List<DateOnly>> GetCourseTimeFrame(string section){

        string error_message = string.Empty;
        List<DateOnly> window = new List<DateOnly>();
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("get_section_timeframe", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@section_code", section);
                    
                    
                    var result = new MySqlParameter("@error_message", MySqlDbType.VarChar);
                    result.Direction= System.Data.ParameterDirection.Output;
                    result.Size = 255;
                    cmd.Parameters.Add(result);


                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        while (await reader.ReadAsync()){
                            
                            DateTime startDate = reader.GetDateTime("StartDate");
                            DateTime endDate = reader.GetDateTime("EndDate");
                            window.Add(DateOnly.FromDateTime(startDate));
                            window.Add(DateOnly.FromDateTime(endDate));
                            
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
        return window;
    }


    /* *************************************************************************************
    *   Procedure Calls for Students                                                       *
    ****************************************************************************************/


    // Based on the student's NetID, returns their Name and UTDID
    // Written by Emma Hockett (ech210001)
    public async Task<Student> GetStudentsInfo(string netid){

        string studentNetid = netid;
        string studentName = "";
        string studentUtdId = "";
        using (var conn = new MySqlConnection(connectionString)){
            string query = "SELECT StuName, StuUTDID FROM Student WHERE StuNetID = @StuNetID";
            
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StuNetID", studentNetid);

                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        if (await reader.ReadAsync()){
                            studentName = reader["StuName"] != DBNull.Value ? reader["StuName"].ToString() ?? string.Empty : string.Empty;
                            studentUtdId = reader["StuUTDID"] != DBNull.Value ? reader["StuUTDID"].ToString() ?? string.Empty : string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex){
                Console.Write(ex.Message);
            }
        }
        Student student = new Student() {name = studentName, netid = studentNetid, utdid = studentUtdId};
        student.name = studentName;
        student.netid = studentNetid;
        student.utdid = studentUtdId;

        return student;
    }


    // Get all of the students and their info in this section 
    // Written by Emma Hockett (ech210001)
    public async Task<ObservableCollection<Student>> GetStudentAndInfo(string code)
    {
        var students = new ObservableCollection<Student>();
        
        using (var conn = new MySqlConnection(connectionString)){
            string query = "SELECT StuName, StuUTDID, StuNetID FROM Student WHERE StuNetID IN (SELECT StuNetID FROM Attends WHERE SecCode = @section_code)";
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand(query, conn)){
                    cmd.Connection = conn;

                    cmd.Parameters.AddWithValue("@section_code", code);

                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        while (await reader.ReadAsync())
                        {
                            students.Add(new Student
                            {
                                netid=reader.GetString("StuNetID"),
                                name = reader.GetString("StuName"),
                                utdid = reader.GetString("StuUTDID"),
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

    //Adds students to a professors class
    // Written by Sarah Yakum (sny200000)
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

    //Allows the professor to edit a student
    // Written by Emma Hockett (ech210001) 
    public async Task<string> EditStudent(string netid, List<string> studentInfo){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand("professor_edit_student", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@original_student_netid", netid);
                    cmd.Parameters.AddWithValue("@updated_netid", studentInfo[0]);
                    cmd.Parameters.AddWithValue("@updated_name", studentInfo[1]);
                    cmd.Parameters.AddWithValue("@updated_utdid", studentInfo[2]);

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


    //Allows the professor to delete a student
    // Written by Emma Hockett (ech210001) Started on November 19, 2024
    public async Task<string> DeleteStudent(string netid){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand("professor_delete_student", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@student_netid", netid);

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

    /* *************************************************************************************
    *   Procedure Calls for Teams                                                          *
    ****************************************************************************************/

    // Gets the teams for the section that the professor teaches
    // Written by Sarah Yakum (sny200000)
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

    // Gets a student team number
    // Written by Emma Hockett (ech210001)
    public async Task<int> GetTeamNumber(string netid)
    {
        using (var conn = new MySqlConnection(connectionString)){
            List<Team> teams = new List<Team>();
            
            string query = "SELECT TeamNum from MemberOf where StuNetID=@student_netID";
            
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    
                    cmd.Parameters.AddWithValue("@student_netID", netid);

                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        if(await reader.ReadAsync())
                        {
                            return reader.GetInt32(0);
                        }
                    }
                }
            }
            catch (Exception ex){
                Console.Write(ex.Message);
            
            }
            return -1;
        }
    }


    //Get Team members for each team
    // Written by Sarah Yakum (sny200000)
    public async Task<List<Student>> GetTeamMembers(string section, int team_num){
        
        using (var conn = new MySqlConnection(connectionString)){
            List<Student> students = new List<Student>();
            
            
            string query = "SELECT StuNetID from MemberOf where TeamNum=@TeamNum and SecCode=@section_code";
            
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand(query, conn)){
                    

                    cmd.Parameters.AddWithValue("@TeamNum", team_num);
                    cmd.Parameters.AddWithValue("@section_code", section);

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

    // Gets all of the students who are not assigned to a team 
    // Written by Emma Hockett (ech210001)
    public async Task<List<Student>> GetUnassignedStudents(string section){
        
        using (var conn = new MySqlConnection(connectionString)){
            List<Student> students = new List<Student>();
            
            string query = "SELECT * from Student WHERE StuNetID NOT IN (SELECT StuNetID FROM MemberOf WHERE SecCode = @section_code) AND StuNetID IN (SELECT StuNetID FROM Attends WHERE SecCode = @section_code)";
            
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand(query, conn)){
                    cmd.Parameters.AddWithValue("@section_code", section);

                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        while (await reader.ReadAsync()){
                            students.Add(new Student{
                                netid= reader.GetString("StuNetID"),
                                name = reader.GetString("StuName"),
                                utdid = reader.GetString("StuUTDID")
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

    //Inserts a team number into for the section
    // Written by Sarah Yakum (sny200000)
    public async Task<string> InsertTeamNum(string section, int teamNum){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("professor_insert_team_num", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@section_code", section);
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

    // Allows the professot to edit a team number 
    // Written by Sarah Yakum (sny200000)
    public async Task<string> EditTeamNumber(string section, int teamNum, int updatedTeamNum){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("professor_edit_team_num", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@section_code", section);
                    cmd.Parameters.AddWithValue("@team_num", teamNum);
                    cmd.Parameters.AddWithValue("@new_team_num", updatedTeamNum);


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
    // Written by Sarah Yakum (sny200000)
    public async Task<string> DeleteTeam(string section, int teamNum){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("professor_delete_team", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
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

    //Changes a students assigned team
    // Written by Sarah Yakum (sny200000)
    public async Task<string> ChangeStuTeam(string section, string stuNetid, int newTeam){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("professor_change_student_team", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
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


    //Removes a student from a team 
    // Written by Sarah Yakum (sny200000)
    public async Task<string> RemoveFromTeam(string stuNetid){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("professor_remove_student_team", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@student_netID", stuNetid);


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
    // Written by Sarah Yakum (sny200000)
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

    // Checks whether a team number already exists for a section
    // Written by Emma Hockett (ech210001)
     public async Task<string> CheckTeamExists(string section, int teamNumber){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("check_if_team_exists", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@section_code", section);
                    cmd.Parameters.AddWithValue("@team_num", teamNumber);

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


    /* *************************************************************************************
    *   Procedure Calls for Time Tracking                                                  *
    ****************************************************************************************/

    //Get timeslot for student by date
    // Written by Sarah Yakum (sny200000)
    public async Task<List<Timeslot>> GetTimeslots(DateOnly date, string netId){
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
                                date = date,
                                description = reader.GetString("TSDate"),
                                hours = reader.GetString("TSDuration")
                                
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

    //Retrieves student's time slot from given date
    //Written by Sarah Yakum (sny200000)
    public async Task<Timeslot> GetTimeslot(DateOnly date, string netId){
        Timeslot timeslot = new Timeslot();
        //Dictionary<DateOnly, string> hours = new Dictionary<DateOnly, string>();
        //Dictionary<DateOnly, string> description = new Dictionary<DateOnly, string>();
        using(var conn = new MySqlConnection(connectionString)){
           
            try{
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand("student_timeslot_by_date", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@stu_netID", netId);
                    cmd.Parameters.AddWithValue("@input_date", date);
                     using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        while (await reader.ReadAsync())
                        {
                            
                            
                                timeslot.date = date;
                                timeslot.hours = reader.GetString("TSDuration");
                                timeslot.description = reader.GetString("Description");
                                
                                
                            
                        }
                    }
                }
            }
            catch (Exception ex){
                    Console.Write(ex.Message);

            }
        }
        return timeslot;
    }
    
    //Retrieves current weeks timeslot
    //Written by Sarah Yakum (sny200000)
    public async Task<List<Timeslot>> GetWeekTimeslots(DateOnly date, string netId){
        //string error_message = string.Empty;
        List<Timeslot> week = new List<Timeslot>();
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("student_timeslot_by_week", conn)){
                    cmd.CommandType=CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@stu_netID", netId);
                    cmd.Parameters.AddWithValue("@start_date",date);

                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        while (await reader.ReadAsync())
                        {
                            week.Add(new Timeslot{
                                studentName = reader.GetString("StuName"),
                                netId = reader.GetString("StuNetID"),
                                date = reader.GetDateOnly("TSDate"),
                                description = reader.GetString("TSDescription"),
                                hours = reader.GetString("TSDuration")
                                }
                            );
                        }
                    }
                }
            }
            catch(Exception ex){
                Console.WriteLine(ex.Message);
            }
        }
    
        return week;
    }

    //Modifies student's timeslot based on given information to the professor
    //Written by Sarah Yakum (sny200000)
    public async Task<string> EditTimeslot(string netid, DateOnly date, string desc, string time){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("professor_edit_timeslot", conn)){
                    cmd.CommandType=CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@student_netID", netid);
                    cmd.Parameters.AddWithValue("@ts_date", date);
                    cmd.Parameters.AddWithValue("@updated_description", desc);
                    cmd.Parameters.AddWithValue("@updated_duration", time);

                    

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

    //Retrieves the total amount of time the student has logged to date
    //Written by Sarah Yakum (sny200000)
    public async Task<string> GetTotalTime(string netid){

        int total = 0;
        string hours = string.Empty;
        string minutes = string.Empty;
               
        using(var conn = new MySqlConnection(connectionString)){
           
            try{
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand("student_total_time", conn)){
                    cmd.CommandType=CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@student_netID", netid);
                    
                    var result = new MySqlParameter("@student_total", MySqlDbType.Int32);
                    result.Direction=ParameterDirection.Output;
                    cmd.Parameters.Add(result);

                    await cmd.ExecuteNonQueryAsync();

                    total = Convert.ToInt32(result.Value);
                }
            }
            catch (Exception ex){
                    Console.Write(ex.Message);

            }
        }

        hours = (total / 60).ToString();
        minutes = (total % 60).ToString();


        return hours +":"+ minutes;

    }

    /* *************************************************************************************
    *   Procedure Calls for Peer Reviews                                                   *
    ****************************************************************************************/


    //Creates criteria for a review type in a specific section
    // Written by Sarah Yakum (sny200000)
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

    //Gets criteria based on the section 
    // Written by Sarah Yakum (sny200000)
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

    //Retrieves criteria ID for a section in order to edit it
    // Written by Sarah Yakum (sny200000)
     public async Task<string> GetAllCriteriaID(string netid, string section, string reviewType){
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

    //Retrieves criteria ID for a section in order to edit it
    // Written by Emma Hockett (ech210001)
     public async Task<int> GetCriteriaID(string section, string name, string description, string reviewType){

        string query = "SELECT CriteriaID FROM Criteria WHERE SecCode=@SecCode AND CriteriaName=@criteria_name AND CriteriaDescription=@criteria_description AND ReviewType=@review_type";
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand(query, conn)){
                    cmd.Parameters.AddWithValue("@SecCode", section);
                    cmd.Parameters.AddWithValue("criteria_name", name);
                    cmd.Parameters.AddWithValue("@criteria_description", description);
                    cmd.Parameters.AddWithValue("@review_type", reviewType);

                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        if(await reader.ReadAsync())
                        {
                            return reader.GetInt32(0);
                        }
                    }
                }
            }
            catch (Exception ex){
                Console.Write(ex.Message);
            }
        }
        return -1;
    }


    //Retrieves criteria ID for a section in order to edit it
    // Written by Emma Hockett (ech210001)
    public async Task<string> CheckCriteriaInPR(string section, string reviewType)
    {
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("check_type_in_pr", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;;
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
    // Written by Sarah Yakum (sny200000)
    public async Task<string> EditCriteria(string section, int criteriaID, string name, string description, string reviewType){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("professor_edit_criteria", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;;
                    cmd.Parameters.AddWithValue("@section_code",section);
                    cmd.Parameters.AddWithValue("@criteria_id", criteriaID);
                    cmd.Parameters.AddWithValue("@criteria_name", name);
                    cmd.Parameters.AddWithValue("@criteria_description", description);
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

    //Deletes a specific criteria
    // Written by Sarah Yakum (sny200000)
    public async Task<string> DeleteCriteria(string section, string criteriaName, string reviewType){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("professor_delete_criteria", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
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


    //Creates a peer review for a specific section with availability dates
    // Written by Sarah Yakum (sny200000)
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

    //Gets the peer reviews for a section
    // Written by Sarah Yakum (sny200000)
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
                            DateTime startDate = reader.GetDateTime("StartDate");
                            DateTime endDate = reader.GetDateTime("EndDate");
                            peerReviews.Add(new PeerReview{
                                section = secCode,
                                type = reader.GetString("ReviewType"),
                                startDate = DateOnly.FromDateTime(startDate),
                                endDate = DateOnly.FromDateTime(endDate)
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


    //Edits the dates the peer review is available
    // Written by Emma Hockett (ech210001)
     public async Task<string> EditPRDates(string section, string type, DateOnly startDate, DateOnly endDate){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand("edit_peer_review_dates", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@section_code", section);
                    cmd.Parameters.AddWithValue("@review_type", type);
                    cmd.Parameters.AddWithValue("@start_date", startDate);
                    cmd.Parameters.AddWithValue("@end_date", endDate);

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


    // Deletes Peer Reviews 
    // Written by Emma Hockett (ech210001)
     public async Task<string> DeletePR(string section, string type){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand("delete_peer_review", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@section_code", section);
                    cmd.Parameters.AddWithValue("@review_type", type);

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
   // Written by Sarah Yakum (sny200000)
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

    // Allows professor to check whether a peer review already exists 
    // (Helps in determing whether a team/ members can be edited or not)
    // Written by Emma Hockett (ech210001)
    public async Task<string> CheckPeerReviewStatus(int teamNum, string section){
        string error_message = string.Empty;
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using(MySqlCommand cmd = new MySqlCommand("check_peer_review_status", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@team_num", teamNum);
                    cmd.Parameters.AddWithValue("@section_code",section);

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
    // Written by Sarah Yakum (sny200000)
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

   
    /* *************************************************************************************
    *   Procedure Calls for Student Emails                                                 *
    ****************************************************************************************/

    //Retrieves email addresses for student who have not submitted their peer reviews
    // Written by Emma Hockett (ech210001)
    public async Task<List<string>> GetPREmails(string sectionCode){
        List<string> emails = new List<string>();
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand("peerReview_student_emails", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@section_code", sectionCode);

                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        while (await reader.ReadAsync())
                        {
                            string email = reader.GetString("Email");
                            emails.Add(email);
                        }
                    }
                }
                return emails;
            }
            catch(Exception ex){
                Console.WriteLine(ex.Message);
                return new List<string>();
            }
        }
    }

    //Retrieves email addresses for students who have not logged any hours for the week
    // Written by Emma Hockett (ech210001)
    public async Task<List<string>> GetTTEmails(string sectionCode, DateOnly firstDay){
        List<string> emails = new List<string>();
        using(var conn = new MySqlConnection(connectionString)){
            try{
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand("timetrack_student_emails", conn)){
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@section_code", sectionCode);
                    cmd.Parameters.AddWithValue("@start_week", firstDay);


                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        while (await reader.ReadAsync())
                        {
                            string email = reader.GetString("Email");
                            emails.Add(email);
                        }
                    }
                }
                return emails;
            }
            catch(Exception ex){
                Console.WriteLine(ex.Message);
                return new List<string>();
            }
        }
    }
}



