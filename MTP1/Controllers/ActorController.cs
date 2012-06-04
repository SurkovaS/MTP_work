// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActorDicController.cs" company="">
//   
// </copyright>
// <summary>
//   The actor dic controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

using MTP1.Helpers;
using MTP1.Models;

namespace MTP1.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using MTP.Controllers;

    using MTP1.Controllers.Abstract;
    using MTP1.Helpers;
    using MTP1.Models;
    using MTP1.Service.Factory;
    using MTP1.Service.Interface;

    /// <summary>
    /// The actor dic controller.
    /// </summary>
    public class ActorController : BaseController<Actor>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public ActorController(IBaseService<Actor> service)
            : base(service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        public ActorController()
            : this(ActorServiceFactory.Create())
        {
        }

        #endregion

        #region Public Methods and Operators

        internal class ActorDto
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public string WeightCoefficientValue { get; set; }
            public string QuantityValue { get; set; }
        }


        public ActionResult GetActorsForUseCase(int useCaseId, int page, int rows, string search, string sidx, string sord)
        {
            var actors =
                this.service.Get().Where(a => a.UseCase == useCaseId).ToList().Select(
                    a => new ActorDto
                    {
                        // эта бадяга затеивалась только чтобы взять ID из справочника
                        ID = a.ActorDic.ID,
                        Title = a.Title,
                        WeightCoefficientValue = a.WeightCoefficientValue,
                        QuantityValue = a.QuantityValue
                    }).ToList();

            List<ActorDic> allActorsDic = ActorDicServiceFactory.Create().Get().ToList();
            var missingActors = allActorsDic.Where(a => !actors.Any(b => b.ID == a.ID));
            actors.AddRange(missingActors.Select(a => new ActorDto { ID = a.ID, Title = a.Title }));

            int actorCount = actors.Count();
            var actorsWithPaging = actors.AsQueryable().ApplyPaging("Title", (page - 1) * rows, rows);

            var jsonData =
                new
                {
                    total = Paging.TotalPages(actorCount, rows),
                    page,
                    records = actorCount,
                    rows = (from m in actorsWithPaging
                            select
                                new
                                {
                                    id = m.ID,
                                    cell =
                            new[]
                                        {
                                            m.Title,
                                            m.WeightCoefficientValue,
                                            m.QuantityValue
                                        }
                                }).ToArray()
                };

            return this.Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
