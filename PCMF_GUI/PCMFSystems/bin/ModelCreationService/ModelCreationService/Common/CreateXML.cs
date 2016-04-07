using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// For XML
using System.Xml;
using System.Collections.Generic;
// For File
using System.IO;


namespace ModelCreationService
{
    class CreateXML
    {
        XmlDocument xDoc;
        XmlElement Top_Node;


        public void Create_XML_Document(String MCSModelInfoFileName

            //// ConditionInfo
            //String inStrategyName, String inStrategyCreateTime, 
            //String inModifier, String inStartSearchTime, 
            //String inEndSearchTime, String inIsNormalUser, 
            //String inPhaseType, String inModelCreateTime, 
            //String inStrategyID,
            
            //// ElementType
            //Dictionary<String, List<String>> inElementTypeDic,
            
            //// Combination
            //Dictionary<String, List<String>> inCombinationDic,
            
            //// PieceCount
            //Dictionary<String, List<String>> inPieceCountDic,
            
            //// VariableDef
            //Dictionary<String, List<String>> inVariableDefDic,
            
            //// FileList
            //Dictionary<String, List<String>> inFileListDic,
            
            //// METROLOGY01O.xml
            //List<String> inMetrologyTableInfoList, String inMetrologyTableName,
            
            //// IndicatorRule
            //List<String> isIndicatorRuleList,
            
            //// Group
            //Dictionary<String, Dictionary<String, List<String>>> inGroupListDic,
            
            //// ExpertKnowledge
            //Dictionary<String, List<String>> inExpertKnowledgeDic,
            
            //// ModuleList
            //String isDQIx, String isDQIy, String isMDFR, String KSS_Type, String KVS_Type, String MR_Type, String GSI_Type, String NN_Type, String RI_Tpye, String isWS,
            
            //// PROCESS01
            //Dictionary<String, List<String>> inPROCESS01_IndicatorType,
            //List<String> PROCESS01_BlockID,
            //List<String> PROCESS01_BlockIDEmpty,
            //String inPROCESSTableName,
            
            //// DCQV
            //Dictionary<String, List<String>> inDCQV_TimePoint,
            //List<String> DCQV_TimePointMode,
            
            //// FileTemporalRule
            //Dictionary<String, List<String>> inFileTemporalRule_Indicator,
            //List<String> FileTemporalRule_IndicatorValue,
            
            //// IndicatorRule
            //Dictionary<String, List<String>> inIndicatorRule_Indicator,
            //List<String> IndicatorRule_IndicatorValueEmpty,
            
            //// ContourInfo
            //Dictionary<String, List<String>> inContourInfo,
            //List<String> ContourInfo_Value,
            
            //// PointList
            //Dictionary<String, List<String>> inPointList_Point,
            //List<String> PointList_PointValue,

            //// MDFR_Parameters
            //String inMDFREwmalamda, String inMDFREwmaWindow, 
            //String inMDFREwmaTolerance, String inMDFRVarConfidence, 
            //String inMDFRbaseSampleNum, String inMDFRRangeMultipleValue,

            //// DQIx_Parameters
            //String inDQIxLambda, String inDQIxConstant, 
            //String inDQIxDQIxFilterPercentage, String inDQIxDQIxRefreshCounter,

            //// DQIy_Parameters
            //String inDQIycorralpha, 
            //String inDQIyIsMixedModel,

            //// KSS_Parameters
            //String inKSSClusterNumber,

            //// KVS_Parameters
            //String inKVSFin_apha, 
            //String inKVSFout_apha, 
            //String inKVSOneByOneChoose,

            //// BPNN_Parameters
            //String inBPNNEpochsRange1, String inBPNNEpochsRange2, String inBPNNEpochsRange3, 
            //String inBPNNMomTermRange1, String inBPNNMomTermRange2, String inBPNNMomTermRange3, 
            //String inBPNNAlphaRange1, String inBPNNAlphaRange2, String inBPNNAlphaRange3, 
            //String inBPNNNodesRange1, String inBPNNNodesRange2, String inBPNNNodesRange3, 
            //String inBPNNOneByOneChoose, String inBPNNBPNNRefreshCounter,
            //List<String> inBPNNNodesRange,

            //// RI_Parameters
            //List<String> inRITolerantMaxError,

            //// MR_Parameters
            //String inMR_TSVD_Condition_Number_Criteria, 
            //String inMR_TSVD_Energy_Ratio_Criteria, 
            //String inMRRefreshCounter,

            //// GSI_Parameters
            //String inGSIRefreshCounter, 
            //String inGSI_TSVD_Condition_Number_Criteria, 
            //String inGSI_TSVD_Energy_Ratio_Criteria,
            //List<String> inGSIRT, 
            //List<String> inGSIThreshold,

            //// AlqorithmPreference
            //String inAlqorithmPreferencePreferredVMOutput
            ) 
        {
            try {
                // 原本直接重建的方式有問題 [10/22/2012 autolab]
                //// [ 建立 XML文件 ]
                //xDoc = new XmlDocument();
                //// [ 定義文件編碼定義 ]
                //xDoc.AppendChild(xDoc.CreateXmlDeclaration("1.0", "UTF-8", null));
                //// [ 建立頂級節點 "Message" ]
                //XmlElement First_Node;

                //First_Node = xDoc.CreateElement("message");
                //xDoc.AppendChild(First_Node);


                //XmlAttribute attr = xDoc.CreateAttribute("xsi", "schemaLocation", " ");
                //attr.Value = "..\\XSD_Format\\BackupModel.xsd";
                //Top_Node.Attributes.Append(attr);
                //Top_Node.SetAttribute("dir", "MCS2VMMS");
                //Top_Node.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                //Top_Node.SetAttribute("xmlns", "http://www.w3schools.com");
                //Top_Node.SetAttribute("time", "");
                //Top_Node.SetAttribute("wait", "0");
                //Top_Node.SetAttribute("id", "1");
                //Top_Node.SetAttribute("src", "MCS");
                //Top_Node.SetAttribute("name", "MCSModelInfo");
                
                // 改為讀取標準檔 [10/22/2012 autolab]

                String strCommand = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
                strCommand += "<message xsi:schemaLocation=\"..\\XSD_Format\\MCSModelInfo.xsd\" xmlns=\"http://www.w3schools.com\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" name=\"MCSModelInfo\" src=\"MCS\" id=\"1\" dir=\"MCS2VMMS\" wait=\"0\">";
                strCommand += "</message>";


                xDoc = new XmlDocument();
                xDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\MCS_MCSModelInfo.xml");
                //xDoc.LoadXml(strCommand);




                #region function

                /*
                // [ B. (OK) 建立 Parameters (ConditionInfo) ]
                this.Create_ConditionInfo(inStrategyName, inStrategyCreateTime, inModifier, inStartSearchTime, inEndSearchTime, inIsNormalUser, inPhaseType, inModelCreateTime, inStrategyID);
                // [ C. (OK) 建立 Parameters (ElementType) ]
                this.Create_ElementType(inElementTypeDic);
                // [ D. (OK) 建立 Parameters (Combination) ]
                this.Create_Combination(inCombinationDic);
                // [ E. (OK) 建立 Parameters (PieceCount) ]
                this.Create_PieceCount(inPieceCountDic);
                // [ F. (OK) 建立 Parameters (VariableDef) ]
                this.Create_VariableDef(inVariableDefDic);
                // [ G. (OK) 建立 Parameters (FileList) ]
                this.Create_FileList(inFileListDic);
                // [ H. (OK) 建立 Parameters (METROLOGY01O.xml) ]
                this.Create_MetrologyTableInfo(inMetrologyTableInfoList, inMetrologyTableName);
                // [ I. 建立 Parameters (PROCESS01I.xml) ]
                this.Create_ProcessTableInfo(inPROCESS01_IndicatorType, PROCESS01_BlockID, PROCESS01_BlockIDEmpty, inPROCESSTableName);
                // [ J. 建立 Parameters (DAQV) ]
                this.Create_DAQV(inDCQV_TimePoint, DCQV_TimePointMode);
                // [ K. 建立 Parameters (FilterTemporalRule) ]
                this.Create_FilterTemporalRule(inFileTemporalRule_Indicator, FileTemporalRule_IndicatorValue);
                // [ L. 建立 Parameters (AlgorithmList) ]
                this.Create_AlgorithmList();
                // [ M. (OK) 建立 Parameters (IndicatorRule) ]
                this.Create_IndicatorRule(isIndicatorRuleList);
                // [ N. 建立 Parameters (IndicatorList) ]
                this.Create_IndicatorList(inIndicatorRule_Indicator, IndicatorRule_IndicatorValueEmpty);
                // [ O. 建立 Parameters (ContourInfo) ]
                this.Create_ContourInfo(inContourInfo, ContourInfo_Value);
                // [ P. 建立 Parameters (PointList) ]
                this.Create_PointList(inPointList_Point, PointList_PointValue);
                // [ Q. (OK) 建立 Parameters (GroupList) ]
                this.Create_GroupList(inGroupListDic);
                // [ R. (OK) 建立 Parameters (ExpertKnowledge) ]
                this.Create_ExpertKnowledge(inExpertKnowledgeDic);
                // (OK) Group 資訊區
                this.Create_GroupInfo(inGroupListDic);
                // [ S. (OK) 建立 Parameters (ModuleList) ]
                this.Create_ModuleList(isDQIx, isDQIy, isMDFR, KSS_Type, KVS_Type, MR_Type, GSI_Type, NN_Type, RI_Tpye, isWS);
                // [ T. 建立 Parameters (MDFR_Parameters) ]
                this.Create_MDFR_Parameters(inMDFREwmalamda, inMDFREwmaWindow, inMDFREwmaTolerance, inMDFRVarConfidence, inMDFRbaseSampleNum, inMDFRRangeMultipleValue);
                // [ U. 建立 Parameters (DQIx_Parameters) ]
                this.Create_DQIx_Parameters(inDQIxLambda, inDQIxConstant, inDQIxDQIxFilterPercentage, inDQIxDQIxRefreshCounter);
                // [ V. 建立 Parameters (DQIy_Parameters) ]
                this.Create_DQIy_Parameters(inDQIycorralpha, inDQIyIsMixedModel);
                // [ W. 建立 Parameters (KSS_Parameters) ]
                this.Create_KSS_Parameters(inKSSClusterNumber);
                // [ X. 建立 Parameters (KVS_Parameters) ]
                this.Create_KVS_Parameters(inKVSFin_apha, inKVSFout_apha, inKVSOneByOneChoose);
                // [ Y. 建立 Parameters (BPNN_Parameters) ]
                this.Create_BPNN_Parameters(
                    inBPNNEpochsRange1, inBPNNEpochsRange2, inBPNNEpochsRange3, inBPNNMomTermRange1, inBPNNMomTermRange2,
                    inBPNNMomTermRange3, inBPNNAlphaRange1, inBPNNAlphaRange2, inBPNNAlphaRange3, inBPNNNodesRange1,
                    inBPNNNodesRange2, inBPNNNodesRange3, inBPNNOneByOneChoose, inBPNNBPNNRefreshCounter, inBPNNNodesRange);
                // [ Z. 建立 Parameters (RI_Parameters) ]
                this.Create_RI_Parameters(inRITolerantMaxError);
                // [ AA. 建立 Parameters (MR_Parameters) ]
                this.Create_MR_Parameters(inMR_TSVD_Condition_Number_Criteria, inMR_TSVD_Energy_Ratio_Criteria, inMRRefreshCounter);
                // [ AB. 建立 Parameters (GSI_Parameters) ]
                this.Create_GSI_Parameters(inGSIRefreshCounter, inGSI_TSVD_Condition_Number_Criteria, inGSI_TSVD_Energy_Ratio_Criteria, inGSIRT, inGSIThreshold);
                // [ AC. (OK) 建立 Parameters (AlqorithmPreference) ]
                this.Create_AlqorithmPreference(inAlqorithmPreferencePreferredVMOutput);
*/
#endregion

                // [ AD. 儲存 XML ]
                xDoc.Save(MCSModelInfoFileName);
            }
            catch (Exception ex) {
                String ErrorWord = ex.ToString();
            }
            finally
            {
                /// 
            }
        }

        public void LoadXML(String strFileName)
        {
            try
            {
                // [ 建立 XML文件 ]
                xDoc = new XmlDocument();
                xDoc.LoadXml(strFileName);

                


            }
            catch (Exception ex) {
                String ErrorWord = ex.ToString();
            }
            finally
            {
                /// 
            }
        }
                                
        
        // [ B. 建立 Parameters (ConditionInfo) ]
        public void Create_ConditionInfo(String strFileName, 
            String inStrategyName, String inStrategyCreateTime, String inModifier, String inStartSearchTime, String inEndSearchTime, String inIsNormalUser, String inPhaseType, String inModelCreateTime, String inStrategyID)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);



            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;
            
            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='ConditionInfo']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }


            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;
            
            // [ A. (OK) 建立 Command ]
            XmlElement Command_Node = xDoc.CreateElement("command");
            Command_Node.SetAttribute("name", "MCSModelInfo");
            Top_Node.AppendChild(Command_Node);

            // [ B. (OK) 建立 ConditionInfo ]
            XmlElement ConditionInfo_Node = xDoc.CreateElement("parameters");
            ConditionInfo_Node.SetAttribute("name", "ConditionInfo");
            Top_Node.AppendChild(ConditionInfo_Node);
            // B1. 建立 Strategy
            XmlElement StrategyName_Node = xDoc.CreateElement("parameter");
            StrategyName_Node.SetAttribute("name", "StrategyName");
            ConditionInfo_Node.AppendChild(StrategyName_Node);
            XmlElement StrategyName_Value = xDoc.CreateElement("value");
            StrategyName_Value.InnerText = inStrategyName;
            StrategyName_Node.AppendChild(StrategyName_Value);
            // B2. Strategy的建立時間
            XmlElement StrategyCreateTime_Node = xDoc.CreateElement("parameter");
            StrategyCreateTime_Node.SetAttribute("name", "StrategyCreateTime");
            ConditionInfo_Node.AppendChild(StrategyCreateTime_Node);
            XmlElement StrategyCreateTime_Value = xDoc.CreateElement("value");
            StrategyCreateTime_Value.InnerText = inStrategyCreateTime;
            StrategyCreateTime_Node.AppendChild(StrategyCreateTime_Value);
            // B3. 編輯者
            XmlElement Modifier_Node = xDoc.CreateElement("parameter");
            Modifier_Node.SetAttribute("name", "Modifier");
            ConditionInfo_Node.AppendChild(Modifier_Node);
            XmlElement Modifier_Value = xDoc.CreateElement("value");
            Modifier_Value.InnerText = inModifier;
            Modifier_Node.AppendChild(Modifier_Value);
            // B4. 建模資料起始時間
            XmlElement StartSearchTime_Node = xDoc.CreateElement("parameter");
            StartSearchTime_Node.SetAttribute("name", "StartSearchTime");
            ConditionInfo_Node.AppendChild(StartSearchTime_Node);
            XmlElement StartSearchTime_Value = xDoc.CreateElement("value");
            StartSearchTime_Value.InnerText = inStartSearchTime;
            StartSearchTime_Node.AppendChild(StartSearchTime_Value);
            // B5. 建模資料結束時間
            XmlElement EndSearchTime_Node = xDoc.CreateElement("parameter");
            EndSearchTime_Node.SetAttribute("name", "EndSearchTime");
            ConditionInfo_Node.AppendChild(EndSearchTime_Node);
            XmlElement EndSearchTime_Value = xDoc.CreateElement("value");
            EndSearchTime_Value.InnerText = inEndSearchTime;
            EndSearchTime_Node.AppendChild(EndSearchTime_Value);
            // B6. 一般使用者 ?
            XmlElement IsNormalUser_Node = xDoc.CreateElement("parameter");
            IsNormalUser_Node.SetAttribute("name", "IsNormalUser");
            ConditionInfo_Node.AppendChild(IsNormalUser_Node);
            XmlElement IsNormalUser_Value = xDoc.CreateElement("value");
            IsNormalUser_Value.InnerText = inIsNormalUser;
            IsNormalUser_Node.AppendChild(IsNormalUser_Value);
            // B7. Phase編號
            XmlElement PhaseType_Node = xDoc.CreateElement("parameter");
            PhaseType_Node.SetAttribute("name", "PhaseType");
            ConditionInfo_Node.AppendChild(PhaseType_Node);
            XmlElement PhaseType_Value = xDoc.CreateElement("value");
            PhaseType_Value.InnerText = inPhaseType;
            PhaseType_Node.AppendChild(PhaseType_Value);
            // B8. STDB
            XmlElement STDBAlias_Node = xDoc.CreateElement("parameter");
            STDBAlias_Node.SetAttribute("name", "STDBAlias");
            STDBAlias_Node.SetAttribute("localstdbestablished", "0");
            ConditionInfo_Node.AppendChild(STDBAlias_Node);
            XmlElement STDBAlias_Value = xDoc.CreateElement("value");
            STDBAlias_Node.AppendChild(STDBAlias_Value);
            // B9. 此AVM的使用環境
            XmlElement CaseName_Node = xDoc.CreateElement("parameter");
            CaseName_Node.SetAttribute("name", "CaseName");
            ConditionInfo_Node.AppendChild(CaseName_Node);
            XmlElement CaseName_Value = xDoc.CreateElement("value");
            CaseName_Value.InnerText = "v-Machine";
            CaseName_Node.AppendChild(CaseName_Value);
            // B10. Strategy ID
            XmlElement StrategyID_Node = xDoc.CreateElement("parameter");
            StrategyID_Node.SetAttribute("name", "StrategyID");
            ConditionInfo_Node.AppendChild(StrategyID_Node);
            // B11. 模型建立時間
            XmlElement ModelCreateTime_Node = xDoc.CreateElement("parameter");
            ModelCreateTime_Node.SetAttribute("name", "ModelCreateTime");
            ConditionInfo_Node.AppendChild(ModelCreateTime_Node);
            XmlElement ModelCreateTime_Value = xDoc.CreateElement("value");
            ModelCreateTime_Value.InnerText = inModelCreateTime;
            ModelCreateTime_Node.AppendChild(ModelCreateTime_Value);
            
            //// B12. Strategy的編號
            //XmlElement STRATEGYID_Node = xDoc.CreateElement("parameter");
            //STRATEGYID_Node.SetAttribute("name", "STRATEGYID");
            //ConditionInfo_Node.AppendChild(STRATEGYID_Node);
            //XmlElement STRATEGYID_Value = xDoc.CreateElement("value");
            ////STRATEGYID_Value.InnerText = inStrategyID;
            //STRATEGYID_Node.AppendChild(STRATEGYID_Value);

            xDoc.Save(strFileName);
        }

        // [ C. 建立 Parameters (ElementType) ]
        public void Create_ElementType(String strFileName, 
            Dictionary<String, List<String>> inElementTypeDir)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='ElementType']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }  

            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement ElementType_Node = xDoc.CreateElement("parameters");
            ElementType_Node.SetAttribute("name", "ElementType");
            Top_Node.AppendChild(ElementType_Node);
            if (inElementTypeDir.Count > 0) {
                int ordernumber = 1;
                foreach (KeyValuePair<String, List<String>> item in inElementTypeDir) {
                    XmlElement ElementTypeSummary_Node = xDoc.CreateElement("parameter");
                    ElementTypeSummary_Node.SetAttribute("name", item.Key);
                    ElementType_Node.AppendChild(ElementTypeSummary_Node);
                    ordernumber = 1;
                    foreach (String myList in item.Value) {
                        XmlElement ElementTypeSummary_value = xDoc.CreateElement("value");
                        ElementTypeSummary_value.SetAttribute("ordernumber", ordernumber.ToString());
                        ElementTypeSummary_value.InnerText = myList;
                        ElementTypeSummary_Node.AppendChild(ElementTypeSummary_value);
                        ordernumber++;
                    }
                }
            }

            xDoc.Save(strFileName);
        }
        
        // [ D. 建立 Parameters (Combination) ]
        public void Create_Combination(String strFileName, 
            Dictionary<String, List<String>> inCombinationDic)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);

            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='Combination']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }


            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement Combination_Node = xDoc.CreateElement("parameters");
            Combination_Node.SetAttribute("name", "Combination");
            Top_Node.AppendChild(Combination_Node);
            if (inCombinationDic.Count > 0) {
                foreach (KeyValuePair<String, List<String>> item in inCombinationDic) {
                    XmlElement Combination_Sub_Node = xDoc.CreateElement("parameter");
                    Combination_Sub_Node.SetAttribute("name", item.Key);
                    Combination_Node.AppendChild(Combination_Sub_Node);
                    foreach (String myList in item.Value) {
                        XmlElement Combination_Sub_value = xDoc.CreateElement("value");
                        Combination_Sub_value.SetAttribute("category", item.Key);
                        Combination_Sub_value.InnerText = myList;
                        Combination_Sub_Node.AppendChild(Combination_Sub_value);
                    }
                }
            }

            xDoc.Save(strFileName);
        }

        // [ E. 建立 Parameters (PieceCount) ]
        public void Create_PieceCount(String strFileName, 
            Dictionary<String, List<String>> inPieceCountDic)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='PieceCount']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }

            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement PieceCount_Node = xDoc.CreateElement("parameters");
            PieceCount_Node.SetAttribute("name", "PieceCount");
            Top_Node.AppendChild(PieceCount_Node);
            if (inPieceCountDic.Count > 0)
            {
                foreach (KeyValuePair<String, List<String>> item in inPieceCountDic)
                {
                    XmlElement PieceCount_Sub_Node = xDoc.CreateElement("parameter");
                    PieceCount_Sub_Node.SetAttribute("name", item.Key);
                    PieceCount_Node.AppendChild(PieceCount_Sub_Node);
                    foreach (String myList in item.Value)
                    {
                        XmlElement PieceCount_Sub_value = xDoc.CreateElement("value");
                        PieceCount_Sub_value.InnerText = myList;
                        PieceCount_Sub_Node.AppendChild(PieceCount_Sub_value);
                    }
                }
            }

            xDoc.Save(strFileName);
        }

        // [ F. 建立 Parameters (VariableDef) ]
        public void Create_VariableDef(String strFileName, 
            Dictionary<String, List<String>> inVariableDefDic)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);
            
            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='VariableDef']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }

            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement VariableDef_Node = xDoc.CreateElement("parameters");
            VariableDef_Node.SetAttribute("name", "VariableDef");
            Top_Node.AppendChild(VariableDef_Node);
            int ordernumber;
            if (inVariableDefDic.Count > 0) {
                foreach (KeyValuePair<String, List<String>> item in inVariableDefDic) {
                    XmlElement VariableDef_Sub_Node = xDoc.CreateElement("parameter");
                    String[] Title = item.Key.Split(new Char[] { ';' });
                    VariableDef_Sub_Node.SetAttribute("name", Title[0]);
                    VariableDef_Sub_Node.SetAttribute("variabletype", Title[1]);
                    VariableDef_Node.AppendChild(VariableDef_Sub_Node);
                    ordernumber = 1;
                    foreach (String myList in item.Value) {
                        XmlElement VariableDef_Sub_value = xDoc.CreateElement("value");
                        VariableDef_Sub_value.SetAttribute("ordernumber", ordernumber.ToString());
                        VariableDef_Sub_value.InnerText = myList;
                        VariableDef_Sub_Node.AppendChild(VariableDef_Sub_value);
                        ordernumber++;
                    }
                }
            }

            xDoc.Save(strFileName);
        }

        // [ G. 建立 Parameters (FileList) ]
        public void Create_FileList(String strFileName, 
            Dictionary<String, List<String>> inFileListDic)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='FileList']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement FileList_Node = xDoc.CreateElement("parameters");
            FileList_Node.SetAttribute("name", "FileList");
            Top_Node.AppendChild(FileList_Node);
            if (inFileListDic.Count > 0) {
                foreach (KeyValuePair<String, List<String>> item in inFileListDic) {
                    XmlElement FileList_Sub_Node = xDoc.CreateElement("parameter");
                    String[] Title = item.Key.Split(new Char[] { ';' });
                    FileList_Sub_Node.SetAttribute("name", Title[0]);
                    FileList_Sub_Node.SetAttribute("defname", Title[1]);
                    FileList_Sub_Node.SetAttribute("variablegroupname", Title[2]);
                    FileList_Sub_Node.SetAttribute("ioid", Title[3]);
                    FileList_Sub_Node.SetAttribute("io", Title[4]);
                    FileList_Node.AppendChild(FileList_Sub_Node);
                    foreach (String myList in item.Value) {
                        XmlElement FileList_Sub_value = xDoc.CreateElement("value");
                        FileList_Sub_value.SetAttribute("defvalue", myList);
                        FileList_Sub_value.InnerText = myList;
                        FileList_Sub_Node.AppendChild(FileList_Sub_value);
                    }
                }
            }
            xDoc.Save(strFileName);
        }

        // [ H. 建立 Parameters (METROLOGY01O.xml) ]
        public void Create_MetrologyTableInfo(String strFileName, 
            List<String> inMetrologyTableInfoList, String inMetrologyTableName)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='" + inMetrologyTableName + "']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement METROLOGY_Node = xDoc.CreateElement("parameters");
            METROLOGY_Node.SetAttribute("name", inMetrologyTableName);
            Top_Node.AppendChild(METROLOGY_Node);
            if (inMetrologyTableInfoList.Count > 0) {
                foreach (String item in inMetrologyTableInfoList) {
                    String[] Title = item.Split(new Char[] { ',' });
                    XmlElement MetrologyTableInfo_Sub_value = xDoc.CreateElement("parameter");
                    MetrologyTableInfo_Sub_value.SetAttribute("name", Title[0]);
                    MetrologyTableInfo_Sub_value.SetAttribute("separator", Title[1]);
                    MetrologyTableInfo_Sub_value.SetAttribute("variableid", Title[2]);
                    METROLOGY_Node.AppendChild(MetrologyTableInfo_Sub_value);
                }
            }
            xDoc.Save(strFileName);
        }

        // [ I. 建立 Parameters (PROCESS01I.xml) ]
        public void Create_ProcessTableInfo(String strFileName, 
            //Dictionary<String, List<String>> inPROCESS01_IndicatorType, List<String> PROCESS01_BlockID, List<String> PROCESS01_BlockIDEmpty, String inPROCESSTableName)
            Dictionary<String, List<String>> inPROCESS01_IndicatorType, String inPROCESSTableName)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='" + inPROCESSTableName + "']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement PROCESS_Node = xDoc.CreateElement("parameters");
            PROCESS_Node.SetAttribute("name", inPROCESSTableName);
            Top_Node.AppendChild(PROCESS_Node);
            foreach (KeyValuePair<String, List<String>> pair in inPROCESS01_IndicatorType)
            {
                String[] split = pair.Key.Split(new Char[] { ',' });
                XmlElement PROCESS_Parameter_Node = xDoc.CreateElement("parameter");
                if (split[0] == "Block_ID")
                {
                    //Block_ID
                    PROCESS_Parameter_Node = xDoc.CreateElement("parameter");
                    PROCESS_Parameter_Node.SetAttribute("name", "Block_ID");
                    PROCESS_Parameter_Node.SetAttribute("separator", split[1]);
                    PROCESS_Parameter_Node.SetAttribute("variableid", split[2]);
                    PROCESS_Node.AppendChild(PROCESS_Parameter_Node);
                    XmlElement PROCESS_value_Nodes = xDoc.CreateElement("value");
                    foreach (String pairBlock in pair.Value)
                    {
                        PROCESS_value_Nodes = xDoc.CreateElement("value");
                        PROCESS_value_Nodes.InnerText = pairBlock;
                        PROCESS_Parameter_Node.AppendChild(PROCESS_value_Nodes);
                    }
                }
                else
                {
                    PROCESS_Parameter_Node = xDoc.CreateElement("parameter");
                    PROCESS_Parameter_Node.SetAttribute("name", split[0]);
                    PROCESS_Parameter_Node.SetAttribute("separator", split[1]);
                    PROCESS_Parameter_Node.SetAttribute("variableid", split[2]);
                    PROCESS_Node.AppendChild(PROCESS_Parameter_Node);
                }
            }

            xDoc.Save(strFileName);
        }

        // [ J. 建立 Parameters (DCQV) ]
        public void Create_DCQV(String strFileName, 
            //Dictionary<String, List<String>> inDCQV_TimePoint, List<String> DCQV_TimePointMode)
            Dictionary<String, List<String>> inDCQV_TimePoint)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='DCQV']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement DCQV_Node = xDoc.CreateElement("parameters");
            DCQV_Node.SetAttribute("name", "DCQV");
            DCQV_Node.SetAttribute("value", "90");
            Top_Node.AppendChild(DCQV_Node);
            foreach (KeyValuePair<String, List<String>> pair in inDCQV_TimePoint)
            {
                String[] split = pair.Key.Split(new Char[] { ',' });
                XmlElement DCQV_Parameter_Node = xDoc.CreateElement("parameter");
                DCQV_Parameter_Node = xDoc.CreateElement("parameter");
                DCQV_Parameter_Node.SetAttribute("name", split[0]);
                DCQV_Parameter_Node.SetAttribute("filename", split[1]);
                DCQV_Parameter_Node.SetAttribute("dcqvid", split[2]);
                DCQV_Node.AppendChild(DCQV_Parameter_Node);
                XmlElement DCQV_value_Nodes = xDoc.CreateElement("value");
                foreach (String pairLimitation in pair.Value)
                {
                    String[] splitLimitation = pairLimitation.Split(new Char[] { ',' });
                    DCQV_value_Nodes = xDoc.CreateElement("value");
                    DCQV_value_Nodes.SetAttribute("variableid", splitLimitation[0]);
                    DCQV_value_Nodes.SetAttribute("standard", splitLimitation[1]);
                    DCQV_value_Nodes.SetAttribute("limition", splitLimitation[2]);
                    DCQV_Parameter_Node.AppendChild(DCQV_value_Nodes);
                }
            }

            xDoc.Save(strFileName);
        }

        // [ K. 建立 Parameters (FilterTemporalRule) ]
        public void Create_FilterTemporalRule(String strFileName, 
            //Dictionary<String, List<String>> inFileTemporalRule_Indicator, List<String> FileTemporalRule_IndicatorValue)
            Dictionary<String, List<String>> inFileTemporalRule_Indicator)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='FilterTemporalRule']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement FilterTemporalRule_Node = xDoc.CreateElement("parameters");
            FilterTemporalRule_Node.SetAttribute("name", "FilterTemporalRule");
            Top_Node.AppendChild(FilterTemporalRule_Node);
            foreach (KeyValuePair<String, List<String>> pair in inFileTemporalRule_Indicator)
            {
                String[] split = pair.Key.Split(new Char[] { ',' });
                XmlElement FTR_Parameter_Node = xDoc.CreateElement("parameter");
                FTR_Parameter_Node = xDoc.CreateElement("parameter");
                FTR_Parameter_Node.SetAttribute("name", split[0]);
                FTR_Parameter_Node.SetAttribute("variableid", split[1]);
                FTR_Parameter_Node.SetAttribute("dcqvid", split[2]);
                FTR_Parameter_Node.SetAttribute("rawdatafilename", split[3]);
                FTR_Parameter_Node.SetAttribute("description", split[4]);
                FTR_Parameter_Node.SetAttribute("lcl", split[5]);
                FTR_Parameter_Node.SetAttribute("ucl", split[6]);
                FTR_Parameter_Node.SetAttribute("lsl", split[7]);
                FTR_Parameter_Node.SetAttribute("usl", split[8]);
                FTR_Parameter_Node.SetAttribute("endcount", split[9]);
                FTR_Parameter_Node.SetAttribute("startcount", split[10]);
                FTR_Parameter_Node.SetAttribute("filterid", split[11]);
                FilterTemporalRule_Node.AppendChild(FTR_Parameter_Node);
                XmlElement FTR_value_Nodes = xDoc.CreateElement("value");
                foreach (String pairLimitation in pair.Value)
                {
                    String[] splitLimitation = pairLimitation.Split(new Char[] { ',' });
                    FTR_value_Nodes = xDoc.CreateElement("value");
                    FTR_value_Nodes.SetAttribute("variableid", splitLimitation[0]);
                    FTR_value_Nodes.SetAttribute("limition", splitLimitation[1]);
                    FTR_value_Nodes.SetAttribute("type", splitLimitation[2]);
                    FTR_value_Nodes.InnerText = splitLimitation[3];
                    FTR_Parameter_Node.AppendChild(FTR_value_Nodes);
                }
            }

            xDoc.Save(strFileName);
        }

        // [ L. 建立 Parameters (AlgorithmList) ]
        public void Create_AlgorithmList(String strFileName)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='AlgorithmList']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement AlgorithmList_Parameters_Node = xDoc.CreateElement("parameters");
            AlgorithmList_Parameters_Node.SetAttribute("name", "AlgorithmList");
            Top_Node.AppendChild(AlgorithmList_Parameters_Node);
            //Mean
            XmlElement AlgorithmList_Parameter_Node = xDoc.CreateElement("parameter");
            AlgorithmList_Parameter_Node.SetAttribute("name", "Mean");
            AlgorithmList_Parameter_Node.SetAttribute("algorithmid", "1");
            AlgorithmList_Parameters_Node.AppendChild(AlgorithmList_Parameter_Node);
            //Min
            AlgorithmList_Parameter_Node = xDoc.CreateElement("parameter");
            AlgorithmList_Parameter_Node.SetAttribute("name", "Min");
            AlgorithmList_Parameter_Node.SetAttribute("algorithmid", "2");
            AlgorithmList_Parameters_Node.AppendChild(AlgorithmList_Parameter_Node);
            //Max
            AlgorithmList_Parameter_Node = xDoc.CreateElement("parameter");
            AlgorithmList_Parameter_Node.SetAttribute("name", "Max");
            AlgorithmList_Parameter_Node.SetAttribute("algorithmid", "3");
            AlgorithmList_Parameters_Node.AppendChild(AlgorithmList_Parameter_Node);
            //Range
            AlgorithmList_Parameter_Node = xDoc.CreateElement("parameter");
            AlgorithmList_Parameter_Node.SetAttribute("name", "Range");
            AlgorithmList_Parameter_Node.SetAttribute("algorithmid", "4");
            AlgorithmList_Parameters_Node.AppendChild(AlgorithmList_Parameter_Node);
            //Std
            AlgorithmList_Parameter_Node = xDoc.CreateElement("parameter");
            AlgorithmList_Parameter_Node.SetAttribute("name", "Std");
            AlgorithmList_Parameter_Node.SetAttribute("algorithmid", "5");
            AlgorithmList_Parameters_Node.AppendChild(AlgorithmList_Parameter_Node);
            //Slope
            AlgorithmList_Parameter_Node = xDoc.CreateElement("parameter");
            AlgorithmList_Parameter_Node.SetAttribute("name", "Slope");
            AlgorithmList_Parameter_Node.SetAttribute("algorithmid", "6");
            AlgorithmList_Parameters_Node.AppendChild(AlgorithmList_Parameter_Node);
            //counter
            AlgorithmList_Parameter_Node = xDoc.CreateElement("parameter");
            AlgorithmList_Parameter_Node.SetAttribute("name", "Counter");
            AlgorithmList_Parameter_Node.SetAttribute("algorithmid", "7");
            AlgorithmList_Parameters_Node.AppendChild(AlgorithmList_Parameter_Node);
            //Accumulation
            AlgorithmList_Parameter_Node = xDoc.CreateElement("parameter");
            AlgorithmList_Parameter_Node.SetAttribute("name", "Accumulation");
            AlgorithmList_Parameter_Node.SetAttribute("algorithmid", "8");
            AlgorithmList_Parameters_Node.AppendChild(AlgorithmList_Parameter_Node);

            xDoc.Save(strFileName);
        }

        // [ M. 建立 Parameters (IndicatorRule) ]
        public void Create_IndicatorRule(String strFileName, 
            List<String> isIndicatorRuleList)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='IndicatorRule']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;


            XmlElement IndicatorRule_Node = xDoc.CreateElement("parameters");
            IndicatorRule_Node.SetAttribute("name", "IndicatorRule");
            Top_Node.AppendChild(IndicatorRule_Node);
            if (isIndicatorRuleList.Count > 0)
            {
                foreach (String item in isIndicatorRuleList)
                {
                    String[] Title = item.Split(new Char[] { ',' });
                    XmlElement IndicatorRule_Sub_value = xDoc.CreateElement("parameter");
                    IndicatorRule_Sub_value.SetAttribute("name", Title[0]);
                    IndicatorRule_Sub_value.SetAttribute("variableid", Title[1]);
                    IndicatorRule_Sub_value.SetAttribute("description", Title[2]);
                    IndicatorRule_Sub_value.SetAttribute("lcl", Title[3]);
                    IndicatorRule_Sub_value.SetAttribute("ucl", Title[4]);
                    IndicatorRule_Sub_value.SetAttribute("lsl", Title[5]);
                    IndicatorRule_Sub_value.SetAttribute("usl", Title[6]);
                    IndicatorRule_Sub_value.SetAttribute("filterid", Title[7]);
                    IndicatorRule_Sub_value.SetAttribute("algorithmid", Title[8]);
                    IndicatorRule_Sub_value.SetAttribute("indicatorid", Title[9]);
                    IndicatorRule_Node.AppendChild(IndicatorRule_Sub_value);
                }
            }

            xDoc.Save(strFileName);
        }

        // [ N. 建立 Parameters (IndicatorList) ]
        public void Create_IndicatorList(String strFileName, 
            //Dictionary<String, List<String>> inIndicatorRule_Indicator, List<String> IndicatorRule_IndicatorValueEmpty)
            Dictionary<String, List<String>> inIndicatorRule_Indicator)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='IndicatorList']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement IndicatorList_Node = xDoc.CreateElement("parameters");
            IndicatorList_Node.SetAttribute("name", "IndicatorList");
            Top_Node.AppendChild(IndicatorList_Node);
            foreach (KeyValuePair<String, List<String>> pair in inIndicatorRule_Indicator)
            {
                String[] split = pair.Key.Split(new Char[] { ',' });
                XmlElement IndicatorList_Parameter_Node = xDoc.CreateElement("parameter");
                IndicatorList_Parameter_Node = xDoc.CreateElement("parameter");
                IndicatorList_Parameter_Node.SetAttribute("name", split[0]);
                IndicatorList_Parameter_Node.SetAttribute("variableid", split[1]);
                IndicatorList_Parameter_Node.SetAttribute("lcl", split[2]);
                IndicatorList_Parameter_Node.SetAttribute("ucl", split[3]);
                IndicatorList_Parameter_Node.SetAttribute("lsl", split[4]);
                IndicatorList_Parameter_Node.SetAttribute("usl", split[5]);
                IndicatorList_Parameter_Node.SetAttribute("filterid", split[6]);
                IndicatorList_Parameter_Node.SetAttribute("algorithmid", split[7]);
                IndicatorList_Parameter_Node.SetAttribute("indicatorid", split[8]);
                IndicatorList_Parameter_Node.SetAttribute("enable", split[9]);
                IndicatorList_Parameter_Node.SetAttribute("steptypeid", split[10]);
                IndicatorList_Node.AppendChild(IndicatorList_Parameter_Node);
            }

            xDoc.Save(strFileName);
        }

        // [ O. 建立 Parameters (ContourInfo) ]
        public void Create_ContourInfo(String strFileName, 
            //Dictionary<String, List<String>> inContourInfo, List<String> ContourInfo_Value)
            Dictionary<String, List<String>> inContourInfo)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='ContourInfo']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement ContourInfo_Node = xDoc.CreateElement("parameters");
            ContourInfo_Node.SetAttribute("name", "ContourInfo");
            Top_Node.AppendChild(ContourInfo_Node);
            foreach (KeyValuePair<String, List<String>> pair in inContourInfo)
            {
                String[] split = pair.Key.Split(new Char[] { ',' });
                XmlElement ContourInfo_Parameter_Node = xDoc.CreateElement("parameter");
                ContourInfo_Parameter_Node = xDoc.CreateElement("parameter");
                ContourInfo_Parameter_Node.SetAttribute("name", split[0]);
                ContourInfo_Node.AppendChild(ContourInfo_Parameter_Node);
                XmlElement ContourInfo_value_Nodes = xDoc.CreateElement("value");
                foreach (String pairContourInfo in pair.Value)
                {
                    String[] splitContourInfo = pairContourInfo.Split(new Char[] { ',' });
                    ContourInfo_value_Nodes = xDoc.CreateElement("value");
                    ContourInfo_value_Nodes.InnerText = splitContourInfo[0];
                    ContourInfo_Parameter_Node.AppendChild(ContourInfo_value_Nodes);
                }
            }

            xDoc.Save(strFileName);
        }

        // [ P. 建立 Parameters (PointList) ]
        public void Create_PointList(String strFileName, 
            //Dictionary<String, List<String>> inPointList_Point, List<String> PointList_PointValue)
            Dictionary<String, List<String>> inPointList_Point)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='PointList']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement PointList_Node = xDoc.CreateElement("parameters");
            PointList_Node.SetAttribute("name", "PointList");
            Top_Node.AppendChild(PointList_Node);
            foreach (KeyValuePair<String, List<String>> pair in inPointList_Point)
            {
                String[] split = pair.Key.Split(new Char[] { ',' });
                XmlElement PointList_Parameter_Node = xDoc.CreateElement("parameter");
                PointList_Parameter_Node = xDoc.CreateElement("parameter");
                PointList_Parameter_Node.SetAttribute("name", split[0]);
                PointList_Parameter_Node.SetAttribute("lcl", split[1]);
                PointList_Parameter_Node.SetAttribute("ucl", split[2]);
                PointList_Parameter_Node.SetAttribute("lsl", split[3]);
                PointList_Parameter_Node.SetAttribute("usl", split[4]);
                PointList_Parameter_Node.SetAttribute("algorithmid", split[5]);
                PointList_Parameter_Node.SetAttribute("Description", split[6]);
                PointList_Parameter_Node.SetAttribute("axisy", split[7]);
                PointList_Parameter_Node.SetAttribute("axisx", split[8]);
                PointList_Parameter_Node.SetAttribute("target", split[9]);
                PointList_Parameter_Node.SetAttribute("measureid", split[10]);
                PointList_Parameter_Node.SetAttribute("pointid", split[11]);
                PointList_Node.AppendChild(PointList_Parameter_Node);
                XmlElement PointList_value_Nodes = xDoc.CreateElement("value");
                foreach (String pairLimitation in pair.Value)
                {
                    String[] splitLimitation = pairLimitation.Split(new Char[] { ',' });
                    PointList_value_Nodes = xDoc.CreateElement("value");
                    PointList_value_Nodes.InnerText = splitLimitation[0];
                    PointList_Parameter_Node.AppendChild(PointList_value_Nodes);
                }
            }

            xDoc.Save(strFileName);
        }

        // [ Q. 建立 Parameters (GroupList) ]
        public void Create_GroupList(String strFileName, 
            Dictionary<String, Dictionary<String, List<String>>> inGroupListDic)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='GroupList']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement GroupList_Node = xDoc.CreateElement("parameters");
            GroupList_Node.SetAttribute("name", "GroupList");
            Top_Node.AppendChild(GroupList_Node);
            if (inGroupListDic.Count > 0)
            {
                foreach (KeyValuePair<String, Dictionary<String, List<String>>> item in inGroupListDic)
                {
                    XmlElement IndicatorRule_Sub_value = xDoc.CreateElement("parameter");
                    IndicatorRule_Sub_value.SetAttribute("name", item.Key);
                    GroupList_Node.AppendChild(IndicatorRule_Sub_value);
                }
            }
            xDoc.Save(strFileName);
        }
        
        // [ 建立每個 Group內的資訊 ]
        public void Create_GroupInfo(String strFileName, 
            Dictionary<String, Dictionary<String, List<String>>> inGroupListDic)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);

            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            if (inGroupListDic.Count > 0)
            {
                foreach (KeyValuePair<String, Dictionary<String, List<String>>> item in inGroupListDic)
                {
                    XmlElement Group_Node = xDoc.CreateElement("parameters");
                    Group_Node.SetAttribute("name", item.Key);
                    Top_Node.AppendChild(Group_Node);
                    foreach (KeyValuePair<String, List<String>> Inside in item.Value)
                    {
                        XmlElement Group_Sub_Node = xDoc.CreateElement("parameter");
                        Group_Sub_Node.SetAttribute("name", Inside.Key);
                        Group_Sub_Node.SetAttribute("groupindicatorsize", Inside.Value.Count.ToString());
                        Group_Node.AppendChild(Group_Sub_Node);
                        foreach (String myList in Inside.Value)
                        {
                            XmlElement Group_Sub_value = xDoc.CreateElement("value");
                            Group_Sub_value.InnerText = myList;
                            Group_Sub_Node.AppendChild(Group_Sub_value);
                        }
                    }
                }
            }
            xDoc.Save(strFileName);
        }
        
        // [ R. 建立 Parameters (ExpertKnowledge) ]
        public void Create_ExpertKnowledge(String strFileName, 
            Dictionary<String, List<String>> inExpertKnowledgeDic)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='ExpertKnowledge']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;


            XmlElement ExpertKnowledge_Node = xDoc.CreateElement("parameters");
            ExpertKnowledge_Node.SetAttribute("name", "ExpertKnowledge");
            Top_Node.AppendChild(ExpertKnowledge_Node);
            if (inExpertKnowledgeDic.Count > 0)
            {
                foreach (KeyValuePair<String, List<String>> item in inExpertKnowledgeDic)
                {
                    XmlElement ExpertKnowledge_Sub_Node = xDoc.CreateElement("parameter");
                    ExpertKnowledge_Sub_Node.SetAttribute("name", item.Key);
                    ExpertKnowledge_Node.AppendChild(ExpertKnowledge_Sub_Node);
                    foreach (String myList in item.Value)
                    {
                        XmlElement ExpertKnowledge_Sub_value = xDoc.CreateElement("value");
                        ExpertKnowledge_Sub_value.InnerText = myList;
                        ExpertKnowledge_Sub_Node.AppendChild(ExpertKnowledge_Sub_value);
                    }
                }
            }

            xDoc.Save(strFileName);
        }

        // [ S. 建立 Parameters (ModuleList) ]
        public void Create_ModuleList(String strFileName, 
            String isDQIx, String isDQIy, String isMDFR, String KSS_Type, String KVS_Type, String MR_Type, String GSI_Type, String NN_Type, String RI_Tpye, String isWS)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='ModuleList']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement ModuleList_Node = xDoc.CreateElement("parameters");
            ModuleList_Node.SetAttribute("name", "ModuleList");
            Top_Node.AppendChild(ModuleList_Node);
            XmlElement Module_Node1 = xDoc.CreateElement("parameter");
            Module_Node1.SetAttribute("name", "DQIx");
            Module_Node1.SetAttribute("value", isDQIx);
            ModuleList_Node.AppendChild(Module_Node1);
            XmlElement Module_Node2 = xDoc.CreateElement("parameter");
            Module_Node2.SetAttribute("name", "DQIy");
            Module_Node2.SetAttribute("value", isDQIy);
            ModuleList_Node.AppendChild(Module_Node2);
            XmlElement Module_Node3 = xDoc.CreateElement("parameter");
            Module_Node3.SetAttribute("name", "MDFR");
            Module_Node3.SetAttribute("value", isMDFR);
            ModuleList_Node.AppendChild(Module_Node3);
            XmlElement Module_Node4 = xDoc.CreateElement("parameter");
            Module_Node4.SetAttribute("name", "KSS");
            Module_Node4.SetAttribute("value", KSS_Type);
            ModuleList_Node.AppendChild(Module_Node4);
            XmlElement Module_Node5 = xDoc.CreateElement("parameter");
            Module_Node5.SetAttribute("name", "KVS");
            Module_Node5.SetAttribute("value", KVS_Type);
            ModuleList_Node.AppendChild(Module_Node5);
            XmlElement Module_Node6 = xDoc.CreateElement("parameter");
            Module_Node6.SetAttribute("name", "MR");
            Module_Node6.SetAttribute("value", MR_Type);
            ModuleList_Node.AppendChild(Module_Node6);
            XmlElement Module_Node7 = xDoc.CreateElement("parameter");
            Module_Node7.SetAttribute("name", "GSI");
            Module_Node7.SetAttribute("value", GSI_Type);
            ModuleList_Node.AppendChild(Module_Node7);
            XmlElement Module_Node8 = xDoc.CreateElement("parameter");
            Module_Node8.SetAttribute("name", "NN");
            Module_Node8.SetAttribute("value", NN_Type);
            ModuleList_Node.AppendChild(Module_Node8);
            XmlElement Module_Node9 = xDoc.CreateElement("parameter");
            Module_Node9.SetAttribute("name", "RI");
            String[] Title = RI_Tpye.Split(new Char[] { ';' });
            Module_Node9.SetAttribute("first", Title[0]);
            Module_Node9.SetAttribute("second", Title[1]);
            ModuleList_Node.AppendChild(Module_Node9);
            XmlElement Module_Node10 = xDoc.CreateElement("parameter");
            Module_Node10.SetAttribute("name", "WS");
            Module_Node10.SetAttribute("value", isWS);
            ModuleList_Node.AppendChild(Module_Node10);

            xDoc.Save(strFileName);
        }

        // [ T. 建立 Parameters (MDFR_Parameters) ]
        public void Create_MDFR_Parameters(String strFileName, 
            String inMDFREwmalamda, String inMDFREwmaWindow, String inMDFREwmaTolerance, String inMDFRVarConfidence, String inMDFRbaseSampleNum, String inMDFRRangeMultipleValue)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='MDFR_Parameters']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement MDFR_Parameters_Node = xDoc.CreateElement("parameters");
            MDFR_Parameters_Node.SetAttribute("name", "MDFR_Parameters");
            Top_Node.AppendChild(MDFR_Parameters_Node);
            //Ewmalamda
            XmlElement MDFR_Parameter_Node = xDoc.CreateElement("parameter");
            MDFR_Parameter_Node.SetAttribute("name", "Ewmalamda");
            MDFR_Parameters_Node.AppendChild(MDFR_Parameter_Node);
            XmlElement MDFR_value_Nodes = xDoc.CreateElement("value");
            MDFR_value_Nodes.InnerText = inMDFREwmalamda;
            MDFR_Parameter_Node.AppendChild(MDFR_value_Nodes);
            //EwmaWindow
            MDFR_Parameter_Node = xDoc.CreateElement("parameter");
            MDFR_Parameter_Node.SetAttribute("name", "EwmaWindow");
            MDFR_Parameters_Node.AppendChild(MDFR_Parameter_Node);
            MDFR_value_Nodes = xDoc.CreateElement("value");
            MDFR_value_Nodes.InnerText = inMDFREwmaWindow;
            MDFR_Parameter_Node.AppendChild(MDFR_value_Nodes);
            //EwmaTolerance
            MDFR_Parameter_Node = xDoc.CreateElement("parameter");
            MDFR_Parameter_Node.SetAttribute("name", "EwmaTolerance");
            MDFR_Parameters_Node.AppendChild(MDFR_Parameter_Node);
            MDFR_value_Nodes = xDoc.CreateElement("value");
            MDFR_value_Nodes.InnerText = inMDFREwmaTolerance;
            MDFR_Parameter_Node.AppendChild(MDFR_value_Nodes);
            //VarConfidence
            MDFR_Parameter_Node = xDoc.CreateElement("parameter");
            MDFR_Parameter_Node.SetAttribute("name", "VarConfidence");
            MDFR_Parameters_Node.AppendChild(MDFR_Parameter_Node);
            MDFR_value_Nodes = xDoc.CreateElement("value");
            MDFR_value_Nodes.InnerText = inMDFRVarConfidence;
            MDFR_Parameter_Node.AppendChild(MDFR_value_Nodes);
            //baseSampleNum
            MDFR_Parameter_Node = xDoc.CreateElement("parameter");
            MDFR_Parameter_Node.SetAttribute("name", "baseSampleNum");
            MDFR_Parameters_Node.AppendChild(MDFR_Parameter_Node);
            MDFR_value_Nodes = xDoc.CreateElement("value");
            MDFR_value_Nodes.InnerText = inMDFRbaseSampleNum;
            MDFR_Parameter_Node.AppendChild(MDFR_value_Nodes);
            //RangeMultipleValue
            MDFR_Parameter_Node = xDoc.CreateElement("parameter");
            MDFR_Parameter_Node.SetAttribute("name", "RangeMultipleValue");
            MDFR_Parameters_Node.AppendChild(MDFR_Parameter_Node);
            MDFR_value_Nodes = xDoc.CreateElement("value");
            MDFR_value_Nodes.InnerText = inMDFRRangeMultipleValue;
            MDFR_Parameter_Node.AppendChild(MDFR_value_Nodes);

            xDoc.Save(strFileName);
        }

        // [ U. 建立 Parameters (DQIx_Parameters) ]
        public void Create_DQIx_Parameters(String strFileName, 
            String inDQIxLambda, String inDQIxConstant, String inDQIxDQIxFilterPercentage, String inDQIxDQIxRefreshCounter)
        {

            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='DQIx_Parameters']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement DQIx_Parameters_Node = xDoc.CreateElement("parameters");
            DQIx_Parameters_Node.SetAttribute("name", "DQIx_Parameters");
            Top_Node.AppendChild(DQIx_Parameters_Node);
            //Lambda
            XmlElement DQIx_Parameter_Node = xDoc.CreateElement("parameter");
            DQIx_Parameter_Node.SetAttribute("name", "Lambda");
            DQIx_Parameter_Node.SetAttribute("value", inDQIxLambda);
            DQIx_Parameters_Node.AppendChild(DQIx_Parameter_Node);
            //Constant
            DQIx_Parameter_Node = xDoc.CreateElement("parameter");
            DQIx_Parameter_Node.SetAttribute("name", "Constant");
            DQIx_Parameter_Node.SetAttribute("value", inDQIxConstant);
            DQIx_Parameters_Node.AppendChild(DQIx_Parameter_Node);
            //DQIxFilterPercentage
            DQIx_Parameter_Node = xDoc.CreateElement("parameter");
            DQIx_Parameter_Node.SetAttribute("name", "DQIxFilterPercentage");
            DQIx_Parameter_Node.SetAttribute("value", inDQIxDQIxFilterPercentage);
            DQIx_Parameters_Node.AppendChild(DQIx_Parameter_Node);
            //DQIxRefreshCounter
            DQIx_Parameter_Node = xDoc.CreateElement("parameter");
            DQIx_Parameter_Node.SetAttribute("name", "DQIxRefreshCounter");
            DQIx_Parameter_Node.SetAttribute("value", inDQIxDQIxRefreshCounter);
            DQIx_Parameters_Node.AppendChild(DQIx_Parameter_Node);

            xDoc.Save(strFileName);
        }

        // [ V. 建立 Parameters (DQIy_Parameters) ]
        public void Create_DQIy_Parameters(String strFileName, 
            String inDQIycorralpha, String inDQIyIsMixedModel)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='DQIy_Parameters']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement DQIy_Parameters_Node = xDoc.CreateElement("parameters");
            DQIy_Parameters_Node.SetAttribute("name", "DQIy_Parameters");
            Top_Node.AppendChild(DQIy_Parameters_Node);
            //corralpha
            XmlElement DQIy_Parameter_Node = xDoc.CreateElement("parameter");
            DQIy_Parameter_Node.SetAttribute("name", "corralpha");
            DQIy_Parameter_Node.SetAttribute("value", inDQIycorralpha);
            DQIy_Parameters_Node.AppendChild(DQIy_Parameter_Node);
            DQIy_Parameter_Node = xDoc.CreateElement("parameter");
            DQIy_Parameter_Node.SetAttribute("name", "IsMixedModel");
            DQIy_Parameter_Node.SetAttribute("value", inDQIyIsMixedModel);
            DQIy_Parameters_Node.AppendChild(DQIy_Parameter_Node);

            xDoc.Save(strFileName);
        }

        // [ W. 建立 Parameters (KSS_Parameters) ]
        public void Create_KSS_Parameters(String strFileName, 
            String inKSSClusterNumber)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='KSS_Parameters']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement KSS_Parameters_Node = xDoc.CreateElement("parameters");
            KSS_Parameters_Node.SetAttribute("name", "KSS_Parameters");
            Top_Node.AppendChild(KSS_Parameters_Node);
            //ClusterNumber
            XmlElement KSS_Parameter_Node = xDoc.CreateElement("parameter");
            KSS_Parameter_Node.SetAttribute("name", "ClusterNumber");
            KSS_Parameters_Node.AppendChild(KSS_Parameter_Node);
            XmlElement KSS_value_Nodes = xDoc.CreateElement("value");
            KSS_value_Nodes.InnerText = inKSSClusterNumber;
            KSS_Parameter_Node.AppendChild(KSS_value_Nodes);

            xDoc.Save(strFileName);
        }

        // [ X. 建立 Parameters (KVS_Parameters) ]
        public void Create_KVS_Parameters(String strFileName, 
            String inKVSFin_apha, String inKVSFout_apha, String inKVSOneByOneChoose)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='KVS_Parameters']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;

            XmlElement KVS_Parameters_Node = xDoc.CreateElement("parameters");
            KVS_Parameters_Node.SetAttribute("name", "KVS_Parameters");
            Top_Node.AppendChild(KVS_Parameters_Node);
            //Fin_apha
            XmlElement KVS_Parameter_Node = xDoc.CreateElement("parameter");
            KVS_Parameter_Node.SetAttribute("name", "Fin_apha");
            KVS_Parameters_Node.AppendChild(KVS_Parameter_Node);
            XmlElement KVS_value_Nodes = xDoc.CreateElement("value");
            KVS_value_Nodes.InnerText = inKVSFin_apha;
            KVS_Parameter_Node.AppendChild(KVS_value_Nodes);
            //Fout_apha
            KVS_Parameter_Node = xDoc.CreateElement("parameter");
            KVS_Parameter_Node.SetAttribute("name", "Fout_apha");
            KVS_Parameters_Node.AppendChild(KVS_Parameter_Node);
            KVS_value_Nodes = xDoc.CreateElement("value");
            KVS_value_Nodes.InnerText = inKVSFout_apha;
            KVS_Parameter_Node.AppendChild(KVS_value_Nodes);
            //OneByOneChoose
            KVS_Parameter_Node = xDoc.CreateElement("parameter");
            KVS_Parameter_Node.SetAttribute("name", "OneByOneChoose");
            KVS_Parameters_Node.AppendChild(KVS_Parameter_Node);
            KVS_value_Nodes = xDoc.CreateElement("value");
            KVS_value_Nodes.InnerText = inKVSOneByOneChoose;
            KVS_Parameter_Node.AppendChild(KVS_value_Nodes);

            xDoc.Save(strFileName);
        }

        // [ Y. 建立 Parameters (BPNN_Parameters) ]
        public void Create_BPNN_Parameters(String strFileName,
            String inBPNNEpochsRange1, String inBPNNEpochsRange2, String inBPNNEpochsRange3, String inBPNNMomTermRange1, String inBPNNMomTermRange2,
            String inBPNNMomTermRange3, String inBPNNAlphaRange1, String inBPNNAlphaRange2, String inBPNNAlphaRange3, String inBPNNNodesRange1,
            String inBPNNNodesRange2, String inBPNNNodesRange3, String inBPNNOneByOneChoose, String inBPNNBPNNRefreshCounter, List<String> inBPNNNodesRange)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='BPNN_Parameters']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;
            
            XmlElement BPNN_Parameters_Node = xDoc.CreateElement("parameters");
            BPNN_Parameters_Node.SetAttribute("name", "BPNN_Parameters");
            Top_Node.AppendChild(BPNN_Parameters_Node);
            //EpochsRange
            XmlElement BPNN_Parameter_Node = xDoc.CreateElement("parameter");
            BPNN_Parameter_Node.SetAttribute("name", "EpochsRange");
            BPNN_Parameters_Node.AppendChild(BPNN_Parameter_Node);
            XmlElement BPNN_value_Nodes = xDoc.CreateElement("value");
            BPNN_value_Nodes.InnerText = inBPNNEpochsRange1;
            BPNN_Parameter_Node.AppendChild(BPNN_value_Nodes);
            BPNN_value_Nodes = xDoc.CreateElement("value");
            BPNN_value_Nodes.InnerText = inBPNNEpochsRange2;
            BPNN_Parameter_Node.AppendChild(BPNN_value_Nodes);
            BPNN_value_Nodes = xDoc.CreateElement("value");
            BPNN_value_Nodes.InnerText = inBPNNEpochsRange3;
            BPNN_Parameter_Node.AppendChild(BPNN_value_Nodes);
            //MomTermRange
            BPNN_Parameter_Node = xDoc.CreateElement("parameter");
            BPNN_Parameter_Node.SetAttribute("name", "MomTermRange");
            BPNN_Parameters_Node.AppendChild(BPNN_Parameter_Node);
            BPNN_value_Nodes = xDoc.CreateElement("value");
            BPNN_value_Nodes.InnerText = inBPNNMomTermRange1;
            BPNN_Parameter_Node.AppendChild(BPNN_value_Nodes);
            BPNN_value_Nodes = xDoc.CreateElement("value");
            BPNN_value_Nodes.InnerText = inBPNNMomTermRange2;
            BPNN_Parameter_Node.AppendChild(BPNN_value_Nodes);
            BPNN_value_Nodes = xDoc.CreateElement("value");
            BPNN_value_Nodes.InnerText = inBPNNMomTermRange3;
            BPNN_Parameter_Node.AppendChild(BPNN_value_Nodes);
            //AlphaRange
            BPNN_Parameter_Node = xDoc.CreateElement("parameter");
            BPNN_Parameter_Node.SetAttribute("name", "AlphaRange");
            BPNN_Parameters_Node.AppendChild(BPNN_Parameter_Node);
            BPNN_value_Nodes = xDoc.CreateElement("value");
            BPNN_value_Nodes.InnerText = inBPNNAlphaRange1;
            BPNN_Parameter_Node.AppendChild(BPNN_value_Nodes);
            BPNN_value_Nodes = xDoc.CreateElement("value");
            BPNN_value_Nodes.InnerText = inBPNNAlphaRange2;
            BPNN_Parameter_Node.AppendChild(BPNN_value_Nodes);
            BPNN_value_Nodes = xDoc.CreateElement("value");
            BPNN_value_Nodes.InnerText = inBPNNAlphaRange3;
            BPNN_Parameter_Node.AppendChild(BPNN_value_Nodes);
            //NodesRange
            BPNN_Parameter_Node = xDoc.CreateElement("parameter");
            BPNN_Parameter_Node.SetAttribute("name", "NodesRange");
            BPNN_Parameters_Node.AppendChild(BPNN_Parameter_Node);

            foreach (String strNode in inBPNNNodesRange)
            {
                BPNN_value_Nodes = xDoc.CreateElement("value");
                BPNN_value_Nodes.InnerText = strNode;
                BPNN_Parameter_Node.AppendChild(BPNN_value_Nodes);
            }
            
            //OneByOneChoose
            BPNN_Parameter_Node = xDoc.CreateElement("parameter");
            BPNN_Parameter_Node.SetAttribute("name", "OneByOneChoose");
            BPNN_Parameters_Node.AppendChild(BPNN_Parameter_Node);
            BPNN_value_Nodes = xDoc.CreateElement("value");
            BPNN_value_Nodes.InnerText = inBPNNOneByOneChoose;
            BPNN_Parameter_Node.AppendChild(BPNN_value_Nodes);
            //BPNNRefreshCounter
            BPNN_Parameter_Node = xDoc.CreateElement("parameter");
            BPNN_Parameter_Node.SetAttribute("name", "BPNNRefreshCounter");
            BPNN_Parameters_Node.AppendChild(BPNN_Parameter_Node);
            BPNN_value_Nodes = xDoc.CreateElement("value");
            BPNN_value_Nodes.InnerText = inBPNNBPNNRefreshCounter;
            BPNN_Parameter_Node.AppendChild(BPNN_value_Nodes);

            xDoc.Save(strFileName);
        }

        // [ Z. 建立 Parameters (RI_Parameters) ]
        public void Create_RI_Parameters(String strFileName, 
            List<String> inRITolerantMaxError)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='RI_Parameters']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;


            XmlElement RI_Parameters_Node = xDoc.CreateElement("parameters");
            RI_Parameters_Node.SetAttribute("name", "RI_Parameters");
            Top_Node.AppendChild(RI_Parameters_Node);
            //FirstAlgo
            XmlElement RI_Parameter_Node = xDoc.CreateElement("parameter");
            RI_Parameter_Node.SetAttribute("name", "FirstAlgo");
            RI_Parameter_Node.SetAttribute("Algoname", "MRConjectureHistory");
            RI_Parameters_Node.AppendChild(RI_Parameter_Node);
            //SecondAlgo
            RI_Parameter_Node = xDoc.CreateElement("parameter");
            RI_Parameter_Node.SetAttribute("name", "SecondAlgo");
            RI_Parameter_Node.SetAttribute("Algoname", "BPNNConjectureHistory");
            RI_Parameters_Node.AppendChild(RI_Parameter_Node);
            //SelectCalculator
            RI_Parameter_Node = xDoc.CreateElement("parameter");
            RI_Parameter_Node.SetAttribute("name", "SelectCalculator");
            RI_Parameter_Node.SetAttribute("value", "ThresholdUserSetting");
            RI_Parameters_Node.AppendChild(RI_Parameter_Node);
            //TolerantMaxError
            RI_Parameter_Node = xDoc.CreateElement("parameter");
            RI_Parameter_Node.SetAttribute("name", "TolerantMaxError");
            RI_Parameters_Node.AppendChild(RI_Parameter_Node);
            XmlElement RI_value_Nodes = null;
            //跑Group迴圈建立
            int iGroupDount = 0;
            foreach (String strGroupTolerantMaxError in inRITolerantMaxError)
            {
                iGroupDount++;
                RI_value_Nodes = xDoc.CreateElement("value");
                RI_value_Nodes.SetAttribute("name", "Group" + iGroupDount.ToString());
                RI_value_Nodes.InnerText = strGroupTolerantMaxError;
                RI_Parameter_Node.AppendChild(RI_value_Nodes);
            }

            xDoc.Save(strFileName);
        }

        // [ AA. 建立 Parameters (MR_Parameters) ]
        public void Create_MR_Parameters(String strFileName, 
            String inMR_TSVD_Condition_Number_Criteria, String inMR_TSVD_Energy_Ratio_Criteria, String inMRRefreshCounter)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='MR_Parameters']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;


            XmlElement MR_Parameters_Node = xDoc.CreateElement("parameters");
            MR_Parameters_Node.SetAttribute("name", "MR_Parameters");
            Top_Node.AppendChild(MR_Parameters_Node);
            //MR_TSVD_Condition_Number_Criteria
            XmlElement MR_Parameter_Node = xDoc.CreateElement("parameter");
            MR_Parameter_Node.SetAttribute("name", "MR_TSVD_Condition_Number_Criteria");
            MR_Parameters_Node.AppendChild(MR_Parameter_Node);
            XmlElement MR_value_Nodes = xDoc.CreateElement("value");
            MR_value_Nodes.InnerText = inMR_TSVD_Condition_Number_Criteria;
            MR_Parameter_Node.AppendChild(MR_value_Nodes);
            //MR_TSVD_Energy_Ratio_Criteria
            MR_Parameter_Node = xDoc.CreateElement("parameter");
            MR_Parameter_Node.SetAttribute("name", "MR_TSVD_Energy_Ratio_Criteria");
            MR_Parameters_Node.AppendChild(MR_Parameter_Node);
            MR_value_Nodes = xDoc.CreateElement("value");
            MR_value_Nodes.InnerText = inMR_TSVD_Energy_Ratio_Criteria;
            MR_Parameter_Node.AppendChild(MR_value_Nodes);
            //MRRefreshCounter
            MR_Parameter_Node = xDoc.CreateElement("parameter");
            MR_Parameter_Node.SetAttribute("name", "MRRefreshCounter");
            MR_Parameters_Node.AppendChild(MR_Parameter_Node);
            MR_value_Nodes = xDoc.CreateElement("value");
            MR_value_Nodes.InnerText = inMRRefreshCounter;
            MR_Parameter_Node.AppendChild(MR_value_Nodes);


            xDoc.Save(strFileName);
        }

        // [ AB. 建立 Parameters (GSI_Parameters) ]
        public void Create_GSI_Parameters(String strFileName, 
            String inGSIRefreshCounter, String inGSI_TSVD_Condition_Number_Criteria, String inGSI_TSVD_Energy_Ratio_Criteria, List<String> inGSIRT, List<String> inGSIThreshold)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='GSI_Parameters']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;


            XmlElement GSI_Parameters_Node = xDoc.CreateElement("parameters");
            GSI_Parameters_Node.SetAttribute("name", "GSI_Parameters");
            Top_Node.AppendChild(GSI_Parameters_Node);
            //GSI_RT
            XmlElement GSI_Parameter_Node = xDoc.CreateElement("parameter");
            GSI_Parameter_Node.SetAttribute("name", "GSI_RT");
            GSI_Parameters_Node.AppendChild(GSI_Parameter_Node);
            XmlElement GSI_value_Nodes = null;
            //跑Group迴圈建立
            foreach (String strGSIRT in inGSIRT)
            {
                GSI_value_Nodes = xDoc.CreateElement("value");
                GSI_value_Nodes.InnerText = strGSIRT;
                GSI_Parameter_Node.AppendChild(GSI_value_Nodes);
            }
            //GSI_Threshold
            GSI_Parameter_Node = xDoc.CreateElement("parameter");
            GSI_Parameter_Node.SetAttribute("name", "GSI_Threshold");
            GSI_Parameters_Node.AppendChild(GSI_Parameter_Node);
            GSI_value_Nodes = null;
            //跑Group迴圈建立
            foreach (String strGSIThreshold in inGSIThreshold)
            {
                GSI_value_Nodes = xDoc.CreateElement("value");
                GSI_value_Nodes.InnerText = strGSIThreshold;
                GSI_Parameter_Node.AppendChild(GSI_value_Nodes);
            }
            //RefreshCounter
            GSI_Parameter_Node = xDoc.CreateElement("parameter");
            GSI_Parameter_Node.SetAttribute("name", "RefreshCounter");
            GSI_Parameters_Node.AppendChild(GSI_Parameter_Node);
            GSI_value_Nodes = xDoc.CreateElement("value");
            GSI_value_Nodes.InnerText = inGSIRefreshCounter;
            GSI_Parameter_Node.AppendChild(GSI_value_Nodes);
            //GSI_TSVD_Condition_Number_Criteria
            GSI_Parameter_Node = xDoc.CreateElement("parameter");
            GSI_Parameter_Node.SetAttribute("name", "GSI_TSVD_Condition_Number_Criteria");
            GSI_Parameters_Node.AppendChild(GSI_Parameter_Node);
            GSI_value_Nodes = xDoc.CreateElement("value");
            GSI_value_Nodes.InnerText = inGSI_TSVD_Condition_Number_Criteria;
            GSI_Parameter_Node.AppendChild(GSI_value_Nodes);
            //GSI_TSVD_Energy_Ratio_Criteria
            GSI_Parameter_Node = xDoc.CreateElement("parameter");
            GSI_Parameter_Node.SetAttribute("name", "GSI_TSVD_Energy_Ratio_Criteria");
            GSI_Parameters_Node.AppendChild(GSI_Parameter_Node);
            GSI_value_Nodes = xDoc.CreateElement("value");
            GSI_value_Nodes.InnerText = inGSI_TSVD_Energy_Ratio_Criteria;
            GSI_Parameter_Node.AppendChild(GSI_value_Nodes);


            xDoc.Save(strFileName);
        }

        // [ AC. 建立 Parameters (AlgorithmPreference) ]
        public void Create_AlgorithmPreference(String strFileName, 
            String inAlgorithmPreferencePreferredVMOutput)
        {
            // [ 建立 XML文件 ]
            xDoc = new XmlDocument();
            xDoc.Load(strFileName);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", xDoc.DocumentElement.NamespaceURI);


            XmlNode CheckTop_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (CheckTop_Node == null) return;

            //////////////////////////////////////////////////////////////////////////
            //檢查是否已存在此節點
            XmlNode Check_Node = xDoc.SelectSingleNode("ns:message/parameters[@name='AlgorithmPreference']", manager);//選擇節點
            if (Check_Node != null)
            {
                CheckTop_Node.RemoveChild(Check_Node);
                xDoc.Save(strFileName);
                xDoc.Load(strFileName);
            }
			
			
            XmlNode Top_Node = xDoc.SelectSingleNode("ns:message", manager);//選擇節點
            if (Top_Node == null) return;


            XmlElement AlgorithmPreference_Parameters_Node = xDoc.CreateElement("parameters");
            AlgorithmPreference_Parameters_Node.SetAttribute("name", "AlgorithmPreference");
            Top_Node.AppendChild(AlgorithmPreference_Parameters_Node);
            XmlElement AlgorithmPreference_Parameter_Node = xDoc.CreateElement("parameter");
            AlgorithmPreference_Parameter_Node.SetAttribute("name", "PreferredVMOutput");
            AlgorithmPreference_Parameters_Node.AppendChild(AlgorithmPreference_Parameter_Node);
            XmlElement AlgorithmPreference_value_Nodes = xDoc.CreateElement("value");
            AlgorithmPreference_value_Nodes.InnerText = inAlgorithmPreferencePreferredVMOutput;
            AlgorithmPreference_Parameter_Node.AppendChild(AlgorithmPreference_value_Nodes);


            xDoc.Save(strFileName);
        }



    }
}
