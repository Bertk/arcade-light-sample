using System;
using Pets;
using Xunit;

namespace PetsUnitTest
{

    public class PetTests
    {
        [Fact]
        public void DogTalkToOwnerReturnsWoof()
        {
            const string expected = "Woof!";
            string actual = new Dog().TalkToOwner();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CatTalkToOwnerReturnsMeow()
        {
            const string expected = "Meow!";
            string actual = new Cat().TalkToOwner();

            Assert.Equal(expected, actual);
        }
    }
}
