import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginRequest } from '../models/Login';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:44381/api/Auth';  // adjust if needed

  constructor(private http: HttpClient) {}

  login(lgn:LoginRequest): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Login`, {
      Login: lgn
    });
  }
}
