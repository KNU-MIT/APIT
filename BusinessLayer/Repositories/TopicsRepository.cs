using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Interfaces;
using DatabaseLayer;
using DatabaseLayer.Entities;

namespace BusinessLayer.Repositories
{
    public class TopicsRepository : ITopicsRepository
    {
        private readonly AppDbContext _ctx;

        public TopicsRepository(AppDbContext context)
        {
            _ctx = context;
        }

        public IEnumerable<Topic> GetAll() => _ctx.Topics;


        public Topic GetById(Guid id) => _ctx.Topics.FirstOrDefault(a => a.Id == id);

        public Topic GetByName(string name) => _ctx.Topics.FirstOrDefault(a => a.Name == name);

        public void SaveChanges() => _ctx.SaveChanges();


        public bool IsExist(Guid id) => _ctx.Topics.Any(a => a.Id == id);

        public bool IsExist(string name) => _ctx.Topics.Any(a => a.Name == name);


        public void Create(Topic entity)
        {
            if (entity == null) throw new ArgumentNullException();

            _ctx.Add(entity);
            SaveChanges();
        }

        public async void Update(Topic entity)
        {
            _ctx.Topics.Update(entity);
            await _ctx.SaveChangesAsync();
        }

        public void Delete(Topic entity)
        {
            _ctx.Topics.Remove(entity);
            SaveChanges();
        }
    }
}