using System;

namespace AutoWrap.Meta
{
    /// <summary>
    /// Defines a type parameter for a C++ templated (=generic) type (like <c>std::list</c>).
    /// </summary>
    public class TypeParamDefinition : ITypeMember
    {
        public readonly AbstractTypeDefinition ParamType;

        #region ITypeMember implementations

        string ITypeMember.MemberTypeName
        {
            get { return ParamType.Name; }
        }

        private readonly PassedByType _passed;
        public PassedByType PassedByType
        {
            get { return _passed; }
        }

        ClassDefinition ITypeMember.ContainingClass
        {
            get { throw new InvalidOperationException("Illogical call"); }
        }

        AbstractTypeDefinition ITypeMember.MemberType
        {
            get { return ParamType; }
        }

        private readonly bool _isConst;
        public virtual bool IsConst
        {
            get { return _isConst; }
        }

        public virtual string MemberTypeNativeName
        {
            get { return ParamType.GetNativeTypeName(IsConst, (this as ITypeMember).PassedByType); }
        }

        private string _clrTypeName;
        public virtual string MemberTypeCLRName 
        {
            get
            {
                if (_clrTypeName == null)
                    _clrTypeName = ParamType.GetCLRTypeName(this);

                return _clrTypeName;
            }
        }

        bool ITypeMember.HasAttribute<T>()
        {
            return false;
        }

        T ITypeMember.GetAttribute<T>()
        {
            throw new InvalidOperationException("Illogical call");
        }

        #endregion

        public TypeParamDefinition(AbstractTypeDefinition type, PassedByType passed, bool isConst)
        {
            ParamType = type;
            _passed = passed;
            _isConst = isConst;
        }
    }
}