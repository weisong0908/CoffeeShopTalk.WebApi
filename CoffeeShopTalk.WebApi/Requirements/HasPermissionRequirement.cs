using System;
using Microsoft.AspNetCore.Authorization;

namespace CoffeeShopTalk.WebApi.Requirements
{
    public class HasPermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; set; }
        public string Issuer { get; set; }

        public HasPermissionRequirement(string permission, string issuer)
        {
            Permission = permission ?? throw new ArgumentNullException(nameof(permission));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(permission));
        }
    }
}