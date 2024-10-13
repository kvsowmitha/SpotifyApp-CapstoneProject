import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ApicallService {
  private apiUrlAlbum = 'http://localhost:5006/Album';
  private apiUrlArtist = 'http://localhost:5006/Artist';
  private apiUrlAddFavorite = 'http://localhost:5006/music/favorites/add';
  private apiUrlGetFavorite = 'http://localhost:5006/music/favorites';
  private apiUrlDeleteFavorite = 'http://localhost:5006/music/favorites/remove';
  private sendOtpUrl = 'http://localhost:5006/Auth/forgot-password';
  private resetUserPasswordUrl ='http://localhost:5006/Auth/reset-password';
  private apiUrlgetProfile = 'http://localhost:5006/user/email';


  constructor(private http: HttpClient) {}

  getPlaylistAlbum(): Observable<any> {
    return this.http.get<any>(this.apiUrlAlbum);
  }
  getPlaylistArtist(): Observable<any> {
    return this.http.get<any>(this.apiUrlArtist);
  }
  addSongToFavorite(userName: any, data: any): Observable<any> {
    return this.http.post(`${this.apiUrlAddFavorite}/${userName}`, data);
  }
  getSongFavorite(userName: any): Observable<any> {
    return this.http.get<any>(`${this.apiUrlGetFavorite}/${userName}`);
  }
  deleteFavoriteSongById(userName: any, musicId: any): Observable<any> {
    return this.http.delete(`${this.apiUrlDeleteFavorite}/${userName}/${musicId}`);
  }
  // getUserProfile(userName: any): Observable<any> {
  //    return this.http.get<any>(${this.apiUrlgetProfile}/${userName});
  //  }
  sendToOTP(data: any): Observable<any> {
    return this.http.post(this.sendOtpUrl, data);
  }
  resetUserPassword(data: any): Observable<any> {
    return this.http.post(this.resetUserPasswordUrl, data);
  }
}
