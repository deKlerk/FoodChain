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
    public class GHScope : GH_Goo<Scope>
    {
        public GHScope() { this.Value = null; }
        public GHScope(GHScope goo) { this.Value = goo.Value; }
        public GHScope(Scope native) { this.Value = native; }

        public override bool IsValid
        {
            get
            {
                Type scType = this.Value.scope.GetType();
                if (scType == typeof(PyScope)){ return true; }
                return false;
            }
        }

        public override string TypeName => "GHScope";

        public override string TypeDescription => "Python.NET Scope";

        public override IGH_Goo Duplicate() => new GHScope(this);

        public override string ToString() => $"{this.Value.scope.GetType()}: {this.Value.Name}";
    }
    
    public class Scope
    {
        public String Name;
        public PyScope scope;
        public Scope(string name)
        {
            using (Py.GIL())
            {
                PythonEngine.Initialize();
                this.Name = name;
                this.scope = Py.CreateScope(name);
            }
        }
        public Scope(string name, PyScope scope)
        {
            this.Name = name;
            this.scope = scope;
        }
        public Scope(PyScope scope)
        {
            this.scope = scope;
            this.Name = scope.Name;
        }
    }
}
