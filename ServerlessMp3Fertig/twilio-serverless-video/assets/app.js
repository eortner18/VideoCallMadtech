
const usernameInput = document.getElementById('username');
const button = document.getElementById('join_leave');
const container = document.getElementById('container');
const count = document.getElementById('count');
let connected = false;
let room;
const list = document.getElementById('recordings');


const addLocalVideo = async () => {
  const track = await Twilio.Video.createLocalVideoTrack();
  const video = document.getElementById('local').firstElementChild;

  video.appendChild(track.attach());

  
};

const connectButtonHandler = async (event) => {
  event.preventDefault();
  if (!connected) {
    const username = usernameInput.value;
    if (!username) {
      alert('Enter your name before connecting');
      return;
    }
    button.disabled = true;
    button.innerHTML = 'Connecting...';
    try {
      await connect(username);
      button.innerHTML = 'Leave call';
      button.disabled = false;
    }
    catch {
      alert('Connection failed. Is the backend running?');
      button.innerHTML = 'Join call';
      button.disabled = false;    
    }
  }
  else {
    disconnect();
    button.innerHTML = 'Join call';
    connected = false;
  }
};

addLocalVideo();
button.addEventListener('click', connectButtonHandler);


const connect = async (username) => {
    const response = await fetch('/get_token', {
      method: 'POST',
      headers: {'Content-Type': 'application/json'},
      body: JSON.stringify({'username': username}),
    });
    const data = await response.json();
    console.log(data.token);
    room = await Twilio.Video.connect(data.token);
    room.participants.forEach(participantConnected);
    room.on('participantConnected', participantConnected);
    room.on('participantDisconnected', participantDisconnected);
    

    connected = true;
    updateParticipantCount();
  };

  const updateParticipantCount = () => {
    if (!connected) {
      count.innerHTML = 'Disconnected.';
    }
    else {
      count.innerHTML = (room.participants.size + 1) + ' participants online.';
    }
  };
  

  const participantConnected = (participant) => {
    const participantDiv = document.createElement('div');
    participantDiv.setAttribute('id', participant.sid);
    participantDiv.setAttribute('class', 'participant');
  
    const tracksDiv = document.createElement('div');
    
    tracksDiv.setAttribute('id', 'video-div');

    // const name = document.createElement("p");
    // name.setAttribute('id', 'username');
    // // name.append(document.createTextNode("Your name"));
    // tracksDiv.appendChild(name);



    participantDiv.appendChild(tracksDiv);
  
    const labelDiv = document.createElement('div');
    labelDiv.innerHTML = participant.identity;
    participantDiv.appendChild(labelDiv);
  
    container.appendChild(participantDiv);
  
    participant.tracks.forEach(publication => {
      if (publication.isSubscribed) {
        trackSubscribed(tracks_div, publication.track);
      }
    });
    participant.on('trackSubscribed', track => trackSubscribed(tracksDiv, track));
    participant.on('trackUnsubscribed', trackUnsubscribed);
    // participant.tracks.forEach(tra=>{
    //   console.log('track: ' );
    //   console.log(tra);
    //   if(tra.kind =='audio'){
    //     console.log('trackAudio');
    //     let t = tra.track;
    //     console.log(t);
    //     t = tra.signaling;
    //     console.log(t);
    //     t= tra._signaling.trackTransceiver;
    //     console.log(t);

    //     // UpdateText(track._track);
    //   }
    // })
    updateParticipantCount();
  };
  
  const participantDisconnected = (participant) => {
    document.getElementById(participant.sid).remove();
    updateParticipantCount();
  };
  
  const trackSubscribed = (div, track) => {
    div.appendChild(track.attach());

    if(track.kind == 'audio'){
      console.log('audio gefunden');
      // UpdateText(track);
    }
    // else{
    //   console.log('video: ');
    //   console.log(track.attach());
    // }

  };
  
  const trackUnsubscribed = (track) => {
    track.detach().forEach(element => element.remove());
  };
  

  const disconnect = () => {
    room.disconnect();
    while (container.lastChild.id != 'local') {
        container.removeChild(container.lastChild);
    }
    button.innerHTML = 'Join call';
    connected = false;
    updateParticipantCount();
  };
  
  const UpdateText = async (track) => {
    try {
        // console.log('attach');
        // console.log(track.attach());
        // let t2 = track.attach().srcObject;

        
        // let t2 = track;
        // console.log(t2);

        // const mediaRecorder = new MediaRecorder(t2);

        
         
          const mimeType = 'audio/webm';
          let chunks = [];
          const mediaRecorder = new MediaRecorder(track.attach().srcObject, { type: mimeType });


          mediaRecorder.addEventListener('dataavailable', event => {
            if (typeof event.data === 'undefined') return;
              if (event.data.size === 0) return;
              chunks.push(event.data);
            });
            mediaRecorder.addEventListener('stop', () => {
            const recording = new Blob(chunks, {
              type: mimeType
            });
            renderRecording(recording, list,track);
            chunks = [];
          });

        console.log('start');
        mediaRecorder.start();

        setTimeout(() => {
            mediaRecorder.stop();
            console.log(mediaRecorder);
            console.log('stop');
        }, 3000); 
    } catch (error) {
        console.error('Error accessing microphone:', error);
    }
};



function saveMp3ToFile(mp3Blob, filename,track) {
    const downloadLink = document.createElement('a');
    downloadLink.href = URL.createObjectURL(mp3Blob);
    downloadLink.download = filename;

    downloadLink.click();

    URL.revokeObjectURL(downloadLink.href);
    UpdateText(track)
}

function renderRecording(blob, list,track) {
  const blobUrl = URL.createObjectURL(blob);
  const li = document.createElement('li');
  const audio = document.createElement('audio');
  const anchor = document.createElement('a');
  anchor.setAttribute('href', blobUrl);
  anchor.setAttribute(
    'download',
    `recording.mp3`
  );
  anchor.innerText = 'Download';
  audio.setAttribute('src', blobUrl);
  audio.setAttribute('controls', 'controls');
  li.appendChild(audio);
  li.appendChild(anchor);
  list.appendChild(li);


  anchor.click();

    URL.revokeObjectURL(anchor.href);
    UpdateText(track);
  }
