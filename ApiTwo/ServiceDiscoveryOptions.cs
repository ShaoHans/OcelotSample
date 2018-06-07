using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApiTwo
{
    /// <summary>
    /// 服务治理第三方组件Consul相关配置参数
    /// </summary>
    public class ServiceDiscoveryOptions
    {
        public string ServiceName { get; set; }

        public ConsulOptions Consul { get; set; }
    }

    public class ConsulOptions
    {
        public string HttpEndPoint { get; set; }

        public TcpEndpoint TcpEndPoint { get; set; }
    }

    public class TcpEndpoint
    {
        public string Address { get; set; }

        public int Port { get; set; }

        public IPEndPoint ToIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(Address), Port);
        }
    }
}
