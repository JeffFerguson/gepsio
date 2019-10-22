using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Xml;

namespace JeffFerguson.Gepsio.IoC
{
    /// <summary>
    /// A very simple IoC container.
    /// </summary>
    public static class Container
    {
        private static Dictionary<Type, Type> registeredTypes;
        private static readonly Dictionary<Type, object> registeredInstances = new Dictionary< Type, object >();
        private static Assembly currentAssembly;
        private static Type[] allTypes;

        static Container()
        {
            registeredTypes = new Dictionary<Type, Type>();
//#if NETFX_CORE
//            currentAssembly = typeof(Container).GetTypeInfo().Assembly;
//#else
            currentAssembly = Assembly.GetExecutingAssembly();
//#endif
            allTypes = currentAssembly.GetTypes();
            RegisterAllTypes();
        }

        private static void RegisterAllTypes()
        {
            Register<IAttribute>();
            Register<IAttributeList>();
            Register<IDocument>();
            Register<INamespaceManager>();
            Register<INode>();
            Register<INodeList>();
            Register<IQualifiedName>();
            Register<ISchema>();
            Register<ISchemaElement>();
            Register<ISchemaSet>();
            Register<ISchemaType>();

			//register default XmlResolver
			RegisterInstance<XmlResolver>( new XmlUrlResolver( ) );
		}

		/// <summary>
		/// Resolves an interface to a created type.
		/// </summary>
		/// <typeparam name="TInterface">
		/// The interface to be resolved.
		/// </typeparam>
		/// <returns>
		/// An object of the type that implements the interface.
		/// </returns>
		internal static TInterface Resolve<TInterface>()
        {
			var type = typeof(TInterface);
			Debug.Assert( registeredInstances != null, nameof(registeredInstances) + " != null" );
			if( registeredInstances.ContainsKey( type ) ) return (TInterface) registeredInstances[type];
			Debug.Assert( registeredTypes != null, nameof(registeredTypes) + " != null" );
			if( registeredTypes.ContainsKey(type) == false)
                throw new KeyNotFoundException("Interface type not registered.");
            Type implementationType = registeredTypes[type];
            return (TInterface)Activator.CreateInstance(implementationType);
        }

        /// <summary>
        /// Registers an interface with the container.
        /// </summary>
        /// <remarks>
        /// Only the interface need be specified. The methods automatically finds the
        /// class that implements the interface and associates the class' type in the
        /// container with the interface.
        /// </remarks>
        /// <typeparam name="TInterface">
        /// The interface to be registered.
        /// </typeparam>
        private static void Register<TInterface>()
        {
            var implementationType = FindTypeWithInterfaceImplementation<TInterface>();
            registeredTypes.Add(typeof(TInterface), implementationType);
        }
		public static void RegisterInstance< TInterface >( object instance ) {
			Debug.Assert( registeredInstances != null, nameof(registeredInstances) + " != null" );

			var type = typeof(TInterface);
			if( registeredInstances.ContainsKey( type ) ) registeredInstances[type] = instance;
			else registeredInstances.Add( type, instance );
		}
        /// <summary>
        /// Finds the class that implements the given interface and returns its type.
        /// </summary>
        /// <typeparam name="TInterface">
        /// The interface whose implementation should be found.
        /// </typeparam>
        /// <returns>
        /// The type of class that implements the given interface.
        /// </returns>
        private static Type FindTypeWithInterfaceImplementation<TInterface>()
        {
            foreach (var currentType in allTypes) {
				if( currentType.GetTypeInfo( ).IsSubclassOf( typeof( TInterface ) ) ) return currentType;

                var typeInterfaces = currentType.GetInterfaces();
                foreach (var currentInterface in typeInterfaces)
                {
                    if (currentInterface.Equals(typeof(TInterface)) == true)
                        return currentType;
                }
            }
            throw new TypeLoadException();
        }
    }
}
