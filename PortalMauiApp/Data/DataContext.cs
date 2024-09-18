using PortalMauiApp.Models.Accounts;
using SQLite;

namespace PortalMauiApp.Data
{
    public class DataContext
    {
        private SQLiteConnection conn;
        public DataContext()
        {
            Init();
        }
        private void Init()
        {
            if (conn != null)
                return;

            try
            {
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string filePath = Path.Combine(documentsPath, "DapperDB.db3");
                if (!File.Exists(filePath))
                {
                    File.Create(filePath);
                }
                conn = new SQLiteConnection(filePath);
                conn.CreateTable<UserRoles>();
                conn.CreateTable<UserCredentials>();

                var existingUser = conn.Table<UserCredentials>();
                if (existingUser == null || existingUser.Count() < 1)
                {
                    var userCredentials = new UserCredentials
                    {
                        UserName = "vis.kumar1503@gmail.com",
                        Password = "1234" // Consider hashing the password in a real-world app
                    };
                    conn.Insert(userCredentials);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool RegisterUser(string username, string password, string role = "User")
        {
            // Check if the user already exists
            var existingUser = conn.Table<UserCredentials>().FirstOrDefault(u => u.UserName == username);
            if (existingUser != null)
            {
                // User already exists, return false or throw an exception
                return false;
            }

            // Insert the new user credentials
            var userCredentials = new UserCredentials
            {
                UserName = username,
                Password = password // Consider hashing the password in a real-world app
            };
            conn.Insert(userCredentials);

            // Optionally assign a role to the user
            var userRole = new UserRoles
            {
                RoleName = role,
            };
            conn.Insert(userRole);

            return true; // User successfully registered
        }

        public UserCredentials GetUserByEmail(string email)
        {
            // Perform a query to get the user with the specified email
            var user = conn.Table<UserCredentials>().FirstOrDefault(u => u.UserName == email);
            return user;
        }
    }
}
