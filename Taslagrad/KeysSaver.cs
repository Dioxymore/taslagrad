using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;

/* 
 * Class KeysSaver
 * Description : This class saves all the keyboard inputs, with the time (in miliseconds) they have been pressed.
 * Version : 1.0.0
 * Author : MetalFox Dioxymore
 */

class KeysSaver
{
    public static IntPtr KEYUP = (IntPtr)0x0101; // Code of the "key up" signal
    public static IntPtr KEYDOWN = (IntPtr)0x0100; // Code of the "key down" signal
    private Stopwatch watch; // Timer used to trace at which millisecond each key have been pressed
    private Dictionary<long, Dictionary<Keys, IntPtr>> savedKeys; // Recorded keys activity, indexed by the millisecond the have been pressed. The activity is indexed by the concerned key ("Keys" type) and is associated with the activity code (0x0101 for "key up", 0x0100 for "key down").
    private IntPtr hookId; // Hook used to listen to the keyboard

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam); // Imported type : LowLevelKeyboardProc. Now we can use this type.


    /*
     * Constructor 
     */
    public KeysSaver()
    {
        this.savedKeys = new Dictionary<long, Dictionary<Keys, IntPtr>>();
        this.watch = new Stopwatch();
    }

    /*
     * method Start()
     * Description : starts to save the keyboard inputs.
     * See : https://msdn.microsoft.com/en-us/library/windows/desktop/ms644990%28v=vs.85%29.aspx
     */

    public void Start()
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule) // Get the actual thread
        {
            // Installs a hook to the keyboard (the "13" params means "keyboard", see the link above for the codes), by saying "Hey, I want the function 'onActivity' being called at each activity. You can find this function in the actual thread (GetModuleHandle(curModule.ModuleName)), and you listen to the keyboard activity of ALL the treads (code : 0)
            this.hookId = SetWindowsHookEx(13, onActivity, GetModuleHandle(curModule.ModuleName), 0);
        }
        this.watch.Start(); // Starts the timer
    }

    /*
     * method Stop()
     * Description : stops to save the keyboard inputs.
     * Returns : the recorded keys activity since Start().
     */
    public Dictionary<long, Dictionary<Keys, IntPtr>> Stop()
    {
        this.watch.Stop(); // Stops the timer
        UnhookWindowsHookEx(this.hookId); //Uninstalls the hook of the keyboard (the one we installed in Start())
        return this.savedKeys;
    }

    /*
     * method onActivity()
     * Description : function called each time there is a keyboard activity (key up of key down). Saves the detected activity and the time at the moment it have been done.
     * @nCode : Validity code. If >= 0, we can use the information, otherwise we have to let it.
     * @wParam : Activity that have been detected (keyup or keydown). Must be compared to KeysSaver.KEYUP and KeysSaver.KEYDOWN to see what activity it is.
     * @lParam : (once read and casted) Key of the keyboard that have been triggered.
     * See : https://msdn.microsoft.com/en-us/library/windows/desktop/ms644985%28v=vs.85%29.aspx (for this function documentation)
     * See : https://msdn.microsoft.com/en-us/library/windows/desktop/ms644974%28v=vs.85%29.aspx (for CallNextHookEx documentation)
     */
    private IntPtr onActivity(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0) //We check the validity of the informations. If >= 0, we can use them.
        {
            long time = this.watch.ElapsedMilliseconds; //Number of milliseconds elapsed since we called the Start() method
            int vkCode = Marshal.ReadInt32(lParam); //We read the value associated with the pointer (?)
            Keys key = (Keys)vkCode; //We convert the int to the Keys type
            if (!this.savedKeys.ContainsKey(time))
            {
                // If no key activity have been detected for this millisecond yet, we create the entry in the savedKeys Dictionnary
                this.savedKeys.Add(time, new Dictionary<Keys, IntPtr>());
            }
            this.savedKeys[time].Add(key, wParam); //Saves the key and the activity
        }
        return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam); //Bubbles the informations for others applications using similar hooks
    }

    // Importation of native libraries
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
}