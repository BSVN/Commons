using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace BSN.Commons.Utilities
{
    /// <summary>
    /// TODO
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ProtoImplementAttribute : Attribute
    {
        /// <summary>
        /// This constructor defines one required parameter
        /// </summary>
        /// <param name="BaseType">Type of base class or interface that you want to implement it</param>
        public ProtoImplementAttribute(Type BaseType)
        {
            this.BaseType = BaseType;
        }

        internal Type BaseType { get; }
    }

    /// <summary>
    /// Active polymorphism for protobuf-net code first approach.
    /// This class is used to enable polymorphism for protobuf-net code first approach on a specific assembly based on ProtoImplementAttribute.
    /// </summary>
    /// <remarks>
    /// You must call this class in the startup of your application.
    /// </remarks>
    public static class GrpcPolymorphismActivator
    {
        /// <summary>
        /// Enable polymorphism for protobuf-net code first approach on a specific assembly based on ProtoImplementAttribute.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="extraTypes"></param>
        /// <exception cref="Exception"></exception>
        public static void Enable(Assembly assembly, (Type, Type)[] extraListOfDeriveds)
        {
            // TODO: Use C# source generator https://stackoverflow.com/q/64926889/1539100

            // ProtoBuf.Meta.RuntimeTypeModel.Default.Add(typeof(Response), false)
            //        .Add(1, nameof(Response.StatusCode))
            //        .Add(2, nameof(Response.Message))
            //        .Add(3, nameof(Response.InvalidItems))
            //        .AddSubType(100, typeof(ErrorResponse))
            //        .AddSubType(101, typeof(Response<SayHelloViewModel>));

            Dictionary<Type, List<Type>> listOfDerivedTypes = new Dictionary<Type, List<Type>>();
            foreach (var type in from type in assembly.GetTypes()
                                 where type.GetCustomAttribute<ProtoImplementAttribute>() != null
                                 select type)
            {
                ProtoImplementAttribute protoImplementAttribute = type.GetCustomAttribute<ProtoImplementAttribute>();

                if (listOfDerivedTypes.TryGetValue(protoImplementAttribute.BaseType, out List<Type> derivedTypes))
                    derivedTypes.Add(type);
                else
                    listOfDerivedTypes.Add(protoImplementAttribute.BaseType, new List<Type>() { type });
            }

            foreach ((Type @base, Type derived) in extraListOfDeriveds)
            {
                if (listOfDerivedTypes.TryGetValue(@base, out List<Type> derivedTypes))
                    derivedTypes.Add(derived);
                else
                    listOfDerivedTypes.Add(@base, new List<Type>() { derived });
            }

            foreach (var derivedType in listOfDerivedTypes)
            {
                MetaType @base = ProtoBuf.Meta.RuntimeTypeModel.Default.Add(derivedType.Key, false);
                int maxOrder = 0;
                foreach (var property in derivedType.Key.GetProperties())
                {
                    DataMemberAttribute dataMemberAttribute = property.GetCustomAttribute<DataMemberAttribute>();
                    if (property.GetCustomAttribute<DataMemberAttribute>() == null)
                        continue;
                    @base = @base.Add(dataMemberAttribute.Order, dataMemberAttribute.Name ?? property.Name);
                    maxOrder = Math.Max(maxOrder, dataMemberAttribute.Order);
                }

                // Reserve some order for well-known derived types
                maxOrder += 100;

                foreach (var type in derivedType.Value)
                {
                    @base.AddSubType(maxOrder + 1, type);
                }
            }
        }
    }
}
