﻿using BugFixer.domain.Entities.Account;
using BugFixer.domain.Entities.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.InterFaces
{
    public interface IUserRepository
    {
        Task<bool> IsExistUserByEmail(string email);

        Task CreateUser(User user);

        Task SaveChanges();

        Task<User> GetUserByEmail(string email);

        Task<User> GetUserByActivationCode(string activationCode);

        void UpdateUser(User user);

        Task<User> GetUserById(long userId);

        IQueryable<User> GetAllUsers();

        Task<bool> CheckUserHasPermission(long userId, long permissionId);
    }
}
