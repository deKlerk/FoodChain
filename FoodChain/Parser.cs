using Python.Runtime;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace FoodChain
{
    public class Parser : GH_Component
    {
        private PyScope ps;
        private string outformat = "turtle";
        private Dictionary<string, bool> flags = new Dictionary<string, bool>();

        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Parser()
          : base("Graph Parser", "Parser",
              "Parses a Graph given an URI",
              "Food Chain", "Inspect")
        {
            Environment.SetEnvironmentVariable("PATH", @"C:\Python37", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", @"C:\Python37", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONPATH", @"C:\Python37\Lib; C:\Python37\Lib\site-packages", EnvironmentVariableTarget.Process);

            PythonEngine.Initialize();

            flags.Add("n3", false);
            flags.Add("turtle", true);
            flags.Add("nt", false);
            flags.Add("xml", false);
            //flags.Add("rdfa", false);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Uri", "U", "URI of RDF data to parse", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Graph", "G", "Graph type", GH_ParamAccess.item);
            pManager.AddTextParameter("Txt", "T", "out", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            using (Py.GIL())
            {
                string uri = null;
                if (!DA.GetData(0, ref uri)) { return; }


                dynamic rdflib = Py.Import("rdflib");    // Imports RDFLib
                dynamic g = rdflib.graph.Graph();        // Creates an empty RDFLib Graph

                string outtext = null;

                if (uri != null)
                {
                    g.parse(uri);

                    outtext = Convert.ToString(g.serialize(Py.kw("format", outformat)).decode("utf-8"));
                }

                DA.SetData(0, g);
                DA.SetData(1, outtext);
            }
        }

        /// <summary>
        /// Appends options for serialization format to dropdown menu
        /// </summary>
        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalMenuItems(menu);

            ToolStripMenuItem n3 = Menu_AppendItem(menu, "N3", PickFormat, true);
            n3.Tag = "n3";
            n3.Checked = flags[n3.Tag.ToString()];


            ToolStripMenuItem ttl = Menu_AppendItem(menu, "Turtle", PickFormat, true);
            ttl.Tag = "turtle";
            ttl.Checked = flags[ttl.Tag.ToString()];

            ToolStripMenuItem nt = Menu_AppendItem(menu, "N-Triple", PickFormat, true);
            nt.Tag = "nt";
            nt.Checked = flags[nt.Tag.ToString()];

            ToolStripMenuItem rdfxml = Menu_AppendItem(menu, "RDF/XML", PickFormat, true);
            rdfxml.Tag = "xml";
            rdfxml.Checked = flags[rdfxml.Tag.ToString()];

            //ToolStripMenuItem rdfa = Menu_AppendItem(menu, "RDFa", PickFormat, true);
            //rdfa.Tag = "rdfa";
            //rdfa.Checked = flags[rdfa.Tag.ToString()];

        }

        private void PickFormat(Object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            outformat = item.Tag.ToString();

            List<string> keys = new List<string>(flags.Keys);
            foreach (string k in keys) { flags[k] = false; }
            flags[outformat] = true;

            this.ExpireSolution(true);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1fb69c8f-7580-46f5-b20b-23fb1e9e9e9d"); }
        }
    }
}
