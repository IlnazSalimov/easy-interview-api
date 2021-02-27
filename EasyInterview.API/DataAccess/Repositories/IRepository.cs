using EasyInterview.API.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyInterview.API.DataAccess.Repositories
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Получить сущность по идентификатору. В ряде случаев использование GetOrThrow более предпочтительно.
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        /// <returns>Сущность с указанным Id, если существует. Иначе - null.</returns>
        Task<TEntity> Get(int id);

        /// <summary>
        /// Загрузка всех объектов данной сущности
        /// </summary>
        /// <returns>Неупорядоченный список всех объектов</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Сохранить объект сущность
        /// </summary>
        /// <param name="entity">Сохраняемый объект</param>
        Task<int> Save(TEntity entity);

        /// <summary>
        /// Удалить объект сущности
        /// </summary>
        Task Delete(TEntity entity);
    }
}
