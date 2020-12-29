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
    public class Class1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;


            FilteredElementCollector all = new FilteredElementCollector(doc);
            all.WhereElementIsNotElementType();
            ICollection<ElementId> all_element = all.ToElementIds();
            

            List<ElementId> Stage1 = new List<ElementId>();
            List<ElementId> Stage2 = new List<ElementId>();
            List<ElementId> Stage3 = new List<ElementId>();
            List<ElementId> Stage4 = new List<ElementId>();
            List<ElementId> Stage5 = new List<ElementId>();
            List<ElementId> Stage6 = new List<ElementId>();
            List<ElementId> Stage7 = new List<ElementId>();
            List<ElementId> Stage8 = new List<ElementId>();
            List<ElementId> Stage9 = new List<ElementId>();
            List<ElementId> Stage10 = new List<ElementId>();
            List<ElementId> Stage11= new List<ElementId>();
            List<ElementId> Stage12 = new List<ElementId>();
            List<ElementId> Stage13 = new List<ElementId>();
            List<ElementId> Stage14 = new List<ElementId>();
            List<ElementId> Stage15 = new List<ElementId>();

            foreach (ElementId elemId in all_element)
            {
                Element elem = doc.GetElement(elemId);
                try
                {
                    String comment = elem.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).AsString();
                    switch (comment)
                    {
                        case "Stage1":
                            Stage1.Add(elemId);
                            break;
                        case "Stage2":
                            Stage2.Add(elemId);
                            break;
                        case "Stage3":
                            Stage3.Add(elemId);
                            break;
                        case "Stage4":
                            Stage4.Add(elemId);
                            break;
                        case "Stage5":
                            Stage5.Add(elemId);
                            break;
                        case "Stage6":
                            Stage6.Add(elemId);
                            break;
                        case "Stage7":
                            Stage7.Add(elemId);
                            break;
                        case "Stage8":
                            Stage8.Add(elemId);
                            break;
                        case "Stage10":
                            Stage10.Add(elemId);
                            break;
                        case "Stage11":
                            Stage11.Add(elemId);
                            break;
                        case "Stage13":
                            Stage13.Add(elemId);
                            break;
                        case "Stage14":
                            Stage14.Add(elemId);
                            break;
                    }
                }
                catch(NullReferenceException e)
                {
                }
               
            }

            List<List<ElementId>> all_stage = new List<List<ElementId>>();
            all_stage.Add(Stage1);
            all_stage.Add(Stage2);
            all_stage.Add(Stage3);
            all_stage.Add(Stage4);
            all_stage.Add(Stage5);
            all_stage.Add(Stage6);
            all_stage.Add(Stage7);
            all_stage.Add(Stage8);
            all_stage.Add(Stage9);
            all_stage.Add(Stage10);
            all_stage.Add(Stage11);
            all_stage.Add(Stage12);
            all_stage.Add(Stage13);
            all_stage.Add(Stage14);
            all_stage.Add(Stage15);


            //開始移動東西
            Transaction transaction = new Transaction(doc);
            transaction.Start("Hide");

            foreach (List<ElementId> stage in all_stage)
            {
                HideStage(commandData, stage);
            }

            ShowStage(commandData, Stage1);
            ShowStage(commandData, Stage2);

            transaction.Commit();
            return Result.Succeeded;
        }

        public void HideStage(ExternalCommandData commandData, List<ElementId> stage)
        {
            var view = commandData.Application.ActiveUIDocument.ActiveView as View3D;
            ICollection<ElementId> collection = stage;
            if (view != null & stage.Count != 0)
            {
                view.HideElements(collection);
            }
        }

        public void ShowStage(ExternalCommandData commandData, List<ElementId> stage)
        {
            var view = commandData.Application.ActiveUIDocument.ActiveView as View3D;
            ICollection<ElementId> collection = stage;
            if(view != null)
            {
                view.UnhideElements(collection);
            }
        }

    }
}
