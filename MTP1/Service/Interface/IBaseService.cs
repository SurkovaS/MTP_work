// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBaseService.cs" company="">
//   
// </copyright>
// <summary>
//   The i base service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MTP1.Service.Interface
{
    using System.Linq;

    using MTP1.Models.Interface;

    /// <summary>
    /// The i base service.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface IBaseService<T>
        where T : class, IBase, new()
    {
        // Получение всех записей из таблицы БД
        #region Public Methods and Operators

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="dataObject">
        /// The data object.
        /// </param>
        void Add(T dataObject);

        // Удаление записи из таблицы
        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="dataObject">
        /// The data object.
        /// </param>
        void Delete(T dataObject);

        /// <summary>
        /// The get.
        /// </summary>
        /// <returns>
        /// </returns>
        IQueryable<T> Get();

        // Получение одной записи с заданным ID
        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        T Get(int id);

        // Получение нескольких записей.
        // Параметр skip - сколько первых записей пропустить, параметр take - сколько записей получить
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
        IQueryable<T> Get(int skip, int take);

        // Добавление записи в таблицу

        // Сохранение внесённых изменений
        /// <summary>
        /// The save.
        /// </summary>
        void Save();

        #endregion
    }
}