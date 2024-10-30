using MySqlConnector;

namespace MauiApp1.Models;

class DatabaseService{
    string connectionString = "Server=localhost;Database=seniordesignproject;User=root;Password=sarahyakum;";

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
                                    netid=reader.GetString("StuNetID")
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


}
