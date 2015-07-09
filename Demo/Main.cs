// This code is distributed under MIT license. 
// Copyright (c) 2015 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using Gma.System.MouseKeyHook.Implementation;
using System.Collections.Generic;
using System.Threading;
using WindowsInput;
using System.Windows.Input;
using System.Diagnostics;
using System.Collections;

namespace Demo
{
    public partial class Main : Form
    {
        private IKeyboardMouseEvents m_Events;
        private volatile Dictionary<long,Dictionary<Keys, string>> keys;
        private volatile Dictionary<Keys, string> currentDown;
        private System.Threading.Timer timer;
        private volatile Stopwatch watch;
        private bool playing;
        private volatile int pointer;
        private volatile int playFrame;
        private volatile int fps;
        private Thread frameThread;
        private bool stopThread;

        //Initialisation 
        public Main()
        {
            InitializeComponent();
            this.keys = new Dictionary<long, Dictionary<Keys, string>>();
            this.currentDown = new Dictionary<Keys, string>();
            this.fps = 7;
            this.watch = new Stopwatch();
            //InterceptKeys.SetHook(Log);
        }

        private void launchRecording(object sender, EventArgs e)
        {
            this.playing = true;
            startButton.Text = "Stop";
            startButton.Click -= launchRecording;
            startButton.Click += stopRecording;
            this.watch.Reset();
            this.watch.Start();
            this.playFrame = 0;
            //this.frameThread = new Thread(this.recordThread);
            
            //this.initTimerForRecording();
            this.initMouseKeyboardEvents(Hook.GlobalEvents());
            //this.frameThread.Start();
            //FormClosing += Main_Closing;
            
        }

        private void recordThread()
        {
            this.pointer = 0;
            this.watch.Reset();
            this.watch.Start();
            while (this.playing)
            {
                long v = this.watch.ElapsedMilliseconds;
                long t = v / this.fps;
                //Console.WriteLine(t);
                if (t == this.pointer)
                {
                    this.tick();
                    Console.WriteLine("Rec -- "+t+" : "+v+"ms");
                    this.pointer++;
                    
                }
                //Thread.Sleep(1);
                //Thread.Sleep(this.fps);
            }
        }

        private void playThread()
        {
            this.pointer = 0;
            this.watch.Reset();
            this.watch.Start();
            IEnumerator<long> enumerator = this.keys.Keys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Thread.Sleep((int)(enumerator.Current - this.pointer - 1));
                while (this.watch.ElapsedMilliseconds < enumerator.Current)
                {
                    //Console.WriteLine(this.watch.ElapsedMilliseconds);
                }
                this.playF(enumerator.Current);
                this.pointer = (int)enumerator.Current;
                //Console.WriteLine(enumerator.Current);
            }
            
            /*while (this.playing)
            {
                long v = this.watch.ElapsedMilliseconds;
                long t = v / this.fps;
                //Console.WriteLine(t);
                if (t == this.pointer)
                {
                    //Thread ft = new Thread(this.play);
                    //ft.Start();
                    this.play();
                    Console.WriteLine("Play -- " + t + " : " + v + "ms");
                    this.pointer++;

                }
                //Thread.Sleep(1);
                //Thread.Sleep(this.fps);
            }*/
        }

        private void playF(long frame){
            foreach (KeyValuePair<Keys, string> kvp in this.keys[frame])
            {
                switch (kvp.Value)
                {
                    case "down": InputSimulator.SimulateKeyDown((VirtualKeyCode)kvp.Key); break;
                    case "up": InputSimulator.SimulateKeyUp((VirtualKeyCode)kvp.Key); break;
                    case "press": InputSimulator.SimulateKeyPress((VirtualKeyCode)kvp.Key); break;
                }
                long t = this.watch.ElapsedMilliseconds;
                if (t != frame)
                {
                    Console.WriteLine(this.watch.ElapsedMilliseconds + " / " + frame + " : " + "(" + kvp.Value + ")" + kvp.Key);
                }
            }
        }

        private void launchPlaying(object sender, EventArgs e)
        {
            this.currentDown = new Dictionary<Keys, string>();
            
            this.playing = true;
            startButton.Text = "Stop";
            startButton.Click -= launchPlaying;
            startButton.Click += stopPlaying;
            this.playFrame = 0;
            this.frameThread = new Thread(this.playThread);
            this.frameThread.Start();
            //this.initTimerForPlaying();
        }

        private void stopRecording(object sender, EventArgs e)
        {
            this.playing = false;
            this.stopMouseKeyboardEvents();
            this.watch.Stop();
            //this.timer.Stop();
            //this.stopThread = true;
            startButton.Text = "Start";
            startButton.Click += launchRecording;
            startButton.Click -= stopRecording;
            //this.timer.Dispose();
            foreach (KeyValuePair<long, Dictionary<Keys, string>> kvp in this.keys)
            {
                //Console.WriteLine("FRAME " + kvp.Key + "=================");
                foreach (KeyValuePair<Keys, string> kvp2 in kvp.Value)
                {
                    Console.WriteLine(kvp.Key+" : (" + kvp2.Value + ")" + kvp2.Key);
                }
            }
        }

        private void initTimerForRecording()
        {
            //this.timer = new System.Threading.Timer((x) => this.tick(), null, 0, 1000 / this.fps);
           /* this.timer.Tick += new EventHandler(tick);
            this.timer.Interval = 1000/this.fps; // in miliseconds
            this.timer.Start();
            this.stopThread = false;*/
            /*this.frameThread = new Thread(this.tick);
            this.frameThread.Start();*/
        }

        private void initTimerForPlaying()
        {
            this.playFrame = 0;
            this.timer = new System.Threading.Timer((x) => this.play(), null, 0, 1000 / this.fps);
            /*this.timer = new System.Windows.Forms.Timer();
            this.timer.Tick += new EventHandler(play);
            this.timer.Interval = 1000 / this.fps; // in miliseconds
            this.timer.Start();*/
            /*this.stopThread = false;
            this.frameThread = new Thread(this.play);
            this.frameThread.Start();
            this.playFrame = 0;*/
        }

        private void stopPlaying(object sender, EventArgs e)
        {
            //this.watch.Stop();
            this.playing = false;
            //this.launchPlaying(null,null);
            /*playButton.Text = "Start";
            playButton.Click += launchPlaying;
            playButton.Click -= stopPlaying;*/
            //this.timer.Stop();
           // this.timer.Dispose();
            //this.stopThread = true;
        }

        private void play()
        {
            if (this.playFrame < this.keys.Count)
            {
                long t = this.watch.ElapsedMilliseconds / this.fps;
                if (this.keys.ContainsKey(t))
                {
                    foreach (KeyValuePair<Keys, string> kvp in this.keys[t])
                    {
                        switch (kvp.Value)
                        {
                            case "down": InputSimulator.SimulateKeyDown((VirtualKeyCode)kvp.Key); break;
                            case "up": InputSimulator.SimulateKeyUp((VirtualKeyCode)kvp.Key); break;
                            case "press": InputSimulator.SimulateKeyPress((VirtualKeyCode)kvp.Key); break;
                        }
                        Console.WriteLine(this.watch.ElapsedMilliseconds + " : " + "(" + kvp.Value + ")" + kvp.Key);
                    }
                }
                this.playFrame++;
                //new System.Threading.ManualResetEvent(false).WaitOne(1000 / this.fps);
            }
            else
            {
                this.stopPlaying(null,null);
            }
        }



        private void tick()
        {
                this.keys.Add(this.pointer, new Dictionary<Keys, string>(this.currentDown));
                //String s = "Frame " + this.playFrame + " : ";
                /*foreach (KeyValuePair<Keys, string> kvp in this.currentDown)
                {
                    s += "(" + kvp.Value + ")" + kvp.Key + " | ";
                }*/
                //this.playFrame++;
                this.currentDown.Clear();
            
                //new System.Threading.ManualResetEvent(false).WaitOne(1000 / this.fps);
        }

        /*private void Main_Closing(object sender, CancelEventArgs e)
        {
            this.stopRecording();
        }*/

        private void initMouseKeyboardEvents(IKeyboardMouseEvents events)
        {
            m_Events = events;
            m_Events.KeyDown += OnKeyDown;
            m_Events.KeyUp += OnKeyUp;
            m_Events.KeyPress += HookManager_KeyPress;

            m_Events.MouseUp += OnMouseUp;
            m_Events.MouseClick += OnMouseClick;
            m_Events.MouseDoubleClick += OnMouseDoubleClick;

            m_Events.MouseMove += HookManager_MouseMove;

            m_Events.MouseDragStarted += OnMouseDragStarted;
            m_Events.MouseDragFinished += OnMouseDragFinished;
        }

        private void stopMouseKeyboardEvents()
        {
            if (m_Events == null) return;
            m_Events.KeyDown -= OnKeyDown;
            m_Events.KeyUp -= OnKeyUp;
            m_Events.KeyPress -= HookManager_KeyPress;

            m_Events.MouseUp -= OnMouseUp;
            m_Events.MouseClick -= OnMouseClick;
            m_Events.MouseDoubleClick -= OnMouseDoubleClick;

            m_Events.MouseMove -= HookManager_MouseMove;

            m_Events.MouseDragStarted -= OnMouseDragStarted;
            m_Events.MouseDragFinished -= OnMouseDragFinished;

            m_Events.Dispose();
            m_Events = null;
        }

        private void HookManager_Supress(object sender, MouseEventExtArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                Log(string.Format("MouseDown \t\t {0}\n", e.Button));
                return;
            }

            Log(string.Format("MouseDown \t\t {0} Suppressed\n", e.Button));
            e.Handled = true;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            long t = this.watch.ElapsedMilliseconds;
            if (!this.keys.ContainsKey(t))
            {
                //Console.WriteLine(this.watch.ElapsedMilliseconds + " : " + "(down)" + e.KeyCode);
                this.keys.Add(t, new Dictionary<Keys, string>());
            }
            this.keys[t].Add(e.KeyCode, "down");
            //Log(string.Format("KeyDown  \t\t {0}\n", e.KeyCode));
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            long t = this.watch.ElapsedMilliseconds;
            if (!this.keys.ContainsKey(t))
            {
                //Console.WriteLine(this.watch.ElapsedMilliseconds + " : " + "(down)" + e.KeyCode);
                this.keys.Add(t, new Dictionary<Keys, string>());
            }
            this.keys[t].Add(e.KeyCode, "up");
            /*
            //this.currentDown.Remove(e.KeyCode);
            if (!this.currentDown.ContainsKey(e.KeyCode))
            {
                Console.WriteLine(this.watch.ElapsedMilliseconds + " : " + "(down)" + e.KeyCode);
                this.currentDown.Add(e.KeyCode, "up");
            }
            //Log(string.Format("KeyUp  \t\t {0}\n", e.KeyCode));*/
        }

        private void HookManager_KeyPress(object sender, KeyPressEventArgs e)
        {
            //this.currentDown.Add((Keys)e.KeyChar, "press");
            //Log(string.Format("KeyPress \t\t {0}\n", e.KeyChar));
        }

        private void HookManager_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            Log(string.Format("MouseDown \t\t {0}\n", e.Button));
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            Log(string.Format("MouseUp \t\t {0}\n", e.Button));
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            Log(string.Format("MouseClick \t\t {0}\n", e.Button));
        }

        private void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            Log(string.Format("MouseDoubleClick \t\t {0}\n", e.Button));
        }

        private void OnMouseDragStarted(object sender, MouseEventArgs e)
        {
            Log("MouseDragStarted\n");
        }

        private void OnMouseDragFinished(object sender, MouseEventArgs e)
        {
            Log("MouseDragFinished\n");
        }

        private void HookManager_MouseWheel(object sender, MouseEventArgs e)
        {
        }
        
        private void HookManager_MouseWheelExt(object sender, MouseEventExtArgs e)
        {
            Log("Mouse Wheel Move Suppressed.\n");
            e.Handled = true;
        }

        private void Log(string text)
        {
            textBoxLog.AppendText(text);
            textBoxLog.ScrollToCaret();
        }

        private void LogRec(string text)
        {
            textBoxRec.AppendText(text);
            textBoxRec.ScrollToCaret();
        }

        private void checkBoxSuppressMouse_CheckedChanged(object sender, EventArgs e)
        {
            if (m_Events == null) return;

            if (((CheckBox)sender).Checked)
            {
                m_Events.MouseDown -= OnMouseDown;
                m_Events.MouseDownExt += HookManager_Supress;
            }
            else
            {
                m_Events.MouseDownExt -= HookManager_Supress;
                m_Events.MouseDown += OnMouseDown;
            }
        }


        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}