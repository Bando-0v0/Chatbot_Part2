using System;

namespace chatbot
{
    public class User
    {
        public string Name { get; set; }
        public DateTime FirstLogin { get; set; }
        public User(string name)
        {
            Name = name;
            FirstLogin = DateTime.Now;
        }
    }
}