using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSWA.Models
{
    public class Proforma
    {
        public EncProforma encProforma { get; set; }
        public List<DetProforma> detProforma { get; set; }
    }
}