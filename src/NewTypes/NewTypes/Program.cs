using System;
using System.Collections.Generic;
using NewTypes.Pets;

namespace NewTypes
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            ArgumentNullException.ThrowIfNull(args);

            List<IPet> pets =
            [
                new Dog(),
                new Cat()
            ];

            foreach (IPet pet in pets)
            {
                Console.WriteLine(pet.TalkToOwner());
            }
        }
    }
}
