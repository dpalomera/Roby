using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManejadorAvatar : MonoBehaviour
{
    public delegate void EventIdle();
    public static event EventIdle OnIdle;

    public string animacionPorDefecto = "pump";
    private static List<string> animaciones = new List<string>()
    {
        "pump",
        "pregunta",
        "hablando",
        "spawn",
        "manos",
        "idle",
    };


    public void SetAnimacionActiva(string animacion)
    {
        Animator anim = null;
        foreach(GameObject obj in FindObjectsOfType<GameObject>(true))
        {
            if (!animaciones.Contains(obj.name))
            {
                continue;
            }
            obj.SetActive(obj.name == animacion);
            if(obj.name == "spawn")
            {
                anim = obj.GetComponent<Animator>();
            }
        }
        if(animacion == "spawn")
        {
            StartCoroutine(EsperaFinalizaSpawn());
        }
        if(animacion == "globo")
        {
            StartCoroutine(AnimacionGlobo());
        }
        if(animacion == "idle")
        {
            OnIdle?.Invoke();
        }
    }

    public IEnumerator EsperaFinalizaSpawn()
    {
        yield return new WaitForSeconds(3);
        SetAnimacionActiva("idle");
    }

    public IEnumerator AnimacionGlobo()
    {
        SetAnimacionActiva("manos");
        yield return new WaitForSeconds(8);

        //que hay dentro? (5 segundos)
        SetAnimacionActiva("pregunta");
        yield return new WaitForSeconds(2);

        //espacio para la respuesta
        SetAnimacionActiva("idle");
        yield return new WaitForSeconds(2);


        //que hace pasa? (2 segundos)
        SetAnimacionActiva("pregunta");
        yield return new WaitForSeconds(2);

        //espacio para la respuesta
        SetAnimacionActiva("idle");
        yield return new WaitForSeconds(2);

        //bombin
        SetAnimacionActiva("idle");
        yield return new WaitForSeconds(1);
        SetAnimacionActiva("pump");
        yield return new WaitForSeconds(4);

        //el resto
        SetAnimacionActiva("manos");
        yield return new WaitForSeconds(45);



        yield return new WaitForSeconds(28);
        SetAnimacionActiva("pump");
        yield return new WaitForSeconds(2);

        SetAnimacionActiva("idle");
    }

    // Start is called before the first frame update
    void Start()
    {
        SetAnimacionActiva(animacionPorDefecto);
        ArTapToPlaceObjectg.OnAvatarMovido += () =>
        {
            SetAnimacionActiva("spawn");
        };
        manejadoraudio.OnStartListening += () =>
        {
            SetAnimacionActiva("manos");
        };
        ConversationManager.OnStartTalking += () =>
        {
            SetAnimacionActiva("hablando");
        };
        ConversationManager.OnFinishTalking += () =>
        {
            SetAnimacionActiva("idle");
        };
        ConversationManager.OnActividad += (nombre) =>
        {
            
            SetAnimacionActiva(nombre);
        };

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}