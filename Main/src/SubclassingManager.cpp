#include "MogreStableHeaders.h"
#include "SubclassingManager.h"

namespace Mogre
{
	namespace Implementation
	{
		unsigned char cachedReturn[CACHED_RETURN_SIZE];
		Ogre::String cachedReturnString;

		bool* SubclassingManager::GetOverridenMethodsArrayPointer(Type^ subclass, Type^ baseclass, int arraySize)
		{
			IntPtr ptr;
			if (!_overridenMethodArrays->TryGetValue(subclass, ptr))
			{
				bool* parr = new bool[arraySize];
				memset(parr, 0, arraySize * sizeof(bool));

				SetOverridenMethods(subclass, baseclass, parr, arraySize);
				_overridenMethodArrays->Add(subclass, (IntPtr)parr);
				return parr;
			}

			return (bool*)(void*)ptr;
		}

		void SubclassingManager::SetOverridenMethods(Type^ subclass, Type^ baseclass, bool* pOverriden, int arraySize)
		{
			if (subclass == baseclass)
				return;

			array<MethodInfo^>^ methods = subclass->GetMethods(BindingFlags::Instance | BindingFlags::DeclaredOnly | BindingFlags::Public | BindingFlags::NonPublic);
            for each (MethodInfo^ mi in methods)
            {
				if (mi->IsVirtual)
				{
					array<MethodIndexAttribute^>^ attrs = static_cast<array<MethodIndexAttribute^>^>(mi->GetCustomAttributes(MethodIndexAttribute::typeid, true));
					if (attrs->Length > 0)
					{
						assert( attrs[0]->index < arraySize );
						pOverriden[attrs[0]->index] = true;
					}
				}
            }

			SetOverridenMethods(subclass->BaseType, baseclass, pOverriden, arraySize);
		}
	}
}
