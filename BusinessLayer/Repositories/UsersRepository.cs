using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.DataServices;
using DatabaseLayer.ConfigModels;
using BusinessLayer.Interfaces;
using DatabaseLayer;
using DatabaseLayer.Entities;

namespace BusinessLayer.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext _ctx;
        private readonly ProjectConfig.UniqueAddressConfig _addressConfig;

        public UsersRepository(AppDbContext context, ProjectConfig config)
        {
            _ctx = context;
            _addressConfig = config.Content.UniqueAddress;
        }

        public IEnumerable<User> GetAll() => _ctx.Users;
        public User GetById(string id) => _ctx.Users.FirstOrDefault(u => u.Id == id);
        public bool IsExist(string id) => _ctx.Users.Any(u => u.Id == id);
        public void SaveChanges() => _ctx.SaveChanges();

        public User GetByEmail(string email) =>
            email == null ? null : _ctx.Users.FirstOrDefault(u => u.Email == email);


        public void Create(User entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _ctx.Users.Add(entity);
            SaveChanges();
        }

        public void Delete(string id)
        {
            _ctx.Users.Remove(new User {Id = id});
            SaveChanges();
        }

        public string GenerateUniqueAddress() =>
            DataUtil.GenerateUniqueAddress(this, _addressConfig.UserAddressSize);

        public User GetByUniqueAddress(string address) =>
            _ctx.Users.FirstOrDefault(a => a.ProfileAddress == address);

        public IEnumerable<User> GetLatest(ushort count) => throw new NotImplementedException();
    }
}