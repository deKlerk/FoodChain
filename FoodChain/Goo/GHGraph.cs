using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace FoodChain.Goo
{
    public class GHGraph : GH_Goo<Graph>
    {

        public override bool IsValid => throw new NotImplementedException();

        public override string TypeName { get; } = "GHGraph";

        public override string TypeDescription { get; } = "GH representation of a RDFLib Graph";

        public override IGH_Goo Duplicate()
        {
            return new GHGraph();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }

    public class Graph
    {
        public List<String> Prefixes { get; set; }
        public List<String> NSpaces { get; set; }

        public Graph(List<String> pref, List<String> nsp)
        {
            this.Prefixes = pref;
            this.NSpaces = nsp;
        }
    }
}
