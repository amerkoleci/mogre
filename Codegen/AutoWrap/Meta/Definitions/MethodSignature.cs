namespace AutoWrap.Meta
{
    /// <summary>
    /// The native (C++) signature of a method. That is: method name, parameters (
    /// </summary>
    public class MethodSignature
    {
        private readonly string _partialSignature;
        private readonly string _completeSignature;

        public MethodSignature(MemberMethodDefinition methodDef)
        {
            _partialSignature = methodDef.NativeName;
            foreach (ParamDefinition param in methodDef.Parameters)
            {
                if (param.Container != "" || param.Array != "")
                {
                    int x = 0;
                    x++;
                }
                _partialSignature += "|" + param.TypeName + "#" + param.PassedByType + "#" + param.Container + "#" + param.Array;
            }

            _completeSignature = methodDef.IsVirtual.ToString() + methodDef.ProtectionLevel + _partialSignature;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MethodSignature);
        }

        public bool Equals(MethodSignature obj, bool strictCompare = true)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj == null)
                return false;

            if (strictCompare)
                return _completeSignature == obj._completeSignature;
            
            return _partialSignature == obj._partialSignature;
        }

        public override int GetHashCode()
        {
            return _completeSignature.GetHashCode();
        }

        public static bool operator ==(MethodSignature a, MethodSignature b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
                return true;

            // If one is null, but not both, return false.
            // IMPORTANT: Convert to "object" to avoid endless recursion of this operator.
            if ((object)a == null || (object)b == null)
                return false;

            // Return true if the fields match:
            return a._completeSignature == b._completeSignature;
        }

        public static bool operator !=(MethodSignature a, MethodSignature b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return _completeSignature;
        }
    }
}