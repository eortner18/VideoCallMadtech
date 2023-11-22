import { Component } from '@angular/core';
import { TwilioRooms, TwillioService } from '../swagger';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent {
  roomNames:TwilioRooms[]=[];
  accessToken:string = "";
  
  constructor(public twilioService:TwillioService){
    twilioService.madTechGetRoomsGet().subscribe(x=>this.roomNames = x);
  }
  
  Join(roomName:string):void{
  
  }
}
