using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Blog.Core.Core;
using Blog.Core.Data;
using Blog.Core.GraphQl;
using Blog.Core.GraphQl.Models;
using FluentValidation;
using GraphQL.Types;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using SimpleInjector;

namespace Blog.Core
{
    public class Bootstrap
    {
        private Container Container { get; }
        public bool IsTesting { get; }

        private readonly Assembly[] _assemblies = AppDomain.CurrentDomain.GetAssemblies();

        public Bootstrap(Container container, bool isTesting = false)
        {
            Container = container;
            IsTesting = isTesting;
        }

        public void Handle()
        {
            RegisterEntityFramework();
            RegisterFluentValidation();
            RegisterAutoMapper();
            RegisterMediatr();
            RegisterGraphql();
        }

        private void RegisterFluentValidation()
        {
            Container.Collection.Register(typeof(IValidator<>), _assemblies);
        }

        private void RegisterGraphql()
        {
            Container.RegisterSingleton<ITableNameLookup, TableNameLookup>();
            Container.RegisterSingleton<IDatabaseMetadata, DatabaseMetadata>();

            Container.Register(() =>
            {
                var tableNameLookup = Container.GetInstance<ITableNameLookup>();
                var databaseMetadata = Container.GetInstance<IDatabaseMetadata>();
                var context = Container.GetInstance<BlogContext>();

                var schema = new Schema
                {
                    Query = new GraphQlQuery(context, databaseMetadata, tableNameLookup),
                };
                schema.Initialize();

                return schema;
            }, Lifestyle.Singleton);
        }

        private void RegisterAutoMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddMaps(_assemblies));
            var mapper = config.CreateMapper();
            Container.RegisterInstance(mapper);
        }

        private void RegisterEntityFramework()
        {
            Container.Register(() =>
            {
                var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

                if (IsTesting)
                {
                    options = new DbContextOptionsBuilder<BlogContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
                }

                return new BlogContext(options);
            }, Lifestyle.Singleton);
        }

        private void RegisterMediatr()
        {
            Container.RegisterSingleton<IMediator, Mediator>();

            Container.Register(typeof(IRequestHandler<,>), _assemblies);

            // we have to do this because by default, generic type definitions (such as the Constrained Notification Handler) won't be registered
            var notificationHandlerTypes = Container.
                GetTypesToRegister(typeof(INotificationHandler<>), _assemblies, 
                new TypesToRegisterOptions
            {
                IncludeGenericTypeDefinitions = true,
                IncludeComposites = false,
            });
            Container.Collection.Register(typeof(INotificationHandler<>), notificationHandlerTypes);

            //Pipeline
            Container.Collection.Register(typeof(IPipelineBehavior<,>), new[]
            {
                typeof(ValidationProcessBehaviour<,>),
                typeof(RequestPreProcessorBehavior<,>),
                typeof(RequestPostProcessorBehavior<,>)
            });

            Container.Collection.Register(typeof(IRequestPreProcessor<>), 
                GetTypesToRegister(typeof(IRequestPreProcessor<>))); 
            Container.Collection.Register(typeof(IRequestPostProcessor<,>), 
                GetTypesToRegister(typeof(IRequestPostProcessor<,>)));

            Container.Register(() => new ServiceFactory(Container.GetInstance), Lifestyle.Singleton);
        }

        private List<Type> GetTypesToRegister(Type type, bool inverseOrder = false)
        {
            var types = Container.GetTypesToRegister(type, _assemblies, new TypesToRegisterOptions
            {
                IncludeGenericTypeDefinitions = true,
                IncludeComposites = false
            }).Where(t => t.IsPublic || t.IsNestedPublic);

            var assemblies = _assemblies.ToList();
            var orderedTypes = inverseOrder ? 
                types.OrderByDescending(t => assemblies.IndexOf(t.Assembly)) : 
                types.OrderBy(t => assemblies.IndexOf(t.Assembly));

            var results = orderedTypes
                .ThenBy(t => t.IsGenericTypeDefinition ? 0 : 1)
                .ThenBy(t => t.Namespace)
                .ThenBy(t => t.Name)
                .ToList();

            return results;
        }
    }
}
