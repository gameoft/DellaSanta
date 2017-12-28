using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dellasanta.Logging;
using DellaSanta.Core;
using DellaSanta.DataLayer;
using DellaSanta.Logging;
using log4net;

namespace DellaSanta.Services
{
    public class UserService : IUserService
    {
        //private readonly IRepository<User> _repository;
        private ApplicationDbContext _applicationDbContext;
        private readonly ILog _log;

        public UserService(ApplicationDbContext applicationDbContext, ILogManager logManager)
        {
            _applicationDbContext = applicationDbContext;
            _log = logManager.GetLog(typeof(UserService));
        }

        public async Task<bool> ValidateCredentialsAsync(string userName, string password)
        {
            var user = await _applicationDbContext.Users.Where(x => x.UserName == userName).FirstOrDefaultAsync();
            if (null != user)
            {
                if (Utils.Hash(password) == user.Password)
                {
                    Log4NetHelper.Log(_log, "Login successful for user", LogLevel.INFO, userName, user.UserId, null, null);
                    return true;
                }
            }
            Log4NetHelper.Log(_log, "Login invalid credentials", LogLevel.WARN, userName + " | " + password, 0, null, null);
            return false;
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            var query = _applicationDbContext.Users
                .Where(x => x.UserName == userName);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IList<CoursePath>> GetCoursePathsAsync()
        {
            return await _applicationDbContext.CoursePaths.ToListAsync<CoursePath>() as IList<CoursePath>;
        }

        public async Task<IList<Course>> GetCoursesAsync(string path)
        {
            var nrPath = Int32.Parse(path);
            var courses = await _applicationDbContext.Courses.Where(x => x.CoursePathId == nrPath).ToListAsync<Course>();
            if (courses?.Count() > 0)
                return courses as IList<Course>;
            else
                return new List<Course>();
        }
        public async Task<int> AddUserAsync(User user)
        {
            try
            {
                _applicationDbContext.Users.Add(user);
                var result = await _applicationDbContext.SaveChangesAsync();
                if (result > 0)
                    return user.UserId;
                else
                    return -1;

            }
            catch (Exception)
            {
                return -1;
            }

            //transaction necessary when using asp.net identity that uses atomic transactions on the usermanager, when wanting to update other tables as well
            //using (var identitydbContextTransaction = _applicationDbContext.Database.BeginTransaction())
            //{
            //    try
            //    {
            //        _applicationDbContext.Users.Add(user);
            //        var result = await _applicationDbContext.SaveChangesAsync();
            //        if (result > 0)
            //        {
            //            identitydbContextTransaction.Commit();
            //            return user.UserId;
            //        }
            //        else
            //            return -1;

            //    }
            //    catch (Exception)
            //    {
            //        identitydbContextTransaction.Rollback();
            //        return -1;

            //    }
            //}
        }


        public async Task<int> AddClassAsync(EnrolledClass enrolledClass)
        {
            try
            {
                var course = _applicationDbContext.Courses.Where(x => x.CourseId == enrolledClass.CourseId).First();
                var student = _applicationDbContext.Users.Where(x => x.UserId == enrolledClass.StudentId).First();
                enrolledClass.Course = course;
                enrolledClass.CourseName = course.CourseName;
                enrolledClass.Student = student;
                enrolledClass.StudentName = student.FirstName + " " + student.LastName;

                if (0 == _applicationDbContext.EnrolledClasses.Where(x => x.CourseId == course.CourseId && x.StudentId == student.UserId).Count())
                {
                    _applicationDbContext.EnrolledClasses.Add(enrolledClass);
                    var result = await _applicationDbContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        return 1;
                    }
                    else
                        return -1;
                }
                else
                    return 1;

            }
            catch (Exception e)
            {
                return -1;

            }

        }



        //public async Task<IPagedList<User>> GetUsersAsync(UserPagedDataRequest request)
        //{
        //    var query = _repository.Entities.AsQueryable();

        //    if (!string.IsNullOrWhiteSpace(request.LastName))
        //        query = query.Where(x => x.LastName.StartsWith(request.LastName));

        //    if (!string.IsNullOrWhiteSpace(request.RoleName))
        //        query = query.Where(x => x.Roles.Any(r => r.Name == request.RoleName));

        //    if (request.Active.HasValue)
        //        query = query.Where(x => x.Active == request.Active.Value);

        //    string orderBy = request.SortField.ToString();
        //    if (QueryHelper.PropertyExists<User>(orderBy))
        //        query = request.SortOrder == SortOrder.Ascending ? query.OrderByProperty(orderBy) : query.OrderByPropertyDescending(orderBy);
        //    else
        //        query = query.OrderBy(x => x.LastName);

        //    var result = new PagedList<User>();
        //    await result.CreateAsync(query, request.PageIndex, request.PageSize);
        //    return result;
        //}



        //public async Task<User> GetUserByIdAsync(int userId)
        //{
        //    var query = _repository.Entities
        //        .Where(x => x.Id == userId);

        //    return await query.FirstOrDefaultAsync();
        //}



        //public async Task UpdateUserAsync(User user)
        //{
        //    if (user == null)
        //        throw new ArgumentNullException(nameof(user));

        //    _repository.Entities.AddOrUpdate(user);
        //    await _repository.SaveChangesAsync();
        //}
    }
}
