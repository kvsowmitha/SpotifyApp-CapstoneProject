import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { AlbumTrackListComponent } from './album-track-list.component';
import { ApicallService } from '../../service/apiCall/apicall.service';
import { DataSharingService } from '../../service/dataSharingService/data-sharing.service';
import { SearchService } from '../../service/searchService/search.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NoopAnimationsModule } from '@angular/platform-browser/animations'; // Import NoopAnimationsModule
import { of } from 'rxjs';

describe('AlbumTrackListComponent', () => {
  let component: AlbumTrackListComponent;
  let fixture: ComponentFixture<AlbumTrackListComponent>;

  const mockApicallService = {
    getAlbumById: jasmine.createSpy('getAlbumById').and.returnValue(of({})),
    getTracksByAlbum: jasmine.createSpy('getTracksByAlbum').and.returnValue(of([])),
    addSongToFavorite: jasmine.createSpy('addSongToFavorite').and.returnValue(of({})),
  };

  const mockDataSharingService = {
    changeUserdata: jasmine.createSpy('changeUserdata'),
    currentData: of(''),
  };

  const mockSearchService = {
    searchInSongTrack: jasmine.createSpy('searchInSongTrack').and.returnValue(of([])),
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        NoopAnimationsModule,
        AlbumTrackListComponent,
      ],
      providers: [
        { provide: ActivatedRoute, useValue: { paramMap: of(convertToParamMap({ id: '1', name: 'Album Name' })) } },
        { provide: ApicallService, useValue: mockApicallService },
        { provide: DataSharingService, useValue: mockDataSharingService },
        { provide: SearchService, useValue: mockSearchService },
        MatSnackBar,
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(AlbumTrackListComponent);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with correct parameters', () => {
    fixture.detectChanges();
    expect(component.albumId).toBe(1);
    expect(component.albumName).toBe('Album Name');
  });

  it('should fetch album details on initialization', () => {
    fixture.detectChanges();
    expect(mockApicallService.getAlbumById).toHaveBeenCalledWith('1');
  });

  it('should fetch tracks on initialization', () => {
    fixture.detectChanges();
    expect(mockApicallService.getTracksByAlbum).toHaveBeenCalledWith('Album Name');
  });

  it('should handle adding a track to favorites', () => {
    const musicTrack = { songId: 1, name: 'Test Song', pictureUrl: '', artistName: 'Artist', audioUrl: '' };
    component.addToFavorite(musicTrack);
    expect(mockApicallService.addSongToFavorite).toHaveBeenCalled();
  });
});