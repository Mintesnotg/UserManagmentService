using Infrastructure.Appdbcontext;
using Infrastructure.Contracts;
using Infrastructure.Dtos;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Infrastructure.Enums.Enumerators;

namespace Infrastructure.Services
{
    public class RegisterPrevilages : IRegisterPreviliege
    {

        private readonly ApplicationDbContext _context;


        public RegisterPrevilages(ApplicationDbContext context)
        {
            _context= context;
        }
        public void RegisterPrivileges(Assembly assembly)
        {

            try
            {
                var privileges = _context.Privileges.ToList();
                var controllerWithActions = new List<ControllerWithAction>();
                List<Privilege> applicationPrivileges = [];


                var controllerTypesName = assembly.GetTypes().Where(type => !type.IsAbstract && typeof(ControllerBase).IsPublic && type.Name.Contains("Controller")).ToList();

                foreach (var controllerType in controllerTypesName)
                {
                    var controllerWithAction = new ControllerWithAction { ControllerType = controllerType, ControllerName = controllerType.Name.Replace("Controller", string.Empty), Actions = [] };
                    var methods = controllerType.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                              .Where(method => !method.IsSpecialName);
                    foreach (var method in methods)
                    {
                        controllerWithAction.Actions.Add(method.Name);
                    }
                    controllerWithActions.Add(controllerWithAction);

                }
                foreach (var controllerType in controllerWithActions)
                {
                    foreach (var action in controllerType.Actions)
                    {
                        string claim = controllerType.ControllerName + "-" + action;
                        var privilege = privileges.Where(c => c.Action.Equals(claim, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                        if (privilege == null)
                        {
                            if (applicationPrivileges.Select(c => c.Action).Contains(claim))
                                continue;
                            applicationPrivileges.Add(new Privilege
                            {
                                Action = claim,
                                Description = action,
                                RecordStatus = RecordStatus.Active,
                                RegisteredDate = DateTime.Now,
                                TimeZoneInfo = "E. Africa Standard Time"
                            });
                        }
                    }


                }

                if (applicationPrivileges.Count > 0)
                {
                    _context.AddRange(applicationPrivileges);
                    _context.SaveChanges();
                }
            }
            catch (Exception )
            {

                throw;
            }

     

        }
    }
}
