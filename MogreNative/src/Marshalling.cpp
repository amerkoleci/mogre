#include "stdafx.h"
#include "Marshalling.h"
#include <iostream>
#include <stdio.h>

namespace Mogre
{
	void InitNativeStringWithCLRString(Ogre::String& ostr, System::String^ mstr)
	{
		if (mstr == nullptr)
			throw gcnew System::NullReferenceException("A null string cannot be converted to an Ogre string.");

		IntPtr p_mstr = Marshal::StringToHGlobalAnsi(mstr);
		ostr = static_cast<char*>(p_mstr.ToPointer());
		Marshal::FreeHGlobal( p_mstr );

	}

	void InitNativeUTFStringWithCLRString(Ogre::UTFString& ostr, System::String^ mstr)
	{
		if (mstr == nullptr)
			throw gcnew System::NullReferenceException("A null string cannot be converted to an Ogre UTFString.");
				
		pin_ptr<const wchar_t> p_mstr = PtrToStringChars(mstr);	
		Ogre::UTFString tmp;
		ostr = tmp.assign(p_mstr);
	}

	void FillMapFromNameValueCollection( std::map<Ogre::String,Ogre::String>& map, Collections::Specialized::NameValueCollection^ col )
	{
		int count = col->Count;
		for (int i=0; i < count; i++)
		{
			DECLARE_NATIVE_STRING(o_key, col->Keys[i]);
			DECLARE_NATIVE_STRING(o_val, col[i]);

			map.insert( std::pair<Ogre::String,Ogre::String>( o_key, o_val ) );
		}
	}

	Vector2 ToVector2(Ogre::Vector2 vector)
	{
		return Vector2(vector.x, vector.y);
	}

	Ogre::Vector2 FromVector2(Vector2 vector)
	{
		return Ogre::Vector2(vector.X, vector.Y);
	}

	Vector3 ToVector3(Ogre::Vector3 vector)
	{
		return Vector3(vector.x, vector.y, vector.z);
	}

	Ogre::Vector3 FromVector3(Vector3 vector)
	{
		return Ogre::Vector3(vector.X, vector.Y, vector.Z);
	}

	Vector4 ToVector4(Ogre::Vector4 vector)
	{
		return Vector4(vector.x, vector.y, vector.z, vector.w);
	}

	Ogre::Vector4 FromVector4(Vector4 vector)
	{
		return Ogre::Vector4(vector.X, vector.Y, vector.Z, vector.W);
	}

	Quaternion ToQuaternion(Ogre::Quaternion value)
	{
		return Quaternion(value.w, value.x, value.y, value.z);
	}

	Ogre::Quaternion FromQuaternion(Quaternion value)
	{
		return Ogre::Quaternion(value.W, value.X, value.Y, value.Z);
	}

	Mogre::ColourValue ToColor4(Ogre::ColourValue value)
	{
		return Mogre::ColourValue(value.r, value.g, value.b, value.a);
	}

	Ogre::ColourValue FromColor4(Mogre::ColourValue value)
	{
		return Ogre::ColourValue(value.R, value.G, value.B, value.A);
	}

	Plane ToPlane(Ogre::Plane value)
	{
		Plane result = Plane();
		result.normal = ToVector3(value.normal);
		result.d = value.d;
		return result;
	}

	Ogre::Plane FromPlane(Plane value)
	{
		Ogre::Plane result;
		result.normal = FromVector3(value.normal);
		result.d = value.d;
		return result;
	}

	Sphere ToSphere(Ogre::Sphere value)
	{
		return Sphere(ToVector3(value.getCenter()), value.getRadius());
	}

	Ogre::Sphere FromSphere(Sphere value)
	{
		return Ogre::Sphere(FromVector3(value.Center), value.Radius);
	}

	Ray ToRay(Ogre::Ray value)
	{
		return Ray(ToVector3(value.getOrigin()), ToVector3(value.getDirection()));
	}

	Ogre::Ray FromRay(Ray value)
	{
		return Ogre::Ray(FromVector3(value.Origin), FromVector3(value.Direction));
	}

	AxisAlignedBox^ ToAxisAlignedBounds(Ogre::AxisAlignedBox value)
	{
		return gcnew AxisAlignedBox(ToVector3(value.getMinimum()), ToVector3(value.getMaximum()));
	}

	Ogre::AxisAlignedBox FromAxisAlignedBounds(AxisAlignedBox^ value)
	{
		return Ogre::AxisAlignedBox(FromVector3(value->Minimum), FromVector3(value->Maximum));
	}

	Matrix3 ToMatrix3(Ogre::Matrix3 value)
	{
		float* ptr = value[0];
		return Matrix3(
			ptr[0], ptr[1], ptr[2],
			ptr[3], ptr[4], ptr[5],
			ptr[6], ptr[7], ptr[8]);
	}

	Ogre::Matrix3 FromMatrix3(Matrix3^ value)
	{
		pin_ptr<Ogre::Matrix3> p_val = interior_ptr<Ogre::Matrix3>(&value->m00);
		return Ogre::Matrix3(*p_val);
	}

	Matrix4 ToMatrix4(Ogre::Matrix4 value)
	{
		float* ptr = value[0];
		return Matrix4(
			ptr[0], ptr[1], ptr[2], ptr[3], 
			ptr[4], ptr[5], ptr[6], ptr[7], 
			ptr[8], ptr[9], ptr[10], ptr[11],
			ptr[12], ptr[13], ptr[14], ptr[15]);
	}

	Ogre::Matrix4 FromMatrix4(Matrix4^ value)
	{
		pin_ptr<Ogre::Matrix4> p_val = interior_ptr<Ogre::Matrix4>(&value->m00);
		return Ogre::Matrix4(*p_val);
	}
}