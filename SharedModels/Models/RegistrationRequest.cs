using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models;

public class RegistrationRequest
{
    public UserData User { get; set; }
    public AuthData AuthData { get; set; }
}
