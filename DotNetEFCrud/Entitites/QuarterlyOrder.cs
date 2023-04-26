using System;
using System.Collections.Generic;

namespace DotNetEFCrud.Entitites
{
    public partial class QuarterlyOrder
    {
        public string? CustomerId { get; set; }
        public string? CompanyName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }
}
