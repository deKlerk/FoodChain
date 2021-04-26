using Python.Runtime;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FoodChain.Goo;
using FoodChain.Parameters;

namespace FoodChain
{
    public class ParseGraph : GH_Component
    {
        private string outformat = "turtle";
        private Dictionary<string, bool> flags = new Dictionary<string, bool>();

        /// <summary>
        /// Initializes a new instance of the DemoSCOPE02 class.
        /// </summary>
        public ParseGraph()
          : base("Parse Graph", "PGraph",
              "Parses a graph from a given URI.",
              "Food Chain", "Scope")
        {
            flags.Add("n3", false);
            flags.Add("turtle", true);
            flags.Add("nt", false);
            flags.Add("xml", false);
            flags.Add("json-ld", false);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new GHPScope(), "Scope", "Sc", "Python.NET scope", GH_ParamAccess.item);
            pManager.AddTextParameter("Graph Name", "GN", "Name of the RDFLib Graph", GH_ParamAccess.item);
            pManager.AddTextParameter("URI", "U", "Uri to parse", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new GHPScope(), "Scope", "Sc", "Python.NET scope", GH_ParamAccess.item);
            pManager.AddTextParameter("Text", "t", "demo", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            using (Py.GIL())
            {
                //PyScope psIn = Py.CreateScope();
                GHScope ghScope = null;
                String gName = null;
                String uri = null;

                if (!DA.GetData(0, ref ghScope)) { return; }
                if (!DA.GetData(1, ref gName)) { return; }
                if (!DA.GetData(2, ref uri)) { return; }

                PyScope psIn = ghScope.Value.scope;

                psIn.Exec($"{gName} = Graph()");
                psIn.Exec($"{gName}.parse('{uri}')");
                psIn.Exec($"{gName}txt = {gName}.serialize(format='{outformat}').decode('utf-8')");

                dynamic outTxt = psIn.Get($"{gName}txt");

                DA.SetData(0, ghScope);
                DA.SetData(1, outTxt.ToString());
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

            ToolStripMenuItem jsonld = Menu_AppendItem(menu, "JSON-LD", PickFormat, true);
            jsonld.Tag = "json-ld";
            jsonld.Checked = flags[jsonld.Tag.ToString()];
            jsonld.ToolTipText = $"Parse in {jsonld.Text} format";

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