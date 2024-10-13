import {
  ChangeDetectorRef,
  Component,
  Input,
  NgZone,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { DataSharingService } from '../../service/dataSharingService/data-sharing.service';

@Component({
  selector: 'app-music-player',
  standalone: true,
  imports: [MatIconModule],
  templateUrl: './music-player.component.html',
  styleUrl: './music-player.component.css',
})
export class MusicPlayerComponent implements OnInit, OnChanges, OnDestroy {
  constructor(
    private dataSharingService: DataSharingService,
    private cdRef: ChangeDetectorRef,
    private zone: NgZone
  ) {}

  @Input() songUrl: string = '';

  audio!: HTMLAudioElement;
  isPlaying = false;
  isMuted = false;
  currentTime: string = '0:00';
  duration: string = '0:00';
  progress = 0;

  currentSongIndex: number = -1;

  ngOnInit(): void {

    this.configureSong();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['songUrl'] && !changes['songUrl'].firstChange) {
      this.configureSong();
    }
  }
  configureSong() {
    if (this.audio) {
      this.audio.pause();
      this.audio.currentTime = 0; // Reset current time
    }
    this.audio = new Audio(this.songUrl);

    this.audio.ontimeupdate = () => this.updateProgress();
    this.audio.onloadedmetadata = () => {
      this.duration = this.formatTime(this.audio.duration);
      this.cdRef.detectChanges();
    };
    this.audio.play();
    this.isPlaying = true;
  }

  playSong() {
    this.audio.play();
    this.isPlaying = true;
  }

  pauseSong() {
    this.audio.pause();
    this.isPlaying = false;
  }

  togglePlay() {
    if (this.isPlaying) {
      this.pauseSong();
    } else {
      this.playSong();
    }
  }

  stopSong() {
    this.pauseSong();
    this.audio.currentTime = 0;
    this.progress = 0;
    this.cdRef.detectChanges();
  }

  toggleMute() {
    this.isMuted = !this.isMuted;
    this.audio.muted = this.isMuted;
    this.cdRef.detectChanges();
  }

  forward_10() {
    const newTime = this.audio.currentTime + 10; // Add 10 seconds
    if (newTime >= this.audio.duration) {
      // If new time exceeds duration, reset to the start
      this.audio.currentTime = 0;
    } else {
      this.audio.currentTime = newTime;
    }
    this.cdRef.detectChanges(); // Trigger UI update
  }

  replay_10() {
    const newTime = this.audio.currentTime - 10; // Subtract 10 seconds
    if (newTime <= 0) {
      // If new time is less than or equal to 0, reset to start
      this.audio.currentTime = 0;
    } else {
      this.audio.currentTime = newTime;
    }
    this.cdRef.detectChanges(); // Trigger UI update
  }

  toggleReplay() {
    this.configureSong();
  }

  seek(event: any) {
    const newValue = event.target.value;
    this.audio.currentTime = (newValue / 100) * this.audio.duration;
  }

  updateProgress() {
    // Run the update in Angular's zone so it triggers change detection
    this.zone.run(() => {
      this.progress = (this.audio.currentTime / this.audio.duration) * 100;
      this.currentTime = this.formatTime(this.audio.currentTime);
      this.cdRef.detectChanges(); // Trigger change detection for the progress bar
    });
  }

  formatTime(time: number): string {
    const minutes: number = Math.floor(time / 60);
    const seconds: number = Math.floor(time - minutes * 60);
    return `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
  }

  ngOnDestroy(): void {
    if (this.audio) {
      this.audio.pause();
      this.audio.currentTime = 0; // Reset current time
    }
  }
}
