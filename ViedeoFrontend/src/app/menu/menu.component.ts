import { Component, inject } from '@angular/core';
import { TwilioRooms, TwillioService } from '../swagger';
import { DataServiceService } from '../data-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent {
  roomNames:TwilioRooms[]=[];
  accessToken:string = "";
  mailInvite:string = "";
  router = inject(Router);

  
  constructor(public twilioService:TwillioService,public dataService:DataServiceService){
    twilioService.madTechGetRoomsGet().subscribe(x=>this.roomNames = x);
  }
  
  Join(roomName:string):void{
    this.twilioService.madTechJoinRoomTokenPost(roomName,this.dataService.user()?.userName!,this.accessToken).subscribe(x=>{
      if(x!= null){
        this.dataService.token = x;
        this.router.navigateByUrl(`video-room/${roomName}/${x}`);
      }
    })
  }

  CreateRoom():void{
    console.log(this.dataService.user()!);
    this.twilioService.madTechCreateRoomPost(this.mailInvite,this.dataService.user()!).subscribe(x=>{
      this.dataService.token = x.jwtToken!;
      console.log(x);
      this.router.navigateByUrl(`video-room/${x.roomName!}/${x.accessToken!}`);
    });
  }
}
