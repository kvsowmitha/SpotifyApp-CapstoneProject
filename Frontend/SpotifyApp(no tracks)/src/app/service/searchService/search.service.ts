import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ApicallService } from '../apiCall/apicall.service';

@Injectable({
  providedIn: 'root',
})
export class SearchService {

  constructor(private apicallService: ApicallService) {
  }

  search(term: string, playlistData: any[]): Observable<any[]> {
    console.log(term);
    console.log(playlistData);

    const filteredData = playlistData.filter((item) =>
      item.musicName.toLowerCase().includes(term.toLowerCase()) || item.singerName.toLowerCase().includes(term.toLowerCase())
    );
    console.log(filteredData);
    return of(filteredData);
  }
}
