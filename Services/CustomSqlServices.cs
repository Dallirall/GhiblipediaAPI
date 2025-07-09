using System.Reflection;

namespace GhiblipediaAPI.Services
{
    public class CustomSqlServices
    {

        public static string CreateInsertQueryStringFromObject(object obj, string tableName)
        {
            //TODO Dubbelkolla denna 
            
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

            //int i = 0;
            //string query = $"UPDATE {tableName} SET ";
            //do
            //{
            //    query += $"{propertyInfo[i].Name} = @{propertyInfo[i].Name}";

            //    i++;

            //    if (i == propertyInfo.Length)
            //        break;

            //    query += " , ";

            //} while (i < propertyInfo.Length);
            //query += $" WHERE {conditionColumn} = {conditionValue};";

        }


    }
}
