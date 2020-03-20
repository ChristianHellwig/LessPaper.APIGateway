using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LessPaper.APIGateway.Interfaces.External.GuardApi
{
    public interface IGuardApi
    {
        /// <summary>
        /// Register a user
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <param name="hashedPassword">Hashed password</param>
        /// <param name="salt">Used salt</param>
        /// <param name="userId">User Id</param>
        /// <returns>True if the registration was successful</returns>
        Task<bool> RegisterUser(string emailAddress, string hashedPassword, string salt, string userId);

        /// <summary>
        /// Return necessary user login information like the password hash
        /// </summary>
        /// <param name="email">Email address which identifies the user</param>
        /// <returns>Necessary data to login a user</returns>
        Task<IUserLoginData> GetUserLoginInformation(string email);
    }
}
