# Roby HEAP
Repositorio con el código fuente de la aplicación para celulares Roby **HEA**lth **P**artner. Esta solución está desarrollanda ocupando el motor Unity en su versión `2020.1.0f1`.

## Descarga
La descarga del proyecto compilado para Android está disponible [desde la página de realeases](https://github.com/dpalomera/Roby/releases).

## Acerca de Roby
Generalmente, los niños no pierden la oportunidad de expresar lo que están pensando o sintiendo, sin embargo, el contexto actual de emergencia producto del COVID-19 los ha llevado a lidiar con nuevos problemas y situaciones a los que sus padres tampoco se han visto enfrentados y muchas veces también se encuentran sobrepasados. 
 
Utilizando el lenguaje hablado como herramienta desarrollamos Roby Health Partner, un acompañante virtual con el que los niños pueden conversar de manera natural y contar como se sienten, encontrando en él una ayuda y respuesta a sus inquietudes. Este acompañante identifica la sintomatología que presenta el niño y la clasificará en: ansiedad, tristeza, alteraciones de sueño, irritabilidad y duelo, entregando escucha, atención, pautas y actividades pertinentes para ayudar al niño a sobrellevar mejor lo que está sintiendo.

## Instalación
TBD

## API
El código fuente de la función de IBM se puede encontrar en [Functions/api.js](Functions/api.js). Para evitar abusos se han borrado todas las llaves de este archivo, por lo que es necesario crear previamente todos los servicios y obtener las llaves pertinentes.

## Arquitectura
Roby HEAP se comunica, mediante una API creada en IBM Cloud Functions, con los servicios de IBM de Speech to Text, Watson Assistant, Text to Speech, y IBM Cloud Object Storage (a modo de caché para los audios del TTS), de la forma que se presenta a continuación:
![arquitectura](/arquitectura.jpg?raw=true "Arquitectura")


## Reconocimientos
Roby HEAP ocupa las siguientes librerías y soluciones de terceros:
* [IBM Speech to Text](https://www.ibm.com/cl-es/cloud/watson-speech-to-text) Para convertir la voz de los usuarios de la app a texto.
* [IBM Watson Assistant](https://www.ibm.com/cloud/watson-assistant/) para gestionar el dialogo conversacional, identificar intenciones, entidades y recomendar actividades.
* [IBM Text to Speech](https://www.ibm.com/ar-es/cloud/watson-text-to-speech) para convertir el texto de las respuestas de Assistant a Audio.
* [IBM Cloud Object Storage](https://www.ibm.com/cl-es/cloud/object-storage) para almacenar y reutilizar las respuestas de Text to Speech.
* [AR Fundation](https://unity.com/es/unity/features/arfoundation) como marco de trabajo para la funcionalidad de realidad aumentada.
* [ARCore](https://unity3d.com/es/partners/google/arcore) como implementación de AR para Android.
* [Jammo Character](https://github.com/mixandjam/Jammo-Character) como avatar.
* [JSONDotNet For Unity](https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347) para obtener la información de respuesta de la API.
* [NAudio](https://github.com/naudio/NAudio) como codificador y decodificador de audio.
