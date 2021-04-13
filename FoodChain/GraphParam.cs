using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using FoodChain.Parameters;

namespace FoodChain
{
    public class GraphParam : GH_Component
    {
        public GraphParam() : base("RDFLib Graph", "Graph", "RDFLib Graph", "Food Chain", "Parameters")
        {

        }

        public override Guid ComponentGuid { get { return new Guid("4849777d-1ff4-4591-a4d8-0f50cce11893"); } }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new GHParamGraph(), "Graph", "G", "RDFLib Graph", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new GHParamGraph(), "Graph", "G", "RDFLib Graph", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
        }
    }
}
