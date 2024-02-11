import { Component, inject } from '@angular/core';
import { LanguageDto, Register, TwillioService } from '../swagger';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  countrys:LanguageDto[]=[];
  router = inject(Router);

  EingabeFalsch:boolean = false;

  firstName:string = "";
  lastName:string = "";
  password:string = "";
  password2:string = "";
  userName:string = "";
  mail:string = "";
  selectedCountry:string = "";
  userExist:boolean = false;

  register:Register|undefined;

  checkFirstname():boolean{
    let pattern:RegExp = /^[A-Z]{1,1}[a-z]{3,}$/g;
    return !pattern.test(this.firstName);
  }

  checkLastName():boolean{
    const pattern = /[A-Z]{1,1}[a-z]{3,}/g;
    return !pattern.test(this.lastName);
  }

  checkPssword():boolean{
    let pattern:RegExp = /^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])[a-zA-Z0-9]{8,}$/g;
    return !pattern.test(this.password);
  }

  checkPssword2():boolean{
    return !(this.password == this.password2);
  }

  checkUsername():boolean{
    let pattern:RegExp = /^\w{4,}$/g;
    return !pattern.test(this.userName);
  }

  checkMail():boolean{
    let pattern:RegExp = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/g;
    return !pattern.test(this.mail);
  }

  
  checkAll():boolean{
    if(this.checkFirstname()){
      return true;
    }
    if(this.checkLastName()){
      return true;
    }
    if(this.checkPssword()){
      return true;
    }
    if(this.checkPssword2()){
      return true;
    }
    if(this.checkMail()){
      return true;
    }
    if(this.checkUsername()){
      return true;
    }
    return false;
  }

  constructor(public twillioService:TwillioService){
    twillioService.madTechGetLanguagesGet().subscribe(x=>{
      this.countrys=x;
      this.selectedCountry = x[0].countryName!; 
    });
  }

  Registrieren():void{
   
        if(!this.checkAll()){
          this.register = {countryName:this.selectedCountry,firstName:this.firstName,lastName:this.lastName,mail:this.mail,password:this.password,userName:this.userName};
        this.twillioService.madTechAddUserPost(this.register).subscribe(x=>{
          if(!x){
            this.userExist = true;
          } 
          else{
            this.router.navigateByUrl("login");

          }
        });
      
      console.log("registrieren");
        }
        else{
          this.EingabeFalsch = true;
        }
    
  }
}
