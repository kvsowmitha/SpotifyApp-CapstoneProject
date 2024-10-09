import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthorizationService } from './authorization.service';

describe('AuthorizationService', () => {
  let service: AuthorizationService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    service = TestBed.inject(AuthorizationService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should login and return user data', () => {
    const mockCredentials = { email: 'test@example.com', password: 'password' };
    const mockResponse = { token: 'abcd1234', user: { email: 'test@example.com' } };

    service.login(mockCredentials).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    // Mock the HTTP request
    const req = httpMock.expectOne('http://localhost:5006/Auth/login');
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });
});