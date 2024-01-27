import { isPlatformBrowser } from '@angular/common';
import { Component, ElementRef, Inject, Input, OnDestroy, OnInit, PLATFORM_ID, ViewChild, inject } from '@angular/core';
import { DataServiceService } from '../data-service.service';
import { LocalTrack, Participant, Track, TrackPublication, connect, createLocalAudioTrack, createLocalTracks, createLocalVideoTrack } from 'twilio-video';
import { Router } from '@angular/router';
import * as Twilio from 'twilio/lib/rest/Twilio';
import { TwillioService } from '../swagger';


@Component({
  selector: 'app-viedo-room',
  templateUrl: './viedo-room.component.html',
  styleUrls: ['./viedo-room.component.scss']
})
export class ViedoRoomComponent {
  // room: Room | undefined;
  // error: Error | undefined;
  // videoRef:any;
  // mySrc:MediaStream|undefined;
  @Input() roomName:string = '';
  @Input() accessToken:string = '';


  router = inject(Router);

  constructor(public dataService:DataServiceService,public twil:TwillioService){}


  async ngOnInit(): Promise<void> {
// this.videoRef = document.getElementById('video');
// console.log(this.videoRef);
// this.setUpCamera();

// this.startLocalVideo();
this.JoinRoom();
  
}

async JoinRoom(): Promise<void>{
  try{
    // const localTracks = await createLocalTracks({audio: true, video: true});
    if(this.dataService.user() === null){
      console.log('gast');
      this.dataService.roomName = this.roomName;
        this.dataService.accessToken = this.accessToken;
        console.log('no username');
        
      this.GastJoinRoom();
      
    }
    else{
      const track = createLocalVideoTrack();
const video = document.getElementById('local-media');

track.then(t => video?.append(t.attach()));
      console.log("room:");
      const room = await connect(this.dataService.token.toString(),{
        networkQuality: {
          local: 2,
          remote: 2
      },
      });
      console.log(room);
    room.participants.forEach(this.participantConnected);
    room.on('participantConnected',this.participantConnected);
    room.on('participantDisconnected', this.participantDisconnected);
    }
    
    

    // console.log(room);
    // connect(this.dataService.token, {name:this.dataService.roomName}).then(room=>{
    //   this.dataService.room = room;
    //   console.log('connected');
    //   this.dataService.room.participants.forEach(this.participantConnected);
    //    this.dataService.room.on('participantConnected', this.participantConnected);
    //   this.dataService.room.on('participantDisconnected', participantDisconnected);
    // },error=>{
    //   console.log('not connected');
    // })
  }
  catch(e){
    // console.log(e);
    console.log(e);
  }


}

startLocalVideo(): void {
 createLocalVideoTrack({
    width: 1280, height: 720 ,
  }).then(track => {
    const div = document.getElementById('local-media');
    div?.appendChild(track.attach());
  });
}




participantConnected(participant:Participant):void{
  const div = document.getElementById('local-media');

  const participantDiv = document.createElement('div');
  participantDiv.setAttribute('id', participant.sid);

  const tracksDiv = document.createElement('div');
  tracksDiv.setAttribute('id', 'video-div');

  

  participantDiv.appendChild(tracksDiv);
  div?.appendChild(participantDiv);

  participant.tracks.forEach(publication => {
    if (publication.isEnabled) {
      this.trackSubscribed(tracksDiv,publication.kind);
    }
  });
  console.log('track');
  participant.on('trackSubscribed', track => tracksDiv.appendChild(track.attach())
  );

}

 trackSubscribed (div:any,track:any):void{
div.appendChild(track.attach());

}


participantDisconnected(participant:Participant):void{
  document.getElementById(participant.sid)?.remove();
}

GastJoinRoom():void{
  if(this.dataService.username.trim().length>0){
    const track = createLocalVideoTrack();
const video = document.getElementById('local-media');

track.then(t => video?.append(t.attach()));
    this.twil.madTechJoinRoomPost(this.dataService.roomName,this.dataService.username).subscribe(async x=>{
      if(x==null){
        
        this.router.navigateByUrl('error-page');
      }
      console.log("room:");
    const room = await connect(x,{
      networkQuality: {
        local: 2,
        remote: 2
    },
    });
    console.log(room);
  room.participants.forEach(this.participantConnected);
  room.on('participantConnected',this.participantConnected);
  room.on('participantDisconnected', this.participantDisconnected);
    });
  }
  else{
    this.router.navigateByUrl('enter-link-username');
  }
}


}

