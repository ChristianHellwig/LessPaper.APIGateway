using System.Threading.Tasks;

namespace LessPaper.APIGateway.Interfaces.External.WriteApi
{
    public interface IWriteApi
    {
        Task<bool> AddUser(string emailAddress, string hashedPassword, string salt, string userId);
    }
}
