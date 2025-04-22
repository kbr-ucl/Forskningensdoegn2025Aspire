# Guide

## Projektet

Projektet består af en web-frontend der taler med en backend.

Backenden består:

- En API gateway (YARP) som er indgangen til backenden.

- To services der indeholder hver sin feature og dennes business logic.

  - Hver service har sin egen SQL database.

  - Der anvendes Entity Framework til at interagere med databasen.

    

## Projekt struktur

Hele løsningen samles i én solution.

**Step 1** Opret en "blank solution" kaldet Forskningensdoegn2025Aspire

**Step 2** Opret en solution folder kaldet Feature

**Step 3** Under Feature folderen. Opret et solution folder kaldet ServiceA

**Step 4** Under Feature folderen. Opret et solution folder kaldet ServiceB

**Step 5** Under Feature folderen. Opret et solution folder kaldet ApiGateway



## Iteration 1

Først vil vi lave en "hello world", hvor vi opretter ServiceA og ServiceB med et hello endpoint, der svare med hhv. "Hello from ServiceA" og "Hello from ServiceB". Herefter opretter vi en YARP ApiGateway foran de to services og ruter alle /ServiceA kald til ServiceA og alle /ServiceB kald til ServiceB. Alle komponenter orkestreres via Aspire. Endelig opretter vi en test, så vi kan tjekke at alt virker. Både ServiceA og ServiceB oprettes som "Minimal API" projekter. Men vi opsætter IKKE Aspire i denne iteration

### ServiceA
Links:

- https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/responses?view=aspnetcore-9.0

  

I solution folderen ServiceA Opret "Empty ASP.NET Core Web API project" kaldet ServiceA
Vælg "Enlist in .NET Aspire orchestration"

![image-20250422203308681](assets/image-20250422203308681.png)

**Tilret Program.cs**

```c#
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/hello", () =>
{
    app.Logger.LogInformation("ServiceA got Hello request");
    var greeting = new HelloResponse("Hello from ServiceA");
    return greeting;
});

app.Run();

internal record HelloResponse(string Greeting)
{
}
```



### ServiceB

I solution folderen ServiceB Opret "Empty ASP.NET Core Web API project" kaldet ServiceB

Vælg "Enlist in .NET Aspire orchestration"

**Tilret Program.cs**

```c#
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/hello", () =>
{
    app.Logger.LogInformation("ServiceB got Hello request");
    var greeting = new HelloResponse("Hello from ServiceB");
    return greeting;
});

app.Run();

internal record HelloResponse(string Greeting)
{
}
```




### ApiGateway

Links:

- https://timdeschryver.dev/blog/integrating-yarp-within-dotnet-aspire
- https://www.milanjovanovic.tech/blog/how-dotnet-aspire-simplifies-service-discovery

I solution folderen ApiGateway Opret "Empty ASP.NET Core Web API project" kaldet Gateway

Vælg "Enlist in .NET Aspire orchestration"

**Nuget pakker:**

Tilføj disse nuget pakker:

- Yarp.ReverseProxy
- Microsoft.Extensions.ServiceDiscovery.Yarp



**Tilret Program.cs**

```c#
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add YARP services
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    // Configures a destination resolver that can use service discovery
    .AddServiceDiscoveryDestinationResolver();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapReverseProxy();

app.Run();
```



**Tilret appsettings.json**

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",

  "ReverseProxy": {
    "Routes": {
      "servicea-route": {
        "ClusterId": "servicea-cluster",
        "Match": {
          "Path": "/servicea/{**catch-all}"
        },
        "Transforms": [
          { "PathRemovePrefix": "/servicea" }
        ]
      },
      "serviceb-route": {
        "ClusterId": "serviceb-cluster",
        "Match": {
          "Path": "/serviceb/{**catch-all}"
        },
        "Transforms": [
          { "PathRemovePrefix": "/serviceb" }
        ]
      }
    },
    "Clusters": {
        "servicea-cluster": {
          "Destinations": {
            "destination1": {
              "Address": "http://localhost:[PORT]"
            }
          }
        },
        "serviceb-cluster": {
          "Destinations": {
            "destination1": {
              "Address": "http://localhost:[PORT]"
            }
          }
        }
      }
    }
}
```

OBS: [PORT] skal udskiftes med de konkrete porte - se i hhv. ServiceA og ServiceB filerne "launchSettings.json"

**Tilret Gateway.http**

```
@Gateway_HostAddress = http://localhost:[PORT]

GET {{Gateway_HostAddress}}/ServiceA/Hello
Accept: application/json

###
GET {{Gateway_HostAddress}}/ServiceB/Hello
Accept: application/json
```

OBS [PORT] skal udskiftes med de konkrete porte - se i Gateway filen "launchSettings.json"

### Test

Hvis ikke projektet "Forskningensdoegn2025Aspire.AppHost" står til at være startup projekt, skal du vælge "Forskningensdoegn2025Aspire.AppHost" som startup projekt.

Kør løsningen - følgende skærmbillede bør dukke op (port numre kan være anderledes hos dig)

![image-20250422212151288](assets/image-20250422212151288.png)

Dette er Aspire monitor billedet - det vender vi tilbage til senere.

Åben nu filen Gateway.http (i Visual Studio)

Klik "Send request" og bemærk resultatet:

![image-20250422212513008](assets/image-20250422212513008.png)



Prøv så den anden Send request og bemærk at greeting ændre sig til "Hello from ServiceB".



### Done

Iteration 1 er afsluttet da vi har en succesfuld test (hvilket var vores DoD kriterie)



## Iteration 2

I denne iteration er der fokus på Aspire orkestrering.

### Forskningensdoegn2025Aspire.AppHost

**Tilret Program.cs**

```c#
var builder = DistributedApplication.CreateBuilder(args);

var serviceA = builder.AddProject<Projects.ServiceA>("servicea");

var serviceB = builder.AddProject<Projects.ServiceB>("serviceb");

var gateway = builder.AddProject<Projects.Gateway>("gateway")
    .WithReference(serviceA)
    .WithReference(serviceB);


builder.Build().Run();
```



### ApiGateway

Den vigtige ændring er her!

Nu udskiftes host og port med orkestrerings info fra Aspire

Links:

- https://www.milanjovanovic.tech/blog/how-dotnet-aspire-simplifies-service-discovery



**Tilret appsettings.json**

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",

  "ReverseProxy": {
    "Routes": {
      "servicea-route": {
        "ClusterId": "servicea-cluster",
        "Match": {
          "Path": "/servicea/{**catch-all}"
        },
        "Transforms": [
          { "PathRemovePrefix": "/servicea" }
        ]
      },
      "serviceb-route": {
        "ClusterId": "serviceb-cluster",
        "Match": {
          "Path": "/serviceb/{**catch-all}"
        },
        "Transforms": [
          { "PathRemovePrefix": "/serviceb" }
        ]
      }
    },
    "Clusters": {
        "servicea-cluster": {
          "Destinations": {
            "destination1": {
              "Address": "https+http://servicea"
            }
          }
        },
        "serviceb-cluster": {
          "Destinations": {
            "destination1": {
              "Address": "https+http://serviceb"
            }
          }
        }
      }
    }
}
```



### Test

Testen er den samme som Iteration 1 testen, idet det vi tester er at Aspire "Service Discovery" er sat rigtigt op.

Bemærk at testen er succesfuld

### Done

Iteration 2 er afsluttet da vi har en succesfuld test - vores abstrakte servicenavne i Apigatewayen virker (hvilket var vores DoD kriterie).
