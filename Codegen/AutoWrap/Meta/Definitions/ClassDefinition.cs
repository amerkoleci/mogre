using System;
using System.Collections.Generic;
using System.Xml;
using System.Collections;

namespace AutoWrap.Meta
{
    /// <summary>
    /// Describes a class.
    /// </summary>
    /// <seealso cref="StructDefinition"/>
    public class ClassDefinition : AbstractTypeDefinition
    {
        /// <summary>
        /// Contains the members (fields and methods) of this class.
        /// </summary>
        public List<MemberDefinitionBase> Members = new List<MemberDefinitionBase>();

        /// <summary>
        /// Contains the types nested in this class (e.g. inner classes).
        /// </summary>
        public List<AbstractTypeDefinition> NestedTypes = new List<AbstractTypeDefinition>();

        /// <summary>
        /// This array contains the names of the base class and all directly implemented interfaces
        /// (or the base interfaces, if this is an interface). The base 
        /// </summary>
        public readonly string[] BaseClassesNames;

        public bool AllowVirtuals
        {
            get { return AllowSubClassing || IsInterface; }
        }

        /// <summary>
        /// Denotes whether subclasses of this class can be created.
        /// </summary>
        public bool AllowSubClassing
        {
            get { return HasWrapType(WrapTypes.Overridable) || (BaseClass != null && BaseClass.AllowSubClassing); }
        }

        /// <summary>
        /// Denotes whether the native class is an abstract class.
        /// </summary>
        public virtual bool IsNativeAbstractClass
        {
            get { return AllAbstractFunctions.Length > 0; }
        }

        public override bool IsIgnored
        {
            get
            {
                if (base.IsIgnored)
                    return true;

                if (SurroundingClass != null)
                    return SurroundingClass.IsIgnored;
                else
                    return false;
            }
        }

        public override bool IsSharedPtr
        {
            get { return (BaseClass != null && BaseClass.Name == "SharedPtr"); }
        }

        public virtual bool IsSingleton
        {
            get
            {
                if (BaseClassesNames == null)
                    return false;

                foreach (string tn in BaseClassesNames)
                {
                    if (tn.StartsWith("Singleton<"))
                        return true;
                }

                return false;
            }
        }

        public override string CLRName
        {
            get
            {
                if (HasWrapType(WrapTypes.NativePtrValueType))
                {
                    return base.CLRName + "_NativePtr";
                }

                if (IsInterface)
                    return "I" + base.CLRName;
                else
                    return base.CLRName;
            }
        }

        /// <summary>
        /// Denotes whether this class definition actually describes an interface.
        /// </summary>
        public virtual bool IsInterface
        {
            get { return HasWrapType(WrapTypes.Interface); }
        }

        private MemberMethodDefinition[] _allAbstractFunctions = null;

        /// <summary>
        /// All abstract functions
        /// </summary>
        public virtual MemberMethodDefinition[] AllAbstractFunctions
        {
            get
            {
                if (_allAbstractFunctions == null)
                {
                    List<MemberMethodDefinition> list = new List<MemberMethodDefinition>();

                    if (BaseClass != null)
                    {
                        foreach (MemberMethodDefinition func in BaseClass.AllAbstractFunctions)
                        {
                            if (!ContainsFunctionSignature(func.Signature, false))
                                list.Add(func);
                        }
                    }

                    foreach (MemberMethodDefinition func in Methods)
                    {
                        if (func.IsAbstract)
                            list.Add(func);
                    }

                    _allAbstractFunctions = list.ToArray();
                }

                return _allAbstractFunctions;
            }
        }

        private MemberMethodDefinition[] _abstractFunctions = null;

        /// <summary>
        /// Only declarable abstract functions
        /// </summary>
        public virtual MemberMethodDefinition[] AbstractFunctions
        {
            get
            {
                if (_abstractFunctions == null)
                {
                    List<MemberMethodDefinition> list = new List<MemberMethodDefinition>();

                    if (BaseClass != null)
                    {
                        foreach (MemberMethodDefinition func in BaseClass.AbstractFunctions)
                        {
                            if (!ContainsFunctionSignature(func.Signature, false))
                                list.Add(func);
                        }
                    }

                    foreach (MemberMethodDefinition func in Methods)
                    {
                        if (func.IsDeclarableFunction && func.IsAbstract)
                            list.Add(func);
                    }

                    _abstractFunctions = list.ToArray();
                }

                return _abstractFunctions;
            }
        }

        private MemberPropertyDefinition[] _abstractProperties = null;
        public virtual MemberPropertyDefinition[] AbstractProperties
        {
            get
            {
                if (_abstractProperties == null)
                {
                    List<MemberPropertyDefinition> list = new List<MemberPropertyDefinition>();

                    if (BaseClass != null)
                    {
                        foreach (MemberPropertyDefinition prop in BaseClass.AbstractProperties)
                        {
                            if (!prop.IsContainedIn(this, false))
                                list.Add(prop);
                        }
                    }

                    foreach (MemberPropertyDefinition prop in GetProperties())
                    {
                        if (prop.IsAbstract)
                            list.Add(prop);
                    }

                    _abstractProperties = list.ToArray();
                }

                return _abstractProperties;
            }
        }

        public IEnumerable PublicNestedTypes
        {
            get
            {
                foreach (AbstractTypeDefinition type in NestedTypes)
                {
                    if (type.ProtectionLevel == ProtectionLevel.Public)
                        yield return type;
                }
            }
        }

        public IEnumerable<MemberMethodDefinition> Methods
        {
            get
            {
                foreach (MemberDefinitionBase m in Members)
                {
                    if (m is MemberMethodDefinition)
                        yield return (MemberMethodDefinition)m;
                }
            }
        }

        public IEnumerable<MemberFieldDefinition> Fields
        {
            get
            {
                foreach (MemberDefinitionBase m in Members)
                {
                    if (m is MemberFieldDefinition)
                        yield return (MemberFieldDefinition)m;
                }
            }
        }

        public IEnumerable<MemberMethodDefinition> PublicMethods
        {
            get
            {
                foreach (MemberMethodDefinition func in Methods)
                {
                    if (!func.IsProperty
                        && func.ProtectionLevel == ProtectionLevel.Public)
                        yield return func;
                }
            }
        }

        public IEnumerable<MemberMethodDefinition> DeclarableMethods
        {
            get
            {
                foreach (MemberMethodDefinition func in Methods)
                {
                    if (func.IsDeclarableFunction
                        && !func.IsProperty)
                        yield return func;
                }
            }
        }

        public IEnumerable<MemberMethodDefinition> ProtectedMethods
        {
            get
            {
                foreach (MemberMethodDefinition func in Methods)
                {
                    if (!func.IsProperty
                        && func.ProtectionLevel == ProtectionLevel.Protected)
                        yield return func;
                }
            }
        }

        public IEnumerable<MemberFieldDefinition> PublicFields
        {
            get
            {
                foreach (MemberFieldDefinition f in Fields)
                {
                    if (f.ProtectionLevel == ProtectionLevel.Public)
                        yield return f;
                }
            }
        }

        public IEnumerable<MemberFieldDefinition> ProtectedFields
        {
            get
            {
                foreach (MemberFieldDefinition f in Fields)
                {
                    if (f.ProtectionLevel == ProtectionLevel.Protected)
                        yield return f;
                }
            }
        }

        #region Code Producing Methods

        public override void ProduceNativeParamConversionCode(ParamDefinition param, out string preConversion, out string conversion, out string postConversion)
        {
            switch (param.PassedByType)
            {
                case PassedByType.PointerPointer:
                    preConversion = FullyQualifiedCLRName + "^ out_" + param.Name + ";";
                    conversion = "out_" + param.Name;
                    postConversion = "if (" + param.Name + ") *" + param.Name + " = out_" + param.Name + ";";
                    return;
            }

            base.ProduceNativeParamConversionCode(param, out preConversion, out conversion, out postConversion);
        }

        public override void ProduceDefaultParamValueConversionCode(ParamDefinition param, out string preConversion, out string conversion, out string postConversion, out AbstractTypeDefinition dependancyType)
        {
            if (param.DefaultValue == null)
                throw new Exception("Unexpected");

            preConversion = postConversion = "";
            dependancyType = null;
            switch (param.PassedByType)
            {
                case PassedByType.Pointer:
                    if (param.DefaultValue == "NULL" || param.DefaultValue == "0")
                    {
                        conversion = "nullptr";
                        return;
                    }
                    else
                        throw new Exception("Unexpected");
                case PassedByType.PointerPointer:
                    throw new Exception("Unexpected");
                default:
                    conversion = param.DefaultValue;
                    break;
            }
        }

        public override string ProducePreCallParamConversionCode(ParamDefinition param, out string newname)
        {
            if (param.Type.IsSharedPtr)
            {
                newname = "(" + param.MemberTypeNativeName + ")" + param.Name;
                return String.Empty;
            }

            if (HasAttribute<NativeValueContainerAttribute>())
            {
                switch (param.PassedByType)
                {
                    case PassedByType.Pointer:
                        newname = "o_" + param.Name;
                        return param.MemberTypeNativeName + " o_" + param.Name + " = reinterpret_cast<" + param.MemberTypeNativeName + ">(" + param.Name + ");\n";
                }
            }

            string nativetype = FullyQualifiedNativeName;

            if (HasAttribute<PureManagedClassAttribute>())
            {
                string first = GetAttribute<PureManagedClassAttribute>().FirstMember;
                if (String.IsNullOrEmpty(first))
                {
                    switch (param.PassedByType)
                    {
                        case PassedByType.Reference:
                        case PassedByType.Value:
                            newname = "(" + nativetype + ")" + param.Name;
                            return null;
                        default:
                            throw new Exception("Unexpected");
                    }
                }
                else
                {
                    switch (param.PassedByType)
                    {
                        case PassedByType.Reference:
                        case PassedByType.Value:
                            newname = "*p_" + param.Name;
                            return "pin_ptr<" + nativetype + "> p_" + param.Name + " = interior_ptr<" + nativetype + ">(&" + param.Name + "->" + first + ");\n";
                        case PassedByType.Pointer:
                            if (param.IsConst)
                            {
                                string name = param.Name;
                                string expr = nativetype + "* arr_" + name + " = new " + nativetype + "[" + name + "->Length];\n";
                                expr += "for (int i=0; i < " + name + "->Length; i++)\n";
                                expr += "{\n";
                                expr += "\tarr_" + name + "[i] = *(reinterpret_cast<interior_ptr<" + nativetype + "&>>(&" + name + "[i]->" + first + "));\n";
                                expr += "}\n";
                                newname = "arr_" + name;
                                return expr;
                            }
                            else
                                throw new Exception("Unexpected");
                        default:
                            throw new Exception("Unexpected");
                    }
                }
            }

            switch (param.PassedByType)
            {
                case PassedByType.Pointer:
                    if (IsValueType)
                    {
                        if (!param.HasAttribute<ArrayTypeAttribute>() && !HasWrapType(WrapTypes.NativePtrValueType))
                        {
                            newname = "o_" + param.Name;
                            return param.MemberTypeNativeName + " o_" + param.Name + " = reinterpret_cast<" + param.MemberTypeNativeName + ">(" + param.Name + ");\n";
                        }

                        return base.ProducePreCallParamConversionCode(param, out newname);
                    }

                    return base.ProducePreCallParamConversionCode(param, out newname);

                case PassedByType.PointerPointer:
                    newname = "&out_" + param.Name;
                    return (param.IsConst ? "const " : "") + FullyQualifiedNativeName + "* out_" + param.Name + ";\n";

                default:
                    return base.ProducePreCallParamConversionCode(param, out newname);
            }
        }

        public override string ProducePostCallParamConversionCleanupCode(ParamDefinition param)
        {
            if (HasAttribute<NativeValueContainerAttribute>())
            {
                switch (param.PassedByType)
                {
                    case PassedByType.Pointer:
                        return "";
                }
            }

            if (param.Type.HasAttribute<PureManagedClassAttribute>())
            {
                switch (param.PassedByType)
                {
                    case PassedByType.Pointer:
                        if (param.IsConst)
                        {
                            return "delete[] arr_" + param.Name + ";\n";
                        }
                        else
                            throw new Exception("Unexpected");
                    default:
                        return base.ProducePostCallParamConversionCleanupCode(param);
                }
            }

            switch (param.PassedByType)
            {
                case PassedByType.PointerPointer:
                    return param.Name + " = out_" + param.Name + ";\n";
                default:
                    return base.ProducePostCallParamConversionCleanupCode(param);
            }
        }

        public override string ProduceNativeCallConversionCode(string expr, ITypeMember m)
        {
            if (HasAttribute<NativeValueContainerAttribute>())
            {
                switch (m.PassedByType)
                {
                    case PassedByType.Pointer:
                        return "reinterpret_cast<" + GetCLRTypeName(m) + ">(" + expr + ")";
                }
            }

            switch (m.PassedByType)
            {
                case PassedByType.Pointer:
                    if (IsValueType)
                    {
                        if (m.HasAttribute<ArrayTypeAttribute>())
                        {
                            int len = m.GetAttribute<ArrayTypeAttribute>().Length;
                            return "GetValueArrayFromNativeArray<" + FullyQualifiedCLRName + ", " + FullyQualifiedNativeName + ">( " + expr + " , " + len + " )";
                        }
                        else if (HasWrapType(WrapTypes.NativePtrValueType))
                        {
                            return expr;
                        }
                        else
                            return "reinterpret_cast<" + GetCLRTypeName(m) + ">(" + expr + ")";
                    }
                    else
                        return base.ProduceNativeCallConversionCode(expr, m);
                default:
                    return base.ProduceNativeCallConversionCode(expr, m);
            }
        }

        #endregion

        public override string GetCLRParamTypeName(ParamDefinition param)
        {
            if (HasAttribute<NativeValueContainerAttribute>())
            {
                switch (param.PassedByType)
                {
                    case PassedByType.Pointer:
                        //return "array<" + FullCLRName + "^>^";
                        return GetCLRTypeName(param);
                }
            }

            switch (param.PassedByType)
            {
                case PassedByType.PointerPointer:
                    return "[Out] " + FullyQualifiedCLRName + (IsValueType ? "%" : "^%");
                default:
                    return GetCLRTypeName(param);
            }
        }

        public override string GetCLRTypeName(ITypeMember m)
        {
            if (HasAttribute<NativeValueContainerAttribute>())
            {
                switch (m.PassedByType)
                {
                    case PassedByType.Pointer:
                        //return "array<" + FullCLRName + "^>^";
                        string name = FullyQualifiedCLRName + "::NativeValue*";
                        if (m.IsConst)
                            name = "const " + name;
                        return name;
                }
            }

            switch (m.PassedByType)
            {
                case PassedByType.Pointer:
                    if (IsValueType)
                    {
                        if (m.HasAttribute<ArrayTypeAttribute>())
                        {
                            return "array<" + FullyQualifiedCLRName + ">^";
                        }
                        else if (HasWrapType(WrapTypes.NativePtrValueType))
                        {
                            return FullyQualifiedCLRName;
                        }
                        else
                        {
                            string name = FullyQualifiedCLRName + "*";
                            if (m.IsConst)
                                name = "const " + name;
                            return name;
                        }
                    }
                    else
                        return FullyQualifiedCLRName + "^";
                case PassedByType.Reference:
                case PassedByType.Value:
                    if (IsSharedPtr)
                        return FullyQualifiedCLRName + "^";
                    else if (IsValueType)
                        return FullyQualifiedCLRName;
                    else
                        return FullyQualifiedCLRName + "^";
                case PassedByType.PointerPointer:
                default:
                    throw new Exception("Unexpected");
            }
        }


        /// <summary>
        /// Indicates whether a method with the specified signature is part of this class.
        /// </summary>
        /// <param name="signature">the signature</param>
        /// <param name="allowInheritedSignature">if this is <c>false</c> only this class will be
        /// checked for the signature. Otherwise all base classes will be checked as well.</param>
        public virtual bool ContainsFunctionSignature(MethodSignature signature, bool allowInheritedSignature)
        {
            MemberMethodDefinition f;
            return ContainsFunctionSignature(signature, allowInheritedSignature, out f);
        }

        /// <summary>
        /// Indicates whether a method with the specified signature is part of this class.
        /// </summary>
        /// <param name="signature">the signature</param>
        /// <param name="allowInheritedSignature">if this is <c>false</c> only this class will be
        /// checked for the signature. Otherwise all base classes will be checked as well.</param>
        /// <param name="basefunc">the method matching the signature</param>
        public virtual bool ContainsFunctionSignature(MethodSignature signature, bool allowInheritedSignature, out MemberMethodDefinition basefunc)
        {
            basefunc = null;
            if (allowInheritedSignature && BaseClass != null)
            {
                BaseClass.ContainsFunctionSignature(signature, allowInheritedSignature, out basefunc);
            }

            if (basefunc != null)
                return true;

            foreach (MemberMethodDefinition func in Methods)
            {
                if (func.Signature == signature)
                {
                    basefunc = func;
                    return true;
                }
            }

            return false;
        }

        public virtual bool ContainsInterfaceFunctionSignature(MethodSignature signature, bool inherit)
        {
            MemberMethodDefinition f;
            return ContainsInterfaceFunctionSignature(signature, inherit, out f);
        }

        public virtual bool ContainsInterfaceFunctionSignature(MethodSignature signature, bool inherit, out MemberMethodDefinition basefunc)
        {
            basefunc = null;
            if (inherit && BaseClass != null)
            {
                BaseClass.ContainsInterfaceFunctionSignature(signature, inherit, out basefunc);
            }

            if (basefunc != null)
                return true;

            foreach (ClassDefinition iface in GetInterfaces())
            {
                if (iface.ContainsFunctionSignature(signature, true, out basefunc))
                    return true;
            }

            return false;
        }

        private bool _isDirectSubclassOfCLRObject = false;

        public bool IsDirectSubclassOfCLRObject
        {
            get { return _isDirectSubclassOfCLRObject; }
        }

        private bool _baseClassSearched = false;
        private ClassDefinition _baseClass = null;
        /// <summary>
        /// The base class of this class. Is <c>null</c>, if there is no base class (i.e. the base class is
        /// <c>object</c>).
        /// </summary>
        public virtual ClassDefinition BaseClass
        {
            get
            {
                if (_baseClassSearched)
                    return _baseClass;

                _baseClassSearched = true;

                if (HasAttribute<BaseClassAttribute>())
                {
                    // Explicit base class name; used when the class has multiple base classes 
                    // (see attribute documentation).
                    string basename = GetAttribute<BaseClassAttribute>().BaseClassName;
                    _baseClass = DetermineType<ClassDefinition>(basename);
                }
                else
                {
                    if (BaseClassesNames == null)
                        return null;

                    if (this.IsDirectSubclassOfCLRObject)
                        return null;

                    List<ClassDefinition> inherits = new List<ClassDefinition>();
                    foreach (string tn in BaseClassesNames)
                    {
                        string n = tn;
                        if (n.Contains("<"))
                        {
                            if (n.StartsWith("Singleton<"))
                                continue;

                            n = n.Substring(0, n.IndexOf("<")).Trim();
                        }

                        inherits.Add(DetermineType<ClassDefinition>(n));
                    }

                    foreach (ClassDefinition t in inherits)
                    {
                        if (!t.IsInterface && !t.HasAttribute<IgnoreAttribute>())
                        {
                            _baseClass = t;
                            break;
                        }
                    }
                }

                return _baseClass;
            }
        }

        protected ClassDefinition[] _interfaces = null;

        public virtual ClassDefinition[] GetInterfaces()
        {
            if (_interfaces != null)
                return _interfaces;

            if (BaseClassesNames == null)
            {
                _interfaces = new ClassDefinition[] { };
                return _interfaces;
            }

            List<ClassDefinition> inherits = new List<ClassDefinition>();
            foreach (string tn in BaseClassesNames)
            {
                string n = tn;
                if (n.Contains("<"))
                {
                    if (n.StartsWith("Singleton<"))
                        continue;

                    n = n.Substring(0, n.IndexOf("<")).Trim();
                }

                inherits.Add(DetermineType<ClassDefinition>(n));
            }

            for (int i = 0; i < inherits.Count; i++)
            {
                if (!inherits[i].IsInterface)
                {
                    inherits.RemoveAt(i);
                    i--;
                }
            }

            _interfaces = inherits.ToArray();
            return _interfaces;
        }

        public virtual MemberPropertyDefinition GetProperty(string name, bool inherit)
        {
            MemberPropertyDefinition prop = null;
            foreach (MemberPropertyDefinition p in this.GetProperties())
            {
                if (p.Name == name)
                {
                    prop = p;
                    break;
                }
            }

            if (prop == null && inherit && BaseClass != null)
                return BaseClass.GetProperty(name, inherit);
            else
                return prop;
        }

        private MemberMethodDefinition[] _constructors;

        public virtual MemberMethodDefinition[] Constructors
        {
            get
            {
                if (_constructors == null)
                {
                    List<MemberMethodDefinition> list = new List<MemberMethodDefinition>();
                    foreach (MemberMethodDefinition f in Methods)
                    {
                        if (f.IsConstructor)
                            list.Add(f);
                    }

                    _constructors = list.ToArray();
                }

                return _constructors;
            }
        }

        private MemberPropertyDefinition[] _properties;
        public virtual MemberPropertyDefinition[] GetProperties()
        {
            if (_properties != null)
                return _properties;

            _properties = MemberPropertyDefinition.GetPropertiesFromMethods(this.Methods);

            if (GetInterfaces().Length > 0)
            {
                foreach (MemberPropertyDefinition prop in _properties)
                {
                    if (!prop.CanWrite)
                    {
                        foreach (ClassDefinition iface in GetInterfaces())
                        {
                            MemberPropertyDefinition ip = iface.GetProperty(prop.Name, true);
                            if (ip != null && ip.CanWrite)
                            {
                                prop.SetterFunction = ip.SetterFunction;
                                break;
                            }
                        }
                    }

                    if (!prop.CanRead)
                    {
                        foreach (ClassDefinition iface in GetInterfaces())
                        {
                            MemberPropertyDefinition ip = iface.GetProperty(prop.Name, true);
                            if (ip != null && ip.CanRead)
                            {
                                prop.GetterFunction = ip.GetterFunction;
                                break;
                            }
                        }
                    }
                }
            }

            return _properties;
        }

        public MemberFieldDefinition GetField(string name)
        {
            return GetField(name, true);
        }

        public MemberFieldDefinition GetField(string name, bool raiseException)
        {
            MemberFieldDefinition field = null;
            foreach (MemberFieldDefinition f in Fields)
            {
                if (f.NativeName == name)
                {
                    field = f;
                    break;
                }
            }

            if (field == null && raiseException)
                throw new Exception(String.Format("DefField not found for '{0}'", name));

            return field;
        }

        /// <summary>
        /// Returns a method with the specified native name. Note: If there more than one method
        /// with the same name, only the first (order is, however, arbitrary) will be returned.
        /// </summary>
        /// <param name="nativeName">the native name of the method</param>
        /// <param name="checkBaseClass">denotes whether the base class are to be search for the
        /// method, if it's not found in the current class</param>
        /// <param name="raiseException">denotes whether an exception shall be thrown when the
        /// specified method couldn't be found. If this is <c>false</c>, <c>null</c> will be returned
        /// instead.</param>
        public MemberMethodDefinition GetMethodByNativeName(string nativeName, bool checkBaseClass = false, bool raiseException = true)
        {
            foreach (MemberMethodDefinition f in Methods)
            {
                if (f.NativeName == nativeName)
                    return f;
            }

            if (checkBaseClass && BaseClass != null)
                return BaseClass.GetMethodByNativeName(nativeName, true, raiseException);

            if (raiseException)
                throw new Exception("Native method not found: " + nativeName);

            return null;
        }

        /// <summary>
        /// Returns a method with the specified CLR name. Note: If there more than one method
        /// with the same name, only the first (order is, however, arbitrary) will be returned.
        /// </summary>
        /// <param name="clrName">the CLR name of the method</param>
        /// <param name="checkBaseClass">denotes whether the base class are to be search for the
        /// method, if it's not found in the current class</param>
        /// <param name="raiseException">denotes whether an exception shall be thrown when the
        /// specified method couldn't be found. If this is <c>false</c>, <c>null</c> will be returned
        /// instead.</param>
        public MemberMethodDefinition GetMethodByCLRName(string clrName, bool checkBaseClass = false, bool raiseException = true)
        {
            foreach (MemberMethodDefinition f in Methods)
            {
                if (f.CLRName == clrName)
                    return f;
            }

            if (checkBaseClass && BaseClass != null)
                return BaseClass.GetMethodByNativeName(clrName, true, raiseException);

            if (raiseException) 
                throw new Exception("CLR method not found: " + clrName);

            return null;
       }

       public virtual MemberMethodDefinition GetMethodWithSignature(MethodSignature signature)
       {
            foreach (MemberMethodDefinition func in this.Methods)
            {
                if (func.Signature == signature)
                    return func;
            }

            return null;
        }

        public AbstractTypeDefinition GetNestedType(string name)
        {
            return GetNestedType(name, true);
        }

        public AbstractTypeDefinition GetNestedType(string name, bool raiseException)
        {
            foreach (AbstractTypeDefinition t in NestedTypes)
            {
                if (t.Name == name)
                    return t;
            }

            if (raiseException)
                throw new Exception(String.Format("DefType not found for '{0}'", name));
            else
                return null;
        }

        public MemberDefinitionBase GetMember(string name)
        {
            return GetMember(name, true);
        }

        public MemberDefinitionBase GetMember(string name, bool raiseException)
        {
            foreach (MemberDefinitionBase m in Members)
            {
                if (m.NativeName == name)
                    return m;
            }

            if (raiseException)
                throw new Exception(String.Format("DeMember not found for '{0}'", name));
            else
                return null;
        }

        public MemberDefinitionBase[] GetMembers(string name)
        {
            return GetMembers(name, true);
        }

        public MemberDefinitionBase[] GetMembers(string name, bool raiseException)
        {
            List<MemberDefinitionBase> list = new List<MemberDefinitionBase>();

            foreach (MemberDefinitionBase m in Members)
            {
                if (m.NativeName == name)
                    list.Add(m);
            }

            if (list.Count == 0 && raiseException)
                throw new Exception(String.Format("DefMembers not found for '{0}'", name));

            return list.ToArray();
        }

        public virtual bool HasAttribute<T>(bool inherit) where T : AutoWrapAttribute
        {
            bool local = HasAttribute<T>();

            if (inherit)
            {
                if (local)
                    return true;
                if (BaseClass == null)
                    return false;
                return BaseClass.HasAttribute<T>(inherit);
            }
            else
                return local;
        }

        public virtual T GetAttribute<T>(bool inherit) where T : AutoWrapAttribute
        {
            T attr = GetAttribute<T>();

            if (inherit)
            {
                if (attr != null)
                    return attr;
                if (BaseClass == null)
                    return default(T);
                return BaseClass.GetAttribute<T>(inherit);
            }
            else
                return attr;
        }

        /// <summary>
        /// Finds a type (e.g. class, enum, typedef) as if it was used directly at a certain position in the code
        /// (i.e. from the type definition's perspective). See base method for more information.
        /// </summary>
        /// <param name="name">the name of the type. Can be fully qualified (i.e. with namespace and surrounding
        /// class).</param>
        /// <param name="raiseException">indicates whether an exception is to be thrown when the type can't
        /// be found. If this is <c>false</c>, an instance of <see cref="DefInternal"/> will be returned when
        /// the type couldn't be found.</param>
        public override T DetermineType<T>(string name, bool raiseException = true)
        {
            if (name.StartsWith(this.MetaDef.NativeNamespace + "::"))
            {
                name = name.Substring(name.IndexOf("::") + 2);
                return Namespace.DetermineType<T>(name, raiseException);
            }

            List<AbstractTypeDefinition> list = new List<AbstractTypeDefinition>();

            foreach (AbstractTypeDefinition t in NestedTypes)
            {
                if (t is T && t.Name == name)
                {
                    list.Add(t);
                }
            }

            if (list.Count == 0)
            {
                if (BaseClass != null)
                {
                    T t = BaseClass.DetermineType<T>(name, false);
                    if (!(t is DefInternal))
                        return t;
                }

                if (SurroundingClass != null)
                    return SurroundingClass.DetermineType<T>(name, raiseException);
                else
                    return Namespace.DetermineType<T>(name, raiseException);
            }
            else if (list.Count > 1)
                throw new Exception("Found more than one type");
            else
            {
                return (T)(object)list[0].ResolveType();
            }
        }

        public ClassDefinition(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
            : base(nsDef, surroundingClass, elem)
        {
            if (GetType() == typeof(ClassDefinition) && elem.Name != "class")
                throw new Exception("Not class element");

            foreach (XmlElement child in elem.ChildNodes)
            {
                switch (child.Name)
                {
                    case "function":
                        MemberMethodDefinition func = new MemberMethodDefinition(child, this);
                        if (func.NativeName != "DECLARE_INIT_CLROBJECT_METHOD_OVERRIDE" && func.NativeName != "_Init_CLRObject" && !func.NativeName.StartsWith("OGRE_"))
                            AddNewFunction(func);
                        break;

                    case "variable":
                        MemberFieldDefinition field = new MemberFieldDefinition(child, this);
                        if (field.NativeName != Name && field.NativeName != "DECLARE_CLRHANDLE" && field.NativeName != "_CLRHandle" && !field.NativeName.StartsWith("OGRE_"))
                        {
                            Members.Add(field);
                        }
                        break;

                    case "inherits":
                        List<string> ilist = new List<string>();
                        foreach (XmlElement sub in child.ChildNodes)
                        {
                            if (sub.Name != "baseClass")
                                throw new Exception("Unknown element; expected 'baseClass'");
                            if (sub.InnerText != "")
                            {
                                if (sub.InnerText == "CLRObject")
                                {
                                    this.AddAttribute(new CLRObjectAttribute());
                                    this._isDirectSubclassOfCLRObject = true;
                                }
                                else
                                    ilist.Add(sub.InnerText);
                            }
                        }
                        BaseClassesNames = ilist.ToArray();
                        break;

                    case "summary":
                        //This is interpreted in the base.
                        break;

                    default:
                        AbstractTypeDefinition type = MetaDef.Factory.CreateType(Namespace, this, child);
                        NestedTypes.Add(type);
                        break;
                }
            }
        }

        private void AddNewFunction(MemberMethodDefinition func)
        {
            MemberDefinitionBase[] members = GetMembers(func.NativeName, false);
            MemberMethodDefinition prevf = null;
            foreach (MemberDefinitionBase m in members)
            {
                if (m is MemberMethodDefinition)
                {
                    if (((MemberMethodDefinition)m).Signature.Equals(func.Signature, false))
                    {
                        prevf = m as MemberMethodDefinition;
                        break;
                    }
                }
            }

            if (prevf != null)
            {
                if ((prevf.IsConstMethod && func.IsConstMethod)
                    || (!prevf.IsConstMethod && !func.IsConstMethod))
                {
                    //throw new Exception("couldn't pick a function to keep");
                    //Add it and sort them out later
                    Members.Add(func);
                }

                if ((prevf.ProtectionLevel == func.ProtectionLevel
                    && prevf.IsConstMethod)
                    || (prevf.ProtectionLevel == ProtectionLevel.Protected
                    && func.ProtectionLevel == ProtectionLevel.Public))
                {
                    Members.Remove(prevf);
                    Members.Add(func);
                }
                else
                {
                    // Don't add func to Members;
                }
            }
            else
                Members.Add(func);
        }

        public static string GetName(XmlElement elem)
        {
            if (elem.Name != "class")
                throw new Exception("Wrong element; expected 'class'.");

            return elem.GetAttribute("name");
        }

        public static string GetFullName(XmlElement elem)
        {
            if (elem.Name != "class")
                throw new Exception("Wrong element; expected 'class'.");

            string pfullname = elem.GetAttribute("fullName");
            if (pfullname == "")
                throw new Exception("class element with no 'fullName' attribute");

            return pfullname;
        }
    }
}