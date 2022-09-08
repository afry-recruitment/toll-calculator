plugins {
    // Apply the application plugin to add support for building a CLI application in Java.
    application
}

val javaVersion: String by project
val appVersion: String by project

version = appVersion

java {
    toolchain {
        languageVersion.set(JavaLanguageVersion.of(javaVersion))
    }
}

repositories {
    // Use Maven Central for resolving dependencies.
    mavenCentral()
}

dependencies {
    // This dependency is used by the application.
    implementation("com.google.guava:guava:31.0.1-jre")
}

testing {
    suites {
        // Configure the built-in test suite
        val test by getting(JvmTestSuite::class) {
            // Use JUnit Jupiter test framework
            useJUnitJupiter("5.8.2")
        }
    }
}

application {
    // Define the main class for the application.
    mainClass.set("calculator.App")
}


