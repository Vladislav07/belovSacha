using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPDM.Interop;
using EPDM.Interop.epdm;

namespace TreeModel
{
   public class Part
    {

      public event Action<string, string, int> IsEventRebuild;

      public string StructureNumber { get; private set; }
      public string CubyNumber { get; private set; }
      public string FullPath { get; private set; }
      IEdmFile5 _File = null;
      public int CurVersion { get; set; }
      public IEdmState5 State { get; set; }
      public IEdmFile5 File
        {
            get { return _File; }
            set { _File = value;
                CurVersion = _File.CurrentVersion;
                State = _File.CurrentState;
                
                }
        }
      public Part(string sn, string cn, string fn)
        {
            StructureNumber = sn;
            CubyNumber = cn;
            FullPath = fn;
            IsEventRebuild = new Action<string, string, int>(UpdateState);
        }

        private void UpdateState(string arg1, string arg2, int arg3)
        {
            IsEventRebuild?.Invoke(StructureNumber, CubyNumber, CurVersion);
            
        }
    }

    public class CompPart : EqualityComparer<Part>
    {
        public override bool Equals(Part x, Part y)
        {
            return (x.CubyNumber == y.CubyNumber);
        }

        public override int GetHashCode(Part obj)
        {
            return obj.CubyNumber.GetHashCode();
        }
    }
}
