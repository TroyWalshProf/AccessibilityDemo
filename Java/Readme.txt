1. Install Chrome

2. Install the Java 8 SDK

3. Install an IDE - Like IntelliJ
   *You will also want the Maven and TestNG extensions

4. In the IDE open this folder - Most IDEs should parse the Maven pom for you

5. Run the Maven compile - Compile pulls down the Chrome driver

6. Run the tests

Troubleshooting:
*TestNG many ask you to us the JVM argument [-Dtestng.dtd.http=true]
 See: https://stackoverflow.com/questions/57299606/testng-by-default-disables-loading-dtd-from-unsecure-urls

*Your tests may fail to start if your web driver is too old.  
 To update the driver version simply update the version under the com.github.webdriverextensions plugin.
  <driver>
    <name>chromedriver</name>
    <customFileName>chromedriver</customFileName>
    <version>!!!!!!85.0.4183.83!!!!!</version>
  </driver>

