import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ErrorPageComponent } from './error-page/error-page.component';
import { LoginComponent } from './login/login.component';
import { MenuComponent } from './menu/menu.component';
import { RegisterComponent } from './register/register.component';
import { ViedoRoomComponent } from './viedo-room/viedo-room.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { envrionment } from './envrionments/environment';
import { BASE_PATH } from './swagger';
import { WebcamModule } from 'ngx-webcam';
import { withComponentInputBinding } from '@angular/router';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    MenuComponent,
    ErrorPageComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    WebcamModule, 
    ReactiveFormsModule
  ],
  providers: [
    {provide:BASE_PATH,useValue:envrionment.apiRoot}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
