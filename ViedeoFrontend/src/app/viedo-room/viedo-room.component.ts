import { isPlatformBrowser } from '@angular/common';
import { Component, ElementRef, Inject, OnDestroy, OnInit, PLATFORM_ID, ViewChild } from '@angular/core';

@Component({
  selector: 'app-viedo-room',
  templateUrl: './viedo-room.component.html',
  styleUrls: ['./viedo-room.component.scss']
})
export class ViedoRoomComponent {
 
  videoRef:any;
  mySrc:MediaStream|undefined;

ngOnInit(): void {
this.videoRef = document.getElementById('video');
console.log(this.videoRef);
this.setUpCamera();

}

setUpCamera():void{
  navigator.mediaDevices.getUserMedia({
    video:{width:300,height:250},
    audio:false
  }).then(stream=>{
    console.log(stream);
    this.mySrc = stream;
    console.log(this.videoRef);
  })
}


}

