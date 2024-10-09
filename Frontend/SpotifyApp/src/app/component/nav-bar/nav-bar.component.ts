import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { SigninComponent } from '../signin/signin.component';
import { DataSharingService } from '../../service/dataSharingService/data-sharing.service';
import { CommonModule } from '@angular/common';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-nav-bar',
  standalone: true,
  imports: [MatDialogModule, CommonModule, MatIcon],
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.css',
})
export class NavBarComponent implements AfterViewInit, OnInit {
  isLoggedIn = false;
  user: any = '';
  uname :string =' ';

  constructor(
    private router: Router,
    public dialog: MatDialog,
    private cdr: ChangeDetectorRef,
    private dataSharingService: DataSharingService
  ) {}

  // Check login status on init
  ngOnInit() {
    
    this.user = localStorage.getItem('user');
    this.isLoggedIn = !!localStorage.getItem('token');
    this.uname = localStorage.getItem('userName') || '';
    this.dataSharingService.isUserLoggedIn.subscribe((res) => {
    this.isLoggedIn = res;
    const updatedEmail = localStorage.getItem('userName') || '';
    this.uname = this.formatUserName(updatedEmail); 

    });
    // Check if token exists
  }

  openSigninPopup() {
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
  }

  // Logout function to clear token
  logout() {
    localStorage.removeItem('token'); // Remove token
    this.isLoggedIn = false; // Update login status
    this.dataSharingService.changeUserdata(false);
    localStorage.removeItem('userName');
    localStorage.removeItem('user');
    this.router.navigate(['/home']);
  }

  // Access the input field using ViewChild
  @ViewChild('searchInput') searchInput!: ElementRef;

  // Automatically focus the input after the view is initialized
  ngAfterViewInit() {
    this.searchInput.nativeElement.focus();
  }

  goToSignIn() {
    this.router.navigate(['/signin']);
  }

  onSearch(event: any): void {
    const searchTerm = event.target.value;

    // console.log(searchTerm);
    this.dataSharingService.changeData(searchTerm);
  }

  formatUserName(email: string): string {
    if (!email) return '';
    const username = email.split('@')[0]; // Extract part before '@'
    const cleanedUsername = username.replace(/[0-9]/g, ''); // Remove numbers
    return cleanedUsername.charAt(0).toUpperCase() + cleanedUsername.slice(1).toLowerCase(); // Capitalize first letter
  }
}
