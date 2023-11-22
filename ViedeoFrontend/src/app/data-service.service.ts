import { Injectable, computed, signal } from '@angular/core';
import { UserDto } from './swagger';

@Injectable({
  providedIn: 'root'
})
export class DataServiceService {

  constructor() { }

  user = signal<UserDto|null>(null);

  isLoggedIn = computed(()=>{
    if(this.user() != null){
      return true;
    }
    return false;
  })
}
