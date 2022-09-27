plugins {
    // Apply the application plugin to add support for building a CLI application in Java.
    application
    id("com.github.johnrengelman.shadow") version "7.1.2"
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
    // handle json
    implementation("com.google.code.gson:gson:2.9.1")
    implementation("org.apache.commons:commons-csv:1.9.0")
    // logging
    implementation("ch.qos.logback:logback-core:1.4.0")
    implementation("org.slf4j:slf4j-api:2.0.0")
    runtimeOnly("ch.qos.logback:logback-classic:1.4.0")
    testRuntimeOnly("ch.qos.logback:logback-classic:1.4.0")
    // annotations
    annotationProcessor("org.projectlombok:lombok:1.18.24")
    testAnnotationProcessor("org.projectlombok:lombok:1.18.24")
    compileOnly("org.projectlombok:lombok:1.18.24")
    testCompileOnly("org.projectlombok:lombok:1.18.24")
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
tasks.shadowJar {
    archiveBaseName.set("TollCalculator")
    archiveClassifier.set("")

}
// copy .exe file to installer build folder
tasks.register<Copy>("copySettings") {
    from(file("settings"))
    into(buildDir.path + "/executable/settings/")
    duplicatesStrategy = DuplicatesStrategy.EXCLUDE
}
tasks.register<Copy>("copyData") {
    from(file("data"))
    into(buildDir.path + "/executable/data/")
    duplicatesStrategy = DuplicatesStrategy.EXCLUDE
}
tasks.register<Copy>("copyFatJar") {
    from(tasks.shadowJar.get())
    into(buildDir.path + "/executable/")
    duplicatesStrategy = DuplicatesStrategy.INCLUDE
}
// build proving range folder for dev
tasks.register("buildExecutable") {
    dependsOn(
        tasks.getByName("copySettings"),
        tasks.getByName("copyData"),
        tasks.getByName("copyFatJar")
    )
}
application {
    // Define the main class for the application.
    mainClass.set("calculator.App")
}


