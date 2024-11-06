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
        NETID:
*/

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

                error_message = result.Value.ToString();



            }
            return error_message;
        }
        catch(Exception ex){
            return "Error: " + ex.Message;
        }
       }

    }
    
    // Gets the sections that a professor teaches 
    public async Task<List<Section>> getSections(string netId){

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
    public async Task<string> changePassword(string netid, string oldPassword, string newPassword){
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

                    error_message = result.Value.ToString();

                

                }
                return error_message;
                

            }
            catch(Exception ex){
                return "Error: " + ex.Message;
            }
        }

    }

    // Gets the NetIDs of all of the students in the section that the professor teaches
    public async Task<List<Student>> getStudents(string code){

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
    public async Task<Student> getStudentsInfo(string netid){

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
                            studentName = reader["StuName"].ToString();
                            studentUtdId = reader["StuUTDID"].ToString();
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

    // Gets the teams for the section that the professor teaches
    public async Task<List<Team>> getTeams(string code){
        
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


}
