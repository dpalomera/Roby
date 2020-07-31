const crypto = require('crypto');

const AWS = require('ibm-cos-sdk');

const AssistantV2 = require('ibm-watson/assistant/v2');
const TextToSpeechV1 = require('ibm-watson/text-to-speech/v1');
const SpeechToTextV1 = require('ibm-watson/speech-to-text/v1');


const ast = new AssistantV2({
    version: '2020-04-01',
    iam_apikey: '',
    url: 'https://api.us-south.assistant.watson.cloud.ibm.com/',
});
const assistant_id = '';


const t2s = new TextToSpeechV1({
    version: '2020-04-01',
    iam_apikey: '',
    url: 'https://api.us-south.text-to-speech.watson.cloud.ibm.com',
});
const t2p_model = "es-CL_NarrowbandModel";


const s2t = new SpeechToTextV1({
    version: '2020-04-01',
    iam_apikey: '',
    url: '',
});
const voice = "es-ES_EnriqueV3Voice";


var cos = new AWS.S3({
    endpoint: 's3.us-east.cloud-object-storage.appdomain.cloud',
    apiKeyId: '',
    ibmAuthEndpoint: 'https://iam.ng.bluemix.net/oidc/token',
    serviceInstanceId: '',
});
const bucketName = "deposito-cache-hackathon-callforcode-us-east";

async function Dialog(text ,session_id) {
    if (!session_id) {
        session_id = (await ast.createSession({
            assistant_id: assistant_id
        })).session_id;
    }
    
    let options = {
        input: {
            options: {
                return_context: true,
            },
            text: text
        },
        assistant_id: assistant_id,
        session_id: session_id,
    }

    let result = await ast.message(options);
    let ret = {
        session_id,
        text: result.output.generic[0].text,
        context: result.context.skills["main skill"].user_defined,
        //result: result
    };

    return ret;
}


function streamToString (stream) {
  const chunks = []
  return new Promise((resolve, reject) => {
    stream.on('data', chunk => chunks.push(chunk))
    stream.on('error', reject)
    stream.on('end', () => resolve(Buffer.concat(chunks).toString('base64')))
  })
}


async function text2Speech(text) {
    let options = {
        text,
        voice,
        accept: "audio/ogg;codecs=vorbis",
        return_response : true
    };

    let buff = await t2s.synthesize(options);
    
    return (await streamToString(buff.result));
}

async function Speech2Text(base64) {
    if(!base64){
        return "";
    }
    let audio = new Buffer(base64, 'base64');
    
    let options = {
        audio,
        model : t2p_model,
        content_type: "audio/mp3",
    };
    
      
     var ret = await s2t.recognize(options);

    return ret.results[0].alternatives[0].transcript;
}

async function GetCache(text) {
    let hash = crypto.createHash('md5').update(text).digest('hex');
    let params = {
        Bucket: bucketName,
        Key: hash
    };
    console.log(params);
	try{
		var content = (await cos.getObject(params).promise()).Body;
	}catch(ex){
		console.log("error =(");
		return null;
	}

    var audio = Buffer.from(content).toString();
    return audio;
}

async function SetCache(text, audio) {
    let hash = crypto.createHash('md5').update(text).digest('hex');
    console.log(hash);
    let params = {
        Bucket: bucketName,
        Key: hash,
        Body: audio
    };
    await cos.putObject(params).promise();
}

async function Cache(text) {
    let audio = await text2Speech(text);
    await SetCache(text, audio);
    return audio;
}


async function main(params) {
    let output = {};
    output.original = params;
    output.transcript = await Speech2Text(params.audio);
    output.dialog = await Dialog(output.transcript, params.session_id);
    output.audio = await GetCache(output.dialog.text);
	if(!output.audio){
		output.audio = await Cache(output.dialog.text);
		output.nuevo_audio = true;
	}else{
		output.nuevo_audio = false;
	}
    output.session_id = output.dialog.session_id;
    output.context = output.dialog.context;
    return output;
}
