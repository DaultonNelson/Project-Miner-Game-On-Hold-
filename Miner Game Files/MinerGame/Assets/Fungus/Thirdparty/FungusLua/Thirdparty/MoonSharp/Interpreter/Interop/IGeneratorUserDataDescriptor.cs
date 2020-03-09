using System;

namespace MoonSharp.Interpreter.Interop
{
	/// <summary>
	/// An interface for type descriptors having the ability to generate other descriptors on the fly.
	/// </summary>
	public interface IGeneratorUserDataDescriptor : IUserDataDescriptor
	{
		/// <summary>
		/// 
		/// Generates a new descriptor for the specified type.
		/// 
		/// The purpose is to allow a mechanism by which a type descriptor can replace itself with another
		/// descriptor for a specific type. For example, descriptors can be created on the fly to support
		/// generic types through this mechanism.
		/// 
		/// The return value should be:
		///		null - if this descriptor is simply skipped for the specified type
		///		this - acts as if the descriptor was a vanilla descriptor
		///		a new descriptor - if a new descriptor should be used in place of this one
		///		
		/// It's recommended that instances of descriptors are cached for future references. One possible way,
		/// to do the caching is to have the generator register the descriptor through <see cref="UserData.RegisterType"/>. 
		/// In that case, it should query whether the type is exactly registered, through <see cref="UserData.IsTypeRegistered"/>
		/// 
		/// 
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>Null, this object instance or a new descriptor.</returns>
		IUserDataDescriptor Generate(Type type);
	}
}
