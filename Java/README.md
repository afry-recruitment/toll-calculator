# Tested Environment

* OS : macOS Monterey Version 12.5.1 (21G83)
* JAVA version : 17.0.4.1

# Used Tools

* Command-line
* VS Code

# Usage

## Using command-line

* Go to the project's main file location

`cd <path-to-this-project-cloned>/toll-calculator/Java/`

* Build the project 

`javac TollFee.java`

* Run the project

`java TollFee`

# Remark

* Developments done in Git branch (_development_) and finally merged to _master_ branch.

# UnitTest

* Intended was not to cover each an every feature but to show it is possible to write Unit Test
* Hasn't used complex frameworks or IDE features to achieve unit test.
* Just used command-line these JAR files (_junit-4.12.jar_ & _hamcrest-core-1.3.jar_)

* Go inside _UnitTests_ directory

* Compile the _SampleData.java_ class

`javac SampleData.java`

* Compile the _SampleDataTest.java_ class

`javac -cp .:junit-4.12.jar SampleDataTest.java`

* Run the test

`java -cp .:junit-4.12.jar:hamcrest-core-1.3.jar org.junit.runner.JUnitCore SampleDataTest`