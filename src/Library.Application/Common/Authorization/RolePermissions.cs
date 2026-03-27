using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Common.Authorization
{
    public static class RolePermissions
    {
        public static List<string> GetPermissions(string role)
        {
            return role switch
            {
                "Admin" => new List<string>
                {
                    Permissions.BookCreate,
                    Permissions.BookDelete,
                    Permissions.BookView
                },

                "User" => new List<string>
                {
                    Permissions.BookView
                },

                _ => new List<string>()
            };
        }
    }
}
