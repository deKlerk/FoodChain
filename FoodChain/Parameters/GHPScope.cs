using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using FoodChain.Goo;

namespace FoodChain.Parameters
{
    public class GHPScope : GH_Param<GHScope>
    {
        public GHPScope() : base("Scope", "Sc",
              "Python.NET Scope",
              "Food Chain", "Parameters", GH_ParamAccess.item) { }

        public override Guid ComponentGuid => new Guid("7e7cedc3-5e6d-4c46-9a39-772f0fb87488");
        public override GH_Exposure Exposure => GH_Exposure.primary;
        public override void CreateAttributes() => base.CreateAttributes();
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Scope;
    }
}
