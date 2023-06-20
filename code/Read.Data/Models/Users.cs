using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Read.Data.Models
{
    /// <summary>
    /// Version object of the class
    /// </summary>
    [Serializable]
    [KnownType(typeof(UsersDto))]
    public class UsersDto
    {
        public string UserName { get; set; }
        public string Hobbies { get; set; }
        public string Location { get; set; }
    }
}
