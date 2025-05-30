using MazraeatiBackOffice.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
namespace MazraeatiBackOffice
{
    public partial class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DataContext _context;
        private DbSet<T> _entities;
        public EfRepository(DataContext context)
        {
            _context = context;
        }
        public T GetById(object id)
        {
            return Entities.Find(id);
        }
        public void Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Entities.Add(entity);
            //_context.SaveChanges();
        }

        public int InsertFarms(T entity)
        {
            int nNewFarms = 0;
            if (entity == null)
                throw new ArgumentNullException("entity");

            Entities.Add(entity);
            //nNewFarms = _context.SaveChanges();
            return nNewFarms;
        }

        public T InsertEntity(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Entities.Add(entity);
            //_context.SaveChanges();
            return entity;
        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            try
            {
                _context.Entry(_context.Set<T>().Local.FirstOrDefault(entry => entry.Id.Equals(entity.Id))).State = EntityState.Detached;
            }
            catch (Exception) { }

            _context.Entry<T>(entity).State = EntityState.Modified;
            //_context.SaveChanges();
        }

        public int UpdateEntity(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            try
            {
                _context.Entry(_context.Set<T>().Local.FirstOrDefault(entry => entry.Id.Equals(entity.Id))).State = EntityState.Detached;
            }
            catch (Exception) { }

            _context.Entry<T>(entity).State = EntityState.Modified;
            //return _context.SaveChanges();
            return 1;
        }

        public void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Entities.Remove(entity);

            //_context.SaveChanges();
        }

        

        public virtual IQueryable<T> Table
        {
            get
            {
                return Entities;
            }
        }
        private DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _context.Set<T>();

                }
                return _entities;
            }
        }
    }
}
