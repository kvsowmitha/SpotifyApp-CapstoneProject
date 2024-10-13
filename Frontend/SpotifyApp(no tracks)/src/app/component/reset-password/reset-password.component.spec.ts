import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { ApicallService } from '../../service/apiCall/apicall.service';
import { DataSharingService } from '../../service/dataSharingService/data-sharing.service';
import { ResetPasswordComponent } from './reset-password.component';

describe('ResetPasswordComponent', () => {
  let component: ResetPasswordComponent;
  let fixture: ComponentFixture<ResetPasswordComponent>;
  let apiServiceSpy: jasmine.SpyObj<ApicallService>;
  let dataSharingServiceSpy: jasmine.SpyObj<DataSharingService>;
  let routerSpy: jasmine.SpyObj<Router>;
  let dialogSpy: jasmine.SpyObj<MatDialog>;

  beforeEach(async () => {
    apiServiceSpy = jasmine.createSpyObj('ApicallService', ['resetUserPassword']);
    dataSharingServiceSpy = jasmine.createSpyObj('DataSharingService', ['changeUserdata']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    dialogSpy = jasmine.createSpyObj('MatDialog', ['open']);

    const dialogRefSpy = jasmine.createSpyObj<MatDialogRef<any>>('MatDialogRef', ['afterClosed']);
    dialogRefSpy.afterClosed.and.returnValue(of(true));

    dialogSpy.open.and.returnValue(dialogRefSpy);

    await TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        ResetPasswordComponent 
      ],
      providers: [
        { provide: ApicallService, useValue: apiServiceSpy },
        { provide: DataSharingService, useValue: dataSharingServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: MatDialog, useValue: dialogSpy }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ResetPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should have a form with email, otp, newPassword, and confirmPassword fields', () => {
    expect(component.resetForm.contains('email')).toBeTrue();
    expect(component.resetForm.contains('otp')).toBeTrue();
    expect(component.resetForm.contains('newPassword')).toBeTrue();
    expect(component.resetForm.contains('confirmPassword')).toBeTrue();
  });

  it('should make the email field required and validate its format', () => {
    const email = component.email;
    email?.setValue('');
    expect(email?.valid).toBeFalsy();

    email?.setValue('not-an-email');
    expect(email?.valid).toBeFalsy();

    email?.setValue('test@example.com');
    expect(email?.valid).toBeTruthy();
  });

  it('should display an error when passwords do not match', () => {
    component.resetForm.patchValue({
      email: 'test@example.com',
      otp: '123456',
      newPassword: 'password123',
      confirmPassword: 'password321',
    });

    spyOn(console, 'error');
    component.onSubmit();
    expect(console.error).toHaveBeenCalledWith('Passwords do not match');
  });

  it('should submit form and reset password successfully', () => {
    component.resetForm.patchValue({
      email: 'test@example.com',
      otp: '123456',
      newPassword: 'password123',
      confirmPassword: 'password123',
    });

    apiServiceSpy.resetUserPassword.and.returnValue(of('Password reset successful'));
    spyOn(window, 'alert');

    component.onSubmit();
    expect(apiServiceSpy.resetUserPassword).toHaveBeenCalledWith({
      email: 'test@example.com',
      otp: '123456',
      newPassword: 'password123',
      confirmPassword: 'password123',
    });
    expect(window.alert).toHaveBeenCalledWith('Password reset successful');
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/home']);
  });

  it('should handle error when API call fails', () => {
    component.resetForm.patchValue({
      email: 'test@example.com',
      otp: '123456',
      newPassword: 'password123',
      confirmPassword: 'password123',
    });

    apiServiceSpy.resetUserPassword.and.returnValue(throwError('Error resetting password'));
    spyOn(window, 'alert');
    spyOn(console, 'log');

    component.onSubmit();
    expect(window.alert).toHaveBeenCalledWith('Error resetting password');
    expect(console.log).toHaveBeenCalledWith('Error resetting password');
  });

  it('should open the sign-in dialog and handle afterClosed logic', () => {
    // Simulate successful password reset and opening the dialog
    component.resetForm.patchValue({
      email: 'test@example.com',
      otp: '123456',
      newPassword: 'password123',
      confirmPassword: 'password123',
    });

    apiServiceSpy.resetUserPassword.and.returnValue(of('Password reset successful'));

    component.onSubmit();

    expect(dialogSpy.open).toHaveBeenCalled();
  });
});