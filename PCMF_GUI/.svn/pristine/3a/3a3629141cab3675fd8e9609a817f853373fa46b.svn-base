using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace IPS.Comm
{
    public class OrderByClass
    {
        public OrderByClass()
        {

        }

        //規則顯示順序排序failed多的排後面
        public static ObservableCollection<OntologyResultForNCFile> GetOrderby(ObservableCollection<OntologyResultForNCFile> _list)
        {
            ObservableCollection<OntologyResultForNCFile> _list2 = new ObservableCollection<OntologyResultForNCFile>();
            ObservableCollection<OntologyResultForCuttingTool> _templist = new ObservableCollection<OntologyResultForCuttingTool>();
            int count = 0;
            foreach (var a in _list)
            {
                OntologyResultForNCFile orfNCF = new OntologyResultForNCFile();
                foreach (var b in a.ReplaceableRuleSet)
                {
                    OntologyResultForCuttingTool orfCT = new OntologyResultForCuttingTool();
                    if (b.ExternalDiameterProcessingRule.Equals("Failed"))
                    {
                        count++;
                    }
                    
                    if (b.FinishingRule.Equals("Failed"))
                    {
                        count++;
                    }
                    
                    if (b.InternalDiameterProcessingCuboidRule.Equals("Failed"))
                    {
                        count++;
                    }
                    
                    if (b.RoughingRule.Equals("Failed"))
                    {
                        count++;
                    }
                    
                    if (b.InternalDiameterProcessingCylinderRule.Equals("Failed"))
                    {
                        count++;
                    }
                    
                    if (b.ToolNWPHardnessRule.Equals("Failed"))
                    {
                        count++;
                    }

                    orfCT.sortindex = count.ToString();
                    orfCT.CuttingToolNo = b.CuttingToolNo;
                    orfCT.index = b.index;
                    orfCT.selectReplace = b.selectReplace;
                    orfCT.ExternalDiameterProcessingRule = b.ExternalDiameterProcessingRule;
                    orfCT.InternalDiameterProcessingCuboidRule = b.InternalDiameterProcessingCuboidRule;
                    orfCT.InternalDiameterProcessingCylinderRule = b.InternalDiameterProcessingCylinderRule;
                    orfCT.ReplaceableCuttingToolNo = b.ReplaceableCuttingToolNo;
                    orfCT.ToolNWPHardnessRule = b.ToolNWPHardnessRule;
                    orfCT.RoughingRule = b.RoughingRule;
                    orfCT.FinishingRule = b.FinishingRule;
                    _templist.Add(orfCT);
                    orfNCF.ReplaceableRuleSet = _templist;
                    //初始化計算共有幾個Failed，一行就是一筆計算資料
                    count = 0;
                }
                orfNCF.index = a.index;
                orfNCF.FileName = a.FileName;
                orfNCF.StageNo = a.StageNo;
                orfNCF.CuttingToolNo = a.CuttingToolNo;
                orfNCF.SelectCT = a.SelectCT;
                _list2.Add(orfNCF);
            }
            return _list2;
        }
    }
}
