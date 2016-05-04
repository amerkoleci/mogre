using AutoWrap.Meta;

namespace AutoWrap.Mogre
{
    public class MogreCodeStyleDef : CodeStyleDefinition
    {
        public override bool AllowIsInPropertyName
        {
            get { return true; }
        }
    }
}