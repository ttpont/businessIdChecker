using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessIdChecker;

namespace BusinessIdCheckerTests
{
    [TestClass]
    public class BusinessIdSpecificationsTests
    {
        private const string InvalidBusinessIdTooShort = "25779-2";
        private const string InvalidBusinessIdTooLong = "25770999-2";
        private const string InvalidBusinessIdFormat = "25770CB-2";
        private const string ValidBusinessId = "2577099-2";
        private const string ValidBusinessId2 = "2544890-1";
        private const string ValidBusinessId3 = "2754676-7";
        private const string ValidBusinessId4 = "1069622-4";

        [TestMethod]
        public void TestIsSatisfiedByAndReasonsForDissatisfaction()
        {
            var businessIdSpecification = new BusinessIdSpecification();
            
            // Too short
            Assert.AreEqual(false, businessIdSpecification.IsSatisfiedBy(InvalidBusinessIdTooShort));
            Assert.AreEqual(3, businessIdSpecification.ReasonsForDissatisfaction.Count());

            // Too long
            Assert.AreEqual(false, businessIdSpecification.IsSatisfiedBy(InvalidBusinessIdTooLong));
            Assert.AreEqual(3, businessIdSpecification.ReasonsForDissatisfaction.Count());

            //Invalid format
            Assert.AreEqual(false, businessIdSpecification.IsSatisfiedBy(InvalidBusinessIdFormat));
            Assert.AreEqual(2, businessIdSpecification.ReasonsForDissatisfaction.Count());

            // Null
            Assert.AreEqual(false, businessIdSpecification.IsSatisfiedBy(null));
            Assert.AreEqual(3, businessIdSpecification.ReasonsForDissatisfaction.Count());

            // Valid
            Assert.AreEqual(true, businessIdSpecification.IsSatisfiedBy(ValidBusinessId));
            Assert.AreEqual(0, businessIdSpecification.ReasonsForDissatisfaction.Count());

            Assert.AreEqual(true, businessIdSpecification.IsSatisfiedBy(ValidBusinessId2));
            Assert.AreEqual(0, businessIdSpecification.ReasonsForDissatisfaction.Count());

            Assert.AreEqual(true, businessIdSpecification.IsSatisfiedBy(ValidBusinessId3));
            Assert.AreEqual(0, businessIdSpecification.ReasonsForDissatisfaction.Count());

            Assert.AreEqual(true, businessIdSpecification.IsSatisfiedBy(ValidBusinessId4));
            Assert.AreEqual(0, businessIdSpecification.ReasonsForDissatisfaction.Count());
        }
    }
}
