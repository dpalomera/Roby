using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using static manejadoraudio;




[Serializable]
class Respuesta
{
    public string audio;
    public string session_id;
    public List<string> actividades = new List<string>();
}

public class ConversationManager : MonoBehaviour
{
    public delegate void EventTalkStart();
    public static event EventTalkStart OnStartTalking;

    public delegate void EventTalkEnd();
    public static event EventTalkEnd OnFinishTalking;

    public delegate void EventActividad(string nombre);
    public static event EventActividad OnActividad;

    private AudioSource audioSource;
    private bool playing;

    public GameObject avisos;
    private string session_id = "";
    private static readonly string uri = "https://us-south.functions.cloud.ibm.com/api/v1/namespaces/dedopalomera%40gmail.com_dev/actions/prueba?blocking=true";

    private object MakeParams(string audio = "")
    {
        return new Respuesta
        {
            session_id = session_id,
            audio = audio
        };

    }

    // Start is called before the first frame update
    void Start()
    {
        var user = "62eec160-364b-47e6-861e-ee40c4698c48";
        var password = "SQFles2YYwCst1MSoprczRl9DL84M7rYSwLqG0hM45fnAtMqV0aUcZLHvEFThSBP";
        var token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{user}:{password}"));

        manejadoraudio.AudioListo += ProcesarAudio;

        RestClient.DefaultRequestHeaders["Authorization"] = $"Basic {token}";

        audioSource = GetComponent<AudioSource>();

        avisos.SetActive(false);
        ArTapToPlaceObjectg.OnAvatarInstanciado += () =>
        {
            ProcesarAudio();
            avisos.SetActive(true);
        };
    }

    void ProcesarAudio(string audio = "")
    {
        RestClient.Post(uri, MakeParams(audio)).Then(response =>
        {
            avisos.GetComponentInChildren<Text>().text = "1";
            File.WriteAllText(Application.persistentDataPath + "/ret.json", response.Text);
            var json = JObject.Parse(response.Text);
            var result = json["response"]["result"];

            avisos.GetComponentInChildren<Text>().text = "2";

            var respuesta = new Respuesta()
            {
                session_id = result.Value<string>("session_id"),
                audio = result.Value<string>("audio"),
            };

            avisos.GetComponentInChildren<Text>().text = "3";
            
            var context = result["context"];

            avisos.GetComponentInChildren<Text>().text = "4";
            var con_actividad = context.Value<JArray>("Actividad").Count > 0;
            if (con_actividad)
            {
                avisos.GetComponentInChildren<Text>().text = "Tengo actividad!";
                
            }
            else
            {
                avisos.GetComponentInChildren<Text>().text = "No Tengo actividad!";
            }
            


            //avisos.GetComponentInChildren<Text>().text = respuesta.session_id + " -  " + respuesta.audio.Length.ToString() + " - " + session_id;

            session_id = respuesta.session_id;
            string path = Application.persistentDataPath + "/miaudio.ogg";
            File.WriteAllBytes(path, Convert.FromBase64String(respuesta.audio));


            StartCoroutine(loadMusic(path, con_actividad));
        });
    }
    IEnumerator loadMusic(string filename, bool con_actividad)
    {

        using (var www = UnityWebRequestMultimedia.GetAudioClip("file://" + filename, AudioType.OGGVORBIS))
        {
            yield return www.SendWebRequest(); // code will wait till file is completely read
            var clip = DownloadHandlerAudioClip.GetContent(www);
            audioSource.clip = clip;
            audioSource.Play();
            playing = true;
            avisos.GetComponentInChildren<Text>().text = "4";
            if (con_actividad)
            {
                OnActividad?.Invoke("globo");
            }
            else
            {
                OnStartTalking?.Invoke();
            }
            

        }

    }


    // Update is called once per frame
    void Update()
    {
        if(playing && !audioSource.isPlaying)
        {
            playing = false;
            OnFinishTalking?.Invoke();

        }
    }
}
