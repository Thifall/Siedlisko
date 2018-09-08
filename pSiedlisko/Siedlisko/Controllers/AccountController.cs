using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Siedlisko.Models;
using Siedlisko.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.Controllers
{
    public class AccountController : Controller
    {
        #region Fields and Properties
        private SignInManager<SiedliskoUser> _signInManager;
        private UserManager<SiedliskoUser> _userManager;
        private static ILogger Logger;
        #endregion

        #region .ctor
        public AccountController(SignInManager<SiedliskoUser> signInManager, UserManager<SiedliskoUser> userManager, ILoggerFactory loggerFactory)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            if (Logger == null)
            {
                Logger = loggerFactory.CreateLogger(typeof(AccountController));
            }
            Logger.LogInformation("[AccountController][line 34] Created AccountController");
        }
        #endregion

        #region Actions
        public IActionResult Login(bool created)
        {
            if (created)
            {
                return View(new LoginViewModel() { Create = true });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
                if (user != null)
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, true, false);
                    if (signInResult.Succeeded)
                    {
                        if (string.IsNullOrWhiteSpace(returnUrl))
                        {
                            return RedirectToAction("Details", new { Id = user.UserName });
                        }
                        else
                        {
                            Redirect(returnUrl);
                        }
                    }
                }
            }
            loginViewModel.Password = "";
            loginViewModel.OperationResultsDescription = "Nieprawidłowe dane logowania";
            loginViewModel.OperationSucces = false;
            //ModelState.AddModelError("", "Incorect login details");
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Register", "Account");
            }

            var userToRegister = new SiedliskoUser()
            {
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email,
                Imie = registerViewModel.Imie,
                Nazwisko = registerViewModel.Nazwisko,
                PhoneNumber = registerViewModel.PhoneNumber
            };

            var createResult = await _userManager.CreateAsync(userToRegister);

            if (createResult.Succeeded)
            {
                var setPasswordResult = await _userManager.AddPasswordAsync(userToRegister, registerViewModel.Password);
                if (setPasswordResult.Succeeded)
                {
                    //userToRegister.Roles.Add(new IdentityUserRole<string>() { RoleId = "User" });
                    await _userManager.UpdateAsync(userToRegister);
                    return RedirectToAction("Login", "Account", new { created = true });
                }
                else
                {
                    await _userManager.DeleteAsync(userToRegister);
                    foreach (var result in setPasswordResult.Errors)
                    {
                        registerViewModel.ErrorMsg += result.Description + Environment.NewLine;
                    }
                    registerViewModel.Password = "";
                    registerViewModel.PasswordRepeat = "";
                    return View(registerViewModel);
                }
            }
            else
            {
                registerViewModel.Password = "";
                registerViewModel.PasswordRepeat = "";
                foreach (var item in createResult.Errors)
                {
                    registerViewModel.ErrorMsg += item.Description + Environment.NewLine;
                }
                return View(registerViewModel);
            }

        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }
            return RedirectToAction("Login");
        }

        [Authorize]
        public async Task<IActionResult> Details(string Id, bool updated)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrWhiteSpace(Id))
            {
                return RedirectToAction("Index", "Home");
            }

            var userDetailsviewModel = Mapper.Map<AccountDetailsViewModel>(await _userManager.FindByNameAsync(Id));
            if (userDetailsviewModel != null && User.Identity.Name == Id)
            {
                if (updated)
                {
                    userDetailsviewModel.ChangesSaved = true;
                    userDetailsviewModel.ChangeNotification = "Informacje zostały zapisane";
                }
                else
                {
                    if (Request.Query.Count > 0)
                    {
                        userDetailsviewModel.ChangeNotification = "Nie udało się zapisać danych!";
                    }
                }
                return View(userDetailsviewModel);
            }
            else
            {
                ModelState.AddModelError("", "Could not get data!");
                return RedirectToAction("Index", "Home");
            }

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(AccountDetailsViewModel detailsVM)
        {
            IdentityResult results = null;
            var user = await _userManager.FindByEmailAsync(detailsVM.Email);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "nieznany błąd");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(detailsVM.PhoneNumber))
                {
                    user.PhoneNumber = detailsVM.PhoneNumber;
                }

                if (!string.IsNullOrWhiteSpace(detailsVM.Imie))
                {
                    user.Imie = detailsVM.Imie;
                }

                if (!string.IsNullOrWhiteSpace(detailsVM.Nazwisko))
                {
                    user.Nazwisko = detailsVM.Nazwisko;
                }
                results = await _userManager.UpdateAsync(user);
            }

            if (results != null && results.Succeeded)
            {
                return RedirectToAction("Details", "Account", new { id = user.UserName, updated = true });
            }
            return RedirectToAction("Details", "Account", new { id = user.UserName, updated = false });
        }
        #endregion
    }
}
