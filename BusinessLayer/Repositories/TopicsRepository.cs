﻿using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Interfaces;
using DatabaseLayer;
using DatabaseLayer.Entities;
using Microsoft.EntityFrameworkCore;

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

        public void Create(Topic entity)
        {
            _ctx.Entry(entity).State = entity.Id == default ? EntityState.Added : EntityState.Modified;
            _ctx.SaveChanges();
        }

        public void Delete(Guid id)
        {
            _ctx.Topics.Remove(new Topic {Id = id});
            _ctx.SaveChanges();
        }
    }
}