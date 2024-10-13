import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FooterComponent } from '../footer/footer.component';
import { SigninComponent } from '../signin/signin.component';
import { DataSharingService } from '../../service/dataSharingService/data-sharing.service';
import { MatDialog } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-side-bar',
  standalone: true,
  imports: [FooterComponent,CommonModule],
  templateUrl: './side-bar.component.html',
  styleUrl: './side-bar.component.css',
})
export class SideBarComponent implements OnInit {
  isLoggedIn = false;
  constructor(
    private router: Router,
    private dataSharingService: DataSharingService,
    public dialog: MatDialog
  ) {}
  ngOnInit(): void {
    this.isLoggedIn = !!localStorage.getItem('token');
    this.dataSharingService.isUserLoggedIn.subscribe((res) => {
      this.isLoggedIn = res;
    });
  }

  goToProfile() {
    this.router.navigate(['/profile']);
  }
  
  goToAlbum() {
    this.router.navigate(['/albums']);
  }
  goToArtist() {
    this.router.navigate(['/artists']);
  }

  goToSearch() {
    this.router.navigate(['/search']);
  }
  goToHome() {
    this.router.navigate(['/home']);
  }

  goToFavorite() {
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
    this.router.navigate(['/favorite']);
  }
}
