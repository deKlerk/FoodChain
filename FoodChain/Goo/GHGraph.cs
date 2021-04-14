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
        public override IGH_Goo Duplicate() { return new GHGraph(); }
        public override string ToString() { throw new NotImplementedException(); }
    }

    public class Graph
    {
        public Dictionary<String, String> Namespaces { get; set; }
        public List<String> Triples { get; set; }
        public Graph() 
        {
            this.Namespaces = null;
            this.Triples = null;
        }
        public Graph(List<String> pref, List<String> nsp, List<String> trp)
        {
            int pCount = pref.Count;
            int nCount = nsp.Count;

            if (pCount == nCount)
            {
                for (int i = 0; i < pCount; i++)
                {
                    this.Namespaces.Add(pref[i], nsp[i]);
                }
            }
            else { throw new Exception($"There must be the same amount of prefixes and namespaces... there are currently {pCount} prefixes and {nCount} namespaces."); }

            this.Triples = trp;
        }
        public Graph(Dictionary<String, String> nsp, List<String> trp)
        {
            this.Namespaces = nsp;
            this.Triples = trp;
        }
    }
}
