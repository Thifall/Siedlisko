using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Siedlisko.Models;
using SiedliskoCommon.Models;
using SiedliskoCommon.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.Controllers
{
    public class EmailsController : Controller
    {
        #region Fields and Properties
        private IConfigurationRoot _config;
        private SignInManager<SiedliskoUser> _signInManager;
        private UserManager<SiedliskoUser> _userManager;
        private EmailRepository _repository;
        #endregion

        #region ctor
        public EmailsController(IConfigurationRoot config, SignInManager<SiedliskoUser> signInManager, UserManager<SiedliskoUser> userManager, EmailRepository repository)
        {
            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;
            _repository = repository;
        }
        #endregion

        #region Actions
        [HttpGet("Api/GetEmailsToSend")]
        public async Task<IActionResult> GetEmailsToSend()
        {
            string credentials = Request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(credentials))
            {
                return Unauthorized();
            }

            if (!(await CheckCredentials(credentials)))
            {
                return Unauthorized();
            }
            return Ok(_repository.GetEmailMessages((x) => x.status == EmailStatus.ToSend));

        }

        [HttpPut("Api/UpdateEmail")]
        public async Task<IActionResult> UpdateEmail([FromBody]EmailMessage email)
        {
            //string credentials = Request.Headers["Authorization"];
            //if (string.IsNullOrWhiteSpace(credentials))
            //{
            //    return Unauthorized();
            //}

            //if (!(await CheckCredentials(credentials)))
            //{
            //    return Unauthorized();
            //}
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var message = await _repository.UpdateEmail(email);

                if (message != null)
                {
                    return Ok(message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error occured during action. Operation was unsuccessfull");
            }
            return BadRequest();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// checking if given credentials are existing account and able to log in
        /// </summary>
        /// <param name="credentials">credentials to check</param>
        /// <returns>true if login succeded, false otherwise</returns>
        private async Task<bool> CheckCredentials(string credentials)
        {
            var authDetails = Encoding.UTF8.GetString(Convert.FromBase64String(credentials.Split(' ').Last())).Split(':');

            if (authDetails.Length != 2)
            {
                return false;
            }

            var user = await _userManager.FindByNameAsync(authDetails[0]);
            if (user == null)
            {
                return false;
            }

            return (await _signInManager.PasswordSignInAsync(user, authDetails[1], false, false)).Succeeded;
        }
        #endregion
    }
}
