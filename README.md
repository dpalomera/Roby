# Roby
Repositorio con el código fuente de la aplicación para celulares Roby **HEA**lth **P**artner. Esta solución está desarrollanda ocupando el motor Unity en su versión `2020.1.0f1`.

## Instalación
TBD

## Arquitectura
Roby HEAP se comunica, mediante una API creada en IBM Cloud Functions, con los servicios de IBM de Speech to Text, Watson Assistant, Text to Speech, y IBM Cloud Object Storage (a modo de caché para los audios del TTS).


## Reconocimientos
Roby HEAP ocupa las siguientes librerías y soluciones de terceros:
* [IBM Speech to Text](https://www.ibm.com/cl-es/cloud/watson-speech-to-text) Para convertir la voz de los usuarios de la app a texto.
* [IBM Watson Assistant](https://www.ibm.com/cloud/watson-assistant/) para gestionar el dialogo conversacional, identificar intenciones, entidades y recomendar actividades.
* [IBM Text to Speech](https://www.ibm.com/ar-es/cloud/watson-text-to-speech) para convertir el texto de las respuestas de Assistant a Audio.
* [IBM Cloud Object Storage](https://www.ibm.com/cl-es/cloud/object-storage) para almacenar y reutilizar las respuestas de Text to Speech.
* [Jammo Character](https://github.com/mixandjam/Jammo-Character) como avatar.
* [JSONDotNet For Unity](https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347) para obtener la información de respuesta de la API.
* [NAudio](https://github.com/naudio/NAudio) como codificador y decodificador de audio.
