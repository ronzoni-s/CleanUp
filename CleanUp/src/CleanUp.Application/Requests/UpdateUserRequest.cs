using System.Runtime.Serialization;

namespace CleanUp.Application.Common.Requests
{
    public partial class UpdateUserRequest 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
    }
}
