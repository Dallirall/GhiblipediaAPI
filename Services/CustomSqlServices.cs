using Newtonsoft.Json;
using System.Reflection;
using System.Text.Json.Serialization;

namespace GhiblipediaAPI.Services
{
    //For building SQL strings dynamically. Not tested for security yet.
    public class CustomSqlServices
    {
        //Returns an INSERT SQL string for the passed DTO's [JsonPropertyName] name value (of properties with assigned values).
        //The VALUES of the query string are in the form of placeholders ('@Value').
        public static string CreateInsertQueryStringFromDTO(object dto, string tableName)
        {
            PropertyInfo[] propertyInfo = dto.GetType()
                                             .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                             .Where(prop => prop.GetValue(dto) != null).ToArray();

            List<string> propertyNames = propertyInfo.Select(prop => prop.GetCustomAttribute<JsonPropertyNameAttribute>())
                                                     .Where(jpa => jpa != null)
                                                     .Select(jpa => jpa.Name).ToList();

            string columnNamesString = string.Join(", ", propertyNames);

            string sqlPlaceHolders = string.Join(", ", propertyInfo.Select(prop => "@" + prop.Name));


            string query = $"INSERT INTO {tableName} ({columnNamesString}) VALUES ({sqlPlaceHolders})";

            return query;           
        }

        //Returns an UPDATE SQL string for the passed DTO's properties that have assigned values. The values are in the form of placeholders ('@Value').
        //Pass in the 'whereConditionClause' param in this example format: 'id = {obj.Id}'
        public static string CreateUpdateQueryStringFromDTO(object dto, string tableName, string whereConditionClause)
        {            
            PropertyInfo[] propertyInfo = dto.GetType()
                                             .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                             .Where(prop => prop.GetValue(dto) != null).ToArray();
            
            List<string> propertyNames = propertyInfo.Select(prop => prop.GetCustomAttribute<JsonPropertyNameAttribute>())
                                                     .Where(jpa => jpa != null)
                                                     .Select(jpa => jpa.Name).ToList();

            string columnNamesString = string.Join(", ", propertyNames);

            string sqlPlaceHolders = string.Join(", ", propertyInfo.Select(prop => "@" + prop.Name));

            string query = "";
            if (propertyNames.Count > 1)
            {
                query = $"UPDATE {tableName} SET ({columnNamesString}) = ({sqlPlaceHolders}) WHERE {whereConditionClause};";
            }
            else
            {
                query = $"UPDATE {tableName} SET {columnNamesString} = {sqlPlaceHolders} WHERE {whereConditionClause};";
            }

            return query;
        }


    }
}
