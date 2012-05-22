// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="">
//   
// </copyright>
// <summary>
//   The base controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MTP1.Controllers.Abstract
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using MTP1.Models.Interface;
    using MTP1.Service.Interface;

    /// <summary>
    /// The base controller.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public abstract class BaseController<T> : Controller
        where T : class, IBase, new()
    {
        // Объект для для работы с данными
        #region Fields

        /// <summary>
        /// The service.
        /// </summary>
        protected IBaseService<T> service;

        #endregion

        // Параметризованный конструктор
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController{T}"/> class.
        /// </summary>
        /// <param name="_service">
        /// The _service.
        /// </param>
        public BaseController(IBaseService<T> _service)
        {
            this.service = _service;
        }

        #endregion

        // Получение списка объектов

        // Открытие формы создания объекта
        #region Public Methods and Operators

        /// <summary>
        /// The create.
        /// </summary>
        /// <returns>
        /// </returns>
        public virtual ActionResult Create()
        {
            return this.View();
        }

        // Обработка формы создания объекта
        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <returns>
        /// </returns>
        [HttpPost]
        public virtual ActionResult Create(FormCollection collection)
        {
            if (this.ModelState.IsValid)
            {
                var obj = new T();
                this.ChangeFormCollectionValues(obj, collection);
                UpdateModel(obj, collection);
                this.AddValuesOnCreate(obj);

                this.service.Add(obj);
                this.service.Save();

                return this.OnCreated(obj);
            }
            else
            {
                return this.View();
            }
        }

        // Открытие формы редактирование объекта

        // Открытие страницы с подтверждением удаления объекта
        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        public virtual ActionResult Delete(int id)
        {
            T obj = this.service.Get(id);
            if (obj == null)
            {
                return this.View("NotFound");
            }

            return View(obj);
        }

        // Удаление объекта после подтверждения
        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <returns>
        /// </returns>
        [HttpPost]
        public virtual ActionResult Delete(int id, FormCollection collection)
        {
            T obj = this.service.Get(id);
            if (obj == null)
            {
                return this.View("NotFound");
            }

            if (!obj.CanBeDeleted())
            {
                return View("DeleteFail", obj);
            }

            ActionResult onDeleted = this.OnDeleted(obj);

            this.OnDelete(obj);
            this.service.Delete(obj);
            this.service.Save();
            return onDeleted;
        }

        /// <summary>
        /// The details.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        public virtual ActionResult Details(int id)
        {
            T obj = this.service.Get(id);
            if (obj == null)
            {
                return this.View("NotFound");
            }

            return View(obj);
        }

        /// <summary>
        /// The edit.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        public virtual ActionResult Edit(int id)
        {
            T obj = this.service.Get(id);
            if (obj == null)
            {
                return this.View("NotFound");
            }

            return View(obj);
        }

        // Обработка формы редактирования объекта
        /// <summary>
        /// The edit.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <returns>
        /// </returns>
        [HttpPost]
        public virtual ActionResult Edit(int id, FormCollection collection)
        {
            T obj = this.service.Get(id);

            if (this.ModelState.IsValid)
            {
                this.ChangeFormCollectionValues(obj, collection);
                UpdateModel(obj, collection);
                this.service.Save();

                return this.OnEdited(obj);
            }
            else
            {
                return View(obj);
            }
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// </returns>
        public virtual ActionResult Index()
        {
            IEnumerable<T> objs = this.service.Get();
            return View(objs);
        }

        #endregion

        // Перенаправление к странице списка объектов

        // Автоматическое добавление свойств объекта, не получаемых с формы создания объекта
        #region Methods

        /// <summary>
        /// The add values on create.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        protected virtual void AddValuesOnCreate(T obj)
        {
        }

        /// <summary>
        /// The change form collection values.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <param name="collection">
        /// The collection.
        /// </param>
        protected virtual void ChangeFormCollectionValues(T obj, FormCollection collection)
        {
        }

        /// <summary>
        /// The on created.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// </returns>
        protected virtual ActionResult OnCreated(T obj)
        {
            return this.ReturnToObject(obj);
        }

        // Действия при удалении объекта
        /// <summary>
        /// The on delete.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        protected virtual void OnDelete(T obj)
        {
        }

        /// <summary>
        /// The on deleted.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// </returns>
        protected virtual ActionResult OnDeleted(T obj)
        {
            return this.ReturnToList(obj);
        }

        /// <summary>
        /// The on edited.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// </returns>
        protected virtual ActionResult OnEdited(T obj)
        {
            return this.ReturnToObject(obj);
        }

        /// <summary>
        /// The return to list.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// </returns>
        protected virtual ActionResult ReturnToList(T obj)
        {
            return this.RedirectToAction("Index");
        }

        // Перенаправление к странице объекта
        /// <summary>
        /// The return to object.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// </returns>
        protected virtual ActionResult ReturnToObject(T obj)
        {
            return this.RedirectToAction("Details", new { id = obj.ID });
        }

        #endregion
    }
}