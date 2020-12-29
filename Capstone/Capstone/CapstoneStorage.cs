using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;

namespace Capstone
{
    public class CapstoneStorage
    {
        private UIDocument UIdoc;

        public UIDocument RevitDoc
        {
            get
            {
                return UIdoc;
            }
        }

        public CapstoneStorage(UIApplication UIapp)
        {
            UIdoc = UIapp.ActiveUIDocument;
        }
    }
}
