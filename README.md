Toll Calculator

A Spring Boot–based toll calculation system designed with extensibility, clean architecture, and configuration-driven behavior in mind.

Architectural Highlights
✔ Externalized toll rules, toll-free vehicles, and toll-free dates into application.yaml
✔ Configuration loaded via @ConfigurationProperties
✔ Easily extendable to load configurations from a database
✔ Profile-based switching between YAML and DB configurations using Spring profiles
✔ Clear separation of interfaces (SPI) and implementations
✔ Business logic decoupled from configuration source
✔ Time zone–aware implementation (configurable ZoneId)
✔ Full unit test coverage for business logic components
✔ Integration tests validating end-to-end functionality within Spring context
✔ Clean layered structure: config, domain, service, spi

Tech Stack
Java 21, Spring Boot 3.3, JUnit 5, Mockito, AssertJ
