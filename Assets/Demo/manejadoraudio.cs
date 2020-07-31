using NAudio.Lame;
using NAudio.Wave;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class manejadoraudio : EventTrigger
{
    public delegate void OnAudioListo(string parametro);
    public static event OnAudioListo AudioListo;

    public delegate void EventRecordStart();
    public static event EventRecordStart OnStartListening;

    public delegate void EventRecordEnd();
    public static event EventRecordEnd OnFinishListening;

    private AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();       
    }


    public override void OnPointerDown(PointerEventData eventDat)
    {
        audioSource.clip = Microphone.Start(null, false, 10, 11025);
        if(OnStartListening != null)
        {
            OnStartListening();
        }
        
    }

    public override void OnPointerUp(PointerEventData eventDat)
    {
        audioSource.Stop();
        var clip = audioSource.clip;
        //originalmente eran 128
        var bitRate = 64;
        //TODO: cortar audio cuando es menos de 10 segundos
        
        //EncodeMP3.convert(audioSource.clip, filePath, 128);

        var samples = new float[clip.samples * clip.channels];

        clip.GetData(samples, 0);

        if(OnFinishListening != null)
        {
            OnFinishListening();
        }
            

        short[] intData = new short[samples.Length];
        //converting in 2 float[] steps to Int16[], //then Int16[] to Byte[]

        byte[] bytesData = new byte[samples.Length * 2];
        //bytesData array is twice the size of
        //dataSource array because a float converted in Int16 is 2 bytes.

        float rescaleFactor = 32767; //to convert float to Int16

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            byte[] byteArr;
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        var retMs = new MemoryStream();
        var ms = new MemoryStream(bytesData);
        var rdr = new RawSourceWaveStream(ms, new WaveFormat(clip.frequency, clip.channels));
        var wtr = new LameMP3FileWriter(retMs, rdr.WaveFormat, bitRate);

        rdr.CopyTo(wtr);
        var data = retMs.ToArray();

        string audio = Convert.ToBase64String(data, 0, data.Length);
        AudioListo(audio);

        //audioSource.Play();
    }

}
