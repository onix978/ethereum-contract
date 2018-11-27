using PagedList;
using SmartContractEthereum.Presentation.Manager.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Helpers;
using System.Web.Mvc;
using SmartContractEthereum.Domain.Interfaces.Repositories;
using SmartContractEthereum.Infrastructure.Data.Repository;
using SmartContractEthereum.Presentation.Manager.Helpers;
using SmartContractEthereum.Presentation.Manager.Utilities;
using SmartContractEthereum.Presentation.Manager.Models.EthereumContract;
using System.Net;

namespace SmartContractEthereum.Presentation.Manager.Controllers
{
    [CustomAuthorize()]
    public class EthereumContractController : Controller
    {
        private readonly IEthereumContractRepository _EthereumContractRepository;

        public EthereumContractController()
        {
            _EthereumContractRepository = new EthereumContractRepository();
        }

        public IList<EthereumContractViewModel> SearchBy(IList<EthereumContractViewModel> list, EthereumContractSearchModel search)
        {
            IList<EthereumContractViewModel> resultList = new List<EthereumContractViewModel>();

            if (search.IsAnyNotNullOrEmpty())
            {
                foreach (PropertyInfo pi in search.GetType().GetProperties())
                {
                    if (pi.PropertyType == typeof(string))
                    {
                        string value = (string)pi.GetValue(search);

                        if (!string.IsNullOrEmpty(value))
                        {
                            var property = typeof(EthereumContractViewModel).GetProperty(pi.Name);
                            resultList = resultList.Concat(list.Where(x => property.GetValue(x, null).ToString().Contains(value)).ToList()).ToList();
                        }
                    }
                    else if(pi.PropertyType == typeof(int))
                    {
                        int value = (int)pi.GetValue(search);

                        if (value != 0)
                        {
                            var property = typeof(EthereumContractViewModel).GetProperty(pi.Name);
                            resultList = resultList.Concat(list.Where(x => property.GetValue(x, null).ToString() == value.ToString()).ToList()).ToList();
                        }
                    }
                    else if (pi.PropertyType.IsEnum)
                    {
                        var property = typeof(EthereumContractViewModel).GetProperty(pi.Name);
                        var status = Enum.Parse(pi.PropertyType, pi.GetValue(search).ToString()) as Enum;

                        int enumValue = Convert.ToInt32(status);

                        if (enumValue > 0 && !string.IsNullOrEmpty(status.ToString()));
                            resultList = resultList.Concat(list.Where(x => property.GetValue(x, null).ToString().Contains(status.ToDescriptionString().ToUpper())).ToList()).ToList();
                    }
                }

                return resultList.Distinct().ToList();
            }

            return list.ToList();
        }

        public IList<EthereumContractViewModel> OrderBy(string sortOrder, bool? asc, IList<EthereumContractViewModel> EthereumContractViewModels)
        {
            ViewBag.nameSortParm = string.IsNullOrEmpty(sortOrder) ? "Name" : sortOrder;

            var property = typeof(EthereumContractViewModel).GetProperty(ViewBag.nameSortParm);

            if (property == null)
            {
                var nameSortParm = Helpers.ReflectionHelper.ReturnNamePropertyByDisplayName(typeof(EthereumContractViewModel), ViewBag.nameSortParm);
                property = typeof(EthereumContractViewModel).GetProperty(nameSortParm);
            }

            asc = asc ?? true;
            SortDirection sortDirection = asc == true ? SortDirection.Ascending : SortDirection.Descending;

            ViewBag.sortOrder = sortOrder;
            ViewBag.asc = asc.Value;

            if (sortDirection == SortDirection.Descending)
                return EthereumContractViewModels = EthereumContractViewModels.OrderByDescending(x => property.GetValue(x)).ToList();
            else
                return EthereumContractViewModels = EthereumContractViewModels.OrderBy(x => property.GetValue(x)).ToList();
        }

        public IPagedList<EthereumContractViewModel> Pagination(IList<EthereumContractViewModel> list, int? page, string sortOrder, EthereumContractSearchModel search, EthereumContractSearchModel currentFilter)
        {
            ViewBag.currentSort = sortOrder;

            if (search.IsAnyNotNullOrEmpty())
                page = 1;
            else
                search = currentFilter;

            ViewBag.page = page ?? 1;

            int pageSize = 10;

            return list.ToPagedList(page ?? 1, pageSize);
        }

        // GET: EthereumContractViewModel
        public ActionResult Index(int? page, string sortOrder, bool? asc, int? institutionType, int? emergencyType, EthereumContractSearchModel search, EthereumContractSearchModel currentSearchFilter)
        {
            if (search != null && !string.IsNullOrEmpty(search.Name))
            {
                ViewBag.currentFilter = search;
                ViewBag.Name = search.Name;
            }

            IList<EthereumContractViewModel> EthereumContractViewModels = _EthereumContractRepository.GetAll().Select(x => new EthereumContractViewModel() { ContractID = x.ContractID}).ToList();

            EthereumContractViewModels = SearchBy(EthereumContractViewModels, search);
            EthereumContractViewModels = OrderBy(sortOrder, asc, EthereumContractViewModels);

            var pageList = Pagination(EthereumContractViewModels, page, sortOrder, search, currentSearchFilter);

            return View(pageList);
        }

        // GET: EthereumContractViewModel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EthereumContractViewModel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EthereumContractViewModel ethereumContractViewModelViewModel)
        {
            if (ModelState.IsValid)
            {
                EthereumContractViewModel EthereumContractViewModel = new EthereumContractViewModel() { ContractID = ethereumContractViewModelViewModel.ContractID };
                _EthereumContractRepository.Add(EthereumContractViewModel);

                return RedirectToAction("Index");
            }

            return View(ethereumContractViewModelViewModel);
        }

        //// GET: EthereumContractViewModel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var EthereumContractViewModel = _EthereumContractRepository.GetById(id);

            if (EthereumContractViewModel == null)
                return HttpNotFound();

            return View(EthereumContractViewModel);
        }

        // POST: EthereumContractViewModel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EthereumContractViewModel EthereumContractViewModel)
        {
            if (ModelState.IsValid)
            {
                _EthereumContractRepository.Update(EthereumContractViewModel);

                return RedirectToAction("Index");
            }

            return View(EthereumContractViewModel);
        }

        // GET: EthereumContractViewModel/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        //    var EthereumContractViewModel = _EthereumContractViewModelRepository.GetById(id);

        //    if (EthereumContractViewModel == null)
        //        return HttpNotFound();

        //    return View(EthereumContractViewModel);
        //}

        //// POST: EthereumContractViewModel/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    _EthereumContractViewModelRepository.DeleteByID(id);

        //    return RedirectToAction("Index");
        //}
    }
}
