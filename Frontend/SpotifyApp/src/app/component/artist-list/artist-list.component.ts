import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatCard, MatCardModule } from '@angular/material/card';
import { ActivatedRoute, Router } from '@angular/router';
import { ApicallService } from '../../service/apiCall/apicall.service';
import { DataSharingService } from '../../service/dataSharingService/data-sharing.service';
import { SearchService } from '../../service/searchService/search.service';

@Component({
  selector: 'app-artist-list',
  standalone: true,
  imports: [CommonModule,MatCard,MatCardModule],
  templateUrl: './artist-list.component.html',
  styleUrl: './artist-list.component.css'
})
export class ArtistListComponent implements OnInit {
  isLoggedIn = false;
  currentUrlSegment: string = '';
  customArtist: any[] = [];
  originalArtist: any[] = [];

  constructor(
    private router: Router,
    private apicall: ApicallService,
    private route: ActivatedRoute,
    private dataSharingService: DataSharingService,
    private searchService: SearchService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.route.url.subscribe((urlSegment) => {
      this.currentUrlSegment = urlSegment
        .map((segment) => segment.path)
        .join('/');
      console.log('URL Segment:', this.currentUrlSegment); // Output: 'artist'
    });
    this.isLoggedIn = !!localStorage.getItem('token');
    this.getArtists();

    // Listen for changes in the search term from the NavBar

    this.dataSharingService.currentData.subscribe((searchTerm: string) => {
      console.log(searchTerm);
      if (searchTerm) {
        this.searchService
          .search(searchTerm, this.customArtist)
          .subscribe((data: any) => {
            console.log(data);
            this.originalArtist = data;
            this.cdr.markForCheck();
          });
      } else {
        this.originalArtist = this.customArtist;
        this.cdr.markForCheck();
      }
    });
  }

  getArtists(): void {
    this.apicall.getArtists().subscribe(
      (data) => {
        this.originalArtist = data;
        this.customArtist = data;
        // console.log(this.artists);
      },
      (error) => console.error('Error fetching artists', error) // Updated message
    );
  }
  goToTracks(artistId: number, name: string): void {
      this.router.navigate(['/artist-tracks', artistId, name]);
  }
}
