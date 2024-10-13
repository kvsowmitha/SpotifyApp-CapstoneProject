import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MusicPlayerComponent } from './music-player.component';
import { DataSharingService } from '../../service/dataSharingService/data-sharing.service';

describe('MusicPlayerComponent', () => {
  let component: MusicPlayerComponent;
  let fixture: ComponentFixture<MusicPlayerComponent>;
  let dataSharingService: jasmine.SpyObj<DataSharingService>;

  // Mock Audio element
  let mockAudio: any;

  beforeEach(() => {
    // Create a mock for DataSharingService
    dataSharingService = jasmine.createSpyObj('DataSharingService', ['']);

    mockAudio = {
      play: jasmine.createSpy('play').and.callFake(() => {
        component.isPlaying = true;
        return Promise.resolve();
      }),
      pause: jasmine.createSpy('pause').and.callFake(() => {
        component.isPlaying = false;
      }),
      currentTime: 0,
      duration: 100,
      muted: false,
    };

    TestBed.configureTestingModule({
      imports: [MusicPlayerComponent],
      providers: [
        { provide: DataSharingService, useValue: dataSharingService },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(MusicPlayerComponent);
    component = fixture.componentInstance;

    component.audio = mockAudio;
  });

  it('should toggle play and pause the song', async () => {
    expect(component.isPlaying).toBeFalse();

    await component.togglePlay();
    expect(component.isPlaying).toBeTrue();
    expect(component.audio.play).toHaveBeenCalled();

    await component.togglePlay();
    expect(component.isPlaying).toBeFalse();
    expect(component.audio.pause).toHaveBeenCalled();
  });

  it('should pause the song and set isPlaying to false', () => {
    component.playSong();
    component.pauseSong();

    // Verify that pause was called
    expect(component.audio.pause).toHaveBeenCalled();
    expect(component.isPlaying).toBeFalse();
  });

  it('should toggle mute state', () => {
    component.playSong();

    component.toggleMute();
    expect(component.isMuted).toBeTrue();
    expect(component.audio.muted).toBeTrue();

    component.toggleMute();
    expect(component.isMuted).toBeFalse();
    expect(component.audio.muted).toBeFalse();
  });
});