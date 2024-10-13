import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatIcon } from '@angular/material/icon';
import { ActivatedRoute, Router } from '@angular/router';
import { ApicallService } from '../../service/apiCall/apicall.service';
import { DataSharingService } from '../../service/dataSharingService/data-sharing.service';
import { SearchService } from '../../service/searchService/search.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
 
@Component({
  selector: 'app-artist-track-list',
  standalone: true,
  imports: [CommonModule,FormsModule,MatIcon],
  templateUrl: './artist-track-list.component.html',
  styleUrl: './artist-track-list.component.css'
})
export class ArtistTrackListComponent implements OnInit {
  @ViewChild('audioPlayer') audioPlayerRef!: ElementRef<HTMLAudioElement>;
  tracks: any[] = [];
  customTracks: any[] = [];
  artistDetails: any | null = null; // Variable to store album details
  artistId: number | null = null;
  artistName: String | undefined;
  currentTrackIndex: number = -1; // Initialize current track index
  isPlaying: boolean = false; // Track playing state
  duration: number = 0; // Track duration
  currentTime: number = 0; // Current time of the track
  isPlayTrack = false;
  userName: any;
  isLoggedIn = false;
  isMuted: boolean = false; // Variable to track mute state
 
  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private apicall: ApicallService,
    private dataSharingService: DataSharingService,
    private cdRef: ChangeDetectorRef,
    private router: Router,
    private searchService: SearchService,
    public snackBar: MatSnackBar // Inject MatSnackBar service
  ) {}
 
  ngOnInit(): void {
    this.userName = localStorage.getItem('userName');
    this.isLoggedIn = !!localStorage.getItem('token');
    this.dataSharingService.changeUserdata(this.isLoggedIn);
      this.route.paramMap.subscribe((params) => {
        this.artistId = +params.get('id')!; // Retrieve albumId
        const artistName = params.get('name'); // Correct syntax for getting route param
        const artistId = params.get('id'); // Assuming albumId is passed in the route
        if (artistName && artistId) {
          this.artistName = artistName;
          this.getTracks(artistName);
          this.getArtistDetails(artistId); // Fetch album details by albumId
        }
      });
 
 
    // Listen for changes in the search term from the NavBar
 
    this.dataSharingService.currentData.subscribe((searchTerm: string) => {
      console.log(searchTerm);
      if (searchTerm) {
        this.searchService
          .searchInSongTrack(searchTerm, this.customTracks)
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
 
  getArtistDetails(artistId: string): void {
    this.apicall.getArtistById(artistId).subscribe(
      (data) => {
        this.artistDetails = data; // Store album details
 
        console.log(data);
      },
      (error) => console.error('Error fetching album details', error)
    );
  }
  getTracks(artistName: string): void {
    this.apicall.getTracksByArtist(artistName).subscribe(
      (data) => {
        this.tracks = data;
        this.customTracks = data;
        console.log(data);
      },
      (error) => console.error('Error fetching tracks', error)
    );
  }
  playTrack(url: string, index: number): void {
    if (!this.isLoggedIn) { // Check if the user is logged in
      this.snackBar.open('Please log in to play songs', 'Close', { duration: 3000 });
      return; // Exit the method if not logged in
    }
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
      this.tracks[this.currentTrackIndex].audioUrl,
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
      this.tracks[this.currentTrackIndex].audioUrl,
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
  addToFavorite(musicTrack: any) {
    if (!this.isLoggedIn) { // Check if the user is logged in
      this.snackBar.open('Please log in to add songs to favorites', 'Close', { duration: 3000 });
      return; // Exit the method if not logged in
    }
    console.log(musicTrack);
    // Mapping the properties to match the desired structure
    let updataMusicTrackForFavorite = {
      MusicId: musicTrack.songId,
      musicName: musicTrack.name,
      pictureUrl: musicTrack.pictureUrl,
      singerName: musicTrack.artistName,
      songUrl: musicTrack.audioUrl,
    };
    console.log(updataMusicTrackForFavorite);
    this.apicall
      .addSongToFavorite(this.userName, updataMusicTrackForFavorite)
      .subscribe(
        (res) => {
          console.log(res);
          //alert('Song added in Favorite');
          this.snackBar.open('Song added in Favorite', 'Close', { duration: 3000 });
        },
        (error) => {
          console.log(error);
          //alert('This song is already in Favoritelist');
          this.snackBar.open('This song is already in Favoritelist', 'Close', { duration: 3000 });
        }
      );
  }
 
}