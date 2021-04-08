using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace FoodChain
{
    public class FoodChainInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "FoodChain";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "Food Chain offers a set of components to work with RDF data inside of Grasshopper. It uses Python.NET ( https://pythonnet.github.io/ ) to access RDFLib, a Python module to create, load, search, edit data in RDF Graphs. ( https://rdflib.readthedocs.io/ )";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("0568a106-1125-4a94-88f2-a3d86f76acaf");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Rui de Klerk";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "rui.klerk@campus.ul.pt";
            }
        }
    }
}
