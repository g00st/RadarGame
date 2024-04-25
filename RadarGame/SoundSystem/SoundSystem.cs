using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace RadarGame.SoundSystem;

public static class SoundSystem
{
    // List<SoundObject> soundObjects = new List<SoundObject>();

    public static unsafe void TrySinusIsUnsafe()
    {
        // Initialize
        var device = ALC.OpenDevice(null);
        var context = ALC.CreateContext(device, (int*)null);

        ALC.MakeContextCurrent(context);

        var version = AL.Get(ALGetString.Version);
        var vendor = AL.Get(ALGetString.Vendor);
        var renderer = AL.Get(ALGetString.Renderer);
        Console.WriteLine(version);
        Console.WriteLine(vendor);
        Console.WriteLine(renderer);

        // Process
        int buffers = 0; int source = 0;  // no need for int* disgusting buffers
       //  int otherbuffer = AL.GenBuffer(); int othersource = AL.GenBuffer(); // this is LEGAL
        AL.GenBuffers(1, ref buffers);       // no out ?
        AL.GenSources(1, ref source);

        int sampleFreq = 44100;   // example freq is sinus curve
        double dt = 2 * Math.PI / sampleFreq;
        double amp = 0.5;

        // ------------
        int freq = 440;  // standard freq
        var dataCount = sampleFreq / freq;
        // System.IntPtr testsinData = new short[dataCount];
        
        var sinData = new short[dataCount];
        for (int i = 0; i < sinData.Length; ++i)
        {
            sinData[i] = (short)(amp * short.MaxValue * Math.Sin(i * dt * freq));
        }
        
        AL.BufferData(buffers, ALFormat.Mono16, sinData, sampleFreq); // mag []

        // AL.BufferData(buffers, ALFormat.Mono16,ref  sinData, sinData.Length, sampleFreq); // ??? short[] nicht zu IntPtr konvertierbar

        AL.Source(source, ALSourcei.Buffer, buffers);
        AL.Source(source, ALSourceb.Looping, true);

        AL.SourcePlay(source);

        
        /*
        ///Dispose
        if (context != ContextHandle.Zero)
        {
            ALC.MakeContextCurrent(ContextHandle.Zero);
            ALC.DestroyContext(context);
        }
        context = ContextHandle.Zero;

        if (device != IntPtr.Zero)
        {
            ALC.CloseDevice(device);
        }
        // ALDevice.Null
        device = IntPtr.Zero;
        */

    }

    /*
    public static unsafe void TryLoopIsUnsafe()
    {
        //Initialize
        var device = Alc.OpenDevice(null);
        var context = Alc.CreateContext(device, (int*)null);

        Alc.MakeContextCurrent(context);

        var version = AL.Get(ALGetString.Version);
        var vendor = AL.Get(ALGetString.Vendor);
        var renderer = AL.Get(ALGetString.Renderer);
        Console.WriteLine(version);
        Console.WriteLine(vendor);
        Console.WriteLine(renderer);

        //Process

        int sampleFreq = 44100;
        double dt = 2 * Math.PI / sampleFreq;
        var dataCount = 100;
        double amp = 0.5;

        for (int freq = 440; freq < 10000; freq += 100)
        {
            int source;
            int buffers;
            object value = AL.GenBuffers(1, out buffers);
            AL.GenSources(1, out source);

            var sinData = new short[dataCount];
            for (int i = 0;
            i < sinData.Length; ++i)
            {
                sinData[i] = (short)(amp * short.MaxValue * Math.Sin(i * dt * freq));
            }

            AL.BufferData(buffers, ALFormat.Mono16, sinData, sinData.Length, sampleFreq);
            AL.Source(source, ALSourcei.Buffer, buffers);
            AL.Source(source, ALSourceb.Looping, true);

            AL.SourcePlay(source);
            Thread.Sleep(100);
        }
        Console.WriteLine("fin");
        Console.ReadKey();

        ///Dispose
        if (context != ContextHandle.Zero)
        {
            Alc.MakeContextCurrent(ContextHandle.Zero);
            Alc.DestroyContext(context);
        }
        context = ContextHandle.Zero;

        if (device != IntPtr.Zero)
        {
            Alc.CloseDevice(device);
        }
        device = IntPtr.Zero;
    }
    */

    /*
    public void Update()
    {
        // DO STUFF WITH LIST
        foreach(SoundObject sound in soundObjects)
        {
            if(sound.isDirectional)
            {
                // calculate direction
            } else
            {
                // is static sound like menu stuff
            }
        }
    }
    */
}