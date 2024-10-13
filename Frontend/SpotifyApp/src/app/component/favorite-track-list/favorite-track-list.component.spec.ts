import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { FavoriteTrackListComponent } from './favorite-track-list.component';
import { ApicallService } from '../../service/apiCall/apicall.service';
import { DataSharingService } from '../../service/dataSharingService/data-sharing.service';
import { SearchService } from '../../service/searchService/search.service';
import { of, throwError } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HttpClientModule } from '@angular/common/http';

class MockRouter {
  navigate = jasmine.createSpy('navigate');
}

class MockApicallService {
  getSongFavorite = jasmine.createSpy('getSongFavorite').and.returnValue(of([])); // Mocking API response
  deleteFavoriteSongById = jasmine.createSpy('deleteFavoriteSongById').and.returnValue(of({})); // Mocking delete response
}

class MockDataSharingService {
  currentData = of(''); // Mocking current data observable
}

class MockSearchService {
  searchInSong = jasmine.createSpy('searchInSong').and.returnValue(of([])); // Mocking search response
}

class MockActivatedRoute {}

describe('FavoriteTrackListComponent', () => {
  let component: FavoriteTrackListComponent;
  let fixture: ComponentFixture<FavoriteTrackListComponent>;
  let apiService: MockApicallService;
  let router: MockRouter;
  let dataSharingService: MockDataSharingService;
  let searchService: MockSearchService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormsModule, HttpClientModule, FavoriteTrackListComponent],
      providers: [
        { provide: Router, useClass: MockRouter },
        { provide: ApicallService, useClass: MockApicallService },
        { provide: DataSharingService, useClass: MockDataSharingService },
        { provide: SearchService, useClass: MockSearchService },
        { provide: ActivatedRoute, useClass: MockActivatedRoute },
        MatSnackBar // Add MatSnackBar to providers
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(FavoriteTrackListComponent);
    component = fixture.componentInstance;
    apiService = TestBed.inject(ApicallService) as unknown as MockApicallService;
    router = TestBed.inject(Router) as unknown as MockRouter;
    dataSharingService = TestBed.inject(DataSharingService) as unknown as MockDataSharingService;
    searchService = TestBed.inject(SearchService) as unknown as MockSearchService;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch favorite songs on init', () => {
    component.userName = 'testUser';
    component.ngOnInit();
    expect(apiService.getSongFavorite).toHaveBeenCalledWith(component.userName);
  });

  it('should perform search based on search term', () => {
    component.customTracks = [{ songUrl: 'track1.mp3' }, { songUrl: 'track2.mp3' }];
    const searchTerm = 'track1';
    dataSharingService.currentData = of(searchTerm);
    
    component.ngOnInit(); // Call ngOnInit to subscribe to the search term
    
    expect(searchService.searchInSong).toHaveBeenCalledWith(searchTerm, component.customTracks);
  });

  it('should reset tracks when search term is empty', () => {
    component.customTracks = [{ songUrl: 'track1.mp3' }, { songUrl: 'track2.mp3' }];
    dataSharingService.currentData = of(''); // Empty search term
    component.ngOnInit();
    
    expect(component.tracks).toEqual(component.customTracks); // Tracks should reset to customTracks
  });
});