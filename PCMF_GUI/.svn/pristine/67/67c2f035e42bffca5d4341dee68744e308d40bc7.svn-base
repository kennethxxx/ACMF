using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using IPS.Views;

namespace IPS.ViewsComponent
{
    public partial class DateTimePicker : UserControl
    {
        public DateTimePicker()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty DisplayDataProperty =
           DependencyProperty.Register("DisplayData", typeof(DateTime), typeof(DateTimePicker), new PropertyMetadata(new PropertyChangedCallback(DisplayDataPropertyCallBack)));
        /// <summary>
        /// 显示的日期及时间
        /// </summary>
        public DateTime DisplayData
        {
            get
            {
                return (DateTime)this.GetValue(DisplayDataProperty);
            }
            set
            {
                SetValue(DisplayDataProperty, value);
            }
        }

        public static void DisplayDataPropertyCallBack(object sender, DependencyPropertyChangedEventArgs args)
        {
            //属性设置的回调事件用于设置显示的值
            DateTimePicker sel = sender as DateTimePicker;
            if (sel != null)
            {
                if (args.NewValue != null)
                {
                    DateTime dt;
                    if (DateTime.TryParse(args.NewValue.ToString(), out dt))
                    {
                        //--设置显示的日期及时间
                        sel.SetDataTime(dt);
                    }
                }
            }
        }
        /// <summary>
        /// 是否正在加载数据
        /// </summary>
        bool isLoaded = false;
        private void SetDataTime(DateTime datatime)
        {
            isLoaded = false;

            dpMain.Text = datatime.ToString();
            tpMain.Value = datatime;
			
            dpMain.SelectedDateChanged -= new EventHandler<SelectionChangedEventArgs>(dpMain_SelectedDateChanged);
            dpMain.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(dpMain_SelectedDateChanged);

            tpMain.ValueChanged -= new RoutedPropertyChangedEventHandler<DateTime?>(tpMain_ValueChanged);
            tpMain.ValueChanged += new RoutedPropertyChangedEventHandler<DateTime?>(tpMain_ValueChanged);

            isLoaded = true;
        }
        /// <summary>
        /// 日期的更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dpMain_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isLoaded == false) return;
            //--修改当前的日期
            int hour = DisplayData.Hour;
            int minute = DisplayData.Minute;
            int second = DisplayData.Second;
            if (dpMain.SelectedDate != null)
            {
                //--如果时间部分为0则需要设置时间部分
                if (dpMain.SelectedDate.Value.Hour == 0 && dpMain.SelectedDate.Value.Minute == 0)
                {
                    DateTime datetime = dpMain.SelectedDate.Value.AddHours(hour).AddMinutes(minute).AddSeconds(second);

                    DisplayData = datetime;
                }
                else
                    DisplayData = dpMain.SelectedDate.Value;
            }
        }
        /// <summary>
        /// 时间的更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tpMain_ValueChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (isLoaded == false) return;

            int year = DisplayData.Year;
            int month = DisplayData.Month;
            int day = DisplayData.Day;

            if (tpMain.Value != null)
            {
                string dt = year.ToString() + "/"
                    + month.ToString() + "/"
                    + day.ToString() + " "
                    + tpMain.Value.Value.Hour.ToString() + ":"
                    + tpMain.Value.Value.Minute.ToString() + ":"
                    + tpMain.Value.Value.Second.ToString();
                DateTime d = Convert.ToDateTime(dt);
                DisplayData = d;
            }
        }
    }
}