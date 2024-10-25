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
}