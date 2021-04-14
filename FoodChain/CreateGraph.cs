using Python.Runtime;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using FoodChain.Goo;
using FoodChain.Parameters;

namespace FoodChain
{
    public class CreateGraph : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateGraph class.
        /// </summary>
        public CreateGraph()
          : base("Create Graph", "Graph",
              "Creates an RDFLib Graph",
              "Food Chain", "Create")
        {
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", @"C:\Python37\", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PATH", @"C:\Python37\", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", @"C:\Python37\", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONPATH", @"C:\Python37\Lib; C:\Python37\Lib\site-packages", EnvironmentVariableTarget.Process);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Prefix", "Pr", "Prefixes for the namespaces", GH_ParamAccess.list);
            pManager.AddTextParameter("Namespace", "NS", "URI of the namespaces", GH_ParamAccess.list);
            pManager.AddTextParameter("Triples", "T", "RDFLib Triples to add", GH_ParamAccess.list);
            
            // set prefix and namespace inputs as optional
            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Graph", "G", "RDFLib Graph (in n3 format)", GH_ParamAccess.item);
            pManager.AddParameter(new GHParamGraph(), "GH Graph", "Graph", "RDFLib Graph in GH format", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            using (Py.GIL())
            {
                // Initialize Python Engine and create Scope
                PythonEngine.Initialize();
                PyScope ps = Py.CreateScope();

                dynamic rdflib = Py.Import("rdflib");   // Imports RDFLib
                dynamic g = rdflib.graph.Graph();       // Creates an empty RDFLib Graph

                // Register component inputs
                var prefixes = new List<string>();
                var nspaces = new List<string>();
                var triples = new List<String>();

                DA.GetDataList(0, prefixes);
                DA.GetDataList(1, nspaces);
                DA.GetDataList(2, triples);

                // Check if the amount of prefixes is the same as the amount of namespaces
                int np = prefixes.Count;
                int nn = nspaces.Count;

                if (np > 0)
                {
                    if (np == nn)
                    {
                        for(int i=0; i<np; i++)
                        {
                            g.bind(prefixes[i], nspaces[i]);
                        }
                    }
                    else
                    {
                        this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, $"Each namespace must have a unique prefix assigned to it. you have {np} prefixes and {nn} namespace URIs...");
                    }
                }

                List<Uri> uris = new List<Uri>();
                foreach (String ns in nspaces)
                {
                    try
                    {
                        uris.Add(new Uri(ns));
                    }
                    catch(Exception e)
                    {
                        this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, e.Message);
                    }
                }

                Graph outGHGraph = new Graph(prefixes, uris, triples);

                string outGraph = Convert.ToString(g.serialize(Py.kw("format", "n3")).decode("utf-8"));
                DA.SetData(0, outGraph);
                DA.SetData(1, outGHGraph);
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
            get { return new Guid("2234ae04-0e1e-4522-b208-191ce85a3a11"); }
        }
    }
}