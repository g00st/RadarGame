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

        int sampleFreq = 44100;   // example freq is sinus curve speed  c
        double dt = 2 * Math.PI / sampleFreq;
        double amp = 0.5;

        // ------------
        int freq = 440;  // standard freqlänge  lambda
        var dataCount = sampleFreq / freq;   // f = c / lambda
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

    public static void PlayFileDotWave(string filename)
    {
        var device = ALC.OpenDevice(null);
        var context = ALC.CreateContext(device, new ALContextAttributes());
        ALC.MakeContextCurrent(context);
        int channels, bits_per_sample, sample_rate;
        byte[] soundData = LoadWave(
            File.Open(filename, FileMode.Open),
            out channels,
            out bits_per_sample,
            out sample_rate);
        ALFormat format = GetSoundFormat(channels, bits_per_sample);

        int bufferId = AL.GenBuffer();
        AL.BufferData(bufferId, format, soundData, bits_per_sample);
        // AL.Listener(ALListener3f.Position, 0.0f, 0.0f, 0.0f);
        // AL.Listener(ALListener3f.Velocity, 0.0f, 0.0f, 0.0f);

        int sourceId = AL.GenSource();
        // AL.Source(sourceId, ALSourcef.Gain, 1);
        // AL.Source(sourceId, ALSourcef.Pitch, 1);
        // AL.Source(sourceId, ALSource3f.Position, 0.0f, 0.0f, 0.0f);

        AL.Source(sourceId, ALSourcei.Buffer, bufferId);
        AL.SourcePlay(sourceId);
    }

    // Loads a wave/riff audio file.
    public static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate)
    {
        if (stream == null)
            throw new ArgumentNullException("stream");

        using (BinaryReader reader = new BinaryReader(stream))
        {
            // RIFF header
            string signature = new string(reader.ReadChars(4));
            if (signature != "RIFF")
                throw new NotSupportedException("Specified stream is not a wave file.");

            int riff_chunck_size = reader.ReadInt32();

            string format = new string(reader.ReadChars(4));
            if (format != "WAVE")
                throw new NotSupportedException("Specified stream is not a wave file.");

            // WAVE header
            string format_signature = new string(reader.ReadChars(4));
            if (format_signature != "fmt ")
                throw new NotSupportedException("Specified wave file is not supported.");

            int format_chunk_size = reader.ReadInt32();
            int audio_format = reader.ReadInt16();
            int num_channels = reader.ReadInt16();
            int sample_rate = reader.ReadInt32();
            int byte_rate = reader.ReadInt32();
            int block_align = reader.ReadInt16();
            int bits_per_sample = reader.ReadInt16();

            string data_signature = new string(reader.ReadChars(4));
            if (data_signature != "data")
                throw new NotSupportedException("Specified wave file is not supported.");

            int data_chunk_size = reader.ReadInt32();

            channels = num_channels;
            bits = bits_per_sample;
            rate = sample_rate;

            return reader.ReadBytes((int)reader.BaseStream.Length);
        }
    }

    // Checks SoundFormat
    public static ALFormat GetSoundFormat(int channels, int bits)
    {
        switch (channels)
        {
            case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
            case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
            default: throw new NotSupportedException("The specified sound format is not supported.");
        }
    }

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