import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="min-h-screen bg-gradient-to-br from-green-600 to-blue-600 flex items-center justify-center py-12 px-4">
      <div class="bg-white rounded-lg shadow-2xl p-8 w-full max-w-md">
        <h2 class="text-3xl font-bold text-green-700 mb-6 text-center">📝 Ro'yxatdan O'tish</h2>

        <form (ngSubmit)="register()" class="space-y-4">
          <div>
            <label class="block text-gray-700 font-semibold mb-2">Email</label>
            <input
              [(ngModel)]="email"
              name="email"
              type="email"
              class="w-full px-4 py-2 border-2 border-gray-300 rounded-lg focus:border-green-600 focus:outline-none"
              placeholder="your@email.com"
            />
          </div>

          <div>
            <label class="block text-gray-700 font-semibold mb-2">Username</label>
            <input
              [(ngModel)]="username"
              name="username"
              type="text"
              class="w-full px-4 py-2 border-2 border-gray-300 rounded-lg focus:border-green-600 focus:outline-none"
              placeholder="trekker123"
            />
          </div>

          <div>
            <label class="block text-gray-700 font-semibold mb-2">Parol</label>
            <input
              [(ngModel)]="password"
              name="password"
              type="password"
              class="w-full px-4 py-2 border-2 border-gray-300 rounded-lg focus:border-green-600 focus:outline-none"
              placeholder="••••••••"
            />
          </div>

          <div>
            <label class="block text-gray-700 font-semibold mb-2">Parolni Tasdiqlash</label>
            <input
              [(ngModel)]="passwordConfirm"
              name="passwordConfirm"
              type="password"
              class="w-full px-4 py-2 border-2 border-gray-300 rounded-lg focus:border-green-600 focus:outline-none"
              placeholder="••••••••"
            />
          </div>

          <button
            type="submit"
            class="w-full bg-green-600 text-white py-2 rounded-lg hover:bg-green-700 transition font-bold"
          >
            Ro'yxatdan O'tish
          </button>

          <p class="text-center text-gray-600">
            Allaqachon akkauntingiz bormi?
            <a routerLink="/login" class="text-green-600 font-bold hover:underline">
              Kirish
            </a>
          </p>
        </form>
      </div>
    </div>
  `,
  styles: []
})
export class RegisterComponent {
  email = '';
  username = '';
  password = '';
  passwordConfirm = '';

  constructor(private authService: AuthService, private router: Router) {}

  register(): void {
    this.authService.register({
      email: this.email,
      username: this.username,
      password: this.password,
      passwordConfirm: this.passwordConfirm
    }).subscribe({
      next: () => {
        this.router.navigate(['/']);
      },
      error: (err) => {
        alert('Registration failed');
        console.error(err);
      }
    });
  }
}
