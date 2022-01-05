using System;


namespace DO
{
    /// <summary>
    /// Customer
    /// </summary>
    public struct User
    {
        public int? Id { get; set; } //optional, null for manager
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsManager { get; set; }
        public override string ToString()
        {
            if (IsManager == false)
                return $"username: {UserName}, password: {Password}";
            else
                return $"managment- username: {UserName}, password: {Password}";
        }
    }
}
