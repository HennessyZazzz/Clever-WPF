using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
using Clever.Model;
using System.Timers;
using System.Linq;
using System;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;

namespace Clever
{
    public class DataViewModel : INotifyPropertyChanged
    {
        // INCAPSULATED
        List<int> obj = new List<int>();
        private int score;
        private int highScore;
        public string[] Primary = { "☠", "♘", "♚", "👌", "☯", "🌀", "♝", "★", "⛄", 
            "🌙", "🌟", "🍒", "🔥", "💢", "☁", "🐭", "🐲", "❃", "☢", "❂", "❇", "☂", "❉" };
        public Random rnd = new Random();
        private ObservableCollection<Cell> clev = new ObservableCollection<Cell>();
        private const int ATTEMPS_LIMIT = 50;
        private int attemps = ATTEMPS_LIMIT;
        private bool SliderEnabled = true;
        public int second = 0;
        DispatcherTimer timer;
        bool islogic = false;
        // COMMAND
        private ICommand [] cellsCommand;
        private Command startCommand;
        // EVENT
        public event PropertyChangedEventHandler PropertyChanged;
        private string stateStart = "START";
        public DataViewModel()
        {
            Load();
            Repair(false);
            List<ICommand> temp = new List<ICommand>();
            foreach (var item in Clevers)
            {
                temp.Add(new DelegateCommand(() => Action(item)));
            }
            cellsCommand = temp.ToArray();

            startCommand = new DelegateCommand(() => StartAction());

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
        }



        // PROPERTY
        public bool SLIDERENABLED
        {
            get => SliderEnabled;
            set
            {
                if (!SliderEnabled.Equals(value))
                {
                    SliderEnabled = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string STATESTART 
        {
            get => stateStart;
            set
            {
                if (!stateStart.Equals(value))
                {
                    stateStart = value;
                    NotifyPropertyChanged();
                }
            }

        }
        public ObservableCollection<Cell> Clevers
        {
            get => clev;
            set
            {
                clev = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(cellsCommand));
            }
        }
        public ICommand [] Commands => cellsCommand;
        public ICommand STARTCOMMAND => startCommand;
        public int SCORE
        {
            get => score;
            set
            {
                if (!score.Equals(value))
                {
                    score = value;
                    if (score > highScore)
                    {
                        HIGHSCORE = value;
                    }
                    NotifyPropertyChanged();
                }
            }
        }
        public int HIGHSCORE
        {
            get => highScore;
            set
            {
                if (!highScore.Equals(value))
                {
                    highScore = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int ATTEMPS
        {
            get => attemps;
            set
            {
                if (!attemps.Equals(value))
                {
                    attemps = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // METHODS
        private void Timer_Tick(object sender, EventArgs e)
        {
            ++second;
            
            if (second == 1)
            {
                Logic();
                timer.Stop();
                second = 0;
                islogic = false;
            }
        }
        public void Repair(bool enable = false)
        {
            ObservableCollection<Cell> tempCells = new ObservableCollection<Cell>();
            string value = Primary[rnd.Next(0, Primary.Length)];
            for (int i = 0; i < 36; ++i)
            {
                if (i % 2 == 0)
                {
                      value = Primary[rnd.Next(0, Primary.Length)];
                }
                tempCells.Add(new Cell(value));
            }
            SHUFFLE.Shuffle(tempCells);
            Clevers = new ObservableCollection<Cell>();
            for (int i = 0; i < 36; ++i)
            {
                tempCells[i].INDEX = i;
                Clevers.Add(tempCells[i]);
            }
            if (enable)
            {
                foreach (var item in Clevers)
                {
                    item.ENABLE = true;
                }
            }
            else
            {
                foreach (var item in Clevers)
                {
                    item.ENABLE = false;
                }
            }
        }
        public void Action(Cell cell)
        {;
            if (!islogic)
            {
                int index = cell.INDEX;
                Clevers[index].selectCell();
                if (!obj.Contains(cell.INDEX))
                {
                    obj.Add(cell.INDEX);
                }
                if (obj.Count == 2)
                {
                    timer.Start();
                    islogic = true;
                };
            }
        }
        public void StartAction()
        {
            if (STATESTART == "START")
            {

                STATESTART = "STOP";          
                SLIDERENABLED = false;
                Repair(true);
            }
            else
            {
                STATESTART = "START";
                ATTEMPS = ATTEMPS_LIMIT;
                SCORE = 0;
                SLIDERENABLED = true;
                Repair(false);
            }
                    
        }
        public void Logic()
        {
            if (obj.Count == 2)
            {
                if (!Clevers[obj[0]].VALUE.Equals(Clevers[obj[1]].VALUE))
                {
                    Clevers[obj[0]].DeclineCell();
                    Clevers[obj[1]].DeclineCell();
                    obj.Clear();
                    ATTEMPS -= 1;
                    if (ATTEMPS == 0)
                    {
                        STATESTART = "START";
                        SLIDERENABLED = true;
                        MessageBox.Show($"   You Lose \n   Your Score : {SCORE}\n   RECORD {HIGHSCORE}", 
                            $"   SCORE : {SCORE}", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                        Repair(false);
                        ATTEMPS = ATTEMPS_LIMIT;
                        SCORE = 0;
                    }
                }
                else
                {
                    if (Clevers[obj[0]].VALUE == "☠" && Clevers[obj[0]].VALUE == "☠")
                    {
                        ATTEMPS -= 2;
                    }
                    else if (Clevers[obj[0]].VALUE == "🌟" && Clevers[obj[0]].VALUE == "🌟")
                    {
                        SCORE += 4;
                    }
                    else if (Clevers[obj[0]].VALUE == "🍒" && Clevers[obj[0]].VALUE == "🍒")
                    {
                        SCORE += 2;
                    }
                    else if (Clevers[obj[0]].VALUE == "⛄" && Clevers[obj[0]].VALUE == "⛄")
                    {
                        SCORE += 6;
                    }
                    else
                    {
                        SCORE += 1;
                    }
                    Clevers[obj[0]].DeleteCell();
                    Clevers[obj[1]].DeleteCell();
                    obj.Clear();
                    int count = 0;
                    for (int i = 0; i < Clevers.Count; ++i)
                    {
                        if (Clevers[i].ENABLE)
                        {
                            ++count;
                        }
                    }
                    if (count == 0)
                    {
                        STATESTART = "START";
                        SLIDERENABLED = true;
                        MessageBox.Show($"   You Win \n   Your Score : {SCORE}\n   RECORD {HIGHSCORE}\n   LOSE ATTEMPS : {ATTEMPS_LIMIT - ATTEMPS}",
                            $"   SCORE : {SCORE}", MessageBoxButton.OKCancel, MessageBoxImage.Information); ;
                        Repair(false);
                        ATTEMPS = ATTEMPS_LIMIT;
                        SCORE = 0;
                    }
                }
            }
        }

        public void NotifyPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public void Save()
        {
            XmlSerializer xml = new XmlSerializer(typeof(int));
            using (TextWriter tw = new StreamWriter("score.xml"))
            {
                xml.Serialize(tw, HIGHSCORE);
            }
        }
        public void Load()
        {
            XmlSerializer xml = new XmlSerializer(typeof(int));
            using (TextReader tr = new StreamReader("score.xml"))
            {
                HIGHSCORE = (int)xml.Deserialize(tr);
            }
        }
    }
    static class SHUFFLE
    {
        static Random rng = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}

