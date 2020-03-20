using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LessPaper.APIGateway.Interfaces.External.GuardApi
{
    public interface IUserLoginData
    {
        string PasswordHash { get; }
        string Salt { get;  }
        string UserId { get; }
    }
}
