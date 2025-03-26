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
using System.Windows.Shapes;

namespace Enigma
{
    /// <summary>
    /// Interaction logic for Configure.xaml
    /// </summary>
    public partial class Configure : Window
    {
        string control = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string ring1 = "DMTWSILRUYQNKFEJCAZBPGXOHV";
        string ring2 = "HQZGPJTMOBLNCIFDYAWVEUSRKX";
        string ring3 = "UQNTLSZFMREHDPXKIBVYGJCWOA";
        string reflector = "YRUHQSLDPXNGOKMIEBFZCWVJAT";
        int[] _keyOffset = { 0, 0, 0 };
        int[] _initOffset = { 0, 0, 0 };
        public bool rotor = false;
        Dictionary<char, char> plugboard = new Dictionary<char, char>();
        public bool plugboardSet = false;
        private bool _plugboardSet;
        MainWindow Window;
        public Configure()
        {
            InitializeComponent();
            SetDefaults();

            LockRotors.IsEnabled = false;
            rotor = false;
        }
        public void DisplayRings()
        {
            DisplayRing(lblRotor1, ring1);
            DisplayRing(lblRotor2, ring2);
            DisplayRing(lblRotor3, ring3);
        }

        public void DisplayRing(Label displayLabel, string ring)
        {
            string temp = "";
            foreach (char r in ring)
                temp += r + "\t"; // Add tab for spacing
            displayLabel.Content = temp;
        }
        private void SetDefaults()
        {
          control = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            ring1 = "DMTWSILRUYQNKFEJCAZBPGXOHV";
            ring2 = "HQZGPJTMOBLNCIFDYAWVEUSRKX";
            ring3 = "UQNTLSZFMREHDPXKIBVYGJCWOA";
 string reflector = "YRUHQSLDPXNGOKMIEBFZCWVJAT";
            _keyOffset = new int[] { 0, 0, 0 };
            _plugboardSet = false;

            DisplayRing(lblControlRing, control);
            DisplayRing(lblRotor1, ring1);
            DisplayRing(lblRotor2, ring2);
            DisplayRing(lblRotor3, ring3);
            DisplayRing(Reflector, reflector);
        }
        private string InitializeRotors(int initial, string ring)
        {
            string newRing = ring;
            for (int x = 0; x < initial; x++)
                newRing = MoveValues(true, newRing);
            return newRing;
        }
        private string MoveValues(bool forward, string ring)
        {
            char movingValue = ' ';
            string newRing = "";

            if (forward)
            {
                movingValue = ring[0];
                for (int x = 1; x < ring.Length; x++)
                    newRing += ring[x];
                newRing += movingValue;
            }
            else
            {
                movingValue = ring[25];
                for (int x = 0; x < ring.Length - 1; x++)
                    newRing += ring[x];
                newRing = movingValue + newRing;
            }

            return newRing;
        }
        private void btnRotor_Click(object sender, RoutedEventArgs e)
        {
            SetDefaults();

            if (int.TryParse(Rotor1Modify.Text, out _initOffset[0]) &&
                int.TryParse(Rotor2Modify.Text, out _initOffset[1]) &&
                int.TryParse(Rotor3Modify.Text, out _initOffset[2]))
            {
                if (_initOffset[0] >= 0 && _initOffset[0] <= 25 &&
                    _initOffset[1] >= 0 && _initOffset[1] <= 25 &&
                    _initOffset[2] >= 0 && _initOffset[2] <= 25)
                {
                    Rotor1Modify.IsEnabled = false;
                    Rotor2Modify.IsEnabled = false;
                    Rotor3Modify.IsEnabled = false;
                    rotor = true;
                    LockRotors.IsEnabled = false;
                    

                    ring1 = InitializeRotors(_initOffset[0], ring1);
                    ring2 = InitializeRotors(_initOffset[1], ring2);
                    ring3 = InitializeRotors(_initOffset[2], ring3);

                    DisplayRing(lblRotor1, ring1);
                    DisplayRing(lblRotor2, ring2);
                    DisplayRing(lblRotor3, ring3);
                    DisplayOffset();
                }
            }
            else
            {
                MessageBox.Show("A field has invalid characters!");
            }
        }
        private void DisplayOffset()
        {
            Rotor1Modify.Text = _keyOffset[0] + "";
            Rotor2Modify.Text = _keyOffset[1] + "";
            Rotor3Modify.Text = _keyOffset[2] + "";
        }
        private void btnSetPlugboard_Click(object sender, RoutedEventArgs e)
        {
            if (!txtPlugboardChecker())
            {
                MessageBox.Show("Field has invalid characters!");
                return;
            }

            if (_plugboardSet)
            {
                MessageBox.Show("Plugboard is already set.");
                return;
            }

            SetupPlugboard(PBModify.Text);
            _plugboardSet = true;
            PBEnable.IsEnabled = false;
            LockRotors.IsEnabled = true; // Enable the rotor button.

            // Explicitly check the flag and perform an action
            if (_plugboardSet)
            {
                PBModify.IsEnabled = false;
                MessageBox.Show("Plugboard has been set!");
            }
        }

        private void SetupPlugboard(string plugboardSettings)
        {
            plugboard.Clear();
            string[] pairs = plugboardSettings.ToUpper().Split(' ');
            foreach (string pair in pairs)
            {
                if (pair.Length == 2)
                {
                    plugboard[pair[0]] = pair[1];
                    plugboard[pair[1]] = pair[0];
                }
            }
        }
        private bool txtPlugboardChecker()
        {
            int charCounter = 0;
            string inputField = PBModify.Text.ToString();
            List<char> temp = new List<char>();

            for (int x = 0; x < inputField.Length; x++)
            {
                if ((inputField[x] >= 33 && inputField[x] <= 64) || (inputField[x] >= 91 && inputField[x] <= 96)
                    || (inputField[x] >= 123 && inputField[x] <= 127))
                    return false;

                if (inputField[x] == ' ')
                {
                    charCounter = 0;
                    temp.Clear();
                    continue;
                }

                charCounter++;

                if (charCounter >= 3)
                {
                    return false;
                }
                else
                {
                    temp.Add(inputField[x]);

                    if (temp.Count == 2)
                    {
                        if (temp[0] == temp[1])
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        private int IndexSearch(string rotor, char letter)
        {
            int index = 0;
            for (int x = 0; x < rotor.Length; x++)
            {
                if (rotor[x] == letter)
                {
                    index = x;
                    continue;
                }
            }
            return index;
        }
        public char Encrypt(char letter)
        {
            char newChar = letter;

            if (plugboard.ContainsKey(newChar))
                newChar = plugboard[newChar];
            else if (plugboard.ContainsValue(newChar))
                newChar = plugboard.FirstOrDefault(x => x.Value == newChar).Key;

            // Rotor pass forward
            newChar = ring1[IndexSearch(control, newChar)];
            newChar = ring2[IndexSearch(control, newChar)];
            newChar = ring3[IndexSearch(control, newChar)];

            // Reflector pass
            newChar = reflector[IndexSearch(control, newChar)];

            // Rotor pass backward
            newChar = ring3[IndexSearch(control, newChar)];
            newChar = ring2[IndexSearch(control, newChar)];
            newChar = ring1[IndexSearch(control, newChar)];

            // Plugboard pass (after rotors)
            if (plugboard.ContainsKey(newChar))
                newChar = plugboard[newChar];
            else if (plugboard.ContainsValue(newChar))
                newChar = plugboard.FirstOrDefault(x => x.Value == newChar).Key;

            return newChar;
        }
        public void Rotate(bool forward)
        {
            if (forward)
            {
                _keyOffset[2]++;
                ring1 = MoveValues(forward, ring1);

                if (_keyOffset[2] / control.Length >= 1)
                {
                    _keyOffset[2] = 0;
                    _keyOffset[1]++;
                    ring2 = MoveValues(forward, ring2);
                    if (_keyOffset[1] / control.Length >= 1)
                    {
                        _keyOffset[1] = 0;
                        _keyOffset[0]++;
                        ring3 = MoveValues(forward, ring3);
                    }
                }
            }
            else
            {
                if (_keyOffset[2] > 0 || _keyOffset[1] > 0)
                {
                    _keyOffset[2]--;
                    ring1 = MoveValues(forward, ring1);
                    if (_keyOffset[2] < 0)
                    {
                        _keyOffset[2] = 25;
                        _keyOffset[1]--;
                        ring2 = MoveValues(forward, ring2);
                        if (_keyOffset[1] < 0)
                        {
                            _keyOffset[1] = 25;
                            _keyOffset[0]--;
                            ring3 = MoveValues(forward, ring3);
                            if (_keyOffset[0] < 0)
                                _keyOffset[0] = 25;
                        }
                    }
                }
            }

            DisplayRing(lblRotor1, ring1);
            DisplayRing(lblRotor2, ring2);
            DisplayRing(lblRotor3, ring3);
            DisplayOffset(); // Update offset display
        }
        public char Mirror(char letter)
        {
            char newChar = Encrypt(letter);

            newChar = ring3[IndexSearch(control, newChar)];
            newChar = ring2[IndexSearch(control, newChar)];
            newChar = ring1[IndexSearch(control, newChar)];

            return newChar;
        }
        public void DisplayRotor(Label displayLabel, string rotor)
        {
            string temp = "";
            foreach (char c in rotor)
                temp += rotor + "\t";
            displayLabel.Content = temp;
        }




        #region Decor
        private void txtBRing1Init_GotFocus(object sender, RoutedEventArgs e)
        {
            Rotor1Modify.Text = "";
        }

        private void txtBRing2Init_GotFocus(object sender, RoutedEventArgs e)
        {
            Rotor2Modify.Text = "";
        }

        private void txtBRing3Init_GotFocus(object sender, RoutedEventArgs e)
        {
            Rotor3Modify.Text = "";
        }

        private void txtPBModify_GotStatus(object sender, RoutedEventArgs e)
        {
            PBModify.Text = "";
        }
        #endregion

    }
}
