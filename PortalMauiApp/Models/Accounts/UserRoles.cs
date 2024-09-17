using SQLite;

namespace PortalMauiApp.Models.Accounts
{
    public class UserRoles
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public required string RoleName { get; set; }
        
    }
}
