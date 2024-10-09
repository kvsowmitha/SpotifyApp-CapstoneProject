import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class FavoriteService {
  private favoriteapi = `http://localhost:5221/api/Music/favorites/add/`;
  constructor(private http: HttpClient) {}
}
