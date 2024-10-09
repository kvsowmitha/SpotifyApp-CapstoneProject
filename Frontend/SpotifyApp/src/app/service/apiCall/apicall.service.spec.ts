import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ApicallService } from './apicall.service';

describe('ApicallService', () => {
  let service: ApicallService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    service = TestBed.inject(ApicallService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get the playlist album', () => {
    const mockResponse = [{ id: 1, title: 'Album 1' }, { id: 2, title: 'Album 2' }];

    service.getPlaylistAlbum().subscribe((response) => {
      expect(response).toEqual(mockResponse);
    });

    const req = httpMock.expectOne('http://localhost:5006/Album');
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });
});