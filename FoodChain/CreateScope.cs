using Python.Runtime;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using FoodChain.Settings;

namespace FoodChain
{
    public class CreateScope : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DemoSCOPE01 class.
        /// </summary>
        public CreateScope()
          : base("Create Scope", "CScope",
              "Creates a new scope for RDFLib to run in and imports its main modules.",
              "Food Chain", "Scope")
        {
            Config cf = new Config();
            cf.Setup();
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Name of the scope.", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Scope", "s", "Python.NET scope", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            using (Py.GIL())
            {
                String sName = null;
                if(!DA.GetData(0, ref sName)) { sName = "Default"; }

                PythonEngine.Initialize();
                try
                {
                    PyScope ps = Py.CreateScope(sName);

                    ps.Exec("from rdflib.graph import Graph, Literal, RDF, URIRef, BNode, plugin");
                    ps.Exec("from rdflib.serializer import Serializer");
                    ps.Exec("from rdflib import Namespace");

                    ps.Exec("from rdflib.namespace import NamespaceManager, RDF, RDFS, OWL, XSD, FOAF, SKOS, DOAP, DC, DCTERMS, VOID");

                    ps.Exec("import SPARQLWrapper");
                    ps.Exec("import json");

                    DA.SetData(0, ps);
                }
                catch(Exception e) { this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, e.Message); }   
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
            get { return new Guid("92b737d3-9ccb-4735-8f0a-05c65cb1f256"); }
        }
    }
}