using Python.Runtime;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace FoodChain
{
    public class DemoSCOPE02 : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DemoSCOPE02 class.
        /// </summary>
        public DemoSCOPE02()
          : base("DemoSCOPE02", "Nickname",
              "Description",
              "Food Chain", "Scope")
        {
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", @"C:\Python37\", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PATH", @"C:\Python37", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", @"C:\Python37", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONPATH", @"C:\Python37\Lib; C:\Python37\Lib\site-packages", EnvironmentVariableTarget.Process);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Scope", "s", "scope", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Txt", "t", "demo", GH_ParamAccess.item);
            pManager.AddGenericParameter("Scope", "s", "scope", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            using (Py.GIL())
            {
                PythonEngine.Initialize();
                PyScope psIn = Py.CreateScope();

                if (!DA.GetData(0, ref psIn)) { return; }

                dynamic outTxt = psIn.Get("txt");

                DA.SetData(0, outTxt.ToString());
                DA.SetData(1, psIn);
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
            get { return new Guid("6e9454eb-e4e3-41ce-83c1-4db0e07f8778"); }
        }
    }
}