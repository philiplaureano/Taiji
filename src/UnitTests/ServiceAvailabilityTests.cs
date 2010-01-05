using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.IoC;
using LinFu.IoC.Interfaces;
using Moq;
using NUnit.Framework;
using Taiji;

namespace Taiji.UnitTests
{
    [TestFixture]
    public class ServiceAvailabilityTests
    {
        private IServiceContainer _container;
        
        [SetUp]
        public void Init()
        {
            _container = new ServiceContainer();
            _container.LoadFromBaseDirectory("*.dll");
        }

        [TearDown]
        public void Term()
        {
            _container = null;
        }

        [Test]
        public void ShouldHaveMethodFinderService()
        {
            AssertServiceExists<IMethodFinder>();
        }

        [Test]
        public void ShouldHaveMethodFilterService()
        {
            AssertServiceExists<IMethodFilter>();
        }

        [Test]
        public void ShouldHaveAddInterfaceMethodService()
        {
            AssertServiceExists<IMethodBuilder>("AddInterfaceMethod");
        }

        [Test]
        public void ShouldHaveInvokedMethodFilter()
        {
            AssertServiceExists<IMethodFilter>("InvokedMethodFilter");
        }

        [Test]
        public void ShouldHaveAddAdapterMethod()
        {
            AssertServiceExists<IAddAdapterMethod>();
        }

        [Test]
        public void ShouldHaveInjectAdapterAsParameter()
        {
            AssertServiceExists<IAdaptParameter>("InjectAdapterAsParameter");
        }

        [Test]
        [Ignore("TODO: Fix this")]
        public void ShouldHaveDefaultAdaptParameterService()
        {
            AssertServiceExists<IAdaptParameter>();
        }

        [Test]
        [Ignore("TODO: Fix this")]
        public void ShouldHaveAdapterBuilder()
        {
            var context = new Mock<IExtractionContext>();
            _container.AddService(context.Object);
            AssertServiceExists<IAdapterBuilder>();
        }

        [Test]
        [Ignore("TODO: Fix this")]
        public void ShouldHavePushParameterOntoArgumentStack()
        {
            AssertServiceExists<IAdaptParameter>("PushParameterOntoArgumentStack");
        }

        [Test]
        public void ShouldHaveInvalidCallFilter()
        {
            AssertServiceExists<IInvalidCallFilter>();
        }

        [Test]
        public void ShouldHavePopMethodArguments()
        {
            AssertServiceExists<IReplaceMethodCall>("PopMethodArguments");
        }

        [Test]
        [Ignore("TODO: Fix this")]
        public void ShouldHavePushMethodArguments()
        {
            var extractionContext = new Mock<IExtractionContext>();
            _container.AddService(extractionContext.Object);

            AssertServiceExists<IReplaceMethodCall>("PushMethodArguments");
        }

        [Test]
        [Ignore("TODO: Fix this")]
        public void ShouldHaveAddInterfaceMethodIfMethodNotFound()
        {
            AssertServiceExists<IMethodBuilder>("AddInterfaeMethodIfMethodNotFound");
        }

        private void AssertServiceExists<TService>()
        {
            AssertServiceExists(string.Empty, typeof(TService));
        }

        private void AssertServiceExists<TService>(string serviceName)
        {
            AssertServiceExists(serviceName, typeof(TService));
        }

        private void AssertServiceExists(string serviceName, Type serviceType)
        {
            var result = _container.Contains(serviceName, serviceType);
            Assert.IsTrue(result);
            Assert.IsNotNull(_container.GetService(serviceName, serviceType));
        }
    }
}
