import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class WebcamService {

  async getMediaStream(): Promise<MediaStream> {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({ video: true });
      return stream;
    } catch (error) {
      console.error('Error accessing webcam:', error);
      throw error;
    }
  }
}
