import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { AlbumListComponent } from './album-list.component';
import { ApicallService } from '../../service/apiCall/apicall.service';
import { DataSharingService } from '../../service/dataSharingService/data-sharing.service';
import { SearchService } from '../../service/searchService/search.service';
import { of } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

class MockRouter {
  navigate = jasmine.createSpy('navigate');
}

class MockApicallService {
  getAlbums = jasmine.createSpy('getAlbums').and.returnValue(of([]));
}

class MockDataSharingService {
  currentData = of('');
}

class MockSearchService {
  search = jasmine.createSpy('search').and.returnValue(of([]));
}

class MockActivatedRoute {
  url = of([{ path: 'album' }]);
}

describe('AlbumListComponent', () => {
  let component: AlbumListComponent;
  let fixture: ComponentFixture<AlbumListComponent>;
  let router: MockRouter;
  let apiService: MockApicallService;
  let dataSharingService: MockDataSharingService;
  let searchService: MockSearchService;
  let activatedRoute: MockActivatedRoute;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AlbumListComponent],
      providers: [
        { provide: Router, useClass: MockRouter },
        { provide: ApicallService, useClass: MockApicallService },
        { provide: DataSharingService, useClass: MockDataSharingService },
        { provide: SearchService, useClass: MockSearchService },
        { provide: ActivatedRoute, useClass: MockActivatedRoute }, // Providing the mock ActivatedRoute
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(AlbumListComponent);
    component = fixture.componentInstance;
    router = TestBed.inject(Router) as unknown as MockRouter;
    apiService = TestBed.inject(ApicallService) as unknown as MockApicallService;
    dataSharingService = TestBed.inject(DataSharingService) as unknown as MockDataSharingService;
    searchService = TestBed.inject(SearchService) as unknown as MockSearchService;
    activatedRoute = TestBed.inject(ActivatedRoute) as unknown as MockActivatedRoute; // Inject the mock
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch albums on initialization', () => {
    component.ngOnInit();
    expect(apiService.getAlbums).toHaveBeenCalled();
  });

  it('should navigate to tracks when logged in', () => {
    component.isLoggedIn = true; // Set isLoggedIn to true
    component.goToTracks(1, 'Album Name'); // Sample albumId and name
    expect(router.navigate).toHaveBeenCalledWith(['/albumtracks', 1, 'Album Name']);
  });

  it('should alert if user is not logged in when navigating to tracks', () => {
    spyOn(window, 'alert'); // Spy on the alert function
    component.isLoggedIn = false; // Set isLoggedIn to false
    component.goToTracks(1, 'Album Name'); // Sample albumId and name
    expect(window.alert).toHaveBeenCalledWith('Login first');
  });

  it('should update originalAlbum when a search term is provided', () => {
    component.customAlbums = [{ id: 1, name: 'Album 1' }, { id: 2, name: 'Album 2' }];
    dataSharingService.currentData = of('Album 1'); // Simulating a search term
    component.ngOnInit(); // Re-initialize to subscribe to search term
    expect(searchService.search).toHaveBeenCalledWith('Album 1', component.customAlbums);
  });
});