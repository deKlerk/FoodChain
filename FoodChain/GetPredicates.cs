using Python.Runtime;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using FoodChain.Parameters;
using FoodChain.Goo;

namespace FoodChain
{
    public class GetPredicates : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public GetPredicates()
          : base("Get Predicates", "GPredicates",
              "Returns the predicates from an RDFLib Graph",
              "Food Chain", "Query")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new GHPScope(), "Scope", "Sc", "Python.NET scope", GH_ParamAccess.item);
            pManager.AddTextParameter("Graph Name", "GN", "Name of the RDFLib Graph", GH_ParamAccess.item);
            pManager.AddTextParameter("Subject", "Sbj", "Subject to search against", GH_ParamAccess.item);
            pManager.AddTextParameter("Object", "Obj", "Object to search against", GH_ParamAccess.item);

            pManager[2].Optional = true;
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new GHPScope(), "Scope", "Sc", "Python.NET scope", GH_ParamAccess.item);
            pManager.AddTextParameter("Predicates", "Pred", "Predicate elements in a RDFLib Graph", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            using (Py.GIL())
            {
                GHScope ghScope = null;
                String gName = null;
                String subj = "None";
                String obj = "None";

                if (!DA.GetData(0, ref ghScope)) { return; }
                if (!DA.GetData(1, ref gName)) { return; }
                
                if (!DA.GetData(2, ref subj)) { }
                else { DA.GetData(2, ref subj); }

                if (!DA.GetData(3, ref obj)) { }
                else { DA.GetData(3, ref obj); }

                PyScope psIn = ghScope.Value.scope;

                psIn.Exec($"try: {gName}\n" +
                          $"except NameError: {gName} = Graph()");
                psIn.Exec($"{gName}Prd = set({gName}.predicates({subj}, {obj}))");

                dynamic predicates = psIn.Get($"{gName}Prd");

                DA.SetData(0, ghScope);
                DA.SetDataList(1, predicates);
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.QueryPrd;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f63f2c5d-58f5-4ca2-8ab5-942de6143b7a"); }
        }
    }
}