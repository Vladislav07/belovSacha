using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeModel
{
   public abstract class Component
    { 
        public event Action<string, string> IsChild;
        public string StructureNumber { get; private set; }
        public string CubyNumber { get; private set; }
        public string FullPath { get; private set; }

        public Component(string sn, string cn, string fn)
        {
            StructureNumber = sn;
            CubyNumber = cn;
            FullPath = fn;
            IsChild = new Action<string, string>(UpdateState);
        }

        private void UpdateState(string arg1, string arg2)
        {
           
            if (IsChild != null)
            {
                IsChild.Invoke(arg1, arg2);
            }

        }
    }
    public class CompPart : EqualityComparer<Component>
    {
        public override bool Equals(Component x, Component y)
        {
            return (x.CubyNumber == y.CubyNumber);
        }

        public override int GetHashCode(Component obj)
        {
            return obj.CubyNumber.GetHashCode();
        }
    }
}
