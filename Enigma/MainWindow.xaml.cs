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

namespace Enigma
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Configure Settings;
        public MainWindow()
        {
            InitializeComponent();
            Settings = new Configure();
            Settings.Show();
        }

        public MainWindow(bool modified)
        {
            ReturnDefault();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString().Length == 1 && Input.Text.ToString().Length < 128)
            {
                if ((int)e.Key.ToString()[0] >= 65 && (int)e.Key.ToString()[0] <= 90)
                {
                    Input.Text += e.Key.ToString(); 
                    Output.Text += Settings.Encrypt(e.Key.ToString()[0]) + ""; 
                    Mirrored.Text += Settings.Mirror(e.Key.ToString()[0]) + ""; 

                    if (Settings.rotor)
                    {
                        Settings.Rotate(true);
                        Settings.DisplayRings();
                    }
                }
            }

            else if (e.Key == Key.Space)
            {
                Input.Text += " ";
                Output.Text += " ";
                Mirrored.Text += " ";
            }
            // Handle backspace key
            else if (e.Key == Key.Back)
            {
                Settings.Rotate(false); // Rotate rotors backward
                Settings.DisplayRings();
                Input.Text = RemoveLastLetter(Input.Text.ToString()); // Remove last character
                Output.Text = RemoveLastLetter(Output.Text.ToString());
                Mirrored.Text = RemoveLastLetter(Mirrored.Text.ToString());
            }
        }
        private string RemoveLastLetter(string word)
        {
            string newWord = "";
            for (int x = 0; x < word.Length - 1; x++)
                newWord += word[x];
            return newWord;
        }
        public void ReturnDefault()
        {
            Input.Text = "";
            Output.Text = "";
            Mirrored.Text = "";
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}

