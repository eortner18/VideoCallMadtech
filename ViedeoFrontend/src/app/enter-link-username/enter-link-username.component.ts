import { Component, inject } from '@angular/core';
import { TwillioService } from '../swagger';
import { DataServiceService } from '../data-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-enter-link-username',
  templateUrl: './enter-link-username.component.html',
  styleUrls: ['./enter-link-username.component.scss']
})
export class EnterLinkUsernameComponent {
username:string = "";
router= inject(Router);

constructor(public twilioService:TwillioService,public dataService:DataServiceService){}


Join():void{
if(this.username.trim().length>0){
  this.dataService.username = this.username;
  this.router.navigateByUrl(`video-room/${this.dataService.roomName!}/${this.dataService.accessToken!}`);

}
}
}
