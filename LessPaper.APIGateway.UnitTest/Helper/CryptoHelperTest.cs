using System;
using System.Collections.Generic;
using System.Text;
using LessPaper.APIGateway.Helper;
using Xunit;

namespace LessPaper.APIGateway.UnitTest.Helper
{
    public class CryptoHelperTest
    {

        [Fact]
        public void GetSalt()
        {
            // Check for duplicate salt
            var salts = new HashSet<string>();
            for (var i = 0; i < 50; i++)
            {
                var salt = CryptoHelper.GetSalt(10);
                Assert.NotNull(salt);
                Assert.False(string.IsNullOrWhiteSpace(salt));
                Assert.DoesNotContain(salt, salts);
                salts.Add(salt);
            }

            Assert.NotEmpty(salts);
        }
        
        [Fact]
        public void GetGuid()
        {
            // Check for duplicate guid
            var guids = new HashSet<string>();
            for (var i = 0; i < 50; i++)
            {
                var guid = CryptoHelper.GetGuid();
                Assert.NotNull(guid);
                Assert.False(string.IsNullOrWhiteSpace(guid));
                Assert.DoesNotContain(guid, guids);
                guids.Add(guid);
            }

            Assert.NotEmpty(guids);
        }

        [Fact]
        public void Sha256FromString()
        {
            // Deterministic
            var hash1 = CryptoHelper.Sha256FromString("a", "b");
            var hash2 = CryptoHelper.Sha256FromString("a", "b");
            Assert.Equal(hash1, hash2);
            
            // Salt is effective
            var hash3 = CryptoHelper.Sha256FromString("a", "a");
            var hash4 = CryptoHelper.Sha256FromString("a", "b");
            Assert.NotEqual(hash3, hash4);

            // Value is effective
            var hash5 = CryptoHelper.Sha256FromString("a", "a");
            var hash6 = CryptoHelper.Sha256FromString("b", "a");
            Assert.NotEqual(hash5, hash6);
        }
    }
}
