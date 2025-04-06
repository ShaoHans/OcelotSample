using Duende.IdentityServer.Models;

using System.Collections.Generic;

namespace Ids4Center.Mvc
{
    public class Config
    {
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client1",
                    ClientSecrets =
                    {
                        new Secret("secret1".Sha256())
                    },

                    // 允许授予类型
                    // GrantType.ClientCredentials（客户端模式），没有用户参与交互，直接通过客户端Id和secret进行授权
                    AllowedGrantTypes = { GrantType.ClientCredentials, GrantType.ResourceOwnerPassword },
                    // 该客户端可以访问的资源集合
                    AllowedScopes = { "one-api" }
                },
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("one-api","OneAPI接口"),
            };
        }
    }
}
