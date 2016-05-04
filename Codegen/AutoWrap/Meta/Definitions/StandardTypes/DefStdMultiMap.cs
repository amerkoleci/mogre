using System.Xml;

namespace AutoWrap.Meta
{
    internal class DefStdMultiMap : DefStdMap
    {
        public override string STLContainer
        {
            get { return "MultiMap"; }
        }

        public override string FullSTLContainerTypeName
        {
            get { return "STLMultiMap<" + TypeParams[0].MemberTypeCLRName + ", " + TypeParams[1].MemberTypeCLRName + ", " + TypeParams[0].MemberTypeNativeName + ", " + TypeParams[1].MemberTypeNativeName + ">"; }
        }

        //public override string GetCLRTypeName(ITypeMember m)
        //{
        //    switch (m.PassedByType)
        //    {
        //        case PassedByType.Reference:
        //            return "Collections::Generic::SortedList<" + TypeMembers[0].CLRTypeName + ", Collections::Generic::List<" + TypeMembers[1].CLRTypeName + ">^>^";
        //        case PassedByType.PointerPointer:
        //        case PassedByType.Value:
        //        case PassedByType.Pointer:
        //        default:
        //            throw new Exception("Unexpected");
        //    }
        //}

        //public override string GetNativeCallConversion(string expr, ITypeMember m)
        //{
        //    switch (m.PassedByType)
        //    {
        //        case PassedByType.Reference:
        //            return "GetSortedListFromMultiMap<" + TypeMembers[0].CLRTypeName + ", " + TypeMembers[1].CLRTypeName + ", " + FullNativeName + ">( " + expr + ")";
        //        case PassedByType.PointerPointer:
        //        case PassedByType.Value:
        //        case PassedByType.Pointer:
        //        default:
        //            throw new Exception("Unexpected");
        //    }
        //}

        public DefStdMultiMap(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
            : base(nsDef, surroundingClass, elem)
        {
        }
    }
}