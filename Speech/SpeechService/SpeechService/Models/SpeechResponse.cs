using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AN.NEOCCS.Core.Assortment.Models
{
    public class SpeechResponse
    {
        public string message { get; set; }
        public SpeechResponse(string message)
        {
            this.message = message;
        }
    }
}
