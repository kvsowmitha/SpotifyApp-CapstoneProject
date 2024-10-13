import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ForgetPasswordComponent } from './forget-password.component';
import { ApicallService } from '../../service/apiCall/apicall.service';
import { of, throwError } from 'rxjs';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSnackBar } from '@angular/material/snack-bar';

class MockRouter {
  navigate = jasmine.createSpy('navigate');
}

class MockApicallService {
  sendToOTP = jasmine.createSpy('sendToOTP').and.returnValue(of({ success: true }));
}

class MockMatSnackBar {
  open = jasmine.createSpy('open'); // Spy for open method
}

describe('ForgetPasswordComponent', () => {
  let component: ForgetPasswordComponent;
  let fixture: ComponentFixture<ForgetPasswordComponent>;
  let apiService: MockApicallService;
  let router: MockRouter;
  let snackBar: MockMatSnackBar;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormsModule, ForgetPasswordComponent, BrowserAnimationsModule],
      providers: [
        { provide: Router, useClass: MockRouter },
        { provide: ApicallService, useClass: MockApicallService },
        { provide: MatSnackBar, useClass: MockMatSnackBar },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ForgetPasswordComponent);
    component = fixture.componentInstance;
    apiService = TestBed.inject(ApicallService) as unknown as MockApicallService;
    router = TestBed.inject(Router) as unknown as MockRouter;
    snackBar = TestBed.inject(MatSnackBar) as unknown as MockMatSnackBar; // Mock instance
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should send OTP and navigate to reset password on valid email', () => {
    component.email = 'user@example.com';
    
    component.onSubmit();

    expect(apiService.sendToOTP).toHaveBeenCalledWith({ email: 'user@example.com' });
    expect(router.navigate).toHaveBeenCalledWith(['/reset-password']);
    expect(snackBar.open).toHaveBeenCalledWith('OTP sent to: user@example.com', 'Close', { duration: 3000 });
  });

  it('should show snackbar on API call failure', () => {
    component.email = 'user@example.com';
    (apiService.sendToOTP as jasmine.Spy).and.returnValue(throwError('Error'));

    component.onSubmit();

    expect(apiService.sendToOTP).toHaveBeenCalled();
    expect(snackBar.open).toHaveBeenCalledWith('OTP send function is not working', 'Close', { duration: 3000 });
  });

  it('should navigate to reset password after successful OTP', () => {
    component.email = 'user@example.com';
    component.onSubmit();

    expect(router.navigate).toHaveBeenCalledWith(['/reset-password']);
  });
});