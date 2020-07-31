using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManejadorAvatar : MonoBehaviour
{
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
            StartCoroutine(EsperaFinalizaSpawn(anim));
        }
        if(animacion == "globo")
        {
            StartCoroutine(AnimacionGlobo());
        }
    }

    public IEnumerator EsperaFinalizaSpawn(Animator anim)
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        SetAnimacionActiva("idle");
    }

    public IEnumerator AnimacionGlobo()
    {
        SetAnimacionActiva("pregunta");
        yield return new WaitForSeconds(2);
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