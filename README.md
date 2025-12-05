# Toll Calculator

The toll calculator has evolved into a REST API packaged as a Docker-ready Go binary.
It runs in a minimal container image as a non-root user. The service serves HTTP traffic
and expects TLS termination to be handled at the edge. The entry point should be a
global load balancer that distributes traffic across regional deployments for high
availability. Each regional deployment includes an application gateway for authorization
and rate limiting before routing to the service.

**Pending improvements on the roadmap**
* when ops team gives us a database to use, we will replace the hardcoded getter implementations
  with database persistence and store the dagsmart holidays response in the database too 
* the holidays fetching will be a separate job and this API will no longer depend on dagsmart availability 
  during a cold start 
* add tracing when we have the infrastructure to monitor it
* use OpenAPI spec and swagger to document our APIs

## Background

Our city has decided to implement toll fees in order to reduce traffic congestion during rush hours. This is the current draft of requirements:

* Fees will differ between 8 SEK and 18 SEK, depending on the time of day
* Rush-hour traffic will render the highest fee
* The maximum fee for one day is 60 SEK
* A vehicle should only be charged once an hour
* In the case of multiple fees in the same hour period, the highest one applies.
* Some vehicle types are fee-free
* Weekends and holidays are fee-free

## Quick start

**In docker compose (Recommended, includes grafana and prometheus)**
```
make docker-up
make docker-down
```

**On devbox**
```
make build
make run
```

**In docker**
```
make docker-build
make docker-run
```

## Optional tooling

1. direnv for loading .envrc automatically
2. mockery to generate mocks

## Lookup optimization

The getPrice part of the toll calculator has been optimized for a high-throughput scenario by converting the price list
into a minutes after midnight lookup.

A benchmark on the test machine shows a performance of ~15ns per 10M GetPrice calls, which optimistically could achieve ~65M lookups/s, 
in the raw function call benchmark. Profiling locally through the /fee REST API averages around 40k req/s and processes 1M 
requests in under 20s, which should suffice for Transportstyrelsen!

## Monitoring

When running with docker-compose, navigate to [grafana](http://localhost:3001) to see metrics. Use the secure admin
login (u: admin, p: securepassword) and load-test to generate some traffic for the dashboard. Based on monitoring 
results, we can set up additional alerting.
