global using RandomOrg.CoreApi;
global using RandomOrg.CoreApi.Errors;
global using Newtonsoft.Json;
global using PasswordGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

namespace PasswordGenerator
{
    public class PasswordClass
    {
        public string Base { get; set; }
        public string? Output { get; set; }
        public int? UserID { get; set; }
        //still currently developing the constructor for the passwordclass

        public PasswordClass(string BasePass)
        {
            passwordBuilder builder = new passwordBuilder();
            this.Base = BasePass;
            
        }



    }


}
