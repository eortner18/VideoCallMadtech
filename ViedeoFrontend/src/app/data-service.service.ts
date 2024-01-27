import { Injectable, computed, signal } from '@angular/core';
import { UserDto } from './swagger';
import { Room } from 'twilio-video';

@Injectable({
  providedIn: 'root'
})
export class DataServiceService {

  constructor() { }

  user = signal<UserDto|null>(null);

  token:string = "";

  username:string = "";

  room:Room|null = null;

  accessToken:string = "";

  roomName:string = "";

  isLoggedIn = computed(()=>{
    if(this.user() != null){
      return true;
    }
    return false;
  })
}
