using Python.Runtime;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using FoodChain.Parameters;
using FoodChain.Goo;

namespace FoodChain
{
    public class GetSubjects : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public GetSubjects()
          : base("Get Subjects", "GSubjects",
              "Returns the subjects from an RDFLib Graph",
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
            pManager.AddTextParameter("Predicate", "Prd", "Predicate to search against", GH_ParamAccess.item);
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
            pManager.AddTextParameter("Subjects", "Sbj", "Subject elements in a RDFLib Graph", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// 
        /// TODO:
        /// . at this stage, predicates and objects have to be input either as URIRefs or directly from their namespaces
        ///   (if the namespaces bound to the graph)
        /// 
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            using (Py.GIL())
            {
                GHScope ghScope = null;
                String gName = null;
                String pred = "None";
                String obj = "None";

                if (!DA.GetData(0, ref ghScope)) { return; }
                if (!DA.GetData(1, ref gName)) { return; }
                
                // Get optional data
                DA.GetData(2, ref pred);
                DA.GetData(3, ref obj); 

                PyScope psIn = ghScope.Value.scope;

                psIn.Exec($"try: {gName}\n" +
                          $"except NameError: {gName} = Graph()");
                psIn.Exec($"{gName}Sbj = set({gName}.subjects({pred}, {obj}))");

                dynamic subjects = psIn.Get($"{gName}Sbj");

                DA.SetData(0, ghScope);
                DA.SetDataList(1, subjects);
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("42d1b3f8-2f96-4181-a085-aee1f0d0d22b"); }
        }
    }
}