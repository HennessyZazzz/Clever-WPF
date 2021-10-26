using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media;

namespace Clever.Model
{
    public class Cell : INotifyPropertyChanged
    {
        // EVENT
        public event PropertyChangedEventHandler PropertyChanged;


        // INCAPSULATED
        private string value = "";
        private string lucky = "?";
        private bool Enabled = true;
        private bool isOpen = false;
        private Brush color = Brushes.White;

        private int index = 0;
        
        public Cell(string value = "")
        {
            VALUE = value;
            LUCKY = "💣";
            //LUCKY = VALUE; // CHEAT
        }

        // PROPERTY

        public string VALUE
        {
            get => value;
            set
            {
                if (!this.value.Equals(value))
                {
                    this.value = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string LUCKY
        {
            get => lucky;
            set
            {
                if (!lucky.Equals(value))
                {
                     this.lucky = value;
                     NotifyPropertyChanged();
                }

            }
        }
        public bool ENABLE
        {
            get => Enabled;
            set
            {
                if (!Enabled.Equals(value))
                {
                    Enabled = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public bool OPENING
        {
            get => isOpen;
            set
            {
                if (!isOpen.Equals(value))
                {
                    isOpen = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int INDEX
        {
            get => index;
            set
            {
                if (!index.Equals(value))
                {
                    index = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Brush BRUSH
        {
            get => color;
            set
            {
                if (!color.Equals(value))
                {
                    color = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // METHOD
        public void selectCell()
        {
            OPENING = true;
            if (VALUE == "☠")
            {
                BRUSH = Brushes.Red;
            }
            else if (VALUE == "🌟" || VALUE == "🍒" || VALUE == "⛄")
            {
                BRUSH = Brushes.Gold;
            }
            else
            {
                BRUSH = Brushes.Gray;
            }
            LUCKY = VALUE;
        }
        public void DeclineCell()
        {
            OPENING = false;
            BRUSH = Brushes.White;
            LUCKY = "💣"; // cheat
        }
        public void DeleteCell()
        {
            ENABLE = false;
            BRUSH = Brushes.White;
            LUCKY = "";
        }

        public void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override string ToString()
        {
            return $"{OPENING}";
        }

    }
}
