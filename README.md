# Roby HEAP
Official repo of Roby **HEA**lth **P**artner mobile App. Please note Unity version `2020.1.0f1` is needed to build and a ARCore-enabled Android device is needed to run it.

## Download
There is a link of the Android APK build in the [releases page](https://github.com/dpalomera/Roby/releases).

## About Roby HEAP
Generally, children do not miss the opportunity to express what they are thinking or feeling, however, the current emergency context resulting from COVID-19 has led them to deal with new problems and situations that their parents have not faced either and many times they are also overwhelmed.

Using the spoken language as our spearhead, we developed Roby Health Partner, a virtual parner with whom children can talk naturally and tell how they feel, finding in it a help and answer to their concerns. Roby identifies the symptoms presented by the child and classifies them into: anxiety, sadness, sleep disturbances, irritability and grief, providing listening, attention, guidelines and relevant activities to help the child to better cope with their feelings.

## Building
TBD

## API
Ruby uses an IBM Cloud Function (an openwhisk-based service) as endpoint to process and answer interactions. The source code of that service can be found in [Functions/api.js](Functions/api.js). In order to prevent abuses, all the keys in this file have been deleted, so it is necessary to previously create all the services and obtain the relevant keys.

## Architecture
Roby HEAP communicates, via an API created in IBM Cloud Functions, with IBM's Speech to Text, Watson Assistant, Text to Speech, and IBM Cloud Object Storage services (as a cache for TTS audios), as follows:
![arquitectura](/arquitectura.jpg?raw=true "Arquitectura")


## Acknowledgments
Roby HEAP uses the following services and libraries:
* [IBM Speech to Text](https://www.ibm.com/cl-es/cloud/watson-speech-to-text) To convert the users' voice to text.
* [IBM Watson Assistant](https://www.ibm.com/cloud/watson-assistant/) to manage the conversational dialogue, identify intentions, entities and recommend activities.
* [IBM Text to Speech](https://www.ibm.com/ar-es/cloud/watson-text-to-speech) to convert the text of the answers from Assistant to audio.
* [IBM Cloud Object Storage](https://www.ibm.com/cl-es/cloud/object-storage) to store and reuse Text to Speech responses.
* [AR Fundation](https://unity.com/es/unity/features/arfoundation) as AR Framework.
* [ARCore](https://unity3d.com/es/partners/google/arcore) as AR implementation for Android.
* [Jammo Character](https://github.com/mixandjam/Jammo-Character) as avatar.
* [JSONDotNet For Unity](https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347) to parse JSON responses.
* [NAudio](https://github.com/naudio/NAudio) to deal with audio encode/decode.
