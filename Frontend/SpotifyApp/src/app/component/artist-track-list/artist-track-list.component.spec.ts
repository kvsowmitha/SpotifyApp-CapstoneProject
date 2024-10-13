import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { of } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ChangeDetectorRef, ElementRef } from '@angular/core'; // Import ElementRef here
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { ArtistTrackListComponent } from './artist-track-list.component';
import { ApicallService } from '../../service/apiCall/apicall.service';
import { DataSharingService } from '../../service/dataSharingService/data-sharing.service';
import { SearchService } from '../../service/searchService/search.service';

describe('ArtistTrackListComponent', () => {
  let component: ArtistTrackListComponent;
  let fixture: ComponentFixture<ArtistTrackListComponent>;
  let apicallService: jasmine.SpyObj<ApicallService>;
  let searchService: jasmine.SpyObj<SearchService>;
  let dataSharingService: jasmine.SpyObj<DataSharingService>;
  let snackBar: jasmine.SpyObj<MatSnackBar>;
  let changeDetectorRef: jasmine.SpyObj<ChangeDetectorRef>;

  beforeEach(async () => {
    const apicallSpy = jasmine.createSpyObj('ApicallService', ['getTracksByArtist', 'getArtistById', 'addSongToFavorite']);
    const searchSpy = jasmine.createSpyObj('SearchService', ['searchInSongTrack']);
    
    // Updated mock to include changeUserdata
    const dataSharingSpy = jasmine.createSpyObj('DataSharingService', ['changeUserdata'], {
      currentData: of(''), // Mocking the search term observable
    });

    const snackBarSpy = jasmine.createSpyObj('MatSnackBar', ['open']);
    const cdrSpy = jasmine.createSpyObj('ChangeDetectorRef', ['markForCheck']);

    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        HttpClientTestingModule,
        MatSnackBarModule,
        ArtistTrackListComponent // Import the standalone component
      ],
      providers: [
        { provide: ApicallService, useValue: apicallSpy },
        { provide: SearchService, useValue: searchSpy },
        { provide: DataSharingService, useValue: dataSharingSpy },
        { provide: MatSnackBar, useValue: snackBarSpy },
        { provide: ChangeDetectorRef, useValue: cdrSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            paramMap: of({ get: (key: string) => (key === 'id' ? '1' : 'Test Artist') }) // Mocking route params
          },
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ArtistTrackListComponent);
    component = fixture.componentInstance;
    apicallService = TestBed.inject(ApicallService) as jasmine.SpyObj<ApicallService>;
    searchService = TestBed.inject(SearchService) as jasmine.SpyObj<SearchService>;
    dataSharingService = TestBed.inject(DataSharingService) as jasmine.SpyObj<DataSharingService>;
    snackBar = TestBed.inject(MatSnackBar) as jasmine.SpyObj<MatSnackBar>;
    changeDetectorRef = TestBed.inject(ChangeDetectorRef) as jasmine.SpyObj<ChangeDetectorRef>;

    fixture.detectChanges(); // Trigger initial data binding
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch artist details on initialization', () => {
    const mockArtistDetails = { id: 1, name: 'Test Artist', details: 'Some details' };
    apicallService.getArtistById.and.returnValue(of(mockArtistDetails));

    component.getArtistDetails('1');
    expect(apicallService.getArtistById).toHaveBeenCalledWith('1');
    expect(component.artistDetails).toEqual(mockArtistDetails);
  });

  it('should fetch tracks on initialization', () => {
    const mockTracks = [
      { audioUrl: 'url1', name: 'Track 1' },
      { audioUrl: 'url2', name: 'Track 2' },
    ];
    apicallService.getTracksByArtist.and.returnValue(of(mockTracks));

    component.getTracks('Test Artist');
    expect(apicallService.getTracksByArtist).toHaveBeenCalledWith('Test Artist');
    expect(component.tracks).toEqual(mockTracks);
    expect(component.customTracks).toEqual(mockTracks);
  });

  it('should pause the track', () => {
    component.audioPlayerRef = { nativeElement: jasmine.createSpyObj('audio', ['pause']) } as ElementRef;
    component.isPlaying = true;

    component.pauseTrack();
    expect(component.audioPlayerRef.nativeElement.pause).toHaveBeenCalled();
    expect(component.isPlaying).toBeFalse();
  });

  it('should add a track to favorites and show a snackbar', () => {
    const mockTrack = { songId: 1, name: 'Track 1', pictureUrl: 'pic.jpg', artistName: 'Artist 1', audioUrl: 'url.mp3' };
    const mockUserName = 'TestUser';
    component.userName = mockUserName;
    apicallService.addSongToFavorite.and.returnValue(of({}));

    component.addToFavorite(mockTrack);
    expect(apicallService.addSongToFavorite).toHaveBeenCalledWith(mockUserName, {
      MusicId: mockTrack.songId,
      musicName: mockTrack.name,
      pictureUrl: mockTrack.pictureUrl,
      singerName: mockTrack.artistName,
      songUrl: mockTrack.audioUrl,
    });
    expect(snackBar.open).toHaveBeenCalledWith('Song added in Favorite', 'Close', { duration: 3000 });
  });
});