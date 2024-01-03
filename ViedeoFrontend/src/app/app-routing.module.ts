import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { MenuComponent } from './menu/menu.component';
import { ErrorPageComponent } from './error-page/error-page.component';
import { ViedoRoomComponent } from './viedo-room/viedo-room.component';

const routes: Routes = [
  {path:'',redirectTo:'/register',pathMatch:'full'},
  {path:'login',component:LoginComponent},
  {path:'register',component:RegisterComponent},
  {path:'menu',component:MenuComponent},
  {path:'video-room/:roomName',component:ViedoRoomComponent},
  {path:'**',component:ErrorPageComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
