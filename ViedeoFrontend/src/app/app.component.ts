import { Component, inject } from '@angular/core';
import { DataServiceService } from './data-service.service';
import { TwillioService } from './swagger';
import { Router } from '@angular/router';

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
    // this.twillioService.madTechLogOutPost(this.myData.user()!);
    // this.myData.user.set(null);
    // this.myData.roomName = "";
    // this.myData.token = "";
    this.myData.user.set(null);

    this.router.navigateByUrl("login");
  }
}
