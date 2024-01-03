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
  roomName:string = "";
  router = inject(Router);

  
  constructor(public twilioService:TwillioService,public dataService:DataServiceService){
    twilioService.madTechGetRoomsGet().subscribe(x=>this.roomNames = x);
  }
  
  Join(roomName:string):void{
  
  }

  CreateRoom():void{
    console.log(this.roomName);
    console.log(this.dataService.user()!);
    this.dataService.roomName = this.roomName;
    this.twilioService.madTechCreateRoomPost(this.roomName,this.dataService.user()!).subscribe(x=>{
      this.dataService.token = x;
      console.log(x);
      this.router.navigateByUrl(`video-room/${this.roomName}`);
    });
  }
}
