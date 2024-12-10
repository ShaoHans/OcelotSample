# Ocelot使用案例
### 解决方案中各项目介绍
* APIGatewayByOcelot：该项目的作用是API网关，采用 [Ocelot](https://github.com/ThreeMammals/Ocelot) 组件
* ApiOne：该项目为下游API服务测试项目，需使用 [Consul](https://github.com/hashicorp/consul) 组件注册自己
* ApiTwo：该项目为下游API服务测试项目，需使用 [Consul](https://github.com/hashicorp/consul) 组件注册自己
* Ids4Center.Mvc：该项目是服务认证授权中心，使用 [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) 组件
* AccessApiOne：客户端项目，访问需要授权的API接口

### Consul服务治理
1. [下载地址](https://www.consul.io/downloads.html)
2. 解压下载完成之后的压缩包，通过cmd命令行定位到consul.exe所在的目录，运行命令：consul.exe agent --dev
3. 通过浏览器访问：http://localhost:8500/ui ，此Consul管理端可以查看注册的服务信息

### 创建下游API服务项目
1. 创建两个Asp.net Core WebApi项目
2. 下载 Consul nuget包
3. 在appsettings.json文件中配置Consul服务端信息
   ```
	   /*服务治理第三方组件Consul相关配置参数*/
	  "ServiceDiscovery": {
		"ServiceName": "ApiOne", //本WebApi项目的服务名称，可以随便取名，将显示Consul的管理页面上，届时通过该服务名查找可用的服务站点
		"Consul": {
		  "HttpEndpoint": "http://127.0.0.1:8500",
		  "TcpEndpoint": {
			"Address": "127.0.0.1",
			"Port": 8600
		  }
		}
	  }
   ```
4. 编写服务注册代码，向Consul注册本项目提供的API服务，详见 RegisterToConsulExtension.cs 代码

### APIGatewayByOcelot网关项目
1. 下载 Ocelot nuget包，已经包含Consul的服务发现功能
2. 添加Ocelot.json配置文件，配置参数详见文件。需要注意的是，如果下游API服务需要授权才能访问，只要添加如下配置参数，并在StartUp类中编写相关代码
```
"AuthenticationOptions": {
	"AuthenticationProviderKey": "TestKey",
	"AllowedScopes": []
  }
```

### Ids4Center.Mvc认证授权中心
1. 下载 IdentityServer4 nuget包
2. 配置客户端和APIResource参数

### AccessApiOne客户端
1. 下载IdentityModel nuget包
2. 由于ApiOne应用的API接口是需要授权后才能访问，不能通过浏览器直接访问，所以单独下一个客户端项目访问

### 运行解决方案
1. 必须先运行Consul，cmd命令：consul.exe agent --dev
2. 启动项目ApiOne和ApiTwo，你可以打开项目所在目录通过命令行命令，每个项目都可以运行多个实例，如：打开ApiOne项目根目录，直接运行如下命令	
	> dotnet run --ip 127.0.0.1 --port 8000  
	> dotnet run --ip 127.0.0.1 --port 8001  
	这样就启动了ApiOne项目的两个实例，能接受ip和port参数是因为自己写的代码才支持这样运行，同理可以允许多个ApiTwo项目的实例
3. 启动Ids4Center.Mvc项目，该项目的端口写死了12345，不需要改动，因为客户端项目AccessApiOne也是写死了这个端口
4. 启动APIGatewayByOcelot网关项目，端口默认5000，SSL端口是5001。此时可以通过浏览器访问:https://localhost:5001/two/values 可以看到返回结果，
   但访问 https://localhost:5001/one/values 时并未看到任何结果，通过F12查看请求，可以看到服务端返回401（未授权），若要访问该接口，可以启动
   客户端AccessApiOne项目查看结果。
