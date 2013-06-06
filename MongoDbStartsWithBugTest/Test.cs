namespace MongoDbStartsWithBugTest
{
    namespace LinqToQuerystring.IntegrationTests.Mongo
    {
        using System;
        using System.Collections.Generic;
        using System.Linq;

        using Machine.Specifications;

        using MongoDB.Driver;
        using MongoDB.Driver.Linq;

        public abstract class MongoFunctions
        {
            private static MongoServer server;

            private static MongoDatabase database;

            protected static List<ConcreteMongoClass> strongResult;

            protected static IQueryable<ConcreteMongoClass> strongCollection;

            private Cleanup cleanup = () =>
            {
                var strongMongoCollection = database.GetCollection<ConcreteMongoClass>("Concrete");
                strongMongoCollection.RemoveAll();
            };

            private Establish context = () =>
            {
                server = MongoServer.Create("mongodb://localhost/BugTest?safe=true");
                database = server.GetDatabase("BugTest");

                var strongMongoCollection = database.GetCollection<ConcreteMongoClass>("Concrete");

                strongMongoCollection.Insert(InstanceBuilders.BuildConcrete("Saturday", 1, new DateTime(2001, 01, 01), true));
                strongMongoCollection.Insert(InstanceBuilders.BuildConcrete("Satnav", 2, new DateTime(2002, 01, 01), false));
                strongMongoCollection.Insert(InstanceBuilders.BuildConcrete("Saturnalia", 3, new DateTime(2003, 01, 01), true));
                strongMongoCollection.Insert(InstanceBuilders.BuildConcrete("Saturn", 4, new DateTime(2004, 01, 01), true));
                strongMongoCollection.Insert(InstanceBuilders.BuildConcrete("Monday", 5, new DateTime(2005, 01, 01), true));
                strongMongoCollection.Insert(InstanceBuilders.BuildConcrete("Tuesday", 5, new DateTime(2005, 01, 01), true));
                strongMongoCollection.Insert(InstanceBuilders.BuildConcrete("Burns", 5, new DateTime(2005, 01, 01), true));

                strongCollection = strongMongoCollection.AsQueryable();
            };
        }

        public class When_filtering_strongly_typed_data_with_startswith_function : MongoFunctions
        {
            private Because of = () => strongResult = strongCollection.Where(o => o.Name.StartsWith("Sat") == true).ToList();

            private It should_return_four_records = () => strongResult.Count().ShouldEqual(4);

            private It should_only_return_records_where_name_starts_with_Sat =
                () => strongResult.ShouldEachConformTo(o => o.Name.StartsWith("Sat"));
        }

        public class When_filtering_strongly_typed_data_with_contains_function : MongoFunctions
        {
            private Because of = () => strongResult = strongCollection.Where(o => o.Name.Contains("urn") == true).ToList();

            private It should_return_three_records = () => strongResult.Count().ShouldEqual(3);

            private It should_only_return_records_where_name_contains_urn =
                () => strongResult.ShouldEachConformTo(o => o.Name.Contains("urn"));
        }

        public class When_filtering_loosely_strongly_data_with_substringof_function_with_tolower : MongoFunctions
        {
            private Because of = () => strongResult = strongCollection.Where(o => o.Name.ToLower().Contains("sat") == true).ToList();

            private It should_return_four_records = () => strongResult.Count().ShouldEqual(4);

            private It should_only_return_records_where_name_contains_sat =
                () => strongResult.ShouldEachConformTo(o => o.Name.Contains("Sat"));
        }
    }
}
