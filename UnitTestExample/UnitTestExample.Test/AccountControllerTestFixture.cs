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
    }
}
