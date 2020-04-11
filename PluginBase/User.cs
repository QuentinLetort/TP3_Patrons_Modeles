namespace PluginBase
{
    public class User
    {
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }

        public User(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
        public override string ToString()
        {
            return "FirstName: "+ FirstName + " | LastName: " + LastName + " | Email: " + Email;
        }
    }
}