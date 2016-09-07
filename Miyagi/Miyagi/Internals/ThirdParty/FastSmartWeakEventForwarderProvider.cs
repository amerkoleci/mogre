/*
 Copyright (c) 2008 Daniel Grunwald
 Permission is hereby granted, free of charge, to any person
 obtaining a copy of this software and associated documentation
 files (the "Software"), to deal in the Software without
 restriction, including without limitation the rights to use,
 copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the
 Software is furnished to do so, subject to the following
 conditions:
 The above copyright notice and this permission notice shall be
 included in all copies or substantial portions of the Software.
 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 OTHER DEALINGS IN THE SOFTWARE.
*/
namespace Miyagi.Internals.ThirdParty
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.CompilerServices;

    // The forwarder-generating code is in a separate class because it does not depend on type T.
    internal static class FastSmartWeakEventForwarderProvider
    {
        #region Fields

        private static readonly Type[] ForwarderParameters = { typeof(WeakReference), typeof(object), typeof(EventArgs) };
        private static readonly Dictionary<MethodInfo, ForwarderDelegate> Forwarders = new Dictionary<MethodInfo, ForwarderDelegate>();
        private static readonly MethodInfo GetTarget = typeof(WeakReference).GetMethod("get_Target");

        #endregion Fields

        #region Delegates

        internal delegate bool ForwarderDelegate(WeakReference wr, object sender, EventArgs e);

        #endregion Delegates

        #region Methods

        #region Internal Static Methods

        internal static ForwarderDelegate GetForwarder(MethodInfo method)
        {
            lock (Forwarders)
            {
                ForwarderDelegate d;
                if (Forwarders.TryGetValue(method, out d))
                {
                    return d;
                }
            }

            if (method.DeclaringType.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Length != 0)
            {
                throw new ArgumentException("Cannot create weak event to anonymous method with closure.");
            }

            var parameters = method.GetParameters();

            Debug.Assert(GetTarget != null, string.Empty);

            DynamicMethod dm = new DynamicMethod(
                "FastSmartWeakEvent", typeof(bool), ForwarderParameters, method.DeclaringType);

            ILGenerator il = dm.GetILGenerator();

            if (!method.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Callvirt, GetTarget);
                il.Emit(OpCodes.Dup);
                Label label = il.DefineLabel();
                il.Emit(OpCodes.Brtrue, label);
                il.Emit(OpCodes.Pop);
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Ret);
                il.MarkLabel(label);

                // The castclass here is required for the generated code to be verifiable.
                // We can leave it out because we know this cast will always succeed
                // (the instance/method pair was taken from a delegate).
                // Unverifiable code is fine because private reflection is only allowed under FullTrust
                // anyways.
                // il.Emit(OpCodes.Castclass, method.DeclaringType);
            }

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldarg_2);

            // This castclass here is required to prevent creating a hole in the .NET type system.
            // See Program.TypeSafetyProblem in the 'SmartWeakEventBenchmark' to see the effect when
            // this cast is not used.
            // You can remove this cast if you trust add FastSmartWeakEvent.Raise callers to do
            // the right thing, but the small performance increase (about 5%) usually isn't worth the risk.
            il.Emit(OpCodes.Castclass, parameters[1].ParameterType);

            il.Emit(OpCodes.Call, method);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ret);

            var fd = (ForwarderDelegate)dm.CreateDelegate(typeof(ForwarderDelegate));
            lock (Forwarders)
            {
                Forwarders[method] = fd;
            }

            return fd;
        }

        #endregion Internal Static Methods

        #endregion Methods
    }
}