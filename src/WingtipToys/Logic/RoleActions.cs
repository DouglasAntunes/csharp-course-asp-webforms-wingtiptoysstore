using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WingtipToys.Models;

namespace WingtipToys.Logic
{
    internal class RoleActions
    {
        internal void AddUserAndRole()
        {
            // Access the application context and create result variables.
            ApplicationDbContext context = new ApplicationDbContext();
            IdentityResult idRoleResult;
            IdentityResult idUserResult;

            /*  Create a RoleStore object by using the ApplicationDbContext object. 
                The RoleStore is only allowed to contain IdentityRole objects.*/
            var roleStore = new RoleStore<IdentityRole>(context);

            /*  Create a RoleManager object that is only allowed to contain IdentityRole objects.
                When creating the RoleManager object, you pass in (as a parameter) a new RoleStore object. */
            var roleMngr = new RoleManager<IdentityRole>(roleStore);

            // Then, you create the "canEdit" role if it doesn't already exists.
            if (!roleMngr.RoleExists("canEdit"))
            {
                idRoleResult = roleMngr.Create(new IdentityRole { Name = "canEdit" });
            }

            /*  Create a UserManager object based on the UserStore object and the ApplicationDbContext  
                object. Note that you can create new objects and use them as parameters in
                a single line of code, rather than using multiple lines of code, as you did
                for the RoleManager object.*/
            var userMngr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var appUser = new ApplicationUser
            {
                UserName = "canedituser@wingtiptoys.com",
                Email = "canedituser@wingtiptoys.com",
            };
            idUserResult = userMngr.Create(appUser, "Pa$$word1");

            /*  If the new "canEdit" user was successfully created, add the "canEdit" user to
                the "canEdit" role.*/
            string canEditUserId = userMngr.FindByEmail("canedituser@wingtiptoys.com").Id;
            if (!userMngr.IsInRole(canEditUserId, "canEdit"))
            {
                idUserResult = userMngr.AddToRole(canEditUserId, "canEdit");
            }
        }
    }
}