import { Component } from '@angular/core';
import { LanguageDto, Register, TwillioService } from '../swagger';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  countrys:LanguageDto[]=[];

  firstName:string = "";
  lastName:string = "";
  password:string = "";
  password2:string = "";
  userName:string = "";
  mail:string = "";
  selectedCountry:string = "";

  register:Register|undefined;


  constructor(public twillioService:TwillioService){
    twillioService.madTechGetLanguagesGet().subscribe(x=>this.countrys=x);
  }

  Registrieren():void{
    if(this.firstName.trim()!= "" && this.lastName.trim()!= "" &&this.userName.trim()!= "" &&
    this.password.trim()!= "" &&this.password2.trim()!= "" &&this.mail.trim()!= "" &&
    this.selectedCountry.trim()!= ""){
      if(this.password == this.password2){
        this.register = {countryName:this.selectedCountry,firstName:this.firstName,lastName:this.lastName,mail:this.mail,password:this.password,userName:this.userName};
        this.twillioService.madTechAddUserPost(this.register).subscribe(x=>console.log(x));
      }
    }
    else{
      console.log("Daten befüllen!");
      console.log(this.firstName.trim());
      console.log(this.lastName.trim());
      console.log(this.mail.trim());
      console.log(this.userName.trim());
      console.log(this.password.trim());
      console.log(this.password2.trim());
      console.log(this.selectedCountry.trim());
    }
  }
}
