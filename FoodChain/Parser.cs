using Python.Runtime;
using Grasshopper.Kernel;
using System;
using System.Reflection;
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
        //private PyScope ps;
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
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", @"C:\Python37\", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PATH", @"C:\Python37", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", @"C:\Python37", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONPATH", @"C:\Python37\Lib; C:\Python37\Lib\site-packages", EnvironmentVariableTarget.Process);

            flags.Add("n3", false);
            flags.Add("turtle", true);
            flags.Add("nt", false);
            flags.Add("xml", false);
            //flags.Add("json-ld", false);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("URI", "U", "URI of RDF data to parse", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Graph", "G", "RDFLib Graph", GH_ParamAccess.item);   
            pManager.AddTextParameter("Text", "T", "Text rendering of the parsed RDFLib Graph", GH_ParamAccess.list);
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
                // Initialize Python Engine and create Scope
                PythonEngine.Initialize(); 
                PyScope ps = Py.CreateScope("GraphScope");

                string uri = null;
                if (!DA.GetData(0, ref uri)) { return; }


                dynamic rdflib = Py.Import("rdflib");   // Imports RDFLib
                //dynamic SPARQLWrapper = Py.Import("SPARQLWrapper");  // Imports SPARQLWrapper
                //dynamic json = Py.Import("json");                    // Imports Json
                dynamic g = rdflib.graph.Graph();       // Creates an empty RDFLib Graph

                string outtext = null;

                if (uri != null)
                {
                    g.parse(uri);

                    outtext = Convert.ToString(g.serialize(Py.kw("format", outformat)).decode("utf-8"));
                }


                Type gType = g.GetType();
                FieldInfo[] fields = gType.GetFields();
                String content = "Type: " + gType.ToString() + "\n" + "number of fields: " + fields.Length.ToString() + "\n";


                foreach(var field in fields)
                {
                    content += field.Name + "\n";
                }

                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, $"fields: {content}");

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

            ToolStripMenuItem n3 = Menu_AppendItem(menu, "Notation 3", PickFormat, true);
            n3.Tag = "n3";
            n3.Checked = flags[n3.Tag.ToString()];
            n3.ToolTipText = $"Parse in {n3.Text} format";


            ToolStripMenuItem ttl = Menu_AppendItem(menu, "Turtle", PickFormat, true);
            ttl.Tag = "turtle";
            ttl.Checked = flags[ttl.Tag.ToString()];
            ttl.ToolTipText = $"Parse in {ttl.Text} format";

            ToolStripMenuItem nt = Menu_AppendItem(menu, "N-Triple", PickFormat, true);
            nt.Tag = "nt";
            nt.Checked = flags[nt.Tag.ToString()];
            nt.ToolTipText = $"Parse in {nt.Text} format";

            ToolStripMenuItem rdfxml = Menu_AppendItem(menu, "RDF/XML", PickFormat, true);
            rdfxml.Tag = "xml";
            rdfxml.Checked = flags[rdfxml.Tag.ToString()];
            rdfxml.ToolTipText = $"Parse in {rdfxml.Text} format";

            //ToolStripMenuItem jsonld = Menu_AppendItem(menu, "JSON-LD", PickFormat, true);
            //jsonld.Tag = "json-ld";
            //jsonld.Checked = flags[jsonld.Tag.ToString()];
            //jsonld.ToolTipText = $"Parse in {jsonld.Text} format";

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
