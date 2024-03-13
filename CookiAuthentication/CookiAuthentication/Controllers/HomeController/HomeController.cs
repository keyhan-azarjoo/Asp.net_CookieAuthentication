using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CookiAuthentication.Controllers.HomeController
{
    [Route("HomeController")]
    public class HomeController :Controller
    {
        [HttpPost]
        [Authorize(Policy = "Mypolicy")]
        public async Task<IActionResult> Login()
        {
            //await HttpContext.SignInAsync("default",new ClaimsPrincipal(
            //    new ClaimsIdentity(
            //        new Claim[]
            //        {
            //            new Claim(ClaimTypes.NameIdentifier , Guid.NewGuid().ToString())

            //        },
            //        "default")
            //    ));
            return Ok();

        }



        [HttpGet]
        [Authorize] // You can call an authorization for this Api like here
        public async Task<IActionResult> getInfos()
        {
            //await HttpContext.SignInAsync("default",new ClaimsPrincipal(
            //    new ClaimsIdentity(
            //        new Claim[]
            //        {
            //            new Claim(ClaimTypes.NameIdentifier , Guid.NewGuid().ToString())

            //        },
            //        "default")
            //    ));
            return Ok();

        }







        [HttpPost("/Login-In-OtherWay")] // You can call an authorization for this Api like here
        public async Task<IActionResult> Testsssss()
        {
            await HttpContext.SignInAsync("default", new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier , Guid.NewGuid().ToString())
                    },
                    "default")
                ),

                new AuthenticationProperties()
                {
                    IsPersistent = true,// showing that this cookie will expier
                });
               return Ok();

        }


    }
}
