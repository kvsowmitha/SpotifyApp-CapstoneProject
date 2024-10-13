import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { of, throwError } from 'rxjs';
import { SigninComponent } from './signin.component';
import { AuthorizationService } from '../../service/userAuth/authorization.service';
import { RegistrationService } from '../../service/userReg/registration.service';
import { DataSharingService } from '../../service/dataSharingService/data-sharing.service';

describe('SigninComponent', () => {
  let component: SigninComponent;
  let fixture: ComponentFixture<SigninComponent>;
  let mockDialogRef: jasmine.SpyObj<MatDialogRef<SigninComponent>>;
  let mockAuthService: jasmine.SpyObj<AuthorizationService>;
  let mockRegistrationService: jasmine.SpyObj<RegistrationService>;
  let mockDataSharingService: jasmine.SpyObj<DataSharingService>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockSnackBar: jasmine.SpyObj<MatSnackBar>;

  beforeEach(async () => {
    // Create mock services
    mockDialogRef = jasmine.createSpyObj('MatDialogRef', ['close']);
    mockAuthService = jasmine.createSpyObj('AuthorizationService', ['login']);
    mockRegistrationService = jasmine.createSpyObj('RegistrationService', ['registerUser']);
    mockDataSharingService = jasmine.createSpyObj('DataSharingService', ['changeUserdata']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);
    mockSnackBar = jasmine.createSpyObj('MatSnackBar', ['open']);

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, SigninComponent],  // Importing the standalone component here
      providers: [
        { provide: MatDialogRef, useValue: mockDialogRef },
        { provide: AuthorizationService, useValue: mockAuthService },
        { provide: RegistrationService, useValue: mockRegistrationService },
        { provide: DataSharingService, useValue: mockDataSharingService },
        { provide: Router, useValue: mockRouter },
        { provide: MatSnackBar, useValue: mockSnackBar },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(SigninComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    // Spy on the toggleSignIn method
    spyOn(component, 'toggleSignIn').and.callThrough();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the forms correctly', () => {
    expect(component.registerForm).toBeDefined();
    expect(component.loginForm).toBeDefined();
    expect(component.registerForm.valid).toBeFalsy();
    expect(component.loginForm.valid).toBeFalsy();
  });

  it('should register user successfully', () => {
    // Mock successful registration response
    mockRegistrationService.registerUser.and.returnValue(of({}));

    // Set form values
    component.registerForm.setValue({
      name: 'John Doe',
      email: 'john.doe@example.com',
      password: 'password123',
      confirmPassword: 'password123',
      dateOfBirth: '01-01-1990',
      gender: 'Male'
    });

    // Call registerUser
    component.registerUser();

    // Verify interactions
    expect(mockRegistrationService.registerUser).toHaveBeenCalled();
    expect(mockSnackBar.open).toHaveBeenCalledWith('User registered successfully', 'Close', { duration: 3000 });
    expect(component.toggleSignIn).toHaveBeenCalled(); // This will now work as it is spied on
  });

  
  it('should log in user successfully', () => {
    // Mock successful login response
    const mockResponse = { token: 'fake-jwt-token', user: 'John Doe' };
    mockAuthService.login.and.returnValue(of(mockResponse));

    // Set form values
    component.loginForm.setValue({
      email: 'john.doe@example.com',
      password: 'password123'
    });

    // Call loginUser
    component.loginUser();

    // Verify interactions
    expect(mockAuthService.login).toHaveBeenCalled();
    expect(localStorage.getItem('token')).toBe(mockResponse.token);
    expect(localStorage.getItem('user')).toBe(mockResponse.user);
    expect(mockDataSharingService.changeUserdata).toHaveBeenCalledWith(true);
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/home']);
    expect(mockSnackBar.open).toHaveBeenCalledWith('Login successful', 'Close', { duration: 3000 });
    expect(mockDialogRef.close).toHaveBeenCalled();
  });

  it('should handle login error', () => {
    // Mock login error response
    const errorResponse = { message: 'Invalid email or password' };
    mockAuthService.login.and.returnValue(throwError(errorResponse));

    // Set form values
    component.loginForm.setValue({
      email: 'john.doe@example.com',
      password: 'wrongpassword'
    });

    // Call loginUser
    component.loginUser();

    // Verify interactions
    expect(mockAuthService.login).toHaveBeenCalled();
    expect(mockSnackBar.open).toHaveBeenCalledWith('Invalid email or password', 'Close', { duration: 3000 });
  });
});