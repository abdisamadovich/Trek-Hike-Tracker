import { Component } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, RouterLink],
  template: `
    <nav class="bg-gradient-to-r from-green-700 to-green-600 shadow-lg">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex justify-between h-16">
          <div class="flex items-center">
            <a routerLink="/" class="text-white font-bold text-xl flex items-center gap-2">
              🏔️ Trek Tracker
            </a>
          </div>
          <div class="flex items-center gap-4">
            <a routerLink="/routes" class="text-white hover:bg-green-800 px-3 py-2 rounded-md transition">
              Routes
            </a>
            <a *ngIf="!(auth.currentUser$ | async)" routerLink="/login"
               class="bg-yellow-500 text-white px-4 py-2 rounded-md hover:bg-yellow-600 transition">
              Login
            </a>
            <a *ngIf="!(auth.currentUser$ | async)" routerLink="/register"
               class="bg-white text-green-700 px-4 py-2 rounded-md hover:bg-gray-100 transition">
              Register
            </a>
            <div *ngIf="auth.currentUser$ | async as user" class="flex items-center gap-3">
              <a routerLink="/profile" class="text-white hover:bg-green-800 px-3 py-2 rounded-md">
                Profile
              </a>
              <button (click)="logout()"
                      class="bg-red-500 text-white px-4 py-2 rounded-md hover:bg-red-600 transition">
                Logout
              </button>
            </div>
          </div>
        </div>
      </div>
    </nav>

    <router-outlet></router-outlet>
  `,
  styles: []
})
export class App {
  constructor(public auth: AuthService) {}

  logout(): void {
    this.auth.logout();
  }
}
