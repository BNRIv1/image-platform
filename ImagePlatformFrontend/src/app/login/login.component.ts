import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from 'src/interfaces/User';
import { UserLogin } from 'src/interfaces/UserLogin';
import { UserRegister } from 'src/interfaces/UserRegister';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  user: User | undefined;
  hide = true;
  loginMode = true;
  registerForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
    confirmPassword: new FormControl('', [Validators.required]),
  });

  loginForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  });

  constructor(private userService: UserService, private router: Router) {}

  register() {
    const data: UserRegister = {
      username: this.registerForm.get('username')?.value!!,
      password: this.registerForm.get('password')?.value!!,
      confirmPassword: this.registerForm.get('confirmPassword')?.value!!,
      email: this.registerForm.get('email')?.value!!,
    };
    this.userService.register(data).subscribe(() => this.reloadPage());
  }

  login() {
    const data: UserLogin = {
      username: this.loginForm.get('username')?.value!!,
      password: this.loginForm.get('password')?.value!!,
    };
    this.userService.login(data).subscribe((token: string) => {
      localStorage.setItem('authToken', token);
      window.location.href = '/';
    });
  }

  checkLoginMode(state: string) {
    if (state == 'Login') {
      this.loginMode = true;
    } else {
      this.loginMode = false;
    }
  }

  reloadPage() {
    const currentUrl = this.router.url;
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate([currentUrl]);
    });
  }
}
