using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Windows.Forms;

namespace Capstone
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    class CapstoneConnect : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication UIapp = commandData.Application;
            UIDocument UIdoc = UIapp.ActiveUIDocument;
            Document doc = UIdoc.Document;

            if (doc == null)
            {
                message = "Active document is null";
                return Result.Failed;
            }

            try
            {
                ExecuteEventHandler executeEventHandler = new ExecuteEventHandler("Creat Model Line");
                ExternalEvent externalEvent = ExternalEvent.Create(executeEventHandler);

                CapstoneStorage creator = new CapstoneStorage(UIapp);
                CapstoneWindowsForm windowsForm = new CapstoneWindowsForm(creator, executeEventHandler, externalEvent);

                //窗口一直显示在主程序之前
                //System.Windows.Interop.WindowInteropHelper mainUI = new System.Windows.Interop.WindowInteropHelper(windowsForm);
                //mainUI.Owner = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
                windowsForm.Show();


                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }

        
    }

    public class ExecuteEventHandler : IExternalEventHandler
    {
        public string Name { get; private set; }

        public Action<UIApplication> ExecuteAction { get; set; }

        public ExecuteEventHandler(string name)
        {
            Name = name;
        }

        public void Execute(UIApplication app)
        {
            if (ExecuteAction != null)
            {
                try
                {
                    ExecuteAction(app);
                }
                catch
                { }
            }
        }

        public string GetName()
        {
            return Name;
        }
    }
}
