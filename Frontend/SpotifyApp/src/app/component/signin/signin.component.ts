import { Component, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthorizationService } from '../../service/userAuth/authorization.service';
import { RegistrationService } from '../../service/userReg/registration.service';
import { DataSharingService } from '../../service/dataSharingService/data-sharing.service';
import { AbstractControl, ValidatorFn } from '@angular/forms';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

// Custom validator to check if passwords match
export function passwordMatchValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: boolean } | null => {
    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;
    if (password !== confirmPassword) {
      return { passwordMismatch: true };
    }
    return null;
  };
}

@Component({
  selector: 'app-signin',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css'],
})
export class SigninComponent implements AfterViewInit {
  @ViewChild('container') container!: ElementRef;

  registerForm: FormGroup;
  loginForm: FormGroup;
  errorMessage: string | null = null;
  userName='';
  Name=" ";
  Dob:'' | undefined;
  Gender:''| undefined;

  constructor(
    public dialogRef: MatDialogRef<SigninComponent>,
    private userRegisterService: RegistrationService,
    private authService: AuthorizationService,
    private router: Router,
    private userdataservice: DataSharingService,
    private fb: FormBuilder,
    private snackBar: MatSnackBar // Inject MatSnackBar service
  ) {
    // Initialize forms with validation
    this.registerForm = this.fb.group(
      {
        name: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required],
        dateOfBirth: ['', Validators.required],
        gender: ['', Validators.required],
      },
      { validators: passwordMatchValidator() } // Apply custom validator here
    );

    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  ngAfterViewInit() {
    if (!this.container) {
      console.error('Container element not found!');
    }
  }

  // Toggle between sign-up and sign-in forms
  toggleSignUp() {
    if (this.container) {
      this.container.nativeElement.classList.add('active');
    }
  }

  toggleSignIn() {
    if (this.container) {
      this.container.nativeElement.classList.remove('active');
    }
  }

  close() {
    this.dialogRef.close();
  }

  // Handle user registration
  registerUser() {
    if (this.registerForm.invalid) {
      //alert('Please fill out all required fields correctly.');
      this.snackBar.open('Please fill out all required fields correctly.', 'Close', { duration: 3000 });
      return;
    }
 
    this.userRegisterService.registerUser(this.registerForm.value).subscribe(
      (response: any) => {
       // alert('User registered successfully');
       this.snackBar.open('User registered successfully', 'Close', { duration: 3000 });
        console.log(response);
        this.toggleSignIn(); // Switch to sign-in form after successful registration
      },
      (error: { message: string }) => {
        alert('Registration failed: ' + error.message);
      }
    );
  }
  
  

  // Handle user login
  loginUser() {
    if (this.loginForm.invalid) {
      //alert('Please fill out all required fields correctly.');
      this.snackBar.open('Please fill out all required fields correctly.', 'Close', { duration: 3000 });
      return;
    }

    this.authService.login(this.loginForm.value).subscribe(
      (response: any) => {
        localStorage.setItem('token', response.token);
        localStorage.setItem('user', response.user);
        this.userName=this.loginForm.value.email.split('@')[0]
        localStorage.setItem('userName', this.userName);
        
        console.log(response);
        console.log(this.userName);
        console.log(this.Name);
        //alert('Login successful');
        this.snackBar.open('Login successful', 'Close', { duration: 3000 });
        this.userdataservice.changeUserdata(true);
        this.close();
        this.router.navigate(['/home']); // Navigate to the home page on success
      },
      (error: { message: string }) => {
       // this.errorMessage = 'Invalid email or password';
        this.snackBar.open('Invalid email or password', 'Close', { duration: 3000 });
      }
    );
  }
  goToForgetPassword() {
    this.close();
    this.router.navigate(['/forget-password']);
  }
}