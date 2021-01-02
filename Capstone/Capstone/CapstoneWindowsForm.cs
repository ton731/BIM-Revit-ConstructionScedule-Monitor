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

        List<ElementId> ColorColumn = new List<ElementId>();

        Material mat1;
        Material mat2;
        Material mat3;
        Material mat4;
        Material mat5;

        //存取每一個column的familysymbol
        List<Element> ColumnTypes = new List<Element>();

        //把Torsa的資料讀近來變成list
        List<List<String>> TorsaData = new List<List<string>>();


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
                        case "ColorColumn":
                            ColorColumn.Add(elemId);
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

            transaction.Commit();


            FilteredElementCollector mat_collector = new FilteredElementCollector(doc);
            var materials = mat_collector.WherePasses(new ElementClassFilter(typeof(Material))).Cast<Material>();
            foreach (Material m in materials)
            {
                switch (m.Name)
                {
                    case "Color1":
                        mat1 = m;
                        break;
                    case "Color2":
                        mat2 = m;
                        break;
                    case "Color3":
                        mat3 = m;
                        break;
                    case "Color4":
                        mat4 = m;
                        break;
                    case "Color5":
                        mat5 = m;
                        break;
                }

            }

            //找到60個column type
            for(int i=1; i<=60; i++)
            {
                string columnTypeName = "Column" + i.ToString();
                Element columnType = FindFamilyType(doc, typeof(FamilySymbol), "混凝土柱-矩形", columnTypeName, null);
                ColumnTypes.Add(columnType);
            }

            String[] lines = System.IO.File.ReadAllLines(@"C:\Users\b06501015\Desktop\G07 BIM\Displacement_1_60m.txt");
            foreach(string line in lines)
            {
                TorsaData.Add(line.Split(',').ToList());
            }



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

                    using (Transaction transaction = new Transaction(doc, "ShowUpToStage"))
                    {
                        transaction.Start();

                        Boolean is_closing = false;
                        if(stage_num > 15)
                        {
                            is_closing = true;
                            stage_num = 15;
                        }

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

                        if (is_closing == true)
                            ChangeColor(1);
                        else
                            ChangeColor(stage_num);

                        transaction.Commit();
                    }
                });
                _externalEvent.Raise();
            }
        }

        public void ChangeColor(int stage_num)
        {
            List<String> displacement_list = TorsaData[stage_num - 1];

            for(int i = 0; i<60; i++)
            {
                //MessageBox.Show(displacement_list[i]);
                double displacement = Convert.ToDouble(displacement_list[i]);
                Element column = doc.GetElement(ColorColumn[i]);

                if (displacement>=0 & displacement < 1)
                {
                    //MessageBox.Show("1");
                    ColumnTypes[i].get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM).Set(mat1.Id);
                    column.ChangeTypeId(ColumnTypes[i].Id);
                }
                else if (displacement >= 1 & displacement < 2)
                {
                    //MessageBox.Show("2");
                    ColumnTypes[i].get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM).Set(mat2.Id);
                    column.ChangeTypeId(ColumnTypes[i].Id);
                }
                else if (displacement >= 2 & displacement < 3)
                {
                    //MessageBox.Show("3");
                    ColumnTypes[i].get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM).Set(mat3.Id);
                    column.ChangeTypeId(ColumnTypes[i].Id);
                }
                else if (displacement >= 3 & displacement < 4)
                {
                    //MessageBox.Show("4");
                    ColumnTypes[i].get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM).Set(mat4.Id);
                    column.ChangeTypeId(ColumnTypes[i].Id);
                }
                else if (displacement >= 4)
                {
                    //MessageBox.Show("5");
                    ColumnTypes[i].get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM).Set(mat5.Id);
                    column.ChangeTypeId(ColumnTypes[i].Id);
                }
            }
        }

        public void Set_GroundDefleciton_WallBuckle(double val1, double val2)
        {
            GroundDeflection.Text = val1.ToString() + " cm";
            WallBuckle.Text = val2.ToString() + " cm";
        }


        private void Stage1_Click(object sender, EventArgs e)
        {
            ShowUpToStage(1, null);
            StageContentLabel.Text = "構築連續壁";
            Set_GroundDefleciton_WallBuckle(0, 0);
        }

        private void Stage2_Click(object sender, EventArgs e)
        {
            ShowUpToStage(2, null);
            StageContentLabel.Text = "構築中間柱";
            Set_GroundDefleciton_WallBuckle(0, 0);
        }

        private void Stage3_Click(object sender, EventArgs e)
        {
            ShowUpToStage(3, null);
            StageContentLabel.Text = "開挖至GL.-1.6m，構築第1層樓版(GL.0m)";
            Set_GroundDefleciton_WallBuckle(0.21, 0.41);
        }

        private void Stage4_Click(object sender, EventArgs e)
        {
            ShowUpToStage(4, null);
            StageContentLabel.Text = "開挖至GL.-6m，構築第2層樓版(GL.-5.05m)";
            Set_GroundDefleciton_WallBuckle(0.27, 0.53);
        }

        private void Stage5_Click(object sender, EventArgs e)
        {
            ShowUpToStage(5, null);
            StageContentLabel.Text = "開挖至GL.-11m，構築第3層樓版(GL.-9.55m)";
            Set_GroundDefleciton_WallBuckle(0.62, 1.12);
        }

        private void Stage6_Click(object sender, EventArgs e)
        {
            ShowUpToStage(6, null);
            StageContentLabel.Text = "開挖至GL.-17m，構築第4層樓版(GL.-15.55m)";
            Set_GroundDefleciton_WallBuckle(1.51, 2.90);
        }

        private void Stage7_Click(object sender, EventArgs e)
        {
            ShowUpToStage(7, null);
            StageContentLabel.Text = "開挖至GL.-22m，架設第1層支撐H428x2@5m, at GL.-21m, 預壓 - 160t";
            Set_GroundDefleciton_WallBuckle(1.99, 3.82);
        }

        private void Stage8_Click(object sender, EventArgs e)
        {
            ShowUpToStage(8, null);
            StageContentLabel.Text = "開挖至GL.-25m，構築第5層樓版(GL.-23.5m)";
            Set_GroundDefleciton_WallBuckle(2.09, 4.02);
        }

        private void Stage9_Click(object sender, EventArgs e)
        {
            List<List<ElementId>> remove_stages = new List<List<ElementId>>();
            remove_stages.Add(Stage7);
            ShowUpToStage(9, remove_stages);
            StageContentLabel.Text = "拆除第1層支撐(GL.-21m)";
            Set_GroundDefleciton_WallBuckle(2.37, 4.55);
        }

        private void Stage10_Click(object sender, EventArgs e)
        {
            List<List<ElementId>> remove_stages = new List<List<ElementId>>();
            remove_stages.Add(Stage7);
            ShowUpToStage(10, remove_stages);
            StageContentLabel.Text = "開挖至GL.-29m，架設第2層支撐H428x2@5m, at GL.-28m, 預壓-160t";
            Set_GroundDefleciton_WallBuckle(2.27, 4.37);
        }

        private void Stage11_Click(object sender, EventArgs e)
        {
            List<List<ElementId>> remove_stages = new List<List<ElementId>>();
            remove_stages.Add(Stage7);
            ShowUpToStage(11, remove_stages);
            StageContentLabel.Text = "開挖至GL.-31m，構築第6層樓版(GL.-29.25m)";
            Set_GroundDefleciton_WallBuckle(2.24, 4.30);
        }

        private void Stage12_Click(object sender, EventArgs e)
        {
            List<List<ElementId>> remove_stages = new List<List<ElementId>>();
            remove_stages.Add(Stage7);
            remove_stages.Add(Stage10);
            ShowUpToStage(12, remove_stages);
            StageContentLabel.Text = "拆除第2層支撐(GL.-28m)";
            Set_GroundDefleciton_WallBuckle(2.34, 4.50);
        }

        private void Stage13_Click(object sender, EventArgs e)
        {
            List<List<ElementId>> remove_stages = new List<List<ElementId>>();
            remove_stages.Add(Stage7);
            remove_stages.Add(Stage10);
            ShowUpToStage(13, remove_stages);
            StageContentLabel.Text = "開挖至GL.-35m，架設第3層支撐H428x2@5m, at GL.-34m, 預壓-120t";
            Set_GroundDefleciton_WallBuckle(2.80, 5.37);
        }

        private void Stage14_Click(object sender, EventArgs e)
        {
            List<List<ElementId>> remove_stages = new List<List<ElementId>>();
            remove_stages.Add(Stage7);
            remove_stages.Add(Stage10);
            ShowUpToStage(14, remove_stages);
            StageContentLabel.Text = "開挖至GL.-37.5m，構築第7層樓版(GL.-36.05m)";
            Set_GroundDefleciton_WallBuckle(3.30, 6.34);
        }

        private void Stage15_Click(object sender, EventArgs e)
        {
            List<List<ElementId>> remove_stages = new List<List<ElementId>>();
            remove_stages.Add(Stage7);
            remove_stages.Add(Stage10);
            remove_stages.Add(Stage13);
            ShowUpToStage(15, remove_stages);
            StageContentLabel.Text = "拆除第3層支撐(GL.-34m)";
            Set_GroundDefleciton_WallBuckle(3.48, 6.70);
        }

        private void ShowAll_Click(object sender, EventArgs e)
        {
            ShowUpToStage(15, null);
            StageContentLabel.Text = "顯示全部的樓板及支撐";
            GroundDeflection.Text = "None";
            WallBuckle.Text = "None";
        }

        private void CapstoneWindowsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ShowUpToStage(20, null);
        }

        public static Element FindFamilyType(
        Document rvtDoc, Type targetType,
        string targetFamilyName,
        string targetTypeName,
        Nullable<BuiltInCategory> targetCategory)
        {
            // first, narrow down to the elements of the given type and category
            var collector =
                new FilteredElementCollector(rvtDoc).OfClass(targetType);
            if (targetCategory.HasValue)
            {
                collector.OfCategory(targetCategory.Value);
            }
            // parse the collection for the given names 
            // using LINQ query here. 
            var targetElems =
                from element in collector
                where element.Name.Equals(targetTypeName) &&
                element.get_Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM).
                AsString().Equals(targetFamilyName)
                select element;

            // put the result as a list of element fo accessibility. 
            IList<Element> elems = targetElems.ToList();
            // return the result. 
            if (elems.Count > 0)
            {
                return elems[0];
            }
            return null;
        }

    }


}
