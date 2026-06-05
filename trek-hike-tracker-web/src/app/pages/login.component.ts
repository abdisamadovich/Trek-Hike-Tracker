import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="min-h-screen bg-gradient-to-br from-green-600 to-blue-600 flex items-center justify-center py-12 px-4">
      <div class="bg-white rounded-lg shadow-2xl p-8 w-full max-w-md">
        <h2 class="text-3xl font-bold text-green-700 mb-6 text-center">🔐 Kirish</h2>

        <form (ngSubmit)="login()" class="space-y-4">
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
            <label class="block text-gray-700 font-semibold mb-2">Parol</label>
            <input
              [(ngModel)]="password"
              name="password"
              type="password"
              class="w-full px-4 py-2 border-2 border-gray-300 rounded-lg focus:border-green-600 focus:outline-none"
              placeholder="••••••••"
            />
          </div>

          <button
            type="submit"
            class="w-full bg-green-600 text-white py-2 rounded-lg hover:bg-green-700 transition font-bold"
          >
            Kirish
          </button>

          <p class="text-center text-gray-600">
            Hali ro'yxatdan o'tmaganmisiz?
            <a routerLink="/register" class="text-green-600 font-bold hover:underline">
              Ro'yxatdan O'tish
            </a>
          </p>
        </form>
      </div>
    </div>
  `,
  styles: []
})
export class LoginComponent {
  email = '';
  password = '';

  constructor(private authService: AuthService, private router: Router) {}

  login(): void {
    this.authService.login({
      email: this.email,
      password: this.password
    }).subscribe({
      next: () => {
        this.router.navigate(['/']);
      },
      error: (err) => {
        alert('Login failed');
        console.error(err);
      }
    });
  }
}
