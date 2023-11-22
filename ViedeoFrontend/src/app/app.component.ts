import { Component } from '@angular/core';
import { DataServiceService } from './data-service.service';
import { TwillioService } from './swagger';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ViedeoFrontend';

  constructor(public myData:DataServiceService,twillioService:TwillioService){
    // console.log(myData.isLoggedIn());
  }
}
