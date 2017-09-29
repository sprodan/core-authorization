namespace Authorization.Data
{
    public class RoleModule
    {
        public int Id { get; set; }    
        public Role Role { get; set; }
        public Module Module { get; set; }
    }
}