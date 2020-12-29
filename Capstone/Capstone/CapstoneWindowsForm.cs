using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace Capstone
{
    public partial class CapstoneWindowsForm : System.Windows.Forms.Form
    {
        private CapstoneStorage m_creator;
        private UIDocument UIdoc;
        private Document doc;
        List<List<ElementId>> all_stage = new List<List<ElementId>>();

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
        List<ElementId> Stage11 = new List<ElementId>();
        List<ElementId> Stage12 = new List<ElementId>();
        List<ElementId> Stage13 = new List<ElementId>();
        List<ElementId> Stage14 = new List<ElementId>();
        List<ElementId> Stage15 = new List<ElementId>();

        ExecuteEventHandler _executeEventHandler = null;
        ExternalEvent _externalEvent = null;



        public CapstoneWindowsForm(CapstoneStorage creator, ExecuteEventHandler executeEventHandler, ExternalEvent externalEvent)
        {
            m_creator = creator;
            UIdoc = m_creator.RevitDoc;
            doc = UIdoc.Document;

            _executeEventHandler = executeEventHandler;
            _externalEvent = externalEvent;

            //加入各個階段內容
            FilteredElementCollector all = new FilteredElementCollector(doc);
            all.WhereElementIsNotElementType();
            ICollection<ElementId> all_element = all.ToElementIds();


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
                catch (NullReferenceException e)
                {
                }
            }

            //List<List<ElementId>> all_stage = new List<List<ElementId>>();
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

            //先把所有東西hide起來
            Transaction transaction = new Transaction(doc);
            transaction.Start("Hide");

            foreach (List<ElementId> stage in all_stage)
            {
                HideStage(stage);
            }

            //ShowStage(Stage1);
            //ShowStage(Stage2);

            transaction.Commit();

            InitializeComponent();
        }

        private void CapstoneWindowsForm_Load(object sender, EventArgs e)
        {

        }

        public void HideStage(List<ElementId> stage)
        {
            var view = UIdoc.ActiveView as View3D;
            ICollection<ElementId> collection = stage;
            if (view != null & stage.Count != 0)
            {
                view.HideElements(collection);
            }
        }

        public void ShowStage(List<ElementId> stage)
        {
            var view = UIdoc.ActiveView as View3D;
            ICollection<ElementId> collection = stage;
            if (view != null & stage.Count != 0)
            {
                view.UnhideElements(collection);
            }
        } 

        public List<List<ElementId>> Slice(List<List<ElementId>> all_stage, int to)
        {
            List<List<ElementId>> new_stages = new List<List<ElementId>>();
            for(int i=0; i<to; i++)
            {
                new_stages.Add(all_stage[i]);
            }

            return new_stages;
        }

        public void ShowUpToStage(int stage_num, List<List<ElementId>> remove_stages)
        {
            if (_externalEvent != null)
            {
                _executeEventHandler.ExecuteAction = new Action<UIApplication>((app) =>
                {
                    if (app.ActiveUIDocument == null || app.ActiveUIDocument.Document == null)
                        return;

                    using (Transaction transaction = new Transaction(doc, "Creat Line1"))
                    {
                        transaction.Start();

                        //先把全部hide起來
                        foreach (List<ElementId> stage in all_stage)
                        {
                            HideStage(stage);
                        }

                        //再把stage1~stage n的顯示出來
                        foreach (List<ElementId> stage in Slice(all_stage, stage_num))
                        {
                            ShowStage(stage);
                        }

                        //如果有的話，remove 支撐
                        if(remove_stages != null)
                        {
                            foreach (List<ElementId> stage in remove_stages)
                            {
                                HideStage(stage);
                            }
                        }

                        transaction.Commit();
                    }
                });
                _externalEvent.Raise();
            }
        }

        private void Stage1_Click(object sender, EventArgs e)
        {
            ShowUpToStage(1, null);
        }

        private void Stage2_Click(object sender, EventArgs e)
        {
            ShowUpToStage(2, null);
        }

        private void Stage3_Click(object sender, EventArgs e)
        {
            ShowUpToStage(3, null);
        }

        private void Stage4_Click(object sender, EventArgs e)
        {
            ShowUpToStage(4, null);
        }

        private void Stage5_Click(object sender, EventArgs e)
        {
            ShowUpToStage(5, null);
        }

        private void Stage6_Click(object sender, EventArgs e)
        {
            ShowUpToStage(6, null);
        }

        private void Stage7_Click(object sender, EventArgs e)
        {
            ShowUpToStage(7, null);
        }

        private void Stage8_Click(object sender, EventArgs e)
        {
            ShowUpToStage(8, null);
        }

        private void Stage9_Click(object sender, EventArgs e)
        {
            List<List<ElementId>> remove_stages = new List<List<ElementId>>();
            remove_stages.Add(Stage7);
            ShowUpToStage(9, remove_stages);
        }

        private void Stage10_Click(object sender, EventArgs e)
        {
            List<List<ElementId>> remove_stages = new List<List<ElementId>>();
            remove_stages.Add(Stage7);
            ShowUpToStage(10, remove_stages);
        }

        private void Stage11_Click(object sender, EventArgs e)
        {
            List<List<ElementId>> remove_stages = new List<List<ElementId>>();
            remove_stages.Add(Stage7);
            ShowUpToStage(11, remove_stages);
        }

        private void Stage12_Click(object sender, EventArgs e)
        {
            List<List<ElementId>> remove_stages = new List<List<ElementId>>();
            remove_stages.Add(Stage7);
            remove_stages.Add(Stage10);
            ShowUpToStage(12, remove_stages);
        }

        private void Stage13_Click(object sender, EventArgs e)
        {
            List<List<ElementId>> remove_stages = new List<List<ElementId>>();
            remove_stages.Add(Stage7);
            remove_stages.Add(Stage10);
            ShowUpToStage(13, remove_stages);
        }

        private void Stage14_Click(object sender, EventArgs e)
        {
            List<List<ElementId>> remove_stages = new List<List<ElementId>>();
            remove_stages.Add(Stage7);
            remove_stages.Add(Stage10);
            ShowUpToStage(14, remove_stages);
        }

        private void Stage15_Click(object sender, EventArgs e)
        {
            List<List<ElementId>> remove_stages = new List<List<ElementId>>();
            remove_stages.Add(Stage7);
            remove_stages.Add(Stage10);
            remove_stages.Add(Stage13);
            ShowUpToStage(15, remove_stages);
        }

        private void ShowAll_Click(object sender, EventArgs e)
        {
            ShowUpToStage(15, null);
        }
    }


}
