import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ForgetPasswordComponent } from './forget-password.component';
import { ApicallService } from '../../service/apiCall/apicall.service';
import { of, throwError } from 'rxjs';

class MockRouter {
  navigate = jasmine.createSpy('navigate');
}

class MockApicallService {
  sendToOTP = jasmine.createSpy('sendToOTP').and.returnValue(of({ success: true }));
}

describe('ForgetPasswordComponent', () => {
  let component: ForgetPasswordComponent;
  let fixture: ComponentFixture<ForgetPasswordComponent>;
  let apiService: MockApicallService;
  let router: MockRouter;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormsModule, ForgetPasswordComponent],
      providers: [
        { provide: Router, useClass: MockRouter },
        { provide: ApicallService, useClass: MockApicallService },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ForgetPasswordComponent);
    component = fixture.componentInstance;
    apiService = TestBed.inject(ApicallService) as unknown as MockApicallService;
    router = TestBed.inject(Router) as unknown as MockRouter;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should send OTP and navigate to reset password on valid email', () => {
    component.email = 'user@example.com';
    spyOn(window, 'alert');

    component.onSubmit();

    expect(apiService.sendToOTP).toHaveBeenCalledWith({ email: 'user@example.com' });
    expect(router.navigate).toHaveBeenCalledWith(['/reset-password']);
    expect(window.alert).toHaveBeenCalledWith('OTP sent to: user@example.com');
  });

  it('should show alert on API call failure', () => {
    component.email = 'user@example.com';
    (apiService.sendToOTP as jasmine.Spy).and.returnValue(throwError('Error'));

    spyOn(window, 'alert');

    component.onSubmit();

    expect(apiService.sendToOTP).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith('OTP send function is not working');
  });

  it('should navigate to reset password after successful OTP', () => {
    component.email = 'user@example.com';
    component.onSubmit();

    expect(router.navigate).toHaveBeenCalledWith(['/reset-password']);
  });
});