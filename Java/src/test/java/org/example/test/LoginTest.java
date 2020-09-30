package org.example.test;

import com.deque.html.axecore.extensions.WebDriverExtensions;
import com.deque.html.axecore.results.Results;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.SerializationFeature;
import com.magenic.jmaqs.selenium.BaseSeleniumTest;
import com.magenic.jmaqs.utilities.logging.MessageType;
import org.example.pagemodels.HomePageModel;
import org.example.pagemodels.LoginPageModel;
import com.magenic.jmaqs.utilities.helper.exceptions.ExecutionFailedException;
import com.magenic.jmaqs.utilities.helper.exceptions.TimeoutException;
import org.testng.Assert;
import org.testng.annotations.Test;

import javax.naming.OperationNotSupportedException;
import java.io.IOException;

public class LoginTest extends BaseSeleniumTest {

  @Test
  public void openPageTest() throws IOException, OperationNotSupportedException {
    LoginPageModel page = new LoginPageModel(this.getTestObject());
    page.openLoginPage();
    Assert.assertTrue(page.isPageLoaded());

    Results results =  WebDriverExtensions.analyze(this.getWebDriver());

    ObjectMapper mapper = new ObjectMapper();
    try {
      mapper.enable(SerializationFeature.INDENT_OUTPUT);
      String json = mapper.writeValueAsString(results);
      this.getLogger().logMessage(MessageType.INFORMATION, json);
    } catch (JsonProcessingException e) {
      this.getLogger().logMessage(MessageType.WARNING, "Failed to log results because: ", e.getMessage());
    }

    Assert.assertTrue(results.violationFree(), "Violations were found");


  }

  @Test
  public void enterValidCredentialsTest() throws InterruptedException, TimeoutException, ExecutionFailedException {
    String username = "Ted";
    String password = "123";
    LoginPageModel page = new LoginPageModel(this.getTestObject());
    page.openLoginPage();
    HomePageModel homepage = page.loginWithValidCredentials(username, password);
    Assert.assertTrue(homepage.isPageLoaded());
  }

  @Test
  public void enterInvalidCredentials() throws InterruptedException, TimeoutException, ExecutionFailedException {
    String username = "NOT";
    String password = "Valid";
    LoginPageModel page = new LoginPageModel(this.getTestObject());
    page.openLoginPage();
    Assert.assertTrue(page.loginWithInvalidCredentials(username, password));
  }
}