using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;
using Microsoft.Extensions.Configuration;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("CatalogApi")
                {
                    Scopes = new List<Scope>
                    {
                        new Scope("catalog")
                    }
                }, 
                new ApiResource("BasketApi")
                {
                    Scopes = new List<Scope>
                    {
                        new Scope("basket")
                    }
                }, 
                new ApiResource("OrderApi")
                {
                    Scopes = new List<Scope>
                    {
                        new Scope("order")
                    }
                },
                
            };
        }

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            return new[]
            {
                new Client
                {
                    ClientId = "BasketClient",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes =
                    {
                        "order", "catalog"
                    }
                    
                },
                new Client
                {
                    ClientId = "OrderClient",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes =
                    {
                        "basket", "catalog"
                    }
                    
                },
                new Client
                {
                    ClientId = "catalogswaggerui",
                    ClientName = "Catalog Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"http://localhost:5288/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"http://localhost:5288/swagger/" },

                    AllowedScopes =
                    {
                        "catalog"
                    }
                },
                new Client
                {
                    ClientId = "basketswaggerui",
                    ClientName = "Basket Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"http://localhost:5286/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"http://localhost:5286/swagger/" },

                    AllowedScopes =
                    {
                        "basket"
                    }
                },
                new Client
                {
                    ClientId = "orderswaggerui",
                    ClientName = "Order Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"http://localhost:5230/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"http://localhost:5230/swagger/" },

                    AllowedScopes =
                    {
                        "order"
                    }
                },
                new Client()
                {
                    ClientId = "ReactClient",
                    Enabled = true,
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = {"http://localhost:5173"},
                    PostLogoutRedirectUris = {"http://localhost:5173/signout-callback-oidc"},
                    AllowedCorsOrigins = {"http://localhost:5173"},
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "basket"
                    }
                },
                
            };
        }
    }
}

