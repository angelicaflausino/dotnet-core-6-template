# Company Default API Template
Projeto Web Api baseado na estrutura do .NET Core 6.0 com foco em consumo de serviços da Microsoft Azure.
A arquitetura do projeto basea-se no modelo **DDD** *(Domain-Driven-Design)*, servindo como projeto base desde de sistemas complexos até os mais simples. Seus domínios são divididos em seis camadas:

![Company Default DDD](https://github.com/angelicaflausino/dotnet-core-6-template/blob/master/Wiki/images/companydefaultDDD.PNG)

## Ambiente de Desenvolvimento

**Ferramentas**

- [.NET 6.0](https://dotnet.microsoft.com/pt-br/download/dotnet/6.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/pt-br/vs/)
- [Azurite - Emulador local do Armazenamento do Microsoft Azure - opcional](https://learn.microsoft.com/pt-br/azure/storage/common/storage-use-azurite?tabs=visual-studio)

**Dependências**

**Company.Default.Api**
- [Microsoft.ApplicationInsights.AspNetCore](https://www.nuget.org/packages/Microsoft.ApplicationInsights.AspNetCore/2.21.0?_src=template)
- [Microsoft.AspNetCore.Authentication.JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer/6.0.12?_src=template#dependencies-body-tab)
- [Microsoft.AspNetCore.Authentication.OpenIdConnect](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.OpenIdConnect/6.0.12?_src=template)
- [Microsoft.Extensions.Azure](https://www.nuget.org/packages/Microsoft.Extensions.Azure/1.6.0?_src=template)
- [Microsoft.Graph](https://www.nuget.org/packages/Microsoft.Graph/4.52.0?_src=template)
- [Microsoft.Identity.Web](https://www.nuget.org/packages/Microsoft.Identity.Web/1.25.10?_src=template)
- [Microsoft.Identity.Web.MicrosoftGraph](https://www.nuget.org/packages/Microsoft.Identity.Web.MicrosoftGraph/1.25.10?_src=template)
- [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/6.2.3?_src=template)

**Company.Default.Cloud**
- [Azure.Data.Tables](https://www.nuget.org/packages/Azure.Data.Tables/12.7.1?_src=template)
- [Azure.Security.KeyVault.Secrets](https://www.nuget.org/packages/Azure.Security.KeyVault.Secrets/4.4.0?_src=template)
- [Azure.Storage.Blobs](https://www.nuget.org/packages/Azure.Storage.Blobs/12.14.1?_src=template)
- [Azure.Storage.Common](https://www.nuget.org/packages/Azure.Storage.Common/12.13.0?_src=template)
- [Azure.Storage.Queues](https://www.nuget.org/packages/Azure.Storage.Queues/12.12.0?_src=template)
- [Microsoft.ApplicationInsights](https://www.nuget.org/packages/Microsoft.ApplicationInsights/2.21.0?_src=template)
- [Microsoft.Extensions.Azure](https://www.nuget.org/packages/Microsoft.Extensions.Azure/1.6.0?_src=template)
- [Microsoft.Extensions.DependencyInjection.Abstractions](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection.Abstractions/7.0.0?_src=template)
- [Microsoft.Graph](https://www.nuget.org/packages/Microsoft.Graph/4.52.0?_src=template)

**Company.Default.Core**
- [AutoMapper](https://www.nuget.org/packages/AutoMapper/12.0.0?_src=template)
- [AutoMapper.Extensions.Microsoft.DependencyInjection](https://www.nuget.org/packages/AutoMapper.Extensions.Microsoft.DependencyInjection/12.0.0?_src=template)
- [FluentValidation](https://www.nuget.org/packages/FluentValidation/11.4.0?_src=template)
- [LinqKit.Core](https://www.nuget.org/packages/LinqKit.Core/1.2.3?_src=template)

**Company.Default.Domain**
- [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration/7.0.0?_src=template)
- [Microsoft.Extensions.Configuration.Abstractions](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Abstractions/7.0.0?_src=template)
- [Microsoft.Extensions.Configuration.Binder](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder/7.0.3?_src=template)
- [Microsoft.Extensions.Logging.Abstractions](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Abstractions/7.0.0?_src=template)
- [Microsoft.Extensions.Options](https://www.nuget.org/packages/Microsoft.Extensions.Options/7.0.1?_src=template)
- [System.Linq.Dynamic.Core](https://www.nuget.org/packages/System.Linq.Dynamic.Core/1.2.24?_src=template)

**Company.Default.Infra**
- [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/7.0.2?_src=template)
- [Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/7.0.2?_src=template)
- [Microsoft.EntityFrameworkCore.Relational](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational/7.0.2?_src=template)
- [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/7.0.2?_src=template)

**Company.Default.Tests**
- [coverlet.collector](https://www.nuget.org/packages/coverlet.collector/3.1.2?_src=template)
- [Microsoft.AspNetCore.StaticFiles](https://www.nuget.org/packages/Microsoft.AspNetCore.StaticFiles/2.2.0?_src=template)
- [Microsoft.Extensions.Configuration.Json](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Json/7.0.0?_src=template)
- [Microsoft.Extensions.Logging](https://www.nuget.org/packages/Microsoft.Extensions.Logging/7.0.0?_src=template)
- [Microsoft.Extensions.Logging.ApplicationInsights](https://www.nuget.org/packages/Microsoft.Extensions.Logging.ApplicationInsights/2.21.0?_src=template)
- [Microsoft.NET.Test.Sdk](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk/17.1.0?_src=template)
- [Moq](https://www.nuget.org/packages/Moq/4.18.4?_src=template)
- [xunit](https://www.nuget.org/packages/xunit/2.4.1?_src=template)
- [xunit.runner.visualstudio](https://www.nuget.org/packages/xunit.runner.visualstudio/2.4.3?_src=template)

## Template do Projeto

Para instalar o template do projeto no **Visual Studio 2022** baixe o arquivo [Company.Default.zip](https://github.com/angelicaflausino/dotnet-core-6-template/blob/master/Template/Company.Default.zip) na pasta **C:\Users\USER-NAME\Documents\Visual Studio 2022\Templates\ProjectTemplates**. Não é necessário descompactar o arquivo.

No Visual Studio, clique em **Criar um novo projeto**, pesquise por **Company** e selecione o template **Company.Default**

![Company Default Template](https://github.com/angelicaflausino/dotnet-core-6-template/blob/master/Wiki/images/companytemplate.PNG)

Na tela seguinte informe o nome do Projeto, selecione o diretório, marque a opção **Colocar a solução e o projeto no mesmo diretório** em seguida clique em **Criar**

![Company Default Project Name](https://github.com/angelicaflausino/dotnet-core-6-template/blob/master/Wiki/images/companyappname.PNG)

**Exemplo do projeto criado:**

![Company App Sample](https://github.com/angelicaflausino/dotnet-core-6-template/blob/master/Wiki/images/companysample.PNG)

