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

  wrongInput:boolean = false;
  
  login:Login|undefined;

  checkPssword():boolean{
    let pattern:RegExp = /^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])[a-zA-Z0-9]{8,}$/g;
    return !pattern.test(this.password);
  }

  checkUsername():boolean{
    let pattern:RegExp = /^(?:[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}|[a-zA-Z0-9]{4,})$/g;
    return !pattern.test(this.userName);
  }

  checkAll():boolean{
    if(this.checkUsername()){
      return true;
    }
    if(this.checkPssword()){
      return true;
    }
    return false;
  }
  
  constructor(public twilioService:TwillioService,public dataService:DataServiceService){
  
  }
  
  Login():void{
      // this.router.navigateByUrl("menu");
  
      this.login = {userName:this.userName,password: this.password};
      this.twilioService.madTechGetUserLoginPost(this.login!).subscribe(x=>{
        if(x!= null){
          this.dataService.user.set(x);
          this.router.navigateByUrl("menu");
        }
        else{
          this.wrongInput = true;
        }
      });
      console.log("login");
    
  }
}
