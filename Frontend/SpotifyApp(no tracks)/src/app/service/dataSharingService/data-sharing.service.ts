import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DataSharingService {
  constructor() {}

  private dataSource = new BehaviorSubject<string>('');
  currentData = this.dataSource.asObservable();

  private userData = new BehaviorSubject<boolean>(false);
  isUserLoggedIn = this.userData.asObservable();

  private dataSourceSong = new BehaviorSubject<string>('');
  currentDataSong = this.dataSourceSong.asObservable();

  changeData(data: string) {
    this.dataSource.next(data);
  }
  chnageSongData(songUrl: string) {
    this.dataSourceSong.next(songUrl);
  }

  changeUserdata(isLoggedIn: boolean) {
    this.userData.next(isLoggedIn);
  }
}
