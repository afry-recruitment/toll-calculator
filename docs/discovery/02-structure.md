# 2. Structure and layout

```
toll-calculator/
├── README.md          # brief + gif
├── C#/                # C# reference implementation
│   ├── TollCalculator.cs
│   ├── Vehicle.cs     (interface)
│   ├── Car.cs
│   ├── Motorbike.cs
│   └── LICENSE
└── Java/              # same design in Java
    ├── TollCalculator.java
    ├── Vehicle.java   (interface)
    ├── Car.java
    └── Motorbike.java
```

## Observations

- **Parallel implementations.** The Java and C# folders are near 1:1
  translations of each other. The two versions diverge in one critical place
  (the time-diff calculation — see [04-bugs.md](04-bugs.md) §4.1) and that
  divergence is itself a bug.
- **No build configuration.** No Maven/Gradle file for Java, no `.csproj` /
  `.sln` for C#. The project does not compile out of the box without manual
  setup.
- **No tests.** No `src/test`, no `*Tests.cs`, no test runner config.
- **No `.gitignore`.** Any `bin/`, `obj/`, or `*.class` artifacts would be
  versioned by accident.
- **License.** Only the C# folder contains a `LICENSE` file; the Java folder
  does not.
- **Domain model.** A `Vehicle` interface with `Car` and `Motorbike` as
  concrete classes. Other vehicle types referenced by name (`Tractor`,
  `Emergency`, `Diplomat`, `Foreign`, `Military`) are mentioned only as
  string literals in the toll-free enum — there are no classes for them.
