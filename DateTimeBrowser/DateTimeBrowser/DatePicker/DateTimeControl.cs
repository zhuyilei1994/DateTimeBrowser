using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DateTimeBrowser.DatePicker
{
    public class DateTimeControl: System.Windows.Controls.DatePicker
    {
        private TextBox textBox1, textBox2, textBox3, currentTextBox;
        private TextBlock textBlock1, textBlock2;

        public DateTimeControl()
        {
            textBlock1 = new TextBlock();
            textBlock1.Text = "/";
            textBlock2 = new TextBlock();
            textBlock2.Text = "/";
            textBox1 = new TextBox() { Name = "Y" };
            textBox1.IsReadOnlyCaretVisible = false;
            textBox1.GotFocus += TextBox_GotFocus;
            textBox1.LostFocus += TextBox_LostFocus;
            textBox2 = new TextBox() { Name = "M" };
            textBox2.IsReadOnlyCaretVisible = false;
            textBox2.GotFocus += TextBox_GotFocus;
            textBox2.LostFocus += TextBox_LostFocus;
            textBox3 = new TextBox() { Name = "D" };
            textBox3.IsReadOnlyCaretVisible = false;
            textBox3.GotFocus += TextBox_GotFocus;
            textBox3.LostFocus += TextBox_LostFocus;

            textBox1.BorderThickness = new Thickness(0);
            textBox1.IsReadOnly = true;
            textBox2.BorderThickness = new Thickness(0);
            textBox2.IsReadOnly = true;
            textBox3.BorderThickness = new Thickness(0);
            textBox3.IsReadOnly = true;

            textBox1.KeyDown += TextBox_KeyDown;
            textBox2.KeyDown += TextBox_KeyDown;
            textBox3.KeyDown += TextBox_KeyDown;

            SelectedDateChanged += DatePickerClass_SelectedDateChanged;
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if ((int)e.Key >= 34 && (int)e.Key <= 43)
            {
                var tb = (TextBox)sender;
                if (tb.Name == "M")
                {
                    var month = (currentTextBox.Text.Length >= 2 ? "" : currentTextBox.Text) +
                                ((int)e.Key - 34).ToString();
                    int.TryParse(month, out int preNumResult); 
                    if (preNumResult > 12)
                    {
                        currentTextBox.Text = Math.Max(((int)e.Key - 34), 1).ToString();
                    }
                    else
                    {

                        preNumResult = Math.Max(1, preNumResult);
                        currentTextBox.Text = preNumResult.ToString();
                    }

                }

                else if (tb.Name == "D")
                {
                    var day = (currentTextBox.Text.Length >= 2 ? "" : currentTextBox.Text) +
                              ((int)e.Key - 34).ToString();
                    int.TryParse(day, out int preNumResult);
                    if (CheckDate(int.Parse(textBox1.Text), int.Parse(textBox2.Text), preNumResult))
                    {
                        preNumResult = Math.Max(preNumResult, 1);
                        currentTextBox.Text = preNumResult.ToString();
                    }
                    else
                    {
                        currentTextBox.Text = Math.Max(((int)e.Key - 34), 1).ToString();
                    }
                }
                else
                {
                    var year = (currentTextBox.Text.Length == 4 ? "" : currentTextBox.Text) + ((int)e.Key - 34);
                    currentTextBox.Text = year;
                }

            }

            SelectedDate = new DateTime(int.Parse(textBox1.Text), int.Parse(textBox2.Text), int.Parse(textBox3.Text));
        }

        bool CheckDate(int year, int month, int date)
        {
            List<int> Month_buf = new List<int>() { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            if (month == 2)
            {
                if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0))
                    Month_buf[1] += 1;
            }

            if (month > 12 || month < 1 || date > Month_buf[month - 1] || date < 1)
                return false;

            return true;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078d7"));
            textBox.Foreground = new SolidColorBrush(Colors.White);
            currentTextBox = textBox;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Name == "Y")
            {
                int.TryParse(textBox.Text, out int year);
                if (year < 1900) textBox.Text = DateTime.Now.Year.ToString();
            }
            textBox.Background = new SolidColorBrush(Colors.Transparent);
            textBox.Foreground = new SolidColorBrush(Colors.Black);
            int.TryParse(currentTextBox.Text, out int numResult);
            currentTextBox.Text = numResult.ToString("00");
        }

        private void DatePickerClass_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedDate != null)
            {
                textBox1.Text = SelectedDate.Value.Year.ToString();
                textBox2.Text = SelectedDate.Value.Month.ToString();
                textBox3.Text = SelectedDate.Value.Day.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //SetDateFormat();
            if (SelectedDate == null) SelectedDate = DateTime.Now;
            DatePickerTextBox box = base.GetTemplateChild("PART_TextBox") as DatePickerTextBox;
            box.Visibility = Visibility.Hidden;
            box.Text = SelectedDate.ToString();
            box.ApplyTemplate();

            var grid = base.GetTemplateChild("PART_Root") as Grid;
            textBox1.Text = SelectedDate.Value.Year.ToString();
            textBox2.Text = SelectedDate.Value.Month.ToString();
            textBox3.Text = SelectedDate.Value.Day.ToString();
            var stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.Children.Add(textBox1);
            stackPanel.Children.Add(textBlock1);
            stackPanel.Children.Add(textBox2);
            stackPanel.Children.Add(textBlock2);
            stackPanel.Children.Add(textBox3);

            grid.Children.Add(stackPanel);
            grid.ApplyTemplate();

            ContentControl watermark = box.Template.FindName("PART_Watermark", box) as ContentControl;
            string datePattern = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;
            watermark.Content = datePattern.ToUpper();
        }
    }
}
