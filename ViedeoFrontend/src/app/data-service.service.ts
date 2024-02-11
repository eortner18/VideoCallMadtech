import { Injectable, computed, inject, signal } from '@angular/core';
import { UserDto } from './swagger';
import { Room } from 'twilio-video';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class DataServiceService {
  router = inject(Router);

  constructor() { }

  user = signal<UserDto|null>(null);

  token:string = "";

  username:string = "";

  room:Room|null = null;

  accessToken:string = "";

  roomName:string = "";

  isLoggedIn:boolean = false;

  inRoom:boolean = this.isLoggedIn && true;

}
