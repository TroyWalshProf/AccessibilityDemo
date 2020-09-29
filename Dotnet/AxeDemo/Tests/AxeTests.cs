using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using PageModel;
using Selenium.Axe;
using System.IO;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]
namespace Tests
{
    /// <summary>
    /// Sample Axe tests
    /// </summary>
    [TestClass]
    public class AxeTests : BaseSeleniumTest
    {
        /// <summary>
        /// Very basic test
        /// </summary>
        [TestMethod]
        public void BasicLoginTest()
        {
            // Get to home page
            LoginPageModel page = new LoginPageModel(this.TestObject);
            page.OpenLoginPage();
            HomePageModel home = page.LoginWithValidCredentials(Config.GetGeneralValue("User"), Config.GetGeneralValue("Pass"));

            Assert.IsTrue(home.IsPageLoaded(), "Failed to load homepage");
        }

        /// <summary>
        /// Basic check
        /// </summary>
        [TestMethod]
        public void LoginPageBasic()
        {
            LoginPageModel page = new LoginPageModel(this.TestObject);
            page.OpenLoginPage();

            AxeResult result = WebDriver.Analyze();

            var resultJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            Assert.IsTrue(string.IsNullOrEmpty(result.Error) && result.Violations.Length == 0, "Failures:" + resultJson);
        }

        /// <summary>
        /// Check with report
        /// </summary>
        [TestMethod]
        public void LoginPageWithReport()
        {
            string reportPath = Path.Combine(LoggingConfig.GetLogDirectory(), "LoginPageWithReport.html");

            LoginPageModel page = new LoginPageModel(this.TestObject);
            page.OpenLoginPage();

            AxeResult result = WebDriver.Analyze();
            WebDriver.CreateAxeHtmlReport(result, reportPath);

            if (!string.IsNullOrEmpty(result.Error) || result.Violations.Length != 0)
            {
                TestObject.AddAssociatedFile(reportPath);
                Assert.Fail($"Failed error and/or violation check see {reportPath} for more details.");
            }
        }

        /// <summary>
        /// Check sub page with report
        /// </summary>
        [TestMethod]
        public void LoginSubPageSubElementWithReport()
        {
            string reportPath = Path.Combine(LoggingConfig.GetLogDirectory(), "LoginSubPageWithReport.html");

            LoginPageModel page = new LoginPageModel(this.TestObject);
            page.OpenLoginPage();

            var jumbotron = WebDriver.FindElement(By.CssSelector(".jumbotron")) as IWrapsElement;
            WebDriver.CreateAxeHtmlReport(jumbotron.WrappedElement, reportPath);

            if (!File.ReadAllText(reportPath).Contains("Violation: 0"))
            {
                TestObject.AddAssociatedFile(reportPath);
                Assert.Fail($"Failed violation check see {reportPath} for more details.");
            }
        }

        /// <summary>
        /// Complex check with report
        /// </summary>
        [TestMethod]
        public void HomePageWithComplexReport()
        {
            string reportPath = Path.Combine(LoggingConfig.GetLogDirectory(), "HomePageWithComplexReport.html");
            string rawResults = Path.Combine(LoggingConfig.GetLogDirectory(), "HomePageWithComplexReport.json");

            // Get to home page
            LoginPageModel page = new LoginPageModel(this.TestObject);
            page.OpenLoginPage();
            page.LoginWithValidCredentials(Config.GetGeneralValue("User"), Config.GetGeneralValue("Pass"));

            // Setup custom rules
            AxeBuilder builder = new AxeBuilder(WebDriver)
                .Exclude("#HomePage")
                .WithOutputFile(rawResults)
                .DisableRules("landmark-one-main", "page-has-heading-one");

            // Reprot
            WebDriver.CreateAxeHtmlReport(builder.Analyze(), reportPath);

            // Check if there were any violations
            if (!File.ReadAllText(reportPath).Contains("Violation: 0"))
            {
                TestObject.AddAssociatedFile(reportPath);
                TestObject.AddAssociatedFile(rawResults);
                Assert.Fail($"Failed violation check see {reportPath} for more details.");
            }
        }
    }
}
