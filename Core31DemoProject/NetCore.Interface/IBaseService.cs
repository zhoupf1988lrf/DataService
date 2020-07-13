using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Interface
{
    public interface IBaseService : IDisposable //是为了释放Context
    {
        #region Query
        /// <summary>
        /// 根据id查询实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T Find<T>(object id) where T : class;
        /// <summary>
        /// 提供对单表的查询
        /// 不推荐对外开放
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Obsolete("建议不使用,使用Query代表表达式目录树的代替")]
        IQueryable<T> Set<T>() where T : class;
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        IQueryable<T> Query<T>(Expression<Func<T, bool>> funcWhere) where T : class;
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="funcWhere"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="funcOrderBy"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        PageResult<T> QueryPage<T, S>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<T, S>> funcOrderBy, bool isAsc = true) where T : class;
        #endregion
        #region Add
        /// <summary>
        /// 新增数据，即时Commit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        T Insert<T>(T t) where T : class;
        /// <summary>
        /// 新增数据，即时Commit
        /// 多条sql 一个连接，事务插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        /// <returns></returns>
        IEnumerable<T> Insert<T>(IEnumerable<T> tList) where T : class;
        #endregion
        #region Update
        /// <summary>
        /// 更新数据，即时Commit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        void Update<T>(T t) where T : class;
        /// <summary>
        /// 更新数据，即时Commit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        void Update<T>(IEnumerable<T> tList) where T : class;
        #endregion
        #region Delete
        /// <summary>
        /// 根据主键删除数据，即时Commit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        void Delete<T>(object id) where T : class;
        /// <summary>
        /// 删除数据，即时Commit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        void Delete<T>(T t) where T : class;
        /// <summary>
        /// 删除数据，即时Commit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        void Delete<T>(IEnumerable<T> tList) where T : class;

        #endregion

        #region Other
        /// <summary>
        /// 立即保存 全部修改
        /// 把增删的savechanges给放到这里，是为了保证事物的
        /// </summary>
        void Commit();
        /// <summary>
        /// 执行sql返回集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IQueryable<T> ExcuteQuery<T>(string sql, SqlParameter[] parameters) where T : class;
        /// <summary>
        /// 执行sql，无返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        void Excute<T>(string sql, SqlParameter[] parameters) where T : class;
        #endregion
    }
}
