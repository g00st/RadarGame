using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Audio;
using OpenTK.Mathematics;
using OpenTK;
using OpenTK.Audio.OpenAL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace RadarGame.SoundSystem;

public static class SoundSystem
{
    // List<SoundObject> soundObjects = new List<SoundObject>();
    private static List<int>Buffers = new List<int>();
    private static List<int>Sources = new List<int>();
    private static ALDevice device;
    private static ALContext context;
    static int sampleFreq = 44100;
    static int freq = 440;
    static int dataCount = sampleFreq / freq;
    static short[] sinData = new short[dataCount];
    private static int sinDataIndex = 0;
    static int source = 0;

    // C:\Users\herob\RealUni\Uni\Computergrafik\Projektordner\code\RadarGame\SoundSystem\Laser3.wav
    // "resources/background2.jpg"
    static string filePath = "resources/Sounds/Laser3.wav";

    public static void DebugDraw()
    {
        ImGuiNET.ImGui.Begin("SoundSystem");
        // ImGuiNET.ImGui.SliderInt("SampleFrequenz", ref sampleFreq, 4000, 60000);
        // ImGuiNET.ImGui.SliderInt("Frequenz", ref freq, 40, 1000);
        ImGuiNET.ImGui.End();
        // ImGuiNET.ImGui.PlotLines("sinData", ref sinData[0], sinData.Length, sinDataIndex, "sinData", 0, 100, new System.Numerics.Vector2(0, 100));

    }
    public static void Update(FrameEventArgs args, KeyboardState keyboardState)
    {
        if (keyboardState.IsKeyDown(Keys.K))
        {
            // kill frequenz
            // StopPlayingSource();
        }
        if (keyboardState.IsKeyDown(Keys.L))
        {
            // play frequenz
            // PlaySinusWaveLoop(sampleFreq, freq);
        }
        /*
        if (keyboardState.IsKeyDown(Keys.I))
        {
            PlaySinusWaveNoLoop(sampleFreq, freq);
        }
        */
        if (keyboardState.IsKeyDown(Keys.B))
        {
            sampleFreq = 13655;
            freq = 494;
            // PlaySinusWaveNoLoop(sampleFreq, freq);
        }
        if(keyboardState.IsKeyDown(Keys.I))
        {
            // try out new library
            NewPlayer();
        }
    }
    

    public static void NewPlayer()
    {
        var audioFile = new AudioFileReader(filePath);
        Console.WriteLine(audioFile);
        Console.WriteLine("In New Player after audioFile");
        var volumeProvider = new VolumeSampleProvider(audioFile);

        // volume between 0.00 and 1.00
        volumeProvider.Volume = 0.5f;
        Console.WriteLine("Before Wave Out");
        var waveOut = new WaveOutEvent();
        waveOut.Init(volumeProvider);
        Console.WriteLine("After Wave Out");

        waveOut.Play();
        Console.WriteLine("After Play");
    }


}