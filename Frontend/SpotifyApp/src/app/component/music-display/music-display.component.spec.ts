import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { ChangeDetectorRef } from '@angular/core';
import { of } from 'rxjs';
import { UrlSegment } from '@angular/router';
import { MusicDisplayComponent } from './music-display.component';
import { SearchService } from '../../service/searchService/search.service';
import { DataSharingService } from '../../service/dataSharingService/data-sharing.service';
import { ApicallService } from '../../service/apiCall/apicall.service';
import { ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

describe('MusicDisplayComponent', () => {
  let component: MusicDisplayComponent;
  let fixture: ComponentFixture<MusicDisplayComponent>;
  let searchService: jasmine.SpyObj<SearchService>;
  let dataSharingService: jasmine.SpyObj<DataSharingService>;
  let apiService: jasmine.SpyObj<ApicallService>;
  let dialog: jasmine.SpyObj<MatDialog>;
  let cdr: jasmine.SpyObj<ChangeDetectorRef>;

  const mockActivatedRoute = {
    url: of([new UrlSegment('home', {})]),
  };

  beforeEach(waitForAsync(() => {
    const searchSpy = jasmine.createSpyObj('SearchService', ['search']);
    const dataSharingSpy = jasmine.createSpyObj('DataSharingService', [
      'changeUserdata', 
      'chnageSongData', 
      'currentData'
    ]);
    const apiServiceSpy = jasmine.createSpyObj('ApicallService', [
      'getPlaylistAlbum', 
      'getPlaylistArtist', 
      'getSongFavorite', 
      'addSongToFavorite', 
      'deleteFavoriteSongById'
    ]);
    const dialogSpy = jasmine.createSpyObj('MatDialog', ['open']);
    const cdrSpy = jasmine.createSpyObj('ChangeDetectorRef', ['markForCheck']);

    dataSharingSpy.isUserLoggedIn = of(true);
    dataSharingSpy.currentData = of('');

    TestBed.configureTestingModule({
      imports: [
        MatCardModule,
        MatButtonModule,
        MatDialogModule,
        MusicDisplayComponent
      ],
      providers: [
        { provide: SearchService, useValue: searchSpy },
        { provide: DataSharingService, useValue: dataSharingSpy },
        { provide: ApicallService, useValue: apiServiceSpy },
        { provide: MatDialog, useValue: dialogSpy },
        { provide: ChangeDetectorRef, useValue: cdrSpy },
        { provide: ActivatedRoute, useValue: mockActivatedRoute },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(MusicDisplayComponent);
    component = fixture.componentInstance;

    searchService = TestBed.inject(SearchService) as jasmine.SpyObj<SearchService>;
    dataSharingService = TestBed.inject(DataSharingService) as jasmine.SpyObj<DataSharingService>;
    apiService = TestBed.inject(ApicallService) as jasmine.SpyObj<ApicallService>;
    dialog = TestBed.inject(MatDialog) as jasmine.SpyObj<MatDialog>;
    cdr = TestBed.inject(ChangeDetectorRef) as jasmine.SpyObj<ChangeDetectorRef>;
  }));

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });


  it('should fetch artist playlist on artist route', () => {
    const mockArtistData = [{ id: 1, name: 'Artist 1' }];
    apiService.getPlaylistArtist.and.returnValue(of(mockArtistData));

    const mockUrlSegment = [new UrlSegment('artist', {})];
    mockActivatedRoute.url = of(mockUrlSegment);

    component.ngOnInit();

    expect(component.currentUrlSegment).toBe('artist');
    expect(apiService.getPlaylistArtist).toHaveBeenCalled();
    expect(component.getDataResult).toEqual(mockArtistData);
  });

  it('should open Signin dialog if user is not logged in while playing a song', () => {
    component.isLoggedIn = false;
    const mockSongUrl = 'song-url';
    dialog.open.and.returnValue({
      afterClosed: () => of(true),
    } as any);

    component.playSong(mockSongUrl);

    expect(dialog.open).toHaveBeenCalled();
    expect(dataSharingService.changeUserdata).toHaveBeenCalled();
  });
});