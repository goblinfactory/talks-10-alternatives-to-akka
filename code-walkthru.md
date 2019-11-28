# objectives

- least amount of code possible to demonstrate something that looks real, and use patterns and learning and real benefits that will be of use in a real live production environment under serious load.
- be able to run all the code locally, with a single checkout and build.
- not require a K8 cluster, or downloading and running any docker containers.
- not require any Amazon, or AWS services in order to truly grok the benefits of the patterns.
- show how at low loads, things works very simplistically.
    - when things go as planned, even simple architectures work really well.
        - except, when life happens, and services we depend on start behaving like distributed systems, then **This** happens. (be able to show what **This** looks like) and how  design philosophies like akka help us with pit of sucess.
            - lastly, show alternatives to akka that can be used in .net for similar benefits.

# code walk through

very quickly demonstrate two approaches to handling common problems

- building loosely coupled systems
- easily scale
- easy to understand and reason about
- no concurrency problems.
- yes, we inherit other problems. (so a big challenge is to make this comparison fair enough to surface any of the trade-offs with event/command sourcing.)

## the akka way

- delegated management (parent/child)
- retry on error 
- ignore error 500
- circuit breaker, handle backoff on 429
- loose coupling

## alternatives in code

- Polly
- MediatR
- Memstate

## what are we going to simulate?

Need to come up with a service that typically would be slow and can be parellelised.

- car insurance quote finder. A "panel" of API's to get the best quote. // panel.txt 
- for our demo we'll start 10 services, so that some can hang, and block independantly of the others.
- need to get a least 3 quotes before we can start streaming `IAsyncEnumerable` results back to the console app over `http2`.
 - will simulate this with an asycn console app, that will start to display results as they come in without blocking.
 - need to report on the progress, and update the UI.

# acceptance tests

## The challenge

**Assuming we have adequate redundancy, up stream services experiencing technical difficulties must not cause technical difficulties for down stream services.**

- different services we depend on have different capacities. When we make too many requests for the external service to handle, we need a way to slow down our requests so that we can run with maximum throughput without overwhelming the external service, and without slowing down access to other services. 

  - I will attempt to simulate multiple providers experiencing different types of technical difficulties. We do not want to exclude them from our quotes, but cannot slow down the rate at which we need to service quote requests. Therefore we will send every request to everyone on our panel, and those who can respond will, and those that cannot will behave as described below, allowing us to back off gracefully, so that we give them the maximum opportunity to provide quotes, while respecting their capacity. (i.e. without DDOS'ng them.)

  - car reg contains `429` should return `429` with  `Retry-After` header for 5 seconds decreasing until 5 seconds passed.
  - car reg contains `503` should return 503 `Service Unavailable`, a retry should succeed.
  - car reg contains `404` should return 404 `Not found`, a retry should return the same.
  - car reg contains `500` should return 500 `Internal server error`, a retry should return the same each time. 



### **backpressure in akka - `akka streams`**

### **alternative #1 : `Rx` (`reactive extensions`) and `throttle`, `buffer` and `sample`**

### **alternative #2 : `Polly` and `RateLimitPolicy`, not yet available.**

- https://github.com/App-vNext/Polly/wiki/Polly-Roadmap

### **alternative #3 : `Polly` and respect `Retry-After`**

- Respect `Retry-After` (e.g. `429`) as described here : https://github.com/App-vNext/Polly/issues/414
- have not tested. Notes appear to suggest not yet available in .NET core?



## The acceptance test

```csharp
GIVEN a panel of external insurance companies
WHEN we start to request quotes faster and faster until we overwhelm individual quote providers
THEN our panel must somehow slow down requests for that one provider, without compromising overall system quote throughput
```

### **back pressure**


### demo services that will be running

1. pre-create new blank webapi open with `vscode` This will give us a default weather app.

> dotnet new webapi -n

2. modify the weather app to return insurance quote. This will be our `QUOTE-PANEL` microservice


### demo microservice 

1. create a get-quote ASP.NET website that will consume the `QUOTE-PANEL` microservice, and have a strategy to deal with `429, 500, 503` respectively.

- `429` : we should back off, and temporarily stop additional requests from hitting this service until backup delay is passed.
- `503`
- `404`
- `500`

###


```csharp

throw new FooException(429,"woah there feller, slow down!");

```

3. build a console consumer app, open a file, 




1.

1. 



## music to code by

- https://www.youtube.com/watch?v=pDGjtJ658YY
- https://www.youtube.com/watch?v=kFF2i6sUj1M&list=PLL92dfFL9ZdKGfNONhiaV_ps0ChlEZko3
- https://www.youtube.com/watch?v=Ur_tXqaNXOI

### references

- https://www.restapitutorial.com/httpstatuscodes.html
- https://github.com/Havret/akka-net-asp-net-core
- https://havret.io/akka-net-asp-net-core ( How to integrate Akka.NET and ASP.NET Core )
- https://restfulapi.net/rest-put-vs-post/ (simple REST best practices doc)

TODO: 


Create alternative to Akka using Memstate, Polly and MediatR (see my training video for this!)
Write up a summary of conclusions.
Timer permitting rewrite using ProtoActor?
