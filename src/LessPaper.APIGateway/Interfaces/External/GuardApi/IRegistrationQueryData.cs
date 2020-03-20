using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LessPaper.APIGateway.Interfaces.External.GuardApi
{
    public interface IRegistrationQueryData
    {
        string Email { get; set; }
        string HashedPassword { get; set; }
        string Salt { get; set; }
        string UserId { get; set; }
    }
}
