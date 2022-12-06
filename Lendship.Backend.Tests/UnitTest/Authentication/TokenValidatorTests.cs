using Lendship.Backend.Authentication;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Lendship.Backend.Tests.UnitTest.Authentication
{
    [TestFixture]
    public class TokenValidatorTests
    {
        private TokenValidator _sut;
        private Mock<ITokenService> _tokenService;
        private Mock<JwtSecurityTokenHandler> _tokenHandler;

        [SetUp]
        public void Setup()
        {
            _tokenService = new Mock<ITokenService>();
            _tokenHandler = new Mock<JwtSecurityTokenHandler>();
            _sut = new TokenValidator(_tokenService.Object, _tokenHandler.Object);
        }

        [TestCase]
        public void CanValidateToken_Should_Return_True()
        {
            var result = _sut.CanValidateToken;
            Assert.IsTrue(result);
        }

        [TestCase]
        public void ValidateToken_Should_Throw_Error_When_Token_Is_Deactivated()
        {
            _tokenService.Setup(x => x.IsCurrentTokenDeactivated()).Returns(true);
            try
            {
                var result = _sut.ValidateToken("", null, out SecurityToken token);
                Assert.Fail();
            } catch (InvalidTokenException)
            {
                Assert.Pass();
            }
        }


        [TestCase]
        public void ValidateToken_Should_Call_JWTTokenHandler_ValidateToken_If_Token_Is_Not_Deactivated()
        {
            _tokenService.Setup(x => x.IsCurrentTokenDeactivated()).Returns(false);

            SecurityToken token = null;
            var result = _sut.ValidateToken("", null, out token);

            _tokenHandler.Verify(x => x.ValidateToken(It.IsAny<string>(), It.IsAny<TokenValidationParameters>(), out token), Times.Once);
        }


        [TestCase]
        public void ValidateToken_Should_Not_Throw_Error_When_Token_Is_Valid()
        {
            try
            {
                SecurityToken token = null;

                _tokenService.Setup(x => x.IsCurrentTokenDeactivated()).Returns(false);
                _tokenHandler.Setup(x => x.ValidateToken(It.IsAny<string>(), It.IsAny<TokenValidationParameters>(), out token)).Returns(It.IsAny<ClaimsPrincipal>());

                var result = _sut.ValidateToken("", null, out token);
                Assert.Pass();
            } catch (InvalidTokenException)
            {
                Assert.Fail();
            }
        }


        [TestCase]
        public void ValidateToken_Should_Not_Throw_Error_When_Token_Is_Not_Valid()
        {
            try
            {
                SecurityToken token = null;

                _tokenService.Setup(x => x.IsCurrentTokenDeactivated()).Returns(false);
                _tokenHandler.Setup(x => x.ValidateToken(It.IsAny<string>(), It.IsAny<TokenValidationParameters>(), out token)).Throws(It.IsAny<InvalidTokenException>());

                var result = _sut.ValidateToken("", null, out token);
                Assert.Fail();
            }
            catch (InvalidTokenException)
            {
                Assert.Pass();
            }
        }
    }
}