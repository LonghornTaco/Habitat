﻿namespace Sitecore.Feature.Accounts.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Security;
    using FluentAssertions;
    using NSubstitute;
    using NSubstitute.Extensions;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoNSubstitute;
    using Ploeh.AutoFixture.Xunit2;
    using Sitecore.Collections;
    using Sitecore.Common;
    using Sitecore.FakeDb.Security.Accounts;
    using Sitecore.FakeDb.Security.Web;
    using Sitecore.FakeDb.Sites;
    using Sitecore.Feature.Accounts.Models;
    using Sitecore.Feature.Accounts.Services;
    using Sitecore.Feature.Accounts.Tests.Extensions;
    using Sitecore.Foundation.Accounts.Repositories;
    using Sitecore.Foundation.Accounts.Services;
    using Sitecore.Foundation.Testing.Attributes;
    using Sitecore.Security.Accounts;
    using Sitecore.Security.Authentication;
    using Sitecore.Security.Domains;
    using Xunit;

    public class AccountsRepositoryTests
    {
        [Theory]
        [AutoFakeUserData]
        public void RestorePassword_ValidUser_ShouldCallResetPassword(FakeMembershipUser user, MembershipProvider membershipProvider, AccountRepository repo)
        {
            user.ProviderName.Returns("fake");
            membershipProvider.ResetPassword(Arg.Any<string>(), Arg.Any<string>()).Returns("new password");
            membershipProvider.Name.Returns("name");
            membershipProvider.GetUser(Arg.Any<string>(), Arg.Any<bool>()).Returns(user);

            using (new MembershipSwitcher(membershipProvider))
            {
                repo.RestorePassword(@"extranet\John");
                membershipProvider.Received(1).ResetPassword(Arg.Any<string>(), Arg.Any<string>());
            }
        }


        [Theory]
        [AutoFakeUserData]
        public void RestorePassword_ValidUser_ShouldReturnsNewPassword(FakeMembershipUser user, MembershipProvider membershipProvider, AccountRepository repo)
        {
            user.ProviderName.Returns("fake");
            membershipProvider.ResetPassword(Arg.Any<string>(), Arg.Any<string>()).Returns("new password");
            membershipProvider.Name.Returns("fake");
            membershipProvider.GetUser(Arg.Any<string>(), Arg.Any<bool>()).Returns(user);

            using (new MembershipSwitcher(membershipProvider))
            {
                repo.RestorePassword(@"extranet\John").Should().Be("new password");
            }
        }

        [Theory]
        [AutoDbData]
        public void Exists_UserExists_ShouldReturnTrue(FakeMembershipUser user, MembershipProvider membershipProvider, AccountRepository repo)
        {
            membershipProvider.GetUser(@"somedomain\John", Arg.Any<bool>()).Returns(user);

            var context = new FakeSiteContext(new StringDictionary
                                              {
                                                  {"domain", "somedomain"}
                                              });
            using (new Switcher<Domain, Domain>(new Domain("somedomain")))
            {
                using (new MembershipSwitcher(membershipProvider))
                {
                    var exists = repo.Exists("John");
                    exists.Should().BeTrue();
                }
            }
        }

        [Theory]
        [AutoDbData]
        public void Exists_UserDoesNotExist_ShouldReturnFalse(FakeMembershipUser user, MembershipProvider membershipProvider, AccountRepository repo)
        {
            membershipProvider.GetUser(Arg.Any<string>(), Arg.Any<bool>()).Returns((MembershipUser)null);
            membershipProvider.GetUser(@"somedomain\John", Arg.Any<bool>()).Returns(user);

            var context = new FakeSiteContext(new StringDictionary
                                              {
                                                  {"domain", "somedomain"}
                                              });
            using (new Switcher<Domain, Domain>(new Domain("somedomain")))
            {
                using (new MembershipSwitcher(membershipProvider))
                {
                    var exists = repo.Exists("Smith");
                    exists.Should().BeFalse();
                }
            }
        }

        [Theory]
        [AutoDbData]
        public void Login_UserIsNotLoggedIn_ShouldReturnFalse(FakeMembershipUser user, AuthenticationProvider authenticationProvider, AccountRepository repo)
        {
            authenticationProvider.Login(@"somedomain\John", Arg.Any<string>(), Arg.Any<bool>()).Returns(false);

            var context = new FakeSiteContext(new StringDictionary
                                              {
                                                  {"domain", "somedomain"}
                                              });
            using (new Switcher<Domain, Domain>(new Domain("somedomain")))
            {
                using (new AuthenticationSwitcher(authenticationProvider))
                {
                    var loginResult = repo.Login("John", "somepassword");
                    loginResult.Should().BeNull();
                }
            }
        }

        [Theory]
        [AutoDbData]
        public void Login_NotLoggedInUser_ShouldNotTrackLoginEvents(FakeMembershipUser user, [Frozen] IAccountTrackerService accountTrackerService, AuthenticationProvider authenticationProvider, AccountRepository repo)
        {
            authenticationProvider.Login(@"somedomain\John", Arg.Any<string>(), Arg.Any<bool>()).Returns(false);

            var context = new FakeSiteContext(new StringDictionary
                                              {
                                                  {"domain", "somedomain"}
                                              });
            using (new Switcher<Domain, Domain>(new Domain("somedomain")))
            {
                using (new AuthenticationSwitcher(authenticationProvider))
                {
                    repo.Login("John", "somepassword");
                    accountTrackerService.DidNotReceive().TrackLogin(Arg.Any<string>());
                }
            }
        }

        public static IEnumerable<object[]> RegistrationInfosArgumentNull
        {
            get
            {
                var fixture = new Fixture();
                return new List<object[]>
                       {
                           new object[] {null, fixture.Create<string>(), fixture.Create<string>()},
                           new object[] {fixture.Create<string>(), null, fixture.Create<string>()}
                       };
            }
        }

        [Theory]
        [MemberData(nameof(RegistrationInfosArgumentNull))]
        public void RegisterUser_NullEmailOrPassword_ShouldThrowArgumentException(string email, string password, string profileId)
        {
            var repository = new AccountRepository(Substitute.For<IAccountTrackerService>());
            repository.Invoking(x => x.RegisterUser(email, password, profileId)).ShouldThrow<ArgumentNullException>();
        }

        [Theory]
        [AutoFakeUserData]
        public void RegisterUser_ValidData_ShouldCreateUserWithEmailAndPassword(FakeMembershipUser user, MembershipProvider membershipProvider, RegistrationInfo registrationInfo, string userProfile, AccountRepository repository)
        {
            user.ProviderName.Returns("fake");
            user.UserName.Returns("name");
            MembershipCreateStatus status;
            membershipProvider.CreateUser(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<object>(), out status).Returns(user);
            membershipProvider.GetUser(Arg.Any<string>(), Arg.Any<bool>()).Returns(user);

            using (new Switcher<Domain, Domain>(new Domain("somedomain")))
            {
                using (new MembershipSwitcher(membershipProvider))
                {
                    repository.RegisterUser(registrationInfo.Email, registrationInfo.Password, userProfile);
                    membershipProvider.Received(1).CreateUser($@"somedomain\{registrationInfo.Email}", registrationInfo.Password, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<object>(), out status);
                }
            }
        }

        [Theory]
        [AutoFakeUserData]
        public void RegisterUser_ValidData_ShouldCreateLoginUser(FakeMembershipUser user, [Substitute] MembershipProvider membershipProvider, [Substitute] AuthenticationProvider authenticationProvider, RegistrationInfo registrationInfo, AccountRepository repository, string profileId)
        {
            user.ProviderName.Returns("fake");
            user.UserName.Returns("name");
            MembershipCreateStatus status;
            membershipProvider.CreateUser(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<object>(), out status).Returns(user);
            membershipProvider.GetUser(Arg.Any<string>(), Arg.Any<bool>()).Returns(user);

            using (new Switcher<Domain, Domain>(new Domain("somedomain")))
            {
                using (new MembershipSwitcher(membershipProvider))
                {
                    using (new AuthenticationSwitcher(authenticationProvider))
                    {
                        repository.RegisterUser(registrationInfo.Email, registrationInfo.Password, profileId);
                        authenticationProvider.Received(1).Login(Arg.Is<string>(u => u == $@"somedomain\{registrationInfo.Email}"), Arg.Is<string>(p => p == registrationInfo.Password), Arg.Any<bool>());
                    }
                }
            }
        }

        [Theory]
        [AutoFakeUserData]
        public void Register_ValidUser_ShouldTrackRegistraionEvents(FakeMembershipUser user, [Substitute] MembershipProvider membershipProvider, [Substitute] AuthenticationProvider authenticationProvider, RegistrationInfo registrationInfo, [Frozen] IAccountTrackerService accountTrackerService, AccountRepository repository, string profileId)
        {
            user.UserName.Returns("name");
            MembershipCreateStatus status;
            membershipProvider.CreateUser(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<object>(), out status).Returns(user);
            membershipProvider.GetUser(Arg.Any<string>(), Arg.Any<bool>()).Returns(user);

            using (new Switcher<Domain, Domain>(new Domain("somedomain")))
            {
                using (new MembershipSwitcher(membershipProvider))
                {
                    using (new AuthenticationSwitcher(authenticationProvider))
                    {
                        repository.RegisterUser(registrationInfo.Email, registrationInfo.Password, profileId);
                        accountTrackerService.Received(1).TrackRegistration();
                    }
                }
            }
        }

        [Theory]
        [AutoDbData]
        public void Logout_ActiveUser_ShouldLogoutUser(User user, MembershipProvider membershipProvider, RegistrationInfo registrationInfo, AccountRepository repository)
        {
            var authenticationProvider = Substitute.For<AuthenticationProvider>();
            authenticationProvider.GetActiveUser().Returns(user);
            using (new AuthenticationSwitcher(authenticationProvider))
            {
                repository.Logout();
                authenticationProvider.Received(1).Logout();
            }
        }
    }
}