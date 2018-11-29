using PagedList;
using MedicalEmergency.Domain.Entities.Manager;
using MedicalEmergency.Domain.Interfaces.Repositories.Manager;
using MedicalEmergency.Domain.Utilities;
using MedicalEmergency.Presentation.Manager.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web.Mvc;
using System.Web.Security;
using MedicalEmergency.Infrastructure.Data.Repository.Manager;

namespace MedicalEmergency.Presentation.Manager.Controllers
{
    [CustomAuthorize()]
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController()
        {
            _accountRepository = new AccountRepository();
        }

        // GET: Accounts
        public ActionResult Index(int? page)
        {
            var role = new Models.Roles();

            IList<Account> accounts;

            accounts = _accountRepository.GetAll().Where(x => x.ID == ((Account)Session["Account"]).ID).ToList();

            var list = accounts;

            int pageSize = 10;

            return View(list.ToPagedList(page ?? 1, pageSize));
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var account = _accountRepository.GetById(id);

            if (account == null)
                return HttpNotFound();
            
            return View(account);
        }

        [Authorize(Roles = "Admin")]
        // GET: Accounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Account account)
        {
            if (ModelState.IsValid)
            {
                account.EncryptPassword();

                _accountRepository.Add(account);

                return RedirectToAction("Index");
            }

            return View(account);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var account = _accountRepository.GetById(id);

            if (account == null)
                return HttpNotFound();

            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Account account)
        {
            if (ModelState.IsValid)
            {
                account.EncryptPassword();

                _accountRepository.Update(account);

                return RedirectToAction("Index");
            }

            return View(account);
        }

        // GET: Accounts/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var account =_accountRepository.GetById(id);

            if (account == null)
                return HttpNotFound();
            
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            _accountRepository.DeleteByID(id);

            return RedirectToAction("Index");
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Account account, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var accountInput = _accountRepository.Get(x => x.Login == account.Login).FirstOrDefault();

                if (accountInput != null)
                {
                    if (Equals(accountInput.Active, true))
                    {
                        if (Encrypt.VerifyMd5Hash(MD5.Create(), account.Password, accountInput.Password))
                        {
                            FormsAuthentication.SetAuthCookie(account.Login, false);

                            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && returnUrl.StartsWith("/\\"))
                                return Redirect(returnUrl);

                            Session["Account"] = accountInput;

                            return RedirectToAction("Index", "HealthUnit");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Senha informada Inválida!!!");

                            return View(new Account());
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Usuário sem acesso para usar o sistema!!!");

                        return View(new Account());
                    }
                }
                else
                {
                    ModelState.AddModelError("", "E-mail informado inválido!!!");

                    return View(new Account());
                }
            }

            return View(account);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            Session.Abandon();

            return RedirectToAction("Login");
        }
    }
}
