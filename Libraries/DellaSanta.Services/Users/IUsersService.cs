using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DellaSanta.Core;

namespace DellaSanta.Services
{
    public interface IUserService
    {
        Task<bool> ValidateCredentialsAsync(string userName, string password);

        Task<User> GetUserByUserNameAsync(string userName);

        //Task<IPagedList<User>> GetUsersAsync(UserPagedDataRequest request);



        //Task<User> GetUserByIdAsync(int userId);

        //Task<int> AddUserAsync(User user);

        //Task UpdateUserAsync(User user);


    }
}
