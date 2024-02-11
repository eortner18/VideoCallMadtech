import { Component, inject } from '@angular/core';
import { DataServiceService } from './data-service.service';
import { TwillioService } from './swagger';
import { Router } from '@angular/router';
import { Participant, RemoteParticipant } from 'twilio-video';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ViedeoFrontend';
  router = inject(Router);

  constructor(public myData:DataServiceService,public twillioService:TwillioService){
    // console.log(myData.isLoggedIn());
  }

  LogOut():void{
    this.myData.user.set(null);
    this.myData.accessToken = "";
    this.myData.isLoggedIn = false;
    this.myData.room = null;
    this.myData.roomName = "";
    this.myData.token = "";
    this.myData.username = "";

    this.router.navigateByUrl("login");
  }

  Leave():void{
    this.myData.room?.disconnect();


    if(this.myData.user()!= null){
      
      this.myData.room!.participants.forEach(remoteParticipant => {
        remoteParticipant.removeAllListeners();
      });



        console.log("bis do hi gehts");

        this.twillioService.madTechDeleteRoomPost(this.myData.roomName);

        this.myData.accessToken = "";
    this.myData.room = null;
    this.myData.roomName = "";
    this.myData.token = "";
    this.myData.username = "";
    this.myData.inRoom = false;
      
    }

    

    if(this.myData.user()!=null){
      this.router.navigateByUrl("menu");
    }
    else{
      open(location.toString(),'_self')?.close();
      open(location.toString(),'_self')?.close();
    }
    

    
   
    
  }
}

