import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { SearchService } from './search.service';
import { ApicallService } from '../apiCall/apicall.service';

describe('SearchService', () => {
  let service: SearchService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ApicallService] 
    });
    service = TestBed.inject(SearchService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return an empty array if no matches are found', () => {
    const playlistData = [
      { musicName: 'Song A', singerName: 'Singer A' },
      { musicName: 'Song B', singerName: 'Singer B' },
    ];

    const searchTerm = 'Non-existent Song';
    service.search(searchTerm, playlistData).subscribe((result) => {
      expect(result).toEqual([]);
    });
  });
});