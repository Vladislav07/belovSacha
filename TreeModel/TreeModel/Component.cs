using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPDM.Interop.epdm;

namespace TreeModel
{
   public class Component
    { 
   
        public string StructureNumber { get; private set; }
        public string CubyNumber { get; private set; }
        public string FullPath { get; private set; }
        IEdmFile5 _File = null;
        public int CurVersion { get; set; }
        public IEdmState5 State { get; set; }
        public int  bFolder { get; set; }

        public Dictionary<string, int> listRefChild;

        public bool IsRebuild { get;private set; }

        public Component(string sn, string cn, string fn)
        {
            StructureNumber = sn;
            CubyNumber = cn;
            FullPath = fn;
            listRefChild = new Dictionary<string, int>();
            IsRebuild = false;
        }
        public IEdmFile5 File
        {
            get { return _File; }
            set
            {
                _File = value;
                CurVersion = _File.CurrentVersion;
                State = _File.CurrentState;
                
            }
        }

        public void isNeedsRebuld()
        {
            if (listRefChild == null) return;
            foreach (KeyValuePair<string,int> item in listRefChild)
            {
               bool isRebuld= Tree.Part_IsChild(item.Key, item.Value);
               if (isRebuld)
                {
                    IsRebuild = true;
                }
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
