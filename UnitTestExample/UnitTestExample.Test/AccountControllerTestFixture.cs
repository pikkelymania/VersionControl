using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UnitTestExample.Controllers; // Behozzuk az eredeti projekt Controllerét

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

    }


}
