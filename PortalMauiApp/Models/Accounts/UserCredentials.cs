using SQLite;

namespace PortalMauiApp.Models.Accounts
{
    public class UserCredentials
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Createdon { get; set; }
        public DateTime? Updatedon { get; set; }
    }
}
