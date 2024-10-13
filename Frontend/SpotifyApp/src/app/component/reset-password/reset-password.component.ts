import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DataSharingService } from '../../service/dataSharingService/data-sharing.service';
import { ApicallService } from '../../service/apiCall/apicall.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup,FormsModule,Validators, ReactiveFormsModule, } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { SigninComponent } from '../signin/signin.component';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent {
  resetForm: FormGroup;
  isLoggedIn = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private apiService: ApicallService,
    private dataSharingService: DataSharingService,
    public dialog: MatDialog,
    private snackBar: MatSnackBar // Inject MatSnackBar service
  ) {
    this.resetForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      otp: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
    });
  }

  get email() {
    return this.resetForm.get('email');
  }

  get otp() {
    return this.resetForm.get('otp');
  }

  get newPassword() {
    return this.resetForm.get('newPassword');
  }

  get confirmPassword() {
    return this.resetForm.get('confirmPassword');
  }

  onSubmit() {
    if (this.resetForm.invalid) {
      // Display validation errors to the user
      console.error('Form is invalid', this.resetForm.errors);
      return;
    }

    // Check if form controls are not null before accessing their values
    if (this.newPassword?.value !== this.confirmPassword?.value) {
      console.error('Passwords do not match');
      return;
    }

    // Logic to send the reset password request
    const resetUserData = {
      email: this.email?.value,
      otp: this.otp?.value,
      newPassword: this.newPassword?.value,
      confirmPassword: this.confirmPassword?.value,
    };

    this.apiService.resetUserPassword(resetUserData).subscribe(
      (res) => {
        alert(res);
        console.log(res);
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
      },
      (error) => {
        console.log(error);
        alert(error);
      }
    );

    this.router.navigate(['/home']);

    console.log(resetUserData);
  }

}
