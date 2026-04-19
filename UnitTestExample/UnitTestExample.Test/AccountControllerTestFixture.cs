using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UnitTestExample.Controllers;
using System.Activities;

namespace UnitTestExample.Test
{
    public class AccountControllerTestFixture
    {
        [TestCase("abcd1234", false)]
        [TestCase("irf@uni-corvinus", false)]
        [TestCase("irf.uni-corvinus.hu", false)]
        [TestCase("irf@uni-corvinus.hu", true)]
        public void TestValidateEmail(string email, bool expectedResult)
        {
            var accountController = new AccountController();

            var actualResult = accountController.ValidateEmail(email);

            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
        [TestCase("Abcdefgh", false)]
        [TestCase("ABCDEF12", false)]
        [TestCase("abcdef12", false)]
        [TestCase("Ab12", false)]
        [TestCase("Abcd1234", true)]
        public void TestValidatePassword(string password, bool expectedResult)
        {
            var accountController = new AccountController();

            var actualResult = accountController.ValidatePassword(password);

            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [TestCase("irf@uni-corvinus.hu", "Abcd1234")]
        [TestCase("teszt@uni-corvinus.hu", "Xyz12345")]
        public void TestRegisterHappyPath(string email, string password)
        {
            var accountController = new AccountController();

            var actualResult = accountController.Register(email, password);

            Assert.That(actualResult.Email, Is.EqualTo(email));
            Assert.That(actualResult.Password, Is.EqualTo(password));

            Assert.That(actualResult.ID, Is.Not.EqualTo(Guid.Empty));
        }

        [TestCase("irf@uni-corvinus", "Abcd1234")]
        [TestCase("irf.uni-corvinus.hu", "Abcd1234")]
        [TestCase("irf@uni-corvinus.hu", "abcd1234")]
        [TestCase("irf@uni-corvinus.hu", "ABCD1234")]
        [TestCase("irf@uni-corvinus.hu", "abcdABCD")]
        [TestCase("irf@uni-corvinus.hu", "Ab1234")]
        public void TestRegisterValidateException(string email, string password)
        {
            var accountController = new AccountController();

            try
            {
                var actualResult = accountController.Register(email, password);

                Assert.Fail("A program nem dobott hibát a rossz adatokra!");
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.InstanceOf<ValidationException>());
            }
        }


    }


}
