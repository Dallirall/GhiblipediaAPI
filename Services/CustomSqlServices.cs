using System.Reflection;

namespace GhiblipediaAPI.Services
{
    public class CustomSqlServices
    {

        public static string CreateInsertQueryStringFromObject(object obj, string tableName)
        {
            //TODO Dubbelkolla denna 
            try
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


                string query = $"INSERT INTO {tableName} ({columnNamesString}) VALUES ({sqlPlaceHolders})";

                return query;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        //public void AddObjectToDBTable(object toRegister, string tableName)
        //{
        //    try
        //    {
        //        PropertyInfo[] propertyInfo = toRegister.GetType()
        //                                                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        //                                                .Where(prop => prop.GetValue(toRegister) != null).ToArray();
        //        List<string> propertyNames = new List<string>();

        //        foreach (PropertyInfo propInf in propertyInfo)
        //        {
        //            propertyNames.Add(propInf.Name);
        //        }

        //        string columnNamesString = string.Join(", ", propertyNames);

        //        string sqlPlaceHolders = string.Join(", ", propertyNames.Select(prop => "@" + prop));



        //        string query = $"INSERT INTO {tableName} ({columnNamesString}) VALUES ({sqlPlaceHolders})";
        //        using (SqliteConnection connection = new SqliteConnection(ConnectionString))
        //        {
        //            connection.Execute(query, toRegister);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error: " + ex.Message);
        //    }
        //}
    }
}
