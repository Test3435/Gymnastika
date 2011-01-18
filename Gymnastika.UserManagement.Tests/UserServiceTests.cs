﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Gymnastika.ProjectResources.Properties;
using Moq;

namespace Gymnastika.UserManagement.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService _userService;

        [SetUp]
        public void Init()
        {
            _userService = new UserService(new InMemoryMembershipService());
        }

        [Test]
        public void Register_InfoValid_ReturnUserWithId()
        {
            User user = _userService.Register(
                new User 
                { 
                    UserName = "Martin", 
                    Password = "Pwd",
                    Gender = Gender.Male,
                    Age = 21
                });

            Assert.IsNotNull(user);
            Assert.AreNotEqual(default(Guid), user.Id);
            Assert.AreEqual("Martin", user.UserName);
            Assert.AreEqual(Gender.Male, user.Gender);
            Assert.AreEqual(21, user.Age);
            Assert.AreEqual("Pwd", user.Password);
        }

        [Test]
        public void Register_PasswordNull_CanPass()
        {
            User user = _userService.Register(
                new User
                {
                    UserName = "Martin",
                    Gender = Gender.Male,
                    Age = 21
                });

            Assert.IsNotNull(user);
            Assert.IsNull(_userService.ErrorString);
            Assert.AreNotEqual(default(Guid), user.Id);
            Assert.AreEqual("Martin", user.UserName);
            Assert.AreEqual(Gender.Male, user.Gender);
            Assert.AreEqual(21, user.Age);
        }

        [Test]
        public void Register_UserExists_ReturnNull()
        {
            _userService.Register(
                new User
                {
                    UserName = "Martin",
                    Gender = Gender.Male,
                    Age = 21
                });

            User user = _userService.Register(
                new User
                {
                    UserName = "Martin",
                    Gender = Gender.Male,
                    Age = 21
                });

            Assert.IsNull(user);
            Assert.AreEqual(Resources.DuplicateUserName, _userService.ErrorString);
        }

        [Test]
        public void Register_AgeNotValid_ReturnNull()
        {
            User user = _userService.Register(
                new User
                {
                    UserName = "Martin",
                    Gender = Gender.Male,
                    Age = -1
                });

            Assert.IsNull(user);
            Assert.AreEqual(Resources.InvalidAge, _userService.ErrorString);
        }

        [Test]
        public void Register_UserNameNull_ReturnNull()
        {
            User user = _userService.Register(new User());
            Assert.IsNull(user);
            Assert.AreEqual(Resources.InvalidUserName, _userService.ErrorString);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_UserNull_Throws()
        {
            _userService.Register(null);
        }

        [Test]
        public void LogOn_InfoValid_ReturnTrue()
        {
            bool result = _userService.LogOn("Martin", "Pwd");

            Assert.IsTrue(result);
        }

        [Test]
        public void LogOn_UserNamePasswordMissmatch_ReturnFalse()
        {
            var mockMembership = new Mock<IMembershipService>();
            mockMembership
                .Setup(m => m.Validate("Martin", "InvalidPassword"))
                .Returns(LogOnStatus.InvalidPassword);

            UserService userService = new UserService(mockMembership.Object);
            bool result = userService.LogOn("Martin", "InvalidPassword");

            Assert.IsFalse(result);
            Assert.AreEqual(Resources.InvalidPassword, userService.ErrorString);

            mockMembership.VerifyAll();
        }

        [Test]
        public void LogOn_UserNameNull_ReturnFalse()
        {
            bool result = _userService.LogOn(string.Empty, null);

            Assert.IsFalse(result);
            Assert.AreEqual(Resources.InvalidUserName, _userService.ErrorString);
        }

        [Test]
        public void LogOut_UserNameExists_ReturnTrue()
        {
            bool result = _userService.LogOut("Martin");

            Assert.IsTrue(result);
        }

        [Test]
        public void LogOut_UserNameNotExists_ReturnFalse()
        {
            bool result = _userService.LogOut("NotExistsUser");

            Assert.IsFalse(result);
        }
    }
}
