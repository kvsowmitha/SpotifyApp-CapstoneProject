import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';
import { ChangeDetectorRef } from '@angular/core';
import { ArtistListComponent } from './artist-list.component';
import { ApicallService } from '../../service/apiCall/apicall.service';
import { DataSharingService } from '../../service/dataSharingService/data-sharing.service';
import { SearchService } from '../../service/searchService/search.service';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';

describe('ArtistListComponent', () => {
  let component: ArtistListComponent;
  let fixture: ComponentFixture<ArtistListComponent>;
  let apicallService: jasmine.SpyObj<ApicallService>;
  let searchService: jasmine.SpyObj<SearchService>;
  let dataSharingService: jasmine.SpyObj<DataSharingService>;
  let changeDetectorRef: jasmine.SpyObj<ChangeDetectorRef>;
  let router: Router;

  beforeEach(async () => {
    const apicallSpy = jasmine.createSpyObj('ApicallService', ['getArtists']);
    const searchSpy = jasmine.createSpyObj('SearchService', ['search']);
    const dataSharingSpy = jasmine.createSpyObj('DataSharingService', [], {
      currentData: of(''),
    });
    const cdrSpy = jasmine.createSpyObj('ChangeDetectorRef', ['markForCheck']);

    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        CommonModule,
        MatCardModule,
        ArtistListComponent, // Importing the standalone component
      ],
      providers: [
        { provide: ApicallService, useValue: apicallSpy },
        { provide: SearchService, useValue: searchSpy },
        { provide: DataSharingService, useValue: dataSharingSpy },
        { provide: ChangeDetectorRef, useValue: cdrSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            url: of([{ path: 'artist' }]), // Mock ActivatedRoute URL for testing
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ArtistListComponent);
    component = fixture.componentInstance;
    apicallService = TestBed.inject(ApicallService) as jasmine.SpyObj<ApicallService>;
    searchService = TestBed.inject(SearchService) as jasmine.SpyObj<SearchService>;
    dataSharingService = TestBed.inject(DataSharingService) as jasmine.SpyObj<DataSharingService>;
    changeDetectorRef = TestBed.inject(ChangeDetectorRef) as jasmine.SpyObj<ChangeDetectorRef>;
    router = TestBed.inject(Router); // Inject the router here
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize and fetch artists on ngOnInit', () => {
    const mockArtists = [
      { id: 1, name: 'Artist 1' },
      { id: 2, name: 'Artist 2' },
    ];

    apicallService.getArtists.and.returnValue(of(mockArtists));
    fixture.detectChanges();

    expect(apicallService.getArtists).toHaveBeenCalled();
    expect(component.originalArtist).toEqual(mockArtists);
    expect(component.customArtist).toEqual(mockArtists);
  });

  it('should navigate to artist tracks if logged in', () => {
    spyOn(router, 'navigate'); // Spy on the injected router's navigate method
    component.isLoggedIn = true;

    component.goToTracks(1, 'Artist 1');

    expect(router.navigate).toHaveBeenCalledWith(['/artist-tracks', 1, 'Artist 1']);
  });

  it('should show alert if not logged in and try to navigate to artist tracks', () => {
    spyOn(window, 'alert');
    component.isLoggedIn = false;

    component.goToTracks(1, 'Artist 1');

    expect(window.alert).toHaveBeenCalledWith('Login first');
  });
});