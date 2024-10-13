import { CommonModule } from '@angular/common';
import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  OnChanges,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { SearchService } from '../../service/searchService/search.service';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { DataSharingService } from '../../service/dataSharingService/data-sharing.service';
import { ChangeDetectorRef } from '@angular/core';
import { ApicallService } from '../../service/apiCall/apicall.service';
import { MusicPlayerComponent } from '../music-player/music-player.component';
import { ActivatedRoute } from '@angular/router';
import { SigninComponent } from '../signin/signin.component';
import { MatDialog } from '@angular/material/dialog';
@Component({
  selector: 'app-music-display',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MusicPlayerComponent],
  templateUrl: './music-display.component.html',
  styleUrl: './music-display.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MusicDisplayComponent implements OnInit {
  getDataResult: any[] = [];
  originalData: any[] = [];
  currentSong: string = '';
  currentUrlSegment: string = '';

  isLoggedIn = false;
  userName: any = '';
  songDataItem: any;
  isFavorite = false;
  // Message variables
  message: string = '';
  isSuccess: boolean = true;

  constructor(
    private searchService: SearchService,
    private dataSharingService: DataSharingService,
    private cdr: ChangeDetectorRef,
    private apiService: ApicallService,
    private route: ActivatedRoute,
    public dialog: MatDialog
  ) {}


  ngOnInit(): void {
    this.userName = localStorage.getItem('userName');
    this.isLoggedIn = !!localStorage.getItem('token');
    this.dataSharingService.changeUserdata(this.isLoggedIn);
    this.dataSharingService.isUserLoggedIn.subscribe((res) => {
      this.isLoggedIn = res;
    });

    this.route.url.subscribe((urlSegment) => {
      this.currentUrlSegment = urlSegment
        .map((segment) => segment.path)
        .join('/');
      console.log('URL Segment:', this.currentUrlSegment); // Output: 'artist'
      this.isLoggedIn = !!localStorage.getItem('token');
    });

    // storing song for album
    if (this.currentUrlSegment == 'album') {
      this.apiService.getPlaylistAlbum().subscribe((res: any[]) => {
        this.getDataResult = res;
        this.originalData = res;
        this.cdr.markForCheck();
        console.log(this.getDataResult);
      });
    }

    // storing song for artist
    if (this.currentUrlSegment == 'artist') {
      this.apiService.getPlaylistArtist().subscribe((res: any[]) => {
        this.getDataResult = res;
        this.originalData = res;
        this.cdr.markForCheck();
        console.log(this.getDataResult);
      });
    }

    // storing all song
    if (this.currentUrlSegment == 'home') {
      this.apiService.getPlaylistAlbum().subscribe((res: any[]) => {
        console.log(res);

        this.getDataResult = res;
        this.originalData = res;
        this.apiService.getPlaylistArtist().subscribe((res: any[]) => {
          console.log(res);

          this.getDataResult = [...this.getDataResult, ...res];
          this.originalData = [...this.originalData, ...res];
          this.cdr.markForCheck();
          console.log(this.getDataResult);
        });
      });
    }

    // storing song for a paticular user's favorite
    if (this.currentUrlSegment == 'favorite') {
      if (!this.isLoggedIn) {
        const dialogRef = this.dialog.open(SigninComponent, {
          width: 'fit-content',
          height: 'auto',
          panelClass: 'custom-dialog-container',
          disableClose: true,
        });

        // Subscribe to afterClosed observable to detect when dialog is closed
        dialogRef.afterClosed().subscribe((result) => {
          // Call your custom function or handle any logic after the dialog closes
          this.isLoggedIn = !!localStorage.getItem('token');
          this.dataSharingService.changeUserdata(this.isLoggedIn);
        });
        return;
      }
      console.log(this.userName);
      this.apiService.getSongFavorite(this.userName).subscribe((res: any[]) => {
        console.log(res);

        this.getDataResult = res;
        this.originalData = res;
        this.cdr.markForCheck();
        if (res) {
          this.isFavorite = true;
        }
        console.log(this.getDataResult);
      });
      console.log('render favorite list here');
    }
    // Listen for changes in the search term from the NavBar
    this.dataSharingService.currentData.subscribe((searchTerm: string) => {
      console.log(searchTerm);
      if (searchTerm) {
        this.searchService
          .searchHomeMusic(searchTerm, this.originalData)
          .subscribe((data: any) => {
            console.log(data);
            this.getDataResult = data;
            this.cdr.markForCheck();
          });
      } else {
        this.getDataResult = this.originalData;
        this.cdr.markForCheck();
      }
    });
  }

  playSong(songUrl: string) {
    // for tokenatization

    if (!this.isLoggedIn) {
      const dialogRef = this.dialog.open(SigninComponent, {
        width: 'fit-content',
        height: 'auto',
        panelClass: 'custom-dialog-container',
        disableClose: true,
      });

      // Subscribe to afterClosed observable to detect when dialog is closed
      dialogRef.afterClosed().subscribe((result) => {
        // Call your custom function or handle any logic after the dialog closes
        this.isLoggedIn = !!localStorage.getItem('token');
        this.dataSharingService.changeUserdata(this.isLoggedIn);
      });
      return;
    }

    this.currentSong = songUrl;
    this.cdr.markForCheck();
    this.dataSharingService.chnageSongData(songUrl);
  }

  AddFavorite(item: any) {
    if (!this.isLoggedIn) {
      const dialogRef = this.dialog.open(SigninComponent, {
        width: 'fit-content',
        height: 'auto',
        panelClass: 'custom-dialog-container',
        disableClose: true,
      });

      // Subscribe to afterClosed observable to detect when dialog is closed
      dialogRef.afterClosed().subscribe((result) => {
        // Call your custom function or handle any logic after the dialog closes
        this.isLoggedIn = !!localStorage.getItem('token');
        this.dataSharingService.changeUserdata(this.isLoggedIn);
      });
      return;
    }
    // Destructure and exclude $id
    const { $id, ...rest } = item;

    // Store the rest of the data in songDataItems
    this.songDataItem = rest;
    
    this.apiService.addSongToFavorite(this.userName, this.songDataItem).subscribe(
      (res) => {
        //alert('Song added in Favorite');
        this.displayMessage('Song added to favorites!', true);
      },
      (error) => {
        console.log(error);
        //alert('This song is already in Favoritelist');
        this.displayMessage('This song is already in the favorites list', false); // Error message
      }
    );
  }

  deleteFavorite(musicId: any) {
    this.apiService
      .deleteFavoriteSongById(this.userName, musicId)
      .subscribe((res) => {
        this.apiService
          .getSongFavorite(this.userName)
          .subscribe((res: any[]) => {
            console.log(res);

            this.getDataResult = res;
            this.originalData = res;
            this.cdr.markForCheck();
            if (res) {
              this.isFavorite = true;
            }
            console.log(this.getDataResult);
          });
      });

    //alert('Favorite song remove');
    this.displayMessage('Favorite song removed', true); // Error message
  }

  private displayMessage(message: string, isSuccess: boolean) {
    this.message = message;
    this.isSuccess = isSuccess;
    
    // Mark for check to ensure the UI updates
    this.cdr.markForCheck();

    // Automatically clear the message after 3 seconds
    setTimeout(() => {
      this.message = ''; // Clear the message
      this.cdr.markForCheck(); // Ensure the UI updates again
    }, 2000); // Duration in milliseconds
  }
}

