import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth';
import { FormsModule } from '@angular/forms';
import { LoginRequest } from '../models/Login';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class Login {

lgn:LoginRequest={username:"",password:""};
  errorMsg = '';

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    this.authService.login(this.lgn).subscribe({
      next: (res) => {
        localStorage.setItem('jwtToken', res.token);
        this.router.navigate(['/chat']); // navigate to Chat component
      },
      error: () => {
        this.errorMsg = 'Invalid username or password';
      }
    });
  }
}
