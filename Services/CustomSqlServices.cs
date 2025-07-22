using System.Reflection;

namespace GhiblipediaAPI.Services
{
    //For building SQL strings dynamically. Not tested for security yet.
    public class CustomSqlServices
    {
        //Returns an INSERT SQL string for the passed object's properties that have assigned values. The values are in the form of placeholders ('@Value').
        public static string CreateInsertQueryStringFromObject(object obj, string tableName)
        {
            //Fixa nåt med prop.CustomAttributes
            PropertyInfo[] propertyInfo = obj.GetType()
                                                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                    .Where(prop => prop.GetValue(obj) != null).ToArray();
            List<string> propertyNames = new List<string>();

            foreach (PropertyInfo propInf in propertyInfo)
            {
                propertyNames.Add(propInf.Name);
            }

            string columnNamesString = string.Join(", ", propertyNames);

            string sqlPlaceHolders = string.Join(", ", propertyNames.Select(prop => "@" + prop));


            string query = $"INSERT INTO {tableName} ({columnNamesString}) VALUES ({sqlPlaceHolders})";

            return query;           
        }

        //Returns an UPDATE SQL string for the passed object's properties that have assigned values. The values are in the form of placeholders ('@Value').
        //Pass in the 'whereConditionClause' param in this example format: 'id = {obj.Id}'
        public static string CreateUpdateQueryStringFromObject(object obj, string tableName, string whereConditionClause)
        {            
            PropertyInfo[] propertyInfo = obj.GetType()
                                                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                    .Where(prop => prop.GetValue(obj) != null).ToArray();
            List<string> propertyNames = new List<string>();
                        

            foreach (PropertyInfo propInf in propertyInfo)
            {
                propertyNames.Add(propInf.Name);
            }

            string columnNamesString = string.Join(", ", propertyNames);

            string sqlPlaceHolders = string.Join(", ", propertyNames.Select(prop => "@" + prop));

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
