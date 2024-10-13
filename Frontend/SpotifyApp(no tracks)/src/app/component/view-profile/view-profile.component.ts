import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-view-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './view-profile.component.html',
  styleUrls: ['./view-profile.component.css']
})
export class ViewProfileComponent implements OnInit {
  isLoggedIn = false;
  user: any = {
    name: '',
    email: '',
    dob: '',
    gender: ''
  };

  constructor(private router: Router) {}

  ngOnInit() {
    // Retrieve user details from localStorage
    
    this.user = localStorage.getItem('user');
    this.isLoggedIn = !!localStorage.getItem('token');
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      try {
        this.user = JSON.parse(storedUser);  // Convert string back to object
        this.isLoggedIn = true;
      } catch (error) {
        console.error('Error parsing user data from localStorage:', error);
      }
    }
  }

}
