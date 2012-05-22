// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseEntityServise.cs" company="">
//   
// </copyright>
// <summary>
//   The base entity service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MTP1.Service.Abstract
{
    using System.Data.Objects;
    using System.Linq;

    using MTP1.Models.Interface;
    using MTP1.Service.Interface;

    /// <summary>
    /// The base entity service.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public abstract class BaseEntityService<T> : IBaseService<T>
        where T : class, IBase, new()
    {
        // Объект Entity Framework
        #region Properties

        /// <summary>
        /// Gets EntitySet.
        /// </summary>
        protected abstract ObjectSet<T> EntitySet { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="dataObject">
        /// The data object.
        /// </param>
        public virtual void Add(T dataObject)
        {
            this.EntitySet.AddObject(dataObject);
        }

        // Удаление объекта
        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="dataObject">
        /// The data object.
        /// </param>
        public virtual void Delete(T dataObject)
        {
            this.EntitySet.DeleteObject(dataObject);
        }

        // Получение полного списка объектов
        /// <summary>
        /// The get.
        /// </summary>
        /// <returns>
        /// </returns>
        public virtual IQueryable<T> Get()
        {
            return from obj in this.EntitySet select obj;
        }

        // Получение объекта по его ID
        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        public virtual T Get(int id)
        {
            return (from obj in this.EntitySet where obj.ID == id select obj).FirstOrDefault();
        }

        // Получение неполного списка объекта
        // skip - сколько первых записей пропустить, take - сколько записей получить
        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="skip">
        /// The skip.
        /// </param>
        /// <param name="take">
        /// The take.
        /// </param>
        /// <returns>
        /// </returns>
        public virtual IQueryable<T> Get(int skip, int take)
        {
            return this.Get().Skip(skip).Take(take);
        }

        // Добавление объекта

        // Сохранение изменений
        /// <summary>
        /// The save.
        /// </summary>
        public void Save()
        {
            this.EntitySet.Context.SaveChanges();
        }

        #endregion
    }
}