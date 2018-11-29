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
using System.Security.Cryptography;
using Nethereum.Web3.Accounts.Managed;
using Nethereum.Web3;
using Nethereum.Geth;
using System.Threading;
using System.Numerics;
using Nethereum.Signer;

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
            ViewBag.nameSortParm = string.IsNullOrEmpty(sortOrder) ? "ContractID" : sortOrder;

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
        public async System.Threading.Tasks.Task<ActionResult> CreateAsync(EthereumContractViewModel ethereumContractViewModelViewModel)
        {
            string fileHash;

            if (ModelState.IsValid)
            {
                using (var md5 = MD5.Create())
                {
                    var hash = md5.ComputeHash(ethereumContractViewModelViewModel.FileUpload.InputStream);
                    fileHash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }

                var senderAddress = "0xCBE60Eaea0cB3fb07348c72F68D8ec9a604aa941";
                var password = "Eduardo1303";
                var abi = @"[{'constant':true,'inputs':[],'name':'getActive','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_active','type':'bool'}],'name':'setActive','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'getDocument','outputs':[{'name':'','type':'string'}],'payable':false,'stateMutability':'view','type':'function'},{'inputs':[{'name':'_requester','type':'address'},{'name':'_recipient','type':'address'},{'name':'_document','type':'string'}],'payable':false,'stateMutability':'nonpayable','type':'constructor'}]";

                var byteCode = "608060405234801561001057600080fd5b506040516103443803806103448339810160409081528151602080840151928401516001805461010060a860020a031916610100600160a060020a03808716919091029190911790915560028054600160a060020a03191691861691909117905590930180519193909161008a9160009190840190610093565b5050505061012e565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f106100d457805160ff1916838001178555610101565b82800160010185558215610101579182015b828111156101015782518255916020019190600101906100e6565b5061010d929150610111565b5090565b61012b91905b8082111561010d5760008155600101610117565b90565b6102078061013d6000396000f3006080604052600436106100555763ffffffff7c01000000000000000000000000000000000000000000000000000000006000350416629ebb10811461005a578063acec338a14610083578063b6f3f14d1461009f575b600080fd5b34801561006657600080fd5b5061006f610129565b604080519115158252519081900360200190f35b34801561008f57600080fd5b5061009d6004351515610132565b005b3480156100ab57600080fd5b506100b4610145565b6040805160208082528351818301528351919283929083019185019080838360005b838110156100ee5781810151838201526020016100d6565b50505050905090810190601f16801561011b5780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b60015460ff1690565b6001805460ff1916911515919091179055565b60008054604080516020601f60026000196101006001881615020190951694909404938401819004810282018101909252828152606093909290918301828280156101d15780601f106101a6576101008083540402835291602001916101d1565b820191906000526020600020905b8154815290600101906020018083116101b457829003601f168201915b50505050509050905600a165627a7a723058200425d4931a5ca733f20ef9228d18cb0c8d3e62f794101a25813ca3fcb04d8d070029";

                var account = new ManagedAccount(senderAddress, password);
                var web3 = new Web3(account);

                web3.TransactionManager.DefaultGas = BigInteger.Parse("290000");
                web3.TransactionManager.DefaultGasPrice = Transaction.DEFAULT_GAS_PRICE;

                var webGeth = new Web3Geth(account);

                webGeth.TransactionManager.DefaultGas = BigInteger.Parse("290000");
                webGeth.TransactionManager.DefaultGasPrice = Transaction.DEFAULT_GAS_PRICE; 

                var unlockAccount = await web3.Personal.UnlockAccount.SendRequestAsync(senderAddress, password, 120);

                await webGeth.Miner.Start.SendRequestAsync(6);

                var transactionHash = await web3.Eth.DeployContract.SendRequestAsync(abi, byteCode, senderAddress, new Nethereum.Hex.HexTypes.HexBigInteger(120), new Nethereum.Hex.HexTypes.HexBigInteger(120), "0xCBE60Eaea0cB3fb07348c72F68D8ec9a604aa941", "0xCBE60Eaea0cB3fb07348c72F68D8ec9a604aa941", fileHash);

                var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);

                while(receipt == null)
                {
                    Thread.Sleep(5000);
                    receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash); 
                }

                var mineResult = await webGeth.Miner.Stop.SendRequestAsync();

                var contractAddress = receipt.ContractAddress;

                var contract = web3.Eth.GetContract(abi, contractAddress);

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
    }
}
