using System;

namespace NewTypes.Pets
{
    public class Dog : IPet
    {
        public string TalkToOwner() => "Woof!";
    }
}
