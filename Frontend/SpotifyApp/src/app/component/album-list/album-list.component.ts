import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApicallService } from '../../service/apiCall/apicall.service';
import { DataSharingService } from '../../service/dataSharingService/data-sharing.service';
import { SearchService } from '../../service/searchService/search.service';
import { CommonModule } from '@angular/common';
import { MatCard, MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-album-list',
  standalone: true,
  imports: [CommonModule,MatCard,MatCardModule],
  templateUrl: './album-list.component.html',
  styleUrl: './album-list.component.css'
})
export class AlbumListComponent implements OnInit{
  isLoggedIn = false;
  currentUrlSegment: string = '';
  customAlbums: any[] = [];
  originalAlbum: any[] = [];

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
    this.getAlbums();

    // Listen for changes in the search term from the NavBar

    this.dataSharingService.currentData.subscribe((searchTerm: string) => {
      console.log(searchTerm);
      if (searchTerm) {
        this.searchService
          .search(searchTerm, this.customAlbums)
          .subscribe((data: any) => {
            console.log(data);
            this.originalAlbum = data;
            this.cdr.markForCheck();
          });
      } else {
        this.originalAlbum = this.customAlbums;
        this.cdr.markForCheck();
      }
    });
  }

  getAlbums(): void {
    this.apicall.getAlbums().subscribe(
      (data) => {
        this.originalAlbum = data;
        this.customAlbums = data;
        console.log(this.originalAlbum);
      },
      (error) => console.error('Error fetching albums', error)
    );
  }
  goToTracks(albumId: number, name: string): void {
      this.router.navigate(['/albumtracks', albumId, name]);
  }
}
