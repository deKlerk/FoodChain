using Python.Runtime;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace FoodChain
{
    public class GetObjects : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public GetObjects()
          : base("Get Objects", "GObjects",
              "Returns the objects from an RDFLib Graph",
              "Food Chain", "Query")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Scope", "s", "scope", GH_ParamAccess.item);
            pManager.AddTextParameter("Graph Name", "GN", "Name of the RDFLib Graph", GH_ParamAccess.item);
            pManager.AddTextParameter("Subject", "Sbj", "Subject to search against", GH_ParamAccess.item);
            pManager.AddTextParameter("Predicate", "Prd", "Predicate to search against", GH_ParamAccess.item);

            pManager[2].Optional = true;
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Scope", "s", "Python.NET scope", GH_ParamAccess.item);
            pManager.AddTextParameter("Objects", "Obj", "Object elements in a RDFLib Graph", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            using (Py.GIL())
            {
                PyScope psIn = Py.CreateScope();
                String gName = null;
                String subj = "None";
                String pred = "None";

                if (!DA.GetData(0, ref psIn)) { return; }
                if (!DA.GetData(1, ref gName)) { return; }
                
                if (!DA.GetData(2, ref subj)) { }
                else { DA.GetData(2, ref subj); }

                if (!DA.GetData(3, ref pred)) { }
                else { DA.GetData(3, ref pred); }

                psIn.Exec($"try: {gName}\n" +
                          $"except NameError: {gName} = Graph()");
                psIn.Exec($"{gName}Obj = set({gName}.objects({subj}, {pred}))");

                dynamic objects = psIn.Get($"{gName}Obj");

                DA.SetData(0, psIn);
                DA.SetDataList(1, objects);
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
            get { return new Guid("280c3316-acee-45d6-a169-a18b5cb78346"); }
        }
    }
}