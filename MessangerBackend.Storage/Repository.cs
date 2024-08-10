﻿using System.Linq.Expressions;
using MessangerBackend.Core.Interfaces;
using MessangerBackend.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace MessangerBackend.Storage;

public class Repository : IRepository
{
    private readonly MessangerContext _context;

    public Repository(MessangerContext context)
    {
        _context = context;
    }

    public Task<T> Add<T>(T entity) where T : class
    {
        throw new NotImplementedException();
    }

    public Task<T> Update<T>(T entity) where T : class
    {
        throw new NotImplementedException();
    }

    public Task<T> Delete<T>(int id) where T : class
    {
        throw new NotImplementedException();
    }

    public Task<T> GetById<T>(int id) where T : class
    {
        throw new NotImplementedException();
    }

    public IQueryable GetAll<T>() where T : class
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> GetQuery<T>(Expression<Func<T, bool>> func) where T : class
    {
        return _context.Set<T>().Where(func);
    }
    
    public async Task<T> GetByNicknameAsync<T>(string nickname) where T : class, IUserWithNickname
    {
        return await _context.Set<T>().SingleOrDefaultAsync(u => u.Nickname == nickname);
    }
}