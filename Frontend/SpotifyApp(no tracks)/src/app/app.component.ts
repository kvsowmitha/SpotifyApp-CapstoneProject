import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FooterComponent } from './component/footer/footer.component';
import { SideBarComponent } from './component/side-bar/side-bar.component';
import { NavBarComponent } from './component/nav-bar/nav-bar.component';
import { SigninComponent } from './component/signin/signin.component';
import { MusicDisplayComponent } from './component/music-display/music-display.component';
import { SearchService } from './service/searchService/search.service';
import { MusicPlayerComponent } from './component/music-player/music-player.component';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { DataSharingService } from './service/dataSharingService/data-sharing.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    FooterComponent,
    SideBarComponent,
    NavBarComponent,
    SigninComponent,
    MusicDisplayComponent,
    MusicPlayerComponent,
    CommonModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  constructor(private dataSharingService: DataSharingService) {}
  title = 'SpotifyApp';
  currentSong: string = '';
  isLoggedIn = false;

  ngOnInit(): void {
    this.isLoggedIn = !!localStorage.getItem('token');
    this.dataSharingService.currentDataSong.subscribe((songUrl: string) => {
      this.currentSong = songUrl;
    });
    this.dataSharingService.isUserLoggedIn.subscribe((res) => {
      this.isLoggedIn = res;
    });
  }
}
