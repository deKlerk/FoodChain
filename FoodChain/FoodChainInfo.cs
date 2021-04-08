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
                return "";
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
                return "HP Inc.";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
