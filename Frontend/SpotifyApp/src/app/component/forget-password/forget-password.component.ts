import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ApicallService } from '../../service/apiCall/apicall.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
@Component({
  selector: 'app-forget-password',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './forget-password.component.html',
  styleUrl: './forget-password.component.css'
})
export class ForgetPasswordComponent {
  email: string = '';
  message: string = '';

  constructor(private router: Router, private apiService: ApicallService, private snackBar: MatSnackBar) {}

  onSubmit() {
    const userEmail = {
      email: this.email,
    };

    if (this.email) {
      this.apiService.sendToOTP(userEmail).subscribe(
        (res) => {
          console.log(res);
        },
        (error) => {
          console.log(error);
          //alert('OTP send function is not working');
          this.snackBar.open('OTP send function is not working', 'Close', { duration: 3000 });
        }
      );
    }
    //alert(`OTP sent to: ${this.email}`);
    this.snackBar.open(`OTP sent to: ${this.email}`, 'Close', { duration: 3000 });

    this.router.navigate(['/reset-password']);
  }

}
