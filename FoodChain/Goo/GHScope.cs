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

        public override string TypeName => "Scope";
        public override string TypeDescription => "Python.NET Scope";
        public override IGH_Goo Duplicate() => new GHScope(this);
        public override string ToString() => $"{this.Value.scope.GetType()}: {this.Value.Name}";
        public override bool CastFrom(object source)
        {
            if (source == null) return false;
            if (source is string str)
            {
                string name;
                if (str.Contains("Python.Runtime.PyScope: ")) 
                {
                    string[] inStr = str.Split(' ');
                    name = inStr[1];
                }
                else { name = str; }

                PyScope scope;
                var state = PyScopeManager.Global.TryGet(name, out scope);

                if (!state) { this.Value = new Scope(name); }
                this.Value.Name = name;
                this.Value.scope = scope;
                return true;
            }
            if (source is GH_String ghStr)
            {
                string name;
                string val = ghStr.Value;

                if (val.Contains("Python.Runtime.PyScope: "))
                {
                    string[] inStr = val.Split(' ');
                    name = inStr[1];
                }
                else { name = val; }

                PyScope scope;
                var state = PyScopeManager.Global.TryGet(name, out scope);

                if (!state) 
                { 
                    this.Value = new Scope(name);
                    return true;
                }

                this.Value = new Scope(scope);
                return true;
            }
            return false;
        }
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
