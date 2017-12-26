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

        Task<int> AddUserAsync(User user);

        Task<IList<CoursePath>> GetCoursePathsAsync();

        Task<IList<Course>> GetCoursesAsync(string path);

        Task<int> AddClassAsync(EnrolledClass enrolledClass);
        

        //Task<IPagedList<User>> GetUsersAsync(UserPagedDataRequest request);



        //Task<User> GetUserByIdAsync(int userId);



        //Task UpdateUserAsync(User user);


    }
}
