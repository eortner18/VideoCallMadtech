import { Component, inject } from '@angular/core';
import { Login, TwillioService } from '../swagger';
import { Router } from '@angular/router';
import { DataServiceService } from '../data-service.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  router = inject(Router);
  userName:string = "";
  password:string = "";
  
  login:Login|undefined;
  
  constructor(public twilioService:TwillioService,public dataService:DataServiceService){
  
  }
  
  Login():void{
    if(this.userName.trim() != "" && this.password.trim()!=""){
      this.router.navigateByUrl("menu");
  
      // this.login = {userName:this.userName,password: this.password};
      // this.twilioService.madTechGetUserLoginGet(this.login).subscribe(x=>{
      //   if(x!= null){
      //     this.dataService.user.update(y=>x);
      //     this.router.navigateByUrl("menu");
      //   }
      // })
    }
  }
}
