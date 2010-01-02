using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.Finders;
using LinFu.Finders.Interfaces;
using LinFu.Reflection.Emit;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SampleLibrary;

namespace TaijiConsole
{
    // Step 1: Determine the class that needs to be modified
    // Step 2: Determine which class constructor needs to be modified
    // Step 3: Determine which dependency needs to be broken using the given parameter
    // Step 4: Determine where the target dependency is being used
    // Step 5: Determine which methods are being called on the target dependency
    // Step 6: Create an interface that contains only the methods used by the target dependency instances
    // Step 7: Replace all type instances of the target dependency with the new interface
    // Step 8: Replace all method calls to the target dependency with calls to the new interface
    // Step 9: Create an instance of the adapter at each method call site
    // Step 10: Delegate the adapter instantiation to an IOC container

    class Program
    {
        static void Main(string[] args)
        {
            var targetAssembly = AssemblyFactory.GetAssembly("SampleLibrary.dll");
            var module = targetAssembly.MainModule;


            //var scope = new TypeScope(targetType);
            IDependencyScope scope = new ModuleScope(module);

            var targetType = GetTargetType(module, t => t.Name == "Greeter");
            var targetDependency = GetTargetType(module, t => t.Name == "Writer");
            ExtractInterfaces(module, targetDependency, targetType, scope);

            //UpdateClientMethods(module, targetDependency, interfaceType, adapterType, clientMethods, modifiedMethods);

            // TODO: Add adapters to the client code
            AssemblyFactory.SaveAssembly(targetAssembly, "output.dll");
            return;
        }

        private static void ExtractInterfaces(ModuleDefinition module, TypeDefinition targetDependency, TypeDefinition targetType, IDependencyScope scope)
        {

            var methodMap = new Dictionary<MethodReference, MethodReference>();

            var interfaceName = string.Format("I{0}", targetDependency.Name);
            var namespaceName = targetType.Namespace;

            var methodFinder = new MethodFinder();
            var methodFilter = new InvokedMethodFilter();
            var addInterfaceMethod = new AddInterfaceMethod();
            var addAdapterMethod = new AddAdapterMethod();
            var injectAdapterAsParameter = new InjectAdapterAsParameter();
            var callFilter = new InvalidCallFilter();
            var popMethodArguments = new PopMethodArguments();

            var methodBuilder = new AddInterfaceMethodIfMethodNotFound(methodFinder, addInterfaceMethod, methodMap);
            var createInterfaceType = new CreateInterfaceType(methodFilter,
                methodBuilder,
                scope,
                targetDependency,
                module);
            var interfaceType = createInterfaceType.CreateType(interfaceName,
                                                          namespaceName,
                                                          methodMap);

            var modifiedMethods = new HashSet<MethodReference>();
            var modifiedConstructors = new HashSet<MethodReference>();

            var extractor = new DependencyExtractor(interfaceType, methodMap, modifiedMethods, modifiedConstructors);
            extractor.Extract(targetDependency, targetType.Module, scope);

            //// Step 9: Create an adapter that maps the calls back to the interface 
            
            var addAdapterConstructor = new AddAdapterConstructor(targetDependency);
            var createAdapterType = new CreateAdapterType(module, targetDependency, interfaceType, addAdapterConstructor, addAdapterMethod);
            var adapterBuilder = new AdapterBuilder(interfaceType, targetDependency, createAdapterType);
            
            var pushParameter = new PushParameterOntoArgumentStack(injectAdapterAsParameter);
            var pushMethodArguments = new PushMethodArguments(adapterBuilder, pushParameter, interfaceType, targetDependency,
                                                              methodMap);
            
            var replaceMethodCall = new InjectInterfaceMethodCall(pushMethodArguments, popMethodArguments);
            var replaceMethodBody = new ReplaceMethodCalls(module, replaceMethodCall, callFilter);


            var updater = new DependencyUpdater(replaceMethodBody);
            //// Step 10: Scan the rest of the assembly and create an instance of the adapter at each method call site
            updater.UpdateAffectedMethods(scope.Methods, modifiedConstructors, methodMap);
        }

        private static TypeDefinition GetTargetType(ModuleDefinition module, Func<TypeReference, bool> typePredicate)
        {
            TypeDefinition targetType = null;
            foreach (TypeDefinition type in module.Types)
            {
                if (!typePredicate(type))
                    continue;

                targetType = type;
                break;
            }
            return targetType;
        }
    }
}
