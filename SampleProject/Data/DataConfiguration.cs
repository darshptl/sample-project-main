using System.Reflection;
using BusinessEntities;
using Common;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Imports.Newtonsoft.Json;
using SimpleInjector;

namespace Data
{
    public class DataConfiguration
    {
        public static void Initialize(Container container, Lifestyle lifestyle, bool createIndexes = true)
        {
            var assembly = typeof(DataConfiguration).Assembly;

            container.RegisterSingleton<IListTypeLookup<Assembly>, ListTypeLookup<Assembly>>();

            InitializeAssemblyInstancesService.RegisterAssemblyWithSerializableTypes(container, typeof(User).Assembly);
            InitializeAssemblyInstancesService.RegisterAssemblyWithSerializableTypes(container, assembly);

            InitializeAssemblyInstancesService.Initialize(container, lifestyle, assembly);
            container.RegisterSingleton(() => InitializeDocumentStore(assembly, createIndexes));

            container.Register(() =>
                               {
                                   var session = container.GetInstance<IDocumentStore>().OpenSession();
                                   session.Advanced.MaxNumberOfRequestsPerSession = 5000;
                                   return session;
                               }, lifestyle);
        }

        private static IDocumentStore InitializeDocumentStore(Assembly assembly, bool createIndexes)
        {
            // Read Raven URL from config with a safe fallback
            //var ravenUrl = ConfigurationManager.AppSettings["Raven.Url"];
            //if (string.IsNullOrWhiteSpace(ravenUrl))
            //{
            //    ravenUrl = "http://127.0.0.1:8080/";
            //}
            var documentStore = new DocumentStore
                                {
                                    //Url = "http://localhost:8080/",
                                    //Changing the Url to http://127.0.0.1:8080 because thats where RavenDB is running for me darshptl. 
                                    //This is the minimal change but yu can also read in the url from the Web.config as below but it requires
                                    //Systeminstalling NuGet package System.Configuration.ConfigurationManager in the Data project so ConfigurationManager
                                    //is available at compile time.
                                    //Url = revenUrl,

                                    Url = "http://127.0.0.1:8080",
                                    DefaultDatabase = "SampleProject",
                                    Conventions =
                                    {
                                        DefaultUseOptimisticConcurrency = true,
                                        DocumentKeyGenerator = (dbname, commands, entity) => "",
                                        SaveEnumsAsIntegers = true,
                                        CustomizeJsonSerializer = serializer =>
                                                                  {
                                                                      serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                                                                      serializer.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                                                                      serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
                                                                      serializer.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                                                                      serializer.NullValueHandling = NullValueHandling.Include;
                                                                  },
                                    }
                                };

            documentStore.Initialize();

            if (createIndexes)
            {
                IndexCreation.CreateIndexes(assembly, documentStore);
            }

            return documentStore;
        }
    }
}