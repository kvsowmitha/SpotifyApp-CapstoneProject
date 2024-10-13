import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, ElementRef, Inject, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApicallService } from '../../service/apiCall/apicall.service';
import { DataSharingService } from '../../service/dataSharingService/data-sharing.service';
import { SearchService } from '../../service/searchService/search.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatIcon } from '@angular/material/icon';
import { MatCard, MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog'; // Import MatDialog
@Component({
  selector: 'app-favorite-track-list',
  standalone: true,
  imports: [CommonModule,FormsModule,MatIcon,MatCard,MatCardModule],
  templateUrl: './favorite-track-list.component.html',
  styleUrl: './favorite-track-list.component.css'
})
export class FavoriteTrackListComponent {
  @ViewChild('audioPlayer') audioPlayerRef!: ElementRef<HTMLAudioElement>;
  tracks: any[] = [];
  customTracks: any[] = [];
  albumDetails: any | null = null; // Variable to store album details
  albumId: number | null = null;
  albumName: String | undefined;
  currentTrackIndex: number = -1; // Initialize current track index
  isPlaying: boolean = false; // Track playing state
  duration: number = 0; // Track duration
  currentTime: number = 0; // Current time of the track
  isPlayTrack = false;
  userName: any;
  isMuted: boolean = false; // Variable to track mute stat
 
  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private apicall: ApicallService,
    private cdRef: ChangeDetectorRef,
    private router: Router,
    private dataSharingService: DataSharingService,
    private searchService: SearchService,
    private snackBar: MatSnackBar, // Inject MatSnackBar service,
    private dialog: MatDialog // Inject MatDialog
  ) {}
 
  ngOnInit(): void {
    this.userName = localStorage.getItem('userName');
    this.getAllFavoriteSong(this.userName); // Fetch all Favorite song
 
    // Listen for changes in the search term from the NavBar
 
    this.dataSharingService.currentData.subscribe((searchTerm: string) => {
      console.log(searchTerm);
      if (searchTerm) {
        this.searchService
          .searchInSong(searchTerm, this.customTracks)
          .subscribe((data: any) => {
            console.log(data);
            this.tracks = data;
            this.cdRef.markForCheck();
          });
      } else {
        this.tracks = this.customTracks;
        this.cdRef.markForCheck();
      }
    });
  }
 
  getAllFavoriteSong(userName: string): void {
    this.apicall.getSongFavorite(userName).subscribe((res: any[]) => {
      console.log(res);
      this.tracks = res;
      this.customTracks = res;
      this.cdRef.markForCheck();
      console.log(this.tracks);
      console.log(this.customTracks);
    });
  }
 
  playTrack(url: string, index: number): void {
    this.isPlayTrack = true;
    this.cdRef.detectChanges();
    const audioElement = this.audioPlayerRef.nativeElement;
    audioElement.src = url;
    console.log(url);
    audioElement.play();
    console.log(`Playing track: ${url} at index: ${index}`); // Log current track
    this.isPlaying = true;
    this.currentTrackIndex = index;
    this.duration = audioElement.duration;
  }
 
  pauseTrack(): void {
    const audioElement = this.audioPlayerRef.nativeElement;
    audioElement.pause();
    this.isPlaying = false;
  }
 
  playCurrentTrack(): void {
    const audioElement = this.audioPlayerRef.nativeElement;
    audioElement.play();
    this.isPlaying = true;
  }
 
  nextTrack(): void {
    if (this.tracks.length === 0) return; // Guard against empty tracks
    // If at the last track, play the first track
    this.currentTrackIndex = (this.currentTrackIndex + 1) % this.tracks.length;
    console.log('Next Track Index:', this.currentTrackIndex); // Log next track index
    this.playTrack(
      this.tracks[this.currentTrackIndex].songUrl,
      this.currentTrackIndex
    );
  }
 
  prevTrack(): void {
    if (this.tracks.length === 0) return; // Guard against empty tracks
    // If at the first track, play the last track
    this.currentTrackIndex =
      (this.currentTrackIndex - 1 + this.tracks.length) % this.tracks.length;
    console.log('Previous Track Index:', this.currentTrackIndex); // Log previous track index
    this.playTrack(
      this.tracks[this.currentTrackIndex].songUrl,
      this.currentTrackIndex
    );
  }
  updateProgress(): void {
    const audioElement = this.audioPlayerRef.nativeElement;
    this.currentTime = audioElement.currentTime;
    this.duration = audioElement.duration || 0;
  }
 
  seekTrack(event: any): void {
    const audioElement = this.audioPlayerRef.nativeElement;
    audioElement.currentTime = event.target.value;
  }
  muteTrack(): void {
    const audioElement = this.audioPlayerRef.nativeElement;
    this.isMuted = !this.isMuted; // Toggle mute state
    audioElement.muted = this.isMuted; // Mute or unmute the audio
  }
 
  replayTrack(): void {
    const audioElement = this.audioPlayerRef.nativeElement;
    audioElement.currentTime = 0; // Reset current time to the start
    audioElement.play(); // Play from the beginning
    this.isPlaying = true; // Update playing state
  }
  formatTime(seconds: number): string {
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = Math.floor(seconds % 60);
    return `${minutes}:${remainingSeconds < 10 ? '0' : ''}${remainingSeconds}`;
  }
  // removeToFavorite(musicId: any) {
  //   console.log(musicId);
  //   this.apicall
  //     .deleteFavoriteSongById(this.userName, musicId)
  //     .subscribe((res) => {
  //       this.apicall.getSongFavorite(this.userName).subscribe((res: any[]) => {
  //         console.log(res);
  //         this.tracks = res;
  //         this.cdRef.markForCheck();
  //       });
  //     });
 
  //   //alert('Favorite song remove');
  //   this.snackBar.open('Favorite song remove', 'Close', { duration: 3000 });
  // }
  removeToFavorite(musicId: any): void {
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '300px',
      data: { message: 'Are you sure you want to remove this song from favorites?' }, // Pass data to dialog
    });
    dialogRef.afterClosed().subscribe((confirmed: boolean) => {
      if (confirmed) {
        console.log(musicId);
        this.apicall
          .deleteFavoriteSongById(this.userName, musicId)
          .subscribe((res) => {
            this.apicall.getSongFavorite(this.userName).subscribe((res: any[]) => {
              console.log(res);
              this.tracks = res;
              this.cdRef.markForCheck();
            });
          });
        this.snackBar.open('Favorite song removed', 'Close', { duration: 3000 });
      }
    });
  }
}
 
// Create an inline dialog component
@Component({
  selector: 'dialog-component',
  template: `
<div class="main">
<h1 mat-dialog-title class="dialog-title">Confirm</h1>
<div mat-dialog-content class="dialog-content">
<p>{{ data.message }}</p>
</div>
<div mat-dialog-actions class="dialog-actions">
<button mat-button (click)="onNoClick()" class="cancel-button">No</button>
<button mat-button (click)="onConfirm()" color="primary" class="confirm-button">Yes</button>
</div>
</div>
  `,
  styles: [`
    .main
    {
     background-color: black;
    }
    .dialog-title {
      text-align: center;
      font-size: 20px;
      font-weight: bold;
      color: white;
    }
    .dialog-content {
      padding: 20px;
      text-align: center;
      font-size: 16px;
      color: white;
    }
    .dialog-actions {
      display: flex;
      justify-content: center;
      padding: 10px;
    }
    .cancel-button {
      color: red;
    }
    .confirm-button {
      margin-left: 10px;
    }
  `]
})
export class DialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { message: string }
  ) {}
 
  onNoClick(): void {
    this.dialogRef.close(false);
  }
 
  onConfirm(): void {
    this.dialogRef.close(true);
  }
}