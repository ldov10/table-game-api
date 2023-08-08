using System;

namespace UserService.Models.Dto
{
    public class AddressInfo
    {
        public string AddressString { get; set; }

        public string Zip { get; set; }

        public string Phone { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public Guid Identifier { get; set; }
    }
}
