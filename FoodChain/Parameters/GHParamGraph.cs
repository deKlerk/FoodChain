using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.GUI;
using FoodChain.Goo;

namespace FoodChain.Parameters
{
    public class GHParamGraph : GH_Param<GHGraph>
    {
        public GHParamGraph() 
            : base("GH Graph", "Graph",
              "Stores RDFLib Graph data in GH",
              "Food Chain", "Create", GH_ParamAccess.item){ }
        public override Guid ComponentGuid { get { return new Guid("857adee6-63b9-4f66-94b4-04489f5c2003"); } }
        public override GH_Exposure Exposure { get; } = GH_Exposure.primary;
        public bool Hidden { get; set; } = false;

        public override void CreateAttributes()
        {
            base.CreateAttributes();
        }
    }
}
