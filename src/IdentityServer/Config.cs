// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(), // subject id
                new IdentityResources.Profile(), // This scope value requests access to the End-User's default profile Claims, which are: name, family_name, given_name, middle_name, nickname, preferred_username, profile, picture, website, gender, birthdate, zoneinfo, locale, and updated_at.
                new IdentityResources.Email(),
                new IdentityResource("identity_role", new[]{ JwtClaimTypes.Role }),
                new IdentityResource("jt_resource_name", new[] { "jt_claim_type", "jt_claim_type2", JwtClaimTypes.WebSite, JwtClaimTypes.Address }),
                // Represent claims about a user like user ID, display name, email address etc…
                // eg. When this scope(jt_resource_name) is allowed, UserClaims in IdentityResource will add to client server claims(HttpContext.User.Claims), access token(jwt) do not have value.
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api1", "API1"),
                new ApiScope("api2", "API2", new[] { "jt_claim_type", "jt_claim_type2" }),
                new ApiScope("api_role", new[] { JwtClaimTypes.Role, "jt_claim_type", "jt_claim_type2" }),
                // Represent functionality a client wants to access. Typically, they are HTTP-based endpoints (aka APIs), but could be also message queuing endpoints or similar.
                // eg. When this scope(api2) is allowed, UserClaims in ApiScope will add to access token(jwt), client server claims(HttpContext.User.Claims) do not have value, must decode access token.
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "WebApiClient",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("WebApiClient".Sha256())
                    },
                    AllowedScopes = { "api_role" }
                },
                new Client
                {
                    ClientId = "MvcClient",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets =
                    {
                        new Secret("MvcClient".Sha256())
                    },
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:5002/signin-oidc" },
                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    AllowOfflineAccess = true,

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "identity_role",
                        "api1",
                        "api2",
                        "jt_resource_name",
                    }
                },
                new Client
                {
                    ClientId = "Front",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    ClientSecrets =
                    {
                        new Secret("Front".Sha256())
                    },
                    // where to redirect to after login
                    RedirectUris = { "http://localhost:8082" },
                    // where to redirect to after logout
                    //PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    AllowOfflineAccess = true,

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        //IdentityServerConstants.StandardScopes.Profile,
                        //IdentityServerConstants.StandardScopes.Email,
                        "api1",
                        "api2",
                        "api_role"
                        //"jt_resource_name",
                    },

                    AllowAccessTokensViaBrowser = true
                },
            };
    }
}