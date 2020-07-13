using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using NetCore.Interface;

namespace NetCore.Service
{
    public class BaseService : IBaseService
    {
        protected DbContext Context { get; private set; }
        public BaseService(DbContext dbContext)
        {
            this.Context = dbContext;
            //this.Context.Database.Log = sql => Console.WriteLine(sql);
        }


        public T Find<T>(object id) where T : class
        {
            return this.Context.Set<T>().Find(id);
        }
        [Obsolete("建议不使用,使用Query代表表达式目录树的代替")]
        public IQueryable<T> Set<T>() where T : class
        {
            return this.Context.Set<T>();
        }
        public IQueryable<T> Query<T>(Expression<Func<T, bool>> funcWhere) where T : class
        {
           return this.Context.Set<T>().Where(funcWhere);
        }

        public PageResult<T> QueryPage<T, S>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<T, S>> funcOrderBy, bool isAsc = true) where T : class
        {
            IQueryable<T> list = this.Context.Set<T>();
            if (funcWhere != null)
                list = list.Where(funcWhere);
            if (isAsc)
                list = list.OrderBy(funcOrderBy);
            else
                list = list.OrderByDescending(funcOrderBy);
            PageResult<T> page = new PageResult<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = list.Count(),
                DataList = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList()
            };
            return page;
        }





        public T Insert<T>(T t) where T : class
        {
            this.Context.Set<T>().Add(t);
            this.Commit();//写在这里，就不需要单独commit。 不写就需要
            return t;
        }

        public IEnumerable<T> Insert<T>(IEnumerable<T> tList) where T : class
        {
            this.Context.Set<T>().AddRange(tList);
            this.Commit(); //一个链接  多个sql
            return tList;
        }




        public void Update<T>(T t) where T : class
        {
            if (t == null) throw new Exception("t is null");
            this.Context.Set<T>().Attach(t);//将数据附加到上下文，支持实体修改和新实体，重置为UnChanged
            this.Context.Entry<T>(t).State = EntityState.Modified;
            this.Commit();//保存 然后重置为UnChanged
        }

        public void Update<T>(IEnumerable<T> tList) where T : class
        {
            foreach (T t in tList)
            {
                this.Update<T>(t);
            }
        }



        public void Commit()
        {
            this.Context.SaveChanges();
        }

        public void Delete<T>(object id) where T : class
        {
            T t = this.Find<T>(id);
            this.Context.Set<T>().Remove(t);
            this.Commit();
        }

        public void Delete<T>(T t) where T : class
        {
            this.Context.Set<T>().Remove(t);
            this.Commit();
        }

        public void Delete<T>(IEnumerable<T> tList) where T : class
        {
            this.Context.Set<T>().RemoveRange(tList);
            this.Commit();
        }

        public virtual void Dispose()
        {
            if (this.Context != null)
                this.Context.Dispose();
        }
        public IQueryable<T> ExcuteQuery<T>(string sql, SqlParameter[] parameters) where T : class
        {
            //return this.Context.Database.SqlQuery<T>(sql, parameters).AsQueryable();
            return this.Context.Set<T>().FromSqlRaw<T>(sql, parameters);
        }
        public void Excute<T>(string sql, SqlParameter[] parameters) where T : class
        {
            using (IDbContextTransaction trans = this.Context.Database.BeginTransaction())
            {
                try
                {
                    //this.Context.Database.ExecuteSqlCommand(sql, parameters);
                    this.Context.Database.ExecuteSqlRaw(sql, parameters);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();
                    throw ex;
                }
            }

        }


    }
}
