using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace pr25_pr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        ObservableCollection<Reminder> reminders = new ObservableCollection<Reminder>();
        DispatcherTimer timer = new DispatcherTimer();


        public MainWindow()
        {
            InitializeComponent();
            
            timer.Interval = TimeSpan.FromSeconds(55);
            timer.Tick += Timer_Tick;

            RemindersGrid.ItemsSource = reminders;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            int h = DateTime.Now.Hour;
            int m = DateTime.Now.Minute;
            foreach(Reminder r in reminders)
            {
                if (r.Hour == h && r.Minute == m && r.IsShown == false) 
                {
                    MessageBox.Show($"{r.Hour}:{r.Minute} - {r.Text}",
                        "Напоминание",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    r.IsShown = true;
                    RemindersGrid.ItemsSource = null;
                    RemindersGrid.ItemsSource = reminders;
                }
                
            }
            

        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (dlg.ShowDialog() == true)
            {
                string path = dlg.FileName;
                string[] temp = File.ReadAllLines(path);

                foreach (string s in temp) 
                {
                    string[] t = s.Split("%%");
                    reminders.Add(new Reminder
                    {
                        Hour = Convert.ToInt32(t[0]), Minute = Convert.ToInt32(t[1]),
                        Text = t[2], IsShown = Convert.ToBoolean(t[3])
                    });
                }
                RemindersGrid.SelectedItem = reminders;
                timer.Start();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Текстовые файлы (*.txt)|*.txt";

            if (dlg.ShowDialog() == true)
            {
                using (StreamWriter writer = new StreamWriter(dlg.FileName)) 
                {
                    foreach (Reminder r in reminders) 
                    {
                        writer.WriteLine(
                            $"{r.Hour}%%{r.Minute}%%{r.Text}%%{r.IsShown}");
                    }
                }

                MessageBox.Show("Файл сохранён!");
            }

        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            reminders.Add(new Reminder()
            {
                Hour = Convert.ToInt32(txtHour.Text),
                Minute = Convert.ToInt32 (txtMin.Text),
                Text = txtText.Text,
                IsShown = false
            });
            timer.Start();
        }
        private void Del_Click(object sender, RoutedEventArgs e)
        {
            if (RemindersGrid.SelectedItem is Reminder r) 
            {
                reminders.Remove(r); 
            }
        }
    }
}