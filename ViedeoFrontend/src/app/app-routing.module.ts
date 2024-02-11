import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { MenuComponent } from './menu/menu.component';
import { ErrorPageComponent } from './error-page/error-page.component';
import { ViedoRoomComponent } from './viedo-room/viedo-room.component';
import { EnterLinkUsernameComponent } from './enter-link-username/enter-link-username.component';

const routes: Routes = [
  {path:'',redirectTo:'/login',pathMatch:'full'},
  {path:'login',component:LoginComponent},
  {path:'register',component:RegisterComponent},
  {path:'menu',component:MenuComponent},
  {path:'video-room/:roomName/:accessToken',component:ViedoRoomComponent},
  {path:'enter-link-username',component:EnterLinkUsernameComponent},
  {path:'**',component:ErrorPageComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes,{bindToComponentInputs:true})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
