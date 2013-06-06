namespace MongoDbStartsWithBugTest
{
    using System;

    using MongoDB.Bson;

    public static class InstanceBuilders
    {
        public static ConcreteMongoClass BuildConcrete(string name, int age, DateTime date, bool complete)
        {
            return new ConcreteMongoClass { Id = ObjectId.GenerateNewId().ToString(), Name = name, Date = date, Age = age, Complete = complete };
        }
    }
}
