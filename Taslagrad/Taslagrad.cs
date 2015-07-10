using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Taslagrad
{
    public partial class Taslagrad : Form
    {
        private KeysSaver k;
        private KeysPlayer p;

        //Initialisation 
        public Taslagrad()
        {
            InitializeComponent();
            this.k = new KeysSaver();
        }

        /*
         * method launchRecording()
         * Description : Starts to record the keys. Called when the "record" button is triggered.
         */
        private void launchRecording(object sender, EventArgs e)
        {
            this.k.Start(); //Starts to save the keys
            startButton.Text = "Stop"; //Updates the button
            startButton.Click -= launchRecording;
            startButton.Click += stopRecording;
        }

        /*
         * method stopRecording()
         * Description : Stops to record the keys and logs the recorded keys in the console. Called when the "record" button is triggered.
         */
        private void stopRecording(object sender, EventArgs e)
        {
            startButton.Text = "Record";//Updates the button
            startButton.Click += launchRecording;
            startButton.Click -= stopRecording;
            Dictionary<long, Dictionary<Keys, IntPtr>> keys = this.k.Stop(); //Gets the recorded keys
            foreach (KeyValuePair<long, Dictionary<Keys, IntPtr>> kvp in keys)
            {
                foreach (KeyValuePair<Keys, IntPtr> kvp2 in kvp.Value)
                {
                    //Displays the recorded keys in the console
                    if (kvp2.Value == KeysSaver.KEYDOWN)
                    {
                        Console.WriteLine(kvp.Key + " : (down)" + kvp2.Key);
                    }
                    if (kvp2.Value == KeysSaver.KEYUP)
                    {
                        Console.WriteLine(kvp.Key + " : (up)" + kvp2.Key);
                    }
                }
            }
            this.p = new KeysPlayer(keys); //Creates a new player and gives it the recorded keys.
        }

        /*
         * method launchPlaying()
         * Description : Starts to play the keys. Called when the "play" button is triggered.
         */
        private void launchPlaying(object sender, EventArgs e)
        {
            this.p.Start(); //Starts to play the keys.
        }
    }
}
