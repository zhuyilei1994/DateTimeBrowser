using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DateTimeBrowser.DatePicker
{
    /// <summary>
    /// DatePickerView.xaml 的交互逻辑
    /// </summary>
    public partial class DatePickerView : UserControl
    {
        public static readonly DependencyProperty DPFormatProperty = DependencyProperty.Register("DPFormat", typeof(string), typeof(DateTimeControl));

        public static readonly DependencyProperty SeparatorProperty = DependencyProperty.Register("Seperator", typeof(string), typeof(DateTimeControl));

        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(Nullable<DateTime>),
            typeof(DatePickerView), new FrameworkPropertyMetadata(new Nullable<DateTime>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnSelectedDateChanged)));

        public string DPFormat
        {
            get { return this.GetValue(DPFormatProperty) as string; }
            set { this.SetValue(DPFormatProperty, value); }
        }

        public string Seperator
        {
            get { return this.GetValue(SeparatorProperty) as string; }
            set { this.SetValue(SeparatorProperty, value); }
        }

        public DateTime? SelectedDate
        {
            get { return this.GetValue(SelectedDateProperty) as Nullable<DateTime>; }
            set { this.SetValue(SelectedDateProperty, value); }
        }

        public DatePickerView()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DatePickerView;
            if (control != null)
                control.CustomDatePicker.SelectedDate = control.SelectedDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var t = sender as System.Windows.Controls.DatePicker;
            if (!t.SelectedDate.HasValue)
                t.DisplayDate = DateTime.Now;

            if (t.SelectedDate.HasValue)
            {
                if (CustomDatePicker.SelectedDate.HasValue)
                {
                    BindingExpression bindingExpression = BindingOperations.GetBindingExpression(CustomDatePicker, System.Windows.Controls.DatePicker.SelectedDateProperty);
                    bindingExpression = BindingOperations.GetBindingExpression(CustomDatePicker, System.Windows.Controls.DatePicker.SelectedDateProperty);
                    BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(CustomDatePicker, System.Windows.Controls.DatePicker.SelectedDateProperty);
                    Validation.ClearInvalid(bindingExpressionBase);
                }
            }
        }

        /// <summary>
        /// Method to restrict user input for dates less than 1900 year.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomDatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            var t = sender as System.Windows.Controls.DatePicker;

            if (t.SelectedDate.HasValue && t.SelectedDate.Value.Year < 1900)
            {
               // MessageBox.Show("Please select a valid date.");
                t.SelectedDate = null;
                t.DisplayDate = DateTime.Now;
            }
        }
    }
}
